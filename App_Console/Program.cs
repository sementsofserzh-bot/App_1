using App_Model_Essence;
using App_Model_TestLogics;
using App_Model_Logics;

namespace App_Console
{
    class Program
    {
        static void ViewEssence<T>(T essence)
        {
            if (essence != null)
            {
                foreach (var property in typeof(T).GetProperties())
                {
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
                        Console.WriteLine($"{property.Name}: {property.GetValue(essence)}");
                    }
                }
            }
            else
            {
                Console.WriteLine("Объект не найден.");
            }
        }

        static void Main()
        {
            Logics logics = new Logics();
            int password = 1234;

            while (true)
            {
                Console.Clear();

                Console.WriteLine("=== PRIME TIME ===");
                Console.WriteLine("1. Сотрудник");
                Console.WriteLine("2. Пользователь");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите роль: ");

                int role = Convert.ToInt32(Console.ReadLine());

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

        static void EmployeeMenu(ILogics logics, int password)
        {
            int attempts = 3;
            bool license = false;

            while (!license && attempts > 0)
            {
                Console.Clear();

                Console.Write("Введите пароль: ");
                int inputPassword = Convert.ToInt32(Console.ReadLine());

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
                Console.Clear();

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

                int choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Console.Clear();

                        Console.WriteLine("=== ДОБАВЛЕНИЕ ТРЕНЕРА ===");
                        Console.WriteLine("Введите данные тренера:");
                        Console.ReadKey();

                        Console.Clear();
                        Console.WriteLine("Введите ФИО:");
                        string fullname_trainer = Console.ReadLine();

                        Console.Clear();
                        Console.WriteLine("Выберите пол:");

                        foreach (var gendre_treiner in Enum.GetValues(typeof(Gendre)))
                        {
                            Console.WriteLine($"{(int)gendre_treiner + 1}. {gendre_treiner}");
                        }

                        Console.WriteLine("Сделайте выбор:");
                        int gendreChoice = Convert.ToInt32(Console.ReadLine());
                        Gendre gender_treiner = (Gendre)(gendreChoice - 1);

                        Console.Clear();
                        Console.WriteLine("Введите тип тренировки:");

                        foreach (var trainingtype in Enum.GetValues(typeof(TrainingType)))
                        {
                            Console.WriteLine($"{(int)trainingtype + 1}. {trainingtype}");
                        }

                        Console.WriteLine("Сделайте выбор:");
                        int trainingTypeChoice_trainer = Convert.ToInt32(Console.ReadLine());
                        TrainingType trainingType_treiner = (TrainingType)(trainingTypeChoice_trainer - 1);

                        Console.Clear();
                        Console.WriteLine("Введите возраст в годах:");
                        int age_treiner = Convert.ToInt32(Console.ReadLine());

                        Console.Clear();
                        Console.WriteLine("Введите стаж работы в годах:");
                        int workExperience_treiner = Convert.ToInt32(Console.ReadLine());

                        Console.Clear();
                        ViewEssence(logics.AddTrainer(fullname_trainer, gender_treiner, trainingType_treiner, age_treiner, workExperience_treiner));
                        Console.WriteLine();
                        Console.WriteLine("Тренер успешно добавлен!");

                        break;

                    case 2:
                        Console.Clear();

                        Console.WriteLine("=== ДОБАВЛЕНИЕ АТЛЕТА ===");
                        Console.WriteLine("Введите данные атлета:");
                        Console.ReadKey();

                        Console.Clear();
                        Console.WriteLine("Введите ФИО:");
                        string fullname_athlete = Console.ReadLine();

                        Console.Clear();
                        Console.WriteLine("Выберите пол:");

                        foreach (var gendre_athlete in Enum.GetValues(typeof(Gendre)))
                        {
                            Console.WriteLine($"{(int)gendre_athlete + 1}. {gendre_athlete}");
                        }

                        Console.WriteLine("Сделайте выбор:");
                        int gendreChoice_athlete = Convert.ToInt32(Console.ReadLine());
                        Gendre gender_athlete = (Gendre)(gendreChoice_athlete - 1);

                        Console.Clear();
                        Console.WriteLine("Введите тип тренировки:");

                        foreach (var trainingtype_athlete in Enum.GetValues(typeof(TrainingType)))
                        {
                            Console.WriteLine($"{(int)trainingtype_athlete + 1}. {trainingtype_athlete}");
                        }

                        Console.WriteLine("Сделайте выбор:");
                        int trainingTypeChoice_athlete = Convert.ToInt32(Console.ReadLine());
                        TrainingType trainingType_athlete = (TrainingType)(trainingTypeChoice_athlete - 1);

                        Console.Clear();
                        Console.WriteLine("Введите возраст в годах:");
                        int age_athlete = Convert.ToInt32(Console.ReadLine());

                        Console.Clear();
                        Console.WriteLine("Введите ваш рост в см:");
                        int height_athlete = Convert.ToInt32(Console.ReadLine());

                        Console.Clear();
                        Console.WriteLine("Введите ваш вес в кг:");
                        int weight_athlete = Convert.ToInt32(Console.ReadLine());

                        Console.Clear();
                        ViewEssence(logics.AddAthlete(fullname_athlete, gender_athlete, trainingType_athlete, age_athlete, height_athlete, weight_athlete));
                        Console.WriteLine();
                        Console.WriteLine("Атлет успешно добавлен!");

                        break;

                    case 3:
                        Console.Clear();

                        Console.WriteLine("=== УДАЛЕНИЕ ТРЕНЕРА ===");
                        Console.WriteLine("Введите ID тренера для удаления:");

                        int deliteTrainerId = Convert.ToInt32(Console.ReadLine());

                        Console.Clear();

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
                        Console.Clear();

                        Console.WriteLine("=== УДАЛЕНИЕ АТЛЕТА ===");
                        Console.WriteLine("Введите ID атлета для удаления:");

                        int deleteAthleteId = Convert.ToInt32(Console.ReadLine());

                        Console.Clear();

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
                        Console.Clear();

                        Console.WriteLine("=== ПРОСМОТР ТРЕНЕРОВ ===");
                        Console.WriteLine("1. Конкретного тренера по ID");
                        Console.WriteLine("2. Просмотр всех тренеров");

                        int viewChoice_trainer = Convert.ToInt32(Console.ReadLine());

                        Console.Clear();

                        switch (viewChoice_trainer)
                        {
                            case 1:
                                Console.WriteLine("Введите ID тренера для просмотра:");

                                int viewTrainerId = Convert.ToInt32(Console.ReadLine());

                                Console.Clear();
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
                        Console.Clear();

                        Console.WriteLine("=== ПРОСМОТР АТЛЕТОВ ===");
                        Console.WriteLine("1. Конкретного атлета по ID");
                        Console.WriteLine("2. Просмотр всех атлетов");

                        int viewChoice_athlete = Convert.ToInt32(Console.ReadLine());

                        Console.Clear();

                        switch (viewChoice_athlete)
                        {
                            case 1:
                                Console.WriteLine("Введите ID атлета для просмотра:");

                                int viewAthleteId = Convert.ToInt32(Console.ReadLine());

                                Console.Clear();
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
                        Console.Clear();

                        Console.WriteLine("=== ИЗМЕНЕНИЕ ТРЕНЕРА ===");
                        Console.WriteLine("Введите ID тренера для изменения:");

                        int changeTrainerId = Convert.ToInt32(Console.ReadLine());

                        Trainer? trainer = logics.CheckTrainer(changeTrainerId);

                        if (trainer == null)
                        {
                            Console.Clear();
                            Console.WriteLine("Тренер не найден!");
                            break;
                        }

                        bool trainerExit = false;

                        while (!trainerExit)
                        {
                            Console.Clear();

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

                            int parametrSelection_trainer = Convert.ToInt32(Console.ReadLine());

                            switch (parametrSelection_trainer)
                            {
                                case 1:
                                    Console.Clear();

                                    Console.WriteLine("Введите измененное ФИО:");
                                    string fullname = Console.ReadLine();

                                    logics.UpdateInfoTrainer(trainer.Id, fullname, null, null, null, null, null, null);

                                    Console.WriteLine("ФИО успешно изменено!");
                                    Console.ReadKey();

                                    break;

                                case 2:
                                    Console.Clear();

                                    Console.WriteLine("Выберите пол:");

                                    foreach (var gendre_trainer in Enum.GetValues(typeof(Gendre)))
                                    {
                                        Console.WriteLine($"{(int)gendre_trainer + 1}. {gendre_trainer}");
                                    }

                                    Console.WriteLine("Сделайте выбор:");

                                    int gendreChoice_trainer = Convert.ToInt32(Console.ReadLine());
                                    Gendre gendre = (Gendre)(gendreChoice_trainer - 1);

                                    logics.UpdateInfoTrainer(trainer.Id, null, gendre, null, null, null, null, null);

                                    Console.WriteLine("Пол успешно изменен!");
                                    Console.ReadKey();

                                    break;

                                case 3:
                                    Console.Clear();

                                    Console.WriteLine("Выберите тип тренировки:");

                                    foreach (var trainingtype_trainer in Enum.GetValues(typeof(TrainingType)))
                                    {
                                        Console.WriteLine($"{(int)trainingtype_trainer + 1}. {trainingtype_trainer}");
                                    }

                                    Console.WriteLine("Сделайте выбор:");

                                    int trainingTypeChoice = Convert.ToInt32(Console.ReadLine());
                                    TrainingType trainingType = (TrainingType)(trainingTypeChoice - 1);

                                    logics.UpdateInfoTrainer(trainer.Id, null, null, trainingType, null, null, null, null);

                                    Console.WriteLine("Тип тренировки успешно изменен!");
                                    Console.ReadKey();

                                    break;

                                case 4:
                                    Console.Clear();

                                    Console.WriteLine("Введите новый возраст:");
                                    int age = Convert.ToInt32(Console.ReadLine());

                                    logics.UpdateInfoTrainer(trainer.Id, null, null, null, age, null, null, null);

                                    Console.WriteLine("Возраст успешно изменен!");
                                    Console.ReadKey();

                                    break;

                                case 5:
                                    Console.Clear();

                                    Console.WriteLine("Введите новый стаж работы:");
                                    int workExperience = Convert.ToInt32(Console.ReadLine());

                                    logics.UpdateInfoTrainer(trainer.Id, null, null, null, null, workExperience, null, null);

                                    Console.WriteLine("Стаж работы успешно изменен!");
                                    Console.ReadKey();

                                    break;

                                case 6:
                                    bool athleteMenuExit = false;

                                    while (!athleteMenuExit)
                                    {
                                        Console.Clear();

                                        Console.WriteLine("=== ЗАКРЕПЛЕННЫЕ АТЛЕТЫ ===");
                                        ViewEssence(trainer);

                                        Console.WriteLine();
                                        Console.WriteLine("1. Добавить атлета");
                                        Console.WriteLine("2. Удалить атлета");
                                        Console.WriteLine("0. Назад");

                                        int athleteAction = Convert.ToInt32(Console.ReadLine());

                                        switch (athleteAction)
                                        {
                                            case 1:
                                                Console.Clear();

                                                Console.WriteLine("=== ДОБАВЛЕНИЕ АТЛЕТА К ТРЕНЕРУ ===");
                                                Console.WriteLine("Введите ID атлета:");

                                                int addAthleteId = Convert.ToInt32(Console.ReadLine());
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
                                                Console.Clear();

                                                Console.WriteLine("=== УДАЛЕНИЕ АТЛЕТА ОТ ТРЕНЕРА ===");
                                                Console.WriteLine("Введите ID атлета:");

                                                int deleteAthleteTrainerId = Convert.ToInt32(Console.ReadLine());
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
                        Console.Clear();

                        Console.WriteLine("=== ИЗМЕНЕНИЕ АТЛЕТА ===");
                        Console.WriteLine("Введите ID атлета для изменения:");

                        int changeAthleteId = Convert.ToInt32(Console.ReadLine());

                        Athlete? athlete = logics.CheckAthlete(changeAthleteId);

                        if (athlete == null)
                        {
                            Console.Clear();
                            Console.WriteLine("Атлет не найден!");
                            break;
                        }

                        bool athleteExit = false;

                        while (!athleteExit)
                        {
                            Console.Clear();

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

                            int parametrSelection_athlete = Convert.ToInt32(Console.ReadLine());

                            switch (parametrSelection_athlete)
                            {
                                case 1:
                                    Console.Clear();

                                    Console.WriteLine("Введите измененное ФИО:");
                                    string fullname = Console.ReadLine();

                                    logics.UpdateInfoAthlete(athlete.Id, fullname, null, null, null, null, null, null);

                                    Console.WriteLine("ФИО успешно изменено!");
                                    Console.ReadKey();

                                    break;

                                case 2:
                                    Console.Clear();

                                    Console.WriteLine("Выберите пол:");

                                    foreach (var gendre_athlete in Enum.GetValues(typeof(Gendre)))
                                    {
                                        Console.WriteLine($"{(int)gendre_athlete + 1}. {gendre_athlete}");
                                    }

                                    Console.WriteLine("Сделайте выбор:");

                                    int gendreChoice_athletes = Convert.ToInt32(Console.ReadLine());
                                    Gendre gender_athletes = (Gendre)(gendreChoice_athletes - 1);

                                    logics.UpdateInfoAthlete(athlete.Id, null, gender_athletes, null, null, null, null, null);

                                    Console.WriteLine("Пол успешно изменен!");
                                    Console.ReadKey();

                                    break;

                                case 3:
                                    Console.Clear();

                                    Console.WriteLine("Введите новый возраст:");
                                    int age = Convert.ToInt32(Console.ReadLine());

                                    logics.UpdateInfoAthlete(athlete.Id, null, null, age, null, null, null, null);

                                    Console.WriteLine("Возраст успешно изменен!");
                                    Console.ReadKey();

                                    break;

                                case 4:
                                    Console.Clear();

                                    Console.WriteLine("Введите новый рост:");
                                    int height = Convert.ToInt32(Console.ReadLine());

                                    logics.UpdateInfoAthlete(athlete.Id, null, null, null, height, null, null, null);

                                    Console.WriteLine("Рост успешно изменен!");
                                    Console.ReadKey();

                                    break;

                                case 5:
                                    Console.Clear();

                                    Console.WriteLine("Введите новый вес:");
                                    int weight = Convert.ToInt32(Console.ReadLine());

                                    logics.UpdateInfoAthlete(athlete.Id, null, null, null, null, weight, null, null);

                                    Console.WriteLine("Вес успешно изменен!");
                                    Console.ReadKey();

                                    break;

                                case 6:
                                    Console.Clear();

                                    Console.WriteLine("Выберите тип тренировки:");

                                    foreach (var trainingtype_athlete in Enum.GetValues(typeof(TrainingType)))
                                    {
                                        Console.WriteLine($"{(int)trainingtype_athlete + 1}. {trainingtype_athlete}");
                                    }

                                    Console.WriteLine("Сделайте выбор:");

                                    int trainingTypeChoice_athletes = Convert.ToInt32(Console.ReadLine());
                                    TrainingType trainingType_athletes = (TrainingType)(trainingTypeChoice_athletes - 1);

                                    logics.UpdateInfoAthlete(athlete.Id, null, null, null, null, null, trainingType_athletes, null);

                                    Console.WriteLine("Тип тренировки успешно изменен!");
                                    Console.ReadKey();

                                    break;

                                case 7:
                                    Console.Clear();

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

                                    int trainerId = Convert.ToInt32(Console.ReadLine());
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
                        Console.Clear();

                        Console.WriteLine("=== РЕГИСТРАЦИЯ АТЛЕТА ЗА ТРЕНЕРОМ ===");

                        Console.WriteLine("Введите ID атлета:");
                        int registrationAthleteId = Convert.ToInt32(Console.ReadLine());

                        Athlete? registrationAthlete = logics.CheckAthlete(registrationAthleteId);

                        if (registrationAthlete == null)
                        {
                            Console.WriteLine("Атлет не найден!");
                            break;
                        }

                        Console.WriteLine("Введите ID тренера:");
                        int registrationTrainerId = Convert.ToInt32(Console.ReadLine());

                        Trainer? registrationTrainer = logics.CheckTrainer(registrationTrainerId);

                        if (registrationTrainer == null)
                        {
                            Console.WriteLine("Тренер не найден!");
                            break;
                        }

                        Console.Clear();

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

        static void UserMenu(ILogics logics)
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

                int choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Console.Clear();

                        Console.WriteLine("=== ДОБАВЛЕНИЕ АТЛЕТА ===");

                        Console.WriteLine("Введите ФИО:");
                        string fullname = Console.ReadLine();

                        Console.Clear();
                        Console.WriteLine("Выберите пол:");

                        foreach (var gendre in Enum.GetValues(typeof(Gendre)))
                        {
                            Console.WriteLine($"{(int)gendre + 1}. {gendre}");
                        }

                        Console.WriteLine("Сделайте выбор:");
                        int gendreChoice = Convert.ToInt32(Console.ReadLine());
                        Gendre gender = (Gendre)(gendreChoice - 1);

                        Console.Clear();
                        Console.WriteLine("Выберите тип тренировки:");

                        foreach (var trainingType in Enum.GetValues(typeof(TrainingType)))
                        {
                            Console.WriteLine($"{(int)trainingType + 1}. {trainingType}");
                        }

                        Console.WriteLine("Сделайте выбор:");
                        int trainingChoice = Convert.ToInt32(Console.ReadLine());
                        TrainingType type = (TrainingType)(trainingChoice - 1);

                        Console.Clear();
                        Console.WriteLine("Введите возраст:");
                        int age = Convert.ToInt32(Console.ReadLine());

                        Console.Clear();
                        Console.WriteLine("Введите рост в см:");
                        int height = Convert.ToInt32(Console.ReadLine());

                        Console.Clear();
                        Console.WriteLine("Введите вес в кг:");
                        int weight = Convert.ToInt32(Console.ReadLine());

                        Console.Clear();

                        ViewEssence(logics.AddAthlete(fullname, gender, type, age, height, weight));

                        Console.WriteLine();
                        Console.WriteLine("Атлет успешно добавлен!");

                        break;

                    case 2:
                        Console.Clear();

                        Console.WriteLine("=== РЕГИСТРАЦИЯ ЗА ТРЕНЕРОМ ===");

                        Console.WriteLine("Введите ID атлета:");
                        int athleteId = Convert.ToInt32(Console.ReadLine());

                        Athlete? athlete = logics.CheckAthlete(athleteId);

                        if (athlete == null)
                        {
                            Console.WriteLine("Атлет не найден!");
                            break;
                        }

                        Console.WriteLine("Введите ID тренера:");
                        int trainerId = Convert.ToInt32(Console.ReadLine());

                        Trainer? trainer = logics.CheckTrainer(trainerId);

                        if (trainer == null)
                        {
                            Console.WriteLine("Тренер не найден!");
                            break;
                        }

                        Console.Clear();

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
                        Console.Clear();

                        Console.WriteLine("=== ПЕРСОНАЛЬНАЯ ТРЕНИРОВКА ===");

                        Console.WriteLine("Введите ID атлета:");
                        int personalAthleteId = Convert.ToInt32(Console.ReadLine());

                        Athlete? personalAthlete = logics.CheckAthlete(personalAthleteId);

                        if (personalAthlete == null)
                        {
                            Console.WriteLine("Атлет не найден!");
                            break;
                        }

                        Console.Clear();
                        Console.WriteLine(logics.PersonalTraining(personalAthlete));

                        break;

                    case 4:
                        Console.Clear();

                        Console.WriteLine("=== ПОДБОР ТРЕНЕРА ===");

                        Console.WriteLine("Введите ID атлета:");
                        int filterAthleteId = Convert.ToInt32(Console.ReadLine());

                        Athlete? filterAthlete = logics.CheckAthlete(filterAthleteId);

                        if (filterAthlete == null)
                        {
                            Console.WriteLine("Атлет не найден!");
                            break;
                        }

                        List<Trainer> trainers = logics.PersonalFilterTrainers(filterAthlete);

                        Console.Clear();

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
                        Console.Clear();

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