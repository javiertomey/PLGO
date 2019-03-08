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

//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PLGOdata
{
    using System;
    using System.Collections.Generic;
    
    public abstract partial class CONTENIDO
    {
        public int CONTENIDO_ID { get; set; }
        public string AUTOR_CREACION_USUARIO_ID { get; set; }
        public System.DateTime FECHA_CREACION { get; set; }
        public string AUTOR_MODIFICACION_USUARIO_ID { get; set; }
        public Nullable<System.DateTime> FECHA_MODIFICACION { get; set; }
        public string VALIDADOR_USUARIO_ID { get; set; }
        public int ESTADO_VALIDACION_ID { get; set; }
        public Nullable<int> TIPO_CAMBIO_CONTENIDO_ID { get; set; }
        public int DEPARTAMENTO_ID { get; set; }
        public bool VISIBLE { get; set; }
        public string COMENTARIO_VALIDADOR { get; set; }
    
        public virtual ESTADOS_VALIDACION ESTADOS_VALIDACION { get; set; }
        public virtual TIPO_CAMBIO_CONTENIDO TIPO_CAMBIO_CONTENIDO { get; set; }
        public virtual DEPARTAMENTO DEPARTAMENTO { get; set; }
        public virtual USUARIOS AUTOR_CREACION { get; set; }
        public virtual USUARIOS AUTOR_MODIFICACION { get; set; }
        public virtual USUARIOS VALIDADOR { get; set; }
    }
}