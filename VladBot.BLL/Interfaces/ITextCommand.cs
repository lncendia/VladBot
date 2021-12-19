using Telegram.Bot;
using Telegram.Bot.Types;
using VladBot.Core.Configuration;
using VladBot.Core.Services;
using User =  VladBot.Core.Models.User;

namespace VladBot.BLL.Interfaces;

public interface ITextCommand
{
    public Task Execute(ITelegramBotClient client, User? user, Message message, IUserService userService, Configuration configuration);
    public bool Compare(Message message, User? user);
}