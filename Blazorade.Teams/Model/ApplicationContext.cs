using Blazorade.Msal.Security;
using Blazorade.Teams.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazorade.Teams.Model
{
    public class ApplicationContext
    {

        public Context Context { get; internal set; }

        public AuthenticationResult AuthResult { get; internal set; }

        public TimeSpan? ClientTimeZoneOffset { get; set; }

        public BlazoradeTeamsInteropModule TeamsInterop { get; internal set; }

    }
}
