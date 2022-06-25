using Antlr4.Runtime.Misc;

namespace spli.Interpreter.Visitors;

public partial class Visitor
{
    public override object? VisitIndexingExpression([NotNull] SpringParser.IndexingExpressionContext context)
    {
        var array = (object?[])Visit(context.expression(0))!;
        var index = (int)Visit(context.expression(1))!;

        return array[index];
    }
}