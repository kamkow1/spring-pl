using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
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

    public void ConfigureServices(IServiceCollection services)
    {
        Console.WriteLine("callll");

        services.Configure<KestrelServerOptions>(options =>
        {
            options.AllowSynchronousIO = true;
        });
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseRouting();

        foreach (var endpoint in Endpoints)
        {
            app.UseEndpoints(endpoints =>
            {
                if (string.Equals(endpoint.HttpVerb, "GET", StringComparison.OrdinalIgnoreCase))
                {
                    endpoints.MapGet(endpoint.Path, (HttpRequest request) =>
                    {
                        var visitor = Initializer.Visitor;

				    	var queryParameters = request.Query;

                        var parameters = queryParameters.Keys.Cast<string>()
                            .ToDictionary(k => k, v => queryParameters[v]);

                        FunctionCaller.Call(
                            ref visitor.functionCallContext!,
                            visitor.Visit,
                            ref visitor.BuiltinFunctions,
                            ref visitor.RuntimeStack,
                            ref visitor.Functions,
				    		endpoint.Handler,
                            null,
				    		parameters
                        );


                    });
                }

                if (string.Equals(endpoint.HttpVerb, "POST", StringComparison.OrdinalIgnoreCase))
                {
                    endpoints.MapPost(endpoint.Path, (HttpRequest request) =>
                    {
                        var visitor = Initializer.Visitor;

				    	var queryParameters = request.Query;

                        var body = request.Body;
                        var streamReader = new StreamReader(body);
                        var bodyJson = streamReader.ReadToEnd();
                        var bodyObj = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(bodyJson);

                        var parameters = queryParameters.Keys.Cast<string>()
                            .ToDictionary(k => k, v => queryParameters[v]);

                        var response = FunctionCaller.Call(
                            ref visitor.functionCallContext!,
                            visitor.Visit,
                            ref visitor.BuiltinFunctions,
                            ref visitor.RuntimeStack,
                            ref visitor.Functions,
				    		endpoint.Handler,
                            null,
				    		parameters,
                            bodyObj!
                        );

                        Console.WriteLine("returned: " + JsonConvert.SerializeObject(response, Formatting.Indented));

                        if (string.Equals(endpoint.ResponseType, "ok", StringComparison.OrdinalIgnoreCase))
                            return Results.Ok(response);
                        
                        throw new Exception($"unimplemented response type {endpoint.ResponseType}");
                    });
                }
            });
        }
    }
}
