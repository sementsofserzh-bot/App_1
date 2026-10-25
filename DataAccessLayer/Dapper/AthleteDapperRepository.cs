using Contracts;
using Dapper;
using Essence;

namespace DataAccessLayer
{
    /// <summary>Хранит спортсменов через Dapper.</summary>
    public class AthleteDapperRepository : IRepository<Athlete>
    {
        private readonly DapperSession _session;

        /// <summary>Отдельный репозиторий сохраняет каждую операцию сразу.</summary>
        public AthleteDapperRepository(string? connectionString = null)
        {
            _session = new DapperSession(connectionString, autoCommit: true);
        }

        /// <summary>Подключает репозиторий к общей сессии единицы работы.</summary>
        internal AthleteDapperRepository(DapperSession session)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
        }

        public void Add(Athlete item)
        {
            const string sql = @"
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

            item.Id = _session.Write((connection, transaction) =>
                connection.QuerySingle<int>(sql, parameters, transaction));
        }

        public void Update(Athlete item)
        {
            const string sql = @"
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

            _session.Write((connection, transaction) =>
                connection.Execute(sql, parameters, transaction));
        }

        public void Delete(int id)
        {
            _session.Write((connection, transaction) =>
                connection.Execute("DELETE FROM Athletes WHERE Id = @Id;", new { Id = id }, transaction));
        }

        public Athlete? ReadById(int id)
        {
            const string sql = @"
                SELECT a.*, t.*
                FROM Athletes a
                LEFT JOIN Trainers t ON a.TrainerId = t.Id
                WHERE a.Id = @Id;";

            return _session.Read((connection, transaction) =>
                connection.Query<Athlete, Trainer, Athlete>(
                    sql,
                    (athlete, trainer) =>
                    {
                        athlete.trainer = trainer;
                        return athlete;
                    },
                    new { Id = id },
                    transaction: transaction,
                    splitOn: "Id").FirstOrDefault());
        }

        public IEnumerable<Athlete> List()
        {
            const string sql = @"
                SELECT a.*, t.*
                FROM Athletes a
                LEFT JOIN Trainers t ON a.TrainerId = t.Id;";

            return _session.Read((connection, transaction) =>
                connection.Query<Athlete, Trainer, Athlete>(
                    sql,
                    (athlete, trainer) =>
                    {
                        athlete.trainer = trainer;
                        return athlete;
                    },
                    transaction: transaction,
                    splitOn: "Id").ToList());
        }
    }
}
