using System;
using System.Collections.Generic;
using System.Text;

namespace App_Model
{
    internal class TestLogics
    {
        public interface ILogics<T1, T2>
        {
            public List<T1> BD_Trainer { get; set; }
            public List<T2> BD_Athlete { get; set; }
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
    