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
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace PLGOdata
{

    public class ObjSerializer : JsonConverter
    {
        /// <summary>
        /// Se utiliza para crear el Json para el listado público. No se mostrarán Objetivos sin Acciones, ni Acciones no visibles
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {

            var name = value as OBJETIVO;

            //Si no tiene acciones no lo escribimos
            if (name.ACCION.Where(a => a.VISIBLE && !String.IsNullOrEmpty(a.INSTRUMENTOS_ACT)).ToList().Count > 0)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("ID");
                serializer.Serialize(writer, name.CONTENIDO_ID.ToString());
                writer.WritePropertyName("Obj".ToString(), true);
                serializer.Serialize(writer, name.OBJETIVO_ESTRATEGICO);
                writer.WritePropertyName("Porc".ToString(), true);
                if (name.PORCENTAJE_AVANCE_CALCULADO != null)
                    serializer.Serialize(writer, name.PORCENTAJE_AVANCE_CALCULADO);
                else
                    serializer.Serialize(writer, 0);
                writer.WritePropertyName("AC");
                if ((name.ACCION != null) && (name.ACCION.Count > 0))
                {
                    writer.WriteStartArray();
                    foreach (ACCION acc in name.ACCION)
                    {
                        if (acc.VISIBLE && !String.IsNullOrEmpty(acc.INSTRUMENTOS_ACT))
                        {
                            writer.WriteStartObject();
                            writer.WritePropertyName("Ins", true);
                            serializer.Serialize(writer, acc.INSTRUMENTOS_ACT);
                            writer.WritePropertyName("Org", true);
                            serializer.Serialize(writer, acc.ORGANO_RESPONSABLE);
                            writer.WritePropertyName("RH", true);
                            serializer.Serialize(writer, acc.RECURSOS_HUMANOS);
                            writer.WritePropertyName("CE", true);
                            serializer.Serialize(writer, acc.COSTE_ECONOMICO);
                            writer.WritePropertyName("OM", true);
                            if (String.IsNullOrEmpty(acc.MEDIOS_OTROS))
                                serializer.Serialize(writer, "-");
                            else
                                serializer.Serialize(writer, acc.MEDIOS_OTROS);
                            writer.WritePropertyName("Tem", true);
                            serializer.Serialize(writer, acc.TEMPORALIDAD);
                            writer.WritePropertyName("Seg", true);
                            serializer.Serialize(writer, acc.SEGUIMIENTO);
                            writer.WritePropertyName("IS", true);
                            serializer.Serialize(writer, acc.PORCENTAJE_AVANCE);
                            writer.WriteEndObject();
                        }
                    }
                    writer.WriteEndArray();
                }
                else
                    serializer.Serialize(writer, "");

                writer.WriteEndObject();
            }

        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(OBJETIVO).IsAssignableFrom(objectType);
        }

    }


    public class ObjOpenDataSerializer : JsonConverter
    {
        /// <summary>
        /// Se utiliza para crear el Json para el listado público. No se mostrarán Objetivos sin Acciones, ni Acciones no visibles
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {

            var name = value as OBJETIVO;

            //Si no tiene acciones no lo escribimos
            if (name.ACCION.Where(a => a.VISIBLE && !String.IsNullOrEmpty(a.INSTRUMENTOS_ACT)).ToList().Count > 0)
            {
                writer.WriteStartObject();                
                writer.WritePropertyName("Objetivo estratégico".ToString(), true);
                serializer.Serialize(writer, name.OBJETIVO_ESTRATEGICO);
                writer.WritePropertyName("Instrumentos y actividades");
                if ((name.ACCION != null) && (name.ACCION.Count > 0))
                {
                    writer.WriteStartArray();
                    foreach (ACCION acc in name.ACCION)
                    {
                        if (acc.VISIBLE && !String.IsNullOrEmpty(acc.INSTRUMENTOS_ACT))
                        {
                            writer.WriteStartObject();
                            writer.WritePropertyName("Instrumentos y actividades", true);
                            serializer.Serialize(writer, acc.INSTRUMENTOS_ACT);
                            writer.WritePropertyName("Órgano responsable", true);
                            serializer.Serialize(writer, acc.ORGANO_RESPONSABLE);
                            writer.WritePropertyName("RRHH", true);
                            serializer.Serialize(writer, acc.RECURSOS_HUMANOS);
                            writer.WritePropertyName("Coste económico", true);
                            serializer.Serialize(writer, acc.COSTE_ECONOMICO);
                            writer.WritePropertyName("Otros medios", true);
                            if (String.IsNullOrEmpty(acc.MEDIOS_OTROS))
                                serializer.Serialize(writer, "-");
                            else
                                serializer.Serialize(writer, acc.MEDIOS_OTROS);
                            writer.WritePropertyName("Temporalidad", true);
                            serializer.Serialize(writer, acc.TEMPORALIDAD);
                            writer.WritePropertyName("Seguimiento", true);
                            serializer.Serialize(writer, acc.SEGUIMIENTO);
                            //writer.WritePropertyName("Estado seguimiento", true);                            
                            //if (acc.ESTADOS_SEGUIMIENTO != null)
                            //    serializer.Serialize(writer, acc.ESTADOS_SEGUIMIENTO.DESCRIPCION);
                            //else
                            //    serializer.Serialize(writer, "");
                            writer.WritePropertyName("Porcentaje avance", true);
                            if (acc.PORCENTAJE_AVANCE != null)
                                serializer.Serialize(writer, acc.PORCENTAJE_AVANCE);
                            else
                                serializer.Serialize(writer, "");

                            writer.WriteEndObject();
                        }
                    }
                    writer.WriteEndArray();
                }
                else
                    serializer.Serialize(writer, "");

                writer.WriteEndObject();
            }

        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(OBJETIVO).IsAssignableFrom(objectType);
        }

    }

    public partial class OBJETIVO
    {

        /// <summary>
        /// Devuelve las validaciones pendientes de las acciones relacionadas con el objetivo actual
        /// </summary>
        [NotMapped]
        public int ValidacionesPendientes
        {
            get
            {
                using (Entities c = new Entities())
                {
                    return c.CONTENIDO.OfType<ACCION>().Where(co => co.OBJETIVO_CONTENIDO_ID == this.CONTENIDO_ID && co.ESTADO_VALIDACION_ID == ESTADOS_VALIDACION.PDTE_VALIDAR).Count();
                }

            }
        }

        /// <summary>
        /// Devuelve si el Objetivo ha sido publicado (aunque pueda tener modificaciones sin validar)
        /// </summary>
        /// <returns></returns>
        public bool YaPublicado()
        {
            //Si tiene ya un texto validado estará publicado
            if (!String.IsNullOrEmpty(this.OBJETIVO_ESTRATEGICO))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Elimina el objetivo y sus acciones asociadas
        /// </summary>
        public static void EliminaObjetivo(int iObjetivoId)
        {
            using (Entities c = new Entities())
            {
                OBJETIVO obj = c.CONTENIDO.OfType<OBJETIVO>().Where(o => o.CONTENIDO_ID == iObjetivoId).FirstOrDefault();
                if (obj != null)
                { 
                    var results = c.CONTENIDO.OfType<ACCION>().Where(st => st.OBJETIVO_CONTENIDO_ID == iObjetivoId).ToList();

                    c.CONTENIDO.RemoveRange(results);
                    c.SaveChanges();
                                        
                    c.CONTENIDO.Remove(obj); 
                    c.SaveChanges();
                }
                else
                {
                    throw new Exception("Objetivo no encontrado: " + iObjetivoId.ToString());
                }
            }
        }

        /// Valida el contenido actual (todos los campos). Si está marcado como tipo de cambio:
        /// - Alta: se mostrará en la parte pública
        /// - Modificación: se modificará y se mostrará
        /// - Elminación: se eliminará (si es un objetivo, eliminará también las acciones dependientes)
        /// </summary>        
        public void Validar(Entities c)
        {
            using (c)
            {
                this.ESTADO_VALIDACION_ID = ESTADOS_VALIDACION.VALIDADO;
                                
                this.OBJETIVO_ESTRATEGICO = this.OBJETIVO_ESTRATEGICO_PDTE_VAL.ToString();

                if (this.TIPO_CAMBIO_CONTENIDO_ID == TIPO_CAMBIO_CONTENIDO.ALTA)
                {
                    this.VISIBLE = true;
                }
                else if (this.TIPO_CAMBIO_CONTENIDO_ID == TIPO_CAMBIO_CONTENIDO.MODIFICACION)
                {

                }
                else if (this.TIPO_CAMBIO_CONTENIDO_ID == TIPO_CAMBIO_CONTENIDO.ELIMINADO)
                {

                }                


                if (this.TIPO_CAMBIO_CONTENIDO_ID != TIPO_CAMBIO_CONTENIDO.ELIMINADO)
                {
                    //Si no es eliminar guardamos los cambios
                    this.TIPO_CAMBIO_CONTENIDO_ID = TIPO_CAMBIO_CONTENIDO.SIN_CAMBIOS;
                    this.FECHA_MODIFICACION = DateTime.Now;
                    c.CONTENIDO.Attach(this);
                    c.Entry(this).State = EntityState.Modified; //Siempre es modificación, ya que aunque sea Alta, ya existirá el registro en la BBDD
                    c.SaveChanges();
                }
                else
                {
                    //Si el cambio es eliminar, eliminamos
                    OBJETIVO.EliminaObjetivo(this.CONTENIDO_ID);
                }

                //Actualizamos porcentajes
                using (var dbContextTransaction = c.Database.BeginTransaction())
                {
                    try
                    {                    
                        Int16 nObjetivos = 0;
                        decimal dPorcentajeAcumulado = 0;
                        var Objetivos = c.CONTENIDO.OfType<OBJETIVO>().Where(o => o.DEPARTAMENTO_ID == this.DEPARTAMENTO_ID);
                        foreach (OBJETIVO obj in Objetivos)
                        {
                            if (obj.VISIBLE && obj.TIPO_CAMBIO_CONTENIDO_ID != 1)
                            {
                                nObjetivos++;
                                if (obj.PORCENTAJE_AVANCE_CALCULADO != null)
                                    dPorcentajeAcumulado += Convert.ToDecimal(obj.PORCENTAJE_AVANCE_CALCULADO);
                            }
                        }
                        //Actualizamos el % en su departamento
                        DEPARTAMENTO dep = c.DEPARTAMENTO.Find(this.DEPARTAMENTO_ID);
                        dep.PORCENTAJE_AVANCE_CALCULADO = (Math.Round(Decimal.Divide(dPorcentajeAcumulado, nObjetivos), 2));                                                                                                                                

                        c.DEPARTAMENTO.Attach(dep);
                        c.Entry(dep).State = EntityState.Modified;
                        c.SaveChanges();

                        dbContextTransaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        throw ex;
                    }
                }

            }

        }
 

        //Deshace el cambio pendiente
        public void DeshacerCambio(Entities c)
        {
            using (c)
            {
                if (this.TIPO_CAMBIO_CONTENIDO_ID != TIPO_CAMBIO_CONTENIDO.SIN_CAMBIOS)
                {
                    if (this.TIPO_CAMBIO_CONTENIDO_ID == TIPO_CAMBIO_CONTENIDO.ALTA)
                    {
                        EliminaObjetivo(this.CONTENIDO_ID);
                    }
                    else if (this.TIPO_CAMBIO_CONTENIDO_ID == TIPO_CAMBIO_CONTENIDO.MODIFICACION)
                    {                        
                        this.TIPO_CAMBIO_CONTENIDO_ID = TIPO_CAMBIO_CONTENIDO.SIN_CAMBIOS;
                        this.ESTADO_VALIDACION_ID = ESTADOS_VALIDACION.VALIDADO;
                        this.FECHA_MODIFICACION = DateTime.Now;
                        this.OBJETIVO_ESTRATEGICO_PDTE_VAL = this.OBJETIVO_ESTRATEGICO;
                        c.CONTENIDO.Attach(this);
                        c.Entry(this).State = EntityState.Modified; 
                        c.SaveChanges();

                    }
                    else if (this.TIPO_CAMBIO_CONTENIDO_ID == TIPO_CAMBIO_CONTENIDO.ELIMINADO)
                    {
                        this.TIPO_CAMBIO_CONTENIDO_ID = TIPO_CAMBIO_CONTENIDO.SIN_CAMBIOS;
                        this.ESTADO_VALIDACION_ID = ESTADOS_VALIDACION.VALIDADO;
                        this.OBJETIVO_ESTRATEGICO_PDTE_VAL = this.OBJETIVO_ESTRATEGICO;
                        this.FECHA_MODIFICACION = DateTime.Now;                        
                        c.CONTENIDO.Attach(this);
                        c.Entry(this).State = EntityState.Modified; 
                        c.SaveChanges();
                    }
                }
            }
        }
    }
}
