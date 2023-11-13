namespace Blazorade.Teams.Configuration;

using Blazorade.Msal.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// The interface that defines the startup configuration builder for Blazorade Teams.
/// </summary>
public interface IBlazoradeTeamsBuilder
{

    /// <summary>
    /// The service collection that the builder uses.
    /// </summary>
    IServiceCollection Services { get; }

    /// <summary>
    /// Adds the necessary services required by Blazorade Teams.
    /// </summary>
    /// <param name="config">The delegate that will take care of setting the options for your application.</param>
    IBlazoradeTeamsBuilder WithOptions(Action<BlazoradeTeamsOptions> config);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="config">The delegate that will take care of setting the options for your application.</param>
    IBlazoradeTeamsBuilder WithOptions(Action<IServiceProvider, BlazoradeTeamsOptions> config);

}

/// <summary>
/// Default implementation of the <see cref="IBlazoradeTeamsBuilder"/> interface.
/// </summary>
internal class BlazoradeTeamsBuilder : IBlazoradeTeamsBuilder
{
    public IServiceCollection Services { get; internal set; }

    public IBlazoradeTeamsBuilder WithOptions(Action<BlazoradeTeamsOptions> config)
    {
        this.Services
            .AddSingleton((p) =>
            {
                var options = new BlazoradeTeamsOptions();
                config?.Invoke(options);
                return options;
            })
            .AddBlazoradeMsal(this.AddMsal)
            ;
        return this;
    }

    public IBlazoradeTeamsBuilder WithOptions(Action<IServiceProvider, BlazoradeTeamsOptions> config)
    {
        this.Services
            .AddSingleton((p) =>
            {
                p.GetService<object>();
                var options = new BlazoradeTeamsOptions();
                config?.Invoke(p, options);
                return options;
            })
            .AddBlazoradeMsal(this.AddMsal)
            ;
        return this;
    }

    private void AddMsal(IServiceProvider sp, BlazoradeMsalOptions config)
    {
        var options = sp.GetRequiredService<BlazoradeTeamsOptions>();
        if(options.ClientId?.Length > 0)
        {
            config.ClientId = options.ClientId;
            config.TenantId = options.TenantId;
            config.RedirectUrl = options.LoginUrl;
            config.DefaultScopes = options.DefaultScopes ?? config.DefaultScopes;

            // We need to use the authentication dialog provided by Teams, in which we will perform a redirect authentication.
            config.InteractiveLoginMode = InteractiveLoginMode.Redirect;

            // Because we need to share the tokens from the authentication dialog with the main application.
            config.TokenCacheScope = TokenCacheScope.Persistent;
        }
    }
}
