using System;
using System.Collections.Generic;
using System.Linq;
using Contracts;
using DataAccessLayer;

namespace App_Model
{
    /// <summary>
    /// Реализует бизнес-логику приложения спортивного зала.
    /// Для доступа к данным использует IRepository,
    /// поэтому не зависит от EF или Dapper.
    /// </summary>
    public class Logics : ILogics
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Инициализирует новый экземпляр Logics с указанной единицей работы.
        /// </summary>
        /// <param name="unitOfWork">Единица работы для доступа к репозиториям и управлению транзакциями.</param>
        /// <exception cref="ArgumentNullException">Если unitOfWork равен null.</exception>
        public Logics(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

            if (!_unitOfWork.Trainers.List().Any() && !_unitOfWork.Athletes.List().Any())
            {
                SeedInitialData();
            }
        }

        /// <summary>
        /// Список тренеров, загруженный из репозитория через _unitOfWork.
        /// </summary>
        /// <remarks>Список материализуется вызовом ToList(), поэтому последующие изменения репозитория не
        /// повлияют на возвращённый список.</remarks>
        public List<Trainer> BD_Trainer => _unitOfWork.Trainers.List().ToList();
        public List<Athlete> BD_Athlete => _unitOfWork.Athletes.List().ToList();


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
        /// <summary>
        /// Создаёт и сохраняет тренера с указанными данными.
        /// </summary>
        /// <remarks>Тренер добавляется в репозиторий через _unitOfWork и изменения сохраняются.</remarks>
        /// <param name="fullname">Полное имя тренера; не может быть пустым или содержать цифры.</param>
        /// <param name="gendre">Пол тренера; значение перечисления Gendre.</param>
        /// <param name="trainingType">Тип проводимых тренировок; значение перечисления TrainingType.</param>
        /// <param name="age">Возраст в годах; допустимый диапазон от 18 до 120.</param>
        /// <param name="workExperience">Стаж работы в годах; неотрицательный и не превышает возраст минус 18.</param>
        /// <returns>Добавленный и сохранённый объект Trainer.</returns>
        /// <exception cref="ArgumentException">Если fullname пустой или содержит цифры; если age вне диапазона 18–120; если workExperience отрицателен или
        /// превышает age − 18.</exception>
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

            _unitOfWork.Trainers.Add(trainer);
            _unitOfWork.Save();

