<%@ Page Title="" Language="C#" MasterPageFile="~/AdmonOrden.master" AutoEventWireup="true" CodeFile="Ajuste.aspx.cs" Inherits="Ajuste" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" />
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server"></telerik:RadSkinManager>
    <div class="row">
        <div class="col-lg-12 col-sm-12 text-center alert-info">
            <h3>
                <i class="fa fa-pencil-square"></i>&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label1" runat="server" Text="Ajuste"></asp:Label>
            </h3>
        </div>
    </div>

    <telerik:RadAjaxManager runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />                    
                </UpdatedControls>
            </telerik:AjaxSetting> 
            <telerik:AjaxSetting AjaxControlID="RadGrid2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid2" LoadingPanelID="RadAjaxLoadingPanel1" />                    
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkGenerarOrdenCompra">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid3" LoadingPanelID="RadAjaxLoadingPanel1" />                    
                </UpdatedControls>
            </telerik:AjaxSetting>            
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1" Skin="MetroTouch"></telerik:RadAjaxLoadingPanel>

            <div class="row">
                <div class="col-lg-12 col-sm-12">
                    <telerik:RadTabStrip RenderMode="Lightweight" runat="server" ID="RadTabStrip1"  Orientation="HorizontalTop" SelectedIndex="0" MultiPageID="RadMultiPage1" Skin="MetroTouch">
                        <Tabs>                                                                       
                            <telerik:RadTab Text="Mano de Obra"/>
                            <telerik:RadTab Text="Refacciones"/>
                        </Tabs>
                    </telerik:RadTabStrip>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-12 col-sm-12">
                    <telerik:RadMultiPage runat="server" ID="RadMultiPage1"  SelectedIndex="0" >
                        <telerik:RadPageView runat="server" ID="RadPageView_MO">
                            <div class="row">
                                <div class="col-lg-12 col-sm-12">
                                    <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" GridLines="None" runat="server" PageSize="50" AllowAutomaticUpdates="True" 
                                        AllowPaging="false" AutoGenerateColumns="False" PagerStyle-AlwaysVisible="false" OnItemUpdated="RadGrid1_ItemUpdated" AllowFilteringByColumn="true"  EnableHeaderContextMenu="true" EnableHeaderContextFilterMenu="true"
                                        AllowSorting="true" DataSourceID="SqlDataSource1" Skin="Metro" AllowAutomaticInserts="false" AllowAutomaticDeletes="false" >
                                        <MasterTableView CommandItemDisplay="Bottom" DataKeyNames="consecutivo" HorizontalAlign="NotSet" ShowFooter="true" EditMode="Batch" AutoGenerateColumns="False" DataSourceID="SqlDataSource1">                                    
                                            <BatchEditingSettings EditType="Row" />
                                            <CommandItemStyle CssClass="text-right" />
                                            <CommandItemSettings SaveChangesText="Guardar Cambios" ShowAddNewRecordButton="false"  ShowRefreshButton="false" ShowSaveChangesButton="true" CancelChangesText="Cancelar Cambios"/>
                                            <Columns>
                                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="consecutivo" HeaderText="Renglon" SortExpression="consecutivo" UniqueName="consecutivo" ReadOnly="true" Visible="false"/>
                                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="grupo" HeaderText="Grupo" SortExpression="grupo" UniqueName="grupo" ReadOnly="true"/>
                                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="operacion" HeaderText="Operación" SortExpression="operacion" UniqueName="operacion" ReadOnly="true"/>
                                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="id_refaccion" HeaderText="Desccripción" SortExpression="id_refaccion" UniqueName="id_refaccion" ReadOnly="true"/>
                                                <telerik:GridNumericColumn FilterCheckListEnableLoadOnDemand="true" DataField="monto_ini" HeaderText="Monto Inicial" SortExpression="monto_ini" UniqueName="monto_ini" DataFormatString="{0:C2}" DecimalDigits="2" ReadOnly="true" />
                                                <telerik:GridTemplateColumn FilterCheckListEnableLoadOnDemand="true" DataField="monto_mo" SortExpression="monto_mo" UniqueName="monto_mo" FooterAggregateFormatString="{0:N2}" Aggregate="Sum" HeaderText="Monto Autorizado">
                                                    <ItemTemplate><asp:Label ID="Label3" runat="server" Text='<%# Eval("monto_mo","{0:C2}") %>'></asp:Label></ItemTemplate>
                                                    <EditItemTemplate>
                                                        <telerik:RadNumericTextBox ID="radMontoMo" runat="server" RenderMode="Lightweight" Skin="MetroTouch" EmptyMessage="Monto Autorizado" NumberFormat-DecimalDigits="2" NumberFormat-DecimalSeparator="." ShowSpinButtons="true"  ></telerik:RadNumericTextBox>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>                                                
                                                <telerik:GridCheckBoxColumn FilterCheckListEnableLoadOnDemand="true" Visible="false" DataField="facturado" HeaderText="Facturado" SortExpression="facturado" UniqueName="facturado" ReadOnly="true"/>
                                                <telerik:GridCheckBoxColumn FilterCheckListEnableLoadOnDemand="true" Visible="false" DataField="pTerminado" HeaderText="Presupuesto Terminado" SortExpression="pTerminado" UniqueName="pTerminado" ReadOnly="true" />
                                                <telerik:GridCheckBoxColumn FilterCheckListEnableLoadOnDemand="true" Visible="false" DataField="ajustado" HeaderText="Ajustado" SortExpression="ajustado" UniqueName="ajustado" />                                                
                                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="nombre_usuario" HeaderText="Usuario Ajuste" SortExpression="nombre_usuario" UniqueName="nombre_usuario" ReadOnly="true"/>
                                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="fechaajustado" HeaderText="Fecha Ajuste" SortExpression="fechaajustado" UniqueName="fechaajustado" ReadOnly="true"/>
                                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="horaajustado" HeaderText="Hora Ajuste" SortExpression="horaajustado" UniqueName="horaajustado" ReadOnly="true"/>
                                            </Columns>
                                            <NoRecordsTemplate>
                                                <asp:Label ID="lblnoReecMo" runat="server" Text="No existe mano de obra registrada" CssClass="errores"></asp:Label>
                                            </NoRecordsTemplate>
                                        </MasterTableView>                                
                                        <ClientSettings AllowKeyboardNavigation="true">
                                            <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" ></Scrolling>
                                            <Selecting AllowRowSelect="true" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>"                                
                                        SelectCommand="select m.consecutivo,m.id_grupo_op,m.id_operacion,g.descripcion_go as grupo,o.descripcion_op as operacion, m.id_refaccion,m.monto_ini, m.monto_mo,m.estatus_presupuesto,m.idCfd,m.statusCfd,cast((case when m.idcfd is null then 0 when m.idcfd>0 then 1 else 0 end) as bit) as facturado,m.ajustado,m.usuarioajustado,u.nombre_usuario,convert(char(10),m.fechaajustado,120) as fechaajustado,convert(char(8),m.horaajustado,120) as horaajustado,cast((case m.estatus_presupuesto when 'T' then 1 Else 0 end) as bit) as pTerminado from mano_obra m left join usuarios u on u.id_usuario=m.usuarioajustado inner join Grupo_Operacion g on g.id_grupo_op=m.id_grupo_op inner join operaciones o on o.id_operacion=m.id_operacion where m.id_taller=@taller and m.no_orden=@orden and m.id_empresa=@empresa"
                                        UpdateCommand="UPDATE mano_obra SET monto_mo=@monto_mo,ajustado=@ajustado,fechaajustado=@fecha,horaajustado=@hora,usuarioajustado=@usuario WHERE no_orden=@orden and id_empresa=@empresa and id_taller=@taller and consecutivo=@consecutivo and (isnull(ajustado,0)=0 and cast((case when idcfd is null then 0 when idcfd>0 then 1 else 0 end) as bit)=0)">
                                        <SelectParameters>
                                            <asp:QueryStringParameter Name="orden" QueryStringField="o" DbType="Int32" DefaultValue="0" />
                                            <asp:QueryStringParameter Name="empresa" QueryStringField="e" DbType="Int32" DefaultValue="0" />
                                            <asp:QueryStringParameter Name="taller" QueryStringField="t" DbType="Int32" DefaultValue="0" />
                                        </SelectParameters>                                
                                        <UpdateParameters>                                            
                                            <asp:Parameter Name="consecutivo" Type="Int32"></asp:Parameter>
                                            <asp:Parameter Name="monto_mo" Type="Decimal"></asp:Parameter>
                                            <asp:QueryStringParameter Name="orden" QueryStringField="o" DbType="Int32" DefaultValue="0" />
                                            <asp:QueryStringParameter Name="empresa" QueryStringField="e" DbType="Int32" DefaultValue="0" />
                                            <asp:QueryStringParameter Name="taller" QueryStringField="t" DbType="Int32" DefaultValue="0" />
                                            <asp:QueryStringParameter Name="usuario" QueryStringField="u" DbType="Int32" DefaultValue="0" />
                                            <asp:Parameter Name="ajustado" Type="Boolean" />
                                            <asp:ControlParameter Name="fecha" ControlID="lblFecha" PropertyName="Text" />
                                            <asp:ControlParameter Name="hora" ControlID="lblHora" PropertyName="Text" />
                                        </UpdateParameters>
                                    </asp:SqlDataSource>
                                </div>
                            </div>
                        </telerik:RadPageView>
                        <telerik:RadPageView runat="server" ID="RadPageView_Ref">
                            <div class="row">
                                <div class="col-lg-12 col-sm-12">
                                    <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid2" GridLines="None" runat="server" PageSize="50" AllowAutomaticUpdates="True" 
                                        AllowPaging="false" AutoGenerateColumns="False" PagerStyle-AlwaysVisible="false" OnItemUpdated="RadGrid2_ItemUpdated" AllowFilteringByColumn="true"  EnableHeaderContextMenu="true" 
                                        EnableHeaderContextFilterMenu="true" AllowSorting="true" DataSourceID="SqlDataSource2" Skin="Metro" ShowFooter="true" AllowAutomaticInserts="false" AllowAutomaticDeletes="false" >
                                        <MasterTableView CommandItemDisplay="Bottom" DataKeyNames="refOrd_Id" HorizontalAlign="NotSet" EditMode="Batch" AutoGenerateColumns="False" DataSourceID="SqlDataSource2">                                    
                                            <BatchEditingSettings EditType="Row" />
                                            <CommandItemStyle CssClass="text-right" />
                                            <CommandItemSettings SaveChangesText="Guardar Cambios" ShowAddNewRecordButton="false"  ShowRefreshButton="false" ShowSaveChangesButton="true" CancelChangesText="Cancelar Cambios"/>
                                            <Columns>
                                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" Resizable="true" DataField="refOrd_Id" HeaderText="Renglon" SortExpression="refOrd_Id" UniqueName="refOrd_Id" ReadOnly="true" Visible="false"/>
                                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" Resizable="true" DataField="refCantidad" HeaderText="Cantidad" SortExpression="refCantidad" UniqueName="refCantidad" ReadOnly="true" FooterText="Totales:"/>
                                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" Resizable="true" DataField="refDescripcion" HeaderText="Refacción" SortExpression="refDescripcion" UniqueName="refDescripcion" ReadOnly="true"/>
                                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" Resizable="true" DataField="razon_social" HeaderText="Proveedor" SortExpression="razon_social" UniqueName="razon_social" ReadOnly="true"/>
                                                <telerik:GridNumericColumn FilterCheckListEnableLoadOnDemand="true" Visible="false" Resizable="true" DataField="refCosto" HeaderText="C.U." SortExpression="refCosto" UniqueName="refCosto" ReadOnly="true" DataFormatString="{0:C2}" />
                                                <telerik:GridNumericColumn FilterCheckListEnableLoadOnDemand="true" Visible="false" Resizable="true" DataField="porc_desc" HeaderText="% Dto." SortExpression="porc_desc" UniqueName="porc_desc" ReadOnly="true" DataFormatString="{0:F2}%" />
                                                <telerik:GridNumericColumn FilterCheckListEnableLoadOnDemand="true" Visible="false" Resizable="true" DataField="importeDesc" HeaderText="Impte. Dto." SortExpression="importeDesc" UniqueName="importeDesc" ReadOnly="true" DataFormatString="{0:C2}" />
                                                <telerik:GridNumericColumn FilterCheckListEnableLoadOnDemand="true" Visible="false" Resizable="true" DataField="importeCosto" HeaderText="Impte. Compra" SortExpression="importeCosto" UniqueName="importeCosto" ReadOnly="true" DataFormatString="{0:C2}" Aggregate="Sum" FooterAggregateFormatString="{0:C2}" />
                                                <%-- 
                                                <telerik:GridTemplateColumn FilterCheckListEnableLoadOnDemand="true" Visible="false" Resizable="true" DataField="porcSobre" HeaderText="% S.C." SortExpression="porcSobre" UniqueName="porcSobre">
                                                    <ItemTemplate><%# Eval("porcSobre","{0:F2}")+"%" %></ItemTemplate>
                                                    <EditItemTemplate>
                                                        <telerik:RadNumericTextBox runat="server" RenderMode="Lightweight" Skin="Metro" ID="radPorcSobreCosto" ShowSpinButtons="true" MaxValue="100" MinValue="0" NumberFormat-DecimalDigits="2" NumberFormat-DecimalSeparator="."></telerik:RadNumericTextBox>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                --%>
                                                <telerik:GridTemplateColumn FilterCheckListEnableLoadOnDemand="true" Resizable="true" DataField="refPrecioVenta" HeaderText="P.U. Autorizado" SortExpression="refPrecioVenta" UniqueName="refPrecioVenta" >
                                                    <ItemTemplate><%# Eval("refPrecioVenta","{0:C2}") %></ItemTemplate>
                                                    <EditItemTemplate>
                                                        <telerik:RadNumericTextBox runat="server" RenderMode="Lightweight" Skin="Metro" ID="radPUAutorizado" Width="100px" ShowSpinButtons="true" MaxValue="9999999999.99" MinValue="0" NumberFormat-DecimalDigits="2" NumberFormat-DecimalSeparator="."></telerik:RadNumericTextBox>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>                                                
                                                <telerik:GridTemplateColumn FilterCheckListEnableLoadOnDemand="true" Resizable="true" DataField="importeVenta" Aggregate="Sum" FooterAggregateFormatString="{0:N2}" HeaderText="Impte. Autorizado" SortExpression="importeVenta" UniqueName="importeVenta"  ReadOnly="true">
                                                    <ItemTemplate><%# Eval("importeVenta","{0:C2}") %></ItemTemplate>
                                                    <EditItemTemplate>
                                                        <telerik:RadNumericTextBox runat="server" RenderMode="Lightweight" Skin="Metro" ID="radImpAutorizado" Width="100px" ShowSpinButtons="true" MaxValue="9999999999.99" MinValue="0" NumberFormat-DecimalDigits="2" NumberFormat-DecimalSeparator="."></telerik:RadNumericTextBox>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <%--
                                                <telerik:GridNumericColumn FilterCheckListEnableLoadOnDemand="true" Visible="false" Resizable="true" DataField="utilidad" HeaderText="Utilidad" SortExpression="utilidad" UniqueName="utilidad" ReadOnly="true" DataFormatString="{0:C2}" Aggregate="Sum" FooterAggregateFormatString="{0:C2}"/>
                                                <telerik:GridTemplateColumn FilterCheckListEnableLoadOnDemand="true" Visible="false" Resizable="true" DataField="refestatus" HeaderText="Estatus" SortExpression="refestatus" UniqueName="refestatus">
                                                    <ItemTemplate>                                                        
                                                        <%# Eval("estatus") %>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>                                                        
                                                        <telerik:RadDropDownList RenderMode="Lightweight" runat="server" ID="ddlEstatus" Skin="Metro" SelectedValue='<%# Eval("refestatus") %>' >
                                                            <Items> 
                                                                <telerik:DropDownListItem Value="NA" Text="No Aplica"  />
                                                                <telerik:DropDownListItem Value="EV" Text="Evaluación" />
                                                                <telerik:DropDownListItem Value="RP" Text="Reparación" />
                                                                <telerik:DropDownListItem Value="CO" Text="Compra" />
                                                                <telerik:DropDownListItem Value="AU" Text="Autorizado" />
                                                                <telerik:DropDownListItem Value="CA" Text="Cancelada" />
                                                            </Items>
                                                        </telerik:RadDropDownList>                                                        
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn FilterCheckListEnableLoadOnDemand="true" Visible="false" Resizable="true" DataField="refEstatusSolicitud" HeaderText="Estatus Refacción" SortExpression="refEstatusSolicitud" UniqueName="refEstatusSolicitud">
                                                    <ItemTemplate>                                                        
                                                        <%# Eval("descripEstatus") %>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <telerik:RadDropDownList RenderMode="Lightweight" runat="server" ID="ddlEstatusRefaccion" DataValueField="refEstatusSolicitud" Skin="Metro" DataTextField="staDescripcion" DataSourceID="SqlDataSource3"/>                                                        
                                                        <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="SELECT staRefID as refEstatusSolicitud, staDescripcion FROM Rafacciones_Estatus"/>                                                        
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>    
                                                --%>
                                                <telerik:GridCheckBoxColumn FilterCheckListEnableLoadOnDemand="true" Resizable="true" Visible="false" DataField="facturado" HeaderText="Facturado" SortExpression="facturado" UniqueName="facturado" ReadOnly="true"/>
                                                <telerik:GridCheckBoxColumn FilterCheckListEnableLoadOnDemand="true" DataField="pTerminado" Visible="false" HeaderText="Presupuesto Terminado" SortExpression="pTerminado" UniqueName="pTerminado" ReadOnly="true" />
                                                <telerik:GridCheckBoxColumn FilterCheckListEnableLoadOnDemand="true" Resizable="true" Visible="false" DataField="ajustado" HeaderText="Ajustado" SortExpression="ajustado" UniqueName="ajustado" />
                                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" Resizable="true" DataField="nombre_usuario" HeaderText="Usuario Ajuste" SortExpression="nombre_usuario" UniqueName="nombre_usuario" ReadOnly="true"/>
                                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" Resizable="true" DataField="fechaajustado" HeaderText="Fecha Ajuste" SortExpression="fechaajustado" UniqueName="fechaajustado" ReadOnly="true"/>
                                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" Resizable="true" DataField="horaajustado" HeaderText="Hora Ajuste" SortExpression="horaajustado" UniqueName="horaajustado" ReadOnly="true"/>
                                            </Columns>
                                            <NoRecordsTemplate>
                                                <asp:Label ID="lblnoReecMo" runat="server" Text="No existen Refacciones registradas" CssClass="errores"></asp:Label>
                                            </NoRecordsTemplate>
                                        </MasterTableView>                                
                                        <ClientSettings AllowKeyboardNavigation="true">
                                            <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" ></Scrolling>
                                            <Selecting AllowRowSelect="true" />
                                        </ClientSettings>
                                        <PagerStyle  PageSizeControlType="RadDropDownList" Mode="NextPrevAndNumeric"></PagerStyle>
                                    </telerik:RadGrid>
                                    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>"                                
                                        SelectCommand="select r.refOrd_Id,r.refCantidad,r.refDescripcion,r.refPrecioVenta,
