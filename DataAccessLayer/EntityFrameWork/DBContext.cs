using Microsoft.EntityFrameworkCore;
using App_Model;

namespace DataAccessLayer
{
    /// <summary>
    /// Контекст базы данных Entity Framework Core.
    /// Определяет сущности базы данных и настраивает подключение к SQLite.
    /// </summary>
    public class DBContext : DbContext
    {
        private readonly string _connectionString;

        public DBContext(string? connectionString = null)
        {
            _connectionString = connectionString ?? DatabaseInitializer.ConnectionString;
        }

        /// <summary>
        /// Набор сущностей Trainer в базе данных.
        /// </summary>
        public DbSet<Trainer> Trainers { get; set; }

        /// <summary>
        /// Набор сущностей Athlete в базе данных.
        /// </summary>
        public DbSet<Athlete> Athletes { get; set; }

        /// <summary>
        /// Настраивает подключение Entity Framework Core к базе данных SQLite.
        /// </summary>
        /// <param name="optionsBuilder">
        /// Объект для настройки параметров контекста базы данных.
        /// </param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_connectionString);
        }

        /// <summary>
        /// Настраивает модель данных и связи между сущностями.
        /// Устанавливает связь один-ко-многим между Trainer и Athlete.
        /// </summary>
        /// <param name="modelBuilder">
        /// Объект для настройки модели Entity Framework Core.
        /// </param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Trainer>()
                .HasMany(t => t.Athlete)
                .WithOne(a => a.trainer)
                .HasForeignKey("TrainerId")
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