            return trainer;
        }
        /// <summary>
        /// Создаёт и сохраняет нового спортсмена с указанными данными.
        /// </summary>
        /// <remarks>Добавляет спортсмена в репозиторий и сохраняет изменения через _unitOfWork.</remarks>
        /// <param name="fullname">Полное имя спортсмена; не должно быть пустым и не может содержать цифры.</param>
        /// <param name="gendre">Пол спортсмена.</param>
        /// <param name="trainingType">Тип тренировки спортсмена.</param>
        /// <param name="age">Возраст в годах; допустимый диапазон: 14–120.</param>
        /// <param name="height">Рост в сантиметрах; должен быть больше 0 и не превышать 300.</param>
        /// <param name="weight">Вес в килограммах; должен быть больше 0 и не превышать 1000.</param>
        /// <returns>Созданный и сохранённый объект Athlete.</returns>
        /// <exception cref="ArgumentException">Если fullname пустой или содержит цифры; либо age вне диапазона 14–120; либо height ≤ 0 или > 300; либо
        /// weight ≤ 0 или > 1000.</exception>
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

            _unitOfWork.Athletes.Add(athlete);
            _unitOfWork.Save();

            return athlete;
        }
        /// <summary>
        /// Удаляет тренера по идентификатору и очищает ссылки на него у связанных спортсменов.
        /// </summary>
        /// <remarks>У связанных сущностей Athlete поле trainer устанавливается в null, изменения
        /// обновляются и сохраняются через слой UnitOfWork.</remarks>
        /// <param name="id">Идентификатор удаляемого тренера.</param>
        /// <returns>true, если тренер найден и удалён; false, если тренер не найден.</returns>
        public bool? RemoveTrainer(int id)
        {
            Trainer? trainer = _unitOfWork.Trainers.ReadById(id);

            if (trainer == null)
                return false;

            List<Athlete> athletes = _unitOfWork.Athletes
                .List()
                .Where(a =>
                    a.trainer != null &&
                    a.trainer.Id == id)
                .ToList();

            foreach (Athlete athlete in athletes)
            {
                athlete.trainer = null;
                _unitOfWork.Athletes.Update(athlete);
            }

            _unitOfWork.Trainers.Delete(id);
            _unitOfWork.Save();

            return true;
        }
        /// <summary>
        /// Удаляет спортсмена с указанным идентификатором из хранилища.
        /// </summary>
        /// <param name="id">Идентификатор спортсмена.</param>
        /// <returns>true при успешном удалении; false, если спортсмен с указанным идентификатором не найден.</returns>
        public bool? RemoveAthlete(int id)
        {
            Athlete? athlete = _unitOfWork.Athletes.ReadById(id);

            if (athlete == null)
                return false;

            _unitOfWork.Athletes.Delete(id);
            _unitOfWork.Save();

            return true;
        }
        /// <summary>
        /// Возвращает тренера с указанным идентификатором.
        /// </summary>
        /// <param name="id">Идентификатор тренера для поиска.</param>
        /// <returns>Тренер с указанным идентификатором или null, если тренер не найден.</returns>
        public Trainer? CheckTrainer(int id)
        {
            return _unitOfWork.Trainers.ReadById(id);
        }
        /// <summary>
        /// Возвращает спортсмена с указанным идентификатором или null, если не найден.
        /// </summary>
        /// <param name="id">Идентификатор спортсмена.</param>
        /// <returns>Объект Athlete с указанным идентификатором или null, если запись не найдена.</returns>
        public Athlete? CheckAthlete(int id)
        {
            return _unitOfWork.Athletes.ReadById(id);
        }
        /// <summary>
        /// Обновляет свойства тренера и управляет привязкой атлетов.
        /// </summary>
        /// <remarks>Вносит изменения в репозиторий через _unitOfWork: обновляет поля тренера, открепляет
        /// и закрепляет атлетов, затем сохраняет изменения.</remarks>
        /// <param name="id">Идентификатор тренера.</param>
        /// <param name="fullname">Новое полное имя тренера; null — не изменять. Не должно быть пустым или содержать цифры.</param>
        /// <param name="gendre">Пол тренера; null — не изменять.</param>
        /// <param name="trainingType">Тип тренировки тренера; null — не изменять.</param>
        /// <param name="age">Возраст тренера в годах; null — не изменять. Допустимый диапазон 18–120.</param>
        /// <param name="workExperience">Стаж работы в годах; null — не изменять. Должен быть неотрицательным и не превышать возраст минус 18.</param>
        /// <param name="deleteathlete">Список атлетов для открепления от тренера; null — пропустить открепление.</param>
        /// <param name="addathlete">Список атлетов для закрепления за тренером; null — пропустить добавление.</param>
        /// <returns>Обновлённый объект Trainer либо null, если тренер с указанным id не найден.</returns>
        /// <exception cref="ArgumentException">Возникает при некорректном возрасте, некорректном стаже работы или некорректном формате ФИО.</exception>
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
                _unitOfWork.Trainers.ReadById(id);

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
            _unitOfWork.Trainers.Update(chosenTrainer);

            /*
             * Открепление атлетов.
             */
            if (deleteathlete != null)
            {
                foreach (Athlete athlete in deleteathlete)
                {
                    Athlete? athleteInDb =
                        _unitOfWork.Athletes.ReadById(athlete.Id);

                    if (athleteInDb == null)
                        continue;

                    if (athleteInDb.trainer != null &&
                        athleteInDb.trainer.Id == chosenTrainer.Id)
                    {
                        athleteInDb.trainer = null;
                        _unitOfWork.Athletes.Update(athleteInDb);
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
                        _unitOfWork.Athletes.ReadById(athlete.Id);

                    if (athleteInDb == null)
                        continue;

                    athleteInDb.trainer = chosenTrainer;

                    _unitOfWork.Athletes.Update(athleteInDb);
                }
            }

            _unitOfWork.Save();
            return _unitOfWork.Trainers.ReadById(id);
        }
        /// <summary>
        /// Обновляет информацию атлета с указанным идентификатором и сохраняет изменения в хранилище.
        /// </summary>
        /// <remarks>Изменения применяются к найденному объекту и сохраняются вызовом Save() у unit of
        /// work; если тренер передан, привязка произойдёт только при наличии тренера в базе.</remarks>
        /// <param name="id">Идентификатор атлета для поиска и обновления.</param>
        /// <param name="fullname">Новое ФИО; если null — поле не изменяется. Не должно быть пустым или содержать цифры.</param>
        /// <param name="gendre">Пол атлета; если null — поле не изменяется.</param>
        /// <param name="age">Возраст в годах; если null — поле не изменяется. Допустимый диапазон 14–120.</param>
        /// <param name="height">Рост в сантиметрах; если null — поле не изменяется. Допустимый диапазон 1–300.</param>
        /// <param name="weight">Вес в килограммах; если null — поле не изменяется. Допустимый диапазон 1–1000.</param>
        /// <param name="trainingType">Тип тренировки; если null — поле не изменяется.</param>
        /// <param name="trainer">Объект тренера для привязки; если null — привязка не выполняется. Привязка выполняется только если тренер
        /// существует в хранилище.</param>
        /// <returns>Обновлённый объект Athlete либо null, если атлет с указанным идентификатором не найден.</returns>
        /// <exception cref="ArgumentException">Выбрасывается при некорректных значениях: возраст вне диапазона 14–120, рост ≤0 или >300, вес ≤0 или >1000,
        /// либо при некорректном ФИО (пустое/пробельное или содержит цифры).</exception>
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
                _unitOfWork.Athletes.ReadById(id);

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
                    _unitOfWork.Trainers.ReadById(trainer.Id);

                if (trainerInDb != null)
                {
                    chosenAthlete.trainer = trainerInDb;
                }
            }

            _unitOfWork.Athletes.Update(chosenAthlete);
            _unitOfWork.Save();
            return chosenAthlete;
        }
        /// <summary>
        /// Привязывает спортсмена к тренеру: проверяет ненулевые аргументы, наличие сущностей в хранилище,
        /// предотвращает повторную привязку, обновляет записи и сохраняет изменения.
        /// </summary>
        /// <remarks>Изменяет переданный объект athlete и коллекцию тренера, затем сохраняет изменения
        /// через UnitOfWork.</remarks>
        /// <param name="trainer">Тренер для привязки; используется для поиска соответствующей записи в хранилище и обновления его коллекции
        /// спортсменов.</param>
        /// <param name="athlete">Спортсмен, которого необходимо привязать к тренеру; обновляется запись спортсмена и переданный объект для
        /// отражения изменения в UI.</param>
        /// <returns>true при успешной привязке и сохранении изменений; false при некорректных аргументах, отсутствии сущностей в
        /// базе или если спортсмен уже привязан к тому же тренеру.</returns>
        public bool Registration(
            Trainer trainer,
            Athlete athlete)
        {
            if (trainer == null || athlete == null)
                return false;

            Trainer? trainerInDb =
                _unitOfWork.Trainers.ReadById(trainer.Id);

            Athlete? athleteInDb =
                _unitOfWork.Athletes.ReadById(athlete.Id);

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

            _unitOfWork.Athletes.Update(athleteInDb);

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
            _unitOfWork.Save();
            return true;
        }
        /// <summary>
        /// Рассчитывает индивидуальную программу тренировок на основе антропометрии, возраста, пола и предпочтений;
        /// назначает рекомендованный тип персонального тренера и возвращает подробный текстовый отчёт о плане.
        /// </summary>
        /// <remarks>Присваивает athlete.TypePersonalTraining и при наличии записи в базе обновляет её
        /// через _unitOfWork (Update + Save). Индекс нагрузки вычисляется как сумма ИМТ и поправок по полу,
        /// предпочтению тренировки и возрасту (начиная с 40 лет).</remarks>
        /// <param name="athlete">Атлет с заполненными свойствами Id, FullName, Age, Height, Weight, Gendre и TrainingType; при null
        /// возвращается сообщение "Атлет не найден!".</param>
        /// <returns>Текстовый отчёт с ИМТ, вычисленным индексом нагрузки, назначенной программой (заголовок, цель, расписание,
        /// содержание) и рекомендованным типом специализации тренера; при null — сообщение "Атлет не найден!".</returns>
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
                _unitOfWork.Athletes.ReadById(athlete.Id);

            if (athleteInDb != null)
            {
                athleteInDb.TypePersonalTraining =
                    recommendedType;

                _unitOfWork.Athletes.Update(athleteInDb);
                _unitOfWork.Save();
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
        /// <summary>
        /// Возвращает список тренеров, отсортированный по убыванию процента соответствия заданному атлету.
        /// </summary>
        /// <remarks>Если у атлета не задан TypePersonalTraining, вызывается PersonalTraining(athlete) для
        /// его инициализации. Список тренеров получается из _unitOfWork.Trainers.List().</remarks>
        /// <param name="athlete">Атлет, для которого подбираются и ранжируются тренеры.</param>
        /// <returns>Список тренеров, упорядоченный по убыванию CalculateMatchPercentage для указанного атлета; при null
        /// возвращается пустой список.</returns>
        public List<Trainer> PersonalFilterTrainers(
            Athlete athlete)
        {
            if (athlete == null)
                return new List<Trainer>();

            if (athlete.TypePersonalTraining == null)
            {
                PersonalTraining(athlete);
            }

            return _unitOfWork.Trainers
                .List()
                .OrderByDescending(
                    t => CalculateMatchPercentage(t, athlete))
                .ToList();
        }
        /// <summary>
        /// Вычисляет и присваивает рейтинг каждому тренеру, обновляет записи в репозитории и сохраняет изменения, затем
        /// возвращает список тренеров, отсортированный по убыванию рейтинга.
        /// </summary>
        /// <remarks>Для расчёта рейтинга используется CalculateRating. Изменения свойства Rating
        /// применяются через механизм UnitOfWork и сохраняются вызовом Save().</remarks>
        /// <returns>Список тренеров, отсортированный по убыванию значения Rating; при отсутствии тренеров возвращается пустой
        /// список.</returns>
        public List<Trainer> RateTrainers()
        {
            List<Trainer> trainers =
                _unitOfWork.Trainers.List().ToList();

            if (trainers.Count == 0)
            {
                return new List<Trainer>();
            }

            foreach (Trainer trainer in trainers)
            {
                trainer.Rating =
                    CalculateRating(trainer);

                _unitOfWork.Trainers.Update(trainer);
            }
            _unitOfWork.Save();

            return trainers
                .OrderByDescending(t => t.Rating)
                .ToList();
        }
        /// <summary>
        /// Вычисляет числовой рейтинг тренера на основе стажа, числа спортсменов и возраста.
        /// </summary>
        /// <remarks>Формула: (WorkExperience * 3.0) + (количество спортсменов * 10.0) - (Age * 0.5).
        /// Количество спортсменов берётся из Trainer.Athlete?.Count с обработкой null.</remarks>
        /// <param name="trainer">Тренер для расчёта рейтинга; при null возвращается 0.</param>
        /// <returns>Дробный рейтинг (double); 0 при null.</returns>
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
        /// <summary>
        /// Вычисляет процент соответствия между тренером и спортсменом на основе типа тренировки, пола, стажа и
        /// загрузки тренера.
        /// </summary>
        /// <remarks>Подсчёт: +45 за точное совпадение типа тренировки; +20 за частичное совпадение типа;
        /// +15 за совпадение пола; до +20 за стаж (2 очка за год, максимум 20); до +20 за доступность (уменьшается на 4
        /// за каждого текущего спортсмена). При необходимости вызывается PersonalTraining(athlete) для установки типа
        /// тренировки. Итог ограничивается 100.</remarks>
        /// <param name="trainer">Тренер для оценки соответствия.</param>
        /// <param name="athlete">Спортсмен для оценки соответствия.</param>
        /// <returns>Процент соответствия в диапазоне 0–100; возвращает 0 при null-аргументах.</returns>
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