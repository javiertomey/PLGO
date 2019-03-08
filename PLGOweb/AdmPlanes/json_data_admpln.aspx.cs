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
using PLGOdata;
using Newtonsoft.Json; //Serializado JSON
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;
using NLog;
using Microsoft.AspNet.Identity;
using System.Diagnostics;


public partial class json_data_admnpln : System.Web.UI.Page
{
    NLog.Logger logger = LogManager.GetCurrentClassLogger();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!(User.IsInRole(AspNetRoles.ADMINISTRADOR)) && !(User.IsInRole(AspNetRoles.VALIDADOR)))
        {
            //Intenta acceder un usuario sin permisos
            logger.Info("No autorizadoError en json_data_admpln.aspx. Usuario: " + User.Identity.GetUserId());            
        }
        
        HttpContext ctx = HttpContext.Current;
        ctx.Response.ContentType = "application/json";
        
        if (!String.IsNullOrEmpty(Request["op"]))
        {

            string sOperacion = Request["op"].ToString();

            //carga del listado
            if (sOperacion == "loadO")
            {
                using (Entities c = new Entities())
                {
                    //Tenemos que recuperar del usuario el id de Entidad relacionado

                    int? iDptoId = null;

                    if ((User.IsInRole(AspNetRoles.ADMINISTRADOR)) || (User.IsInRole(AspNetRoles.VALIDADOR)))
                    {

                        iDptoId = Convert.ToInt32(Request["id"]);
                        if (Request["id"] == null) Response.Redirect("../Default.aspx", true);
                    }
                    else
                    {
                        try
                        {
                            iDptoId = c.USUARIOS.Find(User.Identity.GetUserId().ToString()).DEPARTAMENTOID;
                        }
                        catch (Exception ex)
                        {
                            logger.Fatal("Error en load / json_data_adminmpln.aspx. Error: " + ex.Message + " " + ex.InnerException);
                        }
                    }
                    /////

                    if (iDptoId.HasValue)
                    {

                      

                        c.Configuration.ProxyCreationEnabled = false;
                        //Para poder ver las subclases en el json (si no se quiere más niveles hay que indicar la anotacion JsonIgnore -  https://ardalis.com/adding-attributes-to-generated-classes
                        var results = c.CONTENIDO.Include("AUTOR_CREACION").Include("AUTOR_MODIFICACION").Include("VALIDADOR")
                            .Include("ESTADOS_VALIDACION").Include("TIPO_CAMBIO_CONTENIDO").Include("DEPARTAMENTO")
                            .OfType<OBJETIVO>().Where(st => st.DEPARTAMENTO_ID == iDptoId.Value).ToList();

                        //Para poder listar por Json sin necesidad de personalizarlo
                        Response.Write(JsonConvert.SerializeObject(results, Formatting.Indented,
                            new JsonSerializerSettings
                            {
                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                                 MaxDepth = 1
                            }));
                    }

                }

            }
            else if (sOperacion == "loadA")
            {
                try
                {
                    if (!String.IsNullOrEmpty(Request["id"]))
                    {
                        int? iDptoId = null;
                        int iObjetivoId = Convert.ToInt32(Request["id"]);

                        //Si no es administrador ni validador restringimos por el departamento asociado
                        if (!(User.IsInRole(AspNetRoles.ADMINISTRADOR)) && !(User.IsInRole(AspNetRoles.VALIDADOR)))
                        {
                            using (Entities c = new Entities())
                            {
                                try
                                {
                                    iDptoId = c.USUARIOS.Find(User.Identity.GetUserId().ToString()).DEPARTAMENTOID;
                                    if (iDptoId == null)
                                        throw new Exception("El usuario " + User.Identity.GetUserId().ToString() + " no tiene asociado departamento");
                                }
                                catch (Exception ex)
                                {
                                    logger.Warn("Error en AdminPlan.aspx. Error: " + ex.Message + " " + ex.InnerException);
                                    Response.Redirect("../Default.aspx", true);
                                }
                            }


                        }
                        using (Entities c = new Entities())
                        {
                            c.Configuration.ProxyCreationEnabled = false;
                            if (iDptoId.HasValue)
                            {
                                
                                var results = c.CONTENIDO.Include("AUTOR_CREACION").Include("AUTOR_MODIFICACION").Include("VALIDADOR")
                                    .Include("ESTADOS_VALIDACION").Include("TIPO_CAMBIO_CONTENIDO").Include("DEPARTAMENTO")
                                    .OfType<ACCION>().Where(st => st.DEPARTAMENTO_ID == iDptoId.Value && st.OBJETIVO_CONTENIDO_ID == iObjetivoId).ToList();
                                //Para poder listar por Json sin necesidad de personalizarlo:
                                Response.Write(JsonConvert.SerializeObject(results, Formatting.Indented,
                                    new JsonSerializerSettings
                                    {
                                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                    }));

                            }
                            else
                            {
                                var results = c.CONTENIDO.Include("AUTOR_CREACION").Include("AUTOR_MODIFICACION").Include("VALIDADOR")
                                    .Include("ESTADOS_VALIDACION").Include("TIPO_CAMBIO_CONTENIDO").Include("DEPARTAMENTO")
                                    .OfType<ACCION>().Where(st => st.OBJETIVO_CONTENIDO_ID == iObjetivoId).ToList();
                                //Para poder listar por Json sin necesidad de personalizarlo:
                                Response.Write(JsonConvert.SerializeObject(results, Formatting.Indented,
                                    new JsonSerializerSettings
                                    {
                                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                    }));
                            }
                        }                        
                    }

                }
                catch (Exception ex)
                {
                    logger.Error("Error en getSubTiposDocAdmin / json_data_adminment.aspx. Error: " + ex.Message + " " + ex.InnerException);
                }
            }
            else if (sOperacion == "loadL")
            {
                //Carga de legislaturas
                using (Entities c = new Entities())
                {
                        c.Configuration.ProxyCreationEnabled = false;
                        var results = c.LEGISLATURA.ToList().OrderByDescending(l => l.ACTUAL);

                        //Para poder listar por Json sin necesidad de personalizarlo
                        Response.Write(JsonConvert.SerializeObject(results, Formatting.Indented,
                            new JsonSerializerSettings
                            {
                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                            }));
                }


            }
            else if (sOperacion == "loadD")                    
            {
                //Carga de departamentos
                if (!String.IsNullOrEmpty(Request["id"]))
                {                    
                    int iLegislaturaId = Convert.ToInt32(Request["id"]);

                    using (Entities c = new Entities())
                    {
                        c.Configuration.ProxyCreationEnabled = false;
                        var results = c.DEPARTAMENTO.Where(d => d.LEGISLATURA_ID == iLegislaturaId).ToList().OrderBy(d => d.DESCRIPCION);

                        //Para poder listar por Json sin necesidad de personalizarlo
                        Response.Write(JsonConvert.SerializeObject(results, Formatting.Indented,
                            new JsonSerializerSettings
                            {
                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                            }));
                    }
                }

            }
            else if (sOperacion == "loadU")
            {
                //Carga de usuarios de departamento
                if (!String.IsNullOrEmpty(Request["id"]))
                {
                    int iDptoId = Convert.ToInt32(Request["id"]);

                    using (Entities c = new Entities())
                    {
                        c.Configuration.ProxyCreationEnabled = false;
                        var results = c.USUARIOS.Include("AspNetUsers").Where(d => d.DEPARTAMENTOID == iDptoId).ToList().OrderBy(d => d.APELLIDOS);

                        //Para poder listar por Json sin necesidad de personalizarlo
                        Response.Write(JsonConvert.SerializeObject(results, Formatting.Indented,
                            new JsonSerializerSettings
                            {
                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                            }));
                    }
                }

            }
            else if (sOperacion == "v")
            {
                Response.Write("Web: " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
                Response.Write("\nData: " + AppPLGOdata.Version());
            }
            
        }

    }
}