using Antlr4.Runtime.Misc;
using spli.Interpreter.Struct;

namespace spli.Interpreter.Visitors;

public partial class Visitor
{
    public override object VisitProp_def([NotNull] SpringParser.Prop_defContext context)
    {
        var accessModifier = context.access_mod().GetText();
        var isPublic = accessModifier == "pub";
        var name = context.IDENTIFIER().GetText();

        return new Prop
        {
            Name = name,
            IsPublic = isPublic,
            Value = null
        };
    }
}