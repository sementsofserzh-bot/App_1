namespace App_Model_Essence
{
    public enum Gendre { М, Ж }

    public enum TrainingType { Ж_Силовая, М_Силовая, Ж_Выносливость, М_Выносливость, Ж_Гибкость, М_Гибкость }

    public class Athlete
    {
        public int Id { get; set; }

        public string FullName { get; set; }
        
        public Gendre Gendre { get; set; }
        public int Age { get; set; }

        public int Height { get; set; }

        public int Weight { get; set; }

        public TrainingType TrainingType { get; set; }

        public Trainer? trainer { get; set; }

        public string? TypePersonalTraining { get; set; }
    }
    public class Trainer
    {
        public int Id { get; set; }

        public string FullName { get; set; }
        public Gendre Gendre { get; set; }

        public TrainingType TrainingType { get; set; }

        public int Age { get; set; }
        public int WorkExperience { get; set; }
        public List<Athlete> Athlete { get; set; } = new();

    }
}
