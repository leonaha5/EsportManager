using Dapper;
using Npgsql;

namespace EsportManager.Commands;

public class DatabaseCommands(string connectionString)
{
    public void InitializeDatabase()
    {
        using var connection = new NpgsqlConnection(connectionString);
        connection.Open();

        connection.Execute("""
                           CREATE TABLE IF NOT EXISTS players (
                               id SERIAL PRIMARY KEY,
                               nickname TEXT,
                               skilllevel INTEGER,
                               stresslevel INTEGER,
                               fatiguelevel INTEGER,
                               points INTEGER,
                               game TEXT,
                               money INTEGER
                           );
                           """);

        connection.Execute("""
                           CREATE TABLE IF NOT EXISTS tournaments (
                               id SERIAL PRIMARY KEY,
                               name TEXT,
                               entryfee INTEGER,
                               prizepool INTEGER,
                               minskillrequired INTEGER
                           );
                           """);

        connection.Execute(@"
                           CREATE TABLE IF NOT EXISTS trainings (
                               id SERIAL PRIMARY KEY,
                               name TEXT,
                               skillincrease INTEGER,
                               fatigueincrease INTEGER,
                               stressincrease INTEGER
                           );
        ");
    }
} 