using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    /// <summary>
    /// Содержит информацию о результатах подбора тренера для спортсмена.
    /// </summary>
    public class TrainerMatchDto
    {
        public TrainerDto Trainer { get; set; } = new();

        public int MatchPercentage { get; set; }

        public int AthleteCount { get; set; }
    }
}
