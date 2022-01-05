using VladBot.Core.Interfaces;
using VladBot.Core.Models;
using VladBot.Core.Repositories;
using VladBot.Core.Services;

namespace VladBot.BLL.Services;

public class ChannelService : IChannelService
{
    private readonly IChannelRepository _channelRepository;

    public ChannelService(IChannelRepository channelRepository)
    {
        _channelRepository = channelRepository;
    }

    public List<Channel> GetAll()
    {
        return _channelRepository.GetAll();
    }

    public IOperationResult Delete(Channel entity)
    {
        try
        {
            _channelRepository.Delete(entity);
            return OperationResult.Ok();
        }
        catch (Exception ex)
        {
            return OperationResult.Fail(ex.Message);
        }
    }

    public Channel? Get(long id)
    {
        return _channelRepository.Get(id);
    }

    public void Update(Channel entity)
    {
        _channelRepository.Update(entity);
    }

    public IOperationResult Add(Channel item)
    {
        try
        {
            _channelRepository.Add(item);
            return OperationResult.Ok();
        }
        catch (Exception ex)
        {
            return OperationResult.Fail(ex.Message);
        }
    }

    public IResult<int> GetSubscribersCount(Channel channel)
    {
        try
        {
            return Result<int>.Ok(_channelRepository.GetSubscribersCount(channel));
        }
        catch (Exception ex)
        {
            return Result<int>.Fail(ex.Message);
        }
    }

    public Channel? Get(string inviteLink)
    {
        return _channelRepository.Get(inviteLink);
    }
}