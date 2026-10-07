using Contracts;
namespace App_Model
{
    public enum Gendre { М, Ж }

    public enum TrainingType { Ж_Силовая, М_Силовая, Ж_Выносливость, М_Выносливость, Ж_Гибкость, М_Гибкость }

    

    /// <summary>
    /// Представляет атлета/посетителя.
    /// Содержит личные данные, физические параметры и ссылку на тренера.
    /// </summary>
    public class Athlete : IDomainObject
    {

        public int Id { get; set; }

        public string FullName { get; set; }
        
        public Gendre Gendre { get; set; }
        public int Age { get; set; }

        public int Height { get; set; }

        public int Weight { get; set; }

        public TrainingType TrainingType { get; set; }

        public Trainer? trainer { get; set; }

        public TrainingType? TypePersonalTraining { get; set; }
    }

    /// <summary>
    /// Представляет спортивного тренера.
    /// Содержит личные данные, специализацию и список закреплённых атлетов.
    /// </summary>
    public class Trainer : IDomainObject
    {
        public int Id { get; set; }

        public string FullName { get; set; }
        public Gendre Gendre { get; set; }

        public TrainingType TrainingType { get; set; }

        public int Age { get; set; }
        public int WorkExperience { get; set; }
        public double Rating { get; set; }
        public List<Athlete> Athlete { get; set; } = new();

    }
}
