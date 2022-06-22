using Antlr4.Runtime;
using spli.Interpreter;

public static class Initializer
{
    public static void Run(string program)
    {
        try
        {
            var programInputStream          = new AntlrInputStream(program);
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