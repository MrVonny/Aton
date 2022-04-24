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
    
    public async Task<Guid?> Create(CreateUserViewModel createUserViewModel)
    {
        var createCommand = _mapper.Map<CreateUserCommand>(createUserViewModel);
        var user = await _bus.SendCommand(createCommand);
        return user?.Id;
    }

    public IEnumerable<UserViewModel> GetAll()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<UserViewModel> GetActiveOrdered()
    {
        var active = _userRepository.GetActiveOrdered();
        return _mapper.Map<IEnumerable<UserViewModel>>(active);
    }

    public IEnumerable<UserViewModel> GetAll(int skip, int take)
    {
        throw new NotImplementedException();
    }

    public async Task<UserViewModel> Edit(EditUserInfoModel editUserInfoModel)
    {
        var editCommand = _mapper.Map<EditUserCommand>(editUserInfoModel);
        var user = await _bus.SendCommand(editCommand);
        return _mapper.Map<UserViewModel>(user);
    }

    public async Task<UserViewModel> GetById(Guid id)
    {
        var user = await _userRepository.GetById(id);
        return _mapper.Map<UserViewModel>(user);
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