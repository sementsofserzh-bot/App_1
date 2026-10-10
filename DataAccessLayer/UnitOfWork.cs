using App_Model;
using Contracts;

namespace DataAccessLayer
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly IUnitOfWork _unitOfWork;

        public UnitOfWork(string? connectionString = null)
        {
            _unitOfWork = new DapperUnitOfWork(connectionString);
        }

        public IRepository<Trainer> Trainers => _unitOfWork.Trainers;
        public IRepository<Athlete> Athletes => _unitOfWork.Athletes;

        public void Save() => _unitOfWork.Save();

        public void Dispose() => _unitOfWork.Dispose();
    }
}
