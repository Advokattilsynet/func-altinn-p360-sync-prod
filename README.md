# func-altinn-p360-sync-prod

## Development

Upon local development you'll need a file in root called `local.settings.json`, which will look something like this:

```
{
    "IsEncrypted": false,
    "Values": {
        "AzureWebJobsStorage": "UseDevelopmentStorage=true",
        "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
        "APPLICATIONINSIGHTS_CONNECTION_STRING": "InstrumentationKey=00000000-0000-0000-0000-000000000000",
        "APIM_URL": "",
        "APIM_SUBSCRIPTION_KEY": ""
    }
}
```

To get the `APPLICATIONINSIGHTS_CONNECTION_STRING`-value, check out the _**"Environment variables"**_-section under _**"Settings"**_ inside the Function App in Azure.

TBD: Info about how to get `APIM_URL` and `APIM_SUBSCRIPTION_KEY`, these should probably be removed in favor of Azure Secrets anyways.