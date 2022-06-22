using Antlr4.Runtime.Misc;

namespace spli.Interpreter;

public class ModuleVisitor : ModuleParserBaseVisitor<object?>
{
    public override object VisitContent([NotNull] ModuleParser.ContentContext context)
    {
        var paths = context.include().include_path().Select(Visit).ToList();

        return paths;
    }

    public override object VisitInclude_path([NotNull] ModuleParser.Include_pathContext context)
    {
        var path = context.STRING_VALUE().GetText()
            .Replace(Char.ToString(context.STRING_VALUE().GetText()[0]), "")
            .Replace(Char.ToString(context.STRING_VALUE().GetText()[context.STRING_VALUE().GetText().Length - 1]), "");

        return path;
    }
}