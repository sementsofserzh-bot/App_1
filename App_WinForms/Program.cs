using System;
using System.Windows.Forms;
using App_Model;
using DataAccessLayer;
using 

namespace App_WinForms
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            Logics logics = new Logics(
                new TrainerDapperRepository(),
                new AthleteDapperRepository()
                );
            //Logics logics = new Logics(
            //    new EntityRepository<Trainer>(),
            //    new EntityRepository<Athlete>()
            //    );

            Application.Run(new MainForm(logics));
        }
    }
}