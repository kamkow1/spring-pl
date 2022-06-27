using McMaster.Extensions.CommandLineUtils;
using Newtonsoft.Json;
using spli.Cli.Commands;

var app = new CommandLineApplication
{
    Name = "spli",
    FullName = "interpreter for the spring/PL",
    Description = "a project made for fun by kamkow1"
};

app.HelpOption();

var argsOption = app.Option("-a|--args", "adds commandline arguments to program", CommandOptionType.MultipleValue);
var verboseOption = app.Option("-v|--verbose", "enables more verbose output from the interpreter (ideal for debugging)", CommandOptionType.NoValue);

app.Command(ExecCommand.CommandName, cmd => 
{
    cmd.AddOption(argsOption);
    cmd.AddOption(verboseOption);

    cmd.Description = ExecCommand.Description;

    var filePathArgument = cmd.Argument("[root]", "path to a spring file");

    var isVerbose = verboseOption.Value() is {};
    
    cmd.OnExecute(() => ExecCommand.Execute(filePathArgument, isVerbose, argsOption.Values.ToArray()!));
});



return app.Execute(args);