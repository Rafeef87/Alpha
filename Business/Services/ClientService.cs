
using Business.Models;
using Data.Entities;
using Data.Repositories;
using Data.Extensions;
using Domain.Models;

namespace Business.Services;

public interface IClientService
{
    Task<ClientResult> CreateClientAsync(AddClientFormData formData);
    Task<ClientResult> GetClientsAsync();
    Task<ClientResult> UpdateClientAsync(EditClientFormData formData);
    Task<ClientResult> DeleteClientAsync(string id);
}

public class ClientService(IClientRepository clientRepository) : IClientService
{
    private readonly IClientRepository _clientRepository = clientRepository;

    #region CRUD 

    public async Task<ClientResult> CreateClientAsync(AddClientFormData formData)
    {
        if (formData == null)
            return new ClientResult { Succeeded = false, StatusCode = 400, Error = "Not all required field are supplied." };

        try
        {
            var clientEntity = formData.MapTo<ClientEntity>();
            var result = await _clientRepository.AddAsync(clientEntity);

            return result.Succeeded
                ? new ClientResult { Succeeded = true, StatusCode = 201 }
                : new ClientResult { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };
        }
        catch (Exception)
        {
            // Log the exception (e.g., using a logging framework)
            return new ClientResult { Succeeded = false, StatusCode = 500, Error = "An unexpected error occurred." };
        }
    }

    public async Task<ClientResult> GetClientsAsync()
    {
        var result = await _clientRepository.GetAllAsync();
        return result.MapTo<ClientResult>();
    }
    public async Task<ClientResult> UpdateClientAsync(EditClientFormData formData)
    {
        if (formData == null)
            return new ClientResult { Succeeded = false, StatusCode = 400, Error = "Not all required field are supplied." };

        var clientEntity = formData.MapTo<ClientEntity>();
        var result = await _clientRepository.UpdateAsync(clientEntity);

        return result.Succeeded
            ? new ClientResult { Succeeded = true, StatusCode = 200 }
            : new ClientResult { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };
    }
    public async Task<ClientResult> DeleteClientAsync(string id)
    {

        var clientEntity = new ClientEntity { Id = id };
        var result = await _clientRepository.DeleteAsync(clientEntity);
        return result.Succeeded
            ? new ClientResult { Succeeded = true, StatusCode = 200 }
            : new ClientResult { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };
    }
    #endregion
}