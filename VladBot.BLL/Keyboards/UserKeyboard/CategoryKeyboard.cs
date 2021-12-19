using Telegram.Bot.Types.ReplyMarkups;
using VladBot.Core.Models;

namespace VladBot.BLL.Keyboards.UserKeyboard;

public static class CategoryKeyboard
{
    public static InlineKeyboardMarkup Create(List<string> usernames)
    {
        var list = new List<List<InlineKeyboardButton>>();
        list.AddRange(usernames.Take(10).Select(link => new List<InlineKeyboardButton>
            {InlineKeyboardButton.WithUrl("✅ Подписаться", link)}));
        list.Add(new List<InlineKeyboardButton> {InlineKeyboardButton.WithCallbackData("🔎 Проверить", "check")});
        return new InlineKeyboardMarkup(list);
    }

    public static InlineKeyboardMarkup FinalLink(string link)
    {
        return new InlineKeyboardMarkup(InlineKeyboardButton.WithUrl("Перейти", link));
    }
}