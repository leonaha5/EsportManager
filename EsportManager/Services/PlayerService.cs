using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EsportManager.Commands;
using EsportManager.Models;

namespace EsportManager.Services;

public interface IPlayerService
{
    Task<IEnumerable<Player>> GetAllPlayersAsync();
    Task<Player?> GetPlayerByIdAsync(int id);
    Task AddPlayerAsync(Player player);
    Task UpdatePlayerAsync(Player player);
    Task DeletePlayerAsync(int id);

    // Task TrainPlayerAsync(Player player, Training training);
    // Task JoinTournamentAsync(Player player, Tournament tournament);
}

public class PlayerService(IPlayerCommands playerCommands) : IPlayerService
{
    private readonly Random _random = new();

    public async Task<IEnumerable<Player>> GetAllPlayersAsync()
    {
        return await playerCommands.GetAllAsync();
    }

    public async Task<Player?> GetPlayerByIdAsync(int id)
    {
        return await playerCommands.GetByIdAsync(id);
    }

    public async Task AddPlayerAsync(Player player)
    {
        await playerCommands.AddAsync(player);
    }

    public async Task UpdatePlayerAsync(Player player)
    {
        await playerCommands.UpdateAsync(player);
    }

    public async Task DeletePlayerAsync(int id)
    {
        await playerCommands.DeleteAsync(id);
    }

    // public async Task TrainPlayerAsync(Player player, Training training)
    // {
    //     player.SkillLevel += training.SkillIncrease;
    //     player.FatigueLevel = Math.Min(100, player.FatigueLevel + training.FatigueIncrease);
    //     player.StressLevel = Math.Min(100, player.StressLevel + training.StressIncrease);
    //     await playerCommands.UpdateAsync(player);
    // }
    //
    // public async Task JoinTournamentAsync(Player player, Tournament tournament)
    // {
    //     if (player.SkillLevel >= tournament.MinSkillRequired)
    //     {
    //         var randomNumber = _random.Next(1, 201);
    //
    //         if (randomNumber <= player.SkillLevel)
    //         {
    //             player.Points += 100;
    //             player.Money += tournament.PrizePool;
    //         }
    //         else
    //         {
    //             player.Points -= 10;
    //         }
    //
    //         player.FatigueLevel += 10;
    //         player.StressLevel += 10;
    //
    //         await playerCommands.UpdateAsync(player);
    //     }
    //     else
    //     {
    //         throw new Exception($"Player {player.SkillLevel} is out of range");
    //     }
    // }
}