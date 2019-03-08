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
using System.Web.UI.WebControls;
using System.Web.Services;
using System.IO;
using System.Configuration;
using System.Data;
using System.Web.Configuration;
using NLog;
using System.Data.Entity;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;

public partial class AdminPlanA : System.Web.UI.Page
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

    protected void Page_Load()
    {

        if (!(User.IsInRole(AspNetRoles.ADMINISTRADOR)) && !(User.IsInRole(AspNetRoles.VALIDADOR)))            
        {
            //Intenta acceder un usuario sin permisos
            logger.Fatal("No autorizadoError en AdmPlan.aspx. Comprobar si el directorio está bien configurado. Usuario: " + User.Identity.GetUserId());            
            Response.Redirect("../Default.aspx", true);
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
                WarningMessage = "";
                warningMessage.Visible = false;

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


    protected void btnAddAcc_Click(object sender, EventArgs e)
    {
        if (ModelState.IsValid)
        {
           

                int iDeptoId = Convert.ToInt32(hdDepartamentoId.Value);

                //Como lo creamos desde el validador/administrador lo creamos ya válido y visible
                using (Entities c = new Entities())
                {
                    try
                    {
                        ACCION obj = new ACCION
                        {
                            AUTOR_CREACION_USUARIO_ID = User.Identity.GetUserId().ToString(),
                            DEPARTAMENTO_ID = iDeptoId,
                            ESTADO_VALIDACION_ID = ESTADOS_VALIDACION.VALIDADO, 
                            FECHA_CREACION = DateTime.Now,                            
                            VISIBLE = true,
                            SEGUIMIENTO = txtSeguimientoPV.Text.Trim(),
                            SEGUIMIENTO_PDTE_VAL = txtSeguimientoPV.Text.Trim(),
                            RECURSOS_HUMANOS = txtRecursosHumanosPV.Text.Trim(),
                            RECURSOS_HUMANOS_PDTE_VAL = txtRecursosHumanosPV.Text.Trim(),
                            COSTE_ECONOMICO = txtCosteEconomicoPV.Text.Trim(),
                            COSTE_ECONOMICO_PDTE_VAL = txtCosteEconomicoPV.Text.Trim(),
                            INSTRUMENTOS_ACT = txtInstrumentosActPV.Text.Trim(),
                            INSTRUMENTOS_ACT_PDTE_VAL = txtInstrumentosActPV.Text.Trim(),
                            MEDIOS_OTROS = txtMediosOtrosPV.Text.Trim(),
                            MEDIOS_OTROS_PDTE_VAL = txtMediosOtrosPV.Text.Trim(),
                            TEMPORALIDAD = txtTemporalidadPV.Text.Trim(),
                            TEMPORALIDAD_PDTE_VAL = txtTemporalidadPV.Text.Trim(),
                            INDICADOR_SEGUIMIENTO = txtIndSeguimientoPV.Text.Trim(),
                            INDICADOR_SEGUIMIENTO_PDTE_VAL = txtIndSeguimientoPV.Text.Trim(),
                            /*ESTADO_SEGUIMIENTO_ID = Convert.ToInt32(this.ddlEstadoSeguimientoPV.SelectedItem.Value),
                            ESTADO_SEGUIMIENTO_ID_PDTE_VAL= Convert.ToInt32(this.ddlEstadoSeguimientoPV.SelectedItem.Value),*/
                            PORCENTAJE_AVANCE = Convert.ToByte(this.ddlPorcentajeAvancePV.SelectedItem.Value),
                            PORCENTAJE_AVANCE_PDTE_VAL = Convert.ToByte(this.ddlPorcentajeAvancePV.SelectedItem.Value),
                            ORGANO_RESPONSABLE = txtOrganoResponsablePV.Text.Trim(),
                            ORGANO_RESPONSABLE_PDTE_VAL = txtOrganoResponsablePV.Text.Trim(),
                            TIPO_CAMBIO_CONTENIDO_ID = TIPO_CAMBIO_CONTENIDO.SIN_CAMBIOS,
                            OBJETIVO_CONTENIDO_ID = Convert.ToInt32(this.hdObjetivoIdEdit.Value)
                        };

                        c.CONTENIDO.Add(obj);

                        c.SaveChanges();
                        SuccessMessage = "Instrumento/actividad guardado correctamente.";
                        successMessage.Visible = true;
                    }
                    catch (Exception ex)
                    {
                        logger.Error("Error al crear nueva acción (AdminPlan.aspx). Error: " + ex.Message + " " + ex.InnerException);
                        Notificaciones.NotifySystemOps(ex);
                        ErrorMessage = "Error al crear nueva acción.";
                        errorMessage.Visible = true;
                    }
                }


            }
            else
            {
                ErrorMessage = "Se deben rellenar los campos del formulario.";
                errorMessage.Visible = true;
            }
        
       

    }

    /// <summary>
    /// Añade Objetivo
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (ModelState.IsValid)
        {


            if ((!String.IsNullOrEmpty(this.ObjetivoEstrategicoPV.Text)))
            {

                //Damos de alta directamente el objetivo directamente validado ya que es el perfil del administrador

                int iDeptoId = Convert.ToInt32(hdDepartamentoId.Value);
                using (Entities c = new Entities())
                {

                    OBJETIVO obj = new OBJETIVO
                    {
                        AUTOR_CREACION_USUARIO_ID = User.Identity.GetUserId().ToString(),
                        DEPARTAMENTO_ID = iDeptoId,
                        ESTADO_VALIDACION_ID = ESTADOS_VALIDACION.VALIDADO,
                        TIPO_CAMBIO_CONTENIDO_ID = TIPO_CAMBIO_CONTENIDO.SIN_CAMBIOS,
                        FECHA_CREACION = DateTime.Now,                        
                        OBJETIVO_ESTRATEGICO = this.ObjetivoEstrategicoPV.Text.Trim(),
                        OBJETIVO_ESTRATEGICO_PDTE_VAL = this.ObjetivoEstrategicoPV.Text.Trim(),
                        VISIBLE = true
                    };

                    c.CONTENIDO.Add(obj);
                    try
                    {
                        c.SaveChanges();
                        SuccessMessage = "Objetivo guardado correctamente.";
                        successMessage.Visible = true;
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("Error al crear nuevo objetivo (AdminPlan.aspx). Error: " + ex.Message + " " + ex.InnerException);
                        Notificaciones.NotifySystemOps(ex);
                        ErrorMessage = "Error al crear nuevo objetivo.";
                        errorMessage.Visible = true;
                    }
                }


            }
            else
            {
                ErrorMessage = "Se deben rellenar los campos del formulario.";
                errorMessage.Visible = true;
            }
        }
        else
        {
            ErrorMessage = "Se deben rellenar los campos del formulario.";
            errorMessage.Visible = true;
        }

    }

    /// <summary>
    /// Actualiza Objetivo
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnActualiza_Click(object sender, EventArgs e)
    {
        if (ModelState.IsValid)
        {

            bool bAviso = false;
            OBJETIVO obj = null;

            if ((!String.IsNullOrEmpty(this.ObjetivoEstrategicoPV.Text)))
            {
                int iObjetivoId = Convert.ToInt32(this.hdObjetivoIdEdit.Value);
                using (Entities c = new Entities())
                {
                    try
                    {
                        obj = c.CONTENIDO.OfType<OBJETIVO>().Where(o => o.CONTENIDO_ID == iObjetivoId).FirstOrDefault();

                        //Si hay cambios guardamos si no no, para no marcar como modificado
                        bool bHayCambios = false;

                        if (String.IsNullOrEmpty(obj.OBJETIVO_ESTRATEGICO_PDTE_VAL))
                            bHayCambios = true;
                        else if (obj.OBJETIVO_ESTRATEGICO_PDTE_VAL.Trim() != this.ObjetivoEstrategicoPV.Text.Trim())
                        {
                            bHayCambios = true;
                        }

                        if (bHayCambios) bAviso = true;
                        if (String.IsNullOrEmpty(obj.COMENTARIO_VALIDADOR))
                        {
                            if (!String.IsNullOrEmpty(this.txtObjComentarioValidador.Text))
                                bAviso = true; //solo si hay cambios enviaremos email al usuario
                        }
                        else if (this.txtObjComentarioValidador.Text != obj.COMENTARIO_VALIDADOR)
                            bAviso = true; //solo si hay cambios enviaremos email al usuario

                        if (bHayCambios)
                        {
                            obj.FECHA_MODIFICACION = DateTime.Now;
                            //obj.AUTOR_MODIFICACION_USUARIO_ID = User.Identity.GetUserId().ToString(); //no ponemos como autor para que no afecte a las notificaciones
                            obj.TIPO_CAMBIO_CONTENIDO_ID = TIPO_CAMBIO_CONTENIDO.MODIFICACION;
                            obj.ESTADO_VALIDACION_ID = ESTADOS_VALIDACION.PDTE_VALIDAR;
                            obj.OBJETIVO_ESTRATEGICO_PDTE_VAL = this.ObjetivoEstrategicoPV.Text.Trim();
                        }

                        obj.COMENTARIO_VALIDADOR = this.txtObjComentarioValidador.Text.Trim();                        

                        c.CONTENIDO.Attach(obj);
                        c.Entry(obj).State = EntityState.Modified;
                        c.SaveChanges();

                        SuccessMessage = "Objetivo actualizado correctamente.";
                        successMessage.Visible = true;
                    }                    
                    catch (Exception ex)
                    {
                        logger.Error("Error al actualizar objetivo (AdminPlan.aspx). Error: " + ex.Message + " " + ex.InnerException);
                        Notificaciones.NotifySystemOps(ex);
                        ErrorMessage = "Error al actualizar Instrumento/actividad.";
                        errorMessage.Visible = true;
                        bAviso = false; //si hay error no se envía correo ya que seguramente no se ha actualizado nada
                    }
                }

                if (bAviso)
                {
                    using (Entities c = new Entities())
                    {
                        try
                        {
                            bool bHayComentario = !String.IsNullOrEmpty(this.txtObjComentarioValidador.Text);

                            string email = "";
                            string strComentario = "";
                            if (bHayComentario)
                                strComentario = "<br><br> Comentario del validador: <strong>" + this.txtObjComentarioValidador.Text + "</strong>";
                            string sUrlE = new Uri(Request.Url, "../Planes/AdminPlan.aspx?id=" + obj.DEPARTAMENTO_ID).AbsoluteUri.ToString();
                            string sUrl = new Uri(Request.Url, "../View/DetalleDepartamento.aspx?id=" + obj.DEPARTAMENTO_ID + "&oId=" + obj.CONTENIDO_ID).AbsoluteUri.ToString();
                            string strBody = "Aplicación " + WebConfigurationManager.AppSettings["NombreLargoApp"] + "<br/><br/> El siguiente objetivo estratégico ha sido actualizado: " + obj.OBJETIVO_ESTRATEGICO + strComentario + 
                                "<br><br> Está accesible en la siguiente dirección: " + sUrl + "<br><br> Se puede actualizar en la siguiente dirección: " + sUrlE;
                            //Si el autor de la modificación es el mismo que el usuario actual (validador...) avisamos al creador del contenido
                            if (obj.AUTOR_MODIFICACION_USUARIO_ID == User.Identity.GetUserId())
                            {
                                AspNetUsers usr = c.AspNetUsers.Find(obj.AUTOR_CREACION_USUARIO_ID);
                                email = usr.Email;
                            }
                            else
                            {
                                if (!String.IsNullOrEmpty(obj.AUTOR_MODIFICACION_USUARIO_ID))
                                {
                                    AspNetUsers usr = c.AspNetUsers.Find(obj.AUTOR_MODIFICACION_USUARIO_ID);
                                    email = usr.Email;
                                }
                                else
                                {
                                    AspNetUsers usr = c.AspNetUsers.Find(obj.AUTOR_CREACION_USUARIO_ID);
                                    email = usr.Email;
                                }
                            }
                            if (bHayComentario)
                                Notificaciones.SendEmail(email, WebConfigurationManager.AppSettings["NombreCortoApp"] + " - Objetivo actualizado con Comentario", strBody);
                            else
                                Notificaciones.SendEmail(email, WebConfigurationManager.AppSettings["NombreCortoApp"] + " - Objetivo actualizado", strBody);
                        }
                        catch (Exception ex)
                        {
                            logger.Error("Error al notificar actualización Objetivo (AdminPlan.aspx). Error: " + ex.Message + " " + ex.InnerException);
                            Notificaciones.NotifySystemOps(ex);
                            ErrorMessage = "Error al notificar la actualización del Objetivo.";
                            errorMessage.Visible = true;
                        }
                    }
                }


            }
            else
            {
                ErrorMessage = "Se deben rellenar los campos del formulario.";
                errorMessage.Visible = true;
            }
        }
        else
        {
            ErrorMessage = "Se deben rellenar los campos del formulario.";
            errorMessage.Visible = true;
        }

    }

    /// <summary>
    /// Actualiza Accion
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnActualizaAcc_Click(object sender, EventArgs e)
    {
        if (ModelState.IsValid)
        {

            bool bAviso = false;
            ACCION obj = null;
            int iObjetivoId = Convert.ToInt32(this.hdAccionIdEdit.Value);
            using (Entities c = new Entities())
            {
                try
                {
                    obj = c.CONTENIDO.OfType<ACCION>().Include("OBJETIVO").Where(o => o.CONTENIDO_ID == iObjetivoId).FirstOrDefault();


                    bool bHayCambios = false;

                    if (String.IsNullOrEmpty(obj.INSTRUMENTOS_ACT_PDTE_VAL))
                        bHayCambios = true;
                    else if (
                         obj.INSTRUMENTOS_ACT_PDTE_VAL.Trim() != this.txtInstrumentosActPV.Text.Trim() ||
                         obj.SEGUIMIENTO_PDTE_VAL.Trim() != this.txtSeguimientoPV.Text.Trim() ||
                         obj.RECURSOS_HUMANOS_PDTE_VAL.Trim() != this.txtRecursosHumanosPV.Text.Trim() ||
                         obj.COSTE_ECONOMICO_PDTE_VAL.Trim() != this.txtCosteEconomicoPV.Text.Trim() ||
                         obj.INDICADOR_SEGUIMIENTO_PDTE_VAL.Trim() != this.txtIndSeguimientoPV.Text.Trim() ||
                         obj.TEMPORALIDAD_PDTE_VAL.Trim() != this.txtTemporalidadPV.Text.Trim() ||
                         obj.INDICADOR_SEGUIMIENTO_PDTE_VAL.Trim() != this.txtIndSeguimientoPV.Text.Trim() ||
                        //obj.ESTADO_SEGUIMIENTO_ID_PDTE_VAL.ToString() != this.ddlEstadoSeguimientoPV.SelectedItem.Value ||
                        obj.PORCENTAJE_AVANCE_PDTE_VAL.ToString() != this.ddlPorcentajeAvancePV.SelectedItem.Value ||
                        obj.ORGANO_RESPONSABLE_PDTE_VAL.Trim() != this.txtOrganoResponsablePV.Text.Trim()
                        )
                    {
                        bHayCambios = true;
                    }
                    else if (!String.IsNullOrEmpty(obj.MEDIOS_OTROS_PDTE_VAL))
                    {
                        if (obj.MEDIOS_OTROS_PDTE_VAL.Trim() != this.txtMediosOtrosPV.Text.Trim())
                            bHayCambios = true;
                    }
                    else if (!String.IsNullOrEmpty(this.txtMediosOtrosPV.Text.Trim()))
                        bHayCambios = true;


                    if (bHayCambios) bAviso = true;
                    if (String.IsNullOrEmpty(obj.COMENTARIO_VALIDADOR))
                    {
                        if (!String.IsNullOrEmpty(this.txtAccComentarioValidador.Text))
                            bAviso = true; //solo si hay cambios enviaremos email al usuario
                    }
                    else if (this.txtAccComentarioValidador.Text != obj.COMENTARIO_VALIDADOR)
                        bAviso = true; //solo si hay cambios enviaremos email al usuario                    

                    obj.SEGUIMIENTO_PDTE_VAL = this.txtSeguimientoPV.Text.Trim();
                    obj.RECURSOS_HUMANOS_PDTE_VAL = this.txtRecursosHumanosPV.Text.Trim();
                    obj.COSTE_ECONOMICO_PDTE_VAL = this.txtCosteEconomicoPV.Text.Trim();
                    obj.INDICADOR_SEGUIMIENTO_PDTE_VAL = this.txtIndSeguimientoPV.Text.Trim();
                    obj.MEDIOS_OTROS_PDTE_VAL = this.txtMediosOtrosPV.Text.Trim();
                    obj.TEMPORALIDAD_PDTE_VAL = this.txtTemporalidadPV.Text.Trim();
                    obj.INDICADOR_SEGUIMIENTO_PDTE_VAL = this.txtIndSeguimientoPV.Text.Trim();
                    obj.INSTRUMENTOS_ACT_PDTE_VAL = this.txtInstrumentosActPV.Text.Trim();
                    //if (this.ddlEstadoSeguimientoPV.SelectedItem.Value != "-1")
                    //    obj.ESTADO_SEGUIMIENTO_ID_PDTE_VAL = Convert.ToInt32(this.ddlEstadoSeguimientoPV.SelectedItem.Value);
                    if (this.ddlPorcentajeAvancePV.SelectedItem.Value != "-1")
                        obj.PORCENTAJE_AVANCE_PDTE_VAL = Convert.ToByte(this.ddlPorcentajeAvancePV.SelectedItem.Value);
                    obj.ORGANO_RESPONSABLE_PDTE_VAL = this.txtOrganoResponsablePV.Text.Trim();
                    obj.COMENTARIO_VALIDADOR = this.txtAccComentarioValidador.Text.Trim();
                    obj.OBSERVACIONES = this.txtAccObservaciones.Text.Trim();

                    if (bHayCambios)
                    {
                        obj.FECHA_MODIFICACION = DateTime.Now;
                        //obj.AUTOR_MODIFICACION_USUARIO_ID = User.Identity.GetUserId().ToString(); //no ponemos como autor para que no afecte a las notificaciones
                        obj.TIPO_CAMBIO_CONTENIDO_ID = TIPO_CAMBIO_CONTENIDO.MODIFICACION;
                        obj.ESTADO_VALIDACION_ID = ESTADOS_VALIDACION.PDTE_VALIDAR;
                    }                   

                    c.CONTENIDO.Attach(obj);
                    c.Entry(obj).State = EntityState.Modified;
                    c.SaveChanges();

                    SuccessMessage = "Instrumento/actividad actualizado correctamente.";
                    successMessage.Visible = true;
                }
                catch (Exception ex)
                {
                    logger.Error("Error al actualizar Instrumento/actividad (AdminPlan.aspx). Error: " + ex.Message + " " + ex.InnerException);
                    Notificaciones.NotifySystemOps(ex);
                    ErrorMessage = "Error al actualizar Instrumento/actividad.";
                    errorMessage.Visible = true;                    
                    bAviso = false; //si hay error no se envía correo ya que seguramente no se ha actualizado nada
                }
            }

            if (bAviso)
            {
                using (Entities c = new Entities())
                {
                    try
                    {
                        bool bHayComentario = !String.IsNullOrEmpty(this.txtAccComentarioValidador.Text);
                        string email = "";
                        string strComentario = "";
                        string strObjetivo = "";
                        if (bHayComentario)
                            strComentario = "<br><br> Comentario del validador: <strong>" + this.txtAccComentarioValidador.Text + "</strong>";
                        if (obj.OBJETIVO != null)
                            strObjetivo = " <br/> Objetivo estratégico: " + obj.OBJETIVO.OBJETIVO_ESTRATEGICO + "<br/>";
                        string sUrlE = new Uri(Request.Url, "../Planes/AdminPlan.aspx?id=" + obj.DEPARTAMENTO_ID).AbsoluteUri.ToString();
                        string sUrl = new Uri(Request.Url, "../View/DetalleDepartamento.aspx?id=" + obj.DEPARTAMENTO_ID + "&oId=" + obj.CONTENIDO_ID).AbsoluteUri.ToString();
                        string strBody = "Aplicación " + WebConfigurationManager.AppSettings["NombreLargoApp"] + "<br/><br/> El siguiente instrumento/actividad ha sido actualizado: " + obj.INSTRUMENTOS_ACT  + strObjetivo + strComentario + "<br><br> Está accesible en la siguiente dirección: " + sUrl + "<br><br> Se puede actualizar en la siguiente dirección: " + sUrlE;                        
                        //Si el autor de la modificación es el mismo que el usuario actual (validador...) avisamos al creador del contenido
                        if (obj.AUTOR_MODIFICACION_USUARIO_ID == User.Identity.GetUserId())
                        {
                            AspNetUsers usr = c.AspNetUsers.Find(obj.AUTOR_CREACION_USUARIO_ID);
                            email = usr.Email;
                        }
                        else
                        {
                            if (!String.IsNullOrEmpty(obj.AUTOR_MODIFICACION_USUARIO_ID))
                            {
                                AspNetUsers usr = c.AspNetUsers.Find(obj.AUTOR_MODIFICACION_USUARIO_ID);
                                email = usr.Email;
                            }
                            else
                            {
                                AspNetUsers usr = c.AspNetUsers.Find(obj.AUTOR_CREACION_USUARIO_ID);
                                email = usr.Email;
                            }
                        }
                        if (bHayComentario)
                            Notificaciones.SendEmail(email, WebConfigurationManager.AppSettings["NombreCortoApp"] + " - Instrumento/actividad actualizado con Comentario", strBody);
                        else
                            Notificaciones.SendEmail(email, WebConfigurationManager.AppSettings["NombreCortoApp"] + " - Instrumento/actividad actualizado", strBody);
                    }
                    catch (Exception ex)
                    {
                        logger.Error("Error al notificar actualización Objetivo (AdminPlan.aspx). Error: " + ex.Message + " " + ex.InnerException);
                        Notificaciones.NotifySystemOps(ex);
                        ErrorMessage = "Error al notificar la actualización del Objetivo.";
                        errorMessage.Visible = true;
                    }
                }
            }


        }
        else
        {
            ErrorMessage = "Se deben rellenar los campos del formulario.";
            errorMessage.Visible = true;
        }


    }

    protected void btnEliminaAcc_Click(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(Request["__EVENTARGUMENT"]))
        {
            using (Entities c = new Entities())
            {


                try
                {
                    int id = Convert.ToInt32(Request["__EVENTARGUMENT"]);
                    int iDeptoId = Convert.ToInt32(hdDepartamentoId.Value);
                    ACCION acc = c.CONTENIDO.OfType<ACCION>().SingleOrDefault(x => x.CONTENIDO_ID == id);

                    if (acc.TieneAcceso(User.Identity.GetUserId()))
                    {
                        //Si todavía no ha sido publicado lo eliminamos
                        c.CONTENIDO.Remove(acc);
                        c.SaveChanges();
                        Response.Redirect("AdmPlan?id=" + iDeptoId.ToString() + "&m=" + "Instrumento/actividad eliminado correctamente.", false);
                    }
                    else
                        logger.Info("Acceso - AdmPlan.aspx Usuario: " + User.Identity.GetUserId() + " sin permiso de modificación en Acción: " + id.ToString());



                }
                catch (Exception ex)
                {
                    ErrorMessage = "Error al eliminar el instrumento/actividad.";
                    errorMessage.Visible = true;
                    logger.Error("ERROR: AdminPlan.aspx Error al eliminar accion " + Request["__EVENTARGUMENT"] + ". Error: " + ex.Message + " " + ex.InnerException);
                    Notificaciones.NotifySystemOps(ex);
                }

            }
        }
    }
    protected void btnEliminaDep_Click(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(Request["__EVENTARGUMENT"]))
        {
            using (Entities c = new Entities())
            {
                try
                {
                    int id = Convert.ToInt32(Request["__EVENTARGUMENT"]);                                           
                    
                    DEPARTAMENTO.EliminaDepartamento(id);
                    Response.Redirect("AdmPlan.aspx?m=" + "Departamento eliminado correctamente.", false);
                }
                catch (Exception ex)
                {
                    ErrorMessage = "Error al eliminar el departamento.";
                    errorMessage.Visible = true;
                    logger.Error("ERROR: AdminPlan.aspx Error al eliminar departamento " + Request["__EVENTARGUMENT"] + ". Error: " + ex.Message + " " + ex.InnerException);
                    Notificaciones.NotifySystemOps(ex);
                }

            }
        }
    }

    protected void btnEliminaObj_Click(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(Request["__EVENTARGUMENT"]))
        {
            using (Entities c = new Entities())
            {


                try
                {
                    int id = Convert.ToInt32(Request["__EVENTARGUMENT"]);
                    int iDeptoId = Convert.ToInt32(hdDepartamentoId.Value);
                    OBJETIVO obj = c.CONTENIDO.OfType<OBJETIVO>().SingleOrDefault(x => x.CONTENIDO_ID == id);

                    if (obj.TieneAcceso(User.Identity.GetUserId()))
                    {

                        OBJETIVO.EliminaObjetivo(obj.CONTENIDO_ID);                                                        
                        Response.Redirect("AdmPlan.aspx?id=" + iDeptoId.ToString() + "&m=" + "Objetivo eliminado correctamente.", false);
                     
                    }
                    else
                        logger.Info("Acceso - AdminPlan.aspx Usuario: " + User.Identity.GetUserId() + " sin permiso de modificación en Objetivo: " + id.ToString());



                }
                catch (Exception ex)
                {
                    ErrorMessage = "Error al eliminar el objetivo.";
                    errorMessage.Visible = true;
                    logger.Error("ERROR: AdminPlan.aspx Error al eliminar objetivo " + Request["__EVENTARGUMENT"] + ". Error: " + ex.Message + " " + ex.InnerException);
                    Notificaciones.NotifySystemOps(ex);
                }

            }
        }
    }
    protected void btnOkObj_Click(object sender, EventArgs e)
    {
        //Validar Objetivo        
        if (ModelState.IsValid)
        {
            bool bAviso = false;
            bool bEliminado = false;
            OBJETIVO obj = null;
            int iObjetivoId = Convert.ToInt32(this.hdObjetivoIdEdit.Value);
            using (Entities c = new Entities())
            {
                try
                {
                   
                    obj = c.CONTENIDO.OfType<OBJETIVO>().Where(o => o.CONTENIDO_ID == iObjetivoId).FirstOrDefault();
                    if (obj.TIPO_CAMBIO_CONTENIDO_ID == TIPO_CAMBIO_CONTENIDO.ELIMINADO)
                    {
                        bEliminado = true;
                    }
                    obj.VALIDADOR_USUARIO_ID = User.Identity.GetUserId().ToString();

                    if ((obj.OBJETIVO_ESTRATEGICO != obj.OBJETIVO_ESTRATEGICO_PDTE_VAL) ||
                        this.txtObjComentarioValidador.Text != obj.COMENTARIO_VALIDADOR) bAviso = true; //solo si hay cambios enviaremos email al usuario


                    obj.OBJETIVO_ESTRATEGICO_PDTE_VAL = this.ObjetivoEstrategicoPV.Text.Trim();
                    obj.COMENTARIO_VALIDADOR = this.txtObjComentarioValidador.Text.Trim();

                    obj.Validar(c);

                    if (!bEliminado)
                        SuccessMessage = "Objetivo actualizado correctamente.";
                    else
                        SuccessMessage = "Objetivo eliminado correctamente.";
                    
                    successMessage.Visible = true;
                }
                catch (Exception ex)
                {
                    logger.Error("Error al validar Objetivo (AdminPlan.aspx). Error: " + ex.Message + " " + ex.InnerException);
                    Notificaciones.NotifySystemOps(ex);
                    ErrorMessage = "Error al validar Objetivo.";
                    errorMessage.Visible = true;
                    bAviso = false;
                }
            }

            string strBody;
            string strSubject;

            if (bAviso)
            {
                using (Entities c = new Entities())
                {
                    try
                    {
                        bool bHayComentario = !String.IsNullOrEmpty(this.txtObjComentarioValidador.Text);
                        string strComentario = "";
                        if (bHayComentario)
                            strComentario = "<br><br> Comentario del validador: <strong>" + this.txtObjComentarioValidador.Text + "</strong>";
                        string email = "";

                        string sUrl = new Uri(Request.Url, "../View/DetalleDepartamento.aspx?id=" + obj.DEPARTAMENTO_ID + "&oId=" + obj.CONTENIDO_ID).AbsoluteUri.ToString();
                        string sUrlE = new Uri(Request.Url, "../Planes/AdminPlan.aspx?id=" + obj.DEPARTAMENTO_ID).AbsoluteUri.ToString();

                        if (!bEliminado)
                        {
                            strBody = "Aplicación " + WebConfigurationManager.AppSettings["NombreLargoApp"] + "<br/><br/> El siguiente objetivo estratégico ha sido actualizado: " + obj.OBJETIVO_ESTRATEGICO + strComentario +  "<br><br> Está accesible en la siguiente dirección: " + sUrl + "<br><br> Se puede actualizar en la siguiente dirección: " + sUrlE;
                            strSubject = "Objetivo publicado";
                        }
                        else
                        {
                            strBody = "Aplicación " + WebConfigurationManager.AppSettings["NombreLargoApp"] + "<br/><br/> El siguiente objetivo estratégico ha sido eliminado: " + obj.OBJETIVO_ESTRATEGICO;
                            strSubject = "Objetivo eliminado";
                        }
                        //Si el autor de la modificación es el mismo que el usuario actual (validador...) avisamos al creador del contenido
                        if (obj.AUTOR_MODIFICACION_USUARIO_ID == User.Identity.GetUserId())
                        {
                            if (!String.IsNullOrEmpty(obj.AUTOR_CREACION_USUARIO_ID))
                            {
                                AspNetUsers usr = c.AspNetUsers.Find(obj.AUTOR_CREACION_USUARIO_ID);
                                email = usr.Email;
                            }
                        }
                        else
                        {
                            if (!String.IsNullOrEmpty(obj.AUTOR_MODIFICACION_USUARIO_ID))
                            {
                                AspNetUsers usr = c.AspNetUsers.Find(obj.AUTOR_MODIFICACION_USUARIO_ID);
                                email = usr.Email;
                            }
                            else
                            {
                                if (!String.IsNullOrEmpty(obj.AUTOR_CREACION_USUARIO_ID))
                                {
                                    AspNetUsers usr = c.AspNetUsers.Find(obj.AUTOR_CREACION_USUARIO_ID);
                                    email = usr.Email;
                                }
                            }
                        }
                        if (!String.IsNullOrEmpty(email))
                            Notificaciones.SendEmail(email, WebConfigurationManager.AppSettings["NombreCortoApp"] + " - " + strSubject, strBody);
                    }
                    catch (Exception ex)
                    {
                        logger.Error("Error al notificar validación Objetivo (AdminPlan.aspx). Error: " + ex.Message + " " + ex.InnerException);
                        Notificaciones.NotifySystemOps(ex);
                        ErrorMessage = "Error al notificar la validación del Objetivo.";
                        errorMessage.Visible = true;
                    }
                }
            }            
        }
        else
        {
            ErrorMessage = "Se deben rellenar los campos del formulario.";
            errorMessage.Visible = true;
        }
    }

    protected void btnOkAcc_Click(object sender, EventArgs e)
    {
        //Validar Accion
        if (ModelState.IsValid)
        {
            bool bAviso = false;
            bool bEliminado = false;
            ACCION obj = null;
            int iAccionId = Convert.ToInt32(this.hdAccionIdEdit.Value);            
            using (Entities c = new Entities())
            {
                try
                {
                    obj = c.CONTENIDO.OfType<ACCION>().Include("OBJETIVO").Where(o => o.CONTENIDO_ID == iAccionId).FirstOrDefault();

                    if (obj.TIPO_CAMBIO_CONTENIDO_ID == TIPO_CAMBIO_CONTENIDO.ELIMINADO)
                    {
                        bEliminado = true;
                    }

                    if ((obj.SEGUIMIENTO != obj.SEGUIMIENTO_PDTE_VAL) ||
                        (obj.RECURSOS_HUMANOS != obj.RECURSOS_HUMANOS_PDTE_VAL) ||
                        (obj.COSTE_ECONOMICO != obj.COSTE_ECONOMICO_PDTE_VAL) ||
                        (obj.INSTRUMENTOS_ACT != obj.INSTRUMENTOS_ACT_PDTE_VAL) ||
                        (obj.MEDIOS_OTROS != obj.MEDIOS_OTROS_PDTE_VAL) ||
                        (obj.TEMPORALIDAD != obj.TEMPORALIDAD_PDTE_VAL) ||
                        (obj.INDICADOR_SEGUIMIENTO != obj.INDICADOR_SEGUIMIENTO_PDTE_VAL) ||
                        //(obj.ESTADO_SEGUIMIENTO_ID != obj.ESTADO_SEGUIMIENTO_ID_PDTE_VAL) ||
                        (obj.PORCENTAJE_AVANCE != obj.PORCENTAJE_AVANCE_PDTE_VAL) ||
                        (obj.ORGANO_RESPONSABLE != obj.ORGANO_RESPONSABLE_PDTE_VAL) ||
                        this.txtAccComentarioValidador.Text != obj.COMENTARIO_VALIDADOR) bAviso = true; //solo si hay cambios enviaremos email al usuario


                    obj.VALIDADOR_USUARIO_ID = User.Identity.GetUserId().ToString();

                    obj.SEGUIMIENTO_PDTE_VAL = this.txtSeguimientoPV.Text.Trim();
                    obj.RECURSOS_HUMANOS_PDTE_VAL = this.txtRecursosHumanosPV.Text.Trim();
                    obj.COSTE_ECONOMICO_PDTE_VAL = this.txtCosteEconomicoPV.Text.Trim();
                    obj.INDICADOR_SEGUIMIENTO_PDTE_VAL = this.txtIndSeguimientoPV.Text.Trim();
                    obj.MEDIOS_OTROS_PDTE_VAL = this.txtMediosOtrosPV.Text.Trim();
                    obj.TEMPORALIDAD_PDTE_VAL = this.txtTemporalidadPV.Text.Trim();
                    obj.INDICADOR_SEGUIMIENTO_PDTE_VAL = this.txtIndSeguimientoPV.Text.Trim();
                    obj.INSTRUMENTOS_ACT_PDTE_VAL = this.txtInstrumentosActPV.Text.Trim();
                    //if (this.ddlEstadoSeguimientoPV.SelectedItem.Value != "-1")
                    //    obj.ESTADO_SEGUIMIENTO_ID_PDTE_VAL = Convert.ToInt32(this.ddlEstadoSeguimientoPV.SelectedItem.Value);
                    if (this.ddlPorcentajeAvancePV.SelectedItem.Value != "-1")
                        obj.PORCENTAJE_AVANCE_PDTE_VAL = Convert.ToByte(this.ddlPorcentajeAvancePV.SelectedItem.Value);
                    obj.ORGANO_RESPONSABLE_PDTE_VAL = this.txtOrganoResponsablePV.Text.Trim();
                    obj.COMENTARIO_VALIDADOR = this.txtAccComentarioValidador.Text.Trim();
                    obj.OBSERVACIONES = this.txtAccObservaciones.Text.Trim();                    

                    obj.Validar(c);

                    if (!bEliminado)                    
                        SuccessMessage = "Instrumento/actividad actualizado correctamente.";
                    else
                        SuccessMessage = "Instrumento/actividad eliminado correctamente.";

                    successMessage.Visible = true;
                }
                catch (Exception ex)
                {
                    logger.Error("Error al actualizar Instrumento/actividad (AdminPlan.aspx). Error: " + ex.Message + " " + ex.InnerException);
                    Notificaciones.NotifySystemOps(ex);
                    ErrorMessage = "Error al actualizar Instrumento/actividad.";
                    errorMessage.Visible = true;
                    bAviso = false;
                }
            }

            string strBody;
            string strSubject;

            if (bAviso)
            {
                using (Entities c = new Entities())
                {
                    try
                    {
                        bool bHayComentario = !String.IsNullOrEmpty(this.txtAccComentarioValidador.Text);
                        string email = "";
                        string strComentario = "";
                        string strObjetivo = "";
                        if (bHayComentario)
                            strComentario = "<br><br> Comentario del validador: <strong>" + this.txtAccComentarioValidador.Text + "</strong>";
                        if (obj.OBJETIVO != null)
                            strObjetivo = " <br/> Objetivo estratégico: " + obj.OBJETIVO.OBJETIVO_ESTRATEGICO + "<br/>";
                        string sUrl = new Uri(Request.Url, "../View/DetalleDepartamento.aspx?id=" + obj.DEPARTAMENTO_ID + "&oId=" + obj.OBJETIVO_CONTENIDO_ID).AbsoluteUri.ToString();
                        string sUrlE = new Uri(Request.Url, "../Planes/AdminPlan.aspx?id=" + obj.DEPARTAMENTO_ID).AbsoluteUri.ToString();
                        if (!bEliminado)
                        {
                            strBody = "Aplicación " + WebConfigurationManager.AppSettings["NombreLargoApp"] + "<br/><br/> El siguiente instrumento/actividad ha sido publicado: " + obj.INSTRUMENTOS_ACT + strObjetivo +  strComentario + "<br><br> Está accesible en la siguiente dirección: " + sUrl + "<br><br> Se puede actualizar en la siguiente dirección: " + sUrlE;
                            strSubject = "Instrumento/actividad publicada";
                        }
                        else
                        {
                            strBody = "Aplicación " + WebConfigurationManager.AppSettings["NombreLargoApp"] + "<br/><br/> Se ha eliminado el instrumento/actividad: " + obj.INSTRUMENTOS_ACT;
                            strSubject = "Instrumento/actividad eliminada";
                        }

                        //Si el autor de la modificación es el mismo que el usuario actual (validador...) avisamos al creador del contenido
                        if (obj.AUTOR_MODIFICACION_USUARIO_ID == User.Identity.GetUserId())
                        {
                            if (!String.IsNullOrEmpty(obj.AUTOR_CREACION_USUARIO_ID))
                            {
                                AspNetUsers usr = c.AspNetUsers.Find(obj.AUTOR_CREACION_USUARIO_ID);
                                email = usr.Email;
                            }
                        }
                        else
                        {
                            if (!String.IsNullOrEmpty(obj.AUTOR_MODIFICACION_USUARIO_ID))
                            {
                                AspNetUsers usr = c.AspNetUsers.Find(obj.AUTOR_MODIFICACION_USUARIO_ID);
                                email = usr.Email;
                            }
                            else
                            {

                                if (!String.IsNullOrEmpty(obj.AUTOR_CREACION_USUARIO_ID))
                                {
                                    AspNetUsers usr = c.AspNetUsers.Find(obj.AUTOR_CREACION_USUARIO_ID);
                                    email = usr.Email;
                                }
                            }
                        }
                        if (!String.IsNullOrEmpty(email))
                            Notificaciones.SendEmail(email, WebConfigurationManager.AppSettings["NombreCortoApp"] + " - " + strSubject, strBody);
                    }
                    catch (Exception ex)
                    {
                        logger.Error("Error al notificar validación instrumento (AdminPlan.aspx). Error: " + ex.Message + " " + ex.InnerException);
                        Notificaciones.NotifySystemOps(ex);
                        ErrorMessage = "Error al notificar la validación del Instrumento/actividad.";
                        errorMessage.Visible = true;
                    }
                }
            }
        }
        else
        {
            ErrorMessage = "Se deben rellenar los campos del formulario.";
            errorMessage.Visible = true;
        }

    }

    protected void btnAddLeg_Click(object sender, EventArgs e)
    {
        if (ModelState.IsValid)
        {
                using (Entities c = new Entities())
                {   
                    if (chkLegActual.Checked)
                    {
                        LEGISLATURA.DesmarcaActualLegislatura();
                    }

                LEGISLATURA obj = new LEGISLATURA
                {
                    ACTUAL = chkLegActual.Checked,
                    DESCRIPCION = txtLegislatura.Text.Trim(),
                    FECHA_INICIO = Convert.ToDateTime(this.txtFechaInicioLegislatura.Text),
                    FECHA_FIN = Convert.ToDateTime(this.txtFechaFinLegislatura.Text)                         
                 };                    

                    c.LEGISLATURA.Add(obj);                                   

                    try
                    {
                        c.SaveChanges();
                        SuccessMessage = "Legislatura guardada correctamente.";
                        successMessage.Visible = true;
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("Error al crear nueva legislatura (AdminPlan.aspx). Error: " + ex.Message + " " + ex.InnerException);
                        Notificaciones.NotifySystemOps(ex);
                        ErrorMessage = "Error al crear nueva legislatura.";
                        errorMessage.Visible = true;
                    }
                }


        }
        else
        {
            ErrorMessage = "Se deben rellenar los campos del formulario.";
            errorMessage.Visible = true;
        }
    }

    protected void btnAddDep_Click(object sender, EventArgs e)
    {
        if (ModelState.IsValid)
        {
            try
            {
                if (Convert.ToInt16(this.txtOrdenDepartamento.Text) <= 255)
                {
                    using (Entities c = new Entities())
                    {
                        DEPARTAMENTO obj = new DEPARTAMENTO
                        {
                            DESCRIPCION = txtDepartamento.Text.Trim(),
                            LEGISLATURA_ID = Convert.ToInt32(this.hdLegislaturaId.Value),
                            ORDEN = Convert.ToByte(this.txtOrdenDepartamento.Text),
                            VISIBLE = chkVisibleDep.Checked
                        };

                        c.DEPARTAMENTO.Add(obj);


                        c.SaveChanges();
                        SuccessMessage = "Departamento creado correctamente.";
                        successMessage.Visible = true;
                    }
                }
                else
                {
                    ErrorMessage = "El campo orden no puede ser superior a 255.";
                    errorMessage.Visible = true;
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("Error al crear nuevo departamento (AdminPlan.aspx). Error: " + ex.Message + " " + ex.InnerException);
                Notificaciones.NotifySystemOps(ex);
                ErrorMessage = "Error al crear nuevo departamento.";
                errorMessage.Visible = true;
            }

        }
        else
        {
            ErrorMessage = "Se deben rellenar los campos del formulario.";
            errorMessage.Visible = true;
        }
    }



    protected void btnActualizaLeg_Click(object sender, EventArgs e)
    {
        bool bErrorLegActual = false;
        if (ModelState.IsValid)
        {

            int iLegislaturaId  = Convert.ToInt32(this.hdLegislaturaId.Value);
            using (Entities c = new Entities())
            {
                try
                {
                    LEGISLATURA obj = c.LEGISLATURA.Find(iLegislaturaId);
                    if ((!obj.ACTUAL) && (this.chkLegActual.Checked))
                        LEGISLATURA.DesmarcaActualLegislatura(); //desmarcamos la que ya estuviera marcada, sólo puede haber una actual

                    obj.ACTUAL = this.chkLegActual.Checked;

                    if (!chkLegActual.Checked)
                    {
                        //debe haber una Legislatura Actual, si hemos desmarcado y era la única no se permite desmarcar
                        if (iLegislaturaId == LEGISLATURA.GetActualLegislatura())
                        {
                            obj.ACTUAL = true; //la marcamos nuevamente
                            bErrorLegActual = true;
                        }
                    }

                    obj.DESCRIPCION = this.txtLegislatura.Text.Trim();
                    obj.FECHA_INICIO = Convert.ToDateTime(this.txtFechaInicioLegislatura.Text);
                    obj.FECHA_FIN = Convert.ToDateTime(this.txtFechaFinLegislatura.Text);

                    c.LEGISLATURA.Attach(obj);
                    c.Entry(obj).State = EntityState.Modified;
                    c.SaveChanges();

                    if (!bErrorLegActual)
                    {
                        SuccessMessage = "Legislatura actualizada correctamente.";
                        successMessage.Visible = true;
                    }
                    else
                    {
                        WarningMessage = "Se ha actualizado la Legislatura pero no se puede desmarcar como actual ya que debe haber una en todo momento. Si quiere marcar otra como activa editela y seleccionela como actual";
                        warningMessage.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("Error al actualizar Legislatura (AdminPlan.aspx). Error: " + ex.Message + " " + ex.InnerException);
                    Notificaciones.NotifySystemOps(ex);
                    ErrorMessage = "Error al actualizar legislatura.";
                    errorMessage.Visible = true;
                }
            }


        }
        else
        {
            ErrorMessage = "Se deben rellenar los campos del formulario.";
            errorMessage.Visible = true;
        }
    }

    protected void btnActualizaDep_Click(object sender, EventArgs e)
    {
        if (ModelState.IsValid)
        {
            try
            {

                if (Convert.ToInt16(this.txtOrdenDepartamento.Text) <= 255)
                {

                    int iDepId = Convert.ToInt32(this.hdDepartamentoId.Value);
                    using (Entities c = new Entities())
                    {

                        DEPARTAMENTO obj = c.DEPARTAMENTO.Find(iDepId);


                        obj.DESCRIPCION = this.txtDepartamento.Text.Trim();
                        obj.VISIBLE = this.chkVisibleDep.Checked;
                        obj.ORDEN = Convert.ToByte(this.txtOrdenDepartamento.Text);
                        c.DEPARTAMENTO.Attach(obj);
                        c.Entry(obj).State = EntityState.Modified;
                        c.SaveChanges();

                        SuccessMessage = "Departamento actualizado correctamente.";
                        successMessage.Visible = true;
                    }
                }
                else
                {
                    ErrorMessage = "El campo orden no puede ser superior a 255.";
                    errorMessage.Visible = true;
                }
             
            }
            catch (Exception ex)
            {
                logger.Error("Error al actualizar Departamento (AdminPlan.aspx). Error: " + ex.Message + " " + ex.InnerException);
                Notificaciones.NotifySystemOps(ex);
                ErrorMessage = "Error al actualizar departamento.";
                errorMessage.Visible = true;
            }


        }
        else
        {
            ErrorMessage = "Se deben rellenar los campos del formulario.";
            errorMessage.Visible = true;
        }
    }

    protected void btnObjDeshacerCambio_Click(object sender, EventArgs e)
    {

        int iObjetivoId = Convert.ToInt32(this.hdObjetivoIdEdit.Value);
        using (Entities c = new Entities())
        {
            try
            {
                OBJETIVO obj = c.CONTENIDO.OfType<OBJETIVO>().Where(o => o.CONTENIDO_ID == iObjetivoId).FirstOrDefault();

                if (obj.TIPO_CAMBIO_CONTENIDO_ID != TIPO_CAMBIO_CONTENIDO.SIN_CAMBIOS)
                {
                    obj.DeshacerCambio(c);
                    SuccessMessage = "Los cambios pendientes del Objetivo se han cancelado.";
                    successMessage.Visible = true;
                }
                else
                {
                    ErrorMessage = "No hay ningún cambio que deshacer.";
                    errorMessage.Visible = true;
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("Error al deshacer cambios en Objetivo. Error: " + ex.Message + " " + ex.InnerException);
                Notificaciones.NotifySystemOps(ex);
                ErrorMessage = "Error al deshacer los cambios del Objetivo.";
                errorMessage.Visible = true;
            }


        }
    }

    protected void btnAccDeshacerCambio_Click(object sender, EventArgs e)
    {

        int iObjetivoId = Convert.ToInt32(this.hdAccionIdEdit.Value);
        using (Entities c = new Entities())
        {
            try
            {
                ACCION obj = c.CONTENIDO.OfType<ACCION>().Where(o => o.CONTENIDO_ID == iObjetivoId).FirstOrDefault();

                if (obj.TIPO_CAMBIO_CONTENIDO_ID != TIPO_CAMBIO_CONTENIDO.SIN_CAMBIOS)
                {
                    obj.DeshacerCambio(c);
                    SuccessMessage = "Los cambios pendientes del Objetivo se han cancelado.";
                    successMessage.Visible = true;
                }
                else
                {
                    ErrorMessage = "No hay ningún cambio que deshacer.";
                    errorMessage.Visible = true;
                }

            }
            catch (Exception ex)
            {
                logger.Fatal("Error al deshacer cambios en Objetivo. Error: " + ex.Message + " " + ex.InnerException);
                Notificaciones.NotifySystemOps(ex);
                ErrorMessage = "Error al deshacer los cambios del Objetivo.";
                errorMessage.Visible = true;
            }


        }
    }

    protected void InformeExcel(int? iDptoId, int? iLegislaturaId)
    {
        try
        {

            System.IO.MemoryStream stream;

            //using (Entities c = new Entities())
            {
                //DEPARTAMENTO dep = c.DEPARTAMENTO.Find(iDptoId);

                

                stream = new System.IO.MemoryStream();

                using (SpreadsheetDocument document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook, true))
                {

                    WorkbookPart workbookPart = document.AddWorkbookPart();
                    workbookPart.Workbook = new Workbook();

                    WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                    MergeCells mergeCells;

                    var sheetData = InformesExcel.HojaExcelInforme(iDptoId, iLegislaturaId, false, out mergeCells);

                    //Añadimos ahora las columnas para poder asignarles el ancho personalizado:
                    Columns columns = InformesExcel.SizesInformeDepartamento(sheetData);

                    worksheetPart.Worksheet = new Worksheet();
                    worksheetPart.Worksheet.Append(columns);
                    worksheetPart.Worksheet.Append(sheetData);

                    Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
                    Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Plan de Gobierno - " + DateTime.Now.ToShortDateString().Replace("/", "-") };
                    //añadimos las celdas combinadas
                    worksheetPart.Worksheet.InsertAfter(mergeCells, worksheetPart.Worksheet.Elements<SheetData>().First());
                    sheets.Append(sheet);


                    //Añadimos la pestaña con cambios pendientes de validar
                    MergeCells mergeCellsPV;
                    SheetData sheetDataPV = InformesExcel.HojaExcelInforme(iDptoId, iLegislaturaId, true, out mergeCellsPV);

                    WorksheetPart worksheetPartPV = workbookPart.AddNewPart<WorksheetPart>();
                    Worksheet workSheetPV = new Worksheet();

                    //Añadimos ahora las columnas para poder asignarles el ancho personalizado:
                    Columns columnsPV = InformesExcel.SizesInformeDepartamento(sheetDataPV);

                    worksheetPartPV.Worksheet = new Worksheet();
                    worksheetPartPV.Worksheet.Append(columnsPV);
                    worksheetPartPV.Worksheet.Append(sheetDataPV);

                    Sheet sheetPV = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPartPV), SheetId = 2, Name = "P.G. (sin validar) - " + DateTime.Now.ToShortDateString().Replace("/", "-") };
                    //añadimos las celdas combinadas
                    worksheetPartPV.Worksheet.InsertAfter(mergeCellsPV, worksheetPartPV.Worksheet.Elements<SheetData>().First());
                    sheets.Append(sheetPV);

                    //fin pestaña de cambios pendientes de validar


                    WorkbookStylesPart stylesPart = document.WorkbookPart.AddNewPart<WorkbookStylesPart>();
                    stylesPart.Stylesheet = InformesExcel.GenerateStyleSheet();
                    stylesPart.Stylesheet.Save();
                    worksheetPart.Worksheet.Save();
                    //fin nuevo

                    workbookPart.Workbook.Save();
                }
            }


            stream.Flush();
            stream.Position = 0;

            Response.ClearContent();
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";

            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            Response.AddHeader("content-disposition", "attachment; filename=Plan_de_Gobierno_" + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            byte[] data1 = new byte[stream.Length];
            stream.Read(data1, 0, data1.Length);
            stream.Close();
            Response.BinaryWrite(data1);
            //Response.Flush();

            //  Feb2015: Needed to replace "Response.End();" with the following 3 lines, to make sure the Excel was fully written to the Response
            System.Web.HttpContext.Current.Response.Flush();
            System.Web.HttpContext.Current.Response.SuppressContent = true;
            System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
            //Response.End();


        }
        catch (Exception ex)
        {

            logger.Error("Error al imprimir informe excel departamento (InformeExcel). Dpto: " + iDptoId + " Error: " + ex.Message + " " + ex.InnerException);
            Notificaciones.NotifySystemOps(ex, "Error al imprimir informe excel departamento (InformeExcel). Dpto: " + iDptoId);

            ErrorMessage = "Error al crear el informe.";
            errorMessage.Visible = true;

        }
    }

    protected void btnImprimirInformeExcelLeg_Click(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(Request["__EVENTARGUMENT"]))
        {
            using (Entities c = new Entities())
            {
                try
                {
                    int id = Convert.ToInt32(Request["__EVENTARGUMENT"]);
                    InformeExcel(null, id);
                }
                catch (Exception ex)
                {
                    ErrorMessage = "Error al generar el informe.";
                    errorMessage.Visible = true;
                    logger.Error("ERROR: AdminPlan.aspx Error al eliminar generar informe de legislatura id:" + Request["__EVENTARGUMENT"] + ". Error: " + ex.Message + " " + ex.InnerException);
                    Notificaciones.NotifySystemOps(ex, "ERROR: AdminPlan.aspx Error al eliminar generar informe de legislatura id:" + Request["__EVENTARGUMENT"]);
                }

            }
        }


    }
    protected void btnImprimirInformeExcel_Click(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(Request["__EVENTARGUMENT"]))
        {
            using (Entities c = new Entities())
            {
                try
                {
                    int id = Convert.ToInt32(Request["__EVENTARGUMENT"]);
                    InformeExcel(id, null);
                }
                catch (Exception ex)
                {
                    ErrorMessage = "Error al generar el informe.";
                    errorMessage.Visible = true;
                    logger.Error("ERROR: AdminPlan.aspx Error al eliminar generar informe de departamento id:" + Request["__EVENTARGUMENT"] + ". Error: " + ex.Message + " " + ex.InnerException);
                    Notificaciones.NotifySystemOps(ex, "ERROR: AdminPlan.aspx Error al eliminar generar informe de departamento id:" + Request["__EVENTARGUMENT"]);
                }

            }
        }


    }

    protected void btnExportarDatosLeg_Click(object sender, EventArgs e)
    {
        
        int iLegId = Convert.ToInt32(hdLegislaturaId.Value);


        if (this.ddlTipoInformeLeg.SelectedItem.Value == "PDF")
        {
            Response.Redirect("~/View/reports/ViewReport.aspx?t=plan&f=PDF&lid=" + iLegId, true);
        }
        else if (this.ddlTipoInformeLeg.SelectedItem.Value == "XLSX")
        {
            Response.Redirect("~/View/reports/ViewReport.aspx?t=plan&f=XLSX&lid=" + iLegId, true);
        }
        else if (this.ddlTipoInformeLeg.SelectedItem.Value == "DOCX")
        {
            Response.Redirect("~/View/reports/ViewReport.aspx?t=plan&f=DOCX&lid=" + iLegId, true);
        }
        else if (this.ddlTipoInformeLeg.SelectedItem.Value == "HTML")
        {
            Response.Redirect("~/View/reports/ViewReport.aspx?t=plan&lid=" + iLegId, true);
        }
    }

    protected void btnExportarDatosDep_Click(object sender, EventArgs e)
    {
        try
        {
            int iDptoId = Convert.ToInt32(hdDepartamentoId.Value);


            if (this.ddlTipoInforme.SelectedItem.Value == "PDF")
            {
                Response.Redirect("~/View/reports/ViewReport.aspx?id=" + iDptoId + "&t=plan&f=PDF", true);
            }
            else if (this.ddlTipoInforme.SelectedItem.Value == "XLSX")
            {
                Response.Redirect("~/View/reports/ViewReport.aspx?id=" + iDptoId + "&t=plan&f=XLSX", true);
            }
            else if (this.ddlTipoInforme.SelectedItem.Value == "DOCX")
            {
                Response.Redirect("~/View/reports/ViewReport.aspx?id=" + iDptoId + "&t=plan&f=DOCX", true);
            }
            else if (this.ddlTipoInforme.SelectedItem.Value == "HTML")
            {
                Response.Redirect("~/View/reports/ViewReport.aspx?id=" + iDptoId + "&t=plan", true);
            }
        }
        catch
        {
            ErrorMessage = "Seleccione un departamento para obtener el informe.";
            errorMessage.Visible = true;
        }
    }
}
