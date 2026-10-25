using Essence;

namespace App_Model
{
    /// <summary>Добавление спортсмена.</summary>
    public interface IAthleteCreateLogic
    {
        /// <summary>Создаёт и сохраняет спортсмена с указанными параметрами.</summary>
        Athlete AddAthlete(string fullname, Gendre gendre, TrainingType trainingType,
            int age, int height, int weight);
    }
}
