using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.IO;

namespace DataAccessLayer
{
    public static class DatabaseInitializer
    {
        private static readonly string DbPath = Path.GetFullPath(
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\DataAccessLayer\sports.db")
        );

        public static string ConnectionString => $"Data Source={DbPath};";

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