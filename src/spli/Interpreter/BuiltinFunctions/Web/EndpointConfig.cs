using spli.Interpreter.Functions;

namespace spli.Interpreter.BuiltinFunctions.Web;

public class EndpointConfig
{
    public string Path { get; set; }

    public Function Handler { get; set; }

    public EndpointConfig(string path, Function handler)
    {
        Path = path;
        Handler = handler;
    }
}