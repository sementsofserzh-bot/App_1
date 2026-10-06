using App_Model;
using Contracts;

namespace DataAccessLayer
{
    public class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// Единица работы для координации операций репозиториев и управления транзакциями.
        /// </summary>
        /// <remarks>Инициализируется через конструктор (внедрение зависимостей); поле только для
        /// чтения.</remarks>
        private readonly IUnitOfWork _uow;
        /// <summary>
        /// Инициализирует новый экземпляр UnitOfWork, использующий DapperUnitOfWork.
        /// </summary>
        /// <remarks>По умолчанию используется DapperUnitOfWork. Для переключения на реализацию EF
        /// раскомментируйте создание EfUnitOfWork.</remarks>
        /// <param name="connectionString">Опциональная строка подключения; при null используется значение по умолчанию или конфигурация окружения.</param>
        public UnitOfWork(string? connectionString = null)
        {
            _uow = new DapperUnitOfWork(connectionString);
            // _uow = new EfUnitOfWork(connectionString);
        }
        /// <summary>
        /// Получает репозиторий для работы с сущностями Trainer.
        /// </summary>
        /// <remarks>Репозиторий предоставляется через экземпляр Unit of Work (_uow).</remarks>
        public IRepository<Trainer> Trainers => _uow.Trainers;
        public IRepository<Athlete> Athletes => _uow.Athletes;
        /// <summary>
        /// Сохраняет накопленные изменения через связанную единицу работы.
        /// </summary>
        /// <remarks>Выполняет делегирование вызова в _uow.Save(). Исключения при сохранении
        /// пробрасываются вызывающему. Операция выполняется синхронно.</remarks>
        public void Save() => _uow.Save();
        public void Dispose() => _uow.Dispose();
    }
}