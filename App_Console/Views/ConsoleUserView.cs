
using Essence;
using Shared;

namespace App_Console
{
    public class ConsoleUserView : IUserView
    {
        public event EventHandler? AddAthleteRequested;
        public event EventHandler? RefreshRatingRequested;
        public event EventHandler? RegisterAthleteRequested;
        public event EventHandler? PersonalTrainingRequested;
        public event EventHandler? FilterTrainersRequested;
        public event EventHandler? SignUpFromFilterRequested;

        public AthleteInputDto NewAthlete { get; private set; } = new();

        public int SelectedRegistrationAthleteId { get; private set; }
        public int SelectedRegistrationTrainerId { get; private set; }
        public int SelectedPersonalAthleteId { get; private set; }
        public int SelectedFilterAthleteId { get; private set; }
        public int SelectedFilteredTrainerId { get; private set; }

        public void Run()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("=== МЕНЮ ПОЛЬЗОВАТЕЛЯ ===");
                Console.WriteLine("1. Добавить атлета");
                Console.WriteLine("2. Регистрация за тренером");
                Console.WriteLine("3. Персональная тренировка");
                Console.WriteLine("4. Подбор тренера");
                Console.WriteLine("5. Рейтинг тренеров");
                Console.WriteLine("0. Назад");
                Console.Write("Выберите действие: ");

                string? choice = Console.ReadLine();

                if (choice == "0")
                    return;

                Console.Clear();

                switch (choice)
                {
                    case "1":
                        AddAthlete();
                        break;

                    case "2":
                        RegisterAthlete();
                        break;

                    case "3":
                        PersonalTraining();
                        break;

                    case "4":
                        FilterTrainers();
                        break;

                    case "5":
                        RefreshRatingRequested?.Invoke(
                            this, EventArgs.Empty);
                        break;

                    default:
                        ShowError("Неизвестная команда");
                        break;
                }

