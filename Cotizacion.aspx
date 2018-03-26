<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Cotizacion.aspx.cs" Inherits="Cotizacion" MasterPageFile="~/AdmonOrden.master" Culture="es-Mx" UICulture="es-Mx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function abreWinComp() {
            var oWnd = $find("<%=modalPopupComparativo.ClientID%>");
            oWnd.setUrl('');
            oWnd.show();
        }
        function abreWinAuto() {
            var oWnd = $find("<%=radAutorizacion.ClientID%>");
            oWnd.setUrl('');
            oWnd.show();
        }

        function cierraWinAuto() {
            var oWnd = $find("<%=radAutorizacion.ClientID%>");
            oWnd.close();
        }

        function cierraWinComp() {
            var oWnd = $find("<%=modalPopupComparativo.ClientID%>");
            oWnd.close();
        }
    </script>

    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" />

    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGridCot">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGridCot"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="RadGridRefacciones" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="radEnviados" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>                    
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGridRefacciones">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGridRefacciones"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="radEnviados" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>                    
                </UpdatedControls>
            </telerik:AjaxSetting>     
             <telerik:AjaxSetting AjaxControlID="radEnviados">
                <UpdatedControls>                    
                    <telerik:AjaxUpdatedControl ControlID="radEnviados" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>                    
                </UpdatedControls>
            </telerik:AjaxSetting>           
            <telerik:AjaxSetting AjaxControlID="pnlNuevoProveedorCot">
                <UpdatedControls>                    
                    <telerik:AjaxUpdatedControl ControlID="pnlNuevoProveedorCot" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>                                         
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="MetroTouch"></telerik:RadAjaxLoadingPanel>
    <div class="row">
        <div class="col-lg-12 col-sm-12 text-center alert-info">
            <h3>
                <i class="fa fa-dollar"></i>&nbsp;&nbsp;&nbsp;
                <asp:Label ID="lblTit" runat="server" Text="Cotización"></asp:Label>
            </h3>
        </div>
    </div>
    
             <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-lg-12 col-sm-12 text-center">
                    <asp:Label ID="lblError" runat="server" CssClass="errores" />
                </div>
            </div>

            <asp:Panel runat="server" ID="pnlTrad">
                <div class="col-lg-2 col-sm-2 text-left">
                    <asp:Label ID="Label1" runat="server" Text="Proveedor:" />
                </div>
                <div class="col-lg-2 col-sm-2 text-center">
                    <asp:DropDownList ID="ddlProveedores" CssClass="input-large" runat="server" DataSourceID="SqlDataSource2" DataTextField="razon_social" DataValueField="id_cliprov"></asp:DropDownList>
                    <asp:SqlDataSource runat="server" ID="SqlDataSource2" ConnectionString='<%$ ConnectionStrings:PVW %>'
                        SelectCommand="select razon_social,id_cliprov from cliprov where tipo='P' and estatus='A'"></asp:SqlDataSource>
                </div>
                <div class="col-lg-2 col-sm-2 text-center">
                    <asp:CheckBox ID="chkCambiaMontos" runat="server" Checked="false" AutoPostBack="true" Text="&nbsp;Cambiar Montos" OnCheckedChanged="chkCambiaMontos_CheckedChanged" />
                </div>
                <div class="col-lg-2 col-sm-2 text-center">
                    <asp:Label ID="Label3" runat="server" Text="Costo:" />
                </div>
                <div class="col-lg-2 col-sm-2 text-center">
                    <asp:TextBox ID="txtCosto" runat="server" placeholder="Costo" />
                </div>
                <div class="col-lg-2 col-sm-2 text-center">
                    <asp:LinkButton ID="lnkActualizarCosto" runat="server" CssClass="btn btn-success" OnClick="lnkActualizarCosto_Click">Actualizar Costo</asp:LinkButton>
                </div>
            </asp:Panel>
            <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="UpdatePanel3">
                <ProgressTemplate>
                    <asp:Panel ID="pnlMaskLoad3" runat="server" CssClass="maskLoad"></asp:Panel>
                    <asp:Panel ID="pnlCargando3" runat="server" CssClass="pnlPopUpLoad">
                        <asp:Image ID="imgLoad3" runat="server" ImageUrl="~/img/loading.gif" CssClass="ancho100" />
                    </asp:Panel>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </ContentTemplate>
    </asp:UpdatePanel>

            <asp:Panel runat="server" ID="Panel1" CssClass="col-lg-12 col-sm-12">
                <asp:Panel ID="PnlCotizaciones" runat="server" >
                    <div class="row">
                    <div class="col-lg-6 col-sm-6">
                        <telerik:RadGrid RenderMode="Lightweight" ID="RadGridCot" runat="server"  Culture="es-Mx" Skin="Metro" AutoGenerateColumns="False" AllowPaging="True" PagerStyle-AlwaysVisible="true" DataSourceID="SqlDataSource1" AllowSorting="true" GroupingEnabled="false" PageSize="20" MasterTableView-Caption="Cotizaciones" OnItemDataBound="RadGridCot_ItemDataBound">
                        <MasterTableView DataSourceID="SqlDataSource1" AutoGenerateColumns="False" DataKeyNames="id_cotizacion" >                            
                            <Columns>
                                <telerik:GridBoundColumn UniqueName="id_cotizacion" HeaderText="id_cotizacion" SortExpression="id_cotizacion" Visible="false" DataField="id_cotizacion" ReadOnly="true"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn UniqueName="folio" HeaderText="Folio" SortExpression="folio" DataField="folio" ></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn UniqueName="estatus" HeaderText="estatus" SortExpression="estatus" DataField="estatus" Visible="false"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn UniqueName="valorEstatus" HeaderText="Estatus" SortExpression="valorEstatus" DataField="valorEstatus" ></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn UniqueName="fecha" HeaderText="Fecha" SortExpression="fecha" DataField="fecha" ></telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn HeaderText="Refacciones">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRefacciones" runat="server" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>                                
                                <telerik:GridTemplateColumn>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkCancelar" runat="server" CausesValidation="False" CommandArgument='<%# Eval("id_cotizacion") %>'
                                            CommandName="Cancel" ToolTip="Cancelar" CssClass="btn btn-danger" OnClientClick="return confirm('¿Está seguro de cancelar la cotización?')"
                                            OnClick="lnkCancelar_Click"><i class="fa fa-ban"></i></asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>                       
                            </Columns>                            
                            <NoRecordsTemplate>
                                <asp:Label runat="server" ID="lblVacio" Text="No existen cotizaciones registradas" CssClass="errores"></asp:Label>
                            </NoRecordsTemplate>
                        </MasterTableView>                        
                        <ClientSettings AllowKeyboardNavigation="true" EnablePostBackOnRowClick="true">
                            <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" ></Scrolling>
                            <Selecting AllowRowSelect="true" /> 
                        </ClientSettings>                        
                        <PagerStyle PageSizeControlType="RadDropDownList" Mode="NextPrevAndNumeric"></PagerStyle>
                    </telerik:RadGrid>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                            ConnectionString="<%$ ConnectionStrings:PVW %>"
                            SelectCommand="select id_cotizacion,folio,estatus,case estatus when 'PE' then 'Pendiente' when 'CA' then 'Cancelada' when 'AU' then 'Autorizada' when 'CO' then 'Concluida' else '' end as valorEstatus,convert(char(10),fecha,120) as fecha from Cotizacion_Encabezado where id_empresa=@empresa and id_taller=@taller and no_orden=@orden order by id_cotizacion desc">
                            <SelectParameters>
                                <asp:QueryStringParameter DefaultValue="0" Name="empresa" QueryStringField="e" />
                                <asp:QueryStringParameter DefaultValue="0" Name="taller" QueryStringField="t" />
                                <asp:QueryStringParameter DefaultValue="0" Name="orden" QueryStringField="o" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                    <div class="col-lg-6 col-sm-6">
                         <telerik:RadGrid RenderMode="Lightweight" ID="RadGridRefacciones" runat="server"  Culture="es-Mx" Skin="Metro" AutoGenerateColumns="False" AllowPaging="True" PagerStyle-AlwaysVisible="true" DataSourceID="SqlDataSource4" AllowSorting="true" GroupingEnabled="false" PageSize="500" MasterTableView-Caption="Refacciones Cotización">
                            <MasterTableView DataSourceID="SqlDataSource4" AutoGenerateColumns="False" >                            
                                <Columns>
                                    <telerik:GridBoundColumn UniqueName="id_cotizacion_detalle" HeaderText="id_cotizacion_detalle" SortExpression="id_cotizacion_detalle" Visible="false" DataField="id_cotizacion_detalle" ReadOnly="true"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="cantidad" HeaderText="Cantidad" SortExpression="cantidad" DataField="cantidad" ></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="descripcion" HeaderText="Refacción" SortExpression="descripcion" DataField="descripcion"></telerik:GridBoundColumn>                                    
                                </Columns>                            
                                <NoRecordsTemplate>
                                    <asp:Label runat="server" ID="lblVacio" Text="No existen refacciones registradas en la cotizacion" CssClass="errores"></asp:Label>
                                </NoRecordsTemplate>
                            </MasterTableView>                        
                            <ClientSettings AllowKeyboardNavigation="true">
                                <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" ></Scrolling>
                                <Selecting AllowRowSelect="true" /> 
                            </ClientSettings>                        
                            <PagerStyle PageSizeControlType="RadDropDownList" Mode="NextPrevAndNumeric"></PagerStyle>
                        </telerik:RadGrid>
                        <asp:SqlDataSource ID="SqlDataSource4" runat="server"
                            ConnectionString="<%$ ConnectionStrings:PVW %>"
                            SelectCommand="select distinct id_cotizacion_detalle,cantidad,descripcion from cotizacion_detalle where no_orden=@orden and id_taller=@taller and id_empresa=@empresa and id_cotizacion=@id_cotizacion">
                            <SelectParameters>
                                <asp:QueryStringParameter DefaultValue="0" Name="empresa" QueryStringField="e" />
                                <asp:QueryStringParameter DefaultValue="0" Name="taller" QueryStringField="t" />
                                <asp:QueryStringParameter DefaultValue="0" Name="orden" QueryStringField="o" />
                                <asp:ControlParameter ControlID="RadGridCot" Name="id_cotizacion" DefaultValue="0" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                        </div>
                    <div class="row pad1m">
                    
                    <div class="col-lg-12 col-sm-12 text-center">
                        <asp:LinkButton ID="lnkSeleccionar" runat="server" CausesValidation="False" 
                                            ToolTip="Seleccionar" CssClass="btn btn-success"
                                            OnClick="lnkSeleccionar_Click"><i class="fa fa-check-circle"></i><span>&nbsp;Acceder a Cotización</span></asp:LinkButton>
                    </div>
                        </div>
                    <div class="row marTop">
                        
                        <div class="col-lg-6 col-sm-6" >
                            <asp:Panel ID="pnlNuevoProveedorCot" Visible="false" runat="server" CssClass="row">
                                <div class="col-lg-12 col-lg-12 text-center alert-info t14"><asp:Label ID="Label9" runat="server" Text="Agregar Proveedor a la Cotizacion"></asp:Label></div>
                                <div class="col-lg-12 col-lg-12 text-center"><asp:Label ID="lblErrorNuevoPorveedor" runat="server" CssClass="errores"></asp:Label></div>
                                <div class="col-lg-2 col-sm-2 text-left">
                                    <asp:Label ID="Label11" runat="server" Text="Proveedor:"></asp:Label>
                                </div>
                                <div class="col-lg-6 col-sm-6 text-left">
                                     <telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="ddlPrveedor" AllowCustomText="true" CssClass="input-large" Width="300px" MaxHeight="300px" DataSourceID="SqlDataSource6" DataTextField="razon_social" DataValueField="id_cliprov" Skin="Metro" EmptyMessage="Seleccione Cliente" Filter="Contains" ></telerik:RadComboBox>                                        
                                        <asp:SqlDataSource ID="SqlDataSource6" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select id_cliprov,razon_social from Cliprov where tipo='P' and aseguradora=1 and id_cliprov<>0 and (correo<>'' or not correo is null) and id_cliprov not in (select distinct id_cliprov from cotizacion_detalle where no_orden=@orden and id_empresa=@empresa and id_taller=@taller and id_cotizacion=@id_cotizacion)">
                                            <SelectParameters>
                                                <asp:QueryStringParameter DefaultValue="0" Name="empresa" QueryStringField="e" />
                                                <asp:QueryStringParameter DefaultValue="0" Name="taller" QueryStringField="t" />
                                                <asp:QueryStringParameter DefaultValue="0" Name="orden" QueryStringField="o" />
                                                <asp:ControlParameter ControlID="RadGridCot" Name="id_cotizacion" DefaultValue="0" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                </div>
                                <div class="col-lg-4 col-sm-4">
                                    <asp:LinkButton ID="lnkAgregarCotización" runat="server" ToolTip="Agregar a Cotización" CssClass="btn btn-info"
                                        OnClick="lnkAgregarCotización_Click"><i class="fa fa-plus"></i><span>&nbsp;Agregar a Cotizaci&oacute;n</span></asp:LinkButton>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                    <div class="row center">
                            <telerik:RadGrid RenderMode="Lightweight" ID="radEnviados" runat="server"  Culture="es-Mx" Skin="Metro" AllowAutomaticUpdates="True" AutoGenerateColumns="False" AllowAutomaticInserts="false" AllowAutomaticDeletes="false" AllowPaging="True" PagerStyle-AlwaysVisible="true" DataSourceID="SqlDataSource5" AllowSorting="true" GroupingEnabled="false" PageSize="20" MasterTableView-Caption="Envios de Cotización">
                        <MasterTableView DataSourceID="SqlDataSource5" AutoGenerateColumns="False" DataKeyNames="no_orden,id_empresa,id_taller,id_cotizacion,id_cliprov"  CommandItemDisplay="Bottom" HorizontalAlign="NotSet" EditMode="Batch">
                            <BatchEditingSettings EditType="Row" />
                            <CommandItemStyle CssClass="text-right" />
                            <CommandItemSettings SaveChangesText="Guardar Cambios" ShowAddNewRecordButton="false"  ShowRefreshButton="false" ShowSaveChangesButton="true" CancelChangesText="Cancelar Cambios"/>
                            <Columns>
                                <telerik:GridBoundColumn UniqueName="id_cotizacion" HeaderText="id_cotizacion" SortExpression="id_cotizacion" Visible="false" DataField="id_cotizacion" ReadOnly="true"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn UniqueName="id_cliprov" HeaderText="id_cliprov" SortExpression="id_cliprov" Visible="false" DataField="id_cliprov" ReadOnly="true"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn UniqueName="razon_social" HeaderText="Proveedor" SortExpression="razon_social" DataField="razon_social" ReadOnly="true"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn UniqueName="fecha" HeaderText="Fecha" SortExpression="fecha" DataField="fecha" ReadOnly="true"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn UniqueName="hora" HeaderText="Hora" SortExpression="hora" DataField="hora" ReadOnly="true"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn UniqueName="correo" HeaderText="Correo" SortExpression="correo" DataField="correo"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn UniqueName="nombre_usuario" HeaderText="Usuario" SortExpression="nombre_usuario" DataField="nombre_usuario" ReadOnly="true"></telerik:GridBoundColumn>
                                <telerik:GridCheckBoxColumn UniqueName="enviado" HeaderText="Enviado" SortExpression="enviado" DataField="enviado" ReadOnly="true"></telerik:GridCheckBoxColumn>
                                <telerik:GridBoundColumn UniqueName="motivo_fallo" HeaderText="Motivo" SortExpression="motivo_fallo" DataField="motivo_fallo" ReadOnly="true"></telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkReenviar" runat="server" CssClass="btn btn-success colorBlanco" CommandArgument='<%# Eval("id_cotizacion")+";"+Eval("id_cliprov")+";"+Eval("correo") %>' OnClick="lnkReenviar_Click"><i class="fa fa-mail-reply"></i></asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>                       
                            </Columns>                            
                            <NoRecordsTemplate>
                                <asp:Label runat="server" ID="lblVacio" Text="No existen cotizaciones enviados" CssClass="errores"></asp:Label>
                            </NoRecordsTemplate>
                        </MasterTableView>                        
                        <ClientSettings AllowKeyboardNavigation="true">
                            <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" ></Scrolling>
                            <Selecting AllowRowSelect="true" /> 
                        </ClientSettings>                        
                        <PagerStyle PageSizeControlType="RadDropDownList" Mode="NextPrevAndNumeric"></PagerStyle>
                    </telerik:RadGrid>
                    <asp:SqlDataSource runat="server" ID="SqlDataSource5" ConnectionString='<%$ ConnectionStrings:PVW %>'
                            SelectCommand="select c.no_orden,c.id_empresa,c.id_taller, c.id_cotizacion, c.id_cliprov,p.razon_social,convert(char(10),c.fecha,120) as fecha,convert(char(8),c.hora,120) as hora,c.correo,u.nombre_usuario,c.motivo_fallo,c.enviado
