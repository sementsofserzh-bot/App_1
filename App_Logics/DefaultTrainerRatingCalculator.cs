using Essence;

namespace App_Model
{
    /// <summary>Исходная формула рейтинга тренера.</summary>
    public class DefaultTrainerRatingCalculator : ITrainerRatingCalculator
    {
        public double Calculate(Trainer trainer)
        {
            if (trainer == null)
                return 0;

            int athleteCount = trainer.Athlete?.Count ?? 0;
            return (trainer.WorkExperience * 3.0) +
                   (athleteCount * 10.0) -
                   (trainer.Age * 0.5);
        }
    }
}
