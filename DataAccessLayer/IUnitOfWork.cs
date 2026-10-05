using App_Model;
using Contracts;

namespace DataAccessLayer
{
    public interface IUnitOfWork
    {
        IRepository<Trainer> Trainers { get; }
        IRepository<Athlete> Athletes { get; }
    }
}