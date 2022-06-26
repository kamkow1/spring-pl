using static spli.Interpreter.BuiltinFunctions.IO.IOFunctions;
using static spli.Interpreter.BuiltinFunctions.Casting.CastingFucntions;
using static spli.Interpreter.BuiltinFunctions.Array.ArrayFunctions;
using static spli.Interpreter.BuiltinFunctions.Web.WebFunctions;
using spli.Interpreter.Functions;
using spli.Interpreter.Enums;
using spli.Interpreter.Struct;

namespace spli.Interpreter.Visitors;

public partial class Visitor : SpringParserBaseVisitor<Object?>
{
    private Dictionary<string, Function> _availableFunctions = new();

    private Dictionary<string, Func<object?[]?, object?>> _builtinFunctions = new();

    private Dictionary<string, Structure> _structs = new();

    private Dictionary<string, EnumStructure> _enums = new();

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

        // web
        _builtinFunctions.Add("serve_static",   new Func<object?[]?, object?>(args => ServeStatic(args)));
    }
}