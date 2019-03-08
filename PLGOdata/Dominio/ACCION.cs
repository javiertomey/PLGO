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


namespace PLGOdata
{
    
    public partial class ACCION
    {
        /// <summary>
        /// Devuelve si el Objetivo ha sido publicado (aunque pueda tener modificaciones sin validar)
        /// </summary>
        /// <returns></returns>
        public bool YaPublicado()
        {
            //Si tiene ya un texto validado estará publicado
            if (!String.IsNullOrEmpty(this.SEGUIMIENTO))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Valida el contenido actual (todos los campos). Si está marcado como tipo de cambio:
        /// - Alta: se mostrará en la parte pública
        /// - Modificación: se modificará y se mostrará
        /// - Elminación: se eliminará (si es un objetivo, eliminará también las acciones dependientes)
        /// </summary>        
        public void Validar(Entities c)
        {
            bool bModificadoPorcentajeAvance = false;
            if (this.PORCENTAJE_AVANCE != this.PORCENTAJE_AVANCE_PDTE_VAL) bModificadoPorcentajeAvance = true;

                this.ESTADO_VALIDACION_ID = ESTADOS_VALIDACION.VALIDADO;

                this.SEGUIMIENTO = this.SEGUIMIENTO_PDTE_VAL.ToString(); //ToString: para que haga una copia de la cadena        
                this.RECURSOS_HUMANOS = this.RECURSOS_HUMANOS_PDTE_VAL.ToString();                
                this.COSTE_ECONOMICO = this.COSTE_ECONOMICO_PDTE_VAL.ToString();                
                this.INSTRUMENTOS_ACT = this.INSTRUMENTOS_ACT_PDTE_VAL.ToString();                
                this.TEMPORALIDAD = this.TEMPORALIDAD_PDTE_VAL.ToString();                
                this.INDICADOR_SEGUIMIENTO = this.INDICADOR_SEGUIMIENTO_PDTE_VAL.ToString();
                //this.ESTADO_SEGUIMIENTO_ID = this.ESTADO_SEGUIMIENTO_ID_PDTE_VAL;                
                this.PORCENTAJE_AVANCE = this.PORCENTAJE_AVANCE_PDTE_VAL;
                this.ORGANO_RESPONSABLE = this.ORGANO_RESPONSABLE_PDTE_VAL.ToString();
                this.MEDIOS_OTROS = this.MEDIOS_OTROS_PDTE_VAL.ToString();

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

                    

                    if (!bModificadoPorcentajeAvance)
                    {
                        using (c)
                        {
                            c.CONTENIDO.Attach(this);
                            c.Entry(this).State = EntityState.Modified;
                            c.SaveChanges();
                        }
                    }
                    else
                    { //hay que actualizar los % de avance calculados:


                        using (var dbContextTransaction = c.Database.BeginTransaction())                        
                        {
                            try
                            {                               
                                //Guardamos los cambios pendiente
                                c.CONTENIDO.Attach(this);
                                c.Entry(this).State = EntityState.Modified;
                                c.SaveChanges();

                                //Actualizamos el % en su objetivo
                                Decimal dPorcentajeAcumulado = 0;
                                Int16 nAcciones = 0;

                                var Acciones = c.CONTENIDO.OfType<ACCION>().Where(o => o.OBJETIVO_CONTENIDO_ID == this.OBJETIVO_CONTENIDO_ID);

                                foreach (ACCION acc in Acciones)
                                {
                                    if (acc.VISIBLE && !String.IsNullOrEmpty(acc.INSTRUMENTOS_ACT) && acc.TIPO_CAMBIO_CONTENIDO_ID != 1)
                                    {
                                        nAcciones++;
                                        if (acc.PORCENTAJE_AVANCE != null)
                                            dPorcentajeAcumulado += Convert.ToDecimal(acc.PORCENTAJE_AVANCE);
                                    }
                                }
                                this.OBJETIVO.PORCENTAJE_AVANCE_CALCULADO = (Math.Round(Decimal.Divide(dPorcentajeAcumulado, nAcciones), 2));
                                c.CONTENIDO.Attach(this);
                                c.Entry(this.OBJETIVO).State = EntityState.Modified;
                                c.SaveChanges();

                                Int16 nObjetivos = 0;
                                dPorcentajeAcumulado = 0;
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
                                dep.PORCENTAJE_AVANCE_CALCULADO = (Math.Round(Decimal.Divide(dPorcentajeAcumulado, nObjetivos), 2));  //Cálculo de media, pero como se muestra en la parte pública el % de avance según iniciados/2+terminados lo aplicamos aquí:
                                //dep.PORCENTAJE_AVANCE_CALCULADO = DEPARTAMENTO.GetEvolucion(1);

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
                else
                {
                    //Si el cambio es eliminar, eliminamos                    
                    ACCION a = c.CONTENIDO.OfType<ACCION>().Where(st => st.CONTENIDO_ID  == this.CONTENIDO_ID).FirstOrDefault();
                    c.CONTENIDO.Remove(a);
                    c.SaveChanges();
                    
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

                        c.CONTENIDO.Remove(this);
                        c.SaveChanges();

                        
                    }
                    else if (this.TIPO_CAMBIO_CONTENIDO_ID == TIPO_CAMBIO_CONTENIDO.MODIFICACION)
                    {
                        this.TIPO_CAMBIO_CONTENIDO_ID = TIPO_CAMBIO_CONTENIDO.SIN_CAMBIOS;
                        this.ESTADO_VALIDACION_ID = ESTADOS_VALIDACION.VALIDADO;
                        this.FECHA_MODIFICACION = DateTime.Now;

                        this.RECURSOS_HUMANOS_PDTE_VAL = this.RECURSOS_HUMANOS;
                        this.COSTE_ECONOMICO_PDTE_VAL = this.COSTE_ECONOMICO;
                        this.INSTRUMENTOS_ACT_PDTE_VAL = this.INSTRUMENTOS_ACT;
                        this.TEMPORALIDAD_PDTE_VAL = this.TEMPORALIDAD;
                        this.INDICADOR_SEGUIMIENTO_PDTE_VAL = this.INDICADOR_SEGUIMIENTO;
                        //this.ESTADO_SEGUIMIENTO_ID_PDTE_VAL = this.ESTADO_SEGUIMIENTO_ID;
                        this.PORCENTAJE_AVANCE_PDTE_VAL = this.PORCENTAJE_AVANCE;
                        this.ORGANO_RESPONSABLE_PDTE_VAL = this.ORGANO_RESPONSABLE;
                        this.MEDIOS_OTROS_PDTE_VAL = this.MEDIOS_OTROS;

                        c.CONTENIDO.Attach(this);
                        c.Entry(this).State = EntityState.Modified;
                        c.SaveChanges();

                    }
                    else if (this.TIPO_CAMBIO_CONTENIDO_ID == TIPO_CAMBIO_CONTENIDO.ELIMINADO)
                    {
                        this.TIPO_CAMBIO_CONTENIDO_ID = TIPO_CAMBIO_CONTENIDO.SIN_CAMBIOS;
                        this.ESTADO_VALIDACION_ID = ESTADOS_VALIDACION.VALIDADO;
                        this.FECHA_MODIFICACION = DateTime.Now;


                        this.RECURSOS_HUMANOS_PDTE_VAL = this.RECURSOS_HUMANOS;
                        this.COSTE_ECONOMICO_PDTE_VAL = this.COSTE_ECONOMICO;
                        this.INSTRUMENTOS_ACT_PDTE_VAL = this.INSTRUMENTOS_ACT;
                        this.TEMPORALIDAD_PDTE_VAL = this.TEMPORALIDAD;
                        this.INDICADOR_SEGUIMIENTO_PDTE_VAL = this.INDICADOR_SEGUIMIENTO;
                        //this.ESTADO_SEGUIMIENTO_ID_PDTE_VAL = this.ESTADO_SEGUIMIENTO_ID;
                        this.PORCENTAJE_AVANCE_PDTE_VAL = this.PORCENTAJE_AVANCE;
                        this.ORGANO_RESPONSABLE_PDTE_VAL = this.ORGANO_RESPONSABLE;
                        this.MEDIOS_OTROS_PDTE_VAL = this.MEDIOS_OTROS;

                        c.CONTENIDO.Attach(this);
                        c.Entry(this).State = EntityState.Modified;
                        c.SaveChanges();
                    }
                }
            }
        }
    }
}
