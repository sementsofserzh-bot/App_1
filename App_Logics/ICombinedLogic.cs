namespace App_Model
{
    /// <summary>Операции экрана, совмещающего меню сотрудника и пользователя.</summary>
    public interface ICombinedLogic : IEmployeeLogic, IUserCommonLogic
    {
    }
}
