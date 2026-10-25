using Contracts;
using Essence;

namespace App_Model
{
    /// <summary>Подбирает тренеров для спортсмена.</summary>
    public class TrainerMatchingService : ITrainerMatchingLogic
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITrainingPlanLogic _trainingPlans;
        private readonly ITrainerMatchCalculator _calculator;

        /// <summary>Создаёт сервис подбора с доступом к тренерам, программам и правилу оценки.</summary>
        public TrainerMatchingService(IUnitOfWork unitOfWork, ITrainingPlanLogic trainingPlans,
            ITrainerMatchCalculator calculator)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _trainingPlans = trainingPlans ?? throw new ArgumentNullException(nameof(trainingPlans));
            _calculator = calculator ?? throw new ArgumentNullException(nameof(calculator));
        }

        public List<Trainer> PersonalFilterTrainers(
            Athlete athlete)
        {
            if (athlete == null)
                return new List<Trainer>();

            if (athlete.TypePersonalTraining == null)
            {
                _trainingPlans.PersonalTraining(athlete);
            }

            return _unitOfWork.Trainers
                .List()
                .OrderByDescending(
                    t => CalculateMatchPercentage(t, athlete))
                .ToList();
        }

        public int CalculateMatchPercentage(
            Trainer trainer,
            Athlete athlete)
        {
            if (trainer == null ||
                athlete == null)
            {
                return 0;
            }

            if (athlete.TypePersonalTraining == null)
            {
                _trainingPlans.PersonalTraining(athlete);
            }
            return _calculator.Calculate(trainer, athlete);
        }
    }
}
