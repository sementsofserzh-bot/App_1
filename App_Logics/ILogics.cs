using System.Collections.Generic;

namespace App_Model
{
    /// <summary>
    /// Интерфейс бизнес-логики спортивного зала.
    /// </summary>
    public interface ILogics
    {
        /// <summary>
        /// Возвращает всех тренеров.
        /// </summary>
        List<Trainer> BD_Trainer { get; }

        /// <summary>
        /// Возвращает всех атлетов.
        /// </summary>
        List<Athlete> BD_Athlete { get; }

        Trainer AddTrainer(
            string fullname,
            Gendre gendre,
            TrainingType trainingType,
            int age,
            int workExperience);

        Athlete AddAthlete(
            string fullname,
            Gendre gendre,
            TrainingType trainingType,
            int age,
            int height,
            int weight);

        bool? RemoveTrainer(int id);

        bool? RemoveAthlete(int id);

        Trainer? CheckTrainer(int id);

        Athlete? CheckAthlete(int id);

        Trainer? UpdateInfoTrainer(
            int id,
            string? fullname,
            Gendre? gendre,
            TrainingType? trainingType,
            int? age,
            int? workExperience,
            List<Athlete>? deleteathlete,
            List<Athlete>? addathlete);

        Athlete? UpdateInfoAthlete(
            int id,
            string? fullname,
            Gendre? gendre,
            int? age,
            int? height,
            int? weight,
            TrainingType? trainingType,
            Trainer? trainer);

        bool Registration(
            Trainer trainer,
            Athlete athlete);

        string PersonalTraining(
            Athlete athlete);

        List<Trainer> PersonalFilterTrainers(
            Athlete athlete);

        List<Trainer> RateTrainers();

        double CalculateRating(
            Trainer trainer);

        int CalculateMatchPercentage(
            Trainer trainer,
            Athlete athlete);
    }
}