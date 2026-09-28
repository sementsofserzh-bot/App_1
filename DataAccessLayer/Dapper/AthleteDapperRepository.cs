using System.Collections.Generic;
using System.Data;
using System.Linq;
using App_Model;
using Dapper;
using Microsoft.Data.Sqlite;

namespace DataAccessLayer
{
    public class AthleteDapperRepository : IRepository<Athlete>
    {
        /// <summary>
        /// Строка подключения к базе данных.
        /// </summary>
        /// <remarks>Используется при создании соединений; формат определяется поставщиком
        /// ADO.NET.</remarks>
        private readonly string _connectionString;
        /// <summary>
        /// Инициализирует новый экземпляр AthleteDapperRepository и устанавливает строку подключения.
        /// </summary>
        /// <param name="connectionString">Строка подключения. Если null, используется DatabaseInitializer.ConnectionString.</param>
        public AthleteDapperRepository(string? connectionString = null)
        {
            _connectionString = connectionString ?? DatabaseInitializer.ConnectionString;
        }
        /// <summary>
        /// Создаёт и возвращает новый экземпляр IDbConnection для SQLite, использующий внутреннюю строку подключения.
        /// </summary>
        /// <remarks>Соединение создаётся с помощью SqliteConnection и не открывается автоматически.
        /// Строка подключения берётся из закрытого поля _connectionString.</remarks>
        /// <returns>IDbConnection, представляющий соединение SQLite; соединение не открыто. Вызывающий код обязан открыть и
        /// освободить соединение (Dispose/using).</returns>
        private IDbConnection CreateConnection() => new SqliteConnection(_connectionString);
        /// <summary>
        /// Добавляет спортсмена в таблицу Athletes и устанавливает item.Id в сгенерированный базой данных
        /// идентификатор.
        /// </summary>
        /// <remarks>Проверка item на null не выполняется; при ошибках доступа к базе данных будут
        /// выброшены соответствующие исключения. Операция выполняет INSERT и использует last_insert_rowid() для
        /// получения идентификатора.</remarks>
        /// <param name="item">Спортсмен для вставки; после выполнения свойство Id получает идентификатор, возвращённый базой данных.
        /// Значение TrainerId берётся из item.trainer?.Id.</param>
        public void Add(Athlete item)
        {
            using var connection = CreateConnection();
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
        }
        /// <summary>
        /// Обновляет существующую запись в таблице Athletes по Id, устанавливая значения полей FullName, Gendre, Age,
        /// Height, Weight, TrainingType, TypePersonalTraining и TrainerId.
        /// </summary>
        /// <remarks>Выполняет параметризованный SQL UPDATE через Dapper.</remarks>
        /// <param name="item">Объект Athlete с новыми значениями полей; свойство Id определяет запись для обновления. Если trainer равен
        /// null, TrainerId будет записан как NULL.</param>
        public void Update(Athlete item)
        {
            using var connection = CreateConnection();
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
        }
        /// <summary>
        /// Удаляет запись спортсмена из таблицы Athletes по указанному идентификатору.
        /// </summary>
        /// <remarks>Выполняет параметризованный SQL DELETE через подключение, созданное
        /// CreateConnection().</remarks>
        /// <param name="id">Идентификатор удаляемой записи спортсмена.</param>
        public void Delete(int id)
        {
            using var connection = CreateConnection();
            string sql = "DELETE FROM Athletes WHERE Id = @Id;";
            connection.Execute(sql, new { Id = id });
        }
        /// <summary>
        /// Возвращает спортсмена с указанным идентификатором, включая связанные данные тренера.
        /// </summary>
        /// <remarks>Выполняет SELECT с LEFT JOIN к таблице Trainers и использует Dapper для маппинга
        /// Athlete и Trainer; найденный Trainer присваивается полю athlete.trainer. Подключение создаётся через
        /// CreateConnection и автоматически закрывается.</remarks>
        /// <param name="id">Идентификатор спортсмена.</param>
        /// <returns>Экземпляр Athlete с заполненным полем trainer или null, если запись не найдена.</returns>
        public Athlete? ReadById(int id)
        {
            using var connection = CreateConnection();
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

            return result;
        }
        /// <summary>
        /// Возвращает последовательность всех спортсменов с загруженными данными их тренеров.
        /// </summary>
        /// <remarks>Выполняет LEFT JOIN между таблицами Athletes и Trainers и использует Dapper для
        /// проекции Athlete и Trainer (splitOn="Id").</remarks>
        /// <returns>IEnumerable<Athlete> содержащая спортсменов; у каждого объекта устанавливается свойство trainer (может быть
        /// null).</returns>
        public IEnumerable<Athlete> List()
        {
            using var connection = CreateConnection();
            string sql = @"
                SELECT a.*, t.* 
                FROM Athletes a
                LEFT JOIN Trainers t ON a.TrainerId = t.Id;";

            return connection.Query<Athlete, Trainer, Athlete>(
                sql,
                (athlete, trainer) =>
                {
                    athlete.trainer = trainer;
                    return athlete;
                },
                splitOn: "Id"
            );
        }
    }
}