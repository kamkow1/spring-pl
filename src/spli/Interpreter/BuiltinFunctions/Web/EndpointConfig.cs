using Microsoft.AspNetCore.Http;

namespace spli.Interpreter.BuiltinFunctions.Web;

public class EndpointConfig
{
    public string Path { get; set; }

    public string Handler { get; set; }

    public string HttpVerb { get; set; }

    public string ResponseType { get; set; }

    public EndpointConfig(string path, 
                        string handler, 
                        string httpVerb,
                        string responseType)
    {
        Path = path;
        Handler = handler;
        HttpVerb = httpVerb;
        ResponseType = responseType;
    }
}
