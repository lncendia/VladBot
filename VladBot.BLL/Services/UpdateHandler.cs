using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using VladBot.BLL.CallbackQueryCommands;
using VladBot.BLL.Interfaces;
using VladBot.BLL.TextCommands;
using VladBot.Core.Interfaces;
using VladBot.Core.Services;

namespace VladBot.BLL.Services;

public class UpdateHandler : IUpdateHandler<Update>
{
    private readonly IUserService _userService;
    private readonly Core.Configuration.Configuration _configuration;
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<UpdateHandler> _logger;
    private readonly IChannelService _channelService;

    public UpdateHandler(IUserService userService, Core.Configuration.Configuration configuration,
        ITelegramBotClient botClient, ILogger<UpdateHandler> logger, IChannelService channelService)
    {
        _userService = userService;
        _configuration = configuration;
        _botClient = botClient;
        _logger = logger;
        _channelService = channelService;
    }

    private static readonly List<ITextCommand> TextCommands = new()
    {
        new StartCommand(),
        new AdminMailingCommand(),
        new AdminStatsCommand(),
        new AdminInfoCommand(),
        new AdminDeleteCommand(),
        new EnterMessageToMailingCommand(),
        new SendKeyboardCommand(),
    };

    private static readonly List<ICallbackQueryCommand> CallbackQueryCommands = new()
    {
        new CheckQueryCommand(),
        new StatsQueryCommand(),
        new InfoQueryCommand(),
        new DeleteQueryCommand()
    };

    public async Task HandleAsync(Update update)
    {
        var handler = update.Type switch
        {
            // UpdateType.Unknown:
            // UpdateType.ChannelPost:
            // UpdateType.EditedChannelPost:
            // UpdateType.ShippingQuery:
            // UpdateType.PreCheckoutQuery:
            // UpdateType.Poll:
            UpdateType.ChatMember => BotOnChatMemberReceived(update.ChatMember!),
            UpdateType.Message => BotOnMessageReceived(update.Message!),
            UpdateType.CallbackQuery => BotOnCallbackQueryReceived(update.CallbackQuery!),
            _ => UnknownUpdateHandlerAsync(update)
        };

        try
        {
            await handler;
        }
        catch (Exception exception)
        {
            HandleErrorAsync(update, exception);
        }
    }

    public void HandleErrorAsync(Update update, Exception ex)
    {
        _logger.LogError(ex, "Update id: {Id}", update.Id);
    }

    private Task UnknownUpdateHandlerAsync(Update update)
    {
        return Task.CompletedTask;
    }

    private async Task BotOnCallbackQueryReceived(CallbackQuery updateCallbackQuery)
    {
        var user = _userService.Get(updateCallbackQuery.From.Id);
        var command = CallbackQueryCommands.FirstOrDefault(command => command.Compare(updateCallbackQuery, user));
        if (command != null)
            await command.Execute(_botClient, user, updateCallbackQuery, _userService, _channelService, _configuration);
    }

    private async Task BotOnMessageReceived(Message updateMessage)
    {
        var user = _userService.Get(updateMessage.From!.Id);
        var command = TextCommands.FirstOrDefault(command => command.Compare(updateMessage, user));
        if (command != null)
            await command.Execute(_botClient, user, updateMessage, _userService, _channelService, _configuration);
    }

    private Task BotOnChatMemberReceived(ChatMemberUpdated updateChatMember)
    {
        var user = _userService.Get(updateChatMember.NewChatMember.User.Id);
        if (user is null) return Task.CompletedTask;
        var link = _channelService.Get(updateChatMember.Chat.Id);
        if (link == null) return Task.CompletedTask;
        IOperationResult? result = null;
        if (updateChatMember.InviteLink != null)
        {
            if (!link.FollowLink.Contains(updateChatMember.InviteLink.InviteLink.Replace("...", String.Empty)))
                return Task.CompletedTask;
            result = _userService.SubscribeChannel(user, link);
        }
        else if (updateChatMember.NewChatMember.Status is ChatMemberStatus.Left or ChatMemberStatus.Kicked)
        {
            result = _userService.UnsubscribeChannel(user, link);
        }

        if (result is {Succeeded: false})
        {
            _logger.LogError("Error while handling the ChatMemberUpdate: {Error}", result.ErrorMessage);
        }

        return Task.CompletedTask;
    }
}