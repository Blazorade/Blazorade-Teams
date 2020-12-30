using Blazorade.Teams.Components.Configuration;
using Blazorade.Teams.Components.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensionMethods
    {

        public static IServiceCollection AddBlazoradeTeams(this IServiceCollection services)
        {
            return services
                .AddScoped<BlazoradeTeamsInteropModule>()
                .AddScoped<ApplicationInitializationModule>()
                ;
        }

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
