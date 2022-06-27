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
    public Dictionary<string, Function> Functions = new();

    public Dictionary<string, Func<object?[]?, object?>> BuiltinFunctions = new();

    public Dictionary<string, Structure> _structs = new();

    public Dictionary<string, EnumStructure> Enums = new();

    public CallStack RuntimeStack = new();

    public bool LastConditionResult;

    public bool ShouldSkipCurrentIteration = false;

    public bool _shouldExitCurrentLoop = false;

    public SpringParser.Function_callContext? functionCallContext;

    public Visitor()
    {
        // io
        BuiltinFunctions.Add("println",        new Func<object?[]?, object?>(args => Println(args)));
        BuiltinFunctions.Add("println_j",      new Func<object?[]?, object?>(args => PrintlnJson(args)));
        BuiltinFunctions.Add("print",          new Func<object?[]?, object?>(args => Print(args)));
        BuiltinFunctions.Add("read_console",   new Func<object?[]?, object?>(_ => ReadConsole()));

        // proc
        BuiltinFunctions.Add("create_proc",    new Func<object?[]?, object?>(args => CreateProc(args)));
        BuiltinFunctions.Add("start_proc",     new Func<object?[]?, object?>(args => StartProc(args)));
        BuiltinFunctions.Add("kill_proc",      new Func<object?[]?, object?>(args => KillProc(args)));
        BuiltinFunctions.Add("get_std_op",     new Func<object?[]?, object?>(args => GetStdOutput(args)));

        // fs
        BuiltinFunctions.Add("fs_read_file",   new Func<object?[]?, object?>(args => ReadFile(args)));

        // type casting
        BuiltinFunctions.Add("int",            new Func<object?[]?, object?>(args => Int(args)));
        BuiltinFunctions.Add("float",          new Func<object?[]?, object?>(args => Float(args)));
        BuiltinFunctions.Add("string",         new Func<object?[]?, object?>(args => String(args)));
        BuiltinFunctions.Add("bool",           new Func<object?[]?, object?>(args => Bool(args)));

        // array
        BuiltinFunctions.Add("arr_len",        new Func<object?[]?, object?>(args => ArrayLength(args)));
        BuiltinFunctions.Add("arr_last",       new Func<object?[]?, object?>(args => ArrayLast(args)));
        BuiltinFunctions.Add("arr_add",        new Func<object?[]?, object?>(args => ArrayAdd(args)));
        BuiltinFunctions.Add("arr_del",        new Func<object?[]?, object?>(args => ArrayDelete(args)));
        BuiltinFunctions.Add("arr_pop",        new Func<object?[]?, object?>(args => ArrayPop(args)));

        // web
        BuiltinFunctions.Add("serve_static",   new Func<object?[]?, object?>(args => ServeStatic(args)));
        BuiltinFunctions.Add("create_server",  new Func<object?[]?, object?>(args => CreateServer(args)));
        BuiltinFunctions.Add("create_endpoint",new Func<object?[]?, object?>(args => CreateEndpoint(args)));
        BuiltinFunctions.Add("run_server",     new Func<object?[]?, object?>(_ => { RunServer(); return null; }));
    }
}