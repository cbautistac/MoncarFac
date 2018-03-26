<%@ Page Title="" Language="C#" MasterPageFile="~/AdmonOrden.master" AutoEventWireup="true" CodeFile="Facturaacion.aspx.cs" Inherits="Facturaacion" 
    Culture="es-Mx" UICulture="es-Mx"%>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" />
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
    <telerik:RadFormDecorator ID="RadFormDecorator1" RenderMode="Lightweight" runat="server"
        DecoratedControls="Buttons" />

    <asp:UpdatePanel runat="server" ID="updPanelGeneralesFact">
        <ContentTemplate>

            <asp:Panel runat="server" ID="pnlOperacionesFact" CssClass="col-lg-12 col-ms-12 text-center pad1m">
                <div class="col-lg-12 col-ms-12 text-center">                    
                    <asp:LinkButton ID="lnkCargarInfo" runat="server" CssClass="btn btn-info t14" OnClick="lnkCargarInfo_Click"><i class="fa fa-cogs"></i><span>&nbsp;Obtener Informaci&oacute;n</span></asp:LinkButton>
                </div>                
                <div class="col-lg-12 col-sm-12 text-center">
                    <asp:Label ID="lblError" runat="server" CssClass="errores" />                    
                </div>
            
            </asp:Panel>
           

             <div class="row marTop">
            <%-- Mano de Obra y/o Refaccion --%>
                <div class="col-lg-6 col-sm-6 marTop">
                    
                    <asp:LinkButton ID="lnkAgregarTodo" runat="server" CssClass="btn btn-primary" OnClick="lnkAgregarTodo_Click"><i class="fa fa-plus"></i><span>&nbsp;Agregar Todo</span></asp:LinkButton><br />
                    
                    <telerik:RadAjaxPanel ID="RadAjaxPanel3" runat="server" EnableAJAX="true">
                        <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" AllowFilteringByColumn="true" runat="server"  EnableHeaderContextMenu="true" Culture="es-Mx" Skin="WebBlue" ShowFooter="true"
                            EnableHeaderContextFilterMenu="true" AllowPaging="True" PagerStyle-AlwaysVisible="true" DataSourceID="SqlDataSource3" AllowSorting="true" GroupingEnabled="false" PageSize="100">
                            <MasterTableView DataSourceID="SqlDataSource3" AutoGenerateColumns="False" DataKeyNames="IdFila">
                                <Columns>
                                    <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="txtIdent" HeaderStyle-Width="20%" FilterControlAltText="Filtro Clave" HeaderText="Clave" SortExpression="txtIdent" UniqueName="txtIdent" Resizable="true"/>
                                    <telerik:GridTemplateColumn FilterCheckListEnableLoadOnDemand="true" HeaderText="Producto" SortExpression="txtConcepto" UniqueName="txtConcepto" HeaderStyle-Width="40%" FilterControlAltText="Filtro Producto" DataField="txtConcepto">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkAsignaFacturas" ToolTip="Agregar a la factura" CommandArgument='<%# Bind("IdFila") %>' OnClick="lnkAsignaFacturas_Click" CssClass="textoBold link" runat="server" Text='<%# Bind("txtConcepto") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="radnumCantidad" HeaderStyle-Width="20%" FilterControlAltText="Filtro Cantidad" HeaderText="Cantidad" SortExpression="radnumCantidad" UniqueName="radnumCantidad" Resizable="true"/>
                                    <telerik:GridNumericColumn FilterCheckListEnableLoadOnDemand="true" DataField="txtValUnit" HeaderStyle-Width="20%" FilterControlAltText="Filtro Valor Unitario" HeaderText="Valor Unitario" SortExpression="txtValUnit" UniqueName="txtValUnit" Resizable="true" DataFormatString="{0:N2}"/>
                                    <telerik:GridNumericColumn FilterCheckListEnableLoadOnDemand="true" DataField="lblImporte" HeaderStyle-Width="20%" FilterControlAltText="Filtro Importe" HeaderText="Importe" SortExpression="lblImporte" UniqueName="lblImporte" Resizable="true" Aggregate="Sum" DataFormatString="{0:N2}" FooterAggregateFormatString="{0:N2}"/>
                                </Columns>
                                <NoRecordsTemplate>
                                    <asp:Label runat="server" ID="lblVacio" Text="No se han indicado Conceptos" CssClass="errores"></asp:Label>
                                </NoRecordsTemplate>
                            </MasterTableView>
                            <ClientSettings>
                            <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="1"></Scrolling>
                        </ClientSettings>                        
                        <PagerStyle PageSizeControlType="RadDropDownList" Mode="NextPrevAndNumeric"></PagerStyle>
                        </telerik:RadGrid>
                    </telerik:RadAjaxPanel>
                    <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ConnectionStrings:PVW %>" SelectCommand="select idFila,txtIdent,txtCOncepto,radnumCantidad,Cast(txtValUnit as decimal(15,2)) as txtValUnit,Cast(lblImporte as decimal(15,2)) as lblImporte from documentocfditmp where idusuario=@usuario">
                        <SelectParameters>
                            <asp:QueryStringParameter QueryStringField="u" Name="usuario"></asp:QueryStringParameter> 
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>

            <%-- Mano de Obra y/o Refacciones a Cotizar --%>
         <div class="col-lg-6 col-sm-6 marTop">
             
                 <asp:LinkButton ID="lnkQuitarTodo" runat="server" CssClass="btn btn-primary" OnClick="lnkQuitarTodo_Click"><i class="fa fa-remove"></i><span>&nbsp;Quitar Todo</span></asp:LinkButton><br />
             
                    <telerik:RadAjaxPanel ID="RadAjaxPanel4" runat="server" EnableAJAX="true">
                        <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid2" AllowFilteringByColumn="true" runat="server"  EnableHeaderContextMenu="true" Culture="es-Mx" Skin="WebBlue" ShowFooter="true"
                            EnableHeaderContextFilterMenu="true" AllowPaging="True" PagerStyle-AlwaysVisible="true" DataSourceID="SqlDataSource5" AllowSorting="true" GroupingEnabled="false" PageSize="100">
                            <MasterTableView DataSourceID="SqlDataSource5" AutoGenerateColumns="False" DataKeyNames="idFila">
                                <Columns>
                                    <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="txtIdent" HeaderStyle-Width="20%" FilterControlAltText="Filtro Clave" HeaderText="Clave" SortExpression="txtIdent" UniqueName="txtIdent" Resizable="true"/>
                                    <telerik:GridTemplateColumn FilterCheckListEnableLoadOnDemand="true" HeaderText="Producto" SortExpression="txtConcepto" UniqueName="txtConcepto" HeaderStyle-Width="40%" FilterControlAltText="Filtro Producto" DataField="txtConcepto">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDelProv" ToolTip="Quitar de la factura" CommandArgument='<%# Bind("IdFila") %>' OnClick="lnkDelProv_Click" CssClass="textoBold link" runat="server" Text='<%# Bind("txtConcepto") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="radnumCantidad" HeaderStyle-Width="20%" FilterControlAltText="Filtro Cantidad" HeaderText="Cantidad" SortExpression="radnumCantidad" UniqueName="radnumCantidad" Resizable="true"/>
                                    <telerik:GridNumericColumn FilterCheckListEnableLoadOnDemand="true" DataField="txtValUnit" HeaderStyle-Width="20%" FilterControlAltText="Filtro Valor Unitario" HeaderText="Valor Unitario" SortExpression="txtValUnit" UniqueName="txtValUnit" Resizable="true" DataFormatString="{0:N2}"/>
                                    <telerik:GridNumericColumn FilterCheckListEnableLoadOnDemand="true" DataField="lblImporte" HeaderStyle-Width="20%" FilterControlAltText="Filtro Importe" HeaderText="Importe" SortExpression="lblImporte" UniqueName="lblImporte" Resizable="true" Aggregate="Sum" DataFormatString="{0:N2}" FooterAggregateFormatString="{0:N2}"/>
                                </Columns>
                                <NoRecordsTemplate>
                                    <asp:Label runat="server" ID="lblVacio" Text="No se han seleccionado conceptos para facturar" CssClass="errores"></asp:Label>
                                </NoRecordsTemplate>
                            </MasterTableView>
                            <ClientSettings>
                            <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="1"></Scrolling>
                        </ClientSettings>                        
                        <PagerStyle PageSizeControlType="RadDropDownList" Mode="NextPrevAndNumeric"></PagerStyle>
                        </telerik:RadGrid>
                    </telerik:RadAjaxPanel>
                    <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ConnectionStrings:PVW %>" SelectCommand="select idFila,txtIdent,txtCOncepto,radnumCantidad,Cast(txtValUnit as decimal(15,2)) as txtValUnit,Cast(lblImporte as decimal(15,2)) as lblImporte from documentocfditmp_fact where idusuario=@usuario">
                        <SelectParameters>
                            <asp:QueryStringParameter QueryStringField="u" Name="usuario"></asp:QueryStringParameter> 
                        </SelectParameters>
                    </asp:SqlDataSource>
         </div>

                 </div>
            <div class="row marTop">
                <div class="col-lg-12 col-sm-12 text-center">
                    <asp:LinkButton ID="lnkFacturar" runat="server" CssClass="btn btn-success" OnClientClick="return confirm('¿Está seguro de facturar los conceptos indicados?')" OnClick="lnkFacturar_Click"><i class="fa fa-check-circle"></i><span>&nbsp;Facturar</span></asp:LinkButton>
                </div>
            </div>


     <asp:UpdateProgress ID="UpdateProgressgral" runat="server" AssociatedUpdatePanelID="updPanelGeneralesFact">
                <ProgressTemplate>
                    <asp:Panel ID="pnlMaskLoadgral" runat="server" CssClass="maskLoad" />
                    <asp:Panel ID="pnlCargandogral" runat="server" CssClass="pnlPopUpLoad">
                        <asp:Image ID="imgLoadgral" runat="server" ImageUrl="~/img/loading.gif" CssClass="ancho100" />
                    </asp:Panel>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </ContentTemplate>
    </asp:UpdatePanel>
   
    
</asp:Content>

