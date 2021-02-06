using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TeamsTabAppClient
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services
                .AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) })
                .AddBlazoradeTeams((sp, config) =>
                {
                    var configService = sp.GetService<IConfiguration>();
                    config.ClientId = configService.GetValue<string>("clientId");
                    config.TenantId = configService.GetValue<string>("tenantId");

                    config.LoginUrl = "/login";
                })
                ;

            await builder.Build().RunAsync();
        }
    }
}
