﻿using Aton.Domain.Models;

namespace Aton.Application.ViewModels;

public class UserViewModel
{
    public string Name { get; set; }
    public Gender Gender { get; set; }
    public DateTime? Birthday { get; set; }
}