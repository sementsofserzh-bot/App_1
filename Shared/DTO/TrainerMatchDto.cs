using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public class TrainerMatchDto
    {
        public TrainerDto Trainer { get; set; } = new();

        public int MatchPercentage { get; set; }

        public int AthleteCount { get; set; }
    }
}
