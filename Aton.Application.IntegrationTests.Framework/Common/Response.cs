using System.Text.Json.Serialization;

namespace Aton.Application.IntegrationTests.Framework.Common;

public class Response<T> where T : class
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }
    [JsonPropertyName("data")]
    public T Data { get; set; }
}