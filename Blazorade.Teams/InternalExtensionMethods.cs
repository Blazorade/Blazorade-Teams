using Blazorade.Msal.Services;
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


        public static string CreateKey(this TokenAcquisitionRequest request, string clientId)
        {
            return $"{clientId}.blazorade-teams.token-request-info";
        }
    }
}
