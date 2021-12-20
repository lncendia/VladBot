using System.ComponentModel.DataAnnotations;

namespace VladBot.Core.Configuration;

public class Channel
{
    public long Id { get; set; }
    public string FollowLink { get; set; } = null!;
}