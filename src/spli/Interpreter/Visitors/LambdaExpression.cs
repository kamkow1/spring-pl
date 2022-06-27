using Antlr4.Runtime.Misc;
using spli.Interpreter.Functions;

namespace spli.Interpreter.Visitors;

public partial class Visitor
{
    public override object VisitLambdaExpression([NotNull] SpringParser.LambdaExpressionContext context)
    {
        var parameters = new List<string>();

        foreach (var parameter in context.IDENTIFIER())
            parameters.Add(parameter.GetText());

        var statements = context.scope().statement();

        var name = $"lambda-{Guid.NewGuid().ToString()}";

        return new Function(name, parameters.ToArray(), statements);
    }
}