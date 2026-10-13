
using Essence;
using Contracts;

namespace DataAccessLayer
{
    public sealed class EntityFrameWorkUnitOfWork : IUnitOfWork
    {
        private readonly DBContext _context;

        public IRepository<Trainer> Trainers { get; }
        public IRepository<Athlete> Athletes { get; }

        public EntityFrameWorkUnitOfWork(string? connectionString = null)
        {
            _context = new DBContext(connectionString);
            Trainers = new EntityRepository<Trainer>(_context);
            Athletes = new EntityRepository<Athlete>(_context);
        }

        public void Save() => _context.SaveChanges();

        public void Dispose() => _context.Dispose();
    }
}
