<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Cotiza.aspx.cs" Inherits="Cotiza" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cotizacion de Refacciones</title>
    <meta charset="UTF-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link href="css/cloud-admin.css" rel="stylesheet" type="text/css" />
    <link href="css/menus.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="css/responsive.css" />
    <link href="css/4.4.0/css/font-awesome.min.css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="css/generales.css" />
    <link href="css/dashboard.css" rel="stylesheet" type="text/css" />
    <!-- FONTS -->
    <link href='http://fonts.googleapis.com/css?family=Open+Sans:300,400,600,700' rel='stylesheet' type='text/css' />
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
    <script src="js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="js/bootstrap.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div class="encabezado">
            <table class="ancho100 colorBlanco alineado alto100">
                <tr>
                    <td class="marginLeft textoIzquierda">
                        <asp:Label ID="lbltitulo" runat="server" Text="MoncarWeb" CssClass="margin-left-50 titulo negritas colorMoncarAzul alineado margenLeft" Visible="false" />&nbsp;
                        <asp:Label ID="lblEmpresa" runat="server" CssClass="colorMoncarAzul alineado" Visible="false"></asp:Label>
                        <asp:Image ID="imgEmpresa" runat="server" CssClass="img-responsive imagenLogo" ImageUrl="~/img/moncar.png" />
                    </td>
                </tr>
            </table>
        </div>


        <section id="page">

            <div class="container">
                <div class="row">
                    <div id="content" class="col-lg-12">
                        <!-- PAGE HEADER-->
                        <div class="row">
                            <div class="col-sm-12">
                                <div class="page-header">
                                    <div class="clearfix">
                                        <h3 class="content-title pull-left">
                                            <asp:Label ID="lblOrdenSelect" runat="server"></asp:Label><br />
                                            <asp:Label ID="lblPropveedor" runat="server"></asp:Label>
                                        </h3>
                                    </div>

                                </div>

                                <div class="contenidos">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <div class="col-lg-12 col-sm-12 text-center">
                                                <asp:Label ID="lblError" runat="server" CssClass="errores"></asp:Label>
                                                <asp:Label ID="lblMensaje" runat="server" CssClass="text-success t18 textoBold" Visible="false"></asp:Label>
                                            </div>
                                            <asp:Panel ID="Panel2" runat="server" CssClass="col-lg-12 col-sm-12" Visible="false">

                                                <asp:Panel ID="PanelImg" runat="server" CssClass="col-lg-12 col-sm-12" ScrollBars="Auto">
                                                    <asp:DataList ID="DataListFotosDanos" runat="server" RepeatColumns="10" RepeatDirection="Horizontal"
                                                        DataKeyField="id_empresa" DataSourceID="SqlDataSourceFotosDanos" OnItemCommand="DataListFotosDanos_ItemCommand">
                                                        <ItemTemplate>
                                                            <asp:Label ID="id_empresaLabel" runat="server" Text='<%# Eval("id_empresa") %>' Visible="false" />
                                                            <asp:Label ID="id_tallerLabel" runat="server" Text='<%# Eval("id_taller") %>' Visible="false" />
                                                            <asp:Label ID="no_ordenLabel" runat="server" Text='<%# Eval("no_orden") %>' Visible="false" />
                                                            <asp:Label ID="consecutivoLabel" runat="server" Text='<%# Eval("consecutivo") %>' Visible="false" />
                                                            <asp:Label ID="procesoLabel" runat="server" Text='<%# Eval("proceso") %>' Visible="false" />
                                                            <asp:Label ID="nombre_imagenLabel" runat="server" Text='<%# Eval("nombre_imagen") %>' Visible="false" />
                                                            <asp:Label ID="rutaLabel" runat="server" Text='<%# Eval("ruta") %>' Visible="false" />
                                                            <br />
                                                            <asp:LinkButton ID="btnLogo" runat="server" ToolTip='<%# Eval("nombre_imagen") %>' CommandName="zoom" CommandArgument='<%# Eval("id_empresa")+";"+Eval("id_taller")+";"+Eval("no_orden")+";"+Eval("consecutivo")+";"+Eval("proceso") %>'>
                                                                <asp:Image ID="Image1" runat="server" AlternateText='<%# Eval("nombre_imagen") %>' Width="120px" ImageUrl='<%# "~/ImgEmpresas.ashx?id="+Eval("id_empresa")+";"+Eval("id_taller")+";"+Eval("no_orden")+";"+Eval("consecutivo")+";"+Eval("proceso") %>' />
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="ancho180px textoCentrado" />
                                                    </asp:DataList>
                                                    <asp:SqlDataSource runat="server" ID="SqlDataSourceFotosDanos" ConnectionString='<%$ ConnectionStrings:PVW %>'
                                                        SelectCommand="select top 10 id_empresa,id_taller,no_orden,consecutivo,proceso,nombre_imagen,ruta from Fotografias_Orden where id_empresa=@id_empresa and id_taller=@id_taller and no_orden=@no_orden and proceso=1">
                                                        <SelectParameters>
                                                            <asp:QueryStringParameter Name="no_orden" QueryStringField="o" Type="Int32" />
                                                            <asp:QueryStringParameter Name="id_empresa" QueryStringField="e" Type="Int32" />
                                                            <asp:QueryStringParameter Name="id_taller" QueryStringField="t" Type="Int32" />
                                                        </SelectParameters>
                                                    </asp:SqlDataSource>
                                                    <asp:Panel ID="PanelMascara" runat="server" CssClass="mask zen2"></asp:Panel>
                                                    <asp:Panel ID="PanelImgZoom" runat="server" CssClass="popUp zen3 textoCentrado ancho80 centrado" ScrollBars="Auto">
                                                        <table class="ancho100">
                                                            <tr class="ancho100 centrado">
                                                                <td class="ancho95 text-center encabezadoTabla roundTopLeft ">
                                                                    <asp:Label ID="Label107" runat="server" Text="Fotografía" CssClass="t22 colorMorado textoBold" />
                                                                </td>
                                                                <td class="ancho5 text-right encabezadoTabla roundTopRight">
                                                                    <asp:LinkButton ID="btnCerrarImgZomm" runat="server" ToolTip="Cerrar" OnClick="btnCerrarImgZomm_Click" CssClass="btn btn-danger alingMiddle"><i class="fa fa-remove t18"></i></asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <div class="row">
                                                            <asp:Panel ID="Panel8" runat="server" CssClass="col-lg-12 col-sm-12 text-center ancho100 pnlImagen" ScrollBars="Auto">
                                                                <asp:Image ID="imgZoom" runat="server" CssClass="ancho100" AlternateText="Imagen no disponible" />
                                                            </asp:Panel>
                                                        </div>
                                                    </asp:Panel>
                                                </asp:Panel>
                                                
                                                    <div class="col-lg-8 col-sm-8">
                                                    
                                                    </div>
                                                    <div class="col-lg-2 col-sm-2 text-right">
                                                        <asp:Label ID="Label321" runat="server" Text="Fecha de Vigencia"></asp:Label>    
                                                    </div>
                                                    <div class="col-lg-2 col-sm-2">
                                                        <telerik:RadDatePicker ID="txtFechaFirma" runat="server" Width="70%" >
                                                            <DateInput ID="txtFechaLimite" runat="server" DateFormat="yyyy/MM/dd">
                                                            </DateInput>
                                                        </telerik:RadDatePicker>
                                                    </div>
                                              <br /><br />
                                              <br /><br />
                                                <table class="table table-bordered marTop" cellspacing="0" rules="all">
                                                    <tr class="alert-info">
                                                        <td class="col-lg-1 col-sm-1">
                                                            <asp:Label ID="Label1" runat="server" Text="Cantidad"></asp:Label></td>
                                                        <td class="col-lg-2 col-sm-3">
                                                            <asp:Label ID="Label2" runat="server" Text="Refacción"></asp:Label></td>
                                                        <td class="col-lg-2 col-sm-3">
                                                            <asp:Label ID="Label7" runat="server" Text="Procedenica"></asp:Label></td>
                                                        <td class="col-lg-2 col-sm-1">
                                                            <asp:Label ID="Label3" runat="server" Text="Costo Unitario"></asp:Label></td>
                                                        <td class="col-lg-2 col-sm-1">
                                                            <asp:Label ID="Label4" runat="server" Text="% Descuento"></asp:Label></td>
                                                        <td class="col-lg-2 col-sm-1">
                                                            <asp:Label ID="Label5" runat="server" Text="Existencia"></asp:Label></td>
                                                        <td class="col-lg-2 col-sm-1">
                                                            <asp:Label ID="Label6" runat="server" Text="Días de Entrega"></asp:Label></td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            <asp:Panel ID="Panel1" runat="server" CssClass="paneles col-lg-12 col-sm-12" ScrollBars="Auto" Visible="false">
                                                <asp:ListView ID="ListView1" runat="server">
                                                    <ItemTemplate>
                                                        <table class="table table-bordered" cellspacing="0" rules="all" style="min-width: 600px;">
                                                            <tr style="min-width: 600px;">
                                                                <td class="text-center col-lg-1 col-sm-1" style="min-width: 100px;">
                                                                    <asp:Label ID="lblIdRefaccion" runat="server" Text='<%# Eval("_refaccion") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblCantidad" runat="server" Text='<%# Eval("_cantidad") %>'></asp:Label>
                                                                </td>
                                                                <td class="text-left col-lg-2 col-sm-3" style="min-width: 100px;">
                                                                    <asp:Label ID="lblRefaccion" runat="server" Text='<%# Eval("_descripcion") %>'></asp:Label></td>
                                                                <td class="text-left col-lg-2 col-sm-3" style="min-width: 100px;">
                                                                    <asp:DropDownList ID="ddlProcedencia" runat="server" DataSourceID="SqlDsProced" DataTextField="proc_Descrip" DataValueField="id_Proc" AppendDataBoundItems="true">
                                                                        <asp:ListItem Value="-1">--Procedencia--</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <asp:SqlDataSource runat="server" ID="SqlDsProced" ConnectionString='<%$ ConnectionStrings:PVW %>' SelectCommand="SELECT [id_Proc], [proc_Descrip] FROM [cat_Procedencia]"></asp:SqlDataSource>
                                                                </td>
                                                                <td class="text-left col-lg-2 col-sm-1" style="min-width: 100px;">
                                                                    <asp:Label ID="lblCostoIni" runat="server" Text='<%# Eval("_costo") %>' Visible="false"></asp:Label>
                                                                    <asp:TextBox ID="txtContoUnitario" runat="server" Text='<%# Eval("_costo") %>' CssClass="input-small" MaxLength="16"></asp:TextBox>
                                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtContoUnitario" runat="server" ValidationExpression="^\d{1,13}(\.\d{1,2})?$" ErrorMessage="El costo solo puede contener dígitos y un punto decimal" ValidationGroup="cotiza" Text="*" CssClass="errores"></asp:RegularExpressionValidator>
                                                                </td>
                                                                <td class="text-left col-lg-2 col-sm-1" style="min-width: 100px;">
                                                                    <asp:Label ID="lblPorcIni" runat="server" Text='<%# Eval("_refaccion") %>' Visible="false"></asp:Label>
                                                                    <asp:TextBox ID="txtPorcDesc" runat="server" Text='<%# Eval("_porcentajeDescuento") %>' CssClass="input-mini" MaxLength="6"></asp:TextBox>
                                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="txtPorcDesc" runat="server" ValidationExpression="^100$|^100.00$|^\d{0,2}(\.\d{1,2})? *%?$" ErrorMessage="El porcentaje solo puede contener dígitos y un punto decimal" ValidationGroup="cotiza" Text="*" CssClass="errores"></asp:RegularExpressionValidator>
                                                                </td>
                                                                <td class="text-left col-lg-2 col-sm-1" style="min-width: 100px;">
                                                                    <asp:CheckBox ID="chkExistencia" runat="server" Checked='<%# Eval("_existencia") %>'></asp:CheckBox>
                                                                </td>
                                                                <td class="text-left col-lg-2 col-sm-1" style="min-width: 100px;">
                                                                    <asp:TextBox ID="txtDias" runat="server" Text='<%# Eval("_dias") %>' CssClass="input-mini" MaxLength="3"></asp:TextBox>
                                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" ControlToValidate="txtDias" runat="server" ValidationExpression="^\d{3}|\d{2}|\d{1}|\d$" ErrorMessage="Debe indicar solo dígitos" ValidationGroup="cotiza" Text="*" CssClass="errores"></asp:RegularExpressionValidator>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:ListView>
                                            </asp:Panel>
                                            <div class="col-lg-12 col-sm-12 text-center">
                                                <asp:LinkButton ID="lnkGuardar" runat="server" CssClass="btn btn-primary" OnClick="lnkGuardar_Click" ValidationGroup="cotiza" Visible="false" OnClientClick="return confirm('¿Esta seguro de que los datos ingresados son correctos?. Una vez confirmado no podra realizar ningun cambio')"><i class="fa fa-save"></i><span>&nbsp;Registrar</span></asp:LinkButton>
                                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="cotiza" CssClass="errores" DisplayMode="List"></asp:ValidationSummary>
                                            </div>
                                            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                                                <ProgressTemplate>
                                                    <asp:Panel ID="pnlMaskLoad" runat="server" CssClass="maskLoad"></asp:Panel>
                                                    <asp:Panel ID="pnlCargando" runat="server" CssClass="pnlPopUpLoad">
                                                        <asp:Image ID="imgLoad" runat="server" ImageUrl="~/img/loading.gif" CssClass="ancho100" />
                                                    </asp:Panel>
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- /PAGE HEADER -->
                </div>
            </div>
        </section>
    </form>
</body>
</html>
