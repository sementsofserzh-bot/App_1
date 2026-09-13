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
    }
}