using VladBot.Core.Interfaces;
using VladBot.Core.Models;

namespace VladBot.Core.Repositories;

public interface IChannelRepository : IRepository<Channel>
{
    public int GetSubscribersCount(Channel channel);
    public Channel? Get(string inviteLink);
}