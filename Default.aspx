<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
    <meta name="viewport" content="width=device-width, initial-scale=1.0">

    <link href="http://fonts.googleapis.com/css?family=Open+Sans:400italic,600italic,400,600" rel="stylesheet" />
    <title>Login</title>
    <link rel="Stylesheet" rev="Stylesheet" href="css/generales.css" />
    <link href="css/signin.css" rel="stylesheet" type="text/css" />
    <link href="css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="css/font-awesome.css" rel="stylesheet" />

    <link rel="stylesheet" href="assets/css/reset.css">
        <link rel="stylesheet" href="assets/css/supersized.css">
        <link rel="stylesheet" href="assets/css/style.css">

        <!-- HTML5 shim, for IE6-8 support of HTML5 elements -->
        <!--[if lt IE 9]>
            <script src="http://html5shim.googlecode.com/svn/trunk/html5.js"></script>
        <![endif]-->

    <script type="text/javascript">
        function cierraSesion(usu) {
            var accion = confirm("Tu usuario cuenta con una sesión abierta, ¿Deseas cerrarla?");
            if (accion)
                __doPostBack('cierraSesion', usu);
            else
                return accion;
        }
    </script>
</head>
<body>
    
<div class="page-container">
      
        <asp:Image ID="Image1" runat="server" ImageUrl="img/logo.png" alt="e-Factura 3.3" class="colorMorado textoBold" height="200" width="480" /><br /><br />
            <h1>ACCESO</h1><br /><br />
            <h1>e-Factura 3.3</h1>
            <br />
            <asp:Label ID="lblversion" runat="server" CssClass="t10 alingMiddle colorBlanco" ></asp:Label>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" />
        <!--<div class="encabezado">
            <table class="ancho100 colorBlanco alineado alto100">
                <tr>
                    <td>
                        <asp:Label ID="lbltitulo" runat="server" Text="e-Taller" CssClass="marginLeft t1 colorMorado textoBold" />
                    </td>
                </tr>
            </table>
        </div>-->

        <asp:TextBox ID="txtUsuarioLog" runat="server" CssClass="username" MaxLength="20" placeholder="Usuario" />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Text="*" ForeColor="Red" ValidationGroup="log" ControlToValidate="txtUsuarioLog" ErrorMessage="Debe indicar el usuario." CssClass="pull-right" />
        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Text="*" ForeColor="Red" ValidationGroup="log" ErrorMessage="El usuario debe contener de entre 3 y 20 caracteres." CssClass="pull-right" ControlToValidate="txtUsuarioLog" ValidationExpression="[a-zA-Z0-9]{3,20}" />


        <asp:TextBox ID="txtContraseñaLog" runat="server" CssClass="password" TextMode="Password" MaxLength="20" placeholder="Contraseña" />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Text="*" ForeColor="Red" ValidationGroup="log" ControlToValidate="txtContraseñaLog" ErrorMessage="Debe indicar la contraseña." CssClass="pull-right" />
        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" Text="*" ForeColor="Red" ValidationGroup="log" ErrorMessage="La contraseña debe contener de entre 5 y 20 caracteres." CssClass="pull-right" ControlToValidate="txtContraseñaLog" ValidationExpression="[a-zA-Z0-9]{5,20}" />

        <asp:LinkButton ID="lnkRecPas" runat="server" Text="Olvidaste tu Contraseña" CssClass="pull-left link t18 colorEtiqueta inline" OnClick="lnkRecPas_Click" />
        <asp:Button ID="btnLog1" runat="server" Text=" Ingresar " OnClick="btnLog1_Click" CssClass="button pull-right inline" ValidationGroup="log" />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>        
                <div class="pad1m textoCentrado">
                    <asp:Label ID="lblErrorLog" runat="server" CssClass="erroresLog" />
                    <asp:ValidationSummary ID="ValidationSummary1" ValidationGroup="log" CssClass="erroresLog" runat="server" DisplayMode="List" />
                </div>

                <asp:Panel ID="mask" runat="server" CssClass="mask" Visible="false"></asp:Panel>
                <asp:Panel ID="pnlperfiles" runat="server" CssClass="popUp zen2  textoCentrado ancho40 centrado" Visible="false">
                    <table class="ancho100">
                        <tr class="ancho100 centrado  ">
                            <td colspan="2" class="encabezadoTabla roundTopLeft roundTopRight">
                                <asp:Label ID="Label4" runat="server" Text="Seleccione Perfil" CssClass="t22 colorMorado textoBold" />
                            </td>
                        </tr>
                        <tr>
                            <td class="textoCentrado negritas" colspan="2">
                                <asp:RadioButtonList ID="rdbPerfil" runat="server" CssClass="ancho100"
                                    RepeatColumns="2" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="1" Selected="True">Administración</asp:ListItem>
                                    <asp:ListItem Value="2">Operación</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td class="textoCentrado ancho50 ">
                                <asp:LinkButton ID="btnAceptarPerfil" runat="server" CssClass="alingMiddle link colorBlanco btn btn-success" OnClick="btnAceptarPerfil_Click1" Text="Aceptar"><i class="icon-ok-sign tbtn alingMiddle"></i></asp:LinkButton>
                            </td>
                            <td class="textoCentrado ancho50 ">
                                <asp:LinkButton ID="btnCancelarPerfil" runat="server" CssClass="alingMiddle link colorBlanco btn btn-danger" OnClick="btnCancelarPerfil_Click1" Text="Cancelar"><i class="icon-remove-sign tbtn alingMiddle"></i></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="pnlTalleres" runat="server" CssClass="popUp zen2  textoCentrado ancho40 centrado" Visible="false">
                    <table class="ancho100 ">
                        <tr class="ancho100 centrado">
                            <td colspan="2" class="encabezadoTabla roundTopLeft roundTopRight ancho100">
                                <asp:Label ID="Label5" runat="server" Text="Seleccione Empresa Taller" CssClass="t22 colorMorado textoBold"></asp:Label>
                                <asp:Label ID="lblArrastrePerfil" runat="server" Visible="false" />
                            </td>
                        </tr>
                        <tr class="ancho100 centrado marTop">
                            <td class="textoCentrado ancho50" colspan="2">
                                <asp:Label ID="Label6" runat="server" Text="Empresa:" CssClass="textoBold textoIzquierda colorNegro"></asp:Label><br />
                                <asp:Label ID="lblUsuario" runat="server" Visible="false"></asp:Label>
                                <asp:DropDownList ID="ddlEmpresa" runat="server" DataSourceID="SqlDataSource1" DataTextField="razon_social" DataValueField="id_empresa" CssClass="ancho100"></asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select distinct ut.id_empresa,e.razon_social from Usuarios_Taller ut inner join Empresas e on e.id_empresa=ut.id_empresa where ut.id_usuario=@id_usuario">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="lblUsuario" Name="id_usuario" PropertyName="Text" Type="Int32" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </td>
                        </tr>
                        <tr class="ancho100 centrado">
                            <td class="textoCentrado ancho50" colspan="2">
                                <asp:Label ID="Label7" runat="server" Text="Taller:" CssClass="textoBold textoIzquierda colorNegro"></asp:Label><br />
                                <asp:DropDownList ID="ddlTaller" runat="server" DataSourceID="SqlDataSource2" CssClass="ancho100" DataTextField="nombre_taller" DataValueField="id_taller"></asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select distinct ut.id_taller,t.nombre_taller from Usuarios_Taller ut inner join Talleres t on t.id_taller=ut.id_taller where ut.id_usuario=@id_usuario and ut.id_empresa=@id_empresa">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="lblUsuario" Name="id_usuario" PropertyName="Text" Type="Int32" />
                                        <asp:ControlParameter ControlID="ddlEmpresa" DefaultValue="0" Name="id_empresa" PropertyName="SelectedValue" Type="Int32" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </td>
                        </tr>
                        <tr class="ancho100 centrado">
                            <td class="textoCentrado ancho50">
                                <asp:LinkButton ID="btnAceptarTaller" runat="server" CssClass="alingMiddle link colorBlanco btn btn-success" ToolTip="Aceptar" OnClick="btnAceptarTaller_Click1"><i class="icon-ok-sign tbtn alingMiddle"></i></asp:LinkButton>
                            </td>
                            <td class="textoCentrado ancho50">
                                <asp:LinkButton ID="btnCancelarTaller" runat="server" CssClass="alingMiddle link colorBlanco btn btn-danger" ToolTip="Cancelar" OnClick="btnCancelarTaller_Click1"><i class="icon-remove-sign tbtn alingMiddle"></i></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                    <ProgressTemplate>
                        <asp:Panel ID="pnlMaskLoad" runat="server" CssClass="maskLoad"></asp:Panel>
                        <asp:Panel ID="pnlCargando" runat="server" CssClass="pnlPopUpLoad">
                            <asp:Image ID="imgLoad" runat="server" ImageUrl="~/img/loading.gif" CssClass="ancho100" />
                        </asp:Panel>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnLog1" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>

        <!-- Cumpleaños -->
        <asp:Panel ID="PanelMaskCumple" runat="server" CssClass="mask zen1" Visible="false"></asp:Panel>
        <asp:Panel ID="PanelCumple" runat="server" CssClass="popUp zen2 textoCentrado ancho50 centrado" ScrollBars="Auto" Visible="false">
            <table class="ancho100">
                <tr class="ancho100 centrado">
                    <td class="ancho95 text-center encabezadoTabla roundTopLeft">
                        <asp:Label ID="Label13" runat="server" Text="Feliz Cumpleaños" CssClass="t22 colorMorado textoBold" />
                    </td>
                    <td class="ancho5 text-right encabezadoTabla roundTopRight">
                        <asp:LinkButton ID="btnSalirCumple" runat="server" CssClass="btn btn-danger alingMiddle" ToolTip="Cerrar" OnClick="btnSalirCumple_Click"><i class="fa fa-remove t18"></i></asp:LinkButton>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Panel1" runat="server" CssClass="panelCumple text-center" ScrollBars="Auto">
                <div class="table-responsive">
                    <asp:GridView ID="GridCumpleañeros" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSourceCumple" ShowHeader="false" CssClass="table table-bordered" EmptyDataText="Hoy nadie cumple años" EmptyDataRowStyle-CssClass="errores" PageSize="10" AllowPaging="true" AllowSorting="true">
                        <Columns>
                            <asp:BoundField DataField="nombre" ShowHeader="false" ReadOnly="True" SortExpression="nombre" />
                        </Columns>
                    </asp:GridView>
                    <asp:SqlDataSource runat="server" ID="SqlDataSourceCumple" ConnectionString='<%$ ConnectionStrings:PVW %>'
                        SelectCommand="select ltrim(rtrim(nombres))+' '+ltrim(rtrim(apellido_paterno))+' '+ltrim(rtrim(apellido_materno)) as nombre from empleados where datepart(dd,fecha_de_nacimiento)=datepart(dd,getdate()) and datepart(mm,fecha_de_nacimiento)=datepart(mm,getdate())"></asp:SqlDataSource>
                </div>
            </asp:Panel>
        </asp:Panel>

    </form>

</div>

        <!-- Javascript -->
        <script src="assets/js/jquery-1.8.2.min.js"></script>
        <script src="assets/js/supersized.3.2.7.min.js"></script>
        <script src="assets/js/supersized-init.js"></script>
        <script src="assets/js/scripts.js"></script>
</body>
</html>
