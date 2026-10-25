using Contracts;
using Essence;

namespace App_Model
{
    /// <summary>Рассчитывает и сохраняет рейтинг тренеров.</summary>
    public class TrainerRatingService : ITrainerRatingLogic
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITrainerRatingCalculator _calculator;

        /// <summary>Создаёт сервис рейтинга с указанным правилом расчёта.</summary>
        public TrainerRatingService(IUnitOfWork unitOfWork, ITrainerRatingCalculator calculator)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _calculator = calculator ?? throw new ArgumentNullException(nameof(calculator));
        }

        public List<Trainer> RateTrainers()
        {
            List<Trainer> trainers =
                _unitOfWork.Trainers.List().ToList();

            if (trainers.Count == 0)
            {
                return new List<Trainer>();
            }

            foreach (Trainer trainer in trainers)
            {
                trainer.Rating =
                    CalculateRating(trainer);

                _unitOfWork.Trainers.Update(trainer);
            }
            _unitOfWork.Save();

            return trainers
                .OrderByDescending(t => t.Rating)
                .ToList();
        }

        public double CalculateRating(Trainer trainer)
        {
            return _calculator.Calculate(trainer);
        }
    }
}
