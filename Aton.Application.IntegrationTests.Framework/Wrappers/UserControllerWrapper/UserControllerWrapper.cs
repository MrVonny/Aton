using System.Text;
using System.Web;
using Aton.Application.IntegrationTests.Framework.Extensions;
using Aton.Application.IntegrationTests.Framework.Facades;
using Aton.Application.ViewModels;
using Aton.Domain.Models;
using Aton.Services.Api.ViewModels;

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

    public UserControllerWrapper CreateUser(AspCreateUserViewModel model)
    {
        Client.Tasks.AddTask(async () => await RequestHelper.SendAsync(HttpMethod.Post, Uri , model));
        return this;
    }
    public UserControllerWrapper EditUserInfo(string login,
         string name = null,
         Gender? gender = null,
         DateTime? birthday = null)
    {
        List<string> queries = new List<string>();
        if (name != null)
            queries.Add($"name={HttpUtility.UrlEncode(name)}");
        if (gender != null)
            queries.Add($"gender={HttpUtility.UrlEncode(gender.Value.ToString())}");
        if (birthday != null)
            queries.Add($"birthday={HttpUtility.UrlEncode(birthday.Value.ToString("s"))}");
        var query = string.Join("&", queries);
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
    public UserControllerWrapper Restore(string login) 
    {
        Client.Tasks.AddTask(async () => await RequestHelper.SendAsync(HttpMethod.Post, Uri + $"/{login}/restore"));
        return this;
    }
}