using Essence;

namespace App_Model
{
    /// <summary>Правило расчёта индекса для выбора программы.</summary>
    public interface IFitnessIndexCalculator
    {
        /// <summary>Вычисляет ИМТ и индекс нагрузки спортсмена.</summary>
        (double Bmi, double FitnessIndex) Calculate(Athlete athlete);
    }
}
