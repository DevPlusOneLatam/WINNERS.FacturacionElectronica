CREATE PROCEDURE IDC_FE_EXTRACCION_DOCUMENTO_RELACIONADO
(@ObjType varchar(15), @DocEntry int)
AS BEGIN

DECLARE @ML nvarchar(3);
	
SET @ML = (SELECT TOP 1 T0."MainCurncy" 
FROM OADM T0 INNER JOIN ADM1 T1 ON T0."Code" = T1."Code");

SELECT ROW_NUMBER() OVER (ORDER BY FE_TIPODOCUMENTO, FE_SERIE,FE_CORRELATIVO) "FE_RELACIONADOID",
FE_ID,
FE_TIPODOCUMENTO,
FE_SERIE,
FE_CORRELATIVO
FROM 

(

SELECT DISTINCT
T0.U_BPP_MDTD+T0.U_BPP_MDSD+'-'+RIGHT('00000000' + Ltrim(Rtrim(T0.U_BPP_MDCD)),8)   "FE_ID",
T3.U_BPP_MDTD "FE_TIPODOCUMENTO",
T3.U_BPP_MDSD "FE_SERIE",
RIGHT('00000000' + Ltrim(Rtrim(T3.U_BPP_MDCD)),8) "FE_CORRELATIVO"
FROM OINV T0 
INNER JOIN INV1 T1 on T0.DocEntry=T1.DocEntry
INNER JOIN DLN1 T2 on T2.DocEntry=T1.BaseEntry AND T1.BaseType=T2.ObjType 
INNER JOIN ODLN T3 on T3.DocEntry=T2.DocEntry
WHERE T0."DocEntry"=@DocEntry AND T0."ObjType"=@ObjType

) TT WHERE FE_TIPODOCUMENTO IS NOT NULL ;



END