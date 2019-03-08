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
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using System.Data;

namespace PLGOdata
{
    public static class InformesExcel
    {

        public static Columns AutoSize(SheetData sheetData)
        {
            var maxColWidth = GetMaxCharacterWidth(sheetData); 

            Columns columns = new Columns();
            //this is the width of my font - yours may be different
            double maxWidth = 7;
            foreach (var item in maxColWidth)
            {
                //width = Truncate([{Number of Characters} * {Maximum Digit Width} + {5 pixel padding}]/{Maximum Digit Width}*256)/256
                double width = Math.Truncate((item.Value * maxWidth + 5) / maxWidth * 256) / 256;

                //pixels=Truncate(((256 * {width} + Truncate(128/{Maximum Digit Width}))/256)*{Maximum Digit Width})
                double pixels = Math.Truncate(((256 * width + Math.Truncate(128 / maxWidth)) / 256) * maxWidth);

                //character width=Truncate(({pixels}-5)/{Maximum Digit Width} * 100+0.5)/100
                double charWidth = Math.Truncate((pixels - 5) / maxWidth * 100 + 0.5) / 100;

                Column col = new Column() { BestFit = true, Min = (UInt32)(item.Key + 1), Max = (UInt32)(item.Key + 1), CustomWidth = true, Width = (DoubleValue)width };
                columns.Append(col);
            }

            return columns;
        }


        public static Columns SizesInformeDepartamento(SheetData sheetData)
        {
            //var maxColWidth = GetMaxCharacterWidth(sheetData); //en lugar de auto especificamos el ancho que queremos en las columnas "conflictivas", no es necesario poner todos los anchos...
            //ANCHOS PERSONALIZADOS:
            var maxColWidth = new Dictionary<int, int>() {
            { 0, 2},
            { 1, 2},
            { 2, 80},
            { 3, 20},
            { 4, 18},
            { 5, 18},
            { 6, 18},
            { 7, 18},
            { 8, 60},
            { 9, 80},
            { 10, 22},
            { 11, 22},
            { 12, 22},
            { 13, 15},
            { 14, 15}            
        };

            Columns columns = new Columns();
            //this is the width of my font - yours may be different
            double maxWidth = 7;
            foreach (var item in maxColWidth)
            {
                //width = Truncate([{Number of Characters} * {Maximum Digit Width} + {5 pixel padding}]/{Maximum Digit Width}*256)/256
                double width = Math.Truncate((item.Value * maxWidth + 5) / maxWidth * 256) / 256;

                //pixels=Truncate(((256 * {width} + Truncate(128/{Maximum Digit Width}))/256)*{Maximum Digit Width})
                double pixels = Math.Truncate(((256 * width + Math.Truncate(128 / maxWidth)) / 256) * maxWidth);

                //character width=Truncate(({pixels}-5)/{Maximum Digit Width} * 100+0.5)/100
                double charWidth = Math.Truncate((pixels - 5) / maxWidth * 100 + 0.5) / 100;

                Column col = new Column() { BestFit = true, Min = (UInt32)(item.Key + 1), Max = (UInt32)(item.Key + 1), CustomWidth = true, Width = (DoubleValue)width };
                columns.Append(col);
            }

            return columns;
        }

        private static Dictionary<int, int> GetMaxCharacterWidth(SheetData sheetData)
        {
            //iterate over all cells getting a max char value for each column
            Dictionary<int, int> maxColWidth = new Dictionary<int, int>();
            var rows = sheetData.Elements<Row>();
            UInt32[] numberStyles = new UInt32[] { 5, 6, 7, 8 }; //styles that will add extra chars
            UInt32[] boldStyles = new UInt32[] { 1, 2, 3, 4, 6, 7, 8 }; //styles that will bold
            foreach (var r in rows)
            {
                var cells = r.Elements<Cell>().ToArray();

                //using cell index as my column
                //for (int i = 0; i < cells.Length; i++)
                for (int i = 2; i < cells.Length; i++) //No ponemos ancho en las dos primeras columnas
                {
                    var cell = cells[i];
                    var cellValue = cell.CellValue == null ? string.Empty : cell.CellValue.InnerText;
                    var cellTextLength = cellValue.Length;

                    if (cell.StyleIndex != null && numberStyles.Contains(cell.StyleIndex))
                    {
                        int thousandCount = (int)Math.Truncate((double)cellTextLength / 4);

                        //add 3 for '.00' 
                        cellTextLength += (3 + thousandCount);
                    }

                    if (cell.StyleIndex != null && boldStyles.Contains(cell.StyleIndex))
                    {
                        //add an extra char for bold - not 100% acurate but good enough for what i need.
                        cellTextLength += 1;
                    }

                    if (maxColWidth.ContainsKey(i))
                    {
                        var current = maxColWidth[i];
                        if (cellTextLength > current)
                        {
                            maxColWidth[i] = cellTextLength;
                        }
                    }
                    else
                    {
                        maxColWidth.Add(i, cellTextLength);
                    }
                }
            }

            return maxColWidth;
        }

