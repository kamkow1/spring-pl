namespace spli.Interpreter.Enums;

public class EnumStructure
{
    public Dictionary<string, string> Members { get; set; }

    public EnumStructure(Dictionary<string, string> members)
    {
        Members = members;
    }
}