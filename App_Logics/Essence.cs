namespace App_Model_Essence
{
    public class Athlete
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;

        public int Age { get; set; }

        public int TrainerId { get; set; }
    }
    public class Trainer
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;

        public int Age { get; set; }

        public List<int> AthleteIds { get; set; } = new();

    }
}
