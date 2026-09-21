using App_Model_Essence;
using App_Model_TestLogics;
using App_Model_Logics;

namespace App_Console
{
    /// <summary>
    /// Консольное представление приложения.
    /// Реализует пользовательский интерфейс для работы с тренерами и атлетами:
    /// меню сотрудника (CRUD-операции) и меню пользователя (бизнес-функции).
    /// Взаимодействует с бизнес-логикой через интерфейс ILogics.
    /// </summary>
    class Program
    {
        static readonly Dictionary<string, string> PropertyNames = new()
        {
            { "Id", "ID" },
            { "FullName", "ФИО" },
            { "Gendre", "Пол" },
            { "Age", "Возраст" },
            { "Height", "Рост" },
            { "Weight", "Вес" },
            { "TrainingType", "Тип тренировки" },
            { "WorkExperience", "Стаж работы" },
            { "Rating", "Рейтинг" },
            { "trainer", "Тренер" },
            { "Athlete", "Прикрепленные атлеты" },
            { "TypePersonalTraining", "Рекомендованный тип тренировки" }
        };

        /// <summary>
        /// Выводит в консоль все свойства переданного объекта.
        /// Использует рефлексию для перебора свойств и словарь PropertyNames
        /// для перевода технических имён на русский язык.
        /// Свойства trainer и Athlete обрабатываются отдельно — для них
        /// выводится информация о связанных объектах.
        /// </summary>
        /// <typeparam name="T">Тип объекта (Trainer или Athlete).</typeparam>
        /// <param name="essence">Объект для вывода.</param>
        static void ViewEssence<T>(T essence)
        {
            if (essence != null)
            {
                foreach (var property in typeof(T).GetProperties())
                {
                    string displayName = PropertyNames.TryGetValue(property.Name, out var ru)
                        ? ru
                        : property.Name;

                    if (property.Name == "trainer")
                    {
                        Trainer? trainer = property.GetValue(essence) as Trainer;

                        if (trainer != null)
                        {
                            Console.WriteLine($"Прикреплен к тренеру: {trainer.FullName} с ID: {trainer.Id}.");
                        }
                        else
                        {
                            Console.WriteLine("Не прикреплен к тренеру.");
                        }
                    }
                    else if (property.Name == "Athlete")
                    {
                        List<Athlete>? athletes = property.GetValue(essence) as List<Athlete>;

                        if (athletes != null && athletes.Count > 0)
                        {
                            Console.WriteLine("Прикрепленные атлеты:");

                            foreach (var athlete in athletes)
                            {
                                Console.WriteLine($"  - {athlete.FullName} с ID: {athlete.Id}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Не прикреплены атлеты.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"{displayName}: {property.GetValue(essence)}");
                    }
                }
            }
            else
            {
                Console.WriteLine("Объект не найден.");
            }
        }
        /// <summary>
        /// Очищает экран консоли и буфер прокрутки.
        /// Использует ANSI-последовательность, которая работает
        /// в Windows Terminal и классической консоли.
        /// </summary>
        static void ClearScreen()
        {
            Console.Write("\u001b[2J\u001b[3J\u001b[H");
        }
        /// <summary>
        /// Считывает целое число с консоли. Если ввод некорректен,
        /// выводит сообщение об ошибке и повторяет запрос.
        /// </summary>
        /// <param name="prompt">Текст приглашения к вводу.</param>
        /// <returns>Введённое целое число.</returns>
        static int ReadInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                if (int.TryParse(input, out int result))
                    return result;
                Console.WriteLine("Ошибка: введите целое число.");
            }
        }
        /// <summary>
        /// Считывает с консоли значение перечисления заданного типа.
        /// Пользователь вводит номер элемента, от 1 до количества значений.
        /// При некорректном вводе выводит сообщение и повторяет запрос.
        /// </summary>
        /// <typeparam name="T">Тип перечисления (Gendre или TrainingType).</typeparam>
        /// <param name="prompt">Текст приглашения к вводу.</param>
        /// <returns>Выбранное значение перечисления.</returns>
        static T ReadEnum<T>(string prompt) where T : struct, Enum
        {
            var values = Enum.GetValues<T>();
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                if (int.TryParse(input, out int number) && number >= 1 && number <= values.Length)
                    return values[number - 1];
                Console.WriteLine($"Ошибка: введите число от 1 до {values.Length}.");
            }
        }
        /// <summary>
        /// Точка входа в приложение. Инициализирует бизнес-логику,
        /// настраивает кодировку вывода и запускает главное меню
        /// с выбором роли: сотрудник или пользователь.
        /// </summary>
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Logics logics = new Logics();
            int password = 1234;

