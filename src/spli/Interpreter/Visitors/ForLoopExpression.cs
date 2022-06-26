using Antlr4.Runtime.Misc;
using spli.Interpreter.Loop;

namespace spli.Interpreter.Visitors;

public partial class Visitor
{
    public override object VisitForLoopExpression([NotNull] SpringParser.ForLoopExpressionContext context)
    {
        var left = (int)Visit(context.expression(0))!;
        var right = (int)Visit(context.expression(1))!;

        var iteratorName = context.expression().ElementAtOrDefault(2) != null ? (string?)Visit(context.expression(2)) : null;

        return new LoopConfiguration
        {
            Left = left,
            Right = right,
            IteratorName = iteratorName
        };
    }
}