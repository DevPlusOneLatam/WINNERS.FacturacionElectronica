CREATE PROCEDURE IDC_FE_EXTRACCION_LINEA
(@ObjType varchar(15),@DocEntry int)
AS BEGIN

DECLARE @ML NVARCHAR(20);
SELECT TOP 1 @ML = T0."MainCurncy" FROM OADM T0;

SELECT 
MAX(CASE WHEN T3."DocType" = 'S' THEN 1.00 ELSE T1."Quantity" END) AS "FE_CANTIDAD",
MAX(CASE WHEN T3."DocType" = 'S' THEN NULL ELSE LEFT(T1."ItemCode",34) END) AS "FE_CODIGO",
MAX(ISNULL(T1."Dscription",'')) AS "FE_DESCRIPCION",
MAX(T3.U_BPP_MDTD+T3.U_BPP_MDSD+'-'+RIGHT('00000000' + Ltrim(Rtrim(T3.U_BPP_MDCD)),8)) AS"FE_ID",
(T1."LineNum"+1) AS FE_LINEAID,
MAX(CASE WHEN T1."U_IDC_TAFE" IN ('10','20','30','40') THEN '01' ELSE '02' END) AS "FE_TIPOPRECIO",
MAX(CASE WHEN T3."DocType" = 'S' THEN 'NIU' ELSE ISNULL(T1."unitMsr",ISNULL(T1."unitMsr2",'NIU')) END) AS "FE_UNIDAD_MEDIDA",
MAX(CASE WHEN T3."DocType" = 'S' THEN 'ZZ' WHEN T3."DocType" = 'I' THEN 'NIU' END) AS "FE_UNIDAD_SUNAT",
MAX(T1."Price") AS "FE_VALORUNITARIO",
MAX((CASE WHEN T1."U_IDC_TAFE" IN ('10','11','12','13','14','15','16','17','20','30','40') 
	THEN 
		CASE WHEN T1."Quantity" = 1.000000 OR   (T1."Quantity" =0.000000 AND T3."DocType" = 'S')
			THEN  
				CASE WHEN T1."Currency"=@ML THEN T1."LineTotal" + T1."VatSum"  ELSE T1."TotalFrgn" + T1."VatSumFrgn" END
			ELSE T1."PriceAfVAT"
		END
	ELSE 0.00
END)) AS "FE_PRECIO_VENTA",
MAX(T1."Price" * (CASE WHEN T3."DocType" = 'S' THEN 1.00 ELSE T1."Quantity" END )) AS "FE_VALOR_VENTA",
MAX((T1."PriceAfVAT" - (CASE WHEN T1."U_IDC_TAFE" IN ('10','20','30','40') THEN (T1."PriceBefDi"-T1."Price")*(case when T3."DocType" = 'S' then 1 else T1."Quantity" end) ELSE 0.00 END)) * (CASE WHEN T3."DocType" = 'S' THEN 1 ELSE T1."Quantity" END)) AS "FE_TOTAL_LINEA_CON_IGV",
MAX(T1."U_IDC_TAFE") AS "FE_TIPOAFECTACION",
MAX(CASE WHEN T1."U_IDC_TAFE" IN ('10','20','30','40') THEN ISNULL(T1."DiscPrcnt", 0.00) ELSE 0.00 END) AS "FE_DSCTOPORCENTAJE",
MAX(CASE WHEN T1."U_IDC_TAFE" IN ('10','20','30','40') THEN (T1."PriceBefDi"-T1."Price")*(CASE WHEN T3."DocType" = 'S' then 1 else T1."Quantity" end) ELSE 0.00 END) AS "FE_DSCTOMONTO",
(SELECT distinct MAX(TX1."DistNumber")
FROM OIBT TX0
INNER JOIN OBTN TX1 ON TX1."ItemCode"=TX0."ItemCode" AND TX1."DistNumber"=TX0."BatchNum"
INNER JOIN OBTQ TX2 ON TX2."ItemCode"=TX1."ItemCode" AND TX2."SysNumber"=TX1."SysNumber"
LEFT JOIN IBT1 TX3 ON TX3."ItemCode"=TX0."ItemCode" AND TX3."BatchNum"=TX0."BatchNum"
where TX3."ItemCode" = T1."ItemCode" and TX3."BaseEntry" = T1."BaseEntry") "FE_DCU01",
(SELECT distinct MAX(TX1."ExpDate")
FROM OIBT TX0
INNER JOIN OBTN TX1 ON TX1."ItemCode"=TX0."ItemCode" AND TX1."DistNumber"=TX0."BatchNum"
INNER JOIN OBTQ TX2 ON TX2."ItemCode"=TX1."ItemCode" AND TX2."SysNumber"=TX1."SysNumber"
LEFT JOIN IBT1 TX3 ON TX3."ItemCode"=TX0."ItemCode" AND TX3."BatchNum"=TX0."BatchNum"
where TX3."ItemCode" = T1."ItemCode" and TX3."BaseEntry" = T1."BaseEntry")  "FE_DCU02",
'' "FE_DCU03",
'' "FE_DCU04",
'' "FE_DCU05",
'' "FE_DCU06",
'' "FE_DCU07",
'' "FE_DCU08",
'' "FE_DCU09",
'' "FE_DCU10",
'' "FE_DCU11",
'' "FE_DCU12",
'' "FE_DCU13",
'' "FE_DCU14",
'' "FE_DCU15",
'' "FE_DCU16",
'' "FE_DCU17",
'' "FE_DCU18",
'' "FE_DCU19",
'' "FE_DCU20",
MAX(T2.U_IDC_CODARTSUNAT) "FE_CODIGO_SUNAT"
FROM INV1 T1 
INNER JOIN OINV T3 ON T1."DocEntry" = T3."DocEntry"
LEFT JOIN OITM T2 ON T1."ItemCode" = T2."ItemCode"
LEFT JOIN OITT T4 ON T4."Code" = T1."ItemCode"
INNER JOIN	OCTG T5	ON T5."GroupNum" = T3."GroupNum"
WHERE T3."DocEntry" = @DocEntry and T3."ObjType" = @ObjType AND T1."TreeType"<>'I'
GROUP BY T1."LineNum",T1."DocEntry",T1."ItemCode",T1."BaseEntry"



