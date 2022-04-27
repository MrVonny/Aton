using Aton.Application.IntegrationTests.Framework.Facades;
using Aton.Domain.Commands;

namespace Aton.Application.IntegrationTests.Framework.Wrappers;

public class TaskWrapper : BaseWrapper
{
    public TaskWrapper(ClientFacade client) : base(client)
    {
    }

    private readonly List<Func<Task>> _tasks = new List<Func<Task>>();

    internal void AddTask(Func<Task> task)
    {
        _tasks.Add(task);
    }
    
    public async Task RunAsync()
    {
        foreach (var task in _tasks)
        {
            await task();
        }
    }
}