using Microsoft.Data.Sqlite;

namespace DataAccessLayer
{
    /// <summary>Общее соединение и транзакция репозиториев одной единицы работы.</summary>
    internal sealed class DapperSession : IDisposable
    {
        private readonly string _connectionString;
        private readonly bool _autoCommit;
        private SqliteConnection? _connection;
        private SqliteTransaction? _transaction;
        private bool _disposed;

        /// <summary>Создаёт сессию с автоматическим сохранением или общей транзакцией.</summary>
        public DapperSession(string? connectionString, bool autoCommit = false)
        {
            _connectionString = connectionString ?? DatabaseInitializer.ConnectionString;
            _autoCommit = autoCommit;
        }

        /// <summary>Выполняет чтение на соединении сессии и в текущей транзакции.</summary>
        public T Read<T>(Func<SqliteConnection, SqliteTransaction?, T> action)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            if (_autoCommit)
            {
                using var connection = OpenConnection();
                return action(connection, null);
            }

            return action(GetConnection(), _transaction);
        }

        /// <summary>Выполняет запись и открывает транзакцию, если она ещё не начата.</summary>
        public T Write<T>(Func<SqliteConnection, SqliteTransaction?, T> action)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            if (_autoCommit)
            {
                using var connection = OpenConnection();
                return action(connection, null);
            }

            SqliteConnection connectionInUnit = GetConnection();
            _transaction ??= connectionInUnit.BeginTransaction();

            try
            {
                return action(connectionInUnit, _transaction);
            }
            catch
            {
                _transaction.Rollback();
                _transaction.Dispose();
                _transaction = null;
                throw;
            }
        }

        /// <summary>Подтверждает текущую транзакцию с изменениями.</summary>
        public void Save()
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            if (_transaction == null)
                return;

            _transaction.Commit();
            _transaction.Dispose();
            _transaction = null;
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            try
            {
                _transaction?.Rollback();
            }
            finally
            {
                _transaction?.Dispose();
                _connection?.Dispose();
                _disposed = true;
            }
        }

        /// <summary>Возвращает открытое соединение текущей сессии.</summary>
        private SqliteConnection GetConnection() => _connection ??= OpenConnection();

        /// <summary>Открывает новое подключение SQLite.</summary>
        private SqliteConnection OpenConnection()
        {
            var connection = new SqliteConnection(_connectionString);
            connection.Open();
            return connection;
        }
    }
}
