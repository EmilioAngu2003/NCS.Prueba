using NCS.Prueba.Models.Entities;

namespace NCS.Prueba.Services.Interfaces;

public interface IFacturaService
{
    Task<IEnumerable<Factura>> GetAllAsync();

    Task<Factura?> GetFacturaAsync(int numeroFactura);

    Task<Factura> CreateFacturaAsync(Factura nuevaFactura);

    Task<Factura?> UpdateFacturaAsync(Factura facturaActualizada);

    Task<bool> DeleteFacturaAsync(int numeroFactura);
}