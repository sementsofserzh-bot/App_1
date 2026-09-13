using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using App_Model_Essence;


namespace App_Model_TestLogics
{
    internal class TestLogics
    {
        public interface ILogics
        {
            public void Ilogics()
            {
                trainer = new Trainer();
                athlete = new Athlete();
            }
            Trainer trainer { get; set; }
            Athlete athlete { get; set; }
            public List<Trainer> BD_Trainer { get; set; }
            public List<Athlete> BD_Athlete { get; set; }

            public string AddTrainer(string FullName, string Gender, int Age, int Weight, int Height);

            public string AddAthlete(string FullName, string Gender, int Age, int Weight, int Height);

            public bool RemoveTrainer(int id);

            public bool RemoveAthlete(int id);

            public Trainer? checkTrainer(int id);

            public Athlete? checkAthlete(int id);

            public Trainer? UpdateInfoTrainer(int id, string FullName, string Gender, int Age, int Weight, int Height);

            public Athlete? UpdateInfoAthlete(int id, string FullName, string Gender, int Age, int Weight, int Height);

            public bool registration(int iIdAthlete, int IdTrainer);

            public string PersonalTraining(string gender);


        }
    }
}