case @acceso when 0 then 
case r.refEstatus when 'AU' THEN (CASE WHEN r.refProveedor=0 THEN r.id_cliprov_cotizacion else r.refProveedor end) else
(select isnull((SELECT id_cliprov from Cotizacion_Detalle where no_orden=r.ref_no_orden and id_empresa=r.ref_id_empresa and id_taller=ref_id_taller and id_cotizacion=r.id_cotizacion and id_cotizacion_detalle=r.refOrd_id and id_cliprov=r.id_cliprov_cotizacion ),0)) end
else (case r.refEstatus when 'AU' THEN (CASE WHEN r.refProveedor=0 THEN r.id_cliprov_cotizacion else r.refProveedor end) else r.refProveedor end ) end  as refProveedor,
(select razon_social from Cliprov where tipo='P' and id_cliprov in (select case @acceso when 0 then 
case r.refEstatus when 'AU' THEN (CASE WHEN r.refProveedor=0 THEN r.id_cliprov_cotizacion else r.refProveedor end) else
(select isnull((SELECT id_cliprov from Cotizacion_Detalle where no_orden=r.ref_no_orden and id_empresa=r.ref_id_empresa and id_taller=ref_id_taller and id_cotizacion=r.id_cotizacion and id_cotizacion_detalle=r.refOrd_id and id_cliprov=r.id_cliprov_cotizacion ),0)) end
else (case r.refEstatus when 'AU' THEN (CASE WHEN r.refProveedor=0 THEN r.id_cliprov_cotizacion else r.refProveedor end) else r.refProveedor end ) end)) as razon_social,
case @acceso when 0 then 
case r.refEstatus when 'AU' THEN r.refCosto else
(select isnull((SELECT costo_unitario from Cotizacion_Detalle where no_orden=r.ref_no_orden and id_empresa=r.ref_id_empresa and id_taller=ref_id_taller and id_cotizacion=r.id_cotizacion and id_cotizacion_detalle=r.refOrd_id and id_cliprov=r.id_cliprov_cotizacion ),0)) end
else r.refCosto end  as refCosto,
case @acceso when 0 then 
case r.refEstatus when 'AU' THEN 
(SELECT isnull((SELECT porc_desc from Cotizacion_Detalle where no_orden=r.ref_no_orden and id_empresa=r.ref_id_empresa and id_taller=ref_id_taller and id_cotizacion=r.id_cotizacion_autorizada and id_cotizacion_detalle=r.refOrd_id and id_cliprov=r.refProveedor ),0))
else
(select isnull((SELECT porc_desc from Cotizacion_Detalle where no_orden=r.ref_no_orden and id_empresa=r.ref_id_empresa and id_taller=ref_id_taller and id_cotizacion=r.id_cotizacion and id_cotizacion_detalle=r.refOrd_id and id_cliprov=r.id_cliprov_cotizacion ),0)) end
else
(SELECT isnull((SELECT porc_desc from Cotizacion_Detalle where no_orden=r.ref_no_orden and id_empresa=r.ref_id_empresa and id_taller=ref_id_taller and id_cotizacion=r.id_cotizacion_autorizada and id_cotizacion_detalle=r.refOrd_id and id_cliprov=r.refProveedor ),0)) end
    as porc_desc,
