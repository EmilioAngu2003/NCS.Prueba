using Microsoft.EntityFrameworkCore;
using NCS.Prueba.Data;

namespace NCS.Prueba.Extensions;

public static class MigrationManager
{
    public static IHost MigrateDatabase(this IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<Program>>();
            var context = services.GetRequiredService<ApplicationDbContext>();

            try
            {
                var pendingMigrations = context.Database.GetPendingMigrations().ToList();
                if (pendingMigrations.Any())
                {
                    logger.LogInformation("Se encontraron {Count} migraciones pendientes.", pendingMigrations.Count);
                    context.Database.Migrate();
                    logger.LogInformation("Base de datos actualizada con éxito.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ocurrió un error crítico al migrar la base de datos.");
                throw;
            }
        }
        return host;
    }
}