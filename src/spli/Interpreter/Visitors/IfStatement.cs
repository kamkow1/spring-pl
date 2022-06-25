using Antlr4.Runtime.Misc;

namespace spli.Interpreter.Visitors;

public partial class Visitor
{
    public override object? VisitIf_statement([NotNull] SpringParser.If_statementContext context)
    {
        var condition = (bool)Visit(context.expression())!;

        _lastConditionResult = condition;

        var statements = context.scope().statement(); 

        if (condition == true)
        {
            var activationRecord = new ActivationRecord();

            _stack.Push(activationRecord);

            foreach (var statement in statements)
            {
                if (statement.return_statement() is {} returnStatement)
                    return Visit(returnStatement.expression());

                Visit(statement);
            }

            _stack.Pop();
        }

        if (context.elif_statement() is {})
        {
            foreach(var elifStatement in context.elif_statement())
                Visit(elifStatement);
        }

        if (context.else_statement() is {} && _lastConditionResult)
            Visit(context.else_statement());

        return null;
    }
}