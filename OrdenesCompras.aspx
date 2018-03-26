<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrdenesCompras.aspx.cs" Inherits="OrdenesCompras" MasterPageFile="~/AdmonOrden.master" Culture="es-Mx" UICulture="es-Mx"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    <script type="text/javascript">
        function abreWinCtrl() {
            var oWnd = $find("<%=modalPopupControl.ClientID%>");
            oWnd.setUrl('');
            oWnd.show();
        }
        function cierraWinCtrl(ventana) {
            var oWnd = $find("<%=modalPopupControl.ClientID%>");
            oWnd.close();
        } s
    </script>

    <telerik:RadScriptManager ID="RadScriptManajer1" runat="server" EnableScriptGlobalization="true"></telerik:RadScriptManager>
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server"></telerik:RadSkinManager>
    
    <telerik:RadWindow RenderMode="Lightweight" ID="modalPopupControl" Title="Compras Sin Cotización" EnableShadow="true" Skin="Silk" 
        Behaviors="Close,Maximize,Move,Resize" VisibleOnPageLoad="false" ShowContentDuringLoad="false" DestroyOnClose="true" 
        Animation="Fade" runat="server" Modal="true" Width="1000px" Height="700px" Style="z-index: 1000;" >
        <ContentTemplate>
            <asp:UpdatePanel ID="UpdatePanelControl" runat="server" >
                <ContentTemplate>
                    <div class="col-lg-12 col-sm-12">
                        <div class="row">
                            <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label1" runat="server"  Text="Folio:"></asp:Label></div>
                            <div class="col-lg-4 col-sm-4 text-left">
                                <asp:TextBox ID="txtFolioCompraNew" runat="server" MaxLength="50" placeholder="Folio" CssClass="input-large"  AutoPostBack="true" ReadOnly="true"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"  ErrorMessage="Debe indicar el Folio de Compra" Text="*" CssClass="errores" ValidationGroup="agrega" ControlToValidate="txtFolioCompraNew"></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label3" runat="server" Text="Factura:"></asp:Label></div>
                            <div class="col-lg-4 col-sm-4 text-left">
                                <asp:TextBox ID="txtFacturaNew" runat="server"  MaxLength="50"  placeholder="Factura"  ReadOnly="true" AutoPostBack="true" CssClass="input-large" ></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Debe indicar la Factura" Text="*" CssClass="errores" ValidationGroup="agrega" ControlToValidate="txtFacturaNew"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label5" runat="server" Text="Proveedor:"></asp:Label></div>
                            <div class="col-lg-4 col-sm-4 text-left">
                                <telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="ddlProveedorNew" AllowCustomText="true" CssClass="input-large" Width="300px" MaxHeight="300px" DataSourceID="SqlDataSource2" DataTextField="razon_social" DataValueField="id_cliprov" Skin="MetroTouch" EmptyMessage="Seleccione Proveedor" Filter="Contains" ></telerik:RadComboBox>                                        
                                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select id_cliprov,razon_social from Cliprov where tipo='P' and estatus='A'"></asp:SqlDataSource>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Debe seleccionar el proveedor" Text="*" CssClass="errores" ValidationGroup="agrega" ControlToValidate="ddlProveedorNew"></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label7" runat="server" Text="Fecha Hora Entrega:"></asp:Label></div>
                            <div class="col-lg-4 col-sm-4 text-left">
                                <asp:TextBox ID="txtFechaRecepcion" runat="server" CssClass="input-small" MaxLength="10" Enabled="false" placeholder="Fecha"></asp:TextBox>
                                <cc1:CalendarExtender ID="CalendarExtender2" runat="server" BehaviorID="txtFechaRecepcion_CalendarExtender" TargetControlID="txtFechaRecepcion" Format="yyyy-MM-dd" PopupButtonID="lnkFechaRecepcion" />
                                <asp:LinkButton ID="lnkFechaRecepcion" runat="server" CssClass="btn btn-info t14"><i class="fa fa-calendar"></i></asp:LinkButton>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Debe indicar una fecha de entrega" ValidationGroup="agrega" CssClass="errores" Text="*" ControlToValidate="txtFechaRecepcion" ></asp:RequiredFieldValidator>
                                <telerik:RadTimePicker RenderMode="Lightweight" ID="RadTimeFechaRecepcion" Culture="es-Mx" DateInput-DateFormat="HH:mm:ss" DateInput-EmptyMessage="Hora" Skin="MetroTouch" runat="server"></telerik:RadTimePicker>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Debe indicar una hora de entrega" ValidationGroup="agrega" CssClass="errores" Text="*" ControlToValidate="RadTimeFechaRecepcion" ></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label9" runat="server" Text="Nombre Entrega:"></asp:Label></div>
                            <div class="col-lg-8 col-sm-8 text-left">
                                <asp:TextBox ID="txtNombreNew" runat="server" MaxLength="300" placeholder="Nombre Entrega" CssClass="input-xlarge" ></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Debe indicar el nombre de quien entrega las refacciones" ValidationGroup="agrega" CssClass="errores" Text="*" ControlToValidate="txtNombreNew" ></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-lg-2 col-sm-2 text-left">
                                <asp:LinkButton ID="lnkAceptarEntrada" runat="server" CssClass="btn btn-success" ValidationGroup="agrega" OnClick="lnkAceptarEntrada_Click"><i class="fa fa-plus-circle"></i><span>&nbsp;Aceptar Entrega</span></asp:LinkButton>
                            </div>
                        </div>
                        <div class="row pad1m">
                            <div class="col-lg-12 col-sm-12 text-center">
                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="errores" ValidationGroup="agrega" DisplayMode="List"  />
                            </div>
                            <div class="col-lg-12 col-sm-12 text-center">
                                <asp:Label ID="lblErrorNew" runat="server" CssClass="errores" />
                            </div>
                        </div>
                        <div class="col-lg-12 col-sm-12 text-center alert-info pad1m t14"><asp:Label ID="Label20" runat="server" Text="Captura de Refacciones"></asp:Label></div>
                        <asp:Panel ID="pnlDetalle" runat="server" CssClass="row">
                            <asp:Label ID="lblEntrada" runat="server" Visible="false"></asp:Label>
                            <asp:Label ID="lblDetalle" runat="server" Visible="false"></asp:Label>

                            <div class="col-lg-12 col-sm-12">
                                <div class="row">
                                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label13" runat="server" Text="Refacción:"></asp:Label></div>
                                    <div class="col-lg-4 col-sm-4 text-left">
                                        <telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="ddlRefaccionMod" AllowCustomText="true" CssClass="input-xlarge" Width="300px" MaxHeight="300px" DataSourceID="SqlDataSource4" Skin="MetroTouch" DataTextField="refDescripcion" DataValueField="refOrd_id" EmptyMessage="Indique" Filter="Contains" AutoPostBack="true" OnSelectedIndexChanged="ddlRefaccionMod_SelectedIndexChanged" OnTextChanged="ddlRefaccionMod_TextChanged" ></telerik:RadComboBox>                                        
                                        <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select refOrd_id,refDescripcion from refacciones_orden where ref_no_orden=@orden and ref_id_empresa=@empresa and ref_id_taller=@taller and refEstatus in('CO','AU') and refEstatusSolicitud=1 and proceso is null">
                                            <SelectParameters>
                                                <asp:QueryStringParameter Name="orden" QueryStringField="o" Type="Int32" DefaultValue="0" />
                                                <asp:QueryStringParameter Name="empresa" QueryStringField="e" Type="Int32" DefaultValue="0" />
                                                <asp:QueryStringParameter Name="taller" QueryStringField="t" Type="Int32" DefaultValue="0" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Debe seleccionar la refacción" Text="*" CssClass="errores" ValidationGroup="mod" ControlToValidate="ddlRefaccionMod"></asp:RequiredFieldValidator>                                        
                                    </div>
                                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label14" runat="server" Text="Cantidad:"></asp:Label></div>
                                    <div class="col-lg-4 col-sm-4 text-left">
                                        <telerik:RadNumericTextBox ID="TxtCantidadMod" runat="server" MinValue="0" MaxValue="127" NumberFormat-DecimalDigits="0" ShowSpinButtons="false" Skin="MetroTouch" EmptyMessage="0" OnTextChanged="TxtCantidadMod_TextChanged"  AutoPostBack="true"></telerik:RadNumericTextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="Debe indicar la cantidad" Text="*" CssClass="errores" ValidationGroup="mod" ControlToValidate="TxtCantidadMod"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label15" runat="server" Text="Costo Unitario:"></asp:Label></div>
                                    <div class="col-lg-4 col-sm-4 text-left">
                                        <telerik:RadNumericTextBox ID="txtCostoUnitarioMod" runat="server" MinValue="0" MaxValue="9999999999.99" NumberFormat-DecimalDigits="2" ShowSpinButtons="false" EmptyMessage="0.00" Skin="MetroTouch" OnTextChanged="txtCostoUnitarioMod_TextChanged" AutoPostBack="true"></telerik:RadNumericTextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="Debe indicar el costo unitario" Text="*" CssClass="errores" ValidationGroup="mod" ControlToValidate="txtCostoUnitarioMod"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label16" runat="server" Text="Porcentaje Descuento:"></asp:Label></div>
                                    <div class="col-lg-4 col-sm-4 text-left">
                                        <telerik:RadNumericTextBox ID="txtPorcDesc" runat="server" MinValue="0" MaxValue="100" NumberFormat-DecimalDigits="2"  ShowSpinButtons="false" Skin="MetroTouch" EmptyMessage="0.00" OnTextChanged="txtPorcDesc_TextChanged" AutoPostBack="true"></telerik:RadNumericTextBox>
                                    </div>
                                </div>
                                <div class="row marTop">
                                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label17" runat="server" Text="Importe Descuento:"></asp:Label></div>
                                    <div class="col-lg-4 col-sm-4 text-left"><asp:Label ID="lblImporteDescMod" runat="server" ></asp:Label></div>
                                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label18" runat="server" Text="Importe:"></asp:Label></div>
                                    <div class="col-lg-4 col-sm-4 text-left"><asp:Label ID="lblImporteMod" runat="server" ></asp:Label></div>
                                </div>
                                <div class="row marTop">
                                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label19" runat="server" Text="Procedencia:"></asp:Label></div>
                                    <div class="col-lg-4 col-sm-4 text-left">                                        
                                        <asp:DropDownList ID="ddlProcedenciaMod" runat="server" DataTextField="proc_Descrip" DataSourceID="SqlDataSource5" CssClass="input-large" DataValueField="id_Proc"></asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select 0 as id_Proc,'Seleccione' as proc_Descrip union all select id_Proc,proc_Descrip from cat_Procedencia order by 1"></asp:SqlDataSource>
                                    </div>
                                </div>
                                <div class="row marTop">
                                    <div class="col-lg-12 col-sm-12 text-center">                            
                                        <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="mod" CssClass="errores " DisplayMode="List" />
                                        <asp:Label ID="lblErrorRef" runat="server" CssClass="errores" />
                                    </div>
                                </div>
                                <div class="row marTop">
                                    <div class="col-lg-4 col-sm-4 text-center"></div>
                                    <div class="col-lg-2 col-sm-2 text-center"><asp:LinkButton ID="lnkAceptar" runat="server" CssClass="btn btn-success t14" ValidationGroup="mod" OnClick="lnkAceptar_Click" CommandArgument=""><i class="fa fa-save"></i><span>&nbsp;Guardar</span></asp:LinkButton></div>
                                    <div class="col-lg-2 col-sm-2 text-center"><asp:LinkButton ID="lnkCancelar" runat="server" CssClass="btn btn-danger t14" OnClick="lnkCancelar_Click1"><i class="fa fa-remove"></i><span>&nbsp; Cancelar</span></asp:LinkButton></div>
                                    <div class="col-lg-4 col-sm-4 text-center"></div>
                                </div>
                            </div>                    

                            <div class="row">
                                <div class="col-lg-12 col-sm-12">

                            <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" runat="server" CssClass="RadGrid" GridLines="None" AllowPaging="True" PageSize="100" AllowSorting="True" AutoGenerateColumns="False" ShowFooter="true"
                                ShowStatusBar="false" AllowAutomaticDeletes="false" AllowAutomaticInserts="false"  OnItemDataBound="RadGrid1_ItemDataBound" Skin="Metro" OnSelectedIndexChanged="RadGrid1_SelectedIndexChanged" AllowAutomaticUpdates="false" DataSourceID="SqlDataSource3" >
                                <MasterTableView CommandItemDisplay="None" DataSourceID="SqlDataSource3" DataKeyNames="id_detalle" >
                                    <Columns> 
                                        <telerik:GridBoundColumn UniqueName="cantidad" HeaderStyle-Width="100px" HeaderText="Cantidad" DataField="cantidad" ReadOnly="true"/>
                                        <telerik:GridBoundColumn UniqueName="descripcion" HeaderText="Refacción" DataField="descripcion" ReadOnly="true"/>
                                        <telerik:GridBoundColumn UniqueName="costo_unitario" HeaderStyle-Width="100px" HeaderText="C.U." DataField="costo_unitario" ReadOnly="true"/>
                                        <telerik:GridBoundColumn UniqueName="porc_desc" HeaderStyle-Width="100px" HeaderText="% Dto." DataField="porc_desc" ReadOnly="true"/>
                                        <telerik:GridBoundColumn UniqueName="importe_desc" HeaderStyle-Width="100px" HeaderText="Descuento" DataField="importe_desc" ReadOnly="true"/>
                                        <telerik:GridBoundColumn UniqueName="importe" HeaderStyle-Width="100px" HeaderText="Importe" DataField="importe" Aggregate="Sum" ReadOnly="true"/> 
                                        
                                        <telerik:GridTemplateColumn HeaderStyle-Width="50px">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkEditar" runat="server" CausesValidation="False" ToolTip="Editar" CssClass="btn btn-warning t14" OnClick="lnkEditar_Click" CommandArgument='<%# Eval("id_detalle")+";"+Eval("id_refaccion") %>'><i class="fa fa-edit"></i></asp:LinkButton>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderStyle-Width="50px">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkEliminar" runat="server" CausesValidation="False" ToolTip="Eliminar" CssClass="btn btn-danger t14" OnClick="lnkEliminar_Click" CommandArgument='<%# Eval("id_detalle")+";"+Eval("id_refaccion") %>' OnClientClick="return confirm('¿Está seguro de eliminar el registro?')"><i class="fa fa-trash"></i></asp:LinkButton>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                    <NoRecordsTemplate>
                                        <asp:Label ID="Label11" runat="server" Text="No existen refacciones ingresadas" CssClass="errores"></asp:Label>
                                    </NoRecordsTemplate>                                    
                                </MasterTableView>
                                <ClientSettings>
                                    <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true"></Scrolling>                                    
                                </ClientSettings>
                                <PagerStyle PageSizeControlType="RadDropDownList" Mode="NextPrevAndNumeric"></PagerStyle>
                            </telerik:RadGrid>
                            <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="SELECT * FROM orden_compra_detalle where no_orden=@orden and id_empresa=@empresa and id_taller=@taller and id_orden=@ordenCompra" >
                                <SelectParameters>
                                    <asp:QueryStringParameter Name="orden" QueryStringField="o" Type="Int32" DefaultValue="0" />
                                    <asp:QueryStringParameter Name="empresa" QueryStringField="e" Type="Int32" DefaultValue="0" />
                                    <asp:QueryStringParameter Name="taller" QueryStringField="t" Type="Int32" DefaultValue="0" />
                                    <asp:ControlParameter Name="ordenCompra" ControlID="lblEntrada" Type="Int32" PropertyName="Text" DefaultValue="0" />
                                </SelectParameters>
                            </asp:SqlDataSource>

                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                     <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanelControl">
                        <ProgressTemplate>
                            <asp:Panel ID="pnlMaskLoad" runat="server" CssClass="maskLoad"></asp:Panel>
                            <asp:Panel ID="pnlCargando" runat="server" CssClass="pnlPopUpLoad">
                                <asp:Image ID="imgLoad" runat="server" ImageUrl="~/img/loading.gif" CssClass="ancho100" />
                            </asp:Panel>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
    </telerik:RadWindow>



    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-lg-12 col-sm-12 text-center alert-info">
                    <h3>
                        <i class="fa fa-list"></i></i>&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblTit" runat="server" Text="Ordenes de Compra"></asp:Label>
                    </h3>
                </div>
            </div>
             <div class="row">
                <div class="col-lg-12 col-sm-12 text-right">
                    <asp:LinkButton ID="lnkCompraNoRegistrada" runat="server" CssClass="btn btn-info" OnClick="lnkCompraNoRegistrada_Click"><i class="fa fa-shopping-cart"></i><span>&nbsp;Compra sin Cotización</span></asp:LinkButton>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-12 col-sm-12 text-center">                    
                    <asp:Label ID="lblError" runat="server" CssClass="errores" />
                </div>
            </div>

            <asp:Panel runat="server" ID="Panel1" CssClass="paneles col-lg-12 col-sm-12" ScrollBars="Auto">
                <asp:Panel ID="PnlCotizaciones" runat="server" CssClass="col-lg-12 col-sm-12">
                    <div class="table-responsive">
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered" 
                            DataSourceID="SqlDataSource1" AllowPaging="false" AllowSorting="True" 
                            PageSize="7" onrowdatabound="GridView1_RowDataBound">
                            <Columns>                                
                                <asp:BoundField DataField="id_orden" HeaderText="id_cotizacion" SortExpression="id_orden" Visible="false" />
                                <asp:BoundField DataField="folio_orden" HeaderText="Folio" SortExpression="folio_orden" />
                                <asp:BoundField DataField="estatus" HeaderText="estatus" SortExpression="estatus" Visible="false" />                                
                                <asp:BoundField DataField="fecha" HeaderText="Fecha" ReadOnly="True" SortExpression="fecha" />
                                <asp:BoundField DataField="razon_social" HeaderText="Proveedor" ReadOnly="True" SortExpression="razon_social" />
                                <asp:BoundField DataField="total_orden" HeaderText="Importe" ReadOnly="True" SortExpression="total_orden" DataFormatString="{0:C2}" />
                                <asp:BoundField DataField="valorEstatus" HeaderText="Estatus" ReadOnly="True" SortExpression="valorEstatus" />
                                <asp:BoundField DataField="origenCompra" HeaderText="Origen Compra" ReadOnly="True" SortExpression="origenCompra" />
                                <asp:TemplateField ShowHeader="False" >
                                    <ItemTemplate>
                                    <asp:LinkButton ID="lnkAgregarMasRefacciones"  CommandArgument='<%# Eval("id_orden")+";"+Eval("folio_orden")+";"+Eval("estatus")+";"+Eval("id_cliprov") %>' OnClick="lnkAgregarMasRefacciones_Click" runat="server" CssClass="btn btn-warning" ><i class="fa fa-plus"></i></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkSeleccionar" runat="server" CausesValidation="False" CommandArgument='<%# Eval("id_orden")+";"+Eval("folio_orden")+";"+Eval("estatus")+";"+Eval("id_cliprov") %>'
                                            CommandName="Select" ToolTip="Seleccionar" CssClass="btn btn-success" 
                                            onclick="lnkSeleccionar_Click"><i class="fa fa-check-circle"></i></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField ShowHeader="False" Visible="true">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEliminar" runat="server" CausesValidation="False" CommandArgument='<%# Eval("id_orden") %>'
                                            CommandName="Eliminar" ToolTip="Eliminar" CssClass="btn btn-danger t14"  OnClientClick="return confirm('¿Está seguro de eliminar la orden de compra?')"
                                            onclick="lnkEliminar_Click1"><i class="fa fa-trash"></i></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:PVW %>" 
                        SelectCommand="select o.id_orden,o.folio_orden,o.estatus,
                                            case o.estatus when 'PEN' then 'Pendiente' 
                                                            when 'CAN' then 'Cancelada'
                                                            when 'CON' then 'Concluida' 
                                                            when 'REC' then 'Entregada' 
                                                            when 'SOL' then 'Solicitado' else '' end as valorEstatus,
                                            convert(char(10),o.fecha,126)+' '+convert(char(8),o.hora,108) as fecha,
                                            o.total_orden,
                                            o.id_cliprov,
                                            C.razon_social,
	                                        case o.folioCotizacion when 'S/C' then 'M' else 'S' end as origenCompra
                                            from Orden_Compra_Encabezado o 
                                                    inner join Cliprov c on c.id_cliprov = o.id_cliprov and c.tipo='P'
                                                        where o.id_empresa=@empresa 
                                                            and o.id_taller=@taller 
                                                            and o.no_orden=@orden order by o.id_orden desc
