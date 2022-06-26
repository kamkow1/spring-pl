namespace spli.Interpreter.Struct;

public class StructureInstance
{
    public Structure BaseStructure { get; set; }

    public string Name { get; set; }

    public StructureInstance(Structure b, string name)
    {
        BaseStructure  = b;
        Name = name;
    }
}