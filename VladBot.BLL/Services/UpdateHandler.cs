using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using VladBot.BLL.CallbackQueryCommands;
using VladBot.BLL.Interfaces;
using VladBot.BLL.TextCommands;
using VladBot.Core.Configuration;
using VladBot.Core.Services;

namespace VladBot.BLL.Services;

public class UpdateHandler : IUpdateHandler<Update>
{
    private readonly IUserService _userService;
    private readonly Configuration configuration;
    private readonly ITelegramBotClient _botClient;

    public UpdateHandler(IUserService userService, Configuration configuration, ITelegramBotClient botClient)
    {
        _userService = userService;
        this.configuration = configuration;
        _botClient = botClient;
    }

    private static readonly List<ITextCommand> TextCommands = new()
    {
        new StartCommand(),
        new SendKeyboardCommand(),
        new AdminMailingCommand(),
        new EnterMessageToMailingCommand(),
    };

    private static readonly List<ICallbackQueryCommand> CallbackQueryCommands = new()
    {
        new CheckQueryCommand(),
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
        Console.WriteLine($"Ошибка у {update.Id}: {ex.Message}\n {ex.StackTrace}");
    }

    private Task UnknownUpdateHandlerAsync(Update update)
    {
        return Task.CompletedTask;
    }

    private async Task BotOnCallbackQueryReceived(CallbackQuery updateCallbackQuery)
    {
        var user = _userService.Get(updateCallbackQuery.From!.Id);
        var command = CallbackQueryCommands.FirstOrDefault(command => command.Compare(updateCallbackQuery, user));
        if (command != null)
            await command.Execute(_botClient, user, updateCallbackQuery, _userService, configuration);
    }

    private async Task BotOnMessageReceived(Message updateMessage)
    {
        var user = _userService.Get(updateMessage.From!.Id);
        var command = TextCommands.FirstOrDefault(command => command.Compare(updateMessage, user));
        if (command != null) await command.Execute(_botClient, user, updateMessage, _userService, configuration);
    }
}