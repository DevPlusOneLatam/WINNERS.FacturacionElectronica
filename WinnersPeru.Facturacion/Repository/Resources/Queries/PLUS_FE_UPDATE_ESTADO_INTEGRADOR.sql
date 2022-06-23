DECLARE @EstadoDoc VARCHAR(2)

--PE Doc. Pendiente
--DR Doc. Rechazado
--DA Doc. Aprobado
--DE Doc. Con Error
--DC Doc. Corregido
--DS Doc. En Seguimiento

--BP Baja Pendiente
--BS Baja En Seguimiento
--BE Baja Con Error
--BA Baja Aprobado
--BR Baja Rechazado

 --DocEnSeguimiento = 0, DocConError = 1, BajaEnSeguimiento = 2, BajaConError = 3

SET @EstadoDoc =  CASE '{0}' WHEN 0 THEN 'DS'
							 WHEN 1 THEN 'DE'

							 WHEN 2 THEN 'BS'
							 WHEN 3 THEN 'BE'
							 END

UPDATE OINV SET U_IDC_FESTADO = @EstadoDoc, U_IDC_FERESP = '{1}' WHERE DocEntry = '{2}' AND ObjType = '{3}'

UPDATE ORIN SET U_IDC_FESTADO = @EstadoDoc, U_IDC_FERESP = '{1}' WHERE DocEntry = '{2}' AND ObjType = '{3}'

UPDATE ODPI SET U_IDC_FESTADO = @EstadoDoc, U_IDC_FERESP = '{1}' WHERE DocEntry = '{2}' AND ObjType = '{3}'

UPDATE ODLN SET U_IDC_FESTADO = @EstadoDoc, U_IDC_FERESP = '{1}' WHERE DocEntry = '{2}' AND ObjType = '{3}'

UPDATE OWTR SET U_IDC_FESTADO = @EstadoDoc, U_IDC_FERESP = '{1}' WHERE DocEntry = '{2}' AND ObjType = '{3}'