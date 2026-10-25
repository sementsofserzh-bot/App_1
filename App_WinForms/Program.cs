using System;
using System.Windows.Forms;
using App_Model;
using App_Composition;
using Ninject;



namespace App_WinForms
{
    /// <summary>
    /// Точка входа приложения Windows Forms.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Инициализирует приложение, получает зависимости из контейнера и запускает главную форму.
        /// </summary>
        /// <remarks>Контейнер освобождается после закрытия главной формы.</remarks>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            ApplicationServices.InitializeDatabase();
            using var kernel = ApplicationServices.CreateKernel();
            kernel.Get<InitialDataSeeder>().SeedIfEmpty();
            using MainForm mainForm = kernel.Get<MainForm>();
            Application.Run(mainForm);
        }
    }
}
