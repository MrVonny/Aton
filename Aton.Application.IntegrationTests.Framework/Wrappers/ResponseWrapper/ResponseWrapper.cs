using System.Net;
using System.Text.Json.Nodes;
using Aton.Application.IntegrationTests.Framework.Facades;
using NUnit.Framework;
using NUnit.Framework.Constraints;

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
    
    public ResponseWrapper AssertProperty(string propertyName, Type propertyType , Constraint constraint)
    {
        Client.Tasks.AddTask(async () => await AssertPropertyAsync(propertyName, propertyType, constraint));
        return this;
    }
}