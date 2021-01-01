using Blazorade.Teams.Configuration;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazorade.Teams.Interop
{
    internal class AuthenticationModule : InteropModuleBase
    {
        public AuthenticationModule(AzureAdApplicationOptions appOptions, IJSRuntime jsRuntime) : base(appOptions, jsRuntime) { }

    }
}
