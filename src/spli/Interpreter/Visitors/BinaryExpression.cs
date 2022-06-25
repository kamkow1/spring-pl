using Antlr4.Runtime.Misc;

namespace spli.Interpreter.Visitors;

public partial class Visitor
{
    public override object VisitBinaryExpression([NotNull] SpringParser.BinaryExpressionContext context)
    {
        var opr = context.binary_oper().GetText();

        var left = Visit(context.expression(0));
        var right = Visit(context.expression(1));

        if (opr == "and")
            return (bool)left! && (bool)right!;

        if (opr == "or")
            return (bool)left! || (bool)right!;

        throw new Exception($"cannot evaluate a binary expression with {left} and {right}");
    }
}