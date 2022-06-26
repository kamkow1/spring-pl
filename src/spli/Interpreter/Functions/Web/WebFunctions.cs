using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace spli.Interpreter.Functions.Web;

public static class WebFunctions
{
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
}