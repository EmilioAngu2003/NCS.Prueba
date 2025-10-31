using Microsoft.EntityFrameworkCore;
using NCS.Prueba.Models.Entities;
using NCS.Prueba.Repositories.Interfaces;
using NCS.Prueba.Services.Interfaces;
using System.Globalization;

namespace NCS.Prueba.Services.Implementation;

public class FacturaService : IFacturaService
{
    private readonly IUnitOfWork _uow;
    private readonly IFacturaRepository _facturaRepository;
    private readonly IGenericRepository<FacturaLinea> _lineasRepository;

    public FacturaService(
        IUnitOfWork uow,
        IFacturaRepository facturaRepository,
        IGenericRepository<FacturaLinea> lineasRepository)
    {
        _uow = uow;
        _facturaRepository = facturaRepository;
        _lineasRepository = lineasRepository;
    }

    public async Task<Factura> CreateFacturaAsync(Factura nuevaFactura)
    {
        foreach (var linea in nuevaFactura.FacturaLineas)
        {
            if (decimal.TryParse(linea.PrecioString, NumberStyles.Any, CultureInfo.InvariantCulture, out var precio))
            {
                linea.Precio = precio;
            }
            else
            {
                throw new FormatException($"El valor '{linea.PrecioString}' no es un número decimal válido.");
            }
            linea.Importe = Math.Round(linea.Unidades * linea.Precio, 2);
            linea.Id = null;
            linea.Factura = null;
        }

        await _facturaRepository.Create(nuevaFactura);
        await _uow.CommitChangesAsync();
        return nuevaFactura;
    }

    public async Task<Factura?> UpdateFacturaAsync(Factura facturaActualizada)
        => await _uow.ExecuteInTransactionAsync(async () =>
        {
            var facturaExistente = await _facturaRepository.ReadOne(f => f.NumeroFactura == facturaActualizada.NumeroFactura);
            if (facturaExistente is null)
                throw new Exception("Error al actualizar la factura.");

            facturaExistente.IdCliente = facturaActualizada.IdCliente;
            facturaExistente.Fecha = facturaActualizada.Fecha;

            var lineasQuery = await _lineasRepository.ReadAll(
                l => l.IdFactura == facturaExistente.NumeroFactura
            );

            var lineasExistentes = await lineasQuery.ToListAsync();

            foreach (var lineaActualizada in facturaActualizada.FacturaLineas)
            {
                if (decimal.TryParse(lineaActualizada.PrecioString, NumberStyles.Any, CultureInfo.InvariantCulture, out var precio))
                {
                    lineaActualizada.Precio = precio;
                }
                else
                {
                    throw new FormatException($"El valor '{lineaActualizada.PrecioString}' no es un número decimal válido.");
                }
                lineaActualizada.Importe = Math.Round(lineaActualizada.Unidades * lineaActualizada.Precio, 2);

                lineaActualizada.IdFactura = (int)facturaExistente.NumeroFactura;
                var lineaExistente = lineasExistentes.FirstOrDefault(l => l.Id == lineaActualizada.Id);
                if (lineaExistente is null)
                {
                    lineaActualizada.Id = null;
                    lineaActualizada.Factura = null;
                    await _lineasRepository.Create(lineaActualizada);
                }
                else
                {
                    lineaExistente.Update(lineaActualizada);
                }
            }

            return facturaActualizada;
        });

    public async Task<bool> DeleteFacturaAsync(int numeroFactura)
    {
        var factura = await _facturaRepository.ReadOne(
            f => f.NumeroFactura == numeroFactura,
            f => f.FacturaLineas
        );

        if (factura == null)
            return true;

        await _facturaRepository.Delete(factura);
        await _uow.CommitChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Factura>> GetAllAsync()
    {
        var facturasQuery = await _facturaRepository.ReadAll(
            null,
            f => f.Cliente,
            f => f.FacturaLineas
        );

        return await facturasQuery.ToListAsync();
    }

    public async Task<Factura?> GetFacturaAsync(int numeroFactura)
    {
        var factura = await _facturaRepository.ReadOne(
            f => f.NumeroFactura == numeroFactura,
            f => f.FacturaLineas,
            f => f.Cliente
        );

        if (factura == null)
            return null;

        factura.Total = await _facturaRepository.CalcularTotalAsync(numeroFactura);

        return factura;
    }
}