        public static Stylesheet GenerateStyleSheet()
        {
            return new Stylesheet(
                new Fonts(
                    // Index 0 - Default font.
                    new DocumentFormat.OpenXml.Spreadsheet.Font(
                        new DocumentFormat.OpenXml.Spreadsheet.FontSize() { Val = 11 },
                        new Color() { Rgb = new HexBinaryValue() { Value = "000000" } }
                        ),
                    new DocumentFormat.OpenXml.Spreadsheet.Font(
                        new Bold(),
                        new DocumentFormat.OpenXml.Spreadsheet.FontSize() { Val = 14 },
                        new Color() { Rgb = new HexBinaryValue() { Value = "000000" } }
                        ),
                    new DocumentFormat.OpenXml.Spreadsheet.Font(
                        new Bold(),
                        new DocumentFormat.OpenXml.Spreadsheet.FontSize() { Val = 16 },
                        new Color() { Rgb = new HexBinaryValue() { Value = "ffffff" } }
                        ),
                    new DocumentFormat.OpenXml.Spreadsheet.Font(
                        new Bold(),
                        new DocumentFormat.OpenXml.Spreadsheet.FontSize() { Val = 12 },
                        new Color() { Rgb = new HexBinaryValue() { Value = "000000" } }
                        )
                ),
                new Fills(
                    // Index 0 - Default fill.
                    new Fill(
                        new PatternFill() { PatternType = PatternValues.None }),
                      new Fill(
                        new PatternFill() { PatternType = PatternValues.None }),//NO USARLO, NO VA ESTE FILLID                  
                     new Fill()
                     {
                         PatternFill = new PatternFill()
                         {
                             PatternType = PatternValues.Solid,
                             ForegroundColor = new ForegroundColor() { Rgb = new HexBinaryValue() { Value = "70ad47" } } //Dpto
                     }
                     },
                     new Fill()
                     {
                         PatternFill = new PatternFill()
                         {
                             PatternType = PatternValues.Solid,
                             ForegroundColor = new ForegroundColor() { Rgb = new HexBinaryValue() { Value = "ddebf7" } } //Objetivo
                     }
                     },
                     new Fill()
                     {
                         PatternFill = new PatternFill()
                         {
                             PatternType = PatternValues.Solid,
                             ForegroundColor = new ForegroundColor() { Rgb = new HexBinaryValue() { Value = "FAF0C0" } } //Nuevo                         
                     }
                     }
                ),
                new Borders(
                    // Index 0 - Default border.
                    new Border(
                        new LeftBorder(),
                        new RightBorder(),
                        new TopBorder(),
                        new BottomBorder(),
                        new DiagonalBorder())
                ),
                /*(new Alignment()
                    { WrapText = true, Horizontal = HorizontalAlignmentValues.General, Vertical = VerticalAlignmentValues.Center }
                ),*/
                new CellFormats(
                    // Index 0 - Default cell style
                    new CellFormat() { FontId = 0, FillId = 0, BorderId = 0, ApplyFont = true }, //Por defecto, estilo obligatorio
                    new CellFormat() { FontId = 1, FillId = 3, BorderId = 0, ApplyFont = true }, //Titulo Objetivo
                    new CellFormat() { FontId = 2, FillId = 2, BorderId = 0, ApplyFont = true }, //Titulo Departamento
                    new CellFormat() { FontId = 3, FillId = 0, BorderId = 0, ApplyFont = true }, //Cabecera Accion                
                    new CellFormat(new Alignment() { WrapText = true, Horizontal = HorizontalAlignmentValues.General, Vertical = VerticalAlignmentValues.Center }), //Normal
                    new CellFormat(new Alignment() { WrapText = true, Horizontal = HorizontalAlignmentValues.General, Vertical = VerticalAlignmentValues.Center }) { FontId = 0, FillId = 4, BorderId = 0, ApplyFont = true } //Acciones nuevas
                )
            );
        }

