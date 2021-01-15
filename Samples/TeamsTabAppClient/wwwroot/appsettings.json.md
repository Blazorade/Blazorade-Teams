# appsettings.json

The `appsettings.json` settings file is deliberately excluded from source control. To properly run this sample application in your environment, you need to register the application in Azure AD in your tenant. This is documented in detail on the [Blazorade Teams wiki](https://github.com/Blazorade/Blazorade-Teams/wiki/Getting-Started-Register-Application).

When you have that completed, you need to add the `appsettings.json` file to the same folder with this documentation, and set its content as shown below.

``` JSON
{
	"clientId": "string",
	"tenantId": "string"
}
```

- `clientId`: The client ID (or application ID) of the application you registered in Azure AD.
- `tenantId`: The ID (Guid) or default domain (yourtenant.onmicrosoft.com) or your primary vanity domain (yourcompany.com) of the tenant you registered the application in.