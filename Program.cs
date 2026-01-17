using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using NCS.Prueba.Data;
using NCS.Prueba.Repositories.Implementation;
using NCS.Prueba.Repositories.Interfaces;
using NCS.Prueba.Services.Implementation;
using NCS.Prueba.Services.Interfaces;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

var conString = builder.Configuration.GetConnectionString("DefaultConnection") ??
     throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(conString)
);

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IFacturaRepository, FacturaRepository>();

builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IFacturaService, FacturaService>();

builder.Services.AddControllersWithViews()
    .AddDataAnnotationsLocalization()
    .AddViewLocalization()
    .AddMvcOptions(options =>
    {
        options.ModelBindingMessageProvider.SetValueMustBeANumberAccessor(
            (value) => $"No es un número válido."
        );
    });

var app = builder.Build();

var defaultCulture = new CultureInfo("es-ES");
defaultCulture.NumberFormat.NumberDecimalSeparator = ",";
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(defaultCulture),
    SupportedCultures = new List<CultureInfo> { defaultCulture },
    SupportedUICultures = new List<CultureInfo> { defaultCulture }
};
app.UseRequestLocalization(localizationOptions);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
