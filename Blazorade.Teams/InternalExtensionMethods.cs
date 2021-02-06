using Blazorade.Teams.Configuration;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazorade.Teams
{
    internal static class InternalExtensionMethods
    {
        private const string LoginRedirectFragment = "redirect-to-login";

        internal static Uri CreateLoginRedirectUri(this NavigationManager navMan, BlazoradeTeamsOptions options)
        {
            var loginUri = new Uri(options.LoginUrl, UriKind.RelativeOrAbsolute);
            if (!loginUri.IsAbsoluteUri)
            {
                loginUri = navMan.ToAbsoluteUri(options.LoginUrl);
            }

            var builder = new UriBuilder(loginUri);
            builder.Fragment = LoginRedirectFragment;
            loginUri = builder.Uri;

            return loginUri;
        }

        internal static bool IsLoginRedirectUri(this NavigationManager navMan)
        {
            var uri = new Uri(navMan.Uri);
            return uri.Fragment?.Substring(1) == LoginRedirectFragment; //Starting from the second chart to compare, since the first char in the fragment is always '#'.
        }
    }
}
