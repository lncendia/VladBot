using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using VladBot.BLL.Interfaces;
using VladBot.BLL.Keyboards.AdminKeyboard;
using VladBot.Core.Enums;
using VladBot.Core.Services;
using User = VladBot.Core.Models.User;

namespace VladBot.BLL.TextCommands;

public class AdminStatsCommand : ITextCommand
{
    public async Task Execute(ITelegramBotClient client, User? user, Message message,
        IUserService userService,
        Core.Configuration.Configuration configuration)
    {
        await client.SendTextMessageAsync(user!.Id,
            "Выберите период времени:", replyMarkup: StatsKeyboard.Stats);
    }

    public bool Compare(Message message, User? user)
    {
        return message.Type == MessageType.Text && message.Text == "/stats" &&
               user!.State is State.Main && user.IsAdmin;
    }
}