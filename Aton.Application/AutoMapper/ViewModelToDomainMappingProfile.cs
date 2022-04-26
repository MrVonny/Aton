using Aton.Application.ViewModels;
using Aton.Domain.Commands;
using AutoMapper;

namespace Aton.Application.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<CreateUserViewModel, CreateUserCommand>()
                .ConstructUsing(c => new CreateUserCommand(c.Name, c.Gender.Value ,c.Birthday));
            CreateMap<EditUserInfoModel, EditUserCommand>()
                .ConstructUsing(c => new EditUserCommand(c.Guid.Value, c.Name, c.Gender ,c.Birthday));
        }
    }
}
