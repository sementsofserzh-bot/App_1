namespace App_Model_Essence
{
    public enum Gendre { М, Ж }
    public class Athlete
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;
        
        public Gendre Gendre { get; set; }
        public int Age { get; set; }

        public int TrainerId { get; set; }
    }
    public class Trainer
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;
        public Gendre Gendre { get; set; }

        public int Age { get; set; }
        public int WorkExperience { get; set; }
        public int Rating { get; set; }
        public List<Athlete> AthleteIds { get; set; } = new();

    }
}
