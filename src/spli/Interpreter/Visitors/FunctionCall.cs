using Antlr4.Runtime.Misc;
using spli.Interpreter.Functions;
using spli.Interpreter.Stack;

namespace spli.Interpreter.Visitors;

public partial class Visitor
{
    public override object? VisitFunction_call([NotNull] SpringParser.Function_callContext context)
    {
        var name = context.IDENTIFIER().GetText();


        var arguments = context.expression().Select(Visit).ToArray();

        if (_builtinFunctions.ContainsKey(name))
            return _builtinFunctions[name](arguments);

        var previousAr = _stack.Peek();

        Function? function;

        if (previousAr.Members.ContainsKey(name))
            function = previousAr.Members[name] as Function;
        else
            function = _availableFunctions[name];

        var activationRecord = new ActivationRecord();

        foreach(var parameter in function!.Parameters.Select((value, i) => (value, i)))
        {
            if (activationRecord.CheckIfMemberExists(parameter.value))
                continue;

            activationRecord.SetItem(parameter.value, arguments[parameter.i]);
        }

        _stack.Push(activationRecord);

        foreach(var statement in function.Statements)
        {
            if (statement.return_statement() is {} returnStatement)
                return Visit(returnStatement.expression());

            Visit(statement);
        }

        _stack.Pop();

        return null;
    }
}