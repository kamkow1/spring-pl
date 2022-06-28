using Antlr4.Runtime.Misc;

namespace spli.Interpreter.Visitors;

public partial class Visitor
{
    public override object? VisitIndexingExpression([NotNull] SpringParser.IndexingExpressionContext context)
    {
        var collection = Visit(context.expression(0))!;
        var index = Visit(context.expression(1))!;

        Console.WriteLine(index.GetType());

        if (index is int && collection is object?[])
			return ((object?[])collection)[(int)index];
		else if (index is string && collection is Dictionary<string, Microsoft.Extensions.Primitives.StringValues>)
			return ((Dictionary<string, Microsoft.Extensions.Primitives.StringValues>)collection)[(string)index];
		else throw new Exception($"unable to use expression {index} to access collection {collection}");
    }
}
