using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using VladBot.BLL.Interfaces;
using VladBot.BLL.Keyboards.UserKeyboard;
using VladBot.Core.Configuration;
using VladBot.Core.Services;
using User = VladBot.Core.Models.User;

namespace VladBot.BLL.TextCommands;

public class StartCommand : ITextCommand
{
    public async Task Execute(ITelegramBotClient client, User? user, Message message,
        IUserService userService,
        Configuration configuration)
    {
        user = new User {Id = message.From!.Id};
        var result = userService.Add(user);
        if (result.Succeeded)
        {
            var links = configuration.Ids
                .Select(async id => await client.CreateChatInviteLinkAsync(new ChatId(id)));
            var result1 = await Task.WhenAll(links);
            await client.SendStickerAsync(message.From.Id,
                new InputOnlineFile("CAACAgIAAxkBAAK_HGAQINBHw7QKWWRV4LsEU4nNBxQ3AAKZAAPZvGoabgceWN53_gIeBA"));
            await client.SendTextMessageAsync(message.Chat.Id,
                "Добро пожаловать.\nПодпишитесь на аккаунты.",
                replyMarkup: CategoryKeyboard.Create(result1.Select(x=>x.InviteLink).ToList()));
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