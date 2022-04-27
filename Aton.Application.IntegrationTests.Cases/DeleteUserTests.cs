﻿using System;
using System.Net;
using System.Threading.Tasks;
using Aton.Application.IntegrationTests.Cases.Utils;
using Aton.Application.IntegrationTests.Framework;
using NUnit.Framework;

namespace Aton.Application.IntegrationTests.Cases;

public class DeleteUserTests : TestBase
{
    [Test]
    public async Task SoftDeleteUserOk()
    {
        var user = UserStorage.GenerateValidUserViewModel;
        await Client
            .Auth
                .LoginAsAdmin()
            .UserController
                .CreateUser(user)
                .Response
                    .AssertStatusCode(HttpStatusCode.OK)
            .Client.UserController
                .DeleteUser(user.Login, true)
                .Response
                    .AssertStatusCode(HttpStatusCode.OK)
            .Client.UserController
                .GetUser(user.Login)
                .Response
                    .AssertStatusCode(HttpStatusCode.OK)
                    .AssertProperty("revokedBy", "Admin")
                    .AssertProperty("revokedAt", typeof(DateTime),
                Is.Not.Null.And.EqualTo(DateTime.Now).Within(TimeSpan.FromSeconds(15.0)))
            .Client.Tasks.RunAsync();
    }
    
    [Test]
    public async Task HardDeleteUserOk()
    {
        var user = UserStorage.GenerateValidUserViewModel;
        await Client
            .Auth
                .LoginAsAdmin()
            .UserController
                .CreateUser(user)
                .Response
                    .AssertStatusCode(HttpStatusCode.OK)
            .Client.UserController
                .DeleteUser(user.Login, false)
                .Response
                    .AssertStatusCode(HttpStatusCode.OK)
            .Client.UserController
                .GetUser(user.Login)
                .Response
                    .AssertStatusCode(HttpStatusCode.BadRequest)
            .Client.Tasks.RunAsync();
    }
}