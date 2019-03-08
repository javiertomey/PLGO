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
using System.Web.Optimization;
using System.Web.UI;

namespace PLGO
{
    public class BundleConfig
    {
        // Para obtener más información sobre la unión, visite http://go.microsoft.com/fwlink/?LinkID=303951
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/WebFormsJs").Include(
                            "~/Scripts/WebForms/WebForms.js",
                            "~/Scripts/WebForms/WebUIValidation.js",
                            "~/Scripts/WebForms/MenuStandards.js",
                            "~/Scripts/WebForms/Focus.js",
                            "~/Scripts/WebForms/GridView.js",
                            "~/Scripts/WebForms/DetailsView.js",
                            "~/Scripts/WebForms/TreeView.js",
                            "~/Scripts/WebForms/WebParts.js"));

            // El orden es muy importante para el funcionamiento de estos archivos ya que tienen dependencias explícitas
            bundles.Add(new ScriptBundle("~/bundles/MsAjaxJs").Include(
                    "~/Scripts/WebForms/MsAjax/MicrosoftAjax.js",
                    "~/Scripts/WebForms/MsAjax/MicrosoftAjaxApplicationServices.js",
                    "~/Scripts/WebForms/MsAjax/MicrosoftAjaxTimer.js",
                    "~/Scripts/WebForms/MsAjax/MicrosoftAjaxWebForms.js"));

            // Use la versión de desarrollo de Modernizr para desarrollar y aprender. Luego, cuando esté listo
            // para la producción, use la herramienta de creación en http://modernizr.com para elegir solo las pruebas que necesite
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                            "~/Scripts/modernizr-*"));

            ScriptManager.ScriptResourceMapping.AddDefinition(
                "respond",
                new ScriptResourceDefinition
                {
                    Path = "~/Scripts/respond.min.js",
                    DebugPath = "~/Scripts/respond.js",
                });
        }
    }
}