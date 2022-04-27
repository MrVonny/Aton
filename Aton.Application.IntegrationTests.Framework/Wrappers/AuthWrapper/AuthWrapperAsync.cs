using System.Net.Http.Headers;

namespace Aton.Application.IntegrationTests.Framework.Wrappers.AuthWrapper;

public partial class AuthWrapper
{
    private Task FromUserNameAndPasswordAsync(string username, string password)
    {
        Client.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Basic", Convert.ToBase64String(
                System.Text.Encoding.ASCII.GetBytes(
                    $"{username}:{password}")));
        return Task.CompletedTask;
    }

    private Task LoginAsAdminAsync()
    {
        Client.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Basic", Convert.ToBase64String(
                System.Text.Encoding.ASCII.GetBytes(
                    $"Admin:Admin123")));
        return Task.CompletedTask;
    }

    private Task LoginAsDefaultUserAsync()
    {
        Client.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Basic", Convert.ToBase64String(
                System.Text.Encoding.ASCII.GetBytes(
                    $"TestUser:TestUser")));
        return Task.CompletedTask;
    }

    public Task ClearAsync()
    {
        Client.HttpClient.DefaultRequestHeaders.Authorization = null;
        return Task.CompletedTask;
    }
}