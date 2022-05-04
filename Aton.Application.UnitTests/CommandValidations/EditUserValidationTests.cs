using System;
using Aton.Domain.Commands;
using Aton.Domain.Models;
using Aton.Domain.Validations;
using NUnit.Framework;

namespace Aton.Application.UnitTests.CommandValidations;

public class EditUserValidationTests
{
    [Test]
    public void ValidCommandTest()
    {
        ValidateCommand(true,Guid.NewGuid(),null, null, null);
        ValidateCommand(true, Guid.NewGuid(),"Иван Брайко", Gender.Male, DateTime.Parse("1990-05-12"));
        ValidateCommand(true,Guid.NewGuid(),"Большой Умный Парень Имя", Gender.Male, DateTime.Parse("1990-05-12"));
        ValidateCommand(true,Guid.NewGuid(),"Repon Cristof", Gender.Unknown, DateTime.Parse("1935-12-02"));
        ValidateCommand(true,Guid.NewGuid(),"Роман Ан Дер Гольд", Gender.Female);
        ValidateCommand(true,Guid.NewGuid(),"Mathias d'Arras", Gender.Female);
        ValidateCommand(true,Guid.NewGuid(),"Martin Luther King, Jr.", Gender.Female);
        ValidateCommand(true,Guid.NewGuid(),"Hector Sausage-Hausen", Gender.Female);
    }

    [Test]
    public void InvalidCommandTest()
    {
        ValidateCommand(false, Guid.Empty,"Иван Брайко", Gender.Male, DateTime.Parse("1990-05-12"));
        ValidateCommand(false,Guid.NewGuid(),"Иван Брайко", Gender.Male, DateTime.Parse("1893-05-12"));
        ValidateCommand(false,Guid.NewGuid(),"Repon Cristof", Gender.Unknown, DateTime.Parse("2054-12-02"));
        ValidateCommand(false,Guid.NewGuid(),"Принеси 12 литров", Gender.Female, DateTime.Parse("2012-05-12"));
        ValidateCommand(false,Guid.NewGuid(),"ASWdjhja sk kjsadokjIA SDjioajI JDSiajidajio DIOAsjiopdjkiosajdoiuj iaosjdoijaqoi", Gender.Female);
        ValidateCommand(false,Guid.NewGuid(),"Not", Gender.Female);
        ValidateCommand(false,Guid.NewGuid(),"!@#$%^&*()((*&^%%", Gender.Female);
    }

    private void ValidateCommand(bool isValid, Guid id, string name = null, Gender? gender = null, DateTime? birthday = null)
    {
        var command = new EditUserCommand(id, name, gender, birthday);
        var validationResult = new EditUserCommandValidation(command).Validate();
        if (isValid)
            Assert.True(validationResult.IsValid, validationResult.ToString());
        else
            Assert.False(validationResult.IsValid, validationResult.ToString());
    }
}