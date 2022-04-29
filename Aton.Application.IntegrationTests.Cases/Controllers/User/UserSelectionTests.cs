using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Aton.Application.IntegrationTests.Cases.Utils;
using Aton.Application.IntegrationTests.Framework;
using Aton.Application.ViewModels;
using Aton.Domain.Models;
using Aton.Services.Api.ViewModels;
using NUnit.Framework;

namespace Aton.Application.IntegrationTests.Cases.Controllers.User;

public class UserSelectionTests : TestBase
{
    private List<CreateUserViewModel> UserViewModels { get; set; } = new List<CreateUserViewModel>();
    
    
    [Test]
    public async Task OlderThanValidResponse()
    {
        var users = Enumerable.Range(0, 100).Select(x => UserStorage.GenerateValidUserViewModel).ToList();
        await Client
            .Auth
                .LoginAsAdmin()
            .Utils
                .Repeat(users,
                model =>
                    Client.UserController
                        .CreateUser(model)
                        .Response
                            .AssertStatusCode(HttpStatusCode.Created)
                    )
            .Client.UserController
                .GetOlderThan(40)
                .Response
                    .AssertStatusCode(HttpStatusCode.OK)
                    .Json
                        .AssertThat<List<AspUserViewModel>>(Is.All.Property(nameof(AspUserViewModel.Birthday)).LessThanOrEqualTo(DateTime.Now.AddYears(-40)))
                        .AssertThat<List<AspUserViewModel>>(Has.Count.EqualTo(users.Count(x => x.Birthday <= DateTime.Now.AddYears(-40))))
            .Client.Tasks.RunAsync();
    }
    
    [Test]
    public async Task ActiveValidResponse()
    {
        var users = Enumerable.Range(0, 100).Select(x => UserStorage.GenerateValidUserViewModel).ToList();
        var revokedUsers = users.Where(x => x.Gender == Gender.Female).ToList();
        
        await Client
            .Auth
                .LoginAsAdmin()
            .Utils
                .Repeat(users,
                model =>
                    Client.UserController
                        .CreateUser(model)
                        .Response
                            .AssertStatusCode(HttpStatusCode.Created)
                    )
                .Repeat(revokedUsers,
                model =>
                    Client.UserController
                        .DeleteUser(model.Login, true)
                        .Response
                            .AssertStatusCode(HttpStatusCode.OK)
                    )
            .Client.UserController
                .GetActive()
                .Response
                    .AssertStatusCode(HttpStatusCode.OK)
                    .Json
                        .AssertThat<List<AspUserViewModel>>(Is.All.Property(nameof(AspUserViewModel.Gender)).Not.EqualTo(Gender.Female))
                        .AssertThat<List<AspUserViewModel>>(Has.Count.EqualTo(users.Count - revokedUsers.Count))
            .Client.Tasks.RunAsync();
    }
}