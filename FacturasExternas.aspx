<%@ Page Title="" Language="C#" MasterPageFile="~/AdmOrdenes.master" AutoEventWireup="true" CodeFile="FacturasExternas.aspx.cs" Inherits="FacturasExternas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <telerik:RadScriptManager ID="RadScriptManajer1" runat="server" EnableScriptGlobalization="true"></telerik:RadScriptManager>
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server"></telerik:RadSkinManager>
        
    <div class="page-header">
        <!-- /BREADCRUMBS -->
        <div class="clearfix">
            <h3 class="content-title pull-left">Facturas Externas</h3>
            
        </div>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row marTop">
                <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label1" runat="server" Text="Factura:"></asp:Label></div>
                <div class="col-lg-4 col-sm-4 text-left">
                    <asp:TextBox ID="txtFactura" runat="server" CssClass="input-large" MaxLength="50" placeholder="Factura"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Debe indicar la factura" Text="*" CssClass="errores" ValidationGroup="agrega" ControlToValidate="txtFactura"></asp:RequiredFieldValidator>
                </div>
                <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label2" runat="server" Text="Monto con IVA:"></asp:Label></div>
                <div class="col-lg-4 col-sm-4 text-left">
                    <telerik:RadNumericTextBox ID="txtMonto" runat="server" MinValue="0" NumberFormat-DecimalDigits="2" ShowSpinButtons="false" Skin="MetroTouch" EmptyMessage="0" ></telerik:RadNumericTextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Debe indicar el monto" Text="*" CssClass="errores" ValidationGroup="agrega" ControlToValidate="txtMonto"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="row marTop">
                <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label3" runat="server" Text="Taller:"></asp:Label></div>
                <div class="col-lg-4 col-sm-4 text-left">
                <asp:DropDownList ID="ddlTaller" runat="server" CssClass="input-large">
                    <asp:ListItem Selected="True" Value="0" Text="Selecciona un taller"></asp:ListItem>
                    <asp:ListItem Value="1" Text="Autos"></asp:ListItem>
                    <asp:ListItem Value="2" Text="Tractos"></asp:ListItem>
                    <asp:ListItem Value="3" Text="Pesado"></asp:ListItem>
                </asp:DropDownList>
               <%--
                    <asp:DropDownList ID="ddlTaller" runat="server" DataSourceID="SqlDataSource1" DataTextField="nombre_taller" DataValueField="id_taller" CssClass="input-large"></asp:DropDownList>
                    <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:PVW %>' SelectCommand="SELECT e.id_taller, t.nombre_taller FROM Empresas_Taller e INNER JOIN Talleres  t ON t.id_taller = e.id_taller WHERE (e.id_empresa = @empresa)">
                        <SelectParameters>
                            <asp:QueryStringParameter QueryStringField="e" DefaultValue="0" Name="empresa"></asp:QueryStringParameter>
                        </SelectParameters>
                    </asp:SqlDataSource>
               --%>
                  <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Debe indicar el taller" Text="*" CssClass="errores" ValidationGroup="agrega" ControlToValidate="ddlTaller"></asp:RequiredFieldValidator>
                </div>
                <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label4" runat="server" Text="Orden:"></asp:Label></div>
                <div class="col-lg-4 col-sm-4 text-left">
                    <telerik:RadNumericTextBox ID="txtOrden" runat="server" MinValue="0" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="," NumberFormat-GroupSizes="6" ShowSpinButtons="false" Skin="MetroTouch" EmptyMessage="0" ></telerik:RadNumericTextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Debe indicar la orden" Text="*" CssClass="errores" ValidationGroup="agrega" ControlToValidate="txtOrden"></asp:RequiredFieldValidator>
                </div>
            </div>
             <div class="row marTop">
                 <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label6" runat="server" Text="Tipo:"></asp:Label></div>
                <div class="col-lg-4 col-sm-4 text-left">
                    <telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="radTipo" AllowCustomText="true" CssClass="input-large" Width="300px" MaxHeight="300px" Skin="MetroTouch" EmptyMessage="Seleccione Tipo" AutoPostBack="true"  OnSelectedIndexChanged="radTipo_SelectedIndexChanged" >
                        <Items>
                            <telerik:RadComboBoxItem Value="PA" Text="Por Pagar" />
                            <telerik:RadComboBoxItem Value="CC" Text="Por Cobrar" />
                        </Items>
                    </telerik:RadComboBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Debe seleccionar el tipo" Text="*" CssClass="errores" ValidationGroup="agrega" ControlToValidate="radTipo"></asp:RequiredFieldValidator>
                    <asp:Label ID="lblTipoProveedor" runat="server" Visible="false"></asp:Label>
                </div>
                <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label5" runat="server" Text="Cliente:"></asp:Label></div>
                <div class="col-lg-4 col-sm-4 text-left">
                    <telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="ddlProveedor" AllowCustomText="true" CssClass="input-large" Width="300px" MaxHeight="300px" DataSourceID="SqlDataSource2" DataTextField="razon_social" DataValueField="id_cliprov" Skin="MetroTouch" EmptyMessage="Seleccione Cliente" Filter="Contains" ></telerik:RadComboBox>                                        
                    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select id_cliprov,razon_social from Cliprov where tipo=@tipo and estatus='A'">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="lblTipoProveedor" DefaultValue="P" Name="tipo" PropertyName="Text" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Debe seleccionar el proveedor" Text="*" CssClass="errores" ValidationGroup="agrega" ControlToValidate="ddlProveedor"></asp:RequiredFieldValidator>
                </div>
                 
                </div>
             <div class="row marTop">                
                <div class="col-lg-12 col-sm-12 text-center">
                    <asp:LinkButton ID="lnkAgregarFactura" runat="server" CssClass="btn btn-primary" OnClientClick="return confirm('¿Está seguro de que los datos son correctos?')" OnClick="lnkAgregarFactura_Click" ValidationGroup="agrega"><i class="fa fa-plus"></i><span>&nbsp;Agregar</span></asp:LinkButton>
                </div>
            </div>
            <div class="row marTop">
                <div class="col-lg-12 col-sm-12 text-center">
                    <asp:Label ID="lblError" runat="server" CssClass="alert-danger"></asp:Label>
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="agrega" CssClass="errores" DisplayMode="List" />
                </div>
            </div>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                <ProgressTemplate>
                    <asp:Panel ID="pnlMaskLoad1" runat="server" CssClass="maskLoad"></asp:Panel>
                    <asp:Panel ID="pnlCargando1" runat="server" CssClass="pnlPopUpLoad">
                        <asp:Image ID="imgLoad1" runat="server" ImageUrl="~/img/loading.gif" CssClass="ancho100" />
                    </asp:Panel>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

