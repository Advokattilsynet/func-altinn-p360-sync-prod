using Azure.Monitor.OpenTelemetry.Exporter;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Azure.Functions.Worker.OpenTelemetry;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry;
using System.Security.Cryptography.X509Certificates;
using func_altinn_p360_sync_prod.Models;
using func_altinn_p360_sync_prod.Services;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services.AddOpenTelemetry()
    .UseFunctionsWorkerDefaults()
    .UseAzureMonitorExporter();

builder.Services.AddHttpClient<AltinnService>(client => {
    client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("APIM_URL"));
})
.ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler();

    // currently working locally if you have the files at root
    // to be removed in favor of Key Vault or similar
    try
    {
        X509Certificate2 cert = X509Certificate2.CreateFromPemFile(
            "certificate.pem",
            "keyfile.pem"
        );

        handler.ClientCertificates.Add(cert);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Failed to load certificate: {ex.Message}");
        throw;
    }
    return handler;
});

builder.Build().Run();
