using System.Net;
using System.Text.Json.Nodes;
using Aton.Application.IntegrationTests.Framework.Facades;
using NUnit.Framework;

namespace Aton.Application.IntegrationTests.Framework.Wrappers.ResponseWrapper;

public partial class ResponseWrapper : BaseWrapper
{
    public ResponseWrapper(ClientFacade client) : base(client)
    {
    }

    public ResponseWrapper AssertStatusCode(HttpStatusCode statusCode)
    {
        Client.Tasks.AddTask(async () => await AssertStatusCodeAsync(statusCode));
        return this;
    }

    public ResponseWrapper AssertProperty(string propertyName, string expectedValue)
    {
        Client.Tasks.AddTask(async () => await AssertPropertyAsync(propertyName, expectedValue));
        return this;
    }
}