using App_Model;
using Contracts;

namespace DataAccessLayer
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly DBContext context;

        public UnitOfWork(
            DBContext context,
            IRepository<Trainer> trainers,
            IRepository<Athlete> athletes)
        {
            this.context = context;
            Trainers = trainers;
            Athletes = athletes;
        }

        public IRepository<Trainer> Trainers { get; }
        public IRepository<Athlete> Athletes { get; }

        public int SaveChanges()
        {
            int result = context.SaveChanges();
            context.ChangeTracker.Clear();
            return result;
        }

        public void Dispose() => context.Dispose();
    }
}
