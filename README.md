# func-altinn-p360-sync-prod

## Development

Upon local development you'll need a file in root called `local.settings.json` for Azure Application Insights, which will look something like this:

```
{
    "IsEncrypted": false,
    "Values": {
        "AzureWebJobsStorage": "UseDevelopmentStorage=true",
        "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
        "APPLICATIONINSIGHTS_CONNECTION_STRING": "InstrumentationKey=00000000-0000-0000-0000-000000000000"
    }
}
```

To get the `APPLICATIONINSIGHTS_CONNECTION_STRING`-value, check out the _**"Environment variables"**_-section under _**"Settings"**_ inside the Function App in Azure.

Addtionally, you will need to set the variables inside `appsettings.json` using `dotnet user-secrets`. The secrets existing values can be found in our Azure Keyvault.

