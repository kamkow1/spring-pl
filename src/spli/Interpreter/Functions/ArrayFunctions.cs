namespace spli.Interpreter.Functions;

public static class ArrayFunctions
{
    public static object? ArrayLength(object?[]? args)
    {
        var array = ((List<object?>)args![0]!);
        return array.Count;
    }

    public static object? ArrayLast(object?[]? args)
    {
        var array = ((List<object?>)args![0]!);
        return array.Last();
    }

    public static object? ArrayAdd(object?[]? args)
    {
        var array = ((List<object?>)args![0]!);
        var item = args![1];
        array.Add(item);
        return null;
    }

    public static object? ArrayDelete(object?[]? args)
    {
        var array = ((List<object?>)args![0]!);
        var item = args![1];
        array.Remove(item);
        return null;
    }

    public static object? ArrayPop(object?[]? args)
    {
        var array = ((List<object?>)args![0]!);
        var last = array.Last();
        array.Remove(last);
        return last;
    }
}