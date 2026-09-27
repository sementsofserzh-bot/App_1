using Microsoft.EntityFrameworkCore;
using App_Model;
using DataAccessLayer;
public class DBContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlite(DatabaseInitializer.ConnectionString);
    }
    public DbSet<Athlete> Athletes { get; set; }
    public DbSet<Trainer> Trainers { get; set; }
}