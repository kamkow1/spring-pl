using Antlr4.Runtime.Misc;

namespace spli.Interpreter.Visitors;

public partial class Visitor
{
    public override object VisitNegatedExpression([NotNull] SpringParser.NegatedExpressionContext context)
    {
        return !((bool)Visit(context.expression())!);
    }
}