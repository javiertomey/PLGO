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
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DetalleDepartamento.aspx.cs" Inherits="PLGOweb.View.DetalleDepartamento" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es" xml:lang="es">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta http-equiv="Content-Language" content="es"/>    
    <meta name="Robots" content="all"/>    
    <meta name="geo.region" content="ES-AR" />
    <meta name="geo.position" content="41.6;-0.9" />
    <meta name="ICBM" content="41.6, -0.9" />
    <meta name="DESCRIPTION" content="Plan de Gobierno del Gobierno de Aragón" />
    <meta name="KEYWORDS" lang="es" content="plan de gobierno,transparencia, aragon, gobierno abierto, rendición cuentas"/>
    <meta name="DC.title" content="Plan de Gobierno. Transparencia Aragón"/>    
    <meta name="DC.Subject" content="plan de gobierno,transparencia, aragon, gobierno abierto"/>
    <meta name="DC.Description" content="Plan de Gobierno del Gobierno de Aragón" />
    <meta name="DC.Publisher" content="Gobierno de Aragón"/>
    <title>Plan de Gobierno | Transparencia Arag&oacute;n</title>
    <link rel="shortcut icon" href="img/favicon.ico" type="image/vnd.microsoft.icon" />        
    <link href="<%=ResolveClientUrl("Content/bootstrap.min.css")%>" rel="stylesheet" />
    <link href="<%=ResolveClientUrl("Content/Site.min.css")%>" rel="stylesheet" />
    <link href="<%=ResolveClientUrl("Content/frame/css_PCqb-YGjrLdFwKkgsXXwwQfrrH74oEs2k2Nj86Waazc.css")%>" rel="stylesheet" />
    <link href="<%=ResolveClientUrl("Content/DataTables/css/jquery.dataTables.min.css")%>" rel="stylesheet" />
    <link href="<%=ResolveClientUrl("Content/frame/plgo.css")%>" rel="stylesheet" />
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.7.2/css/all.css" integrity="sha384-fnmOCqbTlWIlj8LyTjo7mOUStjsKC4pOpQbqyi7RrhN7udi9RwhKkMHpvLbHG9Sr" crossorigin="anonymous" />
</head>
<body>
    <form runat="server">
        <asp:ScriptManager runat="server">
            <Scripts>                
                <asp:ScriptReference Path="Scripts/jquery-3.1.1.min.js" />
                <asp:ScriptReference Path="Scripts/jquery-progresspiesvg-min.js" />                
                <asp:ScriptReference Path="Scripts/DataTables/jquery.dataTables.min.js" />
                <asp:ScriptReference Path="Scripts/DataTables/dataTables.select.min.js" />
                <asp:ScriptReference Path="Scripts/bootstrap.min.js" />                
                <asp:ScriptReference Path="Scripts/respond.min.js" />                
                <asp:ScriptReference Path="Scripts/treeview/core.min.js" />
                <asp:ScriptReference Path="Scripts/treeview/tree.min.js" />
                <asp:ScriptReference Path="Scripts/utilidades.js" />                                
                <asp:ScriptReference Path="Scripts/utilidades.js" />                                
                <asp:ScriptReference Path="Scripts/jquery.color.min.js" />                                
                <asp:ScriptReference Path="Scripts/jquery.animateNumber.min.js" />                                


            </Scripts>
        </asp:ScriptManager>

	<script async src="https://www.googletagmanager.com/gtag/js?id=UA-116483485-2"></script>
	<script>
	  window.dataLayer = window.dataLayer || [];
	  function gtag(){dataLayer.push(arguments);}
	  gtag('js', new Date());
	  gtag('config', 'UA-116483485-2');
	</script>
        <script type="text/javascript" charset="utf-8">
            var tableAcc;            

            $(document).ready(function () {


                

                var tableObj = $('#objetivos').DataTable({
                    bPaginate: false,                    
                    order: [[0, "asc"]],
                    sDom: 'rt',
                    'createdRow': function (row, data, dataIndex) {
                        $(row).attr('id', data.ID);
                    },
                    language: {
                        url: 'Content/DataTables/locale/es.json',
                    },
                    "ajax": { "url": "json_data.aspx?op=O&id=<%=Request["id"]%>" + "&t=" + new Date().getTime(), "dataSrc": "" },
                    "columns": [
                       { "data": "Obj" },
                       {
                           "data": null,
                           "className": "dt-left",
                           mRender: function(data, type, row) {
                               return "<span class='grafp'>" + Math.round(data.Porc) + "% </span>";
                           }

                       },
                       {
                           "data": null,
                           "bSortable": false,
                           mRender: function (data, type, row) {                               
                               return "<i class='fa fa-search' aria-hidden='true'></i>";
                           }
                       },
                    ],
                });
                            
                tableObj
                .on('init.dt', function () {
                    var ObjId = getUrlParameter('oId');
                    if (ObjId != null) {
                        $('#' + ObjId)[0].click();
                    }
                    $(".grafp").progressPie({
                        color: "rgba(0,150,100,0.6)",
                        overlap: false,
                    });
                });
                
                $('#objetivos tbody').on('click', 'tr', function () {	
                    var aData = tableObj.row(this).data();
                    VerAccion(encodeURIComponent(JSON.stringify(aData)));
                    $('#myModalDetalle').modal('show');
                });
				
                $('#acciones tbody').on('click', 'tr', function () {
                    var tr = $(this).closest('tr');                                                            
                    var row = tableAcc.row(tr);
                    if (row.child.isShown()) {
                        row.child.hide();
                        tr.removeClass('shown');
                    }
                    else {
                        row.child(format(row.data()), 'no-padding').show();
                        tr.addClass('shown');
                        $('div.slider', row.child()).slideDown();
                    }
                });
            });

            function linkify(inputText) {
                var replacedText, replacePattern1, replacePattern2, replacePattern3;                                

                replacePattern1 = /(\b(https?|ftp):\/\/[-A-Z0-9+&@#\/%?=~_|!:,.;]*[-A-Z0-9+&@#\/%=~_|])/gim;
                replacedText = inputText.replace(replacePattern1, function (url) {
                    urlText = url;
                    if (urlText.length > 30) {
                        urlText = urlText.substring(0, 30) + '...';
                    }
                    return '<a ref="nofollow" target="_blank" href="' + url + '"><img title="$1" alt="' + url + '" src="img/iconmonstr-file-19-24.png"  width="18" height="18"/>' + urlText + '</a>';
                });
                replacePattern3 = /(([a-zA-Z0-9\-\_\.])+@[a-zA-Z\_]+?(\.[a-zA-Z]{2,6})+)/gim;
                replacedText = replacedText.replace(replacePattern3, '<a href="mailto:$1">$1</a>');

                replacePattern2 = /(^|[^\/])(www\.[\S]+(\b|$))/gim;
                replacedText = replacedText.replace(replacePattern2, function (url) {
                    urlText = url;
                    if (urlText.length > 30) {
                        urlText = urlText.substring(0, 30) + '...';
                    }
                    return '<a ref="nofollow" target="_blank" href="' + url + '">' + urlText + '</a>';
                });                
                return replacedText;
            }

            function format(data) {

                try{
                    var img = '';
                    if (data.IS == 1)
                        img = "";
                    else if (data.IS == 2)
                        img = "<img title='En ejecución' alt='En ejecución' src='img/enejecucion.png'  width='23' height='18'/>&nbsp;";
                    else if (data.IS == 3)
                        img = "<img title='Cumplido' alt='Cumplido' src='img/cumplido.png'  width='23' height='18'/>&nbsp;";

                    var seg = '';                                      
                    seg = linkify(data.Seg);                   
                   
                    seg = seg.replace(/\n([ \t]*\n)+/g, '</p><p>').replace(/\n/g, '<br />');
                    
                    if ($(window).width() > 979) {
                        return '<div style="overflow-x:auto;" class="slider" name >' +
                            '<table style="width: 100%;">' +
                            '<thead>' +
                            '<tr>' +
                            '<th>&Oacute;rgano responsable</th>' +
                            '<th>Recursos humanos</th>' +
                            '<th>Coste econ&oacute;mico</th>' +
                            '<th>Otros medios</th>' +
                            '<th>Temporalidad</th>' +
                            '<th>% avance</th>' +
                            '<th>Seguimiento</th>' +
                            '</tr>' +
                            '</thead>' +
                            '<tbody class="tbody">' +
                            '<tr>' +
                            '<td style="min-width: 105px;">' + (data.Org == null ? "-" : data.Org) + '</td>' +
                            '<td class="dont-break-out" style="min-width: 105px;">' + (data.RH == null ? "-" : data.RH) + '</td>' +
                            '<td class="dont-break-out" style="min-width: 105px;">' + (data.CE == null ? "-" : data.CE) + '</td>' +
                            '<td class="dont-break-out" style="min-width: 105px;">' + (data.OM == null ? "-" : data.OM) + '</td>' +
                            '<td class="dont-break-out" style="min-width: 75px;">' + (data.Tem == null ? "-" : data.Tem) + '</td>' +
                            '<td class="dont-break-out" style="min-width: 40px;">' + (data.IS == null ? "0%" : data.IS + '%') + '</td>' +
                            '<td style="min-width: 110px;">' + (seg == '' ? "-" : '<p lang="es" class="dont-break-out">' + img + seg + '</p>') + '</td>' +
                            '</tr>' +
                            '</tbody>' +
                            '</table>' +
                            '</div>'
                    } else {
                        return '<div><ul data-dtr-index="0" class="dtr-details">' +
                            '<li data-dtr-index="2" data-dt-row="0" data-dt-column="2"><span class="dtr-title"><strong>&Oacute;rgano responsable:</strong> </span> <span class="dtr-data">' + (data.Org == null ? "-" : data.Org) + '</span></li>' +
                            '<li data-dtr-index="3" data-dt-row="0" data-dt-column="3"><span class="dtr-title"><strong>Recursos humanos:</strong> </span> <span class="dtr-data">' + (data.RH == null ? "-" : data.RH) + '</span></li>' +
                            '<li data-dtr-index="4" data-dt-row="0" data-dt-column="4"><span class="dtr-title"><strong>Coste econ&oacute;mico: </strong></span> <span class="dtr-data">' + (data.CE == null ? "-" : data.CE) + '</span></li>' +
                            '<li data-dtr-index="5" data-dt-row="0" data-dt-column="5"><span class="dtr-title"><strong>Otros medios:</strong> </span> <span class="dtr-data">' + (data.OM == null ? "-" : data.OM) + '</span></li>' +
                            '<li data-dtr-index="6" data-dt-row="0" data-dt-column="6"><span class="dtr-title"><strong>Temporalidad: </strong></span> <span class="dtr-data">' + (data.Tem == null ? "-" : data.Tem) + '</span></li>' +
                            '<li data-dtr-index="7" data-dt-row="0" data-dt-column="7"><span class="dtr-title"><strong>% avance: </strong></span> <span class="dtr-data">' + (data.IS == null ? "0%" : data.IS + '%') + '</span></li>' +
                            '<li data-dtr-index="8" data-dt-row="0" data-dt-column="8"><span class="dtr-title"><strong>Seguimiento: </strong></span> <span class="dtr-data">' + (seg == '' ? "" : '<p lang="es" ><br/>' + img + seg + '</p>') + '</span></li>' +
                            '</ul></div>'
                    }
                }
                catch (e)
                {
                    console.log(e.message);
                }
            }

            function VerAccion(row) {
                var obj = JSON.parse(decodeURIComponent(row));

                    tableAcc = $('#acciones').DataTable({
                    destroy: true,  
                    autoWidth: false,
                    bPaginate: false,
                    sDom: 'rt',
                    language: {
                        url: 'Content/DataTables/locale/es.json'
                    },                    
                    "aaData": obj.AC, "dataSrc": "",
                    "columns": [
                    { "data": "Ins" },
                    {
                        "data": null,
                        "bSortable": false,
                        "className": 'details-control',
                        "width": "50px",
                        mRender: function (data, type, row) {                            
                            return "<i class='fa fa-search' aria-hidden='true'></i>";
                        }
                    },
                    ]
                    });

                $("#ObjEdit span").text(obj.Obj);
            }

        </script>


        <style type="text/css">        
        tr {
            cursor: pointer;
        }
	</style>

        <div id="header">
            <div id="header_inn">
                <div class="logo_gob">
                    <img src="img/logo_gob.png" alt="Gobierno de Aragón"/>
                </div>

                <div id="logo-floater">
                    <div id="branding">
                        <strong><a href="/">
                            <img src="img/logo.png" alt="Transparencia Aragón " title="Transparencia Aragón" id="logo"/>
                        </a></strong>
                    </div>
                    <div class="info_site"><span>Transparencia Aragón</span></div>

                </div>
            </div>
        </div>

        <div id="wrapper">

            <div id="container" class="clearfix">

                <br />
                <p class="titular-medio-granate">
                    <strong>Departamento:</strong>
                    <asp:Label ID="lblDepartamento" runat="server" Text=""></asp:Label>
                </p>
                <br />

                <table id="objetivos" class="display" cellspacing="0" width="100%">
                    <thead>
                        <tr>
                            <th>Objetivo estrat&eacute;gico</th>
                            <th>Avance</th>
                            <th style="width: 50px">Detalle</th>
                        </tr>
                    </thead>
                    <tbody class="tbody">
                    </tbody>
                </table>
            </div>
        </div>

        <div class="modal fade" id="myModalDetalle" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">

            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>

                        <h5><strong>Departamento:</strong>
                            <asp:Label ID="lblDpto" runat="server" Text=""></asp:Label></h5>
                        <br />
                        <h5></h5>
                        <div id="ObjEdit">
                            <h5><strong>Objetivo: </strong><span></span></h5>
                        </div>
                    </div>
                    <div class="modal-body" style="overflow-x:auto;" >
                        <table id="acciones" class="display" cellspacing="0" width="100%">
                            <thead>
                                <tr>
                                    <th>Instrumentos y actividades</th>
                                    <th style="width: 50px">Detalle</th>
                                </tr>
                            </thead>
                            <tbody class="tbody">
                            </tbody>
                        </table>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
