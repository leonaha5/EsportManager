using System.Collections.Generic;
using System.Threading.Tasks;
using EsportManager.Commands;
using EsportManager.Models;

namespace EsportManager.Services;

public interface ITournamentService
{
    Task<IEnumerable<Tournament>> GetAllTournamentAsync();
    Task AddTournamentAsync(Tournament tournament);
    Task DeleteTournamentAsync(Tournament tournament);
}

public class TournamentService(ITournamentCommands tournamentCommands) : ITournamentService
{
    public async Task<IEnumerable<Tournament>> GetAllTournamentAsync()
    {
        return await tournamentCommands.GetAllAsync();
    }

    public async Task AddTournamentAsync(Tournament tournament)
    {
        await tournamentCommands.AddAsync(tournament);
    }

    public async Task DeleteTournamentAsync(Tournament tournament)
    {
        await tournamentCommands.DeleteAsync(tournament.Id);
    }
}