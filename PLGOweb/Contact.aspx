﻿<%-- 
Autor: Autor: Javier Tomey. Gobierno de Aragón

Éste programa es software libre: usted tiene derecho a redistribuirlo y/o
modificarlo bajo los términos de la Licencia EUPL European Public License 
publicada por el organismo IDABC de la Comisión Europea, en su versión 1.2.
o posteriores.

Éste programa se distribuye de buena fe, pero SIN NINGUNA GARANTÍA, incluso sin 
las presuntas garantías implícitas de USABILIDAD o ADECUACIÓN A PROPÓSITO CONCRETO. 
Para mas información consulte la Licencia EUPL European Public License.

Usted recibe una copia de la Licencia EUPL European Public License 
junto con este programa, si por algún motivo no le es posible visualizarla, 
puede consultarla en la siguiente URL: https://eupl.eu/1.2/es/

You should have received a copy of the EUPL European Public 
License along with this program.  If not, see https://eupl.eu/1.2/en/

Vous devez avoir reçu une copie de la EUPL European Public
License avec ce programme. Si non, voir https://eupl.eu/1.2/fr/

Sie sollten eine Kopie der EUPL European Public License zusammen mit
diesem Programm. Wenn nicht, finden Sie da https://eupl.eu/1.2/de/
--%>
<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="PLGO.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    <h3>Your contact page.</h3>
    <address>
        One Microsoft Way<br />
        Redmond, WA 98052-6399<br />
        <abbr title="Phone">P:</abbr>
        425.555.0100
    </address>

    <address>
        <strong>Support:</strong>   <a href="mailto:Support@example.com">Support@example.com</a><br />
        <strong>Marketing:</strong> <a href="mailto:Marketing@example.com">Marketing@example.com</a>
    </address>
</asp:Content>