# Integration Testing with Refit

This is an example of integration testing an [ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-5.0) web api using [Refit](https://github.com/reactiveui/refit)
 
## Application Settings for the Web API

``` json
{
  "AzureAd": {
    "Instance": "",
    "Domain": "",
    "TenantId": "",
    "ClientId": "",

    "CallbackPath": "/signin-oidc"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "Urls": "https://localhost:44313",
  "AllowedHosts": "*"
}

```

| Setting           | Description                                                      |
| ----------------- | ---------------------------------------------------------------- |
| AzureAd__Instance | Something like https://login.microsoftonline.com                 |
| AzureAd_Domain    | Your tenant so like jwhmtenant.onmicrosoft.com                   |
| AzureAd_TenantId  | The tenant id (guid)                                             |
| AzureAd_ClientId  | The application id of your application registration for Azure AD |

## Application Settings for the Integration Test

``` json
{
  "ClientConfiguration": {
    "ApiServiceUri": ""
  },
  "TokenCreatorConfiguration": {
    "ClientId": "",
    "ClientSecret": "",
    "Scopes": "",
    "TenantId": "",
    "TestUsername": "",
    "TestPassword": ""
  }
}

```

| Setting                                 | Description                                                                                                          |
| --------------------------------------- | -------------------------------------------------------------------------------------------------------------------- |
| ClientConfiguration__ApiServiceUri      | The URL to your Web API                                                                                              |
| TokenCreatorConfiguration__ClientId     | The application id of your application registration for Azure AD                                                     |
| TokenCreatorConfiguration__ClientSecret | The client secret of the application                                                                                 |
| TokenCreatorConfiguration__Scopes       | The scopes of your application (mine were openid api://30827521-946b-4ce1-928c-169981998d48/graphapi offline_access) |
| TokenCreatorConfiguration__TenantId     | 9afb6f1a-9a48-4d54-9549-8a99d4c686c2                                                                                 |
| TokenCreatorConfiguration__TestUsername | The test username (mine was testuser@jwhmtenant.onmicrosoft.com)                                                     |
| TokenCreatorConfiguration__TestPassword | The password of the test user                                                                                        |
