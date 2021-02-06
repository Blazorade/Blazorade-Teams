using Blazorade.Teams.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamsTabAppServer.Shared
{
    partial class MailView
    {
        [Parameter]
        public ApplicationContext Context { get; set; }

        [Parameter]
        public IEnumerable<Message> Messages { get; set; }



        protected async override Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            var authProvider = new AuthenticationProvider(this.Context, this.MsalService);
            var client = new GraphServiceClient(authProvider);
            this.Messages = await client.Me.MailFolders.Inbox.Messages.Request().GetAsync();
        }
    }
}
