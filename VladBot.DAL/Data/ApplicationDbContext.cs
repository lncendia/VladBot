using Microsoft.EntityFrameworkCore;
using VladBot.Core.Models;

namespace VladBot.DAL.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
}