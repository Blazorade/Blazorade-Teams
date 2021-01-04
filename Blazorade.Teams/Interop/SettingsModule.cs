using Blazorade.Teams.Configuration;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazorade.Teams.Interop
{
    /// <summary>
    /// Represents the settings module in the Teams SDK.
    /// </summary>
    /// <remarks>
    /// This class must not be created by application in code. It will be provided through the <see cref="TeamsApplication.ApplicationContext"/>
    /// context property.
    /// </remarks>
    public class SettingsModule : InteropModuleBase
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public SettingsModule(AzureAdApplicationOptions appOptions, IJSRuntime jsRuntime) : base(appOptions, jsRuntime) { }


        public async Task RegisterOnSaveHandlerAsync(Settings settings, Func<Task> successCallback, Func<Task> failureCallback)
        {
            var args = new CallbackMethodArgs
            {
                Args = new Dictionary<string, object>()
                {
                    {  "settings", settings }
                },
                SuccessCallback = CallbackDefinition.Create(successCallback),
                FailureCallback = CallbackDefinition.Create(failureCallback)
            };

            var module = await this.GetBlazoradeTeamsJSModuleAsync();
            await module.InvokeVoidAsync("settings_registerOnSaveHandler", args);
        }

        public async Task SetValidityStateAsync(bool validityState)
        {
            var module = await this.GetBlazoradeTeamsJSModuleAsync();
            await module.InvokeVoidAsync("settings_setValidityState", validityState);
        }
    }
}
