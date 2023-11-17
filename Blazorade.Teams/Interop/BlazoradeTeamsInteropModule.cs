namespace Blazorade.Teams.Interop;

using Blazorade.Core.Interop;
using Blazorade.Teams.Components;
using Blazorade.Teams.Configuration;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// The root module for facilitating communications with the Teams JavaScript SDK.
/// </summary>
public class BlazoradeTeamsInteropModule : InteropModuleBase
{
    /// <summary>
    /// Creates a new instance of the module class.
    /// </summary>
    /// <remarks>
    /// This class should not be initialized in code. It will be created automatically through Dependency Injection
    /// and provided through the <see cref="TeamsApplication.TeamsInterop"/> property.
    /// </remarks>
    public BlazoradeTeamsInteropModule(BlazoradeTeamsOptions appOptions, IJSRuntime jsRuntime, ApplicationInitializationModule appInitModule, SettingsModule settingsModule, AuthenticationModule authModule) : base(appOptions, jsRuntime)
    {
        this.Authentication = authModule ?? throw new ArgumentNullException(nameof(authModule));
        this.AppInitialization = appInitModule ?? throw new ArgumentNullException(nameof(appInitModule));
        this.Settings = settingsModule ?? throw new ArgumentNullException(nameof(settingsModule));
    }

    /// <summary>
    /// Returns a module that is used for authentication.
    /// </summary>
    public AuthenticationModule Authentication { get; protected set; }

    /// <summary>
    /// A module that represents the <c>appInitialization</c> module in the Teams SDK.
    /// </summary>
    public ApplicationInitializationModule AppInitialization { get; protected set; }

    /// <summary>
    /// A module that represents the <c>settings</c> module in the Teams SDK.
    /// </summary>
    public SettingsModule Settings { get; protected set; }



    /// <summary>
    /// Returns the context information loaded from Teams.
    /// </summary>
    /// <remarks>
    /// This method is called for you when you use the <see cref="TeamsApplication"/> component.
    /// </remarks>
    public async Task<Context> GetContextAsync()
    {
        return await new DotNetInstanceCallbackHandler<Context>(await this.GetBlazoradeTeamsJSModuleAsync(), "getContext")
            .GetResultAsync();
    }

    /// <summary>
    /// Initializes your application with Teams.
    /// </summary>
    /// <remarks>
    /// This method is invoked for you when you use the <see cref="TeamsApplication"/> component.
    /// </remarks>
    public async Task InitializeAsync()
    {
        await new DotNetInstanceCallbackHandler(await this.GetBlazoradeTeamsJSModuleAsync(), "initialize")
            .GetResultAsync();
    }

    /// <summary>
    /// Determines whether the application has been loaded inside of Teams.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If this method returns <c>false</c>, all the other methods on this module an its child modules
    /// will also very likely fail.
    /// </para>
    /// <para>
    /// You don't have to worry about this when using the <see cref="TeamsApplication"/> component as the
    /// root component on your pages, and putting your application content in the
    /// <see cref="TeamsApplication.ApplicationTemplate"/> template.
    /// </para>
    /// </remarks>
    public async ValueTask<bool> IsTeamsHostAvailableAsync()
    {
        var module = await this.GetBlazoradeTeamsJSModuleAsync();
        return await module.InvokeAsync<bool>("isTeamsHostAvailable");
    }

}
