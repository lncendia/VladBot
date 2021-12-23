using VladBot.Core.Models;
using VladBot.Core.Repositories;
using VladBot.DAL.Data;

namespace VladBot.DAL.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<User> GetAll()
    {
        return _context.Users.ToList();
    }

    public void Add(User entity)
    {
        _context.Add(entity);
        _context.SaveChanges();
    }

    public void Delete(User entity)
    {
        _context.Remove(entity);
        _context.SaveChanges();
    }

    public void Update(User entity)
    {
        _context.SaveChanges();
    }

    public User? Get(long id)
    {
        return _context.Users.FirstOrDefault(user => user.Id == id);
    }

    public int GetCount(DateTime lowerUtcTime, DateTime upperUtcTime)
    {
        return _context.Users.Count(user =>
            user.RegistrationDate >= lowerUtcTime && user.RegistrationDate <= upperUtcTime);
    }

    public int GetAllCount()
    {
        return _context.Users.Count();
    }
}