CREATE PROCEDURE PLUS_FE_SP_LISTAR_CUOTAS
(
@ObjType VARCHAR(15), 
@DocEntry INT
)
AS BEGIN

DECLARE @ML NVARCHAR(3)= (SELECT TOP 1  ISNULL(T0."MainCurncy",'')   FROM OADM T0);

;WITH RECALCULO (DocEntry, Monto)
AS  
(  
	SELECT "DocEntry"
		  ,"Monto" + "Impuesto"
		 FROM (
				SELECT "DocEntry" = MAX(TS1."DocEntry")
					  ,"Monto"	  = CASE WHEN TS1."U_IDC_TAFE" IN (10, 20, 30, 40) THEN ROUND(SUM(TS1."Quantity"*TS1."Price"),2)
										ELSE 0 END
					  ,"Impuesto" = CASE WHEN TS1."U_IDC_TAFE" = 10 THEN ROUND(SUM(TS1."Quantity"*TS1."Price") * (MAX(TS1."VatPrcnt")/100),2)
										ELSE 0 END
				FROM INV1 TS1 
				WHERE TS1."DocEntry"=@DocEntry AND TS1."ObjType" = @ObjType
				GROUP BY TS1."U_IDC_TAFE"
			  ) T0

	UNION ALL

	SELECT "DocEntry"
		  ,"Monto" + "Impuesto"
		 FROM (
			   SELECT "DocEntry" = MAX(TS1.DocEntry)
					  ,"Monto"	  = CASE WHEN TS1."U_IDC_TAFE" IN (10, 20, 30, 40) THEN ROUND(SUM(TS1."Quantity"*TS1."Price"),2)
										ELSE 0 END
					  ,"Impuesto" = CASE WHEN TS1."U_IDC_TAFE" = 10 THEN ROUND(SUM(TS1."Quantity"*TS1."Price") * (MAX(TS1."VatPrcnt")/100),2)
										ELSE 0 END
				FROM DPI1 TS1 
				WHERE TS1."DocEntry"=@DocEntry AND TS1."ObjType" = @ObjType
				GROUP BY TS1."U_IDC_TAFE"
			  ) T1
)  

-- BEGIN FACTURA, BOLETA
SELECT
"CUOTA_LINEA"		= '00' + CAST(T0."InstlmntID" AS VARCHAR),
"CUOTA_FECHA_PAGO"	= CONVERT(VARCHAR(10), T0."DueDate", 23),
"CUOTA_CODIGO_MONEDA" = T2."ISOCurrCod",
"CUOTA_PORCENTAJE"	= T0."InstPrcnt",
"CUOTA_MONTO"		= ROUND(T3."Monto" * (CASE WHEN ISNULL(T1."DiscPrcnt",0) = 0 THEN 1 ELSE T1."DiscPrcnt" END) * (T0."InstPrcnt"/100),2)
- ISNULL((SELECT CASE T1."DocCur" WHEN @ML THEN SUM(T6."DrawnSum") ELSE SUM(T6."DrawnSumFc") END FROM INV9 T6 WHERE T6."DocEntry"=T1."DocEntry"),0)
FROM INV6 T0
	INNER JOIN OINV T1 ON T0."DocEntry" = T1."DocEntry"
	INNER JOIN OCRN T2 ON T1."DocCur" = T2."CurrCode"
	INNER JOIN RECALCULO T3 ON T1."DocEntry" = T3."DocEntry"
WHERE T0."DocEntry" = @DocEntry AND T0."ObjType" = @ObjType
-- END FACTURA, BOLETA

UNION ALL

-- BEGIN ANTICIPOS
SELECT 
"CUOTA_LINEA"		= '00' + CAST(T0."InstlmntID" AS VARCHAR),
"CUOTA_FECHA_PAGO"	= CONVERT(VARCHAR(10), T0."DueDate", 23),
"CUOTA_CODIGO_MONEDA" = T2."ISOCurrCod",
"CUOTA_PORCENTAJE"	= T0."InstPrcnt",
"CUOTA_MONTO"		= ROUND(T3."Monto" * (CASE WHEN ISNULL(T1."DiscPrcnt",0) = 0 THEN 1 ELSE T1."DiscPrcnt" END) * (T0."InstPrcnt"/100),2)
FROM DPI6 T0
	INNER JOIN ODPI T1 ON T0."DocEntry" = T1."DocEntry"
	INNER JOIN OCRN T2 ON T1."DocCur" = T2."CurrCode"
	INNER JOIN RECALCULO T3 ON T1."DocEntry" = T3."DocEntry"
WHERE T0."DocEntry" = @DocEntry AND T0."ObjType" = @ObjType
-- END ANTICIPOS

END