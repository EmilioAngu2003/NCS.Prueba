CREATE PROCEDURE dbo.CalcularTotalFactura (@NumeroFactura INT)
AS
BEGIN
    SELECT 
        SUM(Importe) 
    FROM 
        FacturaLineas
    WHERE 
        IdFactura = @NumeroFactura;
END