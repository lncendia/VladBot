using Telegram.Bot;
using Telegram.Bot.Types;
using VladBot.Core.Services;
using User =  VladBot.Core.Models.User;

namespace VladBot.BLL.Interfaces;

public interface ICallbackQueryCommand
{
    public Task Execute(ITelegramBotClient client, User? user, CallbackQuery query, IUserService userService, Core.Configuration.Configuration configuration);
    public bool Compare(CallbackQuery query, User? user);
}