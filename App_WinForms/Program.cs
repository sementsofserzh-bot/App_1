using System;
using System.Windows.Forms;
using App_Model;
using DataAccessLayer;
using Contracts;

namespace App_WinForms
{
    internal static class Program
    {
        /// <summary>
        /// Инициализирует конфигурацию приложения и базу данных, создаёт UnitOfWork и слой логики, затем запускает
        /// главный оконный цикл приложения.
        /// </summary>
        /// <remarks>Помечен атрибутом STAThread для однопоточной модели COM. UnitOfWork создаётся до
        /// передачи в слой логики и будет освобождён по завершении метода (после закрытия главной формы).</remarks>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            DatabaseInitializer.Initialize();

            using IUnitOfWork unitOfWork = new UnitOfWork();
            ILogics logics = new Logics(unitOfWork);

            Application.Run(new MainForm(logics));
        }
    }
}