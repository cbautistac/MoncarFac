<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ComparativoCostos.aspx.cs" Inherits="ComparativoCostos" EnableEventValidation="false" MasterPageFile="~/Cuentas.master" Culture="es-Mx" UICulture="es-Mx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

        <script type="text/javascript">
        function abreWinCtrl() {
            var oWnd = $find("<%=modalPopupControl.ClientID%>");
            oWnd.setUrl('');
            oWnd.show();
        }        
        function cierraWinCtrl(ventana) {
            var oWnd = $find("<%=modalPopupControl.ClientID%>");
            oWnd.close();
        }
        

    </script>

    <telerik:RadScriptManager ID="RadScriptManajer1" runat="server" EnableScriptGlobalization="true"></telerik:RadScriptManager>
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server"></telerik:RadSkinManager>
    <div class="row">
        <div class="col-lg-12 col-sm-12 text-center alert-info">
            <h3>
                <i class="fa fa-cubes"></i>&nbsp;&nbsp;&nbsp;<span>Comparativo de Costos</span>
            </h3>
        </div>
    </div>
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
            <div class="page-header">
                <!-- /BREADCRUMBS -->
                <div class="clearfix">
                    <h3 class="content-title pull-left">Ordenes</h3>
                    <asp:Label ID="lblError" runat="server" CssClass="alert-danger"></asp:Label>
                </div>
            </div>
            <asp:Panel ID="Panel4" runat="server" CssClass="panelCatalogos text-center" ScrollBars="Auto">
                <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" AllowFilteringByColumn="true" runat="server"  EnableHeaderContextMenu="true" Culture="es-Mx" Skin="Metro" OnItemDataBound="RadGrid1_ItemDataBound" 
                        EnableHeaderContextFilterMenu="true" AllowPaging="True" PagerStyle-AlwaysVisible="true" DataSourceID="SqlDataSource1" AllowSorting="true" PageSize="1000" ShowGroupPanel="True">
                        <MasterTableView DataSourceID="SqlDataSource1" AutoGenerateColumns="False" DataKeyNames="no_orden,id_taller,id_empresa" ShowGroupFooter="true">
                            <GroupByExpressions>
                                <telerik:GridGroupByExpression>
                                    <SelectFields>
                                        <telerik:GridGroupByField FieldAlias="Empresa" FieldName="empresa" ></telerik:GridGroupByField>
                                    </SelectFields>
                                    <GroupByFields>
                                        <telerik:GridGroupByField FieldName="empresa" SortOrder="Descending"></telerik:GridGroupByField>
                                    </GroupByFields>
                                </telerik:GridGroupByExpression>
                                <telerik:GridGroupByExpression>
                                    <SelectFields>
                                        <telerik:GridGroupByField FieldAlias="Taller" FieldName="taller"></telerik:GridGroupByField>
                                    </SelectFields>
                                    <GroupByFields>
                                        <telerik:GridGroupByField FieldName="taller" SortOrder="Descending"></telerik:GridGroupByField>
                                    </GroupByFields>
                                </telerik:GridGroupByExpression>                                                               
                            </GroupByExpressions>
                            <Columns>
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="empresa" FilterControlAltText="Filtro Empresa" HeaderText="Empresa" SortExpression="empresa" UniqueName="empresa" Visible="false"/>
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="taller" FilterControlAltText="Filtro Taller" HeaderText="Taller" SortExpression="taller" UniqueName="taller" Visible="false"/>
                                <telerik:GridTemplateColumn FooterText="Total Ordenes: " Aggregate="Count" FilterCheckListEnableLoadOnDemand="true" HeaderText="Orden" SortExpression="no_orden" UniqueName="no_orden" FilterControlAltText="Filtro Orden" DataField="no_orden">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnOrden" runat="server" Text='<%# Eval("no_orden") %>' OnClick="btnOrden_Click" CommandArgument='<%# Eval("no_orden")+";"+Eval("id_taller")+";"+Eval("id_empresa") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>                                
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="f_recepcion" FilterControlAltText="Filtro Ingreso" HeaderText="Ingreso" SortExpression="f_recepcion" UniqueName="f_recepcion" DataFormatString="{0:yyyy-MM-dd}" Resizable="true"/>
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="descripcion" FilterControlAltText="Filtro Vehículo" HeaderText="Vehículo" SortExpression="descripcion" UniqueName="descripcion" Resizable="true"/>
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="modelo" FilterControlAltText="Filtro Modelo" HeaderText="Modelo" SortExpression="modelo" UniqueName="modelo" Resizable="true"/>
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="color_ext" FilterControlAltText="Filtro Color" HeaderText="Color" SortExpression="color_ext" UniqueName="color_ext" Resizable="true"/>
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="placas" FilterControlAltText="Filtro Placas" HeaderText="Placas" SortExpression="placas" UniqueName="placas" Resizable="true"/>
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="perfil" FilterControlAltText="Filtro Perfil" HeaderText="Perfil" SortExpression="perfil" UniqueName="perfil" Resizable="true"/>
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="localizacion" FilterControlAltText="Filtro Localización" HeaderText="Localización" SortExpression="localizacion" UniqueName="localizacion" Resizable="true"/>
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="razon_social" FilterControlAltText="Filtro Cliente" HeaderText="Cliente" SortExpression="razon_social" UniqueName="razon_social" Resizable="true"/>
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="no_siniestro" FilterControlAltText="Filtro Siniestro" HeaderText="Siniestro" SortExpression="no_siniestro" UniqueName="no_siniestro" Resizable="true"/>
                            </Columns>
                        </MasterTableView>
                        <ClientSettings>
                            <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="1"></Scrolling>
                        </ClientSettings>                        
                        <PagerStyle PageSizeControlType="RadDropDownList" Mode="NextPrevAndNumeric"></PagerStyle>
                    </telerik:RadGrid>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>"/>
                <div class="ancho100 text-center marTop">
                    <div class="col-lg-12 col-sm-12 alert-info">
                        <h4>Estatus</h4>
                    </div>
                </div>
                <div class="ancho100 text-center marTop">                                       
                    <div class="col-lg-2 col-sm-2 text-center btn btn-primary "><asp:Label ID="Label14" runat="server" Text="Abiertas" CssClass=""></asp:Label></div>
                    <div class="col-lg-2 col-sm-2 text-center btn btn-info"><asp:Label ID="Label16" runat="server" Text="Completadas" CssClass=""></asp:Label></div>
                    <div class="col-lg-2 col-sm-2 text-center btn btn-success"><asp:Label ID="Label17" runat="server" Text="Remisionadas" CssClass=""></asp:Label></div>
                    <div class="col-lg-2 col-sm-2 text-center btn btn-warning"><asp:Label ID="Label18" runat="server" Text="Facturadas" CssClass=""></asp:Label></div>
                    <div class="col-lg-2 col-sm-2 text-center btn btn-default"><asp:Label ID="Label19" runat="server" Text="Cerradas" CssClass=""></asp:Label></div>
                    <div class="col-lg-2 col-sm-2 text-center btn btn-danger"><asp:Label ID="Label45" runat="server" Text="Salida Sin Cargos" CssClass=""></asp:Label></div>
                </div>
            </asp:Panel>
            <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel3">
                <ProgressTemplate>
                    <asp:Panel ID="pnlMaskLoad1" runat="server" CssClass="maskLoad"></asp:Panel>
                    <asp:Panel ID="pnlCargando1" runat="server" CssClass="pnlPopUpLoad">
                        <asp:Image ID="imgLoad1" runat="server" ImageUrl="~/img/loading.gif" CssClass="ancho100" />
                    </asp:Panel>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </ContentTemplate>
    </asp:UpdatePanel>

    <telerik:RadWindow RenderMode="Lightweight" ID="modalPopupControl" Title="Comparativo de Costos" EnableShadow="true" Skin="Metro"
    Behaviors="Close,Maximize,Move,Resize,Reload" VisibleOnPageLoad="false" ShowContentDuringLoad="false" DestroyOnClose="true"
    Animation="Fade" runat="server" Modal="true" Width="1000px" Height="700px" Style="z-index: 1000;" >
        <ContentTemplate>
            <asp:UpdatePanel ID="UpdatePanelControl" runat="server">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-lg-12 col-sm-12">
                            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" EnableAJAX="true">
                                <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid2" AllowFilteringByColumn="true" runat="server"  EnableHeaderContextMenu="true" Culture="es-Mx" Skin="Metro" ShowFooter="true"
                                    EnableHeaderContextFilterMenu="true" AllowPaging="True" PagerStyle-AlwaysVisible="true" DataSourceID="SqlDsCompDet" AllowSorting="true" GroupingEnabled="false" PageSize="100" >                        
                                    <MasterTableView DataSourceID="SqlDsCompDet" AutoGenerateColumns="False" DataKeyNames="ref_no_orden">
                                        <Columns>                                                                      
                                            <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="refCantidad" HeaderStyle-Width="100px" FilterControlAltText="Filtro Cantidad" HeaderText="Cantidad" SortExpression="refCantidad" UniqueName="refCantidad" Aggregate="Sum" FooterText="Totales: "/>
                                            <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="refDescripcion" FilterControlAltText="Filtro Refacciones" HeaderText="Refacciones" SortExpression="refDescripcion" UniqueName="refDescripcion" Resizable="true"/>
                                            <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="Estatus" HeaderStyle-Width="150px" FilterControlAltText="Filtro Estatus" HeaderText="Estatus" SortExpression="Estatus" UniqueName="Estatus" Resizable="true"/>
                                            <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="staDescripcion" HeaderStyle-Width="150px" FilterControlAltText="Filtro Estatus Solicitud" HeaderText="Estatus Solicitud" SortExpression="staDescripcion" UniqueName="staDescripcion" Resizable="true"/>
                                            <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="refPrecioVenta" HeaderStyle-Width="150px" FilterControlAltText="Filtro P.U." HeaderText="P.U." SortExpression="refPrecioVenta" UniqueName="refPrecioVenta" Resizable="true" Visible="false"/>
                                            <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="refTotal" HeaderStyle-Width="150px" FilterControlAltText="Filtro Precio Autorizado" HeaderText="Precio Autorizado" SortExpression="refTotal" UniqueName="refTotal" Resizable="true" Aggregate="Sum"/>
                                            <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="Compra" HeaderStyle-Width="150px" FilterControlAltText="Filtro Compra" HeaderText="Precio Compra" SortExpression="Compra" UniqueName="Compra" Resizable="true" Aggregate="Sum"/>                                            
                                        </Columns>
                                    </MasterTableView>
                                    <ClientSettings>
                                        <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true"></Scrolling>
                                    </ClientSettings>                        
                                    <PagerStyle PageSizeControlType="RadDropDownList" Mode="NextPrevAndNumeric"></PagerStyle>
                                </telerik:RadGrid>
                            </telerik:RadAjaxPanel>
                        </div>
                    </div>
                    
                <asp:SqlDataSource runat="server" ID="SqlDsCompDet" ConnectionString='<%$ ConnectionStrings:PVW %>' 
                    SelectCommand="SELECT ro.ref_no_orden, ro.refDescripcion, ro.refCantidad, ro.refPrecioVenta, 
                    CASE ro.refEstatus WHEN 'CA' THEN 'Cancelada' WHEN 'NA' THEN 'No Aplica' WHEN 'EV' THEN 'Evaluación' WHEN 'AU' THEN 'Autorizado' WHEN 'CO' THEN 'Compra'
                    WHEN 'RP' THEN 'Repación' END AS Estatus, ro.refCantidad * ro.refPrecioVenta AS refTotal, re.staDescripcion, 
                    isnull((select sum(o.importe) from orden_compra_detalle o
inner join refacciones_orden r on r.ref_no_orden=o.no_orden and r.ref_id_empresa=o.id_empresa and r.ref_id_taller=o.id_taller and r.reford_id=o.id_refaccion
where o.no_orden=ro.ref_no_orden and o.id_empresa=ro.ref_id_empresa and o.id_taller=ro.ref_id_taller and r.refEstatus in('CO','AU') and r.refestatussolicitud in (2,3) and r.reford_id=ro.reford_id),0) as Compra
                    FROM Refacciones_Orden AS ro LEFT JOIN Rafacciones_Estatus AS re ON re.staRefID = ro.refEstatusSolicitud
                    WHERE ro.refEstatusSolicitud <> 11 and ro.proceso is null AND ro.ref_no_orden=@noOrden AND ro.ref_id_empresa=@empID AND ro.ref_id_taller=@tallerID GROUP BY ro.ref_no_orden, ro.refDescripcion, ro.refCantidad, ro.refPrecioVenta, ro.refEstatus, ro.refEstatusSolicitud, re.staDescripcion,ro.ref_id_empresa,ro.ref_id_taller,ro.reford_id">
                    <SelectParameters>
                        <asp:Parameter DefaultValue="0" Name="noOrden"></asp:Parameter>
                        <asp:Parameter DefaultValue="0" Name="empID"></asp:Parameter>
                        <asp:Parameter DefaultValue="0" Name="tallerID"></asp:Parameter>
                        </SelectParameters>
                    </asp:SqlDataSource>
                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
    </telerik:RadWindow>


</asp:Content>

