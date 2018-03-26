<%@ Page Title="" Language="C#" MasterPageFile="~/AdmOrdenes.master" AutoEventWireup="true" CodeFile="RepOrdenes.aspx.cs" Inherits="RepOrdenes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true">
    </asp:ScriptManager>
    <div class="page-header">
        <!-- /BREADCRUMBS -->
        <div class="clearfix">
            <h3 class="content-title pull-left">
                Reporte de Ordenes</h3>
        </div>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-lg-4 col-sm-4 text-left">
                    <asp:Label ID="lblIni" runat="server" Text="Fecha Inicial:" CssClass="textoBold"></asp:Label>
                    <telerik:RadDatePicker RenderMode="Lightweight" ID="txtFechaIni" CssClass="input-medium"
                        runat="server" Skin="MetroTouch" DateInput-DateFormat="yyyy-MM-dd">
                    </telerik:RadDatePicker>
                </div>
                <div class="col-lg-4 col-sm-4 text-left">
                    <asp:Label ID="Label1" runat="server" Text="Fecha Final:" CssClass="textoBold"></asp:Label>
                    <telerik:RadDatePicker RenderMode="Lightweight" ID="txtFechaFin" CssClass="toDate input-medium"
                        runat="server" Skin="MetroTouch" DateInput-DateFormat="yyyy-MM-dd">
                    </telerik:RadDatePicker>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-3 col-sm-3 text-left">
                    <asp:Label ID="Label2" runat="server" Text="Estatus:" CssClass="textoBold"></asp:Label>
                    <asp:CheckBoxList ID="chkEstatus" runat="server" RepeatColumns="1" CellPadding="10" CellSpacing="10" >                        
                        <asp:ListItem Text="Abierta" Value="A"></asp:ListItem>
                        <asp:ListItem Text="Remisionada" Value="R"></asp:ListItem>
                        <asp:ListItem Text="Salida sin Cargo" Value="S"></asp:ListItem>
                        <asp:ListItem Text="Facturada" Value="F"></asp:ListItem>
                        <asp:ListItem Text="Completada" Value="T"></asp:ListItem>
                        <asp:ListItem Text="Cerrada" Value="C"></asp:ListItem>
                    </asp:CheckBoxList>                    
                </div>           
                <div class="col-lg-3 col-sm-3 text-left">
                    <asp:Label ID="Label3" runat="server" Text="Localización:" CssClass="textoBold"></asp:Label>
                    <asp:CheckBoxList ID="chkLocalizacion" runat="server" RepeatColumns="2" CellPadding="10" CellSpacing="10" DataSourceID="SqlDataSource10" DataTextField="descripcion" DataValueField="id_localizacion" ></asp:CheckBoxList>                    
                    <asp:SqlDataSource ID="SqlDataSource10" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select id_localizacion, descripcion from Localizaciones"></asp:SqlDataSource>
                </div>            
                <div class="col-lg-3 col-sm-3 text-left">
                    <asp:Label ID="Label4" runat="server" Text="Perfil:" CssClass="textoBold"></asp:Label>
                    <asp:CheckBoxList ID="chkPerfiles" runat="server" RepeatColumns="2" CellPadding="10" CellSpacing="10" DataSourceID="SqlDataSource13" DataTextField="descripcion" DataValueField="id_perfilOrden" ></asp:CheckBoxList>                    
                    <asp:SqlDataSource ID="SqlDataSource13" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select id_perfilOrden, descripcion from PerfilesOrdenes"></asp:SqlDataSource>
                </div>           
                <div class="col-lg-3 col-sm-3 text-left">
                    <asp:Label ID="Label5" runat="server" Text="Cliente:" CssClass="textoBold"></asp:Label>
                    <telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="ddlClienteNuevo" AllowCustomText="true" CssClass="input-large" Width="300px" MaxHeight="300px" DataSourceID="SqlDataSource6" DataTextField="razon_social" DataValueField="id_cliprov" Skin="MetroTouch" EmptyMessage="Seleccione Cliente" Filter="Contains" ></telerik:RadComboBox>                                        
                    <asp:SqlDataSource ID="SqlDataSource6" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select id_cliprov,razon_social from Cliprov where tipo='C'"></asp:SqlDataSource>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-12 col-sm-12 text-center">
                    <asp:LinkButton ID="lnkBuscar" runat="server" CssClass="btn btn-info" ToolTip="Generar"
                        OnClick="lnkBuscar_Click"><i class="fa fa-cog"></i><span>&nbsp;Generar</span></asp:LinkButton>
                </div>                
            </div>
            <div class="row marTop">
                <div class="col-lg-12 col-sm-12 text-center">
                    <asp:Label ID="lblError" runat="server" CssClass="errores" />
                </div>
            </div>
            
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                <ProgressTemplate>
                    <asp:Panel ID="pnlMaskLoad1" runat="server" CssClass="maskLoad">
                    </asp:Panel>
                    <asp:Panel ID="pnlCargando1" runat="server" CssClass="pnlPopUpLoad">
                        <asp:Image ID="imgLoad1" runat="server" ImageUrl="~/img/loading.gif" CssClass="ancho100" />
                    </asp:Panel>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>

