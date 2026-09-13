using System;
using System.Collections.Generic;
using System.Linq;
using App_Model_Essence;
using App_Model_TestLogics;

namespace App_Model_Logics
{
    public class Logics : ILogics
    {
        public List<Trainer> BD_Trainer { get; set; } = new();
        public List<Athlete> BD_Athlete { get; set; } = new();

        private int _nextTrainerId = 1;
        private int _nextAthleteId = 1;

        // ==================== ДОБАВЛЕНИЕ ====================

        public string AddTrainer(string fullName, string gender, int age, double weight, double height)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                return "Ошибка: имя не может быть пустым";
            if (age < 0 || age > 120)
                return "Ошибка: некорректный возраст";

            var trainer = new Trainer
            {
                Id = _nextTrainerId++,
                FullName = fullName,
                Gender = gender,
                Age = age,
                Weight = weight,
                Height = height
            };
            BD_Trainer.Add(trainer);
            return $"Тренер {fullName} добавлен с Id={trainer.Id}";
        }

        public string AddAthlete(string fullName, string gender, int age, double weight, double height)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                return "Ошибка: имя не может быть пустым";
            if (age < 0 || age > 120)
                return "Ошибка: некорректный возраст";

            var athlete = new Athlete
            {
                Id = _nextAthleteId++,
                FullName = fullName,
                Gender = gender,
                Age = age,
                Weight = weight,
                Height = height
            };
            BD_Athlete.Add(athlete);
            return $"Спортсмен {fullName} добавлен с Id={athlete.Id}";
        }

        // ==================== УДАЛЕНИЕ ====================

        public bool RemoveTrainer(int id)
        {
            var trainer = BD_Trainer.FirstOrDefault(t => t.Id == id);
            if (trainer == null) return false;

            BD_Trainer.Remove(trainer);

            foreach (var a in BD_Athlete.Where(a => a.TrainerId == id))
                a.TrainerId = 0;

            return true;
        }

        public bool RemoveAthlete(int id)
        {
            var athlete = BD_Athlete.FirstOrDefault(a => a.Id == id);
            if (athlete == null) return false;

            BD_Athlete.Remove(athlete);

            foreach (var t in BD_Trainer)
                t.AthleteIds.Remove(id);

            return true;
        }

        // ==================== ПОИСК ====================

        public Trainer? CheckTrainer(int id) =>
            BD_Trainer.FirstOrDefault(t => t.Id == id);

        public Athlete? CheckAthlete(int id) =>
            BD_Athlete.FirstOrDefault(a => a.Id == id);

        // ==================== ИЗМЕНЕНИЕ ====================

        public Trainer? UpdateInfoTrainer(int id, string fullName, string gender, int age, double weight, double height)
        {
            var trainer = CheckTrainer(id);
            if (trainer == null) return null;

            trainer.FullName = fullName;
            trainer.Gender = gender;
            trainer.Age = age;
            trainer.Weight = weight;
            trainer.Height = height;
            return trainer;
        }

        public Athlete? UpdateInfoAthlete(int id, string fullName, string gender, int age, double weight, double height)
        {
            var athlete = CheckAthlete(id);
            if (athlete == null) return null;

            athlete.FullName = fullName;
            athlete.Gender = gender;
            athlete.Age = age;
            athlete.Weight = weight;
            athlete.Height = height;
            return athlete;
        }

        // ==================== БИЗНЕС-ФУНКЦИИ ====================

        public bool Registration(int idAthlete, int idTrainer)
        {
            var athlete = CheckAthlete(idAthlete);
            var trainer = CheckTrainer(idTrainer);

            if (athlete == null || trainer == null) return false;

            if (athlete.TrainerId != 0)
            {
                var oldTrainer = CheckTrainer(athlete.TrainerId);
                oldTrainer?.AthleteIds.Remove(idAthlete);
            }

            athlete.TrainerId = idTrainer;
            if (!trainer.AthleteIds.Contains(idAthlete))
                trainer.AthleteIds.Add(idAthlete);

            return true;
        }

        public string PersonalTraining(string gender)
        {
            var matched = BD_Athlete
                .Where(a => a.Gender.Equals(gender, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (matched.Count == 0)
                return $"Нет спортсменов с полом '{gender}'";

            return string.Join("\n", matched.Select(a =>
                $"#{a.Id} {a.FullName} | тренер: {(a.TrainerId == 0 ? "нет" : a.TrainerId.ToString())}"));
        }
    }
}