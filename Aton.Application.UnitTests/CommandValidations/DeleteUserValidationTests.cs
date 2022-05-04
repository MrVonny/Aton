using System;
using Aton.Domain.Commands;
using Aton.Domain.Validations;
using NUnit.Framework;

namespace Aton.Application.UnitTests.CommandValidations;

public class DeleteUserValidationTests
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
        var command = new DeleteUserCommand(id);
        var validationResult = new DeleteUserCommandValidation(command).Validate();
        if (isValid)
            Assert.True(validationResult.IsValid, validationResult.ToString());
        else
            Assert.False(validationResult.IsValid, validationResult.ToString());
    }
}