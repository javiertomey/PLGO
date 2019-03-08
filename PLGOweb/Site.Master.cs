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
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using PLGOdata;
using NLog;

namespace PLGO
{
    public partial class SiteMaster : MasterPage
    {
        Logger logger = LogManager.GetCurrentClassLogger();
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;

        protected void Page_Init(object sender, EventArgs e)
        {
            // El código siguiente ayuda a proteger frente a ataques XSRF
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Utilizar el token Anti-XSRF de la cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generar un nuevo token Anti-XSRF y guardarlo en la cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Establecer token Anti-XSRF
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            else
            {
                // Validar el token Anti-XSRF
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Error de validación del token Anti-XSRF.");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }




        
        public void  Manual(object sender, EventArgs e)
        {
            try
            {
                using (Entities c = new Entities())
                {                    
                    APP_ARCHIVOS_GENERALES aag = c.APP_ARCHIVOS_GENERALES.Find(1);

                    if (aag != null)
                    {
                        byte[] filedata = aag.MANUAL_USUARIO;
                        string filename = aag.NOMBRE_MANUAL_USUARIO;

                        if (filename == null || filedata == null)
                        {
                            logger.Error(this.GetType().FullName + ". Error 1: no se ha podido encontrar el manual en la BBDD (reg 1 de APP_ARCHIVOS_GENERALES)");                            
                        }
                        else
                        {
                            string contentType = MimeMapping.GetMimeMapping(filename);
                            var cd = new System.Net.Mime.ContentDisposition
                            {
                                FileName = filename,
                                Inline = false,
                            };

                            Response.AppendHeader("Content-Disposition", cd.ToString());
                            Response.ContentType = MimeMapping.GetMimeMapping(filename);
                            Response.AddHeader("content-length", aag.MANUAL_USUARIO.Length.ToString());
                            Response.BinaryWrite(aag.MANUAL_USUARIO);
                        }

                    }
                    else
                    {
                        logger.Error(this.GetType().FullName + ". Error 2: no se ha podido encontrar el manual en la BBDD (reg 1 de APP_ARCHIVOS_GENERALES)");                        
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(this.GetType().FullName + ". Error: " + ex.Message + " " + ex.InnerException);
                Notificaciones.NotifySystemOps(ex);               
            }

        }

        public void ManualAdministrador(object sender, EventArgs e)
        {
            try
            {
                using (Entities c = new Entities())
                {
                    APP_ARCHIVOS_GENERALES aag = c.APP_ARCHIVOS_GENERALES.Find(1);

                    if (aag != null)
                    {
                        byte[] filedata = aag.MANUAL_ADMIN;
                        string filename = aag.NOMBRE_MANUAL_ADMIN;

                        if (filename == null || filedata == null)
                        {
                            logger.Error(this.GetType().FullName + ". Error 1: no se ha podido encontrar el manual de admin en la BBDD (reg 1 de APP_ARCHIVOS_GENERALES)");
                        }
                        else
                        {
                            string contentType = MimeMapping.GetMimeMapping(filename);
                            var cd = new System.Net.Mime.ContentDisposition
                            {
                                FileName = filename,
                                Inline = false,
                            };

                            Response.AppendHeader("Content-Disposition", cd.ToString());
                            Response.ContentType = MimeMapping.GetMimeMapping(filename);
                            Response.AddHeader("content-length", aag.MANUAL_ADMIN.Length.ToString());
                            Response.BinaryWrite(aag.MANUAL_ADMIN);
                        }

                    }
                    else
                    {
                        logger.Error(this.GetType().FullName + ". Error 2: no se ha podido encontrar el manual de admin en la BBDD (reg 1 de APP_ARCHIVOS_GENERALES)");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(this.GetType().FullName + ". Error: " + ex.Message + " " + ex.InnerException);
                Notificaciones.NotifySystemOps(ex);
            }

        }


    }

}