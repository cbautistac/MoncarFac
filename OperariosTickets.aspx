<%@ Page Title="" Language="C#" MasterPageFile="~/Cuentas.master" AutoEventWireup="true" CodeFile="OperariosTickets.aspx.cs" Inherits="OperariosTickets" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <telerik:RadScriptManager ID="RadScriptManajer1" runat="server" EnableScriptGlobalization="true"></telerik:RadScriptManager>
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server"></telerik:RadSkinManager>
    <div class="row">
        <div class="col-lg-12 col-sm-12 text-center alert-info">
            <h3>
                <i class="fa fa-users"></i>&nbsp;&nbsp;&nbsp;<asp:Label ID="lblTit" runat="server" Text="Tickets Operarios"></asp:Label>
            </h3>
        </div>
    </div>

     <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
            <div class="page-header">
                <!-- /BREADCRUMBS -->
                <div class="clearfix">
                    <h3 class="content-title pull-left">Tickets Operarios</h3>
                    <asp:Label ID="lblError" runat="server" CssClass="alert-danger"></asp:Label>
                    
                </div>
            </div>
            
            <div class="row">
                <div class="col-lg-3 col-sm-3">
                     <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" EnableAJAX="true">
                        <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" AllowFilteringByColumn="true" runat="server"  EnableHeaderContextMenu="true" Culture="es-Mx" Skin="Metro"
                            EnableHeaderContextFilterMenu="true" AllowPaging="True" PagerStyle-AlwaysVisible="true" DataSourceID="SqlDataSource1" AllowSorting="true" GroupingEnabled="false" PageSize="100">
                            <MasterTableView DataSourceID="SqlDataSource1" AutoGenerateColumns="False" DataKeyNames="idemp">
                                <Columns>                               
                                    <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="nombre" FilterControlAltText="Filtro Empleado" HeaderText="Empleado" SortExpression="nombre" UniqueName="nombre" Resizable="true"/>
                                </Columns>
                                <NoRecordsTemplate>
                                    <asp:Label runat="server" ID="lblVacio" Text="No se han encontrado Personal asignado" CssClass="text-danger"></asp:Label>
                                </NoRecordsTemplate>
                            </MasterTableView>
                            <ClientSettings EnablePostBackOnRowClick="true">
                            <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="1"></Scrolling>
                                <Selecting AllowRowSelect="true" /> 
                        </ClientSettings>                        
                        <PagerStyle PageSizeControlType="RadDropDownList" Mode="NextPrevAndNumeric"></PagerStyle>
                        </telerik:RadGrid>
                    </telerik:RadAjaxPanel>
                    <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:PVW %>' SelectCommand="select r.idemp, isnull(case when e.llave_nombre_empleado is null then ltrim(rtrim(nombres))+' '+ltrim(rtrim(apellido_paterno))+' '+ltrim(rtrim(apellido_materno)) else e.llave_nombre_empleado end,'Pendiente') as nombre from registro_pinturas  r
