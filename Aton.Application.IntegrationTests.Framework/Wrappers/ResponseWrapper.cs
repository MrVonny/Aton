using System.Net;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Aton.Application.IntegrationTests.Framework.Facades;
using NUnit.Framework;

namespace Aton.Application.IntegrationTests.Framework.Wrappers;

public class ResponseWrapper : BaseWrapper
{
    public ResponseWrapper(ClientFacade client) : base(client)
    {
    }

    public ResponseWrapper AssertStatusCode(HttpStatusCode statusCode)
    {
        Client.Tasks.AddTask(async () => await AssertStatusCodeAsync(statusCode));
        return this;
    }

    private async Task AssertStatusCodeAsync(HttpStatusCode statusCode)
    {
        Assert.AreEqual(statusCode, Client.LastResponse.StatusCode, $"{await Client.LastResponse.Content.ReadAsStringAsync()}");
    }
    
    public ResponseWrapper AssertProperty(string propertyName, string expectedValue)
    {
        Client.Tasks.AddTask(async () =>
        {
            var response = await Client.LastResponse.Content.ReadAsStringAsync();
            var jsonResponse = JsonNode.Parse(response)?["data"];
            Assert.NotNull(jsonResponse?[propertyName], $"Response doesn't have property {propertyName}.\nResponse is: {jsonResponse ?? response}");
            Assert.AreEqual(expectedValue, jsonResponse?[propertyName]?.ToString());
        });
        return this;
    }
}