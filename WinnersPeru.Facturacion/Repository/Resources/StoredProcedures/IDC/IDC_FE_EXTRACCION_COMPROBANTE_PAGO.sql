CREATE PROCEDURE IDC_FE_EXTRACCION_COMPROBANTE_PAGO
(@ObjType varchar(15),@DocEntry int )
AS BEGIN

DECLARE @ML nvarchar(3);

SELECT TOP 1 @ML = T0."MainCurncy" 
FROM OADM T0 INNER JOIN ADM1 T1 ON T0."Code" = T1."Code";



END