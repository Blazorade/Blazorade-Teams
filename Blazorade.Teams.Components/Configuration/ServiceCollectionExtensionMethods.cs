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
                .AddScoped<AuthenticationModule>()
                ;
        }
    }
}
