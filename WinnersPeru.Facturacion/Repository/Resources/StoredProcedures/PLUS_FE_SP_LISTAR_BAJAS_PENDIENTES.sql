CREATE PROCEDURE PLUS_FE_SP_LISTAR_BAJAS_PENDIENTES
AS BEGIN

SELECT * FROM  
(

SELECT
"FACTURACION_ID"		= T0."U_BPP_MDTD"+'-'+T0."U_BPP_MDSD"+'-'+RIGHT('00000000' + LTRIM(RTRIM(T0."U_BPP_MDCD")),8),
"FACTURACION_DOCENTRY"	= T0."DocEntry",
"FACTURACION_OBJTYPE"	= T0."ObjType",
"FACTURACION_LINEA_ID"	= 1,
"DOC_TIPO"				= T0."U_BPP_MDTD",
"DOC_SERIE"				= T0."U_BPP_MDSD",
"DOC_CORRELATIVO"		= RIGHT('00000000' + LTRIM(RTRIM(T0."U_BPP_MDCD")),8),
"DOC_FECHA_EMISION"		= CONVERT(VARCHAR(10), T0."DocDate", 23),
"BAJA_FECHA_GENERACION"	= CONVERT(VARCHAR(10), GETDATE(), 23),
"BAJA_MOTIVO_DESCRIPCION"= 'ERROR'
FROM OINV T0
WHERE T0."CANCELED" <> 'C' AND T0."U_IDC_FESTADO"='BP' AND LEFT(T0."U_BPP_MDSD",1)='F'

UNION ALL

SELECT
"FACTURACION_ID"		= T0."U_BPP_MDTD"+'-'+T0."U_BPP_MDSD"+'-'+RIGHT('00000000' + LTRIM(RTRIM(T0."U_BPP_MDCD")),8),
"FACTURACION_DOCENTRY"	= T0."DocEntry",
"FACTURACION_OBJTYPE"	= T0."ObjType",
"FACTURACION_LINEA_ID"	= 1,
"DOC_TIPO"				= T0."U_BPP_MDTD",
"DOC_SERIE"				= T0."U_BPP_MDSD",
"DOC_CORRELATIVO"		= RIGHT('00000000' + LTRIM(RTRIM(T0."U_BPP_MDCD")),8),
"DOC_FECHA_EMISION"		= CONVERT(VARCHAR(10), T0."DocDate", 23),
"BAJA_FECHA_GENERACION"	= CONVERT(VARCHAR(10), GETDATE(), 23),
"BAJA_MOTIVO_DESCRIPCION"= 'ERROR'
FROM ORIN T0
WHERE T0."CANCELED" <> 'C' AND T0."U_IDC_FESTADO"='BP' AND LEFT(T0."U_BPP_MDSD",1)='F'

UNION ALL

SELECT
"FACTURACION_ID"		= T0."U_BPP_MDTD"+'-'+T0."U_BPP_MDSD"+'-'+RIGHT('00000000' + LTRIM(RTRIM(T0."U_BPP_MDCD")),8),
"FACTURACION_DOCENTRY"	= T0."DocEntry",
"FACTURACION_OBJTYPE"	= T0."ObjType",
"FACTURACION_LINEA_ID"	= 1,
"DOC_TIPO"				= T0."U_BPP_MDTD",
"DOC_SERIE"				= T0."U_BPP_MDSD",
"DOC_CORRELATIVO"		= RIGHT('00000000' + LTRIM(RTRIM(T0."U_BPP_MDCD")),8),
"DOC_FECHA_EMISION"		= CONVERT(VARCHAR(10), T0."DocDate", 23),
"BAJA_FECHA_GENERACION"	= CONVERT(VARCHAR(10), GETDATE(), 23),
"BAJA_MOTIVO_DESCRIPCION"= 'ERROR'
FROM ODPI T0
WHERE T0."CANCELED" <> 'C' AND T0."U_IDC_FESTADO"='BP' AND LEFT(T0."U_BPP_MDSD",1)='F'

UNION ALL

SELECT
"FACTURACION_ID"		= T0."U_BPP_MDTD"+'-'+T0."U_BPP_MDSD"+'-'+RIGHT('00000000' + LTRIM(RTRIM(T0."U_BPP_MDCD")),8),
"FACTURACION_DOCENTRY"	= T0."DocEntry",
"FACTURACION_OBJTYPE"	= T0."ObjType",
"FACTURACION_LINEA_ID"	= 1,
"DOC_TIPO"				= T0."U_BPP_MDTD",
"DOC_SERIE"				= T0."U_BPP_MDSD",
"DOC_CORRELATIVO"		= RIGHT('00000000' + LTRIM(RTRIM(T0."U_BPP_MDCD")),8),
"DOC_FECHA_EMISION"		= CONVERT(VARCHAR(10), T0."DocDate", 23),
"BAJA_FECHA_GENERACION"	= CONVERT(VARCHAR(10), GETDATE(), 23),
"BAJA_MOTIVO_DESCRIPCION"= 'ERROR'
FROM ODLN T0
WHERE T0."CANCELED" <> 'C' AND T0."U_IDC_FESTADO"='BP' AND LEFT(T0."U_BPP_MDSD",1)='T'

UNION ALL

SELECT
"FACTURACION_ID"		= T0."U_BPP_MDTD"+'-'+T0."U_BPP_MDSD"+'-'+RIGHT('00000000' + LTRIM(RTRIM(T0."U_BPP_MDCD")),8),
"FACTURACION_DOCENTRY"	= T0."DocEntry",
"FACTURACION_OBJTYPE"	= T0."ObjType",
"FACTURACION_LINEA_ID"	= 1,
"DOC_TIPO"				= T0."U_BPP_MDTD",
"DOC_SERIE"				= T0."U_BPP_MDSD",
"DOC_CORRELATIVO"		= RIGHT('00000000' + LTRIM(RTRIM(T0."U_BPP_MDCD")),8),
"DOC_FECHA_EMISION"		= CONVERT(VARCHAR(10), T0."DocDate", 23),
"BAJA_FECHA_GENERACION"	= CONVERT(VARCHAR(10), GETDATE(), 23),
"BAJA_MOTIVO_DESCRIPCION"= 'ERROR'
FROM OWTR T0
WHERE T0."CANCELED" <> 'C' AND T0."U_IDC_FESTADO"='BP' AND LEFT(T0."U_BPP_MDSD",1)='T'

) T;

END