namespace Blazorade.Teams.Configuration;

using Blazorade.Msal.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Options for configuring your application for authentication.
/// </summary>
public class BlazoradeTeamsOptions
{
    /// <summary>
    /// Creates a new instance.
    /// </summary>
    public BlazoradeTeamsOptions()
    {
        this.LoginUrl = "/login";
        this.DefaultScopes = new BlazoradeMsalOptions().DefaultScopes;
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

    /// <summary>
    /// The relative URL to the login page.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>/login</c>.
    /// </remarks>
    public string LoginUrl { get; set; }

    /// <summary>
    /// The default scopes to acquire if none are specified when acquiring tokens.
    /// </summary>
    /// <remarks>
    /// Defaults to the defaults specified on <see cref="BlazoradeMsalOptions.DefaultScopes"/>.
    /// </remarks>
    public IEnumerable<string> DefaultScopes { get; set; }
}
