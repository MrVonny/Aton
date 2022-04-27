using Aton.Application.IntegrationTests.Framework.Facades;

namespace Aton.Application.IntegrationTests.Framework.Wrappers.AuthWrapper;

public partial class AuthWrapper : BaseWrapper
{
    public AuthWrapper(ClientFacade client) : base(client)
    {
        
    }

    public ClientFacade FromUserNameAndPassword(string username, string password)
    {
        Client.Tasks.AddTask(async () => await FromUserNameAndPasswordAsync(username, password));
        return Client;
    }

    public ClientFacade LoginAsAdmin()
    {
        Client.Tasks.AddTask(async () => await LoginAsAdminAsync()); 
        return Client;
    }

    public ClientFacade LoginAsDefaultUser()
    {
        Client.Tasks.AddTask(async () => await LoginAsDefaultUserAsync()); 
        return Client;
    }

    public ClientFacade Clear()
    {
        Client.Tasks.AddTask(async () => await ClearAsync()); 
        return Client;
    }
}