using Aton.Application.ViewModels;
using Aton.Domain.Models;
using AutoMapper;

namespace Aton.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<User, UserViewModel>();
        }
    }
}
