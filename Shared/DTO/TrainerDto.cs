using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    /// <summary>
    /// Содержит данные тренера для передачи между слоями приложения,
    /// включая информацию о закреплённых спортсменах.
    /// </summary>
    public class TrainerDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public int Gender { get; set; }
        public int TrainingType { get; set; }
        public int Age { get; set; }
        public int WorkExperience { get; set; }
        public double Rating { get; set; }
        public List<AthleteSummaryDto> Athletes { get; set; } = new();
    }
}
