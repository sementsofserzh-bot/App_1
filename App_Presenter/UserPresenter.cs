using App_Model;
using Essence;
using Shared;

namespace App_Presenter
{
    public class UserPresenter
    {
        private readonly IUserView _view;
        private readonly IUserLogic _logic;

        public UserPresenter(IUserView view, IUserLogic logic)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _logic = logic ?? throw new ArgumentNullException(nameof(logic));

            _view.AddAthleteRequested += OnAddAthlete;
            _view.RegisterAthleteRequested += OnRegisterAthlete;
            _view.RefreshRatingRequested += OnRefreshRating;
            _view.PersonalTrainingRequested += OnPersonalTraining;
            _view.FilterTrainersRequested += OnFilterTrainers;
            _view.SignUpFromFilterRequested += OnSignUpFromFilter;
        }

        private void OnAddAthlete(object? sender, EventArgs e)
        {
            try
            {
                AthleteInputDto dto = _view.NewAthlete;

                if (!Enum.IsDefined(typeof(Gendre), dto.Gender))
                    throw new ArgumentException("Некорректный пол");

                if (!Enum.IsDefined(typeof(TrainingType), dto.TrainingType))
                    throw new ArgumentException("Некорректный тип тренировки");

                _logic.AddAthlete(
                    dto.FullName,
                    (Gendre)dto.Gender,
                    (TrainingType)dto.TrainingType,
                    dto.Age,
                    dto.Height,
                    dto.Weight
                );

                _view.ShowMessage("Спортсмен успешно добавлен");

                RefreshAthletes();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
        private void OnRegisterAthlete(object? sender, EventArgs e)
        {
            try
            {
                int athleteId = _view.SelectedRegistrationAthleteId;
                int trainerId = _view.SelectedRegistrationTrainerId;

                if (athleteId <= 0 || trainerId <= 0)
                    throw new ArgumentException(
                        "Выберите спортсмена и тренера");

                Athlete? athlete = _logic.BD_Athlete
                    .FirstOrDefault(a => a.Id == athleteId);

                Trainer? trainer = _logic.BD_Trainer
                    .FirstOrDefault(t => t.Id == trainerId);

                if (athlete == null)
                    throw new ArgumentException("Спортсмен не найден");

                if (trainer == null)
                    throw new ArgumentException("Тренер не найден");

                bool result = _logic.Registration(trainer, athlete);

                if (!result)
                {
                    _view.ShowError("Не удалось зарегистрировать спортсмена");
                    return;
                }

                _view.ShowMessage("Спортсмен успешно зарегистрирован");

                RefreshAthletes();
                RefreshTrainers();
                RefreshRating();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
        private void RefreshAthletes()
        {
            var athletes = _logic.BD_Athlete
                .Select(a => new AthleteDto
                {
                    Id = a.Id,
                    FullName = a.FullName,
                    Gender = (int)a.Gendre,
                    Age = a.Age,
                    Height = a.Height,
                    Weight = a.Weight,
                    TrainingType = (int)a.TrainingType,

                    TrainerId = a.trainer?.Id,
                    TrainerName = a.trainer?.FullName ?? string.Empty,

                    PersonalTrainingType =
                        a.TypePersonalTraining.HasValue
                            ? (int)a.TypePersonalTraining.Value
                            : null
                })
                .ToList();

            _view.ShowAthletes(athletes);
        }

        private void OnRefreshRating(object? sender, EventArgs e)
        {
            try
            {
                RefreshRating();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void RefreshRating()
        {
            List<Trainer> trainers = _logic.RateTrainers();

            var rating = trainers
                .Select((trainer, index) => new TrainerRatingDto
                {
                    Position = index + 1,

                    Trainer = new TrainerDto
                    {
                        Id = trainer.Id,
                        FullName = trainer.FullName,
                        Gender = (int)trainer.Gendre,
                        TrainingType = (int)trainer.TrainingType,
                        Age = trainer.Age,
                        WorkExperience = trainer.WorkExperience,
                        Rating = trainer.Rating
                    },

                    AthleteCount = trainer.Athlete?.Count ?? 0
                })
                .ToList();

            _view.ShowRating(rating);
        }
        private void OnPersonalTraining(object? sender, EventArgs e)
        {
            try
            {
                int athleteId = _view.SelectedPersonalAthleteId;

                if (athleteId <= 0)
                    throw new ArgumentException("Выберите спортсмена");

                Athlete? athlete = _logic.BD_Athlete
                    .FirstOrDefault(a => a.Id == athleteId);

                if (athlete == null)
                    throw new ArgumentException("Спортсмен не найден");

                string result = _logic.PersonalTraining(athlete);

                _view.ShowPersonalTraining(result);

                RefreshAthletes();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
        private void OnFilterTrainers(object? sender, EventArgs e)
        {
            try
            {
                RefreshFilteredTrainers();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
        private void RefreshFilteredTrainers()
        {
            int athleteId = _view.SelectedFilterAthleteId;

            if (athleteId <= 0)
                throw new ArgumentException("Выберите спортсмена");

            Athlete? athlete = _logic.BD_Athlete
                .FirstOrDefault(a => a.Id == athleteId);

            if (athlete == null)
                throw new ArgumentException("Спортсмен не найден");

            List<Trainer> trainers =
                _logic.PersonalFilterTrainers(athlete);

            var matches = trainers
                .Select(trainer => new TrainerMatchDto
                {
                    Trainer = new TrainerDto
                    {
                        Id = trainer.Id,
                        FullName = trainer.FullName,
                        Gender = (int)trainer.Gendre,
                        TrainingType = (int)trainer.TrainingType,
                        Age = trainer.Age,
                        WorkExperience = trainer.WorkExperience,
                        Rating = trainer.Rating
                    },

                    MatchPercentage =
                        _logic.CalculateMatchPercentage(trainer, athlete),

                    AthleteCount = trainer.Athlete?.Count ?? 0
                })
                .ToList();

            _view.ShowFilteredTrainers(matches);

            RefreshAthletes();
        }
        private void RefreshTrainers()
        {
            var trainers = _logic.BD_Trainer
                .Select(t => new TrainerDto
                {
                    Id = t.Id,
                    FullName = t.FullName,
                    Gender = (int)t.Gendre,
                    TrainingType = (int)t.TrainingType,
                    Age = t.Age,
                    WorkExperience = t.WorkExperience,
                    Rating = t.Rating
                })
                .ToList();

            _view.ShowTrainers(trainers);
        }
        private void OnSignUpFromFilter(object? sender, EventArgs e)
        {
            try
            {
                int athleteId = _view.SelectedFilterAthleteId;
                int trainerId = _view.SelectedFilteredTrainerId;

                if (athleteId <= 0)
                    throw new ArgumentException("Выберите спортсмена");

                if (trainerId <= 0)
                    throw new ArgumentException("Выберите тренера из таблицы");

                Athlete? athlete = _logic.BD_Athlete
                    .FirstOrDefault(a => a.Id == athleteId);

                Trainer? trainer = _logic.CheckTrainer(trainerId);

                if (athlete == null)
                    throw new ArgumentException("Спортсмен не найден");

                if (trainer == null)
                    throw new ArgumentException("Тренер не найден");

                if (!_logic.Registration(trainer, athlete))
                {
                    _view.ShowError(
                        "Не удалось записаться. Возможно, спортсмен уже закреплён за этим тренером.");
                    return;
                }

                int matchPercent =
                    _logic.CalculateMatchPercentage(trainer, athlete);

                RefreshAthletes();
                RefreshTrainers();
                RefreshRating();
                RefreshFilteredTrainers();

                _view.ShowMessage(
                    $"Спортсмен {athlete.FullName} успешно записан к тренеру {trainer.FullName}!\n" +
                    $"Совместимость: {matchPercent}%");
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
        public void Initialize()
        {
            try
            {
                RefreshAthletes();
                RefreshTrainers();
                RefreshRating();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}