﻿using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using VladBot.BLL.Interfaces;
using VladBot.Core.Enums;
using VladBot.Core.Services;
using User = VladBot.Core.Models.User;

namespace VladBot.BLL.CallbackQueryCommands;

public class DeleteQueryCommand : ICallbackQueryCommand
{
    public async Task Execute(ITelegramBotClient client, User? user, CallbackQuery query, IUserService userService,
        IChannelService channelService,
        Core.Configuration.Configuration configuration)
    {
        var id = long.Parse(query.Data![7..]);
        var channel = channelService.Get(id);
        if (channel == null)
        {
            await client.EditMessageTextAsync(user!.Id, query.Message!.MessageId,
                "Не удалось найти канал.");
            return;
        }

        var result = channelService.Delete(channel);
        if (result.Succeeded)
        {
            await client.EditMessageTextAsync(user!.Id, query.Message!.MessageId,
                "Канал успешно удалён.");
        }
        else
        {
            await client.EditMessageTextAsync(user!.Id, query.Message!.MessageId,
                $"Не удалось удалить канал: <code>{result.ErrorMessage}</code>.", ParseMode.Html);
        }
    }

    public bool Compare(CallbackQuery query, User? user)
    {
        return user!.State == State.Main && query.Data!.StartsWith("delete") && user.IsAdmin;
    }
}