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

namespace PLGOdata
{
    [MetadataType(typeof(USUARIOS_Validation))]
    public partial class USUARIOS
    {
        public static void setUltimoAcceso(string sUsuarioId)
        {
            using (Entities c = new Entities())
            {
                USUARIOS u = c.USUARIOS.Find(sUsuarioId);
                u.ULTIMO_ACCESO = DateTime.Now;
                c.Entry(u).State = EntityState.Modified;
                c.SaveChanges();
            }
        }
    }

    public class USUARIOS_Validation
    {
        [JsonIgnore]
        public virtual ICollection<CONTENIDO> CONTENIDO_CREADO { get; set; }
        
        [JsonIgnore]
        public virtual ICollection<CONTENIDO> CONTENIDO_MODIFICADO { get; set; }
        
        [JsonIgnore]
        public virtual ICollection<CONTENIDO> CONTENIDO_VALIDADO { get; set; }
    }
}
