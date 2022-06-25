using Antlr4.Runtime.Misc;

namespace spli.Interpreter.Visitors;

public partial class Visitor
{
    public override object? VisitStruct_def([NotNull] SpringParser.Struct_defContext context)
    {
        var props = context.struct_content().prop_def().Select(Visit).ToList();
        var propDictionary = new Dictionary<string, Prop>();

        foreach(var prop in props)
            propDictionary.Add(((Prop)prop!).Name!, (Prop)prop);

        var methods = context.struct_content().method_def().Select(Visit).ToList();
        var methodDictionary = new Dictionary<string, Method>();

        foreach(var method in methods)
            methodDictionary.Add(((Method)method!).Name!, (Method)method);

        var name = context.IDENTIFIER().GetText();

        _structs.Add(name, new Structure(methodDictionary, propDictionary));

        return null;
    }
}