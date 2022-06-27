namespace spli.Interpreter.BuiltinFunctions.FileSystem;

public static class FileSystemFunctions
{
    public static string? ReadFile(object?[]? args)
    {
        var path = (string)args![0]!;
        return File.ReadAllText(path);
    }
}