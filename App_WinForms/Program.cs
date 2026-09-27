using System;
using System.Windows.Forms;
using App_Model;

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