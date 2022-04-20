using Aton.Application.Interfaces;
using Aton.Application.ViewModels;
using Aton.Domain.Commands;
using Aton.Domain.Core.Bus;
using Aton.Domain.Intefaces;
using AutoMapper;

namespace Aton.Application.Services;

public class UserAppService : IUserAppService
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IMediatorHandler _bus;

    public UserAppService(IMapper mapper,
        IUserRepository userRepository,
        IMediatorHandler bus)
    {
        _mapper = mapper;
        _userRepository = userRepository;
        _bus = bus;
    }
    
    public void Create(CreateUserViewModel createUserViewModel)
    {
        var registerCommand = _mapper.Map<CreateUserCommand>(createUserViewModel);
        _bus.SendCommand(registerCommand);
    }

    public IEnumerable<UserViewModel> GetAll()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<UserViewModel> GetAll(int skip, int take)
    {
        throw new NotImplementedException();
    }

    public UserViewModel GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public void Update(UserViewModel userViewModel)
    {
        throw new NotImplementedException();
    }

    public void Remove(Guid id)
    {
        throw new NotImplementedException();
    }

    public IList<UserViewModel> GetAllHistory(Guid id)
    {
        throw new NotImplementedException();
    }
    
    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}