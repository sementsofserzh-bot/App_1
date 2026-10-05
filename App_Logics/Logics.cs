using System;
using System.Collections.Generic;
using System.Linq;
using Contracts;

namespace App_Model
{
    /// <summary>
    /// Реализует бизнес-логику приложения спортивного зала.
    /// Для доступа к данным использует IRepository,
    /// поэтому не зависит от EF или Dapper.
    /// </summary>
    public class Logics : ILogics
    {
        private readonly IRepository<Trainer> trainerRepository;
        private readonly IRepository<Athlete> athleteRepository;

        /// <summary>
        /// Возвращает всех тренеров из репозитория.
        /// Оставлено для совместимости с существующим UI.
        /// </summary>
        public List<Trainer> BD_Trainer
        {
            get => trainerRepository.List().ToList();
        }

        /// <summary>
        /// Возвращает всех атлетов из репозитория.
        /// Оставлено для совместимости с существующим UI.
        /// </summary>
        public List<Athlete> BD_Athlete
        {
            get => athleteRepository.List().ToList();
        }

        /// <summary>
        /// Создаёт объект бизнес-логики с переданными репозиториями.
        /// </summary>
        public Logics(
            IRepository<Trainer> trainerRepository,
            IRepository<Athlete> athleteRepository)
        {
            this.trainerRepository = trainerRepository;
            this.athleteRepository = athleteRepository;

            if (!trainerRepository.List().Any() &&
                !athleteRepository.List().Any())
            {
                SeedInitialData();
            }
        }

        /// <summary>
        /// Заполняет пустую базу начальными данными.
        /// </summary>
        private void SeedInitialData()
        {
            var t1 = AddTrainer(
                "Жежбекович Кайран Альбертович",
                Gendre.М,
                TrainingType.М_Силовая,
                38,
                12);

            var t2 = AddTrainer(
                "Галькова Галина Семёновна",
                Gendre.Ж,
                TrainingType.Ж_Выносливость,
                29,
                6);

            var t3 = AddTrainer(
                "Степанова Виктория Олеговна",
                Gendre.М,
                TrainingType.М_Гибкость,
                45,
                18);

            var t4 = AddTrainer(
                "Трифонова Алена Александровна",
                Gendre.Ж,
                TrainingType.Ж_Гибкость,
                40,
                18);

            AddTrainer(
                "Семенцов Сергей Витальевич",
                Gendre.М,
                TrainingType.Ж_Гибкость,
                50,
                20);

            var a1 = AddAthlete(
                "Корецкий Глеб Александрович",
                Gendre.М,
                TrainingType.М_Силовая,
                16,
                185,
                63);

            var a2 = AddAthlete(
                "Зайцева Алина Максимовна",
                Gendre.Ж,
                TrainingType.Ж_Силовая,
                20,
                165,
                55);

            var a3 = AddAthlete(
                "Смаев Геннадий Викторович",
                Gendre.М,
                TrainingType.М_Гибкость,
                32,
                178,
                95);

            var a4 = AddAthlete(
                "Павлова Екатерина Дмитриевна",
                Gendre.Ж,
                TrainingType.Ж_Выносливость,
                24,
                170,
                58);

            var a5 = AddAthlete(
                "Федоров Егор Романович",
                Gendre.М,
                TrainingType.М_Выносливость,
                15,
                172,
                60);

            var a6 = AddAthlete(
                "Сир Люлю Кебаб",
                Gendre.Ж,
                TrainingType.Ж_Гибкость,
                42,
                162,
                68);

            var a7 = AddAthlete(
                "Ремнев Павел Павлович",
                Gendre.М,
                TrainingType.М_Гибкость,
                55,
                180,
                85);

            var a8 = AddAthlete(
                "Козлова София Евгеньевна",
                Gendre.Ж,
                TrainingType.Ж_Выносливость,
                22,
                168,
                54);

            Registration(t1, a1);
            Registration(t1, a2);

            Registration(t2, a4);
            Registration(t2, a6);
            Registration(t2, a8);

            Registration(t3, a3);
            Registration(t3, a7);

            Registration(t4, a5);
        }

