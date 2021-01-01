using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazorade.Teams.Configuration
{
    internal class MsalConfig
    {
        public MsalConfig(AzureAdApplicationOptions appOptions)
        {
            this.Auth = new MsalAuthConfig
            {
                ClientId = appOptions.ClientId,
                Authority = $"https://login.microsoftonline.com/{appOptions.TenantId ?? "common"}"
            };
        }

        public MsalAuthConfig Auth { get; set; }

    }

    internal class MsalAuthConfig
    {
        public string ClientId { get; set; }

        public string Authority { get; set; }
    }
}
