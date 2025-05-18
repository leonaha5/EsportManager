using System.Collections.Generic;
using System.Threading.Tasks;
using EsportManager.Commands;
using EsportManager.Models;

namespace EsportManager.Services;

public interface ITrainingService
{
    Task<IEnumerable<Training>> GetAllTrainingsAsync();
    Task<Training> GetTrainingByIdAsync(int id);
    Task AddTrainingAsync(Training training);
}

public class TrainingService(ITrainingCommands trainingCommands) : ITrainingService
{
    public async Task<IEnumerable<Training>> GetAllTrainingsAsync()
    {
        return await trainingCommands.GetAllAsync();
    }

    public async Task<Training> GetTrainingByIdAsync(int id)
    {
        return await trainingCommands.GetByIdAsync(id);
    }

    public async Task AddTrainingAsync(Training training)
    {
        await trainingCommands.AddAsync(training);
    }
}