using VladBot.Core.Interfaces;
using VladBot.Core.Models;

namespace VladBot.Core.Services;

public interface IChannelService : IService<Channel>
{
    public IResult<int> GetSubscribersCount(Channel channel);
    public Channel? Get(string inviteLink);
}