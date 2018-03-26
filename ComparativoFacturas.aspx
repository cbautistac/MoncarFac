<%@ Page Title="" Language="C#" MasterPageFile="~/Cuentas.master" AutoEventWireup="true" CodeFile="ComparativoFacturas.aspx.cs" Inherits="ComparativoFacturas" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <script type="text/javascript">
        function abreWinCtrl() {
            var oWnd = $find("<%=modalPopupControl.ClientID%>");
            oWnd.setUrl('');
            oWnd.show();
        }
        function cierraWinCtrl() {
            var oWnd = $find("<%=modalPopupControl.ClientID%>");
            oWnd.close();
        }
    </script>

    <telerik:RadScriptManager ID="RadScriptManajer1" runat="server" EnableScriptGlobalization="true"></telerik:RadScriptManager>
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server"></telerik:RadSkinManager>
    <div class="row">
        <div class="col-lg-12 col-sm-12 text-center alert-info">
            <h3>
                <i class="fa fa-archive"></i>&nbsp;&nbsp;&nbsp;<asp:Label ID="lblTit" runat="server" Text="Facturas"></asp:Label>
            </h3>
        </div>
    </div>


     <telerik:RadWindow RenderMode="Lightweight" ID="modalPopupControl" Title="Control de Costos" EnableShadow="true" Skin="Metro"
        Behaviors="Close,Maximize,Move,Resize,Reload" VisibleOnPageLoad="false" ShowContentDuringLoad="false" DestroyOnClose="true"
        Animation="Fade" runat="server" Modal="true" Width="1000px" Height="700px" Style="z-index: 1000;" >
        <ContentTemplate>
            <asp:UpdatePanel ID="UpdatePanelControl" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="Panel1" runat="server" CssClass="ancho95 centrado">
                        <div class="row">
                            <div class="col-lg-12 col-sm-12 text-center alert-info">
                                <h3>
                                    <asp:Label ID="lblOrdenSelect" runat="server"></asp:Label></h3>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-12 col-sm-12 text-left">
                                <asp:Label ID="Label1" runat="server" Text="Encabezado"></asp:Label>
                            </div>
                            <div class="col-lg-12 col-sm-12 text-left">
                                <div class="col-lg-2 col-sm-2 text-left">
                                    <asp:Label ID="Label2" runat="server" Text="Factura:"></asp:Label>
                                </div>
                                <div class="col-lg-4 col-sm-4 text-left">
                                     <asp:TextBox ID="lblFacturaPop" runat="server" CssClass="alingMiddle input-large" MaxLength="50" ></asp:TextBox>
                                    
                                </div>
                            </div>
                            <div class="col-lg-12 col-sm-12 text-left">
                                <div class="col-lg-2 col-sm-2 text-left">
                                    <asp:Label ID="Label3" runat="server" Text="Fecha Revision:"></asp:Label>
                                </div>
                                <div class="col-lg-4 col-sm-4 text-left">
                                    <asp:TextBox ID="txtFecharevisionPop" runat="server" CssClass="alingMiddle input-small" Enabled="false" AutoPostBack="true" OnTextChanged="txtFecharevisionPop_TextChanged" ></asp:TextBox>
                                    <cc1:CalendarExtender ID="txtFecharevisionPop_CalendarExtender" runat="server" BehaviorID="txtFecharevisionPop_CalendarExtender"
                                        TargetControlID="txtFecharevisionPop" Format="yyyy-MM-dd" PopupButtonID="lnkFecharevisionPop" />
                                    <asp:LinkButton ID="lnkFecharevisionPop" runat="server" CssClass="btn btn-info t14"><i class="fa fa-calendar"></i></asp:LinkButton>
                                </div>
                                <div class="col-lg-2 col-sm-2 text-left">
                                    <asp:Label ID="Label4" runat="server" Text="Politica:"></asp:Label>
                                </div>
                                <div class="col-lg-4 col-sm-4 text-left">
                                    <asp:DropDownList ID="ddlPoliticaPagoPop" AutoPostBack="true" OnSelectedIndexChanged="ddlPoliticaPagoPop_SelectedIndexChanged" CssClass="input-medium" runat="server" DataSourceID="SqlDataSource3" DataTextField="descripcion" DataValueField="id_politica"></asp:DropDownList>
                                    <asp:SqlDataSource runat="server" ID="SqlDataSource3" ConnectionString='<%$ ConnectionStrings:PVW %>' SelectCommand="select 0 as id_politica, 'Indique Politica' as descripcion union all select id_politica,descripcion+' ('+ltrim(rtrim(Convert(char(5),dias_plazo)))+')' as descripcion from politica_pago"></asp:SqlDataSource>
                                </div>
                            </div>
                            <div class="col-lg-12 col-sm-12 text-left">
                                <div class="col-lg-2 col-sm-2 text-left">
                                    <asp:Label ID="Label5" runat="server" Text="Fecha Prog. Pago:"></asp:Label>
                                </div>
                                <div class="col-lg-4 col-sm-4 text-left">
                                    <asp:TextBox ID="txtFechaProgPagopop" CssClass="input-small" Enabled="false" runat="server"></asp:TextBox>
                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" BehaviorID="txtFechaProgPagopop_CalendarExtender"
                                        TargetControlID="txtFechaProgPagopop" Format="yyyy-MM-dd" PopupButtonID="lnkFechaProgPagopop" />
                                    <asp:LinkButton ID="lnkFechaProgPagopop" runat="server" CssClass="btn btn-info t14"><i class="fa fa-calendar"></i></asp:LinkButton>
                                </div>
                                <div class="col-lg-2 col-sm-2 text-left">
                                    <asp:Label ID="Label6" runat="server" Text="Forma Pago:"></asp:Label>
                                </div>
                                <div class="col-lg-4 col-sm-4 text-left">
                                    <asp:DropDownList ID="ddlFormaPagopop" AutoPostBack="true" OnSelectedIndexChanged="ddlFormaPagopop_SelectedIndexChanged" CssClass="input-medium" runat="server">
                                        <asp:ListItem Text="Efectivo" Value="E" Selected="True" />
                                        <asp:ListItem Text="Cheque" Value="C"/>
                                        <asp:ListItem Text="T. Débito" Value="D" />
                                        <asp:ListItem Text="T. Crédito" Value="A" />
                                        <asp:ListItem Text="Transferencia" Value="B" />
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-lg-12 col-sm-12 text-left">
                                <div class="col-lg-2 col-sm-2 text-left">
                                    <asp:Label ID="Label7" runat="server" Text="Fecha Pago:"></asp:Label>
                                </div>
                                <div class="col-lg-4 col-sm-4 text-left">
                                    <asp:TextBox ID="txtFechaPagoPop" Enabled="false" CssClass="input-small" runat="server"></asp:TextBox>
                                    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" BehaviorID="txtFechaPagoPop_CalendarExtender"
                                        TargetControlID="txtFechaPagoPop" Format="yyyy-MM-dd" PopupButtonID="lnkFechaPagoPop" />
                                    <asp:LinkButton ID="lnkFechaPagoPop" runat="server" CssClass="btn btn-info t14"><i class="fa fa-calendar"></i></asp:LinkButton>
                                </div>
                                <div class="col-lg-2 col-sm-2 text-left">
                                    <asp:Label ID="Label8" runat="server" Text="Banco:"></asp:Label>
                                </div>
                                <div class="col-lg-4 col-sm-4 text-left">
                                    <asp:DropDownList ID="ddlBanco" CssClass="input-medium" runat="server" AppendDataBoundItems="true" DataSourceID="SqlDataSource2" DataTextField="Nombre_Banco" DataValueField="ClvBanco">
                                        <asp:ListItem Value="" Text="Seleccione Banco" />
                                    </asp:DropDownList>
                                    <asp:SqlDataSource runat="server" ID="SqlDataSource2" ConnectionString='<%$ ConnectionStrings:PVW %>' SelectCommand="SELECT [ClvBanco], [Nombre_Banco] FROM [cat_bancos]"></asp:SqlDataSource>
                                </div>
                            </div>
                            <div class="col-lg-12 col-sm-12 text-left">
                                <div class="col-lg-2 col-sm-2 text-left">
                                    <asp:Label ID="Label9" runat="server" Text="Proveedor:"></asp:Label>
                                </div>
                                <div class="col-lg-4 col-sm-4 text-left">
                                    <asp:Label ID="lblProveedorPop" runat="server"></asp:Label>
                                </div>
                                <div class="col-lg-2 col-sm-2 text-left">
                                    <asp:Label ID="Label10" runat="server" Text="Referencia Pago:"></asp:Label>
                                </div>
                                <div class="col-lg-4 col-sm-4 text-left">
                                    <asp:TextBox ID="txtReferenciaPagPop" CssClass="input-medium" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-lg-12 col-sm-12 text-left">
                                 <div class="col-lg-2 col-sm-2 text-left">
                                    <asp:Label ID="Label20" runat="server" Text="Firmante:"></asp:Label>
                                </div>
                                <div class="col-lg-4 col-sm-4 text-left">
                                    <asp:TextBox ID="txtFirmante" CssClass="input-medium" runat="server" MaxLength="300" placeholder="Firmante"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row padding-top-10">
                            <div class="col-lg-12 col-sm-12 text-left padding-top-10">
                                <asp:GridView ID="GridDetalleFac" CssClass="table table-bordered" runat="server"
                                    AutoGenerateColumns="False" OnRowDataBound="GridDetalleFac_RowDataBound" DataKeyNames="no_orden,id_taller,id_empresa,Renglon" AllowPaging="true" AllowSorting="true"
                                    PageSize="7">
                                    <Columns>
                                        <asp:BoundField DataField="Renglon" HeaderText="Renglon" ReadOnly="True" SortExpression="Renglon"></asp:BoundField>
                                        <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" SortExpression="Descripcion"></asp:BoundField>
                                        <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" SortExpression="Cantidad"></asp:BoundField>
                                        <asp:BoundField DataField="Costo_Unitario" ItemStyle-CssClass="text-right" HeaderText="Costo Unitario" SortExpression="Costo_Unitario"></asp:BoundField>
                                        <asp:TemplateField ItemStyle-CssClass="text-right" HeaderText="Importe">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMonto" runat="server" Text='<%# Eval("Importe") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-12 col-sm-12 text-left">
                                <div class="col-lg-8 col-sm-8 text-left">
                                    <asp:TextBox ID="txtObsevacionesPop" placeholder="Observaciones" runat="server" TextMode="MultiLine" />
                                </div>
                                <div class="col-lg-4 col-sm-4 text-left">
                                    <div class="col-lg-12 col-sm-12 text-left">
                                        <div class="row">
                                            <div class="col-lg-4 col-sm-4 text-left">
                                                <asp:Label ID="Label999" runat="server" Text="Suma:" />
                                            </div>
                                            <div class="col-lg-4 col-sm-4 text-right"></div>
                                            <div class="col-lg-4 col-sm-4 text-right">
                                                <asp:Label ID="lblSuma" runat="server" />
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-lg-4 col-sm-4 text-left">
                                                <asp:Label ID="Label11" runat="server" Text="Descuento:" />
                                            </div>
                                            <div class="col-lg-4 col-sm-4 text-right">
                                                <asp:Label ID="lblDescuento" runat="server" Text="0.00" />
                                                <asp:Label ID="Label12" runat="server" Text="%" />
                                            </div>
                                            <div class="col-lg-4 col-sm-4 text-right">
                                                <asp:Label ID="lblDescuentoAplicado" runat="server" />
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-lg-4 col-sm-4 text-left">
                                                <asp:Label ID="Label15" runat="server" Text="Subtotal:" />
                                            </div>
                                            <div class="col-lg-4 col-sm-4 text-right"></div>
                                            <div class="col-lg-4 col-sm-4 text-right">
                                                <asp:Label ID="lblSubtotal" runat="server" />
                                            </div>

                                        </div>
                                        <div class="row">
                                            <div class="col-lg-4 col-sm-4 text-left">
                                                <asp:Label ID="Label13" runat="server" Text="IVA:" />
                                            </div>
                                            <div class="col-lg-4 col-sm-4 text-right">
                                                <asp:Label ID="lblIVA" runat="server" Text="16.00%" />
                                            </div>
                                            <div class="col-lg-4 col-sm-4 text-right">
                                                <asp:Label ID="lblIVAAplicado" runat="server" />
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-lg-4 col-sm-4 text-left">
                                                <asp:Label ID="Label17" runat="server" Text="Total:" />
                                            </div>
                                            <div class="col-lg-4 col-sm-4 text-right"></div>
                                            <div class="col-lg-4 col-sm-4 text-right">
                                                <asp:Label ID="lblTotal" runat="server" />
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-lg-4 col-sm-4 text-left">
                                                <asp:TextBox ID="txtPPP" runat="server" Text="P.P.P." CssClass="input-small" MaxLength="10" />
                                            </div>
                                            <div class="col-lg-4 col-sm-4 text-right">
                                                <asp:TextBox ID="txtPorcPPP" OnTextChanged="txtPorcPPP_TextChanged" AutoPostBack="true" MaxLength="5" runat="server" Text="0.00" CssClass="input-small" />
                                            </div>
                                            <div class="col-lg-4 col-sm-4 text-right">
                                                <asp:TextBox ID="txtPorcPPPAplicado" Enabled="false" runat="server" Text="" CssClass="input-small" />
                                            </div>
                                        </div>

                                        <div class="row padding-top-10">
                                            <div class="col-lg-4 col-sm-4 text-left">
                                                <asp:Label ID="Label998" runat="server" Text="Monto a Pagar:" />
                                            </div><div class="col-lg-4 col-sm-4 text-right"></div>
                                            <div class="col-lg-4 col-sm-4 text-right">
                                                <asp:Label ID="lblMontoAPagarSuma" runat="server" Text="0.00" />
                                            </div>
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-12 col-sm-12 text-center">
                                <asp:Label ID="lblErrorPopup" runat="server" CssClass="errores"></asp:Label></div>
                        </div>
                        <div class="row padding-top-10">
                            <div class="col-lg-12 col-sm-12 text-left padding-top-10">
                                <div class="col-lg-8 col-sm-8 text-left">
                                </div>
                                <div class="col-lg-2 col-sm-2 text-right">
                                    <asp:LinkButton ID="lnkGuardarPopup" OnClientClick="return confirm('¿Está seguro que la información capturada es correcta?')" CssClass="btn btn-info" OnClick="lnkGuardarPopup_Click" runat="server"><i class="fa fa-save"></i><span>&nbsp;&nbsp;Guardar</span></asp:LinkButton>
                                </div>
                                <div class="col-lg-2 col-sm-2 text-center">
                                    <asp:LinkButton ID="lnkSalirPop" OnClick="lnkSalirPop_Click" CssClass="btn btn-danger" Visible="false" runat="server"><i class="fa fa-arrow-circle-left"></i><span>&nbsp;&nbsp;Salir</span></asp:LinkButton>
                                </div>
                                </div>
                            </div>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
    </telerik:RadWindow>
    
    <asp:Label ID="lblOrden" runat="server" Visible="false" />
    <asp:Label ID="lblIdEmpresa" runat="server" Visible="false" />
    <asp:Label ID="lblIdTaller" runat="server" Visible="false" />
    <asp:Label ID="lblFactura" runat="server" Visible="false" />
    <asp:Label ID="lblIdCliprov" runat="server" Visible="false" />
    <asp:Label ID="lblRenglonFactura" runat="server" Visible="false" />

    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
            <div class="page-header">
                <!-- /BREADCRUMBS -->
                <div class="clearfix">
                    <h3 class="content-title pull-left">Facturas por Pagar</h3>
                    <asp:Label ID="lblError" runat="server" CssClass="alert-danger"></asp:Label>
                    <asp:Label ID="lbFecha" Visible="false" runat="server"></asp:Label>
                </div>
            </div>
            
            <div class="row">
                <div class="col-lg-12 col-sm-12">
                    <telerik:RadAjaxPanel ID="RadAjaxPanel3" runat="server" EnableAJAX="true">
                        <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" AllowPaging="true" PageSize="50" AllowFilteringByColumn="true" runat="server"  EnableHeaderContextMenu="true" Culture="es-Mx" Skin="Metro"  ShowFooter="true" OnNeedDataSource="RadGrid1_NeedDataSource" EnableHeaderContextFilterMenu="true" AllowSorting="true" ShowGroupPanel="true" OnItemDataBound="RadGrid1_ItemDataBound" >                        
                            <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_cliprov" GroupLoadMode="Server">
                                <NestedViewTemplate>
                                    <div class="row">
                                        <div class="col-lg-12 col-sm-12">
                                             <asp:Label ID="Label3" Text='<%# Eval("id_cliprov") %>' Visible="false" runat="server"></asp:Label>
                                            <telerik:RadTabStrip RenderMode="Lightweight" runat="server" ID="TabStip1" MultiPageID="Multipage1" SelectedIndex="0" Skin="MetroTouch" >
                                                <Tabs>                                                
                                                    <telerik:RadTab runat="server" Text="Pendientes" PageViewID="PageView2"/>
                                                    <telerik:RadTab runat="server" Text="Pagadas" PageViewID="PageView3"/>
                                                    <telerik:RadTab runat="server" Text="Programadas" PageViewID="PageView4"/>
                                                    <telerik:RadTab runat="server" Text="Vencidas" PageViewID="PageView5"/>
                                                    <telerik:RadTab runat="server" Text="Canceladas" PageViewID="PageView6"/>
                                                    <telerik:RadTab runat="server" Text="Por Pagar " PageViewID="PageView7"/>
                                                </Tabs>
                                            </telerik:RadTabStrip>
                                            <telerik:RadMultiPage runat="server" ID="Multipage1" SelectedIndex="0" RenderSelectedPageOnly="false">                                              
                                                <telerik:RadPageView runat="server" ID="PageView2">
                                                    <asp:Label ID="lblId" Text='<%# Eval("id_cliprov") %>' Visible="false" runat="server"></asp:Label>
                                                    <div class="row">
                                                        <div class="col-lg-12 col-sm-12">
                                                            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" EnableAJAX="true">
                                                                <telerik:RadGrid RenderMode="Lightweight" ID="RadGridPendientes" runat="server"  Culture="es-Mx" Skin="Metro" AllowFilteringByColumn="true" EnableHeaderContextMenu="true" EnableHeaderContextFilterMenu="true" OnItemDataBound="RadGridPendientes_ItemDataBound"
                                                                     AllowPaging="True" PagerStyle-AlwaysVisible="true" AllowSorting="true" GroupingEnabled="false" PageSize="20" DataSourceID="SqlDataSource2" ShowFooter="true">
                                                                    <MasterTableView  AutoGenerateColumns="False" DataKeyNames="folio"  >
                                                                        <Columns>
                                                                            <telerik:GridTemplateColumn FilterCheckListEnableLoadOnDemand="true" HeaderText="Factura" SortExpression="factura" UniqueName="factura" ItemStyle-CssClass="text-center" FilterControlAltText="Filtro Factura" HeaderStyle-Width="200px"  FilterListOptions="VaryByDataType" DataField="factura">
                                                                               <ItemTemplate>
                                                                                    <asp:LinkButton ID="btnfactura" runat="server" CommandArgument='<%# Eval("id_cliprov")+";"+Eval("id_empresa")+";"+Eval("id_taller")+";"+Eval("no_orden")+";"+Eval("estatus") %>' Text='<%# Bind("factura") %>' OnClick="btnfactura_Click" ></asp:LinkButton>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>
                                                                            <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="FechaRevision" ItemStyle-CssClass="text-right" FilterControlAltText="Filtro Fecha Revision" HeaderStyle-Width="150px"  HeaderText="Fecha Revision" SortExpression="FechaRevision" UniqueName="FechaRevision" DataFormatString="{0:yyyy-MM-dd}" Resizable="true" />
                                                                            <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="FechaProgPago" ItemStyle-CssClass="text-right" FilterControlAltText="Filtro Fecha Prog. Pago" HeaderStyle-Width="150px" HeaderText="Fecha Prog. Pago" SortExpression="FechaProgPago" UniqueName="FechaProgPago" DataFormatString="{0:yyyy-MM-dd}" Resizable="true" />
                                                                            <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="FechaPago" ItemStyle-CssClass="text-right" FilterControlAltText="Filtro Fecha Pago" HeaderStyle-Width="150px" HeaderText="Fecha Pago" SortExpression="FechaPago" UniqueName="FechaPago" DataFormatString="{0:yyyy-MM-dd}" Resizable="true" />
                                                                            <telerik:GridNumericColumn FilterCheckListEnableLoadOnDemand="true" DataField="monto_pagar" ItemStyle-CssClass="text-right" FilterControlAltText="Filtro Monto" HeaderStyle-Width="150px" HeaderText="Monto" SortExpression="monto_pagar" UniqueName="monto_pagar" DataFormatString="{0:N2}" Aggregate="Sum" FooterAggregateFormatString="Total Pendiente: {0:N2}"/>                                
                                                                        </Columns>
                                                                    </MasterTableView>
                                                                    <ClientSettings>
                                                                        <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true"></Scrolling>
                                                                    </ClientSettings>
                                                                    <PagerStyle PageSizeControlType="RadDropDownList" Mode="NextPrevAndNumeric"></PagerStyle>
                                                                </telerik:RadGrid>
                                                            </telerik:RadAjaxPanel>                                         
                                                            <asp:SqlDataSource runat="server" ID="SqlDataSource2" ConnectionString='<%$ ConnectionStrings:PVW %>' SelectCommand="select *,(select top 1 case when monto_pagar=0 then monto else monto_pagar END as monto_pagar from facturas where tipocuenta='PA' AND FechaPago IS NULL AND FechaProgPago IS NULL AND FechaRevision IS NULL and id_cliprov = @id and no_orden = t.no_orden AND estatus = 'PEN' and factura=t.factura  order by 1 desc ) as monto_pagar from (select f.factura,f.FechaRevision,f.FechaProgPago,f.FechaPago,f.id_cliprov,f.folio,estatus,f.id_empresa,f.id_taller,f.no_orden,f.clv_politica,f.razon_social from facturas f where f.TipoCuenta = 'PA' AND f.FechaPago IS NULL AND f.FechaProgPago IS NULL AND f.FechaRevision IS NULL and f.id_cliprov = @id and f.no_orden = no_orden AND f.estatus = 'PEN') as t group by factura,FechaRevision,FechaProgPago,FechaPago,id_cliprov,folio,estatus,id_empresa,id_taller,no_orden,clv_politica,razon_social order by t.factura, t.no_orden, t.id_empresa, t.id_taller ">  <%-- PENDIENTES --%>                                                                
                                                                <SelectParameters>
                                                                    <asp:ControlParameter ControlID="lblId" PropertyName="Text" DefaultValue="0" Name="id"></asp:ControlParameter>
                                                                </SelectParameters>
                                                            </asp:SqlDataSource>   
                                                        </div>
                                                    </div>
                                                </telerik:RadPageView>
                                                <telerik:RadPageView runat="server" ID="PageView3">
                                                    <asp:Label ID="IblId" Text='<%# Eval("id_cliprov") %>' Visible="false" runat="server"></asp:Label>
                                                    <div class="row">
                                                        <div class="col-lg-12 col-sm-12">
                                                            <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" EnableAJAX="true">
                                                                <telerik:RadGrid RenderMode="Lightweight" ID="RadGridPagadas" runat="server"  Culture="es-Mx" Skin="Metro" AllowFilteringByColumn="true" EnableHeaderContextMenu="true" EnableHeaderContextFilterMenu="true"
                                                                     AllowPaging="True" PagerStyle-AlwaysVisible="true" AllowSorting="true" GroupingEnabled="false" PageSize="20" DataSourceID="SqlDataSource3" ShowFooter="true">
                                                                    <MasterTableView  AutoGenerateColumns="False" DataKeyNames="folio"  >
                                                                        <Columns>
                                                                            <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="factura" ItemStyle-CssClass="text-center" FilterControlAltText="Filtro fACTURA" HeaderStyle-Width="200px"  HeaderText="Factura" SortExpression="factura" UniqueName="factura" Resizable="true" />                                                                        
                                                                            <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="FechaRevision" ItemStyle-CssClass="text-right" FilterControlAltText="Filtro Fecha Revision" HeaderStyle-Width="150px"  HeaderText="Fecha Revision" SortExpression="FechaRevision" UniqueName="FechaRevision" DataFormatString="{0:yyyy-MM-dd}" Resizable="true" />
                                                                            <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="FechaProgPago" ItemStyle-CssClass="text-right" FilterControlAltText="Filtro Fecha Prog. Pago" HeaderStyle-Width="150px" HeaderText="Fecha Prog. Pago" SortExpression="FechaProgPago" UniqueName="FechaProgPago" DataFormatString="{0:yyyy-MM-dd}" Resizable="true" />
                                                                            <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="FechaPago" ItemStyle-CssClass="text-right" FilterControlAltText="Filtro Fecha Pago" HeaderStyle-Width="150px" HeaderText="Fecha Pago" SortExpression="FechaPago" UniqueName="FechaPago" DataFormatString="{0:yyyy-MM-dd}" Resizable="true" />
                                                                            <telerik:GridNumericColumn FilterCheckListEnableLoadOnDemand="true" DataField="monto_pagar" ItemStyle-CssClass="text-right" FilterControlAltText="Filtro Monto" HeaderStyle-Width="150px" HeaderText="Monto" SortExpression="monto_pagar" UniqueName="monto_pagar" DataFormatString="{0:N2}" Aggregate="Sum" FooterAggregateFormatString="Total Pagado: {0:N2}"/>                                
                                                                        </Columns>
                                                                    </MasterTableView>
                                                                    <ClientSettings>
                                                                        <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true"></Scrolling>
                                                                    </ClientSettings>
                                                                    <PagerStyle PageSizeControlType="RadDropDownList" Mode="NextPrevAndNumeric"></PagerStyle>
                                                                </telerik:RadGrid>
                                                            </telerik:RadAjaxPanel> 
                                                            <asp:SqlDataSource runat="server" ID="SqlDataSource3" ConnectionString='<%$ ConnectionStrings:PVW %>' SelectCommand="select *,(select top 1 case when monto_pagar=0 then monto else monto_pagar END as monto_pagar from facturas where TipoCuenta = 'PA' AND(FechaPago IS NOT NULL or fechaPago is null and estatus = 'PAG') and id_cliprov =@id and no_orden = t.no_orden and factura=t.factura order by 1 desc ) as monto_pagar from (select f.factura,f.FechaRevision,f.FechaProgPago,f.FechaPago,f.id_cliprov,f.folio,estatus,f.id_empresa,f.id_taller,f.no_orden,f.clv_politica,f.razon_social from facturas f where f.TipoCuenta = 'PA' AND(f.FechaPago IS NOT NULL or f.fechaPago is null and f.estatus = 'PAG') and f.id_cliprov =@id and f.no_orden = no_orden) as t group by factura,FechaRevision,FechaProgPago,FechaPago,id_cliprov,folio,estatus,id_empresa,id_taller,no_orden,clv_politica,razon_social order by t.factura, t.no_orden, t.id_empresa, t.id_taller"> <%-- PAGADAS --%>                                                                
                                                                <SelectParameters>
                                                                    <asp:ControlParameter ControlID="lblId" PropertyName="Text" DefaultValue="0" Name="id"></asp:ControlParameter>
                                                                </SelectParameters>
                                                            </asp:SqlDataSource>   
                                                        </div>
                                                    </div>
                                                </telerik:RadPageView>
                                                <telerik:RadPageView runat="server" ID="PageView4">
                                                    <asp:Label ID="Label2" Text='<%# Eval("id_cliprov") %>' Visible="false" runat="server"></asp:Label>
                                                    <div class="row">
                                                        <div class="col-lg-12 col-sm-12">
                                                            <telerik:RadAjaxPanel ID="RadAjaxPanel3" runat="server" EnableAJAX="true">
                                                                <telerik:RadGrid RenderMode="Lightweight" ID="RadGridProgramadas" runat="server"  Culture="es-Mx" Skin="Metro" AllowFilteringByColumn="true" EnableHeaderContextMenu="true" EnableHeaderContextFilterMenu="true" OnItemDataBound="RadGridPendientes_ItemDataBound"
                                                                     AllowPaging="True" PagerStyle-AlwaysVisible="true" AllowSorting="true" GroupingEnabled="false" PageSize="20" DataSourceID="SqlDataSource4" ShowFooter="true">
                                                                    <MasterTableView  AutoGenerateColumns="False" DataKeyNames="folio"  >
                                                                        <Columns>
                                                                            <telerik:GridTemplateColumn FilterCheckListEnableLoadOnDemand="true" HeaderText="Factura" SortExpression="factura" UniqueName="factura" ItemStyle-CssClass="text-center" FilterControlAltText="Filtro Factura" HeaderStyle-Width="200px"  FilterListOptions="VaryByDataType" DataField="factura">
                                                                               <ItemTemplate>
                                                                                    <asp:LinkButton ID="btnfactura" runat="server" CommandArgument='<%# Eval("id_cliprov")+";"+Eval("id_empresa")+";"+Eval("id_taller")+";"+Eval("no_orden")+";"+Eval("estatus") %>' Text='<%# Bind("factura") %>' OnClick="btnfactura_Click" ></asp:LinkButton>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>
                                                                            <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="FechaRevision" ItemStyle-CssClass="text-right" FilterControlAltText="Filtro Fecha Revision" HeaderStyle-Width="150px"  HeaderText="Fecha Revision" SortExpression="FechaRevision" UniqueName="FechaRevision" DataFormatString="{0:yyyy-MM-dd}" Resizable="true" />
                                                                            <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="FechaProgPago" ItemStyle-CssClass="text-right" FilterControlAltText="Filtro Fecha Prog. Pago" HeaderStyle-Width="150px" HeaderText="Fecha Prog. Pago" SortExpression="FechaProgPago" UniqueName="FechaProgPago" DataFormatString="{0:yyyy-MM-dd}" Resizable="true" />
                                                                            <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="FechaPago" ItemStyle-CssClass="text-right" FilterControlAltText="Filtro Fecha Pago" HeaderStyle-Width="150px" HeaderText="Fecha Pago" SortExpression="FechaPago" UniqueName="FechaPago" DataFormatString="{0:yyyy-MM-dd}" Resizable="true" />
                                                                            <telerik:GridNumericColumn FilterCheckListEnableLoadOnDemand="true" DataField="monto_pagar" ItemStyle-CssClass="text-right" FilterControlAltText="Filtro Monto" HeaderStyle-Width="150px" HeaderText="Monto" SortExpression="monto_pagar" UniqueName="monto_pagar" DataFormatString="{0:N2}" Aggregate="Sum" FooterAggregateFormatString="Total Programado: {0:N2}"/>                                
                                                                        </Columns>
                                                                    </MasterTableView>
                                                                    <ClientSettings>
                                                                        <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true"></Scrolling>
                                                                    </ClientSettings>
                                                                    <PagerStyle PageSizeControlType="RadDropDownList" Mode="NextPrevAndNumeric"></PagerStyle>
                                                                </telerik:RadGrid>
                                                            </telerik:RadAjaxPanel> 
                                                            <asp:SqlDataSource runat="server" ID="SqlDataSource4" ConnectionString='<%$ ConnectionStrings:PVW %>' SelectCommand="select *,(select top 1 case when monto_pagar=0 then monto else monto_pagar END as monto_pagar from facturas where TipoCuenta = 'PA' AND FechaPago IS NULL AND FechaProgPago IS NOT NULL and id_cliprov =@id and no_orden = t.no_orden and factura=t.factura order by 1 desc ) as monto_pagar from (select f.factura,f.FechaRevision,f.FechaProgPago,f.FechaPago,f.id_cliprov,f.folio,estatus,f.id_empresa,f.id_taller,f.no_orden,f.clv_politica,f.razon_social from facturas f where f.TipoCuenta = 'PA' AND f.FechaPago IS NULL AND f.FechaProgPago IS NOT NULL and f.id_cliprov =@id and f.no_orden = no_orden) as t group by factura,FechaRevision,FechaProgPago,FechaPago,id_cliprov,folio,estatus,id_empresa,id_taller,no_orden,clv_politica,razon_social">    <%-- Programadas --%>                                                                
                                                                <SelectParameters>
                                                                    <asp:ControlParameter ControlID="lblId" PropertyName="Text" DefaultValue="0" Name="id"></asp:ControlParameter>
                                                                </SelectParameters>
                                                            </asp:SqlDataSource>   
                                                        </div>
                                                    </div>
                                                </telerik:RadPageView>
                                                <telerik:RadPageView runat="server" ID="PageView5">
                                                      <asp:Label ID="Label1" Text='<%# Eval("id_cliprov") %>' Visible="false" runat="server"></asp:Label>
                                                    <div class="row">
                                                        <div class="col-lg-12 col-sm-12">
                                                            <telerik:RadAjaxPanel ID="RadAjaxPanel4" runat="server" EnableAJAX="true">
                                                                <telerik:RadGrid RenderMode="Lightweight" ID="RadGridVencidas" runat="server"  Culture="es-Mx" Skin="Metro" AllowFilteringByColumn="true" EnableHeaderContextMenu="true" EnableHeaderContextFilterMenu="true" OnItemDataBound="RadGridPendientes_ItemDataBound"
                                                                     AllowPaging="True" PagerStyle-AlwaysVisible="true" AllowSorting="true" GroupingEnabled="false" PageSize="20" DataSourceID="SqlDataSource5" ShowFooter="true">
                                                                    <MasterTableView  AutoGenerateColumns="False" DataKeyNames="folio"  >
                                                                        <Columns>
                                                                             <telerik:GridTemplateColumn FilterCheckListEnableLoadOnDemand="true" HeaderText="Factura" SortExpression="factura" UniqueName="factura" ItemStyle-CssClass="text-center" FilterControlAltText="Filtro Factura" HeaderStyle-Width="200px"  FilterListOptions="VaryByDataType" DataField="factura">
                                                                               <ItemTemplate>
                                                                                    <asp:LinkButton ID="btnfactura" runat="server" CommandArgument='<%# Eval("id_cliprov")+";"+Eval("id_empresa")+";"+Eval("id_taller")+";"+Eval("no_orden")+";"+Eval("estatus") %>' Text='<%# Bind("factura") %>' OnClick="btnfactura_Click" ></asp:LinkButton>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>
                                                                            <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="FechaRevision" ItemStyle-CssClass="text-right" FilterControlAltText="Filtro Fecha Revision" HeaderStyle-Width="150px"  HeaderText="Fecha Revision" SortExpression="FechaRevision" UniqueName="FechaRevision" DataFormatString="{0:yyyy-MM-dd}" Resizable="true" />
                                                                            <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="FechaProgPago" ItemStyle-CssClass="text-right" FilterControlAltText="Filtro Fecha Prog. Pago" HeaderStyle-Width="150px" HeaderText="Fecha Prog. Pago" SortExpression="FechaProgPago" UniqueName="FechaProgPago" DataFormatString="{0:yyyy-MM-dd}" Resizable="true" />
                                                                            <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="FechaPago" ItemStyle-CssClass="text-right" FilterControlAltText="Filtro Fecha Pago" HeaderStyle-Width="150px" HeaderText="Fecha Pago" SortExpression="FechaPago" UniqueName="FechaPago" DataFormatString="{0:yyyy-MM-dd}" Resizable="true" />
                                                                            <telerik:GridNumericColumn FilterCheckListEnableLoadOnDemand="true" DataField="monto_pagar" ItemStyle-CssClass="text-right" FilterControlAltText="Filtro Monto" HeaderStyle-Width="150px" HeaderText="Monto" SortExpression="monto_pagar" UniqueName="monto_pagar" DataFormatString="{0:N2}" Aggregate="Sum" FooterAggregateFormatString="Total Pendiente: {0:N2}"/>                                
                                                                        </Columns>
                                                                    </MasterTableView>
                                                                    <ClientSettings>
                                                                        <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true"></Scrolling>
                                                                    </ClientSettings>
                                                                    <PagerStyle PageSizeControlType="RadDropDownList" Mode="NextPrevAndNumeric"></PagerStyle>
                                                                </telerik:RadGrid>
                                                            </telerik:RadAjaxPanel> 
                                                            <asp:SqlDataSource runat="server" ID="SqlDataSource5" ConnectionString='<%$ ConnectionStrings:PVW %>' SelectCommand="select *,(select top 1 case when monto_pagar=0 then monto else monto_pagar END as monto_pagar from facturas where TipoCuenta = 'PA' AND FechaPago IS NULL AND FechaProgPago IS NOT NULL and datediff(day, fechaprogpago, '2017-05-18')> 0 and id_cliprov =@id and no_orden = t.no_orden and factura=t.factura order by 1 desc ) as monto_pagar from (select f.factura,f.FechaRevision,f.FechaProgPago,f.FechaPago,f.id_cliprov,f.folio,estatus,f.id_empresa,f.id_taller,f.no_orden,f.clv_politica,f.razon_social from facturas f where f.TipoCuenta = 'PA' AND f.FechaPago IS NULL AND f.FechaProgPago IS NOT NULL and datediff(day, f.fechaprogpago, '2017-05-18')> 0 and f.id_cliprov =@id and f.no_orden = no_orden) as t group by factura,FechaRevision,FechaProgPago,FechaPago,id_cliprov,folio,estatus,id_empresa,id_taller,no_orden,clv_politica,razon_social">  <%-- Vencidas --%>                                                                
                                                                <SelectParameters>
                                                                    <asp:ControlParameter ControlID="lblId" PropertyName="Text" DefaultValue="0" Name="id"></asp:ControlParameter>
                                                                    <asp:ControlParameter ControlID="lbFecha" PropertyName="Text" DefaultValue="0" Name="Fecha"></asp:ControlParameter>
                                                                </SelectParameters>
                                                            </asp:SqlDataSource>   
                                                        </div>
                                                    </div>
                                                </telerik:RadPageView>                                                
                                                <telerik:RadPageView runat="server" ID="PageView6">
                                                      <asp:Label ID="Label4" Text='<%# Eval("id_cliprov") %>' Visible="false" runat="server"></asp:Label>
                                                    <div class="row">
                                                        <div class="col-lg-12 col-sm-12">
                                                            <telerik:RadAjaxPanel ID="RadAjaxPanel6" runat="server" EnableAJAX="true">
                                                                <telerik:RadGrid RenderMode="Lightweight" ID="RadGridCanceladas" runat="server"  Culture="es-Mx" Skin="Metro" AllowFilteringByColumn="true" EnableHeaderContextMenu="true" EnableHeaderContextFilterMenu="true"
                                                                     AllowPaging="True" PagerStyle-AlwaysVisible="true" AllowSorting="true" GroupingEnabled="false" PageSize="20" DataSourceID="SqlDataSource10" ShowFooter="true">
                                                                    <MasterTableView  AutoGenerateColumns="False" DataKeyNames="folio"  >
                                                                        <Columns>
                                                                            <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="factura" ItemStyle-CssClass="text-center" FilterControlAltText="Filtro fACTURA" HeaderStyle-Width="200px"  HeaderText="Factura" SortExpression="factura" UniqueName="factura" Resizable="true" />                                                                        
                                                                            <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="FechaRevision" ItemStyle-CssClass="text-right" FilterControlAltText="Filtro Fecha Revision" HeaderStyle-Width="150px"  HeaderText="Fecha Revision" SortExpression="FechaRevision" UniqueName="FechaRevision" DataFormatString="{0:yyyy-MM-dd}" Resizable="true" />
                                                                            <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="FechaProgPago" ItemStyle-CssClass="text-right" FilterControlAltText="Filtro Fecha Prog. Pago" HeaderStyle-Width="150px" HeaderText="Fecha Prog. Pago" SortExpression="FechaProgPago" UniqueName="FechaProgPago" DataFormatString="{0:yyyy-MM-dd}" Resizable="true" />
                                                                            <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="FechaPago" ItemStyle-CssClass="text-right" FilterControlAltText="Filtro Fecha Pago" HeaderStyle-Width="150px" HeaderText="Fecha Pago" SortExpression="FechaPago" UniqueName="FechaPago" DataFormatString="{0:yyyy-MM-dd}" Resizable="true" />
                                                                            <telerik:GridNumericColumn FilterCheckListEnableLoadOnDemand="true" DataField="monto_pagar" ItemStyle-CssClass="text-right" FilterControlAltText="Filtro Monto" HeaderStyle-Width="150px" HeaderText="Monto" SortExpression="monto_pagar" UniqueName="monto_pagar" DataFormatString="{0:N2}" Aggregate="Sum" FooterAggregateFormatString="Total Cancelado: {0:N2}"/>                                
                                                                        </Columns>
                                                                    </MasterTableView>
                                                                    <ClientSettings>
                                                                        <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true"></Scrolling>
                                                                    </ClientSettings>
                                                                    <PagerStyle PageSizeControlType="RadDropDownList" Mode="NextPrevAndNumeric"></PagerStyle>
                                                                </telerik:RadGrid>
                                                            </telerik:RadAjaxPanel> 
                                                            <asp:SqlDataSource runat="server" ID="SqlDataSource10" ConnectionString='<%$ ConnectionStrings:PVW %>' SelectCommand="select *,(select top 1 case when monto_pagar=0 then monto else monto_pagar END as monto_pagar from facturas where TipoCuenta = 'PA' and estatus='CAN' and id_cliprov =@id and no_orden = t.no_orden and factura=t.factura order by 1 desc ) as monto_pagar from (select f.factura,f.FechaRevision,f.FechaProgPago,f.FechaPago,f.id_cliprov,f.folio,estatus,f.id_empresa,f.id_taller,f.no_orden,f.clv_politica,f.razon_social from facturas f where f.TipoCuenta = 'PA' and f.estatus='CAN' and f.id_cliprov =@id and f.no_orden = no_orden) as t group by factura,FechaRevision,FechaProgPago,FechaPago,id_cliprov,folio,estatus,id_empresa,id_taller,no_orden,clv_politica,razon_social">  <%-- Canceladas --%>                                                                
                                                                <SelectParameters>
                                                                    <asp:ControlParameter ControlID="lblId" PropertyName="Text" DefaultValue="0" Name="id"></asp:ControlParameter>                                                                    
                                                                </SelectParameters>
                                                            </asp:SqlDataSource>   
                                                        </div>
                                                    </div>
                                                </telerik:RadPageView>
                                                <telerik:RadPageView runat="server" ID="PageView7">
                                                      <asp:Label ID="Label5" Text='<%# Eval("id_cliprov") %>' Visible="false" runat="server"></asp:Label>
                                                    <div class="row">
                                                        <div class="col-lg-12 col-sm-12">
                                                            <telerik:RadAjaxPanel ID="RadAjaxPanel7" runat="server" EnableAJAX="true">
                                                                <telerik:RadGrid RenderMode="Lightweight" ID="RadGridTotal" runat="server"  Culture="es-Mx" Skin="Metro" AllowFilteringByColumn="true" EnableHeaderContextMenu="true" EnableHeaderContextFilterMenu="true" OnItemDataBound="RadGridPendientes_ItemDataBound"
                                                                     AllowPaging="True" PagerStyle-AlwaysVisible="true" AllowSorting="true" GroupingEnabled="false" PageSize="20" DataSourceID="SqlDataSource11" ShowFooter="true">
                                                                    <MasterTableView  AutoGenerateColumns="False" DataKeyNames="folio"  >
                                                                        <Columns>
                                                                             <telerik:GridTemplateColumn FilterCheckListEnableLoadOnDemand="true" HeaderText="Factura" SortExpression="factura" UniqueName="factura" ItemStyle-CssClass="text-center" FilterControlAltText="Filtro Factura" HeaderStyle-Width="200px"  FilterListOptions="VaryByDataType" DataField="factura">
                                                                               <ItemTemplate>
                                                                                    <asp:LinkButton ID="btnfactura" runat="server" CommandArgument='<%# Eval("id_cliprov")+";"+Eval("id_empresa")+";"+Eval("id_taller")+";"+Eval("no_orden")+";"+Eval("estatus") %>' Text='<%# Bind("factura") %>' OnClick="btnfactura_Click" ></asp:LinkButton>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>
                                                                            <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="FechaRevision" ItemStyle-CssClass="text-right" FilterControlAltText="Filtro Fecha Revision" HeaderStyle-Width="150px"  HeaderText="Fecha Revision" SortExpression="FechaRevision" UniqueName="FechaRevision" DataFormatString="{0:yyyy-MM-dd}" Resizable="true" />
                                                                            <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="FechaProgPago" ItemStyle-CssClass="text-right" FilterControlAltText="Filtro Fecha Prog. Pago" HeaderStyle-Width="150px" HeaderText="Fecha Prog. Pago" SortExpression="FechaProgPago" UniqueName="FechaProgPago" DataFormatString="{0:yyyy-MM-dd}" Resizable="true" />
                                                                            <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="FechaPago" ItemStyle-CssClass="text-right" FilterControlAltText="Filtro Fecha Pago" HeaderStyle-Width="150px" HeaderText="Fecha Pago" SortExpression="FechaPago" UniqueName="FechaPago" DataFormatString="{0:yyyy-MM-dd}" Resizable="true" />
                                                                            <telerik:GridNumericColumn FilterCheckListEnableLoadOnDemand="true" DataField="monto_pagar" ItemStyle-CssClass="text-right" FilterControlAltText="Filtro Monto" HeaderStyle-Width="150px" HeaderText="Monto" SortExpression="monto" UniqueName="monto" DataFormatString="{0:N2}" Aggregate="Sum" FooterAggregateFormatString="Total Por Pagar: {0:N2}"/>                                
                                                                        </Columns>
                                                                    </MasterTableView>
                                                                    <ClientSettings>
                                                                        <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true"></Scrolling>
                                                                    </ClientSettings>
                                                                    <PagerStyle PageSizeControlType="RadDropDownList" Mode="NextPrevAndNumeric"></PagerStyle>
                                                                </telerik:RadGrid>
                                                            </telerik:RadAjaxPanel> 
                                                            <asp:SqlDataSource runat="server" ID="SqlDataSource11" ConnectionString='<%$ ConnectionStrings:PVW %>' SelectCommand="select *,(select top 1 case when monto_pagar=0 then monto else monto_pagar END as monto_pagar from facturas where tipocuenta='PA' AND FechaPago IS NULL AND FechaProgPago IS NULL AND FechaRevision IS NULL and id_cliprov = @id and no_orden = t.no_orden AND estatus = 'PEN' and factura=t.factura  order by 1 desc ) as monto_pagar from (select f.factura,f.FechaRevision,f.FechaProgPago,f.FechaPago,f.id_cliprov,f.folio,estatus,f.id_empresa,f.id_taller,f.no_orden,f.clv_politica,f.razon_social from facturas f where f.TipoCuenta = 'PA' AND f.FechaPago IS NULL AND f.FechaProgPago IS NULL AND f.FechaRevision IS NULL and f.id_cliprov = @id and f.no_orden = no_orden AND f.estatus = 'PEN') as t group by factura,FechaRevision,FechaProgPago,FechaPago,id_cliprov,folio,estatus,id_empresa,id_taller,no_orden,clv_politica,razon_social union all select *,(select top 1 case when monto_pagar=0 then monto else monto_pagar END as monto_pagar from facturas where TipoCuenta = 'PA' AND FechaPago IS NULL AND FechaProgPago IS NOT NULL and id_cliprov =@id and no_orden = t.no_orden and factura=t.factura order by 1 desc) as monto_pagar from (select f.factura,f.FechaRevision,f.FechaProgPago,f.FechaPago,f.id_cliprov,f.folio,estatus,f.id_empresa,f.id_taller,f.no_orden,f.clv_politica,f.razon_social from facturas f where f.TipoCuenta = 'PA' AND f.FechaPago IS NULL AND f.FechaProgPago IS NOT NULL and f.id_cliprov =@id and f.no_orden = no_orden) as t group by factura,FechaRevision,FechaProgPago,FechaPago,id_cliprov,folio,estatus,id_empresa,id_taller,no_orden,clv_politica,razon_social"> <%-- Por cobrar --%>                                                                
                                                                <SelectParameters>
                                                                    <asp:ControlParameter ControlID="lblId" PropertyName="Text" DefaultValue="0" Name="id"></asp:ControlParameter>
                                                                </SelectParameters>
                                                            </asp:SqlDataSource>   
                                                        </div>
                                                    </div>
                                                </telerik:RadPageView>
                                            </telerik:RadMultiPage>
                                        </div>
                                    </div>
                                </NestedViewTemplate>
                                <Columns>                                                                                           
                                    <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="razon_social" FilterControlAltText="Filtro Proveedor" ItemStyle-Width="300px" HeaderText="Cliente/Proveedor" SortExpression="razon_social" UniqueName="razon_social" Resizable="true"/>
                                    <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="fPen" FilterControlAltText="Filtro Facturas Pendientes" ItemStyle-Width="150px" HeaderText="Facturas Pendientes" SortExpression="fPen" UniqueName="fPen" Resizable="true" Aggregate="Sum" FooterAggregateFormatString="Pendientes: {0}" FooterStyle-CssClass="text-danger t18"/>
                                    <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="mPen" FilterControlAltText="Filtro Monto Pendiente" ItemStyle-Width="150px" HeaderText="Monto Pendiente" SortExpression="mPen" UniqueName="mPen"  Resizable="true" DataFormatString="{0:N2}" Aggregate="Sum" FooterAggregateFormatString="{0:C2}" FooterStyle-CssClass="text-danger t18"/>
                                    <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="fPag" FilterControlAltText="Filtro Facturas Pagadas" ItemStyle-Width="150px" HeaderText="Facturas Pagadas" SortExpression="fPag" UniqueName="fPag"  Resizable="true" Aggregate="Sum" FooterAggregateFormatString="Pagadas: {0}" FooterStyle-CssClass="text-success t18"/>
                                    <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="mPag" FilterControlAltText="Filtro Monto Pagadas" ItemStyle-Width="150px" HeaderText="Monto Pagadas" SortExpression="mPag" UniqueName="mPag"  Resizable="true" DataFormatString="{0:N2}" Aggregate="Sum" FooterAggregateFormatString="{0:C2}" FooterStyle-CssClass="text-success t18" />
                                    <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="fPro" FilterControlAltText="Filtro Facturas Programadas" ItemStyle-Width="150px" HeaderText="Facturas Programadas" SortExpression="fPro" UniqueName="fPro"  Resizable="true" Aggregate="Sum" FooterAggregateFormatString="Programadas: {0}" FooterStyle-CssClass="text-info t18" />
                                    <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="mPro" FilterControlAltText="Filtro Monto Programado" ItemStyle-Width="150px" HeaderText="Monto Programado" SortExpression="mPro" UniqueName="mPro"  Resizable="true" DataFormatString="{0:N2}" Aggregate="Sum" FooterAggregateFormatString="{0:C2}" FooterStyle-CssClass="text-info t18" />
                                    <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="fVen" FilterControlAltText="Filtro Facturas Vencidas" ItemStyle-Width="150px" HeaderText="Facturas Vencidas" SortExpression="fVen" UniqueName="fVen"  Resizable="true" Aggregate="Sum" FooterAggregateFormatString="Vencidas: {0}" FooterStyle-CssClass="text-warning t18" />
                                    <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="mVen" FilterControlAltText="Filtro Monto Vencido" ItemStyle-Width="150px" HeaderText="Monto Vencido" SortExpression="mVen" UniqueName="mVen"  Resizable="true" DataFormatString="{0:N2}" Aggregate="Sum" FooterAggregateFormatString="{0:C2}" FooterStyle-CssClass="text-warning t18"/>
                                    <telerik:GridTemplateColumn ItemStyle-Width="50px" FilterControlWidth="0px" HeaderText="Descargar">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDescargaPorv" runat="server" CssClass="btn btn-primary t14" CommandArgument='<%# Eval("id_cliprov") %>' OnClick="lnkDescargaProv_Click"><i class="fa fa-download"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                            <ClientSettings>
                                <Scrolling UseStaticHeaders="True" FrozenColumnsCount="1"></Scrolling>
                            </ClientSettings>
                        </telerik:RadGrid>
                    </telerik:RadAjaxPanel>
                    
                                            
                </div>
            </div>
            
           <div class="row">
                <div class="col-lg-12 col-sm-12 text-center">
                    <asp:LinkButton ID="lnkDescarga" runat="server" CssClass="btn btn-primary t14" OnClick="lnkDescarga_Click"><i class="fa fa-download"></i><span>&nbsp;Descargar</span></asp:LinkButton>
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
        <Triggers>
            <asp:PostBackTrigger ControlID="lnkDescarga" />
            
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>

