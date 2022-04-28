using Aton.Application.IntegrationTests.Framework.Extensions;
using Aton.Application.IntegrationTests.Framework.Facades;
using Aton.Application.ViewModels;
using Aton.Domain.Models;

namespace Aton.Application.IntegrationTests.Framework.Wrappers.UserControllerWrapper;

public class UserControllerWrapper : BaseWrapper
{
    private static string Uri => "/api/v1/users";
    public UserControllerWrapper(ClientFacade clientFacade) : base(clientFacade)
    {
        
    }

    public ResponseWrapper.ResponseWrapper Response => new(Client);

    public UserControllerWrapper GetActive()
    {
        Client.Tasks.AddTask(async () => await RequestHelper.SendAsync(HttpMethod.Get, Uri + "/active"));
        return this;
    }

    public UserControllerWrapper GetOlderThan(int age)
    {
        Client.Tasks.AddTask(async () => await RequestHelper.SendAsync(HttpMethod.Get, Uri + $"/older-than/{age}"));
        return this;
    }

    public UserControllerWrapper GetUser(string login)
    {
        Client.Tasks.AddTask(async () => await RequestHelper.SendAsync(HttpMethod.Get, Uri + $"/{login}"));
        return this;
    }

    public UserControllerWrapper GetMe()
    {
        Client.Tasks.AddTask(async () => await RequestHelper.SendAsync(HttpMethod.Get, Uri + "/me"));
        return this;
    }
    public UserControllerWrapper DeleteUser(string login, bool soft = true)
    {
        Client.Tasks.AddTask(async () => await RequestHelper.SendAsync(HttpMethod.Delete, Uri + $"/{login}?soft={soft.ToString()}"));
        return this;
    }

    public UserControllerWrapper CreateUser(CreateUserViewModel model)
    {
        Client.Tasks.AddTask(async () => await RequestHelper.SendAsync(HttpMethod.Post, Uri , model));
        return this;
    }
    public UserControllerWrapper EditUserInfo(string login,
         string name = null,
         Gender? gender = null,
         DateTime? birthday = null)
    {
        var query = new { name, gender, birthday }.GetQueryString();
        Client.Tasks.AddTask(async () => await RequestHelper.SendAsync(HttpMethod.Post, Uri + $"/{login}/info?{query}"));
        return this;
    }
    public UserControllerWrapper EditUserLogin(string login, string newLogin)
    {
        Client.Tasks.AddTask(async () => await RequestHelper.SendAsync(HttpMethod.Post, Uri + $"/{login}/login?newLogin={newLogin}"));
        return this;
    }

    public UserControllerWrapper EditUserPassword(string login, string password)
    {
        Client.Tasks.AddTask(async () => await RequestHelper.SendAsync(HttpMethod.Post, Uri + $"/{login}/password?newPassword={password}"));
        return this;
    }
    public UserControllerWrapper Restore() => throw new NotImplementedException();
}