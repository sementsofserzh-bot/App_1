using Contracts;
using Essence;

namespace App_Model
{
    /// <summary>Добавляет начальные данные при первом запуске.</summary>
    public class InitialDataSeeder
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITrainerEditLogic _trainers;
        private readonly IAthleteCreateLogic _athletes;
        private readonly IRegistrationLogic _registration;

        /// <summary>Создаёт компонент начального заполнения базы данных.</summary>
        public InitialDataSeeder(IUnitOfWork unitOfWork, ITrainerEditLogic trainers,
            IAthleteCreateLogic athletes, IRegistrationLogic registration)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _trainers = trainers ?? throw new ArgumentNullException(nameof(trainers));
            _athletes = athletes ?? throw new ArgumentNullException(nameof(athletes));
            _registration = registration ?? throw new ArgumentNullException(nameof(registration));
        }

        /// <summary>Добавляет примеры тренеров и спортсменов, если обе таблицы пусты.</summary>
        public void SeedIfEmpty()
        {
            if (!_unitOfWork.Trainers.List().Any() && !_unitOfWork.Athletes.List().Any())
                SeedInitialData();
        }

        /// <summary>Создаёт исходных тренеров, спортсменов и связи между ними.</summary>
        private void SeedInitialData()
        {
            var t1 = _trainers.AddTrainer(
                "Жежбекович Кайран Альбертович",
                Gendre.М,
                TrainingType.М_Силовая,
                38,
                12);

            var t2 = _trainers.AddTrainer(
                "Галькова Галина Семёновна",
                Gendre.Ж,
                TrainingType.Ж_Выносливость,
                29,
                6);

            var t3 = _trainers.AddTrainer(
                "Степанова Виктория Олеговна",
                Gendre.М,
                TrainingType.М_Гибкость,
                45,
                18);

            var t4 = _trainers.AddTrainer(
                "Трифонова Алена Александровна",
                Gendre.Ж,
                TrainingType.Ж_Гибкость,
                40,
                18);

            _trainers.AddTrainer(
                "Семенцов Сергей Витальевич",
                Gendre.М,
                TrainingType.Ж_Гибкость,
                50,
                20);

            var a1 = _athletes.AddAthlete(
                "Корецкий Глеб Александрович",
                Gendre.М,
                TrainingType.М_Силовая,
                16,
                185,
                63);

            var a2 = _athletes.AddAthlete(
                "Зайцева Алина Максимовна",
                Gendre.Ж,
                TrainingType.Ж_Силовая,
                20,
                165,
                55);

            var a3 = _athletes.AddAthlete(
                "Смаев Геннадий Викторович",
                Gendre.М,
                TrainingType.М_Гибкость,
                32,
                178,
                95);

            var a4 = _athletes.AddAthlete(
                "Павлова Екатерина Дмитриевна",
                Gendre.Ж,
                TrainingType.Ж_Выносливость,
                24,
                170,
                58);

            var a5 = _athletes.AddAthlete(
                "Федоров Егор Романович",
                Gendre.М,
                TrainingType.М_Выносливость,
                15,
                172,
                60);

            var a6 = _athletes.AddAthlete(
                "Сир Люлю Кебаб",
                Gendre.Ж,
                TrainingType.Ж_Гибкость,
                42,
                162,
                68);

            var a7 = _athletes.AddAthlete(
                "Ремнев Павел Павлович",
                Gendre.М,
                TrainingType.М_Гибкость,
                55,
                180,
                85);

            var a8 = _athletes.AddAthlete(
                "Козлова София Евгеньевна",
                Gendre.Ж,
                TrainingType.Ж_Выносливость,
                22,
                168,
                54);

            _registration.Registration(t1, a1);
            _registration.Registration(t1, a2);

            _registration.Registration(t2, a4);
            _registration.Registration(t2, a6);
            _registration.Registration(t2, a8);

            _registration.Registration(t3, a3);
            _registration.Registration(t3, a7);

            _registration.Registration(t4, a5);
        }
    }
}
