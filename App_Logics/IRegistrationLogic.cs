using Essence;

namespace App_Model
{
    /// <summary>Запись спортсмена к тренеру.</summary>
    public interface IRegistrationLogic
    {
        /// <summary>Закрепляет спортсмена за тренером, если оба существуют и связь ещё не создана.</summary>
        bool Registration(Trainer trainer, Athlete athlete);
    }
}
