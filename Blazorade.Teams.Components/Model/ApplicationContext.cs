using Blazorade.Teams.Components.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazorade.Teams.Components.Model
{
    public class ApplicationContext
    {

        public Context Context { get; internal set; }

        public AuthenticationResult AuthResult { get; internal set; }

    }
}
