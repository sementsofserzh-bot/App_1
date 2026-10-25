using Essence;

namespace App_Model
{
    /// <summary>Чтение тренеров.</summary>
    public interface ITrainerCatalog
    {
        /// <summary>Возвращает текущий список тренеров.</summary>
        List<Trainer> BD_Trainer { get; }
        /// <summary>Возвращает тренера по идентификатору или null, если он не найден.</summary>
        Trainer? CheckTrainer(int id);
    }
}
