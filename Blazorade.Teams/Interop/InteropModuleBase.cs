namespace Blazorade.Teams.Interop;

using Blazorade.Teams.Configuration;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Base class implementation for interop modules.
/// </summary>
public abstract class InteropModuleBase
{

    /// <summary>
    /// Creates a new instance of the class.
    /// </summary>
    /// <param name="appOptions"></param>
    /// <param name="jsRuntime"></param>
    protected InteropModuleBase(BlazoradeTeamsOptions appOptions, IJSRuntime jsRuntime)
    {
        this.ApplicationSettings = appOptions ?? throw new ArgumentNullException(nameof(appOptions));
        this.JSRuntime = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
    }

    /// <summary>
    /// The JS runtime instance to use for interop calls.
    /// </summary>
    protected IJSRuntime JSRuntime { get; private set; }

    /// <summary>
    /// The application settings configured on the application.
    /// </summary>
    protected BlazoradeTeamsOptions ApplicationSettings { get; private set; }


    private IJSObjectReference _BlazoradeTeamsJSModule;
    /// <summary>
    /// Gets the JS Module that represents the JavaScript
    /// </summary>
    /// <returns></returns>
    protected async Task<IJSObjectReference> GetBlazoradeTeamsJSModuleAsync()
    {
        if(null == _BlazoradeTeamsJSModule)
        {
            //-------------------------------------------------------------------------
            // First import the JS modules that blazoradeTeams.js is dependent upon.
            var teamsModule = await this.GetTeamsSdkModuleAsync();
            //-------------------------------------------------------------------------

            _BlazoradeTeamsJSModule = await this.JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Blazorade.Teams/js/blazoradeTeams.js").AsTask();
        }

        return _BlazoradeTeamsJSModule;
    }



    private Task<IJSObjectReference> _TeamsSdkModule;
    private Task<IJSObjectReference> GetTeamsSdkModuleAsync()
    {
        return _TeamsSdkModule ??= this.JSRuntime.InvokeAsync<IJSObjectReference>("import", "https://statics.teams.cdn.office.net/sdk/v1.7.0/js/MicrosoftTeams.min.js").AsTask();
    }

}
