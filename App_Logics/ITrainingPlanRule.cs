 using Essence;

namespace App_Model
{
    /// <summary>Правило выбора и создания программы тренировки.</summary>
    public interface ITrainingPlanRule
    {
        /// <summary>Определяет, подходит ли правило спортсмену при заданном индексе.</summary>
        bool Matches(Athlete athlete, double fitnessIndex);
        /// <summary>Создаёт описание программы для спортсмена.</summary>
        TrainingPlanDefinition Create(Athlete athlete);
    }
}
