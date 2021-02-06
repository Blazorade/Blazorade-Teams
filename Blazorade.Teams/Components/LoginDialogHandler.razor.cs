using Blazorade.Msal.Security;
using Blazorade.Teams.Interop.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Blazorade.Teams.Components
{
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
                        var key = TokenRequestInfo.CreateKey(this.Options.ClientId);
                        var requestInfo = await this.LocalStorage.GetItemAsync<TokenRequestInfo>(key);
                        await this.LocalStorage.RemoveItemAsync(key);
                        await this.MsalService.AcquireTokenInteractiveAsync(loginHint: requestInfo.LoginHint, scopes: requestInfo.Scopes);
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
}
