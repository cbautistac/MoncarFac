<%@ Page Title="" Language="C#" MasterPageFile="~/Cuentas.master" AutoEventWireup="true" CodeFile="PagoOperarios.aspx.cs" Inherits="PagoOperarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<telerik:RadScriptManager ID="RadScriptManajer1" runat="server" EnableScriptGlobalization="true"></telerik:RadScriptManager>
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server"></telerik:RadSkinManager>
    <div class="row">
        <div class="col-lg-12 col-sm-12 text-center alert-info">
            <h3>
                <i class="fa fa-dollar"></i>&nbsp;&nbsp;&nbsp;<asp:Label ID="lblTit" runat="server" Text="Pago Operarios"></asp:Label>&nbsp;&nbsp;&nbsp;<i class="fa fa-users"></i>            
            </h3>
        </div>
    </div>
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
            <div class="page-header">
                <!-- /BREADCRUMBS -->
                <div class="clearfix">
                    <h3 class="content-title pull-left">Operarios</h3>
                    <div class="row pad1m">
                        <div class="col-lg-12 col-sm-12 text-center"><asp:Label ID="lblError" runat="server" CssClass="errores"></asp:Label></div>
                    </div>                    
                </div>
            </div>
            <asp:Panel ID="Panel4" runat="server" CssClass="panelCatalogos text-center" ScrollBars="Auto">
                <asp:Label ID="lblAno" runat="server" Visible="false"></asp:Label>
                <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" EnableAJAX="true">
                    <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" AllowFilteringByColumn="true" runat="server"  EnableHeaderContextMenu="true" Culture="es-Mx" Skin="Metro" OnItemDataBound="RadGrid1_ItemDataBound"
                        EnableHeaderContextFilterMenu="true" AllowPaging="True" ShowFooter="true" PagerStyle-AlwaysVisible="true" DataSourceID="SqlDataSource1" AllowSorting="true" PageSize="20" ShowGroupPanel="True">
                        <MasterTableView DataSourceID="SqlDataSource1" ShowGroupFooter="true" AutoGenerateColumns="False" DataKeyNames="no_orden,id_taller,id_empresa,idEmp">
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
                                <telerik:GridGroupByExpression>
                                    <SelectFields>
                                        <telerik:GridGroupByField FieldAlias="Operario" FieldName="nombre"></telerik:GridGroupByField>
                                    </SelectFields>
                                    <GroupByFields>
                                        <telerik:GridGroupByField FieldName="nombre" SortOrder="Descending"></telerik:GridGroupByField>
                                    </GroupByFields>
                                </telerik:GridGroupByExpression>                                
                            </GroupByExpressions>
                            <Columns>
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="empresa" FilterControlAltText="Filtro Empresa" HeaderText="Empresa" SortExpression="empresa" UniqueName="empresa" Visible="false"/>
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="taller" FilterControlAltText="Filtro Taller" HeaderText="Taller" SortExpression="taller" UniqueName="taller" Visible="false"/>
                                <telerik:GridTemplateColumn FooterText="Total Ordenes: " Aggregate="Count" FilterCheckListEnableLoadOnDemand="true" HeaderText="Orden" SortExpression="no_orden" UniqueName="no_orden" FilterControlAltText="Filtro Orden" DataField="no_orden">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOrden" runat="server" Text='<%# Eval("no_orden") %>' ></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>                                
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="avance_orden" FilterControlAltText="Filtro Avance" HeaderText="% Avance" SortExpression="avance_orden" UniqueName="avance_orden"/>
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="vehiculo" FilterControlAltText="Filtro Vehículo" HeaderText="Vehículo" SortExpression="vehiculo" UniqueName="vehiculo"/>
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="placas" FilterControlAltText="Filtro Placas" HeaderText="Placas" SortExpression="placas" UniqueName="placas"/>                                
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="modelo" FilterControlAltText="Filtro Modelo" HeaderText="Modelo" SortExpression="modelo" UniqueName="modelo"/>
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="color" FilterControlAltText="Filtro Color" HeaderText="Color" SortExpression="color" UniqueName="color"/>                                                                
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="nombre" FilterControlAltText="Filtro Operario" HeaderText="Operario" SortExpression="nombre" UniqueName="nombre"/>
                                <telerik:GridNumericColumn FilterCheckListEnableLoadOnDemand="true" DataField="montoAutorizado" FilterControlAltText="Filtro Monto Autorizado" HeaderText="Monto Autorizado" SortExpression="montoAutorizado" UniqueName="montoAutorizado" DataFormatString="{0:C2}"/>
                                <telerik:GridNumericColumn FilterCheckListEnableLoadOnDemand="true" DataField="montoUsuado" FilterControlAltText="Filtro Monto Usado" HeaderText="Monto Usado" SortExpression="montoUsuado" UniqueName="montoUsuado" DataFormatString="{0:C2}"/>
                                <telerik:GridTemplateColumn FooterText="Total: " Aggregate="Sum" FilterCheckListEnableLoadOnDemand="true" HeaderText="Monto" SortExpression="pagar" UniqueName="pagar" FilterControlAltText="Filtro Monto" DataField="pagar">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMonto" runat="server" Text='<%# Eval("pagar") %>' ></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>                       
                                <telerik:GridCheckBoxColumn FilterCheckListEnableLoadOnDemand="true" DataField="pagado" FilterControlAltText="Filtro Pagado" HeaderText="Pagado" SortExpression="pagado" UniqueName="pagado" />                                
                                <telerik:GridTemplateColumn HeaderText="Pagar" >
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnPagar" runat="server" CommandArgument='<%# Eval("no_orden")+";"+Eval("id_empresa")+";"+Eval("id_taller")+";"+Eval("idEmp")+";"+Eval("pagar") %>' OnClick="btnPagar_Click" OnClientClick="return confirm('¿Está seguro de colocar al operario como pagado?')" CssClass="btn btn-info colorBlanco"><i class="fa fa-dollar"></i></asp:LinkButton>
                                        <asp:LinkButton ID="btnCanPagar" runat="server" CommandArgument='<%# Eval("no_orden")+";"+Eval("id_empresa")+";"+Eval("id_taller")+";"+Eval("idEmp")+";"+Eval("pagar") %>' OnClick="btnCanPagar_Click" OnClientClick="return confirm('¿Está seguro de colocar al operario como no pagado?')" CssClass="btn btn-danger colorBlanco"><i class="fa fa-ban"></i></asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn> 
                            </Columns>
                        </MasterTableView>
                        <ClientSettings>
                            <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="1"></Scrolling>
                        </ClientSettings>                        
                        <PagerStyle PageSizeControlType="RadDropDownList" Mode="NextPrevAndNumeric"></PagerStyle>
                    </telerik:RadGrid>
                </telerik:RadAjaxPanel>
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
                    <div class="col-lg-2 col-sm-2 text-center btn btn-danger"><asp:Label ID="Label1" runat="server" Text="Salida Sin Cargos" CssClass=""></asp:Label></div>
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

</asp:Content>

