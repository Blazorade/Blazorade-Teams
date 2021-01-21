using System;
using Blazorade.Teams.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.Graph;
using System.Threading.Tasks;
using Blazorade.Teams.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace TeamsTabAppServer.Shared
{
    partial class UserInfoView
    {

        [Parameter]
        public ApplicationContext Context { get; set; }

        [Parameter]
        public string DisplayName { get; set; }

        [Parameter]
        public string Email { get; set; }

        [Parameter]
        public string FirstName { get; set; }

        [Parameter]
        public string JobTitle { get; set; }

        [Parameter]
        public string LastName { get; set; }

        [Parameter]
        public string MobilePhone { get; set; }

        [Parameter]
        public string Upn { get; set; }

        [Inject]
        private IOptions<AzureAdApplicationOptions> configuration { get; set; }


        protected async override Task OnParametersSetAsync()
        {
            var authProvider = new AuthenticationProvider(this.Context, configuration.Value);
            GraphServiceClient client = new GraphServiceClient(authProvider);
            var me = await client.Me.Request().GetAsync();

            this.DisplayName = me.DisplayName;
            this.FirstName = me.GivenName;
            this.LastName = me.Surname;
            this.JobTitle = me.JobTitle;
            this.Email = me.Mail;
            this.MobilePhone = me.MobilePhone;
            this.Upn = me.UserPrincipalName;

            await base.OnParametersSetAsync();
        }
    }
}
