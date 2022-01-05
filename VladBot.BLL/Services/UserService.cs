using VladBot.Core.Interfaces;
using VladBot.Core.Models;
using VladBot.Core.Repositories;
using VladBot.Core.Services;

namespace VladBot.BLL.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public List<User> GetAll()
    {
        return _userRepository.GetAll();
    }

    public IOperationResult Delete(User entity)
    {
        try
        {
            _userRepository.Delete(entity);
            return OperationResult.Ok();
        }
        catch (Exception ex)
        {
            return OperationResult.Fail(ex.Message);
        }
    }

    public User? Get(long id)
    {
        return _userRepository.Get(id);
    }

    public void Update(User entity)
    {
        _userRepository.Update(entity);
    }

    public IOperationResult Add(User item)
    {
        try
        {
            _userRepository.Add(item);
            return OperationResult.Ok();
        }
        catch (Exception ex)
        {
            return OperationResult.Fail(ex.Message);
        }
    }

    public IResult<int> GetCount(DateTime lowerTime, DateTime upperTime, TimeZoneInfo cZoneInfo)
    {
        try
        {
            var lower = TimeZoneInfo.ConvertTimeToUtc(lowerTime, cZoneInfo);
            var upper = TimeZoneInfo.ConvertTimeToUtc(upperTime, cZoneInfo);
            return Result<int>.Ok(_userRepository.GetCount(lower, upper));
        }
        catch (Exception ex)
        {
            return Result<int>.Fail(ex.Message);
        }
    }

    public IResult<int> GetAllCount()
    {
        try
        {
            return Result<int>.Ok(_userRepository.GetAllCount());
        }
        catch (Exception ex)
        {
            return Result<int>.Fail(ex.Message);
        }
    }

    public IOperationResult SubscribeChannel(User user, Channel channel)
    {
        try
        {
            _userRepository.SubscribeChannel(user, channel);
            return OperationResult.Ok();
        }
        catch (Exception ex)
        {
            return OperationResult.Fail(ex.Message);
        }
    }

    public IOperationResult UnsubscribeChannel(User user, Channel channel)
    {
        try
        {
            _userRepository.UnsubscribeChannel(user, channel);
            return OperationResult.Ok();
        }
        catch (Exception ex)
        {
            return OperationResult.Fail(ex.Message);
        }
    }
}