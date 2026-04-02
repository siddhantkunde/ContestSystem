using Microsoft.EntityFrameworkCore;
using ContestSystem.Models;

namespace ContestSystem.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Contest> Contests { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Option> Options { get; set; }
    public DbSet<Participation> Participations { get; set; }
    public DbSet<Answer> Answers { get; set; }
}