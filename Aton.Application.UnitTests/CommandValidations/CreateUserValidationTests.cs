using System;
using Aton.Domain.Commands;
using Aton.Domain.Models;
using Aton.Domain.Validations;
using FluentValidation.Results;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Aton.Application.UnitTests.CommandValidations;

[TestFixture]
public class CreateUserValidationTests
{
    [Test]
    public void ValidCommandTest()
    {
        ValidateCommand(true,"Иван Брайко", Gender.Male, DateTime.Parse("1990-05-12"));
        ValidateCommand(true,"Большой Умный Парень Имя", Gender.Male, DateTime.Parse("1990-05-12"));
        ValidateCommand(true,"Repon Cristof", Gender.Unknown, DateTime.Parse("1935-12-02"));
        ValidateCommand(true,"Mathias d'Arras", Gender.Female);
        ValidateCommand(true,"Martin Luther King, Jr.", Gender.Female);
        ValidateCommand(true,"Hector Sausage-Hausen", Gender.Female);
        ValidateCommand(true,"Роман Ан Дер Гольд", Gender.Female);
        ValidateCommand(true,"Роман Ан Дер Гольд", Gender.Female);
    }

    [Test]
    public void InvalidCommandTest()
    {
        ValidateCommand(false,"Иван Брайко", Gender.Male, DateTime.Parse("1893-05-12"));
        ValidateCommand(false,"Repon Cristof", Gender.Unknown, DateTime.Parse("2054-12-02"));
        ValidateCommand(false,"Принеси 12 литров", Gender.Female, DateTime.Parse("2012-05-12"));
        ValidateCommand(false,"ASWdjhja sk kjsadokjIA SDjioajI JDSiajidajio DIOAsjiopdjkiosajdoiuj iaosjdoijaqoi", Gender.Female);
        ValidateCommand(false,"Not", Gender.Female);
        ValidateCommand(false,"!@#$%^&*()((*&^%%", Gender.Female);
        ValidateCommand(false,"Валерий_Никитин", Gender.Female);
    }

    private void ValidateCommand(bool constraint, string name, Gender gender = Gender.Unknown, DateTime? birthday = null)
    {
        var command = new CreateUserCommand(name, gender, birthday);
        var validationResult = new CreateUserCommandValidation(command).Validate();
        if (constraint)
            Assert.True(validationResult.IsValid, validationResult.ToString());
        else
            Assert.False(validationResult.IsValid, validationResult.ToString());
    }
}