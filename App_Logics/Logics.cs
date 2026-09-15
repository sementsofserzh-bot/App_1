using System;
using System.Collections.Generic;
using System.Linq;
using App_Model_Essence;
using App_Model_TestLogics;

namespace App_Model_Logics
{
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