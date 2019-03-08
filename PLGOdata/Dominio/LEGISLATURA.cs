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
using Newtonsoft.Json; //Serializado JSON
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data;

namespace PLGOdata
{
    public partial class LEGISLATURA
    {
        public static void DesmarcaActualLegislatura()
        {
            using (Entities c = new Entities())
            {
                LEGISLATURA l = c.LEGISLATURA.Where(a => a.ACTUAL == true).FirstOrDefault();
                if (l != null)
                {
                    l.ACTUAL = false;
                    c.LEGISLATURA.Attach(l);
                    c.Entry(l).State = EntityState.Modified; //Siempre es modificación, ya que aunque sea Alta, ya existirá el registro en la BBDD
                    c.SaveChanges();
                }
            }
        }

        public static int GetActualLegislatura()
        {
            using (Entities c = new Entities())
            {
                LEGISLATURA l = c.LEGISLATURA.Where(a => a.ACTUAL == true).FirstOrDefault();
                if (l != null)
                {
                    return l.LEGISLATURA_ID;
                }
                else
                {
                    throw new Exception("GetActualLegislatura - No se ha encontrado la legislatura actual");
                }
            }
        }

        /// <summary>
        /// Devuelve el % del avance de la Legislatura Actual (según el tiempo)
        /// </summary>
        /// <returns></returns>
        public static decimal GetTiempoLegislatura()
        {
            //Progreso Evolución General
            using (Entities c = new Entities())
            {
                using (var conn = c.Database.Connection)
                {

                    int iDiasTotales = 0;
                    int iDiasHastaHoy = 0;

                    var cmd = conn.CreateCommand();
                    conn.Open();
                    cmd.CommandText = @"select ((select FECHA_FIN from PLGO_OWN.LEGISLATURA WHERE ACTUAL = 1) - (select FECHA_INICIO from PLGO_OWN.LEGISLATURA WHERE ACTUAL = 1)) as dias_totales,
                        (trunc(sysdate) - (select FECHA_INICIO from PLGO_OWN.LEGISLATURA WHERE ACTUAL = 1)) as dias_hasta_hoy
                         from dual";

                    var reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess);
                    if (reader.Read())
                    {
                        iDiasTotales = reader.GetInt32(0);
                        iDiasHastaHoy = reader.GetInt32(1);
                    }

                    if (iDiasHastaHoy < iDiasTotales)
                        return (Math.Round(Decimal.Divide(iDiasHastaHoy * 100, iDiasTotales), 0));
                    else
                        return 100; //para no devolver nunca más de 100%
                }
            }
        }     
       

        public static decimal GetPorcentajeGlobalCumplimiento()
        {
            using (Entities c = new Entities())
            {
                c.Configuration.ProxyCreationEnabled = false;
                int iLegActual = LEGISLATURA.GetActualLegislatura();
                var results = c.DEPARTAMENTO.Where(d => d.LEGISLATURA_ID == iLegActual && d.VISIBLE == true).ToList().OrderBy(d => d.ORDEN);

                decimal dPorcentajeAvanceAcumulado = 0;
                int iNumDptos = 0;


                foreach (DEPARTAMENTO dep in results)
                {
                    if (dep.PORCENTAJE_AVANCE_CALCULADO.HasValue)
                        dPorcentajeAvanceAcumulado += dep.PORCENTAJE_AVANCE_CALCULADO.Value;
                    iNumDptos++;
                }

                return (Math.Round(Decimal.Divide(dPorcentajeAvanceAcumulado, iNumDptos), 0));
            }

          }
                

