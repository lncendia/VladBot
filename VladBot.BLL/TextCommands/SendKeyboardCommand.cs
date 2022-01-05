using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using VladBot.BLL.Interfaces;
using VladBot.BLL.Keyboards.UserKeyboard;
using VladBot.Core.Enums;
using VladBot.Core.Services;
using User = VladBot.Core.Models.User;

namespace VladBot.BLL.TextCommands;

public class SendKeyboardCommand : ITextCommand
{
    public async Task Execute(ITelegramBotClient client, User? user, Message message,
        IUserService userService, IChannelService channelService,
        Core.Configuration.Configuration configuration)
    {
        await client.SendTextMessageAsync(message.Chat.Id,
            "⛔ЧТОБЫ ПОСМОТРЕТЬ ФИЛЬМЫ ИЗ ТИКТОКА\nНУЖНО ПОДПИСАТЬСЯ НА КАНАЛЫ НИЖЕ⬇\n\nподпишись на каналы и нажми 🔍 ПРОВЕРИТЬ!",
            replyMarkup: CategoryKeyboard.Create(channelService.GetAll().Select(x => x.FollowLink).ToList()));
    }

    public bool Compare(Message message, User? user)
    {
        return message.Type == MessageType.Text && user!.State == State.Main;
    }
}