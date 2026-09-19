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

    public class Logics : ILogics
    {
        public List<Trainer> BD_Trainer { get; set; } = new List<Trainer>();
        public List<Athlete> BD_Athlete { get; set; } = new List<Athlete>();

        private int _nextTrainerId = 1;
        private int _nextAthleteId = 1;

        public Logics()
        {
            SeedInitialData();
        }

        private void SeedInitialData()
        {
            var t1 = AddTrainer("Иванов Александр Сергеевич", (Gendre)0, (TrainingType)0, 32, 8);
            var t2 = AddTrainer("Петрова Елена Игоревна", (Gendre)1, (TrainingType)1, 27, 4);

            var a1 = AddAthlete("Сидоров Алексей Викторович", (Gendre)0, (TrainingType)0, 25, 180, 82);
            var a2 = AddAthlete("Ковалева Ольга Андреевна", (Gendre)1, (TrainingType)1, 22, 168, 55);

            Registration(t1, a1);
            Registration(t2, a2);
        }

        public Trainer AddTrainer(string fullname, Gendre gendre, TrainingType trainingType, int age, int workExperience)
        {
            var trainer = new Trainer
            {
                Id = _nextTrainerId++,
                FullName = string.IsNullOrWhiteSpace(fullname) ? "Без имени" : fullname,
                Gendre = gendre,
                TrainingType = trainingType,
                Age = age,
                WorkExperience = workExperience,
                Athlete = new List<Athlete>()
            };

            BD_Trainer.Add(trainer);
            return trainer;
        }

        public Athlete AddAthlete(string fullname, Gendre gendre, TrainingType trainingType, int age, int height, int weight)
        {
            var athlete = new Athlete
            {
                Id = _nextAthleteId++,
                FullName = string.IsNullOrWhiteSpace(fullname) ? "Без имени" : fullname,
                Gendre = gendre,
                TrainingType = trainingType,
                Age = age,
                Height = height,
                Weight = weight,
                trainer = null
            };

            BD_Athlete.Add(athlete);
            return athlete;
        }

        public bool? RemoveTrainer(int id)
        {
            var trainer = CheckTrainer(id);
            if (trainer == null) return false;

            if (trainer.Athlete != null)
            {
                foreach (var athlete in trainer.Athlete.ToList())
                {
                    athlete.trainer = null;
                }
            }

            BD_Trainer.Remove(trainer);
            return true;
        }

        public bool? RemoveAthlete(int id)
        {
            var athlete = CheckAthlete(id);
            if (athlete == null) return false;

            if (athlete.trainer != null && athlete.trainer.Athlete != null)
            {
                athlete.trainer.Athlete.Remove(athlete);
            }

            BD_Athlete.Remove(athlete);
            return true;
        }

        public Trainer? CheckTrainer(int id)
        {
            return BD_Trainer.FirstOrDefault(t => t.Id == id);
        }

        public Athlete? CheckAthlete(int id)
        {
            return BD_Athlete.FirstOrDefault(a => a.Id == id);
        }

        public Trainer? UpdateInfoTrainer(int id, string? fullname, Gendre? gendre, TrainingType? trainingType, int? age, int? workExperience, List<Athlete>? deleteathlete, List<Athlete>? addathlete)
        {
            var trainer = CheckTrainer(id);
            if (trainer == null) return null;

            if (!string.IsNullOrWhiteSpace(fullname)) trainer.FullName = fullname;
            if (gendre.HasValue) trainer.Gendre = gendre.Value;
            if (trainingType.HasValue) trainer.TrainingType = trainingType.Value;
            if (age.HasValue) trainer.Age = age.Value;
            if (workExperience.HasValue) trainer.WorkExperience = workExperience.Value;

            if (deleteathlete != null)
            {
                foreach (var athlete in deleteathlete)
                {
                    if (trainer.Athlete.Contains(athlete))
                    {
                        trainer.Athlete.Remove(athlete);
                        athlete.trainer = null;
                    }
                }
            }

            if (addathlete != null)
            {
                foreach (var athlete in addathlete)
                {
                    Registration(trainer, athlete);
                }
            }

            return trainer;
        }

        public Athlete? UpdateInfoAthlete(int id, string? fullname, Gendre? gendre, int? age, int? height, int? weight, TrainingType? trainingType, Trainer? trainer)
        {
            var athlete = CheckAthlete(id);
            if (athlete == null) return null;

            if (!string.IsNullOrWhiteSpace(fullname)) athlete.FullName = fullname;
            if (gendre.HasValue) athlete.Gendre = gendre.Value;
            if (age.HasValue) athlete.Age = age.Value;
            if (height.HasValue) athlete.Height = height.Value;
            if (weight.HasValue) athlete.Weight = weight.Value;
            if (trainingType.HasValue) athlete.TrainingType = trainingType.Value;

            if (trainer != null)
            {
                Registration(trainer, athlete);
            }

            return athlete;
        }

        public bool Registration(Trainer trainer, Athlete athlete)
        {
            if (trainer == null || athlete == null) return false;

            if (trainer.Athlete == null)
            {
                trainer.Athlete = new List<Athlete>();
            }

            if (athlete.trainer != null && athlete.trainer != trainer)
            {
                athlete.trainer.Athlete?.Remove(athlete);
            }

            if (!trainer.Athlete.Contains(athlete))
            {
                trainer.Athlete.Add(athlete);
                athlete.trainer = trainer;
                return true;
            }

            return false;
        }

        public string PersonalTraining(Athlete athlete)
        {
            if (athlete == null) return "Атлет не найден.";

            double heightInMeters = athlete.Height / 100.0;
            double bmi = heightInMeters > 0 ? athlete.Weight / (heightInMeters * heightInMeters) : 0;

            string plan;
            if (bmi < 18.5)
                plan = "Силовой тренинг на массу + профицит калорий.";
            else if (bmi <= 24.9)
                plan = "Сбалансированная программа (Силовая + Кардио).";
            else if (bmi <= 29.9)
                plan = "Функциональный тренинг + Жиросжигание.";
            else
                plan = "Низкоударные аэробные нагрузки.";

            string result = $"ИМТ: {bmi:F1} | Направление: {athlete.TrainingType} | Рекомендация: {plan}";

            return result;
        }

        public List<Trainer> PersonalFilterTrainers(Athlete athlete)
        {
            if (athlete == null) return new List<Trainer>();

            return BD_Trainer
                .Where(t => t.TrainingType == athlete.TrainingType)
                .ToList();
        }

        public List<Trainer> RateTrainers()
        {
            return BD_Trainer
                .OrderByDescending(t => t.Athlete?.Count ?? 0)
                .ThenByDescending(t => t.WorkExperience)
                .ToList();
        }
    }
}