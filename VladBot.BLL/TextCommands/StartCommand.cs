using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using VladBot.BLL.Interfaces;
using VladBot.BLL.Keyboards.UserKeyboard;
using VladBot.Core.Services;
using User = VladBot.Core.Models.User;

namespace VladBot.BLL.TextCommands;

public class StartCommand : ITextCommand
{
    public async Task Execute(ITelegramBotClient client, User? user, Message message,
        IUserService userService, IChannelService channelService,
        Core.Configuration.Configuration configuration)
    {
        user = new User {Id = message.From!.Id};
        var result = userService.Add(user);
        if (result.Succeeded)
        {
            await client.SendStickerAsync(message.From.Id,
                new InputOnlineFile("CAACAgIAAxkBAAEDh2ZhwNXpm0Vikt-5J5yPWTbDPeUwvwAC-BIAAkJOWUoAAXOIe2mqiM0jBA"));
            await client.SendTextMessageAsync(message.Chat.Id,
                "Здравствуйте!🙊\nЕсли хотите найти тот самый фильм из ТикТока😱\nПодпишись на каналы внизу ⬇ после нажми 🔍 Проверить\nИ переходи в канал с фильмом😉",
                replyMarkup: CategoryKeyboard.Create(channelService.GetAll().Select(x => x.FollowLink).ToList()));
        }
        else
        {
            await client.SendTextMessageAsync(message.Chat.Id,
                $"Произошла ошибка ({result.ErrorMessage}). Обратитесь в поддержку.");
        }
    }

    public bool Compare(Message message, User? user)
    {
        return user is null;
    }
}