namespace App_Model
{
    /// <summary>Все операции сервиса спортсменов, необходимые фасаду.</summary>
    public interface IAthleteService : IAthleteCatalog, IAthleteEditLogic
    {
        /// <summary>Удаляет спортсмена по идентификатору; возвращает false, если он не найден.</summary>
        bool? RemoveAthlete(int id);
    }
}
