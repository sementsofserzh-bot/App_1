namespace App_Model
{
    /// <summary>Операции меню сотрудника.</summary>
    public interface IEmployeeLogic : ITrainerCatalog, IAthleteCatalog,
        ITrainerEditLogic, IAthleteEditLogic, IRegistrationLogic
    {
        /// <summary>Удаляет тренера и снимает его назначение у спортсменов.</summary>
        bool? RemoveTrainer(int id);
        /// <summary>Удаляет спортсмена по идентификатору.</summary>
        bool? RemoveAthlete(int id);
    }
}
