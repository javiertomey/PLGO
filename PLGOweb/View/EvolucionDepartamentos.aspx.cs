// Autor: Javier Tomey. Gobierno de Aragón
// 
// Éste programa es software libre: usted tiene derecho a redistribuirlo y/o
// modificarlo bajo los términos de la Licencia EUPL European Public License 
// publicada por el organismo IDABC de la Comisión Europea, en su versión 1.2.
// o posteriores.
// 
// Éste programa se distribuye de buena fe, pero SIN NINGUNA GARANTÍA, incluso sin 
// las presuntas garantías implícitas de USABILIDAD o ADECUACIÓN A PROPÓSITO CONCRETO. 
// Para mas información consulte la Licencia EUPL European Public License.
// 
// Usted recibe una copia de la Licencia EUPL European Public License 
// junto con este programa, si por algún motivo no le es posible visualizarla, 
// puede consultarla en la siguiente URL: https://eupl.eu/1.2/es/
// 
// You should have received a copy of the EUPL European Public 
// License along with this program.  If not, see https://eupl.eu/1.2/en/
// 
// Vous devez avoir reçu une copie de la EUPL European Public
// License avec ce programme. Si non, voir https://eupl.eu/1.2/fr/
// 
// Sie sollten eine Kopie der EUPL European Public License zusammen mit
// diesem Programm. Wenn nicht, finden Sie da https://eupl.eu/1.2/de/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PLGOdata;
using System.Data.Entity;
using System.Text;

namespace PLGOweb.View
{
    public partial class EvolucionDepartamentos : System.Web.UI.Page
    {
        

