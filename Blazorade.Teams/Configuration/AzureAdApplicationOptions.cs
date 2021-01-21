namespace Blazorade.Teams.Configuration
{
    /// <summary>
    /// Options for configuring your application for authentication.
    /// </summary>
    public class AzureAdApplicationOptions
    {
        public AzureAdApplicationOptions()
        {
            TenantId = "common";
        }

        /// <summary>
        /// appsettings section 
        /// </summary>
        public const string Section = "teamsApp";

        /// <summary>
        /// The application (client) ID of your Azure AD application.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// The tenant ID or tenant name ({tenant}.onmicrosoft.com) of the tenant your
        /// application specified in <see cref="ClientId"/> is registered in.
        /// </summary>
        public string TenantId { get; set; }

        /// <summary>
        /// The client secret configured in AAD for the application.
        /// This is required for the on-behalf-flow when exchanging the token to
        /// enable call Microsoft Graph
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Typically set to https://login.microsoftonline.com/
        /// </summary>
        public string AADInstance { get; set; }

        /// <summary>
        /// Used when validating the JWT
        /// </summary>
        public string Authority => AADInstance + TenantId;
    }
}
