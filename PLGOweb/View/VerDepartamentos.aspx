﻿<%-- 
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
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VerDepartamentos.aspx.cs" Inherits="PLGOweb.View.VerDepartamento" %>

<%@ Import Namespace="PLGOdata" %>
<%@ Import Namespace="PLGO" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="Content-Language" content="es"/>    
    <meta name="Robots" content="all"/>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta name="geo.region" content="ES-AR" />
    <meta name="geo.position" content="41.6;-0.9" />
    <meta name="ICBM" content="41.6, -0.9" />
    <meta name="DESCRIPTION" content="Plan de Gobierno de la actual Legislatura del Gobierno de Aragón"/>
    <meta name="KEYWORDS" lang="es" content="plan de gobierno,transparencia, aragon, gobierno abierto"/>
    <meta name="DC.title" content="Plan de Gobierno. Transparencia Aragón"/>    
    <meta name="DC.Subject" content="plan de gobierno,transparencia, aragon, gobierno abierto"/>
    <meta name="DC.Description" content="Plan de Gobierno de la actual Legislatura del Gobierno de Aragón"/>
    <meta name="DC.Publisher" content="Gobierno de Aragón"/>
    <title>Plan de Gobierno | Transparencia Arag&oacute;n</title>
    <link rel="shortcut icon" href="img/favicon.ico" type="image/vnd.microsoft.icon" />
        
    <link href="<%=ResolveClientUrl("Content/bootstrap.min.css")%>" rel="stylesheet" />
    <link href="<%=ResolveClientUrl("Content/Site.min.css")%>" rel="stylesheet" />
    <link href="<%=ResolveClientUrl("Content/frame/css_PCqb-YGjrLdFwKkgsXXwwQfrrH74oEs2k2Nj86Waazc.css")%>" rel="stylesheet" />
    <link href="<%=ResolveClientUrl("Content/DataTables/css/jquery.dataTables.min.css")%>" rel="stylesheet" />
    <link href="<%=ResolveClientUrl("Content/frame/plgo.css")%>" rel="stylesheet" />


</head>
<body>
    <form runat="server">
        <asp:ScriptManager runat="server">
            <Scripts>
                <asp:ScriptReference Path="Scripts/jquery-3.1.1.min.js" />
                <asp:ScriptReference Path="Scripts/DataTables/jquery.dataTables.min.js" />
                <asp:ScriptReference Path="Scripts/DataTables/dataTables.select.min.js" />
                <asp:ScriptReference Path="Scripts/bootstrap.min.js" />
                <asp:ScriptReference Path="Scripts/respond.min.js" />
                <asp:ScriptReference Path="Scripts/treeview/core.min.js" />
                <asp:ScriptReference Path="Scripts/treeview/tree.min.js" />
                <asp:ScriptReference Path="Scripts/utilidades.js" />
            </Scripts>
        </asp:ScriptManager>


        <div class="gadget entity-gadget gadget-texto gadget-texto-default clearfix" style="">
            <div class="content">
                <div class="field field-name-field-texto field-type-text-long field-label-hidden">
                    <div class="field-items">
                       

                                <br />

                                <%
                                    if (!IsPostBack)
                                    {
                                        using (Entities c = new Entities())
                                        {
                                            c.Configuration.ProxyCreationEnabled = false;
                                            int iLegActual = LEGISLATURA.GetActualLegislatura();
                                            var results = c.DEPARTAMENTO.Where(d => d.LEGISLATURA_ID == iLegActual && d.VISIBLE == true).ToList().OrderBy(d => d.ORDEN);

                                            foreach (DEPARTAMENTO dep in results)
                                            {

                                                Response.Write("<div class='field-item even'><p class='titular-granate' style='text-align: center;'>");

                                                //Si especificamos el parametro d cargaremos los enlaces para apuntar directamente a la página de Drupal
                                                if (Request["d"] != null)
                                                    Response.Write("<a class='apg' style='cursor: pointer;' target='_parent' href='//transparencia.aragon.es/content/plan-de-gobierno-detalle-departamento?id=" + dep.DEPARTAMENTO_ID + "'>" + dep.DESCRIPCION + "</a><br/>");
                                                else
                                                    Response.Write("<a class='apg' style='cursor: pointer;' target='_blank' href='DetalleDepartamento?id=" + dep.DEPARTAMENTO_ID + "'>" + dep.DESCRIPCION + "</a><br/>");
                                                Response.Write("</p></div>");
                                            }

                                        }
                                    }
                                %>
                        
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
