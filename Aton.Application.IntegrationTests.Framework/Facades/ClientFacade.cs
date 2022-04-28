using Aton.Application.IntegrationTests.Framework.Wrappers;
using Aton.Application.IntegrationTests.Framework.Wrappers.AuthWrapper;
using Aton.Application.IntegrationTests.Framework.Wrappers.UserControllerWrapper;
using Aton.Application.IntegrationTests.Framework.Wrappers.UtilsWrapper;

namespace Aton.Application.IntegrationTests.Framework.Facades;

public class ClientFacade
{
    internal HttpClient HttpClient;
    internal HttpResponseMessage LastResponse = null;
    public readonly TaskWrapper Tasks;
    public ClientFacade(HttpClient client)
    {
        Tasks = new TaskWrapper(this);
        HttpClient = client;
    }
    
    public AuthWrapper Auth => new AuthWrapper(this);
    public UtilsWrapper Utils => new UtilsWrapper(this);
    public AccountsWrapper Accounts => new AccountsWrapper(this);
    public UserControllerWrapper UserController => new(this);
}