        public Trainer AddTrainer(
            string fullname,
            Gendre gendre,
            TrainingType trainingType,
            int age,
            int workExperience)
        {
            if (string.IsNullOrWhiteSpace(fullname))
                throw new ArgumentException("Имя не может быть пустым");

            if (fullname.Any(char.IsDigit))
                throw new ArgumentException("Имя не может содержать цифры");

            if (age < 18 || age > 120)
                throw new ArgumentException("Некорректный возраст");

            if (workExperience < 0 || workExperience > age - 18)
                throw new ArgumentException("Некорректный опыт работы");

            Trainer trainer = new Trainer
            {
                FullName = fullname,
                Gendre = gendre,
                TrainingType = trainingType,
                Age = age,
                WorkExperience = workExperience
            };

            trainerRepository.Add(trainer);

            return trainer;
        }

        public Athlete AddAthlete(
            string fullname,
            Gendre gendre,
            TrainingType trainingType,
            int age,
            int height,
            int weight)
        {
            if (string.IsNullOrWhiteSpace(fullname))
                throw new ArgumentException("Имя не может быть пустым");

            if (fullname.Any(char.IsDigit))
                throw new ArgumentException("Имя не может содержать цифры");

            if (age < 14 || age > 120)
                throw new ArgumentException("Некорректный возраст");

            if (height <= 0 || height > 300)
                throw new ArgumentException("Некорректный рост");

            if (weight <= 0 || weight > 1000)
                throw new ArgumentException("Некорректный вес");

            Athlete athlete = new Athlete
            {
                FullName = fullname,
                Gendre = gendre,
                TrainingType = trainingType,
                Age = age,
                Height = height,
                Weight = weight
            };

            athleteRepository.Add(athlete);

            return athlete;
        }

        public bool? RemoveTrainer(int id)
        {
            Trainer? trainer = trainerRepository.ReadById(id);

            if (trainer == null)
                return false;

            List<Athlete> athletes = athleteRepository
                .List()
                .Where(a =>
                    a.trainer != null &&
                    a.trainer.Id == id)
                .ToList();

            foreach (Athlete athlete in athletes)
            {
                athlete.trainer = null;
                athleteRepository.Update(athlete);
            }

            trainerRepository.Delete(id);

            return true;
        }

        public bool? RemoveAthlete(int id)
        {
            Athlete? athlete = athleteRepository.ReadById(id);

            if (athlete == null)
                return false;

            athleteRepository.Delete(id);

            return true;
        }

        public Trainer? CheckTrainer(int id)
        {
            return trainerRepository.ReadById(id);
        }

        public Athlete? CheckAthlete(int id)
        {
            return athleteRepository.ReadById(id);
        }

        public Trainer? UpdateInfoTrainer(
            int id,
            string? fullname,
            Gendre? gendre,
            TrainingType? trainingType,
            int? age,
            int? workExperience,
            List<Athlete>? deleteathlete,
            List<Athlete>? addathlete)
        {
            Trainer? chosenTrainer =
                trainerRepository.ReadById(id);

            if (chosenTrainer == null)
                return null;

            int finalAge =
                age ?? chosenTrainer.Age;

            int finalExperience =
                workExperience ?? chosenTrainer.WorkExperience;

            if (finalAge < 18 || finalAge > 120)
                throw new ArgumentException("Некорректный возраст");

            if (finalExperience < 0 ||
                finalExperience > finalAge - 18)
            {
                throw new ArgumentException(
                    "Некорректный опыт работы");
            }

            if (fullname != null)
            {
                if (string.IsNullOrWhiteSpace(fullname) ||
                    fullname.Any(char.IsDigit))
                {
                    throw new ArgumentException(
                        "Некорректное ФИО");
                }

                chosenTrainer.FullName = fullname;
            }

            if (gendre.HasValue)
                chosenTrainer.Gendre = gendre.Value;

            if (trainingType.HasValue)
                chosenTrainer.TrainingType = trainingType.Value;

            if (age.HasValue)
                chosenTrainer.Age = age.Value;

            if (workExperience.HasValue)
                chosenTrainer.WorkExperience =
                    workExperience.Value;

            /*
             * Сначала сохраняем обычные поля тренера.
             */
            trainerRepository.Update(chosenTrainer);

            /*
             * Открепление атлетов.
             */
            if (deleteathlete != null)
            {
                foreach (Athlete athlete in deleteathlete)
                {
                    Athlete? athleteInDb =
                        athleteRepository.ReadById(athlete.Id);

                    if (athleteInDb == null)
                        continue;

                    if (athleteInDb.trainer != null &&
                        athleteInDb.trainer.Id == chosenTrainer.Id)
                    {
                        athleteInDb.trainer = null;
                        athleteRepository.Update(athleteInDb);
                    }
                }
            }

            /*
             * Закрепление атлетов.
             */
            if (addathlete != null)
            {
                foreach (Athlete athlete in addathlete)
                {
                    Athlete? athleteInDb =
                        athleteRepository.ReadById(athlete.Id);

                    if (athleteInDb == null)
                        continue;

                    athleteInDb.trainer = chosenTrainer;

                    athleteRepository.Update(athleteInDb);
                }
            }

            return trainerRepository.ReadById(id);
        }

