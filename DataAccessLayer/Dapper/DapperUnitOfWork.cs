
using Contracts;
using Essence;


namespace DataAccessLayer
{
    /// <summary>
    /// Реализация Unit of Work для работы с базой данных через Dapper.
    /// </summary>
    public class DapperUnitOfWork : IUnitOfWork
    {
        private readonly DapperSession _session;

        public IRepository<Trainer> Trainers { get; }
        public IRepository<Athlete> Athletes { get; }

        /// <summary>Создаёт общую сессию Dapper для двух репозиториев.</summary>
        public DapperUnitOfWork(string? connectionString = null)
        {
            _session = new DapperSession(connectionString);
            Trainers = new TrainerDapperRepository(_session);
            Athletes = new AthleteDapperRepository(_session);
        }

        /// <summary>
        /// Подтверждает общую транзакцию репозиториев.
        /// </summary>
        public void Save() => _session.Save();

        /// <summary>
        /// Отменяет неподтверждённые изменения и освобождает соединение.
        /// </summary>
        public void Dispose() => _session.Dispose();
    }
}
