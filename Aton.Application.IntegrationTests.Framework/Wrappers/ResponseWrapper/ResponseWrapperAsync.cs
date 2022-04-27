using System.Net;
using System.Text.Json.Nodes;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Aton.Application.IntegrationTests.Framework.Wrappers.ResponseWrapper;

public partial class ResponseWrapper : BaseWrapper
{
    private async Task AssertStatusCodeAsync(HttpStatusCode statusCode)
    {
        Assert.AreEqual(statusCode, Client.LastResponse.StatusCode, $"{await Client.LastResponse.Content.ReadAsStringAsync()}");
    }

    private async Task AssertPropertyAsync(string propertyName, string expectedValue)
    {
        var response = await Client.LastResponse.Content.ReadAsStringAsync();
        var jsonResponse = JsonNode.Parse(response)?["data"];
        Assert.NotNull(jsonResponse?[propertyName], $"Response doesn't have property {propertyName}.\nResponse is: {jsonResponse ?? response}");
        Assert.AreEqual(expectedValue, jsonResponse?[propertyName]?.ToString());
    }
    
    private async Task AssertPropertyAsync(string propertyName, Type propertyType, Constraint constraint)
    {
        var response = await Client.LastResponse.Content.ReadAsStringAsync();
        var jsonResponse = JsonNode.Parse(response)?["data"];
        Assert.NotNull(jsonResponse?[propertyName], $"Response doesn't have property {propertyName}.\nResponse is: {jsonResponse ?? response}");
        var property = jsonResponse?[propertyName];
        if(propertyType == typeof(DateTime)) 
            Assert.That(DateTime.Parse(property.ToString()), constraint);
        else 
            Assert.That(property.ToString(), constraint);
        
    }
}