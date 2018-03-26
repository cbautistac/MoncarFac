<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Errores.ascx.cs" Inherits="Errores" %>
<link rel="Stylesheet" rev="Stylesheet" href="css/estilos.css" />
 <div class="mask zen3"> 
    <div class="carga zen4">
        <table class="centrado textoCentrado ancho200px fondoBlanco">
            <tr>
                <td class="titulo negritas fondoGris"><asp:Label ID="Label1" runat="server" Text="Información"></asp:Label></td>
            </tr>
            <tr>
                <td class="negritas pad1m">
                    <asp:Label ID="Label2" runat="server" Text="Su sesión ha caducado, por favor vuelva a iniciar sesión."></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="fondoGris">
                    <asp:ImageButton ID="btnContinuar" runat="server" AlternateText="Continuar" 
                        ToolTip="Continuar" ImageUrl="~/img/aceptar.png" onclick="btnContinuar_Click" />
                </td>
            </tr>
        </table>
    </div> 
</div> 