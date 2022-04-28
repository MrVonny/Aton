using System.Diagnostics;
using System.Text.Json;
using Aton.Application.IntegrationTests.Framework.Common;
using Aton.Application.IntegrationTests.Framework.Facades;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Aton.Application.IntegrationTests.Framework.Wrappers.JsonResponseWrapper;

public partial class JsonResponseWrapper : BaseWrapper
{
    public JsonResponseWrapper(ClientFacade client) : base(client)
    {
    }


    public JsonResponseWrapper AssertThat<T>(Constraint constraint) where T : class
    {
        Client.Tasks.AddTask(async () => await  AssertThatAsync<T>(constraint));
        return this;
    }
}