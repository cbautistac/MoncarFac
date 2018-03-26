<%@ Page Title="" Language="C#" MasterPageFile="~/AdmOrdenes.master" AutoEventWireup="true" CodeFile="RefaccionesPendientes.aspx.cs" Inherits="RefaccionesPendientes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true">
    </asp:ScriptManager>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>

    <div class="page-header">
        <!-- /BREADCRUMBS -->
        <div class="clearfix">
            <h3 class="content-title pull-left">Refacciones Pendientes</h3>
        </div>
    </div>
    <div class="row">
            <div class="col-lg-12 col-sm-12 text-center">
                <asp:LinkButton ID="lnkRegresar" runat="server" OnClick="lnkRegresar_Click" CssClass="btn btn-info t14"><i class="fa fa-reply">&nbsp;&nbsp;</i><i class="fa fa-th-list"></i>&nbsp;<span>Órdenes</span></asp:LinkButton>
            </div>
        </div>

    <asp:Panel ID="Panel1" runat="server" CssClass="panelCatalogos text-center" ScrollBars="Auto">
        <div class="table-responsive">
            <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" runat="server" ShowStatusBar="true"
                AutoGenerateColumns="False" AllowSorting="True" AllowMultiRowSelection="False" 
                OnDetailTableDataBind="RadGrid1_DetailTableDataBind" OnNeedDataSource="RadGrid1_NeedDataSource"
                OnPreRender="RadGrid1_PreRender" Skin="MetroTouch" >
                <PagerStyle Mode="NumericPages"></PagerStyle>
                <MasterTableView DataKeyNames="ref_no_orden" AllowMultiColumnSorting="True">
                    <DetailTables>
                        <telerik:GridTableView DataKeyNames="ref_no_orden" Name="ingreso" Width="100%">
                            <Columns>
                                <telerik:GridBoundColumn SortExpression="razon_social" HeaderText="Proveedor" HeaderButtonType="TextButton" DataField="razon_social"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn SortExpression="refdescripcion" HeaderText="Refacción" HeaderButtonType="TextButton" DataField="refdescripcion"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn SortExpression="reffechsolicitud" HeaderText="Fecha Solicitud" HeaderButtonType="TextButton" DataFormatString="{0:yyyy-MM-dd}" DataField="reffechsolicitud"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="reffechentregaest" HeaderText="Fecha Estimada" SortExpression="reffechentregaest" DataFormatString="{0:yyyy-MM-dd}" HeaderButtonType="TextButton" />
                            </Columns>
                        </telerik:GridTableView>
                    </DetailTables>
                    <Columns>
                        <telerik:GridTemplateColumn SortExpression="ref_no_orden" HeaderText="No. Orden">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnOrden" runat="server" Text='<%# Eval("ref_no_orden") %>' CommandArgument='<%# Eval("fase_orden") %>' OnClick="btnOrden_Click" ForeColor="White" CssClass="btn btn-info textoBold"></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vehículo" SortExpression="vehiculo">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblVehiculo" Text='<%# Eval("vehiculo") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Cliente" SortExpression="razon_social">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblRazon" Text='<%# Eval("razon_social") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Perfil" SortExpression="perfil">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblPerfil" Text='<%# Eval("perfil") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Refacciones Pendientes" SortExpression="refacciones">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblRefacciones" Text='<%# Eval("refacciones") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <NoRecordsTemplate>
                        <asp:Label ID="lnlemptygrid" runat="server" CssClass="errores" Text="No existen refacciones pendientes de entrega"></asp:Label>
                    </NoRecordsTemplate>
                </MasterTableView>
            </telerik:RadGrid>
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

