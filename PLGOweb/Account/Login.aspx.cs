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
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using PLGO.Models;
using PLGOdata;
using NLog;
using System.DirectoryServices.Protocols;

using System.DirectoryServices.AccountManagement;
using System.Configuration;
using System.Net;
using System.Web.Configuration;

namespace PLGO.Account
{
    public partial class Login : Page
    {
        Logger logger = NLoggerManager.Instance;

        protected void Page_Load(object sender, EventArgs e)
        {
           // ForgotPasswordHyperLink.NavigateUrl = "Forgot";
        }

        
        private SignInStatus verificaLdap(string login, string password, string dominio)
        {
            SignInStatus ret = SignInStatus.Failure;

            if ((dominio.ToLower() == "educa.aragon.es") || (dominio.ToLower() == "salud.aragon.es") || (dominio.ToLower() == "aragon.es"))
            {

                LdapConnection ldapConnection = new LdapConnection(new LdapDirectoryIdentifier(WebConfigurationManager.AppSettings["ServidorLDAP"],
                Convert.ToInt32(WebConfigurationManager.AppSettings["PuertoLDAP"])));
                try
                {
                    ldapConnection.AuthType = AuthType.Basic;
                    ldapConnection.Bind(new NetworkCredential("uid=" + login + ",ou=people,o=" + dominio + ",o=isp", password));


                    

                    ret = SignInStatus.Success;
                }
                catch (Exception ex)
                {                    
                    if ((!ex.Message.Contains("La credencial proporcionada no es válida")) && !(ex.Message.Contains("The supplied credential is invalid")))
                    {
                        logger.Fatal("VerificaLdap. Error: " + ex.Message + " " + ex.InnerException);
                        Notificaciones.NotifySystemOps(ex);
                    }
                    return SignInStatus.Failure;
                }

                return ret;
            }
            else
            {
                logger.Info("VerificaLdap. Dominio no válido: " + dominio);
                return SignInStatus.Failure;
            }

        }    

             

        protected void LogIn(object sender, EventArgs e)
        {
            if (IsValid)
            {
                // Validar la contraseña del usuario
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var signinManager = Context.GetOwinContext().GetUserManager<ApplicationSignInManager>();
                


                try
                {

                    ApplicationUser au = manager.FindByEmail(Email.Text);

                    if (au != null)
                    {
                        //var userid = manager.FindByEmail(Email.Text).Id;
                        var userid = au.Id;
                        // Esto no cuenta los errores de inicio de sesión hacia el bloqueo de cuenta
                        // Para habilitar los errores de contraseña para desencadenar el bloqueo, cambie a shouldLockout: true
                        //var result = signinManager.PasswordSignIn(Email.Text, Password.Text, RememberMe.Checked, shouldLockout: true);  //descomentar para usar autenticación por BBDD
                                                
                        string[] login = Email.Text.Split('@');
                        var result = verificaLdap(login[0], Password.Text, login[1]);

                        //Comprobamos si es administrador o no
                        bool admin = manager.GetRoles(userid).Contains("Administrador");
                        bool sSinDepartamento = false;

                        //Si no es admin comprobamos que el usuario tenga departamento, si no lo tiene no podrá entrar en la aplicación (estaría "eliminado")
                        if (!admin)
                        {
                            using (Entities c = new Entities())
                            {
                                try
                                {
                                    USUARIOS u = c.USUARIOS.Find(userid);
                                    if (!u.DEPARTAMENTOID.HasValue)
                                    {
                                        result = SignInStatus.Failure;
                                        sSinDepartamento = true;
                                        logger.Info("Usuario sin departamento asociado: " + userid);

                                    }
                                }
                                catch (Exception ex)
                                {
                                    logger.Warn("Error en AdminPlan.aspx. Error: " + ex.Message + " " + ex.InnerException);
                                    Notificaciones.NotifySystemOps(ex);
                                    Response.Redirect("../Default.aspx", true);
                                }
                            }
                        }

                        switch (result)
                        {
                            case SignInStatus.Success:
                                USUARIOS.setUltimoAcceso(userid);
                                signinManager.SignIn(au, false, RememberMe.Checked);
                                IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response, admin);
                                break;
                            case SignInStatus.LockedOut:
                                Response.Redirect("/Account/Lockout");
                                break;
                            case SignInStatus.RequiresVerification:
                                Response.Redirect(String.Format("/Account/TwoFactorAuthenticationSignIn?ReturnUrl={0}&RememberMe={1}",
                                                                Request.QueryString["ReturnUrl"],
                                                                RememberMe.Checked),
                                                    true);
                                break;
                            case SignInStatus.Failure:
                            default:
                                if (!sSinDepartamento)
                                    FailureText.Text = "Intento de inicio de sesión no válido";
                                else
                                    FailureText.Text = "Usuario sin departamento asociado";
                                ErrorMessage.Visible = true;
                                break;
                        }
                        
                    }
                    else
                    {
                        //No existe el usuario
                        FailureText.Text = "Usuario y/o contraseña no válidos";
                        ErrorMessage.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    FailureText.Text = "Usuario y/o contraseña no válidos.";
                    ErrorMessage.Visible = true;
                    logger.Fatal("Error en Login.aspx. Error: " + ex.Message + " " + ex.InnerException);
                    Notificaciones.NotifySystemOps(ex);                    
                }
            }
        }
    }
}