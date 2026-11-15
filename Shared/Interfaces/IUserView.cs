using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    /// <summary>
    /// Определяет контракт пользовательского представления.
    /// Предоставляет Presenter события пользовательских действий,
    /// данные для выполнения операций и методы отображения результатов.
    /// </summary>
    public interface IUserView : IView
    {
        /// <summary>
        /// Возникает при запросе добавления нового спортсмена.
        /// </summary>
        event EventHandler AddAthleteRequested;
        /// <summary>
        /// Возникает при запросе обновления рейтинга тренеров.
        /// </summary>
        event EventHandler RefreshRatingRequested;
        /// <summary>
        /// Возникает при запросе регистрации спортсмена у тренера.
        /// </summary>
        event EventHandler RegisterAthleteRequested;
        /// <summary>
        /// Возникает при запросе получения персональной программы тренировок.
        /// </summary>
        event EventHandler PersonalTrainingRequested;
        /// <summary>
        /// Возникает при запросе подбора подходящих тренеров для спортсмена.
        /// </summary>
        event EventHandler FilterTrainersRequested;
        /// <summary>
        /// Возникает при запросе записи спортсмена к тренеру
        /// из результатов подбора.
        /// </summary>
        event EventHandler SignUpFromFilterRequested;
        /// <summary>
        /// Возникает при запросе обновления списка спортсменов.
        /// </summary>
        event EventHandler RefreshAthletesRequested;
        /// <summary>
        /// Возникает при запросе обновления списка тренеров.
        /// </summary>
        event EventHandler RefreshTrainersRequested;
        /// <summary>
        /// Возвращает данные нового спортсмена, введённые пользователем.
        /// </summary>
        AthleteInputDto NewAthlete { get; }
        /// <summary>
        /// Возвращает идентификатор выбранного спортсмена для регистрации.
        /// </summary>
        int SelectedRegistrationAthleteId { get; }
        /// <summary>
        /// Возвращает идентификатор выбранного тренера для регистрации.
        /// </summary>
        int SelectedRegistrationTrainerId { get; }
        /// <summary>
        /// Возвращает идентификатор спортсмена для получения
        /// персональной программы тренировок.
        /// </summary>
        int SelectedPersonalAthleteId { get; }
        /// <summary>
        /// Возвращает идентификатор спортсмена для подбора тренеров.
        /// </summary>
        int SelectedFilterAthleteId { get; }
        /// <summary>
        /// Возвращает идентификатор тренера, выбранного
        /// из результатов подбора.
        /// </summary>
        int SelectedFilteredTrainerId { get; }
        /// <summary>
        /// Отображает список спортсменов.
        /// </summary>
        /// <param name="athletes">Список спортсменов для отображения.</param>
        void ShowAthletes(IReadOnlyList<AthleteDto> athletes);
        /// <summary>
        /// Отображает список тренеров.
        /// </summary>
        /// <param name="trainers">Список тренеров для отображения.</param>
        void ShowTrainers(IReadOnlyList<TrainerDto> trainers);
        /// <summary>
        /// Отображает рейтинг тренеров.
        /// </summary>
        /// <param name="rating">Данные рейтинга тренеров.</param>
        void ShowRating(IReadOnlyList<TrainerRatingDto> rating);
        /// <summary>
        /// Отображает персональную программу тренировок спортсмена.
        /// </summary>
        /// <param name="trainingPlan">Текст персональной программы.</param>
        void ShowPersonalTraining(string trainingPlan);
        /// <summary>
        /// Отображает результаты подбора подходящих тренеров.
        /// </summary>
        /// <param name="trainers">Список подходящих тренеров с результатами подбора.</param>
        void ShowFilteredTrainers(IReadOnlyList<TrainerMatchDto> trainers);
    }
}
