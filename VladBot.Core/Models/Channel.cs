namespace VladBot.Core.Models;

public class Channel
{
    public long Id { get; set; }
    public string FollowLink { get; set; } = null!;
    public List<User>? Followers { get; set; }
}