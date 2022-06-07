# INTEGRADOR DE FACTURACIÓN ELECTRÓNICA
Esta solución reemplaza el integrador de facturación electrónica anterior para Winners, con el proveedor Close2u con sus sistema TeFactura.pe.
El proyecto de facturación electrónica trabaja con el API «TeFacturo.pe» de Close2u.

## Objetos de base de datos
 
La solución reutiliza procedimientos almacenados y campos de usuario del integrador anterior.

Función creada para enviar la información según los catálogos de Close2u:
- **PLUS_FE_FN_CLOSE2U_CATALOGO**

Procedimientos almacenados reusado del integrador anterior y modificados levemente:
- **PLUS_FE_SP_LISTA_DOCUMENTO**
- **PLUS_FE_SP_LISTA_LINEA**
- **PLUS_FE_SP_LISTA_ANTICIPO**
- **PLUS_FE_SP_LISTA_CUOTA**
- **PLUS_FE_SP_LISTA_DOCUMENTOS_SIN_PDF**
- **PLUS_FE_SP_LISTA_DOCUMENTOS_POR_RESPONDER**
- **PLUS_FE_SP_DATOS_EMISOR**
- **PLUS_FE_SP_DATOS_GUIA_REMISION**
- **PLUS_FE_SP_COMPROBANTE_COMUNICACION_BAJA**

Procedimientos almacenados adecuados para enviar la información según los catálogos de Close2u:
- **PLUS_FE_SP_CLOSE2U_DOCUMENTO**
- **PLUS_FE_SP_CLOSE2U_LINEA**
- **PLUS_FE_SP_CLOSE2U_GUIA_REMISION**
- **PLUS_FE_SP_LISTA_GUIA_REMISION_REFERENCIA**

Campos de usuarios reusados para actualizar el estado envío y respuesta SUNAT:
- **U_IDC_FESTADO** (Código de estado SUNAT)
- **U_IDC_FERESP**  (Respuesta de estado SUNAT)

Valores de **U_IDC_FESTADO**:

  - **IN** - INTERNO
  - **PE** - DOC. PENDIENTE
  - **DR** - DOC. RECHAZADO
  - **DA** - DOC. APROBADO
  - **DE** - DOC. CON ERROR
  - **DC** - DOC. CORREGIDO
  - **DS** - DOC. EN SEGUIMIENTO
  - **BP** - COM. BAJA PENDIENTE
  - **BS** - COM. BAJA EN SEGUIMIENTO
  - **BE** - COM. BAJA CON ERROR
  - **BA** - COM. BAJA APROBADO
  - **BR** - COM. BAJA RECHAZADO
  - **RP** - RESUMEN PENDIENTE
  - **RS** - RESUMEN EN SEGUIMIENTO
  - **RE** - RESUMEN CON ERROR
  - **RA** - RESUMEN APROBADO
  - **RR** - RESUMEN RECHAZADO

Campos creados para actualizar el estado de PDF:
- **U_PLUS_ESTADOPDF** (Código de estado)
- **U_PLUS_RESPDF**  (Respuesta de estado)

Valores de U_PLUS_ESTADOPDF:
  - **PN** - PDF NO DESCARGAR
  - **PP** - PDF PENDIENTE
  - **PD** - PDF DESCARGADO
  - **PE** - PDF ERROR AL DESCARGAR

Sentencias usadas para actualizar los campos de estado:
- **PLUS_FE_UPDATE_ESTADO_SUNAT** (Actualiza con la respuesta con de SUNAT)
- **PLUS_FE_UPDATE_ESTADO_INTEGRADOR** (Actualiza con la respuesta del integrador)
- **PLUS_FE_UPDATE_ESTADO_PDF** (Actualiza el estado de descarga del PDF)


## Solución en visual studio

La solución es de tipo consola, en pruebas, en producción se cambia a tipo aplicación. Esta está dividida en 5 proyectos que tienen las siguientes funciones:

- **Facturacion.Common**: Contiene clases y método comunes reutilizados en los demás proyectos.
- **Facturacion.Model**: Contiene los modelos/estructuras de las solicitudes y respuestas del API.
- **Facturacion.Repository**: Contiene las clases y métodos para conectarse y llamar a los objetos de base de datos, asimismo para los servicios del API.
- **Facturacion.Business**: Se encarga de cargar los datos recuperados de la base de datos a los modelos de solicitud y respuesta.
- **Facturacion.App**: Inicia todo el proceso de recuperación de información y envío a los diferentes servicios de TeFacturo.

### App.config

