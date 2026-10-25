using Essence;

namespace App_Model
{
    /// <summary>Рейтинг тренеров.</summary>
    public interface ITrainerRatingLogic
    {
        /// <summary>Обновляет рейтинги и возвращает тренеров по их убыванию.</summary>
        List<Trainer> RateTrainers();
        /// <summary>Вычисляет рейтинг одного тренера.</summary>
        double CalculateRating(Trainer trainer);
    }
}
