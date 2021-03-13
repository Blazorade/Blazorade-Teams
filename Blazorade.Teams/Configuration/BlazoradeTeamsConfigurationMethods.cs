using Blazorade.Msal.Configuration;
using Blazorade.Teams.Configuration;
using Blazorade.Teams.Interop;
using Blazorade.Teams.Interop.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// By convention, the Add{Group} methods should be placed in the namespace below.
// https://docs.microsoft.com/aspnet/core/fundamentals/dependency-injection#register-groups-of-services-with-extension-methods

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for working with Dependency Injection in Blazorade Teams.
    /// </summary>
    public static class BlazoradeTeamsConfigurationMethods
    {

        /// <summary>
        /// Adds the necessary services required by Blazorade Teams.
        /// </summary>
        public static IBlazoradeTeamsBuilder AddBlazoradeTeams(this IServiceCollection services)
        {
            return new BlazoradeTeamsBuilder
            {
                Services = services
                    .AddBlazoradeCore()
                    .AddSingleton<BlazoradeTeamsOptions>()
                    .AddScoped<BlazoradeTeamsInteropModule>()
                    .AddScoped<ApplicationInitializationModule>()
                    .AddScoped<SettingsModule>()
                    .AddScoped<AuthenticationModule>()
                    .AddScoped<LocalStorageService>()
            };
        }

    }
}
