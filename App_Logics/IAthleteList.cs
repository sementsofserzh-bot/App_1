using Essence;

namespace App_Model
{
    /// <summary>Список спортсменов для выбора в интерфейсе.</summary>
    public interface IAthleteList
    {
        /// <summary>Возвращает текущий список спортсменов.</summary>
        List<Athlete> BD_Athlete { get; }
    }
}
