using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
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
                endpoints.MapGet(endpoint.Path, () => 
                {
                    
                });
            }); 
        }
    }
}