">
                        <SelectParameters>
                            <asp:QueryStringParameter DefaultValue="0" Name="empresa" QueryStringField="e" />
                            <asp:QueryStringParameter DefaultValue="0" Name="taller" QueryStringField="t" />
                            <asp:QueryStringParameter DefaultValue="0" Name="orden" QueryStringField="o" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </asp:Panel>
            </asp:Panel>

            <div class="pie pad1m">		                                		                                
		        <div class="clearfix">
			        <div class="row colorBlanco textoBold">
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label2" runat="server" Text="Tipo Orden:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="ddlToOrden" runat="server" ></asp:Label>
                        </div>
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label4" runat="server" Text="Cliente:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="ddlClienteOrden" runat="server" ></asp:Label>
                        </div>
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label6" runat="server" Text="Tipo Servicio:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="ddlTsOrden" runat="server" ></asp:Label>
                        </div>
                    </div>                                              
                    <div class="row colorBlanco textoBold">
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label8" runat="server" Text="Tipo Valuación:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="ddlValOrden" runat="server" ></asp:Label>
                        </div>
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label10" runat="server" Text="Tipo Asegurado:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="ddlTaOrden" runat="server" ></asp:Label>
                        </div>
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label12" runat="server" Text="Localización:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="ddlLocOrden" runat="server" ></asp:Label>
                        </div>
                    </div>    
                    <div class="row colorBlanco textoBold">
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label108" runat="server" Text="Perfil:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="ddlPerfil" runat="server" ></asp:Label>
                        </div>
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label109" runat="server" Text="Siniestro:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="lblSiniestro" runat="server" ></asp:Label>
                        </div>
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label110" runat="server" Text="Deducible:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="lblDeducible" runat="server" ></asp:Label>
                        </div>
                    </div>  
                    <div class="row colorBlanco textoBold">
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label111" runat="server" Text="Total Orden:" CssClass="colorEtiqueta" Visible="false"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="lblTotOrden" runat="server" Visible="false" ></asp:Label>
                        </div>  
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label112" runat="server" Text="Promesa:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="lblEntregaEstimada" runat="server" ></asp:Label>
                        </div>
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="lblPorcDeduEti" runat="server" Text="% Deducible:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="lblPorcDedu" runat="server" ></asp:Label>
                        </div>                      
                    </div>
		        </div>
            </div>

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