using NCS.Prueba.Models.Entities;

namespace NCS.Prueba.Repositories.Interfaces;

public interface IFacturaRepository : IGenericRepository<Factura>
{
    Task<decimal> CalcularTotalAsync(int numeroFactura);
}
