using McMaster.Extensions.CommandLineUtils;

namespace spli.Cli.Commands;

public static class ExecCommand 
{
    public static string CommandName = "exec";

    public static string Description = "executes a .spring file";

    public static int Execute(CommandArgument? filePathArgument, string[] args)
    {
        var filePath = filePathArgument?.Value;

        if (filePath is null)
            throw new Exception($"could not resolve given path ${filePath}");

        var fileExtension = Path.GetExtension(Path.GetFileName(filePath));

        if (fileExtension != ".spring" && fileExtension != ".spr")
            throw new Exception($"cannot execute file with file extension {fileExtension}");

        var fileContent = File.ReadAllText(filePath);

        Initializer.Run(fileContent, filePath, args);

        return 0;
    }
}