from cotizaciones_enviadas c 
inner join Cliprov p on p.id_cliprov = c.id_cliprov and p.tipo='P'
inner join usuarios u on u.id_usuario=c.usuario
where c.no_orden=@no_orden and c.id_empresa=@id_empresa and c.id_taller=@id_taller and id_cotizacion=@id_cotizacion"
                            UpdateCommand ="update cotizaciones_enviadas set correo=@correo where no_orden=@no_orden and id_empresa=@id_empresa and id_taller=@id_taller and id_cotizacion=@id_cotizacion and id_cliprov=@id_cliprov update cliprov set correo=@correo where id_cliprov=@id_cliprov and tipo='P' ">
                            <SelectParameters>
                                <asp:QueryStringParameter QueryStringField="o" Name="no_orden"></asp:QueryStringParameter>
                                <asp:QueryStringParameter QueryStringField="t" Name="id_taller"></asp:QueryStringParameter>
                                <asp:QueryStringParameter QueryStringField="e" Name="id_empresa"></asp:QueryStringParameter>                                
                                <asp:ControlParameter ControlID="RadGridCot" Name="id_cotizacion" DefaultValue="0" />
                            </SelectParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="correo" Type="String" />
                            <asp:Parameter Name="no_orden" Type="Int32" />
                            <asp:Parameter Name="id_empresa" Type="Int32" />
                            <asp:Parameter Name="id_taller" Type="Int32" />
                            <asp:Parameter Name="id_cotizacion" Type="Int32" />
                            <asp:Parameter Name="id_cliprov" Type="Int32" />                            
                        </UpdateParameters>
                        </asp:SqlDataSource>
                        </div>
                </asp:Panel>
            </asp:Panel>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" Visible="false">
        <ContentTemplate>
            <div class="row">
                <div class="col-lg-6 col-sm-6 text-center pad1m">
                    <asp:LinkButton ID="lnkImprimir" Visible="false" runat="server" ToolTip="Impresion Valuación Gral." CssClass="btn btn-info"
                        OnClick="lnkImprimir_Click"><i class="fa fa-print"></i><span>&nbsp;Impresion Valuación Gral.</span></asp:LinkButton>
                </div>
                <div class="col-lg-6 col-sm-6 text-center pad1m">
                    <asp:LinkButton ID="lnkComparativo"  runat="server" ToolTip="Comparativo General" Visible="false" CssClass="btn btn-info"
                        OnClick="lnkComparativo_Click"><i class="fa fa-check-circle"></i><span>&nbsp;Comparativo General</span></asp:LinkButton>
                </div>
            </div>

            <div class="pie pad1m">
                <div class="clearfix">
                    <div class="row colorBlanco textoBold">
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label2" runat="server" Text="Tipo Orden:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="ddlToOrden" runat="server"></asp:Label>
                        </div>
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label4" runat="server" Text="Cliente:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="ddlClienteOrden" runat="server"></asp:Label>
                        </div>
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label6" runat="server" Text="Tipo Servicio:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="ddlTsOrden" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="row colorBlanco textoBold">
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label8" runat="server" Text="Tipo Valuación:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="ddlValOrden" runat="server"></asp:Label>
                        </div>
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label10" runat="server" Text="Tipo Asegurado:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="ddlTaOrden" runat="server"></asp:Label>
                        </div>
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label12" runat="server" Text="Localización:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="ddlLocOrden" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="row colorBlanco textoBold">
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label108" runat="server" Text="Perfil:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="ddlPerfil" runat="server"></asp:Label>
                        </div>
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label109" runat="server" Text="Siniestro:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="lblSiniestro" runat="server"></asp:Label>
                        </div>
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label110" runat="server" Text="Deducible:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="lblDeducible" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="row colorBlanco textoBold">
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label111" runat="server" Text="Total Orden:" CssClass="colorEtiqueta" Visible="false"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="lblTotOrden" runat="server" Visible="false"></asp:Label>
                        </div>
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label112" runat="server" Text="Promesa:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="lblEntregaEstimada" runat="server"></asp:Label>
                        </div>
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="lblPorcDeduEti" runat="server" Text="% Deducible:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="lblPorcDedu" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
            <asp:Label ID="lblIdRef" runat="server" Visible="false" />
            <asp:Label ID="lblRefaccion" runat="server" Visible="false" />
            <asp:Label ID="lblCantidad" runat="server" Visible="false" />
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

    <telerik:RadWindow RenderMode="Lightweight" ID="radAutorizacion" Title="Autorización" EnableShadow="true" Skin="Metro"
        Behaviors="Close,Move" VisibleOnPageLoad="false" ShowContentDuringLoad="false" DestroyOnClose="true"
        Animation="Fade" runat="server" Modal="true" Width="400px" Height="250px" Style="z-index: 2001;">
        <ContentTemplate>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="Panel2" runat="server" CssClass="ancho80 centrado">
                        <div class="row">
                            <div class="col-lg-4 col-sm-4 text-left">
                                <asp:Label ID="Label7" runat="server" Text="Usuario:" CssClass="textoBold" />
                            </div>
                            <div class="col-lg-8 col-sm-8 text-left">
                                <asp:TextBox ID="txtUsuarioLog" runat="server" CssClass="login input-large" MaxLength="20"
                                    placeholder="Usuario" TextMode="Password" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" Text="*"
                                    ForeColor="Red" ValidationGroup="log" ControlToValidate="txtUsuarioLog" ErrorMessage="Debe indicar el usuario."
                                    CssClass="pull-right" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" Text="*"
                                    ForeColor="Red" ValidationGroup="log" ErrorMessage="El usuario debe contener de entre 3 y 20 caracteres."
                                    CssClass="pull-right" ControlToValidate="txtUsuarioLog" ValidationExpression="[a-zA-Z0-9]{3,20}" />
                            </div>
                            <div class="col-lg-4 col-sm-4 text-left padding-top-10">
                                <asp:Label ID="Label5" runat="server" Text="Contraseña:" CssClass="textoBold" />&nbsp;
                            </div>
                            <div class="col-lg-8 col-sm-8 text-left padding-top-10">
                                <asp:TextBox ID="txtContraseñaLog" runat="server" CssClass="login input-large" TextMode="Password"
                                    MaxLength="20" placeholder="Contraseña" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" Text="*"
                                    ForeColor="Red" ValidationGroup="log" ControlToValidate="txtContraseñaLog" ErrorMessage="Debe indicar la contraseña."
                                    CssClass="pull-right" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" Text="*"
                                    ForeColor="Red" ValidationGroup="log" ErrorMessage="La contraseña debe contener de entre 5 y 20 caracteres."
                                    CssClass="pull-right" ControlToValidate="txtContraseñaLog" ValidationExpression="[a-zA-Z0-9]{5,20}" />
                            </div>
                            <div class="field col-lg-12 col-sm-12 textoCentrado textoBold padding-top-10">
                                <div class="col-lg-12 col-sm-12 text-center">
                                    <asp:Label ID="lblErrorLog" runat="server" CssClass="errores" />
                                </div>
                                <div class="col-lg-12 col-sm-12 text-center">
                                    <asp:ValidationSummary ID="ValidationSummary3" ValidationGroup="log" CssClass="errores"
                                        runat="server" DisplayMode="List" />
                                </div>
                            </div>
                            <div class="col-lg-12 col-sm-12 text-center pad1m">
                                <div class="col-lg-6 col-sm-6 text-center">
                                    <asp:LinkButton ID="btnAceptar" runat="server" OnClick="btnAceptar_Click" CssClass="btn btn-success"
                                        ValidationGroup="log"><i class="fa fa-check"></i><span>&nbsp;Autorizar</span></asp:LinkButton>
                                </div>
                                <div class="col-lg-6 col-sm-6 text-center">
                                    <asp:LinkButton ID="btnCancelar" OnClick="btnCancelar_Click" runat="server" CssClass="btn btn-danger"><i class="fa fa-ban"></i><span>&nbsp;Cancelar</span></asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel2">
                        <ProgressTemplate>
                            <asp:Panel ID="pnlMaskLoadEmi1" runat="server" CssClass="maskLoad" />
                            <asp:Panel ID="pnlCargandoEmi1" runat="server" CssClass="pnlPopUpLoad">
                                <asp:Image ID="imgLoadEmi1" runat="server" ImageUrl="~/img/loading.gif" CssClass="ancho100" />
                            </asp:Panel>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
    </telerik:RadWindow>

    <telerik:RadWindow RenderMode="Lightweight" ID="modalPopupComparativo" Title="Comparativo" EnableShadow="true" Skin="Metro"
        Behaviors="Close,Maximize,Move,Resize" VisibleOnPageLoad="false" ShowContentDuringLoad="false" DestroyOnClose="true"
        Animation="Fade" runat="server" Modal="true" Width="1000px" Height="700px" Style="z-index: 1000;" InitialBehaviors="Maximize">
        <ContentTemplate>
            <asp:UpdatePanel ID="UpdatePanelEmi" runat="server">
                <ContentTemplate>
                    <div class="row ancho95 centrado">
                        <div class="col-lg-12 col-sm-12">

                            <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" SkinID="MetroTouch" runat="server"
                                OnItemDataBound="RadGrid1_ItemDataBound" AutoGenerateColumns="false" AutoGenerateHierarchy="false" Height="650px">
                                <ClientSettings>
                                    <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true"></Scrolling>
                                </ClientSettings>
                                <MasterTableView TableLayout="Fixed">
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="cantidad" HeaderText="Cantidad" ReadOnly="true"
                                            SortExpression="cantidad">
                                            <ItemStyle Width="100px" />
                                            <HeaderStyle Width="100px" CssClass="text-center" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="descripcion" HeaderText="Refacción" ReadOnly="true"
                                            SortExpression="descripcion">
                                            <ItemStyle Width="150px" />
                                            <HeaderStyle Width="150px" CssClass="text-center" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn SortExpression="razon_social1" HeaderText="">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkProveedor1" runat="server" Text='<%# Bind("razon_social1") %>'
                                                    CssClass="link textoBold alert-primary ancho100 text-center" CommandArgument='<%# Eval("id_prov1")+";"+Eval("id_cotizacion_detalle")+";"+Eval("costo_unitario1") %>'></asp:LinkButton><br />
                                                <asp:Panel ID="pp1" runat="server">
                                                    <table class="table-bordered">
                                                        <tr>
                                                            <td rowspan="2" class="pad1m text-center">
                                                                <asp:Label ID="Label21" runat="server" Text='<%# Bind("importe1") %>' CssClass="textoBold text-info t14"></asp:Label>
                                                            </td>
                                                            <td colspan="2" class="pad1m">
                                                                <asp:Label ID="Label22" runat="server" Text="Costo Unitario" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label23" runat="server" Text='<%# Bind("costo_unitario1") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label24" runat="server" Text="Porc. Dto." CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label27" runat="server" Text='<%# Bind("porc_desc1") %>'></asp:Label>
                                                            </td>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label25" runat="server" Text="Descuento" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label26" runat="server" Text='<%# Bind("importe_desc1") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label28" runat="server" Text="Existencia" CssClass="textoBold"></asp:Label><br />
                                                                <asp:CheckBox ID="CheckBox2" runat="server" Checked='<%# Bind("existencia1") %>'
                                                                    Enabled="false" />
                                                            </td>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label29" runat="server" Text="Dias Entrega" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label30" runat="server" Text='<%# Bind("dias1") %>'></asp:Label>
                                                            </td>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label13" runat="server" Text="Procedencia" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label15" runat="server" Text='<%# Bind("dias1") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </ItemTemplate>
                                            <ItemStyle Width="240px" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn SortExpression="razon_social2" HeaderText="">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkProveedor2" runat="server" Text='<%# Bind("razon_social2") %>'
                                                    CssClass="link textoBold alert-primary ancho100 text-center" CommandArgument='<%# Eval("id_prov2")+";"+Eval("id_cotizacion_detalle")+";"+Eval("costo_unitario2") %>'></asp:LinkButton><br />
                                                <asp:Panel ID="pp2" runat="server">
                                                    <table class="table-bordered">
                                                        <tr>
                                                            <td rowspan="2" class="pad1m text-center">
                                                                <asp:Label ID="Label212" runat="server" Text='<%# Bind("importe2") %>' CssClass="textoBold text-info t14"></asp:Label>
                                                            </td>
                                                            <td colspan="2" class="pad1m">
                                                                <asp:Label ID="Label222" runat="server" Text="Costo Unitario" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label232" runat="server" Text='<%# Bind("costo_unitario2") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label242" runat="server" Text="Porc. Dto." CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label272" runat="server" Text='<%# Bind("porc_desc2") %>'></asp:Label>
                                                            </td>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label252" runat="server" Text="Descuento" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label262" runat="server" Text='<%# Bind("importe_desc2") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label282" runat="server" Text="Existencia" CssClass="textoBold"></asp:Label><br />
                                                                <asp:CheckBox ID="CheckBox22" runat="server" Checked='<%# Bind("existencia2") %>'
                                                                    Enabled="false" />
                                                            </td>
                                                            <td class="pad1m" colspan="2">
                                                                <asp:Label ID="Label292" runat="server" Text="Dias Entrega" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label302" runat="server" Text='<%# Bind("dias2") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </ItemTemplate>
                                            <ItemStyle Width="240px" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn SortExpression="razon_social3" HeaderText="">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkProveedor3" runat="server" Text='<%# Bind("razon_social3") %>'
                                                    CssClass="link textoBold alert-primary ancho100 text-center" CommandArgument='<%# Eval("id_prov3")+";"+Eval("id_cotizacion_detalle")+";"+Eval("costo_unitario3") %>'></asp:LinkButton><br />
                                                <asp:Panel ID="pp3" runat="server">
                                                    <table class="table-bordered">
                                                        <tr>
                                                            <td rowspan="2" class="pad1m text-center">
                                                                <asp:Label ID="Label213" runat="server" Text='<%# Bind("importe3") %>' CssClass="textoBold text-info t14"></asp:Label>
                                                            </td>
                                                            <td colspan="2" class="pad1m">
                                                                <asp:Label ID="Label223" runat="server" Text="Costo Unitario" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label233" runat="server" Text='<%# Bind("costo_unitario3") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label243" runat="server" Text="Porc. Dto." CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label273" runat="server" Text='<%# Bind("porc_desc3") %>'></asp:Label>
                                                            </td>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label253" runat="server" Text="Descuento" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label263" runat="server" Text='<%# Bind("importe_desc3") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label283" runat="server" Text="Existencia" CssClass="textoBold"></asp:Label><br />
                                                                <asp:CheckBox ID="CheckBox23" runat="server" Checked='<%# Bind("existencia3") %>'
                                                                    Enabled="false" />
                                                            </td>
                                                            <td class="pad1m" colspan="2">
                                                                <asp:Label ID="Label293" runat="server" Text="Dias Entrega" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label303" runat="server" Text='<%# Bind("dias3") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </ItemTemplate>
                                            <ItemStyle Width="240px" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn SortExpression="razon_social4" HeaderText="">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkProveedor4" runat="server" Text='<%# Bind("razon_social4") %>'
                                                    CssClass="link textoBold alert-primary ancho100 text-center" CommandArgument='<%# Eval("id_prov4")+";"+Eval("id_cotizacion_detalle")+";"+Eval("costo_unitario4") %>'></asp:LinkButton><br />
                                                <asp:Panel ID="pp4" runat="server">
                                                    <table class="table-bordered">
                                                        <tr>
                                                            <td rowspan="2" class="pad1m text-center">
                                                                <asp:Label ID="Label241" runat="server" Text='<%# Bind("importe4") %>' CssClass="textoBold text-info t14"></asp:Label>
                                                            </td>
                                                            <td colspan="2" class="pad1m">
                                                                <asp:Label ID="Label224" runat="server" Text="Costo Unitario" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label234" runat="server" Text='<%# Bind("costo_unitario4") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label244" runat="server" Text="Porc. Dto." CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label274" runat="server" Text='<%# Bind("porc_desc4") %>'></asp:Label>
                                                            </td>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label254" runat="server" Text="Descuento" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label264" runat="server" Text='<%# Bind("importe_desc4") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label284" runat="server" Text="Existencia" CssClass="textoBold"></asp:Label><br />
                                                                <asp:CheckBox ID="CheckBox24" runat="server" Checked='<%# Bind("existencia4") %>'
                                                                    Enabled="false" />
                                                            </td>
                                                            <td class="pad1m" colspan="2">
                                                                <asp:Label ID="Label294" runat="server" Text="Dias Entrega" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label304" runat="server" Text='<%# Bind("dias4") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </ItemTemplate>
                                            <ItemStyle Width="240px" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn SortExpression="razon_social5" HeaderText="">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkProveedor5" runat="server" Text='<%# Bind("razon_social5") %>'
                                                    CssClass="link textoBold alert-primary ancho100 text-center" CommandArgument='<%# Eval("id_prov5")+";"+Eval("id_cotizacion_detalle")+";"+Eval("costo_unitario5") %>'></asp:LinkButton><br />
                                                <asp:Panel ID="pp5" runat="server">
                                                    <table class="table-bordered">
                                                        <tr>
                                                            <td rowspan="2" class="pad1m text-center">
                                                                <asp:Label ID="Label2455" runat="server" Text='<%# Bind("importe5") %>' CssClass="textoBold text-info t14"></asp:Label>
                                                            </td>
                                                            <td colspan="2" class="pad1m">
                                                                <asp:Label ID="Label225" runat="server" Text="Costo Unitario" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label235" runat="server" Text='<%# Bind("costo_unitario5") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label245" runat="server" Text="Porc. Dto." CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label275" runat="server" Text='<%# Bind("porc_desc5") %>'></asp:Label>
                                                            </td>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label255" runat="server" Text="Descuento" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label265" runat="server" Text='<%# Bind("importe_desc5") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label285" runat="server" Text="Existencia" CssClass="textoBold"></asp:Label><br />
                                                                <asp:CheckBox ID="CheckBox25" runat="server" Checked='<%# Bind("existencia5") %>'
                                                                    Enabled="false" />
                                                            </td>
                                                            <td class="pad1m" colspan="2">
                                                                <asp:Label ID="Label295" runat="server" Text="Dias Entrega" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label305" runat="server" Text='<%# Bind("dias5") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </ItemTemplate>
                                            <ItemStyle Width="240px" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn SortExpression="razon_social6" HeaderText="">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkProveedor6" runat="server" Text='<%# Bind("razon_social6") %>'
                                                    CssClass="link textoBold alert-primary ancho100 text-center" CommandArgument='<%# Eval("id_prov6")+";"+Eval("id_cotizacion_detalle")+";"+Eval("costo_unitario6") %>'></asp:LinkButton><br />
                                                <asp:Panel ID="pp6" runat="server">
                                                    <table class="table-bordered">
                                                        <tr>
                                                            <td rowspan="2" class="pad1m text-center">
                                                                <asp:Label ID="Label2466" runat="server" Text='<%# Bind("importe6") %>' CssClass="textoBold text-info t14"></asp:Label>
                                                            </td>
                                                            <td colspan="2" class="pad1m">
                                                                <asp:Label ID="Label226" runat="server" Text="Costo Unitario" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label236" runat="server" Text='<%# Bind("costo_unitario6") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label246" runat="server" Text="Porc. Dto." CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label276" runat="server" Text='<%# Bind("porc_desc6") %>'></asp:Label>
                                                            </td>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label256" runat="server" Text="Descuento" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label266" runat="server" Text='<%# Bind("importe_desc6") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label286" runat="server" Text="Existencia" CssClass="textoBold"></asp:Label><br />
                                                                <asp:CheckBox ID="CheckBox26" runat="server" Checked='<%# Bind("existencia6") %>'
                                                                    Enabled="false" />
                                                            </td>
                                                            <td class="pad1m" colspan="2">
                                                                <asp:Label ID="Label296" runat="server" Text="Dias Entrega" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label306" runat="server" Text='<%# Bind("dias6") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </ItemTemplate>
                                            <ItemStyle Width="240px" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn SortExpression="razon_social7" HeaderText="">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkProveedor7" runat="server" Text='<%# Bind("razon_social7") %>'
                                                    CssClass="link textoBold alert-primary ancho100 text-center" CommandArgument='<%# Eval("id_prov7")+";"+Eval("id_cotizacion_detalle")+";"+Eval("costo_unitario7") %>'></asp:LinkButton><br />
                                                <asp:Panel ID="pp7" runat="server">
                                                    <table class="table-bordered">
                                                        <tr>
                                                            <td rowspan="2" class="pad1m text-center">
                                                                <asp:Label ID="Label2477" runat="server" Text='<%# Bind("importe7") %>' CssClass="textoBold text-info t14"></asp:Label>
                                                            </td>
                                                            <td colspan="2" class="pad1m">
                                                                <asp:Label ID="Label227" runat="server" Text="Costo Unitario" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label237" runat="server" Text='<%# Bind("costo_unitario7") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label247" runat="server" Text="Porc. Dto." CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label277" runat="server" Text='<%# Bind("porc_desc7") %>'></asp:Label>
                                                            </td>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label257" runat="server" Text="Descuento" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label267" runat="server" Text='<%# Bind("importe_desc7") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label287" runat="server" Text="Existencia" CssClass="textoBold"></asp:Label><br />
                                                                <asp:CheckBox ID="CheckBox27" runat="server" Checked='<%# Bind("existencia7") %>'
                                                                    Enabled="false" />
                                                            </td>
                                                            <td class="pad1m" colspan="2">
                                                                <asp:Label ID="Label297" runat="server" Text="Dias Entrega" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label307" runat="server" Text='<%# Bind("dias7") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </ItemTemplate>
                                            <ItemStyle Width="240px" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn SortExpression="razon_social8" HeaderText="">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkProveedor8" runat="server" Text='<%# Bind("razon_social8") %>'
                                                    CssClass="link textoBold alert-primary ancho100 text-center" CommandArgument='<%# Eval("id_prov8")+";"+Eval("id_cotizacion_detalle")+";"+Eval("costo_unitario8") %>'></asp:LinkButton><br />
                                                <asp:Panel ID="pp8" runat="server">
                                                    <table class="table-bordered">
                                                        <tr>
                                                            <td rowspan="2" class="pad1m text-center">
                                                                <asp:Label ID="Label248" runat="server" Text='<%# Bind("importe8") %>' CssClass="textoBold text-info t14"></asp:Label>
                                                            </td>
                                                            <td colspan="2" class="pad1m">
                                                                <asp:Label ID="Label228" runat="server" Text="Costo Unitario" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label238" runat="server" Text='<%# Bind("costo_unitario8") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label281" runat="server" Text="Porc. Dto." CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label278" runat="server" Text='<%# Bind("porc_desc8") %>'></asp:Label>
                                                            </td>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label258" runat="server" Text="Descuento" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label268" runat="server" Text='<%# Bind("importe_desc8") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label288" runat="server" Text="Existencia" CssClass="textoBold"></asp:Label><br />
                                                                <asp:CheckBox ID="CheckBox28" runat="server" Checked='<%# Bind("existencia8") %>'
                                                                    Enabled="false" />
                                                            </td>
                                                            <td class="pad1m" colspan="2">
                                                                <asp:Label ID="Label298" runat="server" Text="Dias Entrega" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label308" runat="server" Text='<%# Bind("dias8") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </ItemTemplate>
                                            <ItemStyle Width="240px" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn SortExpression="razon_social9" HeaderText="">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkProveedor9" runat="server" Text='<%# Bind("razon_social9") %>'
                                                    CssClass="link textoBold alert-primary ancho100 text-center" CommandArgument='<%# Eval("id_prov9")+";"+Eval("id_cotizacion_detalle")+";"+Eval("costo_unitario9") %>'></asp:LinkButton><br />
                                                <asp:Panel ID="pp9" runat="server">
                                                    <table class="table-bordered">
                                                        <tr>
                                                            <td rowspan="2" class="pad1m text-center">
                                                                <asp:Label ID="Label249" runat="server" Text='<%# Bind("importe9") %>' CssClass="textoBold text-info t14"></asp:Label>
                                                            </td>
                                                            <td colspan="2" class="pad1m">
                                                                <asp:Label ID="Label229" runat="server" Text="Costo Unitario" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label239" runat="server" Text='<%# Bind("costo_unitario9") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label291" runat="server" Text="Porc. Dto." CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label279" runat="server" Text='<%# Bind("porc_desc9") %>'></asp:Label>
                                                            </td>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label259" runat="server" Text="Descuento" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label269" runat="server" Text='<%# Bind("importe_desc9") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label289" runat="server" Text="Existencia" CssClass="textoBold"></asp:Label><br />
                                                                <asp:CheckBox ID="CheckBox29" runat="server" Checked='<%# Bind("existencia9") %>'
                                                                    Enabled="false" />
                                                            </td>
                                                            <td class="pad1m" colspan="2">
                                                                <asp:Label ID="Label299" runat="server" Text="Dias Entrega" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label309" runat="server" Text='<%# Bind("dias9") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </ItemTemplate>
                                            <ItemStyle Width="240px" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn SortExpression="razon_social10" HeaderText="">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkProveedor10" runat="server" Text='<%# Bind("razon_social10") %>'
                                                    CssClass="link textoBold alert-primary ancho100 text-center" CommandArgument='<%# Eval("id_prov10")+";"+Eval("id_cotizacion_detalle")+";"+Eval("costo_unitario10") %>'></asp:LinkButton><br />
                                                <asp:Panel ID="pp10" runat="server">
                                                    <table class="table-bordered">
                                                        <tr>
                                                            <td rowspan="2" class="pad1m text-center">
                                                                <asp:Label ID="Label2410" runat="server" Text='<%# Bind("importe10") %>' CssClass="textoBold text-info t14"></asp:Label>
                                                            </td>
                                                            <td colspan="2" class="pad1m">
                                                                <asp:Label ID="Label2210" runat="server" Text="Costo Unitario" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label2310" runat="server" Text='<%# Bind("costo_unitario10") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label2101" runat="server" Text="Porc. Dto." CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label2710" runat="server" Text='<%# Bind("porc_desc10") %>'></asp:Label>
                                                            </td>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label2510" runat="server" Text="Descuento" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label2610" runat="server" Text='<%# Bind("importe_desc10") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label2810" runat="server" Text="Existencia" CssClass="textoBold"></asp:Label><br />
                                                                <asp:CheckBox ID="CheckBox210" runat="server" Checked='<%# Bind("existencia10") %>'
                                                                    Enabled="false" />
                                                            </td>
                                                            <td class="pad1m" colspan="2">
                                                                <asp:Label ID="Label2910" runat="server" Text="Dias Entrega" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label3010" runat="server" Text='<%# Bind("dias10") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </ItemTemplate>
                                            <ItemStyle Width="240px" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn SortExpression="razon_social11" HeaderText="">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkProveedor11" runat="server" Text='<%# Bind("razon_social11") %>'
                                                    CssClass="link textoBold alert-primary ancho100 text-center" CommandArgument='<%# Eval("id_prov11")+";"+Eval("id_cotizacion_detalle")+";"+Eval("costo_unitario11") %>'></asp:LinkButton><br />
                                                <asp:Panel ID="pp11" runat="server">
                                                    <table class="table-bordered">
                                                        <tr>
                                                            <td rowspan="2" class="pad1m text-center">
                                                                <asp:Label ID="Label2411" runat="server" Text='<%# Bind("importe11") %>' CssClass="textoBold text-info t14"></asp:Label>
                                                            </td>
                                                            <td colspan="2" class="pad1m">
                                                                <asp:Label ID="Label2211" runat="server" Text="Costo Unitario" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label2311" runat="server" Text='<%# Bind("costo_unitario11") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label2111" runat="server" Text="Porc. Dto." CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label2711" runat="server" Text='<%# Bind("porc_desc11") %>'></asp:Label>
                                                            </td>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label2511" runat="server" Text="Descuento" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label2611" runat="server" Text='<%# Bind("importe_desc11") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label2811" runat="server" Text="Existencia" CssClass="textoBold"></asp:Label><br />
                                                                <asp:CheckBox ID="CheckBox211" runat="server" Checked='<%# Bind("existencia11") %>'
                                                                    Enabled="false" />
                                                            </td>
                                                            <td class="pad1m" colspan="2">
                                                                <asp:Label ID="Label2911" runat="server" Text="Dias Entrega" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label3011" runat="server" Text='<%# Bind("dias11") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </ItemTemplate>
                                            <ItemStyle Width="240px" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn SortExpression="razon_social12" HeaderText="">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkProveedor12" runat="server" Text='<%# Bind("razon_social12") %>'
                                                    CssClass="link textoBold alert-primary ancho100 text-center" CommandArgument='<%# Eval("id_prov12")+";"+Eval("id_cotizacion_detalle")+";"+Eval("costo_unitario12") %>'></asp:LinkButton><br />
                                                <asp:Panel ID="pp12" runat="server">
                                                    <table class="table-bordered">
                                                        <tr>
                                                            <td rowspan="2" class="pad1m text-center">
                                                                <asp:Label ID="Label2412" runat="server" Text='<%# Bind("importe12") %>' CssClass="textoBold text-info t14"></asp:Label>
                                                            </td>
                                                            <td colspan="2" class="pad1m">
                                                                <asp:Label ID="Label2212" runat="server" Text="Costo Unitario" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label2312" runat="server" Text='<%# Bind("costo_unitario12") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label2121" runat="server" Text="Porc. Dto." CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label2712" runat="server" Text='<%# Bind("porc_desc12") %>'></asp:Label>
                                                            </td>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label2512" runat="server" Text="Descuento" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label2612" runat="server" Text='<%# Bind("importe_desc12") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label2812" runat="server" Text="Existencia" CssClass="textoBold"></asp:Label><br />
                                                                <asp:CheckBox ID="CheckBox212" runat="server" Checked='<%# Bind("existencia12") %>'
                                                                    Enabled="false" />
                                                            </td>
                                                            <td class="pad1m" colspan="2">
                                                                <asp:Label ID="Label2912" runat="server" Text="Dias Entrega" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label3012" runat="server" Text='<%# Bind("dias12") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </ItemTemplate>
                                            <ItemStyle Width="240px" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn SortExpression="razon_social13" HeaderText="">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkProveedor13" runat="server" Text='<%# Bind("razon_social13") %>'
                                                    CssClass="link textoBold alert-primary ancho100 text-center" CommandArgument='<%# Eval("id_prov13")+";"+Eval("id_cotizacion_detalle")+";"+Eval("costo_unitario13") %>'></asp:LinkButton><br />
                                                <asp:Panel ID="pp13" runat="server">
                                                    <table class="table-bordered">
                                                        <tr>
                                                            <td rowspan="2" class="pad1m text-center">
                                                                <asp:Label ID="Label2413" runat="server" Text='<%# Bind("importe13") %>' CssClass="textoBold text-info t14"></asp:Label>
                                                            </td>
                                                            <td colspan="2" class="pad1m">
                                                                <asp:Label ID="Label2213" runat="server" Text="Costo Unitario" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label2313" runat="server" Text='<%# Bind("costo_unitario13") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label2131" runat="server" Text="Porc. Dto." CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label2713" runat="server" Text='<%# Bind("porc_desc13") %>'></asp:Label>
                                                            </td>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label2513" runat="server" Text="Descuento" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label2613" runat="server" Text='<%# Bind("importe_desc13") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label28131" runat="server" Text="Existencia" CssClass="textoBold"></asp:Label><br />
                                                                <asp:CheckBox ID="CheckBox213" runat="server" Checked='<%# Bind("existencia13") %>'
                                                                    Enabled="false" />
                                                            </td>
                                                            <td class="pad1m" colspan="2">
                                                                <asp:Label ID="Label2913" runat="server" Text="Dias Entrega" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label3013" runat="server" Text='<%# Bind("dias13") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </ItemTemplate>
                                            <ItemStyle Width="240px" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn SortExpression="razon_social14" HeaderText="">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkProveedor14" runat="server" Text='<%# Bind("razon_social14") %>'
                                                    CssClass="link textoBold alert-primary ancho100 text-center" CommandArgument='<%# Eval("id_prov14")+";"+Eval("id_cotizacion_detalle")+";"+Eval("costo_unitario14") %>'></asp:LinkButton><br />
                                                <asp:Panel ID="pp14" runat="server">
                                                    <table class="table-bordered">
                                                        <tr>
                                                            <td rowspan="2" class="pad1m text-center">
                                                                <asp:Label ID="Label2414" runat="server" Text='<%# Bind("importe14") %>' CssClass="textoBold text-info t14"></asp:Label>
                                                            </td>
                                                            <td colspan="2" class="pad1m">
                                                                <asp:Label ID="Label2214" runat="server" Text="Costo Unitario" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label2314" runat="server" Text='<%# Bind("costo_unitario14") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label2141" runat="server" Text="Porc. Dto." CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label2714" runat="server" Text='<%# Bind("porc_desc14") %>'></asp:Label>
                                                            </td>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label2514" runat="server" Text="Descuento" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label2614" runat="server" Text='<%# Bind("importe_desc14") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label28132" runat="server" Text="Existencia" CssClass="textoBold"></asp:Label><br />
                                                                <asp:CheckBox ID="CheckBox214" runat="server" Checked='<%# Bind("existencia14") %>'
                                                                    Enabled="false" />
                                                            </td>
                                                            <td class="pad1m" colspan="2">
                                                                <asp:Label ID="Label2914" runat="server" Text="Dias Entrega" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label3014" runat="server" Text='<%# Bind("dias14") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </ItemTemplate>
                                            <ItemStyle Width="240px" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn SortExpression="razon_social15" HeaderText="">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkProveedor15" runat="server" Text='<%# Bind("razon_social15") %>'
                                                    CssClass="link textoBold alert-primary ancho100 text-center" CommandArgument='<%# Eval("id_prov15")+";"+Eval("id_cotizacion_detalle")+";"+Eval("costo_unitario15") %>'></asp:LinkButton><br />
                                                <asp:Panel ID="pp15" runat="server">
                                                    <table class="table-bordered">
                                                        <tr>
                                                            <td rowspan="2" class="pad1m text-center">
                                                                <asp:Label ID="Label2415" runat="server" Text='<%# Bind("importe15") %>' CssClass="textoBold text-info t14"></asp:Label>
                                                            </td>
                                                            <td colspan="2" class="pad1m">
                                                                <asp:Label ID="Label2215" runat="server" Text="Costo Unitario" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label2315" runat="server" Text='<%# Bind("costo_unitario15") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label2151" runat="server" Text="Porc. Dto." CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label2715" runat="server" Text='<%# Bind("porc_desc15") %>'></asp:Label>
                                                            </td>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label2515" runat="server" Text="Descuento" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label2615" runat="server" Text='<%# Bind("importe_desc15") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label2815" runat="server" Text="Existencia" CssClass="textoBold"></asp:Label><br />
                                                                <asp:CheckBox ID="CheckBox215" runat="server" Checked='<%# Bind("existencia15") %>'
                                                                    Enabled="false" />
                                                            </td>
                                                            <td class="pad1m" colspan="2">
                                                                <asp:Label ID="Label2915" runat="server" Text="Dias Entrega" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label3015" runat="server" Text='<%# Bind("dias15") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </ItemTemplate>
                                            <ItemStyle Width="240px" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn SortExpression="razon_social16" HeaderText="">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkProveedor16" runat="server" Text='<%# Bind("razon_social16") %>'
                                                    CssClass="link textoBold alert-primary ancho100 text-center" CommandArgument='<%# Eval("id_prov16")+";"+Eval("id_cotizacion_detalle")+";"+Eval("costo_unitario16") %>'></asp:LinkButton><br />
                                                <asp:Panel ID="pp16" runat="server">
                                                    <table class="table-bordered">
                                                        <tr>
                                                            <td rowspan="2" class="pad1m text-center">
                                                                <asp:Label ID="Label2416" runat="server" Text='<%# Bind("importe16") %>' CssClass="textoBold text-info t14"></asp:Label>
                                                            </td>
                                                            <td colspan="2" class="pad1m">
                                                                <asp:Label ID="Label2216" runat="server" Text="Costo Unitario" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label2316" runat="server" Text='<%# Bind("costo_unitario16") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label2161" runat="server" Text="Porc. Dto." CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label2716" runat="server" Text='<%# Bind("porc_desc16") %>'></asp:Label>
                                                            </td>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label2516" runat="server" Text="Descuento" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label2616" runat="server" Text='<%# Bind("importe_desc16") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label2816" runat="server" Text="Existencia" CssClass="textoBold"></asp:Label><br />
                                                                <asp:CheckBox ID="CheckBox216" runat="server" Checked='<%# Bind("existencia16") %>'
                                                                    Enabled="false" />
                                                            </td>
                                                            <td class="pad1m" colspan="2">
                                                                <asp:Label ID="Label2916" runat="server" Text="Dias Entrega" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label3016" runat="server" Text='<%# Bind("dias16") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </ItemTemplate>
                                            <ItemStyle Width="240px" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn SortExpression="razon_social17" HeaderText="">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkProveedor17" runat="server" Text='<%# Bind("razon_social17") %>'
                                                    CssClass="link textoBold alert-primary ancho100 text-center" CommandArgument='<%# Eval("id_prov17")+";"+Eval("id_cotizacion_detalle")+";"+Eval("costo_unitario17") %>'></asp:LinkButton><br />
                                                <asp:Panel ID="pp17" runat="server">
                                                    <table class="table-bordered">
                                                        <tr>
                                                            <td rowspan="2" class="pad1m text-center">
                                                                <asp:Label ID="Label2417" runat="server" Text='<%# Bind("importe17") %>' CssClass="textoBold text-info t14"></asp:Label>
                                                            </td>
                                                            <td colspan="2" class="pad1m">
                                                                <asp:Label ID="Label2217" runat="server" Text="Costo Unitario" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label2317" runat="server" Text='<%# Bind("costo_unitario17") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label2171" runat="server" Text="Porc. Dto." CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label2717" runat="server" Text='<%# Bind("porc_desc17") %>'></asp:Label>
                                                            </td>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label2517" runat="server" Text="Descuento" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label2617" runat="server" Text='<%# Bind("importe_desc17") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label2817" runat="server" Text="Existencia" CssClass="textoBold"></asp:Label><br />
                                                                <asp:CheckBox ID="CheckBox217" runat="server" Checked='<%# Bind("existencia17") %>'
                                                                    Enabled="false" />
                                                            </td>
                                                            <td class="pad1m" colspan="2">
                                                                <asp:Label ID="Label2917" runat="server" Text="Dias Entrega" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label3017" runat="server" Text='<%# Bind("dias17") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </ItemTemplate>
                                            <ItemStyle Width="240px" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn SortExpression="razon_social18" HeaderText="">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkProveedor18" runat="server" Text='<%# Bind("razon_social18") %>'
                                                    CssClass="link textoBold alert-primary ancho100 text-center" CommandArgument='<%# Eval("id_prov18")+";"+Eval("id_cotizacion_detalle")+";"+Eval("costo_unitario18") %>'></asp:LinkButton><br />
                                                <asp:Panel ID="pp18" runat="server">
                                                    <table class="table-bordered">
                                                        <tr>
                                                            <td rowspan="2" class="pad1m text-center">
                                                                <asp:Label ID="Label2418" runat="server" Text='<%# Bind("importe18") %>' CssClass="textoBold text-info t14"></asp:Label>
                                                            </td>
                                                            <td colspan="2" class="pad1m">
                                                                <asp:Label ID="Label2218" runat="server" Text="Costo Unitario" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label2318" runat="server" Text='<%# Bind("costo_unitario18") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label2181" runat="server" Text="Porc. Dto." CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label2718" runat="server" Text='<%# Bind("porc_desc18") %>'></asp:Label>
                                                            </td>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label2518" runat="server" Text="Descuento" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label2618" runat="server" Text='<%# Bind("importe_desc18") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label2818" runat="server" Text="Existencia" CssClass="textoBold"></asp:Label><br />
                                                                <asp:CheckBox ID="CheckBox218" runat="server" Checked='<%# Bind("existencia18") %>'
                                                                    Enabled="false" />
                                                            </td>
                                                            <td class="pad1m" colspan="2">
                                                                <asp:Label ID="Label2918" runat="server" Text="Dias Entrega" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label3018" runat="server" Text='<%# Bind("dias18") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </ItemTemplate>
                                            <ItemStyle Width="240px" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn SortExpression="razon_social19" HeaderText="">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkProveedor19" runat="server" Text='<%# Bind("razon_social19") %>'
                                                    CssClass="link textoBold alert-primary ancho100 text-center" CommandArgument='<%# Eval("id_prov19")+";"+Eval("id_cotizacion_detalle")+";"+Eval("costo_unitario19") %>'></asp:LinkButton><br />
                                                <asp:Panel ID="pp19" runat="server">
                                                    <table class="table-bordered">
                                                        <tr>
                                                            <td rowspan="2" class="pad1m text-center">
                                                                <asp:Label ID="Label2419" runat="server" Text='<%# Bind("importe19") %>' CssClass="textoBold text-info t14"></asp:Label>
                                                            </td>
                                                            <td colspan="2" class="pad1m">
                                                                <asp:Label ID="Label2219" runat="server" Text="Costo Unitario" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label2319" runat="server" Text='<%# Bind("costo_unitario19") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label2191" runat="server" Text="Porc. Dto." CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label2719" runat="server" Text='<%# Bind("porc_desc19") %>'></asp:Label>
                                                            </td>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label2519" runat="server" Text="Descuento" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label2619" runat="server" Text='<%# Bind("importe_desc19") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label2819" runat="server" Text="Existencia" CssClass="textoBold"></asp:Label><br />
                                                                <asp:CheckBox ID="CheckBox219" runat="server" Checked='<%# Bind("existencia19") %>'
                                                                    Enabled="false" />
                                                            </td>
                                                            <td class="pad1m" colspan="2">
                                                                <asp:Label ID="Label2919" runat="server" Text="Dias Entrega" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label3019" runat="server" Text='<%# Bind("dias19") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </ItemTemplate>
                                            <ItemStyle Width="240px" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn SortExpression="razon_social20" HeaderText="">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkProveedor20" runat="server" Text='<%# Bind("razon_social20") %>'
                                                    CssClass="link textoBold alert-primary ancho100 text-center" CommandArgument='<%# Eval("id_prov20")+";"+Eval("id_cotizacion_detalle")+";"+Eval("costo_unitario20") %>'></asp:LinkButton><br />
                                                <asp:Panel ID="pp20" runat="server">
                                                    <table class="table-bordered">
                                                        <tr>
                                                            <td rowspan="2" class="pad1m text-center">
                                                                <asp:Label ID="Label2420" runat="server" Text='<%# Bind("importe20") %>' CssClass="textoBold text-info t14"></asp:Label>
                                                            </td>
                                                            <td colspan="2" class="pad1m">
                                                                <asp:Label ID="Label2220" runat="server" Text="Costo Unitario" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label2320" runat="server" Text='<%# Bind("costo_unitario20") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label2120" runat="server" Text="Porc. Dto." CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label2720" runat="server" Text='<%# Bind("porc_desc20") %>'></asp:Label>
                                                            </td>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label2520" runat="server" Text="Descuento" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label2620" runat="server" Text='<%# Bind("importe_desc20") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="pad1m">
                                                                <asp:Label ID="Label2820" runat="server" Text="Existencia" CssClass="textoBold"></asp:Label><br />
                                                                <asp:CheckBox ID="CheckBox220" runat="server" Checked='<%# Bind("existencia20") %>'
                                                                    Enabled="false" />
                                                            </td>
                                                            <td class="pad1m" colspan="2">
                                                                <asp:Label ID="Label2920" runat="server" Text="Dias Entrega" CssClass="textoBold"></asp:Label><br />
                                                                <asp:Label ID="Label3020" runat="server" Text='<%# Bind("dias20") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </ItemTemplate>
                                            <ItemStyle Width="240px" />
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                                </MasterTableView>
                            </telerik:RadGrid>

                        </div>
                        <div class="col-lg-12 col-sm-12 marTop">
                            <asp:Panel ID="pnlNuevoProv" runat="server" CssClass="ancho95 centrado">
                                <div class="col-lg-3 col-sm-3 text-left">
                                    <asp:Label ID="Label14" runat="server" Text="Refacción:" CssClass="textoBold"></asp:Label>
                                    <asp:DropDownList RenderMode="Lightweight" runat="server" ID="ddlRefaccionesNueva"
                                        CssClass="input-xlarge" DataSourceID="SqlDataSource3"
                                        DataTextField="refaccion" DataValueField="reford_id">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>"
                                        SelectCommand="select reford_id,'('+rtrim(ltrim(cast(refcantidad as char(10))))+') '+refdescripcion as refaccion from refacciones_orden where refEstatusSolicitud in(1,2,4) and ref_id_empresa=@empresa and reF_id_taller=@taller and ref_no_orden=@orden and refOrd_Id in(select distinct id_cotizacion_detalle from Cotizacion_Detalle where id_empresa=@empresa and id_taller=@taller and no_orden=@orden)">
                                        <SelectParameters>
                                            <asp:QueryStringParameter Name="empresa" QueryStringField="e" DefaultValue="0" />
                                            <asp:QueryStringParameter Name="taller" QueryStringField="t" DefaultValue="0" />
                                            <asp:QueryStringParameter Name="orden" QueryStringField="o" DefaultValue="0" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>
                                <div class="col-lg-3 col-sm-3 text-left">
                                    <asp:Label ID="lblEtiquetaProveedor" runat="server" Text="Proveedor:" CssClass="textoBold"></asp:Label>
                                    <asp:TextBox ID="txtProveedorAdd" runat="server" CssClass="input-large"></asp:TextBox>
                                    <cc1:TextBoxWatermarkExtender ID="txtProveedorAdd_TextBoxWatermarkExtender" runat="server"
                                        BehaviorID="txtProveedorAdd_TextBoxWatermarkExtender" TargetControlID="txtProveedorAdd"
                                        WatermarkCssClass="water input-large" WatermarkText="Proveedor" />
                                    <div style="z-index: 5000;">
                                        <cc1:AutoCompleteExtender ID="txtProveedorAdd_AutoCompleteExtender" runat="server"
                                            BehaviorID="txtProveedorAdd_AutoCompleteExtender" Enabled="true" ServiceMethod="obtieneProveedores"
                                            UseContextKey="True" MinimumPrefixLength="1" CompletionInterval="10" CompletionSetCount="10"
                                            TargetControlID="txtProveedorAdd">
                                        </cc1:AutoCompleteExtender>
                                    </div>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Debe indicar el proveedor"
                                        Text="*" ValidationGroup="manual" CssClass="errores" ControlToValidate="txtProveedorAdd"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-lg-3 col-sm-3 text-left">
                                    <asp:Label ID="lblCU" runat="server" Text="Cost. Unit.:" CssClass="textoBold"></asp:Label>
                                    <telerik:RadNumericTextBox RenderMode="Lightweight" runat="server" ID="radCostoUnitario" CssClass="input-mini" Value="0" EmptyMessage="Cost. Unit." MinValue="0" MaxValue="1000000" ShowSpinButtons="true" NumberFormat-DecimalDigits="2" Skin="MetroTouch"></telerik:RadNumericTextBox>
                                    <%--<asp:TextBox ID="txtContoUnitario" runat="server" CssClass="input-small" MaxLength="16"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Debe indicar le costo unitario"
                                        Text="*" CssClass="errores" ControlToValidate="txtContoUnitario" ValidationGroup="manual"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtContoUnitario"
                                        runat="server" ValidationExpression="^\d{1,13}(\.\d{1,2})?$" ErrorMessage="El costo solo puede contener dígitos y un punto decimal"
                                        ValidationGroup="manual" Text="*" CssClass="errores"></asp:RegularExpressionValidator>--%>
                                </div>
                                <div class="col-lg-2 col-sm-2 text-left">
                                    <asp:Label ID="lblPorcDesc" runat="server" Text="% Dto.:" CssClass="textoBold"></asp:Label>
                                    <telerik:RadNumericTextBox RenderMode="Lightweight" runat="server" ID="radPorcDesc" CssClass="input-mini" Value="0" EmptyMessage="% Dto." MinValue="0" MaxValue="100" ShowSpinButtons="true" NumberFormat-DecimalDigits="2" Skin="MetroTouch"></telerik:RadNumericTextBox>
                                    <%--<asp:TextBox ID="txtPorcDesc" runat="server" CssClass="input-mini" MaxLength="6"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="txtPorcDesc"
                                        runat="server" ValidationExpression="^100$|^100.00$|^\d{0,2}(\.\d{1,2})? *%?$"
                                        ErrorMessage="El porcentaje solo puede contener dígitos y un punto decimal" ValidationGroup="manual"
                                        Text="*" CssClass="errores"></asp:RegularExpressionValidator>--%>
                                </div>
                                <div class="col-lg-1 col-sm-1 text-left">
                                    <asp:CheckBox ID="chkExistencia" runat="server" Text="Existencia" />
                                </div>
                                <div class="col-lg-3 col-sm-3 text-left">
                                    <asp:Label ID="lblDias" runat="server" Text="Días Entrega:" CssClass="textoBold"></asp:Label>
                                    <telerik:RadNumericTextBox RenderMode="Lightweight" runat="server" ID="radDias" CssClass="input-mini" Value="0" EmptyMessage="Días" MinValue="0" MaxValue="365" ShowSpinButtons="true" NumberFormat-DecimalDigits="0" Skin="MetroTouch"></telerik:RadNumericTextBox>
                                    <%--<asp:TextBox ID="txtDias" runat="server" CssClass="input-mini" MaxLength="3"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" ControlToValidate="txtDias"
                                        runat="server" ValidationExpression="^\d{3}|\d{2}|\d{1}|\d$" ErrorMessage="Debe indicar solo dígitos"
                                        ValidationGroup="manual" Text="*" CssClass="errores"></asp:RegularExpressionValidator>--%>
                                </div>
                                <div class="col-lg-12 col-sm-12 text-center">
                                    <asp:ValidationSummary ID="valManual" CssClass="errores" runat="server" DisplayMode="List"
                                        ValidationGroup="manual" />
                                    <asp:Label ID="lblErrorNuevo" runat="server" CssClass="errores"></asp:Label>
                                </div>
                            </asp:Panel>
                            <div class="row ancho95 centrado text-center marTop">
                                <div class="col-lg-4 col-sm-4 text-center">
                                    <asp:LinkButton ID="lnkAgregarProveedor" runat="server" ToolTip="Agregar" CssClass="btn btn-success alingMiddle"
                                        ValidationGroup="manual" OnClick="lnkAgregarProveedor_Click"><i class="fa fa-save"></i><span>&nbsp;Agregar</span>
                                    </asp:LinkButton>
                                </div>
                                <div class="col-lg-4 col-sm-4 text-center">
                                    <asp:LinkButton ID="lnkImprimeComparativo" runat="server" ToolTip="Imprimir Comparativo"
                                        CssClass="btn btn-info alingMiddle" OnClick="lnkImprimeComparativo_Click"><i class="fa fa-print"></i><span>&nbsp;Imprimir</span>
                                    </asp:LinkButton>
                                </div>
                                <div class="col-lg-4 col-sm-4 text-center">
                                    <asp:LinkButton ID="lnkCerrar" runat="server" ToolTip="Cerrar"
                                        CssClass="btn btn-danger alingMiddle" OnClientClick="cierraWinComp()"><i class="fa fa-remove"></i><span>&nbsp;Cerrar</span>
                                    </asp:LinkButton>
                                </div>
                            </div>

                            <div class="row ancho95 centrado text-center marTop">
                                
                                <div class="col-lg-2 col-sm-2 text-center">
                                    <asp:LinkButton ID="lnkImprimeComparativoMapfre" runat="server" CssClass="btn btn-info alingMiddle" OnClick="lnkImprimeComparativoMapfre_Click" ><asp:image runat="server" ID="imgMap" ImageUrl="~/img/bitacoras/mapfre.png" Width="80px" Height="80px" AlternateText="Mapfre" /></asp:LinkButton>
                                </div>
                                <div class="col-lg-2 col-sm-2 text-center">
                                    <asp:LinkButton ID="lnkPresupuestoRefaccionesExel" runat="server" CssClass="btn btn-info" OnClick="lnkPresupuestoRefaccionesExel_Click" ><asp:image runat="server" ID="Image1" ImageUrl="~/img/bitacoras/multiva.png" Width="80px" Height="80px" AlternateText="Multiva"/></asp:LinkButton>
                                </div>
                                <div class="col-lg-2 col-sm-2 text-center">
                                    <asp:LinkButton ID="lnkCompleMO" CssClass="btn btn-info" OnClick="lnkCompleMO_Click" runat="server"><asp:image runat="server" ID="Image2" ImageUrl="~/img/bitacoras/aba.png" Width="80px" Height="80px" AlternateText="Aba"/><br /><span>Mano Obra</span></asp:LinkButton>
                                </div>
                                <div class="col-lg-2 col-sm-2 text-center">
                                    <asp:LinkButton ID="lnkCompleREF" CssClass="btn btn-info" OnClick="lnkCompleREF_Click" runat="server"><asp:image runat="server" ID="Image3" ImageUrl="~/img/bitacoras/aba.png" Width="80px" Height="80px" AlternateText="Aba"/><br /><span>Refacciones</span></asp:LinkButton>
                                </div>
                                <div class="col-lg-2 col-sm-2 text-center">
                                    <asp:LinkButton ID="btnComparativoAfirme" CssClass="btn btn-info" OnClick="btnComparativoAfirme_Click" runat="server"><asp:image runat="server" ID="Image4" ImageUrl="~/img/bitacoras/afirme.png" Width="80px" Height="80px" AlternateText="Afirme"/></asp:LinkButton>
                                </div>
                                <div class="col-lg-2 col-sm-2 text-center">
                                    <asp:LinkButton ID="btnComparativoAtlas" CssClass="btn btn-info" OnClick="btnComparativoAtlas_Click" runat="server"><asp:image runat="server" ID="Image5" ImageUrl="~/img/bitacoras/atlas.png" Width="80px" Height="80px" AlternateText="Atlas"/></asp:LinkButton>
                                </div>
                                <div class="col-lg-2 col-sm-2 text-center">
                                    <asp:LinkButton ID="btnComparativoHDI" CssClass="btn btn-info" OnClick="btnComparativoHDI_Click" runat="server"><asp:image runat="server" ID="Image6" ImageUrl="~/img/bitacoras/hdi.png" Width="80px" Height="80px" AlternateText="HDI"/></asp:LinkButton>
                                </div>
                                    
                            </div>
                        </div>
                    </div>
                    <asp:UpdateProgress ID="updProgEmi" runat="server" AssociatedUpdatePanelID="UpdatePanelEmi">
                        <ProgressTemplate>
                            <asp:Panel ID="pnlMaskLoadEmi" runat="server" CssClass="maskLoad" />
                            <asp:Panel ID="pnlCargandoEmi" runat="server" CssClass="pnlPopUpLoad">
                                <asp:Image ID="imgLoadEmi" runat="server" ImageUrl="~/img/loading.gif" CssClass="ancho100" />
                            </asp:Panel>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="lnkImprimeComparativoMapfre" />                    
                    <asp:PostBackTrigger ControlID="lnkPresupuestoRefaccionesExel" />                    
                    <asp:PostBackTrigger ControlID="lnkCompleMO" />                    
                    <asp:PostBackTrigger ControlID="lnkCompleREF" />                    
                    <asp:PostBackTrigger ControlID="btnComparativoAfirme" />
                    <asp:PostBackTrigger ControlID="btnComparativoAtlas" />
                    <asp:PostBackTrigger ControlID="btnComparativoHDI" />
                </Triggers>
            </asp:UpdatePanel>
        </ContentTemplate>
    </telerik:RadWindow>
</asp:Content>
