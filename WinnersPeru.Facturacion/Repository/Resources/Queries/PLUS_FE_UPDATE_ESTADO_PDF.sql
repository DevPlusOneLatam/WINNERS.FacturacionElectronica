DECLARE @EstadoPdf VARCHAR(2)

--PP Pdf Pendiente
--PD Pdf Descargado
--PE Pdf Error al descargar
--PN Pdf No descargar

--Descargado = 0, Error = 1

SET @EstadoPdf =  CASE '{0}' WHEN 0 THEN 'PD'
							 WHEN 1 THEN 'PE'
							 WHEN 2 THEN 'PP'
							 END

UPDATE OINV SET U_PLUS_ESTADOPDF = @EstadoPdf, U_PLUS_RESPDF = '{1}' WHERE DocEntry = '{2}' AND ObjType = '{3}'

UPDATE ORIN SET U_PLUS_ESTADOPDF = @EstadoPdf, U_PLUS_RESPDF = '{1}' WHERE DocEntry = '{2}' AND ObjType = '{3}'

UPDATE ODPI SET U_PLUS_ESTADOPDF = @EstadoPdf, U_PLUS_RESPDF = '{1}' WHERE DocEntry = '{2}' AND ObjType = '{3}'

UPDATE ODLN SET U_PLUS_ESTADOPDF = @EstadoPdf, U_PLUS_RESPDF = '{1}' WHERE DocEntry = '{2}' AND ObjType = '{3}'

UPDATE OWTR SET U_PLUS_ESTADOPDF = @EstadoPdf, U_PLUS_RESPDF = '{1}' WHERE DocEntry = '{2}' AND ObjType = '{3}'