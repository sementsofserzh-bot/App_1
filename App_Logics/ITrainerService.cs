namespace App_Model
{
    /// <summary>Все операции сервиса тренеров, необходимые фасаду.</summary>
    public interface ITrainerService : ITrainerCatalog, ITrainerEditLogic
    {
        /// <summary>Удаляет тренера и снимает его назначение у спортсменов.</summary>
        bool? RemoveTrainer(int id);
    }
}
