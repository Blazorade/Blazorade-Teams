using Blazorade.Teams.Components;
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
    /// <para>
    /// This class must not be created by application in code. It will be provided through the <see cref="TeamsApplication.ApplicationContext"/>
    /// context property.
    /// </para>
    /// </remarks>
    public class SettingsModule : InteropModuleBase
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public SettingsModule(AzureAdApplicationOptions appOptions, IJSRuntime jsRuntime) : base(appOptions, jsRuntime) { }


        /// <summary>
        /// Registers 
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="savingCallback"></param>
        /// <param name="savingCallbackData"></param>
        /// <param name="successCallback"></param>
        /// <param name="failureCallback"></param>
        /// <returns></returns>
        public async Task RegisterOnSaveHandlerAsync(Settings settings, Func<Dictionary<string, object>, Task> savingCallback = null, Dictionary<string, object> savingCallbackData = null, Func<Task> successCallback = null, Func<Task> failureCallback = null)
        {
            var args = new CallbackMethodArgs
            {
                Args = new Dictionary<string, object>()
                {
                    {  "settings", settings },
                    { "savingCallback", CallbackDefinition.Create(savingCallback) },
                    { "savingCallbackData", savingCallbackData }
                },
                SuccessCallback = CallbackDefinition.Create(successCallback),
                FailureCallback = CallbackDefinition.Create(failureCallback)
            };

            var module = await this.GetBlazoradeTeamsJSModuleAsync();
            await module.InvokeVoidAsync("settings_registerOnSaveHandler", args);
        }

        /// <summary>
        /// Sets the validity state for the settings. The initial value is <c>false</c>, so the user
        /// cannot save the settings until this is called with <c>true</c>.
        /// </summary>
        /// <param name="validityState">Set to <c>true</c> if settings are valid and saving them is allowed.</param>
        public async Task SetValidityStateAsync(bool validityState)
        {
            var module = await this.GetBlazoradeTeamsJSModuleAsync();
            await module.InvokeVoidAsync("settings_setValidityState", validityState);
        }
    }
}
