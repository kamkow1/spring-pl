using Antlr4.Runtime.Misc;
using spli.Interpreter.Struct;

namespace spli.Interpreter.Visitors;

public partial class Visitor
{
    public override object VisitAssign_struct_prop([NotNull] SpringParser.Assign_struct_propContext context)
    {
        var value = Visit(context.expression(0));
        var structure = (StructureInstance)Visit(context.expression(1))!;

        var propName = context.IDENTIFIER().GetText();

        if (structure.Name == "self")
        {
            structure.BaseStructure.Props[propName].Value = value;
            return true;
        }

        if (structure.BaseStructure.Props[propName].IsPublic)
            structure.BaseStructure.Props[propName].Value = value;
        else throw new Exception($"cannot access a private property {propName}");

        return true;
    }
}