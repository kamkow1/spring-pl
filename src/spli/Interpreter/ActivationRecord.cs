using Newtonsoft.Json;

namespace spli.Interpreter;

public class ActivationRecord
{
    public int NestingLevel { get; set; }

    private Dictionary<string, object?> _members = new();

    public void SetItem(string key, object? value)
    {
        _members.Add(key, value);
    }

    public object? GetItem(string key)
    {
        return _members[key];
    }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented);
    }
}