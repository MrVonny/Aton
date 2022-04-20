using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace AtonTest;

public abstract class TestBase
{
    protected AtonDbContext AtonDbContext;
    
    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AtonDbContext>()
            .UseInMemoryDatabase(databaseName: "Test")
            .Options;
        IList<User> entities = new List<User> { 
            new()
            {
                Admin = false,
                Birthday = DateTime.Parse("1992-04-18"),
                Gender = Gender.Female,
                UserName = "Козлова Марина Викторовна",
                Login = "Koz123",
                CreatedOn = DateTime.Parse("2021-11-04 22:13:46"),
                
            }, 
            new()
            {
                Admin = false,
                Birthday = DateTime.Parse("1994-10-04"),
                Gender = Gender.Male,
                UserName = "Брайко Иван Сергеевич",
                Login = "Bis010",
                CreatedOn = DateTime.Parse("2021-04-01 02:53:12"),
                
            }, 
            new()
            {
                Admin = true,
                Birthday = DateTime.Parse("1970-12-10"),
                Gender = Gender.Unknown,
                UserName = "Очень важный человек",
                Login = "Admin10",
                CreatedOn = DateTime.Parse("2002-01-02 12:00:02"),
                
            }, 
        };
        AtonDbContext = new AtonDbContext(options);
        AtonDbContext.Users.AddRange(entities);
        AtonDbContext.SaveChanges();
    }

    [TearDown]
    public void TearDown()
    {
        AtonDbContext.Dispose();
    }
    
    
}