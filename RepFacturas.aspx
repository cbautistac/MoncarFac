<%@ Page Title="" Language="C#" MasterPageFile="~/AdmOrdenes.master" AutoEventWireup="true" CodeFile="RepFacturas.aspx.cs" Inherits="RepFacturas" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true"></asp:ScriptManager>
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server"></telerik:RadSkinManager>
    <div class="page-header">		                                
		<!-- /BREADCRUMBS -->
		<div class="clearfix">
            <h3 class="content-title pull-left">Reporte Facturass</h3>             			                                
		</div>
	</div>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>  
            <div class="row"> 
                <div class="col-lg-3 col-sm-3 text-center">
                    <asp:Label ID="Label2" runat="server" Text="Fecha Inicial:"></asp:Label>
                    <asp:TextBox ID="txtFechaIni" runat="server" CssClass="alingMiddle input-small" Enabled="false"  ></asp:TextBox>
                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" BehaviorID="txtFechaIni_CalendarExtender" TargetControlID="txtFechaIni" Format="yyyy-MM-dd" PopupButtonID="lnkFechaIni" />
                    <asp:LinkButton ID="lnkFechaIni" runat="server" CssClass="btn btn-info t14"><i class="fa fa-calendar"></i></asp:LinkButton>
                </div>
                <div class="col-lg-3 col-sm-3 text-center">
                    <asp:Label ID="Label3" runat="server" Text="Fecha Inicial:"></asp:Label>
                    <asp:TextBox ID="txtFechaFin" runat="server" CssClass="alingMiddle input-small" Enabled="false"  ></asp:TextBox>
                    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" BehaviorID="txtFechaFin_CalendarExtender" TargetControlID="txtFechaFin" Format="yyyy-MM-dd" PopupButtonID="lnkFechaFin" />
                    <asp:LinkButton ID="lnkFechaFin" runat="server" CssClass="btn btn-info t14"><i class="fa fa-calendar"></i></asp:LinkButton>
                </div>
                 <div class="col-lg-1 col-sm-1 text-center">
                     <asp:Label ID="Label4" runat="server" Text="Estatus:"></asp:Label>
                </div>
                <div class="col-lg-2 col-sm-2 text-center">
                     <asp:DropDownList ID="ddlEstatus" runat="server" CssClass="input-medium">
                            <asp:ListItem Selected="True" Text="En Captura" Value="P"></asp:ListItem>
                            <asp:ListItem Text="Timbrado" Value="T"></asp:ListItem>
                            <asp:ListItem Text="Cancelada" Value="C"></asp:ListItem>
                        </asp:DropDownList>
                 </div>                                        
                <div class="col-lg-1 col-sm-1 text-center">
                    <asp:LinkButton ID="lnkBuscar" runat="server" CssClass="btn btn-info t14" OnClick="lnkBuscar_Click"><i class="fa fa-search"></i><span>&nbsp;Buscar</span></asp:LinkButton>
                </div>
                <div class="col-lg-1 col-sm-1 text-center">
                    <asp:LinkButton ID="lnkImprime" runat="server" CssClass="btn btn-primary t14" OnClick="lnkImprime_Click"><i class="fa fa-print"></i><span>&nbsp;Imprimir</span></asp:LinkButton>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-12 col-sm-12">
                    <asp:Label ID="lblError" runat="server" CssClass="errores" ></asp:Label>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-12 col-sm-12">
                        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" EnableAJAX="true">
                            <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" AllowFilteringByColumn="true" runat="server"  EnableHeaderContextMenu="true" Culture="es-Mx" Skin="Metro" 
                                EnableHeaderContextFilterMenu="true" AllowPaging="True" PagerStyle-AlwaysVisible="true" DataSourceID="SqlDataSource1" AllowSorting="true" GroupingEnabled="false" PageSize="100" >                        
                                <MasterTableView DataSourceID="SqlDataSource1" AutoGenerateColumns="False" DataKeyNames="idCfd">
                                    <Columns>                                                                
                                        <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="idCfd" FilterControlAltText="Filtro idCfd" HeaderText="idCfd" SortExpression="idCfd" UniqueName="idCfd" Visible="false"/>
                                        <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="EncFolioUUID" HeaderStyle-Width="300px" FilterControlAltText="Filtro UUID" HeaderText="UUID" SortExpression="EncFolioUUID" UniqueName="EncFolioUUID" Resizable="true"/>
                                        <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="EncReferencia" HeaderStyle-Width="300px" FilterControlAltText="Filtro Referenica" HeaderText="Referencia Externa" SortExpression="EncReferencia" UniqueName="EncReferencia" Resizable="true"/>
                                        <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="EncFechaGenera" HeaderStyle-Width="150px" FilterControlAltText="Filtro Fecha" HeaderText="Fecha" SortExpression="EncFechaGenera" UniqueName="EncFechaGenera" Resizable="true"/>
                                        <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="EncReRFC" HeaderStyle-Width="150px" FilterControlAltText="Filtro RFC" HeaderText="Emitida al R.F.C." SortExpression="EncReRFC" UniqueName="EncReRFC" Resizable="true"/>
                                        <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="EncReNombre" FilterControlAltText="Filtro Cliente" HeaderText="Nombre del Receptor del Documento" SortExpression="EncReNombre" UniqueName="EncReNombre" Resizable="true"/>
                                        <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="est" FilterControlAltText="Filtro Estatus" HeaderText="Estatus" SortExpression="est" UniqueName="est" Resizable="true" Visible="false"/>                                                                      
                                    </Columns>
                                    <NoRecordsTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" CssClass="errores" Text="No existen facturas registradas" ></asp:Label>
                                    </NoRecordsTemplate>
                                </MasterTableView>
                                <ClientSettings>
                                    <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true"></Scrolling>
                                </ClientSettings>                        
                                <PagerStyle PageSizeControlType="RadDropDownList" Mode="NextPrevAndNumeric"></PagerStyle>
                            </telerik:RadGrid>
                        </telerik:RadAjaxPanel>
                    </div>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>"
                            SelectCommand="
select e.idCfd,e.EncFolioUUID,e.EncReferencia,convert(char(10),e.EncFechaGenera,120) as EncFechaGenera,e.EncReRFC,case when e.EncReNombre is null then (select renombre from receptores_f where idrecep=e.idrecep) when e.encrenombre='' then (select renombre from receptores_f where idrecep=e.idrecep) else e.encrenombre end as EncReNombre,e.EncEstatus, 
case e.encestatus when 'P' then 'En Captura' when 'E' then 'En Tránsito' when 'T' then 'Timbrado' when 'R' then 'Rechazado' when 'C' then 'Cancelado'	 else 'Otro' end as est
from EncCFD_f e where e.encestatus=@estatus and encfecha between @fechaini and @fechafin order by e.idcfd desc">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="ddlEstatus" PropertyName="SelectedValue" Name="estatus" />
                                <asp:ControlParameter ControlID="txtFechaIni" PropertyName="Text" Name="fechaini" />
                                <asp:ControlParameter ControlID="txtFechaFin" PropertyName="Text" Name="fechafin" />
                            </SelectParameters>
                        </asp:SqlDataSource>
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

