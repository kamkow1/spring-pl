using System.Text;
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

        _server.CreateEndpoint(path, functionName, httpVerb);
        return null;
    }
    public static void RunServer()
    {
        _server.RunServer();
    }

    public static object? CreateResponse(object?[]? args)
    {
        var value = args![0];
        var type = (string)args![1]!;
        var contentType = args.Length - 1 >= 2 ? (string?)args![2] : null;
        var code = args.Length - 1 >= 3 ? (int?)args![3] : null;
        var encoding = args.Length - 1 >= 4 ? (string?)args![4] : null;

        switch (type)
        {
            case "ok":          return Results.Ok(value);
            case "bad":         return Results.BadRequest(value);
            case "content":     return Results.Content((string)value!, contentType, Encoding.GetEncoding(encoding!));
            case "conflict":    return Results.Conflict();
            case "code":        return Results.StatusCode((int)code!);
        }

        throw new Exception($"couldn't match reposne type {type}");
    }
}
