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
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;
using Newtonsoft.Json;
using PLGO;
using PLGOdata;
using PLGO.Models;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.IO;
using System.Configuration;
using System.Web.Configuration;
using NLog;
using System.Data.Entity;

public partial class AdminUsuariosDep : System.Web.UI.Page
{
    //Logger logger = NLoggerManager.Instance;
    Logger logger = LogManager.GetCurrentClassLogger();

    protected string SuccessMessage
    {
        get;
        private set;
    }

    protected string WarningMessage
    {
        get;
        private set;
    }
        
    protected string ErrorMessage
    {
        get;
        private set;
    }

    protected string NombreDepartamento
    {
        get;
        private set;
    }

    protected void Page_Load()
    {

        if (!(User.IsInRole(AspNetRoles.ADMINISTRADOR)) && !(User.IsInRole(AspNetRoles.VALIDADOR)))            
        {
            //Intenta acceder un usuario sin permisos
            logger.Fatal("No autorizadoError en AdmPlan.aspx. Comprobar si el directorio está bien configurado. Usuario: " + User.Identity.GetUserId());
            Response.Redirect("../Default.aspx", true);
        }

        if (Request["id"] == null)
            Response.Redirect("../Default.aspx", false);
        

       
            using (Entities c = new Entities())
            {
                try
                {
                    DEPARTAMENTO dep = c.DEPARTAMENTO.Find(Convert.ToInt32(Request["id"]));
                    NombreDepartamento = dep.DESCRIPCION;
                }
                catch { }
            }

        if (!IsPostBack)
        {
            if (!String.IsNullOrEmpty(Request["e"]))
            {
                ErrorMessage = Request["e"];
                errorMessage.Visible = true;
            }
            else if (!String.IsNullOrEmpty(Request["m"]))
            {
                SuccessMessage = Request["m"];
                successMessage.Visible = true;
            }
            else
            {
                ErrorMessage = "";
                errorMessage.Visible = false;
                SuccessMessage = "";
                successMessage.Visible = false;
            }

        }



    }


