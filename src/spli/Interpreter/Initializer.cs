using Antlr4.Runtime;
using spli.Interpreter.Visitors;

public static class Initializer
{
    public static void Run(string program, string filePath, string[] args)
    {
        try
        {
            var includes = "[]\n[/]";

            if (program.LastIndexOf("[/]") > 0)
                includes = program.Substring(0, program.LastIndexOf("[/]")) + "[/]";

            program = includes + program;

            var moduleInputStream           = new AntlrInputStream(includes);
            var moduleLexer                 = new ModuleLexer(moduleInputStream);
            var moduleCommonTokenStream     = new CommonTokenStream(moduleLexer);
            var moduleParser                = new ModuleParser(moduleCommonTokenStream);
            var moduleParseContext          = moduleParser.parse();
            var moduleVisitor               = new ModuleVisitor(Path.GetDirectoryName(filePath)!);

            var includePaths = moduleVisitor.Visit(moduleParseContext);

            var includedCode = string.Empty;
            foreach(var p in (List<object?>)includePaths!)
                includedCode += File.ReadAllText(p!.ToString()!) + "\n";

            var arguments = string.Join(", ", args.Select(a => "\"" + a + "\"").ToArray());

            var fullCode = includedCode + 
                program.Split(includes)[2] + 
                (Path.GetFileName(filePath).Split(".")[0] == "main" ? $"\n\nmain [{arguments}];" : "");

            var programInputStream          = new AntlrInputStream(fullCode);
            var programLexer                = new SpringLexer(programInputStream);
            var programCommonTokenStream    = new CommonTokenStream(programLexer);
            var programParser               = new SpringParser(programCommonTokenStream);

            var programParseContext = programParser.parse();

            var visitor = new Visitor();

            visitor.Visit(programParseContext);
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e);
            Environment.Exit(1);
        }
    }
}