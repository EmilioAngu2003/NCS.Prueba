using Microsoft.AspNetCore.Mvc;
using NCS.Prueba.Models.Entities;
using NCS.Prueba.Services.Interfaces;

namespace NCS.Prueba.Controllers;

public class ClienteController : Controller
{
    private readonly IClienteService _clienteService;

    public ClienteController(IClienteService clienteService)
    {
        _clienteService = clienteService;
    }

    public async Task<IActionResult> Index()
    {
        var clientes = await _clienteService.GetAllAsync();
        ViewBag.Titulo = "Gestión de Clientes";
        return View(clientes);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var cliente = await _clienteService.GetClienteAsync(id);
        if (cliente is null)
            return NotFound();

        return View(cliente);
    }

    public async Task<IActionResult> Details(int id)
    {
        var cliente = await _clienteService.GetClienteAsync(id);
        if (cliente is null)
            return NotFound();

        return View(cliente);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Cliente clienteActualizado)
    {
        if (!ModelState.IsValid)
            return View(clienteActualizado);


        if (id != clienteActualizado.ID)
            return NotFound();

        var existingNIF = await _clienteService.NIFExisteAsync(clienteActualizado.NIF, clienteActualizado.ID);
        if (existingNIF)
        {
            ModelState.AddModelError("NIF", "El NIF ya existe en otro cliente.");
            return View(clienteActualizado);
        }

        try
        {
            var clienteEditado = await _clienteService.UpdateClienteAsync(clienteActualizado);

            if (clienteEditado is null)
                throw new Exception("Error al actualizar");

            return RedirectToAction(nameof(Details), new { id = clienteEditado.ID });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Ocurrió un error al actualizar el cliente: " + ex.Message);
            return View(clienteActualizado);
        }
    }

    public async Task<IActionResult> Delete(int id)
    {
        var cliente = await _clienteService.GetClienteAsync(id);

        if (cliente is null)
            return NotFound();

        return View(cliente);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            bool eliminado = await _clienteService.DeleteClienteAsync(id);

            if (!eliminado) throw new Exception("Error al eliminar");

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Ocurrió un error al eliminar el cliente: " + ex.Message);
            return RedirectToAction(nameof(Delete), new { id });
        }
    }

    public IActionResult Create()
    {
        ViewBag.Titulo = "Alta de Nuevo Cliente";
        return View(new Cliente { FechaAlta = DateTime.Now });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Cliente nuevoCliente)
    {
        if (!ModelState.IsValid)
            return View(nuevoCliente);

        var existingNIF = await _clienteService.NIFExisteAsync(nuevoCliente.NIF);
        if (existingNIF)
        {
            ModelState.AddModelError("NIF", "El NIF ya existe en otro cliente.");
            ViewBag.Titulo = "Alta de Nuevo Cliente";
            return View(nuevoCliente);
        }

        try
        {
            var clienteCreado = await _clienteService.CreateClienteAsync(nuevoCliente);
            return RedirectToAction(nameof(Details), new { id = clienteCreado.ID });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Ocurrió un error al intentar guardar la factura: " + ex.Message);
            ViewBag.Titulo = "Alta de Nuevo Cliente";
            return View(nuevoCliente);
        }
    }
}
