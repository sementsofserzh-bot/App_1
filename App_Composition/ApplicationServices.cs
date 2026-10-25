using DataAccessLayer;
using Ninject;

namespace App_Composition
{
    /// <summary>Создаёт контейнер Ninject для обоих приложений.</summary>
    public static class ApplicationServices
    {
        /// <summary>Создаёт таблицы локальной базы данных при необходимости.</summary>
        public static void InitializeDatabase() => DatabaseInitializer.Initialize();

        /// <summary>Создаёт контейнер Ninject с привязками приложения.</summary>
        public static IKernel CreateKernel() => new StandardKernel(new ApplicationModule());
    }
}