UNION ALL

SELECT 
MAX(CASE WHEN T3."DocType" = 'S' THEN 1.00 ELSE T1."Quantity" END) AS "FE_CANTIDAD",
MAX(CASE WHEN T3."DocType" = 'S' THEN '' ELSE LEFT(T1."ItemCode",34) END) AS "FE_CODIGO",
mAX(ISNULL(T1."Dscription",'')) AS "FE_DESCRIPCION",
MAX(T3.U_BPP_MDTD+T3.U_BPP_MDSD+'-'+RIGHT('00000000' + Ltrim(Rtrim(T3.U_BPP_MDCD)),8)) AS"FE_ID",
(T1."LineNum"+1) AS FE_LINEAID,
MAX(CASE WHEN T1."U_IDC_TAFE" IN ('10','20','30','40') THEN '01' ELSE '02' END) AS "FE_TIPOPRECIO",
MAX(CASE WHEN T3."DocType" = 'S' THEN 'NIU' ELSE ISNULL(T1."unitMsr",ISNULL(T1."unitMsr2",'NIU')) END) AS "FE_UNIDAD_MEDIDA",
MAX(CASE WHEN T3."DocType" = 'S' THEN 'ZZ' WHEN T3."DocType" = 'I' THEN 'NIU' END) AS "FE_UNIDAD_SUNAT",
MAX(T1."Price") AS "FE_VALORUNITARIO",
MAX((CASE WHEN T1."U_IDC_TAFE" IN ('10','20','30','40') 
	THEN 
		CASE WHEN T1."Quantity" = 1.000000 OR   (T1."Quantity" =0.000000 AND T3."DocType" = 'S')
			THEN  
				CASE WHEN T1."Currency"=@ML THEN T1."LineTotal" + T1."VatSum"  ELSE T1."TotalFrgn" + T1."VatSumFrgn" END
			ELSE T1."PriceAfVAT"
		END
	ELSE 0.00
END)) AS "FE_PRECIO_VENTA",
MAX(T1."Price" * (CASE WHEN T3."DocType" = 'S' THEN 1 ELSE T1."Quantity" END )) AS "FE_VALOR_VENTA",
MAX((T1."PriceAfVAT" - (CASE WHEN T1."U_IDC_TAFE" IN ('10','20','30','40') THEN (T1."PriceBefDi"-T1."Price")*(case when T3."DocType" = 'S' THEN 1 ELSE T1."Quantity" end) ELSE 0.00 END)) * (CASE WHEN T3."DocType" = 'S' THEN 1 ELSE T1."Quantity" END)) AS "FE_TOTAL_LINEA_CON_IGV",
MAX(T1."U_IDC_TAFE") AS "FE_TIPOAFECTACION",
MAX(CASE WHEN T1."U_IDC_TAFE" IN ('10','20','30','40') THEN ISNULL(T1."DiscPrcnt", 0.00) ELSE 0.00 END) AS "FE_DSCTOPORCENTAJE",
MAX(CASE WHEN T1."U_IDC_TAFE" IN ('10','20','30','40') THEN (T1."PriceBefDi"-T1."Price")*(case when T3."DocType" = 'S' then 1 else T1."Quantity" end) ELSE 0.00 END) AS "FE_DSCTOMONTO",
(SELECT distinct MAX(TX1."DistNumber")
FROM OIBT TX0
INNER JOIN OBTN TX1 ON TX1."ItemCode"=TX0."ItemCode" AND TX1."DistNumber"=TX0."BatchNum"
INNER JOIN OBTQ TX2 ON TX2."ItemCode"=TX1."ItemCode" AND TX2."SysNumber"=TX1."SysNumber"
LEFT JOIN IBT1 TX3 ON TX3."ItemCode"=TX0."ItemCode" AND TX3."BatchNum"=TX0."BatchNum"
where TX3."ItemCode" = T1."ItemCode" and TX3."BaseEntry" = T1."BaseEntry") "FE_DCU01",
(SELECT distinct MAX(TX1."ExpDate")
FROM OIBT TX0
INNER JOIN OBTN TX1 ON TX1."ItemCode"=TX0."ItemCode" AND TX1."DistNumber"=TX0."BatchNum"
INNER JOIN OBTQ TX2 ON TX2."ItemCode"=TX1."ItemCode" AND TX2."SysNumber"=TX1."SysNumber"
LEFT JOIN IBT1 TX3 ON TX3."ItemCode"=TX0."ItemCode" AND TX3."BatchNum"=TX0."BatchNum"
where TX3."ItemCode" = T1."ItemCode" and TX3."BaseEntry" = T1."BaseEntry")  "FE_DCU02",
'' "FE_DCU03",
'' "FE_DCU04",
'' "FE_DCU05",
'' "FE_DCU06",
'' "FE_DCU07",
'' "FE_DCU08",
'' "FE_DCU09",
'' "FE_DCU10",
'' "FE_DCU11",
'' "FE_DCU12",
'' "FE_DCU13",
'' "FE_DCU14",
'' "FE_DCU15",
'' "FE_DCU16",
'' "FE_DCU17",
'' "FE_DCU18",
'' "FE_DCU19",
'' "FE_DCU20",
MAX(T2.U_IDC_CODARTSUNAT) "FE_CODIGO_SUNAT"
FROM RIN1 T1 
INNER JOIN ORIN T3 ON T1."DocEntry" = T3."DocEntry"
LEFT JOIN OITM T2 ON T1."ItemCode" = T2."ItemCode"
LEFT JOIN OITT T4 ON T4."Code" = T1."ItemCode"
INNER JOIN	OCTG T5	ON T5."GroupNum" = T3."GroupNum"
LEFT JOIN	IBT1 T6 ON	T6."BaseType" = CASE WHEN ISNULL(T1."BaseType",'')<>'15' THEN T1."ObjType"  ELSE T1."BaseType"  END
							AND T6."BaseEntry"  = CASE WHEN ISNULL(T1."BaseType",'')<>'15' THEN T1."DocEntry" ELSE T1."BaseEntry" END
							AND T6."BaseLinNum" = CASE WHEN ISNULL(T1."BaseType",'')<>'15' THEN T1."LineNum"  ELSE T1."BaseLine"  END
							AND T6."ItemCode" = T1."ItemCode" AND T6."WhsCode"=T1."WhsCode" 
