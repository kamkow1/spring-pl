using Antlr4.Runtime.Misc;
using spli.Interpreter.Struct;

namespace spli.Interpreter.Visitors;

public partial class Visitor
{
    public override object VisitMethod_def([NotNull] SpringParser.Method_defContext context)
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

        var accessModifier = context.access_mod().GetText();
        var isPublic = accessModifier == "pub";

        return new Method(name, isPublic, statements, parameters.ToArray());
    }
}