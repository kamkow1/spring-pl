using Antlr4.Runtime.Misc;
using spli.Interpreter.Struct;

namespace spli.Interpreter.Visitors;

public partial class Visitor
{
    public override object VisitNewStructExpression([NotNull] SpringParser.NewStructExpressionContext context)
    {
        var name = context.IDENTIFIER().GetText();

        var structure = _structs[name];

        return new StructureInstance(structure, "self");
    }
}