using VladBot.Core.Interfaces;
using VladBot.Core.Models;

namespace VladBot.Core.Repositories;

public interface IUserRepository : IRepository<User>
{
    public int GetCount(DateTime lowerUtcTime, DateTime upperUtcTime);
    public int GetAllCount();
}