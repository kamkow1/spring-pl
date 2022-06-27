using Antlr4.Runtime.Tree;
using spli.Interpreter.Stack;

namespace spli.Interpreter.Functions;

public static class FunctionCaller
{
    public static object? Call(
        ref SpringParser.Function_callContext context,
        Func<IParseTree, object?> Visit,
        ref Dictionary<string, Func<object?[]?, object?>> _builtinFunctions,
        ref CallStack _stack,
        ref  Dictionary<string, Function> _availableFunctions,
        string? name = null,
        object?[]? arguments = null)
    {
        name = name ?? context.IDENTIFIER().GetText();


        arguments = arguments ?? context.expression().Select(Visit).ToArray();

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