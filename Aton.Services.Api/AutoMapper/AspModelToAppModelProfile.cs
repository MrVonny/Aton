using Aton.Application.ViewModels;
using Aton.Services.Api.ViewModels;
using AutoMapper;

namespace Aton.Services.Api.AutoMapper;

public class AspModelToAppModelProfile : Profile
{
    public AspModelToAppModelProfile()
    {
        CreateMap<AspCreateUserViewModel, CreateUserViewModel>();
    }
}