LEFT JOIN OIBT T7 ON T6."ItemCode"=T7."ItemCode" AND T6."BatchNum"=T7."BatchNum"
WHERE T3."DocEntry" = @DocEntry and T3."ObjType" = @ObjType AND T1."TreeType"<>'I'
GROUP BY T1."LineNum",T1."DocEntry",T1."ItemCode",T1."BaseEntry"

UNION ALL

SELECT 
MAX(CASE WHEN T3."DocType" = 'S' THEN 1.00 ELSE T1."Quantity" END) AS "FE_CANTIDAD",
MAX(CASE WHEN T3."DocType" = 'S' THEN '' ELSE LEFT(T1."ItemCode",34) END) AS "FE_CODIGO",
MAX(ISNULL(T1."Dscription",'')) AS "FE_DESCRIPCION",
MAX(T3.U_BPP_MDTD+T3.U_BPP_MDSD+'-'+RIGHT('00000000' + Ltrim(Rtrim(T3.U_BPP_MDCD)),8)) AS"FE_ID",
MAX((T1."LineNum"+1)) AS FE_LINEAID,
MAX(CASE WHEN T1."U_IDC_TAFE" IN ('10','20','30','40') THEN '01' ELSE '02' END) AS "FE_TIPOPRECIO",
MAX(CASE WHEN T3."DocType" = 'S' THEN 'NIU' ELSE ISNULL(T1."unitMsr",ISNULL(T1."unitMsr2",'NIU')) END) AS "FE_UNIDAD_MEDIDA",
MAX(CASE WHEN T3."DocType" = 'S' THEN 'ZZ' WHEN T3."DocType" = 'I' THEN 'NIU' END) AS "FE_UNIDAD_SUNAT",
MAX(CAST(ROUND(T1."Price"*T3."DpmPrcnt"/100.0,2) AS NUMERIC(19,6))) AS "FE_VALORUNITARIO",
MAX((CASE WHEN T1."U_IDC_TAFE" IN ('10','20','30','40') 
	THEN 
		CASE WHEN T1."Quantity" = 1.000000 OR   (T1."Quantity" =0.000000 AND T3."DocType" = 'S')
			THEN  
				CASE WHEN T1."Currency"=@ML THEN T1."LineTotal" + T1."VatSum"  ELSE T1."TotalFrgn" + T1."VatSumFrgn" END
			ELSE T1."PriceAfVAT"
		END
	ELSE 0.00
END)) AS "FE_PRECIO_VENTA",
MAX(CAST(ROUND((CASE WHEN T1."U_IDC_TAFE" IN ('10','20','30','40') THEN  ( 
												T1."Price"*(case when T3."DocType" = 'S' then 1 else T1."Quantity" end)
								)ELSE 0.00 END)*T3."DpmPrcnt"/100.0,2) AS NUMERIC(19,6))) AS "FE_VALOR_VENTA",
