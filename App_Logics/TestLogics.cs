using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace App_Model
{
    internal class TestLogics
    {
        public interface ILogics
        {
            public List<T1> BD_Trainer { get; set; }
            public List<T2> BD_Athlete { get; set; }

            public void AddTrainer();
            public void AddAthlete();

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

