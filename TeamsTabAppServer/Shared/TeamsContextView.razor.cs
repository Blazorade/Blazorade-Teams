using System;
using Blazorade.Teams.Model;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamsTabAppServer.Shared
{
    partial class TeamsContextView
    {

        [Parameter]
        public ApplicationContext Context { get; set; }

    }

}
