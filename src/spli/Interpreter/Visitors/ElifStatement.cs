using Antlr4.Runtime.Misc;
using spli.Interpreter.Stack;

namespace spli.Interpreter.Visitors;

public partial class Visitor
{
    public override object? VisitElif_statement([NotNull] SpringParser.Elif_statementContext context)
    {
        var elifCondition = (bool)Visit(context.expression())!;
        LastConditionResult = elifCondition;

        if (elifCondition! == true)
        {
            var activationRecord = new ActivationRecord();

            RuntimeStack.Push(activationRecord);
            foreach (var statement in context.scope().statement())
            {
                if (statement.return_statement() is {} returnStatement)
                    return Visit(returnStatement.expression());
                Visit(statement);
            }
            RuntimeStack.Pop();
        }

        return null;
    }
}