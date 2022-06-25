using Antlr4.Runtime.Misc;

namespace spli.Interpreter.Visitors;

public partial class Visitor
{
    public override object VisitEachLoopExpression([NotNull] SpringParser.EachLoopExpressionContext context)
    {
        var array = (object?[])Visit(context.expression(1))!;

        var itemName = (string)Visit(context.expression(0))!;

        var iteratorName = context.expression().ElementAtOrDefault(2) != null ? (string?)Visit(context.expression(2)) : null;

        return new EachConfiguration(array, itemName, iteratorName);
    }
}