        protected void Page_Load(object sender, EventArgs e)
        {            
            StringBuilder htmlText = new StringBuilder();
            if (!IsPostBack)
            {

                bool bSinMarcoEnDrupal = false;

                //Si especificamos el parametro d cargaremos los enlaces para apuntar directamente a la página de Drupal
                if (Request["d"] != null)
                    bSinMarcoEnDrupal = true;
                    

                // Check to see if the startup script is already registered.
                //if (!cs.IsStartupScriptRegistered(cstype, csname1))
                {
                    using (Entities c = new Entities())
                    {
                        c.Configuration.ProxyCreationEnabled = false;
                        int iLegActual = LEGISLATURA.GetActualLegislatura();
                        var results = c.DEPARTAMENTO.Where(d => d.LEGISLATURA_ID == iLegActual && d.VISIBLE == true).ToList().OrderBy(d => d.ORDEN);
                                               
                        
                        string sProgBarId = "";
                        string sLinkDep = "";
                        decimal decProg;


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


                        //decimal decEg = LEGISLATURA.GetObjetivosEnMarcha(;
                        //decimal decTL = LEGISLATURA.GetTiempoLegislatura();

                        htmlText.Append("<div class='cont-progress-bar-sinmarco' title='" + dPorcIndiceGlobalCumplimiento + "% Índice global de cumplimiento. " + dLegTranscurrida + "% Legislatura completada'>Índice global de cumplimiento");
                        htmlText.Append("<div class='progress'>");


                        if (dPorcIndiceGlobalCumplimiento >= dLegTranscurrida)
                        {
                            htmlText.Append("<div class='progress-bar-marca-leg-sup' style='width: " + dLegTranscurrida + "%' title='" + dPorcIndiceGlobalCumplimiento + "% Índice global de cumplimiento. " + dLegTranscurrida + "% Legislatura completada'></div>");                            
                            htmlText.Append("<div class='progress-bar progress-bar-striped active-sup' style='background-color: #a2c451; width: " + dPorcIndiceGlobalCumplimiento + "%;'>" + dPorcIndiceGlobalCumplimiento + "%</div>");
                        }
                        else
                        { 
                            htmlText.Append("<div class='progress-bar-marca-leg' style='width: " + dLegTranscurrida + "%' title='" + dLegTranscurrida + "% Legislatura completada'></div>");
                            htmlText.Append("<div class='progress-bar progress-bar-striped active' style='background-color: #a2c451; width: " + dPorcIndiceGlobalCumplimiento + "%;'>" + dPorcIndiceGlobalCumplimiento + "%</div>");
                        }
                        
                        htmlText.Append("</div></div>");

                        foreach (DEPARTAMENTO dep in results)
                        {
                            sProgBarId = "progbar" + dep.DEPARTAMENTO_ID;
                            sLinkDep = "a" + dep.DEPARTAMENTO_ID;                            

                            //DEPARTAMENTO.GetEvolucion(dep.DEPARTAMENTO_ID, out iObjetivosSinIniciar,  out iObjetivosIniciados, out iObjetivosTerminados, 
                            //    out iSumaPorcenajesAvanceAcciones, out iNumAcciones, out iNumObjetivos);

                            //decProg = LEGISLATURA.GetPorcentajeGlobalCumplimiento(iObjetivosSinIniciar, iObjetivosIniciados, iObjetivosTerminados);


                            decProg = dep.GetEvolucion();

                            htmlText.Append("<div class='cont-progress-bar' title='Plan de Gobierno del Departamento de " + dep.DESCRIPCION + ": " + decProg + "% índice global de cumplimiento. Haga clic para ver el detalle.' onclick='document.getElementById(\"" + sLinkDep +"\").click()'>");
                            if (bSinMarcoEnDrupal)                                
                                htmlText.Append("<a title='Plan de Gobierno del Departamento de " + dep.DESCRIPCION + ": " + decProg + "% índice global de cumplimiento. Haga clic para ver el detalle.' class='apg' id=" + sLinkDep + " style='cursor: pointer;' target='_parent' onclick='return handleOnclick(event);' href='//transparencia.aragon.es/content/plan-de-gobierno-detalle-departamento?id=" + dep.DEPARTAMENTO_ID + "'>" + dep.DESCRIPCION + "</a>:");
                            else
                                htmlText.Append("<a title='Plan de Gobierno del Departamento de " + dep.DESCRIPCION  +": "+ decProg + "% índice global de cumplimiento. Haga clic para ver el detalle.' class='apg' id=" + sLinkDep + " style='cursor: pointer;' target='_blank' onclick='return handleOnclick(event);'  href='DetalleDepartamento?id=" + dep.DEPARTAMENTO_ID + "'>" + dep.DESCRIPCION + "</a>:");
                            

                            htmlText.Append("<div class='progress'>");

                            if (decProg >= dLegTranscurrida)
                            { 
                                htmlText.Append("<div class='progress-bar-marca-leg-sup' style='width: " + dLegTranscurrida + "%' title='Plan de Gobierno del Departamento de " + dep.DESCRIPCION + ": " + decProg + "% índice global de cumplimiento. " + dLegTranscurrida + "% Legislatura completada. Haga clic para ver el detalle.'></div>");
                                htmlText.Append("<div class='progress-bar progress-bar-striped active-sup' title='Plan de Gobierno del Departamento de " + dep.DESCRIPCION + ": " + decProg + "% índice global de cumplimiento. Haga clic para ver el detalle.' style='width: " + decProg + "%;'>" + decProg + "%</div>");
                            }
                            else
                            {
                                htmlText.Append("<div class='progress-bar-marca-leg' style='width: " + dLegTranscurrida + "%' title='" + dLegTranscurrida + "% Legislatura completada'></div>");
                                htmlText.Append("<div class='progress-bar progress-bar-striped active' title='Plan de Gobierno del Departamento de " + dep.DESCRIPCION + ": " + decProg + "% índice global de cumplimiento. Haga clic para ver el detalle.' style='width: " + decProg + "%;'>" + decProg + "%</div>");
                            }
                            
                            htmlText.Append("</div></div>");

                        }
                        
                        
                    }                    
                    
                    this.litBarrasDep.Text = htmlText.ToString();
                }

                

            }


        }
    }
}