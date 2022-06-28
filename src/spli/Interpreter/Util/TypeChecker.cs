namespace spli.Interpreter.Util;

public static class TypeChecker
{
	public static bool CheckType(dynamic variable, dynamic expression)
	{
		if (variable.GetType() == expression.GetType())
			return true;
		else return false;
	}
}
