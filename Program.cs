using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace DllMetadataExtractor;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        // Redirect console logging to stderr
        builder.Logging.AddConsole(options => options.LogToStandardErrorThreshold = LogLevel.Trace);

        builder.Services.AddSingleton<Extractor>();
        builder.Services
            .AddMcpServer(server =>
            {
                server.ServerInfo = new() { Name = "DllMetadataExtractor", Version = "1.0.0" };
            })
            .WithStdioServerTransport()
            .WithToolsFromAssembly(); // Scans the assembly for [McpServerToolType]

        var host = builder.Build();
        await host.RunAsync();
    }
}


[McpServerToolType]
public static class DllMetadataTool
{
    [McpServerTool, Description("Analyzes a .NET assembly file (.dll) to extract its complete public API information, including classes, methods, properties, signatures, and XML documentation comments if available. It returns a detailed JSON string. The primary use case is for an AI to get the most accurate, up-to-date API definition of a library when API calls fail or usage is unclear, helping to analyze and correct code. It is crucial that the caller should make to find and provides the absolute path to the target library's DLL file.")]
    public static string ExtractMetadata(
        Extractor extractor,
        [Description("The absolute path to the DLL file.")] string dllPath)
    {
        return extractor.Extract(dllPath);
    }
} 