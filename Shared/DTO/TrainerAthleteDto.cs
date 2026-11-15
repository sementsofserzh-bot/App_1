using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    /// <summary>
    /// Содержит идентификаторы тренера и спортсмена для управления их связью.
    /// </summary>
    public class TrainerAthleteDto
    {
        public int TrainerId { get; set; }

        public int AthleteId { get; set; }
    }
}
