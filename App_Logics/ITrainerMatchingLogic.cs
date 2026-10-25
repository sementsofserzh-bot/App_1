using Essence;

namespace App_Model
{
    /// <summary>Подбор тренеров и расчёт соответствия.</summary>
    public interface ITrainerMatchingLogic
    {
        /// <summary>Возвращает тренеров по убыванию соответствия спортсмену.</summary>
        List<Trainer> PersonalFilterTrainers(Athlete athlete);
        /// <summary>Вычисляет процент соответствия тренера спортсмену.</summary>
        int CalculateMatchPercentage(Trainer trainer, Athlete athlete);
    }
}