En el archivo se encuentran 3 valores definidos de forma predeterminada:

```xml
<add key="Settings.MSSQLConnection" value="Data Source=srvpedc002;Initial Catalog=SBO_WINPERU_PRD;User ID=sa;password=Dire1691" />
<add key="Settings.PathForPDF" value="E:\PLUS-Close2u\PDF" />
<add key="Settings.ActivePathForPDF" value="true" />
```

--En el archivo App.Config se configura tanto la cadena de conexión como la ruta para guardar los archivos PDF, si la ruta para el PDF es «false» se tomará la ruta interna del proyecto. 

Para la ejecución automática del integrador, se optó por usar Task Scheduler, para ello es importante crear una nueva tarea, programar el intervalo de cada 5 minutos y asignar la ruta del ejecutable.

**¿Cómo recupero la ruta de mi ejecutable?**

Como el instalador es generado desde la publicación de archivos de Visual Studio, al instalar, no se podrá obtener la ruta del ejecutable directamente, para ello debe ejecutar el programa con el acceso directo, luego desde el administrador de tareas dar clic derecho y seleccionar la opción de abrir ubicación de archivo, con la ruta recuperada solo quedaría pegarla en la tarea, pero es necesario poner la ruta entre comillas o no lo leerá.

**Configuración en Task Scheduler**

Al ingresar a la programación de tareas, seleccionamos la primera carpeta (Biblioteca de programa de tareas), en la parte derecha veremos opciones, donde eligiremos **«Nueva Carpeta»**, posteriormente seleccionaremos ese folder y en el lado derecho buscaremos la opción **«Nueva tarea»**.
Cuando creamos una nueva tarea aparecere una ventana con diferentes pestañas, solo configuraremos las 3 primeras.

La pestaña general muestra la información de con la que creamos la nueva tarea, asignaremos el nombre.

![Verifique la ruta][img1]

La pestaña desencadenadores nos permite configurar el horario en el que se ejecutará nuestra aplicación.

![Verifique la ruta][img2]

Finalmente, la pestaña acciones es donde indicamos la ruta de nuestro ejecutable. Por ello es importante leer arriba para saber como encontrar la ruta luego de instalar la aplicación.

![Verifique la ruta][img2]

[img1]: Capturas/Task_General.png "Pestaña General"
[img2]: Capturas/Task_Horario.png "Pestaña Desencadenadores"
[img3]: Capturas/Task_Ejecutable.png "Acciones"

Más sobre tareas programadas: [Task Schedule]

[Task Schedule]: https://www.c-sharpcorner.com/article/setup-task-scheduler-to-run-application/

## Servicios de TeFacturo.pe y casos de Winners Sport

### Servicios de TeFacturo:
- Factura
- Boleta
- Nota de crédito
- Guia de remisión
- Consultar PDF
- Resumen de baja
- Consulta de estados

### Casos de Winners:

##### Facturas
- Venta interna nacional - IGV
- Venta interna nacional - Gravadas - Descuento por item
- Venta interna nacional - Gravadas - Descuento global
- Venta interna nacional - Gravadas - Recargo Global (Flete)
- Venta interna nacional - Inafecta - Inafecta
- Venta interna nacional - Exonerada - Exonerada
- Venta interna nacional - Gratuitas gravadas - IGV (Gravada por retiro)
- Venta de exportacion - Exportación - Exportación
- Venta de exportacion - Exportación - Otros cargos (Flete)
- Venta de exportacion - Exportación - Otras monedas
- Venta con anticipos - Exonerada (venta nacional)
- Venta con anticipos - Exportación
- Venta interna nacional - Contingencia

##### Boletas
- Venta interna nacional - IGV
- Venta interna nacional - Gravadas - Descuento por item
- Venta interna nacional - Gravadas - Descuento global
- Venta interna nacional - Gravadas - Recargo por item (Flete)
- Venta interna nacional - Gravadas - Monto menor a S/ 700
- Venta interna nacional - Gratuitas gravadas - IGV
- Venta interna nacional - Gratuitas inafecta - Inafecta
- Venta interna nacional - Contingencia

##### Notas de crédito
- Venta interna nacional - Gravadas - IGV
- Venta interna nacional - Gravadas - Una sola referencia
- Venta interna nacional - Inafecta - Inafecta
- Venta interna nacional - Exoneradas - Exoneradas
- Venta de exportacion - Exportacion - Exportacion

##### Notas de débito
- No realizan


##### Guias de remisión
- Guia de remisión - Venta
- Guia de remisión - Traslado
