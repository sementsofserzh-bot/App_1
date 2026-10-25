using Essence;

namespace App_Model
{
    /// <summary>Операции пользовательского меню консоли.</summary>
    public interface IConsoleUserLogic : IAthleteCreateLogic, IRegistrationLogic,
        IUserCommonLogic
    {
        /// <summary>Возвращает тренера по идентификатору или null, если он не найден.</summary>
        Trainer? CheckTrainer(int id);
        /// <summary>Возвращает спортсмена по идентификатору или null, если он не найден.</summary>
        Athlete? CheckAthlete(int id);
    }
}
