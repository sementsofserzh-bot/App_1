using Essence;

namespace App_Model
{
    /// <summary>Правило расчёта рейтинга тренера.</summary>
    public interface ITrainerRatingCalculator
    {
        /// <summary>Вычисляет рейтинг тренера по заданной формуле.</summary>
        double Calculate(Trainer trainer);
    }
}
