using System.Text.RegularExpressions;
using Antlr4.Runtime.Misc;
using static spli.Interpreter.Functions.IOFunctions;
using static spli.Interpreter.Functions.CastingFucntions;
using static spli.Interpreter.Functions.ArrayFunctions;
using Newtonsoft.Json;

namespace spli.Interpreter;

public class Visitor : SpringParserBaseVisitor<Object?>
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

    public override object? VisitIdentifier_expr([NotNull] SpringParser.Identifier_exprContext context)
    {
        var activationRecord = _stack.Peek();
        var name = context.IDENTIFIER().GetText();

        var previousAr = _stack.GetPreviousArOrCurrent();

        if (previousAr.Members.ContainsKey(name))
            return previousAr.GetItem(name);

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

        var function = new Function(parameters.ToArray(), statements);

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
        {
            if (activationRecord.CheckIfMemberExists(parameter.value))
                continue;

            activationRecord.SetItem(parameter.value, arguments[parameter.i]);
        }

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

        _lastConditionResult = condition;

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

        if (context.elif_statement() is {})
        {
            foreach(var elifStatement in context.elif_statement())
                Visit(elifStatement);
        }

        if (context.else_statement() is {} && !_lastConditionResult)
            Visit(context.else_statement());

        return null;
    }

    public override object? VisitElif_statement([NotNull] SpringParser.Elif_statementContext context)
    {
        var elifCondition = (bool)Visit(context.expression())!;
        _lastConditionResult = elifCondition;

        if (elifCondition! == true)
        {
            var activationRecord = new ActivationRecord();

            _stack.Push(activationRecord);
            foreach (var statement in context.scope().statement())
            {
                if (statement.return_statement() is {} returnStatement)
                    return Visit(returnStatement.expression());
                Visit(statement);
            }
            _stack.Pop();
        }

        return null;
    }

    public override object? VisitElse_statement([NotNull] SpringParser.Else_statementContext context)
    {
        var activationRecord = new ActivationRecord();

        _stack.Push(activationRecord);
        foreach (var statement in context.scope().statement())
        {
            if (statement.return_statement() is {} returnStatement)
                return Visit(returnStatement.expression());
            Visit(statement);
        }

        return _stack.Pop();
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

        if (new Regex("^==$", RegexOptions.Compiled).IsMatch(opr))
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

        if (new Regex("^!=$", RegexOptions.Compiled).IsMatch(opr))
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

        if (new Regex("^>=$", RegexOptions.Compiled).IsMatch(opr))
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

        if (new Regex("^<=$", RegexOptions.Compiled).IsMatch(opr))
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

        if (new Regex("^>$", RegexOptions.Compiled).IsMatch(opr))
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

        if (new Regex("^<$", RegexOptions.Compiled).IsMatch(opr))
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

    public override object VisitMathExpression([NotNull] SpringParser.MathExpressionContext context)
    {
        var oper = context.math_oper().GetText();

        var left = Visit(context.expression(0));
        var right = Visit(context.expression(1));

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

    public override object? VisitIndexingExpression([NotNull] SpringParser.IndexingExpressionContext context)
    {
        var array = (object?[])Visit(context.expression(0))!;
        var index = (int)Visit(context.expression(1))!;

        return array[index];
    }

    public override object? VisitLoop_statement([NotNull] SpringParser.Loop_statementContext context)
    {
        if (context.expression() is not {})
        {
            while(true)
            {
                if (_shouldExitCurrentLoop)
                {
                    _shouldExitCurrentLoop = false;
                    break;
                }

                var activationRecord = new ActivationRecord();

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

        if (context.expression() is {})
        {
            var loopConfig = (LoopConfiguration)Visit(context.expression())!;

            var isAscending = loopConfig.Left < loopConfig.Right;

            if (isAscending)
            {
                for (var i = loopConfig.Left; i <= loopConfig.Right; ++i)
                {
                    if (_shouldExitCurrentLoop)
                    {
                        _shouldExitCurrentLoop = false;
                        break;
                    }

                    var activationRecord = new ActivationRecord();

                    if (loopConfig.IteratorName is not null)
                        activationRecord.SetItem(loopConfig.IteratorName, i);

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
            else
            {
                for (var i = loopConfig.Left; i >= loopConfig.Right; --i)
                {
                    if (_shouldExitCurrentLoop)
                    {
                        _shouldExitCurrentLoop = false;
                        break;
                    }

                    var activationRecord = new ActivationRecord();

                    if (loopConfig.IteratorName is not null)
                        activationRecord.SetItem(loopConfig.IteratorName, i);
                        
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
        }

        return null;
    }

    public override object? VisitSkip_iteration([NotNull] SpringParser.Skip_iterationContext context)
    {
        _shouldSkipCurrentIteration = true;
        return null;
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