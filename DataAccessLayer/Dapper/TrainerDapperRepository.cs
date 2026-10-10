using System.Collections.Generic;
using System.Data;
using System.Linq;
using App_Model;
using Contracts;
using Dapper;
using Microsoft.Data.Sqlite;

namespace DataAccessLayer
{
    public class TrainerDapperRepository : IRepository<Trainer>
    {
        private readonly string _connectionString;
        /// <summary>
        /// Инициализирует новый экземпляр TrainerDapperRepository с указанной строкой подключения или с
        /// DatabaseInitializer.ConnectionString по умолчанию.
        /// </summary>
        /// <remarks>Переданное значение сохраняется в поле _connectionString.</remarks>
        /// <param name="connectionString">Строка подключения к базе данных; при null используется DatabaseInitializer.ConnectionString.</param>
        public TrainerDapperRepository(string? connectionString = null)
        {
            _connectionString = connectionString ?? DatabaseInitializer.ConnectionString;
        }
        /// <summary>
        /// Создаёт и возвращает новое подключение IDbConnection к базе данных SQLite, инициализированное текущей
        /// строкой подключения.
        /// </summary>
        /// <returns>Новый экземпляр IDbConnection, инициализированный строкой подключения; подключение не открыто.</returns>
        private IDbConnection CreateConnection() => new SqliteConnection(_connectionString);
        /// <summary>
        /// Добавляет запись тренера в базу данных и устанавливает свойству Id значение, сгенерированное при вставке.
        /// </summary>
        /// <remarks>Открывает подключение, выполняет INSERT и использует last_insert_rowid() для
        /// получения идентификатора. Исключения от операций ADO.NET/Dapper не обрабатываются внутри метода.</remarks>
        /// <param name="item">Trainer для вставки; после выполнения его Id будет установлен в идентификатор вставленной записи.</param>
        public void Add(Trainer item)
        {
            using var connection = CreateConnection();
            connection.Open();

            string sql = @"
                INSERT INTO Trainers (FullName, Gendre, TrainingType, Age, WorkExperience, Rating)
                VALUES (@FullName, @Gendre, @TrainingType, @Age, @WorkExperience, @Rating);
                SELECT last_insert_rowid();";

            item.Id = connection.QuerySingle<int>(sql, item);

            connection.Close();
        }
        /// <summary>
        /// Обновляет существующую запись тренера в базе данных по значению Id.
        /// </summary>
        /// <remarks>Выполняет SQL UPDATE через Dapper: открывает подключение, выполняет запрос и
        /// закрывает подключение. Явной транзакции не используется; при ошибках выполнения выбрасываются
        /// исключения.</remarks>
        /// <param name="item">Экземпляр Trainer с актуальными значениями полей; поле Id определяет запись для обновления.</param>
        public void Update(Trainer item)
        {
            using var connection = CreateConnection();
            connection.Open();

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

            connection.Close();
        }
        /// <summary>
        /// Удаляет запись тренера из таблицы Trainers по заданному идентификатору.
        /// </summary>
        /// <remarks>Открывает подключение и выполняет SQL DELETE; не проверяет наличие записи и не
        /// возвращает результат операции. Исключения при ошибках подключения или выполнения SQL не
        /// обрабатываются.</remarks>
        /// <param name="id">Идентификатор тренера для удаления.</param>
        public void Delete(int id)
        {
            using var connection = CreateConnection();
            connection.Open();

            string sql = "DELETE FROM Trainers WHERE Id = @Id;";
            connection.Execute(sql, new { Id = id });

            connection.Close();
        }
        /// <summary>
        /// Возвращает тренера с указанным идентификатором вместе с его спортсменами, загруженными через LEFT JOIN;
        /// возвращает null, если тренер не найден.
        /// </summary>
        /// <remarks>Выполняет SQL-запрос с LEFT JOIN и использует Dapper для маппинга Trainer и Athlete;
        /// открывает и закрывает соединение и предотвращает дублирование спортсменов по Id.</remarks>
        /// <param name="id">Идентификатор тренера.</param>
        /// <returns>Экземпляр Trainer с заполненным списком Athlete или null, если запись не найдена.</returns>
        public Trainer? ReadById(int id)
        {
            using var connection = CreateConnection();
            connection.Open();

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

            connection.Close();
            return trainer;
        }
        /// <summary>
        /// Возвращает перечисление тренеров с их связанными спортсменами, полученное из базы данных.
        /// </summary>
        /// <remarks>Запрос выполняется с использованием Dapper; соединение открывается и закрывается в
        /// методе. В результирующей коллекции каждый тренер представлен единожды, дубликаты спортсменов
        /// игнорируются.</remarks>
        /// <returns>IEnumerable<Trainer>: перечисление тренеров, у каждого заполнена коллекция Athlete со связанными сущностями
        /// Athlete; для тренеров без спортсменов коллекция будет пустой.</returns>
        public IEnumerable<Trainer> List()
        {
            using var connection = CreateConnection();
            connection.Open();

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

            connection.Close();
            return trainerDictionary.Values.ToList();
        }
    }
}