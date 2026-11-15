using App_Model;
using Essence;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace App_Presenter
{
    /// <summary>
    /// Управляет взаимодействием между представлением сотрудника
    /// и бизнес-логикой приложения.
    /// </summary>
    public class EmployeePresenter
    {
        private readonly IEmployeeView _view;
        private readonly IEmployeeLogic _logic;
        /// <summary>
        /// Создаёт Presenter и подписывает его на события представления сотрудника.
        /// </summary>
        /// <param name="view">Представление сотрудника.</param>
        /// <param name="logic">Интерфейс бизнес-логики приложения.</param>
        public EmployeePresenter(IEmployeeView view, IEmployeeLogic logic)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _logic = logic ?? throw new ArgumentNullException(nameof(logic));
            _view.AddTrainerRequested += OnAddTrainer;
            _view.AddAthleteRequested += OnAddAthlete;
            _view.RefreshDataRequested += OnRefreshData;
            _view.UpdateTrainerRequested += OnUpdateTrainer;
            _view.UpdateAthleteRequested += OnUpdateAthlete;
            _view.DeleteTrainerRequested += OnDeleteTrainer;
            _view.DeleteAthleteRequested += OnDeleteAthlete;
            _view.AttachAthleteRequested += OnAttachAthlete;
            _view.DetachAthleteRequested += OnDetachAthlete;
        }

        private void RefreshData()
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
                    Rating = t.Rating,

                    Athletes = t.Athlete?
                        .Select(a => new AthleteSummaryDto
                        {
                            Id = a.Id,
                            FullName = a.FullName
                        })
                        .ToList() ?? new List<AthleteSummaryDto>()
                })
                .ToList();

            var athletes = _logic.BD_Athlete
                .Select(a => new AthleteDto
                {
                    Id = a.Id,
                    FullName = a.FullName,
                    Gender = (int)a.Gendre,
                    TrainingType = (int)a.TrainingType,
                    Age = a.Age,
                    Height = a.Height,
                    Weight = a.Weight,
                    TrainerId = a.trainer?.Id,
                    TrainerName = a.trainer?.FullName ?? string.Empty,
                    PersonalTrainingType =
                        a.TypePersonalTraining.HasValue
                            ? (int)a.TypePersonalTraining.Value
                            : null
                })
                .ToList();

            _view.ShowTrainers(trainers);
            _view.ShowAthletes(athletes);
        }

        private void OnRefreshData(object? sender, EventArgs e)
        {
            try
            {
                RefreshData();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OnAddTrainer(object? sender, EventArgs e)
        {
            try
            {
                TrainerInputDto dto = _view.NewTrainer;

                if (!Enum.IsDefined(typeof(Gendre), dto.Gender))
                    throw new ArgumentException("Некорректный пол");

                if (!Enum.IsDefined(
                    typeof(TrainingType), dto.TrainingType))
                    throw new ArgumentException(
                        "Некорректный тип тренировки");

                _logic.AddTrainer(
                    dto.FullName,
                    (Gendre)dto.Gender,
                    (TrainingType)dto.TrainingType,
                    dto.Age,
                    dto.WorkExperience
                );

                RefreshData();

                _view.ShowMessage("Тренер успешно добавлен");
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OnAddAthlete(object? sender, EventArgs e)
        {
            try
            {
                AthleteInputDto dto = _view.NewAthlete;

                if (!Enum.IsDefined(typeof(Gendre), dto.Gender))
                    throw new ArgumentException("Некорректный пол");

                if (!Enum.IsDefined(
                    typeof(TrainingType), dto.TrainingType))
                    throw new ArgumentException(
                        "Некорректный тип тренировки");

                _logic.AddAthlete(
                    dto.FullName,
                    (Gendre)dto.Gender,
                    (TrainingType)dto.TrainingType,
                    dto.Age,
                    dto.Height,
                    dto.Weight
                );

                RefreshData();

                _view.ShowMessage("Спортсмен успешно добавлен");
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OnUpdateTrainer(object? sender, EventArgs e)
        {
            try
            {
                TrainerEditDto dto = _view.EditedTrainer;

                if (dto.Id <= 0)
                    throw new ArgumentException("Выберите тренера");

                if (dto.Gender.HasValue &&
                    !Enum.IsDefined(typeof(Gendre), dto.Gender.Value))
                    throw new ArgumentException("Некорректный пол");

                if (dto.TrainingType.HasValue &&
                    !Enum.IsDefined(typeof(TrainingType), dto.TrainingType.Value))
                    throw new ArgumentException("Некорректный тип тренировки");

                Trainer? result = _logic.UpdateInfoTrainer(
                    dto.Id,
                    dto.FullName,
                    dto.Gender.HasValue ? (Gendre)dto.Gender.Value : null,
                    dto.TrainingType.HasValue
                        ? (TrainingType)dto.TrainingType.Value : null,
                    dto.Age,
                    dto.WorkExperience,
                    null,
                    null
                );

                if (result == null)
                {
                    _view.ShowError("Тренер не найден");
                    return;
                }

                RefreshData();

                _view.ShowMessage("Данные тренера успешно обновлены");
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OnUpdateAthlete(object? sender, EventArgs e)
        {
            try
            {
                AthleteEditDto dto = _view.EditedAthlete;

                if (dto.Id <= 0)
                    throw new ArgumentException("Выберите спортсмена");

                if (dto.Gender.HasValue &&
                    !Enum.IsDefined(typeof(Gendre), dto.Gender.Value))
                    throw new ArgumentException("Некорректный пол");

                if (dto.TrainingType.HasValue &&
                    !Enum.IsDefined(typeof(TrainingType), dto.TrainingType.Value))
                    throw new ArgumentException("Некорректный тип тренировки");

                Trainer? trainer = null;

                if (dto.TrainerId.HasValue)
                {
                    if (dto.TrainerId.Value <= 0)
                        throw new ArgumentException("Некорректный ID тренера");

                    trainer = _logic.CheckTrainer(dto.TrainerId.Value);

                    if (trainer == null)
                        throw new ArgumentException("Тренер не найден");
                }

                Athlete? result = _logic.UpdateInfoAthlete(
                    dto.Id,
                    dto.FullName,
                    dto.Gender.HasValue ? (Gendre)dto.Gender.Value : null,
                    dto.Age,
                    dto.Height,
                    dto.Weight,
                    dto.TrainingType.HasValue
                        ? (TrainingType)dto.TrainingType.Value : null,
                    trainer
                );

                if (result == null)
                {
                    _view.ShowError("Спортсмен не найден");
                    return;
                }

                RefreshData();

                _view.ShowMessage("Данные спортсмена успешно обновлены");
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OnDeleteTrainer(object? sender, EventArgs e)
        {
            try
            {
                int trainerId = _view.SelectedTrainerId;

                if (trainerId <= 0)
                    throw new ArgumentException("Выберите тренера");

                bool? result = _logic.RemoveTrainer(trainerId);

                if (result != true)
                {
                    _view.ShowError("Тренер не найден");
                    return;
                }

                RefreshData();

                _view.ShowMessage("Тренер успешно удалён");
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OnDeleteAthlete(object? sender, EventArgs e)
        {
            try
            {
                int athleteId = _view.SelectedAthleteId;

                if (athleteId <= 0)
                    throw new ArgumentException("Выберите спортсмена");

                bool? result = _logic.RemoveAthlete(athleteId);

                if (result != true)
                {
                    _view.ShowError("Спортсмен не найден");
                    return;
                }

                RefreshData();

                _view.ShowMessage("Спортсмен успешно удалён");
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OnAttachAthlete(object? sender, EventArgs e)
        {
            try
            {
                TrainerAthleteDto dto = _view.SelectedTrainerAthlete;

                if (dto.TrainerId <= 0 || dto.AthleteId <= 0)
                    throw new ArgumentException(
                        "Выберите тренера и спортсмена");

                Trainer? trainer = _logic.CheckTrainer(dto.TrainerId);
                Athlete? athlete = _logic.CheckAthlete(dto.AthleteId);

                if (trainer == null || athlete == null)
                    throw new ArgumentException(
                        "Тренер или спортсмен не найден");

                bool result = _logic.Registration(trainer, athlete);

                if (!result)
                {
                    _view.ShowError(
                        "Не удалось прикрепить спортсмена");
                    return;
                }

                RefreshData();

                _view.ShowMessage("Спортсмен успешно прикреплён");
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OnDetachAthlete(object? sender, EventArgs e)
        {
            try
            {
                TrainerAthleteDto dto = _view.SelectedTrainerAthlete;

                if (dto.TrainerId <= 0 || dto.AthleteId <= 0)
                    throw new ArgumentException(
                        "Выберите тренера и спортсмена");

                Trainer? trainer = _logic.CheckTrainer(dto.TrainerId);
                Athlete? athlete = _logic.CheckAthlete(dto.AthleteId);

                if (trainer == null || athlete == null)
                    throw new ArgumentException(
                        "Тренер или спортсмен не найден");

                if (athlete.trainer?.Id != trainer.Id)
                    throw new ArgumentException(
                        "Спортсмен не закреплён за этим тренером");

                var deleteList = new List<Athlete> { athlete };

                Trainer? result = _logic.UpdateInfoTrainer(
                    trainer.Id,
                    null,
                    null,
                    null,
                    null,
                    null,
                    deleteList,
                    null
                );

                if (result == null)
                {
                    _view.ShowError("Не удалось открепить спортсмена");
                    return;
                }

                RefreshData();

                _view.ShowMessage("Спортсмен успешно откреплён");
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
        /// <summary>
        /// Инициализирует представление, загружая данные о тренерах и спортсменах.
        /// </summary>
        public void Initialize()
        {
            try
            {
                RefreshData();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
