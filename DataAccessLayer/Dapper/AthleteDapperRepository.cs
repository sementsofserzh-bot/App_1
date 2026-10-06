using System.Collections.Generic;
using System.Data;
using System.Linq;
using App_Model;
using Contracts;
using Dapper;
using Microsoft.Data.Sqlite;

namespace DataAccessLayer
{
    public class AthleteDapperRepository : IRepository<Athlete>
    {
        private readonly string _connectionString;
        /// <summary>
        /// Инициализирует новый экземпляр AthleteDapperRepository и задаёт строку подключения, используя переданное
        /// значение или значение по умолчанию.
        /// </summary>
        /// <remarks>Если параметр connectionString равен null, применяется
        /// DatabaseInitializer.ConnectionString.</remarks>
        /// <param name="connectionString">Строка подключения к базе данных; при null используется DatabaseInitializer.ConnectionString.</param>
        public AthleteDapperRepository(string? connectionString = null)
        {
            _connectionString = connectionString ?? DatabaseInitializer.ConnectionString;
        }

        private IDbConnection CreateConnection() => new SqliteConnection(_connectionString);
        /// <summary>
        /// Вставляет объект Athlete в таблицу Athletes и присваивает item.Id сгенерированный в базе идентификатор.
        /// </summary>
        /// <remarks>Открывает соединение через CreateConnection, использует Dapper для выполнения INSERT
        /// и SELECT last_insert_rowid() для получения созданного идентификатора.</remarks>
        /// <param name="item">Экземпляр Athlete для вставки; после успешной вставки свойство Id устанавливается в сгенерированный
        /// идентификатор. TrainerId берётся из item.trainer?.Id.</param>
        public void Add(Athlete item)
        {
            using var connection = CreateConnection();
            connection.Open();

            string sql = @"
                INSERT INTO Athletes (FullName, Gendre, Age, Height, Weight, TrainingType, TypePersonalTraining, TrainerId)
                VALUES (@FullName, @Gendre, @Age, @Height, @Weight, @TrainingType, @TypePersonalTraining, @TrainerId);
                SELECT last_insert_rowid();";

            var parameters = new
            {
                item.FullName,
                item.Gendre,
                item.Age,
                item.Height,
                item.Weight,
                item.TrainingType,
                item.TypePersonalTraining,
                TrainerId = item.trainer?.Id
            };

            item.Id = connection.QuerySingle<int>(sql, parameters);

            connection.Close();
        }
        /// <summary>
        /// Обновляет запись Athletes в базе данных по Id, устанавливая FullName, Gendre, Age, Height, Weight,
        /// TrainingType, TypePersonalTraining и TrainerId.
        /// </summary>
        /// <remarks>Выполняет SQL UPDATE через Dapper, используя соединение, возвращаемое
        /// CreateConnection(). Операция не обёрнута в транзакцию; ошибки пробрасываются вызывающему.</remarks>
        /// <param name="item">Athlete с заполненным Id; его свойства используются для обновления соответствующих столбцов. TrainerId
        /// устанавливается в null, если trainer равен null.</param>
        public void Update(Athlete item)
        {
            using var connection = CreateConnection();
            connection.Open();

            string sql = @"
                UPDATE Athletes 
                SET FullName = @FullName, 
                    Gendre = @Gendre, 
                    Age = @Age, 
                    Height = @Height, 
                    Weight = @Weight, 
                    TrainingType = @TrainingType, 
                    TypePersonalTraining = @TypePersonalTraining, 
                    TrainerId = @TrainerId
                WHERE Id = @Id;";

            var parameters = new
            {
                item.Id,
                item.FullName,
                item.Gendre,
                item.Age,
                item.Height,
                item.Weight,
                item.TrainingType,
                item.TypePersonalTraining,
                TrainerId = item.trainer?.Id
            };

            connection.Execute(sql, parameters);

            connection.Close();
        }
        /// <summary>
        /// Удаляет запись спортсмена с указанным идентификатором из таблицы Athletes.
        /// </summary>
        /// <remarks>Открывает подключение к базе, выполняет SQL-команду DELETE и закрывает подключение.
        /// Если запись с указанным идентификатором отсутствует, изменений не происходит. Не выполняется проверка
        /// входных данных и обработка исключений.</remarks>
        /// <param name="id">Идентификатор удаляемого спортсмена.</param>
        public void Delete(int id)
        {
            using var connection = CreateConnection();
            connection.Open();

            string sql = "DELETE FROM Athletes WHERE Id = @Id;";
            connection.Execute(sql, new { Id = id });

            connection.Close();
        }
        /// <summary>
        /// Возвращает спортсмена с указанным идентификатором вместе с его тренером.
        /// </summary>
        /// <param name="id">Идентификатор спортсмена.</param>
        /// <returns>Экземпляр Athlete с заполненным полем trainer, или null, если запись не найдена.</returns>
        public Athlete? ReadById(int id)
        {
            using var connection = CreateConnection();
            connection.Open();

            string sql = @"
                SELECT a.*, t.* 
                FROM Athletes a
                LEFT JOIN Trainers t ON a.TrainerId = t.Id
                WHERE a.Id = @Id;";

            var result = connection.Query<Athlete, Trainer, Athlete>(
                sql,
                (athlete, trainer) =>
                {
                    athlete.trainer = trainer;
                    return athlete;
                },
                new { Id = id },
                splitOn: "Id"
            ).FirstOrDefault();

            connection.Close();
            return result;
        }
        /// <summary>
        /// Возвращает перечисление спортсменов с загруженными данными их тренеров.
        /// </summary>
        /// <remarks>Использует Dapper с LEFT JOIN для сопоставления Athlete и Trainer и присваивает
        /// объект тренера полю athlete.trainer. Подключение к базе открывается внутри метода и закрывается перед
        /// возвратом.</remarks>
        /// <returns>IEnumerable<Athlete> с найденными спортсменами; коллекция может быть пустой, но не null.</returns>
        public IEnumerable<Athlete> List()
        {
            using var connection = CreateConnection();
            connection.Open();

            string sql = @"
                SELECT a.*, t.* 
                FROM Athletes a
                LEFT JOIN Trainers t ON a.TrainerId = t.Id;";

            var result = connection.Query<Athlete, Trainer, Athlete>(
                sql,
                (athlete, trainer) =>
                {
                    athlete.trainer = trainer;
                    return athlete;
                },
                splitOn: "Id"
            ).ToList();

            connection.Close();
            return result;
        }
    }
}