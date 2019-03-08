<%-- 
Autor: Autor: Javier Tomey. Gobierno de Aragón

Éste programa es software libre: usted tiene derecho a redistribuirlo y/o
modificarlo bajo los términos de la Licencia EUPL European Public License 
publicada por el organismo IDABC de la Comisión Europea, en su versión 1.2.
o posteriores.

Éste programa se distribuye de buena fe, pero SIN NINGUNA GARANTÍA, incluso sin 
las presuntas garantías implícitas de USABILIDAD o ADECUACIÓN A PROPÓSITO CONCRETO. 
Para mas información consulte la Licencia EUPL European Public License.

Usted recibe una copia de la Licencia EUPL European Public License 
junto con este programa, si por algún motivo no le es posible visualizarla, 
puede consultarla en la siguiente URL: https://eupl.eu/1.2/es/

You should have received a copy of the EUPL European Public 
License along with this program.  If not, see https://eupl.eu/1.2/en/

Vous devez avoir reçu une copie de la EUPL European Public
License avec ce programme. Si non, voir https://eupl.eu/1.2/fr/

Sie sollten eine Kopie der EUPL European Public License zusammen mit
diesem Programm. Wenn nicht, finden Sie da https://eupl.eu/1.2/de/
--%>
<%@ Page Title="Administrador" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdmPlan.aspx.cs" Inherits="AdminPlanA" EnableEventValidation="false" %>


<%@ Import Namespace="PLGOdata" %>
<%@ Import Namespace="PLGO" %>

