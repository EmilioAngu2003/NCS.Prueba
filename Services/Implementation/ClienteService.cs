using Microsoft.EntityFrameworkCore;
using NCS.Prueba.Models.Entities;
using NCS.Prueba.Repositories.Interfaces;
using NCS.Prueba.Services.Interfaces;

namespace NCS.Prueba.Services.Implementation;

public class ClienteService : IClienteService
{
    private readonly IGenericRepository<Cliente> _clienteRepository;
    private readonly IUnitOfWork _uow;

    public ClienteService(
        IGenericRepository<Cliente> clienteRepository,
        IUnitOfWork uow)
    {
        _clienteRepository = clienteRepository;
        _uow = uow;
    }

    public async Task<Cliente> CreateClienteAsync(Cliente nuevoCliente)
    {
        await _clienteRepository.Create(nuevoCliente);
        await _uow.CommitChangesAsync();
        return nuevoCliente;
    }

    public async Task<Cliente?> UpdateClienteAsync(Cliente clienteActualizado)
    {
        var clienteExistente = await _clienteRepository.ReadOne(c => c.ID == clienteActualizado.ID);
        if (clienteExistente is null)
            return null;

        clienteExistente.Update(clienteActualizado);

        await _uow.CommitChangesAsync();
        return clienteActualizado;
    }

    public async Task<bool> DeleteClienteAsync(int id)
    {
        var clienteQuery = await _clienteRepository.ReadAll(c => c.ID == id);

        var cliente = await clienteQuery
            .Include(c => c.Facturas)
                .ThenInclude(f => f.FacturaLineas)
            .FirstOrDefaultAsync();

        if (cliente is null)
            return true;

        await _clienteRepository.Delete(cliente);
        await _uow.CommitChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Cliente>> GetAllAsync()
    {
        var clientesQuery = await _clienteRepository.ReadAll();
        return await clientesQuery.ToListAsync();
    }

    public async Task<Cliente?> GetClienteAsync(int id)
        => await _clienteRepository.ReadOne(c => c.ID == id);

    public async Task<bool> NIFExisteAsync(string nif, int? idActual = null)
    {
        string nifUpper = nif.ToUpperInvariant();

        var query = await _clienteRepository.ReadAll(c => c.NIF.ToUpper() == nifUpper);

        if (idActual.HasValue)
            query = query.Where(c => c.ID != idActual.Value);

        return await query.AnyAsync();
    }
}
