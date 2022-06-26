using Antlr4.Runtime.Misc;
using spli.Interpreter.Functions;

namespace spli.Interpreter.Visitors;

public partial class Visitor
{
    public override object? VisitFunction_def([NotNull] SpringParser.Function_defContext context)
    {
        var name = context.IDENTIFIER(0).GetText();

        if (_builtinFunctions.ContainsKey(name))
            throw new Exception($"function {name} already exists");

        var parameters = new List<string>();

        for(var i = 1; i < context.IDENTIFIER().Length; ++i) 
        {
            parameters.Add(context.IDENTIFIER(i).GetText());
        }

        var statements = context.scope().statement();

        var function = new Function(parameters.ToArray(), statements);

        _availableFunctions.Add(name, function);

        return null;
    }
}