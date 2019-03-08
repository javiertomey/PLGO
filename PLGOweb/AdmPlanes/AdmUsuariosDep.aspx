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
<%@ Page Title="Administrador" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdmUsuariosDep.aspx.cs" Inherits="AdminUsuariosDep" EnableEventValidation="false" %>


<%@ Import Namespace="PLGOdata" %>
<%@ Import Namespace="PLGO" %>




<asp:Content runat="server" ContentPlaceHolderID="BottonScripts">
    <script src="../Scripts/utilidades.js"></script>
    <script type="text/javascript" charset="utf-8">

       
        function limpiaFormularioUsersDep() {
            $('#txtCorreoUsuarioDep').val(" ");
            $('#txtNombreUsuarioDep').val(" ");
            $('#txtApellidosUsuarioDep').val(" ");

            $('#btnUpdateUserDep').hide();
            $('#btnAddUserDep').show();
        }
      
        function EditaUserDep(row) {
            var obj = JSON.parse(decodeURIComponent(row));                     
            $("#hdUserDepId").val(obj['USUARIOID']);
            $('#txtCorreoUsuarioDep').val(obj['AspNetUsers']['Email']);
            $('#txtNombreUsuarioDep').val(obj['NOMBRE']);
            $('#txtApellidosUsuarioDep').val(obj['APELLIDOS']);
            $('#btnAddUserDep').hide();
            $('#btnUpdateUserDep').show();            
        }       

        function errorCB(result) {
            alert(result.status + ' ' + result.statusText);
        }


         
              

                 $(document).ready(function () {

                     $('[data-toggle="tooltip"]').tooltip();


                     var tableUsrDep = $('#usuariosDep').DataTable({                         
                         order: [[0, "desc"]],
                         "iDisplayLength": 10,
                         "lengthMenu": [ [2, 5, 10, 25, 50, -1], [2, 5, 10, 25, 50, "Todos"] ],
                         language: {
                             url: '../Content/DataTables/locale/es.json',                            
                         },
                         "ajax": { "url": "json_data_admpln.aspx?op=loadU&id=" + <%=Request["id"]%>, "dataSrc": "" },
                         "columns": [
                            {
                                "data": null,
                                "bSortable": true,
                                mRender: function (data, type, row) {
                                    return data["AspNetUsers"]["Email"];
                                }
                            },
                            { "data": "APELLIDOS" },
                            { "data": "NOMBRE" },
                            {
                                "data": null,
                                "bSortable": false,
                                mRender: function (data, type, row) {
                                    return '<button type="button" class="btn" data-toggle="modal" data-target="#myModalUsrsDep" onclick="EditaUserDep(&quot;' + encodeURIComponent(JSON.stringify(row)) + '&quot;);" value="edLeg" formnovalidate><img src="../img/edit_16.png">&nbspEditar</button>&nbsp; <button type="button" class="btn" onclick="if (confirm(&quot;¿Desea eliminar el usuario (no se borrarán los datos del departamento asociado pero no podrá acceder a la aplicación)?&quot;)) __doPostBack(&quot;<%= btnEliminaUserDep.UniqueID%>&quot;, &quot;' + data.USUARIOID + '&quot;);"  href="#" formnovalidate><img src="../img/eliminar_16.png">&nbsp Eliminar</button>';
                                }
                            },
                         ],
                     });                    


                 });
    </script>

</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <asp:Button ID="btnEliminaUserDep" ClientIDMode="Static" runat="server" OnClick="btnEliminaUserDep_Click" Style="display: none" />



    <h2><%: Title %></h2>


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
                <h3>Usuarios del departamento <%: NombreDepartamento %></h3>
                <br />
                
                <br />

                <table id="usuariosDep" class="display" cellspacing="0" width="100%">
                    <thead>
                        <tr>
                            <th>Usuario</th>                            
                            <th>Apellidos</th>                            
                            <th>Nombre</th>                                                        
                            <th style="width: 200px"></th>
                        </tr>
                    </thead>
                    <tfoot>
                        <tr>
                            <th>Usuario</th>                            
                            <th>Apellidos</th>                            
                            <th>Nombre</th>                                                        
                            <th style="width: 200px"></th>
                        </tr>
                    </tfoot>
                    <tbody class="tbody">
                    </tbody>
                </table>
                <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#myModalUsrsDep" onclick=" limpiaFormularioUsersDep();" value="Cancel" formnovalidate>Agregar usuario</button><br /><br />
                 <a class="btn btn-default" href="AdmPlan.aspx">Volver &raquo;</a>
            </div>
            <p></p>
        </div>

        

        <div class="modal fade" id="myModalUsrsDep" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">

                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title"></h4>
                            <asp:HiddenField ID="hdUserDepId" ClientIDMode="Static" runat="server" Value="" />
                        </div>
                        <div class="modal-body">

                            <asp:ValidationSummary runat="server" CssClass="text-danger"  />
                            <div class="form-group">
                                <label for="txtCorreoUsuarioDep">Usuario (correo electrónico)</label>
                                <asp:TextBox runat="server" ID="txtCorreoUsuarioDep" placeholder="Sólo cuentas corporativas (*.aragon.es)" ToolTip="Sólo se admiten cuentas corporativas del Gobierno de Aragón (p.e. xxxx@aragon.es)" ClientIDMode="Static" MaxLength="150" CssClass="form-control" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCorreoUsuarioDep" 
                                    CssClass="text-danger" ErrorMessage="El campo es obligatorio." />
                                <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"   ControlToValidate = "txtCorreoUsuarioDep"  ValidationExpression="^[a-zA-Z0-9._%+-]+(@aragon\.es|@salud\.aragon\.es|@educa\.aragon\.es)$"
                                    CssClass="text-danger" ErrorMessage="Sólo sirven cuentas corporativas (*aragon.es)"></asp:RegularExpressionValidator>--%>

                            </div>

                            <div class="form-group">
                                <label for="txtNombreUsuarioDep">Nombre</label>
                                <asp:TextBox runat="server" ID="txtNombreUsuarioDep" ClientIDMode="Static" MaxLength="150" CssClass="form-control" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNombreUsuarioDep" ValidationGroup="userdep"
                                    CssClass="text-danger" ErrorMessage="El campo es obligatorio." />
                            </div>

                            <div class="form-group">
                                <label for="txtApellidosUsuarioDep">Apellidos</label>
                                <asp:TextBox runat="server" ID="txtApellidosUsuarioDep" ClientIDMode="Static" MaxLength="150" CssClass="form-control" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtApellidosUsuarioDep" ValidationGroup="userdep"
                                    CssClass="text-danger" ErrorMessage="El campo es obligatorio." />
                            </div>
                           
                            <br />
                            <br />
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnAddUserDep" CssClass="btn btn-primary" ClientIDMode="Static" runat="server" Text="Guardar" OnClick="btnAddUserDep_Click" CausesValidation="true"
                                />
                            <asp:Button ID="btnUpdateUserDep" CssClass="btn btn-primary" ClientIDMode="Static" runat="server" Text="Actualizar" OnClick="btnActualizaUserDep_Click" CausesValidation="true"
                                 />
                            <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                        </div>
                    </div>
                </div>
            </div>
       
      
       
</div>
</asp:Content>
