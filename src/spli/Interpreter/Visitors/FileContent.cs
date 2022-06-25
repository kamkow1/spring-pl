using Antlr4.Runtime.Misc;

namespace spli.Interpreter.Visitors;

public partial class Visitor
{
    public override object? VisitFile_content([NotNull] SpringParser.File_contentContext context)
    {
        return Visit(context.scope());
    }
}