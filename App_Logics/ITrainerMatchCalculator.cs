using Essence;

namespace App_Model
{
    /// <summary>Правило расчёта соответствия тренера спортсмену.</summary>
    public interface ITrainerMatchCalculator
    {
        /// <summary>Вычисляет процент соответствия тренера спортсмену.</summary>
        int Calculate(Trainer trainer, Athlete athlete);
    }
}
