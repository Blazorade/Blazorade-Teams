using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazorade.Teams.Configuration
{
    /// <summary>
    /// Options for configuring your application for authentication.
    /// </summary>
    public class AzureAdApplicationOptions
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public AzureAdApplicationOptions()
        {
        }


        /// <summary>
        /// The application (client) ID of your Azure AD application.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// The tenant ID or tenant name ({tenant}.onmicrosoft.com) of the tenant your
        /// application specified in <see cref="ClientId"/> is registered in.
        /// </summary>
        public string TenantId { get; set; }

    }
}
