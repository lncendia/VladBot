using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using VladBot.Core.Services;

namespace VladBot.Controllers;

public class BotController : ControllerBase
{
    private readonly IUpdateHandler<Update> _updateService;

    public BotController(IUpdateHandler<Update> updateService)
    {
        _updateService = updateService;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Update update)
    {
        await _updateService.HandleAsync(update);
        return Ok();
    }
}