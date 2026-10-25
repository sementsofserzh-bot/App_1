using Essence;

namespace App_Model
{
    /// <summary>Исходная формула индекса для выбора программы.</summary>
    public class DefaultFitnessIndexCalculator : IFitnessIndexCalculator
    {
        public (double Bmi, double FitnessIndex) Calculate(Athlete athlete)
        {
            double heightInM = athlete.Height / 100.0;
            double bmi = athlete.Weight / (heightInM * heightInM);
            double genderDelta = athlete.Gendre == Gendre.М ? -1.0 : 1.5;

            double prefDelta = 0.0;
            if (athlete.TrainingType == TrainingType.М_Силовая ||
                athlete.TrainingType == TrainingType.Ж_Силовая)
                prefDelta = -3.0;
            else if (athlete.TrainingType == TrainingType.М_Гибкость ||
                     athlete.TrainingType == TrainingType.Ж_Гибкость)
                prefDelta = 4.0;

            double ageDelta = athlete.Age >= 40 ? (athlete.Age - 40) * 0.25 : 0.0;
            return (bmi, bmi + genderDelta + prefDelta + ageDelta);
        }
    }
}
