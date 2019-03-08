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
using System.Text;
using System.Threading.Tasks;

using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PLGOdata
{
    public class InformeDepartamento : PdfWriter
    {
       

        // This is the contentbyte object of the writer
        PdfContentByte cb;

        // we will put the final number of pages in a template
        PdfTemplate headerTemplate, footerTemplate;

        // this is the BaseFont we are going to use for the header / footer
        BaseFont bf = null;

        // This keeps track of the creation time
        DateTime PrintTime = DateTime.Now;

        protected iTextSharp.text.Font _fb = FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.NORMAL);
        protected iTextSharp.text.Font _fb8 = FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.NORMAL);
        protected iTextSharp.text.Font _fb8w = FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.WHITE);
        protected iTextSharp.text.Font _f = FontFactory.GetFont(FontFactory.HELVETICA, 10);
        protected iTextSharp.text.Font _f10 = FontFactory.GetFont(FontFactory.HELVETICA, 8);




        /// <summary>
        /// Genera el informe
        /// </summary>        
        public Document genera()
        {
            try
            {
                // HeaderAndFooter PdfPageEventHelper = new HeaderAndFooter();


                /*  using (Document document = new Document(PageSize.A4.Rotate()))
                  {
                      // step 2
                      PdfWriter writer = PdfWriter.GetInstance(document, stream);
                      // step 3
                      document.Open();
                      // step 4
                      DrawTimeTable(writer.DirectContentUnder);
                      DrawTimeSlots(writer.DirectContent);
                  }*/


                //iTextSharp.text.PageSize.A4

                //_document = new Document(PageSize.A4, 60, 60, 120, 80);
                //iTextSharp.text.Document doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 30f, 30f, 30f, 30f);

                using (Document doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 60, 60, 120, 80))
                {
                    doc.Open();
                    //metadatos
                    doc.AddCreationDate();                    
                    doc.Add(new Paragraph("Hello World"));

                    return doc;
                }

                /* PdfWriter writer = PdfWriter.GetInstance(_document, this.getStream());
                 writer.PageEvent = PdfPageEventHelper;
                 writer.CompressionLevel = PdfStream.BEST_COMPRESSION;*/

                /*this.OnOpenDocument(this.getWriter(), this.getDocument()); //este evento aún no se ha podido enlazar
                this.setPageEvent(this);
                this.setCompressionLevel(PdfStream.BEST_COMPRESSION);*/



                
            }
            catch (Exception ex)
            {               
                return null;
            }
        }


    }
}
