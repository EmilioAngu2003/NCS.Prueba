using NCS.Prueba.Models.Entities;

namespace NCS.Prueba.Services.Interfaces;

public interface IClienteService
{
    Task<IEnumerable<Cliente>> GetAllAsync();

    Task<Cliente?> GetClienteAsync(int id);

    Task<Cliente> CreateClienteAsync(Cliente nuevoCliente);

    Task<Cliente?> UpdateClienteAsync(Cliente clienteActualizado);

    Task<bool> DeleteClienteAsync(int id);

    Task<bool> NIFExisteAsync(string nif, int? idActual = null);
}
