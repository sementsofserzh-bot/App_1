using DataAccessLayer;
using Ninject;
using App_Model;
using App_Presenter;
using Shared;

namespace App_Composition
{
    /// <summary>Создаёт контейнер Ninject для обоих приложений.</summary>
    public static class ApplicationServices
    {
        /// <summary>Создаёт таблицы локальной базы данных при необходимости.</summary>
        public static void InitializeDatabase() => DatabaseInitializer.Initialize();

        /// <summary>Создаёт контейнер Ninject с привязками приложения.</summary>
        public static IKernel CreateKernel() => new StandardKernel(new ApplicationModule());

        public static UserPresenter CreateUserPresenter(IKernel kernel, IUserView view)
        {
            return new UserPresenter(
                view,
                kernel.Get<IUserLogic>()
            );
        }

        public static EmployeePresenter CreateEmployeePresenter(IKernel kernel, IEmployeeView view)
        {
            return new EmployeePresenter(
                view,
                kernel.Get<IEmployeeLogic>()
            );
        }
    }
}
