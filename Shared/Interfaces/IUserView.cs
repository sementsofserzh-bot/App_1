using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public interface IUserView : IView
    {
        event EventHandler AddAthleteRequested;
        event EventHandler RefreshRatingRequested;
        event EventHandler RegisterAthleteRequested;
        event EventHandler PersonalTrainingRequested;
        event EventHandler FilterTrainersRequested;
        event EventHandler SignUpFromFilterRequested;
        AthleteInputDto NewAthlete { get; }
        int SelectedRegistrationAthleteId { get; }
        int SelectedRegistrationTrainerId { get; }
        int SelectedPersonalAthleteId { get; }
        int SelectedFilterAthleteId { get; }
        int SelectedFilteredTrainerId { get; }
        void ShowAthletes(IReadOnlyList<AthleteDto> athletes);
        void ShowTrainers(IReadOnlyList<TrainerDto> trainers);
        void ShowRating(IReadOnlyList<TrainerRatingDto> rating);
        void ShowPersonalTraining(string trainingPlan);
        void ShowFilteredTrainers(IReadOnlyList<TrainerMatchDto> trainers);
    }
}
