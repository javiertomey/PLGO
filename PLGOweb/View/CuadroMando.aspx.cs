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
    public partial class CuadroMando : System.Web.UI.Page
    {       
        
        protected void Page_Load(object sender, EventArgs e)
        {
            StringBuilder cstext1 = new StringBuilder();
            StringBuilder htmlText = new StringBuilder();
            ClientScriptManager cs = Page.ClientScript;            
            Type cstype = this.GetType();

            if (!IsPostBack)
            {
                
                {

                    /*
                  decimal decEg = LEGISLATURA.GetEvolucionGeneral();
                  decimal decTL = LEGISLATURA.GetTiempoLegislatura();*/

                    //  //htmlText.Append("Evoluci&oacute;n general:<div class='progress' ><div class='progress-bar progress-bar-striped active' style='width:" + decEg + "%;'>" + decEg + "%</div></div>");
                    //  htmlText.Append("Evoluci&oacute;n general:<div class='progress' ><div id='pbEG' class='progress-bar progress-bar-striped active' style='background-color: #1179a1; width:" + 0 + "%;'>" + decEg + "%</div></div>");

                  
                  //  //htmlText.Append("Avance Legislatura:<div class='progress' ><div class='progress-bar progress-bar-striped active' style='width:" + decTL + "%;'>" + decTL + "%</div></div>");
                  //  htmlText.Append("Progreso Legislatura:<div class='progress' ><div id='pbTL' class='progress-bar progress-bar-striped active' style='background-color: #acc45b;width:" + 0 + "%;'>" + decTL + "%</div></div>");

                  //  this.litBarrasEG.Text = htmlText.ToString();       
                    
                                 
                }
            }
        }
    }
}