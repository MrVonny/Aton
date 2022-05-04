using System;
using Aton.Domain.Commands;
using Aton.Domain.Validations;
using NUnit.Framework;

namespace Aton.Application.UnitTests.CommandValidations;

public class RevokeUserValidationTests
{
    [Test]
    public void ValidCommandTest()
    {
        ValidateCommand(true, Guid.NewGuid(), "Admin");
    }

    [Test]
    public void InvalidCommandTest()
    {
        ValidateCommand(false, Guid.Empty, "Admin");
    }

    private void ValidateCommand(bool isValid, Guid id, string revokedBy)
    {
        var command = new RevokeUserCommand(id, revokedBy);
        var validationResult = new RevokeUserCommandValidation(command).Validate();
        if (isValid)
            Assert.True(validationResult.IsValid, validationResult.ToString());
        else
            Assert.False(validationResult.IsValid, validationResult.ToString());
    }
}