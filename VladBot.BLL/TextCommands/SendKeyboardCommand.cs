using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using VladBot.BLL.Interfaces;
using VladBot.Core.Enums;
using VladBot.Core.Services;
using User = VladBot.Core.Models.User;

namespace VladBot.BLL.TextCommands;

public class SendKeyboardCommand : ITextCommand
{
    public async Task Execute(ITelegramBotClient client, User? user, Message message,
        IUserService userService,
        Configuration.Configuration configuration)
    {
        await client.SendTextMessageAsync(user.Id,
            "Подпишитесь на каналы.");
        //replyMarkup: Keyboards.UserKeyboard.CategoryKeyboard.Create(configuration.Usernames));
    }

    public bool Compare(Message message, User? user)
    {
        return message.Type == MessageType.Text && user!.State == State.Main && message.Text!.StartsWith("/start");
    }
}