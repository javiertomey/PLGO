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
using System.Web.Configuration;
using NLog;
using System.Data.Entity;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using System.Data;
using System.ComponentModel;

public partial class AdminPlan : System.Web.UI.Page
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



    protected int getDptoId()
    {
        int? iDptoId = null;

        if ((User.IsInRole(AspNetRoles.ADMINISTRADOR)) || (User.IsInRole(AspNetRoles.VALIDADOR)))
        {
            if (Request["id"] == null) Response.Redirect("../Default.aspx", false);
            iDptoId = Convert.ToInt32(Request["id"]);
        }
        else
        {
            //Usuario  (accede sólo a los datos de su departamento) 
            using (Entities c = new Entities())
            {
                try
                {
                    iDptoId = c.USUARIOS.Find(User.Identity.GetUserId().ToString()).DEPARTAMENTOID;
                    if (Request["id"] == null) Response.Redirect("AdminPlan.aspx?id=" + iDptoId, false);
                }
                catch (Exception ex)
                {
                    logger.Warn("Error en AdminPlan.aspx. Error: " + ex.Message + " " + ex.InnerException);
                    Notificaciones.NotifySystemOps(ex);
                    Response.Redirect("../Default.aspx", true);
                }
            }
        }
        if (iDptoId.HasValue)
            return iDptoId.Value;
        else
        {
            logger.Error("Usuario sin permisos. Error en AdminPlan.aspx.");            
            Response.Redirect("../Default.aspx", true);
            return -1;
        }
    }

    protected void Page_Load()
    {
        if (Request["id"] == null)
            Response.Redirect("AdminPlan.aspx?id=" + getDptoId().ToString(), true);


        if (!IsPostBack)
        {

            int? iDptoId = null;

            iDptoId = getDptoId();

            //Comprobamos lo primero de todo que la entidad a administrar es la del usuario o es un administrador                  
            try
            {
                if (!User.IsInRole(AspNetRoles.ADMINISTRADOR) && !User.IsInRole(AspNetRoles.VALIDADOR))
                {
                    if (Request["id"] != null)
                    {
                        if (iDptoId != Convert.ToInt32(Request["id"]))
                        {
                            //No autorizado
                            logger.Info("No autorizado en AdminPlan.aspx. Usuario: " + User.Identity.GetUserId() + " Departamento: " + Request["id"]);
                            Response.Redirect("../Default.aspx", true);
                        }
                    }
                }                

            }
            catch (Exception ex)
            {
                logger.Warn("Error en AdminPlan.aspx (2). Error: " + ex.Message + " " + ex.InnerException);
                Notificaciones.NotifySystemOps(ex);
                Response.Redirect("../Default.aspx", true);
            }

            using (Entities c = new Entities())
            {
                DEPARTAMENTO dep = c.DEPARTAMENTO.Find(iDptoId);
                if (dep != null)
                {

                    LEGISLATURA leg = c.LEGISLATURA.Find(dep.LEGISLATURA_ID);

                    if (!dep.VISIBLE)
                    {
                        if (!User.IsInRole(AspNetRoles.ADMINISTRADOR) && !User.IsInRole(AspNetRoles.VALIDADOR))
                        {
                            ////No se puede editar un departamento no visible
                            //logger.Info("Editando un departamento no visible. Usuario: " + User.Identity.GetUserId() + " Departamento: " + iDptoId);
                            //Response.Redirect("../Default.aspx", true);
                        }
                    }
                    else if (!leg.ACTUAL)
                    {
                        //if (!User.IsInRole(AspNetRoles.ADMINISTRADOR))
                        //{                            
                        //    logger.Info("Editando un departamento de una legislatura no actual. Usuario: " + User.Identity.GetUserId() + " Departamento: " + iDptoId);
                        //    Response.Redirect("../Default.aspx", true);
                        //}
                    }


                    //List<ESTADOS_SEGUIMIENTO> tipos = c.ESTADOS_SEGUIMIENTO.ToList();
                    //tipos.Add(new ESTADOS_SEGUIMIENTO() { ESTADO_SEGUIMIENTO_ID = -1, DESCRIPCION = "Seleccione..." });

                    //this.ddlEstadoSeguimiento.DataTextField = "DESCRIPCION";
                    //this.ddlEstadoSeguimiento.DataValueField = "ESTADO_SEGUIMIENTO_ID";
                    //this.ddlEstadoSeguimiento.DataSource = tipos;
                    //this.ddlEstadoSeguimiento.DataBind();
                    //this.ddlEstadoSeguimiento.ClearSelection();
                    //this.ddlEstadoSeguimiento.Items.FindByText("Seleccione...").Selected = true;


                    this.litLegislatura.Text = leg.DESCRIPCION;
                    this.litNombredDEP.Text = dep.DESCRIPCION;
                    this.Title = dep.DESCRIPCION;
                }
                else
                {
                    logger.Info("El departamento no existe (Planes/AdminPlan.aspx)." );                    
                    Response.Redirect("../Default.aspx", true);
                }
            }

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


    protected void btnAddAcc_Click(object sender, EventArgs e)
    {
        if (ModelState.IsValid)
        {
            if ((!String.IsNullOrEmpty(this.txtInstrumentosAct.Text)) && (!String.IsNullOrEmpty(this.txtOrganoResponsable.Text)) &&
                (!String.IsNullOrEmpty(this.txtSeguimiento.Text)) && (!String.IsNullOrEmpty(this.txtRecursosHumanos.Text)) &&
                (!String.IsNullOrEmpty(this.txtCosteEconomico.Text)) &&
                (!String.IsNullOrEmpty(this.txtTemporalidad.Text)) && (!String.IsNullOrEmpty(this.txtIndSeguimiento.Text)) &&
                (this.ddlPorcentajeAvance.SelectedItem.Value != "-1"))
                //(this.ddlEstadoSeguimiento.SelectedItem.Value != "-1"))
            {

                int iDeptoId = getDptoId();
                using (Entities c = new Entities())
                {
                    try
                    {
                        ACCION obj = new ACCION
                        {
                            AUTOR_CREACION_USUARIO_ID = User.Identity.GetUserId().ToString(),
                            DEPARTAMENTO_ID = iDeptoId,
                            ESTADO_VALIDACION_ID = ESTADOS_VALIDACION.PDTE_VALIDAR,
                            FECHA_CREACION = DateTime.Now,
                            TIPO_CAMBIO_CONTENIDO_ID = TIPO_CAMBIO_CONTENIDO.ALTA,
                            VISIBLE = false,
                            SEGUIMIENTO_PDTE_VAL = txtSeguimiento.Text.Trim(),
                            RECURSOS_HUMANOS_PDTE_VAL = txtRecursosHumanos.Text.Trim(),
                            COSTE_ECONOMICO_PDTE_VAL = txtCosteEconomico.Text.Trim(),
                            INSTRUMENTOS_ACT_PDTE_VAL = txtInstrumentosAct.Text.Trim(),
                            MEDIOS_OTROS_PDTE_VAL = txtMediosOtros.Text.Trim(),
                            TEMPORALIDAD_PDTE_VAL = txtTemporalidad.Text.Trim(),
                            INDICADOR_SEGUIMIENTO_PDTE_VAL = txtIndSeguimiento.Text.Trim(),
                            //ESTADO_SEGUIMIENTO_ID_PDTE_VAL = Convert.ToInt32(this.ddlEstadoSeguimiento.SelectedItem.Value),
                            PORCENTAJE_AVANCE_PDTE_VAL = Convert.ToByte(this.ddlPorcentajeAvance.SelectedItem.Value),
                            ORGANO_RESPONSABLE_PDTE_VAL = txtOrganoResponsable.Text.Trim(),
                            OBJETIVO_CONTENIDO_ID = Convert.ToInt32(this.hdObjetivoId.Value),
                            OBSERVACIONES = txtAccObservaciones.Text.Trim()
                        };

                        c.CONTENIDO.Add(obj);

                        c.SaveChanges();
                        SuccessMessage = "Instrumento/actividad guardado correctamente.";
                        successMessage.Visible = true;
                    }
                    catch (Exception ex)
                    {
                        logger.Error("Error al crear nuevo instrumento/actividad (AdminPlan.aspx). Error: " + ex.Message + " " + ex.InnerException);
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

                int iDeptoId = getDptoId();
                using (Entities c = new Entities())
                {

                    OBJETIVO obj = new OBJETIVO
                    {
                        AUTOR_CREACION_USUARIO_ID = User.Identity.GetUserId().ToString(),
                        DEPARTAMENTO_ID = iDeptoId,
                        ESTADO_VALIDACION_ID = ESTADOS_VALIDACION.PDTE_VALIDAR,
                        FECHA_CREACION = DateTime.Now,                        
                        OBJETIVO_ESTRATEGICO_PDTE_VAL = this.ObjetivoEstrategicoPV.Text.Trim(),
                        TIPO_CAMBIO_CONTENIDO_ID = TIPO_CAMBIO_CONTENIDO.ALTA,
                        VISIBLE = false
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

            if ((!String.IsNullOrEmpty(this.ObjetivoEstrategicoPV.Text)))
            {
                int iObjetivoId = Convert.ToInt32(this.hdObjetivoIdEdit.Value);
                using (Entities c = new Entities())
                {
                    try
                    {
                        OBJETIVO obj = c.CONTENIDO.OfType<OBJETIVO>().Where(o => o.CONTENIDO_ID == iObjetivoId).FirstOrDefault();
                        if (obj.YaPublicado())
                        {
                            obj.FECHA_MODIFICACION = DateTime.Now;
                            obj.AUTOR_MODIFICACION_USUARIO_ID = User.Identity.GetUserId().ToString();
                            obj.TIPO_CAMBIO_CONTENIDO_ID = TIPO_CAMBIO_CONTENIDO.MODIFICACION;
                        }
                        else
                        {
                            //Si todavía no ha sido publicado lo trataremos como un alta
                            obj.FECHA_CREACION = DateTime.Now;
                            obj.AUTOR_CREACION_USUARIO_ID = User.Identity.GetUserId().ToString();
                            obj.TIPO_CAMBIO_CONTENIDO_ID = TIPO_CAMBIO_CONTENIDO.ALTA;
                        }

                        //Si hay cambios guardamos si no no, para no marcar como modificado
                        bool bHayCambios = false;

                        if (String.IsNullOrEmpty(obj.OBJETIVO_ESTRATEGICO_PDTE_VAL))
                            bHayCambios = true;
                        else if (obj.OBJETIVO_ESTRATEGICO_PDTE_VAL.Trim() != this.ObjetivoEstrategicoPV.Text.Trim())
                        {
                            bHayCambios = true;
                        }

                        if (bHayCambios)
                        {
                            obj.OBJETIVO_ESTRATEGICO_PDTE_VAL = this.ObjetivoEstrategicoPV.Text.Trim();
                            obj.ESTADO_VALIDACION_ID = ESTADOS_VALIDACION.PDTE_VALIDAR;

                            c.CONTENIDO.Attach(obj);
                            c.Entry(obj).State = EntityState.Modified;
                            c.SaveChanges();

                            SuccessMessage = "Objetivo actualizado correctamente.";
                            successMessage.Visible = true;
                        }                       
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("Error al actualizar objetivo (AdminPlan.aspx). Error: " + ex.Message + " " + ex.InnerException);
                        Notificaciones.NotifySystemOps(ex);
                        ErrorMessage = "Error al actualizar nuevo objetivo.";
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
    protected void btnActualizaAcc_Click(object sender, EventArgs e)
    {
        if (ModelState.IsValid)
        {

            int iObjetivoId = Convert.ToInt32(this.hdObjetivoId.Value);
            using (Entities c = new Entities())
            {
                try
                {
                    ACCION obj = c.CONTENIDO.OfType<ACCION>().Where(o => o.CONTENIDO_ID == iObjetivoId).FirstOrDefault();

                    bool bHayCambios = false;

                    if (String.IsNullOrEmpty(obj.INSTRUMENTOS_ACT_PDTE_VAL))
                        bHayCambios = true;
                    else if (
                         obj.INSTRUMENTOS_ACT_PDTE_VAL.Trim() != this.txtInstrumentosAct.Text.Trim() ||
                         obj.SEGUIMIENTO_PDTE_VAL.Trim() != this.txtSeguimiento.Text.Trim() ||
                         obj.RECURSOS_HUMANOS_PDTE_VAL.Trim() != this.txtRecursosHumanos.Text.Trim() ||
                         obj.COSTE_ECONOMICO_PDTE_VAL.Trim() != this.txtCosteEconomico.Text.Trim() ||
                         obj.INDICADOR_SEGUIMIENTO_PDTE_VAL.Trim() != this.txtIndSeguimiento.Text.Trim() ||
                         obj.TEMPORALIDAD_PDTE_VAL.Trim() != this.txtTemporalidad.Text.Trim() ||
                         obj.INDICADOR_SEGUIMIENTO_PDTE_VAL.Trim() != this.txtIndSeguimiento.Text.Trim() ||
                        //obj.ESTADO_SEGUIMIENTO_ID_PDTE_VAL.ToString() != this.ddlEstadoSeguimiento.SelectedItem.Value ||
                        obj.PORCENTAJE_AVANCE_PDTE_VAL.ToString() != this.ddlPorcentajeAvance.SelectedItem.Value ||
                        obj.ORGANO_RESPONSABLE_PDTE_VAL.Trim() != this.txtOrganoResponsable.Text.Trim()
                        )
                    {
                        bHayCambios = true;
                    }
                    else if (!String.IsNullOrEmpty(obj.MEDIOS_OTROS_PDTE_VAL))
                    {
                        if (obj.MEDIOS_OTROS_PDTE_VAL.Trim() != this.txtMediosOtros.Text.Trim())
                            bHayCambios = true;
                    }
                    else if (!String.IsNullOrEmpty(this.txtMediosOtros.Text.Trim()))
                        bHayCambios = true;

                    if (bHayCambios)
                    {
                        if (obj.YaPublicado())
                        {
                            obj.FECHA_MODIFICACION = DateTime.Now;
                            obj.AUTOR_MODIFICACION_USUARIO_ID = User.Identity.GetUserId().ToString();
                            obj.TIPO_CAMBIO_CONTENIDO_ID = TIPO_CAMBIO_CONTENIDO.MODIFICACION;
                        }
                        else
                        {
                            //Si todavía no ha sido publicado lo trataremos como un alta
                            obj.FECHA_CREACION = DateTime.Now;
                            obj.AUTOR_CREACION_USUARIO_ID = User.Identity.GetUserId().ToString();
                            obj.TIPO_CAMBIO_CONTENIDO_ID = TIPO_CAMBIO_CONTENIDO.ALTA;
                        }

                        obj.INSTRUMENTOS_ACT_PDTE_VAL = this.txtInstrumentosAct.Text.Trim();
                        obj.SEGUIMIENTO_PDTE_VAL = this.txtSeguimiento.Text.Trim();
                        obj.RECURSOS_HUMANOS_PDTE_VAL = this.txtRecursosHumanos.Text.Trim();
                        obj.COSTE_ECONOMICO_PDTE_VAL = this.txtCosteEconomico.Text.Trim();
                        obj.INDICADOR_SEGUIMIENTO_PDTE_VAL = this.txtIndSeguimiento.Text.Trim();
                        obj.MEDIOS_OTROS_PDTE_VAL = this.txtMediosOtros.Text.Trim();
                        obj.TEMPORALIDAD_PDTE_VAL = this.txtTemporalidad.Text.Trim();
                        obj.INDICADOR_SEGUIMIENTO_PDTE_VAL = this.txtIndSeguimiento.Text.Trim();
                        //if (this.ddlEstadoSeguimiento.SelectedItem.Value != "-1")
                        //    obj.ESTADO_SEGUIMIENTO_ID_PDTE_VAL = Convert.ToInt32(this.ddlEstadoSeguimiento.SelectedItem.Value);
                        if (this.ddlPorcentajeAvance.SelectedItem.Value != "-1")
                            obj.PORCENTAJE_AVANCE_PDTE_VAL = Convert.ToByte(this.ddlPorcentajeAvance.SelectedItem.Value);
                        obj.ORGANO_RESPONSABLE_PDTE_VAL = this.txtOrganoResponsable.Text.Trim();
                        obj.OBSERVACIONES = this.txtAccObservaciones.Text.Trim();
                        obj.ESTADO_VALIDACION_ID = ESTADOS_VALIDACION.PDTE_VALIDAR;

                        c.CONTENIDO.Attach(obj);
                        c.Entry(obj).State = EntityState.Modified;
                        c.SaveChanges();

                        SuccessMessage = "Instrumento/actividad actualizado correctamente.";
                        successMessage.Visible = true;
                    }
                    else
                    {
                        //no marcamos como cambio si actualizan observaciones
                        obj.OBSERVACIONES = this.txtAccObservaciones.Text.Trim();
                        c.CONTENIDO.Attach(obj);
                        c.Entry(obj).State = EntityState.Modified;
                        c.SaveChanges();
                        SuccessMessage = "Instrumento/actividad actualizado correctamente.";
                        successMessage.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    logger.Fatal("Error al actualizar Instrumento/actividad (AdminPlan.aspx). Error: " + ex.Message + " " + ex.InnerException);
                    Notificaciones.NotifySystemOps(ex);
                    ErrorMessage = "Error al actualizar nuevo Instrumento/actividad.";
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

    protected void btnEliminaAcc_Click(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(Request["__EVENTARGUMENT"]))
        {
            using (Entities c = new Entities())
            {


                try
                {
                    int id = Convert.ToInt32(Request["__EVENTARGUMENT"]);
                    int iDeptoId = getDptoId();
                    ACCION acc = c.CONTENIDO.OfType<ACCION>().SingleOrDefault(x => x.CONTENIDO_ID == id);

                    if (acc.TieneAcceso(User.Identity.GetUserId()))
                    {


                        if (acc.YaPublicado())
                        {
                            //Marcamos el cambio como eliminación
                            acc.FECHA_MODIFICACION = DateTime.Now;
                            acc.AUTOR_MODIFICACION_USUARIO_ID = User.Identity.GetUserId().ToString();
                            acc.TIPO_CAMBIO_CONTENIDO_ID = TIPO_CAMBIO_CONTENIDO.ELIMINADO;
                            acc.ESTADO_VALIDACION_ID = ESTADOS_VALIDACION.PDTE_VALIDAR;

                            c.CONTENIDO.Attach(acc);
                            c.Entry(acc).State = EntityState.Modified;
                            c.SaveChanges();

                            SuccessMessage = "Instrumento/actividad marcado para eliminación (se eliminará en la validación).";
                            successMessage.Visible = true;
                        }
                        else
                        {
                            //Si todavía no ha sido publicado lo eliminamos
                            c.CONTENIDO.Remove(acc);
                            c.SaveChanges();
                            Response.Redirect("AdminPlan?id=" + iDeptoId.ToString() + "&m=" + "Instrumento/actividad eliminado correctamente.", false);
                        }
                    }
                    else
                        logger.Info("Acceso - AdminPlan.aspx Usuario: " + User.Identity.GetUserId() + " sin permiso de modificación en Acción: " + id.ToString());



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
    protected void btnEliminaObj_Click(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(Request["__EVENTARGUMENT"]))
        {
            using (Entities c = new Entities())
            {


                try
                {
                    int id = Convert.ToInt32(Request["__EVENTARGUMENT"]);
                    int iDeptoId = getDptoId();
                    OBJETIVO obj = c.CONTENIDO.OfType<OBJETIVO>().SingleOrDefault(x => x.CONTENIDO_ID == id);

                    if (obj.TieneAcceso(User.Identity.GetUserId()))
                    {   
                        if (obj.YaPublicado())
                        {
                            //Marcamos el cambio como eliminación
                            obj.FECHA_MODIFICACION = DateTime.Now;
                            obj.AUTOR_MODIFICACION_USUARIO_ID = User.Identity.GetUserId().ToString();
                            obj.TIPO_CAMBIO_CONTENIDO_ID = TIPO_CAMBIO_CONTENIDO.ELIMINADO;
                            obj.ESTADO_VALIDACION_ID = ESTADOS_VALIDACION.PDTE_VALIDAR;

                            c.CONTENIDO.Attach(obj);
                            c.Entry(obj).State = EntityState.Modified;
                            c.SaveChanges();

                            SuccessMessage = "Objetivo marcado para eliminación (se deberá validar previamente).";
                            successMessage.Visible = true;
                        }
                        else
                        {
                            //Si todavía no ha sido publicado lo eliminamos
                            OBJETIVO.EliminaObjetivo(obj.CONTENIDO_ID);                                                        
                            Response.Redirect("AdminPlan?id=" + iDeptoId.ToString() + "&m=" + "Objetivo eliminado correctamente.", false);
                        }
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

        int iObjetivoId = Convert.ToInt32(this.hdObjetivoId.Value);
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


    //***********************************************************************************************EXPORTACION******************************

    #region ExportacionListados

    //protected void btnImprimirInforme_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        InformeDepartamento inf = new InformeDepartamento();
    //        Document doc = inf.genera();

    //        System.IO.MemoryStream ms = new System.IO.MemoryStream();
    //        //iTextSharp.text.Document doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 30f, 30f, 30f, 30f);
    //        //iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, ms);

            

    //        //Response.Clear();
    //        //Response.ContentType = "application/pdf";
    //        //Response.AddHeader("Content-Disposition",
    //        //    "attachment;filename=\"Plan_de_Gobierno.pdf\"");
    //        //Response.Cache.SetCacheability(HttpCacheability.NoCache);

    //        //Response.BinaryWrite(ms.ToArray());

    //        //Response.Flush();

    //        //Response.End();



    //        using (PdfWriter writer = PdfWriter.GetInstance(doc, ms))
    //        {
    //            /*doc.Open();
    //            document.Add(new Paragraph("Hello World"));
    //            document.Close();*/
    //            writer.Close();
    //            ms.Close();
    //            Response.ContentType = "pdf/application";
    //            Response.AddHeader("content-disposition", "attachment;filename=First_PDF_document.pdf");
    //            Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        //TODO*************
    //    }
    //}
    
    
    protected void InformeExcel(int iDptoId)
    {
        try
        {
            
            System.IO.MemoryStream stream;

            //using (Entities c = new Entities())
            {
                //DEPARTAMENTO dep = c.DEPARTAMENTO.Find(iDptoId);
                
                //DataTable tabla = Utils.ToDataTable<CONTENIDO>(dep.CONTENIDO.ToList());                
                
                stream = new System.IO.MemoryStream();
                
                using (SpreadsheetDocument document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook, true))
                {

                    WorkbookPart workbookPart = document.AddWorkbookPart();
                    workbookPart.Workbook = new Workbook();

                    WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                    MergeCells mergeCells;
                    
                    var sheetData = InformesExcel.HojaExcelInforme(iDptoId, null, false, out mergeCells);

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
                        SheetData sheetDataPV = InformesExcel.HojaExcelInforme(iDptoId, null, true, out mergeCellsPV);

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
            Response.AddHeader("content-disposition", "attachment; filename=Plan_de_Gobierno_"+ DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx");
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

    protected void btnImprimirInformeExcel_Click(object sender, EventArgs e)
    {        
        InformeExcel(getDptoId());
    }


    protected void btnExportarDatos_Click(object sender, EventArgs e)
    {
        int? iDptoId = null;
        iDptoId = getDptoId();

        if (this.ddlTipoInforme.SelectedItem.Value == "PDF")
        {
            Response.Redirect("~/View/reports/ViewReport.aspx?id=" + iDptoId +"&t=plan&f=PDF", true);          
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

    #endregion


}
