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

    

   

   

    

    

    

   

    

    

    

    

    

    public override object? VisitBail_statement([NotNull] SpringParser.Bail_statementContext context)
    {
        _shouldExitCurrentLoop = true;
        return null;
    }

    public override object VisitForLoopExpression([NotNull] SpringParser.ForLoopExpressionContext context)
    {
        var left = (int)Visit(context.expression(0))!;
        var right = (int)Visit(context.expression(1))!;

        var iteratorName = context.expression().ElementAtOrDefault(2) != null ? (string?)Visit(context.expression(2)) : null;

        return new LoopConfiguration
        {
            Left = left,
            Right = right,
            IteratorName = iteratorName
        };
    }

    public override object? VisitEach_loop_statement([NotNull] SpringParser.Each_loop_statementContext context)
    {
        var config = (EachConfiguration)Visit(context.expression())!;

        if (config.OptionalIteratorName is not null)
        {
            foreach(var item in config.Array.Select((value, i) => new { value, i }))
            {
                if (_shouldExitCurrentLoop)
                {
                    _shouldExitCurrentLoop = false;
                    break;
                }

                var activationRecord = new ActivationRecord();

                activationRecord.SetItem(config.ItemName, item.value);
                activationRecord.SetItem(config.OptionalIteratorName, item.i);

                _stack.Push(activationRecord);
                foreach (var statement in context.scope().statement())
                {
                    if (statement.return_statement() is {} returnStatement)
                        return Visit(returnStatement.expression());

                    if (_shouldSkipCurrentIteration)
                    {
                        _shouldSkipCurrentIteration = false;
                        continue;
                    }

                    Visit(statement);
                }
                _stack.Pop();
            }
        }
        else
        {
            foreach(var item in config.Array)
            {
                if (_shouldExitCurrentLoop)
                {
                    _shouldExitCurrentLoop = false;
                    break;
                }

                var activationRecord = new ActivationRecord();

                activationRecord.SetItem(config.ItemName, item);

                _stack.Push(activationRecord);
                foreach (var statement in context.scope().statement())
                {

                    if (_shouldSkipCurrentIteration)
                    {
                        _shouldSkipCurrentIteration = false;
                        continue;
                    }

                    if (statement.return_statement() is {} returnStatement)
                        return Visit(returnStatement.expression());


                    Visit(statement);
                }
                _stack.Pop();
            }
        }

        return null;
    }

    public override object VisitEachLoopExpression([NotNull] SpringParser.EachLoopExpressionContext context)
    {
        var array = (object?[])Visit(context.expression(1))!;

        var itemName = (string)Visit(context.expression(0))!;

        var iteratorName = context.expression().ElementAtOrDefault(2) != null ? (string?)Visit(context.expression(2)) : null;

        return new EachConfiguration(array, itemName, iteratorName);
    }

    public override object? VisitStruct_def([NotNull] SpringParser.Struct_defContext context)
    {
        var props = context.struct_content().prop_def().Select(Visit).ToList();
        var propDictionary = new Dictionary<string, Prop>();

        foreach(var prop in props)
            propDictionary.Add(((Prop)prop!).Name!, (Prop)prop);

        var methods = context.struct_content().method_def().Select(Visit).ToList();
        var methodDictionary = new Dictionary<string, Method>();

        foreach(var method in methods)
            methodDictionary.Add(((Method)method!).Name!, (Method)method);

        var name = context.IDENTIFIER().GetText();

        _structs.Add(name, new Structure(methodDictionary, propDictionary));

        return null;
    }

    public override object VisitProp_def([NotNull] SpringParser.Prop_defContext context)
    {
        var accessModifier = context.access_mod().GetText();
        var isPublic = accessModifier == "pub";
        var name = context.IDENTIFIER().GetText();

        return new Prop
        {
            Name = name,
            IsPublic = isPublic,
            Value = null
        };
    }

    public override object VisitMethod_def([NotNull] SpringParser.Method_defContext context)
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

        var accessModifier = context.access_mod().GetText();
        var isPublic = accessModifier == "pub";

        return new Method(name, isPublic, statements, parameters.ToArray());
    }

    public override object? VisitPropAccessExpression([NotNull] SpringParser.PropAccessExpressionContext context)
    {
        var structure = (StructureInstance)Visit(context.expression())!;
        var propName = context.IDENTIFIER().GetText();

        if (structure.Name == "self")
            return structure.BaseStructure.Props[propName].Value;

        if (structure.BaseStructure.Props[propName].IsPublic)
            return structure.BaseStructure.Props[propName].Value;

        throw new Exception($"cannot access a private property {propName}");
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