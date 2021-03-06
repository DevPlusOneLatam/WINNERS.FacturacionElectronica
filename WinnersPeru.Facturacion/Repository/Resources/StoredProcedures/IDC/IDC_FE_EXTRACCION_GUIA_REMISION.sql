CREATE PROCEDURE IDC_FE_EXTRACCION_GUIA_REMISION
(@ObjType varchar(15),@DocEntry int )
AS BEGIN


DECLARE @ML nvarchar(3);
	
SET @ML =( SELECT TOP 1 T0."MainCurncy" 
FROM OADM T0 INNER JOIN ADM1 T1 ON T0."Code" = T1."Code");



SELECT DISTINCT
T1.U_BPP_MDTD+T1.U_BPP_MDSD+'-'+RIGHT('00000000' + Ltrim(Rtrim(T1.U_BPP_MDCD)),8) "FE_ID",
T1.U_BPP_CDAD "FE_CODIGO_PUERTO", 
NULL "FE_NUM_CONTENEDOR",
T1.U_BPP_TDC "FE_TIPODOCUMENTO_CONDUCTOR",
T1.U_BPP_DCON "FE_DOCUMENTOIDENTIDAD_CONDUCTOR",
T1.U_BPP_MDFC "FE_LICENCIACONDUCIR",
T1.U_BPP_MDVC "FE_PLACAVEHICULO",
T1.U_BPP_NDOCT "FE_DOCUMENTOIDENTIDAD_TRANSPORTISTA",
CASE WHEN T1.U_BPP_MDNT IS NOT NULL THEN '6' ELSE NULL END "FE_TIPODOCUMENTO_TRANSPORTISTA",
T1.U_BPP_MDNT "FE_RAZONSOCIAL_TRANSPORTISTA",
T1.U_BPP_FIT "FE_FECHAINICIOTRASLADO",
T1.U_BPP_MT "FE_MODALIDADTRASLADO",
T1.U_BPP_NBP "FE_NUMEROBULTO",
'KGM'"FE_UNIDADMEDIDA",
T1.U_BPP_PBT "FE_PESO",
T1.U_BPP_TRSB "FE_INDICADORTRANSBORDO_PROGRAMADO",
T1.U_BPP_MDMT "FE_CODIGOMOTIVO",
CASE WHEN T1.U_BPP_MDMT='01' THEN 'VENTA' WHEN T1.U_BPP_MDMT='14' THEN 'VENTA SUJETA A CONFIRMACION DEL COMPRADOR' WHEN T1.U_BPP_MDMT='02' THEN 'COMPRA' WHEN T1.U_BPP_MDMT='04' THEN 'TRASLADO ENTRE ESTABLECIMIENTOS DE LA MISMA EMPRESA' WHEN T1.U_BPP_MDMT='18' THEN 'TRASLADO EMISOR ITINERANTE CP' WHEN T1.U_BPP_MDMT='08' THEN 'IMPORTACION' WHEN T1.U_BPP_MDMT='09' THEN 'EXPORTACION' WHEN T1.U_BPP_MDMT='13' THEN 'OTROS' END "FE_DESCRIPCIONMOTIVO",
( SELECT TOP 1 CONCAT(Tx1.Street,' ',Tx1.StreetNo,' ',tx1.Block) FROM DLN1 TX0 INNER JOIN OWHS TX1 ON TX1.WhsCode=Tx0.WhsCode WHERE TX0.DocEntry=@DocEntry AND TX0.ObjType=@ObjType) "FE_DIRECCIONPARTIDA",
( SELECT TOP 1 ISNULL(tx1.GlblLocNum,'101010') FROM DLN1 TX0 INNER JOIN OWHS TX1 ON TX1.WhsCode=Tx0.WhsCode WHERE TX0.DocEntry=@DocEntry AND TX0.ObjType=@ObjType) "FE_UBIGEOPARTIDA",
CONCAT(T6.StreetS,' ',ISNULL(T6.StreetNoS,''),' ',ISNULL(T6.BlockS,'')) "FE_DIRECCIONLLEGADA",
ISNULL(T7.GlblLocNum,'101010') "FE_UBIGEOLLEGADA",
T1.U_BPP_MDVN FE_MARCA_VEHICULO,
T1.U_BPP_MDVC FE_NRO_PLACA,
NULL FE_CERTIFICADO_INSCRIPCION,
T8.Name "FE_DCU01",
'' "FE_DCU02",
'' "FE_DCU03",
'' "FE_DCU08",
'' "FE_DCU09",
'' "FE_DCU04",
'' "FE_DCU05",
'' "FE_DCU06",
'' "FE_DCU07",
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
'' "FE_DCU20"
FROM ODLN T1
LEFT JOIN DLN1 T2 ON T1.DocEntry=T2.DocEntry
LEFT JOIN OITM T3 ON T2.ItemCode=T3.ItemCode
LEFT JOIN OCRD T5 ON T1.CardCode = T5.CardCode
LEFT JOIN DLN12 T6 ON T1.DocEntry=T6.DocEntry
LEFT JOIN CRD1 T7 ON T5.CardCode=T7.CardCode
LEFT JOIN "@BPP_CONDUC" T8 ON T1.U_BPP_MDFN=T8.Code
WHERE T1.DocEntry= @docentry	AND  T1.ObjType= @objtype

