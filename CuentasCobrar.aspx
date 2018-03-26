<%@ Page Title="" Language="C#" MasterPageFile="~/Cuentas.master" AutoEventWireup="true" CodeFile="CuentasCobrar.aspx.cs" Inherits="CuentasCobrar" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

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
                <i class="fa fa-file"></i>&nbsp;&nbsp;&nbsp;<asp:Label ID="lblTit" runat="server" Text="Cuentas por Cobrar"></asp:Label>&nbsp;&nbsp;&nbsp;<i class="fa fa-dollar"></i>
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
            <asp:Label ID="lblEstatus" runat="server" Visible="false"></asp:Label>
            <asp:Panel ID="Panel4" runat="server" CssClass="panelCatalogos text-center" ScrollBars="Auto">
                <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" EnableAJAX="true">
                    <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" AllowFilteringByColumn="true" runat="server" EnableHeaderContextMenu="true" Culture="es-Mx" Skin="Metro" OnItemDataBound="RadGrid1_ItemDataBound" OnNeedDataSource="RadGrid1_NeedDataSource"
                        EnableHeaderContextFilterMenu="true" AllowPaging="True" PagerStyle-AlwaysVisible="true" AllowSorting="true" GroupingEnabled="false" PageSize="20">
                        <MasterTableView  AutoGenerateColumns="False" DataKeyNames="Folio" NoMasterRecordsText="No hay facturas con este criterio.">
                            <Columns>
                                <telerik:GridTemplateColumn FilterCheckListEnableLoadOnDemand="true" HeaderText="Factura" SortExpression="Factura" UniqueName="Factura" FilterControlAltText="Filtro Factura" HeaderStyle-Width="200px" DataField="Factura">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnfactura" runat="server" CommandArgument='<%# Eval("idrecep")+";"+Eval("id_empresa")+";"+Eval("id_taller")+";"+Eval("no_orden")+";"+Eval("monto_pagar")+";"+Eval("estatus")+";"+Eval("renglon") %>' Text='<%# Bind("Factura") %>' OnClick="btnfactura_Click"></asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="renombre" ItemStyle-CssClass="text-left" HeaderStyle-Width="200px" FilterControlAltText="Filtro Cliente" HeaderText="Cliente" SortExpression="renombre" UniqueName="renombre" Resizable="true" />
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="no_orden" HeaderStyle-Width="150px" ItemStyle-CssClass="text-right" FilterControlAltText="Filtro Orden-Ticket" HeaderText="Orden / Ticket" SortExpression="no_orden" UniqueName="no_orden" Resizable="true" />
                                <%-- <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="taller" HeaderStyle-Width="150px" ItemStyle-CssClass="text-right" FilterControlAltText="Filtro Taller-Tienda" HeaderText="Taller / Tienda" SortExpression="taller" UniqueName="taller" Resizable="true" />
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="siniestro" HeaderStyle-Width="150px" ItemStyle-CssClass="text-right" FilterControlAltText="Filtro Siniestro" HeaderText="Siniestro" SortExpression="siniestro" UniqueName="siniestro" Resizable="true" />
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="poliza" HeaderStyle-Width="150px" ItemStyle-CssClass="text-right" FilterControlAltText="Filtro Poliza" HeaderText="Póliza" SortExpression="poliza" UniqueName="poliza" Resizable="true" />--%>
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="monto_pagar" HeaderStyle-Width="150px" ItemStyle-CssClass="text-right" FilterControlAltText="Filtro Monto" HeaderText="Monto" SortExpression="monto_pagar" UniqueName="monto_pagar" DataFormatString="{0:N2}" Resizable="true" />
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="estatusFacturacion" HeaderStyle-Width="150px" FilterControlAltText="Filtro Estatus" HeaderText="Estatus" SortExpression="estatusFacturacion" UniqueName="estatusFacturacion" />                                 
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
                <div class="ancho100 text-center marTop">                                       
                    <div class="col-lg-2 col-sm-2 text-center btn btn-default"><asp:LinkButton runat="server" ID="lnkEstatusTodas" Text="Todas" OnClick="lnkEstatus_Click" CommandArgument="TOD" CssClass="btn-default" /></div>                    
                    <div class="col-lg-2 col-sm-2 text-center btn btn-danger "><asp:LinkButton runat="server" ID="lnkEstatusCan" Text="Canceladas" OnClick="lnkEstatus_Click" CommandArgument="CAN" CssClass="btn-danger" /></div>
                    <div class="col-lg-2 col-sm-2 text-center btn btn-warning"><asp:LinkButton runat="server" ID="lnkEstatusRev" Text="Revisadas" OnClick="lnkEstatus_Click" CommandArgument="REV" CssClass="btn-warning" /></div>
                    <div class="col-lg-2 col-sm-2 text-center btn btn-info"><asp:LinkButton runat="server" ID="lnkEstatusPend" Text="Pendientes" OnClick="lnkEstatus_Click" CommandArgument="PEN" CssClass="btn-info" /></div>
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

    <telerik:RadWindow RenderMode="Lightweight" ID="modalPopupControl" Title="Seguimiento de Facturas" EnableShadow="true" Skin="Metro"
        Behaviors="Close,Maximize,Move,Resize,Reload" VisibleOnPageLoad="false" ShowContentDuringLoad="false" DestroyOnClose="true"
        Animation="Fade" runat="server" Modal="true" Width="1000px" Height="700px" Style="z-index: 1000;" >
        <ContentTemplate>
            <asp:UpdatePanel ID="UpdatePanelControl" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="Panel1" runat="server" CssClass="ancho95 centrado"> 
                        <div class="row marTop">
                            <div class="col-lg-12 col-sm-12 text-center alert-info">
                                <h3>
                                    <asp:Label ID="Label2" runat="server" Text="Factura:"></asp:Label>&nbsp;&nbsp;                                
                                    <asp:Label ID="lblFacturaPop" runat="server"></asp:Label>                                
                                </h3>
                            </div>                            
                        </div>
                        <div class="row marTop">
                            <div class="col-lg-4 col-sm-4 text-left">
                                <asp:Label ID="Label1" runat="server" Text="Orden:" CssClass="textoBold"></asp:Label>&nbsp;
                                <asp:Label ID="lblOrdenF" runat="server" ></asp:Label>
                            </div>
                            <div class="col-lg-4 col-sm-4 text-left">
                                <asp:Label ID="Label3" runat="server" Text="Taller:" CssClass="textoBold"></asp:Label>&nbsp;
                                <asp:Label ID="lblTaller" runat="server" ></asp:Label>
                            </div>
                            <div class="col-lg-4 col-sm-4 text-left">
                                <asp:Label ID="Label25" runat="server" Text="Tipo Valuación:" CssClass="textoBold"></asp:Label>&nbsp;
                                <asp:Label ID="lblValuacion" runat="server" ></asp:Label>
                            </div>
                        </div>
                        <div class="row marTop">
                            <div class="col-lg-6 col-sm-6 text-left">
                                <asp:Label ID="Label4" runat="server" Text="Fecha Recepción:" CssClass="textoBold"></asp:Label>&nbsp;
                                <asp:Label ID="lblFrecepcion" runat="server" ></asp:Label>
                            </div>
                            <div class="col-lg-6 col-sm-6 text-left">
                                <asp:Label ID="Label6" runat="server" Text="Fecha Entrega Estimada:" CssClass="textoBold"></asp:Label>&nbsp;
                                <asp:Label ID="lblFentregaEst" runat="server" ></asp:Label>
                            </div>
                        </div>
                        <div class="row marTop">
                            <div class="col-lg-6 col-sm-6 text-left">
                                <asp:Label ID="Label8" runat="server" Text="Cliente:" CssClass="textoBold"></asp:Label>&nbsp;
                                <asp:Label ID="lblCliente" runat="server" ></asp:Label>
                            </div>
                            <div class="col-lg-6 col-sm-6 text-left">
                                <asp:Label ID="Label10" runat="server" Text="Aseguradora:" CssClass="textoBold"></asp:Label>&nbsp;
                                <asp:Label ID="lblAseguradora" runat="server" ></asp:Label>
                            </div>
                        </div>
                        <div class="row marTop">
                            <div class="col-lg-12 col-sm-12 text-left">
                                <asp:Label ID="Label16" runat="server" Text="Asegurado:" CssClass="textoBold"></asp:Label>&nbsp;
                                <asp:Label ID="lblAsegurado" runat="server" ></asp:Label>
                            </div>                            
                        </div>
                        <div class="row marTop">
                            <div class="col-lg-4 col-sm-4 text-left">
                                <asp:Label ID="Label21" runat="server" Text="Vehículo:" CssClass="textoBold"></asp:Label>&nbsp;
                                <asp:Label ID="lblVehiculo" runat="server" ></asp:Label>
                            </div>
                            <div class="col-lg-4 col-sm-4 text-left">
                                <asp:Label ID="Label23" runat="server" Text="Modelo:" CssClass="textoBold"></asp:Label>&nbsp;
                                <asp:Label ID="lblModelo" runat="server" ></asp:Label>
                            </div>
                            <div class="col-lg-4 col-sm-4 text-left">
                                <asp:Label ID="Label31" runat="server" Text="Placas:" CssClass="textoBold"></asp:Label>&nbsp;
                                <asp:Label ID="lblPlacas" runat="server" ></asp:Label>
                            </div>
                        </div>
                        <div class="row marTop">
                            <div class="col-lg-6 col-sm-6 text-left">
                                <asp:Label ID="Label5" runat="server" Text="Siniestro:" CssClass="textoBold"></asp:Label>&nbsp;
                                <asp:Label ID="lblSiniestro" runat="server" ></asp:Label>
                            </div>
                            <div class="col-lg-6 col-sm-6 text-left">
                                <asp:Label ID="Label9" runat="server" Text="Póliza:" CssClass="textoBold"></asp:Label>&nbsp;
                                <asp:Label ID="lblPoliza" runat="server" ></asp:Label>
                            </div>
                        </div>
                        <div class="row marTop">
                            <div class="col-lg-6 col-sm-6 text-left">
                                <asp:Label ID="Label18" runat="server" Text="Fecha Revisión:" CssClass="textoBold"></asp:Label>&nbsp;
                                <asp:TextBox ID="txtFecharevisionPop" runat="server" CssClass="alingMiddle input-small" Enabled="false"></asp:TextBox>
                                <cc1:CalendarExtender ID="txtFecharevisionPop_CalendarExtender" runat="server" BehaviorID="txtFecharevisionPop_CalendarExtender"
                                    TargetControlID="txtFecharevisionPop" Format="yyyy-MM-dd" PopupButtonID="lnkFecharevisionPop" />
                                <asp:LinkButton ID="lnkFecharevisionPop" runat="server" CssClass="btn btn-info t14"><i class="fa fa-calendar"></i></asp:LinkButton>
                            </div>
                            <div class="col-lg-6 col-sm-6 text-left">
                                <asp:Label ID="Label19" runat="server" Text="Política:" CssClass="textoBold"></asp:Label>&nbsp;
                                <asp:DropDownList ID="ddlPoliticaPagoPop" AutoPostBack="true" OnSelectedIndexChanged="ddlPoliticaPagoPop_SelectedIndexChanged" CssClass="input-medium" runat="server" DataSourceID="SqlDataSource3" DataTextField="descripcion" DataValueField="id_politica"></asp:DropDownList>
                                <asp:SqlDataSource runat="server" ID="SqlDataSource3" ConnectionString='<%$ ConnectionStrings:PVW %>' SelectCommand="select 0 as id_politica, 'Indique Politica' as descripcion union all select id_politica,descripcion+' ('+ltrim(rtrim(Convert(char(5),dias_plazo)))+')' as descripcion from politica_pago"></asp:SqlDataSource>
                            </div>
                        </div>
                        <div class="row marTop">
                            <div class="col-lg-6 col-sm-6 text-left">
                                <asp:Label ID="Label20" runat="server" Text="Fecha Prog. Pago:" CssClass="textoBold"></asp:Label>&nbsp;
                                <asp:TextBox ID="txtFechaProgPagopop" CssClass="input-small" Enabled="false" runat="server"></asp:TextBox>
                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" BehaviorID="txtFechaProgPagopop_CalendarExtender"
                                    TargetControlID="txtFechaProgPagopop" Format="yyyy-MM-dd" PopupButtonID="lnkFechaProgPagopop" />
                                <asp:LinkButton ID="lnkFechaProgPagopop" runat="server" CssClass="btn btn-info t14"><i class="fa fa-calendar"></i></asp:LinkButton>
                            </div>
                            <div class="col-lg-6 col-sm-6 text-left">
                                <asp:Label ID="Label7" runat="server" Text="Fecha Pago:" CssClass="textoBold"></asp:Label>&nbsp;
                                <asp:TextBox ID="txtFechaPagoPop" Enabled="false" CssClass="input-small" runat="server"></asp:TextBox>
                                <cc1:CalendarExtender ID="CalendarExtender2" runat="server" BehaviorID="txtFechaPagoPop_CalendarExtender"
                                    TargetControlID="txtFechaPagoPop" Format="yyyy-MM-dd" PopupButtonID="lnkFechaPagoPop" />
                                <asp:LinkButton ID="lnkFechaPagoPop" runat="server" CssClass="btn btn-info t14"><i class="fa fa-calendar"></i></asp:LinkButton>
                            </div>
                        </div>
                        <div class="row marTop">
                            <div class="col-lg-4 col-sm-4 text-left">
                                <asp:Label ID="Label14" runat="server" Text="Forma Pago:" CssClass="textoBold"></asp:Label>&nbsp;
                                <asp:DropDownList ID="ddlFormaPagopop" AutoPostBack="true" OnSelectedIndexChanged="ddlFormaPagopop_SelectedIndexChanged" CssClass="input-medium" runat="server">
                                    <asp:ListItem Text="Efectivo" Value="E" Selected="True" />
                                    <asp:ListItem Text="Cheque" Value="C"/>
                                    <asp:ListItem Text="T. Débito" Value="D" />
                                    <asp:ListItem Text="T. Crédito" Value="A" />
                                    <asp:ListItem Text="Transferencia" Value="B" />
                                </asp:DropDownList>
                            </div>
                            <div class="col-lg-4 col-sm-4 text-left">
                                <asp:Label ID="Label22" runat="server" Text="Referencia Pago:" CssClass="textoBold"></asp:Label>&nbsp;
                                <asp:TextBox ID="txtReferenciaPagPop" CssClass="input-medium" runat="server" placeholder="Referencia"></asp:TextBox>
                            </div>
                            <div class="col-lg-4 col-sm-4 text-left">
                                <asp:Label ID="Label26" runat="server" Text="Banco:" CssClass="textoBold"></asp:Label>&nbsp;
                                <asp:DropDownList ID="ddlBanco" CssClass="input-medium" runat="server" AppendDataBoundItems="true" DataSourceID="SqlDataSource2" DataTextField="Nombre_Banco" DataValueField="ClvBanco">
                                    <asp:ListItem Value="" Text="Seleccione Banco" />
                                </asp:DropDownList>
                                <asp:SqlDataSource runat="server" ID="SqlDataSource2" ConnectionString='<%$ ConnectionStrings:PVW %>' SelectCommand="SELECT [Clave_Banco], [Nombre_Banco] FROM [cat_bancos_f]"></asp:SqlDataSource>
                            </div>
                        </div>                        
                         <div class="row marTop">
                            <div class="col-lg-4 col-sm-4 text-left">
                                <asp:Label ID="Label11" runat="server" Text="Observaciones:" CssClass="textoBold"></asp:Label>&nbsp;
                                <asp:TextBox ID="txtObsevacionesPop" placeholder="Observaciones" runat="server" TextMode="MultiLine" CssClass="textNota" Rows="9" />
                            </div>
                            <div class="col-lg-8 col-sm-8 text-left">
                                <asp:Label ID="Label13" runat="server" Text="Nota de Crédito:" CssClass="textoBold"></asp:Label>&nbsp;
                                <asp:CheckBox ID="chkNota" runat="server" AutoPostBack="true"  Text="" OnCheckedChanged="chkNota_CheckedChanged" />
                                <asp:TextBox ID="txtNota" runat="server" CssClass="input-medium" placeholder="Nota de Crédito" MaxLength="10" />
                            </div>
                         </div> 
                        <div class="row marTop">
                            <div class="col-lg-12 col-sm-12 text-center">
                                <asp:LinkButton ID="lnkAgregarAnticipo" CssClass="btn btn-info" OnClick="lnkAgregarAnticipo_Click" runat="server" ><i class="fa fa-plus"></i><span>&nbsp;&nbsp;Agregar Anticipos</span></asp:LinkButton>
                            </div>
                            <div class="col-lg-12 col-sm-12 text-center">
                                <asp:Panel ID="pnlAnticipos" runat="server" Visible="false" CssClass="row marTop border">
                                    <div class="col-lg-4 col-sm-4">
                                        <asp:Label ID="Label15" runat="server" Text="Fecha Pago:" CssClass="textoBold"></asp:Label>&nbsp;
                                        <asp:TextBox ID="txtFechaPagoAnticipo" Enabled="false" CssClass="input-small" runat="server"></asp:TextBox>
                                        <cc1:CalendarExtender ID="CalendarExtender3" runat="server" BehaviorID="txtFechaPagoAnticipo_CalendarExtender"
                                            TargetControlID="txtFechaPagoAnticipo" Format="yyyy-MM-dd" PopupButtonID="lnkFechaPagoAnticipo" />
                                        <asp:LinkButton ID="lnkFechaPagoAnticipo" runat="server" CssClass="btn btn-info t14"><i class="fa fa-calendar"></i></asp:LinkButton>
                                    </div>
                                    <div class="col-lg-4 col-sm-4 text-left">
                                        <asp:Label ID="Label24" runat="server" Text="Forma Pago:" CssClass="textoBold"></asp:Label>&nbsp;
                                        <asp:DropDownList ID="ddlFormaPagoAnticipo" AutoPostBack="true" OnSelectedIndexChanged="ddlFormaPagoAnticipo_SelectedIndexChanged" CssClass="input-medium" runat="server">
                                            <asp:ListItem Text="Efectivo" Value="E" Selected="True" />
                                            <asp:ListItem Text="Cheque" Value="C"/>
                                            <asp:ListItem Text="T. Débito" Value="D" />
                                            <asp:ListItem Text="T. Crédito" Value="A" />
                                            <asp:ListItem Text="Transferencia" Value="B" />
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-lg-4 col-sm-4 text-left">
                                        <asp:Label ID="Label27" runat="server" Text="Referencia Pago:" CssClass="textoBold"></asp:Label>&nbsp;
                                        <asp:TextBox ID="txtReferenciaAnticipo" CssClass="input-medium" runat="server" placeholder="Referencia"></asp:TextBox>
                                    </div>
                                    <div class="col-lg-4 col-sm-4 text-left">
                                        <asp:Label ID="Label28" runat="server" Text="Banco:" CssClass="textoBold"></asp:Label>&nbsp;
                                        <asp:DropDownList ID="ddlBancoAnticipo" CssClass="input-medium" runat="server" AppendDataBoundItems="true" DataSourceID="SqlDataSource4" DataTextField="Nombre_Banco" DataValueField="ClvBanco">
                                            <asp:ListItem Value="" Text="Seleccione Banco" />
                                        </asp:DropDownList>
                                        <asp:SqlDataSource runat="server" ID="SqlDataSource4" ConnectionString='<%$ ConnectionStrings:PVW %>' SelectCommand="SELECT [ClvBanco], [Nombre_Banco] FROM [cat_bancos]"></asp:SqlDataSource>
                                    </div>
                                    <div class="col-lg-4 col-sm-4 text-left">
                                        <asp:Label ID="Label29" runat="server" Text="Monto:" CssClass="textoBold"></asp:Label>&nbsp;
                                        <telerik:RadNumericTextBox RenderMode="Lightweight" runat="server" ID="radMontoAnticipo" CssClass="input-mini" Value="0" EmptyMessage="Monto" MinValue="0" ShowSpinButtons="true" NumberFormat-DecimalDigits="2" Skin="MetroTouch"></telerik:RadNumericTextBox>
                                    </div>
                                    <div class="col-lg-4 col-sm-4">
                                        <asp:LinkButton ID="lnkAgregaPagoAnticipo" CssClass="btn btn-info" OnClick="lnkAgregaPagoAnticipo_Click" runat="server" ><i class="fa fa-check"></i><span>&nbsp;&nbsp;Agregar Pago</span></asp:LinkButton>
                                    </div>
                                </asp:Panel>
                            </div>
                        </div> 
                        <div class="row marTop">
                            <div class="col-lg-12 col-sm-12">
                                <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid4" runat="server" DataSourceID="SqlDataSource5"
                                    ShowFooter="true" EnableHeaderContextMenu="true" Skin="Metro" >
                                    <ClientSettings AllowKeyboardNavigation="true" EnablePostBackOnRowClick="false">
                                        <Selecting AllowRowSelect="true"></Selecting>
                                        <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true"></Scrolling>
                                    </ClientSettings>
                                    <MasterTableView DataKeyNames="Renglon,Factura,Folio" AutoGenerateColumns="False" >
                                        <Columns>                                
                                            <telerik:GridBoundColumn DataField="pago" HeaderText="#" ReadOnly="True" SortExpression="pago" UniqueName="pago"/>
                                            <telerik:GridBoundColumn DataField="FechaPago" DataFormatString="{0:yyyy-MM-dd}" HeaderText="Fecha Pago" ReadOnly="True" SortExpression="FechaPago" UniqueName="FechaPago"/>
                                            <telerik:GridBoundColumn DataField="FormaPago" HeaderText="Forma de Pago" ReadOnly="True" SortExpression="FormaPago" UniqueName="FormaPago"/>
                                            <telerik:GridBoundColumn DataField="ReferenciaPago" HeaderText="Referencia de Pago" ReadOnly="True" SortExpression="ReferenciaPago" UniqueName="ReferenciaPago"/>
                                            <telerik:GridBoundColumn DataField="nombre_banco" HeaderText="Banco" ReadOnly="True" SortExpression="nombre_banco" UniqueName="nombre_banco"/>
                                            <telerik:GridBoundColumn DataField="Importe" HeaderText="Importe" DataFormatString="{0:C2}" ReadOnly="True" SortExpression="Importe" UniqueName="Importe"/>
                                            <telerik:GridBoundColumn DataField="monto_pagado" HeaderText="Monto Pagado" DataFormatString="{0:C2}" ReadOnly="True" SortExpression="monto_pagado" UniqueName="monto_pagado"/>
                                            <telerik:GridBoundColumn DataField="monto_restante" HeaderText="Restante" DataFormatString="{0:C2}" ReadOnly="True" SortExpression="monto_restante" UniqueName="monto_restante"/>
                                        </Columns>
                                        <NoRecordsTemplate>
                                            <asp:Label ID="LabelEmpty3" runat="server" Text="No existe información de pagos anticipados" Font-Bold="True" Font-Size="0.9em" ForeColor="Red"></asp:Label>
                                        </NoRecordsTemplate>
                                    </MasterTableView>                        
                                </telerik:RadGrid>
                                <asp:SqlDataSource runat="server" ID="SqlDataSource5" ConnectionString='<%$ ConnectionStrings:PVW %>' SelectCommand="select d.Renglon,d.Factura,d.Pago,d.FechaPago,
                                case d.FormaPago when 'A' then 'T. Crédito' when 'B' then 'Transferencia' when 'C' then 'Cheque' when 'D' then 'T. Débito' when 'E' then 'Efectivo' else '' end as FormaPago,
                                d.ReferenciaPago,c.nombre_banco as banco,Importe,monto_pagado,monto_restante,d.Folio
                                from detallefacturas d
                                left join cat_bancos c on c.clvBanco=d.clvBanco
                                where renglon=@renglon and factura=@factura and tipocuenta='CC' and folio =@folio order by d.pago desc">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="lblRenglonFactura" Name="renglon" PropertyName="Text"/>
                                        <asp:ControlParameter ControlID="lblFactura" Name="factura" PropertyName="Text" />
                                        <asp:ControlParameter ControlID="lblOrden" Name="folio" PropertyName="Text" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </div>
                        </div>                                             
                         <div class="row marTop">
                            <div class="col-lg-6 col-sm-6">
                                &nbsp;
                            </div>                            
                            <div class="col-lg-6 col-sm-6 text-left alert-info">
                                <div class="row pad1m">
                                    <div class="col-lg-4 col-sm-4 text-left">
                                        <asp:Label ID="Label12" runat="server" Text="Importe:" CssClass="textoBold"></asp:Label>
                                    </div>
                                    <div class="col-lg-8 col-sm-8 text-right">
                                        <asp:Label ID="lblImporte" runat="server" ></asp:Label>
                                    </div>
                                </div>                                
                                <div class="row border-bottom">
                                    <div class="col-lg-4 col-sm-4 text-left">
                                        <asp:TextBox ID="txtPPP" runat="server" Text="P.P.P." CssClass="input-small" MaxLength="10" />
                                    </div>
                                    <div class="col-lg-4 col-sm-4 text-right">
                                        <telerik:RadNumericTextBox RenderMode="Lightweight" runat="server" ID="radDcto" CssClass="input-mini" AutoPostBack="true" OnTextChanged="radDcto_TextChanged" Value="0" EmptyMessage="Descuento" MinValue="0" MaxValue="100" ShowSpinButtons="true" NumberFormat-DecimalDigits="2" Skin="MetroTouch"></telerik:RadNumericTextBox>
                                    </div>
                                    <div class="col-lg-4 col-sm-4 text-right">
                                        <telerik:RadNumericTextBox RenderMode="Lightweight" runat="server" ID="radImpDcto" CssClass="input-mini" AutoPostBack="true" OnTextChanged="radImpDcto_TextChanged" Value="0" EmptyMessage="Importe Descuento" MinValue="0" MaxValue="9999999999.99" ShowSpinButtons="true" NumberFormat-DecimalDigits="2" Skin="MetroTouch"></telerik:RadNumericTextBox>
                                    </div>
                                </div>                                
                                <div class="row pad1m">
                                    <div class="col-lg-4 col-sm-4 text-left">
                                        <asp:Label ID="Label17" runat="server" Text="Monto Pagar:" CssClass="textoBold"></asp:Label>
                                    </div>
                                    <div class="col-lg-8 col-sm-8 text-right">
                                        <asp:Label ID="lblMontoPagar" runat="server" CssClass="textoBold"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-12 col-sm-12 text-center">
                                <asp:Label ID="lblErrorPopup" runat="server" CssClass="errores"></asp:Label>
                            </div>
                        </div>
                        <div class="row marTop">
                            <div class="col-lg-6 col-sm-6 text-center">                                
                                <asp:LinkButton ID="lnkGuardarPopup" OnClientClick="return confirm('¿Está seguro que los datos indicados son correctos?')" CssClass="btn btn-info" OnClick="lnkGuardarPopup_Click" runat="server"><i class="fa fa-save"></i><span>&nbsp;&nbsp;Guardar</span></asp:LinkButton>
                            </div>
                            <div class="col-lg-6 col-sm-6 text-center">
                                <asp:LinkButton ID="lnkSalirPop" OnClientClick="cierraWinCtrl()" CssClass="btn btn-danger" runat="server"><i class="fa fa-arrow-circle-left"></i><span>&nbsp;&nbsp;Salir</span></asp:LinkButton>
                            </div>
                        </div>                        
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
    </telerik:RadWindow>

</asp:Content>

