using System.Net;
using Aton.Application.IntegrationTests.Framework.Facades;

namespace Aton.Application.IntegrationTests.Framework.Wrappers;

public class ResponseWrapper : BaseWrapper
{
    public ResponseWrapper(ClientFacade client) : base(client)
    {
    }

    public ResponseWrapper AssertStatusCode(HttpStatusCode statusCode)
    {
        Client.Tasks.AddTask(async () => NUnit.Framework.Assert.AreEqual(statusCode, Client.LastResponse.StatusCode));
        return this;
    }
}