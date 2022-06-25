namespace spli.Interpreter.Visitors;

public partial class Visitor
{
    public override object VisitAssign_var([NotNull] SpringParser.Assign_varContext context)
    {
        var name = context.IDENTIFIER().GetText();

        var value = Visit(context.expression());

       var currentActivationRecord = _stack.Peek();

       currentActivationRecord.SetItem(name, value);

        return true;
    }
}