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

namespace PLGO.Tests
{
    //[TestClass]
    //public class EFtests
    //{

    //    [TestMethod]
    //    public void Inicial()
    //    {

    //        using (Entities c = new Entities())
    //        {
    //            USUARIOS u = null;
    //            try
    //            {
    //                /*Falta AspNetUsers  */
    //                u = new USUARIOS() { APELLIDOS = "APE", NOMBRE = "NOMBRE", DEPARTAMENTOID = 1, USUARIOID = "50612fe6-f39d-44ca-b071-b6eb7a27e4fd", FECHA_ALTA = DateTime.Now };
    //                c.USUARIOS.Add(u);
    //                c.SaveChanges();

    //            }
    //            catch (Exception ex)
    //            {
    //                Assert.Fail(" ERROR USUARIO" + ex.Message);
    //            }


    //            try
    //            {
    //                OBJETIVO o = new OBJETIVO()
    //                {                        
    //                    AUTOR_CREACION = u,
    //                    DEPARTAMENTO_ID = 1,
    //                    ESTADO_VALIDACION_ID = ESTADOS_VALIDACION.PDTE_VALIDAR,
    //                    FECHA_CREACION = DateTime.Now,                        
    //                    TIPO_CAMBIO_CONTENIDO_ID = TIPO_CAMBIO_CONTENIDO.ALTA,
    //                    VISIBLE = false                        
    //                };
    //                c.CONTENIDO.Add(o);
    //                c.SaveChanges();

    //            }
    //            catch (Exception ex)
    //            {
    //                Assert.Fail(" ERROR OBJETIVO: " + ex.Message);
    //            }


    //        }



    //    }


    //    [TestMethod]
    //    public void AniadeContenido()
    //    {
    //        //Comprobamos inserts con identity (sequence)

    //        using (Entities c = new Entities())
    //        {
    //            USUARIOS u = null;
    //            try
    //            {
    //                u = c.USUARIOS.Find("50612fe6-f39d-44ca-b071-b6eb7a27e4fd");

    //            }
    //            catch (Exception ex)
    //            {
    //                Assert.Fail(" ERROR USUARIO" + ex.Message);
    //            }

    //            OBJETIVO o = null;
    //            try
    //            {
    //                o = new OBJETIVO()
    //                {
    //                    AUTOR_CREACION = u,
    //                    DEPARTAMENTO_ID = 1,
    //                    ESTADO_VALIDACION_ID = ESTADOS_VALIDACION.PDTE_VALIDAR,
    //                    FECHA_CREACION = DateTime.Now,                        
    //                    TIPO_CAMBIO_CONTENIDO_ID = TIPO_CAMBIO_CONTENIDO.ALTA,
    //                    VISIBLE = true                        
    //                };
    //                c.CONTENIDO.Add(o);
    //                c.SaveChanges();

    //            }
    //            catch (Exception ex)
    //            {
    //                Assert.Fail(" ERROR OBJETIVO: " + ex.Message);
    //            }


    //            try
    //            {
    //                ACCION a = new ACCION()
    //                {
    //                    AUTOR_CREACION = u,
    //                    DEPARTAMENTO_ID = 1,                        
    //                    ESTADO_VALIDACION_ID = ESTADOS_VALIDACION.PDTE_VALIDAR,
    //                    FECHA_CREACION = DateTime.Now,
    //                    SEGUIMIENTO_PDTE_VAL = "SEGUIMIENTO PDT VAL",
    //                    COSTE_ECONOMICO_PDTE_VAL = "COSTE ECON",
    //                    MEDIOS_OTROS_PDTE_VAL = "OTROS MEDIOS",
    //                    TEMPORALIDAD_PDTE_VAL = "temporalidad",                        
    //                    ORGANO_RESPONSABLE_PDTE_VAL ="ORG RESP",
    //                    TIPO_CAMBIO_CONTENIDO_ID = TIPO_CAMBIO_CONTENIDO.ALTA,
    //                    OBJETIVO = o,
    //                    VISIBLE = true
                        
    //                };
    //                c.CONTENIDO.Add(a);
    //                c.SaveChanges();

    //            }
    //            catch (Exception ex)
    //            {
    //                Assert.Fail(" ERROR OBJETIVO: " + ex.Message);
    //            }

    //            LEGISLATURA l = null;

    //            try
    //            {
    //                l = new LEGISLATURA() { DESCRIPCION = "Legislatura - pruebas unitarias" };
    //                c.LEGISLATURA.Add(l);
    //                c.SaveChanges();
    //            }
    //            catch (Exception ex)
    //            {
    //                Assert.Fail(" ERROR LEGISLATURA: " + ex.Message);
    //            }

    //            DEPARTAMENTO d;
    //            try
    //            {
    //                d = new DEPARTAMENTO() { DESCRIPCION = "Departamento - pruebas unitarias", LEGISLATURA = l, VISIBLE = true };
    //                c.DEPARTAMENTO.Add(d);
    //                c.SaveChanges();
    //            }
    //            catch (Exception ex)
    //            {
    //                Assert.Fail(" ERROR DEPARTAMENTO: " + ex.Message);
    //            }


    //        }
    //    }
    //}
}
