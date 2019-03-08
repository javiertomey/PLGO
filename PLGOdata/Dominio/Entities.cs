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
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;


namespace PLGOdata{

    public partial class Entities : DbContext
    {
        //Recupera la información de los tipos de excepción personalizada para EntityFramework y relanza la excepción como Excepción normal para tener toda la información
        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                string sErrorEF = "DbEntityValidationException ";

                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        sErrorEF = ("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                    }
                }
                throw new Exception("DbEntityValidationException " + sErrorEF + ex.Message + " " + ex.InnerException + " ");
            }
            catch (DbUpdateException ex)
            {
                string sErrorEF = "DbUpdateException " + ex.InnerException.InnerException + "";
                try
                {
                    foreach (var result in ex.Entries)
                    {
                        sErrorEF += "Type: " + result.Entity.GetType().Name + " was part of the problem.";
                    }
                }
                catch (Exception exc)
                {
                    sErrorEF += "Error parsing DbUpdateException: " + exc.ToString();
                }
                throw new Exception("DbUpdateException " + sErrorEF + " " + ex.Message + " " + ex.InnerException + " ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}