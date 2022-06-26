using Antlr4.Runtime.Misc;
using spli.Interpreter.Enums;

namespace spli.Interpreter.Visitors;

public partial class Visitor
{
    public override object? VisitEnum_def([NotNull] SpringParser.Enum_defContext context)
    {
        var name = context.IDENTIFIER(0).GetText();

        var members = new List<string>();

        foreach (var member in context.IDENTIFIER().Select((v, i) => new { v, i }))
        {
            if (member.i == 0)
                continue;
            members.Add(member.v.GetText());
        }

        var memberDictionary = new Dictionary<string, string>();

        foreach(var m in members)
            memberDictionary.Add(m, m);

        var e = new EnumStructure(memberDictionary);

        _enums.Add(name, e);

        return null;
    }
}