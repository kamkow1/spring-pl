using Antlr4.Runtime.Misc;

namespace spli.Interpreter.Visitors;

public partial class Visitor
{
     public override object VisitCompareExpression([NotNull] SpringParser.CompareExpressionContext context)
    {
        var opr = context.compare_oper().GetText();

        var left = Visit(context.expression(0));
        var right = Visit(context.expression(1));

        if (opr == "==")
        {
            if (left is int && right is int)
                return int.Parse(left.ToString()!) == int.Parse(right.ToString()!);
            else if (left is int && right is float)
                return int.Parse(left.ToString()!) == float.Parse(right.ToString()!);
            else if (left is float && right is int)
                return float.Parse(left.ToString()!) == int.Parse(right.ToString()!);
            else if (left is float && right is float)
                return float.Parse(left.ToString()!) == float.Parse(right.ToString()!);
            else if (left is string && right is string)
                return left.ToString() == right.ToString();
        }

        if (opr == "!=")
        {
            if (left is int && right is int)
                return int.Parse(left.ToString()!) != int.Parse(right.ToString()!);
            else if (left is int && right is float)
                return int.Parse(left.ToString()!) != float.Parse(right.ToString()!);
            else if (left is float && right is int)
                return float.Parse(left.ToString()!) != int.Parse(right.ToString()!);
            else if (left is float && right is float)
                return float.Parse(left.ToString()!) != float.Parse(right.ToString()!);
            else if (left is string && right is string)
                return left.ToString() != right.ToString();
        }

        if (opr == ">=")
        {
            if (left is int && right is int)
                return int.Parse(left.ToString()!) >= int.Parse(right.ToString()!);
            else if (left is int && right is float)
                return int.Parse(left.ToString()!) >= float.Parse(right.ToString()!);
            else if (left is float && right is int)
                return float.Parse(left.ToString()!) >= int.Parse(right.ToString()!);
            else if (left is float && right is float)
                return float.Parse(left.ToString()!) >= float.Parse(right.ToString()!);
            else if (left is string && right is string)
                return left.ToString()!.Length >= right.ToString()!.Length;
            else throw new Exception($"cannot compare if {left} is qreater or equal than {right} since they're not either int, float or string");
        }

        if (opr == "<=")
        {
            if (left is int && right is int)
                return int.Parse(left.ToString()!) <= int.Parse(right.ToString()!);
            else if (left is int && right is float)
                return int.Parse(left.ToString()!) <= float.Parse(right.ToString()!);
            else if (left is float && right is int)
                return float.Parse(left.ToString()!) <= int.Parse(right.ToString()!);
            else if (left is float && right is float)
                return float.Parse(left.ToString()!) <= float.Parse(right.ToString()!);
            else if (left is string && right is string)
                return left.ToString()!.Length <= right.ToString()!.Length;
            else throw new Exception($"cannot compare if {left} is less or equal than {right} since they're not either int, float or string");
        }

        if (opr == ">")
        {
            if (left is int && right is int)
                return int.Parse(left.ToString()!) > int.Parse(right.ToString()!);
            else if (left is int && right is float)
                return int.Parse(left.ToString()!) > float.Parse(right.ToString()!);
            else if (left is float && right is int)
                return float.Parse(left.ToString()!) > int.Parse(right.ToString()!);
            else if (left is float && right is float)
                return float.Parse(left.ToString()!) > float.Parse(right.ToString()!);
            else if (left is string && right is string)
                return left.ToString()!.Length > right.ToString()!.Length;
            else throw new Exception($"cannot compare if {left} is qreater than {right} since they're not either int, float or string");
        }

        if (opr == "<")
        {
            if (left is int && right is int)
                return int.Parse(left.ToString()!) < int.Parse(right.ToString()!);
            else if (left is int && right is float)
                return int.Parse(left.ToString()!) < float.Parse(right.ToString()!);
            else if (left is float && right is int)
                return float.Parse(left.ToString()!) < int.Parse(right.ToString()!);
            else if (left is float && right is float)
                return float.Parse(left.ToString()!) < float.Parse(right.ToString()!);
            else if (left is string && right is string)
                return left.ToString()!.Length < right.ToString()!.Length;
            else throw new Exception($"cannot compare if {left} is less than {right} since they're not either int, float or string");
        }

        return false;
    }
}