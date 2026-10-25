using Essence;

namespace App_Model
{
    /// <summary>Исходная формула соответствия тренера спортсмену.</summary>
    public class DefaultTrainerMatchCalculator : ITrainerMatchCalculator
    {
        public int Calculate(Trainer trainer, Athlete athlete)
        {
            if (trainer == null || athlete == null)
                return 0;

            int score = 0;

            if (trainer.TrainingType == athlete.TypePersonalTraining)
                score += 45;
            else if (trainer.TrainingType.ToString().Contains(athlete.Gendre.ToString()))
                score += 20;

            if (trainer.Gendre == athlete.Gendre)
                score += 15;

            score += Math.Min(trainer.WorkExperience * 2, 20);

            int count = trainer.Athlete?.Count ?? 0;
            score += Math.Max(0, 20 - count * 4);

            return Math.Min(score, 100);
        }
    }
}
