using Blazorade.Teams.Components;
using Blazorade.Core.Interop;
using Blazorade.Teams.Configuration;
using Blazorade.Teams.Model;
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
    /// This class must not be created by application in code. It will be provided through the <see cref="ApplicationContext"/>
    /// context property.
    /// </para>
    /// </remarks>
    public class SettingsModule : InteropModuleBase
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public SettingsModule(BlazoradeTeamsOptions appOptions, IJSRuntime jsRuntime) : base(appOptions, jsRuntime) { }


        /// <summary>
        /// Gets the settings for the current instance.
        /// </summary>
        public async Task<Settings> GetSettingsAsync()
        {
            var handler = new DotNetInstanceCallbackHandler<Settings>(await this.GetBlazoradeTeamsJSModuleAsync(), "settings_getSettings");
            return await handler.GetResultAsync();
        }

        /// <summary>
        /// Registers a handler that Teams will call when the user clicks the Save button on the tab configuration dialog.
        /// </summary>
        /// <param name="settings">The settings to save when the handler is called.</param>
        /// <param name="savingCallback">
        /// <para>
        /// A callback that will be called before saving the settings with Teams.
        /// </para>
        /// <para>
        /// This allows you to perform tasks while the settings dialog is showing before the settings are saved
        /// with Teams and the dialog is closed. This is useful if you for instance want to perform some tasks
        /// to initialize your tab application.
        /// </para>
        /// 
        /// </param>
        /// <param name="savingCallbackData">The data that will be passed to <paramref name="savingCallback"/>.</param>
        /// <param name="successCallback">The callback that will be called when the settings have been successfully saved.</param>
        /// <param name="failureCallback">The callback that will be called when there was an error saving the settings.</param>
        public async Task RegisterOnSaveHandlerAsync(Settings settings, Func<Dictionary<string, object>, Task> savingCallback = null, Dictionary<string, object> savingCallbackData = null, Func<Task> successCallback = null, Func<Task> failureCallback = null)
        {
            var args = new DotNetInstanceCallbackArgs
            {
                Data = new Dictionary<string, object>()
                {
                    {  "settings", settings },
                    { "savingCallback", DotNetInstanceMethod.Create(savingCallback) },
                    { "savingCallbackData", savingCallbackData }
                },
                SuccessCallback = DotNetInstanceMethod.Create(successCallback),
                FailureCallback = DotNetInstanceMethod.Create(failureCallback)
            };

            var module = await this.GetBlazoradeTeamsJSModuleAsync();
            await module.InvokeVoidAsync("settings_registerOnSaveHandler", args);
        }

        /// <summary>
        /// Registers a handler that Teams will call when the user clicks the Remove button on the tab remove dialog.
        /// </summary>
        /// <param name="removingCallback">
        /// <para>
        /// The callback to call after the Remove button has been clicked and before the tab is actually
        /// removed from Teams.
        /// </para>
        /// <para>
        /// This allows you to perform tasks while the remove dialog is showing before the tab is removed. This is useful
        /// for instance if you want to remove data together with the tab.
        /// </para>
        /// </param>
        /// <param name="removingCallbackData">The data that will be passed to <paramref name="removingCallback"/>.</param>
        /// <param name="successCallback">The callback that will be called when the tab has successfully and completely been removed.</param>
        /// <param name="failureCallback">The callback that will be called in case an error occurs during removal.</param>
        /// <returns></returns>
        public async Task RegisterOnRemoveHandlerAsync(Func<Dictionary<string, object>, Task> removingCallback = null, Dictionary<string, object> removingCallbackData = null, Func<Task> successCallback = null, Func<Task> failureCallback = null)
        {
            var args = new DotNetInstanceCallbackArgs
            {
                Data = new Dictionary<string, object>
                {
                    { "removingCallback", DotNetInstanceMethod.Create(removingCallback) },
                    { "removingCallbackData", removingCallbackData }
                },
                SuccessCallback = DotNetInstanceMethod.Create(successCallback),
                FailureCallback = DotNetInstanceMethod.Create(failureCallback)
            };

            var module = await this.GetBlazoradeTeamsJSModuleAsync();
            await module.InvokeVoidAsync("settings_registerOnRemoveHandler", args);
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
