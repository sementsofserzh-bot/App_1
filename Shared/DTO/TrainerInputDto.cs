using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public class TrainerInputDto
    {
        public string FullName { get; set; } = string.Empty;
        public int Gender { get; set; }
        public int TrainingType { get; set; }
        public int Age { get; set; }
        public int WorkExperience { get; set; }
    }
}
