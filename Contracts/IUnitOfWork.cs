using System;
using App_Model;
using Contracts;

namespace DataAccessLayer
{
    /// <summary>
    /// Интерфейс паттерна Unit of Work для управления репозиториями и транзакциями.
    /// Наследуется от IDisposable для освобождения ресурсов (например, DbContext в EF).
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Репозиторий для работы с тренерами.
        /// </summary>
        IRepository<Trainer> Trainers { get; }

        /// <summary>
        /// Репозиторий для работы со спортсменами.
        /// </summary>
        IRepository<Athlete> Athletes { get; }

        /// <summary>
        /// Фиксирует/сохраняет изменения.
        /// </summary>
        void Save();

        void Dispose();
    }
}