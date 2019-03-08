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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PLGOdata;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace PLGO.Tests
{
    [TestClass]
    public class EFtestsCiclo
    {
        USUARIOS u = null;
        LEGISLATURA l = null;
        DEPARTAMENTO d = null;
        OBJETIVO o = null;
        ACCION a = null;

        [TestMethod]
        public void Inicial()
        {
            using (Entities c = new Entities())
            {
                
                try
                {
                    /*Falta AspNetUsers  */
                    u = c.USUARIOS.FirstOrDefault(); //cogemos el primer usuario que haya en la BBDD
                }
                catch (Exception ex)
                {
                    Assert.Fail(" ERROR USUARIO" + ex.Message);
                }

                /*
                try
                {
                    OBJETIVO o = new OBJETIVO()
                    {                        
                        AUTOR_CREACION = u,
                        DEPARTAMENTO_ID = 1,
                        ESTADO_VALIDACION_ID = ESTADOS_VALIDACION.PDTE_VALIDAR,
                        FECHA_CREACION = DateTime.Now,                        
                        TIPO_CAMBIO_CONTENIDO_ID = TIPO_CAMBIO_CONTENIDO.ALTA,
                        VISIBLE = false                        
                    };
                    c.CONTENIDO.Add(o);
                    c.SaveChanges();

                }
                catch (Exception ex)
                {
                    Assert.Fail(" ERROR OBJETIVO: " + ex.Message);
                }*/


            }



        }

        [TestMethod]
        public void CreaLegislatura()
        {
           
                using (Entities c = new Entities())
                {
                    l = new LEGISLATURA()
                    {
                        DESCRIPCION = "Legislatura Desde Test"
                    };
                    c.LEGISLATURA.Add(l);
                }            
        }

        [TestMethod]
        public void CreaDepartamento()
        {
            if (u == null)
                Inicial();

            if (l == null)
                CreaLegislatura();

            using (Entities c = new Entities())
            {
                d = new DEPARTAMENTO()
                {
                    DESCRIPCION = "Departamento Desde Test",                    
                    LEGISLATURA_ID = l.LEGISLATURA_ID,
                    VISIBLE = true
                };
                c.DEPARTAMENTO.Add(d);
            }
        }

        [TestMethod]
        public void CreaObjetivo()
        {

            using (Entities c = new Entities())
            {
                if (u == null)
                    Inicial();

                if (l == null)
                    CreaLegislatura();

                if (d == null)
                    CreaDepartamento();

                if (o == null)
                {
                    CreaObjetivo();
                }

                o = new OBJETIVO()
                {
                AUTOR_CREACION = u,
                    DEPARTAMENTO_ID = d.DEPARTAMENTO_ID,                    
                    ESTADO_VALIDACION_ID = ESTADOS_VALIDACION.PDTE_VALIDAR,
                    FECHA_CREACION = DateTime.Now,
                    TIPO_CAMBIO_CONTENIDO_ID = TIPO_CAMBIO_CONTENIDO.ALTA,
                    OBJETIVO_ESTRATEGICO_PDTE_VAL = "Objetivo desde Test"
                };
                c.CONTENIDO.Add(o);
            }
        }


        [TestMethod]
        public void CreaAccion()
        {

            try
            {
                using (Entities c = new Entities())
                {
                    if (u == null)
                        Inicial();

                    if (l == null)
                        CreaLegislatura();

                    if (d == null)
                        CreaDepartamento();

                    if (o == null)
                    {
                        CreaObjetivo(); 
                    }

                    a = new ACCION()
                    {
                        AUTOR_CREACION = u,
                        DEPARTAMENTO_ID = d.DEPARTAMENTO_ID,
                        ESTADO_VALIDACION_ID = ESTADOS_VALIDACION.PDTE_VALIDAR,
                        FECHA_CREACION = DateTime.Now,
                        TIPO_CAMBIO_CONTENIDO_ID = TIPO_CAMBIO_CONTENIDO.ALTA,

                        SEGUIMIENTO_PDTE_VAL = "Objetivo desde Test",
                        COSTE_ECONOMICO_PDTE_VAL = "COSTE ECON",
                        MEDIOS_OTROS_PDTE_VAL = "OTROS MEDIOS",
                        TEMPORALIDAD_PDTE_VAL = "temporalidad",
                        ESTADO_SEGUIMIENTO_ID_PDTE_VAL = ESTADOS_SEGUIMIENTO.INICIADO,
                        INDICADOR_SEGUIMIENTO_PDTE_VAL = "Indicador Seg",
                        INSTRUMENTOS_ACT_PDTE_VAL = "Inst Act",
                        RECURSOS_HUMANOS_PDTE_VAL = "rrhh",
                        ORGANO_RESPONSABLE_PDTE_VAL = "ORG RESP",
                        OBJETIVO = o
                    };
                    c.CONTENIDO.Add(a);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }





       
    }
}
