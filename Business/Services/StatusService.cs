
using Business.Models;
using Data.Repositories;
using Domain.Extensions;

namespace Business.Services;

public class StatusService(IStatusRepository statusRepository)
{
    private readonly IStatusRepository _statusRepository = statusRepository;

    public async Task<StatusResult> GetStatusAsync()
    {
        var result = await _statusRepository.GetAllAsync();
        return result.MapTo<StatusResult>();
    }
}