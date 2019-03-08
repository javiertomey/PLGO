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

using PLGO;
using PLGOdata;
using System.Text;
using NLog;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Data;
using System.Data.Common;

public partial class json_data_p : System.Web.UI.Page
{
    NLog.Logger logger = LogManager.GetCurrentClassLogger();

    protected void Page_Load(object sender, EventArgs e)
    {

        HttpContext ctx = HttpContext.Current;
        ctx.Response.ContentType = "application/json";

        if (!String.IsNullOrEmpty(Request["op"]))
        {

            string sOperacion = Request["op"].ToString();
            string sEcho = "";
            int? displayLength = null;
            int? displayStart = null;
            int? sortCol = null;
            string sortOrder = null;

            if (!String.IsNullOrEmpty(Request["sEcho"]))
                sEcho = HttpContext.Current.Request.Params["sEcho"];
            if (!String.IsNullOrEmpty(Request["iDisplayLength"]))
                displayLength = int.Parse(HttpContext.Current.Request.Params["iDisplayLength"]);
            if (!String.IsNullOrEmpty(Request["iDisplayStart"]))
                displayStart = int.Parse(HttpContext.Current.Request.Params["iDisplayStart"]);
            if (!String.IsNullOrEmpty(Request["sSortDir_0"]))
                sortOrder = HttpContext.Current.Request.Params["sSortDir_0"];
            if (!String.IsNullOrEmpty(Request["iSortCol_0"]))
                sortCol = int.Parse(HttpContext.Current.Request.Params["iSortCol_0"]);


            if (sOperacion == "O")
            {
                //Carga de Objetivo y sus Acciones
                if (Request["id"] != null)
                {
                    using (Entities c = new Entities())
                    {

                        int iDeptId;
                        try
                        {
                            iDeptId = Convert.ToInt32(Request["id"]);

                            DEPARTAMENTO d = c.DEPARTAMENTO.Find(Convert.ToInt32(Request["id"]));
                            LEGISLATURA l = c.LEGISLATURA.Find(d.LEGISLATURA_ID);
                            if (d.VISIBLE && l.ACTUAL)
                            {
                                c.Configuration.ProxyCreationEnabled = false;
                                var results = c.CONTENIDO.Include("ACCION").OfType<OBJETIVO>().Where(st => st.DEPARTAMENTO_ID == iDeptId && st.VISIBLE == true && st.OBJETIVO_ESTRATEGICO != null).ToList();

                                //Para poder listar por Json sin necesidad de personalizarlo
                                /*Response.Write(JsonConvert.SerializeObject(results, Formatting.Indented,
                                        new JsonSerializerSettings
                                        {
                                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                        }, ObjSerializer));*/

                                Response.Write(JsonConvert.SerializeObject(results, Formatting.Indented, new JsonConverter[] { new ObjSerializer() }));


                                /* //Así con json personalizado
                                    List<OBJETIVO> list = new List<OBJETIVO>();                            
                                        list = c.CONTENIDO.OfType<OBJETIVO>().Where(st => st.DEPARTAMENTO_ID == iDptoId.Value).ToList();

                                        //Response.Write("{ \"data\":");
                                        Response.Write(Json.GetJson(list) );
                                        //Response.Write("}");*/
                            }

                        }
                        catch (Exception ex)
                        {
                            logger.Error("View/json_data.aspx.cs (op=O). Error: " + ex.Message + " " + ex.InnerException);
                            Notificaciones.NotifySystemOps(ex, "View/json_data.aspx.cs (op=O). Error: " + ex.Message + " " + ex.InnerException);
                        }

                    }
                }

            }            
            else if (sOperacion == "dG")
            {
                //Datos Generales: num objetivos, num acciones, fecha de última actualización
                using (Entities c = new Entities())
                {
                    using (var conn = c.Database.Connection)
                    {
                        try
                        {
                            int iAcciones = 0;
                            int iObjetivos = 0;
                            List<DateTime> dtFechas = new List<DateTime>();

                            /*  Response.Write("[{\"objetivos\": XXX, \"acciones\":XXX, \"ultima_mod\":\"XXX\"}]");*/

                            var cmd = conn.CreateCommand();
                            conn.Open();
                            cmd.CommandText = @"select COUNT(A.CONTENIDO_ID) as acciones, max(fecha_modificacion) as ultimo_modificado, max (fecha_creacion) as ultimo_creado
                                from PLGO_OWN.accion a
                                inner join PLGO_OWN.CONTENIDO c on c.contenido_id = A.CONTENIDO_ID
                                inner join PLGO_OWN.DEPARTAMENTO d on C.DEPARTAMENTO_ID = D.DEPARTAMENTO_ID
                                inner join PLGO_OWN.LEGISLATURA l on L.LEGISLATURA_ID = D.LEGISLATURA_ID
                                WHERE l.actual = 1 and d.visible = 1 and c.visible = 1 and C.TIPO_CAMBIO_CONTENIDO_ID <> 1
                                ";
                            using (var reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess))
                            {
                                while (reader.Read())
                                {
                                    iAcciones = reader.GetInt32(0);
                                    dtFechas.Add(reader.GetDateTime(1));
                                    dtFechas.Add(reader.GetDateTime(2));
                                }
                            }

                            cmd.CommandText = @"select COUNT(A.CONTENIDO_ID) as objetivos, max(fecha_modificacion) as ultimo_modificado, max (fecha_creacion) as ultimo_creado
                            from PLGO_OWN.OBJETIVO a
                            inner join PLGO_OWN.CONTENIDO c on c.contenido_id = A.CONTENIDO_ID
                            inner join PLGO_OWN.DEPARTAMENTO d on C.DEPARTAMENTO_ID = D.DEPARTAMENTO_ID
                            inner join PLGO_OWN.LEGISLATURA l on L.LEGISLATURA_ID = D.LEGISLATURA_ID
                            WHERE l.actual = 1 and d.visible = 1 and c.visible = 1 and C.TIPO_CAMBIO_CONTENIDO_ID <> 1
                                ";


                            using (var reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess))
                            {
                                while (reader.Read())
                                {
                                    iObjetivos = reader.GetInt32(0);
                                    dtFechas.Add(reader.GetDateTime(1));
                                    dtFechas.Add(reader.GetDateTime(2));
                                }
                            }
                            Response.Write("[{\"objetivos\":" + iObjetivos + ",\"acciones\":" + iAcciones + ",\"ultima_mod\":\"" + dtFechas.OrderByDescending(x => x).First() + "\"}]");


                        }
                        catch (Exception ex)
                        {
                            logger.Error("View/json_data.aspx.cs (op=dG). Error: " + ex.Message + " " + ex.InnerException);
                            Notificaciones.NotifySystemOps(ex, "View/json_data.aspx.cs (op=dG). Error: " + ex.Message + " " + ex.InnerException);
                        }
                    }
                }

            }           
            //Detalle Departamento
            else if (sOperacion == "dD")
            {
                if (Request["id"] != null)
                {
                    try
                    {
                        using (Entities c = new Entities())
                        {
                            if (Request["id"] == null)
                                Response.Redirect("VerDepartamentos.aspx", false);
                            else
                            {
                                DEPARTAMENTO d = c.DEPARTAMENTO.Find(Convert.ToInt32(Request["id"]));
                                if (d.VISIBLE)
                                    Response.Write("{\"desc\":\"" + d.DESCRIPCION + "\"}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error("View/json_data.aspx.cs (op=dD) ID= " + Request["id"] + ". Error: " + ex.Message + " " + ex.InnerException);
                        Notificaciones.NotifySystemOps(ex, "View/json_data.aspx.cs (op=dD) ID= " + Request["id"] + ". Error: " + ex.Message + " " + ex.InnerException);
                    }
                }
            }           
            //Cuadro de mando
            else if (sOperacion == "cm")
            {

                /*  Response.Write("{\"pLegT\": XXX, \"pObjM\":XXX, \"pObjNI\":\"XXX\", \"pObjI\":\"XXX\", \"pObjT\":\"XXX\"}");*/
                decimal dLegTranscurrida;
                int iObjetivosSinIniciar = 0;
                int iObjetivosIniciados = 0;
                int iObjetivosTerminados = 0;
                int iNumAcciones = 0;
                int iNumObjetivos = 0;
                int iSumaPorcenajesAvanceAcciones = 0;
                decimal dPorcSinIniciar = 0;
                decimal dPorcIniciados = 0;
                decimal dPorcTerminados = 0;
                decimal dPorcIndiceGlobalCumplimiento = 0;

                
                LEGISLATURA.GetDatosCuadroMando(out dLegTranscurrida, out iObjetivosSinIniciar, out iObjetivosIniciados, out iObjetivosTerminados,
                        out iSumaPorcenajesAvanceAcciones, out iNumAcciones, out iNumObjetivos,
                        out dPorcSinIniciar, out dPorcIniciados, out dPorcTerminados, out dPorcIndiceGlobalCumplimiento);
                
                dPorcTerminados = 100 - dPorcSinIniciar - dPorcIniciados; //nos aseguramos que la suma no sea mayor que 100%

                Response.Write("{\"pLegT\": " + dLegTranscurrida + ", \"pObjM\": " + dPorcIndiceGlobalCumplimiento + ", \"pObjNI\":" + dPorcSinIniciar + ", \"pObjI\":" + dPorcIniciados + ", \"pObjT\":" + dPorcTerminados + "}");

            }
            //Cuadro de mando por departamento
            else if (sOperacion == "cmd")
            {

                if (Request["id"] != null)
                {
                    try
                    {
                        using (Entities c = new Entities())
                        {
                            if (Request["id"] == null)
                                Response.Redirect("VerDepartamentos.aspx", false);
                            else
                            {
                                DEPARTAMENTO d = c.DEPARTAMENTO.Find(Convert.ToInt32(Request["id"]));
                                LEGISLATURA l = c.LEGISLATURA.Find(d.LEGISLATURA_ID);
                                if (d.VISIBLE && l.ACTUAL)
                                {
                                    //Response.Write("{\"desc\":\"" + d.DESCRIPCION + "\"}");

                                    decimal dLegTranscurrida = LEGISLATURA.GetTiempoLegislatura();
                                    decimal dPorcIndiceGlobalCumplimiento = d.GetEvolucion();                            
                                    string sFecha = "";

                                    using (var conn = c.Database.Connection)
                                    {

                                        var cmd = conn.CreateCommand();
                                        conn.Open();
                                        cmd.CommandText = @"select max(fecha_modificacion) as ultimo_modificado
                                        from PLGO_OWN.accion a
                                        inner join PLGO_OWN.CONTENIDO c on c.contenido_id = A.CONTENIDO_ID
                                        inner join PLGO_OWN.DEPARTAMENTO d on C.DEPARTAMENTO_ID = D.DEPARTAMENTO_ID
                                        inner join PLGO_OWN.LEGISLATURA l on L.LEGISLATURA_ID = D.LEGISLATURA_ID
                                        WHERE l.actual = 1 and d.visible = 1 and d.DEPARTAMENTO_ID = :DptoId  and c.visible = 1 and C.TIPO_CAMBIO_CONTENIDO_ID <> 1
                                        ";


                                        var parameter = cmd.CreateParameter();
                                        parameter.ParameterName = ":DptoId";
                                        parameter.Value = d.DEPARTAMENTO_ID;
                                        cmd.Parameters.Add(parameter);

                                        using (var reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess))
                                        {
                                            while (reader.Read())
                                            {
                                                try
                                                {
                                                    sFecha = reader.GetDateTime(0).ToShortDateString();
                                                }
                                                catch {
                                                    sFecha = "No disponible";
                                                }
                                            }
                                        }
                                    }
                                    Response.Write("{\"desc\":\"" + d.DESCRIPCION + "\", \"pLegT\": " + dLegTranscurrida + ", \"pObjM\": " + dPorcIndiceGlobalCumplimiento + ", \"um\":\"" + sFecha + "\"}");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error("View/json_data.aspx.cs (op=dD) ID= " + Request["id"] + ". Error: " + ex.Message + " " + ex.InnerException);
                        Notificaciones.NotifySystemOps(ex, "View/json_data.aspx.cs (op=dD) ID= " + Request["id"] + ". Error: " + ex.Message + " " + ex.InnerException);
                    }
                }
            }
        }

    }
}
