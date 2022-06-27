namespace spli.Interpreter.Functions;

public class Function
{
    public string Name { get; set; }

    public string[] Parameters { get; set; }

    public SpringParser.StatementContext[] Statements { get; set; }

    public Function(string name, string[] parameters, SpringParser.StatementContext[] statements)
    {
        Name = name;
        Parameters = parameters;
        Statements = statements;
    }
}