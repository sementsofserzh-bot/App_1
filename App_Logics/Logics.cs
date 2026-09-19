using System;
using System.Collections.Generic;
using System.Linq;
using App_Model_Essence;
using App_Model_TestLogics;
namespace App_Model_Logics
{

    public class Logics : ILogics
    {
        public List<Trainer> BD_Trainer { get; set; } = new();
        public List<Athlete> BD_Athlete { get; set; } = new();

        public Logics()
        {
            SeedInitialData();
        }

        private void SeedInitialData()
        {
            var t1 = AddTrainer("Соколов Виктор Игоревич", Gendre.М, TrainingType.М_Силовая, 38, 12);
            var t2 = AddTrainer("Морозова Анна Сергеевна", Gendre.Ж, TrainingType.Ж_Выносливость, 29, 6);
            var t3 = AddTrainer("Кузнецов Дмитрий Анатольевич", Gendre.М, TrainingType.М_Гибкость, 45, 18);
            var t4 = AddTrainer("Трифонова Алена Александровна", Gendre.Ж, TrainingType.Ж_Гибкость, 40, 18);
            var t5 = AddTrainer("Семенцов Сергей Витальевич", Gendre.М, TrainingType.Ж_Гибкость, 50, 20);


            var a1 = AddAthlete("Волков Артём Денисович", Gendre.М, TrainingType.М_Силовая, 16, 185, 63);
            var a2 = AddAthlete("Зайцева Алина Максимовна", Gendre.Ж, TrainingType.Ж_Силовая, 20, 165, 55);
            var a3 = AddAthlete("Смирнов Михаил Александрович", Gendre.М, TrainingType.М_Гибкость, 32, 178, 95);
            var a4 = AddAthlete("Павлова Екатерина Дмитриевна", Gendre.Ж, TrainingType.Ж_Выносливость, 24, 170, 58);
            var a5 = AddAthlete("Федоров Егор Романович", Gendre.М, TrainingType.М_Выносливость, 15, 172, 60);
            var a6 = AddAthlete("Романова Мария Владимировна", Gendre.Ж, TrainingType.Ж_Гибкость, 42, 162, 68);
            var a7 = AddAthlete("Попов Никита Васильевич", Gendre.М, TrainingType.М_Гибкость, 55, 180, 85);
            var a8 = AddAthlete("Козлова София Евгеньевна", Gendre.Ж, TrainingType.Ж_Выносливость, 22, 168, 54);

            Registration(t1, a1);
            Registration(t1, a2);

            Registration(t2, a4);
            Registration(t2, a6);
            Registration(t2, a8);

            Registration(t3, a3);
            Registration(t3, a7);
            Registration(t4, a5);
            

        }
        int nextTrainer_ID = 0;
        int nextAthlete_ID = 0;
        //Добавление
        public Trainer AddTrainer(string fullname, Gendre gendre, TrainingType trainingType, int age, int workExperience)
        {
            if (string.IsNullOrWhiteSpace(fullname))
                throw new ArgumentException("Имя не может быть пустым");
            if (age < 18 || age > 120)
                throw new ArgumentException("Некорректный возраст"); //ДЛЯ СЕРЕГИ: ВО ВЬЮХЕ ВОЗРАСТ И ОПЫТ ПРОСИ УКАЗЫВАТЬ В ГОДАХ
            if (workExperience < 0 || workExperience > age - 18)
                throw new ArgumentException("Некорректный опыт работы");
            BD_Trainer.Add(new Trainer { Id = nextTrainer_ID++, FullName = fullname, Gendre = gendre, TrainingType = trainingType, Age = age, WorkExperience = workExperience });
            return BD_Trainer[BD_Trainer.Count - 1];
        }
        public Athlete AddAthlete(string fullname, Gendre gendre, TrainingType trainingType, int age, int height, int weight)
        {
            if (string.IsNullOrWhiteSpace(fullname))
                throw new ArgumentException("Имя не может быть пустым");
            if (age < 14 || age > 120)
                throw new ArgumentException("Некорректный возраст");
            if (height <= 0 || height > 300) //ДЛЯ СЕРЕГИ: ВО ВЬЮХЕ РОСТ ПРОСИ УКАЗЫВАТЬ В СМ
                throw new ArgumentException("Некорректный рост");
            if (weight <= 0 || weight > 1000) //ДЛЯ СЕРЕГИ:ВО ВЬЮХЕ ВЕС ПРОСИ УКАЗЫВАТЬ В КГ
                throw new ArgumentException("Некорректный вес");
            BD_Athlete.Add(new Athlete { Id = nextAthlete_ID++, FullName = fullname, Gendre = gendre, TrainingType = trainingType, Age = age, Height = height, Weight = weight });
            return BD_Athlete[BD_Athlete.Count - 1];
        }
        //удаление
        public bool? RemoveTrainer(int id)
        {
            var trainer = BD_Trainer.FirstOrDefault(t => t.Id == id);
            if (trainer == null) { return false; }

            if (trainer.Athlete != null)
            {
                foreach (var athlete in trainer.Athlete.ToList())
                {
                    athlete.trainer = null;
                }
                trainer.Athlete.Clear();
            }

            foreach (var athlete in BD_Athlete)
            {
                if (athlete.trainer != null && athlete.trainer.Id == id)
                {
                    athlete.trainer = null;
                }
            }

            BD_Trainer.Remove(trainer);
            return true;
        }

