using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace NCS.Prueba.Models.Entities;

public class FacturaLinea
{
    private string _precioString;

    [Key]
    public int? Id { get; set; }

    [ForeignKey("Factura")]
    public int IdFactura { get; set; }

    [Required(ErrorMessage = "Campo obligatoro.")]
    [StringLength(255, ErrorMessage = "Maximo 255 caracteres.")]
    public string Concepto { get; set; }

    [Required(ErrorMessage = "Campo obligatoro.")]
    [Range(1, int.MaxValue, ErrorMessage = "Solo valores positivos.")]
    [RegularExpression(@"^-?\d+$", ErrorMessage = "Solo números enteros.")]
    public int Unidades { get; set; }

    [NotMapped]
    [Required(ErrorMessage = "Campo obligatoro.")]
    [CustomValidation(typeof(PrecioValidation), nameof(PrecioValidation.ValidarPositivo))]
    public string PrecioString
    {
        get => _precioString;
        set => _precioString = PrecioValidation.EstandarizarDecimalString(value);
    }

    [Column(TypeName = "decimal(18, 2)")]
    [ValidateNever]
    public decimal Precio { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    [ValidateNever]
    public decimal Importe { get; set; }

    [ValidateNever]
    public Factura Factura { get; set; }

    public void Update(FacturaLinea linea)
    {
        Concepto = linea.Concepto;
        Unidades = linea.Unidades;
        Precio = linea.Precio;
        Importe = linea.Importe;
    }
}

public static class PrecioValidation
{
    public static ValidationResult ValidarPositivo(string value, ValidationContext context)
    {
        if (string.IsNullOrWhiteSpace(value))
            return ValidationResult.Success;

        if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var precio))
        {
            if (precio <= 0)
                return new ValidationResult("Solo valores positivos.");

            if (precio * 100 != Math.Truncate(precio * 100))
                return new ValidationResult("Máximo 2 decimales.");
        }
        else
        {
            return new ValidationResult("No es un número válido.");
        }

        return ValidationResult.Success;
    }

    public static string EstandarizarDecimalString(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        string value = input.Trim();

        bool tieneComa = value.Contains(',');
        bool tienePunto = value.Contains('.');

        if (tieneComa && (!tienePunto || value.LastIndexOf(',') > value.LastIndexOf('.')))
        {
            value = value.Replace(".", "");
            value = value.Replace(",", ".");
        }
        else if (tienePunto && (!tieneComa || value.LastIndexOf('.') > value.LastIndexOf(',')))
        {
            value = value.Replace(",", "");
        }

        return value;
    }
}