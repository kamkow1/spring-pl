using System.Linq;

namespace spli.Interpreter.BuiltinFunctions.Process;

public static class ProcessFunctions
{
    private static System.Diagnostics.Process _process;

    public static object? CreateProc(object?[]? args)
    {
        var filename = (string)args![0]!;
        var cmdArgs = new string[] {};

        foreach(var arg in args.Select((v, i) => new { v, i }))
        {
            if (arg.i == 0)
                continue;

            cmdArgs.Append(arg.v);
        }

        _process = new System.Diagnostics.Process();
        _process.StartInfo.FileName = filename;
        _process.StartInfo.Arguments = string.Join("", cmdArgs);
        _process.StartInfo.RedirectStandardInput = true;
        _process.StartInfo.RedirectStandardOutput = true;
        _process.StartInfo.RedirectStandardError = true;
        _process.StartInfo.CreateNoWindow = false;
        _process.StartInfo.UseShellExecute = false;

        return _process;
    }

    public static object? StartProc(object?[]? args)
    {
        var proc = (System.Diagnostics.Process)args![0]!;

        proc.Start();

        return null;
    }

    public static object? KillProc(object?[]? args)
    {
        var proc = (System.Diagnostics.Process)args![0]!;

        proc.Kill();

        return null;
    }

    public static object? GetStdOutput(object?[]? args)
    {
        var proc = (System.Diagnostics.Process)args![0]!;

        return proc.StandardOutput.ReadToEnd();
    }
}