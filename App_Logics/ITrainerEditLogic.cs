using Essence;

namespace App_Model
{
    /// <summary>Действия формы редактирования тренера.</summary>
    public interface ITrainerEditLogic
    {
        /// <summary>Создаёт и сохраняет тренера с указанными параметрами.</summary>
        Trainer AddTrainer(string fullname, Gendre gendre, TrainingType trainingType,
            int age, int workExperience);
        /// <summary>Обновляет данные тренера и его связи со спортсменами.</summary>
        Trainer? UpdateInfoTrainer(int id, string? fullname, Gendre? gendre,
            TrainingType? trainingType, int? age, int? workExperience,
            List<Athlete>? deleteathlete, List<Athlete>? addathlete);
    }
}
