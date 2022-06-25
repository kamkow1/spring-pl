using System.Text.RegularExpressions;
using Antlr4.Runtime.Misc;
using static spli.Interpreter.Functions.IOFunctions;
using static spli.Interpreter.Functions.CastingFucntions;
using static spli.Interpreter.Functions.ArrayFunctions;

namespace spli.Interpreter.Visitors;

public partial class Visitor : SpringParserBaseVisitor<Object?>
{
    private Dictionary<string, Function> _availableFunctions = new();

    private Dictionary<string, Func<object?[]?, object?>> _builtinFunctions = new();

    private Dictionary<string, Structure> _structs = new();

    private CallStack _stack = new();

    private bool _lastConditionResult;

    private bool _shouldSkipCurrentIteration = false;

    private bool _shouldExitCurrentLoop = false;

    public Visitor()
    {
        // io
        _builtinFunctions.Add("println",        new Func<object?[]?, object?>(args => Println(args)));
        _builtinFunctions.Add("print",          new Func<object?[]?, object?>(args => Print(args)));
        _builtinFunctions.Add("read_console",   new Func<object?[]?, object?>(_ => ReadConsole()));

        // type casting
        _builtinFunctions.Add("int",            new Func<object?[]?, object?>(args => Int(args)));
        _builtinFunctions.Add("float",          new Func<object?[]?, object?>(args => Float(args)));
        _builtinFunctions.Add("string",         new Func<object?[]?, object?>(args => String(args)));
        _builtinFunctions.Add("bool",           new Func<object?[]?, object?>(args => Bool(args)));

        // array
        _builtinFunctions.Add("arr_len",        new Func<object?[]?, object?>(args => ArrayLength(args)));
        _builtinFunctions.Add("arr_last",       new Func<object?[]?, object?>(args => ArrayLast(args)));
        _builtinFunctions.Add("arr_add",        new Func<object?[]?, object?>(args => ArrayAdd(args)));
        _builtinFunctions.Add("arr_del",        new Func<object?[]?, object?>(args => ArrayDelete(args)));
        _builtinFunctions.Add("arr_pop",        new Func<object?[]?, object?>(args => ArrayPop(args)));
    }

    

   

   

    

    

    

   

    

    

    

    

    

    

    

    

    

    

    

    

    

    public override object VisitNewStructExpression([NotNull] SpringParser.NewStructExpressionContext context)
    {
        var name = context.IDENTIFIER().GetText();

        var structure = _structs[name];

        return new StructureInstance(structure, "self");
    }

    public override object VisitAssign_struct_prop([NotNull] SpringParser.Assign_struct_propContext context)
    {
        var value = Visit(context.expression(0));
        var structure = (StructureInstance)Visit(context.expression(1))!;

        var propName = context.IDENTIFIER().GetText();

        if (structure.Name == "self")
        {
            structure.BaseStructure.Props[propName].Value = value;
            return true;
        }

        if (structure.BaseStructure.Props[propName].IsPublic)
            structure.BaseStructure.Props[propName].Value = value;
        else throw new Exception($"cannot access a private property {propName}");

        return true;
    }

    public override object? VisitMethodCallExpression([NotNull] SpringParser.MethodCallExpressionContext context)
    {
        var name = context.IDENTIFIER().GetText();

        var arguments = new List<object?>();

        foreach(var item in context.expression().Select((value, i) => new { value, i }))
        {
            if (item.i == 0)
                continue;

            arguments.Add(Visit(item.value));
        }

        var structure = (StructureInstance)Visit(context.expression(0))!;
        var method = structure.BaseStructure.Methods[name];

        if (!method.IsPublic && structure.Name != "self")
            throw new Exception($"cannot access a private method {name}");

        var activationRecord = new ActivationRecord();

        foreach(var parameter in method.Parameters!.Select((value, i) => (value, i)))
        {
            if (activationRecord.CheckIfMemberExists(parameter.value))
                continue;

            activationRecord.SetItem(parameter.value, arguments[parameter.i]);
        }

        _stack.Push(activationRecord);

        foreach(var statement in method.Statements)
        {
            if (statement.return_statement() is {} returnStatement)
                return Visit(returnStatement.expression())!;

            Visit(statement);
        }

        _stack.Pop();

        return null;
    }
}