        public bool? RemoveAthlete(int id)
        {
            var athlete = BD_Athlete.FirstOrDefault(a => a.Id == id);
            if (athlete == null) { return false; }

            if (athlete.trainer != null)
            {
                var trainer = BD_Trainer.FirstOrDefault(t => t.Id == athlete.trainer.Id);
                if (trainer != null && trainer.Athlete != null)
                {
                    trainer.Athlete.RemoveAll(a => a.Id == id);
                }
                athlete.trainer = null;
            }

            BD_Athlete.Remove(athlete);
            return true;
        }
        //Чтение
        public Trainer? CheckTrainer(int id)
        {
            return BD_Trainer.FirstOrDefault(t => t.Id == id);
        }
        public Athlete? CheckAthlete(int id)
        {
            return BD_Athlete.FirstOrDefault(a => a.Id == id);
        }
        //обновление
        public Trainer? UpdateInfoTrainer(int id, string? fullname, Gendre? gendre, TrainingType? trainingType, int? age, int? workExperience, List<Athlete>? deleteathlete, List<Athlete>? addathlete)
        {
            var chosen_trainer = BD_Trainer.FirstOrDefault(t => t.Id == id);
            if (chosen_trainer == null) { return null; }

            // Обновляем только то, что пришло (не null)
            if (fullname != null)
                chosen_trainer.FullName = fullname;

            if (gendre.HasValue)
                chosen_trainer.Gendre = gendre.Value;

            if (trainingType.HasValue)
                chosen_trainer.TrainingType = trainingType.Value;

            if (age.HasValue)
                chosen_trainer.Age = age.Value;

            if (workExperience.HasValue)
                chosen_trainer.WorkExperience = workExperience.Value;

            // Удаление атлетов
            if (deleteathlete != null && deleteathlete.Count != 0)
            {
                foreach (var athlete in deleteathlete)
                {
                    var athleteInTrainerList = chosen_trainer.Athlete
                        .FirstOrDefault(a => a.Id == athlete.Id);
                    if (athleteInTrainerList != null)
                        chosen_trainer.Athlete.Remove(athleteInTrainerList);

                    var athleteInDb = BD_Athlete.FirstOrDefault(x => x.Id == athlete.Id);
                    if (athleteInDb != null)
                        athleteInDb.trainer = null;
                }
            }
            //добавление атлетов
            if (addathlete != null && addathlete.Count != 0)
            {
                foreach (var athlete in addathlete)
                {
                    var athleteInDb_2 = BD_Athlete.FirstOrDefault(y => y.Id == athlete.Id);
                    if (athleteInDb_2 == null)
                    {
                        continue;
                    }

                    // Если атлет уже закреплён за другим тренером — открепляем от него
                    if (athleteInDb_2.trainer != null && athleteInDb_2.trainer.Id != chosen_trainer.Id)
                    {
                        var oldTrainer = BD_Trainer.FirstOrDefault(t => t.Id == athleteInDb_2.trainer.Id);
                        if (oldTrainer != null)
                        {
                            var athleteInOldTrainerList = oldTrainer.Athlete.FirstOrDefault(a => a.Id == athleteInDb_2.Id);
                            if (athleteInOldTrainerList != null)
                            {
                                oldTrainer.Athlete.Remove(athleteInOldTrainerList);
                            }
                        }
                    }

                    // Прикрепляем атлета к текущему тренеру
                    athleteInDb_2.trainer = chosen_trainer;

                    // Добавляем в список тренера, если его там ещё нет
                    var athleteInTrainerList_2 = chosen_trainer.Athlete.FirstOrDefault(a => a.Id == athleteInDb_2.Id);
                    if (athleteInTrainerList_2 == null)
                    {
                        chosen_trainer.Athlete.Add(athleteInDb_2);
                    }
                }
            }
            return chosen_trainer;
        }

