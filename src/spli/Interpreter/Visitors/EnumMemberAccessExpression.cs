using Antlr4.Runtime.Misc;

namespace spli.Interpreter.Visitors;

public partial class Visitor
{
    public override object? VisitEnumMemberAccessExpression([NotNull] SpringParser.EnumMemberAccessExpressionContext context)
    {
        var name = context.IDENTIFIER(0).GetText();
        var foundEnum = _enums[name];

        var memberName = context.IDENTIFIER(1).GetText();
        var member = foundEnum.Members[memberName];

        return member;
    }
}