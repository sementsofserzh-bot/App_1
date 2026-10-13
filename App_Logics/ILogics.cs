using Essence;

namespace App_Model
{
    /// <summary>
    /// Интерфейс для реализации бизнес-логики
    /// </summary>
    public interface ILogics
    {
        /// <summary>Возвращает список всех тренеров.</summary>
        List<Trainer> BD_Trainer { get; }

        /// <summary>Возвращает список всех спортсменов.</summary>
        List<Athlete> BD_Athlete { get; }

        /// <summary>Создаёт и сохраняет тренера с указанными данными.</summary>
        Trainer AddTrainer(string fullname, Gendre gendre, TrainingType trainingType, int age, int workExperience);

        /// <summary>Создаёт и сохраняет спортсмена с указанными данными.</summary>
        Athlete AddAthlete(string fullname, Gendre gendre, TrainingType trainingType, int age, int height, int weight);

        /// <summary>Удаляет тренера по идентификатору и снимает его назначение у спортсменов.</summary>
        bool? RemoveTrainer(int id);

        /// <summary>Удаляет спортсмена по идентификатору.</summary>
        bool? RemoveAthlete(int id);

        /// <summary>Находит тренера по идентификатору.</summary>
        Trainer? CheckTrainer(int id);

        /// <summary>Находит спортсмена по идентификатору.</summary>
        Athlete? CheckAthlete(int id);

        /// <summary>Обновляет данные тренера и список закреплённых за ним спортсменов.</summary>
        Trainer? UpdateInfoTrainer(int id, string? fullname, Gendre? gendre, TrainingType? trainingType,
            int? age, int? workExperience, List<Athlete>? deleteathlete, List<Athlete>? addathlete);

        /// <summary>Обновляет данные спортсмена и назначенного ему тренера.</summary>
        Athlete? UpdateInfoAthlete(int id, string? fullname, Gendre? gendre, int? age, int? height,
            int? weight, TrainingType? trainingType, Trainer? trainer);

        /// <summary>Закрепляет спортсмена за тренером.</summary>
        bool Registration(Trainer trainer, Athlete athlete);

        /// <summary>Составляет персональную программу тренировок для спортсмена.</summary>
        string PersonalTraining(Athlete athlete);

        /// <summary>Возвращает тренеров по убыванию соответствия спортсмену.</summary>
        List<Trainer> PersonalFilterTrainers(Athlete athlete);

        /// <summary>Обновляет рейтинги тренеров и возвращает их по убыванию рейтинга.</summary>
        List<Trainer> RateTrainers();

        /// <summary>Вычисляет рейтинг тренера.</summary>
        double CalculateRating(Trainer trainer);

        /// <summary>Вычисляет процент соответствия тренера спортсмену.</summary>
        int CalculateMatchPercentage(Trainer trainer, Athlete athlete);
    }
}
