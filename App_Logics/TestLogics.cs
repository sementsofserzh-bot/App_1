using System;
using System.Collections.Generic;
using System.Linq;
using App_Model_Essence;

namespace App_Model_TestLogics
{
    public interface ILogics
    {
        List<Trainer> BD_Trainer { get; set; }
        List<Athlete> BD_Athlete { get; set; }

        Trainer AddTrainer(string fullname, Gendre gendre, TrainingType trainingType, int age, int workExperience);
        Athlete AddAthlete(string fullname, Gendre gendre, TrainingType trainingType, int age, int height, int weight);

        bool? RemoveTrainer(int id);
        bool? RemoveAthlete(int id);

        Trainer? CheckTrainer(int id);
        Athlete? CheckAthlete(int id);

        Trainer? UpdateInfoTrainer(int id, string? fullname, Gendre? gendre, TrainingType? trainingType, int? age, int? workExperience, List<Athlete>? deleteathlete, List<Athlete>? addathlete);
        Athlete? UpdateInfoAthlete(int id, string? fullname, Gendre? gendre, int? age, int? height, int? weight, TrainingType? trainingType, Trainer? trainer);

        bool Registration(Trainer trainer, Athlete athlete);
        string PersonalTraining(Athlete athlete);
        List<Trainer> PersonalFilterTrainers(Athlete athlete);
        List<Trainer> RateTrainers();
    }
}