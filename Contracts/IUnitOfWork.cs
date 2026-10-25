using Essence;

namespace Contracts
{
    /// <summary>
    /// Общая единица работы для репозиториев. Изменения становятся постоянными после Save().
    /// Если объект освобождён до Save(), неподтверждённые изменения отменяются.
    /// После Save() можно начать следующую группу изменений.
    /// Значения Id новых объектов следует считать постоянными только после Save().
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>Репозиторий тренеров текущей единицы работы.</summary>
        IRepository<Trainer> Trainers { get; }
        /// <summary>Репозиторий спортсменов текущей единицы работы.</summary>
        IRepository<Athlete> Athletes { get; }

        /// <summary>Атомарно сохраняет изменения обоих репозиториев.</summary>
        void Save();
    }
}
