using App_Model;
using Contracts;

namespace DataAccessLayer
{
    /// <summary>
    /// Реализация Unit of Work для работы с базой данных через Dapper.
    /// </summary>
    public class DapperUnitOfWork : IUnitOfWork
    {
        public IRepository<Trainer> Trainers { get; }
        public IRepository<Athlete> Athletes { get; }

        public DapperUnitOfWork(string? connectionString = null)
        {
            string connStr = connectionString ?? DatabaseInitializer.ConnectionString;

            Trainers = new TrainerDapperRepository(connStr);
            Athletes = new AthleteDapperRepository(connStr);
        }

        /// <summary>
        /// В Dapper-репозиториях запросы отправляются в БД сразу при вызове Add/Update/Delete.
        /// Метод присутствует для сохранения единого контракта с EFUnitOfWork.
        /// </summary>
        public void Save()
        {
            // Пустая реализация
        }

        /// <summary>
        /// В Dapper каждое подключение открывается и закрывается внутри репозитория (using var connection).
        /// Метод реализует IDisposable для совместимости с общепринятым интерфейсом IUnitOfWork.
        /// </summary>
        public void Dispose()
        {
            // Пустая реализация
        }
    }
}