using Antlr4.Runtime.Misc;
using spli.Interpreter.Loop;
using spli.Interpreter.Stack;

namespace spli.Interpreter.Visitors;

public partial class Visitor
{
    public override object? VisitLoop_statement([NotNull] SpringParser.Loop_statementContext context)
    {
        if (context.expression() is not {})
        {
            while(true)
            {
                if (_shouldExitCurrentLoop)
                {
                    _shouldExitCurrentLoop = false;
                    break;
                }

                var activationRecord = new ActivationRecord();

                RuntimeStack.Push(activationRecord);
                foreach (var statement in context.scope().statement())
                {
                    if (_shouldSkipCurrentIteration)
                    {
                        _shouldSkipCurrentIteration = false;
                        continue;
                    }

                    if (statement.return_statement() is {} returnStatement)
                        return Visit(returnStatement.expression());
                    Visit(statement);
                }
                RuntimeStack.Pop();
            }
        }

        if (context.expression() is {})
        {
            var loopConfig = (LoopConfiguration)Visit(context.expression())!;

            var isAscending = loopConfig.Left < loopConfig.Right;

            if (isAscending)
            {
                for (var i = loopConfig.Left; i <= loopConfig.Right; ++i)
                {
                    if (_shouldExitCurrentLoop)
                    {
                        _shouldExitCurrentLoop = false;
                        break;
                    }

                    var activationRecord = new ActivationRecord();

                    if (loopConfig.IteratorName is not null)
                        activationRecord.SetItem(loopConfig.IteratorName, i);

                    RuntimeStack.Push(activationRecord);
                    foreach (var statement in context.scope().statement())
                    {
                        if (_shouldSkipCurrentIteration)
                        {
                            _shouldSkipCurrentIteration = false;
                            continue;
                        }

                        if (statement.return_statement() is {} returnStatement)
                            return Visit(returnStatement.expression());
                        Visit(statement);
                    }
                    RuntimeStack.Pop();
                }
            }
            else
            {
                for (var i = loopConfig.Left; i >= loopConfig.Right; --i)
                {
                    if (_shouldExitCurrentLoop)
                    {
                        _shouldExitCurrentLoop = false;
                        break;
                    }

                    var activationRecord = new ActivationRecord();

                    if (loopConfig.IteratorName is not null)
                        activationRecord.SetItem(loopConfig.IteratorName, i);
                        
                    RuntimeStack.Push(activationRecord);
                    foreach (var statement in context.scope().statement())
                    {
                        if (_shouldSkipCurrentIteration)
                        {
                            _shouldSkipCurrentIteration = false;
                            continue;
                        }

                        if (statement.return_statement() is {} returnStatement)
                            return Visit(returnStatement.expression());

                        Visit(statement);
                    }
                    RuntimeStack.Pop();
                }   
            }
        }

        return null;
    }
}