using Antlr4.Runtime.Misc;

namespace spli.Interpreter.Visitors;

public partial class Visitor
{
    public override object VisitAssign_var([NotNull] SpringParser.Assign_varContext context)
    {
        var name = context.IDENTIFIER().GetText();

        var value = Visit(context.expression());

        var currentActivationRecord = RuntimeStack.Peek();

        if (context.DECLARE() is {})
            currentActivationRecord.SetItem(name, value);
        else
            currentActivationRecord.Members[name] = value;


        return true;
    }
}