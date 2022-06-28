using spli.Interpreter.Functions;

namespace spli.Interpreter.BuiltinFunctions.Web;

public class EndpointConfig
{
    public string Path { get; set; }

    public string Handler { get; set; }

    public EndpointConfig(string path, string handler)
    {
        Path = path;
        Handler = handler;
    }
}
