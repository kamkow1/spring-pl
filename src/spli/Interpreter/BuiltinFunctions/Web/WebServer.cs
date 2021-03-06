using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using spli.Interpreter.Functions;

namespace spli.Interpreter.BuiltinFunctions.Web;

public class WebServer
{
    private IWebHost? _server;

    public void CreateServer()
    {
        var server = WebHost.CreateDefaultBuilder()
            .UseStartup<WebServerStartup>()
            .Build();

        _server = server;
    }

    public void CreateEndpoint(string path, 
                            string handler, 
                            string httpVerb)
    {
        WebServerStartup.AddEndpoint(WebServerStartup.Endpoints, 
                                    new EndpointConfig(path, 
                                                        handler, 
                                                        httpVerb));
    }

    public void RunServer()
    {
        _server!.Run();
    }
}
