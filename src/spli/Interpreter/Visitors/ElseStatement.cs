using Antlr4.Runtime.Misc;

namespace spli.Interpreter.Visitors;

public partial class Visitor
{
    public override object? VisitElse_statement([NotNull] SpringParser.Else_statementContext context)
    {
        var activationRecord = new ActivationRecord();

        _stack.Push(activationRecord);
        foreach (var statement in context.scope().statement())
        {
            if (statement.return_statement() is {} returnStatement)
                return Visit(returnStatement.expression());
            Visit(statement);
        }

        return _stack.Pop();
    }
}