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
    
    public async Task<Guid?> Create(CreateUserViewModel createUserViewModel, string createdBy)
    {
        var createCommand = _mapper.Map<CreateUserCommand>(createUserViewModel);
        createCommand.CreatedBy = createdBy;
        var user = await _bus.SendCommand(createCommand);
        return user?.Id;
    }
    
    public async Task<IEnumerable<UserViewModel>> GetActiveOrdered()
    {
        var active = await _userRepository.GetActiveOrdered();
        return _mapper.Map<IEnumerable<UserViewModel>>(active);
    }

    public async Task<UserViewModel> Edit(EditUserInfoModel editUserInfoModel, string updatedBy)
    {
        var editCommand = _mapper.Map<EditUserCommand>(editUserInfoModel);
        editCommand.UpdatedBy = updatedBy;
        var user = await _bus.SendCommand(editCommand);
        return _mapper.Map<UserViewModel>(user);
    }

    public async Task<UserViewModel> GetById(Guid id)
    {
        var user = await _userRepository.GetById(id);
        return _mapper.Map<UserViewModel>(user);
    }

    public async Task Remove(Guid id)
    {
        var command = new DeleteUserCommand(id);
        await _bus.SendCommand(command);
    }
    
    public async Task Revoke(Guid guid, string revokedBy)
    {
        var command = new RevokeUserCommand(guid, revokedBy);
        await _bus.SendCommand(command);
    }

    public async Task<IEnumerable<UserViewModel>> GetOlderThan(int olderThan)
    {
        var users = await _userRepository.GetOlderThan(olderThan);
        return _mapper.Map<IEnumerable<UserViewModel>>(users);
    }

    public async Task Restore(Guid guid)
    {
        var command = new RestoreUserCommand(guid);
        await _bus.SendCommand(command);
    }
    
    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}