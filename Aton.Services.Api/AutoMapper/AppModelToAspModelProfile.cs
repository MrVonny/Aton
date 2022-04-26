using Aton.Application.ViewModels;
using Aton.Services.Api.ViewModels;
using AutoMapper;

namespace Aton.Services.Api.AutoMapper;

public class AppModelToAspModelProfile : Profile
{
    public AppModelToAspModelProfile()
    {
        CreateMap<UserViewModel, AspUserViewModel>();
    }
}