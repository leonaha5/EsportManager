using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using EsportManager.Models;
using Npgsql;

namespace EsportManager.Commands;

public interface IPlayerCommands
{
    Task<IEnumerable<Player>> GetAllAsync();
    Task<Player?> GetByIdAsync(int playerId);
    Task AddAsync(Player player);
    Task UpdateAsync(Player player);
    Task DeleteAsync(int playerId);
}

public class PlayerCommands(string connectionString) : IPlayerCommands
{
    public async Task<IEnumerable<Player>> GetAllAsync()
    {
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();

        return await connection.QueryAsync<Player>("SELECT * FROM players");
    }

    public async Task<Player?> GetByIdAsync(int playerId)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        return await connection.QueryFirstOrDefaultAsync<Player>(
            "SELECT * FROM players WHERE Id = @Id",
            new { Id = playerId }
        );
    }

    public async Task AddAsync(Player player)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync(
            @"INSERT INTO players 
              (nickname, skilllevel, stresslevel, fatiguelevel, points, game, money)
              VALUES
              (@Nickname, @SkillLevel, @StressLevel, @FatigueLevel, @Points, @Game, @Money)",
            player
        );
    }

    public async Task UpdateAsync(Player player)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync(
            @"UPDATE players
              SET nickname     = @Nickname,
                  skilllevel   = @SkillLevel,
                  stresslevel  = @StressLevel,
                  fatiguelevel = @FatigueLevel,
                  points       = @Points,
                  game         = @Game,
                  money        = @Money
              WHERE id = @Id",
            player
        );
    }

    public async Task DeleteAsync(int playerId)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync(
            "DELETE FROM Players WHERE Id = @Id",
            new { Id = playerId }
        );
    }
}