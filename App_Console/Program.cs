using System;
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
                        Console.WriteLine(ILogics.AddTrainer(fullName, gender, age));
                        break;
                    case "2":
                        // Логика добавления спортсмена
                        break;
                    case "3":
                        // Логика удаления тренера
                        break;
                    case "4":
                        // Логика удаления спортсмена
                        break;
                    case "5":
                        // Логика проверки тренера
                        break;
                    case "6":
                        // Логика проверки спортсмена
                        break;
                    case "7":
                        // Логика обновления информации о тренере
                        break;
                    case "8":
                        // Логика обновления информации о спортсмене
                        break;
                    case "9":
                        // Логика регистрации спортсмена у тренера
                        break;
                    case "10":
                        // Логика персональной тренировки по полу
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