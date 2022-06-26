namespace spli.Interpreter.Functions;

public class Function
{
    public string[] Parameters { get; set; }

    public SpringParser.StatementContext[] Statements { get; set; }

    public Function(string[] parameters, SpringParser.StatementContext[] statements)
    {
        Parameters = parameters;
        Statements = statements;
    }
}