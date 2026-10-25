using Contracts;
using Dapper;
using Essence;

namespace DataAccessLayer
{
    /// <summary>Хранит тренеров через Dapper.</summary>
    public class TrainerDapperRepository : IRepository<Trainer>
    {
        private readonly DapperSession _session;

        /// <summary>Отдельный репозиторий сохраняет каждую операцию сразу.</summary>
        public TrainerDapperRepository(string? connectionString = null)
        {
            _session = new DapperSession(connectionString, autoCommit: true);
        }

        /// <summary>Подключает репозиторий к общей сессии единицы работы.</summary>
        internal TrainerDapperRepository(DapperSession session)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
        }

        public void Add(Trainer item)
        {
            const string sql = @"
                INSERT INTO Trainers (FullName, Gendre, TrainingType, Age, WorkExperience, Rating)
                VALUES (@FullName, @Gendre, @TrainingType, @Age, @WorkExperience, @Rating);
                SELECT last_insert_rowid();";

            item.Id = _session.Write((connection, transaction) =>
                connection.QuerySingle<int>(sql, item, transaction));
        }

        public void Update(Trainer item)
        {
            const string sql = @"
                UPDATE Trainers
                SET FullName = @FullName,
                    Gendre = @Gendre,
                    TrainingType = @TrainingType,
                    Age = @Age,
                    WorkExperience = @WorkExperience,
                    Rating = @Rating
                WHERE Id = @Id;";

            _session.Write((connection, transaction) =>
                connection.Execute(sql, item, transaction));
        }

        public void Delete(int id)
        {
            _session.Write((connection, transaction) =>
                connection.Execute("DELETE FROM Trainers WHERE Id = @Id;", new { Id = id }, transaction));
        }

        public Trainer? ReadById(int id)
        {
            const string sql = @"
                SELECT t.*, a.*
                FROM Trainers t
                LEFT JOIN Athletes a ON a.TrainerId = t.Id
                WHERE t.Id = @Id;";

            return _session.Read((connection, transaction) =>
            {
                Trainer? trainer = null;
                connection.Query<Trainer, Athlete, Trainer>(
                    sql,
                    (current, athlete) =>
                    {
                        trainer ??= current;
                        if (athlete != null && !trainer.Athlete.Any(x => x.Id == athlete.Id))
                            trainer.Athlete.Add(athlete);
                        return trainer;
                    },
                    new { Id = id },
                    transaction: transaction,
                    splitOn: "Id");
                return trainer;
            });
        }

        public IEnumerable<Trainer> List()
        {
            const string sql = @"
                SELECT t.*, a.*
                FROM Trainers t
                LEFT JOIN Athletes a ON a.TrainerId = t.Id;";

            return _session.Read((connection, transaction) =>
            {
                var trainers = new Dictionary<int, Trainer>();
                connection.Query<Trainer, Athlete, Trainer>(
                    sql,
                    (trainer, athlete) =>
                    {
                        if (!trainers.TryGetValue(trainer.Id, out var current))
                        {
                            current = trainer;
                            trainers.Add(current.Id, current);
                        }

                        if (athlete != null && !current.Athlete.Any(x => x.Id == athlete.Id))
                            current.Athlete.Add(athlete);
                        return current;
                    },
                    transaction: transaction,
                    splitOn: "Id");
                return trainers.Values.ToList();
            });
        }
    }
}
