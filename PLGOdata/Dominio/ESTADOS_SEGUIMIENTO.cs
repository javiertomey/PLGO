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

namespace PLGOdata
{
    public partial class ESTADOS_SEGUIMIENTO
    {
        public const int SIN_INICIAR = 1;
        public const int INICIADO = 2;
        public const int TERMINADO = 3;

        public static string GetColor(int iEstadoSeguimientoId)
        {
            string strRetValue = "";
            switch (iEstadoSeguimientoId)
            {
                case SIN_INICIAR:
                    strRetValue = "#df4625";
                    break;
                case INICIADO:
                    strRetValue = "#e8b520";
                    break;
                case TERMINADO:
                    strRetValue = "#a3c451";
                    break;
                default:
                    throw new Exception("Color de Estado de Seguimiento no especificado en ESTADOS_SEGUIMIENTO.GetColor()");

            }
            return strRetValue;
        }
    }

}
