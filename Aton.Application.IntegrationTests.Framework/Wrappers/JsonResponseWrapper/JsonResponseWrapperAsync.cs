using System.Text.Json;
using Aton.Application.IntegrationTests.Framework.Common;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Aton.Application.IntegrationTests.Framework.Wrappers.JsonResponseWrapper;

public partial class JsonResponseWrapper
{
    private async Task AssertThatAsync<T>(Constraint constraint) where T : class
    {
        var response = await Client.LastResponse.Content.ReadAsStringAsync();
        var jResponse = JsonSerializer.Deserialize<Response<T>>(response, new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        Assert.That(jResponse?.Data, constraint);
    }
}