using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using EsportManager.Models;
using Npgsql;

namespace EsportManager.Commands;

public interface ITournamentCommands
{
    Task<IEnumerable<Tournament>> GetAllAsync();
    Task<Tournament> GetByIdAsync(int id);
    Task AddAsync(Tournament tournament);
    Task DeleteAsync(int tournamentId);
}

public class TournamentCommands(string connectionString) : ITournamentCommands
{
    public async Task<IEnumerable<Tournament>> GetAllAsync()
    {
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        return await connection.QueryAsync<Tournament>("SELECT * FROM tournaments");
    }

    public async Task<Tournament> GetByIdAsync(int id)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        return await connection.QueryFirstOrDefaultAsync<Tournament>("SELECT * FROM tournaments WHERE id = @Id",
            new { id });
    }

    public async Task AddAsync(Tournament tournament)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync(
            "INSERT INTO tournaments (name, entryfee, prizepool, minskillrequired) VALUES (@Name, @EntryFee, @PrizePool, @MinSkillRequired)",
            tournament);
    }

    public async Task DeleteAsync(int id)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync("DELETE FROM tournaments WHERE id = @Id", new { Id = id });
    }
}