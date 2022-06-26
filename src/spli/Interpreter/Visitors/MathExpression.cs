using Antlr4.Runtime.Misc;

namespace spli.Interpreter.Visitors;

public partial class Visitor
{
    public override object VisitMathExpression([NotNull] SpringParser.MathExpressionContext context)
    {
        var oper = context.math_oper().GetText();

        var left = Visit(context.expression(0))!;
        var right = Visit(context.expression(1))!;

        if (oper == "+")
        {
            if (left is int && right is int)
                return int.Parse(left.ToString()!) + int.Parse(right.ToString()!);
            else if (left is int && right is float)
                return int.Parse(left.ToString()!) + float.Parse(right.ToString()!);
            else if (left is float && right is int)
                return float.Parse(left.ToString()!) + int.Parse(right.ToString()!);
            else if (left is float && right is float)
                return float.Parse(left.ToString()!) + float.Parse(right.ToString()!);
            else if (left is string || right is string)
                return left!.ToString() + right!.ToString();
            else throw new Exception($"cannot add {left} to {right} since they're not either int, float or string");
        }

        if (oper == "-")
        {
            if (left is int && right is int)
                return int.Parse(left.ToString()!) - int.Parse(right.ToString()!);
            else if (left is int && right is float)
                return int.Parse(left.ToString()!) - float.Parse(right.ToString()!);
            else if (left is float && right is int)
                return float.Parse(left.ToString()!) - int.Parse(right.ToString()!);
            else if (left is float && right is float)
                return float.Parse(left.ToString()!) - float.Parse(right.ToString()!);
            else throw new Exception($"cannot subtract {right} from {left} since they're not either int or float");
        }

        if (oper == "*")
        {
            if (left is int && right is int)
                return int.Parse(left.ToString()!) * int.Parse(right.ToString()!);
            else if (left is int && right is float)
                return int.Parse(left.ToString()!) * float.Parse(right.ToString()!);
            else if (left is float && right is int)
                return float.Parse(left.ToString()!) * int.Parse(right.ToString()!);
            else if (left is float && right is float)
                return float.Parse(left.ToString()!) * float.Parse(right.ToString()!);
            else throw new Exception($"cannot multiply {left} by {right} since they're not either int or float");
        }

        if (oper == "%")
        {
            if (left is int && right is int)
                return int.Parse(left.ToString()!) % int.Parse(right.ToString()!);
            else if (left is int && right is float)
                return int.Parse(left.ToString()!) % float.Parse(right.ToString()!);
            else if (left is float && right is int)
                return float.Parse(left.ToString()!) % int.Parse(right.ToString()!);
            else if (left is float && right is float)
                return float.Parse(left.ToString()!) % float.Parse(right.ToString()!);
            else throw new Exception($"cannot multiply {left} by {right} since they're not either int or float");
        }

        if (oper == "/")
        {
            if (int.Parse(right.ToString()!) == 0)
                throw new Exception("cannot divide by 0");

            if (left is int && right is int)
                return int.Parse(left.ToString()!) / int.Parse(right.ToString()!);
            else if (left is int && right is float)
                return int.Parse(left.ToString()!) / float.Parse(right.ToString()!);
            else if (left is float && right is int)
                return float.Parse(left.ToString()!) / int.Parse(right.ToString()!);
            else if (left is float && right is float)
                return float.Parse(left.ToString()!) / float.Parse(right.ToString()!);
            else throw new Exception($"cannot divide {left} by {right} since they're not either int or float");
        }

        if (oper == "^")
        {
            if (left is int && right is int)
                return Math.Pow(int.Parse(left.ToString()!), int.Parse(right.ToString()!));
            else if (left is int && right is float)
                return Math.Pow(int.Parse(left.ToString()!), float.Parse(right.ToString()!));
            else if (left is float && right is int)
                return Math.Pow(float.Parse(left.ToString()!), int.Parse(right.ToString()!));
            else if (left is float && right is float)
                return Math.Pow(float.Parse(left.ToString()!), float.Parse(right.ToString()!));
            else throw new Exception($"cannot raise {left} to {right} since they're not either int or float");
        }

        throw new Exception($"evaluating math expression on {left} and {right} failed");
    }
}