MAX(CAST(ROUND((CASE WHEN T1."U_IDC_TAFE" IN ('10','20','30','40') THEN  ( 
												T1."PriceAfVAT"*(case when T3."DocType" = 'S' then 1 else T1."Quantity" end)
								)ELSE 0.00 END)*T3."DpmPrcnt"/100.0,2) AS NUMERIC(19,6))) AS "FE_TOTAL_LINEA_CON_IGV",
MAX(T1."U_IDC_TAFE") AS "FE_TIPOAFECTACION",
MAX(CASE WHEN T1."U_IDC_TAFE" IN ('10','20','30','40') THEN ISNULL(T1."DiscPrcnt", 0.00) ELSE 0.00 END) AS "FE_DSCTOPORCENTAJE",
MAX(CASE WHEN T1."U_IDC_TAFE" IN ('10','20','30','40') THEN (T1."PriceBefDi"-T1."Price")*(case when T3."DocType" = 'S' then 1 else T1."Quantity" end) ELSE 0.00 END) AS "FE_DSCTOMONTO",
(SELECT distinct MAX(TX1."DistNumber")
FROM OIBT TX0
INNER JOIN OBTN TX1 ON TX1."ItemCode"=TX0."ItemCode" AND TX1."DistNumber"=TX0."BatchNum"
INNER JOIN OBTQ TX2 ON TX2."ItemCode"=TX1."ItemCode" AND TX2."SysNumber"=TX1."SysNumber"
LEFT JOIN IBT1 TX3 ON TX3."ItemCode"=TX0."ItemCode" AND TX3."BatchNum"=TX0."BatchNum"
where TX3."ItemCode" = T1."ItemCode" and TX3."BaseEntry" = T1."BaseEntry") "FE_DCU01",
(SELECT distinct MAX(TX1."ExpDate")
FROM OIBT TX0
INNER JOIN OBTN TX1 ON TX1."ItemCode"=TX0."ItemCode" AND TX1."DistNumber"=TX0."BatchNum"
INNER JOIN OBTQ TX2 ON TX2."ItemCode"=TX1."ItemCode" AND TX2."SysNumber"=TX1."SysNumber"
LEFT JOIN IBT1 TX3 ON TX3."ItemCode"=TX0."ItemCode" AND TX3."BatchNum"=TX0."BatchNum"
where TX3."ItemCode" = T1."ItemCode" and TX3."BaseEntry" = T1."BaseEntry")  "FE_DCU02",
'' "FE_DCU03",
'' "FE_DCU04",
'' "FE_DCU05",
'' "FE_DCU06",
'' "FE_DCU07",
'' "FE_DCU08",
'' "FE_DCU09",
'' "FE_DCU10",
'' "FE_DCU11",
'' "FE_DCU12",
'' "FE_DCU13",
'' "FE_DCU14",
'' "FE_DCU15",
'' "FE_DCU16",
'' "FE_DCU17",
'' "FE_DCU18",
'' "FE_DCU19",
'' "FE_DCU20",
MAX(T2.U_IDC_CODARTSUNAT) "FE_CODIGO_SUNAT"
FROM DPI1 T1 
INNER JOIN ODPI T3 ON T1."DocEntry" = T3."DocEntry"
LEFT JOIN OITM T2 ON T1."ItemCode" = T2."ItemCode"
LEFT JOIN OITT T4 ON T4."Code" = T1."ItemCode"
INNER JOIN	OCTG T5	ON T5."GroupNum" = T3."GroupNum"
LEFT JOIN	IBT1 T6 ON	T6."BaseType" = CASE WHEN ISNULL(T1."BaseType",'')<>'15' THEN T1."ObjType"  ELSE T1."BaseType"  END
							AND T6."BaseEntry"  = CASE WHEN ISNULL(T1."BaseType",'')<>'15' THEN T1."DocEntry" ELSE T1."BaseEntry" END
							AND T6."BaseLinNum" = CASE WHEN ISNULL(T1."BaseType",'')<>'15' THEN T1."LineNum"  ELSE T1."BaseLine"  END
							AND T6."ItemCode" = T1."ItemCode" AND T6."WhsCode"=T1."WhsCode" 
