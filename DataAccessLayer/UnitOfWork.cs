using Essence;
using Contracts;


namespace DataAccessLayer
{
    /// <summary>Совместимый фасад единицы работы с реализацией Dapper по умолчанию.</summary>
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>Создаёт единицу работы с указанной строкой подключения.</summary>
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
