<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReporteFacturacion.aspx.cs" Inherits="ReporteFacturacion" MasterPageFile="~/AdmOrdenes.master"  UICulture="es-Mx" Culture="es-Mx" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true"></asp:ScriptManager>
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server"></telerik:RadSkinManager>
    <div class="page-header">		                                
		<!-- /BREADCRUMBS -->
		<div class="clearfix">
            <h3 class="content-title pull-left">Exportar Facturas</h3>             			                                
		</div>
	</div>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-lg-4 col-sm-4 text-center">
                    <asp:Label ID="lblFechaIni" runat="server" Text="Fecha Inicial:" ></asp:Label>
                    <asp:TextBox ID="txtFechaIni" runat="server" CssClass="alingMiddle input-small" Enabled="false" Text="Fecha Inicial" ></asp:TextBox>
                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" BehaviorID="txtFechaIni_CalendarExtender" TargetControlID="txtFechaIni" Format="yyyy-MM-dd" PopupButtonID="lnkFechaIni"  />
                    <asp:LinkButton ID="lnkFechaIni" runat="server" CssClass="btn btn-info t14" ><i class="fa fa-calendar" ></i></asp:LinkButton>
                </div>
                <div class="col-lg-4 col-sm-4 text-center">
                    <asp:Label ID="lbl" runat="server" Text="Fecha Final:"></asp:Label>
                    <asp:TextBox ID="txtFechaFin" runat="server" CssClass="alingMiddle input-small" Enabled="false" Text="Fecha Final"></asp:TextBox>
                    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" BehaviorID="txtFechaFin_CalendarExtender" TargetControlID="txtFechaFin" Format="yyyy-MM-dd" PopupButtonID="lnkFechaFin" />
                    <asp:LinkButton ID="lnkFechaFin" runat="server" CssClass="btn btn-info t14"><i class="fa fa-calendar"></i></asp:LinkButton>
                </div>
                <div class="col-lg-4 col-sm-4 text-center">
                    <asp:LinkButton ID="lnkGeneraInfo" runat="server" CssClass="btn btn-primary" OnClick="lnkGeneraInfo_Click"><i class="fa fa-search"></i><span>&nbsp;Buscar</span></asp:LinkButton>
                </div>
            </div>
            <div class="row pad1m text-center">
                <div class="col-lg-12 col-sm-12">
                    <asp:Label ID="lblError" runat="server" CssClass="errores" ></asp:Label>
                </div>                
            </div>
            <div class="row">
                <div class="col-lg-12 col-sm-12">
                    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" EnableAJAX="true">
                        <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" AllowFilteringByColumn="true" runat="server"  EnableHeaderContextMenu="true" Culture="es-Mx" Skin="Metro" OnItemDataBound="RadGrid1_ItemDataBound" 
                            EnableHeaderContextFilterMenu="true" AllowPaging="True" PagerStyle-AlwaysVisible="true" AllowSorting="true" GroupingEnabled="false" PageSize="50" >                        
                            <MasterTableView AutoGenerateColumns="False" >
                                <Columns>                               
                                    <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="RFC" FilterControlAltText="Filtro R.F.C." HeaderText="R.F.C." SortExpression="RFC" UniqueName="RFC" Resizable="true"/>
                                    <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="Nombre" FilterControlAltText="Filtro Nombre" HeaderText="Nombre" SortExpression="Nombre" UniqueName="Nombre" Resizable="true"/>
                                    <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="UUID" FilterControlAltText="Filtro UUID" HeaderText="UUID" SortExpression="UUID" UniqueName="UUID" Resizable="true"/>
                                    <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="Factura" FilterControlAltText="Filtro Factura" HeaderText="Factura" SortExpression="Factura" UniqueName="Factura" Resizable="true"/>
                                    <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="Fecha_Crea" FilterControlAltText="Filtro Fecha Genera" HeaderText="Fecha Genera" SortExpression="Fecha_Crea" UniqueName="Fecha_Crea" Resizable="true"/>
                                    <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="Fecha_Genera" FilterControlAltText="Filtro Timbrado" HeaderText="Fecha Timbrado" SortExpression="Fecha_Genera" UniqueName="Fecha_Genera" Resizable="true"/>
                                    <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="Fecha_Cancela" FilterControlAltText="Filtro Cancela" HeaderText="Fecha Cancela" SortExpression="Fecha_Cancela" UniqueName="Fecha_Cancela" Resizable="true"/>
                                    <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="Neto" FilterControlAltText="Filtro Neto" HeaderText="Neto" SortExpression="Neto" UniqueName="Neto" Resizable="true"/>
                                    <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="Descuento_Global" FilterControlAltText="Filtro Descuento" HeaderText="Descuento" SortExpression="Descuento_Global" UniqueName="Descuento_Global" Resizable="true"/>
                                    <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="Subtotal" FilterControlAltText="Filtro Subtotal" HeaderText="Subtotal" SortExpression="Subtotal" UniqueName="Subtotal" Resizable="true"/>
                                    <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="Traslados" FilterControlAltText="Filtro Traslados" HeaderText="Traslados" SortExpression="Traslados" UniqueName="Traslados" Resizable="true"/>
                                    <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="Retenciones" FilterControlAltText="Filtro Retenciones" HeaderText="Retenciones" SortExpression="Retenciones" UniqueName="Retenciones" Resizable="true"/>
                                    <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="Total" FilterControlAltText="Filtro Total" HeaderText="Total" SortExpression="Total" UniqueName="Total" Resizable="true"/>
                                    <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="Taller_Tienda" FilterControlAltText="Filtro Taller-Tienda" HeaderText="Taller/Tienda" SortExpression="Taller_Tienda" UniqueName="Taller_Tienda" Resizable="true"/>
                                    <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="Orden_Ticket" FilterControlAltText="Filtro Orden-Ticket" HeaderText="Orden/Ticket" SortExpression="Orden_Ticket" UniqueName="Orden_Ticket" Resizable="true"/>
                                    <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="Marca" FilterControlAltText="Filtro Vehículo" HeaderText="Vehículo" SortExpression="Marca" UniqueName="Marca" Resizable="true"/>
                                    <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="Modelo" FilterControlAltText="Filtro Modelo" HeaderText="Modelo" SortExpression="Modelo" UniqueName="Modelo" Resizable="true"/>
                                    <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="Color" FilterControlAltText="Filtro Color" HeaderText="Color" SortExpression="Color" UniqueName="Color" Resizable="true"/>
                                    <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="Placas" FilterControlAltText="Filtro Placas" HeaderText="Placas" SortExpression="Placas" UniqueName="Placas" Resizable="true"/>
                                    <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="Sinistero" FilterControlAltText="Filtro Sinistero" HeaderText="Sinistero" SortExpression="Sinistero" UniqueName="Sinistero" Resizable="true"/>
                                    <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="Fecha_Pago" FilterControlAltText="Filtro Fecha Pago" HeaderText="Fecha Pago" SortExpression="Fecha_Pago" UniqueName="Fecha_Pago" Resizable="true"/>
                                </Columns>
                                <NoRecordsTemplate>
                                    <asp:Label runat="server" ID="lblVacio" Text="No existen facturas registradas" CssClass="errores"></asp:Label>
                                </NoRecordsTemplate>
                            </MasterTableView>
                            <ClientSettings>
                                <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" ></Scrolling>                                
                            </ClientSettings>                                                
                            <PagerStyle PageSizeControlType="RadDropDownList" Mode="NextPrevAndNumeric"></PagerStyle>
                        </telerik:RadGrid>
                    </telerik:RadAjaxPanel>
                </div>                
            </div>            
    
            
            <div class="row marTop">
                <div class="col-lg-6 col-sm-6 text-center">
                    <asp:LinkButton ID="lnkDescarga" runat="server" CssClass="btn btn-primary t14" OnClick="lnkDescarga_Click"><i class="fa fa-download"></i><span>&nbsp;Descargar</span></asp:LinkButton>
                </div> 
                 <div class="col-lg-6 col-sm-6 text-center">
                    <asp:LinkButton ID="lnkTodo" runat="server" CssClass="btn btn-primary t14" OnClick="lnkTodo_Click"><i class="fa fa-download"></i><span>&nbsp;Descargar Todo</span></asp:LinkButton>
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
        <Triggers>
            <asp:PostBackTrigger ControlID="lnkDescarga" />
            <asp:PostBackTrigger ControlID="lnkTodo" />            
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