        public Athlete? UpdateInfoAthlete(
            int id,
            string? fullname,
            Gendre? gendre,
            int? age,
            int? height,
            int? weight,
            TrainingType? trainingType,
            Trainer? trainer)
        {
            Athlete? chosenAthlete =
                athleteRepository.ReadById(id);

            if (chosenAthlete == null)
                return null;

            int finalAge =
                age ?? chosenAthlete.Age;

            int finalHeight =
                height ?? chosenAthlete.Height;

            int finalWeight =
                weight ?? chosenAthlete.Weight;

            if (finalAge < 14 || finalAge > 120)
                throw new ArgumentException(
                    "Некорректный возраст");

            if (finalHeight <= 0 || finalHeight > 300)
                throw new ArgumentException(
                    "Некорректный рост");

            if (finalWeight <= 0 || finalWeight > 1000)
                throw new ArgumentException(
                    "Некорректный вес");

            if (fullname != null)
            {
                if (string.IsNullOrWhiteSpace(fullname) ||
                    fullname.Any(char.IsDigit))
                {
                    throw new ArgumentException(
                        "Некорректное ФИО");
                }

                chosenAthlete.FullName = fullname;
            }

            if (gendre.HasValue)
                chosenAthlete.Gendre = gendre.Value;

            if (trainingType.HasValue)
                chosenAthlete.TrainingType =
                    trainingType.Value;

            if (age.HasValue)
                chosenAthlete.Age = age.Value;

            if (height.HasValue)
                chosenAthlete.Height = height.Value;

            if (weight.HasValue)
                chosenAthlete.Weight = weight.Value;

            if (trainer != null)
            {
                Trainer? trainerInDb =
                    trainerRepository.ReadById(trainer.Id);

                if (trainerInDb != null)
                {
                    chosenAthlete.trainer = trainerInDb;
                }
            }

            athleteRepository.Update(chosenAthlete);

            return chosenAthlete;
        }

        public bool Registration(
            Trainer trainer,
            Athlete athlete)
        {
            if (trainer == null || athlete == null)
                return false;

            Trainer? trainerInDb =
                trainerRepository.ReadById(trainer.Id);

            Athlete? athleteInDb =
                athleteRepository.ReadById(athlete.Id);

            if (trainerInDb == null ||
                athleteInDb == null)
            {
                return false;
            }

            if (athleteInDb.trainer != null &&
                athleteInDb.trainer.Id == trainerInDb.Id)
            {
                return false;
            }

            athleteInDb.trainer = trainerInDb;

            athleteRepository.Update(athleteInDb);

            /*
             * Также меняем переданный объект,
             * чтобы старый UI сразу увидел изменение.
             */
            athlete.trainer = trainerInDb;

            if (!trainer.Athlete.Any(
                a => a.Id == athlete.Id))
            {
                trainer.Athlete.Add(athlete);
            }

            return true;
        }

