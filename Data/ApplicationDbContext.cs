using Microsoft.EntityFrameworkCore;
using NCS.Prueba.Models.Entities;
using NCS.Prueba.Repositories.Interfaces;
using System.Linq.Expressions;

namespace NCS.Prueba.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Factura> Facturas { get; set; }
    public DbSet<FacturaLinea> FacturaLineas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var propertyMethod = typeof(EF).GetMethod(nameof(EF.Property))!
                    .MakeGenericMethod(typeof(bool));
                var estaEliminadoProperty = Expression.Call(
                    propertyMethod,
                    parameter,
                    Expression.Constant("EstaEliminado"));
                var notEstaEliminado = Expression.Not(estaEliminadoProperty);
                var lambda = Expression.Lambda(notEstaEliminado, parameter);

                modelBuilder.Entity(entityType.ClrType)
                    .HasQueryFilter(lambda);
            }
        }

        modelBuilder.Entity<Cliente>()
            .HasIndex(c => c.CodigoCliente)
            .IsUnique();
        //.HasFilter("EstaEliminado = 0");

        modelBuilder.Entity<Cliente>()
            .HasIndex(c => c.NIF)
            .IsUnique();
        //.HasFilter("EstaEliminado = 0");
    }
}