    private void AddErrors(IdentityResult result)
    {
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error);
        }
    }


    protected void LimpiaUsuarioDep()
    {
        this.txtApellidosUsuarioDep.Text = "";
        this.txtCorreoUsuarioDep.Text = "";
        txtNombreUsuarioDep.Text = "";
        this.hdUserDepId.Value = "";
    }

    protected void btnAddUserDep_Click(object sender, EventArgs e)
    {
        if (ModelState.IsValid)
        {
            if (this.txtCorreoUsuarioDep.Text.ToLower().EndsWith("aragon.es"))
            {

                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();
                ApplicationUser user = null;
                USUARIOS usuario = null;

                //Comprobamos si existe el usuario sin departamento asociado:
                user = manager.FindByEmail(this.txtCorreoUsuarioDep.Text.Trim().ToLower());

                if (user != null)
                {//Existe el usuario de la aplicación
                    //Comprobamos si es administrador o no
                    bool admin = manager.GetRoles(user.Id).Contains("Administrador");

                    if (!admin)
                    {
                        //Comprobamos si existe el usuario en la tabla USUARIOS:
                        using (Entities c = new Entities())
                        {
                            usuario = c.USUARIOS.Find(user.Id);
                            if (usuario != null)
                            {
                                //Existe el usuario, simplemente le cambiamos el departamento asociado

                                usuario.NOMBRE = this.txtNombreUsuarioDep.Text.Trim();
                                usuario.DEPARTAMENTOID = Convert.ToInt32(Request["id"]);
                                usuario.APELLIDOS = this.txtApellidosUsuarioDep.Text.Trim();

                                c.USUARIOS.Attach(usuario);
                                c.Entry(usuario).State = EntityState.Modified;
                                c.SaveChanges();

                                SuccessMessage = "El usuario existente se ha vinculado al departamento correctamente.";
                                successMessage.Visible = true;
                                logger.Info("Actualizado usuario: " + user.Id + " dpto: " + Convert.ToInt32(Request["id"]));


                            }
                            else
                            {
                                //No existe el usuario de la aplicación, lo creamos
                               
                                USUARIOS u = new USUARIOS
                                {
                                    NOMBRE = this.txtNombreUsuarioDep.Text.Trim(),
                                    APELLIDOS = this.txtApellidosUsuarioDep.Text.Trim(),
                                    DEPARTAMENTOID = Convert.ToInt32(Request["id"]),
                                    FECHA_ALTA = DateTime.Now,
                                    AspNetUsers = c.AspNetUsers.Find(user.Id)
                                };
                                c.USUARIOS.Add(u);
                                c.SaveChanges();
                                SuccessMessage = "Nuevo usuario creado correctamente";
                                successMessage.Visible = true;
                                errorMessage.Visible = false;
                                LimpiaUsuarioDep();
                                logger.Info("Añadido usuario: " + user.Id + " dpto: " + Convert.ToInt32(Request["id"]));
                            }
                            
                        }
                    }
                    else
                    {
                        ErrorMessage = "Error al crear un nuevo usuario: no se puede añadir ya que es un usuario administrador existente.";
                        errorMessage.Visible = true;
                    }
                }
                else
                {

                    //No existe ni usuario de aplicación ni info en USUARIOS, creamos AppUser y USUARIO
                    user = new ApplicationUser() { UserName = this.txtCorreoUsuarioDep.Text.Trim().ToLower(), Email = this.txtCorreoUsuarioDep.Text.Trim(), EmailConfirmed = true };
                    IdentityResult result = null;
                    try
                    {
                        result = manager.Create(user);

                        if (result.Succeeded)
                        {
                            result = manager.AddToRole(user.Id, "Editor");

                            using (Entities c = new Entities())
                            {
                                USUARIOS u = new USUARIOS
                                {
                                    NOMBRE = this.txtNombreUsuarioDep.Text.Trim(),
                                    APELLIDOS = this.txtApellidosUsuarioDep.Text.Trim(),
                                    DEPARTAMENTOID = Convert.ToInt32(Request["id"]),
                                    FECHA_ALTA = DateTime.Now,
                                    AspNetUsers = c.AspNetUsers.Find(user.Id)
                                };
                                c.USUARIOS.Add(u);
                                c.SaveChanges();
                                SuccessMessage = "Nuevo usuario creado correctamente";
                                successMessage.Visible = true;
                                errorMessage.Visible = false;
                                LimpiaUsuarioDep();
                                logger.Info("Añadido usuario: " + user.Id + " dpto: " + Convert.ToInt32(Request["id"]));
                            }
                        }
                        else
                        {
                            ErrorMessage = "Error al crear un nuevo usuario: " + result.Errors.FirstOrDefault();
                            errorMessage.Visible = true;
                        }

                    }
                    catch (Exception ex)
                    {
                        ErrorMessage = "Error al crear un nuevo usuario: " + ex.Message + " " + ex.InnerException;
                        errorMessage.Visible = true;
                        logger.Error("Error al crear un nuevo usuario. Error: " + ex.Message + " " + ex.InnerException);
                        Notificaciones.NotifySystemOps(ex);
                    }
                }
            }
            else
            {
                ErrorMessage = "Sólo se permiten cuentas de correo electrónico corporativas (*@aragon.es)";
                errorMessage.Visible = true;
            }
        }
        else
        {
            ErrorMessage = "Se deben rellenar los campos del formulario.";
            errorMessage.Visible = true;
        }
    }

    protected void btnEliminaUserDep_Click(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(Request["__EVENTARGUMENT"]))
        {
            using (Entities c = new Entities())
            {
                try
                {
                    string id = (Request["__EVENTARGUMENT"]);

                    USUARIOS usuario = c.USUARIOS.Find(id);

                    usuario.DEPARTAMENTOID = null;
                    c.USUARIOS.Attach(usuario);
                    c.Entry(usuario).State = EntityState.Modified;
                    c.SaveChanges();

                    SuccessMessage = "Usuario eliminado correctamente.";
                    successMessage.Visible = true;
                    logger.Info("Usuario: " + usuario.USUARIOID + " desasignado del dpto: " + Convert.ToInt32(Request["id"]));

                    //No eliminamos el usuario porque tendrá seguramente información asociada de contenido (autor...), le desasignamos el departamento

                    /*c.USUARIOS.Remove(u);
                    AspNetUsers anu = c.AspNetUsers.Find(id);
                    c.AspNetUsers.Remove(anu);
                    c.SaveChanges();                    
                    Response.Redirect("AdmUsuariosDep?id=" + Request["id"] + "&m=" + "Usuario eliminado correctamente.", false);*/
                }
                catch (Exception ex)
                {
                    ErrorMessage = "Error al eliminar el usuario.";
                    errorMessage.Visible = true;
                    logger.Error("ERROR: AdmnUsuariosDep.aspx Error al eliminar usuario " + Request["__EVENTARGUMENT"] + ". Error: " + ex.Message + " " + ex.InnerException);
                    Notificaciones.NotifySystemOps(ex);
                }

            }
        }
    }

    protected void btnActualizaUserDep_Click(object sender, EventArgs e)
    {
        if (ModelState.IsValid)
        {
            if (this.txtCorreoUsuarioDep.Text.ToLower().EndsWith("aragon.es"))
            {

                string sUserId = this.hdUserDepId.Value;
                using (Entities c = new Entities())
                {
                    try
                    {
                        USUARIOS usr = c.USUARIOS.Find(sUserId);
                        AspNetUsers anusr = c.AspNetUsers.Find(sUserId);

                        usr.NOMBRE = this.txtNombreUsuarioDep.Text;
                        usr.APELLIDOS = this.txtApellidosUsuarioDep.Text;
                        anusr.Email = this.txtCorreoUsuarioDep.Text;
                        anusr.UserName = this.txtCorreoUsuarioDep.Text;

                        c.USUARIOS.Attach(usr);
                        c.Entry(usr).State = EntityState.Modified;
                        c.AspNetUsers.Attach(anusr);
                        c.Entry(anusr).State = EntityState.Modified;
                        c.SaveChanges();

                        SuccessMessage = "Usuario actualizado correctamente.";
                        successMessage.Visible = true;
                        errorMessage.Visible = false;
                        LimpiaUsuarioDep();
                    }
                    catch (Exception ex)
                    {
                        logger.Error("Error al actualizar usuario (AdmUsuariosDep.aspx). Error: " + ex.Message + " " + ex.InnerException);
                        ErrorMessage = "Error al actualizar usuario.";
                        errorMessage.Visible = true;
                    }
                }
            }
            else
            {
                ErrorMessage = "Sólo se permiten cuentas de correo electrónico corporativas (*@aragon.es)";
                errorMessage.Visible = true;
            }


        }
        else
        {
            ErrorMessage = "Se deben rellenar los campos del formulario.";
            errorMessage.Visible = true;
        }
    }

   
}
