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

namespace PLGOdata
{

    public abstract partial class CONTENIDO
    {
        /// <summary>
        /// Devuelve si el usuario indicado tiene permiso de modificación sobre el contenido.
        /// Se comprobará el departamento de ambos o si el usuario es administrador o validador
        /// </summary>
        /// <param name="sUsuarioId"></param>
        /// <returns></returns>
        public bool TieneAcceso(string sUsuarioId)
        {
            bool bRet = false;
            using (Entities c = new Entities())
            {
                //Primero comprobamos si no es un usuario administrador o validador
                AspNetUsers us = c.AspNetUsers.Find(sUsuarioId);
                if (us != null)
                {
                    foreach (AspNetRoles r in us.AspNetRoles)
                    {
                        if (r.Name.Equals(AspNetRoles.ADMINISTRADOR) || r.Name.Equals(AspNetRoles.VALIDADOR))
                        {
                            bRet = true;
                            return bRet;
                        }
                    }
                    //comprobamos si el usuario tiene permiso para el departamento del contenido
                    USUARIOS u = c.USUARIOS.Find(sUsuarioId);
                    if (u != null)
                    {
                        if (this.DEPARTAMENTO_ID == u.DEPARTAMENTOID)
                            return true;
                        else
                            return false;
                    }
                    else
                    {
                        bRet = false;
                        return bRet;
                    }
                }
                else
                {
                    bRet = false;
                    return bRet;
                }
            }

        }


        //public abstract void Validar();       
    }   
}
