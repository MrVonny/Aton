using System.ComponentModel.DataAnnotations;
using Aton.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Aton.Application.ViewModels;

public class EditUserInfoModel
{
    [FromRoute(Name = "login")]
    public string Login { get; set; }
    public Guid? Guid { get; set; }
    [RegularExpression(@"[ЁёА-яa-zA-Z]")]
    public string UserName { get; set; }
    public Gender Gender { get; set; }
    //[Range(typeof(DateTime), "1/1/1900", null)]
    public DateTime? Birthday { get; set; } 
}