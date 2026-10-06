using App_Model;
using System.Collections.Generic;

namespace Contracts
{
    public interface ILogics
    {
        // Свойства получения списков
        List<Trainer> BD_Trainer { get; }
        List<Athlete> BD_Athlete { get; }

        // Добавление
        Trainer AddTrainer(string fullname, Gendre gendre, TrainingType trainingType, int age, int workExperience);
        Athlete AddAthlete(string fullname, Gendre gendre, TrainingType trainingType, int age, int height, int weight);

        // Удаление
        bool? RemoveTrainer(int id);
        bool? RemoveAthlete(int id);

        // Поиск
        Trainer? CheckTrainer(int id);
        Athlete? CheckAthlete(int id);

        // Редактирование
        Trainer? UpdateInfoTrainer(int id, string? fullname, Gendre? gendre, TrainingType? trainingType, int? age, int? workExperience, List<Athlete>? deleteathlete, List<Athlete>? addathlete);
        Athlete? UpdateInfoAthlete(int id, string? fullname, Gendre? gendre, int? age, int? height, int? weight, TrainingType? trainingType, Trainer? trainer);

        // Бизнес-операции
        bool Registration(Trainer trainer, Athlete athlete);
        string PersonalTraining(Athlete athlete);
        List<Trainer> PersonalFilterTrainers(Athlete athlete);
        List<Trainer> RateTrainers();
        double CalculateRating(Trainer trainer);
        int CalculateMatchPercentage(Trainer trainer, Athlete athlete);
    }
}