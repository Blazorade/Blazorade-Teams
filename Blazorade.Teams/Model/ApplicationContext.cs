using Blazorade.Teams.Interop;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazorade.Teams.Model
{
    public class ApplicationContext
    {

        public Context Context { get; internal set; }

        //public AuthenticationResult AuthResult { get; internal set; }
        public JwtSecurityToken AuthResult { get; internal set; }

        public BlazoradeTeamsInteropModule TeamsInterop { get; internal set; }

    }
}
