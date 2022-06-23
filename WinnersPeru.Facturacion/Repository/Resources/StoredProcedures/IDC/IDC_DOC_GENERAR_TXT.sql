CREATE PROCEDURE IDC_DOC_GENERAR_TXT
@ObjType varchar(15), 
@DocEntry int
AS BEGIN

	DECLARE @cont int;
	DECLARE @CABECERA AS TABLE (
		[DOCENTRY]					int,	
		[OBJTYPE]					int,
		[CODIGO_BODEGA]				varchar(100),
		[CODIGO_CLIENTE]			varchar(100),	
		[CANTIDAD_LINEAS]			varchar(100),
		[CANTIDAD_TRANSACCIONES] 	varchar(100),
		[TIPO_TRANSACCION]			varchar(100),
		[ESTADO]					varchar(100),
		[NOMBRE_CLIENTE]			varchar(100),
		[DIRECCION_CLIENTE]			varchar(MAX),
		[FECHA_TRASLADO]			varchar(100)
	)

	INSERT INTO @CABECERA EXEC [IDC_DOC_EXTRACCION_CABECERA] 
	

	DECLARE @LINEAS AS TABLE (
		[CORRELATIVO_TRANSACCION]		varchar(100),
		[NUMERO_DOCUMENTO_RESPALDO]		varchar(100),	
		[RUC_CLIENTE]					varchar(100),
		[FECHA]							varchar(100),
		[CODIGO_ARTICULO]				varchar(100),
		[NOMBRE_ARTICULO]				varchar(100),
		[ID_ADICIONAL_ARTICULO]			varchar(100),
		[CANTIDAD]						varchar(100),
		[CODIGO_LOCAL]					varchar(100),
		[CODIGO_LOTE]					varchar(100),
		[DOCENTRY]						varchar(100),
		[OBJTYPE]						varchar(100)
		
	)

INSERT INTO @LINEAS EXEC [IDC_DOC_EXTRACCION_LINEA] @ObjType , @DocEntry

SELECT  @cont = COUNT(1) FROM @CABECERA T0 WHERE T0."DOCENTRY" =@DocEntry  AND T0."OBJTYPE" = @ObjType ;


IF (@cont>0)
BEGIN 


	IF ( @ObjType =  '17'  OR @ObjType = '1250000001' )
	
	BEGIN 

		
		SELECT
		 T0.CODIGO_BODEGA
		 + '|' + 'WINPERU'
		 + '|' + T0."CANTIDAD_LINEAS"
		 + '|' + T0."CANTIDAD_TRANSACCIONES" 
		 + '|' + T0."TIPO_TRANSACCION" 
		 + '|' + T0."CODIGO_CLIENTE"
		 + '|' + T0."NOMBRE_CLIENTE"
		 + '|' + T0."DIRECCION_CLIENTE"
		 + '|' + CAST(T0."DOCENTRY" AS NVARCHAR)
		 + '|' + CAST(T0."OBJTYPE" AS NVARCHAR)
		 + '|' + T0."FECHA_TRASLADO"
		 + '|' AS	"CADENA"
		 ,T0."DOCENTRY" AS "DOCENTRY",T0."OBJTYPE"  AS "OBJTYPE"	
		FROM @CABECERA  T0 WHERE T0."DOCENTRY" =@DocEntry  AND T0."OBJTYPE" = @ObjType

		
		UNION ALL
		
		
		SELECT
		 T0."CORRELATIVO_TRANSACCION"
		 + '|' + T0."NUMERO_DOCUMENTO_RESPALDO"
		 + '|' + T0."RUC_CLIENTE"
		 + '|' + T0."FECHA"
		 + '|' + T0."CODIGO_ARTICULO"
		 + '|' + T0."NOMBRE_ARTICULO"
		 + '|' + T0."ID_ADICIONAL_ARTICULO"
		 + '|' + T0."CANTIDAD"
		 + '|' + T0."CODIGO_LOCAL"
		 + '|' + T0."CODIGO_LOTE" 
		 + '|'   AS	"CADENA"
		 ,T0."DOCENTRY" AS "DOCENTRY",T0."OBJTYPE"  AS "OBJTYPE"	
		FROM @LINEAS T0
		 WHERE T0."DOCENTRY" =@DocEntry  AND T0."OBJTYPE" = @ObjType;
		
	END	
		
	
ELSE

SELECT '' as "CADENA",0 as "DOCENTRY",''as "OBJTYPE";

END; 




END