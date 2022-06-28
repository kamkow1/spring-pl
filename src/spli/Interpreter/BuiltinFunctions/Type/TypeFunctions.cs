namespace spli.Interpreter.BuiltinFunctions.Type;

public static class TypeFunctions
{
	public static object? TypeOf(object?[]? args)
	{
		var expr = args![0]!;

		return expr.GetType();
	}
}
