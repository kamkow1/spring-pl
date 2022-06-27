using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using spli.Interpreter.Functions;

namespace spli.Interpreter.BuiltinFunctions.Web;

public static class WebFunctions
{
    private static WebServer _server = new WebServer();

    public static object? ServeStatic(object?[]? args)
    {
        var wwwroot = (string)args![0]!;
        WebHost
            .CreateDefaultBuilder()
            .Configure(c => c
                .UseDefaultFiles()
                .UseStaticFiles()
            )
            .UseWebRoot(wwwroot)
            .Build()
            .Run();

        return null;
    }

    public static object? CreateServer(object?[]? _)
    {
        _server.CreateServer();
        return null;
    }

    public static object? CreateEndpoint(object?[]? args)
    {
        var path = (string)args![0]!;
        var function = (Function)args![1]!;

        _server.CreateEndpoint(path, function);
        return null;
    }
    public static void RunServer()
    {
        _server.RunServer();
    }
}