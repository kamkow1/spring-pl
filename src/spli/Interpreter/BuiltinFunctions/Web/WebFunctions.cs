using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

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
        var httpVerb = (string)args![1]!;
		var functionName = (string)args![2]!;
        var responseType = (string)args![3]!;

        _server.CreateEndpoint(path, functionName, httpVerb, responseType);
        return null;
    }
    public static void RunServer()
    {
        _server.RunServer();
    }
}
