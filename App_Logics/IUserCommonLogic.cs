using Essence;

namespace App_Model
{
    /// <summary>Программа, подбор и рейтинг для пользовательских экранов.</summary>
    public interface IUserCommonLogic
    {
        /// <summary>Составляет персональную программу спортсмена.</summary>
        string PersonalTraining(Athlete athlete);
        /// <summary>Возвращает тренеров по убыванию соответствия спортсмену.</summary>
        List<Trainer> PersonalFilterTrainers(Athlete athlete);
        /// <summary>Обновляет рейтинги и возвращает тренеров по их убыванию.</summary>
        List<Trainer> RateTrainers();
    }
}
