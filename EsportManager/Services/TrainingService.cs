using System.Collections.Generic;
using System.Threading.Tasks;
using EsportManager.Commands;
using EsportManager.Models;

namespace EsportManager.Services;

public interface ITrainingService
{
    Task<IEnumerable<Training>> GetAllTrainingsAsync();
    Task AddTrainingAsync(Training training);
    Task DeleteTrainingAsync(Training training);
}

public class TrainingService(ITrainingCommands trainingCommands) : ITrainingService
{
    public async Task<IEnumerable<Training>> GetAllTrainingsAsync()
    {
        return await trainingCommands.GetAllAsync();
    }

    public async Task AddTrainingAsync(Training training)
    {
        await trainingCommands.AddAsync(training);
    }

    public async Task DeleteTrainingAsync(Training training)
    {
        await trainingCommands.DeleteAsync(training.Id);
    }
}