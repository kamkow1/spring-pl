using McMaster.Extensions.CommandLineUtils;
using spli.Cli.Commands;

var app = new CommandLineApplication
{
    Name = "spli",
    FullName = "interpreter for the spring/PL",
    Description = "a project made for fun by kamkow1"
};

app.HelpOption();


app.Command(ExecCommand.CommandName, cmd => 
{
    cmd.Description = ExecCommand.Description;

    var filePathArgument = cmd.Argument("[root]", "path to a spring file");

    cmd.OnExecute(() => ExecCommand.Execute(filePathArgument));
});

app.OnExecute(() => 
{
    Console.WriteLine("finished executing");
    return 0;
});

return app.Execute(args);
