using Essence;

namespace App_Model
{
    /// <summary>Чтение спортсменов.</summary>
    public interface IAthleteCatalog : IAthleteList
    {
        /// <summary>Возвращает спортсмена по идентификатору или null, если он не найден.</summary>
        Athlete? CheckAthlete(int id);
    }
}
