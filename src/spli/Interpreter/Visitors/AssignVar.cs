using Antlr4.Runtime.Misc;
using spli.Interpreter.Util;

namespace spli.Interpreter.Visitors;

public partial class Visitor
{
    public override object VisitAssign_var([NotNull] SpringParser.Assign_varContext context)
    {
        var name = context.IDENTIFIER().GetText();

        var value = Visit(context.expression());

        var currentActivationRecord = RuntimeStack.Peek();

		ref var members = ref currentActivationRecord.Members;



        if (members.ContainsKey(name))
		{
			if (TypeChecker.CheckType(members[name], value))
				members[name] = value;
			else throw new Exception($"mismatched types of {members[name].GetType()} and {value.GetType()}");
		}
		else currentActivationRecord.SetItem(name, value);

		/*if (TypeChecker.CheckType(members[name], value))
			currentActivationRecord.SetItem(name, value);
		else */


        return true;
    }
}
