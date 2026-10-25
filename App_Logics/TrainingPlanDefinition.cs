using Essence;

namespace App_Model
{
    /// <summary>Содержание выбранной программы и рекомендуемый тип тренировки.</summary>
    public record TrainingPlanDefinition(
        string Title,
        string Goal,
        string Schedule,
        string Details,
        TrainingType RecommendedType);
}
