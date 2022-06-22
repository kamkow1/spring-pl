namespace spli.Interpreter.Functions;

public static class CastingFucntions
{
    public static object? Int(object?[]? args)
    {
        var num = int.Parse(args![0]!.ToString()!);
        return num;
    }

    public static object? Float(object?[]? args)
    {
        var num = float.Parse(args![0]!.ToString()!);
        return num;
    }

    public static object? String(object?[]? args)
    {
        var str = args![0]!.ToString()!;
        return str;
    }

    public static object? Bool(object?[]? args)
    {
        if (args![0]!.ToString() == "True")
            return true;

        if (args![0]!.ToString() == "False")
            return false;

        if (int.Parse(args![0]!.ToString()!) == 1)
            return true;

        if (int.Parse(args![0]!.ToString()!) == 0)
            return false;

        throw new Exception("could not cast to bool type");
    }
}