using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    /// <summary>
    /// Содержит данные для изменения информации о существующем тренере.
    /// </summary>
    public class TrainerEditDto
    {
        public int Id { get; set; }

        public string? FullName { get; set; }
        public int? Gender { get; set; }
        public int? TrainingType { get; set; }

        public int? Age { get; set; }
        public int? WorkExperience { get; set; }
    }
}
