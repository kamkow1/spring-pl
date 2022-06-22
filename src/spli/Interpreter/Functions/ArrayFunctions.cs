using Newtonsoft.Json;

namespace spli.Interpreter.Functions;

public static class ArrayFunctions
{
    public static object? ArrayLength(object?[]? args)
    {
        var array = ((object?[])args![0]!);
        return array.Length;
    }

    public static object? ArrayLast(object?[]? args)
    {
        var array = ((object?[])args![0]!);
        return array.Last();
    }

    public static object? ArrayAdd(object?[]? args)
    {

        var array = ((object?[])args![0]!);
        var item = args![1];
        return AddElementToArray(array, item);
    }

    public static object? ArrayDelete(object?[]? args)
    {
        var array = ((object?[])args![0]!);
        var item = args![1];
        array.ToList().Remove(item);
        return array.ToArray();
    }

    public static object? ArrayPop(object?[]? args)
    {
        var array = ((object?[])args![0]!);
        var last = array.Last();
        array.ToList().Remove(last);
        return last;
    }

    private static T[] AddElementToArray<T>(T[] array, T element) {
        T[] newArray = new T[array.Length + 1];
        int i;
        for (i = 0; i < array.Length; i++) {
            newArray[i] = array[i];
        }
        newArray[i] = element;
        return newArray;
    }
}