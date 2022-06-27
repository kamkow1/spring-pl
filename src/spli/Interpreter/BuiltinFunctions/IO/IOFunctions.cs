using Newtonsoft.Json;

namespace spli.Interpreter.BuiltinFunctions.IO;

public static class IOFunctions
{
    public static object? PrintlnJson(object?[]? args)
    {
        if (args is not null && args.Length != 0)
        {
            foreach(var arg in args)
                Console.WriteLine(JsonConvert.SerializeObject(arg));
        }

        return null;
    }

    public static object? Println(object?[]? args)
    {
        if (args is not null && args.Length != 0)
        {
            foreach(var arg in args)
                Console.WriteLine(arg);
        }

        return null;
    }

    public static object? Print(object?[]? args)
    {
        if (args is not null && args.Length != 0)
        {
            foreach(var arg in args)
                Console.Write(JsonConvert.SerializeObject(arg, Formatting.Indented));
        }

        return null;
    }

    public static string? ReadConsole()
    {
        return Console.ReadLine();
    }
}