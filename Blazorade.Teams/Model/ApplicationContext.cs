using Blazorade.Teams.Interop;
using System.IdentityModel.Tokens.Jwt;

namespace Blazorade.Teams.Model
{
    public class ApplicationContext
    {
        public Context Context { get; internal set; }

        public JwtSecurityToken AuthResult { get; internal set; }

        public BlazoradeTeamsInteropModule TeamsInterop { get; internal set; }
    }
}
