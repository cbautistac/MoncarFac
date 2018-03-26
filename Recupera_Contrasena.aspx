<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Recupera_Contrasena.aspx.cs" Inherits="Recupera_Contrasena" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="Stylesheet" rev="Stylesheet" href="css/estilos.css" />
    <link rel="icon" type="image/ico" href="img/ico_moncar.png" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="encabezado">
        <table class="ancho100">
            <tr>
                <td class="ancho70 textoIzquierda textobold">
                    <asp:Label ID="lbltitulo" runat="server" Text="MoncarWeb" CssClass="titulo negritas colorMoncarAzul" />&nbsp;
                </td>
                <td class="ancho30 textoDer">
                    <asp:Image ID="Image1" AlternateText="e-Lockers" ImageUrl="~/IMG/logo.png" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    <div class="textoCentrado ancho100 top errores">
        <asp:Label ID="lblParametros" runat="server" CssClass="errores top" />
    </div>
    <asp:Panel ID="PanelContraseña" runat="server" CssClass="textoCentrado centrado ancho50">
        <table class="centrado">
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Recuperar Contraseña" /></td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="txtNick" runat="server" placeholder="Usuario" MaxLength="15" />
                    <asp:RequiredFieldValidator ControlToValidate="txtNick" Text="*" CssClass="errores" ID="RequiredFieldValidator1" runat="server" ErrorMessage="Necesita colocar un Usuario" />
                </td>
                <td>
                    <asp:TextBox ID="txtCorreo" runat="server" placeholder="Correo" MaxLength="150" />
                    <asp:RequiredFieldValidator ControlToValidate="txtCorreo" Text="*" CssClass="errores" ID="RequiredFieldValidator2" runat="server" ErrorMessage="Necesita colocar un correo " />
                    <asp:RegularExpressionValidator ControlToValidate="txtCorreo" Text="*" CssClass="errores"  ID="RegularExpressionValidator1" runat="server" ErrorMessage="La información ingresada no corresponde a un correo" ValidationExpression="(\w[-._\w]*\w@\w[-._\w]*\w\.\w{2,3})" />
                </td>
                <td>
                    <asp:ImageButton ID="btnAceptar" runat="server" AlternateText="Aceptar" ToolTip="Aceptar" OnClick="btnAceptar_Click" ImageUrl="~/img/aceptar.png" />
                </td>
            </tr>
            <tr>
                <td colspan="3" class="textoCentrado">
                    <asp:Label ID="lblErrores" runat="server" CssClass="errores" />
                </td>
            </tr>
            <tr>
                <td colspan="3" class="textoCentrado">
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="errores" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblSeleccione" runat="server" Text="Seleccione un usuario" Visible="false" />
                </td>
                <td>
                    <asp:DropDownList ID="ddlUsuarios" runat="server" Visible="false" >
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:ImageButton ID="btnAceptarUsuarios" runat="server" AlternateText="Aceptar" ToolTip="Aceptar" OnClick="btnAceptarUsuarios_Click" ImageUrl="~/img/aceptar.png" Visible="false" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    </form>
</body>
</html>
