using Blazorade.Msal.Security;
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
                var context = await this.TeamsInterop.GetContextAsync();

                AuthenticationResult authResult = null;

                if(this.NavMan.IsLoginRedirectUri())
                {
                    authResult = await this.MsalService.AcquireTokenInteractiveAsync(loginHint: context?.LoginHint);
                }
                else
                {
                    try
                    {
                        authResult = await this.MsalService.HandleRedirectPromiseAsync();
                    }
                    catch (Exception ex)
                    {
                        await this.TeamsInterop.Authentication.NotifyFailureAsync(reason: ex.ToString());
                    }
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
