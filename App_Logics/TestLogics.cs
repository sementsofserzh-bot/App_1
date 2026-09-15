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
        Athlete? CheckAthlete(int id); //просмотр данных об отлете

        Trainer? UpdateInfoTrainer(int id, string fullname, Gendre gendre, TrainingType trainingType, int age, int workExperience, List<Athlete> deleteathlete, List<Athlete> addathlete); // изменение данных тренера, в чтом числе изменение закрепленных за тренером спортсменов
        Athlete? UpdateInfoAthlete(int id, string fullname, Gendre gendre, int age, int height, int wight, TrainingType trainingType, Trainer trainer); // изменение данных спортсмена, в том числе закрепление за тренером

        bool? Registration(Trainer trainer, Athlete athlete); //закрепление спортсмена за тренером
        string PersonalTraining(Athlete athlete); //бизнес функция: подбор типа тренировки по параметрам спортсмена

        List<Trainer> PersonalFilterTrainers(Athlete athlete); //дополнение функции выше которая реализует подбор тренеров по параметрам спортсмена

        List<Trainer> RateTrainers(); // рейтинг тренеров по парметрам: возраст, опыт работы и колл закрепленных спортсменов за тренером

        public class Logics : ILogics //заглушка для тестов, в которой реализованы все методы интерфейса ILogics
        {
            // Базы данных в памяти
            public List<Trainer> BD_Trainer { get; set; } = new List<Trainer>();
            public List<Athlete> BD_Athlete { get; set; } = new List<Athlete>();

            // --- Добавление ---

            public Trainer AddTrainer(string fullname, Gendre gendre, TrainingType trainingType, int age, int workExperience)
            {
                return default!;
            }

            public Athlete AddAthlete(string fullname, Gendre gendre, TrainingType trainingType, int age, int height, int weight)
            {
                return default!;
            }

            // --- Удаление ---

            public bool? RemoveTrainer(int id)
            {
                return null;
            }

            public bool? RemoveAthlete(int id)
            {
                return null;
            }

            // --- Просмотр ---

            public Trainer? CheckTrainer(int id)
            {
                return null;
            }

            public Athlete? CheckAthlete(int id)
            {
                return null;
            }

            // --- Редактирование ---

            public Trainer? UpdateInfoTrainer(int id, string fullname, Gendre gendre, TrainingType trainingType, int age, int workExperience, List<Athlete> deleteathlete, List<Athlete> addathlete)
            {
                return null;
            }

            public Athlete? UpdateInfoAthlete(int id, string fullname, Gendre gendre, int age, int height, int wight, TrainingType trainingType, Trainer trainer)
            {
                return null;
            }

            // --- Бизнес-функции ---

            public bool? Registration(Trainer trainer, Athlete athlete)
            {
                return null;
            }

            public string PersonalTraining(Athlete athlete)
            {
                return string.Empty;
            }

            public List<Trainer> PersonalFilterTrainers(Athlete athlete)
            {
                return new List<Trainer>();
            }

            public List<Trainer> RateTrainers()
            {
                return new List<Trainer>();
            }
        }
    }
}