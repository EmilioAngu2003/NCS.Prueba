using Microsoft.EntityFrameworkCore;
using NCS.Prueba.Data;
using NCS.Prueba.Models.Entities;
using NCS.Prueba.Repositories.Interfaces;

namespace NCS.Prueba.Repositories.Implementation;

public class FacturaRepository : GenericRepository<Factura>, IFacturaRepository
{
    private readonly ApplicationDbContext _context;

    public FacturaRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _context = dbContext;
    }

    public async Task<decimal> CalcularTotalAsync(int numeroFactura)
    {
        return _context.Database
            .SqlQuery<decimal>($"EXEC dbo.CalcularTotalFactura @NumeroFactura = {numeroFactura}")
            .AsEnumerable<decimal>().FirstOrDefault();
    }
}