<asp:Content runat="server" ContentPlaceHolderID="HeaderScripts">
    <link href="<%=ResolveClientUrl("../Content/bootstrap-datepicker.css")%>" rel="stylesheet" />
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="BottonScripts">
    <script src="../Scripts/utilidades.js"></script>
    <script src="../Scripts/loadingoverlay.min.js"></script>
    <script src="../Scripts/bootstrap-datepicker.js"></script>

    <%--<script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.20.1/locale/es.js"></script>--%>

    <script type="text/javascript" charset="utf-8">

        function IniciaDatePickers() {

            $.fn.datepicker.dates['es'] = {
                days: ["Domigno", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado"],
                daysShort: ["Dom", "Lun", "Mar", "Mie", "Jue", "Vie", "Sab"],
                daysMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"],
                months: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
                monthsShort: ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"],
                today: "Hoy",
                clear: "Borrar",
                format: "dd/mm/yyyy",
                titleFormat: "MM yyyy",
                weekStart: 1
            };

            $("input[id*='txtFechaInicioLegislatura']").datepicker({
                language: 'es'
            });

            $("input[id*='txtFechaFinLegislatura']").datepicker({
                language: 'es'
            });

        }
        function ResaltaCambios() {
            if ($('#ObjetivoEstrategicoPV').val() != $('#ObjetivoEstrategico').val()) {
                $('#ObjetivoEstrategicoPV').css("background-color", "#ffe97c");
            }
            else {
                $('#ObjetivoEstrategicoPV').css("background-color", "#fff");
            }
        }
        function limpiaFormulario() {
            $('#ObjetivoEstrategicoPV').val(" ");
            $('#ObjetivoEstrategico').val(" ");
            $('#txtObjComentarioValidador').val(" ");
            $("#lblObjTipoCambio").text('Alta');
            $("#lblObjFecha").text(hoy());
            $("#lblObjEstadoVal").text('');
            $("#lblObjPORCENTAJE_AVANCE_CALCULADO").text('');            
            $('#btnUpdate').hide();
            $('#btnOkObj').hide();
            $('#btnAdd').show();
            $('#btnDeshacerCambio').hide();

        }
        function limpiaFormularioLeg() {
            $('#txtLegislatura').val(" ");
            $('#chkLegActual').attr('checked', false);
            $('#txtFechaInicioLegislatura').val(" ");
            $('#txtFechaFinLegislatura').val(" ");
            $('#btnUpdateLeg').hide();
            IniciaDatePickers();
            $('#btnAddLeg').show();
        }
        function limpiaFormularioDep() {
            $('#txtDepartamento').val(" ");
            $('#txtOrdenDepartamento').val(" ");
            $('#chkVisibleDep').attr('checked', true);

            $('#btnUpdateDep').hide();
            $('#btnAddDep').show();
        }

        function ResaltaCambiosIns() {
            if ($('#txtInstrumentosActPV').val() != $('#txtInstrumentosAct').val()) {
                $('#txtInstrumentosActPV').css("background-color", "#ffe97c");
            }
            else {
                $('#txtInstrumentosActPV').css("background-color", "#fff");
            }
            if ($('#txtOrganoResponsablePV').val() != $('#txtOrganoResponsable').val()) {
                $('#txtOrganoResponsablePV').css("background-color", "#ffe97c");
            }
            else {
                $('#txtOrganoResponsablePV').css("background-color", "#fff");
            }
            if ($('#txtSeguimientoPV').val() != $('#txtSeguimiento').val()) {
                $('#txtSeguimientoPV').css("background-color", "#ffe97c");
            }
            else {
                $('#txtSeguimientoPV').css("background-color", "#fff");
            }
            if ($('#txtRecursosHumanosPV').val() != $('#txtRecursosHumanos').val()) {
                $('#txtRecursosHumanosPV').css("background-color", "#ffe97c");
            }
            else {
                $('#txtRecursosHumanosPV').css("background-color", "#fff");
            }
            if ($('#txtCosteEconomicoPV').val() != $('#txtCosteEconomico').val()) {
                $('#txtCosteEconomicoPV').css("background-color", "#ffe97c");
            }
            else {
                $('#txtCosteEconomicoPV').css("background-color", "#fff");
            }
            if ($('#txtMediosOtrosPV').val() != $('#txtMediosOtros').val()) {
                $('#txtMediosOtrosPV').css("background-color", "#ffe97c");
            }
            else {
                $('#txtMediosOtrosPV').css("background-color", "#fff");
            }
            if ($('#txtTemporalidadPV').val() != $('#txtTemporalidad').val()) {
                $('#txtTemporalidadPV').css("background-color", "#ffe97c");
            }
            else {
                $('#txtTemporalidadPV').css("background-color", "#fff");
            }
            if ($('#txtIndSeguimientoPV').val() != $('#txtIndSeguimiento').val()) {
                $('#txtIndSeguimientoPV').css("background-color", "#ffe97c");
            }
            else {
                $('#txtIndSeguimientoPV').css("background-color", "#fff");
            }
            if ($('#ddlPorcentajeAvancePV').val() != $('#ddlPorcentajeAvance').val()) {
                $('#ddlPorcentajeAvancePV').css("background-color", "#ffe97c");
            }
            else {
                $('#ddlPorcentajeAvancePV').css("background-color", "#fff");
            }
        }

        function limpiaFormularioAcc() {
            $('#txtInstrumentosAct').val(" ");
            $('#txtOrganoResponsable').val(" ");
            $('#txtSeguimiento').val(" ");
            $('#txtRecursosHumanos').val(" ");
            $('#txtCosteEconomico').val(" ");
            $('#txtMediosOtros').val(" ");
            $('#txtTemporalidad').val(" ");
            $('#txtIndSeguimiento').val(" ");            
            $('#ddlPorcentajeAvance').val("-1");
            $('#txtAccComentarioValidador').val(" ");
            $('#txtInstrumentosActPV').val(" ");
            $('#txtOrganoResponsablePV').val(" ");
            $('#txtSeguimientoPV').val(" ");
            $('#txtRecursosHumanosPV').val(" ");
            $('#txtCosteEconomicoPV').val(" ");
            $('#txtMediosOtrosPV').val(" ");
            $('#txtTemporalidadPV').val(" ");
            $('#txtIndSeguimientoPV').val(" ");
            $('#ddlPorcentajeAvancePV').val("-1");
            $('#txtAccComentarioValidadorPV').val(" ");
            $('#txtAccObservaciones').val(" ");

            $("#lblAccTipoCambio").text('Alta');
            $("#lblAccFecha").text(hoy());
            $("#lblAccEstadoVal").text('');

            $('#btnGuardarAcc').hide();
            $('#btnOkAcc').hide();
            $('#btnAddAcc').show();
            $('#btnDeshacerCambioAcc').hide();
        }


        function GetData(serviceName, params) {
            $.ajax({
                type: "POST",
                url: "json_data_adminpln?op=" + serviceName + params,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: successSubTipo,
                failure: errorCB
            });
        }

        function errorCB(result) {
            alert(result.status + ' ' + result.statusText);
        }


        function cargaDepartamentos(iLegislaturaId) {
            $("#departamentos").LoadingOverlay("show");
            $.get("json_data_admpln.aspx?op=loadD&id=" + iLegislaturaId + "&t=" + new Date().getTime(), function (newDataArray) {
                $("#departamentos").DataTable().clear();
                $("#departamentos").DataTable().rows.add(newDataArray);
                $("#departamentos").DataTable().draw();
                $("#departamentos").LoadingOverlay("hide", true);
            });           

            $("#objetivos").dataTable().fnClearTable();
            $("#acciones").dataTable().fnClearTable();
        }

        function cargaObjetivos(iDepartamentoId) {

            $("#objetivos").LoadingOverlay("show");
            $.get("json_data_admpln.aspx?op=loadO&id=" + iDepartamentoId + "&t=" + new Date().getTime(), function (newDataArray) {
                $("#objetivos").DataTable().clear();
                $("#objetivos").DataTable().rows.add(newDataArray);
                $("#objetivos").DataTable().draw();
                $("#objetivos").LoadingOverlay("hide", true);
            });
        }

        function cargaAcciones(iObjetivoId, objetivo) {

            $("#acciones").LoadingOverlay("show");
            $.get("json_data_admpln.aspx?op=loadA&id=" + iObjetivoId + "&t=" + new Date().getTime(), function (newDataArray) {
                $("#acciones").DataTable().clear();
                $("#acciones").DataTable().rows.add(newDataArray);
                $("#acciones").DataTable().draw();
                $("#acciones").LoadingOverlay("hide", true);
            });

            $("#hdObjetivoIdEdit").val(iObjetivoId);
        }
        function EditaObjetivo(row) {
            var obj = JSON.parse(decodeURIComponent(row));
            $("#hdObjetivoIdEdit").val(obj['CONTENIDO_ID']);
            $('#ObjetivoEstrategicoPV').val(obj['OBJETIVO_ESTRATEGICO_PDTE_VAL']);
            $('#ObjetivoEstrategico').val(obj['OBJETIVO_ESTRATEGICO']);

            $('#txtObjComentarioValidador').val(obj['COMENTARIO_VALIDADOR']);
            if (obj['AUTOR_MODIFICACION'] === null) {
                $('#lblObjAutor').text(obj['AUTOR_CREACION']['NOMBRE'] + ' ' + obj['AUTOR_CREACION']['APELLIDOS']);
            } else {
                $('#lblObjAutor').text(obj['AUTOR_MODIFICACION']['NOMBRE'] + ' ' + obj['AUTOR_MODIFICACION']['APELLIDOS']);
            }
            if (obj['FECHA_MODIFICACION'] === null)
                var fecha = new Date(obj['FECHA_CREACION']);
            else
                var fecha = new Date(obj['FECHA_MODIFICACION']);
            $('#lblObjFecha').text(fecha.getDate() + '/' + (fecha.getMonth() + 1) + '/' + fecha.getFullYear());

            $('#lblObjTipoCambio').text(obj['TIPO_CAMBIO_CONTENIDO']['TIPO_CAMBIO']);
            $('#lblObjEstadoVal').text(obj['ESTADOS_VALIDACION']['DESCRIPCION']);
            if (obj['PORCENTAJE_AVANCE_CALCULADO'] === null)
                $('#lblObjPORCENTAJE_AVANCE_CALCULADO').text('0 %');
            else
                $('#lblObjPORCENTAJE_AVANCE_CALCULADO').text(obj['PORCENTAJE_AVANCE_CALCULADO'] + ' %');
            $('#btnVerObj').attr('onclick', "window.open('../View/DetalleDepartamento?id=" + obj['DEPARTAMENTO_ID'] + "&oId=" + obj['CONTENIDO_ID'] + "', '_blank', 'location=yes,height=640,width=1037,scrollbars=yes,status=yes')");

            $('#btnAdd').hide();
            $('#btnOkObj').show();

            ResaltaCambios();
            if (obj['TIPO_CAMBIO_CONTENIDO_ID'] != 0) {
                $('#btnDeshacerCambio').show();
            }
            else {
                $('#btnDeshacerCambio').hide();
            }

            $('#btnUpdate').show();

        }
        function EditaAccion(row) {
            var obj = JSON.parse(decodeURIComponent(row));
            $("#hdAccionIdEdit").val(obj['CONTENIDO_ID']);
            $('#txtInstrumentosActPV').val(obj['INSTRUMENTOS_ACT_PDTE_VAL']);
            $('#txtInstrumentosAct').val(obj['INSTRUMENTOS_ACT']);
            $('#txtOrganoResponsablePV').val(obj['ORGANO_RESPONSABLE_PDTE_VAL']);
            $('#txtOrganoResponsable').val(obj['ORGANO_RESPONSABLE']);
            $('#txtSeguimientoPV').val(obj['SEGUIMIENTO_PDTE_VAL']);
            $('#txtSeguimiento').val(obj['SEGUIMIENTO']);
            $('#txtRecursosHumanosPV').val(obj['RECURSOS_HUMANOS_PDTE_VAL']);
            $('#txtRecursosHumanos').val(obj['RECURSOS_HUMANOS']);
            $('#txtCosteEconomicoPV').val(obj['COSTE_ECONOMICO_PDTE_VAL']);
            $('#txtCosteEconomico').val(obj['COSTE_ECONOMICO']);
            $('#txtMediosOtrosPV').val(obj['MEDIOS_OTROS_PDTE_VAL']);
            $('#txtMediosOtros').val(obj['MEDIOS_OTROS']);
            $('#txtTemporalidadPV').val(obj['TEMPORALIDAD_PDTE_VAL']);
            $('#txtTemporalidad').val(obj['TEMPORALIDAD']);
            $('#txtIndSeguimientoPV').val(obj['INDICADOR_SEGUIMIENTO_PDTE_VAL']);
            $('#txtIndSeguimiento').val(obj['INDICADOR_SEGUIMIENTO']);
            $('#ddlPorcentajeAvancePV').val(obj['PORCENTAJE_AVANCE_PDTE_VAL']);
            $('#ddlPorcentajeAvance').val(obj['PORCENTAJE_AVANCE']);
            $('#txtAccComentarioValidador').val(obj['COMENTARIO_VALIDADOR']);
            $('#txtAccObservaciones').val(obj['OBSERVACIONES']);
            if (obj['AUTOR_MODIFICACION'] === null) {
                $('#lblAccAutor').text(obj['AUTOR_CREACION']['NOMBRE'] + ' ' + obj['AUTOR_CREACION']['APELLIDOS']);
            } else {
                $('#lblAccAutor').text(obj['AUTOR_MODIFICACION']['NOMBRE'] + ' ' + obj['AUTOR_MODIFICACION']['APELLIDOS']);
            }
            if (obj['FECHA_MODIFICACION'] === null)
                var fecha = new Date(obj['FECHA_CREACION']);
            else
                var fecha = new Date(obj['FECHA_MODIFICACION']);
            $('#lblAccFecha').text(fecha.getDate() + '/' + (fecha.getMonth() + 1) + '/' + fecha.getFullYear());

            $('#lblAccTipoCambio').text(obj['TIPO_CAMBIO_CONTENIDO']['TIPO_CAMBIO']);
            $('#lblAccEstadoVal').text(obj['ESTADOS_VALIDACION']['DESCRIPCION']);

            $('#btnAddAcc').hide();
            $('#btnGuardarAcc').show();
            $('#btnOkAcc').show();

            ResaltaCambiosIns();
            if (obj['TIPO_CAMBIO_CONTENIDO_ID'] != 0) {
                $('#btnDeshacerCambioAcc').show();
            }
            else {
                $('#btnDeshacerCambioAcc').hide();
            }
        }
        function EditaLegislatura(row) {
            var obj = JSON.parse(decodeURIComponent(row));
            $("#hdLegislaturaId").val(obj['LEGISLATURA_ID']);
            $('#txtLegislatura').val(obj['DESCRIPCION']);
            $('#chkLegActual').attr('checked', obj['ACTUAL']);
            if (obj['FECHA_INICIO'] === null)
                var fecha = new Date(obj['FECHA_INICIO']);
            else
                var fecha = new Date(obj['FECHA_INICIO']);
            $('#txtFechaInicioLegislatura').val(fecha.getDate() + '/' + (fecha.getMonth() + 1) + '/' + fecha.getFullYear());
            if (obj['FECHA_FIN'] === null)
                var fecha = new Date(obj['FECHA_FIN']);
            else
                var fecha = new Date(obj['FECHA_FIN']);
            $('#txtFechaFinLegislatura').val(fecha.getDate() + '/' + (fecha.getMonth() + 1) + '/' + fecha.getFullYear());
            $('#btnAddLeg').hide();
            $('#btnUpdateLeg').show();
            $('#btnVerLeg').attr('onclick', "window.open('../View/VerDepartamentos.aspx', '_blank', 'location=yes,height=640,width=1037,scrollbars=yes,status=yes')");
            IniciaDatePickers();
        }
        function EditaDepartamento(row) {
            var obj = JSON.parse(decodeURIComponent(row));
            $("#hdLegislaturaId").val(obj['LEGISLATURA_ID']);
            $("#hdDepartamentoId").val(obj['DEPARTAMENTO_ID']);
            $('#txtDepartamento').val(obj['DESCRIPCION']);
            $('#txtOrdenDepartamento').val(obj['ORDEN']);
            $('#chkVisibleDep').attr('checked', obj['VISIBLE']);
            $('#btnAddDep').hide();
            $('#btnUpdateDep').show();
            $('#btnVerDep').attr('onclick', "window.open('../View/DetalleDepartamento?id=" + obj['DEPARTAMENTO_ID'] + "', '_blank', 'location=yes,height=640,width=1037,scrollbars=yes,status=yes')");
        }
        function sinObjSeleccionado() {
            $("#acciones").dataTable().fnClearTable();           
        }
        function conObjSeleccionado() {

        }
        function sinDepSeleccionado() {          
            $("#objetivos").dataTable().fnClearTable();          
            $("#acciones").dataTable().fnClearTable();

        }
        function conDepSeleccionado() {

        }


        $(document).ready(function () {            

            $('[data-toggle="tooltip"]').tooltip();

            $("#legislaturas").LoadingOverlay("show");
            var tableLeg = $('#legislaturas').DataTable({
                select: {
                    style: 'single'
                },
                order: [[0, "desc"]],
                "iDisplayLength": 2,
                "lengthMenu": [[2, 5, 10, 25, 50, -1], [2, 5, 10, 25, 50, "Todos"]],
                language: {
                    url: '../Content/DataTables/locale/es.json',
                    select: {
                        rows: {
                            _: "",
                            0: "<br/> Seleccione una legislatura para ver sus departamentos asociados",
                            1: ""
                        }
                    }
                },
                "ajax": { "url": "json_data_admpln.aspx?op=loadL" + "&t=" + new Date().getTime(), "dataSrc": "", "cache": false, },
                "columns": [
                    {
                        "data": null,
                        "className": "dt-center",
                        "bSortable": false,
                        mRender: function (data, type, row) {
                            if (data.ACTUAL == 1)
                                return '<a data-toggle="tooltip" title="Legislatura vigente"><img  src="../img/iconmonstr-check-mark-13-16.png" width="16px" /></a>';
                            else
                                return '<a data-toggle="tooltip" title="Legislatura no vigente"><img  src="../img/iconmonstr-checkbox-6-16.png" width="16px" /></a>';
                        }
                    },
           { "data": "DESCRIPCION" },
           {
               "data": null,
               "bSortable": false,
               mRender: function (data, type, row) {
                   return '<button type="button" title="Exporta en un archivo excel el informe del Plan de Gobierno de la Legislatura actual" class="btn" onclick=" __doPostBack(&quot;<%= btnImprimirInformeExcelLeg.UniqueID%>&quot;, &quot;' + data.LEGISLATURA_ID + '&quot;);" value="expExcel" formnovalidate=""><img src="../img/doc_16.png">&nbsp;Informe</button>&nbsp;<button type="button" class="btn" data-toggle="modal" data-target="#myModalLeg" onclick="EditaLegislatura(&quot;' + encodeURIComponent(JSON.stringify(row)) + '&quot;);" value="edLeg" formnovalidate><img src="../img/edit_16.png">&nbspEditar</button>';
               }
           },
                ],
            });

            tableLeg
           .on('select', function (e, dt, type, indexes) {
               var rowData = tableLeg.rows(indexes).data().toArray();
               $("#hdLegislaturaId").val(rowData[0]['LEGISLATURA_ID']);
               cargaDepartamentos(rowData[0]['LEGISLATURA_ID']);
           })
             .on('init.dt', function () {
                 tableLeg.row(0).select();
             })
            .on('deselect', function (e, dt, type, indexes) {
                if (tableLeg.rows('.selected').any()) {
                    var rowData = tableLeg.rows(0).data().toArray();
                    $("#hdLegislaturaId").val(rowData[0]['LEGISLATURA_ID']);
                    tableLeg.row(0).select();
                }
            })
            .on('xhr', function () {
                $("#legislaturas").LoadingOverlay("hide", true);
            })
            ;



            var tableAcc = $('#acciones').DataTable({
                order: [[1, "asc"], [2, "asc"]],
                language: {
                    url: '../Content/DataTables/locale/esAcciones.json',
                    select: {
                        rows: {
                            _: "",
                            0: "",
                            1: ""
                        }
                    }
                },                
                "columns": [
                     {
                         "data": null,
                         "className": "dt-center",
                         "bSortable": false,
                         mRender: function (data, type, row) {
                             if (data.TIPO_CAMBIO_CONTENIDO_ID == '<%=TIPO_CAMBIO_CONTENIDO.ALTA%>')
                                 return '<a data-toggle="tooltip" title="Nuevo contenido"><img  src="../img/plus_16.png" width="16px" /></a>';
                             if (data.TIPO_CAMBIO_CONTENIDO_ID == '<%=TIPO_CAMBIO_CONTENIDO.MODIFICACION%>')
                                 return '<a data-toggle="tooltip" title="Modificación"><img  src="../img/edit_16.png" width="16px" /></a>';
                             if (data.TIPO_CAMBIO_CONTENIDO_ID == '<%=TIPO_CAMBIO_CONTENIDO.ELIMINADO%>')
                                 return '<a data-toggle="tooltip" title="Pdte. eliminación"><img  src="../img/eliminar_16.png" width="16px" /></a>';
                             if (data.TIPO_CAMBIO_CONTENIDO_ID == '<%=TIPO_CAMBIO_CONTENIDO.SIN_CAMBIOS%>')
                                 return '';
                         }
                     },
                    {
                        "data": null,
                        "className": "dt-center",
                        "bSortable": false,
                        mRender: function (data, type, row) {
                            if (data.ESTADO_VALIDACION_ID == '<%=ESTADOS_VALIDACION.PDTE_VALIDAR%>')
                                return '<a data-toggle="tooltip" title="No validado ' + data.COMENTARIO_VALIDADOR + '" onclick="return alert(&quot;No validado ' + data.COMENTARIO_VALIDADOR + '&quot;)"><img  src="../img/iconmonstr-x-mark-1-16.png" width="16px" /></a>';
                            else if (data.ESTADO_VALIDACION_ID == '<%=ESTADOS_VALIDACION.VALIDADO%>')
                                return '<a data-toggle="tooltip" title="Validado"> <img  src="../img/iconmonstr-check-mark-11-16.png" width="16px" /></a>';


                        }
                    },
                     {
                         "data": null,
                         "bSortable": true,
                         mRender: function (data, type, row) {
                             if (data.INSTRUMENTOS_ACT == null)
                                 return data.INSTRUMENTOS_ACT_PDTE_VAL;
                             else
                                 return data.INSTRUMENTOS_ACT;

                         }
                     },

                     {
                         "data": null,
                         "bSortable": false,
                         mRender: function (data, type, row) {
                             return '<button type="button" class="btn" data-toggle="modal" data-target="#myModalAcc" onclick="EditaAccion(&quot;' + encodeURIComponent(JSON.stringify(row)) + '&quot;);" value="edObj" formnovalidate><img src="../img/edit_16.png">&nbspEditar</button>&nbsp; <button type="button" class="btn" onclick="if (confirm(&quot;¿Desea eliminar el instrumento/actividad?&quot;)) __doPostBack(&quot;<%= btnEliminaAcc.UniqueID%>&quot;, &quot;' + data.CONTENIDO_ID + '&quot;);"  href="#" formnovalidate><img src="../img/eliminar_16.png">&nbsp Eliminar</button>';
                         }
                     }
                ]
            });



            var tableObj = $('#objetivos').DataTable({
                clear: true,
                destroy: true,
                order: [[3, "desc"], [2, "asc"]],
                "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "Todos"]],
                select: {
                    style: 'single'
                },
                language: {
                    url: '../Content/DataTables/locale/esObjetivos.json',
                    select: {
                        rows: {
                            _: "",
                            0: "",
                            1: ""
                        }
                    }
                },
                "rowId": 'CONTENIDO_ID',
                "columns": [
                    {
                        "data": null,
                        "className": "dt-center",
                        "bSortable": false,
                        mRender: function (data, type, row) {
                            if (data.TIPO_CAMBIO_CONTENIDO_ID == '<%=TIPO_CAMBIO_CONTENIDO.ALTA%>')
                                         return '<a data-toggle="tooltip" title="Nuevo contenido"><img  src="../img/plus_16.png" width="16px" /></a>';
                                     if (data.TIPO_CAMBIO_CONTENIDO_ID == '<%=TIPO_CAMBIO_CONTENIDO.MODIFICACION%>')
                                         return '<a data-toggle="tooltip" title="Modificación"><img  src="../img/edit_16.png" width="16px" /></a>';
                                     if (data.TIPO_CAMBIO_CONTENIDO_ID == '<%=TIPO_CAMBIO_CONTENIDO.ELIMINADO%>')
                                         return '<a data-toggle="tooltip" title="Pdte. eliminación"><img  src="../img/eliminar_16.png" width="16px" /></a>';
                                     if (data.TIPO_CAMBIO_CONTENIDO_ID == '<%=TIPO_CAMBIO_CONTENIDO.SIN_CAMBIOS%>')
                                         return '';
                                     else
                                         return '';
                                 }
                             },
                    {
                        "data": null,
                        "className": "dt-center",
                        "bSortable": false,
                        mRender: function (data, type, row) {
                            if (data.ESTADO_VALIDACION_ID == '<%=ESTADOS_VALIDACION.PDTE_VALIDAR%>')
                                return '<a data-toggle="tooltip" title="No validado ' + data.COMENTARIO_VALIDADOR + '" onclick="return alert(&quot;No validado ' + data.COMENTARIO_VALIDADOR + '&quot;)"><img  src="../img/iconmonstr-x-mark-1-16.png" width="16px" /></a>';
                            else if (data.ESTADO_VALIDACION_ID == '<%=ESTADOS_VALIDACION.VALIDADO%>')
                                return '<a data-toggle="tooltip" title="Validado"> <img  src="../img/iconmonstr-check-mark-11-16.png" width="16px" /></a>';
                            else
                                return ' ';

                        }
                    },
                     {
                         "data": null,
                         "bSortable": true,
                         mRender: function (data, type, row) {
                             if (data.OBJETIVO_ESTRATEGICO == null)
                                 return data.OBJETIVO_ESTRATEGICO_PDTE_VAL;
                             else
                                 return data.OBJETIVO_ESTRATEGICO;

                         }
                     },
                     {
                         "data": "ValidacionesPendientes",
                         "className": "dt-center",
                     },
                     {
                         "data": "PORCENTAJE_AVANCE_CALCULADO",
                         "className": "dt-center",
                     },
                     {
                        "data": null,
                        "bSortable": false,
                        mRender: function (data, type, row) {
                            return '<button type="button" class="btn" data-toggle="modal" data-target="#myModalObj" onclick="EditaObjetivo(&quot;' + encodeURIComponent(JSON.stringify(row)) + '&quot;);" value="edObj" formnovalidate><img src="../img/edit_16.png">&nbspEditar</button>&nbsp;<button type="button" class="btn" onclick="if (confirm(&quot;¿Desea eliminar el Objetivo?&quot;)) __doPostBack(&quot;<%= btnEliminaObj.UniqueID%>&quot;, &quot;' + data.CONTENIDO_ID + '&quot;);"  href="#" formnovalidate><img src="../img/eliminar_16.png">&nbsp Eliminar</button>';
                        }
                    },
                         ],
                         "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {

                             return nRow;
                         },
                     });


            tableObj
              .on('select', function (e, dt, type, indexes) {
                  var rowData = tableObj.rows(indexes).data().toArray();
                  if (rowData.length != 0) {
                      conObjSeleccionado();
                      $("#hdObjetivoIdEdit").val(rowData[0]['CONTENIDO_ID']);
                      /*if (rowData[0]['OBJETIVO_ESTRATEGICO'] === null)
                          $("#AccionEdit span").text(rowData[0]['OBJETIVO_ESTRATEGICO_PDTE_VAL']);
                      else
                          $("#AccionEdit span").text(rowData[0]['OBJETIVO_ESTRATEGICO']);*/
                      cargaAcciones(rowData[0]['CONTENIDO_ID'], rowData[0]['OBJETIVO_ESTRATEGICO_PDTE_VAL']);
                      if (window.localStorage) {
                          localStorage['selobjid'] = rowData[0]['CONTENIDO_ID'];
                      }
                  }
                  else {
                      sinObjSeleccionado();
                      if (window.localStorage) {
                          localStorage['selobjid'] = '';
                      }
                  }
              })
              .on('deselect', function (e, dt, type, indexes) {
                  sinObjSeleccionado();
                  if (window.localStorage) {
                      localStorage['selobjid'] = '';
                  }
              })
                .on('init.dt', function () {
                    if (window.localStorage) {
                        if (localStorage['selobjid'] === undefined || localStorage['selobjid'] === null || localStorage['selobjid'] === '')
                            tableObj.row(0).select();
                        else {                            
                            tableObj.row('#' + localStorage['selobjid']).select();
                            conObjSeleccionado();
                            $("#hdObjetivoIdEdit").val(localStorage['selobjid']);
                            var rowData = tableDep.rows($("#hdDepartamentoId").val).data().toArray();
                            if (rowData['OBJETIVO_ESTRATEGICO'] === null)
                                $("#AccionEdit span").text(rowData['OBJETIVO_ESTRATEGICO_PDTE_VAL']);
                            else
                                $("#AccionEdit span").text(rowData['OBJETIVO_ESTRATEGICO']);
                            cargaAcciones(localStorage['selobjid'], '');
                        }
                    }
                })
             .on('draw.dt', function () {                 
                 if (tableObj.data().any()) {
                     $("#objetivos").LoadingOverlay("hide", true);
                     if (window.localStorage) {
                         if (!(localStorage['selobjid'] === undefined || localStorage['selobjid'] === null || localStorage['selobjid'] === '')) {                             
                             tableObj.row('#' + localStorage['selobjid']).select();
                             conObjSeleccionado();
                             $("#hdObjetivoIdEdit").val(localStorage['selobjid']);
                             var rowData = tableDep.rows($("#hdDepartamentoId").val).data().toArray();
                             if (rowData['OBJETIVO_ESTRATEGICO'] === null)
                                 $("#AccionEdit span").text(rowData['OBJETIVO_ESTRATEGICO_PDTE_VAL']);
                             else
                                 $("#AccionEdit span").text(rowData['OBJETIVO_ESTRATEGICO']);
                         }
                     }
                 }

             });;


            tableDep = $('#departamentos').DataTable({
                clear: true,
                destroy: true,
                select: {
                    style: 'single'
                },
                order: [[1, "desc"]],
                language: {
                    url: '../Content/DataTables/locale/es.json',
                    select: {
                        rows: {
                            _: "",
                            0: "",
                            1: ""
                        }
                    }

                },
                "rowId": 'DEPARTAMENTO_ID',
                "columns": [
                     { "data": "DESCRIPCION" },
                     { "data": "ValidacionesPendientes", "className": "dt-center" },
                     { "data": "PORCENTAJE_AVANCE_CALCULADO", "className": "dt-center" },
                     {
                         "data": null,
                         "bSortable": false,
                         mRender: function (data, type, row) {
                             return '<button type="button" title="añade o elimina usuarios asociados para la edición del Departamento" class="btn" onclick="window.open(\'AdmUsuariosDep.aspx?id=' + data.DEPARTAMENTO_ID + '\', \'_self\')" value="edObj" formnovalidate><img src="../img/user_16.png">&nbspVer usuarios</button>&nbsp;<button type="button" title="Exporta en un archivo excel el informe del Plan de Gobierno del Departamento actual" class="btn" onclick=" __doPostBack(&quot;<%= btnImprimirInformeExcel.UniqueID%>&quot;, &quot;' + data.DEPARTAMENTO_ID + '&quot;);" value="expExcel" formnovalidate=""><img src="../img/doc_16.png">&nbsp;Informe</button>&nbsp;<button type="button" class="btn" data-toggle="modal" data-target="#myModalDep" onclick="EditaDepartamento(&quot;' + encodeURIComponent(JSON.stringify(row)) + '&quot;);" value="edObj" formnovalidate><img src="../img/edit_16.png">&nbspEditar</button>&nbsp;<button type="button" class="btn" onclick="if (confirm(&quot;¿Desea eliminar el departamento (se eliminarán sus objetivos e instrumentos asociados)?&quot;)) __doPostBack(&quot;<%= btnEliminaDep.UniqueID%>&quot;, &quot;' + data.DEPARTAMENTO_ID + '&quot;);"  href="#" formnovalidate><img src="../img/eliminar_16.png">&nbsp Eliminar</button>';
                                          }
                                      }
                                 ]
                             });

            tableDep
            .on('select', function (e, dt, type, indexes) {
                var rowData = tableDep.rows(indexes).data().toArray();
                if (rowData.length != 0) {
                    conDepSeleccionado();
                    $("#hdDepartamentoId").val(rowData[0]['DEPARTAMENTO_ID']);
                    $("#ObjetivoEdit span").text(rowData[0]['DESCRIPCION']);
                    cargaObjetivos(rowData[0]['DEPARTAMENTO_ID']);
                    if (window.localStorage) {
                        localStorage['seldepid'] = rowData[0]['DEPARTAMENTO_ID'];
                    }
                }
                else {
                    sinDepSeleccionado();
                    if (window.localStorage) {
                        localStorage['seldepid'] = '';
                        localStorage['selobjid'] = '';
                    }
                }
            })
                .on('init.dt', function () {
                    var json = tableDep.ajax.json();
                    if (json === undefined || json.length == 0) {
                        sinDepSeleccionado();                      
                    }
                    else {
                        if (window.localStorage) {
                            if (localStorage['seldepid'] === undefined || localStorage['seldepid'] === null || localStorage['seldepid'] === '')
                                tableDep.row(0).select();
                            else {
                                tableDep.row('#' + localStorage['seldepid']).select();
                                conDepSeleccionado();
                                $("#hdDepartamentoId").val(localStorage['seldepid']);
                            }
                        }
                        else {
                            tableDep.row(0).select();
                            if (window.localStorage) {
                                localStorage['seldepid'] = rowData[0]['DEPARTAMENTO_ID'];
                            }
                        }
                    }
                })
            .on('deselect', function (e, dt, type, indexes) {
                sinDepSeleccionado();
                if (window.localStorage) {
                    localStorage['seldepid'] = '';
                    localStorage['selobjid'] = '';
                }
            })
            .on('draw.dt', function () {
                
                if (tableDep.data().any()) {
                    $("#departamentos").LoadingOverlay("hide", true);
                    if (window.localStorage) {
                        if (!(localStorage['seldepid'] === undefined || localStorage['seldepid'] === null || localStorage['seldepid'] === '')) {
                            tableDep.row('#' + localStorage['seldepid']).select();
                            conDepSeleccionado();
                            $("#hdDepartamentoId").val(localStorage['seldepid']);
                            $("#ObjetivoEdit span").text(tableDep.row('#' + localStorage['seldepid']).data()['DESCRIPCION']);
                        }
                    }
                }
               
            });

            $("#txtOrdenDepartamento").keypress(function (e) {
                if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                    $("#errmsgorden").html("Sólo números").show().fadeOut("slow");
                    return false;
                }
            });
            $.getJSON("../View/json_data.aspx?op=dG", function (data) {
                $('#nObjetivos').text(data[0].objetivos);
                $('#nAcciones').text(data[0].acciones);
                $('#nUltimaMod').text(data[0].ultima_mod.substring(0, 10));
            });

            //setInterval(function () {
            //    if (tableObj.data().any()) {
            //        tableObj.ajax.reload(null, false);
            //        console.log("recargando");
            //    }
            //}, 300); //30000

            //setInterval(function () {
            //    if (tableAcc.data().any()) {
            //        tableAcc.ajax.reload(null, false);
            //    }
            //}, 300); //30000

        });
    </script>

