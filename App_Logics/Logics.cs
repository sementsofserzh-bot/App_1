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
        public string AddTrainer(string fullName, string gender, int age)
        {
            BD_Trainer.Add(new Trainer { Age = age, Gender = gender, FullName = fullName } );
        }

      

        
    }
}