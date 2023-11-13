namespace Blazorade.Teams.Interop.Internal;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class TokenRequestInfo
{

    public string LoginHint { get; set; }

    public List<string> Scopes { get; set; }



    public static string CreateKey(string clientId)
    {
        return $"{clientId}.blazorade-teams.token-request-info";
    }
}
