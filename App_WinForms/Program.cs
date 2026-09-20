using System;
using System.Windows.Forms;
using App_Model_TestLogics;
using App_Model_Logics;

namespace App_WinForms
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            Logics logics = new Logics();

            Application.Run(new MainForm(logics));
        }
    }
}