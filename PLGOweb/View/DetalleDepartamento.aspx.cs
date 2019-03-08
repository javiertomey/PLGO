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
using PLGO;
using PLGOdata;
using NLog;
using System.Web.UI.HtmlControls;

namespace PLGOweb.View
{
    public partial class DetalleDepartamento : System.Web.UI.Page
    {
        Logger logger = NLoggerManager.Instance;        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                using (Entities c = new Entities())
                {
                    try {

                        if (Request["id"] == null)
                            Response.Redirect("VerDepartamentos.aspx", false);
                        else
                        {                         
                            DEPARTAMENTO d = c.DEPARTAMENTO.Find(Convert.ToInt32(Request["id"]));
                            this.lblDepartamento.Text = d.DESCRIPCION;
                            this.lblDpto.Text = d.DESCRIPCION;

                            Page.Title = "Plan de Gobierno del Departamento de "+ d.DESCRIPCION +" | Transparencia Arag&oacute;n";
                            Page.MetaKeywords = d.DESCRIPCION + ",plan de gobierno,transparencia, aragon, gobierno abierto";
                            Page.MetaDescription = "Plan de Gobierno del Departamento de " + d.DESCRIPCION + " del Gobierno de Aragón";
                        }
                    }
                    catch  (Exception ex)
                    {

                        logger.Error("Error en DetalleDepartamento.aspx. Error: " + ex.Message + " " + ex.InnerException);
                        Notificaciones.NotifySystemOps(ex);
                    }
                }
            }
        }
    }
}