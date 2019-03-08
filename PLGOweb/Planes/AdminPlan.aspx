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
<%@ Page Title="Departamento" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminPlan.aspx.cs" Inherits="AdminPlan" EnableEventValidation="false" %>


<%@ Import Namespace="PLGOdata" %>
<%@ Import Namespace="PLGO" %>


<asp:Content runat="server" ContentPlaceHolderID="BottonScripts">    
    <script src="../Scripts/loadingoverlay.min.js"></script>         
    <script src="https://cdn.datatables.net/plug-ins/1.10.18/sorting/date-eu.js" integrity="sha384-73NtJ6lG5riO+oPkrdf286CE8rOPLXx+wdEYCx+3Sc2BeWxUBjPo2wc9dQPK0Q1S" crossorigin="anonymous"></script>    			
    <script type="text/javascript" charset="utf-8">
        

        function limpiaFormulario() {
            $('#ObjetivoEstrategicoPV').val(" ");            

            $('#btnUpdate').hide();
            $('#btnAdd').show();
            $('#btnDeshacerCambio').hide();
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
            $('#txtAccObservaciones').val(" ");
            $('#txtAccComentarioValidador').val(" ");

            $('#btnUpdateAcc').hide();
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

        function cargaAcciones(iObjetivoId, objetivo) {
            $("#acciones").LoadingOverlay("show");
            $.get("json_data_adminpln.aspx?op=loadA&id=" + iObjetivoId + "&t=" + new Date().getTime(), function (newDataArray) {
                $("#acciones").DataTable().clear();
                $("#acciones").DataTable().rows.add(newDataArray);
                $("#acciones").DataTable().draw();
                $("#acciones").LoadingOverlay("hide", true);
            });

            $("#ObjetivoEdit span").text(objetivo);           
            $("#hdObjetivoId").val(iObjetivoId);
            
        }
        function EditaObjetivo(row) {            
            var obj = JSON.parse(decodeURIComponent(row));            
            $("#hdObjetivoIdEdit").val(obj['CONTENIDO_ID']);
            $('#ObjetivoEstrategicoPV').val(obj['OBJETIVO_ESTRATEGICO_PDTE_VAL']);
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

            $('#btnAdd').hide();
            $('#btnUpdate').show();
            $('#btnDeshacerCambio').show();

            if (obj['TIPO_CAMBIO_CONTENIDO_ID'] != 0) {             
                $('#btnDeshacerCambio').show();
            }
            else {
                $('#btnDeshacerCambio').hide();
            }
        }
        function EditaAccion(row) {
            var obj = JSON.parse(decodeURIComponent(row));
            $("#hdObjetivoId").val(obj['CONTENIDO_ID']);
            $('#txtInstrumentosAct').val(obj['INSTRUMENTOS_ACT_PDTE_VAL']);
            $('#txtOrganoResponsable').val(obj['ORGANO_RESPONSABLE_PDTE_VAL']);
            $('#txtSeguimiento').val(obj['SEGUIMIENTO_PDTE_VAL']);
            $('#txtRecursosHumanos').val(obj['RECURSOS_HUMANOS_PDTE_VAL']);
            $('#txtCosteEconomico').val(obj['COSTE_ECONOMICO_PDTE_VAL']);
            $('#txtMediosOtros').val(obj['MEDIOS_OTROS_PDTE_VAL']);
            $('#txtTemporalidad').val(obj['TEMPORALIDAD_PDTE_VAL']);
            $('#txtIndSeguimiento').val(obj['INDICADOR_SEGUIMIENTO_PDTE_VAL']);
            $('#ddlPorcentajeAvance').val(obj['PORCENTAJE_AVANCE_PDTE_VAL']);
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
            $('#btnUpdateAcc').show();            
            if (obj['TIPO_CAMBIO_CONTENIDO_ID'] != 0) {                
                $('#btnDeshacerCambioAcc').show();
            }
            else {
                $('#btnDeshacerCambioAcc').hide();
            }
        }

                 function sinObjSeleccionado() {
                     $("#accionesGrupo").hide();
                 }
                 function conObjSeleccionado() {
                     $("#accionesGrupo").show();
                 }


                 $(document).ready(function () {

                     $('[data-toggle="tooltip"]').tooltip();

                     $("#objetivos").LoadingOverlay("show");
                     var tableObj = $('#objetivos').DataTable({
                         select: {
                             style: 'single'
                         },
                         order: [[0, "desc"]],
                         "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "Todos"]],
                         language: {
                             url: '../Content/DataTables/locale/es.json',
                             select: {
                                 rows: {
                                     _: "",
                                     0: "<br/> Seleccione un objetivo para ver sus instrumentos y acciones",
                                     1: ""
                                 }
                             }
                         },
                         "ajax": { "url": "json_data_adminpln.aspx?op=loadO&id=<%=Request["id"]%>" + "&t=" + new Date().getTime(), "dataSrc": "" },
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
                             if (data.OBJETIVO_ESTRATEGICO == null)
                                 return data.OBJETIVO_ESTRATEGICO_PDTE_VAL;
                             else
                                 return data.OBJETIVO_ESTRATEGICO;

                         }
                     },                   
                    {
                        "data": "PORCENTAJE_AVANCE_CALCULADO",
                        "className": "dt-center",
                    },
                    {
                        "data": null,
                        "bSortable": false,
                        mRender: function (data, type, row) {                            
                            return '<button type="button" title="Muestra como se está visualizando en el Portal de Transparencia" class="btn" onclick="window.open(&quot;../View/DetalleDepartamento?id=' + getUrlParameter('id') + '&oId=' + data.CONTENIDO_ID + '&quot;, &quot;_blank&quot;);" value="edObj" formnovalidate><img src="../img/view-16.png">&nbspVer</button>&nbsp;<button type="button" class="btn" data-toggle="modal" data-target="#myModalObj" onclick="EditaObjetivo(&quot;' + encodeURIComponent(JSON.stringify(row)) + '&quot;);" value="edObj" formnovalidate><img src="../img/edit_16.png">&nbspEditar</button>&nbsp;<button type="button" class="btn" onclick="if (confirm(&quot;¿Desea eliminar el Objetivo?&quot;)) __doPostBack(&quot;<%= btnEliminaObj.UniqueID%>&quot;, &quot;' + data.CONTENIDO_ID + '&quot;);"  href="#" formnovalidate><img src="../img/eliminar_16.png">&nbsp Eliminar</button>';
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
                  conObjSeleccionado();
                  cargaAcciones(rowData[0]['CONTENIDO_ID'], rowData[0]['OBJETIVO_ESTRATEGICO_PDTE_VAL']);
                  if (window.localStorage) { 
                      localStorage['selobjid'] = rowData[0]['CONTENIDO_ID']; 
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
                            conObjSeleccionado();
                            tableObj.row('#' + localStorage['selobjid']).select();
                            cargaAcciones(localStorage['selobjid'], '');
                        }

                    }
                    else
                        tableObj.row(0).select();
                })
            .on('click', 'tr', function () {
                var data = tableObj.row(this).data();
                EditaObjetivo(encodeURIComponent(JSON.stringify(data)));
            } )
            .on('xhr', function () {
                var json = tableObj.ajax.json();
                if (json.length == 0) {
                    sinObjSeleccionado();
                }
                $("#objetivos").LoadingOverlay("hide", true);
            });



                var tableAcc = $('#acciones').DataTable({
                destroy: true,  
                language: {
                    url: '../Content/DataTables/locale/es.json'
                },                
                order: [[0, "desc"]],
                "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "Todos"]],
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
                              if (data.INSTRUMENTOS_ACT_PDTE_VAL == null)
                                  return data.INSTRUMENTOS_ACT;                                  
                              else
                                  return data.INSTRUMENTOS_ACT_PDTE_VAL;

                          }
                      },                   
                      {
                         "data": null,
                         "bSortable": true,
                         "className": "dt-left",
                         mRender: function (data, type, row) {                             
                             if (data.FECHA_MODIFICACION === null)
                                 var fecha = new Date(data.FECHA_CREACION);
                             else
                                 var fecha = new Date(data.FECHA_MODIFICACION);
                             return ("0" + (fecha.getDate() + 1)).slice(-2) +  '/' + ("0" + (fecha.getMonth() + 1)).slice(-2) + '/' + fecha.getFullYear().toString().substr(-2);
                         }                          
                      },
                      {
                            "data": "PORCENTAJE_AVANCE",
                            "className": "dt-center",
                      },
                     {
                         "data": null,
                         "bSortable": false,
                         mRender: function (data, type, row) {                             
                             return '<button type="button" class="btn" title="Muestra como se está visualizando en el Portal de Transparencia" onclick="window.open(&quot;../View/DetalleDepartamento?id=' + getUrlParameter('id') + '&oId=' + data.OBJETIVO_CONTENIDO_ID + '&quot;, &quot;_blank&quot;);" value="edObj" formnovalidate><img src="../img/view-16.png">&nbspVer</button>&nbsp;<button type="button" class="btn" data-toggle="modal" data-target="#myModalAcc" onclick="EditaAccion(&quot;' + encodeURIComponent(JSON.stringify(row)) + '&quot;);" value="edObj" formnovalidate><img src="../img/edit_16.png">&nbspEditar</button>&nbsp; <button type="button" class="btn" onclick="if (confirm(&quot;¿Desea eliminar el instrumento/actividad?&quot;)) __doPostBack(&quot;<%= btnEliminaAcc.UniqueID%>&quot;, &quot;' + data.CONTENIDO_ID + '&quot;);"  href="#" formnovalidate><img src="../img/eliminar_16.png">&nbsp Eliminar</button>';                             
                         }
                     }
                ],
              
                    columnDefs: [
                          { type: 'date-eu', targets: 3 }
                    ],

                });             
        });






    </script>

