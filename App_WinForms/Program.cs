using System;
using System.Windows.Forms;
using App_Model_TestLogics; // Подключаем пространство имен с логикой

namespace App_WinForms
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // Стандартные настройки WinForms (в .NET 6+ это ApplicationConfiguration.Initialize())
            // Если у тебя более старый .NET Framework, используй закомментированные строки ниже:
            ApplicationConfiguration.Initialize();
            // Application.EnableVisualStyles();
            // Application.SetCompatibleTextRenderingDefault(false);

            // 1. Создаем единый экземпляр нашей заглушки со всеми тестовыми данными
            ILogics logics = new Logics();

            // 2. Передаем готовую логику в главную форму при запуске
            Application.Run(new MainForm(logics));
        }
    }
}