        public Athlete? UpdateInfoAthlete(int id, string? fullname, Gendre? gendre, int? age, int? height, int? weight, TrainingType? trainingType, Trainer? trainer)
        {
            var chosen_athlete = BD_Athlete.FirstOrDefault(a => a.Id == id);
            if (chosen_athlete == null)
                return null;

            // Обновляем простые поля — только если пришли
            if (fullname != null)
                chosen_athlete.FullName = fullname;

            if (gendre.HasValue)
                chosen_athlete.Gendre = gendre.Value;

            if (trainingType.HasValue)
                chosen_athlete.TrainingType = trainingType.Value;

            if (age.HasValue)
                chosen_athlete.Age = age.Value;

            if (height.HasValue)
                chosen_athlete.Height = height.Value;

            if (weight.HasValue)
                chosen_athlete.Weight = weight.Value;

            // Работа с тренером — только если пришёл
            if (trainer != null)
            {
                // Если у атлета уже был другой тренер — открепляем
                if (chosen_athlete.trainer != null && chosen_athlete.trainer.Id != trainer.Id)
                {
                    var oldTrainer = BD_Trainer.FirstOrDefault(t => t.Id == chosen_athlete.trainer.Id);
                    if (oldTrainer != null)
                        oldTrainer.Athlete.RemoveAll(a => a.Id == chosen_athlete.Id);
                }

                // Закрепляем за новым
                chosen_athlete.trainer = trainer;

                // Добавляем в список нового тренера, если его там нет
                if (!trainer.Athlete.Any(a => a.Id == chosen_athlete.Id))
                    trainer.Athlete.Add(chosen_athlete);
            }

            return chosen_athlete;
        }