</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <asp:Button ID="btnImprimirInformeExcelLeg" runat="server" Style="display: none" Text="Exportar informe" OnClick="btnImprimirInformeExcelLeg_Click" />
    <asp:Button ID="btnImprimirInformeExcel" runat="server" Style="display: none" Text="Exportar informe" OnClick="btnImprimirInformeExcel_Click" />

    <h2><%: WarningMessage %></h2>


    <div>
        <asp:PlaceHolder runat="server" ID="successMessage" Visible="false" ViewStateMode="Disabled">
            <p class="alert alert-success alert-dismissible">
                <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
                <%: SuccessMessage %>
            </p>
        </asp:PlaceHolder>


        <asp:PlaceHolder runat="server" ID="warningMessage" Visible="false" ViewStateMode="Disabled">
            <p class="alert alert-warning alert-dismissible">
                <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
                <%: WarningMessage %>
            </p>
        </asp:PlaceHolder>

        <asp:PlaceHolder runat="server" ID="errorMessage" Visible="false" ViewStateMode="Disabled">
            <p class="alert alert-danger alert-dismissible">
                <strong>Error:</strong>
                <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
                <%: ErrorMessage %>
            </p>
        </asp:PlaceHolder>
    </div>



    <div class="panel panel-default">

        <div>
            <div class="container">
                <br />


                <div>
                    <span class="small">Legislatura actual: <strong><span id="nObjetivos"></span></strong> objetivos estratégicos,  <strong><span id="nAcciones"></span></strong> instrumentos/actividades, última actualización: <strong><span id="nUltimaMod"></span></strong></span>.
                    <div class="pull-right">
                        <button type="button" title="Abre en una nueva ventana el gráfico de cuadro de mando" class="btn" onclick="window.open('../View/CuadroMando')" formnovalidate="">
                            <img src="../img/iconmonstr-chart-4-16.png">&nbsp;Cuadro de mando</button>&nbsp;
                        <button type="button" title="Abre en una nueva ventana el gráfico de evolución de los departamentos" class="btn" onclick="window.open('../View/EvolucionDepartamentos')" formnovalidate="">
                            <img src="../img/iconmonstr-chart-4-16.png">&nbsp;Evoluci&oacute;n por departamentos</button>&nbsp;
                    </div>
                </div>

                <h3>Legislaturas</h3>
                <br />

                <br />
                  <div class="pull-right" style="margin-bottom: 5px;">
                   <asp:DropDownList ID="ddlTipoInformeLeg" CssClass="form-control btn" runat="server" title="Seleccione el tipo de exportación de datos para generar el informe" ClientIDMode="Static" style="width: 90px;">                            
                            <asp:ListItem Value="PDF" Text="PDF"></asp:ListItem>
                            <asp:ListItem Value="XLSX" Text="Excel"></asp:ListItem>
                            <asp:ListItem Value="DOCX" Text="Word"></asp:ListItem>
                            <asp:ListItem Value="HTML" Text="Visor"></asp:ListItem>
                        </asp:DropDownList>                        
                         <button type="button" onclick=" __doPostBack(&quot;<%= btnExportarDatosLeg.UniqueID%>&quot);" title="Exporta la información según el formato seleccionado" class="btn" value="expDatos" formnovalidate=""><img src="../img/iconmonstr-magnifier-6-16.png">&nbsp;Descargar informe impreso</button>&nbsp;
                        <asp:Button ID="btnExportarDatosLeg" OnClientClick="aspnetForm.target ='_blank';"  runat="server"  style="display:none"  Text="Exportar datos" OnClick="btnExportarDatosLeg_Click"    />    
                 </div>

                <table id="legislaturas" class="display" cellspacing="0" width="100%">
                    <thead>
                        <tr>
                            <th style="width: 80px">Actual</th>
                            <th style="width: 250px">Legislatura</th>
                            <th style="width: 200px"></th>
                        </tr>
                    </thead>
                    <tfoot>
                        <tr>
                            <th style="width: 80px">Actual</th>
                            <th style="width: 250px">Legislatura</th>
                            <th style="width: 200px"></th>
                        </tr>
                    </tfoot>
                    <tbody class="tbody">
                    </tbody>
                </table>
                <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#myModalLeg" onclick=" limpiaFormularioLeg();" value="Cancel" formnovalidate>Nueva legislatura</button><br />
            </div>
            <p></p>
        </div>

        <hr />
        <div>
            <div class="container">
                <h3>Departamentos</h3>
                <br />
                <asp:Button ID="btnEliminaDep" ClientIDMode="Static" runat="server" OnClick="btnEliminaDep_Click" Style="display: none" />
                <br />
                 <div class="pull-right" style="margin-bottom: 5px;">
                   <asp:DropDownList ID="ddlTipoInforme" CssClass="form-control btn" runat="server" title="Seleccione el tipo de exportación de datos para generar el informe" ClientIDMode="Static" style="width: 90px;">                            
                            <asp:ListItem Value="PDF" Text="PDF"></asp:ListItem>
                            <asp:ListItem Value="XLSX" Text="Excel"></asp:ListItem>
                            <asp:ListItem Value="DOCX" Text="Word"></asp:ListItem>
                            <asp:ListItem Value="HTML" Text="Visor"></asp:ListItem>
                        </asp:DropDownList>                        
                         <button type="button" onclick=" __doPostBack(&quot;<%= btnExportarDatosDep.UniqueID%>&quot);" title="Exporta la información según el formato seleccionado" class="btn" value="expDatos" formnovalidate=""><img src="../img/iconmonstr-magnifier-6-16.png">&nbsp;Descargar informe impreso</button>&nbsp;
                        <asp:Button ID="btnExportarDatosDep" OnClientClick="aspnetForm.target ='_blank';"  runat="server" style="display:none"   Text="Exportar datos" OnClick="btnExportarDatosDep_Click"    />    
                 </div>

                <table id="departamentos" class="display" cellspacing="0" width="100%">
                    <thead>
                        <tr>
                            <th>Departamento</th>
                            <th style="width: 100px">Validaciones pendientes</th>
                            <th style="width: 50px" title="porcentaje de avance (50% iniciados + terminados)">%</th>
                            <th style="width: 400px"></th>
                        </tr>
                    </thead>
                    <tfoot>
                        <tr>
                            <th>Departamento</th>
                            <th style="width: 100px">Validaciones pendientes</th>
                            <th style="width: 50px" title="porcentaje de avance (50% iniciados + terminados)">%</th>
                            <th style="width: 400px"></th>
                        </tr>
                    </tfoot>
                    <tbody class="tbody">
                    </tbody>
                </table>
                <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#myModalDep" onclick=" limpiaFormularioDep();" value="Cancel" formnovalidate>Nuevo departamento</button><br />
            </div>
            <p></p>
        </div>
        <hr />
        <div>
            <div id="conDepSel">
                <div class="container">
                    <h3>Objetivos estratégicos</h3>
                    <br />
                    <asp:Button ID="btnEliminaObj" ClientIDMode="Static" runat="server" OnClick="btnEliminaObj_Click" Style="display: none" />
                    <br />

                    <table id="objetivos" class="display" cellspacing="0" width="100%">
                        <thead>
                            <tr>
                                <th style="width: 50px">Cambio</th>
                                <th style="width: 60px">Validado</th>
                                <th style="width: 400px">Objetivo estratégico</th>
                                <th style="width: 100px">Validaciones pendientes</th>
                                <th style="width: 50px" title="porcentaje de avance (Suma de % de avance de sus acciones)">%</th>
                                <th style="width: 257px"></th>

                            </tr>
                        </thead>
                        <tfoot>
                            <tr>
                                <th style="width: 50px">Cambio</th>
                                <th style="width: 60px">Validado</th>
                                <th style="width: 400px">Objetivo estratégico</th>
                                <th style="width: 100px">Validaciones pendientes</th>
                                <th style="width: 50px" title="porcentaje de avance (Suma de % de avance de sus acciones)">%</th>
                                <th style="width: 257px"></th>

                            </tr>
                        </tfoot>
                        <tbody class="tbody">
                        </tbody>
                    </table>
                    <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#myModalObj" onclick=" limpiaFormulario();" value="Cancel" formnovalidate>Nuevo objetivo</button><br />
                </div>
                <p></p>
                <hr />
                <div id="accionesGrupo">
                    <div class="container">
                        <h3>Instrumentos/actividades</h3>
                        <br />
                        <asp:Button ID="btnEliminaAcc" ClientIDMode="Static" runat="server" OnClick="btnEliminaAcc_Click" Style="display: none" />
                        <br />

                        <table id="acciones" class="display" cellspacing="0" width="100%">

                            <thead>
                                <tr>
                                    <th style="width: 60px">Cambio</th>
                                    <th style="width: 60px">Validado</th>
                                    <th style="width: 500px">Instrumentos y actividades</th>
                                    <th style="width: 237px"></th>

                                </tr>
                            </thead>
                            <tfoot>
                                <tr>
                                    <th style="width: 60px">Cambio</th>
                                    <th style="width: 60px">Validado</th>
                                    <th style="width: 500px">Instrumentos y actividades</th>
                                    <th style="width: 237px"></th>

                                </tr>
                            </tfoot>


                            <tbody class="tbody">
                            </tbody>
                        </table>
                        <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#myModalAcc" onclick=" limpiaFormularioAcc();" value="Cancel" formnovalidate>Nuevo instrumento/actividades</button><br />
                        <br />
                    </div>
                </div>


            </div>

            <div class="modal fade" id="myModalLeg" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">

                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Legislatura</h4>
                            <asp:HiddenField ID="hdLegislaturaId" ClientIDMode="Static" runat="server" Value="" />
                        </div>
                        <div class="modal-body">

                            <asp:ValidationSummary runat="server" CssClass="text-danger" ValidationGroup="leg" />
                            <div class="form-group">
                                <label for="txtLegislatura">Legislatura</label>
                                <asp:TextBox runat="server" ID="txtLegislatura" ClientIDMode="Static" MaxLength="50" CssClass="form-control" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtLegislatura" ValidationGroup="leg"
                                    CssClass="text-danger" ErrorMessage="El campo legislatura es obligatorio." />
                            </div>

                            <div class="form-group">
                                <div class="col-md-10">
                                    <div class="checkbox">
                                        <label class="btn btn-default">
                                            <asp:CheckBox ID="chkLegActual" runat="server" ClientIDMode="Static"
                                                Text="Legislatura actual" ToolTip="Sólo se mostrará en el Portal de Transparencia la legislatura que esté activa actualmente (se desmarcará la existente). Sólo se debería marcar como actual cuando tuviese la información adecuada para que no apareciese en blanco o con excasa información." />
                                        </label>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-xs-6 form-group">
                                    <br />
                                    <label for="txtFechaInicioLegislatura">Inicio Legislatura</label>
                                    <asp:TextBox ID="txtFechaInicioLegislatura" ClientIDMode="Static" runat="server" Width="88" placeholder="dd/mm/aaaa" ToolTip="Se utiliza para el cálculo del progreso de la Legislatura"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtFechaInicioLegislatura" ValidationGroup="leg"
                                        CssClass="text-danger" ErrorMessage="fecha inicio de legislatura es obligatorio" />
                                </div>
                                <div class="col-xs-6 form-group">
                                    <br />
                                    <label for="txtFechaFinLegislatura">Fin Legislatura</label>
                                    <asp:TextBox ID="txtFechaFinLegislatura" runat="server" ClientIDMode="Static" Width="88" placeholder="dd/mm/aaaa" ToolTip="Se utiliza para el cálculo del progreso de la Legislatura"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtFechaFinLegislatura" ValidationGroup="leg"
                                        CssClass="text-danger" ErrorMessage="fecha inicio de legislatura es obligatorio" />
                                </div>
                            </div>

                            <br />
                            <br />
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnAddLeg" CssClass="btn btn-primary" ClientIDMode="Static" runat="server" Text="Guardar" OnClick="btnAddLeg_Click" CausesValidation="true"
                                ValidationGroup="leg" />
                            <asp:Button ID="btnUpdateLeg" CssClass="btn btn-primary" ClientIDMode="Static" runat="server" Text="Actualizar" OnClick="btnActualizaLeg_Click" CausesValidation="true"
                                ValidationGroup="leg" />
                            <button id="btnVerLeg" onclick="" title="Muestra los enlaces de los departamentos de la Legislatura actual (vigente)" type="button" class="btn btn-default">Ver</button>
                            <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                        </div>
                    </div>
                </div>
            </div>

            <div class="modal fade" id="myModalDep" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">

                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Departamento</h4>
                            <div id="DepartamentoEdit">
                                <span></span>
                                <asp:HiddenField ID="hdDepartamentoId" ClientIDMode="Static" runat="server" Value="" />
                            </div>
                        </div>
                        <div class="modal-body">

                            <asp:ValidationSummary runat="server" CssClass="text-danger" ValidationGroup="dep" />
                            <div class="form-group">
                                <label for="txtDepartamento">Departamento</label>
                                <asp:TextBox runat="server" ID="txtDepartamento" ClientIDMode="Static" MaxLength="250" CssClass="form-control" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtDepartamento" ValidationGroup="dep"
                                    CssClass="text-danger" ErrorMessage="El campo departamento es obligatorio." />
                            </div>
                            <div class="form-group">
                                <label for="txtOrdenDepartamento">Prelación</label>
                                <asp:TextBox runat="server" ID="txtOrdenDepartamento" placeholder="orden numérico del dpto según la estructura orgánica" ClientIDMode="Static" MaxLength="3" CssClass="form-control" /><span id="errmsgorden"></span>
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtOrdenDepartamento" ValidationGroup="dep"
                                    CssClass="text-danger" ErrorMessage="El campo orden departamento es obligatorio." />
                            </div>

                            <div class="form-group">
                                <div class="col-md-10">
                                    <div class="checkbox">
                                        <label class="btn btn-default">
                                            <asp:CheckBox ID="chkVisibleDep" runat="server" ClientIDMode="Static"
                                                Text="Visible" ToolTip="Si no está visible no se mostrará en la parte pública" />
                                        </label>
                                    </div>
                                </div>
                            </div>

                            <br />
                            <br />

                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnAddDep" CssClass="btn btn-primary" ClientIDMode="Static" runat="server" Text="Guardar" OnClick="btnAddDep_Click" CausesValidation="true"
                                ValidationGroup="dep" />
                            <asp:Button ID="btnUpdateDep" CssClass="btn btn-primary" ClientIDMode="Static" runat="server" Text="Actualizar" OnClick="btnActualizaDep_Click" CausesValidation="true"
                                ValidationGroup="dep" />
                            <button id="btnVerDep" onclick="" title="Muestra como se está visualizando en el Portal de Transparencia el Departamento Actual" type="button" class="btn btn-default">Ver</button>
                            <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                        </div>
                    </div>
                </div>



            </div>


            <div class="modal fade" id="myModalObj" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">

                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title" id="myModalLabelobj">Objetivo</h4>

                            <div id="ObjetivoEdit">
                                <span></span>
                                <asp:HiddenField ID="hdObjetivoIdEdit" ClientIDMode="Static" runat="server" Value="" />
                            </div>


                        </div>
                        <div class="modal-body">
                            <asp:ValidationSummary runat="server" CssClass="text-danger" ValidationGroup="obj" />
                            <div class="row small">
                                <div class="col-xs-6 form-group">
                                    <label for="lblObjAutor">Autor</label>
                                    <asp:Label ID="lblObjAutor" ClientIDMode="Static" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="col-xs-6 form-group">
                                    <label for="lblObjFecha">Fecha</label>
                                    <asp:Label ID="lblObjFecha" ClientIDMode="Static" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                            <div class="row small">
                                <div class="col-xs-6 form-group">
                                    <label for="lblObjTipoCambio">Tipo de cambio</label>
                                    <asp:Label ID="lblObjTipoCambio" ClientIDMode="Static" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="col-xs-6 form-group">
                                    <label for="lblObjEstadoVal">Estado validación</label>
                                    <asp:Label ID="lblObjEstadoVal" ClientIDMode="Static" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                            <hr />
                            <div class="row">
                                <div class="col-xs-6 form-group">
                                    Pendiente validación:
                                </div>
                                <div class="col-xs-6 form-group">
                                    Publicado actualmente:
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-6 form-group">
                                    <label for="ObjetivoEstrategicoPV">Objetivo estratégico</label>
                                    <asp:TextBox runat="server" ID="ObjetivoEstrategicoPV" ClientIDMode="Static" TextMode="MultiLine" Height="100px" MaxLength="450" CssClass="form-control" />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="ObjetivoEstrategicoPV" ValidationGroup="obj"
                                        CssClass="text-danger" ErrorMessage="El campo Objetivo estratégico es obligatorio." />
                                </div>
                                <div class="col-xs-6 form-group">
                                    <label for="ObjetivoEstrategico">Objetivo estratégico</label>
                                    <asp:TextBox runat="server" ID="ObjetivoEstrategico" ClientIDMode="Static" TextMode="MultiLine" Height="100px" MaxLength="450" CssClass="form-control solo-lectura" />
                                </div>
                            </div>
                               <div class="row">
                                <div class="col-xs-6 form-group">
                                  
                                </div>
                                <div class="col-xs-6 form-group">
                                   <label for="ObjetivoAvance">Avance: </label>
                                   <asp:Label ID="lblObjPORCENTAJE_AVANCE_CALCULADO" ClientIDMode="Static" runat="server" Text=""></asp:Label> 
                                </div>
                            </div>


                            <hr />

                            <div class="form-group">
                                <label for="txtObjComentarioValidador">Comentario validación</label>
                                <asp:TextBox runat="server" ID="txtObjComentarioValidador" ClientIDMode="Static" TextMode="MultiLine" MaxLength="450" Height="100px" CssClass="form-control" />
                            </div>


                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnOkObj" CssClass="btn btn-primary" ClientIDMode="Static" runat="server" Text="Aceptar cambios" OnClientClick="return confirm(&quot;Se sustituirán los datos publicados por los campos pendientes de publicar y se marcará como validado (Si el cambio fuese eliminación se eliminará el registro), ¿desea continuar?&quot;)" ToolTip="Se publicarán los cambios pendientes y se notificará al usuario (Si el cambio fuese eliminación se eliminará el registro)" OnClick="btnOkObj_Click" CausesValidation="true"
                                ValidationGroup="obj" />
                            <asp:Button ID="btnAdd" CssClass="btn btn-primary" ClientIDMode="Static" runat="server" Text="Guardar" OnClick="btnAdd_Click" ToolTip="Se guardarán los datos y se publicará el objetivo" CausesValidation="true"
                                ValidationGroup="obj" />
                            <asp:Button ID="btnUpdate" CssClass="btn btn-default" ClientIDMode="Static" runat="server" Text="Actualizar sin publicar" ToolTip="Guardará los campos pendientes de validación y el comentario del validador" OnClick="btnActualiza_Click" CausesValidation="true"
                                ValidationGroup="obj" />
                            <asp:Button ID="btnDeshacerCambio" ToolTip="Permite deshacer el cambio pendiente. Si es un alta, se eliminará. Si es una modificación, se deshacen las mismas. Si es una eliminación, se anula la petición de eliminación" OnClientClick="return confirm(&quot;¿Desea cancelar los cambios del Objetivo?&quot;);" CssClass="btn btn-default" ClientIDMode="Static" runat="server" Text="Deshacer cambios pendientes" OnClick="btnObjDeshacerCambio_Click" CausesValidation="true"
                                ValidationGroup="obj" />
                            <button id="btnVerObj" onclick="" type="button" class="btn btn-default">Ver</button>
                            <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                        </div>
                    </div>
                </div>



            </div>



            <div class="modal fade" id="myModalAcc" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">

                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title" id="myModalLabel">Instrumento / actividades</h4>                           
                            <div id="AccionEdit">
                                 Objetivo: <span></span>
                                <asp:HiddenField ID="hdAccionIdEdit" ClientIDMode="Static" runat="server" Value="" />
                            </div>

                        </div>
                        <div class="modal-body">

                            <asp:ValidationSummary runat="server" CssClass="text-danger" ValidationGroup="Acc" />
                            <div class="row small">
                                <div class="col-xs-6 form-group">
                                    <label for="lblAccAutor">Autor</label>
                                    <asp:Label ID="lblAccAutor" ClientIDMode="Static" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="col-xs-6 form-group">
                                    <label for="lblAccFecha">Fecha</label>
                                    <asp:Label ID="lblAccFecha" ClientIDMode="Static" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                            <div class="row small">
                                <div class="col-xs-6 form-group">
                                    <label for="lblAccTipoCambio">Tipo de cambio</label>
                                    <asp:Label ID="lblAccTipoCambio" ClientIDMode="Static" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="col-xs-6 form-group">
                                    <label for="lblAccEstadoVal">Estado validación</label>
                                    <asp:Label ID="lblAccEstadoVal" ClientIDMode="Static" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                            <hr />
                            <div class="row">
                                <div class="col-xs-6 form-group">
                                    Pendiente validación:
                                </div>
                                <div class="col-xs-6 form-group">
                                    Publicado actualmente:
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-xs-6 form-group">
                                    <label for="txtInstrumentosActPV">Instrumentos y actividades</label>
                                    <asp:TextBox runat="server" ID="txtInstrumentosActPV" ClientIDMode="Static" Height="100px" TextMode="MultiLine" MaxLength="450" CssClass="form-control" />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtInstrumentosActPV" ValidationGroup="Acc"
                                        CssClass="text-danger" ErrorMessage="El campo Instrumentos y actividades es obligatorio." />
                                </div>
                                <div class="col-xs-6 form-group">
                                    <label for="txtInstrumentosAct">Instrumentos y actividades</label>
                                    <asp:TextBox runat="server" ID="txtInstrumentosAct" ClientIDMode="Static" Height="100px" TextMode="MultiLine" MaxLength="450" CssClass="form-control solo-lectura" />
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-xs-6 form-group">
                                    <label for="txtOrganoResponsablePV">Órgano responsable</label>
                                    <asp:TextBox runat="server" ID="txtOrganoResponsablePV" ClientIDMode="Static" Height="100px" TextMode="MultiLine" MaxLength="450" CssClass="form-control" />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtOrganoResponsablePV" ValidationGroup="Acc"
                                        CssClass="text-danger" ErrorMessage="El campo Órgano responsable es obligatorio." />
                                </div>
                                <div class="col-xs-6 form-group">
                                    <label for="txtOrganoResponsable">Órgano responsable</label>
                                    <asp:TextBox runat="server" ID="txtOrganoResponsable" ClientIDMode="Static" Height="100px" TextMode="MultiLine" MaxLength="450" CssClass="form-control solo-lectura" />
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-xs-6 form-group">
                                    <label for="txtRecursosHumanosPV">Recursos Humanos</label>
                                    <asp:TextBox runat="server" ID="txtRecursosHumanosPV" ClientIDMode="Static" Height="100px" TextMode="MultiLine" MaxLength="450" CssClass="form-control" />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtRecursosHumanosPV" ValidationGroup="Acc"
                                        CssClass="text-danger" ErrorMessage="El campo RRHH es obligatorio." />
                                </div>
                                <div class="col-xs-6 form-group">
                                    <label for="txtRecursosHumanos">Recursos Humanos</label>
                                    <asp:TextBox runat="server" ID="txtRecursosHumanos" ClientIDMode="Static" Height="100px" TextMode="MultiLine" MaxLength="450" CssClass="form-control solo-lectura" />
                                </div>
                            </div>


                            <div class="row">
                                <div class="col-xs-6 form-group">
                                    <label for="txtCosteEconomicoPV">Coste económico</label>
                                    <asp:TextBox runat="server" ID="txtCosteEconomicoPV" ClientIDMode="Static" Height="100px" TextMode="MultiLine" MaxLength="450" CssClass="form-control" />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCosteEconomicoPV" ValidationGroup="Acc"
                                        CssClass="text-danger" ErrorMessage="El campo Coste económico es obligatorio." />
                                </div>
                                <div class="col-xs-6 form-group">
                                    <label for="txtCosteEconomico">Coste económico</label>
                                    <asp:TextBox runat="server" ID="txtCosteEconomico" ClientIDMode="Static" TextMode="MultiLine" Height="100px" MaxLength="450" CssClass="form-control solo-lectura" />
                                </div>

                            </div>

                            <div class="row">
                                <div class="col-xs-6 form-group">
                                    <label for="txtMediosOtrosPV">Otros medios</label>
                                    <asp:TextBox runat="server" ID="txtMediosOtrosPV" ClientIDMode="Static" Height="100px" TextMode="MultiLine" MaxLength="450" CssClass="form-control" />
                                </div>
                                <div class="col-xs-6 form-group">
                                    <label for="txtMediosOtros">Otros medios</label>
                                    <asp:TextBox runat="server" ID="txtMediosOtros" ClientIDMode="Static" Height="100px" TextMode="MultiLine" MaxLength="450" CssClass="form-control solo-lectura" />
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-xs-6 form-group">
                                    <label for="txtTemporalidadPV">Temporalidad</label>
                                    <asp:TextBox runat="server" ID="txtTemporalidadPV" ClientIDMode="Static" TextMode="MultiLine" Height="100px" MaxLength="450" CssClass="form-control" />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtTemporalidadPV" ValidationGroup="Acc"
                                        CssClass="text-danger" ErrorMessage="El campo Temporalidad es obligatorio." />
                                </div>
                                <div class="col-xs-6 form-group">
                                    <label for="txtTemporalidad">Temporalidad</label>
                                    <asp:TextBox runat="server" ID="txtTemporalidad" ClientIDMode="Static" TextMode="MultiLine" Height="100px" MaxLength="450" CssClass="form-control solo-lectura" />
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-xs-6 form-group">
                                    <label for="txtIndSeguimientoPV">Indicadores de seguimiento y evaluación</label>
                                    <asp:TextBox runat="server" ID="txtIndSeguimientoPV" ClientIDMode="Static" TextMode="MultiLine" Height="100px" MaxLength="450" CssClass="form-control" />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtIndSeguimientoPV" ValidationGroup="Acc"
                                        CssClass="text-danger" ErrorMessage="El campo Indicadores seguimiento y evaluación es obligatorio." />
                                </div>
                                <div class="col-xs-6 form-group">
                                    <label for="txtIndSeguimiento">Indicadores de seguimiento y evaluación</label>
                                    <asp:TextBox runat="server" ID="txtIndSeguimiento" ToolTip="Campo no visible públicamente." ClientIDMode="Static" TextMode="MultiLine" Height="100px" MaxLength="450" CssClass="form-control solo-lectura" />

                                </div>
                            </div>

                            <div class="row">
                                <div class="col-xs-6 form-group">
                                    <label for="txtSeguimientoPV">Seguimiento</label>
                                    <asp:TextBox runat="server" ID="txtSeguimientoPV" ClientIDMode="Static" TextMode="MultiLine" Height="100px" MaxLength="450" CssClass="form-control" />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtSeguimientoPV" ValidationGroup="Acc"
                                        CssClass="text-danger" ErrorMessage="El campo Seguimiento es obligatorio." />
                                </div>
                                <div class="col-xs-6 form-group">
                                    <label for="txtSeguimiento">Seguimiento</label>
                                    <asp:TextBox runat="server" ID="txtSeguimiento" ClientIDMode="Static" TextMode="MultiLine" Height="100px" MaxLength="450" CssClass="form-control solo-lectura" />
                                </div>
                            </div>

                     <%--       <div class="row">
                                <div class="col-xs-6 form-group">
                                    <label for="ddlEstadoSeguimientoPV">Estado de seguimiento</label>
                                    <asp:DropDownList ID="ddlEstadoSeguimientoPV" CssClass="form-control" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlEstadoSeguimientoPV" InitialValue="-1" ValidationGroup="Acc"
                                        CssClass="text-danger" ErrorMessage="El campo Estado Seguimiento es obligatorio." />
                                </div>
                                <div class="col-xs-6 form-group">
                                    <label for="ddlEstadoSeguimiento">Estado de seguimiento</label>
                                    <asp:DropDownList ID="ddlEstadoSeguimiento" CssClass="form-control" Enabled="false" ReadOnly="true" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                </div>
                            </div>--%>

                            <div class="row">
                                <div class="col-xs-6 form-group">
                                    <label for="ddlPorcentajeAvancePV">% de avance</label>
                                    <asp:DropDownList ID="ddlPorcentajeAvancePV" CssClass="form-control" runat="server" ClientIDMode="Static">
                                        <asp:ListItem Text="Seleccione..." Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="0%" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="25%" Value="25"></asp:ListItem>
                                        <asp:ListItem Text="50%" Value="50"></asp:ListItem>
                                        <asp:ListItem Text="75%" Value="75"></asp:ListItem>
                                        <asp:ListItem Text="100%" Value="100"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlPorcentajeAvancePV" InitialValue="-1" ValidationGroup="Acc"
                                        CssClass="text-danger" ErrorMessage="El campo porcentaje de avance es obligatorio." />
                                </div>
                                <div class="col-xs-6 form-group">
                                    <label for="ddlPorcentajeAvance">% de avance</label>
                                    <asp:DropDownList ID="ddlPorcentajeAvance" CssClass="form-control" Enabled="false" ReadOnly="true" runat="server" ClientIDMode="Static">
                                        <asp:ListItem Text="Seleccione..." Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="0%" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="25%" Value="25"></asp:ListItem>
                                        <asp:ListItem Text="50%" Value="50"></asp:ListItem>
                                        <asp:ListItem Text="75%" Value="75"></asp:ListItem>
                                        <asp:ListItem Text="100%" Value="100"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div class="form-group">
                                <label for="txtAccComentarioValidador">Comentario validación</label>
                                <asp:TextBox runat="server" ID="txtAccComentarioValidador" ToolTip="Comentarios del validador del contenido." ClientIDMode="Static" TextMode="MultiLine" MaxLength="450" Height="100px" CssClass="form-control" />
                            </div>
                            <div class="form-group">
                                <label for="txtAccObservaciones">Observaciones</label>
                                <asp:TextBox runat="server" ID="txtAccObservaciones" ToolTip="Campo no visible públicamente. Únicamente para gestión interna." ClientIDMode="Static" TextMode="MultiLine" MaxLength="450" Height="100px" CssClass="form-control" />
                            </div>


                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnOkAcc" CssClass="btn btn-primary" OnClientClick="return confirm(&quot;Se sustituirán los datos publicados por los campos pendientes de publicar y se marcará como validado (Si el cambio fuese eliminación se eliminará el registro), ¿desea continuar?&quot;)" ClientIDMode="Static" runat="server" Text="Aceptar cambios" ToolTip="Se publicarán los cambios pendientes (Si el cambio fuese eliminación se eliminará el registro)" OnClick="btnOkAcc_Click" CausesValidation="true"
                                ValidationGroup="Acc" />
                            <asp:Button ID="btnAddAcc" CssClass="btn btn-primary" runat="server" ClientIDMode="Static" Text="Publicar" ToolTip="Guardará los datos y lo publicará directamente" OnClick="btnAddAcc_Click" CausesValidation="true"
                                ValidationGroup="Acc" />
                            <asp:Button ID="btnGuardarAcc" CssClass="btn btn-default" ClientIDMode="Static" runat="server" ToolTip="Guardará los campos pendientes de validación y el comentario del validador" Text="Actualizar" OnClick="btnActualizaAcc_Click" CausesValidation="true"
                                ValidationGroup="Acc" />
                            <asp:Button ID="btnDeshacerCambioAcc" ToolTip="Permite deshacer el cambio pendiente. Si es un alta, se eliminará. Si es una modificación, se deshacen las mismas. Si es una eliminación, se anula la petición de eliminación" OnClientClick="return confirm(&quot;¿Desea cancelar los cambios del Instrumento/actuación?&quot;);" CssClass="btn btn-default" ClientIDMode="Static" runat="server" Text="Deshacer cambios pendientes" OnClick="btnAccDeshacerCambio_Click" CausesValidation="true"
                                ValidationGroup="Acc" />
                            <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                        </div>
                    </div>
                </div>



            </div>
        </div>
    </div>
</asp:Content>
