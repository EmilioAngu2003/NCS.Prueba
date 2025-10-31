using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NCS.Prueba.Models.Entities;

public class Factura : IValidatableObject
{
    [Key]
    [Display(Name = "Nº de Factura")]
    public int? NumeroFactura { get; set; }

    [ForeignKey("Cliente")]
    [Display(Name = "Cliente")]
    [Required(ErrorMessage = "El cliente es un campo obligatorio.")]
    public int IdCliente { get; set; }

    [Column(TypeName = "date")]
    [Required(ErrorMessage = "La fecha es un campo obligatorio.")]
    [DataType(DataType.Date)]
    public DateTime Fecha { get; set; }

    [ValidateNever]
    [NotMapped]
    public decimal Total { get; set; }

    [ValidateNever]
    public Cliente Cliente { get; set; }

    [Required(ErrorMessage = "Las Lineas son un campo obligatorio.")]
    public ICollection<FacturaLinea> FacturaLineas { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (FacturaLineas == null || !FacturaLineas.Any())
        {
            yield return new ValidationResult(
                "Debe agregar al menos una línea a la factura.",
                new[] { nameof(FacturaLineas) }
            );
        }
    }
}