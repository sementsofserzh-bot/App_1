
using Essence;
using Shared;

namespace App_Console
{
    public class ConsoleEmployeeView : IEmployeeView
    {
        public event EventHandler? AddTrainerRequested;
        public event EventHandler? AddAthleteRequested;
        public event EventHandler? RefreshDataRequested;
        public event EventHandler? UpdateTrainerRequested;
        public event EventHandler? DeleteTrainerRequested;
        public event EventHandler? UpdateAthleteRequested;
        public event EventHandler? DeleteAthleteRequested;
        public event EventHandler? AttachAthleteRequested;
        public event EventHandler? DetachAthleteRequested;

        public TrainerInputDto NewTrainer { get; private set; } = new();
        public AthleteInputDto NewAthlete { get; private set; } = new();
        public TrainerEditDto EditedTrainer { get; private set; } = new();
        public AthleteEditDto EditedAthlete { get; private set; } = new();

        public TrainerAthleteDto SelectedTrainerAthlete { get; private set; } = new();
        public int SelectedTrainerId { get; private set; }
        public int SelectedAthleteId { get; private set; }

        private IReadOnlyList<TrainerDto> _trainers = new List<TrainerDto>();
        private IReadOnlyList<AthleteDto> _athletes = new List<AthleteDto>();

        private bool _suppressOutput;

        public void Run()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("=== МЕНЮ СОТРУДНИКА ===");
                Console.WriteLine("1. Добавить тренера");
                Console.WriteLine("2. Добавить атлета");
                Console.WriteLine("3. Удалить тренера");
                Console.WriteLine("4. Удалить атлета");
                Console.WriteLine("5. Просмотреть тренеров");
                Console.WriteLine("6. Просмотреть атлетов");
                Console.WriteLine("7. Изменить тренера");
                Console.WriteLine("8. Изменить атлета");
                Console.WriteLine("9. Зарегистрировать атлета за тренером");
                Console.WriteLine("0. Назад");

                Console.Write("Выберите действие: ");
                string? choice = Console.ReadLine();

                if (choice == "0")
                    return;

                Console.Clear();

                switch (choice)
                {
                    case "1":
                        AddTrainer();
                        break;

                    case "2":
                        AddAthlete();
                        break;

                    case "3":
                        DeleteTrainer();
                        break;

                    case "4":
                        DeleteAthlete();
                        break;

                    case "5":
                        ViewTrainers();
                        break;

                    case "6":
                        ViewAthletes();
                        break;

                    case "7":
                        EditTrainer();
                        break;

                    case "8":
                        EditAthlete();
                        break;

                    case "9":
                        AttachAthlete();
                        break;

                    default:
                        ShowError("Неизвестная команда");
                        break;
                }

