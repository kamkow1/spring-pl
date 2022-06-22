using System.Text.RegularExpressions;
using Antlr4.Runtime.Misc;

using static BuiltinFunctions;

namespace spli.Interpreter;

public class Visitor : SpringParserBaseVisitor<Object?>
{
    private Dictionary<string, Function> _availableFunctions = new();

    private Dictionary<string, Func<object?[]?, object?>> _builtinFunctions = new();

    private CallStack _stack = new();

    public Visitor()
    {
        _builtinFunctions.Add("println", new Func<object?[]?, object?>(args => Println(args)));
        _builtinFunctions.Add("print", new Func<object?[]?, object?>(args => Print(args)));
        _builtinFunctions.Add("read_console", new Func<object?[]?, object?>(_ => ReadConsole()));
    }

    public override object? VisitFile_content([NotNull] SpringParser.File_contentContext context)
    {
        return Visit(context.scope());
    }

    public override object? VisitScope([NotNull] SpringParser.ScopeContext context)
    {
        var activationRecord = new ActivationRecord();

        _stack.Push(activationRecord);

        foreach (var statement in context.statement())
            Visit(statement);
            
        return _stack.Pop();
    }

    public override object VisitAssign_var([NotNull] SpringParser.Assign_varContext context)
    {
        var name = context.IDENTIFIER().GetText();

        var value = Visit(context.expression());

       var currentActivationRecord = _stack.Peek();

       currentActivationRecord.SetItem(name, value);

        return true;
    }

    public override object? VisitConstant([NotNull] SpringParser.ConstantContext context)
    {
        if (context.INTEGER_VALUE() is {} intValue)
            return int.Parse(intValue.GetText());

        if (context.FLOAT_VALUE() is {} floatValue)
            return float.Parse(floatValue.GetText());

        if (context.STRING_VALUE() is {} stringValue)
            return stringValue.GetText()
                .Replace(Char.ToString(stringValue.GetText()[stringValue.GetText().Length - 1]), "")
                .Replace(Char.ToString(stringValue.GetText()[stringValue.GetText().Length - 1]), "")
                .Replace(Char.ToString(stringValue.GetText()[0]), "")
                .Replace(Char.ToString(stringValue.GetText()[0]), "");

        if (context.BOOL_VALUE() is {} boolValue)
            return boolValue.GetText() == "True" ? true : false;

        if (context.NULL() is {} nullValue)
            return null;

        if (context.array() is {} array)
            return Visit(array);

        throw new Exception($"unknown type {context.GetText()}");
    }

    public override object VisitArray([NotNull] SpringParser.ArrayContext context)
    {
        return context.expression().Select(Visit).ToArray();
    }

    public override object? VisitIdentifierExpression([NotNull] SpringParser.IdentifierExpressionContext context)
    {
        var activationRecord = _stack.Peek();
        var name = context.IDENTIFIER().GetText();
        return activationRecord.GetItem(name);
    }

    public override object? VisitFunction_def([NotNull] SpringParser.Function_defContext context)
    {
        var name = context.IDENTIFIER(0).GetText();

        if (_builtinFunctions.ContainsKey(name))
            throw new Exception($"function {name} already exists");

        var parameters = new List<string>();

        for(var i = 1; i < context.IDENTIFIER().Length; ++i) 
        {
            parameters.Add(context.IDENTIFIER(i).GetText());
        }

        var statements = context.scope().statement();

        var function = new Function
        {
            Parameters = parameters.ToArray(),
            Statements = statements
        };

        _availableFunctions.Add(name, function);

        return null;
    }

    public override object? VisitFunction_call([NotNull] SpringParser.Function_callContext context)
    {
        var name = context.IDENTIFIER().GetText();

        var arguments = context.expression().Select(Visit).ToArray();

        if (_builtinFunctions.ContainsKey(name))
            return _builtinFunctions[name](arguments);

        var function = _availableFunctions[name];

        var activationRecord = new ActivationRecord();

        foreach(var parameter in function.Parameters.Select((value, i) => (value, i)))
            activationRecord.SetItem(parameter.value, arguments[parameter.i]);

        _stack.Push(activationRecord);

        foreach(var statement in function.Statements)
        {
            if (statement.return_statement() is {} returnStatement)
                return Visit(returnStatement.expression());

            Visit(statement);
        }

        _stack.Pop();

        return null;
    }

    public override object? VisitEmphasizedExpression([NotNull] SpringParser.EmphasizedExpressionContext context)
    {
        return Visit(context.expression());
    }

    public override object? VisitIf_statement([NotNull] SpringParser.If_statementContext context)
    {
        var condition = (bool)Visit(context.expression())!;

        var statements = context.scope().statement();

        if (condition == true)
        {
            var activationRecord = new ActivationRecord();

            _stack.Push(activationRecord);

            foreach (var statement in statements)
            {
                if (statement.return_statement() is {} returnStatement)
                    return Visit(returnStatement.expression());

                Visit(statement);
            }

            _stack.Pop();
        }

        return null;
    }

    public override object VisitNegatedExpression([NotNull] SpringParser.NegatedExpressionContext context)
    {
        return !((bool)Visit(context.expression())!);
    }

    public override object VisitCompareExpression([NotNull] SpringParser.CompareExpressionContext context)
    {
        var opr = context.compare_oper().GetText();

        var left = Visit(context.expression(0));
        var right = Visit(context.expression(1));

        if (new Regex("^==$").IsMatch(opr))
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

        if (new Regex("^!=$").IsMatch(opr))
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

        if (new Regex("^>=$").IsMatch(opr))
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

        if (new Regex("^<=$").IsMatch(opr))
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

        if (new Regex("^>$").IsMatch(opr))
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

        if (new Regex("^<$").IsMatch(opr))
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

    public override object VisitBinaryExpression([NotNull] SpringParser.BinaryExpressionContext context)
    {
        var opr = context.binary_oper().GetText();

        var left = Visit(context.expression(0));
        var right = Visit(context.expression(1));

        if (opr == "and")
            return (bool)left! && (bool)right!;

        if (opr == "or")
            return (bool)left! || (bool)right!;

        throw new Exception($"cannot evaluate a binary expression with {left} and {right}");
    }
}