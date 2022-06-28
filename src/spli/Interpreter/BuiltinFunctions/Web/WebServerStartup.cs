using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using spli.Interpreter.Functions;

namespace spli.Interpreter.BuiltinFunctions.Web;

public class WebServerStartup
{
    public static List<EndpointConfig> Endpoints = new();

    public static void AddEndpoint(List<EndpointConfig> endpoints, EndpointConfig endpoint)
    {
        endpoints.Add(endpoint);
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseRouting();

        foreach (var endpoint in Endpoints)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet(endpoint.Path, () =>
                {
                    var visitor = Initializer.Visitor;

					Console.WriteLine("handler name " + endpoint.Handler);

                    FunctionCaller.Call(
                        ref visitor.functionCallContext!,
                        visitor.Visit,
                        ref visitor.BuiltinFunctions,
                        ref visitor.RuntimeStack,
                        ref visitor.Functions,
						endpoint.Handler
                    );
                });
            });
        }
    }
}