            while (true)
            {
                ClearScreen();

                Console.WriteLine("=== PRIME TIME ===");
                Console.WriteLine("1. Сотрудник");
                Console.WriteLine("2. Пользователь");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите роль: ");

                int role = ReadInt("");

                switch (role)
                {
                    case 1:
                        EmployeeMenu(logics, password);
                        break;

                    case 2:
                        UserMenu(logics);
                        break;

                    case 0:
                        return;
                }
            }
        }
        /// <summary>
        /// Меню сотрудника. Требует ввода пароля (3 попытки), после чего предоставляет доступ к CRUD-операциям над тренерами и атлетами.
        /// </summary>
        /// <param name="logics">Бизнес-логика приложения.</param>
        /// <param name="password">Пароль для доступа к меню.</param>
        static void EmployeeMenu(ILogics logics, int password)
        {
            int attempts = 3;
            bool license = false;

            while (!license && attempts > 0)
            {
                ClearScreen();

                Console.Write("Введите пароль: ");
                int inputPassword = ReadInt("");

                if (inputPassword == password)
                {
                    license = true;
                    Console.WriteLine("Добро пожаловать!");
                    Console.ReadKey();
                }
                else
                {
                    attempts--;
                    Console.WriteLine($"Неверный пароль. Осталось попыток: {attempts}");
                    Console.ReadKey();
                }
            }

            if (!license)
                return;

            while (true)
            {
                ClearScreen();

                Console.WriteLine("=== МЕНЮ СОТРУДНИКА ===");
                Console.WriteLine("1. Добавить тренера");
                Console.WriteLine("2. Добавить атлета");
                Console.WriteLine("3. Удалить тренера");
                Console.WriteLine("4. Удалить атлета");
                Console.WriteLine("5. Просмотреть тренера");
                Console.WriteLine("6. Просмотреть атлета");
                Console.WriteLine("7. Изменить тренера");
                Console.WriteLine("8. Изменить атлета");
                Console.WriteLine("9. Зарегистрировать атлета за тренером");
                Console.WriteLine("0. Назад");
                Console.Write("Выберите действие: ");

                int choice = ReadInt("");

                switch (choice)
                {
                    case 1:
                        ClearScreen();

                        Console.WriteLine("=== ДОБАВЛЕНИЕ ТРЕНЕРА ===");
                        Console.WriteLine("Введите данные тренера:");
                        Console.ReadKey();

                        ClearScreen();
                        Console.WriteLine("Введите ФИО:");
                        string fullname_trainer = Console.ReadLine();

                        ClearScreen();
                        Console.WriteLine("Выберите пол:");

                        foreach (var gendre_treiner in Enum.GetValues(typeof(Gendre)))
                        {
                            Console.WriteLine($"{(int)gendre_treiner + 1}. {gendre_treiner}");
                        }

                        Gendre gender_treiner = ReadEnum<Gendre>("Сделайте выбор: ");

                        ClearScreen();
                        Console.WriteLine("Введите тип тренировки:");

                        foreach (var trainingtype in Enum.GetValues(typeof(TrainingType)))
                        {
                            Console.WriteLine($"{(int)trainingtype + 1}. {trainingtype}");
                        }

                        TrainingType trainingType_treiner = ReadEnum<TrainingType>("Сделайте выбор: ");

                        ClearScreen();
                        Console.WriteLine("Введите возраст в годах:");
                        int age_treiner = ReadInt("");

                        ClearScreen();
                        Console.WriteLine("Введите стаж работы в годах:");
                        int workExperience_treiner = ReadInt("");

                        ClearScreen();
                        try
                        {
                            ViewEssence(logics.AddTrainer(fullname_trainer, gender_treiner, trainingType_treiner, age_treiner, workExperience_treiner));
                            Console.WriteLine();
                            Console.WriteLine("Тренер успешно добавлен!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка: {ex.Message}");
                        }

                        break;

                    case 2:
                        ClearScreen();

                        Console.WriteLine("=== ДОБАВЛЕНИЕ АТЛЕТА ===");
                        Console.WriteLine("Введите данные атлета:");
                        Console.ReadKey();

                        ClearScreen();
                        Console.WriteLine("Введите ФИО:");
                        string fullname_athlete = Console.ReadLine();

                        ClearScreen();
                        Console.WriteLine("Выберите пол:");

                        foreach (var gendre_athlete in Enum.GetValues(typeof(Gendre)))
                        {
                            Console.WriteLine($"{(int)gendre_athlete + 1}. {gendre_athlete}");
                        }

                        Gendre gender_athlete = ReadEnum<Gendre>("Сделайте выбор: ");

                        ClearScreen();
                        Console.WriteLine("Введите тип тренировки:");

                        foreach (var trainingtype_athlete in Enum.GetValues(typeof(TrainingType)))
                        {
                            Console.WriteLine($"{(int)trainingtype_athlete + 1}. {trainingtype_athlete}");
                        }

                        TrainingType trainingType_athlete = ReadEnum<TrainingType>("Сделайте выбор: ");

                        ClearScreen();
                        Console.WriteLine("Введите возраст в годах:");
                        int age_athlete = ReadInt("");

                        ClearScreen();
                        Console.WriteLine("Введите ваш рост в см:");
                        int height_athlete = ReadInt("");

                        ClearScreen();
                        Console.WriteLine("Введите ваш вес в кг:");
                        int weight_athlete = ReadInt("");

                        ClearScreen();
                        try
                        {
                            ViewEssence(logics.AddAthlete(fullname_athlete, gender_athlete, trainingType_athlete, age_athlete, height_athlete, weight_athlete));
                            Console.WriteLine();
                            Console.WriteLine("Атлет успешно добавлен!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка: {ex.Message}");
                        }

                        break;

                    case 3:
                        ClearScreen();

                        Console.WriteLine("=== УДАЛЕНИЕ ТРЕНЕРА ===");
                        foreach (var trainer_view in logics.BD_Trainer)
                        {
                            ViewEssence(trainer_view);
                        }
                        Console.WriteLine("Введите ID тренера для удаления:");

                        int deliteTrainerId = ReadInt("");

                        ClearScreen();

                        if (logics.RemoveTrainer(deliteTrainerId) == true)
                        {
                            Console.WriteLine("Тренер успешно удален!");
                        }
                        else
                        {
                            Console.WriteLine("Тренер не найден!");
                        }

                        break;

                    case 4:
                        ClearScreen();

                        Console.WriteLine("=== УДАЛЕНИЕ АТЛЕТА ===");
                        foreach (var athlete_view in logics.BD_Athlete)
                        {
                            ViewEssence(athlete_view);
                        }
                        Console.WriteLine("Введите ID атлета для удаления:");

                        int deleteAthleteId = ReadInt("");

                        ClearScreen();

                        if (logics.RemoveAthlete(deleteAthleteId) == true)
                        {
                            Console.WriteLine("Атлет успешно удален!");
                        }
                        else
                        {
                            Console.WriteLine("Атлет не найден!");
                        }

                        break;

                    case 5:
                        ClearScreen();

                        Console.WriteLine("=== ПРОСМОТР ТРЕНЕРОВ ===");
                        Console.WriteLine("1. Конкретного тренера по ID");
                        Console.WriteLine("2. Просмотр всех тренеров");

                        int viewChoice_trainer = ReadInt("");

                        ClearScreen();

                        switch (viewChoice_trainer)
                        {
                            case 1:
                                Console.WriteLine("Введите ID тренера для просмотра:");

                                int viewTrainerId = ReadInt("");

                                ClearScreen();
                                ViewEssence(logics.CheckTrainer(viewTrainerId));

                                break;

                            case 2:
                                Console.WriteLine("=== СПИСОК ВСЕХ ТРЕНЕРОВ ===");

                                if (logics.BD_Trainer.Count == 0)
                                {
                                    Console.WriteLine("Список тренеров пуст!");
                                    break;
                                }

                                foreach (var trainer_ed in logics.BD_Trainer)
                                {
                                    Console.WriteLine();
                                    ViewEssence(trainer_ed);
                                    Console.WriteLine("-------------------------");
                                }

                                break;
                        }

                        break;

                    case 6:
                        ClearScreen();

                        Console.WriteLine("=== ПРОСМОТР АТЛЕТОВ ===");
                        Console.WriteLine("1. Конкретного атлета по ID");
                        Console.WriteLine("2. Просмотр всех атлетов");

                        int viewChoice_athlete = ReadInt("");

                        ClearScreen();

                        switch (viewChoice_athlete)
                        {
                            case 1:
                                Console.WriteLine("Введите ID атлета для просмотра:");

                                int viewAthleteId = ReadInt("");

                                ClearScreen();
                                ViewEssence(logics.CheckAthlete(viewAthleteId));

                                break;

                            case 2:
                                Console.WriteLine("=== СПИСОК ВСЕХ АТЛЕТОВ ===");

                                if (logics.BD_Athlete.Count == 0)
                                {
                                    Console.WriteLine("Список атлетов пуст!");
                                    break;
                                }

                                foreach (var athletes in logics.BD_Athlete)
                                {
                                    Console.WriteLine();
                                    ViewEssence(athletes);
                                    Console.WriteLine("-------------------------");
                                }

                                break;
                        }

                        break;

                    case 7:
                        ClearScreen();

                        Console.WriteLine("=== ИЗМЕНЕНИЕ ТРЕНЕРА ===");
                        foreach (var trainer_view in logics.BD_Trainer)
                        {
                            ViewEssence(trainer_view);
                        }
                        Console.WriteLine("Введите ID тренера для изменения:");

                        int changeTrainerId = ReadInt("");

                        Trainer? trainer = logics.CheckTrainer(changeTrainerId);

                        if (trainer == null)
                        {
                            ClearScreen();
                            Console.WriteLine("Тренер не найден!");
                            break;
                        }

                        bool trainerExit = false;

                        while (!trainerExit)
                        {
                            ClearScreen();

                            Console.WriteLine("=== ИЗМЕНЕНИЕ ТРЕНЕРА ===");
                            ViewEssence(trainer);

                            Console.WriteLine();
                            Console.WriteLine("Какой параметр вы хотите изменить?");
                            Console.WriteLine("1. ФИО");
                            Console.WriteLine("2. Пол");
                            Console.WriteLine("3. Тип тренировки");
                            Console.WriteLine("4. Возраст");
                            Console.WriteLine("5. Стаж работы");
                            Console.WriteLine("6. Закрепленные атлеты");
                            Console.WriteLine("7. Закончить изменения");

                            int parametrSelection_trainer = ReadInt("");

                            switch (parametrSelection_trainer)
                            {
                                case 1:
                                    ClearScreen();

                                    Console.WriteLine("Введите измененное ФИО:");
                                    string fullname = Console.ReadLine();

                                    try
                                    {
                                        logics.UpdateInfoTrainer(trainer.Id, fullname, null, null, null, null, null, null);
                                        Console.WriteLine("ФИО успешно изменено!");
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Ошибка: {ex.Message}");
                                    }

                                    Console.ReadKey();

                                    break;

                                case 2:
                                    ClearScreen();

                                    Console.WriteLine("Выберите пол:");

                                    foreach (var gendre_trainer in Enum.GetValues(typeof(Gendre)))
                                    {
                                        Console.WriteLine($"{(int)gendre_trainer + 1}. {gendre_trainer}");
                                    }

                                    Gendre gendre = ReadEnum<Gendre>("Сделайте выбор: ");

                                    logics.UpdateInfoTrainer(trainer.Id, null, gendre, null, null, null, null, null);

                                    Console.WriteLine("Пол успешно изменен!");
                                    Console.ReadKey();

                                    break;

                                case 3:
                                    ClearScreen();

                                    Console.WriteLine("Выберите тип тренировки:");

                                    foreach (var trainingtype_trainer in Enum.GetValues(typeof(TrainingType)))
                                    {
                                        Console.WriteLine($"{(int)trainingtype_trainer + 1}. {trainingtype_trainer}");
                                    }

                                    TrainingType trainingType = ReadEnum<TrainingType>("Сделайте выбор: ");

                                    logics.UpdateInfoTrainer(trainer.Id, null, null, trainingType, null, null, null, null);

                                    Console.WriteLine("Тип тренировки успешно изменен!");
                                    Console.ReadKey();

                                    break;

                                case 4:
                                    ClearScreen();

                                    Console.WriteLine("Введите новый возраст:");
                                    int age = ReadInt("");

                                    try
                                    {
                                        logics.UpdateInfoTrainer(trainer.Id, null, null, null, age, null, null, null);
                                        Console.WriteLine("Возраст успешно изменен!");
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Ошибка: {ex.Message}");
                                    }

                                    Console.ReadKey();

                                    break;

                                case 5:
                                    ClearScreen();

                                    Console.WriteLine("Введите новый стаж работы:");
                                    int workExperience = ReadInt("");

                                    try
                                    {
                                        logics.UpdateInfoTrainer(trainer.Id, null, null, null, null, workExperience, null, null);
                                        Console.WriteLine("Стаж работы успешно изменен!");
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Ошибка: {ex.Message}");
                                    }

                                    Console.ReadKey();

                                    break;

                                case 6:
                                    bool athleteMenuExit = false;

                                    while (!athleteMenuExit)
                                    {
                                        ClearScreen();

                                        Console.WriteLine("=== ЗАКРЕПЛЕННЫЕ АТЛЕТЫ ===");
                                        ViewEssence(trainer);

                                        Console.WriteLine();
                                        Console.WriteLine("1. Добавить атлета");
                                        Console.WriteLine("2. Удалить атлета");
                                        Console.WriteLine("0. Назад");

                                        int athleteAction = ReadInt("");

                                        switch (athleteAction)
                                        {
                                            case 1:
                                                ClearScreen();

                                                Console.WriteLine("=== ДОБАВЛЕНИЕ АТЛЕТА К ТРЕНЕРУ ===");
                                                Console.WriteLine("Введите ID атлета:");

                                                int addAthleteId = ReadInt("");
                                                Athlete? addAthlete = logics.CheckAthlete(addAthleteId);

                                                if (addAthlete == null)
                                                {
                                                    Console.WriteLine("Атлет не найден!");
                                                    Console.ReadKey();
                                                    break;
                                                }

                                                if (trainer.Athlete.Contains(addAthlete))
                                                {
                                                    Console.WriteLine("Этот атлет уже закреплен за данным тренером!");
                                                    Console.ReadKey();
                                                    break;
                                                }

                                                List<Athlete> addList = new List<Athlete>();
                                                addList.Add(addAthlete);

                                                logics.UpdateInfoTrainer(trainer.Id, null, null, null, null, null, null, addList);

                                                Console.WriteLine("Атлет успешно закреплен за тренером!");
                                                Console.ReadKey();

                                                break;

                                            case 2:
                                                ClearScreen();

                                                Console.WriteLine("=== УДАЛЕНИЕ АТЛЕТА ОТ ТРЕНЕРА ===");
                                                Console.WriteLine("Введите ID атлета:");

                                                int deleteAthleteTrainerId = ReadInt("");
                                                Athlete? deleteAthlete = logics.CheckAthlete(deleteAthleteTrainerId);

                                                if (deleteAthlete == null)
                                                {
                                                    Console.WriteLine("Атлет не найден!");
                                                    Console.ReadKey();
                                                    break;
                                                }

                                                if (!trainer.Athlete.Contains(deleteAthlete))
                                                {
                                                    Console.WriteLine("Этот атлет не закреплен за данным тренером!");
                                                    Console.ReadKey();
                                                    break;
                                                }

                                                List<Athlete> deleteList = new List<Athlete>();
                                                deleteList.Add(deleteAthlete);

                                                logics.UpdateInfoTrainer(trainer.Id, null, null, null, null, null, deleteList, null);

                                                Console.WriteLine("Атлет успешно откреплен от тренера!");
                                                Console.ReadKey();

                                                break;

                                            case 0:
                                                athleteMenuExit = true;
                                                break;
                                        }
                                    }

                                    break;

                                case 7:
                                    trainerExit = true;
                                    break;
                            }
                        }

                        break;

                    case 8:
                        ClearScreen();

                        Console.WriteLine("=== ИЗМЕНЕНИЕ АТЛЕТА ===");
                        foreach (var athlete_view in logics.BD_Athlete)
                        {
                            ViewEssence(athlete_view);
                        }
                        Console.WriteLine("Введите ID атлета для изменения:");

                        int changeAthleteId = ReadInt("");

                        Athlete? athlete = logics.CheckAthlete(changeAthleteId);

                        if (athlete == null)
                        {
                            ClearScreen();
                            Console.WriteLine("Атлет не найден!");
                            break;
                        }

                        bool athleteExit = false;

                        while (!athleteExit)
                        {
                            ClearScreen();

                            Console.WriteLine("=== ИЗМЕНЕНИЕ АТЛЕТА ===");
                            ViewEssence(athlete);

                            Console.WriteLine();
                            Console.WriteLine("Какой параметр вы хотите изменить?");
                            Console.WriteLine("1. ФИО");
                            Console.WriteLine("2. Пол");
                            Console.WriteLine("3. Возраст");
                            Console.WriteLine("4. Рост");
                            Console.WriteLine("5. Вес");
                            Console.WriteLine("6. Тип тренировки");
                            Console.WriteLine("7. Тренер");
                            Console.WriteLine("8. Закончить изменения");

                            int parametrSelection_athlete = ReadInt("");

                            switch (parametrSelection_athlete)
                            {
                                case 1:
                                    ClearScreen();

                                    Console.WriteLine("Введите измененное ФИО:");
                                    string fullname = Console.ReadLine();

                                    try
                                    {
                                        logics.UpdateInfoAthlete(athlete.Id, fullname, null, null, null, null, null, null);
                                        Console.WriteLine("ФИО успешно изменено!");
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Ошибка: {ex.Message}");
                                    }

                                    Console.ReadKey();

                                    break;

                                case 2:
                                    ClearScreen();

                                    Console.WriteLine("Выберите пол:");

                                    foreach (var gendre_athlete in Enum.GetValues(typeof(Gendre)))
                                    {
                                        Console.WriteLine($"{(int)gendre_athlete + 1}. {gendre_athlete}");
                                    }

                                    Gendre gender_athletes = ReadEnum<Gendre>("Сделайте выбор: ");

                                    logics.UpdateInfoAthlete(athlete.Id, null, gender_athletes, null, null, null, null, null);

                                    Console.WriteLine("Пол успешно изменен!");
                                    Console.ReadKey();

                                    break;

                                case 3:
                                    ClearScreen();

                                    Console.WriteLine("Введите новый возраст:");
                                    int age = ReadInt("");

                                    try
                                    {
                                        logics.UpdateInfoAthlete(athlete.Id, null, null, age, null, null, null, null);
                                        Console.WriteLine("Возраст успешно изменен!");
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Ошибка: {ex.Message}");
                                    }

                                    Console.ReadKey();

                                    break;

                                case 4:
                                    ClearScreen();

                                    Console.WriteLine("Введите новый рост:");
                                    int height = ReadInt("");

                                    try
                                    {
                                        logics.UpdateInfoAthlete(athlete.Id, null, null, null, height, null, null, null);
                                        Console.WriteLine("Рост успешно изменен!");
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Ошибка: {ex.Message}");
                                    }

                                    Console.ReadKey();

                                    break;

                                case 5:
                                    ClearScreen();

                                    Console.WriteLine("Введите новый вес:");
                                    int weight = ReadInt("");

                                    try
                                    {
                                        logics.UpdateInfoAthlete(athlete.Id, null, null, null, null, weight, null, null);
                                        Console.WriteLine("Вес успешно изменен!");
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Ошибка: {ex.Message}");
                                    }

                                    Console.ReadKey();

                                    break;

                                case 6:
                                    ClearScreen();

                                    Console.WriteLine("Выберите тип тренировки:");

                                    foreach (var trainingtype_athlete in Enum.GetValues(typeof(TrainingType)))
                                    {
                                        Console.WriteLine($"{(int)trainingtype_athlete + 1}. {trainingtype_athlete}");
                                    }

                                    TrainingType trainingType_athletes = ReadEnum<TrainingType>("Сделайте выбор: ");

                                    logics.UpdateInfoAthlete(athlete.Id, null, null, null, null, null, trainingType_athletes, null);

                                    Console.WriteLine("Тип тренировки успешно изменен!");
                                    Console.ReadKey();

                                    break;

                                case 7:
                                    ClearScreen();

                                    Console.WriteLine("=== ВЫБОР ТРЕНЕРА ===");

                                    if (logics.BD_Trainer.Count == 0)
                                    {
                                        Console.WriteLine("В базе нет тренеров.");
                                        Console.ReadKey();
                                        break;
                                    }

                                    foreach (var trainerItem in logics.BD_Trainer)
                                    {
                                        Console.WriteLine($"ID: {trainerItem.Id} | {trainerItem.FullName} | {trainerItem.TrainingType}");
                                    }

                                    Console.WriteLine();
                                    Console.WriteLine("Введите ID тренера:");

                                    int trainerId = ReadInt("");
                                    Trainer? newTrainer = logics.CheckTrainer(trainerId);

                                    if (newTrainer == null)
                                    {
                                        Console.WriteLine("Тренер не найден!");
                                        Console.ReadKey();
                                        break;
                                    }

                                    logics.UpdateInfoAthlete(athlete.Id, null, null, null, null, null, null, newTrainer);

                                    Console.WriteLine("Тренер успешно изменен!");
                                    Console.ReadKey();

                                    break;

                                case 8:
                                    athleteExit = true;
                                    break;
                            }
                        }

                        break;

                    case 9:
                        ClearScreen();

                        Console.WriteLine("=== РЕГИСТРАЦИЯ АТЛЕТА ЗА ТРЕНЕРОМ ===");
                        foreach (var athlete_view in logics.BD_Athlete)
                        {
                            ViewEssence(athlete_view);
                        } 
                        Console.WriteLine("Введите ID атлета:");
                        int registrationAthleteId = ReadInt("");

                        Athlete? registrationAthlete = logics.CheckAthlete(registrationAthleteId);

                        if (registrationAthlete == null)
                        {
                            Console.WriteLine("Атлет не найден!");
                            break;
                        }
                        Console.Clear();

                        foreach (var trainer_view in logics.BD_Trainer)
                        {
                            ViewEssence(trainer_view);
                        }
                        Console.WriteLine("Введите ID тренера:");
                        int registrationTrainerId = ReadInt("");

                        Trainer? registrationTrainer = logics.CheckTrainer(registrationTrainerId);

                        if (registrationTrainer == null)
                        {
                            Console.WriteLine("Тренер не найден!");
                            break;
                        }

                        ClearScreen();

                        if (logics.Registration(registrationTrainer, registrationAthlete))
                        {
                            Console.WriteLine("Атлет успешно зарегистрирован за тренером!");
                        }
                        else
                        {
                            Console.WriteLine("Не удалось зарегистрировать атлета.");
                            Console.WriteLine("Возможно, атлет уже закреплен за тренером.");
                        }

                        break;

                    case 0:
                        return;
                }

                Console.ReadKey();
            }
        }
        /// <summary>
        /// Меню пользователя. Доступ к функциям:
        /// добавление атлета, регистрация за тренером, подбор персональной
        /// программы, подбор тренеров и просмотр рейтинга тренеров.
        /// </summary>
        /// <param name="logics">Бизнес-логика приложения.</param>
        static void UserMenu(ILogics logics)
        {
            while (true)
            {
                ClearScreen();

                Console.WriteLine("=== МЕНЮ ПОЛЬЗОВАТЕЛЯ ===");
                Console.WriteLine("1. Добавить атлета");
                Console.WriteLine("2. Регистрация за тренером");
                Console.WriteLine("3. Персональная тренировка");
                Console.WriteLine("4. Подбор тренера");
                Console.WriteLine("5. Рейтинг тренеров");
                Console.WriteLine("0. Назад");
                Console.Write("Выберите действие: ");

                int choice = ReadInt("");

                switch (choice)
                {
                    case 1:
                        ClearScreen();

                        Console.WriteLine("=== ДОБАВЛЕНИЕ АТЛЕТА ===");

                        Console.WriteLine("Введите ФИО:");
                        string fullname = Console.ReadLine();

                        ClearScreen();
                        Console.WriteLine("Выберите пол:");

                        foreach (var gendre in Enum.GetValues(typeof(Gendre)))
                        {
                            Console.WriteLine($"{(int)gendre + 1}. {gendre}");
                        }

                        Gendre gender = ReadEnum<Gendre>("Сделайте выбор: ");

                        ClearScreen();
                        Console.WriteLine("Выберите тип тренировки:");

                        foreach (var trainingType in Enum.GetValues(typeof(TrainingType)))
                        {
                            Console.WriteLine($"{(int)trainingType + 1}. {trainingType}");
                        }

                        TrainingType type = ReadEnum<TrainingType>("Сделайте выбор: ");

                        ClearScreen();
                        Console.WriteLine("Введите возраст:");
                        int age = ReadInt("");

                        ClearScreen();
                        Console.WriteLine("Введите рост в см:");
                        int height = ReadInt("");

                        ClearScreen();
                        Console.WriteLine("Введите вес в кг:");
                        int weight = ReadInt("");

                        ClearScreen();

                        try
                        {
                            ViewEssence(logics.AddAthlete(fullname, gender, type, age, height, weight));
                            Console.WriteLine();
                            Console.WriteLine("Атлет успешно добавлен!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка: {ex.Message}");
                        }

                        break;

                    case 2:
                        ClearScreen();

                        Console.WriteLine("=== РЕГИСТРАЦИЯ ЗА ТРЕНЕРОМ ===");

                        Console.WriteLine("Введите ID атлета:");
                        int athleteId = ReadInt("");

                        Athlete? athlete = logics.CheckAthlete(athleteId);

                        if (athlete == null)
                        {
                            Console.WriteLine("Атлет не найден!");
                            break;
                        }

                        Console.WriteLine("Введите ID тренера:");
                        int trainerId = ReadInt("");

                        Trainer? trainer = logics.CheckTrainer(trainerId);

                        if (trainer == null)
                        {
                            Console.WriteLine("Тренер не найден!");
                            break;
                        }

                        ClearScreen();

                        if (logics.Registration(trainer, athlete))
                        {
                            Console.WriteLine("Регистрация прошла успешно!");
                        }
                        else
                        {
                            Console.WriteLine("Не удалось зарегистрировать атлета.");
                            Console.WriteLine("Возможно, атлет уже закреплен за тренером.");
                        }

                        break;

                    case 3:
                        ClearScreen();

                        Console.WriteLine("=== ПЕРСОНАЛЬНАЯ ТРЕНИРОВКА ===");

                        Console.WriteLine("Введите ID атлета:");
                        int personalAthleteId = ReadInt("");

                        Athlete? personalAthlete = logics.CheckAthlete(personalAthleteId);

                        if (personalAthlete == null)
                        {
                            Console.WriteLine("Атлет не найден!");
                            break;
                        }

                        ClearScreen();
                        Console.WriteLine(logics.PersonalTraining(personalAthlete));

                        break;

                    case 4:
                        ClearScreen();

                        Console.WriteLine("=== ПОДБОР ТРЕНЕРА ===");

                        Console.WriteLine("Введите ID атлета:");
                        int filterAthleteId = ReadInt("");

                        Athlete? filterAthlete = logics.CheckAthlete(filterAthleteId);

                        if (filterAthlete == null)
                        {
                            Console.WriteLine("Атлет не найден!");
                            break;
                        }

                        List<Trainer> trainers = logics.PersonalFilterTrainers(filterAthlete);

                        ClearScreen();

                        if (trainers.Count == 0)
                        {
                            Console.WriteLine("Подходящих тренеров не найдено.");
                            break;
                        }

                        Console.WriteLine("=== ПОДХОДЯЩИЕ ТРЕНЕРЫ ===");

                        foreach (var trainerItem in trainers)
                        {
                            ViewEssence(trainerItem);
                            Console.WriteLine("-------------------------");
                        }

                        break;

                    case 5:
                        ClearScreen();

                        Console.WriteLine("=== РЕЙТИНГ ТРЕНЕРОВ ===");

                        List<Trainer> rating = logics.RateTrainers();

                        if (rating.Count == 0)
                        {
                            Console.WriteLine("Тренеров нет.");
                            break;
                        }

                        int number = 1;

                        foreach (var trainerItem in rating)
                        {
                            Console.WriteLine($"{number}. {trainerItem.FullName}");
                            Console.WriteLine($"ID: {trainerItem.Id}");
                            Console.WriteLine($"Возраст: {trainerItem.Age}");
                            Console.WriteLine($"Стаж: {trainerItem.WorkExperience}");
                            Console.WriteLine($"Закреплено атлетов: {trainerItem.Athlete.Count}");
                            Console.WriteLine($"Тип тренировки: {trainerItem.TrainingType}");
                            Console.WriteLine("-------------------------");

                            number++;
                        }

                        break;

                    case 0:
                        return;
                }

                Console.ReadKey();
            }
        }
    }
}