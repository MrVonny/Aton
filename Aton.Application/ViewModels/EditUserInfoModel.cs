using System.ComponentModel.DataAnnotations;
using Aton.Domain.Models;

namespace Aton.Application.ViewModels;

public class EditUserInfoModel
{
    public EditUserInfoModel(Guid? guid, string name = null, Gender? gender = null, DateTime? birthday = null)
    {
        Guid = guid;
        Name = name;
        Gender = gender;
        Birthday = birthday;
    }
    public Guid? Guid { get; set; }
    [RegularExpression(@"[ЁёА-яa-zA-Z]")]
    public string Name { get; set; }
    public Gender? Gender { get; set; }
    //[Range(typeof(DateTime), "1/1/1900", null)]
    public DateTime? Birthday { get; set; } 
}