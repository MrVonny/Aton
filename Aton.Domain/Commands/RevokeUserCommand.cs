﻿using Aton.Domain.Validations;

namespace Aton.Domain.Commands;

public class RevokeUserCommand : UserCommand
{
    public RevokeUserCommand(Guid guid, string revokedBy)
    {
        Id = guid;
        RevokedBy = revokedBy;
    }
    
    public string RevokedBy { get; set; }
    public override bool IsValid()
    {
        ValidationResult = new RevokeUserCommandValidation(this).Validate(this);
        return ValidationResult.IsValid;
    }
}