UNION ALL


SELECT DISTINCT
T1.U_BPP_MDTD+T1.U_BPP_MDSD+'-'+RIGHT('00000000' + Ltrim(Rtrim(T1.U_BPP_MDCD)),8) "FE_ID",
T1.U_BPP_CDAD "FE_CODIGO_PUERTO", 
NULL "FE_NUM_CONTENEDOR",
T1.U_BPP_TDC "FE_TIPODOCUMENTO_CONDUCTOR",
T1.U_BPP_DCON "FE_DOCUMENTOIDENTIDAD_CONDUCTOR",
T1.U_BPP_MDFC "FE_LICENCIACONDUCIR",
T1.U_BPP_MDVC "FE_PLACAVEHICULO",
T1.U_BPP_NDOCT "FE_DOCUMENTOIDENTIDAD_TRANSPORTISTA",
CASE WHEN T1.U_BPP_MDNT IS NOT NULL THEN '6' ELSE NULL END "FE_TIPODOCUMENTO_TRANSPORTISTA",
T1.U_BPP_MDNT "FE_RAZONSOCIAL_TRANSPORTISTA",
T1.U_BPP_FIT "FE_FECHAINICIOTRASLADO",
T1.U_BPP_MT "FE_MODALIDADTRASLADO",
T1.U_BPP_NBP "FE_NUMEROBULTO",
'KGM'"FE_UNIDADMEDIDA",
T1.U_BPP_PBT "FE_PESO",
T1.U_BPP_TRSB "FE_INDICADORTRANSBORDO_PROGRAMADO",
T1.U_BPP_MDMT "FE_CODIGOMOTIVO",
CASE WHEN T1.U_BPP_MDMT='01' THEN 'VENTA' WHEN T1.U_BPP_MDMT='14' THEN 'VENTA SUJETA A CONFIRMACION DEL COMPRADOR' WHEN T1.U_BPP_MDMT='02' THEN 'COMPRA' WHEN T1.U_BPP_MDMT='04' THEN 'TRASLADO ENTRE ESTABLECIMIENTOS DE LA MISMA EMPRESA' WHEN T1.U_BPP_MDMT='18' THEN 'TRASLADO EMISOR ITINERANTE CP' WHEN T1.U_BPP_MDMT='08' THEN 'IMPORTACION' WHEN T1.U_BPP_MDMT='09' THEN 'EXPORTACION' WHEN T1.U_BPP_MDMT='13' THEN 'OTROS' END "FE_DESCRIPCIONMOTIVO",
( SELECT TOP 1 CONCAT(ISNULL(Tx1.Street,''),' ',ISNULL(Tx1.StreetNo,''),' ',ISNULL(tx1.Block,'')) FROM WTR1 TX0 INNER JOIN OWHS TX1 ON TX1.WhsCode=Tx0.FromWhsCod WHERE TX0.DocEntry=@DocEntry AND TX0.ObjType=@ObjType) "FE_DIRECCIONPARTIDA",
( SELECT TOP 1 ISNULL(tx1.GlblLocNum,'101010') FROM WTR1 TX0 INNER JOIN OWHS TX1 ON TX1.WhsCode=Tx0.FromWhsCod WHERE TX0.DocEntry=@DocEntry AND TX0.ObjType=@ObjType) "FE_UBIGEOPARTIDA",
( SELECT TOP 1 CONCAT(ISNULL(Tx1.Street,''),' ',ISNULL(Tx1.StreetNo,''),' ',ISNULL(tx1.Block,'')) FROM WTR1 TX0 INNER JOIN OWHS TX1 ON TX1.WhsCode=Tx0.WhsCode WHERE TX0.DocEntry=@DocEntry AND TX0.ObjType=@ObjType) "FE_DIRECCIONLLEGADA",
( SELECT TOP 1 ISNULL(tx1.GlblLocNum,'101010') FROM WTR1 TX0 INNER JOIN OWHS TX1 ON TX1.WhsCode=Tx0.WhsCode WHERE TX0.DocEntry=@DocEntry AND TX0.ObjType=@ObjType) "FE_UBIGEOLLEGADA",
T1.U_BPP_MDVN FE_MARCA_VEHICULO,
T1.U_BPP_MDVC FE_NRO_PLACA,
NULL FE_CERTIFICADO_INSCRIPCION,
T8.Name "FE_DCU01",
'' "FE_DCU02",
'' "FE_DCU03",
'' "FE_DCU08",
'' "FE_DCU09",
'' "FE_DCU04",
'' "FE_DCU05",
'' "FE_DCU06",
'' "FE_DCU07",
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
'' "FE_DCU20"
FROM OWTR T1
LEFT JOIN WTR1 T2 ON T1.DocEntry=T2.DocEntry
LEFT JOIN OITM T3 ON T2.ItemCode=T3.ItemCode
LEFT JOIN OCRD T5 ON T1.CardCode = T5.CardCode
LEFT JOIN WTR12 T6 ON T1.DocEntry=T6.DocEntry
LEFT JOIN CRD1 T7 ON T5.CardCode=T7.CardCode
LEFT JOIN "@BPP_CONDUC" T8 ON T1.U_BPP_MDFN=T8.Code
WHERE T1.DocEntry= @docentry	AND  T1.ObjType= @ObjType 


END