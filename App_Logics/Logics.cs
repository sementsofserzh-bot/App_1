using Essence;

namespace App_Model
{
    /// <summary>Сохраняет общий API приложения и направляет вызовы профильным сервисам.</summary>
    public class Logics : ILogics, IConsoleUserLogic
    {
        private readonly ITrainerService _trainers;
        private readonly IAthleteService _athletes;
        private readonly IRegistrationLogic _registration;
        private readonly ITrainingPlanLogic _trainingPlans;
        private readonly ITrainerMatchingLogic _matching;
        private readonly ITrainerRatingLogic _rating;

        /// <summary>Собирает общий фасад из профильных сервисов бизнес-логики.</summary>
        public Logics(ITrainerService trainers, IAthleteService athletes,
            IRegistrationLogic registration, ITrainingPlanLogic trainingPlans,
            ITrainerMatchingLogic matching, ITrainerRatingLogic rating)
        {
            _trainers = trainers ?? throw new ArgumentNullException(nameof(trainers));
            _athletes = athletes ?? throw new ArgumentNullException(nameof(athletes));
            _registration = registration ?? throw new ArgumentNullException(nameof(registration));
            _trainingPlans = trainingPlans ?? throw new ArgumentNullException(nameof(trainingPlans));
            _matching = matching ?? throw new ArgumentNullException(nameof(matching));
            _rating = rating ?? throw new ArgumentNullException(nameof(rating));
        }

        public List<Trainer> BD_Trainer => _trainers.BD_Trainer;
        public List<Athlete> BD_Athlete => _athletes.BD_Athlete;

        public Trainer AddTrainer(string fullname, Gendre gendre, TrainingType trainingType,
            int age, int workExperience) =>
            _trainers.AddTrainer(fullname, gendre, trainingType, age, workExperience);

        public Athlete AddAthlete(string fullname, Gendre gendre, TrainingType trainingType,
            int age, int height, int weight) =>
            _athletes.AddAthlete(fullname, gendre, trainingType, age, height, weight);

        public bool? RemoveTrainer(int id) => _trainers.RemoveTrainer(id);
        public bool? RemoveAthlete(int id) => _athletes.RemoveAthlete(id);
        public Trainer? CheckTrainer(int id) => _trainers.CheckTrainer(id);
        public Athlete? CheckAthlete(int id) => _athletes.CheckAthlete(id);

        public Trainer? UpdateInfoTrainer(int id, string? fullname, Gendre? gendre,
            TrainingType? trainingType, int? age, int? workExperience,
            List<Athlete>? deleteathlete, List<Athlete>? addathlete) =>
            _trainers.UpdateInfoTrainer(id, fullname, gendre, trainingType, age,
                workExperience, deleteathlete, addathlete);

        public Athlete? UpdateInfoAthlete(int id, string? fullname, Gendre? gendre,
            int? age, int? height, int? weight, TrainingType? trainingType, Trainer? trainer) =>
            _athletes.UpdateInfoAthlete(id, fullname, gendre, age, height, weight,
                trainingType, trainer);

        public bool Registration(Trainer trainer, Athlete athlete) =>
            _registration.Registration(trainer, athlete);

        public string PersonalTraining(Athlete athlete) =>
            _trainingPlans.PersonalTraining(athlete);

        public List<Trainer> PersonalFilterTrainers(Athlete athlete) =>
            _matching.PersonalFilterTrainers(athlete);

        public List<Trainer> RateTrainers() => _rating.RateTrainers();
        public double CalculateRating(Trainer trainer) => _rating.CalculateRating(trainer);

        public int CalculateMatchPercentage(Trainer trainer, Athlete athlete) =>
            _matching.CalculateMatchPercentage(trainer, athlete);
    }
}
