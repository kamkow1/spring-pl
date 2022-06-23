namespace spli.Interpreter;

public class Method
{
    public string Name { get; set; }

    public bool IsPublic { get; set; }  

    public string[] Parameters { get; set; }

    public SpringParser.StatementContext[] Statements { get; set; }
}