        public bool Registration(Trainer trainer, Athlete athlete)
        {
            
            if (athlete == null || trainer == null)
            {
                return false;
            }
            if (athlete.trainer != null && athlete.trainer.Id == trainer.Id)
            {
                return false;
            }
            if (athlete.trainer != null )
            {
                var OldTrainer = BD_Trainer.FirstOrDefault(o => o.Id == athlete.trainer.Id);
                if (OldTrainer != null)
                {
                    OldTrainer.Athlete.RemoveAll(o => o.Id == athlete.Id);
                }

            }
            athlete.trainer = trainer;
            if (!trainer.Athlete.Any(a => a.Id == athlete.Id))
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

            // 1. Расчет ИМТ
            double heightInM = athlete.Height / 100.0;
            double bmi = athlete.Weight / (heightInM * heightInM);

            // 2. Расчет поправок для формулы
            double genderDelta = (athlete.Gendre == Gendre.М) ? -1.0 : 1.5;

            double prefDelta = 0.0;
            if (athlete.TrainingType == TrainingType.М_Силовая || athlete.TrainingType == TrainingType.Ж_Силовая)
                prefDelta = -3.0;
            else if (athlete.TrainingType == TrainingType.М_Гибкость || athlete.TrainingType == TrainingType.Ж_Гибкость)
                prefDelta = 4.0;

            double ageDelta = athlete.Age >= 40 ? (athlete.Age - 40) * 0.25 : 0.0;

            // 3. Вычисление итогового Индекса Нагрузки
            double fitnessIndex = bmi + genderDelta + prefDelta + ageDelta;

            // 4. Подбор одного из 5 планов и назначение типа для фильтрации тренеров
            string planTitle;
            string planGoal;
            string planSchedule;
            string planDetails;
            TrainingType recommendedType;

            if (fitnessIndex < 19.0)
            {
                planTitle = "ПЛАН 1: Базовый Массонабор & Сила";
                planGoal = "Набор мышечной массы, рост силовых показателей.";
                planSchedule = "3 раза в неделю (Понедельник / Среда / Пятница)";
                planDetails = "• Тяжелая база: Приседания, Жим лежа, Становая тяга, Подтягивания.\n• Объем: 3-4 подхода по 6-8 повторений.\n• Кардио: Минимальное (5 минут разминки).";
                recommendedType = athlete.Gendre == Gendre.М ? TrainingType.М_Силовая : TrainingType.Ж_Силовая;
            }
            else if (fitnessIndex >= 19.0 && fitnessIndex < 24.0)
            {
                planTitle = "ПЛАН 2: Силовой Рельеф & Гипертрофия";
                planGoal = "Проработка рельефа мышц, гипертрофия, сбалансированное телосложение.";
                planSchedule = "4 раза в неделю (Сплит: Грудь/Трицепс, Спина/Бицепс, Ноги/Плечи)";
                planDetails = "• Сочетание базовых и изолирующих упражнений.\n• Объем: 3-4 подхода по 8-12 повторений.\n• Кардио: 15 минут заминки в конце тренировки.";
                recommendedType = athlete.Gendre == Gendre.М ? TrainingType.М_Силовая : TrainingType.Ж_Силовая;
            }
            else if (fitnessIndex >= 24.0 && fitnessIndex < 28.0)
            {
                planTitle = "ПЛАН 3: Атлетический Баланс & Кроссфит";
                planGoal = "Развитие выносливости, плотности мышц и функциональной силы.";
                planSchedule = "3-4 раза в неделю";
                planDetails = "• Круговые тренировки (работа с гирями, гантелями, брусьями).\n• Объем: 3-4 круга по 10-15 повторений.\n• Кардио: Гребной тренажер / бег 15 минут.";
                recommendedType = athlete.Gendre == Gendre.М ? TrainingType.М_Выносливость : TrainingType.Ж_Выносливость;
            }
            else if (fitnessIndex >= 28.0 && fitnessIndex < 33.0)
            {
                planTitle = "ПЛАН 4: Жиросжигающий Интенсив (HIIT & Сушка)";
                planGoal = "Активное жиросжигание, сушка, ускорение метаболизма.";
                planSchedule = "4 раза в неделю";
                planDetails = "• Высокоинтенсивный интервальный тренинг (HIIT) и суперсеты.\n• Объем: 4 подхода по 15-20 повторений с коротким отдыхом.\n• Кардио: 25 минут эллипса или беговой дорожки в целевой зоне пульса.";
                recommendedType = athlete.Gendre == Gendre.М ? TrainingType.М_Выносливость : TrainingType.Ж_Выносливость;
            }
            else
            {
                planTitle = "ПЛАН 5: Оздоровительный Фитнес, Осанка & Гибкость";
                planGoal = "Укрепление суставов и связок, улучшение гибкости, снятие спазмов.";
                planSchedule = "3 раза в неделю";
                planDetails = "• Пилатес, упражнения с фитболом и фитнес-резинками, суставная гимнастика.\n• Объем: Мягкая нагрузка, 12-15 плавных повторений.\n• Растяжка: 20 минут глубокого стретчинга и МФР-ролл.";
                recommendedType = athlete.Gendre == Gendre.М ? TrainingType.М_Гибкость : TrainingType.Ж_Гибкость;
            }

            // Сохраняем рекомендованное направление для корректного поиска тренеров в PersonalFilterTrainers
            athlete.TypePersonalTraining = recommendedType;

            // Формируем красивый итоговый отчет для многострочного TextBox
            return $"=== ИНДИВИДУАЛЬНЫЙ РАСЧЕТ ПРОГРАММЫ ===" + Environment.NewLine +
                   $"Атлет: {athlete.FullName} ({athlete.Gendre})" + Environment.NewLine +
                   $"Параметры: Возраст — {athlete.Age} лет | Рост — {athlete.Height} см | Вес — {athlete.Weight} кг" + Environment.NewLine +
                   $"ИМТ: {bmi:F1} | Индекс нагрузки (по формуле): {fitnessIndex:F1}" + Environment.NewLine +
                   $"Предпочтение атлета: {athlete.TrainingType}" + Environment.NewLine +
                   Environment.NewLine +
                   $"--------------------------------------------------" + Environment.NewLine +
                   $"НАЗНАЧЕННАЯ ПРОГРАММА: {planTitle}" + Environment.NewLine +
                   $"--------------------------------------------------" + Environment.NewLine +
                   $"🎯 Цель: {planGoal}" + Environment.NewLine +
                   $"📅 График: {planSchedule}" + Environment.NewLine +
                   $"📋 Содержание тренировок:" + Environment.NewLine +
                   $"{planDetails}" + Environment.NewLine +
                   Environment.NewLine +
                   $"Рекомендованный тип специализации тренера: {recommendedType}";
        }

