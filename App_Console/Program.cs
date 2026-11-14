
using App_Composition;
using App_Model;
using App_Presenter;
using Ninject;

namespace App_Console
{
    internal class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            ApplicationServices.InitializeDatabase();

            using var kernel = ApplicationServices.CreateKernel();
            kernel.Get<InitialDataSeeder>().SeedIfEmpty();

            var userView = new ConsoleUserView();
            var employeeView = new ConsoleEmployeeView();

            var userPresenter =
                ApplicationServices.CreateUserPresenter(
                    kernel, userView);

            var employeePresenter =
                ApplicationServices.CreateEmployeePresenter(
                    kernel, employeeView);

            const int password = 1234;

            while (true)
            {
                Console.Clear();

                Console.WriteLine("=== PRIME TIME ===");
                Console.WriteLine("1. Сотрудник");
                Console.WriteLine("2. Пользователь");
                Console.WriteLine("0. Выход");

                int role = ReadInt("Выберите роль: ");

                switch (role)
                {
                    case 1:
                        if (CheckEmployeePassword(password))
                        {
                            employeePresenter.Initialize();
                            employeeView.Run();
                        }
                        break;

                    case 2:
                        userPresenter.Initialize();
                        userView.Run();
                        break;

                    case 0:
                        return;

                    default:
                        Console.WriteLine("Неизвестная команда.");
                        Console.ReadKey(true);
                        break;
                }
            }
        }

        private static bool CheckEmployeePassword(int password)
        {
            int attempts = 3;

            while (attempts > 0)
            {
                Console.Clear();
                Console.Write("Введите пароль: ");

                int entered = ReadInt("");

                if (entered == password)
                {
                    Console.WriteLine("Добро пожаловать!");
                    Console.ReadKey(true);
                    return true;
                }

                attempts--;

                Console.WriteLine(
                    $"Неверный пароль. Осталось попыток: {attempts}");

                Console.ReadKey(true);
            }

            return false;
        }

        private static int ReadInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);

                if (int.TryParse(Console.ReadLine(), out int value))
                    return value;

                Console.WriteLine("Введите целое число.");
            }
        }
    }
}
