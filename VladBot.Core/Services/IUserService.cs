using VladBot.Core.Interfaces;
using VladBot.Core.Models;

namespace VladBot.Core.Services;

public interface IUserService : IService<User>
{
    public IResult<int> GetCount(DateTime lowerUtcTime, DateTime upperUtcTime, TimeZoneInfo currentZoneInfo);
    public IResult<int> GetAllCount();
    public IOperationResult SubscribeChannel(User user, Channel channel);
    public IOperationResult UnsubscribeChannel(User user, Channel channel);
}