        public List<Trainer> PersonalFilterTrainers(Athlete athlete)
        {
            if (athlete == null) return new List<Trainer>();

            if (athlete.TypePersonalTraining == null)
            {
                PersonalTraining(athlete);
            }

            // Сортируем всех тренеров от лучшего совпадения к худшему
            return BD_Trainer
                .OrderByDescending(t => CalculateMatchPercentage(t, athlete))
                .ToList();
        }
        public List<Trainer> RateTrainers()
        {
            if (BD_Trainer.Count == 0)
            {
                return new List<Trainer>();
            }
            
            foreach (var t in BD_Trainer)
            {
                t.Rating = CalculateRating(t);
            }
            return BD_Trainer.OrderByDescending(t => t.Rating).ToList();
            
        }
        public double CalculateRating(Trainer trainer)
        {
            if (trainer == null) { return 0; }
            double rating = (trainer.WorkExperience * 3.0) + (trainer.Athlete.Count * 10.0) - (trainer.Age * 0.5);
            return rating;
        }
        public int CalculateMatchPercentage(Trainer trainer, Athlete athlete)
        {
            if (trainer == null || athlete == null) return 0;

            int score = 0;

            // 1. Проверка по рекомендованной программе (до 45 баллов)
            if (athlete.TypePersonalTraining == null)
            {
                PersonalTraining(athlete);
            }

            if (trainer.TrainingType == athlete.TypePersonalTraining)
            {
                score += 45;
            }
            else if (trainer.TrainingType.ToString().Contains(athlete.Gendre.ToString()))
            {
                score += 20;
            }

            // 2. Совпадение по полу (15 баллов)
            if (trainer.Gendre == athlete.Gendre)
            {
                score += 15;
            }

            // 3. Опыт работы (до 20 баллов: 2 балла за каждый год)
            score += Math.Min(trainer.WorkExperience * 2, 20);

            // 4. Свобода графика (до 20 баллов: чем меньше забит тренер, тем выше балл)
            int count = trainer.Athlete?.Count ?? 0;
            score += Math.Max(0, 20 - (count * 4));

            return Math.Min(score, 100);
        }
    }
}



        