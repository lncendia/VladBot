using Telegram.Bot;
using Telegram.Bot.Types;
using VladBot.Core.Configuration;
using VladBot.Core.Services;
using User =  VladBot.Core.Models.User;

namespace VladBot.BLL.Interfaces;

public interface ICallbackQueryCommand
{
    public Task Execute(ITelegramBotClient client, User? user, CallbackQuery query, IUserService userService, Configuration configuration);
    public bool Compare(CallbackQuery query, User? user);
}