// Autor: Gobierno de Aragón
// 
// Derechos de explotación propiedad de la Gobierno de Aragón.
//
// Éste programa es software libre: usted tiene derecho a redistribuirlo y/o
// modificarlo bajo los términos de la Licencia EUPL European Public License 
// publicada por el organismo IDABC de la Comisión Europea, en su versión 1.1.
// o posteriores.
// 
// Éste programa se distribuye de buena fe, pero SIN NINGUNA GARANTÍA, incluso sin 
// las presuntas garantías implícitas de USABILIDAD o ADECUACIÓN A PROPÓSITO CONCRETO. 
// Para mas información consulte la Licencia EUPL European Public License.
// 
// Usted recibe una copia de la Licencia EUPL European Public License 
// junto con este programa, si por algún motivo no le es posible visualizarla, 
// puede consultarla en la siguiente URL: http://ec.europa.eu/idabc/servlets/Docb4f4.pdf?id=31980
// 
// You should have received a copy of the EUPL European Public 
// License along with this program.  If not, see http://ec.europa.eu/idabc/servlets/Docbb6d.pdf?id=31979
// 
// Vous devez avoir reçu une copie de la EUPL European Public
// License avec ce programme. Si non, voir http://ec.europa.eu/idabc/servlets/Doc5a41.pdf?id=31983
// 
// Sie sollten eine Kopie der EUPL European Public License zusammen mit
// diesem Programm. Wenn nicht, finden Sie da http://ec.europa.eu/idabc/servlets/Doc9dbe.pdf?id=31977
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PLGOdata;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;
using NLog;
using Microsoft.AspNet.Identity;
using System.Diagnostics;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using System.Data;

public partial class get_report : System.Web.UI.Page
{
    NLog.Logger logger = LogManager.GetCurrentClassLogger();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!(User.IsInRole(AspNetRoles.ADMINISTRADOR)) && !(User.IsInRole(AspNetRoles.VALIDADOR)))
        {
            //Intenta acceder un usuario sin permisos
            logger.Info("No autorizadoError en get_report.aspx. Usuario: " + User.Identity.GetUserId());            
        }
        

        ReportDocument rpt = new ReportDocument();        

        int? iDptoId = null;

        try
        {

            if (!String.IsNullOrEmpty(Request["id"]))
                iDptoId = Convert.ToInt32(Request["id"]);




            using (Entities c = new Entities())
            {
                List<DEPARTAMENTO> dptos = null;

                if (iDptoId.HasValue)
                    dptos = c.DEPARTAMENTO.Where(st => st.DEPARTAMENTO_ID == iDptoId.Value).ToList();
                else
                {
                    int iLegActual = LEGISLATURA.GetActualLegislatura();
                    dptos = c.DEPARTAMENTO.Where(st => st.LEGISLATURA_ID == iLegActual).OrderBy(st => st.ORDEN).ToList();
                }

                int iRow = 0;
                DataSet ds = new DataSet();
                ds.ReadXmlSchema(Server.MapPath("~/App_Code/informePLGO.xsd"));


                foreach (DEPARTAMENTO dep in dptos)
                {
                    List<OBJETIVO> objetivos;

                    objetivos = c.CONTENIDO.OfType<OBJETIVO>().Where(st => st.DEPARTAMENTO_ID == dep.DEPARTAMENTO_ID && st.VISIBLE == true).OrderBy(st => st.OBJETIVO_ESTRATEGICO).ToList();

                    foreach (OBJETIVO obj in objetivos)
                    {
                        List<ACCION> acciones;
                        acciones = c.CONTENIDO.OfType<ACCION>().Where(st => st.DEPARTAMENTO_ID == dep.DEPARTAMENTO_ID && st.OBJETIVO_CONTENIDO_ID == obj.CONTENIDO_ID && st.VISIBLE == true).OrderBy(st => st.INSTRUMENTOS_ACT).ToList();

                        foreach (ACCION acc in acciones)
                        {
                            DataRow dr = ds.Tables[0].NewRow();
                            dr[0] = dep.DESCRIPCION;
                            dr[1] = obj.OBJETIVO_ESTRATEGICO;
                            dr[2] = acc.INSTRUMENTOS_ACT;
                            dr[3] = acc.ORGANO_RESPONSABLE;
                            dr[4] = acc.RECURSOS_HUMANOS;
                            dr[5] = acc.COSTE_ECONOMICO;
                            dr[6] = acc.MEDIOS_OTROS;
                            dr[7] = acc.TEMPORALIDAD;
                            dr[8] = acc.SEGUIMIENTO;
                            dr[9] = acc.ESTADOS_SEGUIMIENTO.DESCRIPCION;

                            if (acc.FECHA_MODIFICACION.HasValue)
                                dr[10] = acc.FECHA_MODIFICACION.Value.ToShortDateString();
                            else
                                dr[10] = acc.FECHA_CREACION.ToShortDateString();

                            dr[11] = dep.ORDEN;

                            ds.Tables[0].Rows.Add(dr);
                        }
                    }
                }
                

                    rpt.Load(MapPath(Request.ApplicationPath + "/AdmPlanes/InformeAdmPlanes.rpt"));
                  
                    rpt.Database.Tables["DataTable1"].SetDataSource(ds);

                    string sFormato = "";
                    string sNombreArchivo = "EstadoSeguimientoPlanDeGobierno";

                    if (!String.IsNullOrEmpty(Request["format"]))
                        sFormato = Request["format"].ToUpper();

                    
                    if (String.IsNullOrEmpty(sFormato))
                        //por defecto exportamos en PDF
                        rpt.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, sNombreArchivo);

                    if (sFormato.Equals("PDF"))                    
                        rpt.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, sNombreArchivo);
                    else if (sFormato.Equals("DOC"))
                        rpt.ExportToHttpResponse(ExportFormatType.WordForWindows, Response, false, sNombreArchivo);
                    else if (sFormato.Equals("XLS"))
                        rpt.ExportToHttpResponse(ExportFormatType.Excel, Response, false, sNombreArchivo);
                    else if (sFormato.Equals("XLSREC"))
                        rpt.ExportToHttpResponse(ExportFormatType.ExcelRecord, Response, false, sNombreArchivo);                    
                    else if (sFormato.Equals("RTF"))
                        rpt.ExportToHttpResponse(ExportFormatType.RichText, Response, false, sNombreArchivo);
                    /*else if (sFormato.Equals("XML")) //NO ES COMPATIBLE CON CR10, si que va con CR13
                        rpt.ExportToHttpResponse(ExportFormatType.Xml, Response, false, sNombreArchivo);                */
            }
        }
        catch (Exception ex)
        {
            logger.Error("Error en AdmPlanes/get_report.aspx  get_report(). Error: " + ex.Message + " " + ex.InnerException);
        }

    }
}