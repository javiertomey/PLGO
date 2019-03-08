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
using Microsoft.AspNet.Identity;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mail;

namespace PLGO
{
    public class Notificaciones
    {
        public static void SendEmail(string to, string subject, string body)
        {

            System.Net.Configuration.SmtpSection smtp = new System.Net.Configuration.SmtpSection();
        var section = ConfigurationManager.GetSection("system.net/mailSettings/smtp") as SmtpSection;

        //Aunque lo marca como obsoleto, no funciona bien la alternativa (System.Net.Mail.MailMessage), en ese caso da el siguiente error:
        //El servidor ha cometido una infracción de protocolo La respuesta del servidor fue: UGFzc3dvcmQ6
        //Más info: https://tutel.me/c/programming/questions/35148760/c+smtp+authentication+failed+but+credentials+are+correct
        System.Web.Mail.MailMessage msg = new System.Web.Mail.MailMessage();
        msg.Body = body;

            string smtpServer = section.Network.Host;
        string userName = section.Network.UserName;
        string password = section.Network.Password;
        int cdoBasic = 1;
        int cdoSendUsingPort = 2;
            if (userName.Length > 0)
            {
                msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserver", smtpServer);
                msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", section.Network.Port);
                msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusing", cdoSendUsingPort);
                msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", cdoBasic);
                msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", userName);
                msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", password);
            }
    msg.To = to;
            msg.From = section.From;
            msg.Subject = subject;
            msg.BodyFormat = MailFormat.Html;//System.Text.Encoding.UTF8;
            SmtpMail.SmtpServer = smtpServer;

            try
            {
                SmtpMail.Send(msg);
            }
            catch (Exception ex)
            {
                Logger logger = NLoggerManager.Instance;
                logger.Error("Error al enviar correo. Error: " + ex.Message + " " + ex.InnerException);
                Notificaciones.NotifySystemOps(ex);
                throw ex;//devolvemos la excepción para controlar que no se ha enviado y que la pagina origen lo tenga en cuenta
            }
        }

        public static void NotifySystemOps(Exception ex)
        {
            try
            {
                var stringBuilder = new StringBuilder();
                while (ex != null)
                {
                    stringBuilder.AppendLine(ex.Message);
                    if (ex.InnerException != null)
                        stringBuilder.AppendLine(ex.InnerException.ToString());
                    stringBuilder.AppendLine(ex.StackTrace);
                    ex = ex.InnerException;
                }

                SendEmail(WebConfigurationManager.AppSettings["EmailAlertaErrores"], WebConfigurationManager.AppSettings["NombreCortoApp"] + " - " 
                    + WebConfigurationManager.AppSettings["Entorno"]  + " - Log ",  stringBuilder.ToString());
            }
            catch 
            {

            }
        }

        public static void NotifySystemOps(Exception ex, String sTextoDescriptivo)
        {
            try
            {
                var stringBuilder = new StringBuilder();
                while (ex != null)
                {
                    stringBuilder.AppendLine(ex.Message);
                    if (ex.InnerException != null)
                        stringBuilder.AppendLine(ex.InnerException.ToString());
                    stringBuilder.AppendLine(ex.StackTrace);
                    ex = ex.InnerException;
                }

                SendEmail(WebConfigurationManager.AppSettings["EmailAlertaErrores"], WebConfigurationManager.AppSettings["NombreCortoApp"] + " - "
                    + WebConfigurationManager.AppSettings["Entorno"] + " - Log ", stringBuilder.ToString() + " " + sTextoDescriptivo);
            }
            catch
            {

            }
        }
    }
}
