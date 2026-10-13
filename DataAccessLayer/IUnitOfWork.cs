using Contracts;
using Essence;
namespace Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Trainer> Trainers { get; }
        IRepository<Athlete> Athletes { get; }
        void Save();
    }
}
