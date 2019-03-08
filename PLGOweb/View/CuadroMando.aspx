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
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CuadroMando.aspx.cs" Inherits="PLGOweb.View.CuadroMando" %>

<%@ Import Namespace="PLGOdata" %>
<%@ Import Namespace="PLGO" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="Content-Language" content="es" />
    <meta name="Robots" content="all" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta name="geo.region" content="ES-AR" />
    <meta name="geo.position" content="41.6;-0.9" />
    <meta name="ICBM" content="41.6, -0.9" />
    <meta name="DESCRIPTION" content="Plan de Gobierno de la actual Legislatura del Gobierno de Aragón" />
    <meta name="KEYWORDS" lang="es" content="plan de gobierno,transparencia, aragon, gobierno abierto" />
    <meta name="DC.title" content="Evolución General. Plan de Gobierno. Transparencia Aragón" />
    <meta name="DC.Subject" content="plan de gobierno,transparencia, aragon, gobierno abierto" />
    <meta name="DC.Description" content="Plan de Gobierno de la actual Legislatura del Gobierno de Aragón" />
    <meta name="DC.Publisher" content="Gobierno de Aragón" />
    <title>Evolución General del Plan de Gobierno | Transparencia Arag&oacute;n</title>
    <link rel="shortcut icon" href="img/favicon.ico" type="image/vnd.microsoft.icon" />
    <link href="<%=ResolveClientUrl("Content/bootstrap.min.css")%>" rel="stylesheet" />
    <link href="<%=ResolveClientUrl("Content/Site.min.css")%>" rel="stylesheet" />    
    <link href="<%=ResolveClientUrl("Content/frame/plgo.css?v2")%>" rel="stylesheet" />
    <link href="<%=ResolveClientUrl("Content/c3.min.css")%>" rel="stylesheet" />
    <link href="<%=ResolveClientUrl("Content/c3plgo.css?v2")%>" rel="stylesheet" />
    <style type="text/css">
        .c3-arc-Índice-global-de-cumplimiento {
             fill: #a2c451 !important;            
        }
        .c3-tooltip-name--Índice-global-de-cumplimiento td.name span{
            background-color: #a2c451 !important;            
        }
         .c3-legend-item-Índice-global-de-cumplimiento line {
             stroke: #a2c451 !important;            
        }
       .c3-arc-Objetivos-en-consecución {
            fill: #1179a1 !important;            
        }
         .c3-tooltip-name--Objetivos-en-consecución td.name span{
            background-color: #1179a1 !important;            
        }
         .c3-legend-item-Objetivos-en-consecución line {
            stroke: #1179a1 !important;            
        }
    </style>
