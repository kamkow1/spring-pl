using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace spli.Interpreter.Functions.Web;

public class WebServer
{
    private IWebHost? _server;

    public void createServer()
    {
        var builder = WebHost.CreateDefaultBuilder();
        var server = builder.Build();
        _server = server;
    }


}