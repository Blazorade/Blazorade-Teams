using Blazorade.Teams.Configuration;
using Blazorade.Teams.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for working with Dependency Injection in Blazorade Teams.
    /// </summary>
    public static class BlazoradeTeamsServiceCollectionExtensionMethods
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
                ;
        }

        /// <summary>
        /// Adds services needed by Blazorade Teams and allows you to specify the application configuration required
        /// in order to perform user authentication.
        /// </summary>
        public static IServiceCollection AddBlazoradeTeams(this IServiceCollection services, Action<AzureAdApplicationOptions> config)
        {
            return services
                .AddSingleton((p) =>
                {
                    var options = new AzureAdApplicationOptions();
                    config?.Invoke(options);
                    return options;
                })
                .AddBlazoradeTeams();
        }

        /// <summary>
        /// Adds services need by Blazorade Teams and allows you to configure the Azure AD application associated with your application
        /// using other services configured in the application.
        /// </summary>
        public static IServiceCollection AddBlazoradeTeams(this IServiceCollection services, Action<IServiceProvider, AzureAdApplicationOptions> config)
        {
            return services
                .AddSingleton((p) =>
                {
                    var options = new AzureAdApplicationOptions();
                    config?.Invoke(p, options);
                    return options;
                })
                .AddBlazoradeTeams()
                ;
        }
    }
}
