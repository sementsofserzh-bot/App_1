using System.Collections.Generic;
using App_Model_Essence;

namespace App_Model_TestLogics
{
    public interface ILogics
    {
        List<Trainer> BD_Trainer { get; set; }
        List<Athlete> BD_Athlete { get; set; }

        string AddTrainer(string fullName, Gendre gendre, int age, int workExperience);
        string AddAthlete(string fullName, Gendre gendre, int age, int height, int weight);

        bool RemoveTrainer(int id);
        bool RemoveAthlete(int id);

        Trainer? CheckTrainer(int id);
        Athlete? CheckAthlete(int id);

        Trainer? UpdateInfoTrainer(int id, string fullName, Gendre gendre, int age, int workExperience);
        Athlete? UpdateInfoAthlete(int id, string fullName, Gendre gendre, int age, int height, int weight);

        bool Registration(int idAthlete, int idTrainer);
        string PersonalTraining(Gendre gendre, int height, int weight, int age); //бизнес функция (тренировка)

        public List<Trainer> RateTrainers(); // рейтинг тренеров 
    }
}