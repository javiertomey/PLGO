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
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace PLGO
{
    public class Global : HttpApplication
    {
        Logger logger = LogManager.GetCurrentClassLogger();
        bool bRedirigirPorHttps = false;

        protected void Application_PreSendRequestHeaders(Object source, EventArgs e)
        {
            //https://sonarwhal.com/docs/user-guide/rules/x-content-type-options/
            HttpContext.Current.Request.Headers.Add("X-Content-Type-Options", "nosniff");
        }

        /*
        *  HSTS by returning the "Strict-Transport-Security" header to the browser. The browser has to support this (and at present, it's primarily Chrome and Firefox that do), 
        *  but it means that once set, the browser won't make requests to the site over HTTP and will instead translate them to HTTPS requests before issuing them. 
        *  Try this in combination with a redirect from HTTP:
        *  
        *  Browsers that aren't HSTS aware will just ignore the header but will still get caught by the switch statement and sent over to HTTPS.
        * 
       https://stackoverflow.com/questions/47089/best-way-in-asp-net-to-force-https-for-an-entire-site*/
        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            if (bRedirigirPorHttps)
            {
                //PIDEN DESDE COMUNICACIONES QUITAR ESTA REDIRECCIÓN PARA EVITAR PROBLEMAS EN LOS APACHES
                if ((!Request.IsLocal) && (!Request.FilePath.StartsWith("/View/")))
                {   //NO para localhost y no para la parte pública (/View/*)
                    switch (Request.Url.Scheme)
                    {
                        case "https":
                            Response.AddHeader("Strict-Transport-Security", "max-age=300");
                            break;
                        case "http":
                            var path = "https://" + Request.Url.Host + Request.Url.PathAndQuery;
                            Response.Status = "301 Moved Permanently";
                            Response.AddHeader("Location", path);
                            break;
                    }
                }
            }
        }

        void Application_Start(object sender, EventArgs e)
        {
            logger.Info("Iniciando aplicacion PLGO");

            try
            {
                bRedirigirPorHttps = Convert.ToBoolean(WebConfigurationManager.AppSettings["redirigirPorHttps"]);
            }
            catch { };

            // Código que se ejecuta al iniciar la aplicación
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            SqlServerTypes.Utilities.LoadNativeAssemblies(Server.MapPath("~/bin"));
        }


        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

            // Get the exception object.
            Exception exc = Server.GetLastError();

            // Handle HTTP errors
            if (exc.GetType() == typeof(HttpException))
            {
                // The Complete Error Handling Example generates
                // some errors using URLs with "NoCatch" in them;
                // ignore these here to simulate what would happen
                // if a global.asax handler were not implemented.
                if (exc.Message.Contains("NoCatch") || exc.Message.Contains("maxUrlLength"))
                    return;

                //Redirect HTTP errors to HttpError page
                Server.Transfer("Default.aspx");
            }

            logger.Fatal("Excepcion (Global.asax). Error: " + exc.Message + " " + exc.InnerException);
            Notificaciones.NotifySystemOps(exc);

            // Clear the error from the server
            Server.ClearError();
        }
    }
}