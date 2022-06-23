CREATE PROCEDURE PLUS_FE_SP_LISTAR_GUIAS_REFERENCIAS
(
@DocEntry INT
)
AS BEGIN

SELECT DISTINCT
"GUIA_TIPO"		= 'GUIAEMISIONREMITENTE',
"GUIA_SERIE"	= T1."U_BPP_MDSD",
"GUIA_CORRELATIVO" = RIGHT('00000000' + LTRIM(RTRIM(T1."U_BPP_MDCD")),8)
FROM ODLN T1
	INNER JOIN DLN1 T2 ON T1."DocEntry"=T2."DocEntry"
WHERE T1."DocEntry" IN (SELECT "BaseEntry" FROM INV1 WHERE "BaseType"=15 AND "DocEntry"=@DocEntry)

END