</head>
<body>
    <form runat="server">
        <asp:ScriptManager runat="server">
            <Scripts>
                <asp:ScriptReference Path="Scripts/jquery-3.1.1.min.js" />
                <asp:ScriptReference Path="Scripts/bootstrap.min.js" />
                <asp:ScriptReference Path="Scripts/respond.min.js" />
                <asp:ScriptReference Path="Scripts/utilidades.js" />
                <asp:ScriptReference Path="Scripts/loadingoverlay.min.js" />
                <asp:ScriptReference Path="Scripts/d3.min.js" />
                <asp:ScriptReference Path="Scripts/c3.min.js" />
            </Scripts>
        </asp:ScriptManager>
        <script type="text/javascript" charset="utf-8">

                     

            $.LoadingOverlay("show");
            <%           
          

            decimal dLegTranscurrida;
            int iObjetivosSinIniciar = 0;
            int iObjetivosIniciados = 0;
            int iObjetivosTerminados = 0;
            int iNumAcciones = 0;
            int iNumObjetivos = 0;
            int iSumaPorcenajesAvanceAcciones = 0;
            decimal dPorcSinIniciar = 0;
            decimal dPorcIniciados = 0;
            decimal dPorcTerminados = 0;
            decimal dPorcIndiceGlobalCumplimiento = 0;

            LEGISLATURA.GetDatosCuadroMando(out dLegTranscurrida, out iObjetivosSinIniciar, out iObjetivosIniciados, out iObjetivosTerminados,
                    out iSumaPorcenajesAvanceAcciones, out iNumAcciones, out iNumObjetivos,
                    out dPorcSinIniciar, out dPorcIniciados, out dPorcTerminados, out dPorcIndiceGlobalCumplimiento);   
          
            %>
            LegTranscurrida =   <%= dLegTranscurrida %>;
            LegRestante =   100 - LegTranscurrida;
            ObjetivosEnMarcha = <%= dPorcIndiceGlobalCumplimiento %>;
            ObjetivosRestante = 100 - ObjetivosEnMarcha;
            timeout = 100;  
                        
            $(document).ready(function () {                
                var chart = c3.generate({
                    bindto: '#chartLT',
                    size: {
                        height: 280,
                        width: 280,
                    },
                    data: {
                        columns: [                                                   
                        ],
                        type: 'donut',                                                
                        hide: true,
                    },
                    tooltip: {
                        format: {
                            value: function (value, ratio, id, index) { return value + "%"; }
                        }
                    },
                    donut: {
                        title: LegTranscurrida + "%",
                        width: 32,
                        label: {
                            show: false
                        },
                    },
                    legend: {
                        show: false
                    },
                    expand: false,
                });

                function addColumn(data, delay, chart) {
                    var dataTmp = [data[0], 0];
                    setTimeout(function () {
                        chart.internal.d3.transition().duration(100);
                        chart.load({
                            columns: [
                              dataTmp
                            ]
                        });
                    }, timeout);
                    timeout += 200;
                    data.forEach(function (value, index) {
                        setTimeout(function () {
                            dataTmp[index] = value;
                            if (index < 10) dataTmp.push(0);
                            chart.load({
                                columns: [
                                    dataTmp
                                ],
                                length: 0
                            });
                        }, (timeout + (delay / data.length * index)));
                    });
                    timeout += delay;
                }
                setTimeout(function () {
                    chart.axis.range({
                        min: {
                            x: 0,
                            y: 1
                        },
                        max: {
                            x: 6,
                            y: 10
                        }
                    });
                }, timeout);
                timeout += 500;

                addColumn(['Legislatura transcurrida', LegTranscurrida], 0, chart);
                addColumn(['Legislatura restante', LegRestante], 20, chart);             


                var chartOM = c3.generate({
                    bindto: '#chartOM',
                    size: {
                        height: 280,
                        width: 280,
                    },
                    data: {
                        columns: [                        
                        ],
                        type: 'donut',                        
                    },
                    tooltip: {
                        format: {
                            value: function (value, ratio, id, index) { return value + "%"; }
                        }
                    },
                    donut: {
                        title: ObjetivosEnMarcha+ "%",
                        width: 32,
                        label: {
                            show: false
                        },
                    },
                    legend: {
                        show: false
                    },
                    expand: false,
                });
                addColumn(['Índice global de cumplimiento', ObjetivosEnMarcha], 0, chartOM);
                addColumn(['Objetivos pendientes de iniciar', ObjetivosRestante], 20, chartOM);

                var chartOE = c3.generate({
                    bindto: '#chartOE',
                    data: {
                        columns: [
                        ],
                        type: 'donut',                        
                    },                   
                    tooltip: {
                        format: {
                            value: function (value, ratio, id, index) { return value + "%"; }
                        }
                    },
                    donut: {
                        title: "",
                        width: 48,
                        label: {
                            show: true,
                            format: function (value, ratio, id) {
                                return value + "%";
                            },
                        },                        
                       
                    },
                    legend: {
                        show: true,                        
                    },
                    expand: false,
                });
                addColumn(['Pendiente de iniciar', <%= dPorcSinIniciar%>], 0, chartOE);
                addColumn(['Objetivos conseguidos', <%= dPorcTerminados%>], 0, chartOE);
                addColumn(['Objetivos en consecución', <%=dPorcIniciados%>], 0, chartOE);

                document.getElementById('descLT').setAttribute('title', LegTranscurrida + '% de Legislatura transcurrida');
                document.getElementById('chartLT').setAttribute('aria-labelledby', LegTranscurrida + '% de Legislatura transcurrida');
                document.getElementById('descOM').setAttribute('title', ObjetivosEnMarcha + '% Índice global de cumplimiento');
                document.getElementById('chartOM').setAttribute('aria-labelledby', ObjetivosEnMarcha + '% Índice global de cumplimiento');
                document.getElementById('descOE').setAttribute('title', <%= dPorcSinIniciar%> + '% de Objetivos sin iniciar. ' + <%=dPorcIniciados%> + '% de Objetivos en consecución. ' + <%= dPorcTerminados%> + '% de Objetivos conseguidos' );
                document.getElementById('chartOE').setAttribute('aria-labelledby', <%= dPorcSinIniciar%> + '% de Objetivos sin iniciar. ' + <%=dPorcIniciados%> + '% de Objetivos en consecución. ' + <%= dPorcTerminados%> + '% de Objetivos conseguidos' );

                $.LoadingOverlay("hide", true);
            });
        </script>

        <div class="container">
            <div class="row">
                <div class="col-md-6 text-center">
                    <span id="descLT" style="font-size: 2.4rem; color: #72ADC0;">Legislatura transcurrida</span>
                    <div style="text-align: center;" class="chart" id="chartLT"></div>
                </div>
                <div class="col-md-6 text-center">
                    <span id="descOM" style="font-size: 2.4rem; color: #72ADC0;">Índice global de cumplimiento</span>
                    <div style="text-align: center;" class="chart" id="chartOM"></div>
                </div>
            </div>
            <div class="row justify-content-center">
                <div class="col text-center">
                    <span id="descOE" style="font-size: 2.4rem; color: #72ADC0;">Objetivos estrat&eacute;gicos</span>
                    <div style="text-align: center;" class="chart" id="chartOE"></div>
                </div>
            </div>
        </div>
		<script type="text/javascript" src="Scripts/iframeResizer.contentWindow.min.js" defer></script>
    </form>
</body>
</html>