                Console.WriteLine();
                Console.WriteLine("Нажмите любую клавишу...");
                Console.ReadKey(true);
            }
        }

        private void AddAthlete()
        {
            Console.WriteLine("=== ДОБАВЛЕНИЕ АТЛЕТА ===");

            Console.Write("Введите ФИО: ");
            string fullName = Console.ReadLine() ?? "";

            Gendre gender = ReadEnum<Gendre>("Выберите пол");

            TrainingType trainingType =
                ReadAthleteTrainingType(gender);

            int age = ReadInt("Возраст: ");
            int height = ReadInt("Рост в см: ");
            int weight = ReadInt("Вес в кг: ");

            NewAthlete = new AthleteInputDto
            {
                FullName = fullName,
                Gender = (int)gender,
                TrainingType = (int)trainingType,
                Age = age,
                Height = height,
                Weight = weight
            };

            AddAthleteRequested?.Invoke(this, EventArgs.Empty);
        }

        private void RegisterAthlete()
        {
            Console.WriteLine("=== РЕГИСТРАЦИЯ ЗА ТРЕНЕРОМ ===");

            SelectedRegistrationAthleteId =
                ReadInt("ID атлета: ");

            SelectedRegistrationTrainerId =
                ReadInt("ID тренера: ");

            RegisterAthleteRequested?.Invoke(
                this, EventArgs.Empty);
        }

        private void PersonalTraining()
        {
            Console.WriteLine("=== ПЕРСОНАЛЬНАЯ ТРЕНИРОВКА ===");

            SelectedPersonalAthleteId =
                ReadInt("ID атлета: ");

            PersonalTrainingRequested?.Invoke(
                this, EventArgs.Empty);
        }

        private void FilterTrainers()
        {
            Console.WriteLine("=== ПОДБОР ТРЕНЕРА ===");

            SelectedFilterAthleteId =
                ReadInt("ID атлета: ");

            FilterTrainersRequested?.Invoke(
                this, EventArgs.Empty);
        }

        public void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }

        public void ShowError(string message)
        {
            Console.WriteLine($"Ошибка: {message}");
        }

        public void ShowAthletes(
            IReadOnlyList<AthleteDto> athletes)
        {
            Console.WriteLine("=== СПИСОК АТЛЕТОВ ===");

            if (athletes.Count == 0)
            {
                Console.WriteLine("Атлетов нет.");
                return;
            }

            foreach (var athlete in athletes)
            {
                Console.WriteLine(
                    $"ID: {athlete.Id}, " +
                    $"ФИО: {athlete.FullName}, " +
                    $"Возраст: {athlete.Age}, " +
                    $"Рост: {athlete.Height}, " +
                    $"Вес: {athlete.Weight}, " +
                    $"Тренер: {athlete.TrainerName}");
            }
        }

        public void ShowTrainers(
            IReadOnlyList<TrainerDto> trainers)
        {
            Console.WriteLine("=== СПИСОК ТРЕНЕРОВ ===");

            if (trainers.Count == 0)
            {
                Console.WriteLine("Тренеров нет.");
                return;
            }

            foreach (var trainer in trainers)
            {
                Console.WriteLine(
                    $"ID: {trainer.Id}, " +
                    $"ФИО: {trainer.FullName}, " +
                    $"Возраст: {trainer.Age}, " +
                    $"Стаж: {trainer.WorkExperience}, " +
                    $"Рейтинг: {trainer.Rating}");
            }
        }

        public void ShowRating(
            IReadOnlyList<TrainerRatingDto> rating)
        {
            Console.WriteLine("=== РЕЙТИНГ ТРЕНЕРОВ ===");

            if (rating.Count == 0)
            {
                Console.WriteLine("Тренеров нет.");
                return;
            }

            foreach (var item in rating)
            {
                Console.WriteLine($"{item.Position}. {item.Trainer.FullName}");
                Console.WriteLine($"ID: {item.Trainer.Id}");
                Console.WriteLine($"Возраст: {item.Trainer.Age}");
                Console.WriteLine($"Стаж: {item.Trainer.WorkExperience}");
                Console.WriteLine($"Закреплено атлетов: {item.AthleteCount}");
                Console.WriteLine($"Тип тренировки: {(TrainingType)item.Trainer.TrainingType}");
                Console.WriteLine($"Рейтинг: {item.Trainer.Rating}");
                Console.WriteLine("-------------------------");
            }
        }

        public void ShowPersonalTraining(string trainingPlan)
        {
            Console.WriteLine("=== ПЕРСОНАЛЬНАЯ ТРЕНИРОВКА ===");
            Console.WriteLine(trainingPlan);
        }

        public void ShowFilteredTrainers(
            IReadOnlyList<TrainerMatchDto> trainers)
        {
            Console.WriteLine("=== ПОДХОДЯЩИЕ ТРЕНЕРЫ ===");

            if (trainers.Count == 0)
            {
                Console.WriteLine("Подходящих тренеров не найдено.");
                return;
            }

            foreach (var match in trainers)
            {
                Console.WriteLine($"ID: {match.Trainer.Id}");
                Console.WriteLine($"ФИО: {match.Trainer.FullName}");
                Console.WriteLine($"Совместимость: {match.MatchPercentage}%");
                Console.WriteLine($"Количество атлетов: {match.AthleteCount}");
                Console.WriteLine("-------------------------");
            }
        }

        private static int ReadInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);

                if (int.TryParse(Console.ReadLine(), out int value))
                    return value;

                Console.WriteLine("Ошибка: введите целое число.");
            }
        }

        private static T ReadEnum<T>(string prompt)
            where T : struct, Enum
        {
            T[] values = Enum.GetValues<T>();

            Console.WriteLine(prompt);

            for (int i = 0; i < values.Length; i++)
                Console.WriteLine($"{i + 1}. {values[i]}");

            while (true)
            {
                int choice = ReadInt("Сделайте выбор: ");

                if (choice >= 1 && choice <= values.Length)
                    return values[choice - 1];

                Console.WriteLine("Некорректный выбор.");
            }
        }

        private static TrainingType ReadAthleteTrainingType(
            Gendre gender)
        {
            TrainingType[] types = Enum.GetValues<TrainingType>()
                .Where(t => t.ToString().StartsWith(
                    $"{gender}_", StringComparison.Ordinal))
                .ToArray();

            Console.WriteLine("Выберите тип тренировки:");

            for (int i = 0; i < types.Length; i++)
                Console.WriteLine($"{i + 1}. {types[i]}");

            while (true)
            {
                int choice = ReadInt("Сделайте выбор: ");

                if (choice >= 1 && choice <= types.Length)
                    return types[choice - 1];

                Console.WriteLine("Некорректный выбор.");
            }
        }
    }
}
