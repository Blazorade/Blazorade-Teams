namespace Blazorade.Teams.Components;

using Blazorade.Msal.Security;
using Blazorade.Msal.Services;
using Blazorade.Teams.Interop.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

partial class LoginDialogHandler
{
    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            await this.TeamsInterop.InitializeAsync();

            AuthenticationResult authResult = null;

            try
            {
                authResult = await this.MsalService.HandleRedirectPromiseAsync();
                if(null == authResult)
                {
                    var request = new TokenAcquisitionRequest();
                    var key = request.CreateKey(this.Options.ClientId);
                    request = await this.LocalStorage.GetItemAsync<TokenAcquisitionRequest>(key);
                    await this.LocalStorage.RemoveItemAsync(key);
                    await this.MsalService.AcquireTokenInteractiveAsync(request);
                }
            }
            catch (Exception ex)
            {
                await this.TeamsInterop.Authentication.NotifyFailureAsync(reason: ex.ToString());
            }

            if(null != authResult)
            {
                var json = JsonSerializer.Serialize(authResult);
                await this.TeamsInterop.Authentication.NotifySuccessAsync(result: json);
            }
        }
    }
}
