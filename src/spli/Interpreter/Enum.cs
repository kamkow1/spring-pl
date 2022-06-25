namespace spli.Interpreter;

public class Enum
{
    public Dictionary<string, string> Members { get; set; }

    public Enum(Dictionary<string, string> members)
    {
        Members = members;
    }
}