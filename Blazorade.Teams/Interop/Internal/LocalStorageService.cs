namespace Blazorade.Teams.Interop.Internal;

using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class LocalStorageService
{
    public LocalStorageService(IJSRuntime jsRuntime)
    {
        this.JSRuntime = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
    }

    private readonly IJSRuntime JSRuntime;


    public async Task ClearAsync()
    {
        await this.JSRuntime.InvokeVoidAsync("localStorage.clear");
    }

    public async Task<string> GetItemAsync(string key)
    {
        return await this.JSRuntime.InvokeAsync<string>("localStorage.getItem", key);
    }

    public async Task<T> GetItemAsync<T>(string key)
    {
        var json = await this.GetItemAsync(key);
        return JsonSerializer.Deserialize<T>(json);
    }

    public async Task RemoveItemAsync(string key)
    {
        await this.JSRuntime.InvokeVoidAsync("localStorage.removeItem", key);
    }

    public async Task SetItemAsync(string key, string value)
    {
        await this.JSRuntime.InvokeVoidAsync("localStorage.setItem", key, value);
    }

    public async Task SetItemAsync(string key, object value)
    {
        string str = null;
        if(null != value)
        {
            str = JsonSerializer.Serialize(value);
        }

        await this.SetItemAsync(key, str);
    }
}
