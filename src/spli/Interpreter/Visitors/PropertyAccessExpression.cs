using Antlr4.Runtime.Misc;

namespace spli.Interpreter.Visitors;

public partial class Visitor
{
    public override object? VisitPropAccessExpression([NotNull] SpringParser.PropAccessExpressionContext context)
    {
        var structure = (StructureInstance)Visit(context.expression())!;
        var propName = context.IDENTIFIER().GetText();

        if (structure.Name == "self")
            return structure.BaseStructure.Props[propName].Value;

        if (structure.BaseStructure.Props[propName].IsPublic)
            return structure.BaseStructure.Props[propName].Value;

        throw new Exception($"cannot access a private property {propName}");
    }
}