        public static SheetData HojaExcelInforme(int? iDptoId, int? iLegislaturaId, bool bConCambios, out MergeCells mergeCells)
        {

            try
            {

                using (Entities c = new Entities())
                {
                    List<DEPARTAMENTO> dptos;
                    mergeCells = new MergeCells();

                    if (iDptoId.HasValue)
                        dptos = c.DEPARTAMENTO.Where(st => st.DEPARTAMENTO_ID == iDptoId.Value).ToList();
                    else if (iLegislaturaId.HasValue)
                        dptos = c.DEPARTAMENTO.Where(st => st.LEGISLATURA_ID == iLegislaturaId.Value).OrderBy(st => st.ORDEN).ToList();
                    else
                        throw new Exception("Invocación errónea en InformesExcel.HojaExcelInforme");

                    var sheetData = new SheetData();

                    int iRow = 0;

                    foreach (DEPARTAMENTO dep in dptos)
                    { 
                        //DataTable tabla = Utils.ToDataTable<CONTENIDO>(dep.CONTENIDO.ToList());

                        List<OBJETIVO> objetivos;

                    if (!bConCambios)
                        objetivos = c.CONTENIDO.OfType<OBJETIVO>().Where(st => st.DEPARTAMENTO_ID == dep.DEPARTAMENTO_ID && st.VISIBLE == true).OrderBy(st => st.OBJETIVO_ESTRATEGICO).ToList();
                    else
                        objetivos = c.CONTENIDO.OfType<OBJETIVO>().Where(st => st.DEPARTAMENTO_ID == dep.DEPARTAMENTO_ID).OrderBy(st => st.OBJETIVO_ESTRATEGICO).ToList();

                    

                    Row DeptoRow = new Row();
                    Cell cell = new Cell();
                    cell.DataType = CellValues.String;
                    cell.StyleIndex = 2;
                    cell.CellValue = new CellValue(dep.DESCRIPCION);
                    DeptoRow.AppendChild(cell);
                    DeptoRow.Height = 30;                     
                    sheetData.AppendChild(DeptoRow);
                    iRow++;
                        //En el caso del título del Departamento creamos una gran celda combinada

                        mergeCells.Append(new MergeCell() { Reference = new StringValue("A" + iRow + ":O" + iRow) });

                        foreach (OBJETIVO obj in objetivos)
                        {
                            Row ObjRow = new Row();
                            ObjRow.AppendChild(new Cell() { DataType = CellValues.String, CellValue = new CellValue("") });
                            cell = new Cell();
                            cell.DataType = CellValues.String;

                            if (bConCambios && ((obj.TIPO_CAMBIO_CONTENIDO_ID.Equals(TIPO_CAMBIO_CONTENIDO.ALTA)) || (obj.TIPO_CAMBIO_CONTENIDO_ID.Equals(TIPO_CAMBIO_CONTENIDO.MODIFICACION))))
                                cell.CellValue = new CellValue(obj.OBJETIVO_ESTRATEGICO_PDTE_VAL);
                            else
                                cell.CellValue = new CellValue(obj.OBJETIVO_ESTRATEGICO);
                            cell.StyleIndex = 1;

                            ObjRow.AppendChild(cell);
                            sheetData.AppendChild(ObjRow);
                            iRow++;
                            mergeCells.Append(new MergeCell() { Reference = new StringValue("B" + iRow + ":O" + iRow) });

                            List<ACCION> acciones;

                            if (!bConCambios)
                                acciones = c.CONTENIDO.OfType<ACCION>().Where(st => st.DEPARTAMENTO_ID == dep.DEPARTAMENTO_ID && st.OBJETIVO_CONTENIDO_ID == obj.CONTENIDO_ID && st.VISIBLE == true).OrderBy(st => st.INSTRUMENTOS_ACT).ToList();
                            else
                                acciones = c.CONTENIDO.OfType<ACCION>().Where(st => st.DEPARTAMENTO_ID == dep.DEPARTAMENTO_ID && st.OBJETIVO_CONTENIDO_ID == obj.CONTENIDO_ID).OrderBy(st => st.INSTRUMENTOS_ACT).ToList();

                            Row AccRow = new Row();
                            AccRow.AppendChild(new Cell() { DataType = CellValues.String, CellValue = new CellValue("") });
                            AccRow.AppendChild(new Cell() { DataType = CellValues.String, CellValue = new CellValue("") });

                            cell = new Cell();
                            cell.DataType = CellValues.String;
                            cell.StyleIndex = 3;
                            cell.CellValue = new CellValue("Instrumentos y actividades");
                            AccRow.AppendChild(cell);

                            cell = new Cell();
                            cell.DataType = CellValues.String;
                            cell.StyleIndex = 3;
                            cell.CellValue = new CellValue("Órgano responsable");
                            AccRow.AppendChild(cell);
                            cell = new Cell();
                            cell.DataType = CellValues.String;
                            cell.CellValue = new CellValue("RRHH");
                            cell.StyleIndex = 3;
                            AccRow.AppendChild(cell);
                            cell = new Cell();
                            cell.DataType = CellValues.String;
                            cell.StyleIndex = 3;
                            cell.CellValue = new CellValue("Coste económico");
                            AccRow.AppendChild(cell);
                            cell = new Cell();
                            cell.DataType = CellValues.String;
                            cell.CellValue = new CellValue("Otros medios");
                            cell.StyleIndex = 3;
                            AccRow.AppendChild(cell);
                            cell = new Cell();
                            cell.DataType = CellValues.String;
                            cell.CellValue = new CellValue("Temporalidad");
                            cell.StyleIndex = 3;
                            AccRow.AppendChild(cell);
                            cell = new Cell();
                            cell.DataType = CellValues.String;
                            cell.CellValue = new CellValue("Indicadores de seguimiento y evaluación");
                            cell.StyleIndex = 3;
                            AccRow.AppendChild(cell);
                            cell = new Cell();
                            cell.DataType = CellValues.String;
                            cell.CellValue = new CellValue("Seguimiento");
                            cell.StyleIndex = 3;
                            AccRow.AppendChild(cell);
                            cell = new Cell();
                            cell.DataType = CellValues.String;
                            cell.CellValue = new CellValue("% avance");
                            cell.StyleIndex = 3;
                            AccRow.AppendChild(cell);
                            cell = new Cell();
                            cell.DataType = CellValues.String;
                            cell.CellValue = new CellValue("Tipo cambio");
                            cell.StyleIndex = 3;
                            AccRow.AppendChild(cell);
                            cell = new Cell();
                            cell.DataType = CellValues.String;
                            cell.CellValue = new CellValue("Estado Validación");
                            cell.StyleIndex = 3;
                            AccRow.AppendChild(cell);
                            cell = new Cell();
                            cell.DataType = CellValues.String;
                            cell.CellValue = new CellValue("Fecha alta");
                            cell.StyleIndex = 3;
                            AccRow.AppendChild(cell);
                            cell = new Cell();
                            cell.DataType = CellValues.String;
                            cell.CellValue = new CellValue("Fecha modificación");
                            cell.StyleIndex = 3;
                            AccRow.AppendChild(cell);
                            //cell = new Cell();
                            //cell.DataType = CellValues.String;
                            //cell.CellValue = new CellValue("Comentario validación");
                            //cell.StyleIndex = 3;
                            //AccRow.AppendChild(cell);

                            sheetData.AppendChild(AccRow);
                            iRow++;

                            foreach (ACCION acc in acciones)
                            {

                                if (!(!bConCambios && acc.TIPO_CAMBIO_CONTENIDO_ID.Equals(TIPO_CAMBIO_CONTENIDO.ALTA)))
                                {  //Si no queremos cambios y la acción es alta pasamos al siguiente registro

                                    string sInstrumentos = "", sOrgano = "", sRRHH = "", sCoste = "", sMediosOtros = "";
                                    string sTemporalidad = "", sIndicadorSeguimiento = "", sSeguimiento = "", sEstadoSeguimiento = "", sFechaAlta = "", sFechaModificacion = "";
                                    UInt32 iStyle = 4;

                                    if (bConCambios && ((acc.TIPO_CAMBIO_CONTENIDO_ID.Equals(TIPO_CAMBIO_CONTENIDO.ALTA)) || (acc.TIPO_CAMBIO_CONTENIDO_ID.Equals(TIPO_CAMBIO_CONTENIDO.MODIFICACION))))
                                    {
                                        iStyle = 5;
                                        sInstrumentos = acc.INSTRUMENTOS_ACT_PDTE_VAL;
                                        sOrgano = acc.ORGANO_RESPONSABLE_PDTE_VAL;
                                        sRRHH = acc.RECURSOS_HUMANOS_PDTE_VAL;
                                        sCoste = acc.COSTE_ECONOMICO_PDTE_VAL;
                                        sMediosOtros = acc.MEDIOS_OTROS_PDTE_VAL;
                                        sTemporalidad = acc.TEMPORALIDAD_PDTE_VAL;
                                        sIndicadorSeguimiento = acc.INDICADOR_SEGUIMIENTO_PDTE_VAL;
                                        sSeguimiento = acc.SEGUIMIENTO_PDTE_VAL;

                                        if (acc.PORCENTAJE_AVANCE_PDTE_VAL != null)
                                            sEstadoSeguimiento = acc.PORCENTAJE_AVANCE_PDTE_VAL.ToString();
                                        else if (acc.ESTADO_SEGUIMIENTO_ID_PDTE_VAL.HasValue)
                                            sEstadoSeguimiento = acc.ESTADOS_SEGUIMIENTO_PDTE_VAL.DESCRIPCION;
                                        
                                        if (acc.FECHA_CREACION != null)
                                            sFechaAlta = acc.FECHA_CREACION.ToShortDateString();
                                        if (acc.FECHA_MODIFICACION.HasValue)
                                            sFechaModificacion = acc.FECHA_MODIFICACION.Value.ToShortDateString();
                                    }
                                    else if (bConCambios && (acc.TIPO_CAMBIO_CONTENIDO_ID.Equals(TIPO_CAMBIO_CONTENIDO.ELIMINADO)))
                                    {
                                        iStyle = 5;
                                        sInstrumentos = acc.INSTRUMENTOS_ACT;
                                        sOrgano = acc.ORGANO_RESPONSABLE;
                                        sRRHH = acc.RECURSOS_HUMANOS;
                                        sCoste = acc.COSTE_ECONOMICO;
                                        sMediosOtros = acc.MEDIOS_OTROS;
                                        sTemporalidad = acc.TEMPORALIDAD;
                                        sIndicadorSeguimiento = acc.INDICADOR_SEGUIMIENTO;
                                        sSeguimiento = acc.SEGUIMIENTO;
                                        if (acc.PORCENTAJE_AVANCE != null)
                                            sEstadoSeguimiento = acc.PORCENTAJE_AVANCE.ToString();
                                        else if (acc.ESTADO_SEGUIMIENTO_ID.HasValue)
                                            sEstadoSeguimiento = acc.ESTADOS_SEGUIMIENTO.DESCRIPCION;
                                        if (acc.FECHA_CREACION != null)
                                            sFechaAlta = acc.FECHA_CREACION.ToShortDateString();
                                        if (acc.FECHA_MODIFICACION.HasValue)
                                            sFechaModificacion = acc.FECHA_MODIFICACION.Value.ToShortDateString();
                                    }
                                    else
                                    {
                                        iStyle = 4;
                                        sInstrumentos = acc.INSTRUMENTOS_ACT;
                                        sOrgano = acc.ORGANO_RESPONSABLE;
                                        sRRHH = acc.RECURSOS_HUMANOS;
                                        sCoste = acc.COSTE_ECONOMICO;
                                        sMediosOtros = acc.MEDIOS_OTROS;
                                        sTemporalidad = acc.TEMPORALIDAD;
                                        sIndicadorSeguimiento = acc.INDICADOR_SEGUIMIENTO;
                                        sSeguimiento = acc.SEGUIMIENTO;
                                        if (acc.PORCENTAJE_AVANCE != null)
                                            sEstadoSeguimiento = acc.PORCENTAJE_AVANCE.ToString();
                                        else if (acc.ESTADO_SEGUIMIENTO_ID.HasValue)
                                            sEstadoSeguimiento = acc.ESTADOS_SEGUIMIENTO.DESCRIPCION;                                         
                                        if (acc.FECHA_CREACION != null)
                                            sFechaAlta = acc.FECHA_CREACION.ToShortDateString();
                                        if (acc.FECHA_MODIFICACION.HasValue)
                                            sFechaModificacion = acc.FECHA_MODIFICACION.Value.ToShortDateString();
                                    }

                                    Row AccValRow = new Row();
                                    AccValRow.Height = 70;
                                    AccValRow.CustomHeight = true;
                                    cell = new Cell();
                                    cell.DataType = CellValues.String;
                                    AccValRow.AppendChild(cell);
                                    AccValRow.AppendChild(new Cell());
                                    cell = new Cell();
                                    cell.DataType = CellValues.String;
                                    cell.CellValue = new CellValue(sInstrumentos);
                                    cell.StyleIndex = iStyle;
                                    AccValRow.AppendChild(cell);
                                    cell = new Cell();
                                    cell.DataType = CellValues.String;
                                    cell.CellValue = new CellValue(sOrgano);
                                    cell.StyleIndex = iStyle;
                                    AccValRow.AppendChild(cell);
                                    cell = new Cell();
                                    cell.DataType = CellValues.String;
                                    cell.CellValue = new CellValue(sRRHH);
                                    cell.StyleIndex = iStyle;
                                    AccValRow.AppendChild(cell);
                                    cell = new Cell();
                                    cell.DataType = CellValues.String;
                                    cell.CellValue = new CellValue(sCoste);
                                    cell.StyleIndex = iStyle;
                                    AccValRow.AppendChild(cell);
                                    cell = new Cell();
                                    cell.DataType = CellValues.String;
                                    cell.CellValue = new CellValue(sMediosOtros);
                                    cell.StyleIndex = iStyle;
                                    AccValRow.AppendChild(cell);
                                    cell = new Cell();
                                    cell.DataType = CellValues.String;
                                    cell.CellValue = new CellValue(sTemporalidad);
                                    cell.StyleIndex = iStyle;
                                    AccValRow.AppendChild(cell);
                                    cell = new Cell();
                                    cell.DataType = CellValues.String;
                                    cell.CellValue = new CellValue(sIndicadorSeguimiento);
                                    cell.StyleIndex = iStyle;
                                    AccValRow.AppendChild(cell);
                                    cell = new Cell();
                                    cell.DataType = CellValues.String;
                                    cell.CellValue = new CellValue(sSeguimiento);
                                    cell.StyleIndex = iStyle;
                                    AccValRow.AppendChild(cell);
                                    cell = new Cell();
                                    cell.DataType = CellValues.String;
                                    cell.CellValue = new CellValue(sEstadoSeguimiento);
                                    cell.StyleIndex = 4;
                                    AccValRow.AppendChild(cell);
                                    cell = new Cell();
                                    cell.DataType = CellValues.String;
                                    if (acc.TIPO_CAMBIO_CONTENIDO != null)
                                        cell.CellValue = new CellValue(acc.TIPO_CAMBIO_CONTENIDO.TIPO_CAMBIO);
                                    else
                                        cell.CellValue = new CellValue("Sin cambios");
                                    cell.StyleIndex = 4;
                                    AccValRow.AppendChild(cell);
                                    cell = new Cell();
                                    cell.DataType = CellValues.String;
                                    if (acc.ESTADOS_VALIDACION != null)
                                        cell.CellValue = new CellValue(acc.ESTADOS_VALIDACION.DESCRIPCION);
                                    cell.StyleIndex = 4;
                                    AccValRow.AppendChild(cell);
                                    cell = new Cell();
                                    cell.DataType = CellValues.String;                                    
                                    cell.CellValue = new CellValue(sFechaAlta);
                                    cell.StyleIndex = 4;
                                    AccValRow.AppendChild(cell);
                                    cell = new Cell();
                                    cell.DataType = CellValues.String;
                                    cell.CellValue = new CellValue(sFechaModificacion);
                                    cell.StyleIndex = 4;
                                    AccValRow.AppendChild(cell);
                                    //cell = new Cell();
                                    //cell.DataType = CellValues.String;
                                    //if (acc.COMENTARIO_VALIDADOR != null)
                                    //    cell.CellValue = new CellValue(acc.COMENTARIO_VALIDADOR);
                                    //cell.StyleIndex = 4;
                                    //AccValRow.AppendChild(cell);
                                    sheetData.AppendChild(AccValRow);
                                    iRow++;
                                }

                            }

                            ObjRow = new Row();
                            sheetData.AppendChild(ObjRow);
                            iRow++ ;
                        }

                        Row ObjNewRow = new Row();
                        sheetData.AppendChild(ObjNewRow);
                        iRow++;
                    }
                    return sheetData;
                }
            }
            catch (Exception ex)
            {
                throw ex;                                
            }
        }
    }
}
