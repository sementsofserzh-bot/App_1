using System;
using System.Collections.Generic;
using System.Linq;
using App_Model_Essence;

namespace App_Model_TestLogics
{
    /// <summary>
    /// Интерфейс бизнес-логики трен-го зала.
    /// Определяет работу с тренерами и атлетами:
    /// добавление, удаление, просмотр и редактирование данных,
    /// бизнес-функции — подбор персональной программы тренировок,
    /// подбор подходящих тренеров и формирование рейтинга тренеров.
    /// Реализуется классом App_Model_Logics.Logics.
    /// </summary>
    public interface ILogics
    {
        List<Trainer> BD_Trainer { get; set; }
        List<Athlete> BD_Athlete { get; set; }

        /// <summary>
        /// Добавляет нового тренера в базу данных.
        /// </summary>
        /// <param name="fullname">ФИО тренера (не пустое, без цифр).</param>
        /// <param name="gendre">Пол тренера.</param>
        /// <param name="trainingType">Специализация тренера.</param>
        /// <param name="age">Возраст в годах (от 18 до 120).</param>
        /// <param name="workExperience">Стаж работы в годах (не больше (age - 18)).</param>
        /// <returns>Добавленный объект Trainer с присвоенным Id.</returns>
        /// <exception cref="ArgumentException">
        /// Бросается при некорректном ФИО, возрасте или стаже.
        /// </exception>
        
        Trainer AddTrainer(string fullname, Gendre gendre, TrainingType trainingType, int age, int workExperience);
        /// <summary>
        /// Добавляет нового атлета в базу данных.
        /// </summary>
        /// <param name="fullname">ФИО атлета (не пустое, без цифр).</param>
        /// <param name="gendre">Пол атлета.</param>
        /// <param name="trainingType">Предпочтительный тип тренировки.</param>
        /// <param name="age">Возраст в годах (от 14 до 120).</param>
        /// <param name="height">Рост в сантиметрах (от 1 до 300).</param>
        /// <param name="weight">Вес в килограммах (от 1 до 1000).</param>
        /// <returns>Добавленный объект Athlete с присвоенным Id.</returns>
        /// <exception cref="ArgumentException">
        /// Бросается при некорректном ФИО, возрасте, росте или весе.
        /// </exception>
        Athlete AddAthlete(string fullname, Gendre gendre, TrainingType trainingType, int age, int height, int weight);

        /// <summary>
        /// Удаляет тренера из базы данных по Id.
        /// Открепляет всех его атлетов, если такие были у него.
        /// </summary>
        /// <param name="id">Id тренера.</param>
        /// <returns>true — если удалён, false — если не найден.</returns>
        bool? RemoveTrainer(int id);

        /// <summary>
        /// Удаляет атлета из базы данных по Id.
        /// Открепляет его от тренера, если он был закреплён.
        /// </summary>
        /// <param name="id">Id атлета.</param>
        /// <returns>true — если удалён, false — если не найден.</returns>
        bool? RemoveAthlete(int id);

        /// <summary>
        /// Возвращает тренера по Id или null, если не найден.
        /// </summary>
        /// <param name="id">Id тренера.</param>
        /// <returns>Объект Trainer или null.</returns>
        Trainer? CheckTrainer(int id);

        /// <summary>
        /// Возвращает атлета по Id или null, если не найден.
        /// </summary>
        /// <param name="id">Id атлета.</param>
        /// <returns>Объект Athlete или null.</returns>

        Athlete? CheckAthlete(int id);

        /// <summary>
        /// Обновляет данные тренера.
        /// Открепить/закрепить атлетов через списки.
        /// </summary>
        /// <param name="id">Id тренера.</param>
        /// <param name="fullname">Новое ФИО или null.</param>
        /// <param name="gendre">Новый пол или null.</param>
        /// <param name="trainingType">Новая специализация или null.</param>
        /// <param name="age">Новый возраст или null.</param>
        /// <param name="workExperience">Новый стаж или null.</param>
        /// <param name="deleteathlete">Список атлетов для открепления или null.</param>
        /// <param name="addathlete">Список атлетов для закрепления или null.</param>
        /// <returns>Обновлённый Trainer или null, если не найден.</returns>
        /// <exception cref="ArgumentException">
        /// Бросается при некорректном ФИО, возрасте или стаже.
        /// </exception>
        Trainer? UpdateInfoTrainer(int id, string? fullname, Gendre? gendre, TrainingType? trainingType, int? age, int? workExperience, List<Athlete>? deleteathlete, List<Athlete>? addathlete);

        /// <summary>
        /// Обновляет данные атлета.
        /// Закрепление за тренером.
        /// </summary>
        /// <param name="id">Id атлета.</param>
        /// <param name="fullname">Новое ФИО или null.</param>
        /// <param name="gendre">Новый пол или null.</param>
        /// <param name="age">Новый возраст или null.</param>
        /// <param name="height">Новый рост или null.</param>
        /// <param name="weight">Новый вес или null.</param>
        /// <param name="trainingType">Новый тип тренировки или null.</param>
        /// <param name="trainer">Новый тренер или null.</param>
        /// <returns>Обновлённый Athlete или null, если не найден.</returns>
        /// <exception cref="ArgumentException">
        /// Бросается при некорректном ФИО, возрасте, росте или весе.
        /// </exception>
        Athlete? UpdateInfoAthlete(int id, string? fullname, Gendre? gendre, int? age, int? height, int? weight, TrainingType? trainingType, Trainer? trainer);


        /// <summary>
        /// Закрепляет атлета за тренером.
        /// Если атлет уже закреплён за другим — открепляет от него и закрепляет за выбранным.
        /// </summary>
        /// <param name="trainer">Тренер.</param>
        /// <param name="athlete">Атлет.</param>
        /// <returns>
        /// true — если закреплён; false — если уже закреплён или null.
        /// </returns>
        bool Registration(Trainer trainer, Athlete athlete);

        /// <summary>
        /// Бизнес-функция: подбирает персональную программу тренировок
        /// на основе возраста, роста, веса, пола и предпочтений атлета.
        /// Результат сохраняется в свойстве атлета TypePersonalTraining.
        /// </summary>
        /// <param name="athlete">Атлет, для которого подбирается программа.</param>
        /// <returns>Текстовое описание программы тренировок.</returns>
        string PersonalTraining(Athlete athlete);

        /// <summary>
        /// Бизнес-функция: подбирает тренеров под параметры атлета
        /// и сортирует их по проценту совпадения.
        /// </summary>
        /// <param name="athlete">Атлет, для которого подбираются тренеры.</param>
        /// <returns>Список тренеров, отсортированный по убыванию совпадения.</returns>
        List<Trainer> PersonalFilterTrainers(Athlete athlete);

        /// <summary>
        /// Бизнес-функция: формирует рейтинг тренеров
        /// на основе опыта работы, количества атлетов и возраста.
        /// </summary>
        /// <returns>Список тренеров, отсортированный по убыванию рейтинга.</returns>
        List<Trainer> RateTrainers();

        /// <summary>
        /// Вычисляет процент совпадения тренера с параметрами атлета (0–100).
        /// Учитывает специализацию, пол, опыт и загруженность.
        /// </summary>
        /// <param name="trainer">Тренер.</param>
        /// <param name="athlete">Атлет.</param>
        /// <returns>Целое число от 0 до 100.</returns>
        int CalculateMatchPercentage(Trainer trainer, Athlete athlete);
    }
}