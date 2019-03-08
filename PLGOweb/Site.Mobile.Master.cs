// Autor: Javier Tomey. Gobierno de Arag�n
// 
// �ste programa es software libre: usted tiene derecho a redistribuirlo y/o
// modificarlo bajo los t�rminos de la Licencia EUPL European Public License 
// publicada por el organismo IDABC de la Comisi�n Europea, en su versi�n 1.2.
// o posteriores.
// 
// �ste programa se distribuye de buena fe, pero SIN NINGUNA GARANT�A, incluso sin 
// las presuntas garant�as impl�citas de USABILIDAD o ADECUACI�N A PROP�SITO CONCRETO. 
// Para mas informaci�n consulte la Licencia EUPL European Public License.
// 
// Usted recibe una copia de la Licencia EUPL European Public License 
// junto con este programa, si por alg�n motivo no le es posible visualizarla, 
// puede consultarla en la siguiente URL: https://eupl.eu/1.2/es/
// 
// You should have received a copy of the EUPL European Public 
// License along with this program.  If not, see https://eupl.eu/1.2/en/
// 
// Vous devez avoir re�u une copie de la EUPL European Public
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

namespace PLGO
{
    public partial class Site_Mobile : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var AlternateView = "Desktop";
            var switchViewRouteName = "AspNet.FriendlyUrls.SwitchView";
            var url = GetRouteUrl(switchViewRouteName, new { view = AlternateView, __FriendlyUrls_SwitchViews = true });
            url += "?ReturnUrl=" + HttpUtility.UrlEncode(Request.RawUrl);
            Response.Redirect(url);
        }
    }
}