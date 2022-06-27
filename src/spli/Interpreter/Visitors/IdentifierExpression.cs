using Antlr4.Runtime.Misc;

namespace spli.Interpreter.Visitors;

public partial class Visitor
{
    public override object? VisitIdentifier_expr([NotNull] SpringParser.Identifier_exprContext context)
    {
        var activationRecord = RuntimeStack.Peek();
        var name = context.IDENTIFIER().GetText();

        var previousAr = RuntimeStack.GetPreviousArOrCurrent();

        if (previousAr.Members.ContainsKey(name))
            return previousAr.GetItem(name);

        return activationRecord.GetItem(name);
    }
}