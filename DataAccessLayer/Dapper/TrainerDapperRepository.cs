using System.Collections.Generic;
using System.Data;
using System.Linq;
using App_Model;
using Dapper;
using Microsoft.Data.Sqlite;

namespace DataAccessLayer
{
    public class TrainerDapperRepository : IRepository<Trainer>
    {
        /// <summary>
        /// Строка подключения, используемая для создания подключений к хранилищу данных.
        /// </summary>
        /// <remarks>Инициализируется в конструкторе и неизменяема в течение времени жизни
        /// экземпляра.</remarks>
        private readonly string _connectionString;
        /// <summary>
        /// Инициализирует экземпляр TrainerDapperRepository, устанавливая строку подключения из аргумента или
        /// DatabaseInitializer.ConnectionString при null.
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных; при null используется DatabaseInitializer.ConnectionString.</param>
        public TrainerDapperRepository(string? connectionString = null)
        {
            _connectionString = connectionString ?? DatabaseInitializer.ConnectionString;
        }
        /// <summary>
        /// Создаёт новое подключение SQLite на основе _connectionString.
        /// </summary>
        /// <remarks>Владелец должен освободить ресурс (вызвать Dispose или использовать using) после
        /// использования. Подключение не открывается автоматически.</remarks>
        /// <returns>IDbConnection, представляющее новое подключение SQLite. Подключение не открыто.</returns>
        private IDbConnection CreateConnection() => new SqliteConnection(_connectionString);
        /// <summary>
        /// Вставляет объект Trainer в таблицу Trainers и присваивает свойству Id сгенерированный идентификатор записи.
        /// </summary>
        /// <remarks>Открывается подключение через CreateConnection(), выполняется INSERT и SELECT
        /// last_insert_rowid() для получения идентификатора. Исключения при ошибках выполнения передаются
        /// вызывающему.</remarks>
        /// <param name="item">Объект Trainer с данными для вставки; после выполнения Id обновляется значением, возвращённым базой.</param>
        public void Add(Trainer item)
        {
            using var connection = CreateConnection();
            string sql = @"
                INSERT INTO Trainers (FullName, Gendre, TrainingType, Age, WorkExperience, Rating)
                VALUES (@FullName, @Gendre, @TrainingType, @Age, @WorkExperience, @Rating);
                SELECT last_insert_rowid();";

            item.Id = connection.QuerySingle<int>(sql, item);
        }
        /// <summary>
        /// Обновляет запись Trainer в хранилище по идентификатору, записывая новые значения полей FullName, Gendre,
        /// TrainingType, Age, WorkExperience и Rating.
        /// </summary>
        /// <remarks>Выполняет SQL UPDATE через соединение; все перечисленные поля перезаписываются в базе
        /// данных.</remarks>
        /// <param name="item">Экземпляр Trainer с заполненным Id и обновлёнными значениями полей для сохранения.</param>
        public void Update(Trainer item)
        {
            using var connection = CreateConnection();
            string sql = @"
                UPDATE Trainers 
                SET FullName = @FullName, 
                    Gendre = @Gendre, 
                    TrainingType = @TrainingType, 
                    Age = @Age, 
                    WorkExperience = @WorkExperience, 
                    Rating = @Rating
                WHERE Id = @Id;";

            connection.Execute(sql, item);
        }
        /// <summary>
        /// Удаляет запись тренера из таблицы Trainers по указанному идентификатору.
        /// </summary>
        /// <remarks>Выполняет SQL-команду DELETE через подключение, создаваемое CreateConnection; не
        /// проверяет наличие записи и не возвращает результат выполнения.</remarks>
        /// <param name="id">Идентификатор удаляемой записи тренера.</param>
        public void Delete(int id)
        {
            using var connection = CreateConnection();
            string sql = "DELETE FROM Trainers WHERE Id = @Id;";
            connection.Execute(sql, new { Id = id });
        }
        /// <summary>
        /// Возвращает тренера с указанным идентификатором, включая связанных спортсменов.
        /// </summary>
        /// <remarks>Выполняется LEFT JOIN для загрузки связанных Athlete; дублирующие записи спортсменов
        /// фильтруются. Использует Dapper для сопоставления Trainer и Athlete (splitOn: "Id").</remarks>
        /// <param name="id">Идентификатор тренера.</param>
        /// <returns>Экземпляр Trainer с заполненным списком Athlete или null, если тренер не найден.</returns>
        public Trainer? ReadById(int id)
        {
            using var connection = CreateConnection();
            string sql = @"
                SELECT t.*, a.* 
                FROM Trainers t
                LEFT JOIN Athletes a ON a.TrainerId = t.Id
                WHERE t.Id = @Id;";

            Trainer? trainer = null;

            connection.Query<Trainer, Athlete, Trainer>(
                sql,
                (t, a) =>
                {
                    if (trainer == null)
                    {
                        trainer = t;
                    }
                    if (a != null && !trainer.Athlete.Any(x => x.Id == a.Id))
                    {
                        trainer.Athlete.Add(a);
                    }
                    return trainer;
                },
                new { Id = id },
                splitOn: "Id"
            );

            return trainer;
        }
        /// <summary>
        /// Возвращает последовательность тренеров с заполненными коллекциями связанных атлетов.
        /// </summary>
        /// <remarks>Открывает соединение через CreateConnection, выполняет LEFT JOIN между Trainers и
        /// Athletes и объединяет строки в объекты Trainer с помощью Dapper (multi-mapping, splitOn: "Id").</remarks>
        /// <returns>Коллекция уникальных Trainer, каждый с заполненной коллекцией Athlete.</returns>
        public IEnumerable<Trainer> List()
        {
            using var connection = CreateConnection();
            string sql = @"
                SELECT t.*, a.* 
                FROM Trainers t
                LEFT JOIN Athletes a ON a.TrainerId = t.Id;";

            var trainerDictionary = new Dictionary<int, Trainer>();

            connection.Query<Trainer, Athlete, Trainer>(
                sql,
                (t, a) =>
                {
                    if (!trainerDictionary.TryGetValue(t.Id, out var currentTrainer))
                    {
                        currentTrainer = t;
                        trainerDictionary.Add(currentTrainer.Id, currentTrainer);
                    }

                    if (a != null && !currentTrainer.Athlete.Any(x => x.Id == a.Id))
                    {
                        currentTrainer.Athlete.Add(a);
                    }

                    return currentTrainer;
                },
                splitOn: "Id"
            );

            return trainerDictionary.Values;
        }
    }
}