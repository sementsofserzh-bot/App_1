using Essence;

namespace App_Model
{
    /// <summary>Действия формы редактирования спортсмена.</summary>
    public interface IAthleteEditLogic : IAthleteCreateLogic
    {
        /// <summary>Обновляет данные спортсмена и его назначенного тренера.</summary>
        Athlete? UpdateInfoAthlete(int id, string? fullname, Gendre? gendre, int? age,
            int? height, int? weight, TrainingType? trainingType, Trainer? trainer);
    }
}
