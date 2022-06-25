using Antlr4.Runtime.Misc;

namespace spli.Interpreter.Visitors;

public partial class Visitor
{
    public override object VisitArray([NotNull] SpringParser.ArrayContext context)
    {
        return context.expression().Select(Visit).ToArray();
    }
}