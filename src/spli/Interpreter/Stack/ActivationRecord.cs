using Newtonsoft.Json;

namespace spli.Interpreter.Stack;

public class ActivationRecord
{
    public Dictionary<string, object?> Members = new();

    public void SetItem(string key, object? value)
    {
        Members[key] = value;
    }

    public object? GetItem(string key)
    {
        return Members[key];
    }

    public bool CheckIfMemberExists(string key)
    {
        return Members.ContainsKey(key);
    }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented);
    }
}