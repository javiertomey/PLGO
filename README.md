# PLGO

PLGO es una herramienta para la gestión de la publicación y cambios del Plan de Gobierno de una administración pública. Actualmente en uso por el Gobierno de Aragón. Permite gestionar para cada legislatura, departamentos, objetivos estratégicos y acciones o instrumentos asociados a dichos objetivos. 

## Getting Started

Una vez obtenido los archivos es necesario crear la base de datos, ya que el proyecto es de tipo Database-first. Se ha utilizado una base de datos Oracle pero podría funcionar con otro tipo. Existen dos archivos sql en el directorio gettingstarted para ello. 

Será necesario editar el archivo PLGOweb\web.config y configurar: la conexión a la base de datos, el directorio ldap, el envío de correo y recepción de alertas.


### Prerequisites

.Net Framework 4.5.2

Microsoft Report Viewer 2015    https://www.microsoft.com/es-es/download/details.aspx?id=45496

Microsoft System CLR Types for SQL Server 2014    https://www.microsoft.com/es-es/download/confirmation.aspx?id=42295

Si sólo se desea ejecutar será necesario servidor IIS y base de datos (configuradas librerías con Oracle 11).

El directorio PLGOweb/View contiene las url para la obtención de los datos en json para poder integrar el contenido del plan.

Para abrir el proyecto es necesario Microsoft Visual Studio Community 2015 o similar, el proyecto está configurado con dependencias de paquetes NuGet.


## Built With

Microsoft Visual Studio Community 2015 
EntityFramework
Nlog
Microsoft Report Viewer
Microsoft AspNet Identity
DocumentFormat.OpenXML



## Authors

* **Javier Tomey** - *Initial work* - (https://github.com/javiertomey)


## License

This project is licensed under the European Union Public Licence 1.2 - see the [LICENSE.txt](LICENSE.txt) file for details
