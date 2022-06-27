using Antlr4.Runtime.Misc;
using spli.Interpreter.Loop;
using spli.Interpreter.Stack;

namespace spli.Interpreter.Visitors;

public partial class Visitor
{
    public override object? VisitEach_loop_statement([NotNull] SpringParser.Each_loop_statementContext context)
    {
        var config = (EachConfiguration)Visit(context.expression())!;

        if (config.OptionalIteratorName is not null)
        {
            foreach(var item in config.Array.Select((value, i) => new { value, i }))
            {
                if (_shouldExitCurrentLoop)
                {
                    _shouldExitCurrentLoop = false;
                    break;
                }

                var activationRecord = new ActivationRecord();

                activationRecord.SetItem(config.ItemName, item.value);
                activationRecord.SetItem(config.OptionalIteratorName, item.i);

                RuntimeStack.Push(activationRecord);
                foreach (var statement in context.scope().statement())
                {
                    if (statement.return_statement() is {} returnStatement)
                        return Visit(returnStatement.expression());

                    if (_shouldSkipCurrentIteration)
                    {
                        _shouldSkipCurrentIteration = false;
                        continue;
                    }

                    Visit(statement);
                }
                RuntimeStack.Pop();
            }
        }
        else
        {
            foreach(var item in config.Array)
            {
                if (_shouldExitCurrentLoop)
                {
                    _shouldExitCurrentLoop = false;
                    break;
                }

                var activationRecord = new ActivationRecord();

                activationRecord.SetItem(config.ItemName, item);

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

        return null;
    }
}