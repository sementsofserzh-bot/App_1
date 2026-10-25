using Contracts;
using Essence;

namespace App_Model
{
    /// <summary>Закрепляет спортсменов за тренерами.</summary>
    public class RegistrationService : IRegistrationLogic
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>Создаёт сервис записи спортсменов к тренерам.</summary>
        public RegistrationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public bool Registration(
            Trainer trainer,
            Athlete athlete)
        {
            if (trainer == null || athlete == null)
                return false;

            Trainer? trainerInDb =
                _unitOfWork.Trainers.ReadById(trainer.Id);

            Athlete? athleteInDb =
                _unitOfWork.Athletes.ReadById(athlete.Id);

            if (trainerInDb == null ||
                athleteInDb == null)
            {
                return false;
            }

            if (athleteInDb.trainer != null &&
                athleteInDb.trainer.Id == trainerInDb.Id)
            {
                return false;
            }

            athleteInDb.trainer = trainerInDb;

            _unitOfWork.Athletes.Update(athleteInDb);

            /*
             * Также меняем переданный объект,
             * чтобы старый UI сразу увидел изменение.
             */
            athlete.trainer = trainerInDb;

            if (!trainer.Athlete.Any(
                a => a.Id == athlete.Id))
            {
                trainer.Athlete.Add(athlete);
            }
            _unitOfWork.Save();
            return true;
        }
    }
}
