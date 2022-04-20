using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AtonTest;

[TestFixture]
public class UserExtensionsTest : TestBase
{
    [Test]
    public async Task ToIndexModelTest()
    {
        var user = await AtonDbContext.Users.FirstAsync();
        var indexUser = user.ToIndexModel();
        Assert.AreEqual(user.Admin, indexUser.Admin);
        Assert.AreEqual(user.Id, indexUser.Id);
        Assert.AreEqual(user.Login, indexUser.Login);
        Assert.AreEqual(user.CreatedOn, indexUser.CreatedOn);
        Assert.AreEqual(user.UserName, indexUser.UserName);
    }

    [Test]
    public async Task ToIndexModelQueryTest()
    {
        var users = await AtonDbContext.Users
            .ToIndexModel()
            .ToListAsync();

        Assert.AreEqual(await AtonDbContext.Users.CountAsync(), users.Count);
        Assert.That(users, Is.All.Property("Id").Not.Null);
        Assert.That(users, Is.All.Property("Login").Not.Null);
    }

    [Test]
    [TestCase(10)]
    [TestCase(20)]
    [TestCase(50)]
    public async Task OlderThanTest(int olderThan)
    {
        var users = await AtonDbContext.Users
            .OlderThan(olderThan)
            .ToListAsync();
        foreach (var user in users)
        {
            var years = DateTime.Now.Year - user.Birthday!.Value.Year;
            var birthdayThisYear = user.Birthday.Value.AddYears(years);
            var age = birthdayThisYear > DateTime.Now ? years - 1 : years;
            Assert.Greater(age, olderThan);
        }
    }
}