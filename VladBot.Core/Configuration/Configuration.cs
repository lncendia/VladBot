using System.ComponentModel.DataAnnotations;

namespace VladBot.Core.Configuration;

public class Configuration
{
    [Required(ErrorMessage = "The final channel is not set")]
    public Channel FinalChanel { get; set; } = null!;
    [Required(ErrorMessage = "The channels is not set")]
    public List<Channel> Channels { get; set; } = null!;
}