LEFT JOIN OIBT T7 ON T6."ItemCode"=T7."ItemCode" AND T6."BatchNum"=T7."BatchNum"
WHERE T3."DocEntry" = @DocEntry and T3."ObjType" = @ObjType AND T1."TreeType"<>'I'
GROUP BY T1."LineNum",T1."DocEntry",T1."ItemCode",T1."BaseEntry"


UNION ALL

SELECT 
(CASE WHEN T3."DocType" = 'S' THEN 1.00 ELSE T1."Quantity" END) AS "FE_CANTIDAD",
(CASE WHEN T3."DocType" = 'S' THEN '' ELSE LEFT(T1."ItemCode",34) END) AS "FE_CODIGO",
(ISNULL(T1."Dscription",'')) AS "FE_DESCRIPCION",
(T3.U_BPP_MDTD+T3.U_BPP_MDSD+'-'+RIGHT('00000000' + Ltrim(Rtrim(T3.U_BPP_MDCD)),8)) AS"FE_ID",
((T1."LineNum"+1)) AS FE_LINEAID,
NULL  "FE_TIPOPRECIO",
(CASE WHEN T3."DocType" = 'S' THEN 'NIU' ELSE ISNULL(T1."unitMsr",ISNULL(T1."unitMsr2",'NIU')) END) AS "FE_UNIDAD_MEDIDA",
(CASE WHEN T3."DocType" = 'S' THEN 'ZZ' WHEN T3."DocType" = 'I' THEN 'NIU' END) AS "FE_UNIDAD_SUNAT",
0.00 "FE_VALORUNITARIO",
0.00 "FE_PRECIO_VENTA",
0.00 "FE_VALOR_VENTA",
0.00 "FE_TOTAL_LINEA_CON_IGV",
NULL "FE_TIPOAFECTACION",
0.00 "FE_DSCTOPORCENTAJE",
0.00 "FE_DSCTOMONTO",
NULL "FE_DCU01",
NULL "FE_DCU02",
NULL "FE_DCU03",
NULL "FE_DCU04",
NULL "FE_DCU05",
NULL "FE_DCU06",
NULL "FE_DCU07",
NULL "FE_DCU08",
NULL "FE_DCU09",
NULL "FE_DCU10",
NULL "FE_DCU11",
NULL "FE_DCU12",
NULL "FE_DCU13",
NULL "FE_DCU14",
NULL "FE_DCU15",
NULL "FE_DCU16",
NULL "FE_DCU17",
NULL "FE_DCU18",
NULL "FE_DCU19",
NULL "FE_DCU20",
NULL
FROM DLN1 T1 
INNER JOIN ODLN T3 ON T1."DocEntry" = T3."DocEntry"
LEFT JOIN OITM T2 ON T1."ItemCode" = T2."ItemCode"
LEFT JOIN OITT T4 ON T4."Code" = T1."ItemCode"
INNER JOIN	OCTG T5	ON T5."GroupNum" = T3."GroupNum"
WHERE T3."DocEntry" = @DocEntry and T3."ObjType" = @ObjType AND T1."TreeType"<>'I'


