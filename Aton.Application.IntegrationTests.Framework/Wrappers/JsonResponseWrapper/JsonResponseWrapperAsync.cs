
using Aton.Application.IntegrationTests.Framework.Common;
using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Aton.Application.IntegrationTests.Framework.Wrappers.JsonResponseWrapper;

public partial class JsonResponseWrapper
{
    private async Task AssertThatAsync<T>(Constraint constraint) where T : class
    {
        var response = await Client.LastResponse.Content.ReadAsStringAsync();
        var jResponse = JsonConvert.DeserializeObject<Response<T>>(response, new JsonSerializerSettings()
        {

        });
        Assert.That(jResponse?.Data, constraint);
    }
}