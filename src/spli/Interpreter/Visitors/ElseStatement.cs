using Antlr4.Runtime.Misc;
using spli.Interpreter.Stack;

namespace spli.Interpreter.Visitors;

public partial class Visitor
{
    public override object? VisitElse_statement([NotNull] SpringParser.Else_statementContext context)
    {
        var activationRecord = new ActivationRecord();

        RuntimeStack.Push(activationRecord);
        foreach (var statement in context.scope().statement())
        {
            if (statement.return_statement() is {} returnStatement)
                return Visit(returnStatement.expression());
            Visit(statement);
        }

        return RuntimeStack.Pop();
    }
}