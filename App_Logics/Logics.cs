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
        public bool RemoveTrainer(int id)
        {
            var trainer = BD_Trainer.FirstOrDefault(t => t.Id == id);
            if (trainer == null) { return false; }
            BD_Trainer.Remove(trainer);
            return true;
        }
        public bool RemoveAthlete(int id)
        {
            var athlete = BD_Athlete.FirstOrDefault(a => a.Id == id);
            if (athlete == null) { return false; }
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
            if ((athlete.Age >= 15 && athlete.Age <= 17 && Gendre g = Gendre.М) || () ) //Пометка Глебу: Додумать условия для тренировок
            {
                TrainingType t = TrainingType.М_Выносливость;
                string t_str = t.ToString();
                return $"Вам подойдут следующие типы тренировок:{t_str} ";
            }
            
        }
    }
}



        