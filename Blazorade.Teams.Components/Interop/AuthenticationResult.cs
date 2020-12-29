using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazorade.Teams.Components.Interop
{
    public class AuthenticationResult
    {

        public string AccessToken { get; set; }

        public DateTimeOffset? ExpiresOn { get; set; }

        public string IdToken { get; set; }

        public Dictionary<string, object> IdTokenClaims { get; set; }

        public List<string> Scopes { get; set; }

        public string TokenType { get; set; }

        public bool? IsFailed { get; set; }

        public string FailureReason { get; set; }

    }
}
