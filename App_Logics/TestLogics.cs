using System;
using System.Collections.Generic;
using System.Text;

namespace App_Model
{
    internal class TestLogics
    {
        public interface ILogics
        {
            public void Ilogics()
            {
                trainer= new Trainer();
                athlete = new Athlete();
            }
            Trainer trainer { get; set; }
            Athlete athlete { get; set; }
            public List<Trainer> BD_Trainer { get; set; }
            public List<Athlete> BD_Athlete { get; set; }

            public void AddTrainer();
            public void AddAthlete();

            public void RemoveTrainer();

            public void RemoveAthlete();

            public void checkTrainer();

            public void checkAthlete();

            public void UpdateInfoTrainer();

            public void UpdateInfoAthlete();

            public void registration();

            public void PersonalTraining();


        }
    }
}
    