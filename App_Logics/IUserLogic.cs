using Essence;

namespace App_Model
{
    /// <summary>Операции пользовательской формы.</summary>
    public interface IUserLogic : ITrainerCatalog, IAthleteList,
        IAthleteCreateLogic, IRegistrationLogic, IUserCommonLogic
    {
        /// <summary>Вычисляет процент соответствия тренера спортсмену.</summary>
        int CalculateMatchPercentage(Trainer trainer, Athlete athlete);
    }
}
