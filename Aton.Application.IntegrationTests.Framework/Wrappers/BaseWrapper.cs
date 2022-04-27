using Aton.Application.IntegrationTests.Framework.Facades;
using Aton.Application.IntegrationTests.Framework.Helpers;

namespace Aton.Application.IntegrationTests.Framework.Wrappers;

public class BaseWrapper
{
    public ClientFacade Client;

    public BaseWrapper(ClientFacade client)
    {
        Client = client;
    }

    protected RequestHelper RequestHelper => new(Client);
}