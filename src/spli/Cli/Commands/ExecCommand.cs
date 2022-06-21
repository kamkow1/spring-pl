using McMaster.Extensions.CommandLineUtils;

namespace spli.Cli.Commands;

public static class ExecCommand 
{
    public static string CommandName = "exec";

    public static string Description = "executes a .spring file";

    public static int Execute(CommandArgument? filePathArgument)
    {
        var filePath = filePathArgument?.Value;

        if (filePath is null)
        {
            Console.WriteLine($"could not resolve given path ${filePath}");
            Environment.Exit(1);
        }

        var fileExtension = Path.GetExtension(Path.GetFileName(filePath));
        Console.WriteLine(fileExtension);

        if (fileExtension != ".spring" && fileExtension != ".spr")
        {
            Console.WriteLine($"cannot execute file with file extension {fileExtension}");
            Environment.Exit(1);
        }

        var fileContent = File.ReadAllText(filePath);


        return 0;
    }
}