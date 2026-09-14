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
            LogicStub logic = new LogicStub();

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
                        Console.WriteLine(logic.AddTrainer(fullName, gender, age));
                        break;
                    case "2":
                        Console.WriteLine("Введите данные спортсмена:");
                        Console.WriteLine("ФИО:");
                        fullName = Console.ReadLine();
                        Console.WriteLine("Пол:");
                        gender = Console.ReadLine();
                        Console.WriteLine("Возраст:");
                        age = int.Parse(Console.ReadLine());
                        Console.WriteLine(logic.AddAthlete(fullName, gender, age));
                        break;
                    case "3":
                        Console.WriteLine("Введите ID тренера для удаления:");
                        int idTrainer = int.Parse(Console.ReadLine());
                        Console.WriteLine(logic.RemoveTrainer(idTrainer));
                        break;
                    case "4":
                        Console.WriteLine("Введите ID спортсмена для удаления:");
                        int idAthlete = int.Parse(Console.ReadLine());
                        Console.WriteLine(logic.RemoveAthlete(idAthlete));
                        break;
                    case "5":
                        Console.WriteLine("Введите ID тренера для проверки:");
                        idTrainer = int.Parse(Console.ReadLine());
                        Trainer? trainer = logic.CheckTrainer(idTrainer);
                        if (trainer != null)
                        {
                            Console.WriteLine($"Тренер найден: {trainer.FullName}, {trainer.Gender}, {trainer.Age}");
                            if (trainer.AthleteIds.Count > 0)
                            {
                                Console.WriteLine("Список спортсменов тренера:");
                                for (int i = 0; i < trainer.AthleteIds.Count; i++)
                                {
                                    Console.WriteLine($"Спортсмен {i + 1}: {logic.CheckAthlete(trainer.AthleteIds[i]).FullName}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("У тренера нет спортсменов.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Тренер не найден.");
                        }
                        break;
                    case "6":
                        Console.WriteLine("Введите ID атлета для проверки:");
                        idAthlete = int.Parse(Console.ReadLine());
                        Athlete? athlete = logic.CheckAthlete(idAthlete);
                        if(athlete != null)
                        {
                            Console.WriteLine($"Атлет найден: {athlete.FullName}, {athlete.Gender}, {athlete.Age}");
                            if(athlete.TrainerId != null)
                            {
                                Console.WriteLine($"Прикреплен к тренеру: {logic.CheckTrainer(athlete.TrainerId).FullName}");
                            }
                            else
                            {
                                Console.WriteLine("У атлета нет персонального тренера.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Атлдет не найден.");
                        }
                        break;
                    case "7":
                        Console.WriteLine("введите ID тренера у которого желаете обновить данные");
                        idTrainer = int.Parse(Console.ReadLine());
                        Trainer? trainer_check = logic.CheckTrainer(idTrainer);
                        if (trainer_check != null)
                        {
                            Console.WriteLine("Введите ФИО:");
                            string FullName = Console.ReadLine();
                            Console.WriteLine("Введите пол:");
                            string gender_check_trainer = Console.ReadLine();
                            Console.WriteLine("Введите возраст:");
                            int age_check_trainer = int.Parse(Console.ReadLine());
                            Trainer New_Trainer = logic.UpdateInfoTrainer(idTrainer, FullName, gender_check_trainer, age_check_trainer);
                            for(int i = 0; logic.Check_AthleteInTrainer(idTrainer).Count)
                            Console.WriteLine($"Обновленные данные тренера: {New_Trainer.FullName}, {New_Trainer.Gender}, {New_Trainer.Age}");

                        }
                        else
                        {
                            Console.WriteLine("Тренер не найден.");
                        }
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

  