case @acceso when 0 then 
case r.refEstatus when 'AU' THEN 
(SELECT isnull((SELECT importe_desc from Cotizacion_Detalle where no_orden=r.ref_no_orden and id_empresa=r.ref_id_empresa and id_taller=ref_id_taller and id_cotizacion=r.id_cotizacion_autorizada and id_cotizacion_detalle=r.refOrd_id and id_cliprov=r.refProveedor ),0))
else
(SELECT isnull((SELECT importe_desc from Cotizacion_Detalle where no_orden=r.ref_no_orden and id_empresa=r.ref_id_empresa and id_taller=ref_id_taller and id_cotizacion=r.id_cotizacion and id_cotizacion_detalle=r.refOrd_id and id_cliprov=r.id_cliprov_cotizacion ),0)) end
else
(SELECT isnull((SELECT importe_desc from Cotizacion_Detalle where no_orden=r.ref_no_orden and id_empresa=r.ref_id_empresa and id_taller=ref_id_taller and id_cotizacion=r.id_cotizacion_autorizada and id_cotizacion_detalle=r.refOrd_id and id_cliprov=r.refProveedor ),0)) end
as importeDesc,
case @acceso when 0 then 
case r.refEstatus when 'AU' THEN 
(SELECT isnull((SELECT importe from Cotizacion_Detalle where no_orden=r.ref_no_orden and id_empresa=r.ref_id_empresa and id_taller=ref_id_taller and id_cotizacion=r.id_cotizacion_autorizada and id_cotizacion_detalle=r.refOrd_id and id_cliprov=r.refProveedor ),0))
else
(SELECT isnull((SELECT importe from Cotizacion_Detalle where no_orden=r.ref_no_orden and id_empresa=r.ref_id_empresa and id_taller=ref_id_taller and id_cotizacion=r.id_cotizacion and id_cotizacion_detalle=r.refOrd_id and id_cliprov=r.id_cliprov_cotizacion ),0)) end
else
(SELECT isnull((SELECT importe from Cotizacion_Detalle where no_orden=r.ref_no_orden and id_empresa=r.ref_id_empresa and id_taller=ref_id_taller and id_cotizacion=r.id_cotizacion_autorizada and id_cotizacion_detalle=r.refOrd_id and id_cliprov=r.refProveedor ),0)) end
as importeCosto,
isnull((r.refCantidad*r.refPrecioVenta),0) as importeVenta,r.refEstatus,
isnull((SELECT estatus from Cotizacion_Detalle where no_orden=r.ref_no_orden and id_empresa=r.ref_id_empresa and id_taller=ref_id_taller and id_cotizacion=r.id_cotizacion and id_cotizacion_detalle=r.refOrd_id and id_cliprov=r.refProveedor ),'CAN') as estatusRef,r.refEstatusSolicitud,
(select staDescripcion from rafacciones_estatus where staRefId=r.refEstatusSolicitud) as descripEstatus,
(isnull((r.refCantidad*r.refPrecioVenta),0)-
(select case @acceso when 0 then 
case r.refEstatus when 'AU' THEN 
(SELECT isnull((SELECT importe from Cotizacion_Detalle where no_orden=r.ref_no_orden and id_empresa=r.ref_id_empresa and id_taller=ref_id_taller and id_cotizacion=r.id_cotizacion_autorizada and id_cotizacion_detalle=r.refOrd_id and id_cliprov=r.refProveedor ),0))
else
(SELECT isnull((SELECT importe from Cotizacion_Detalle where no_orden=r.ref_no_orden and id_empresa=r.ref_id_empresa and id_taller=ref_id_taller and id_cotizacion=r.id_cotizacion and id_cotizacion_detalle=r.refOrd_id and id_cliprov=r.id_cliprov_cotizacion ),0)) end
else
(SELECT isnull((SELECT importe from Cotizacion_Detalle where no_orden=r.ref_no_orden and id_empresa=r.ref_id_empresa and id_taller=ref_id_taller and id_cotizacion=r.id_cotizacion_autorizada and id_cotizacion_detalle=r.refOrd_id and id_cliprov=r.refProveedor ),0)) end)
) as utilidad,
r.refPorcentSobreCosto as porcSobre,
case r.refEstatus when 'NA' then 'No Aplica' when 'EV' then 'Evaluación' when 'RP' THEN 'Reparación' when 'CO' then 'Compra' when 'CA' THEN 'Cancelada' when 'AP' then 'Aplicada' when 'AU' then 'Autorizada' else '' end as estatus,
cast((case isnull(r.idcfd,0) when 0 then 0 else 1 end) as bit) as facturado,isnull(r.ajustado,0) as ajustado,isnull(r.usuarioajustado,0) as usuarioajustado,u.nombre_usuario,convert(char(10),r.fechaajustado,120) as fechaajustado,convert(char(8),r.horaajustado,120) as horaajustado,cast((case r.estatus_presupuesto when 'T' then 1 Else 0 end) as bit) as pTerminado
from Refacciones_Orden r
left join Cliprov c on c.id_cliprov=r.refProveedor and c.tipo='P'
left join Usuarios u on r.usuarioajustado=u.id_usuario
where r.ref_no_orden=@orden and r.ref_id_empresa=@empresa and r.ref_id_taller=@taller and r.refEstatusSolicitud <> 11 and r.proceso is null "
                                        UpdateCommand="UPDATE Refacciones_Orden SET refPrecioVenta=@refPrecioVenta,ajustado=@ajustado,fechaajustado=@fecha,horaajustado=@hora,usuarioajustado=@usuario  WHERE refOrd_Id = @refOrd_Id AND ref_no_orden = @orden and ref_id_empresa=@empresa and ref_id_taller=@taller and (ajustado=0 and cast((case when idcfd is null then 0 when idcfd>0 then 1 else 0 end) as bit)=0)">
                                        <SelectParameters>
                                            <asp:QueryStringParameter Name="orden" QueryStringField="o" DbType="Int32" DefaultValue="0" />
                                            <asp:QueryStringParameter Name="empresa" QueryStringField="e" DbType="Int32" DefaultValue="0" />
                                            <asp:QueryStringParameter Name="taller" QueryStringField="t" DbType="Int32" DefaultValue="0" />
                                            <asp:Parameter Name="acceso" DbType="Int32" DefaultValue="0" />
                                        </SelectParameters>                                
                                        <UpdateParameters>                                            
                                            <asp:Parameter Name="refOrd_Id" Type="Int32"></asp:Parameter>
                                            <asp:Parameter Name="refestatus" Type="String"></asp:Parameter>
                                            <asp:Parameter Name="refEstatusSolicitud" Type="Int32"></asp:Parameter>
                                            <asp:Parameter Name="refPrecioVenta" Type="Decimal"></asp:Parameter>                                            
                                            <asp:QueryStringParameter Name="orden" QueryStringField="o" DbType="Int32" DefaultValue="0" />
                                            <asp:QueryStringParameter Name="empresa" QueryStringField="e" DbType="Int32" DefaultValue="0" />
                                            <asp:QueryStringParameter Name="taller" QueryStringField="t" DbType="Int32" DefaultValue="0" />
                                            <asp:QueryStringParameter Name="usuario" QueryStringField="u" DbType="Int32" DefaultValue="0" />
                                            <asp:Parameter Name="ajustado" Type="Boolean" />
                                            <asp:ControlParameter Name="fecha" ControlID="lblFecha" PropertyName="Text" />
                                            <asp:ControlParameter Name="hora" ControlID="lblHora" PropertyName="Text" />
                                        </UpdateParameters>
                                    </asp:SqlDataSource>
                                </div>
                            </div>
                        </telerik:RadPageView>
                    </telerik:RadMultiPage>
                </div>
            </div>    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblFecha" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="lblHora" runat="server" Visible="false"></asp:Label>
            <div class="row pad1m">
                <div class="col-lg-6 col-sm-6 text-center">
                    <asp:Label ID="lblTotalMo" runat="server" CssClass="t14 text-primary textoBold" ></asp:Label>
                </div>
                <div class="col-lg-6 col-sm-6 text-center">
                    <asp:Label ID="lblTotalRef" runat="server" CssClass="t14 text-primary textoBold" ></asp:Label>
                </div>
                <div class="col-lg-12 col-sm-12">
                    <asp:Label ID="lblError" runat="server" CssClass="errores" ></asp:Label>
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
    <%--<div class="row">
        <div class="col-lg-12 col-sm-12 text-center">
            <asp:LinkButton ID="lnkGenerarOrdenCompra" runat="server" CssClass="btn btn-info t14" OnClick="lnkGenerarOrdenCompra_Click" Visible="false"><i class="fa fa-shopping-cart"></i>&nbsp;<span>Generar Orden de Compra</span></asp:LinkButton>
        </div>
    </div>

    <br />
            <div class="row">
                <div class="col-lg-12 col-sm-12 text-center pad1m alert-info">
                    <asp:Label runat="server" ID="lblTituloEnvios" Text="Envio de Orden de Compra" ></asp:Label>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-12 col-sm-12 text-center">
                    <asp:Label ID="lblCotizacion" runat="server" Visible="false" />
                    
                    <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid3" runat="server"  Culture="es-Mx" Skin="Metro" CssClass="col-lg-12 col-sm-12" AllowAutomaticUpdates="True" AutoGenerateColumns="False" 
                                    AllowAutomaticInserts="false" AllowAutomaticDeletes="false" AllowPaging="True" PagerStyle-AlwaysVisible="true" DataSourceID="SqlDataSource10" AllowSorting="true" GroupingEnabled="false" PageSize="20">
                        <MasterTableView DataSourceID="SqlDataSource10" AutoGenerateColumns="False" DataKeyNames="no_orden,id_empresa,id_taller,id_compra,id_cliprov"  CommandItemDisplay="Bottom" HorizontalAlign="NotSet" EditMode="Batch">
                            <BatchEditingSettings EditType="Row" />
                            <CommandItemStyle CssClass="text-right" />
                            <CommandItemSettings SaveChangesText="Guardar Cambios" ShowAddNewRecordButton="false"  ShowRefreshButton="false" ShowSaveChangesButton="true" CancelChangesText="Cancelar Cambios"/>
                            <Columns>
                                <telerik:GridBoundColumn UniqueName="id_compra" HeaderText="id_compra" SortExpression="id_compra" Visible="false" DataField="id_compra" ReadOnly="true"></telerik:GridBoundColumn>
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
                                        <asp:LinkButton ID="lnkReenviar" runat="server" CssClass="btn btn-success colorBlanco" CommandArgument='<%# Eval("id_compra")+";"+Eval("id_cliprov")+";"+Eval("correo") %>' OnClick="lnkReenviar_Click"><i class="fa fa-mail-reply"></i></asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>                       
                            </Columns>                            
                            <NoRecordsTemplate>
                                <asp:Label runat="server" ID="lblVacio" Text="No existen ordenes de compra enviadas" CssClass="errores"></asp:Label>
                            </NoRecordsTemplate>
                        </MasterTableView>                        
                        <ClientSettings AllowKeyboardNavigation="true">
                            <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" ></Scrolling>
                            <Selecting AllowRowSelect="true" /> 
                        </ClientSettings>                        
                        <PagerStyle PageSizeControlType="RadDropDownList" Mode="NextPrevAndNumeric"></PagerStyle>
                    </telerik:RadGrid>
                    <asp:SqlDataSource runat="server" ID="SqlDataSource10" ConnectionString='<%$ ConnectionStrings:PVW %>'
                            SelectCommand="select c.no_orden,c.id_empresa,c.id_taller, c.id_compra, c.id_cliprov,p.razon_social,convert(char(10),c.fecha,120) as fecha,convert(char(8),c.hora,120) as hora,c.correo,u.nombre_usuario,c.motivo_fallo,c.enviado
