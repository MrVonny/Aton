using System;
using System.Linq;
using Aton.Application.ViewModels;
using Aton.Domain.Models;
using Aton.Services.Api.ViewModels;

namespace Aton.Application.IntegrationTests.Cases.Utils;

public static class UserStorage
{
    public static AspCreateUserViewModel ValidUserViewModel => new AspCreateUserViewModel()
    {
        Name = "Козлов Кирилл",
        Login = "Trishu123",
        Password = "12345678",
        Admin = false,
        Gender = Gender.Male,
        Birthday = DateTime.Parse("1999-04-01")
    };
    
    public static AspCreateUserViewModel InvalidUserViewModel => new AspCreateUserViewModel()
    {
        Name = "Новиков Генадий",
        Login = "Tri",
        Password = "10",
        Admin = false,
        Gender = Gender.Male,
        Birthday = DateTime.Parse("1999-04-01")
    };

    public static AspCreateUserViewModel GenerateValidUserViewModel => new()
    {
        Name = RandomName,
        Login = RandomLogin,
        Password = RandomPassword,
        Admin = false,
        Gender = RandomGender,
        Birthday = RandomDate(RandomDate(DateTime.Parse("1930-01-01")))
    };

    private static Random random = new Random();
    private static string RandomString(int length, bool numeric = false)
    {
        string chars = numeric ? "ABCDEFGHIJKLMNOPQRSTUVWXYZ123456789" : "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    private static string RandomName => $"{RandomString(9)} {RandomString(5)}";
    private static string RandomLogin => $"{RandomString(9, true)}";
    private static string RandomPassword => $"{RandomString(12, true)}";
    private static Gender RandomGender => (Gender)Enum.GetValues(typeof(Gender)).GetValue(random.Next(Enum.GetValues(typeof(Gender)).Length))!;
    
    private static DateTime RandomDate(DateTime start)
    {
        int range = (DateTime.Today - start).Days;           
        return start.AddDays(random.Next(range));
    }
}