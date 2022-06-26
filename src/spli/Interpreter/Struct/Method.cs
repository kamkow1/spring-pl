namespace spli.Interpreter.Struct;

public class Method
{
    public string Name { get; set; }

    public bool IsPublic { get; set; }  

    public string[]? Parameters { get; set; }

    public SpringParser.StatementContext[] Statements { get; set; }

    public Method(string name, 
                bool isPublic, 
                SpringParser.StatementContext[] 
                statements, string[]? parameters)
    {
        Name = name;
        IsPublic = isPublic;
        Statements = statements;
        Parameters = parameters;
    }
}