        public string PersonalTraining(Athlete athlete)
        {
            if (athlete == null)
            {
                return "Атлет не найден!";
            }

            double heightInM =
                athlete.Height / 100.0;

            double bmi =
                athlete.Weight /
                (heightInM * heightInM);

            double genderDelta =
                athlete.Gendre == Gendre.М
                    ? -1.0
                    : 1.5;

            double prefDelta = 0.0;

            if (athlete.TrainingType ==
                    TrainingType.М_Силовая ||
                athlete.TrainingType ==
                    TrainingType.Ж_Силовая)
            {
                prefDelta = -3.0;
            }
            else if (
                athlete.TrainingType ==
                    TrainingType.М_Гибкость ||
                athlete.TrainingType ==
                    TrainingType.Ж_Гибкость)
            {
                prefDelta = 4.0;
            }

            double ageDelta =
                athlete.Age >= 40
                    ? (athlete.Age - 40) * 0.25
                    : 0.0;

            double fitnessIndex =
                bmi +
                genderDelta +
                prefDelta +
                ageDelta;

            string planTitle;
            string planGoal;
            string planSchedule;
            string planDetails;

            TrainingType recommendedType;

            if (fitnessIndex < 19.0)
            {
                planTitle =
                    "ПЛАН 1: Базовый Массонабор & Сила";

                planGoal =
                    "Набор мышечной массы, рост силовых показателей.";

                planSchedule =
                    "3 раза в неделю (Понедельник / Среда / Пятница)";

                planDetails =
                    "• Тяжелая база: Приседания, Жим лежа, " +
                    "Становая тяга, Подтягивания.\n" +
                    "• Объем: 3-4 подхода по 6-8 повторений.\n" +
                    "• Кардио: Минимальное (5 минут разминки).";

                recommendedType =
                    athlete.Gendre == Gendre.М
                        ? TrainingType.М_Силовая
                        : TrainingType.Ж_Силовая;
            }
            else if (fitnessIndex < 24.0)
            {
                planTitle =
                    "ПЛАН 2: Силовой Рельеф & Гипертрофия";

                planGoal =
                    "Проработка рельефа мышц, гипертрофия, " +
                    "сбалансированное телосложение.";

                planSchedule =
                    "4 раза в неделю (Сплит: Грудь/Трицепс, " +
                    "Спина/Бицепс, Ноги/Плечи)";

                planDetails =
                    "• Сочетание базовых и изолирующих упражнений.\n" +
                    "• Объем: 3-4 подхода по 8-12 повторений.\n" +
                    "• Кардио: 15 минут заминки в конце тренировки.";

                recommendedType =
                    athlete.Gendre == Gendre.М
                        ? TrainingType.М_Силовая
                        : TrainingType.Ж_Силовая;
            }
            else if (fitnessIndex < 28.0)
            {
                planTitle =
                    "ПЛАН 3: Атлетический Баланс & Кроссфит";

                planGoal =
                    "Развитие выносливости, плотности мышц " +
                    "и функциональной силы.";

                planSchedule =
                    "3-4 раза в неделю";

                planDetails =
                    "• Круговые тренировки " +
                    "(работа с гирями, гантелями, брусьями).\n" +
                    "• Объем: 3-4 круга по 10-15 повторений.\n" +
                    "• Кардио: Гребной тренажер / бег 15 минут.";

                recommendedType =
                    athlete.Gendre == Gendre.М
                        ? TrainingType.М_Выносливость
                        : TrainingType.Ж_Выносливость;
            }
            else if (fitnessIndex < 33.0)
            {
                planTitle =
                    "ПЛАН 4: Жиросжигающий Интенсив (HIIT & Сушка)";

                planGoal =
                    "Активное жиросжигание, сушка, " +
                    "ускорение метаболизма.";

                planSchedule =
                    "4 раза в неделю";

                planDetails =
                    "• Высокоинтенсивный интервальный тренинг " +
                    "(HIIT) и суперсеты.\n" +
                    "• Объем: 4 подхода по 15-20 повторений " +
                    "с коротким отдыхом.\n" +
                    "• Кардио: 25 минут эллипса или беговой " +
                    "дорожки в целевой зоне пульса.";

                recommendedType =
                    athlete.Gendre == Gendre.М
                        ? TrainingType.М_Выносливость
                        : TrainingType.Ж_Выносливость;
            }
            else
            {
                planTitle =
                    "ПЛАН 5: Оздоровительный Фитнес, Осанка & Гибкость";

                planGoal =
                    "Укрепление суставов и связок, улучшение " +
                    "гибкости, снятие спазмов.";

                planSchedule =
                    "3 раза в неделю";

                planDetails =
                    "• Пилатес, упражнения с фитболом и " +
                    "фитнес-резинками, суставная гимнастика.\n" +
                    "• Объем: Мягкая нагрузка, " +
                    "12-15 плавных повторений.\n" +
                    "• Растяжка: 20 минут глубокого " +
                    "стретчинга и МФР-ролл.";

                recommendedType =
                    athlete.Gendre == Gendre.М
                        ? TrainingType.М_Гибкость
                        : TrainingType.Ж_Гибкость;
            }

            athlete.TypePersonalTraining =
                recommendedType;

            Athlete? athleteInDb =
                athleteRepository.ReadById(athlete.Id);

            if (athleteInDb != null)
            {
                athleteInDb.TypePersonalTraining =
                    recommendedType;

                athleteRepository.Update(athleteInDb);
            }

            return
                "=== ИНДИВИДУАЛЬНЫЙ РАСЧЕТ ПРОГРАММЫ ===" +
                Environment.NewLine +
                $"Атлет: {athlete.FullName} ({athlete.Gendre})" +
                Environment.NewLine +
                $"Параметры: Возраст — {athlete.Age} лет | " +
                $"Рост — {athlete.Height} см | " +
                $"Вес — {athlete.Weight} кг" +
                Environment.NewLine +
                $"ИМТ: {bmi:F1} | " +
                $"Индекс нагрузки (по формуле): {fitnessIndex:F1}" +
                Environment.NewLine +
                $"Предпочтение атлета: {athlete.TrainingType}" +
                Environment.NewLine +
                Environment.NewLine +
                "--------------------------------------------------" +
                Environment.NewLine +
                $"НАЗНАЧЕННАЯ ПРОГРАММА: {planTitle}" +
                Environment.NewLine +
                "--------------------------------------------------" +
                Environment.NewLine +
                $"🎯 Цель: {planGoal}" +
                Environment.NewLine +
                $"📅 График: {planSchedule}" +
                Environment.NewLine +
                "📋 Содержание тренировок:" +
                Environment.NewLine +
                planDetails +
                Environment.NewLine +
                Environment.NewLine +
                $"Рекомендованный тип специализации тренера: " +
                $"{recommendedType}";
        }

