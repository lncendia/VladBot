using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using VladBot.BLL.Interfaces;
using VladBot.BLL.Keyboards.UserKeyboard;
using VladBot.Core.Configuration;
using VladBot.Core.Enums;
using VladBot.Core.Services;
using User = VladBot.Core.Models.User;

namespace VladBot.BLL.CallbackQueryCommands;

public class CheckQueryCommand : ICallbackQueryCommand
{
    public async Task Execute(ITelegramBotClient client, User? user, CallbackQuery query, IUserService userService,
        Configuration configuration)
    {
        var tasks = configuration.Ids.Select(link => client.GetChatMemberAsync(new ChatId(link), user!.Id));
        var results = await Task.WhenAll(tasks);
        if (results.Any(result => result.Status == ChatMemberStatus.Left))
        {
            await client.AnswerCallbackQueryAsync(query.Id, "Вы не подисались на все каналы.");
        }

        await client.DeleteMessageAsync(user!.Id, query.Message!.MessageId);
        var link = await client.CreateChatInviteLinkAsync(new ChatId(configuration.ResultId));
        await client.SendTextMessageAsync(user.Id,
            "Успешно.", replyMarkup: CategoryKeyboard.FinalLink(link.InviteLink));
    }

    public bool Compare(CallbackQuery query, User? user)
    {
        return user!.State == State.Main && query.Data!.StartsWith("check");
    }
}