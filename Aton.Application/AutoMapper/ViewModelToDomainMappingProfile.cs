using Aton.Application.ViewModels;
using Aton.Domain.Commands;
using AutoMapper;

namespace Aton.Application.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<UserViewModel, CreateUserCommand>()
                .ConstructUsing(c => new CreateUserCommand());
            // CreateMap<CustomerViewModel, UpdateCustomerCommand>()
            //     .ConstructUsing(c => new UpdateCustomerCommand(c.Id, c.Name, c.Email, c.BirthDate));
        }
    }
}
