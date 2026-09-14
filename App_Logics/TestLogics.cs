using System.Collections.Generic;
using App_Model_Essence;

namespace App_Model_TestLogics
{
    public interface ILogics
    {
        List<Trainer> BD_Trainer { get; set; }
        List<Athlete> BD_Athlete { get; set; }

        string AddTrainer(string fullName, string gender, int age);
        string AddAthlete(string fullName, string gender, int age);

        bool RemoveTrainer(int id);
        bool RemoveAthlete(int id);

        Trainer? CheckTrainer(int id);
        Athlete? CheckAthlete(int id);

        Trainer? UpdateInfoTrainer(int id, string fullName, string gender, int age);
        Athlete? UpdateInfoAthlete(int id, string fullName, string gender, int age);

        bool Registration(int idAthlete, int idTrainer);
        string PersonalTraining(string gender);

        List<Athlete>? Check_AtleteInTrainer(int id);
    }
    public class LogicStub : ILogics
    {
        public List<Trainer> BD_Trainer { get; set; } = new();
        public List<Athlete> BD_Athlete { get; set; } = new();

        public string AddTrainer(
            string fullName,
            string gender,
            int age)
        {
            return "ЗАГЛУШКА: тренер добавлен";
        }

        public string AddAthlete(
            string fullName,
            string gender,
            int age)
        {
            return "ЗАГЛУШКА: спортсмен добавлен";
        }

        public bool RemoveTrainer(int id)
        {
            return true;
        }

        public bool RemoveAthlete(int id)
        {
            return true;
        }

        public Trainer? CheckTrainer(int id)
        {
            return new Trainer
            {
                Id = id,
                FullName = "Тестовый тренер",
                Gender = "М",
                Age = 30
            };
        }

        public Athlete? CheckAthlete(int id)
        {
            return new Athlete
            {
                Id = id,
                FullName = "Тестовый спортсмен",
                Gender = "М",
                Age = 20
            };
        }

        public Trainer? UpdateInfoTrainer(
            int id,
            string fullName,
            string gender,
            int age)
        {
            return new Trainer
            {
                Id = id,
                FullName = fullName,
                Gender = gender,
                Age = age
            };
        }

        public Athlete? UpdateInfoAthlete(
            int id,
            string fullName,
            string gender,
            int age)
        {
            return new Athlete
            {
                Id = id,
                FullName = fullName,
                Gender = gender,
                Age = age
            };
        }

        public bool Registration(int idAthlete, int idTrainer)
        {
            return true;
        }

        public string PersonalTraining(string gender)
        {
            return $"ЗАГЛУШКА: персональная тренировка для пола {gender}";
        }

        public List<Athlete>? Check_AthleteInTrainer(int id)
        {
            return new List<Athlete>();
        }
    }
}