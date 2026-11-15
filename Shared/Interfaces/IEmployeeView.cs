using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    /// <summary>
    /// Определяет контракт представления сотрудника.
    /// Предоставляет Presenter события операций с тренерами и спортсменами,
    /// данные для выполнения этих операций и методы отображения результатов.
    /// </summary>
    public interface IEmployeeView : IView
    {
        /// <summary>
        /// Возникает при запросе добавления нового тренера.
        /// </summary>
        event EventHandler AddTrainerRequested;
        /// <summary>
        /// Возникает при запросе добавления нового спортсмена.
        /// </summary>
        event EventHandler AddAthleteRequested;
        /// <summary>
        /// Возникает при запросе обновления данных о тренерах и спортсменах.
        /// </summary>
        event EventHandler RefreshDataRequested;
        /// <summary>
        /// Возникает при запросе изменения информации о тренере.
        /// </summary>
        event EventHandler UpdateTrainerRequested;
        /// <summary>
        /// Возникает при запросе удаления тренера.
        /// </summary>
        event EventHandler DeleteTrainerRequested;
        /// <summary>
        /// Возникает при запросе изменения информации о спортсмене.
        /// </summary>
        event EventHandler UpdateAthleteRequested;
        /// <summary>
        /// Возникает при запросе удаления спортсмена.
        /// </summary>
        event EventHandler DeleteAthleteRequested;
        /// <summary>
        /// Возникает при запросе прикрепления спортсмена к тренеру.
        /// </summary>
        event EventHandler AttachAthleteRequested;
        /// <summary>
        /// Возникает при запросе открепления спортсмена от тренера.
        /// </summary>
        event EventHandler DetachAthleteRequested;
        /// <summary>
        /// Возвращает данные нового тренера, введённые пользователем.
        /// </summary>
        /// 
        TrainerInputDto NewTrainer { get; }
        /// <summary>
        /// Возвращает данные нового спортсмена, введённые пользователем.
        /// </summary>
        AthleteInputDto NewAthlete { get; }
        /// <summary>
        /// Возвращает изменённые данные выбранного тренера.
        /// </summary>
        TrainerEditDto EditedTrainer { get; }
        /// <summary>
        /// Возвращает изменённые данные выбранного спортсмена.
        /// </summary>
        AthleteEditDto EditedAthlete { get; }
        /// <summary>
        /// Возвращает идентификаторы выбранного тренера и спортсмена
        /// для выполнения операции изменения их связи.
        /// </summary>
        TrainerAthleteDto SelectedTrainerAthlete { get; }
        /// <summary>
        /// Возвращает идентификатор выбранного тренера.
        /// </summary>
        int SelectedTrainerId { get; }
        /// <summary>
        /// Возвращает идентификатор выбранного спортсмена.
        /// </summary>
        int SelectedAthleteId { get; }
        /// <summary>
        /// Отображает список тренеров.
        /// </summary>
        /// <param name="trainers">Список тренеров для отображения.</param>
        void ShowTrainers(IReadOnlyList<TrainerDto> trainers);
        /// <summary>
        /// Отображает список спортсменов.
        /// </summary>
        /// <param name="athletes">Список спортсменов для отображения.</param>
        void ShowAthletes(IReadOnlyList<AthleteDto> athletes);
    }
}
