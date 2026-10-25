using Contracts;
using Essence;

namespace App_Model
{
    /// <summary>Управляет данными спортсменов.</summary>
    public class AthleteService : IAthleteService
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>Создаёт сервис спортсменов с указанной единицей работы.</summary>
        public AthleteService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public List<Athlete> BD_Athlete => _unitOfWork.Athletes.List().ToList();

        public Athlete AddAthlete(
            string fullname,
            Gendre gendre,
            TrainingType trainingType,
            int age,
            int height,
            int weight)
        {
            if (string.IsNullOrWhiteSpace(fullname))
                throw new ArgumentException("Имя не может быть пустым");

            if (fullname.Any(char.IsDigit))
                throw new ArgumentException("Имя не может содержать цифры");

            if (age < 14 || age > 120)
                throw new ArgumentException("Некорректный возраст");

            if (height <= 0 || height > 300)
                throw new ArgumentException("Некорректный рост");

            if (weight <= 0 || weight > 1000)
                throw new ArgumentException("Некорректный вес");

            Athlete athlete = new Athlete
            {
                FullName = fullname,
                Gendre = gendre,
                TrainingType = trainingType,
                Age = age,
                Height = height,
                Weight = weight
            };

            _unitOfWork.Athletes.Add(athlete);
            _unitOfWork.Save();

            return athlete;
        }

        public bool? RemoveAthlete(int id)
        {
            Athlete? athlete = _unitOfWork.Athletes.ReadById(id);

            if (athlete == null)
                return false;

            _unitOfWork.Athletes.Delete(id);
            _unitOfWork.Save();

            return true;
        }

        public Athlete? CheckAthlete(int id)
        {
            return _unitOfWork.Athletes.ReadById(id);
        }

        public Athlete? UpdateInfoAthlete(
            int id,
            string? fullname,
            Gendre? gendre,
            int? age,
            int? height,
            int? weight,
            TrainingType? trainingType,
            Trainer? trainer)
        {
            Athlete? chosenAthlete =
                _unitOfWork.Athletes.ReadById(id);

            if (chosenAthlete == null)
                return null;

            int finalAge =
                age ?? chosenAthlete.Age;

            int finalHeight =
                height ?? chosenAthlete.Height;

            int finalWeight =
                weight ?? chosenAthlete.Weight;

            if (finalAge < 14 || finalAge > 120)
                throw new ArgumentException(
                    "Некорректный возраст");

            if (finalHeight <= 0 || finalHeight > 300)
                throw new ArgumentException(
                    "Некорректный рост");

            if (finalWeight <= 0 || finalWeight > 1000)
                throw new ArgumentException(
                    "Некорректный вес");

            if (fullname != null)
            {
                if (string.IsNullOrWhiteSpace(fullname) ||
                    fullname.Any(char.IsDigit))
                {
                    throw new ArgumentException(
                        "Некорректное ФИО");
                }

                chosenAthlete.FullName = fullname;
            }

            if (gendre.HasValue)
                chosenAthlete.Gendre = gendre.Value;

            if (trainingType.HasValue)
                chosenAthlete.TrainingType =
                    trainingType.Value;

            if (age.HasValue)
                chosenAthlete.Age = age.Value;

            if (height.HasValue)
                chosenAthlete.Height = height.Value;

            if (weight.HasValue)
                chosenAthlete.Weight = weight.Value;

            if (trainer != null)
            {
                Trainer? trainerInDb =
                    _unitOfWork.Trainers.ReadById(trainer.Id);

                if (trainerInDb != null)
                {
                    chosenAthlete.trainer = trainerInDb;
                }
            }

            _unitOfWork.Athletes.Update(chosenAthlete);
            _unitOfWork.Save();
            return chosenAthlete;
        }
    }
}
