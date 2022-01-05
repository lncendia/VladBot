using VladBot.Core.Models;
using VladBot.Core.Repositories;
using VladBot.DAL.Data;

namespace VladBot.DAL.Repositories;

public class ChannelRepository : IChannelRepository
{
    private readonly ApplicationDbContext _context;

    public ChannelRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Add(Channel entity)
    {
        _context.Add(entity);
        _context.SaveChanges();
    }

    public void Delete(Channel entity)
    {
        _context.Entry(entity).Collection(channel => channel.Followers!).Load();
        _context.Remove(entity);
        _context.SaveChanges();
    }

    public void Update(Channel entity)
    {
        _context.SaveChanges();
    }

    public Channel? Get(long id)
    {
        return _context.Channels.FirstOrDefault(channel => channel.Id == id);
    }

    public int GetSubscribersCount(Channel channel)
    {
        return _context.Channels.Where(channel1 => channel1 == channel).Select(channel1 => channel1.Followers!.Count)
            .First();
    }

    public Channel? Get(string inviteLink)
    {
        return _context.Channels.FirstOrDefault(channel => channel.FollowLink.Contains(inviteLink));
    }

    public List<Channel> GetAll()
    {
        return _context.Channels.ToList();
    }
}