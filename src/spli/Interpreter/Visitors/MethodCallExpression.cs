using Antlr4.Runtime.Misc;

namespace spli.Interpreter.Visitors;

public partial class Visitor
{
    public override object? VisitMethodCallExpression([NotNull] SpringParser.MethodCallExpressionContext context)
    {
        var name = context.IDENTIFIER().GetText();

        var arguments = new List<object?>();

        foreach(var item in context.expression().Select((value, i) => new { value, i }))
        {
            if (item.i == 0)
                continue;

            arguments.Add(Visit(item.value));
        }

        var structure = (StructureInstance)Visit(context.expression(0))!;
        var method = structure.BaseStructure.Methods[name];

        if (!method.IsPublic && structure.Name != "self")
            throw new Exception($"cannot access a private method {name}");

        var activationRecord = new ActivationRecord();

        foreach(var parameter in method.Parameters!.Select((value, i) => (value, i)))
        {
            if (activationRecord.CheckIfMemberExists(parameter.value))
                continue;

            activationRecord.SetItem(parameter.value, arguments[parameter.i]);
        }

        _stack.Push(activationRecord);

        foreach(var statement in method.Statements)
        {
            if (statement.return_statement() is {} returnStatement)
                return Visit(returnStatement.expression())!;

            Visit(statement);
        }

        _stack.Pop();

        return null;
    }
}