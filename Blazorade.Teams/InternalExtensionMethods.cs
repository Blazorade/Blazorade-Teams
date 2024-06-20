namespace Blazorade.Teams;

using Blazorade.Msal.Services;
using Blazorade.Teams.Configuration;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal static class InternalExtensionMethods
{


    public static string CreateKey(this TokenAcquisitionRequest request, string clientId)
    {
        return $"{clientId}.blazorade-teams.token-request-info";
    }
}
