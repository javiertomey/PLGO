Insert into PLGO_OWN."AspNetRoles"
   ("Id", "Name")
 Values
   ('1', 'Administrador');
Insert into PLGO_OWN."AspNetRoles"
   ("Id", "Name")
 Values
   ('2', 'Validador');
Insert into PLGO_OWN."AspNetRoles"
   ("Id", "Name")
 Values
   ('3', 'Editor');
COMMIT;


Insert into PLGO_OWN.ESTADOS_SEGUIMIENTO
   (ESTADO_SEGUIMIENTO_ID, DESCRIPCION)
 Values
   (1, 'Sin iniciar');
Insert into PLGO_OWN.ESTADOS_SEGUIMIENTO
   (ESTADO_SEGUIMIENTO_ID, DESCRIPCION)
 Values
   (2, 'Iniciado');
Insert into PLGO_OWN.ESTADOS_SEGUIMIENTO
   (ESTADO_SEGUIMIENTO_ID, DESCRIPCION)
 Values
   (3, 'Terminado');
COMMIT;


Insert into PLGO_OWN.ESTADOS_VALIDACION
   (ESTADO_VALIDACION_ID, DESCRIPCION)
 Values
   (1, 'Pendiente validar');
Insert into PLGO_OWN.ESTADOS_VALIDACION
   (ESTADO_VALIDACION_ID, DESCRIPCION)
 Values
   (2, 'Validado');
COMMIT;


Insert into PLGO_OWN.TIPO_CAMBIO_CONTENIDO
   (TIPO_CAMBIO_ID, TIPO_CAMBIO)
 Values
   (1, 'Alta');
Insert into PLGO_OWN.TIPO_CAMBIO_CONTENIDO
   (TIPO_CAMBIO_ID, TIPO_CAMBIO)
 Values
   (2, 'Modificación');
Insert into PLGO_OWN.TIPO_CAMBIO_CONTENIDO
   (TIPO_CAMBIO_ID, TIPO_CAMBIO)
 Values
   (3, 'Eliminado');
Insert into PLGO_OWN.TIPO_CAMBIO_CONTENIDO
   (TIPO_CAMBIO_ID, TIPO_CAMBIO)
 Values
   (0, 'Sin cambios');
COMMIT;

