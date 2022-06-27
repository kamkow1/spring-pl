using static spli.Interpreter.BuiltinFunctions.IO.IOFunctions;
using static spli.Interpreter.BuiltinFunctions.Casting.CastingFucntions;
using static spli.Interpreter.BuiltinFunctions.Array.ArrayFunctions;
using static spli.Interpreter.BuiltinFunctions.Web.WebFunctions;
using static spli.Interpreter.BuiltinFunctions.FileSystem.FileSystemFunctions;
using static spli.Interpreter.BuiltinFunctions.Process.ProcessFunctions;
using spli.Interpreter.Functions;
using spli.Interpreter.Enums;
using spli.Interpreter.Struct;
using spli.Interpreter.Stack;

namespace spli.Interpreter.Visitors;

public partial class Visitor : SpringParserBaseVisitor<Object?>
{
    public Dictionary<string, Function> _availableFunctions = new();

    public Dictionary<string, Func<object?[]?, object?>> _builtinFunctions = new();

    public Dictionary<string, Structure> _structs = new();

    public Dictionary<string, EnumStructure> _enums = new();

    public CallStack _stack = new();

    public bool _lastConditionResult;

    public bool _shouldSkipCurrentIteration = false;

    public bool _shouldExitCurrentLoop = false;

    public SpringParser.Function_callContext? functionCallContext;

    public Visitor()
    {
        // io
        _builtinFunctions.Add("println",        new Func<object?[]?, object?>(args => Println(args)));
        _builtinFunctions.Add("print",          new Func<object?[]?, object?>(args => Print(args)));
        _builtinFunctions.Add("read_console",   new Func<object?[]?, object?>(_ => ReadConsole()));

        // proc
        _builtinFunctions.Add("create_proc",    new Func<object?[]?, object?>(args => CreateProc(args)));
        _builtinFunctions.Add("start_proc",     new Func<object?[]?, object?>(args => StartProc(args)));
        _builtinFunctions.Add("kill_proc",      new Func<object?[]?, object?>(args => KillProc(args)));
        _builtinFunctions.Add("get_std_op",     new Func<object?[]?, object?>(args => GetStdOutput(args)));

        // fs
        _builtinFunctions.Add("fs_read_file",   new Func<object?[]?, object?>(args => ReadFile(args)));

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
        _builtinFunctions.Add("create_server",  new Func<object?[]?, object?>(args => CreateServer(args)));
        _builtinFunctions.Add("create_endpoint",new Func<object?[]?, object?>(args => CreateEndpoint(args)));
        _builtinFunctions.Add("run_server",     new Func<object?[]?, object?>(_ => { RunServer(); return null; }));
    }
}