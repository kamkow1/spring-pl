using Antlr4.Runtime.Tree;
using Microsoft.AspNetCore.Http;
using spli.Interpreter.Stack;

namespace spli.Interpreter.Functions;

public static class FunctionCaller
{
    public static object? Call(
        ref SpringParser.Function_callContext context,
        Func<IParseTree, object?> Visit,
        ref Dictionary<string, Func<object?[]?, object?>> BuiltinFunctions,
        ref CallStack RuntimeStack,
        ref  Dictionary<string, Function> Functions,
        string? name = null,
        object?[]? arguments = null,
        Dictionary<string, Microsoft.Extensions.Primitives.StringValues>? query = null)
    {
        name = name ?? context.IDENTIFIER().GetText();

        arguments = arguments ?? context.expression().Select(Visit).ToArray();

        if (BuiltinFunctions.ContainsKey(name))
            return BuiltinFunctions[name](arguments);

        var previousAr = RuntimeStack.Peek();

        Function? function;

        if (previousAr.Members.ContainsKey(name))
            function = previousAr.Members[name] as Function;
        else
            function = Functions[name];

        var activationRecord = new ActivationRecord();

        foreach(var parameter in function!.Parameters.Select((value, i) => (value, i)))
        {
            if (activationRecord.CheckIfMemberExists(parameter.value))
                continue;

            activationRecord.SetItem(parameter.value, query is not null ? query : arguments[parameter.i]);
        }

        RuntimeStack.Push(activationRecord);

        foreach(var statement in function.Statements)
        {
            if (statement.return_statement() is {} returnStatement)
                return Visit(returnStatement.expression());

            Visit(statement);
        }

        RuntimeStack.Pop();

        return null;
    }
}
