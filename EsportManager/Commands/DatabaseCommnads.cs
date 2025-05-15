using Dapper;
using Npgsql;

namespace EsportManager.Commands;

public class DatabaseCommands
{
    private readonly string _connectionString;

    public DatabaseCommands(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void InitializeDatabase()
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        connection.Execute("""
                                       CREATE TABLE IF NOT EXISTS Players (
                                           Id SERIAL PRIMARY KEY,
                                           Name TEXT,
                                           Skill INTEGER,
                                           Stress INTEGER,
                                           Fatigue INTEGER,
                                           Points INTEGER,
                                           Game TEXT,
                                           Money DECIMAL
                                       );
                           """);

        connection.Execute("""
                                       CREATE TABLE IF NOT EXISTS Tournaments (
                                           Id SERIAL PRIMARY KEY,
                                           Name TEXT,
                                           Entry DATE,
                                           Prize DECIMAL,
                                           MinSkillRequired INTEGER
                                       );
                           """);

        connection.Execute(@"
            CREATE TABLE IF NOT EXISTS Trainings (
                Id SERIAL PRIMARY KEY,
                Type TEXT,
                SkillIncrease INTEGER,
                FatigueIncrease INTEGER,
                StressIncrease INTEGER
            );
        ");
    }
}