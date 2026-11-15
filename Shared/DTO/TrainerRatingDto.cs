using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    /// <summary>
    /// Содержит данные о позиции тренера в рейтинге и количестве его спортсменов.
    /// </summary>
    public class TrainerRatingDto
    {
        public int Position { get; set; }
        public TrainerDto Trainer { get; set; } = new();
        public int AthleteCount { get; set; }
    }
}
