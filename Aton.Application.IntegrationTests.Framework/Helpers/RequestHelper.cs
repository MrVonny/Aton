using System.Net.Http.Json;
using System.Net.Mime;
using Aton.Application.IntegrationTests.Framework.Facades;
using Aton.Application.IntegrationTests.Framework.Wrappers;
using Aton.Domain.Core.Commands;

namespace Aton.Application.IntegrationTests.Framework.Helpers;

public class RequestHelper : BaseHelper
{
    private readonly ClientFacade _client;
    public RequestHelper(ClientFacade client)
    {
        _client = client;
    }

    public async Task SendAsync(HttpMethod httpMethod, string uri)
    {
        var httpRequest = new HttpRequestMessage(httpMethod, uri);
        _client.LastResponse = await _client.HttpClient.SendAsync(httpRequest);
    }
    public async Task SendAsync<T>(HttpMethod httpMethod, string uri, T content)
    {
        var myContent = JsonContent.Create(content);
        var httpRequest = new HttpRequestMessage(httpMethod, uri)
        {
            Content = myContent,
        };
        _client.LastResponse = await _client.HttpClient.SendAsync(httpRequest);
    }
}