from compras_enviadas c 
inner join Cliprov p on p.id_cliprov = c.id_cliprov and p.tipo='P'
inner join usuarios u on u.id_usuario=c.usuario
where c.no_orden=@no_orden and c.id_empresa=@id_empresa and c.id_taller=@id_taller"
                            UpdateCommand ="update compras_enviadas set correo=@correo where no_orden=@no_orden and id_empresa=@id_empresa and id_taller=@id_taller and id_compra=@id_compra and id_cliprov=@id_cliprov update cliprov set correo=@correo where id_cliprov=@id_cliprov and tipo='P' ">
                            <SelectParameters>
                                <asp:QueryStringParameter QueryStringField="o" Name="no_orden"></asp:QueryStringParameter>
                                <asp:QueryStringParameter QueryStringField="t" Name="id_taller"></asp:QueryStringParameter>
                                <asp:QueryStringParameter QueryStringField="e" Name="id_empresa"></asp:QueryStringParameter>
                            </SelectParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="correo" Type="String" />
                            <asp:Parameter Name="no_orden" Type="Int32" />
                            <asp:Parameter Name="id_empresa" Type="Int32" />
                            <asp:Parameter Name="id_taller" Type="Int32" />
                            <asp:Parameter Name="id_compra" Type="Int32" />
                            <asp:Parameter Name="id_cliprov" Type="Int32" />                            
                        </UpdateParameters>
                        </asp:SqlDataSource>
                </div>
            </div>--%>
            <br /><br /><br />

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
                            <asp:Label ID="lblTotOrden" Visible="false" runat="server"></asp:Label>
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

</asp:Content>

