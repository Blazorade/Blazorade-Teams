using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazorade.Teams.Components.Configuration
{
    public class AzureAdApplicationOptions
    {
        public AzureAdApplicationOptions()
        {
            this.TenantId = "common";
        }


        public string ClientId { get; set; }

        public string TenantId { get; set; }

    }
}
