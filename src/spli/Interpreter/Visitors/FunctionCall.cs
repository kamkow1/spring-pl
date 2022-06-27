using Antlr4.Runtime.Misc;
using spli.Interpreter.Functions;
using spli.Interpreter.Stack;

namespace spli.Interpreter.Visitors;

public partial class Visitor
{
    public override object? VisitFunction_call([NotNull] SpringParser.Function_callContext context)
    {
        functionCallContext = context;

        return FunctionCaller.Call(
            ref context,
            Visit,
            ref _builtinFunctions,
            ref _stack,
            ref _availableFunctions
        );
    }
}