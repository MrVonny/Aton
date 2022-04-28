using Aton.Application.IntegrationTests.Framework.Facades;

namespace Aton.Application.IntegrationTests.Framework.Wrappers.UtilsWrapper;

public class UtilsWrapper : BaseWrapper
{
    public UtilsWrapper(ClientFacade client) : base(client)
    {
    }

    public UtilsWrapper Repeat<T>(IEnumerable<T> enumerable, Action<T> action)
    {
        foreach (var value in enumerable)
            action(value);
        return this;
    }
}