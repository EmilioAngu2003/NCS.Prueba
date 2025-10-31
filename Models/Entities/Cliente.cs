using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace NCS.Prueba.Models.Entities;

public class Cliente
{
    [Key]
    public int? ID { get; set; }

    [Display(Name = "Código Cliente")]
    [Required(ErrorMessage = "El codigo del cliente es un campo obligatorio.")]
    public string CodigoCliente { get; set; }

    [Required(ErrorMessage = "El nombre es un campo obligatorio.")]
    [StringLength(100, ErrorMessage = "Maximo 100 caracteres.")]
    public string Nombre { get; set; }

    [Required(ErrorMessage = "El NIF es un campo obligatorio.")]
    [StringLength(15, ErrorMessage = "Maximo 15 caracteres.")]
    public string NIF { get; set; }

    [Required(ErrorMessage = "El domicilio es un campo obligatorio.")]
    [StringLength(255, ErrorMessage = "Maximo 255 caracteres")]
    public string Domicilio { get; set; }

    [Display(Name = "Población")]
    [Required(ErrorMessage = "La población es un campo obligatorio.")]
    [StringLength(100, ErrorMessage = "Maximo 100 caracteres")]
    public string Poblacion { get; set; }

    [Display(Name = "Código Postal")]
    [Required(ErrorMessage = "El codigo postal es un campo obligatorio.")]
    [StringLength(10, ErrorMessage = "Maximo 10 caracteres")]
    public string CodigoPostal { get; set; }

    [Required(ErrorMessage = "La provincia es un campo obligatorio.")]
    [StringLength(100, ErrorMessage = "Maximo 100 caracteres")]
    public string Provincia { get; set; }

    [Display(Name = "País")]
    [Required(ErrorMessage = "El pais es un campo obligatorio.")]
    [StringLength(50, ErrorMessage = "Maximo 50 caracteres")]
    public string Pais { get; set; }

    [Display(Name = "Fecha de Alta")]
    public DateTime FechaAlta { get; set; }

    [ValidateNever]
    public ICollection<Factura> Facturas { get; set; }


    public void Update(Cliente cliente)
    {
        CodigoCliente = cliente.CodigoCliente;
        Nombre = cliente.Nombre;
        NIF = cliente.NIF;
        Domicilio = cliente.Domicilio;
        Poblacion = cliente.Poblacion;
        CodigoPostal = cliente.CodigoPostal;
        Provincia = cliente.Provincia;
        Pais = cliente.Pais;
    }
}