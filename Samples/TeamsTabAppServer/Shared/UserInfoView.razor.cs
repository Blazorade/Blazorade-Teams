namespace TeamsTabAppServer.Shared;

using System;
using Blazorade.Teams.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.Graph;
using System.Threading.Tasks;

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


    protected async override Task OnParametersSetAsync()
    {
        var authProvider = new AuthenticationProvider(this.Context, this.MsalService);
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
