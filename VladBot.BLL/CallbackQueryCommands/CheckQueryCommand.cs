using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using VladBot.BLL.Interfaces;
using VladBot.BLL.Keyboards.UserKeyboard;
using VladBot.Core.Enums;
using VladBot.Core.Services;
using User = VladBot.Core.Models.User;

namespace VladBot.BLL.CallbackQueryCommands;

public class CheckQueryCommand : ICallbackQueryCommand
{
    public async Task Execute(ITelegramBotClient client, User? user, CallbackQuery query, IUserService userService,
        Core.Configuration.Configuration configuration)
    {
        var tasks = configuration.Channels.Select(link => client.GetChatMemberAsync(new ChatId(link.Id), user!.Id));
        var results = await Task.WhenAll(tasks);
        if (results.Any(result => result.Status == ChatMemberStatus.Left))
        {
            await client.AnswerCallbackQueryAsync(query.Id, "Вы не подисались на все каналы.", true);
            return;
        }

        await client.EditMessageTextAsync(user!.Id, query.Message!.MessageId, "✅ ДОСТУП ОТКРЫТ\n\nВсе фильмы загрузили на наш основной канал 👇",
            replyMarkup: CategoryKeyboard.FinalLink(configuration.FinalChanel.FollowLink));
    }

    public bool Compare(CallbackQuery query, User? user)
    {
        return user!.State == State.Main && query.Data!.StartsWith("check");
    }
}