<%-- 
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
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OpenAuthProviders.ascx.cs" Inherits="PLGO.Account.OpenAuthProviders" %>

<div id="socialLoginList">
    <h4>Utilice otro servicio para iniciar sesión.</h4>
    <hr />
    <asp:ListView runat="server" ID="providerDetails" ItemType="System.String"
        SelectMethod="GetProviderNames" ViewStateMode="Disabled">
        <ItemTemplate>
            <p>
                <button type="submit" class="btn btn-default" name="provider" value="<%#: Item %>"
                    title="Inicie sesión con su <%#: Item %> cuenta.">
                    <%#: Item %>
                </button>
            </p>
        </ItemTemplate>
        <EmptyDataTemplate>
            <div>
                <p>No existen servicios de autenticación externos configurados. Consulte <a href="http://go.microsoft.com/fwlink/?LinkId=252803">este artículo</a> para obtener información sobre la configuración de esta aplicación de ASP.NET para admitir el inicio de sesión a través de servicios externos.</p>
            </div>
        </EmptyDataTemplate>
    </asp:ListView>
</div>
