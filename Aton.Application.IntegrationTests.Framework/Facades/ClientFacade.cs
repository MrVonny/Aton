using Aton.Application.IntegrationTests.Framework.Wrappers;
using Aton.Application.IntegrationTests.Framework.Wrappers.Controllers;

namespace Aton.Application.IntegrationTests.Framework.Facades;

public class ClientFacade
{
    internal HttpClient HttpClient;
    internal HttpResponseMessage LastResponse = null;
    public ClientFacade(HttpClient client)
    {
        HttpClient = client;
    }

    public TaskWrapper Tasks => new TaskWrapper(this);
    public AuthWrapper Auth => new AuthWrapper(this);
    public AccountsWrapper Accounts => new AccountsWrapper(this);
    public UserControllerWrapper UserController => new(this);
}