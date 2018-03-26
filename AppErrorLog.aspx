<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AppErrorLog.aspx.cs" Inherits="AppErrorLog" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Error de la Aplicacion</title>
    <link rel="Stylesheet" rev="Stylesheet" href="css/estilos.css" />
</head>
<body>
    <form id="form1" runat="server">            
    <asp:Panel CssClass="cargaE" runat="server" ID="pnlCont">
        <table class="centrado textoCentrado ancho50 fondoBlanco top">
            <tr>
                <td class="titulo negritas fondoGris"><asp:Label ID="Label1" runat="server" Text="Información"></asp:Label></td>
            </tr>
            <tr>
                <td class="negritas pad1m">
                    <asp:Label ID="Label2" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="fondoGris">
                    <asp:ImageButton ID="btnContinuar" runat="server" AlternateText="Continuar" 
                        ToolTip="Continuar" ImageUrl="~/img/aceptar.png" onclick="btnContinuar_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel> 
    </form>
</body>
</html>