left join empleados e on e.idemp=r.idemp
group by r.idemp,e.llave_nombre_empleado,e.nombres,e.apellido_paterno,e.apellido_materno"/>
                </div>
            
                <div class="col-lg-9 col-sm-9">
                     <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" EnableAJAX="true">
                        <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid2" AllowFilteringByColumn="true" runat="server"  EnableHeaderContextMenu="true" Culture="es-Mx" Skin="Metro" OnItemDataBound="RadGrid2_ItemDataBound"
                            EnableHeaderContextFilterMenu="true" AllowPaging="True" PagerStyle-AlwaysVisible="true" DataSourceID="SqlDataSource2" AllowSorting="true" GroupingEnabled="false" PageSize="100">
                            <MasterTableView DataSourceID="SqlDataSource2" AutoGenerateColumns="False" DataKeyNames="ano,id_solicitud">
                                <Columns>
                                    <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="ticket" FilterControlAltText="Filtro Ticket" HeaderText="Ticket" SortExpression="ticket" UniqueName="ticket" Resizable="true"/>
                                    <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="folio_solicitud" FilterControlAltText="Filtro Solicitud" HeaderText="Solicitud" SortExpression="folio_solicitud" UniqueName="folio_solicitud" Resizable="true"/>
                                    <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="no_orden" FilterControlAltText="Filtro Orden" HeaderText="Orden" SortExpression="no_orden" UniqueName="no_orden" Resizable="true"/>
                                    <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="detalle" FilterControlAltText="Filtro Detalle" HeaderText="Detalle" SortExpression="detalle" UniqueName="detalle" Resizable="true"/>
                                    <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="montoAutorizado" FilterControlAltText="Filtro Autorizado" HeaderText="Autorizado" SortExpression="montoAutorizado" UniqueName="montoAutorizado" Resizable="true"/>
                                    <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="montoUsado" FilterControlAltText="Filtro Usado" HeaderText="Usado" SortExpression="montoUsado" UniqueName="montoUsado" Resizable="true"/>
                                    <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="montoPagar" FilterControlAltText="Filtro Pagar" HeaderText="Pagar" SortExpression="montoPagar" UniqueName="montoPagar" Resizable="true"/>
                                     <telerik:GridTemplateColumn HeaderText= "Aplicar Pagar" >
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnPagar" runat="server" CommandArgument='<%# Eval("no_orden")+";"+Eval("id_empresa")+";"+Eval("id_taller")+";"+Eval("idemp")+";"+Eval("id_asignacion")+";"+Eval("montoPagar") %>' OnClick="btnPagar_Click" OnClientClick="return confirm('¿Está seguro de colocar al operario como pagado?')" CssClass="btn btn-info colorBlanco"><i class="fa fa-dollar"></i></asp:LinkButton>
                                        <asp:LinkButton ID="btnCanPagar" runat="server" CommandArgument='<%# Eval("no_orden")+";"+Eval("id_empresa")+";"+Eval("id_taller")+";"+Eval("idemp")+";"+Eval("id_asignacion")+";"+Eval("montoPagar") %>' OnClick="btnCanPagar_Click" OnClientClick="return confirm('¿Está seguro de colocar al operario como no pagado?')" CssClass="btn btn-danger colorBlanco"><i class="fa fa-ban"></i></asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                </Columns>
                                <NoRecordsTemplate>
                                    <asp:Label runat="server" ID="lblVacio" Text="No se han encontrado informacion" CssClass="text-danger"></asp:Label>
                                </NoRecordsTemplate>
                            </MasterTableView>
                            <ClientSettings>
                            <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="1"></Scrolling>
                                
                        </ClientSettings>                        
                        <PagerStyle PageSizeControlType="RadDropDownList" Mode="NextPrevAndNumeric"></PagerStyle>
                        </telerik:RadGrid>
                    </telerik:RadAjaxPanel>
                    <asp:SqlDataSource runat="server" ID="SqlDataSource2" ConnectionString='<%$ ConnectionStrings:PVW %>' SelectCommand="select r.ano,r.id_solicitud,r.idemp,isnull(r.ticket,0) as ticket,r.folio_solicitud,r.id_empresa,r.id_taller,r.no_orden, r.detalle, cast(isnull(v.total+v.iva,0) as decimal(15,2)) as montoUsado,cast(isnull(o.monto,0) as decimal(15,2)) as montoAutorizado,cast((isnull(o.monto,0)-isnull(v.total+v.iva,0))as decimal(15,2)) as montoPagar,isnull(o.id_asignacion,0) as id_asignacion,isnull(o.pagado,0) as pagado
from registro_pinturas r 
left join venta_enc v on v.ticket=isnull(r.ticket,0)
left join operativos_orden o on o.id_empresa=r.id_empresa and o.id_taller=r.id_taller and o.no_orden=r.no_orden and o.idemp=r.idemp
where r.idemp=@id">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="RadGrid1" Name="id" DefaultValue="0" Type="Int32" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
            </div>

  


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