UNION ALL

SELECT 
(CASE WHEN T3."DocType" = 'S' THEN 1.00 ELSE T1."Quantity" END) AS "FE_CANTIDAD",
(CASE WHEN T3."DocType" = 'S' THEN '' ELSE LEFT(T1."ItemCode",34) END) AS "FE_CODIGO",
(ISNULL(T1."Dscription",'')) AS "FE_DESCRIPCION",
(T3.U_BPP_MDTD+T3.U_BPP_MDSD+'-'+RIGHT('00000000' + Ltrim(Rtrim(T3.U_BPP_MDCD)),8)) AS"FE_ID",
((T1."LineNum"+1)) AS FE_LINEAID,
NULL  "FE_TIPOPRECIO",
(CASE WHEN T3."DocType" = 'S' THEN 'NIU' ELSE ISNULL(T1."unitMsr",ISNULL(T1."unitMsr2",'NIU')) END) AS "FE_UNIDAD_MEDIDA",
(CASE WHEN T3."DocType" = 'S' THEN 'ZZ' WHEN T3."DocType" = 'I' THEN 'NIU' END) AS "FE_UNIDAD_SUNAT",
0.00 "FE_VALORUNITARIO",
0.00 "FE_PRECIO_VENTA",
0.00 "FE_VALOR_VENTA",
0.00 "FE_TOTAL_LINEA_CON_IGV",
NULL "FE_TIPOAFECTACION",
0.00 "FE_DSCTOPORCENTAJE",
0.00 "FE_DSCTOMONTO",
NULL "FE_DCU01",
NULL "FE_DCU02",
NULL "FE_DCU03",
NULL "FE_DCU04",
NULL "FE_DCU05",
NULL "FE_DCU06",
NULL "FE_DCU07",
NULL "FE_DCU08",
NULL "FE_DCU09",
NULL "FE_DCU10",
NULL "FE_DCU11",
NULL "FE_DCU12",
NULL "FE_DCU13",
NULL "FE_DCU14",
NULL "FE_DCU15",
NULL "FE_DCU16",
NULL "FE_DCU17",
NULL "FE_DCU18",
NULL "FE_DCU19",
NULL "FE_DCU20",
NULL
FROM WTR1 T1 
INNER JOIN OWTR T3 ON T1."DocEntry" = T3."DocEntry"
LEFT JOIN OITM T2 ON T1."ItemCode" = T2."ItemCode"
LEFT JOIN OITT T4 ON T4."Code" = T1."ItemCode"
--INNER JOIN	OCTG T5	ON T5."GroupNum" = T3."GroupNum"
WHERE T3."DocEntry" = @DocEntry and T3."ObjType" = @ObjType AND T1."TreeType"<>'I'

END