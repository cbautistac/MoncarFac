<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Valuacion.aspx.cs" Inherits="Valuacion" MasterPageFile="~/AdmonOrden.master" Culture="es-Mx" UICulture="es-Mx"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function abreWin() {
            var oWnd = $find("<%=modalPopup.ClientID%>");
            oWnd.setUrl('');
            oWnd.show();
        }        
        function cierraWin() {
            var oWnd = $find("<%=modalPopup.ClientID%>");
            oWnd.close();
        }
        function abreWin2() {
            var oWnd = $find("<%=RadWindow1.ClientID%>");
            oWnd.setUrl('');
            oWnd.show();
        }        
        function cierraWin2() {
            var oWnd = $find("<%=RadWindow1.ClientID%>");
            oWnd.close();
        }
</script>

    <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1" Skin="MetroTouch"></telerik:RadAjaxLoadingPanel>

    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-lg-12 col-sm-12 text-center alert-info">
                    <h3>
                        <i class="fa fa-check-square-o"></i></i>&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblTit" runat="server" Text="Validación Refacciones"></asp:Label>
                    </h3>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-12 col-sm-12 text-center">                    
                    <asp:Label ID="lblError" runat="server" CssClass="errores" />
                </div>
            </div>

            <asp:Panel runat="server" ID="Panel2" CssClass="paneles col-lg-12 col-sm-12" ScrollBars="Auto">
                <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" GridLines="None" runat="server" PageSize="200" AllowAutomaticUpdates="True" 
                                        AllowPaging="True" AutoGenerateColumns="False" PagerStyle-AlwaysVisible="true" AllowFilteringByColumn="true"  EnableHeaderContextMenu="true" EnableHeaderContextFilterMenu="true"
                                        AllowSorting="true" DataSourceID="sqlDSRefOrden" Skin="Metro" AllowAutomaticInserts="false" AllowAutomaticDeletes="false" OnItemDataBound="RadGrid1_ItemDataBound" >
                                        <MasterTableView CommandItemDisplay="Bottom" DataKeyNames="refOrd_Id,refProveedor"  HorizontalAlign="NotSet" ShowFooter="true" EditMode="Batch" AutoGenerateColumns="False" DataSourceID="sqlDSRefOrden">                                    
                                            <BatchEditingSettings EditType="Row" />
                                            <CommandItemStyle CssClass="text-right" />
                                            <CommandItemSettings SaveChangesText="Guardar Cambios" ShowAddNewRecordButton="false"  ShowRefreshButton="false" ShowSaveChangesButton="true" CancelChangesText="Cancelar Cambios"/>
                                            <Columns>
                                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" HeaderStyle-Width="10%"  DataField="refCantidad" HeaderText="Cantidad" SortExpression="refCantidad" UniqueName="refCantidad" ReadOnly="true"/>
                                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="refDescripcion" HeaderText="Refaccion" SortExpression="refDescripcion" UniqueName="refDescripcion" ReadOnly="true"/>
                                                <telerik:GridTemplateColumn FilterCheckListEnableLoadOnDemand="true" DataField="refProveedor" SortExpression="refProveedor" UniqueName="refProveedor" HeaderText="Proveedor">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEstatusText" runat="server" Text='<%# Bind("provRazSoc")%>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn FilterCheckListEnableLoadOnDemand="true" DataField="estatus" SortExpression="estatus" UniqueName="estatus" HeaderText="Estatus">
                                                    <ItemTemplate>
                                                        <%# Eval("estatus") %>
                                                        <asp:Label ID="lblEstatusProveedorEdita" Visible="false" runat="server" Text='<%# Bind("estatus")%>' />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <telerik:RadDropDownList RenderMode="Lightweight" ID="ddlEstatusRefEdit" Skin="Metro" runat="server" SelectedValue='<%# Eval("refestatus") %>' >
                                                            <Items>
                                                                <telerik:DropDownListItem Text="No Aplica" Value="NA" />
                                                                <telerik:DropDownListItem Text="Evaluación" Value="EV" />
                                                                <telerik:DropDownListItem Text="Reparación" Value="RP" />
                                                                <telerik:DropDownListItem Text="Compra" Value="CO" />                                                    
                                                                <telerik:DropDownListItem Text="Cancelada" Value="CA" />
                                                            </Items>
                                                        </telerik:RadDropDownList>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn FilterCheckListEnableLoadOnDemand="true" DataField="estatus_ref" SortExpression="estatus_ref" UniqueName="estatus_ref" HeaderText="Estatus Refaccion">
                                                    <ItemTemplate>
                                                        <%# Eval("estatus_ref") %>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <telerik:RadDropDownList RenderMode="Lightweight" runat="server" ID="ddlEstatus" Skin="Metro" SelectedValue='<%# Eval("refEstatusSolicitud") %>' >
                                                            <Items>
                                                                <telerik:DropDownListItem Text="No Solicitado" Value="1" />
                                                                <telerik:DropDownListItem  Text="Solicitado" Value="2" />
                                                                <telerik:DropDownListItem  Text="Surtido" Value="3" />
                                                                <telerik:DropDownListItem  Text="Devolucion" Value="4" />                                                    
                                                                <telerik:DropDownListItem  Text="Cancelado" Value="5" />
                                                                <telerik:DropDownListItem  Text="Cambio de Pieza" Value="6" />
                                                                <telerik:DropDownListItem  Text="Pendiente" Value="7" />
                                                                <telerik:DropDownListItem  Text="Sin Vale" Value="8" />
                                                                <telerik:DropDownListItem  Text="Reparado" Value="9" />
                                                                <telerik:DropDownListItem  Text="Mismo" Value="10" />
                                                                <telerik:DropDownListItem  Text="No Autorizado" Value="11" />
                                                                <telerik:DropDownListItem  Text="Prestamo" Value="12" />
                                                                <telerik:DropDownListItem  Text="Cambio de Vale" Value="13" />
                                                                <telerik:DropDownListItem  Text="Almacén" Value="14" />
                                                                <telerik:DropDownListItem  Text="TOT" Value="15" />
                                                                <telerik:DropDownListItem  Text="Maquila" Value="16" />
                                                                <telerik:DropDownListItem  Text="Extremo" Value="17" />
                                                                <telerik:DropDownListItem  Text="Compras Chofer" Value="18" />
                                                                <telerik:DropDownListItem  Text="Disponible en Taller" Value="19" />
                                                            </Items>
                                                        </telerik:RadDropDownList>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn Visible="true" HeaderStyle-Width="10%"> 
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnCambiaProveedor" runat="server" OnClick="lnkCambiarProveedor_Click" CssClass="btn btn-danger t14" CommandArgument='<%# Eval("refOrd_Id") %>'><i class="fa fa-check"></i></asp:LinkButton>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
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
                                <asp:SqlDataSource runat="server" ID="sqlDSRefOrden" ConnectionString='<%$ ConnectionStrings:PVW %>'
                                    SelectCommand="SELECT [refOrd_Id],[refCantidad],[refDescripcion],refProveedor,  (SELECT razon_social FROM Cliprov WHERE id_cliprov = ro.refProveedor AND tipo = 'P') AS provRazSoc, [ref_no_orden],
                                        refCosto, 
                                        (select isnull((select top 1 porc_desc from Cotizacion_Detalle where no_orden=RO.ref_no_orden and id_empresa=ro.ref_id_empresa and id_taller=ro.ref_id_taller and id_cotizacion_detalle=ro.refOrd_Id and id_cliprov=ro.refProveedor and estatus in('COT','SOL') ORDER BY fecha_captura DESC),0)) as porc_desc, 
                                        (select isnull((select top 1 importe_desc from Cotizacion_Detalle where no_orden=RO.ref_no_orden and id_empresa=ro.ref_id_empresa and id_taller=ro.ref_id_taller and id_cotizacion_detalle=ro.refOrd_Id and id_cliprov=ro.refProveedor and estatus in('COT','SOL') ORDER BY fecha_captura DESC),0)) as importe_desc, 
                                        refPorcentSobreCosto,refPrecioVenta,
                                        (select isnull((select top 1 importe from Cotizacion_Detalle where no_orden=RO.ref_no_orden and id_empresa=ro.ref_id_empresa and id_taller=ro.ref_id_taller and id_cotizacion_detalle=ro.refOrd_Id and id_cliprov=ro.refProveedor and estatus in('COT','SOL') ORDER BY fecha_captura DESC),0)) as importe_compra,
                                        (isnull(refPrecioVenta,0)*refCantidad) as importe ,
                                        ((isnull(refPrecioVenta,0)*refCantidad)-(isnull(refCosto,0)*refCantidad)) as utilidad,
                                        case when refestatus='AU' then 'NA' else refestatus end as refestatus,case refestatus when 'NA' then 'No Aplica' when 'CO' then 'Compra' when 'RP' then 'Reparación' when 'EV' then 'Evaluación' when 'AP' then 'Aplicado' when 'AU' then 'Autorizado' when 'FA' then 'Facturado' WHEN 'CA' THEN 'Cancelada' else 'No Aplica' end as estatus,  
                                        refEstatusSolicitud,(select stadescripcion from Rafacciones_Estatus where staRefID=ro.refEstatusSolicitud) as estatus_ref,refObservaciones,
                                        (select staRefID from Rafacciones_Estatus where staRefID=ro.refEstatusSolicitud) as staRefID,isnull(ro.idCfd,0) as idCfd
                                        FROM [Refacciones_Orden] ro 
                                        WHERE ([ref_no_orden] = @ref_no_orden) and [ref_id_empresa]=@ref_id_empresa and [ref_id_taller]=@ref_id_taller and ro.proceso is null order by refOrd_id"
                                    UpdateCommand="update refacciones_orden set refEstatus=@estatus ,refEstatusSolicitud=@estatus_ref where ref_no_orden=@ref_no_orden and ref_id_empresa=@ref_id_empresa and ref_id_taller=ref_id_taller and refOrd_id=@refOrd_id">
                                    <SelectParameters>
                                        <asp:QueryStringParameter Name="ref_no_orden" QueryStringField="o" Type="Int32" />
                                        <asp:QueryStringParameter Name="ref_id_empresa" QueryStringField="e" Type="Int32" />
                                        <asp:QueryStringParameter Name="ref_id_taller" QueryStringField="t" Type="Int32" />
                                    </SelectParameters>
                                    <UpdateParameters>
                                        <asp:QueryStringParameter Name="ref_no_orden" QueryStringField="o" Type="Int32" />
                                        <asp:QueryStringParameter Name="ref_id_empresa" QueryStringField="e" Type="Int32" />
                                        <asp:QueryStringParameter Name="ref_id_taller" QueryStringField="t" Type="Int32" />
                                        <asp:Parameter Name="estatus" Type="String" />
                                        <asp:Parameter Name="estatus_ref" Type="Int32" />
                                        <asp:Parameter Name="refOrd_id" Type="Int32" />
                                    </UpdateParameters>
                                </asp:SqlDataSource>
            </asp:Panel>

            <%--       
            <asp:Panel runat="server" ID="Panel1" CssClass="paneles col-lg-12 col-sm-12" ScrollBars="Auto">
                <div class="table-responsive">
                                <asp:GridView ID="grdRefacOrd" Visible="true" runat="server"
                                    EmptyDataText="No se han agregado refacciones a la orden."
                                    CssClass="table table-bordered" EmptyDataRowStyle-CssClass="errores" AutoGenerateColumns="False"
                                    DataKeyNames="refOrd_Id" DataSourceID="sqlDSRefOrden"  AllowSorting="True" OnRowCommand="grdRefacOrd_RowCommand"
                                    OnRowDeleted="grdRefacOrd_RowDeleted" 
                                    OnRowDataBound="grdRefacOrd_RowDataBound" ShowFooter="True">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Cantidad" InsertVisible="False" SortExpression="refCantidad">
                                            <EditItemTemplate>
                                                <asp:Label runat="server" Text='<%# Bind("refCantidad") %>' ID="Label12"></asp:Label>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Bind("refCantidad") %>' ID="Label2"></asp:Label>
                                            </ItemTemplate>
                                             <FooterTemplate>
                                                <asp:Label ID="lblTot" runat="server" Text="Total:"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Refacción" InsertVisible="False" SortExpression="refDescripcion">
                                            <EditItemTemplate>
                                                 <asp:Label runat="server" Text='<%# Bind("refDescripcion") %>' ID="Label311"></asp:Label>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Bind("refDescripcion") %>' ID="Label31"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotRef" runat="server" ></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="provRazSoc" SortExpression="provRazSoc" HeaderText="Proveedor" ReadOnly="true" />
                                        <asp:TemplateField HeaderText="Cost. Unit." SortExpression="refCosto" Visible="false">
                                            <EditItemTemplate>
                                                <asp:Label ID="Label6" runat="server" Text='<%# Eval("refCosto", "{0:C2}") %>'></asp:Label>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label8" runat="server" Text='<%# Bind("refCosto", "{0:C2}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="porc_desc" SortExpression="porc_desc" HeaderText="% Dto." ReadOnly="true" Visible="false"/>
                                        <asp:BoundField DataField="importe_desc" SortExpression="importe_desc" HeaderText="Impte. Dto." ReadOnly="true" DataFormatString="{0:C2}" Visible="false"/>
                                        <asp:TemplateField HeaderText="Impte. Cmpra." SortExpression="importe_compra" Visible="false">
                                            <EditItemTemplate>
                                                <asp:Label ID="Label2" runat="server" 
                                                    Text='<%# Eval("importe_compra", "{0:C2}") %>'></asp:Label>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label5" runat="server" 
                                                    Text='<%# Bind("importe_compra", "{0:C2}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalCompra" runat="server" ></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="% S.C." SortExpression="refPorcentSobreCosto" Visible="false">
                                            <EditItemTemplate>                                                
                                                <asp:TextBox ID="txtPorcSob" runat="server" Text='<%# Bind("refPorcentSobreCosto") %>' CssClass="input-mini" MaxLength="9"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorSC" ControlToValidate="txtPorcSob" runat="server" ValidationExpression="^\d{1,3}(\.\d{1,2})?$" ErrorMessage="El porcentaje de sobre costo solo puede contener dígitos y un punto decimal" Text="*" CssClass="errores" ></asp:RegularExpressionValidator>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label4" runat="server" Text='<%# Bind("refPorcentSobreCosto") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Prec. Unit. Autorizado" InsertVisible="False" SortExpression="refPrecioVenta" Visible="false">
                                            <EditItemTemplate>
                                                <asp:TextBox runat="server" ID="txtPrecioM" MaxLength="8" CssClass="alingMiddle input-small" Text='<%# Bind("refPrecioVenta") %>' />
                                                <cc1:TextBoxWatermarkExtender ID="txtPrecioM_TextBoxWatermarkExtender" runat="server" BehaviorID="txtPrecioM_TextBoxWatermarkExtender" TargetControlID="txtPrecioM" WatermarkCssClass="water input-small" WatermarkText="Precio" />
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" BehaviorID="txtPrecioM_FilteredTextBoxExtender" TargetControlID="txtPrecioM" FilterType="Numbers, Custom" ValidChars="." />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Bind("refPrecioVenta","{0:C2}") %>' ID="Label3"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="Impte. Autorizado" SortExpression="importe" Visible="false">
                                            <EditItemTemplate>
                                                <asp:Label ID="Label4" runat="server" Text='<%# Eval("importe", "{0:C2}") %>'></asp:Label>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label6" runat="server" Text='<%# Bind("importe", "{0:C2}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalVenta" runat="server" ></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Utilidad" SortExpression="utilidad" Visible="false">
                                            <EditItemTemplate>
                                                <asp:Label ID="Label5" runat="server" Text='<%# Eval("utilidad", "{0:C2}") %>'></asp:Label>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label7" runat="server" Text='<%# Bind("utilidad", "{0:C2}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalutilidad" runat="server" ></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Estatus" SortExpression="refestatus">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEstatus" runat="server" Text='<%# Bind("refestatus") %>' Visible="false" />
                                                <asp:Label ID="lblEstatusText" runat="server" Text='<%# Bind("estatus") %>' />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblEstatus" runat="server" Text='<%# Bind("refestatus") %>' Visible="false" />                                                
                                                <asp:DropDownList ID="ddlEstatusRefEdit" runat="server" SelectedValue='<%# Eval("refestatus") %>' >
                                                    <asp:ListItem Text="No Aplica" Value="NA" />
                                                    <asp:ListItem Text="Evaluación" Value="EV" />
                                                    <asp:ListItem Text="Reparación" Value="RP" />
                                                    <asp:ListItem Text="Compra" Value="CO" />                                                    
                                                    <asp:ListItem Text="Cancelada" Value="CA" />
                                                </asp:DropDownList>
                                            </EditItemTemplate>
                                            <FooterTemplate>                                        
                                                <asp:Label ID="lblPorcUtilidad" runat="server" ></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Estatus Refacción" SortExpression="estatus_ref">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("estatus_ref") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlEstatusRefaccion" runat="server" 
                                                    DataSourceID="SqlDataSource1" DataTextField="staDescripcion" 
                                                    DataValueField="staRefID" SelectedValue='<%# Bind("refEstatusSolicitud") %>'>
                                                </asp:DropDownList>
                                                <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                                                    ConnectionString="<%$ ConnectionStrings:PVW %>" 
                                                    SelectCommand="SELECT [staRefID], [staDescripcion] FROM [Rafacciones_Estatus]">
                                                </asp:SqlDataSource>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIdEstRef" runat="server" Text='<%# Eval("staRefID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="true">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkEditarR" runat="server" CausesValidation="False" CommandName="Edit" ToolTip="Editar" CssClass="btn btn-warning t14"><i class="fa fa-edit"></i></asp:LinkButton>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:LinkButton ID="lnkActualizaR" runat="server" CausesValidation="True" CommandName="Update" ToolTip="Guardar" CssClass="btn btn-success t14" CommandArgument='<%# Eval("refOrd_Id") %>' ValidationGroup="valRefaccMod"><i class="fa fa-save"></i></asp:LinkButton>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="false">
                                            <EditItemTemplate>
                                                <asp:LinkButton ID="lnkCancelarR" runat="server" CausesValidation="false" CommandName="Cancel" ToolTip="Cancelar" CssClass="btn btn-danger t14"><i class="fa fa-times-circle"></i></asp:LinkButton>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:LinkButton Visible="false" ID="lnkEliminarR" runat="server" CausesValidation="False" CommandName="Delete" ToolTip="Eliminar" CssClass="btn btn-danger t14" CommandArgument='<%# Eval("refOrd_Id") %>' OnClientClick="return confirm('¿Está seguro de eliminar la refacción?')"><i class="fa fa-trash"></i></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EditRowStyle CssClass="alert-warning" />
                                    <EmptyDataRowStyle CssClass="errores" />
                                </asp:GridView>
                            </div>
            </asp:Panel>
                  --%>

            <div class="row">
                <div class="col-lg-6 col-sm-6 text-center pad1m">
                    <asp:LinkButton ID="lnkImprimir" runat="server" CssClass="btn btn-info" OnClick="lnkImprimir_Click"><i class="fa fa-print"></i><span>&nbsp;Imprimir Seguimiento de Refacciones</span></asp:LinkButton>
                </div>
                <div class="col-lg-6 col-sm-6 text-center pad1m">
                    <asp:LinkButton ID="lnkConsultaNoAuto" runat="server" CssClass="btn btn-info" OnClick="lnkConsultaNoAuto_Click"><i class="fa fa-search"></i><span>&nbsp;Refacciones No Autorizadas</span></asp:LinkButton>
                </div>
            </div>

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

    <telerik:RadWindow RenderMode="Lightweight" ID="modalPopup" Title="Refacciones No Autorizadas" EnableShadow="true" Skin="Metro"
        Behaviors="Close,Maximize,Move,Resize" VisibleOnPageLoad="false" ShowContentDuringLoad="false" DestroyOnClose="true"
        Animation="Fade" runat="server" Modal="true" Width="1000px" Height="700px" Style="z-index: 1000;" >
        <ContentTemplate>
            <asp:UpdatePanel ID="UpdatePanel" runat="server">
                <ContentTemplate>
                    <div class="row ancho95 centrado">
                        <div class="col-lg-12 col-sm-12">
                            <div class="table-responsive">
                                <asp:GridView ID="GridView1" runat="server" EmptyDataText="No existen Refacciones no Autorizadas."
                                    CssClass="table table-bordered" EmptyDataRowStyle-CssClass="errores" AutoGenerateColumns="False"
                                    DataKeyNames="refOrd_Id" DataSourceID="SqlDataSource2"  AllowSorting="True">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Cantidad" InsertVisible="False" SortExpression="refCantidad">                                            
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Bind("refCantidad") %>' ID="Label2"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Refacción" InsertVisible="False" SortExpression="refDescripcion">                                            
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Bind("refDescripcion") %>' ID="Label31"></asp:Label>
                                            </ItemTemplate>                                           
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="provRazSoc" SortExpression="provRazSoc" HeaderText="Proveedor" ReadOnly="true" />                                        
                                        <asp:TemplateField HeaderText="Estatus" SortExpression="refestatus">
                                            <ItemTemplate>                                                
                                                <asp:Label ID="lblEstatusTextNoaut" runat="server" Text='<%# Bind("estatus") %>' />
                                            </ItemTemplate>                                            
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Estatus Refacción" SortExpression="estatus_ref">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("estatus_ref") %>'></asp:Label>
                                            </ItemTemplate>                                            
                                        </asp:TemplateField>                                        
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkReautoriza" runat="server" CausesValidation="False" CommandName="ReAut" ToolTip="Reactivar" OnClick="lnkReautoriza_Click" CssClass="btn btn-success t14" CommandArgument='<%# Eval("refOrd_Id") %>' OnClientClick="return confirm('¿Está seguro de reactivar la refacción?')"><i class="fa fa-reply"></i></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>                                        
                                    </Columns>
                                    <EditRowStyle CssClass="alert-warning" />
                                    <EmptyDataRowStyle CssClass="errores" />
                                </asp:GridView>
                                <asp:SqlDataSource runat="server" ID="SqlDataSource2" ConnectionString='<%$ ConnectionStrings:PVW %>'
                                    SelectCommand="SELECT [refOrd_Id],[refCantidad],[refDescripcion],refProveedor,  (SELECT razon_social FROM Cliprov WHERE id_cliprov = ro.refProveedor AND tipo = 'P') AS provRazSoc, [ref_no_orden],
                                        refCosto, 
                                        (select isnull((select top 1 porc_desc from Cotizacion_Detalle where no_orden=RO.ref_no_orden and id_empresa=ro.ref_id_empresa and id_taller=ro.ref_id_taller and id_cotizacion_detalle=ro.refOrd_Id and id_cliprov=ro.refProveedor and estatus in('COT','SOL') ORDER BY fecha_captura DESC),0)) as porc_desc, 
                                        (select isnull((select top 1 importe_desc from Cotizacion_Detalle where no_orden=RO.ref_no_orden and id_empresa=ro.ref_id_empresa and id_taller=ro.ref_id_taller and id_cotizacion_detalle=ro.refOrd_Id and id_cliprov=ro.refProveedor and estatus in('COT','SOL') ORDER BY fecha_captura DESC),0)) as importe_desc, 
                                        refPorcentSobreCosto,refPrecioVenta,
                                        (select isnull((select top 1 importe from Cotizacion_Detalle where no_orden=RO.ref_no_orden and id_empresa=ro.ref_id_empresa and id_taller=ro.ref_id_taller and id_cotizacion_detalle=ro.refOrd_Id and id_cliprov=ro.refProveedor and estatus in('COT','SOL') ORDER BY fecha_captura DESC),0)) as importe_compra,
                                        (isnull(refPrecioVenta,0)*refCantidad) as importe ,
                                        ((isnull(refPrecioVenta,0)*refCantidad)-(isnull(refCosto,0)*refCantidad)) as utilidad,
                                        refestatus,case refestatus when 'NA' then 'No Aplica' when 'CO' then 'Compra' when 'RP' then 'Reparación' when 'EV' then 'Evaluación' when 'AP' then 'Aplicado' when 'AU' then 'Autorizado' when 'FA' then 'Facturado' WHEN 'CA' THEN 'Cancelada' else 'No Aplica' end as estatus,  
                                        refEstatusSolicitud,(select stadescripcion from Rafacciones_Estatus where staRefID=ro.refEstatusSolicitud) as estatus_ref,refObservaciones,
                                        (select staRefID from Rafacciones_Estatus where staRefID=ro.refEstatusSolicitud) as staRefID
                                        FROM [Refacciones_Orden] ro 
                                        WHERE ([ref_no_orden] = @ref_no_orden) and [ref_id_empresa]=@ref_id_empresa and [ref_id_taller]=@ref_id_taller and refEstatusSolicitud= 11">
                                    <SelectParameters>
                                        <asp:QueryStringParameter Name="ref_no_orden" QueryStringField="o" Type="Int32" />
                                        <asp:QueryStringParameter Name="ref_id_empresa" QueryStringField="e" Type="Int32" />
                                        <asp:QueryStringParameter Name="ref_id_taller" QueryStringField="t" Type="Int32" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </div>
                
            
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
    </telerik:RadWindow>


    <telerik:RadWindow RenderMode="Lightweight" ID="RadWindow1" Title="Cambiar Proveedor" EnableShadow="true" Skin="Metro"
        Behaviors="Close,Maximize,Move,Resize" VisibleOnPageLoad="false" ShowContentDuringLoad="false" DestroyOnClose="true"
        Animation="Fade" runat="server" Modal="true" Width="500px" Height="350px" Style="z-index: 1000;" >
        <ContentTemplate>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <div class="row text-center">
                        <asp:Label ID="lblRefaccion" Visible="true" runat="server"></asp:Label>
                        <asp:Label ID="Label3" Text="Seleccione un proveedor" runat="server"></asp:Label>
                    </div>
                    <br /><br />
                    <div class="col-12-lg col-sm-12 text-center">
                        <telerik:RadComboBox ID="cmbProveedorEditar" DataSourceID="SqlDataSourceComboProveedorEdita" Filter="Contains" AllowCustomText="true" Width="250px" MaxHeight="300px"  DataValueField="id_cliprov" DataTextField="razon_social" runat="server"></telerik:RadComboBox>
                        <asp:SqlDataSource ID="SqlDataSourceComboProveedorEdita" runat="server" ConnectionString='<%$ ConnectionStrings:PVW %>' SelectCommand="select id_cliprov,razon_social from cliprov where tipo='P'"></asp:SqlDataSource>
                    </div>
                    <div class="col-12-lg col-sm-12 text-center">
                        <asp:Label ID="lblErrorCambio" runat="server" ></asp:Label>
                    </div>
                    <br /><br />
                    <div class="col-lg-12 col-sm-12 text-center">
                        <asp:LinkButton ID="btnCambiaProveedor" OnClick="btnCambiaProveedor_Click" runat="server" CssClass="btn btn-info">Cambiar Proveedor</asp:LinkButton>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
       </ContentTemplate>
   </telerik:RadWindow>

</asp:Content>