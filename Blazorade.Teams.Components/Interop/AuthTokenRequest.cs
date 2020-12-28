using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazorade.Teams.Components.Interop
{
    public class AuthTokenRequest
    {

        public List<string> Claims { get; set; }

        public List<string> Resources { get; set; }

        public bool? Silent { get; set; }



        public AuthTokenRequest WithClaims(params string[] claims)
        {
            this.Claims ??= new List<string>();
            foreach(var c in claims ?? new string[0])
            {
                this.Claims.Add(c);
            }
            return this;
        }

        public AuthTokenRequest WithResources(params string[] resources)
        {
            this.Resources ??= new List<string>();
            foreach(var r in resources ?? new string[0])
            {
                this.Resources.Add(r);
            }
            return this;
        }

        public AuthTokenRequest WithSilent(bool? silent)
        {
            this.Silent = silent;
            return this;
        }

    }
}
