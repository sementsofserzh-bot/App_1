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
        int nextTrainer_ID = 0; 
        int nextAthlete_ID = 0;
        //Добавление
        public Trainer AddTrainer(string fullname, Gendre gendre, TrainingType trainingType, int age, int workExperience)
        {
            if (string.IsNullOrWhiteSpace(fullname))
                throw new ArgumentException("Имя не может быть пустым");
            if (age < 18 || age > 120) 
                throw new ArgumentException("Некорректный возраст"); //ДЛЯ СЕРЕГИ: ВО ВЬЮХЕ ВОЗРАСТ И ОПЫТ ПРОСИ УКАЗЫВАТЬ В ГОДАХ
            if (workExperience < 0 || workExperience > age - 18)
                throw new ArgumentException("Некорректный опыт работы");
            BD_Trainer.Add(new Trainer { Id = nextTrainer_ID++, FullName = fullname, Gendre = gendre, TrainingType = trainingType, Age = age, WorkExperience = workExperience });
            return BD_Trainer[BD_Trainer.Count - 1];
        }
        public Athlete AddAthlete(string fullname, Gendre gendre, TrainingType trainingType, int age, int height, int weight)
        {
            if (string.IsNullOrWhiteSpace(fullname))
                throw new ArgumentException("Имя не может быть пустым");
            if (age < 14 || age > 120)
                throw new ArgumentException("Некорректный возраст");
            if (height <= 0 || height > 300) //ДЛЯ СЕРЕГИ: ВО ВЬЮХЕ РОСТ ПРОСИ УКАЗЫВАТЬ В СМ
                throw new ArgumentException("Некорректный рост");
            if (weight <= 0 || weight > 1000) //ДЛЯ СЕРЕГИ:ВО ВЬЮХЕ ВЕС ПРОСИ УКАЗЫВАТЬ В КГ
                throw new ArgumentException("Некорректный вес");
            BD_Athlete.Add(new Athlete { Id = nextAthlete_ID++, FullName = fullname, Gendre = gendre, TrainingType = trainingType, Age = age, Height = height, Weight = weight });
            return BD_Athlete[BD_Athlete.Count - 1];
        }
        //удаление
        public bool RemoveTrainer(int id)
        {
            var trainer = BD_Trainer.FirstOrDefault(t => t.Id == id);
            if (trainer == null) { return false; }
            BD_Trainer.Remove(trainer);
            return true;
        }
        public bool RemoveAthlete(int id)
        {
            var athlete = BD_Athlete.FirstOrDefault(a => a.Id == id);
            if (athlete == null) { return false; }
            BD_Athlete.Remove(athlete);
            return true;
        }
        //Чтение
        public Trainer? CheckTrainer(int id)
        {
            return BD_Trainer.FirstOrDefault(t => t.Id == id);
        }
        public Athlete? CheckAthlete(int id)
        {
            return BD_Athlete.FirstOrDefault(a => a.Id == id);
        }
        //обновление
        public Trainer? UpdateInfoTrainer(int id, string fullname, Gendre gendre, TrainingType trainingType, int age, int workExperience, List<Athlete> deleteathlete, List<Athlete> addathlete)
        {
            var chosen_trainer = BD_Trainer.FirstOrDefault(a => a.Id == id);
            if (chosen_trainer == null) { return null; }
            chosen_trainer.FullName = fullname;
            chosen_trainer.Gendre = gendre;
            chosen_trainer.TrainingType = trainingType;
            chosen_trainer.Age = age;
            chosen_trainer.WorkExperience = workExperience;
            if (deleteathlete.Count != 0)
            {
                foreach (var athlete in deleteathlete) 
                {
                    chosen_trainer.AthleteIds.Remove(athlete.Id);
                    var athlete_In_DB = BD_Athlete.FirstOrDefault(x => x.Id == athlete.Id);
                    if (athlete_In_DB != null) { athlete_In_DB. = 0; }
                }

        }





    }
    //public class Logics : ILogics
    //{
    //    public List<Trainer> BD_Trainer { get; set; } = new();
    //    public List<Athlete> BD_Athlete { get; set; } = new();

    //    private int _nextTrainerId = 1;
    //    private int _nextAthleteId = 1;
    //    public string AddTrainer(string fullName, Gendre gendre, int age)
    //    {
    //        BD_Trainer.Add(new Trainer { Age = age, Gendre = gendre, FullName = fullName } );
    //        if (string.IsNullOrWhiteSpace(fullName)) { return "Имя тренера не может быть пустым!"; }
    //        return $"Новый тренер - {fullName} добавлен";
    //    }
    //    public string AddAthlete(string fullName, Gendre gendre, int age)
    //    {
    //        BD_Athlete.Add(new Athlete { Age = age, Gendre = gendre, FullName = fullName });
    //        if (string.IsNullOrWhiteSpace(fullName)) { return "Имя спортсмена не может быть пустым!"; }
    //        return $"Новый спортсмен - {fullName} добавлен";
    //    }
    //    public bool RemoveTrainer(int id)
    //    {
    //        BD_Trainer.Remove(BD_Trainer[id - 1]);
    //        return true;
    //    }
    //    public bool RemoveAthlete(int id)
    //    {
    //        BD_Athlete.Remove(BD_Athlete[id - 1]);
    //        return true;
    //    }
    //    public Trainer? CheckTrainer(int id) => BD_Trainer.FirstOrDefault(t => t.Id == id);
    //    public Athlete? CheckAthlete(int id) => BD_Athlete.FirstOrDefault(a => a.Id == id);

    //    public Trainer? UpdateInfoTrainer(int id, string fullName, Gendre gendre, int age) 

    //    =>    BD_Trainer[id - 1] = new Trainer { Age = age, Gendre = gendre, FullName = fullName  };

    //    public Athlete? UpdateInfoAthlete(int id, string fullName, Gendre gendre, int age)

    //    => BD_Athlete[id - 1] = new Athlete { Age = age, Gendre = gendre, FullName = fullName };

    //    public bool Registration(int idAthlete, int idTrainer)
    //    {
    //        //coming soon
    //    }
    //    string PersonalTraining(Gendre gendre, int height, int weight, int age)
    //    {
    //        //coming soon
    //    }
    //    public List<Trainer> RateTrainers()
    //    {
    //        return BD_Trainer.OrderByDescending();
    //    }

    //}
}