                Console.WriteLine("\nНажмите любую клавишу...");
                Console.ReadKey(true);
            }
        }

        private void AddTrainer()
        {
            Console.WriteLine("=== ДОБАВЛЕНИЕ ТРЕНЕРА ===");

            Console.Write("ФИО: ");
            string name = Console.ReadLine() ?? "";

            Gendre gender = ReadEnum<Gendre>("Выберите пол");

            TrainingType type = ReadEnum<TrainingType>(
                "Выберите тип тренировки");

            NewTrainer = new TrainerInputDto
            {
                FullName = name,
                Gender = (int)gender,
                TrainingType = (int)type,
                Age = ReadInt("Возраст: "),
                WorkExperience = ReadInt("Стаж: ")
            };

            ExecuteWithoutOutput(() =>
                AddTrainerRequested?.Invoke(this, EventArgs.Empty));
        }

        private void AddAthlete()
        {
            Console.WriteLine("=== ДОБАВЛЕНИЕ АТЛЕТА ===");

            Console.Write("ФИО: ");
            string name = Console.ReadLine() ?? "";

            Gendre gender = ReadEnum<Gendre>("Выберите пол");
            TrainingType type = ReadAthleteTrainingType(gender);

            NewAthlete = new AthleteInputDto
            {
                FullName = name,
                Gender = (int)gender,
                TrainingType = (int)type,
                Age = ReadInt("Возраст: "),
                Height = ReadInt("Рост: "),
                Weight = ReadInt("Вес: ")
            };

            ExecuteWithoutOutput(() =>
                AddAthleteRequested?.Invoke(this, EventArgs.Empty));
        }

        private void DeleteTrainer()
        {
            RefreshLists();
            ShowTrainers(_trainers);

            SelectedTrainerId = ReadInt("ID тренера для удаления: ");

            if (Confirm("Удалить выбранного тренера?"))
            {
                ExecuteWithoutOutput(() =>
                    DeleteTrainerRequested?.Invoke(this, EventArgs.Empty));
            }
        }

        private void DeleteAthlete()
        {
            RefreshLists();
            ShowAthletes(_athletes);

            SelectedAthleteId = ReadInt("ID атлета для удаления: ");

            if (Confirm("Удалить выбранного атлета?"))
            {
                ExecuteWithoutOutput(() =>
                    DeleteAthleteRequested?.Invoke(this, EventArgs.Empty));
            }
        }

        private void ViewTrainers()
        {
            RefreshLists();

            Console.WriteLine("=== ПРОСМОТР ТРЕНЕРОВ ===");
            Console.WriteLine("1. Конкретный тренер по ID");
            Console.WriteLine("2. Все тренеры");

            int choice = ReadInt("Выбор: ");

            switch (choice)
            {
                case 1:
                    int id = ReadInt("ID тренера: ");

                    TrainerDto? trainer =
                        _trainers.FirstOrDefault(t => t.Id == id);

                    if (trainer == null)
                    {
                        ShowError("Тренер не найден");
                        return;
                    }

                    ShowTrainers(new List<TrainerDto> { trainer });
                    break;

                case 2:
                    ShowTrainers(_trainers);
                    break;

                default:
                    ShowError("Некорректная команда");
                    break;
            }
        }

        private void ViewAthletes()
        {
            RefreshLists();

            Console.WriteLine("=== ПРОСМОТР АТЛЕТОВ ===");
            Console.WriteLine("1. Конкретный атлет по ID");
            Console.WriteLine("2. Все атлеты");

            int choice = ReadInt("Выбор: ");

            switch (choice)
            {
                case 1:
                    int id = ReadInt("ID атлета: ");

                    AthleteDto? athlete =
                        _athletes.FirstOrDefault(a => a.Id == id);

                    if (athlete == null)
                    {
                        ShowError("Атлет не найден");
                        return;
                    }

                    ShowAthletes(new List<AthleteDto> { athlete });
                    break;

                case 2:
                    ShowAthletes(_athletes);
                    break;

                default:
                    ShowError("Некорректная команда");
                    break;
            }
        }

        private void EditTrainer()
        {
            RefreshLists();
            ShowTrainers(_trainers);

            int id = ReadInt("ID тренера для изменения: ");

            if (!_trainers.Any(t => t.Id == id))
            {
                ShowError("Тренер не найден");
                return;
            }

            Console.WriteLine("1. Изменить ФИО");
            Console.WriteLine("2. Изменить пол");
            Console.WriteLine("3. Изменить тип тренировки");
            Console.WriteLine("4. Изменить возраст");
            Console.WriteLine("5. Изменить стаж");
            Console.WriteLine("6. Прикрепить атлета");
            Console.WriteLine("7. Открепить атлета");

            int choice = ReadInt("Действие: ");

            EditedTrainer = new TrainerEditDto { Id = id };

            switch (choice)
            {
                case 1:
                    Console.Write("Новое ФИО: ");
                    EditedTrainer.FullName = Console.ReadLine();
                    break;

                case 2:
                    EditedTrainer.Gender =
                        (int)ReadEnum<Gendre>("Новый пол");
                    break;

                case 3:
                    EditedTrainer.TrainingType =
                        (int)ReadEnum<TrainingType>("Новый тип");
                    break;

                case 4:
                    EditedTrainer.Age = ReadInt("Новый возраст: ");
                    break;

                case 5:
                    EditedTrainer.WorkExperience =
                        ReadInt("Новый стаж: ");
                    break;

                case 6:
                    AttachAthlete(id);
                    return;

                case 7:
                    DetachAthlete(id);
                    return;

                default:
                    ShowError("Некорректная команда");
                    return;
            }

            ExecuteWithoutOutput(() =>
                UpdateTrainerRequested?.Invoke(this, EventArgs.Empty));
        }

        private void EditAthlete()
        {
            RefreshLists();
            ShowAthletes(_athletes);

            int id = ReadInt("ID атлета для изменения: ");

            if (!_athletes.Any(a => a.Id == id))
            {
                ShowError("Атлет не найден");
                return;
            }

            Console.WriteLine("1. Изменить ФИО");
            Console.WriteLine("2. Изменить пол");
            Console.WriteLine("3. Изменить тип тренировки");
            Console.WriteLine("4. Изменить возраст");
            Console.WriteLine("5. Изменить рост");
            Console.WriteLine("6. Изменить вес");
            Console.WriteLine("7. Изменить тренера");

            int choice = ReadInt("Действие: ");

            EditedAthlete = new AthleteEditDto { Id = id };

            switch (choice)
            {
                case 1:
                    Console.Write("Новое ФИО: ");
                    EditedAthlete.FullName = Console.ReadLine();
                    break;

                case 2:
                    EditedAthlete.Gender =
                        (int)ReadEnum<Gendre>("Новый пол");
                    break;

                case 3:
                    EditedAthlete.TrainingType =
                        (int)ReadEnum<TrainingType>("Новый тип");
                    break;

                case 4:
                    EditedAthlete.Age = ReadInt("Новый возраст: ");
                    break;

                case 5:
                    EditedAthlete.Height = ReadInt("Новый рост: ");
                    break;

                case 6:
                    EditedAthlete.Weight = ReadInt("Новый вес: ");
                    break;

                case 7:
                    EditedAthlete.TrainerId =
                        ReadInt("ID нового тренера: ");
                    break;

                default:
                    ShowError("Некорректная команда");
                    return;
            }

            ExecuteWithoutOutput(() =>
                UpdateAthleteRequested?.Invoke(this, EventArgs.Empty));
        }

        private void AttachAthlete(int? trainerId = null)
        {
            Console.WriteLine("=== ПРИКРЕПЛЕНИЕ АТЛЕТА ===");

            int selectedTrainerId = trainerId ??
                ReadInt("ID тренера: ");

            int athleteId = ReadInt("ID атлета: ");

            SelectedTrainerAthlete = new TrainerAthleteDto
            {
                TrainerId = selectedTrainerId,
                AthleteId = athleteId
            };

            ExecuteWithoutOutput(() =>
                AttachAthleteRequested?.Invoke(this, EventArgs.Empty));
        }

        private void DetachAthlete(int trainerId)
        {
            Console.WriteLine("=== ОТКРЕПЛЕНИЕ АТЛЕТА ===");

            int athleteId = ReadInt("ID атлета: ");

            SelectedTrainerAthlete = new TrainerAthleteDto
            {
                TrainerId = trainerId,
                AthleteId = athleteId
            };

            if (Confirm("Открепить атлета?"))
            {
                ExecuteWithoutOutput(() =>
                    DetachAthleteRequested?.Invoke(this, EventArgs.Empty));
            }
        }

        public void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }

        public void ShowError(string message)
        {
            Console.WriteLine($"Ошибка: {message}");
        }

        public void ShowTrainers(IReadOnlyList<TrainerDto> trainers)
        {
            _trainers = trainers;

            if (_suppressOutput)
                return;

            Console.WriteLine("\n=== ТРЕНЕРЫ ===");

            foreach (var t in trainers)
            {
                Console.WriteLine(
                    $"ID: {t.Id} | {t.FullName} | " +
                    $"Пол: {(Gendre)t.Gender} | " +
                    $"Возраст: {t.Age} | " +
                    $"Тип: {(TrainingType)t.TrainingType} | " +
                    $"Стаж: {t.WorkExperience} | " +
                    $"Рейтинг: {t.Rating}");

                if (t.Athletes.Count == 0)
                {
                    Console.WriteLine("Не прикреплены атлеты.");
                }
                else
                {
                    Console.WriteLine("Прикрепленные атлеты:");

                    foreach (var athlete in t.Athletes)
                    {
                        Console.WriteLine(
                            $"  - {athlete.FullName} с ID: {athlete.Id}");
                    }
                }
            }

            if (trainers.Count == 0)
                Console.WriteLine("Список пуст.");
        }

        public void ShowAthletes(IReadOnlyList<AthleteDto> athletes)
        {
            _athletes = athletes;

            if (_suppressOutput)
                return;

            Console.WriteLine("\n=== АТЛЕТЫ ===");

            foreach (var a in athletes)
            {
                Console.WriteLine(
                    $"ID: {a.Id} | {a.FullName} | " +
                    $"Пол: {(Gendre)a.Gender} | " +
                    $"Возраст: {a.Age} | " +
                    $"Рост: {a.Height} | Вес: {a.Weight} | " +
                    $"Тип: {(TrainingType)a.TrainingType} | " +
                    $"Тренер: {a.TrainerName}");
            }

            if (athletes.Count == 0)
                Console.WriteLine("Список пуст.");
        }

        private void ExecuteWithoutOutput(Action action)
        {
            bool previousValue = _suppressOutput;

            try
            {
                _suppressOutput = true;
                action();
            }
            finally
            {
                _suppressOutput = previousValue;
            }
        }

        private void RefreshLists()
        {
            ExecuteWithoutOutput(() =>
                RefreshDataRequested?.Invoke(this, EventArgs.Empty));
        }

        private static int ReadInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);

                if (int.TryParse(Console.ReadLine(), out int result))
                    return result;

                Console.WriteLine("Введите целое число.");
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
                int choice = ReadInt("Ваш выбор: ");

                if (choice >= 1 && choice <= values.Length)
                    return values[choice - 1];

                Console.WriteLine("Некорректный выбор.");
            }
        }

        private static TrainingType ReadAthleteTrainingType(
            Gendre gender)
        {
            var types = Enum.GetValues<TrainingType>()
                .Where(t => t.ToString().StartsWith(
                    $"{gender}_", StringComparison.Ordinal))
                .ToArray();

            Console.WriteLine("Выберите тип тренировки:");

            for (int i = 0; i < types.Length; i++)
                Console.WriteLine($"{i + 1}. {types[i]}");

            while (true)
            {
                int choice = ReadInt("Ваш выбор: ");

                if (choice >= 1 && choice <= types.Length)
                    return types[choice - 1];

                Console.WriteLine("Некорректный выбор.");
            }
        }

        private static bool Confirm(string question)
        {
            Console.Write($"{question} (д/н): ");

            return Console.ReadLine()?.Trim().ToLowerInvariant()
                is "д" or "да" or "y" or "yes";
        }
    }
}
