using Microsoft.Data.Sqlite;
using System;
using System.IO; 

namespace JournalApp.Components.Service
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService()
        {
            var dbPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "JournalApp",
                "JournalApp.db"
            );

            //checking for directory 
            Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);

            _connectionString = $"Data Source = {dbPath}"; 

            InitializeDatabase();
        }

        //initialize database

        private void InitializeDatabase()
        {
            //initializing connection
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            //command for creating user table
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Users(
                    userID INTEGER PRIMARY KEY AUTOINCREMENT,
                    userName TEXT NOT NULL UNIQUE,
                    pinHash TEXT NOT NULL,
                    CreatedDate TEXT NOT NULL,
                    LastLoginDate TEXT NOT NULL);
                    ";
                    //executing command
                    command.ExecuteNonQuery();
                    
        }

        public SqliteConnection GetConnection()
        {
            return new SqliteConnection(_connectionString);
        }
    }
}