        /// <summary>
        /// Devuelve los datos calculados para el Cuadro de Mando, todo en una función para minimizar conexiones con la BBDD y agilizar la carga, ya que será llamado desde la página principal
        /// Los parámetros son de salida
        /// </summary>
        /// <param name="dLegTranscurrida">% de Legislatura transcurrida</param>
        /// <param name="iObjetivosSinIniciar">Número de objetivos sin iniciar (cada objetivo donde todas sus acciones estarán sin iniciar)</param>
        /// <param name="iObjetivosIniciados">Número de objetivos iniciados (cada uno tendrá al menos una acción iniciada)</param>
        /// <param name="iObjetivosTerminados">Número de objetivos terminados (cada objetivo tendrá todas sus acciones terminadas)</param>    
        /// <param name="dPorcIndiceGlobalCumplimiento">Iniciados + Terminados</param>
        /// <param name="dPorcIniciados">% Objetivos iniciados del total</param>
        /// <param name="dPorcSinIniciar">% Objetivos sin iniciar</param>
        /// <param name="dPorcTerminados">% Objetivos terminados</param>
        /// <param name="iNumAcciones">Número de acciones totales</param>
        /// <param name="iNumObjetivos">Número de objetivos totales</param>
        /// <param name="iSumaPorcenajesAvanceAcciones">Suma de todos los avances de las acciones</param>
        /// <see cref="DEPARTAMENTO.GetEvolucion()"/>        
        public static void GetDatosCuadroMando(out decimal dLegTranscurrida, out int iObjetivosSinIniciar, out int iObjetivosIniciados, out int iObjetivosTerminados,
            out int iSumaPorcenajesAvanceAcciones, out int iNumAcciones, out int iNumObjetivos,
            out decimal dPorcSinIniciar, out decimal dPorcIniciados, out decimal dPorcTerminados, out decimal dPorcIndiceGlobalCumplimiento)
        {
            //Progreso Evolución General
            using (Entities c = new Entities())
            {
                using (var conn = c.Database.Connection)
                {

                    dLegTranscurrida = 0;
                    iObjetivosSinIniciar = 0;
                    iObjetivosIniciados = 0;
                    iObjetivosTerminados = 0;
                    iSumaPorcenajesAvanceAcciones = 0;
                    iNumAcciones = 0;
                    iNumObjetivos = 0;
                    dPorcSinIniciar = 0;
                    dPorcIniciados = 0;
                    dPorcTerminados = 0;

                    int iDiasTotales = 0;
                    int iDiasHastaHoy = 0;

                    var cmd = conn.CreateCommand();
                    conn.Open();

                    //Legislatura transcurrida
                    cmd.CommandText = @"select ((select FECHA_FIN from PLGO_OWN.LEGISLATURA WHERE ACTUAL = 1) - (select FECHA_INICIO from PLGO_OWN.LEGISLATURA WHERE ACTUAL = 1)) as dias_totales,
                        (trunc(sysdate) - (select FECHA_INICIO from PLGO_OWN.LEGISLATURA WHERE ACTUAL = 1)) as dias_hasta_hoy
                         from dual";

                    var reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess);
                    if (reader.Read())
                    {
                        iDiasTotales = reader.GetInt32(0);
                        iDiasHastaHoy = reader.GetInt32(1);
                    }

                    if (iDiasHastaHoy < iDiasTotales)
                        dLegTranscurrida = (Math.Round(Decimal.Divide(iDiasHastaHoy * 100, iDiasTotales), 0));

                    else
                        dLegTranscurrida = 100; //para no devolver nunca más de 100%



                    //Acciones
                    cmd.CommandText = @"select sum(PORCENTAJE_AVANCE) as spa, count(porcentaje_avance) as cpa, OBJETIVO_CONTENIDO_ID from PLGO_OWN.accion a
                                       inner join PLGO_OWN.CONTENIDO c on c.contenido_id = A.CONTENIDO_ID
                                       inner join PLGO_OWN.DEPARTAMENTO d on C.DEPARTAMENTO_ID = D.DEPARTAMENTO_ID
                                       inner join PLGO_OWN.LEGISLATURA l on L.LEGISLATURA_ID = D.LEGISLATURA_ID
                                       WHERE l.actual = 1 and d.visible = 1 and c.visible = 1 and C.TIPO_CAMBIO_CONTENIDO_ID <> 1
                                        AND PORCENTAJE_AVANCE IS NOT NULL GROUP BY OBJETIVO_CONTENIDO_ID";

                    reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess);

                    int iPorcentaje = 0;
                    int iNumAccionesPorObjetivo = 0;

                    while (reader.Read())
                    {
                        iPorcentaje = reader.GetInt32(0);
                        iNumAccionesPorObjetivo = reader.GetInt32(1);

                        if (iPorcentaje == 0)
                        {
                            //Todas las acciones del objetivo valen 0: objetivo sin inciar
                            iObjetivosSinIniciar++;
                        }
                        else if (iPorcentaje == (iNumAccionesPorObjetivo * 100))
                        {
                            //Todas las acciones valen 100: objetivo terminado
                            iObjetivosTerminados++;
                        }
                        else
                        {
                            iObjetivosIniciados++;
                        }
                        iNumObjetivos++;
                        iSumaPorcenajesAvanceAcciones += iPorcentaje;
                        iNumAcciones += iNumAccionesPorObjetivo;
                    }


                    dPorcSinIniciar = Math.Round(((decimal)iObjetivosSinIniciar * 100 / iNumObjetivos), 0);
                    dPorcIniciados = Math.Round(((decimal)iObjetivosIniciados * 100 / iNumObjetivos), 0);
                    dPorcTerminados = 100 - dPorcSinIniciar - dPorcIniciados; //nos aseguramos que la suma no sea mayor que 100%

                    dPorcIndiceGlobalCumplimiento = LEGISLATURA.GetPorcentajeGlobalCumplimiento();

                   
                }
            }
        }



        public static DataSet Informe(int? iDepartamentoId, string pathXmlSchema, int? iLegislaturaId)
        {
          
          
                using (Entities c = new Entities())
                {
                    List<DEPARTAMENTO> dptos = null;

                    if (iDepartamentoId.HasValue)
                        dptos = c.DEPARTAMENTO.Where(st => st.DEPARTAMENTO_ID == iDepartamentoId.Value).ToList();
                    else
                    {
                        if (iLegislaturaId.HasValue)
                        {
                            dptos = c.DEPARTAMENTO.Where(st => st.LEGISLATURA_ID == iLegislaturaId.Value).OrderBy(st => st.ORDEN).ToList();
                    }
                        else
                        {
                            int iLegActual = LEGISLATURA.GetActualLegislatura();
                            dptos = c.DEPARTAMENTO.Where(st => st.LEGISLATURA_ID == iLegActual).OrderBy(st => st.ORDEN).ToList();
                        }
                    }

                    DataSet ds = new DataSet();
                    ds.ReadXmlSchema(pathXmlSchema);

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
                                //dr[9] = acc.ESTADOS_SEGUIMIENTO.DESCRIPCION;
                                dr[9] = "";

                                if (acc.FECHA_MODIFICACION.HasValue)
                                    dr[10] = acc.FECHA_MODIFICACION.Value.ToShortDateString();
                                else
                                    dr[10] = acc.FECHA_CREACION.ToShortDateString();

                                dr[11] = dep.ORDEN;
                                if (acc.PORCENTAJE_AVANCE.HasValue)
                                    dr[12] = acc.PORCENTAJE_AVANCE;
                                else if (acc.ESTADOS_SEGUIMIENTO != null)
                                    dr[12] = acc.ESTADOS_SEGUIMIENTO.DESCRIPCION;

                            ds.Tables[0].Rows.Add(dr);
                            }
                        }
                    }
                    return ds;
                }           
        }
    }
}
