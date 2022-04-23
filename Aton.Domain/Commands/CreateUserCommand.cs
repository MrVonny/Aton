using Aton.Domain.Models;
using MediatR;

namespace Aton.Domain.Commands;

public class CreateUserCommand : UserCommand
{
    public string Name { get; protected set; }
    public Gender Gender { get; protected set; }
    public DateTime? Birthday { get; protected set; }
    public CreateUserCommand(string login, string name, Gender gender, DateTime? birthday = null)
    {
        Login = login;
        Name = name;
        Gender = gender;
        Birthday = birthday;
    }
    public override bool IsValid()
    {
        if (Birthday != null && (Birthday > DateTime.Now || Birthday.Value.Year < 1900))
            return false;
        return true; 
    }
}