using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using EsportManager.Models;
using Npgsql;

namespace EsportManager.Commands;

public interface ITrainingCommands
{
    Task<IEnumerable<Training>> GetAllAsync();
    Task<Training> GetByIdAsync(int id);
    Task AddAsync(Training training);
}

public class TrainingCommands(string connectionString) : ITrainingCommands
{
    public async Task<IEnumerable<Training>> GetAllAsync()
    {
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        return await connection.QueryAsync<Training>("SELECT * FROM trainings");
    }

    public async Task<Training> GetByIdAsync(int trainingId)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        return await connection.QueryFirstOrDefaultAsync<Training>("SELECT * FROM trainings WHERE id = @id",
            new { id = trainingId });
    }

    public async Task AddAsync(Training training)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync(
            "INSERT INTO trainings (name, skillincrease, fatigueincrease, stressincrease) VALUES (@Name, @SkillIncrease, @FatigueIncrease, @StressIncrease)",
            training);
    }
}