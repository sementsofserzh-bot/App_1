using Contracts;
using Essence;

namespace App_Model
{
    /// <summary>Управляет данными тренеров и их связями со спортсменами.</summary>
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>Создаёт сервис тренеров с указанной единицей работы.</summary>
        public TrainerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public List<Trainer> BD_Trainer => _unitOfWork.Trainers.List().ToList();

        public Trainer AddTrainer(
            string fullname,
            Gendre gendre,
            TrainingType trainingType,
            int age,
            int workExperience)
        {
            if (string.IsNullOrWhiteSpace(fullname))
                throw new ArgumentException("Имя не может быть пустым");

            if (fullname.Any(char.IsDigit))
                throw new ArgumentException("Имя не может содержать цифры");

            if (age < 18 || age > 120)
                throw new ArgumentException("Некорректный возраст");

            if (workExperience < 0 || workExperience > age - 18)
                throw new ArgumentException("Некорректный опыт работы");

            Trainer trainer = new Trainer
            {
                FullName = fullname,
                Gendre = gendre,
                TrainingType = trainingType,
                Age = age,
                WorkExperience = workExperience
            };

            _unitOfWork.Trainers.Add(trainer);
            _unitOfWork.Save();

            return trainer;
        }

        public bool? RemoveTrainer(int id)
        {
            Trainer? trainer = _unitOfWork.Trainers.ReadById(id);

            if (trainer == null)
                return false;

            List<Athlete> athletes = _unitOfWork.Athletes
                .List()
                .Where(a =>
                    a.trainer != null &&
                    a.trainer.Id == id)
                .ToList();

            foreach (Athlete athlete in athletes)
            {
                athlete.trainer = null;
                _unitOfWork.Athletes.Update(athlete);
            }

            _unitOfWork.Trainers.Delete(id);
            _unitOfWork.Save();

            return true;
        }

        public Trainer? CheckTrainer(int id)
        {
            return _unitOfWork.Trainers.ReadById(id);
        }

        public Trainer? UpdateInfoTrainer(
            int id,
            string? fullname,
            Gendre? gendre,
            TrainingType? trainingType,
            int? age,
            int? workExperience,
            List<Athlete>? deleteathlete,
            List<Athlete>? addathlete)
        {
            Trainer? chosenTrainer =
                _unitOfWork.Trainers.ReadById(id);

            if (chosenTrainer == null)
                return null;

            int finalAge =
                age ?? chosenTrainer.Age;

            int finalExperience =
                workExperience ?? chosenTrainer.WorkExperience;

            if (finalAge < 18 || finalAge > 120)
                throw new ArgumentException("Некорректный возраст");

            if (finalExperience < 0 ||
                finalExperience > finalAge - 18)
            {
                throw new ArgumentException(
                    "Некорректный опыт работы");
            }

            if (fullname != null)
            {
                if (string.IsNullOrWhiteSpace(fullname) ||
                    fullname.Any(char.IsDigit))
                {
                    throw new ArgumentException(
                        "Некорректное ФИО");
                }

                chosenTrainer.FullName = fullname;
            }

            if (gendre.HasValue)
                chosenTrainer.Gendre = gendre.Value;

            if (trainingType.HasValue)
                chosenTrainer.TrainingType = trainingType.Value;

            if (age.HasValue)
                chosenTrainer.Age = age.Value;

            if (workExperience.HasValue)
                chosenTrainer.WorkExperience =
                    workExperience.Value;

            /*
             * Сначала сохраняем обычные поля тренера.
             */
            _unitOfWork.Trainers.Update(chosenTrainer);

            /*
             * Открепление атлетов.
             */
            if (deleteathlete != null)
            {
                foreach (Athlete athlete in deleteathlete)
                {
                    Athlete? athleteInDb =
                        _unitOfWork.Athletes.ReadById(athlete.Id);

                    if (athleteInDb == null)
                        continue;

                    if (athleteInDb.trainer != null &&
                        athleteInDb.trainer.Id == chosenTrainer.Id)
                    {
                        athleteInDb.trainer = null;
                        _unitOfWork.Athletes.Update(athleteInDb);
                    }
                }
            }

            /*
             * Закрепление атлетов.
             */
            if (addathlete != null)
            {
                foreach (Athlete athlete in addathlete)
                {
                    Athlete? athleteInDb =
                        _unitOfWork.Athletes.ReadById(athlete.Id);

                    if (athleteInDb == null)
                        continue;

                    athleteInDb.trainer = chosenTrainer;

                    _unitOfWork.Athletes.Update(athleteInDb);
                }
            }

            _unitOfWork.Save();
            return _unitOfWork.Trainers.ReadById(id);
        }
    }
}
