using App_Model;

namespace Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Trainer> Trainers { get; }
        IRepository<Athlete> Athletes { get; }
        int SaveChanges();
    }
}
