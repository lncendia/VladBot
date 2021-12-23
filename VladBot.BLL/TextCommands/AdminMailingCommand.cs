using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using VladBot.BLL.Interfaces;
using VladBot.Core.Enums;
using VladBot.Core.Services;
using User = VladBot.Core.Models.User;

namespace VladBot.BLL.TextCommands;

public class AdminMailingCommand : ITextCommand
{
    public async Task Execute(ITelegramBotClient client, User? user, Message message,
        IUserService userService,
        Core.Configuration.Configuration configuration)
    {
        if (user!.State == State.Main)
        {
            await client.SendTextMessageAsync(user.Id,
                "Введите сообщение, которое хотите разослать.");
            user.State = State.EnterMessageToMailing;
        }
        else
        {
            await client.SendTextMessageAsync(user.Id, "Вы вышли из панели рассылки.");
            user.State = State.Main;
        }

        userService.Update(user);
    }

    public bool Compare(Message message, User? user)
    {
        return message.Type == MessageType.Text && message.Text == "/mailing" &&
               user!.State is State.Main or State.EnterMessageToMailing  && user.IsAdmin;
    }
}