</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">


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

    <div class="jumbotron">
        <h1>
            <asp:Literal ID="litNombredDEP" runat="server" Text="dpto"></asp:Literal></h1>
        <p class="lead">
            <asp:Literal ID="litLegislatura" runat="server" Text="legislatura"></asp:Literal>
        </p>
    </div>


    <div class="panel panel-default">
        <div class="panel-body">
                
        


        </div>
       
        
        <div>
            <div class="container">
                <div class="pull-right">
                        <asp:DropDownList ID="ddlTipoInforme" CssClass="form-control btn" runat="server" title="Seleccione el tipo de exportación de datos para generar el informe" ClientIDMode="Static" style="width: 90px;">                            
                            <asp:ListItem Value="PDF" Text="PDF"></asp:ListItem>
                            <asp:ListItem Value="XLSX" Text="Excel"></asp:ListItem>
                            <asp:ListItem Value="DOCX" Text="Word"></asp:ListItem>
                            <asp:ListItem Value="HTML" Text="Visor"></asp:ListItem>
                        </asp:DropDownList>                        
                         <button type="button" onclick=" __doPostBack(&quot;<%= btnExportarDatos.UniqueID%>&quot);" title="Exporta la información según el formato seleccionado" class="btn" value="expDatos" formnovalidate=""><img src="../img/iconmonstr-magnifier-6-16.png">&nbsp;Descargar informe</button>&nbsp;
                        <asp:Button ID="btnExportarDatos" OnClientClick="aspnetForm.target ='_blank';"  runat="server" style="display:none"   Text="Exportar datos" OnClick="btnExportarDatos_Click"   />                        

                    <button type="button" title="Exporta en un archivo excel los contenidos del Departamento. Se exportan dos hojas (contenido validado y pendiente de validar)" class="btn" onclick=" __doPostBack(&quot;<%= btnImprimirInformeExcel.UniqueID%>&quot);" value="expExcel" formnovalidate=""><img src="../img/doc_16.png">&nbsp;Informe cambios</button>&nbsp;
                    <asp:Button ID="btnImprimirInformeExcel" runat="server" style="display:none"  Text="Exportar informe" OnClick="btnImprimirInformeExcel_Click"  />    
                    <button type="button" title="Abre en una nueva ventana el gráfico de cuadro de mando" class="btn" onclick="window.open('../View/CuadroMando')"  formnovalidate=""><img src="../img/iconmonstr-chart-4-16.png">&nbsp;Cuadro de mando</button>&nbsp;
                    <button type="button" title="Abre en una nueva ventana el gráfico de evolución de los departamentos" class="btn" onclick="window.open('../View/EvolucionDepartamentos')"  formnovalidate=""><img src="../img/iconmonstr-chart-4-16.png">&nbsp;Evoluci&oacute;n departamentos</button>&nbsp;
                </div>

                 <h3>Objetivos estratégicos</h3>
                 <br />                
                <asp:Button ID="btnEliminaObj" ClientIDMode="Static"  runat="server" OnClick="btnEliminaObj_Click" style="display:none"  />
                <br />

                <table id="objetivos" class="display" cellspacing="0" width="100%">
                    <thead>
                        <tr>
                            <th style="width:60px">Cambio</th>
                            <th style="width:60px">Validado</th>
                            <th style="width:250px">Objetivo estratégico</th>                            
                            <th style="width:50px" title="porcentaje de avance validado">%</th>                            
                            <th style="width:305px"></th>

                        </tr>
                    </thead>
                    <tfoot>
                        <tr>
                            <th style="width:60px">Cambio</th>
                            <th style="width:60px">Validado</th>
                            <th style="width:250px">Objetivo estratégico</th>                            
                            <th style="width:50px" title="porcentaje de avance validado">%</th>                            
                            <th style="width:305px"></th>

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
                    <h3>Instrumentos / actividades</h3>
                     <br />                                        
                    <asp:Button ID="btnEliminaAcc" ClientIDMode="Static"  runat="server" OnClick="btnEliminaAcc_Click" style="display:none"  />                  
                    
                    <br />

                    <table id="acciones" class="display" cellspacing="0" width="100%">

                        <thead>
                            <tr>
                                <th style="width:60px">Cambio</th>
                                <th style="width:60px">Validado</th>
                                <th style="width:250px">Instrumentos y actividades</th>                                
                                <th style="width:50px" title="fecha modificación">Mod.</th>                                
                                <th style="width:50px" title="porcentaje de avance validado">%</th>                                
                                <th style="width:257px"></th>
                            </tr>
                        </thead>
                        <tfoot>
                            <tr>
                                <th style="width:60px">Cambio</th>
                                <th style="width:60px">Validado</th>
                                <th style="width:250px">Instrumentos y actividades</th>                                
                                <th style="width:50px" title="fecha modificación">Mod.</th>                                
                                <th style="width:50px" title="porcentaje de avance validado">%</th>                                
                                <th style="width:257px"></th>
                            </tr>
                        </tfoot>


                        <tbody class="tbody">
                        </tbody>
                    </table>
                    <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#myModalAcc" onclick=" limpiaFormularioAcc();" value="Cancel" formnovalidate>Nuevo instrumento/actividades</button><br /><br />
                </div>
            </div>

                   


            <div class="modal fade" id="myModalObj" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">

                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title" id="myModalLabelobj">Objetivo</h4>
                            <asp:HiddenField ID="hdObjetivoIdEdit" ClientIDMode="Static" runat="server" value="" /> 
                              <br />
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
                        </div>
                        <div class="modal-body">                         

                            <asp:ValidationSummary runat="server" CssClass="text-danger" ValidationGroup="obj" />
                            <div class="form-group">
                                <label for="ObjetivoEstrategicoPV">Objetivo estratégico</label>
                                <asp:TextBox runat="server" ID="ObjetivoEstrategicoPV" ClientIDMode="Static" TextMode="MultiLine" MaxLength="450" CssClass="form-control" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="ObjetivoEstrategicoPV" ValidationGroup="obj"
                                    CssClass="text-danger" ErrorMessage="El campo Objetivo estratégico es obligatorio." />
                            </div>                            

                             <div class="form-group">
                                <label for="txtObjComentarioValidador">Comentario validación</label>
                                <asp:TextBox runat="server" ID="txtObjComentarioValidador" Enabled="false" ClientIDMode="Static" TextMode="MultiLine" MaxLength="450" Height="100px" CssClass="form-control" />
                            </div>

                        </div>
                        <div class="modal-footer">                            
                            <asp:Button ID="btnAdd" CssClass="btn btn-primary" ClientIDMode="Static" runat="server" Text="Guardar" OnClick="btnAdd_Click" causesvalidation="true"
                            validationgroup="obj" />
                            <asp:Button ID="btnUpdate" CssClass="btn btn-primary" ClientIDMode="Static" runat="server" Text="Actualizar" OnClick="btnActualiza_Click" causesvalidation="true"
                            validationgroup="obj" />                            
                            <asp:Button ID="btnDeshacerCambio"  ToolTip="Permite deshacer el cambio pendiente. Si es un alta, se eliminará. Si es una modificación, se deshacen las mismas. Si es una eliminación, se anula la petición de eliminación" OnClientClick="return confirm(&quot;¿Desea cancelar los cambios del Objetivo?&quot;);"  CssClass="btn btn-default" ClientIDMode="Static" runat="server" Text="Deshacer cambios pendientes" OnClick="btnObjDeshacerCambio_Click" causesvalidation="true"
                            validationgroup="obj" />
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
                            <div id="ObjetivoEdit">Objetivo: <strong><span></span></strong>
                            <asp:HiddenField ID="hdObjetivoId" ClientIDMode="Static" runat="server" value="" /> 
                         </div>
                            <br />
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
                        </div>
                        <div class="modal-body">                            

                            <asp:ValidationSummary runat="server" CssClass="text-danger" ValidationGroup="Acc" />                                            
                            
                            <div class="form-group">
                                <label for="txtInstrumentosAct">Instrumentos y actividades</label>
                                <asp:TextBox runat="server" ID="txtInstrumentosAct" ClientIDMode="Static" TextMode="MultiLine" MaxLength="450" CssClass="form-control" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtInstrumentosAct" ValidationGroup="Acc"
                                    CssClass="text-danger" ErrorMessage="El campo Instrumentos y actividades es obligatorio." />
                            </div>
                            <div class="form-group">
                                <label for="txtOrganoResponsable">Órgano responsable</label>
                                <asp:TextBox runat="server" ID="txtOrganoResponsable" ClientIDMode="Static" TextMode="MultiLine" MaxLength="450" CssClass="form-control" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtOrganoResponsable" ValidationGroup="Acc"
                                    CssClass="text-danger" ErrorMessage="El campo Órgano responsable es obligatorio." />
                            </div>                           
                            <div class="form-group">
                                <label for="txtRecursosHumanos">Recursos Humanos</label>
                                <asp:TextBox runat="server" ID="txtRecursosHumanos" ClientIDMode="Static" TextMode="MultiLine" MaxLength="450" CssClass="form-control" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtRecursosHumanos" ValidationGroup="Acc"
                                    CssClass="text-danger" ErrorMessage="El campo RRHH es obligatorio." />
                            </div>
                            <div class="form-group">
                                <label for="txtCosteEconomico">Coste económico</label>
                                <asp:TextBox runat="server" ID="txtCosteEconomico" ClientIDMode="Static" TextMode="MultiLine" MaxLength="450" CssClass="form-control" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCosteEconomico" ValidationGroup="Acc"
                                    CssClass="text-danger" ErrorMessage="El campo Coste económico es obligatorio." />
                            </div>
                            <div class="form-group">
                                <label for="txtMediosOtros">Otros medios</label>
                                <asp:TextBox runat="server" ID="txtMediosOtros" ClientIDMode="Static" TextMode="MultiLine" MaxLength="450" CssClass="form-control" />                                
                            </div>
                            <div class="form-group">
                                <label for="txtTemporalidad">Temporalidad</label>
                                <asp:TextBox runat="server" ID="txtTemporalidad" ClientIDMode="Static" TextMode="MultiLine" MaxLength="450" CssClass="form-control" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtTemporalidad" ValidationGroup="Acc"
                                    CssClass="text-danger" ErrorMessage="El campo Temporalidad es obligatorio." />
                            </div>
                            <div class="form-group">
                                <label for="txtIndSeguimiento">Indicadores de seguimiento y evaluación</label>
                                <asp:TextBox runat="server" ToolTip="Campo no visible públicamente." ID="txtIndSeguimiento" ClientIDMode="Static" TextMode="MultiLine" MaxLength="450" CssClass="form-control" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtIndSeguimiento" ValidationGroup="Acc"
                                    CssClass="text-danger" ErrorMessage="El campo Indicadores seguimiento y evaluación es obligatorio." />
                            </div>
                             <div class="form-group">
                                <label for="txtSeguimiento">Seguimiento</label>
                                <asp:TextBox 
                                    data-toggle='tooltip' data-original-title='En caso de querer indicar un enlace a una página web por ejemplo al BOA, puede indicar la dirección URL (debe comenzar por http://). Por ejemplo: Texto de ejemplo http://www.boa.aragon.es. '

                                    runat="server" ID="txtSeguimiento" ClientIDMode="Static" TextMode="MultiLine" MaxLength="450" CssClass="form-control" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtSeguimiento" ValidationGroup="Acc"
                                    CssClass="text-danger" ErrorMessage="El campo Seguimiento es obligatorio." />
                            </div>
                            <%--<div class="form-group">
                                <label for="ddlEstadoSeguimiento">Estado de seguimiento</label>
                                <asp:DropDownList ID="ddlEstadoSeguimiento" CssClass="form-control" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlEstadoSeguimiento" InitialValue="-1" ValidationGroup="Acc"
                                    CssClass="text-danger" ErrorMessage="El campo Estado Seguimiento es obligatorio." />
                            </div>    --%>

                            <div class="form-group">
                                <label for="ddlPorcentajeAvance">Estado de seguimiento</label>
                               <asp:DropDownList ID="ddlPorcentajeAvance" CssClass="form-control" runat="server" ClientIDMode="Static">
                                        <asp:ListItem Text="Seleccione..." Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="0%" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="25%" Value="25"></asp:ListItem>
                                        <asp:ListItem Text="50%" Value="50"></asp:ListItem>
                                        <asp:ListItem Text="75%" Value="75"></asp:ListItem>
                                        <asp:ListItem Text="100%" Value="100"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlPorcentajeAvance" InitialValue="-1" ValidationGroup="Acc"
                                    CssClass="text-danger" ErrorMessage="El campo porcentaje de avance es obligatorio." />                                
                            </div>


                            
                            <div class="form-group">
                                <label for="txtAccComentarioValidador">Comentario validación</label>
                                <asp:TextBox runat="server" ID="txtAccComentarioValidador" ToolTip="Comentarios del validador del contenido." Enabled="false" ClientIDMode="Static" TextMode="MultiLine" MaxLength="450" Height="100px" CssClass="form-control" />
                            </div>                      

                            <div class="form-group">
                                <label for="txtAccObservaciones">Observaciones</label>
                                <asp:TextBox runat="server" ID="txtAccObservaciones" ToolTip="Campo no visible públicamente. Únicamente para gestión interna." ClientIDMode="Static" TextMode="MultiLine" MaxLength="450" Height="100px" CssClass="form-control" />
                            </div>                  
                                                    

                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnAddAcc" CssClass="btn btn-primary" runat="server" ClientIDMode="Static" Text="Guardar" OnClick="btnAddAcc_Click"  causesvalidation="true"
                            validationgroup="Acc" />
                            <asp:Button ID="btnUpdateAcc" CssClass="btn btn-primary" ClientIDMode="Static" runat="server" Text="Actualizar" OnClick="btnActualizaAcc_Click" causesvalidation="true"
                            validationgroup="Acc" />                                                        
                            <asp:Button ID="btnDeshacerCambioAcc"  ToolTip="Permite deshacer el cambio pendiente. Si es un alta, se eliminará. Si es una modificación, se deshacen las mismas. Si es una eliminación, se anula la petición de eliminación" OnClientClick="return confirm(&quot;¿Desea cancelar los cambios del Instrumento/actuación?&quot;);"  CssClass="btn btn-default" ClientIDMode="Static" runat="server" Text="Deshacer cambios pendientes" OnClick="btnAccDeshacerCambio_Click" causesvalidation="true"
                            validationgroup="Acc" />
                            <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                        </div>
                    </div>
                </div>



            </div>
        </div>
    </div>
</asp:Content>
