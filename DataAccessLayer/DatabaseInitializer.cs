using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.IO;

namespace DataAccessLayer
{
    /// <summary>
    /// Выполняет инициализацию локальной SQLite‑базы: формирует абсолютный путь к файлу sports.db, предоставляет строку
    /// подключения и создаёт таблицы Trainers и Athletes при отсутствии.
    /// </summary>
    /// <remarks>Путь к файлу вычисляется относительно AppDomain.CurrentDomain.BaseDirectory и может
    /// отличаться в разных средах развёртывания; при необходимости скорректируйте его для продакшн‑сборок. SQL
    /// использует CREATE TABLE IF NOT EXISTS, из-за чего операция идемпотентна; таблица Athletes содержит внешний ключ
    /// TrainerId с поведением ON DELETE SET NULL.</remarks>
    public static class DatabaseInitializer
    {
        /// <summary>
        /// Абсолютный путь к файлу базы данных sports.db, вычисляемый относительно BaseDirectory приложения.
        /// </summary>
        /// <remarks>Вычисляется при инициализации типа с использованием
        /// AppDomain.CurrentDomain.BaseDirectory и относительного перехода к папке DataAccessLayer; значение зависит от
        /// среды выполнения и развертывания и может требовать корректировки для продакшн-сборок.</remarks>
        private static readonly string DbPath = Path.GetFullPath(
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\DataAccessLayer\sports.db")
        );
        /// <summary>
        /// Строка подключения SQLite в формате 'Data Source={DbPath};'.
        /// </summary>
        /// <remarks>Вычисляется динамически на основе текущего значения DbPath.</remarks>
        public static string ConnectionString => $"Data Source={DbPath};";
        /// <summary>
        /// Инициализирует базу данных SQLite: открывает соединение и создаёт таблицы Trainers и Athletes, если они
        /// отсутствуют.
        /// </summary>
        /// <remarks>SQL создаёт схему с полями для тренеров и спортсменов; таблица Athletes содержит
        /// внешний ключ TrainerId с поведением ON DELETE SET NULL. Операция идемпотентна благодаря CREATE TABLE IF NOT
        /// EXISTS.</remarks>
        public static void Initialize()
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            string createTablesSql = @"
                CREATE TABLE IF NOT EXISTS Trainers (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    FullName TEXT NOT NULL,
                    Gendre INTEGER NOT NULL,
                    TrainingType INTEGER NOT NULL,
                    Age INTEGER NOT NULL,
                    WorkExperience INTEGER NOT NULL,
                    Rating REAL NOT NULL
                );

                CREATE TABLE IF NOT EXISTS Athletes (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    FullName TEXT NOT NULL,
                    Gendre INTEGER NOT NULL,
                    Age INTEGER NOT NULL,
                    Height INTEGER NOT NULL,
                    Weight INTEGER NOT NULL,
                    TrainingType INTEGER NOT NULL,
                    TypePersonalTraining INTEGER NULL,
                    TrainerId INTEGER NULL,
                    FOREIGN KEY (TrainerId) REFERENCES Trainers(Id) ON DELETE SET NULL
                );
            ";

            connection.Execute(createTablesSql);
        }
    }
}