using System.Net.Http.Headers;
using Aton.Application.IntegrationTests.Framework.Facades;

namespace Aton.Application.IntegrationTests.Framework.Wrappers;

public class AuthWrapper : BaseWrapper
{
    public AuthWrapper(ClientFacade client) : base(client)
    {
        
    }

    public ClientFacade FromUserNameAndPassword(string username, string password)
    {
        Client.Tasks.AddTask(async () =>
        {
            Client.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic", Convert.ToBase64String(
                    System.Text.Encoding.ASCII.GetBytes(
                        $"{username}:{password}")));
        });
        return Client;
    }
    
    public ClientFacade LoginAsAdmin()
    {
        Client.Tasks.AddTask(async () =>
        {
            Client.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic", Convert.ToBase64String(
                    System.Text.Encoding.ASCII.GetBytes(
                        $"Admin:Admin123")));
        }); 
        return Client;
    }
    
    public ClientFacade LoginAsDefaultUser()
    {
        Client.Tasks.AddTask(async () =>
        {
            Client.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic", Convert.ToBase64String(
                    System.Text.Encoding.ASCII.GetBytes(
                        $"TestUser:TestUser")));
        }); 
        return Client;
    }
}