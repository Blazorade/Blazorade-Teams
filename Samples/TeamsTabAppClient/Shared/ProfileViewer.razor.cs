namespace TeamsTabAppClient.Shared;

using Blazorade.Teams.Model;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

partial class ProfileViewer
{

    [Parameter]
    public ApplicationContext Context { get; set; }

    [Parameter]
    public UserProfileModel Model { get; set; } = new UserProfileModel();

    private static HttpClient Client = new HttpClient();

    protected async override Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        var authResult = await this.MsalService.AcquireTokenSilentAsync(loginHint: this.Context.Context.LoginHint);
        var request = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/me/");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authResult.AccessToken);
        var response = await Client.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            this.Model = JsonSerializer.Deserialize<UserProfileModel>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}

public class UserProfileModel
{
    public string DisplayName { get; set; }

    public string JobTitle { get; set; }

    public string Mail { get; set; }

    public string MobilePhone { get; set; }

    public string UserPrincipalName { get; set; }

}
