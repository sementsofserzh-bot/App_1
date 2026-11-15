using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    /// <summary>
    /// Содержит данные спортсмена для передачи между слоями приложения.
    /// </summary>
    public class AthleteDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public int Gender { get; set; }
        public int Age { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public int TrainingType { get; set; }
        public int? TrainerId { get; set; }
        public string TrainerName { get; set; } = string.Empty;
        public int? PersonalTrainingType { get; set; }
    }
}
