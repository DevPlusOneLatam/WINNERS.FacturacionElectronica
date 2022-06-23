DECLARE @EstadoSunat VARCHAR(2)

-- 0 | Por procesar
-- 1 | Aceptado
-- 2 | Aceptado con observacion
-- 3 | Rechazado
-- 4 | En cola
-- 5 | Pendiente Respuesta
-- 6 | Anulado

SET @EstadoSunat =  CASE '{0}' WHEN 1 THEN 'DA' --Documento aprobado
							   WHEN 2 THEN 'DA' --Documento aprobado
							   WHEN 3 THEN 'DR' --Documento rechazado
							   WHEN 6 THEN 'BA' --Com. Baja aprobado
							   ELSE 'DS' END --Documento en seguimiento

UPDATE OINV SET U_IDC_FESTADO = @EstadoSunat, U_IDC_FERESP = '{1}' WHERE DocEntry = '{2}' AND ObjType = '{3}'

UPDATE ORIN SET U_IDC_FESTADO = @EstadoSunat, U_IDC_FERESP = '{1}' WHERE DocEntry = '{2}' AND ObjType = '{3}'

UPDATE ODPI SET U_IDC_FESTADO = @EstadoSunat, U_IDC_FERESP = '{1}' WHERE DocEntry = '{2}' AND ObjType = '{3}'

UPDATE ODLN SET U_IDC_FESTADO = @EstadoSunat, U_IDC_FERESP = '{1}' WHERE DocEntry = '{2}' AND ObjType = '{3}'

UPDATE OWTR SET U_IDC_FESTADO = @EstadoSunat, U_IDC_FERESP = '{1}' WHERE DocEntry = '{2}' AND ObjType = '{3}'

