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
    Task JoinTournamentAsync(Player player, Tournament tournament);
    Task TrainPlayerAsync(Player player, Training training);
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

    public async Task JoinTournamentAsync(Player player, Tournament tournament)
    {
        if (player.SkillLevel >= tournament.MinSkillRequired && player.Money >= tournament.EntryFee)
        {
            player.Money -= tournament.EntryFee;

            var skillDifference = player.SkillLevel - tournament.MinSkillRequired;

            const double scalingFactor = 50.0;
            var winProbability = 0.2 + 0.6 * skillDifference / (skillDifference + scalingFactor);

            var randomValue = _random.NextDouble();

            if (randomValue < winProbability)
            {
                _ = HistoryService.Instance.AddRecord(
                    $"""Player "{player.Nickname}" won with a {winProbability * 100}% chance of winning!""");
                player.Points += _random.Next(10, 101);
                player.Money += tournament.PrizePool;
            }
            else
            {
                _ = HistoryService.Instance.AddRecord(
                    $"""Player "{player.Nickname}" lost with a {winProbability * 100}% chance of winning!""");
                player.Points = Math.Max(0, player.Points - _random.Next(10, 26));
            }

            player.FatigueLevel += 10;
            player.StressLevel += 10;

            await playerCommands.UpdateAsync(player);
        }
    }

    public async Task TrainPlayerAsync(Player player, Training training)
    {
        _ = HistoryService.Instance.AddRecord(
            $"""Player "{player.Nickname}" participated in "{training.Name}" training!""");
        player.SkillLevel += training.SkillIncrease;
        player.FatigueLevel = Math.Min(100, player.FatigueLevel + training.FatigueIncrease);
        player.StressLevel = Math.Min(100, player.StressLevel + training.StressIncrease);
        await playerCommands.UpdateAsync(player);
    }
}