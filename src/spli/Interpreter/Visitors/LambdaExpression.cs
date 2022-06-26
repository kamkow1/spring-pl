using Antlr4.Runtime.Misc;

namespace spli.Interpreter.Visitors;

public partial class Visitor
{
    public override object VisitLambdaExpression([NotNull] SpringParser.LambdaExpressionContext context)
    {
        var parameters = new List<string>();

        foreach (var parameter in context.IDENTIFIER())
            parameters.Add(parameter.GetText());

        var statements = context.scope().statement();

        return new Function(parameters.ToArray(), statements);
    }
}