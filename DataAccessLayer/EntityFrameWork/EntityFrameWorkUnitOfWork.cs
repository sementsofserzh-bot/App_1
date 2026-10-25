
using Essence;
using Contracts;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    /// <summary>
    /// Реализация Unit of Work для работы с БД через EF
    /// </summary>
    public sealed class EntityFrameWorkUnitOfWork : IUnitOfWork
    {
        private readonly DBContext _context;

        public IRepository<Trainer> Trainers { get; }
        public IRepository<Athlete> Athletes { get; }

        /// <summary>Создаёт контекст EF и репозитории с заданным подключением.</summary>
        public EntityFrameWorkUnitOfWork(string? connectionString = null)
        {
            _context = new DBContext(connectionString);
            Trainers = new EntityRepository<Trainer>(_context, query => query.Include(t => t.Athlete));
            Athletes = new EntityRepository<Athlete>(_context, query => query.Include(a => a.trainer));
        }
        /// <summary>
        /// Метод для сохранения изменений в контексте БД
        /// </summary>
        public void Save() => _context.SaveChanges();

        public void Dispose() => _context.Dispose();
    }
}
