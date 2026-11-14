using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public interface IEmployeeView : IView
    {
        event EventHandler AddTrainerRequested;
        event EventHandler AddAthleteRequested;
        event EventHandler RefreshDataRequested;
        event EventHandler UpdateTrainerRequested;
        event EventHandler DeleteTrainerRequested;
        event EventHandler UpdateAthleteRequested;
        event EventHandler DeleteAthleteRequested;
        event EventHandler AttachAthleteRequested;
        event EventHandler DetachAthleteRequested;
        TrainerInputDto NewTrainer { get; }
        AthleteInputDto NewAthlete { get; }
        TrainerEditDto EditedTrainer { get; }
        AthleteEditDto EditedAthlete { get; }
        TrainerAthleteDto SelectedTrainerAthlete { get; }
        int SelectedTrainerId { get; }
        int SelectedAthleteId { get; }
        void ShowTrainers(IReadOnlyList<TrainerDto> trainers);
        void ShowAthletes(IReadOnlyList<AthleteDto> athletes);
    }
}
