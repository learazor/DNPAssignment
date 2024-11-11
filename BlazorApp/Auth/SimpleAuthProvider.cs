using DTOs;
using Microsoft.JSInterop;

namespace BlazorApp.Auth;

using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;

public class SimpleAuthProvider : AuthenticationStateProvider
{
    private readonly HttpClient httpClient;
    private readonly IJSRuntime jsRuntime;
    private const string UserSessionKey = "currentUser";

    public SimpleAuthProvider(HttpClient httpClient, IJSRuntime jsRuntime)
    {
        this.httpClient = httpClient;
        this.jsRuntime = jsRuntime;
    }
    
    public async Task LoginAsync(string userName, string password)
    {
        var response = await httpClient.PostAsJsonAsync("auth/login", new LoginRequest { UserName = userName, Password = password });
        
        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            throw new Exception(errorMessage);
        }
        
        var userDto = await response.Content.ReadFromJsonAsync<UserDto>();
        
        string serializedData = JsonSerializer.Serialize(userDto);
        await jsRuntime.InvokeVoidAsync("sessionStorage.setItem", UserSessionKey, serializedData);
        
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(CreateClaimsPrincipal(userDto))));
    }
    
    public async Task LogoutAsync()
    {
        await jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", UserSessionKey);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal())));
    }
    
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var userAsJson = await jsRuntime.InvokeAsync<string>("sessionStorage.getItem", UserSessionKey);

        if (string.IsNullOrEmpty(userAsJson))
        {
            return new AuthenticationState(new ClaimsPrincipal());
        }

        var userDto = JsonSerializer.Deserialize<UserDto>(userAsJson);
        return new AuthenticationState(CreateClaimsPrincipal(userDto));
    }

    private ClaimsPrincipal CreateClaimsPrincipal(UserDto user)
    {
        if (user == null)
        {
            return new ClaimsPrincipal();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var identity = new ClaimsIdentity(claims, "apiauth");
        return new ClaimsPrincipal(identity);
    }
}
