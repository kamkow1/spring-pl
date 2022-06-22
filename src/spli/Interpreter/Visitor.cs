using System.Collections.Generic;
using Antlr4.Runtime.Misc;

namespace spli.Interpreter;

public class Visitor : SpringParserBaseVisitor<Object?>
{
    private Dictionary<string, Function> _availableFunctions = new();

    private CallStack _stack = new();

    private int _currentNestingLevel = 0;

    public override object? VisitFile_content([NotNull] SpringParser.File_contentContext context)
    {
        return Visit(context.scope());
    }

    public override object? VisitScope([NotNull] SpringParser.ScopeContext context)
    {
        var activationRecord = new ActivationRecord
        {
            NestingLevel = _currentNestingLevel
        };

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
                .Replace(stringValue.GetText()[stringValue.GetText().Length - 1], '\0')
                .Replace(stringValue.GetText()[0], '\0');

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

        var function = _availableFunctions[name];

        var activationRecord = new ActivationRecord
        {
            NestingLevel = _currentNestingLevel
        };

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
}