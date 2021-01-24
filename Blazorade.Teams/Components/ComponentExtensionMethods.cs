using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazorade.Teams.Components
{
    internal static class ComponentExtensionMethods
    {

        private const string LoginRequestFragment = "#blazorade-login-request";


        internal static string GetLoginHint(this NavigationManager navMan)
        {
            var uri = new Uri(navMan.Uri);
            return uri.Query?.Substring(1);
        }

        public static string GetLoginState(this NavigationManager navMan)
        {
            return Guid.NewGuid().ToString();
        }

        internal static bool IsLoginRequest(this NavigationManager navMan)
        {
            var uri = new Uri(navMan.Uri);
            return uri.Fragment == LoginRequestFragment;
        }

        internal static bool IsLoginResponse(this NavigationManager navMan)
        {
            var uri = new Uri(navMan.Uri);
            if(uri.Fragment?.Length > 0)
            {
                var arr = uri.Fragment.Split("&", StringSplitOptions.RemoveEmptyEntries);
                if(arr.Length > 0)
                {
                    Dictionary<string, string> args = new Dictionary<string, string>();

                }
            }

            return false;
        }
    }
}
