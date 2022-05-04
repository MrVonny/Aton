using System;
using Aton.Domain.Commands;
using Aton.Domain.Validations;
using NUnit.Framework;

namespace Aton.Application.UnitTests.CommandValidations;

public class RestoreUserValidationTests
{
    [Test]
    public void ValidCommandTest()
    {
        ValidateCommand(true, Guid.NewGuid());
    }

    [Test]
    public void InvalidCommandTest()
    {
        ValidateCommand(false, Guid.Empty);
    }

    private void ValidateCommand(bool isValid, Guid id)
    {
        var command = new RestoreUserCommand(id);
        var validationResult = new RestoreUserCommandValidation(command).Validate();
        if (isValid)
            Assert.True(validationResult.IsValid, validationResult.ToString());
        else
            Assert.False(validationResult.IsValid, validationResult.ToString());
    }
}