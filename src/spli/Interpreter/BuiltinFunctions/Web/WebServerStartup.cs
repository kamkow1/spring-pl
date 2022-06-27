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
            Console.WriteLine("endp");

            app.UseEndpoints(endpoints => 
            {
                Console.WriteLine("u e");
                endpoints.MapGet(endpoint.Path, () => 
                {
                    Console.WriteLine("get");

                    var visitor = Initializer.Visitor;

                    Console.WriteLine(endpoint.Handler.Name);

                    FunctionCaller.Call(
                        ref visitor.functionCallContext,
                        visitor.Visit,
                        ref visitor.BuiltinFunctions,
                        ref visitor._stack,
                        ref visitor._availableFunctions,
                        endpoint.Handler.Name
                    );
                });
            }); 
        }
    }
}