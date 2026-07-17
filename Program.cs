using Azure.Monitor.OpenTelemetry.Exporter;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Azure.Functions.Worker.OpenTelemetry;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using OpenTelemetry;
using func_altinn_p360_sync_prod.Models;
using func_altinn_p360_sync_prod.Services;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Security.KeyVault.Certificates;
using System.Security.Cryptography.X509Certificates;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services.AddOpenTelemetry()
    .UseFunctionsWorkerDefaults()
    .UseAzureMonitorExporter();

// Load app settings/secrets
var apimSettings = builder.Configuration.GetSection("ApimSettings").Get<ApimSettings>();
if (apimSettings == null || string.IsNullOrEmpty(apimSettings.KeyVaultUri))
{
    throw new Exception("ApimSettings or KeyVaultUri is missing in configuration.");
}

// Get cert from Keyvault
X509Certificate2 cert;
try
{
    var secretClient = new SecretClient(
        new Uri(apimSettings.KeyVaultUri),
        new DefaultAzureCredential()
    );

    KeyVaultSecret certSecret = await secretClient.GetSecretAsync(apimSettings.CertName!);
    byte[] privateKeyBytes = Convert.FromBase64String(certSecret.Value);

    cert = X509CertificateLoader.LoadPkcs12(privateKeyBytes, null, X509KeyStorageFlags.MachineKeySet);
}
catch (Exception ex)
{
    Console.WriteLine($"Failed to load certificate from Key Vault: {ex.Message}");
    throw;
}

// HttpClient
var appId = "tra/soknad-om-advokatbevilling";
var todaysDate = DateTime.Now.ToString("yyyy-MM-dd");
builder.Services.AddHttpClient<AltinnService>(client => {
    client.BaseAddress = new Uri(apimSettings.Url! + $"?appId={appId}&process.isComplete=true&process.ended=gt:{todaysDate}");
})
.ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler();
    handler.ClientCertificates.Add(cert);
    return handler;
});

builder.Services.Configure<ApimSettings>(builder.Configuration.GetSection("ApimSettings")); // Bind settings to DI container for rest of the app

// Add Keyvault source
builder.Configuration.AddAzureKeyVault(
    new Uri(apimSettings.KeyVaultUri),
    new DefaultAzureCredential());

builder.Build().Run();
