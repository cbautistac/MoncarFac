<%@ Page Title="" Language="C#" MasterPageFile="~/Cuentas.master" AutoEventWireup="true" CodeFile="FacturasPorPagar.aspx.cs" Inherits="FacturasPorPagar" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

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
                <i class="fa fa-file"></i>&nbsp;&nbsp;&nbsp;<asp:Label ID="lblTit" runat="server" Text="Cuentas Por Pagar"></asp:Label>&nbsp;&nbsp;&nbsp;<i class="fa fa-dollar"></i>
            </h3>
        </div>
    </div>


    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
            <div class="page-header">
                <!-- /BREADCRUMBS -->
                <div class="clearfix">
                    <h3 class="content-title pull-left">Facturas</h3>
                    <asp:Label ID="lblError" runat="server" CssClass="alert-danger"></asp:Label>
                </div>
            </div>
            <asp:Panel ID="Panel4" runat="server" CssClass="panelCatalogos text-center" ScrollBars="Auto">
                <asp:Label ID="lblEstatus" runat="server" Visible="false"></asp:Label>
                <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" EnableAJAX="true">
                    <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" AllowFilteringByColumn="true" runat="server" EnableHeaderContextMenu="true" Culture="es-Mx" Skin="Metro" 
                            OnItemDataBound="RadGrid1_ItemDataBound" OnNeedDataSource="RadGrid1_NeedDataSource"
                                EnableHeaderContextFilterMenu="true" AllowPaging="True" PagerStyle-AlwaysVisible="true" AllowSorting="true" GroupingEnabled="false" PageSize="20">
                        <MasterTableView  AutoGenerateColumns="False" DataKeyNames="folio" NoMasterRecordsText="No hay facturas con este criterio.">
                            <Columns> 
                                <telerik:GridTemplateColumn FilterCheckListEnableLoadOnDemand="true" HeaderText="Factura" SortExpression="factura" UniqueName="factura" 
                                            FilterControlAltText="Filtro Factura" HeaderStyle-Width="200px"  FilterListOptions="VaryByDataType" DataField="factura">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnfactura" runat="server" CommandArgument='<%# Eval("id_cliprov")+";"+Eval("id_empresa")+";"+Eval("id_taller")+";"+Eval("no_orden")+";"+Eval("estatus") %>' Text='<%# Bind("factura") %>' OnClick="btnfactura_Click" ></asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="razon_social" ItemStyle-CssClass="text-left" FilterControlAltText="Filtro Proveedor" HeaderStyle-Width="200px"  HeaderText="Proveedor" SortExpression="razon_social" UniqueName="razon_social" Resizable="true" />
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="FechaRevision" ItemStyle-CssClass="text-right" FilterControlAltText="Filtro Fecha Revision" HeaderStyle-Width="150px"  HeaderText="Fecha Revision" SortExpression="FechaRevision" UniqueName="FechaRevision" DataFormatString="{0:yyyy-MM-dd}" Resizable="true" />
                                <%-- <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="FechaProgPago" ItemStyle-CssClass="text-right" FilterControlAltText="Filtro Fecha Prog. Pago" HeaderStyle-Width="150px" HeaderText="Fecha Prog. Pago" SortExpression="FechaProgPago" UniqueName="FechaProgPago" DataFormatString="{0:yyyy-MM-dd}" Resizable="true" /> --%>
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="FechaPago" ItemStyle-CssClass="text-right" FilterControlAltText="Filtro Fecha Pago" HeaderStyle-Width="150px" HeaderText="Fecha Pago" SortExpression="FechaPago" UniqueName="FechaPago" DataFormatString="{0:yyyy-MM-dd}" Resizable="true" />
                                <telerik:GridNumericColumn FilterCheckListEnableLoadOnDemand="true" DataField="monto" ItemStyle-CssClass="text-right" FilterControlAltText="Filtro Monto" HeaderStyle-Width="150px" HeaderText="Monto" SortExpression="monto" UniqueName="monto" DataFormatString="{0:N2}"/>                                
                            </Columns>
                            <NoRecordsTemplate>
                                <asp:Label ID="lblNoRegistros" runat="server" CssClass="alert-danger" Text="No existen facturas registradas con los criterios indicados"></asp:Label>
                            </NoRecordsTemplate>
                        </MasterTableView>
                        <ClientSettings>
                            <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true"></Scrolling>
                        </ClientSettings>
                        <PagerStyle PageSizeControlType="RadDropDownList" Mode="NextPrevAndNumeric"></PagerStyle>
                    </telerik:RadGrid>
                </telerik:RadAjaxPanel>               
                <%-- Botones de estados || Todas |Sin Revisión| Revisadas|Programadas| Pagadas||   --%>
                <div class="ancho100 text-center marTop">
                    <div class="col-lg-2 col-sm-2 text-center btn btn-default"><asp:LinkButton runat="server" ID="lnkEstatusTodas" Text="Todas" OnClick="lnkEstatus_Click" CommandArgument="TOD" CssClass="btn-default" /></div>                    
                    <div class="col-lg-2 col-sm-2 text-center btn btn-danger "><asp:LinkButton runat="server" ID="lnkEstatusSinRev" Text="Sin Revisión" OnClick="lnkEstatus_Click" CommandArgument="PEN" CssClass="btn-danger" /></div>
                    <div class="col-lg-2 col-sm-2 text-center btn btn-primary"><asp:LinkButton runat="server" ID="lnkEstatusRev" Text="Revisadas" OnClick="lnkEstatus_Click" CommandArgument="REV" CssClass="btn-primary" /></div>
                    <div class="col-lg-2 col-sm-2 text-center btn btn-info"><asp:LinkButton runat="server" ID="lnkEstatusProg" Text="Programadas" OnClick="lnkEstatus_Click" CommandArgument="PRO" CssClass="btn-info" /></div>
                    <div class="col-lg-2 col-sm-2 text-center btn btn-success"><asp:LinkButton runat="server" ID="lnkEstatusPag" Text="Pagadas" OnClick="lnkEstatus_Click" CommandArgument="PAG" CssClass="btn-success" /></div>                    
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

    <asp:Label ID="lblOrden" runat="server" Visible="false" />
    <asp:Label ID="lblIdEmpresa" runat="server" Visible="false" />
    <asp:Label ID="lblIdTaller" runat="server" Visible="false" />
    <asp:Label ID="lblFactura" runat="server" Visible="false" />
    <asp:Label ID="lblIdCliprov" runat="server" Visible="false" />
    <asp:Label ID="lblRenglonFactura" runat="server" Visible="false" />

    <telerik:RadWindow RenderMode="Lightweight" ID="modalPopupControl" Title="Factura" EnableShadow="true" Skin="Metro"
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
                                    AutoGenerateColumns="False" OnRowDataBound="GridDetalleFac_RowDataBound" DataKeyNames="no_orden,id_taller,id_empresa,Renglon" AllowSorting="true">
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
                                    <asp:LinkButton ID="lnkSalirPop" OnClientClick="" CssClass="btn btn-danger" Visible="false" runat="server"><i class="fa fa-arrow-circle-left"></i><span>&nbsp;&nbsp;Salir</span></asp:LinkButton>
                                </div>
                                </div>
                            </div>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
    </telerik:RadWindow>

</asp:Content>

