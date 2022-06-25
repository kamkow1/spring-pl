using Antlr4.Runtime.Misc;

namespace spli.Interpreter.Visitors;

public class ModuleVisitor : ModuleParserBaseVisitor<object?>
{
    private string _workingDirPath;

    public ModuleVisitor(string workingDirPath)
    {
        _workingDirPath = workingDirPath;
    }

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


        Console.WriteLine(path);

        return Path.Combine(Path.GetFullPath(_workingDirPath), path);
    }
}