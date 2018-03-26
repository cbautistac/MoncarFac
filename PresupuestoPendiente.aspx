<%@ Page Title="" Language="C#" MasterPageFile="~/AdmOrdenes.master" AutoEventWireup="true" CodeFile="PresupuestoPendiente.aspx.cs" Inherits="PresupuestoPendiente" UICulture="es-Mx" Culture="es-Mx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true">
    </asp:ScriptManager>
    <asp:Timer ID="Timer1" OnTick="Timer1_Tick" runat="server" Interval="300000"></asp:Timer>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>

    <div class="page-header">
        <!-- /BREADCRUMBS -->
        <div class="clearfix">
            <h3 class="content-title pull-left">Presupuestos Pendientes</h3>
        </div>
    </div>   

    <asp:Panel ID="Panel4" runat="server" CssClass="panelCatalogos text-center" ScrollBars="Auto">
                <div class="ancho100 textoDerecha">
                    <asp:Label ID="Label13" runat="server" Text="Estatus" CssClass="textoBold"></asp:Label>
                    <asp:DropDownList ID="ddlEstatus" runat="server" AppendDataBoundItems="True" 
                        AutoPostBack="True" onselectedindexchanged="ddlEstatus_SelectedIndexChanged">
                        <asp:ListItem Selected="True" Value="A">Abierta</asp:ListItem>
                        <asp:ListItem Value="T">Completada</asp:ListItem>
                        <asp:ListItem Value="F">Facturada</asp:ListItem>
                        <asp:ListItem Value="C">Cerrada</asp:ListItem>
                    </asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;                   
                    <asp:TextBox ID="txtFiltro" runat="server" CssClass="input-medium alingMiddle"></asp:TextBox>
                    <cc1:TextBoxWatermarkExtender ID="txtFiltro_TextBoxWatermarkExtender" runat="server" BehaviorID="txtFiltro_TextBoxWatermarkExtender" TargetControlID="txtFiltro" WatermarkCssClass="water input-medium" WatermarkText="Buscar..." />
                    &nbsp;&nbsp;
                    <asp:LinkButton ID="lnkBuscar" runat="server" ToolTip="Buscar" CssClass="btn btn-info alingMiddle"><i class="fa fa-search t18"></i></asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:LinkButton ID="lnkLimpiar" runat="server" ToolTip="Limpiar Búsqueda" CssClass="btn btn-info alingMiddle" OnClick="lnkLimpiar_Click"><i class="fa fa-eraser t18"></i>&nbsp;<span>Limpiar B&uacute;squeda</span></asp:LinkButton>
                </div>
                <br />
                <div class="table-responsive">
                    <asp:GridView ID="GridView1" runat="server" CssClass="table table-bordered" GridLines="None"
                        AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="no_orden"
                        DataSourceID="SqlDataSource1" EmptyDataText="No existen órdenes registradas"
                        OnRowDataBound="GridView1_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Orden" SortExpression="no_orden">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnOrden" runat="server" Text='<%# Bind("no_orden") %>' CommandArgument='<%# Bind("fase_orden") %>' OnClick="btnOrden_Click" CssClass="btn btn-info textoBold"></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle CssClass="alto40px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Ingreso" SortExpression="f_recepcion">
                                <ItemTemplate>
                                    <asp:Label ID="lblFechaRecp" runat="server" Text='<%# Bind("f_recepcion", "{0:yyyy-MM-dd}") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="descripcion" HeaderText="Vehículo" SortExpression="descripcion" />
                            <asp:BoundField DataField="modelo" HeaderText="Modelo" SortExpression="modelo" />
                            <asp:BoundField DataField="color_ext" HeaderText="Color" SortExpression="color_ext" />
                            <asp:BoundField DataField="placas" HeaderText="Placas" SortExpression="placas" />
                            <asp:BoundField DataField="perfil" HeaderText="Perfil" SortExpression="perfil" />
                            <asp:BoundField DataField="localizacion" HeaderText="Localización" SortExpression="localizacion" />
                            <asp:BoundField DataField="razon_social" HeaderText="Cliente" SortExpression="razon_social" />
                            <asp:BoundField DataField="no_siniestro" HeaderText="Siniestro" SortExpression="no_siniestro" />
                            <asp:TemplateField HeaderText="Fase" SortExpression="fase_orden" Visible="false">
                                <ItemTemplate>
                                    <asp:Image ID="imgFase" runat="server" CssClass="alto40px" />
                                </ItemTemplate>
                                <ItemStyle CssClass="alto40px" />
                            </asp:TemplateField>
                        </Columns>
                        <EditRowStyle CssClass="alert-warning" />
                        <EmptyDataRowStyle CssClass="alert-danger" />
                        <AlternatingRowStyle CssClass="alterTable" />
                        <SelectedRowStyle CssClass="alert-success" />
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>"></asp:SqlDataSource>
                </div>
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
        </asp:UpdatePanel>
</asp:Content>

