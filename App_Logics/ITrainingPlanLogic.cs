using Essence;

namespace App_Model
{
    /// <summary>Составление персональной программы.</summary>
    public interface ITrainingPlanLogic
    {
        /// <summary>Составляет, сохраняет и возвращает персональную программу спортсмена.</summary>
        string PersonalTraining(Athlete athlete);
    }
}