        public List<Trainer> PersonalFilterTrainers(
            Athlete athlete)
        {
            if (athlete == null)
                return new List<Trainer>();

            if (athlete.TypePersonalTraining == null)
            {
                PersonalTraining(athlete);
            }

            return trainerRepository
                .List()
                .OrderByDescending(
                    t => CalculateMatchPercentage(t, athlete))
                .ToList();
        }

        public List<Trainer> RateTrainers()
        {
            List<Trainer> trainers =
                trainerRepository.List().ToList();

            if (trainers.Count == 0)
            {
                return new List<Trainer>();
            }

            foreach (Trainer trainer in trainers)
            {
                trainer.Rating =
                    CalculateRating(trainer);

                trainerRepository.Update(trainer);
            }

            return trainers
                .OrderByDescending(t => t.Rating)
                .ToList();
        }

        public double CalculateRating(Trainer trainer)
        {
            if (trainer == null)
                return 0;

            int athleteCount =
                trainer.Athlete?.Count ?? 0;

            return
                (trainer.WorkExperience * 3.0) +
                (athleteCount * 10.0) -
                (trainer.Age * 0.5);
        }

        public int CalculateMatchPercentage(
            Trainer trainer,
            Athlete athlete)
        {
            if (trainer == null ||
                athlete == null)
            {
                return 0;
            }

            int score = 0;

            if (athlete.TypePersonalTraining == null)
            {
                PersonalTraining(athlete);
            }

            if (trainer.TrainingType ==
                athlete.TypePersonalTraining)
            {
                score += 45;
            }
            else if (
                trainer.TrainingType
                    .ToString()
                    .Contains(
                        athlete.Gendre.ToString()))
            {
                score += 20;
            }

            if (trainer.Gendre == athlete.Gendre)
            {
                score += 15;
            }

            score += Math.Min(
                trainer.WorkExperience * 2,
                20);

            int count =
                trainer.Athlete?.Count ?? 0;

            score += Math.Max(
                0,
                20 - count * 4);

            return Math.Min(score, 100);
        }
    }
}