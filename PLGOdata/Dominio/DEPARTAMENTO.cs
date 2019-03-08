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
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Entity;

namespace PLGOdata
{   
    [MetadataType(typeof(DEPARTAMENTO_Validation))]
    public partial class DEPARTAMENTO
    {
        /// <summary>
        /// Devuelve las validaciones pendientes del departamento actual
        /// </summary>
        [NotMapped]
        public int ValidacionesPendientes
        {
            get {

                using (Entities c = new Entities())
                {
                    return c.CONTENIDO.Where(co => co.DEPARTAMENTO_ID == this.DEPARTAMENTO_ID && co.ESTADO_VALIDACION_ID == ESTADOS_VALIDACION.PDTE_VALIDAR).Count();
                }
                
            }
        }


        /// <summary>
        /// Elimina el departamento y sus objetivos / acciones asociadas
        /// </summary>
        public static void EliminaDepartamento(int iDepartamentoId)
        {
            using (Entities c = new Entities())
            {
                using (var dbContextTransaction = c.Database.BeginTransaction())
                {
                    try
                    {

                        foreach (OBJETIVO obj in c.CONTENIDO.OfType<OBJETIVO>().Where(o => o.DEPARTAMENTO_ID == iDepartamentoId))
                        {       
                            var results = c.CONTENIDO.OfType<ACCION>().Where(st => st.OBJETIVO_CONTENIDO_ID == obj.CONTENIDO_ID).ToList();
                            //Borramos acciones
                            c.CONTENIDO.RemoveRange(results);
                            c.SaveChanges();
                            //Borramos el objetivo
                            c.CONTENIDO.Remove(obj);
                            c.SaveChanges();                                
                        }


                        DEPARTAMENTO dep = c.DEPARTAMENTO.Find(iDepartamentoId);
                        if (dep != null)
                        {
                            c.DEPARTAMENTO.Remove(dep);
                            c.SaveChanges();
                            dbContextTransaction.Commit();
                        }
                        else
                        {
                            throw new Exception("Departamento no encontrado: " + iDepartamentoId.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public static List<DEPARTAMENTO> ListadoPlanGobierno(int? iDepartamentoId)
        {

            int iLegislaturaId = LEGISLATURA.GetActualLegislatura();

            List<DEPARTAMENTO> list = new List<DEPARTAMENTO>();
            using (Entities c = new Entities())
            {

                c.Configuration.LazyLoadingEnabled = false;

                if (iDepartamentoId.HasValue)
                    list = c.DEPARTAMENTO.Include("CONTENIDO").Where(st => st.LEGISLATURA_ID == iLegislaturaId && st.DEPARTAMENTO_ID == iDepartamentoId && st.VISIBLE == true).ToList();
                else
                    list = c.DEPARTAMENTO.Include("CONTENIDO").Where(st => st.LEGISLATURA_ID == iLegislaturaId && st.VISIBLE == true).ToList();

                return list;
            }
        }



        /// <summary>
        /// Devuelve el % de avance del Departamento según la fórmula empleada para dicho cálculo
        /// </summary>
        /// <returns></returns>
        /// <see cref="DEPARTAMENTO.GetEvolucion(int, out int, out int, out int, out int, out int, out int)"/>
        /// <see cref="LEGISLATURA.GetPorcentajeGlobalCumplimiento(decimal, decimal)"/>
        public decimal GetEvolucion()
        {
            return GetEvolucion(0);
        }


        /// <summary>
        /// Devuelve el % de avance del Departamento según la fórmula empleada para dicho cálculo
        /// </summary>
        /// <returns></returns>
        /// <see cref="DEPARTAMENTO.GetEvolucion(int, out int, out int, out int, out int, out int, out int)"/>
        /// <see cref="LEGISLATURA.GetPorcentajeGlobalCumplimiento(decimal, decimal)"/>
        public decimal GetEvolucion(int iPosicionesDecimales)
        {
            /*int iObjetivosSinIniciar = 0;
            int iObjetivosIniciados = 0;
            int iObjetivosTerminados = 0;
            int iNumAcciones = 0;
            int iNumObjetivos = 0;
            int iSumaPorcenajesAvanceAcciones = 0;         

            DEPARTAMENTO.GetEvolucion(this.DEPARTAMENTO_ID, out iObjetivosSinIniciar, out iObjetivosIniciados, out iObjetivosTerminados,
            out iSumaPorcenajesAvanceAcciones, out iNumAcciones, out iNumObjetivos);           

            return LEGISLATURA.GetPorcentajeGlobalCumplimiento(iObjetivosSinIniciar, iObjetivosIniciados, iObjetivosTerminados, iPosicionesDecimales);*/

            if (this.PORCENTAJE_AVANCE_CALCULADO.HasValue)
                return ((Math.Round(this.PORCENTAJE_AVANCE_CALCULADO.Value, iPosicionesDecimales)));
            else
                return 0;

        }



        /// <summary>
        /// Devuelve los datos estadisticos para el Departamento indicado, similar a la funcion de Cuadro de mando de legislatura
        /// </summary>
        /// <param name="iDeptId"></param>      
        /// <param name="iObjetivosSinIniciar"></param>
        /// <param name="iObjetivosIniciados"></param>
        /// <param name="iObjetivosTerminados"></param>
        /// <param name="iSumaPorcenajesAvanceAcciones"></param>
        /// <param name="iNumAcciones"></param>
        /// <param name="iNumObjetivos"></param>
        public static void GetEvolucion(int iDeptId, out int iObjetivosSinIniciar, out int iObjetivosIniciados, out int iObjetivosTerminados,
            out int iSumaPorcenajesAvanceAcciones, out int iNumAcciones, out int iNumObjetivos)
        {
            using (Entities c = new Entities())
            {
                using (var conn = c.Database.Connection)
                {
                    iObjetivosSinIniciar = 0;
                    iObjetivosIniciados = 0;
                    iObjetivosTerminados = 0;
                    iSumaPorcenajesAvanceAcciones = 0;
                    iNumAcciones = 0;
                    iNumObjetivos = 0;

                    var cmd = conn.CreateCommand();
                    conn.Open();

                    //Acciones
                    cmd.CommandText = @"select sum(PORCENTAJE_AVANCE) as spa, count(porcentaje_avance) as cpa, OBJETIVO_CONTENIDO_ID from PLGO_OWN.accion a
                                       inner join PLGO_OWN.CONTENIDO c on c.contenido_id = A.CONTENIDO_ID
                                       inner join PLGO_OWN.DEPARTAMENTO d on C.DEPARTAMENTO_ID = D.DEPARTAMENTO_ID
                                       inner join PLGO_OWN.LEGISLATURA l on L.LEGISLATURA_ID = D.LEGISLATURA_ID
                                       WHERE l.actual = 1 and d.visible = 1 and c.visible = 1 and C.TIPO_CAMBIO_CONTENIDO_ID <> 1
                                        AND PORCENTAJE_AVANCE IS NOT NULL and C.DEPARTAMENTO_ID = :DptoId GROUP BY OBJETIVO_CONTENIDO_ID ";

                    var parameter = cmd.CreateParameter();
                    parameter.ParameterName = ":DptoId";
                    parameter.Value = iDeptId;
                    cmd.Parameters.Add(parameter);

                    var reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess);

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
                }
            }
        }       
}

    public class DEPARTAMENTO_Validation
    {
        [JsonIgnore]
        public virtual ICollection<CONTENIDO> CONTENIDO { get; set; }
        
        [JsonIgnore]
        public virtual ICollection<USUARIOS> USUARIOS { get; set; }

        [JsonIgnore]
        public virtual LEGISLATURA LEGISLATURA { get; set; }
    }
}
