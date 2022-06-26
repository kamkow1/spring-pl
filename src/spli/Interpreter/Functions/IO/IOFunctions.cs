using Newtonsoft.Json;

namespace spli.Interpreter.Functions.IO;

public static class IOFunctions
{
    public static object? Println(object?[]? args)
    {
        if (args is not null && args.Length != 0)
        {
            foreach(var arg in args)
                Console.WriteLine(JsonConvert.SerializeObject(arg, Formatting.Indented));
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

    public static object? ReadConsole()
    {
        return Console.ReadLine();
    }
}