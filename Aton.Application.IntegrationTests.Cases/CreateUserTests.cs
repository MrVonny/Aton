using System;
using System.Net;
using System.Threading.Tasks;
using Aton.Application.IntegrationTests.Framework;
using Aton.Application.ViewModels;
using Aton.Domain.Models;
using NUnit.Framework;

namespace Aton.Application.IntegrationTests.Cases;

public class CreateUserTests : TestBase
{
    private static CreateUserViewModel ValidUser => new CreateUserViewModel()
    {
        Name = "Козлов Кирилл",
        Login = "Trishu123",
        Password = "12345678",
        Admin = false,
        Gender = Gender.Male,
        Birthday = DateTime.Parse("1999-04-01")
    };
    
    private static CreateUserViewModel InvalidUser => new CreateUserViewModel()
    {
        Name = "Новиков Генадий",
        Login = "Tri",
        Password = "10",
        Admin = false,
        Gender = Gender.Male,
        Birthday = DateTime.Parse("1999-04-01")
    };
    
    [Test]
    public async Task CreateUserTest()
    {
        await Client
            .Auth
                .LoginAsAdmin()
            //Create User
            .UserController
                .CreateUser(ValidUser)
                .Response
                    .AssertStatusCode(HttpStatusCode.OK)
            //Get this user
            .Client.UserController
                .GetUser(ValidUser.Login)
                .Response
                    .AssertProperty("login", ValidUser.Login)
                    .AssertProperty("createdBy", "Admin")
                    .AssertProperty("name", ValidUser.Name)
            //Create duplicate
            .Client.UserController
                .CreateUser(ValidUser)
                .Response
                    .AssertStatusCode(HttpStatusCode.BadRequest)
            .Client.Tasks.RunAsync();
    }
    
    [Test]
    public async Task CreateUserFromInvalidDataTest()
    {
        await Client
            .Auth
                .LoginAsAdmin()
            .UserController
                .CreateUser(InvalidUser)
                .Response
                    .AssertStatusCode(HttpStatusCode.BadRequest)
            .Client.Tasks.RunAsync();
    }
    
    
    [Test]
    public async Task DefaultUserCantCreateUserTest()
    {
        await Client
            .Auth
                .LoginAsDefaultUser()
            //Create User
            .UserController
                .CreateUser(ValidUser)
                .Response
                    .AssertStatusCode(HttpStatusCode.Forbidden)
            .Client.Tasks.RunAsync();
        
    }
}