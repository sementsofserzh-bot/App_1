using System;
using System.Security.Cryptography.X509Certificates;
using App_Model_Essence;
using App_Model_TestLogics;

namespace App_Console
{
    class Program
    {
        static void Main()
        {


            bool UsExit = true;

            while (UsExit)
            {
                Console.WriteLine("Выберите действие:");
                Console.WriteLine("1. Добавить тренера");
                Console.WriteLine("2. Добавить спортсмена");
                Console.WriteLine("3. Удалить тренера");
                Console.WriteLine("4. Удалить спортсмена");
                Console.WriteLine("5. Проверить тренера");
                Console.WriteLine("6. Проверить спортсмена");
                Console.WriteLine("7. Обновить информацию о тренере");
                Console.WriteLine("8. Обновить информацию о спортсмене");
                Console.WriteLine("9. Регистрация спортсмена у тренера");
                Console.WriteLine("10. Персональная тренировка по полу");
                Console.WriteLine("0. Выход");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Введите данные тренера:");
                        Console.WriteLine("ФИО:");
                        string fullName = Console.ReadLine();
                        Console.WriteLine("Пол:");
                        string gender = Console.ReadLine();
                        Console.WriteLine("Возраст:");
                        int age = int.Parse(Console.ReadLine());
                        Console.WriteLine("Вес:");
                        double weight = double.Parse(Console.ReadLine());
                        Console.WriteLine("Рост:");
                        double height = double.Parse(Console.ReadLine());
                        break;
                    case "2":
                        break;
                    case "3":
                        break;
                    case "4":
                        break;
                    case "5":
                        break;
                    case "6":
                        break;
                    case "7":
                        break;
                    case "8":
                        break;
                    case "9":
                        break;
                    case "10":
                        break;
                    case "0":
                        UsExit = false;
                        break;
                    default:
                        Console.WriteLine("Неверный выбор, попробуйте снова.");
                        break;
                }
            }
        }
    }
}

  