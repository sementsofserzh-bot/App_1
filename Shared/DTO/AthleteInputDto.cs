using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public class AthleteInputDto
    {
        public string FullName { get; set; } = string.Empty;

        public int Gender { get; set; }

        public int TrainingType { get; set; }

        public int Age { get; set; }

        public int Height { get; set; }

        public int Weight { get; set; }
    }
}
