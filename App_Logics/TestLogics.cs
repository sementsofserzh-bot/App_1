using App_Model_Essence;

namespace App_Model_TestLogics
{
    public interface ILogics
    {
        List<Trainer> BD_Trainer { get; set; } // типо бд всех тренеров
        List<Athlete> BD_Athlete { get; set; } // типо бд всех атлетов

        Trainer AddTrainer(string fullname, Gendre gendre, TrainingType trainingType, int age, int workExperience); //добавление тренера в базу данных
        Athlete AddAthlete(string fullname, Gendre gendre, TrainingType trainingType, int age, int height, int weight); //добавление спортсмена в базу данных

        bool? RemoveTrainer(int id); //удаление тренера из базы данных
        bool? RemoveAthlete(int id); //удаление спортсмена из базы данных

        Trainer? CheckTrainer(int id); //просмотр данных об тренере
        Athlete? CheckAthlete(int id); //просмотр данных об атлете

        Trainer? UpdateInfoTrainer(int id, string? fullname, Gendre? gendre, TrainingType? trainingType, int? age, int? workExperience, List<Athlete>? deleteathlete, List<Athlete>? addathlete); // изменение данных тренера, в чтом числе изменение закрепленных за тренером спортсменов
        Athlete? UpdateInfoAthlete(int id, string? fullname, Gendre? gendre, int? age, int? height, int? weight, TrainingType? trainingType, Trainer? trainer); // изменение данных спортсмена, в том числе закрепление за тренером

        bool Registration(Trainer trainer, Athlete athlete); //закрепление спортсмена за тренером
        string PersonalTraining(Athlete athlete); //бизнес функция: подбор типа тренировки по параметрам спортсмена

        List<Trainer> PersonalFilterTrainers(Athlete athlete); //дополнение функции выше которая реализует подбор тренеров по параметрам спортсмена

        List<Trainer> RateTrainers(); // рейтинг тренеров по парметрам: возраст, опыт работы и колл закрепленных спортсменов за тренером

        
    }

    public class Logics : ILogics
    {
        public List<Trainer> BD_Trainer { get; set; } = new();
        public List<Athlete> BD_Athlete { get; set; } = new();

        // =========================
        // ДОБАВЛЕНИЕ
        // =========================

        public Trainer AddTrainer(
            string fullname,
            Gendre gendre,
            TrainingType trainingType,
            int age,
            int workExperience)
        {
            throw new NotImplementedException();
        }

        public Athlete AddAthlete(
            string fullname,
            Gendre gendre,
            TrainingType trainingType,
            int age,
            int height,
            int weight)
        {
            throw new NotImplementedException();
        }

        // =========================
        // УДАЛЕНИЕ
        // =========================

        public bool? RemoveTrainer(int id)
        {
            throw new NotImplementedException();
        }

        public bool? RemoveAthlete(int id)
        {
            throw new NotImplementedException();
        }

        // =========================
        // ПРОВЕРКА
        // =========================

        public Trainer? CheckTrainer(int id)
        {
            throw new NotImplementedException();
        }

        public Athlete? CheckAthlete(int id)
        {
            throw new NotImplementedException();
        }

        // =========================
        // ОБНОВЛЕНИЕ ТРЕНЕРА
        // =========================

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
            throw new NotImplementedException();
        }

        // =========================
        // ОБНОВЛЕНИЕ АТЛЕТА
        // =========================

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
            throw new NotImplementedException();
        }

        // =========================
        // РЕГИСТРАЦИЯ
        // =========================

        public bool Registration(Trainer trainer, Athlete athlete)
        {
            throw new NotImplementedException();
        }

        // =========================
        // ПЕРСОНАЛЬНАЯ ТРЕНИРОВКА
        // =========================

        public string PersonalTraining(Athlete athlete)
        {
            throw new NotImplementedException();
        }

        // =========================
        // ФИЛЬТР ТРЕНЕРОВ
        // =========================

        public List<Trainer> PersonalFilterTrainers(Athlete athlete)
        {
            throw new NotImplementedException();
        }

        // =========================
        // РЕЙТИНГ ТРЕНЕРОВ
        // =========================

        public List<Trainer> RateTrainers()
        {
            throw new NotImplementedException();
        }
    }
}