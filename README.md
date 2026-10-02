# func-altinn-p360-sync-prod

## Development

### Prerequisites

- Azure CLI
- Access to Advokattilsynets Azure subscription

### Local secrets / settings

#### local.settings.json

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

#### appsettings.json

You will need to set the variables inside `appsettings.json` using `dotnet user-secrets`. The secrets existing values can be found in our Azure Keyvault.

1. Initialize user-secrets by running dotnet user-secrets init.
2. Start setting secrets by running dotnet user-secrets set "HEADING:SUB-HEADING" "VALUE", e.g. dotnet user-secrets set "ApimSettings:SubscriptionKey" "12345".

You will now see your UserSecretsId being added to `func-altinn-p360-sync-prod.csproj`, similar to the one that's existing `<UserSecretsId>17cc8b3f-a8f0-4f10-a4c1-c82be03fa50d</UserSecretsId>`.

### Working with the app locally

`func start`
