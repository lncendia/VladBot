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

public class AdminDeleteCommand : ITextCommand
{
    public async Task Execute(ITelegramBotClient client, User? user, Message message,
        IUserService userService,IChannelService channelService, 
        Core.Configuration.Configuration configuration)
    {
        var tasks = channelService.GetAll().Select(channel => client.GetChatAsync(new ChatId(channel.Id)));
        try
        {
            var result = await Task.WhenAll(tasks);
            await client.SendTextMessageAsync(user!.Id,
                "Выберите канал:", replyMarkup: DeleteKeyboard.Delete(result.Select(chat=>(chat.Username, chat.Id)).ToList()));
        }
        catch (Exception ex)
        {
            await client.SendTextMessageAsync(user!.Id,
                $"Не удалось получить названия каналов: <code>{ex.Message}</code>.", ParseMode.Html);
        }
    }

    public bool Compare(Message message, User? user)
    {
        return message.Type == MessageType.Text && message.Text == "/delete" &&
               user!.State is State.Main && user.IsAdmin;
    }
}