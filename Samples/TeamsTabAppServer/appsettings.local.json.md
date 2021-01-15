# appsettings.local.json

To successfully run this sample application locally, you need to make sure that you have a `appsettings.local.json` file in the same folder with this documentation. That settings file has deliberately been excluded from source control.

The contents of that file must be:

``` JSON
{
    "teamsApp": {
        "clientId": "<client ID of your app>",
        "tenantId": "<yourtenant>.onmicrosoft.com"
    }
}
```

- `clientId`: The application ID or client ID of your application.
- `tenantId`: The tenant your application is registered in. This can either be the tenant GUID, the default domain ([your tenant].onmicrosoft.com) or any vanity domain you have configured on your tenant.

## Permissions to Microsoft Graph

This application requires the following permissions to Microsoft Graph

### Delegated Permissions

- `User.Read`: This permission is configured on any application that you register with Azure AD. Just make sure that you don't remove that.

You can let each user grant the permission themselves when they use the app, or as a global admin, grant the permission on behalf of your entire tenant.
