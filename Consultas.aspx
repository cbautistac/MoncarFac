<%@ Page Title="" Language="C#" MasterPageFile="~/AdmOrdenes.master" AutoEventWireup="true" CodeFile="Consultas.aspx.cs" Inherits="Consultas" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
        <ContentTemplate>
            <div class="page-header">
                <!-- /BREADCRUMBS -->
                <div class="clearfix">
                    <h3 class="content-title pull-left"><asp:Literal runat="server" ID="litTit" Text="" /></h3>
                    <asp:Label ID="lblError" runat="server" CssClass="alert-danger"></asp:Label>
                </div>
            </div>

            <asp:Panel ID="Panel4" runat="server" CssClass="panelCatalogos text-center" ScrollBars="Auto">
                <div class="ancho100 textoDerecha"> 
                    <div style="fl">
                        <asp:TextBox ID="txtFiltro" runat="server" CssClass="input-medium alingMiddle"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtFiltro_TextBoxWatermarkExtender" runat="server" BehaviorID="txtFiltro_TextBoxWatermarkExtender" TargetControlID="txtFiltro" WatermarkCssClass="water input-medium" WatermarkText="Buscar..." />
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="lnkBuscar" runat="server" ToolTip="Buscar" CssClass="btn btn-info alingMiddle"><i class="fa fa-search t18"></i></asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkLimpiar" runat="server" ToolTip="Limpiar Búsqueda" CssClass="btn btn-info alingMiddle" OnClick="lnkLimpiar_Click"><i class="fa fa-eraser t18"></i>&nbsp;<span>Limpiar B&uacute;squeda</span></asp:LinkButton>
                        <asp:LinkButton ID="lnkRegresarOrdenes" runat="server" OnClick="lnkRegresarOrdenes_Click" CssClass="btn btn-info t14"><i class="fa fa-reply">&nbsp;&nbsp;</i><i class="fa fa-th-list"></i>&nbsp;<span>Órdenes</span></asp:LinkButton>
                    </div>
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
                            <asp:BoundField DataField="entEstimada" HeaderText="Fecha Promesa" SortExpression="entEstimada" />
                            <asp:BoundField DataField="f_pactada" HeaderText="Fecha Pactada" SortExpression="f_pactada" />
                            <asp:TemplateField HeaderText="Refacciones">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkSeleccionar" runat="server" CausesValidation="False" 
                                        CommandName="Select" ToolTip="Ver Refacciones" CssClass="btn btn-info" 
                                        onclick="lnkSeleccionar_Click">
                                        <i class="fa fa-check-circle"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EditRowStyle CssClass="alert-warning" />
                        <EmptyDataRowStyle CssClass="alert-danger" />
                        <AlternatingRowStyle CssClass="alterTable" />
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>"></asp:SqlDataSource>
                </div>
            </asp:Panel>

            <asp:Panel ID="PanelMascara" runat="server" CssClass="mask zen2" Visible="true" ></asp:Panel>                            
            <asp:Panel ID="pnlRefOrd" runat="server" CssClass="popUp zen3 textoCentrado ancho80 centrado" ScrollBars="Auto" Visible="false">
                <table class="ancho100">
                    <tr class="ancho100 centrado">
                        <td class="ancho95 text-center encabezadoTabla roundTopLeft ">
                            <span class="t22 colorMorado textoBold">Refacciones de la Orden&nbsp;</span>
                            <asp:Label ID="lblIdOrden" runat="server" CssClass="t22 colorMorado textoBold" />  
                        </td>
                        <td class="ancho5 text-right encabezadoTabla roundTopRight">
                            <asp:LinkButton ID="btnCerrarComp" runat="server" ToolTip="Cerrar" OnClick="btnCerrarComp_Click" CssClass="btn btn-danger alingMiddle" >
                                <i class="fa fa-remove t18"></i></asp:LinkButton>                            
                        </td>
                    </tr>                        
                </table>
                <asp:Panel ID="prov" runat="server" CssClass="panelesInv centrado" ScrollBars="Auto">
                <div class="table-responsive">
                    <asp:GridView ID="grdRefOrd" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered" 
                        AllowPaging="True" AllowSorting="True" DataSourceID="SqlDsRefOrd" ShowHeaderWhenEmpty="true" UseAccessibleHeader="true" 
                        EmptyDataText="No se encontraron refacciones para esta orden." >
                        <Columns>
                            <asp:BoundField DataField="refDescripcion" HeaderText="Refacción" SortExpression="refDescripcion" />
                            <asp:BoundField DataField="refCantidad" HeaderText="Cantidad" SortExpression="refCantidad" />
                            <asp:BoundField DataField="staDescripcion" HeaderText="Estatus" SortExpression="staDescripcion" />                                
                            <asp:BoundField DataField="razon_social" HeaderText="Proveedor" SortExpression="razon_social" />                                
                        </Columns>
                    </asp:GridView>
                                            
                    <asp:SqlDataSource ID="SqlDsRefOrd" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" 
                        SelectCommand="SELECT ro.refDescripcion, ro.refCantidad, re.staDescripcion, p.razon_social
                        FROM Refacciones_Orden AS ro INNER JOIN Cliprov AS p ON ro.refProveedor = p.id_cliprov INNER JOIN
                        Rafacciones_Estatus AS re ON ro.refEstatusSolicitud = re.staRefID
                        WHERE (ro.ref_no_orden = @ref_no_orden AND p.tipo = 'P')">
                        <SelectParameters>
                            <asp:ControlParameter Name="ref_no_orden" ControlID="lblIdOrden" Type="Int32" PropertyName="Text" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                                            
                    </div>
                    </asp:Panel>
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

