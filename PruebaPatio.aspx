<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PruebaPatio.aspx.cs" Inherits="PruebaPatio" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="css/cloud-admin.css" rel="stylesheet" type="text/css" />
    <link href="css/menus.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="css/responsive.css" />
    <link href="css/4.4.0/css/font-awesome.min.css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="css/generales.css" />
    <link href="css/dashboard.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div style="width:90%; height:100%; margin: 0 auto; text-align:right;">
        
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <h1 class="colorBlanco">Taller Autos</h1>
        <h2 class="colorEtiqueta text-center">Entregas</h2>
        <telerik:RadRotator RenderMode="Lightweight" ID="RadRotator1" runat="server"  CssClass="top0"
            ItemWidth="810px"  ItemHeight="600px" ScrollDuration="600" FrameDuration="10000"
            PauseOnMouseOver="false" RotatorType="CarouselButtons" DataSourceID="SqlDataSource1" ScrollDirection="Up" >
            <ItemTemplate>
                <div class="form textoBold">
                    <h3 class="colorNegro textoBold">
                        Orden:&nbsp;<asp:Label ID="no_ordenLabel" runat="server" Text='<%# Eval("no_orden") %>' /></h3>
                    <div class="form-group">
                        Placas:<asp:Label ID="placasLabel" runat="server" Text='<%# Bind("placas") %>' /><br />
                        Placas:<asp:Label ID="Label1" runat="server" Text='<%# Bind("placas") %>' /><br />
                        Placas:<asp:Label ID="Label2" runat="server" Text='<%# Bind("placas") %>' /><br />
                        Placas:<asp:Label ID="Label3" runat="server" Text='<%# Bind("placas") %>' /><br />
                        Placas:<asp:Label ID="Label4" runat="server" Text='<%# Bind("placas") %>' /><br />
                        Placas:<asp:Label ID="Label5" runat="server" Text='<%# Bind("placas") %>' /><br />
                        Placas:<asp:Label ID="Label6" runat="server" Text='<%# Bind("placas") %>' /><br />
                        Placas:<asp:Label ID="Label7" runat="server" Text='<%# Bind("placas") %>' /><br />
                        Placas:<asp:Label ID="Label8" runat="server" Text='<%# Bind("placas") %>' /><br />
                        Placas:<asp:Label ID="Label9" runat="server" Text='<%# Bind("placas") %>' /><br />
                        Placas:<asp:Label ID="Label11" runat="server" Text='<%# Bind("placas") %>' /><br />
                        Placas:<asp:Label ID="Label10" runat="server" Text='<%# Bind("placas") %>' /><br />
                        Placas:<asp:Label ID="Label12" runat="server" Text='<%# Bind("placas") %>' /><br />
                        Placas:<asp:Label ID="Label13" runat="server" Text='<%# Bind("placas") %>' /><br />
                        Placas:<asp:Label ID="Label14" runat="server" Text='<%# Bind("placas") %>' /><br />
                        Placas:<asp:Label ID="Label15" runat="server" Text='<%# Bind("placas") %>' /><br />

                    </div>
                </div>
            </ItemTemplate>
        </telerik:RadRotator>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>"
            SelectCommand="SELECT [no_orden], [placas], [nombre_propietario], [ap_paterno_propietario], [ap_materno_propietario], [tel_part_propietario], [tel_ofi_propietario], [tel_cel_propietario], [correo_electronico], [no_siniestro], [no_poliza], [id_cliprov_aseguradora], [id_marca], [id_tipo_vehiculo], [id_tipo_unidad], [id_vehiculo], [id_empresa], [id_taller] FROM [Ordenes_Reparacion] WHERE ([status_orden] = @status_orden) ORDER BY [no_orden] DESC">
            <SelectParameters>
                <asp:Parameter DefaultValue="A" Name="status_orden" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
        
    </div>
    </form>
</body>
</html>
<style>
    body
    {
        background: url("img/vectores/back.jpg")#FF9000;
    }
    h1
    {
        color: #AAA173;
        text-align: center;
        font-family: icon;
    }
    
    .form
    {
        width: 800px;
        padding-top: 50px;
        padding-right:50px;
        background-color: rgba(158,181,197,1);
        background-image: url("img/vectores/dato.jpg");
        background-repeat:no-repeat;
        background-position:left;
        border-radius: 10px;
        -moz-border-radius: 10px;
        -webkit-border-radius: 10px;
        margin: 10px auto;
-webkit-box-shadow: 1px 1px 5px 0px rgba(255,255,255,1);
-moz-box-shadow: 1px 1px 5px 0px rgba(255,255,255,1);
box-shadow: 1px 1px 5px 0px rgba(255,255,255,1);
    }
    .form-group
    {
        position: relative;
        margin-bottom: 15px;
    }
</style>
