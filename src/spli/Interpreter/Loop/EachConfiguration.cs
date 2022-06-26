namespace spli.Interpreter.Loop;

public class EachConfiguration
{
    public object?[] Array { get; set; } 

    public string ItemName { get; set; }

    public string? OptionalIteratorName { get; set; }

    public EachConfiguration(object?[] array, string itemName, string? optIteratorName)
    {
        Array = array;
        ItemName = itemName;
        OptionalIteratorName = optIteratorName;
    }
}