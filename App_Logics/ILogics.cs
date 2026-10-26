using Essence;

namespace App_Model
{
    /// <summary>
    /// Совместимый полный интерфейс фасада. Потребители используют более узкие
    /// IEmployeeLogic, IUserLogic и интерфейсы форм редактирования.
    /// </summary>
    public interface ILogics : ICombinedLogic, IUserLogic
    {
        /// <summary>Вычисляет рейтинг тренера по текущему правилу.</summary>
        double CalculateRating(Trainer trainer);
    }
}