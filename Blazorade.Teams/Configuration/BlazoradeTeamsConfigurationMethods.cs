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
        /// <remarks>
        /// If you require that your application users are properly authenticated, then you need to use the
        /// other overloaded methods to specify the needed information.
        /// </remarks>
        public static IServiceCollection AddBlazoradeTeams(this IServiceCollection services)
        {
            return services
                .AddScoped<BlazoradeTeamsInteropModule>()
                .AddScoped<ApplicationInitializationModule>()
                .AddScoped<SettingsModule>()
                .AddScoped<AuthenticationModule>()
                .AddScoped<LocalStorageService>()
                .AddBlazoradeCore()
                .AddBlazoradeMsal((sp, config) =>
                {
                    var options = sp.GetService<BlazoradeTeamsOptions>();

                    config.ClientId = options.ClientId;
                    config.TenantId = options.TenantId;
                    config.RedirectUrl = options.LoginUrl;
                    config.DefaultScopes = options.DefaultScopes ?? config.DefaultScopes;

                    // We need to use the authentication dialog provided by Teams, in which we will perform a redirect authentication.
                    config.InteractiveLoginMode = InteractiveLoginMode.Redirect;

                    // Because we need to share the tokens from the authentication dialog with the main application.
                    config.TokenCacheScope = TokenCacheScope.Persistent; 
                })
                ;
        }

        /// <summary>
        /// Adds services needed by Blazorade Teams and allows you to specify the application configuration required
        /// in order to perform user authentication.
        /// </summary>
        public static IServiceCollection AddBlazoradeTeams(this IServiceCollection services, Action<BlazoradeTeamsOptions> config)
        {
            return services
                .AddSingleton((p) =>
                {
                    var options = new BlazoradeTeamsOptions();
                    config?.Invoke(options);
                    return options;
                })
                .AddBlazoradeTeams();
        }

        /// <summary>
        /// Adds services need by Blazorade Teams and allows you to configure the Azure AD application associated with your application
        /// using other services configured in the application.
        /// </summary>
        public static IServiceCollection AddBlazoradeTeams(this IServiceCollection services, Action<IServiceProvider, BlazoradeTeamsOptions> config)
        {
            return services
                .AddSingleton((p) =>
                {
                    var options = new BlazoradeTeamsOptions();
                    config?.Invoke(p, options);
                    return options;
                })
                .AddBlazoradeTeams()
                ;
        }
    }
}
