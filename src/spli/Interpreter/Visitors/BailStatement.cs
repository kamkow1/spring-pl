using Antlr4.Runtime.Misc;

namespace spli.Interpreter.Visitors;

public partial class Visitor
{
    public override object? VisitBail_statement([NotNull] SpringParser.Bail_statementContext context)
    {
        _shouldExitCurrentLoop = true;
        return null;
    }
}