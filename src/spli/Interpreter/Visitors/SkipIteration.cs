using Antlr4.Runtime.Misc;

namespace spli.Interpreter.Visitors;

public partial class Visitor
{
    public override object? VisitSkip_iteration([NotNull] SpringParser.Skip_iterationContext context)
    {
        ShouldSkipCurrentIteration = true;
        return null;
    }
}