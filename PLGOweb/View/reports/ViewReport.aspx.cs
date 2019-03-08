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
using Microsoft.Reporting.WebForms;
using System.Data;

namespace PLGOweb.View.reports
{
    public partial class ViewReport : System.Web.UI.Page
    {
        DataSet ds = null;

        void LegPlanSubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            if (ds != null)
            {
                e.DataSources.Add(new ReportDataSource("DataSetPLGO", ds.Tables["DataTable1"]));
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {


            if (!IsPostBack)
            {


                if (!String.IsNullOrEmpty(Request["t"]))
                {
                    string sInforme = Request["t"].ToString();
                    bool exportacionDirecta = false;
                    bool bFormatoCorrecto = false;
                    string sFormato = "";
                    if (!String.IsNullOrEmpty(Request["f"]))
                    {
                        sFormato = Request["f"].ToString().ToUpper();
                        exportacionDirecta = true;
                    }

                    Warning[] warnings;
                    string[] streamIds;
                    string mimeType = string.Empty;
                    string encoding = string.Empty;
                    string extension = string.Empty;
                    string contentType = string.Empty;
                    string nombreArchivo = string.Empty;
                    byte[] bytes = null;



                    ReportViewer1.ProcessingMode = ProcessingMode.Local;
                    ReportViewer1.AsyncRendering = false;
                    ReportViewer1.SizeToReportContent = true;
                    ReportViewer1.Width = Unit.Percentage(100);
                    ReportViewer1.Height = Unit.Percentage(100);
                    ReportViewer1.LocalReport.EnableHyperlinks = true;
                    
                    

                    ReportViewer1.ZoomMode = ZoomMode.PageWidth;
                    

                    if (sInforme == "plan")
                    {
                        int? id = null;
                        int? LegislaturaId = null;

                        if (!String.IsNullOrEmpty(Request["id"]))
                        {
                            int i;
                            Int32.TryParse(Request["id"], out i);
                            id = i;
                        }

                        if (!String.IsNullOrEmpty(Request["lid"]))
                        {
                            int i;
                            Int32.TryParse(Request["lid"], out i);
                            LegislaturaId = i;
                        }


                        if (id.HasValue)
                            ReportViewer1.LocalReport.ReportPath = Server.MapPath(@"PlanGobiernoXdpto.rdlc");
                        else
                        {
                            ReportViewer1.LocalReport.ReportPath = Server.MapPath(@"PlanGobierno.rdlc");
                          //  ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(LegPlanSubreportProcessingEventHandler);
                        }


                                                
                        ds = LEGISLATURA.Informe(id, Server.MapPath("informePLGO.xsd"), LegislaturaId);

                        ReportDataSource datasource = new ReportDataSource("DataSetPLGO", ds.Tables["DataTable1"]);
                        ReportViewer1.LocalReport.DataSources.Clear();
                        ReportViewer1.LocalReport.DataSources.Add(datasource);
                        nombreArchivo = "PlanDeGobierno";
                    }
                 


                    if (sFormato == "PDF")
                    {
                        bytes = ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
                        contentType = "application/pdf";
                        bFormatoCorrecto = true;
                    }
                    else if (sFormato == "XLSX")
                    {
                        bytes = ReportViewer1.LocalReport.Render("EXCELOPENXML", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
                        contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        bFormatoCorrecto = true;
                    }
                    else if (sFormato == "DOCX")
                    {
                        bytes = ReportViewer1.LocalReport.Render("WORDOPENXML", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
                        contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.template";
                        bFormatoCorrecto = true;
                    }
                  


                    if (exportacionDirecta && bFormatoCorrecto)
                    {                    
                       
                        Response.Buffer = true;
                        Response.Clear();
                        Response.ContentType = contentType;
                        Response.AddHeader("content-disposition", "attachment; filename=" + nombreArchivo + "." + extension);
                        Response.AddHeader("content-length", bytes.Length.ToString());
                        Response.BinaryWrite(bytes);
                        Response.Flush();
                    }
                }
            }
        }
            
    }
}