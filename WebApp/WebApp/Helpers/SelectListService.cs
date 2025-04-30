
using Data.Context;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Business.Services;

public class SelectListService(DataContext dbContext)
{
    private readonly DataContext _dbContext = dbContext;

    public async Task<List<SelectListItem>> GetClientsAsync()
    {
        return await _dbContext.Clients
           
            .Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.ClientName
            })
            .ToListAsync();
    }

    public async Task<List<SelectListItem>> GetUsersAsync()
    {
        return await _dbContext.Users
        
            .Select(u => new SelectListItem
            {
                Value = u.Id.ToString(),
                Text = u.UserName
            })
            .ToListAsync(); 
    }

    public async Task<List<SelectListItem>> GetStatusesAsync()
    {
        return await _dbContext.Statuses
            .Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.StatusName
            })
            .ToListAsync(); 
    }
}
