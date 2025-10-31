using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NCS.Prueba.Models.Entities;
using NCS.Prueba.Services.Interfaces;

namespace NCS.Prueba.Controllers;

public class FacturaController : Controller
{
    private readonly IFacturaService _facturaService;
    private readonly IClienteService _clienteService;

    public FacturaController(IFacturaService facturaService, IClienteService clienteService)
    {
        _facturaService = facturaService;
        _clienteService = clienteService;
    }

    public async Task<IActionResult> Index()
    {
        var facturas = await _facturaService.GetAllAsync();
        ViewBag.Titulo = "Gestión de Facturas";
        return View(facturas);
    }
    public async Task<IActionResult> Create()
    {
        var nuevaFactura = new Factura
        {
            NumeroFactura = 0,
            Fecha = DateTime.Today,
            FacturaLineas = new List<FacturaLinea>
            {
                new FacturaLinea { Id = 0 }
            }
        };

        ViewBag.ClientesList = await GetClientesList();
        ViewBag.Titulo = "Crear Nueva Factura";

        return View(nuevaFactura);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Factura nuevaFactura)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.ClientesList = await GetClientesList(nuevaFactura.IdCliente);
            ViewBag.Titulo = "Crear Nueva Factura";
            return View(nuevaFactura);
        }

        try
        {
            var facturaCreada = await _facturaService.CreateFacturaAsync(nuevaFactura);
            return RedirectToAction(nameof(Details), new { id = facturaCreada.NumeroFactura });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Ocurrió un error al intentar guardar la factura: " + ex.Message);
            ViewBag.ClientesList = await GetClientesList(nuevaFactura.IdCliente);
            ViewBag.Titulo = "Crear Nueva Factura";
            return View(nuevaFactura);
        }
    }

    public async Task<IActionResult> Edit(int id)
    {
        var factura = await _facturaService.GetFacturaAsync(id);
        if (factura is null)
            return NotFound();

        ViewBag.ClientesList = await GetClientesList(factura.IdCliente);
        ViewBag.Titulo = $"Editar Factura Nº {id}";

        return View(factura);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Factura facturaActualizada)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.ClientesList = await GetClientesList(facturaActualizada.IdCliente);
            ViewBag.Titulo = $"Editar Factura Nº {facturaActualizada.NumeroFactura}";
            return View(facturaActualizada);
        }

        if (id != facturaActualizada.NumeroFactura)
            return NotFound();

        try
        {
            var facturaEditada = await _facturaService.UpdateFacturaAsync(facturaActualizada);

            if (facturaEditada is null)
                throw new Exception("Error al actualizar");

            return RedirectToAction(nameof(Details), new { id = facturaEditada.NumeroFactura });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Ocurrió un error al actualizar la factura: " + ex.Message);
            ViewBag.ClientesList = await GetClientesList(facturaActualizada.IdCliente);
            ViewBag.Titulo = $"Editar Factura Nº {facturaActualizada.NumeroFactura}";
            return View(facturaActualizada);
        }
    }

    public async Task<IActionResult> Details(int id)
    {
        var factura = await _facturaService.GetFacturaAsync(id);

        if (factura is null)
            return NotFound();

        return View(factura);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var factura = await _facturaService.GetFacturaAsync(id);

        if (factura is null)
            return NotFound();

        ViewBag.Titulo = $"Eliminar Factura Nº {id}";
        return View(factura);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            bool eliminado = await _facturaService.DeleteFacturaAsync(id);

            if (!eliminado) throw new Exception("Error al eliminar la factura");

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "No se pudo eliminar la factura. Intente de nuevo. " + ex.Message);
            return RedirectToAction(nameof(Delete), new { id });
        }
    }


    public async Task<IActionResult> Print(int id)
    {
        var factura = await _facturaService.GetFacturaAsync(id);

        if (factura is null)
        {
            return NotFound();
        }

        return View("Print", factura);
    }

    public async Task<SelectList> GetClientesList(int IdCliente = 0)
    {
        var clientes = await _clienteService.GetAllAsync();
        return new SelectList(clientes, "ID", "Nombre", IdCliente);
    }
}
