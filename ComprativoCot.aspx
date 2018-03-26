<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ComprativoCot.aspx.cs" Inherits="ComprativoCot"
    MasterPageFile="~/AdmonOrden.master" Culture="es-Mx" UICulture="es-Mx" %>

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

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-lg-12 col-sm-12 text-center alert-info">
                    <h3>
                        <i class="fa fa-dollar"></i></i>&nbsp;&nbsp;&nbsp;<asp:Label ID="lblTit" runat="server"
                            Text="Cotización"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkRefrescar" runat="server" OnClick="lnkRefrescar_Click"><i class="fa fa-refresh"></i></asp:LinkButton>
                    </h3>
                </div>
            </div>
            <div class="col-lg-12 col-sm-12 text-center">
                <div class="col-lg-1 col-sm-1 text-center">
                    
                    <%-- 
                         <asp:LinkButton ID="lnkAutTodo" runat="server" CssClass="btn btn-primary t14" OnClientClick="return confirm('¿Está seguro de autorizar todas las refacciones?. Una vez autorizadas solo podra cancelarlas')"
                        OnClick="lnkAutTodo_Click" ><i class="fa fa-list"></i><span>&nbsp;Autorizar Todo</span></asp:LinkButton>
                        --%> 

                    <asp:LinkButton ID="lnkAutTodo" runat="server" CssClass="btn btn-primary t14" 
                        OnClick="lnkAutTodo_Click" Visible="false" ><i class="fa fa-list"></i><span>&nbsp;Autorizar Todo</span></asp:LinkButton>
                       
                    
                    
                </div>
                <div class="col-lg-10 col-sm-10 text-center">
                    <asp:Label ID="lblProveedoresPendientes" runat="server" CssClass="errores"></asp:Label>
                </div>
                <div class="col-lg-1 col-sm-1 text-center">
                    <asp:LinkButton ID="lnkCancelar" runat="server" CssClass="btn btn-danger t14" OnClick="lnkCancelar_Click"><i class="fa fa-ban"></i><span>&nbsp;Salir</span></asp:LinkButton>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-12 col-sm-12 text-center">
                    <asp:Label ID="lblError" runat="server" CssClass="errores" />
                </div>
            </div>
            <asp:Panel ID="PanelPorcentaje" runat="server">
                <div class="row marTop">
                    <div class="col-lg-12 col-sm-12 text-center">
                        <asp:Label ID="Label11" runat="server" CssClass="textoBold" Text="Porcentaje de Sobre Costo:" />
                        <asp:TextBox ID="txtPorcGral" runat="server" CssClass="input-mini" MaxLength="9"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorSCG" ControlToValidate="txtPorcGral"
                            runat="server" ValidationExpression="^\d{1,3}(\.\d{1,2})?$" ErrorMessage="El porcentaje de sobre costo solo puede contener dígitos y un punto decimal"
                            ValidationGroup="actualiza" Text="*" CssClass="errores col-lg-1 col-sm-1 text-left"></asp:RegularExpressionValidator>
                        <asp:LinkButton ID="lnkAplicaSobreCosto" ValidationGroup="actualiza" runat="server"
                            CssClass="btn btn-info" ToolTip="Aplica Porcentaje" OnClick="lnkAplicaSobreCosto_Click"><i class="fa fa-check-circle"></i><span>&nbsp;Aplica Porcentaje</span></asp:LinkButton>
                        <asp:Label ID="lblVisiblePorc" runat="server" Visible="false" />
                    </div>
                </div>
            </asp:Panel>
            <div class="col-lg-12 col-sm-12 text-center pad1m">
                <asp:LinkButton ID="lnkComparativo" runat="server" ToolTip="Comparativo" CssClass="btn btn-info"
                    OnClick="lnkComparativo_Click"><i class="fa fa-check-circle"></i><span>&nbsp;Comparativo</span></asp:LinkButton>
            </div>
            <asp:Panel runat="server" ID="Panel1" CssClass="paneles col-lg-12 col-sm-12 marTop"
                ScrollBars="Auto">
                <div class="col-lg-12 col-sm-12">
                    <asp:Panel ID="pnlRefacciones" runat="server" CssClass="paneles text-center" ScrollBars="Auto">
                        <div class="table-responsive">
                            <asp:GridView ID="grdRefacciones" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1"
                                CssClass="table table-bordered" OnRowCommand="grdRefacciones_RowCommand" AllowSorting="True"
                                OnRowDataBound="grdRefacciones_RowDataBound" ShowFooter="True">
                                <Columns>
                                    <asp:TemplateField HeaderText="refOrd_Id" InsertVisible="False" SortExpression="refOrd_Id"
                                        Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("refOrd_Id") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lblRef" runat="server" Text='<%# Eval("refOrd_Id") %>'></asp:Label>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cant." SortExpression="refCantidad">
                                        <ItemTemplate>
                                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("refCantidad") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("refCantidad") %>'></asp:Label>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="Label9" runat="server" Text="Total:"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Refacción" SortExpression="refDescripcion">
                                        <ItemTemplate>
                                            <asp:Label ID="Label4" runat="server" Text='<%# Bind("refDescripcion") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="Label2" runat="server" Text='<%# Eval("refDescripcion") %>'></asp:Label>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotRef" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="refProveedor" HeaderText="refProveedor" SortExpression="refProveedor"
                                        Visible="False" />
                                    <asp:BoundField DataField="razon_social" HeaderText="Proveedor" SortExpression="razon_social"
                                        ReadOnly="True" />
                                    <asp:TemplateField HeaderText="Cost. Unit." SortExpression="refCosto">
                                        <EditItemTemplate>
                                            <asp:Label ID="Label7" runat="server" Text='<%# Eval("refCosto", "{0:C2}") %>'></asp:Label>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label9" runat="server" Text='<%# Bind("refCosto", "{0:C2}") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="% Dto." SortExpression="porc_desc">
                                        <EditItemTemplate>
                                            <asp:Label ID="Label8" runat="server" Text='<%# Eval("porc_desc") %>'></asp:Label>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label10" runat="server" Text='<%# Bind("porc_desc") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="importeDesc" HeaderText="Impte. Dto." SortExpression="importeDesc"
                                        ReadOnly="True" DataFormatString="{0:C2}" />
                                    <asp:TemplateField HeaderText="Impte. Cmpra." SortExpression="importeCosto">
                                        <EditItemTemplate>
                                            <asp:Label ID="Label3" runat="server" Text='<%# Eval("importeCosto", "{0:C2}") %>'></asp:Label>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label6" runat="server" Text='<%# Bind("importeCosto", "{0:C2}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotalCompra" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="% S.C." SortExpression="porcSobre">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProcSob" runat="server" Text='<%# Bind("porcSobre") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtPorcSob" runat="server" Text='<%# Bind("porcSobre") %>' CssClass="input-mini"
                                                MaxLength="9"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidatorSC" ControlToValidate="txtPorcSob"
                                                runat="server" ValidationExpression="^\d{1,3}(\.\d{1,2})?$" ErrorMessage="El porcentaje de sobre costo solo puede contener dígitos y un punto decimal"
                                                ValidationGroup="edita" Text="*" CssClass="errores"></asp:RegularExpressionValidator>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Prec. Unit. Autorizado" SortExpression="refPrecioVenta">
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("refPrecioVenta","{0:C2}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtPrecioMod" runat="server" Text='<%# Bind("refPrecioVenta") %>'
                                                CssClass="input-small" MaxLength="9"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtPrecioMod"
                                                runat="server" ValidationExpression="^\d{1,6}(\.\d{1,2})?$" ErrorMessage="El precio solo puede contener dígitos y un punto decimal"
                                                ValidationGroup="edita" Text="*" CssClass="errores"></asp:RegularExpressionValidator>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Debe indicar el precio unitario"
                                                ValidationGroup="edita" Text="*" CssClass="errores" ControlToValidate="txtPrecioMod"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Impte. Autorizado" SortExpression="importeVenta">
                                        <EditItemTemplate>
                                            <asp:Label ID="Label4" runat="server" Text='<%# Eval("importeVenta", "{0:C2}") %>'></asp:Label>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label7" runat="server" Text='<%# Bind("importeVenta", "{0:C2}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotalVenta" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Utilidad" SortExpression="utilidad">
                                        <EditItemTemplate>
                                            <asp:Label ID="Label5" runat="server" Text='<%# Eval("utilidad", "{0:C2}") %>'></asp:Label>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label8" runat="server" Text='<%# Bind("utilidad", "{0:C2}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotalutilidad" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="descripEstatus" SortExpression="descripEstatus" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="Label5" runat="server" Text='<%# Bind("descripEstatus") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="Label6" runat="server" Text='<%# Eval("descripEstatus") %>'></asp:Label>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Porcentaje" Visible="true" SortExpression="estatus">
                                        <FooterTemplate>
                                            <asp:Label ID="lblPorcUtilidad" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Fecha Límite" InsertVisible="False" SortExpression="fecha_limite">
                                        <ItemTemplate>
                                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("fecha_limite") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lblRef" runat="server" Text='<%# Eval("fecha_limite") %>'></asp:Label>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Estatus Presupuesto" Visible="false" SortExpression="estatus">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEsta" runat="server" Text='<%# Bind("estatus") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlEstatusRefEdit" runat="server">
                                                <asp:ListItem Text="" Value="" Selected="True" />
                                                <asp:ListItem Text="Cancelada" Value="CA" />
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <%--Footer para el porcentaje de la parte de abajo (azul) --%>
                                        
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Estatus Refacción" Visible="false" SortExpression="estatusSoli">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEstaRef" runat="server" Text='<%# Bind("estatusSoli") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlEstatus" runat="server" SelectedValue='<%# Bind("refEstatusSolicitud") %>'
                                                DataSourceID="SqlDataSource2" DataTextField="stadescripcion" DataValueField="starefid">
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>"
                                                SelectCommand="select starefid,stadescripcion from rafacciones_estatus"></asp:SqlDataSource>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="False" Visible="false">
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="False" Visible="false">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEditar" runat="server" CausesValidation="False" CommandName="Edit"
                                                ToolTip="Editar" CssClass="btn btn-warning"><i class="fa fa-edit"></i></asp:LinkButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:LinkButton ID="lnkActualizar" runat="server" CausesValidation="True" ValidationGroup="edita"
                                                CommandName="Update" ToolTip="Actualizar" CssClass="btn btn-success" CommandArgument='<%# Eval("refOrd_Id") %>'><i class="fa fa-save"></i></asp:LinkButton>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="False" Visible="false">
                                        <EditItemTemplate>
                                            <asp:LinkButton ID="lnkCancelar" runat="server" CausesValidation="False" CommandName="Cancel"
                                                ToolTip="Cancelar" CssClass="btn btn-danger"><i class="fa fa-remove"></i></asp:LinkButton>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkAutorizar" runat="server" CausesValidation="False" CommandName="Autoriza"
                                                ToolTip="Autorizar" CssClass="btn btn-success" CommandArgument='<%# Eval("refOrd_Id")+";"+Eval("refDescripcion")+";"+Eval("refCantidad")+";"+Eval("descripEstatus") %>'
                                                OnClientClick="return confirm('¿Está seguro de autorizar la refacción?. Una vez autorizada solo podra cancelarla')"
                                                OnClick="lnkAutorizar_Click"><i class="fa fa-check"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <SelectedRowStyle CssClass="alert-success" />
                                <EditRowStyle CssClass="alert-warning" />
                            </asp:GridView>
                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>"
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
                                    isnull((SELECT estatus from Cotizacion_Detalle where no_orden=r.ref_no_orden and id_empresa=r.ref_id_empresa and id_taller=ref_id_taller and id_cotizacion=@cotizacion and id_cotizacion_detalle=r.refOrd_id and id_cliprov=r.refProveedor ),'CAN') as estatusRef,r.refEstatusSolicitud,
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
                                    (select staDescripcion from rafacciones_estatus where starefid=r.refEstatusSolicitud) as estatusSoli,
                                    r.refEstatusSolicitud,
                                    case r.refEstatus when 'NA' then 'No Aplica' when 'EV' then 'Evaluación' when 'RP' THEN 'Reparación' when 'CO' then 'Compra' when 'CA' THEN 'Cancelada' when 'AP' then 'Aplicada' when 'AU' then 'Autorizada' else '' end as estatus,
                                    (SELECT convert(char(10),fecha_limite,120) from Cotizacion_Detalle where no_orden=r.ref_no_orden and id_empresa=r.ref_id_empresa and id_taller=ref_id_taller and id_cotizacion=r.id_cotizacion_autorizada and id_cotizacion_detalle=r.refOrd_id and id_cliprov=r.refProveedor) as fecha_limite
                                    from Refacciones_Orden r
                                    left join Cliprov c on c.id_cliprov=r.refProveedor and c.tipo='P'
                                    where r.ref_no_orden=@orden and r.ref_id_empresa=@empresa and r.ref_id_taller=@taller 
                                    and refOrd_Id in (select distinct id_cotizacion_detalle from Cotizacion_Detalle where no_orden=r.ref_no_orden and id_empresa=r.ref_id_empresa and id_taller=r.ref_id_taller and id_cotizacion=@cotizacion and existencia !=0)">
                                <SelectParameters>
                                    <asp:QueryStringParameter DefaultValue="0" Name="cotizacion" QueryStringField="c" />
                                    <asp:QueryStringParameter DefaultValue="0" Name="orden" QueryStringField="o" />
                                    <asp:QueryStringParameter DefaultValue="0" Name="empresa" QueryStringField="e" />
                                    <asp:QueryStringParameter DefaultValue="0" Name="taller" QueryStringField="t" />
                                    <asp:QueryStringParameter DefaultValue="0" Name="acceso" QueryStringField="a" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                    </asp:Panel>
                </div>
            </asp:Panel>
            <div class="col-lg-12 col-sm-12 text-center">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="edita" CssClass="errores" DisplayMode="List" />
                <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="actualiza" CssClass="errores" DisplayMode="List" />
                <asp:LinkButton ID="lnkComprar" runat="server" CssClass="btn btn-primary t14" Visible="false" OnClick="lnkComprar_Click" OnClientClick="return confirm('¿Está seguro de generar la orden de compra?')"><i class="fa fa-file-text"></i><span>&nbsp;Generar Orden de Compra</span></asp:LinkButton>
            </div>
            <asp:Panel ID="pnlComprasEnviadas" runat="server" CssClass="row" Visible="false">                
                <div class="col-lg-12 col-sm-12">
                    <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid3" runat="server"  Culture="es-Mx" Skin="Metro" CssClass="col-lg-12 col-sm-12" AllowAutomaticUpdates="True" AutoGenerateColumns="False" AllowAutomaticInserts="false" AllowAutomaticDeletes="false" AllowPaging="True" PagerStyle-AlwaysVisible="true" DataSourceID="SqlDataSource10" AllowSorting="true" GroupingEnabled="false" PageSize="20">
                        <MasterTableView DataSourceID="SqlDataSource10" AutoGenerateColumns="False" DataKeyNames="no_orden,id_empresa,id_taller,id_compra,id_cliprov"  CommandItemDisplay="Bottom" HorizontalAlign="NotSet" EditMode="Batch">
                            <BatchEditingSettings EditType="Row" />
                            <CommandItemStyle CssClass="text-right" />
                            <CommandItemSettings SaveChangesText="Guardar Cambios" ShowAddNewRecordButton="false"  ShowRefreshButton="false" ShowSaveChangesButton="true" CancelChangesText="Cancelar Cambios"/>
                            <Columns>
                                <telerik:GridBoundColumn UniqueName="id_compra" HeaderText="id_compra" SortExpression="id_compra" Visible="false" DataField="id_compra" ReadOnly="true"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn UniqueName="id_cliprov" HeaderText="id_cliprov" SortExpression="id_cliprov" Visible="false" DataField="id_cliprov" ReadOnly="true"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn UniqueName="folio_orden" HeaderText="Folio" SortExpression="folio_orden" DataField="folio_orden" ReadOnly="true"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn UniqueName="fecha" HeaderText="Fecha" SortExpression="fecha" DataField="fecha" ReadOnly="true"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn UniqueName="hora" HeaderText="Hora" SortExpression="hora" DataField="hora" ReadOnly="true"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn UniqueName="razon_social" HeaderText="Proveedor" SortExpression="razon_social" DataField="razon_social" ReadOnly="true"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn UniqueName="fechaEnvio" HeaderText="Fecha Envio" SortExpression="fechaEnvio" DataField="fechaEnvio" ReadOnly="true"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn UniqueName="horaEnvio" HeaderText="Hora Envio" SortExpression="horaEnvio" DataField="horaEnvio" ReadOnly="true"></telerik:GridBoundColumn>
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
                            SelectCommand="select oc.no_orden,oc.id_empresa,oc.id_taller,ce.id_compra,oc.id_orden,oc.folio_orden,convert(char(10),oc.fecha,120) as fecha,convert(char(8),oc.hora,120) as hora,oc.id_cliprov,c.razon_social,convert(char(10),ce.fecha,120) as fechaEnvio,convert(char(8),ce.hora,120) as horaEnvio,ce.correo,u.nombre_usuario,ce.motivo_fallo,ce.enviado
from orden_compra_encabezado oc 
left join cliprov c on c.id_cliprov=oc.id_cliprov and c.tipo='P'
left join compras_enviadas ce on ce.no_orden=oc.no_orden and ce.id_empresa=oc.id_empresa and ce.id_taller=oc.id_taller and ce.id_compra=oc.id_orden
left join usuarios u on u.id_usuario=ce.usuario
where oc.no_orden=@no_orden and oc.id_empresa=@id_empresa and oc.id_taller=@id_taller and oc.id_cotizacion=@id_cotizacion"
                            UpdateCommand ="update compras_enviadas set correo=@correo where no_orden=@no_orden and id_empresa=@id_empresa and id_taller=@id_taller and id_compra=@id_compra and id_cliprov=@id_cliprov update cliprov set correo=@correo where id_cliprov=@id_cliprov and tipo='P' ">
                            <SelectParameters>
                                <asp:QueryStringParameter QueryStringField="o" Name="no_orden"></asp:QueryStringParameter>
                                <asp:QueryStringParameter QueryStringField="t" Name="id_taller"></asp:QueryStringParameter>
                                <asp:QueryStringParameter QueryStringField="e" Name="id_empresa"></asp:QueryStringParameter>
                                <asp:QueryStringParameter QueryStringField="c" Name="id_cotizacion"></asp:QueryStringParameter>
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

            </asp:Panel>
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
                            <asp:Label ID="Label111" runat="server" Text="Total Orden:" CssClass="colorEtiqueta"
                                Visible="false"></asp:Label>&nbsp;&nbsp;
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
            <asp:Panel ID="PanelMascara" runat="server" CssClass="mask zen2">
            </asp:Panel>
            <asp:Label ID="lblIdRef" runat="server" Visible="false" />
            <asp:Label ID="lblRefaccion" runat="server" Visible="false" />
            <asp:Label ID="lblCantidad" runat="server" Visible="false" />
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                <ProgressTemplate>
                    <asp:Panel ID="pnlMaskLoad" runat="server" CssClass="maskLoad">
                    </asp:Panel>
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
                        OnItemDataBound="RadGrid1_ItemDataBound" AutoGenerateColumns="false"  AutoGenerateHierarchy="false" Height="650px">
                        <ClientSettings>
                            <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true">
                            </Scrolling>
                        </ClientSettings>
                        <MasterTableView TableLayout="Fixed">
                            <Columns>
                                <telerik:GridBoundColumn DataField="cantidad" HeaderText="Cantidad" ReadOnly="true"
                                    SortExpression="cantidad">
                                    <ItemStyle Width="100px" />
                                    <HeaderStyle Width="100px" CssClass="text-center" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="descripcion" HeaderText="Refacción" ReadOnly="true"
                                    SortExpression="descripcion" >
                                    <ItemStyle Width="150px" />
                                    <HeaderStyle Width="150px" CssClass="text-center" />
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn SortExpression="razon_social1" HeaderText="">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkProveedor1" runat="server" Text='<%# Bind("razon_social1") %>'
                                            CssClass="link textoBold alert-primary ancho100 text-center" CommandArgument='<%# Eval("id_prov1")+";"+Eval("id_cotizacion_detalle")+";"+Eval("costo_unitario1") %>'
                                            OnClick="lnkProveedor_Click"></asp:LinkButton><br />
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
                                                    <td class="pad1m" colspan="2">
                                                        <asp:Label ID="Label29" runat="server" Text="Dias Entrega" CssClass="textoBold"></asp:Label><br />
                                                        <asp:Label ID="Label30" runat="server" Text='<%# Bind("dias1") %>'></asp:Label>
                                                    </td>
                                                    <td class="pad1m" colspan="2">
                                                        <asp:Label ID="Lbl131" runat="server" Text="Fecha Límite" CssClass="textoBold"></asp:Label><br />
                                                        <asp:Label ID="lbl151" runat="server" Text='<%# Bind("fecha_limite1") %>'></asp:Label>
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
                                            CssClass="link textoBold alert-primary ancho100 text-center" CommandArgument='<%# Eval("id_prov2")+";"+Eval("id_cotizacion_detalle")+";"+Eval("costo_unitario2") %>'
                                            OnClick="lnkProveedor_Click"></asp:LinkButton><br />
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
                                                    <td class="pad1m" colspan="2">
                                                        <asp:Label ID="lbl132" runat="server" Text="Fecha Límite" CssClass="textoBold"></asp:Label><br />
                                                        <asp:Label ID="lbl152" runat="server" Text='<%# Bind("fecha_limite2") %>'></asp:Label>
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
                                            CssClass="link textoBold alert-primary ancho100 text-center" CommandArgument='<%# Eval("id_prov3")+";"+Eval("id_cotizacion_detalle")+";"+Eval("costo_unitario3") %>'
                                            OnClick="lnkProveedor_Click"></asp:LinkButton><br />
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
                                                    <td class="pad1m" colspan="2">
                                                        <asp:Label ID="lbl133" runat="server" Text="Fecha Límite" CssClass="textoBold"></asp:Label><br />
                                                        <asp:Label ID="lbl153" runat="server" Text='<%# Bind("fecha_limite3") %>'></asp:Label>
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
                                            CssClass="link textoBold alert-primary ancho100 text-center" CommandArgument='<%# Eval("id_prov4")+";"+Eval("id_cotizacion_detalle")+";"+Eval("costo_unitario4") %>'
                                            OnClick="lnkProveedor_Click"></asp:LinkButton><br />
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
                                                    <td class="pad1m" colspan="2">
                                                        <asp:Label ID="lbl134" runat="server" Text="Fecha Límite" CssClass="textoBold"></asp:Label><br />
                                                        <asp:Label ID="lbl154" runat="server" Text='<%# Bind("fecha_limite4") %>'></asp:Label>
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
                                            CssClass="link textoBold alert-primary ancho100 text-center" CommandArgument='<%# Eval("id_prov5")+";"+Eval("id_cotizacion_detalle")+";"+Eval("costo_unitario5") %>'
                                            OnClick="lnkProveedor_Click"></asp:LinkButton><br />
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
                                                    <td class="pad1m" colspan="2">
                                                        <asp:Label ID="lbl135" runat="server" Text="Fecha Límite" CssClass="textoBold"></asp:Label><br />
                                                        <asp:Label ID="lbl155" runat="server" Text='<%# Bind("fecha_limite5") %>'></asp:Label>
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
                                            CssClass="link textoBold alert-primary ancho100 text-center" CommandArgument='<%# Eval("id_prov6")+";"+Eval("id_cotizacion_detalle")+";"+Eval("costo_unitario6") %>'
                                            OnClick="lnkProveedor_Click"></asp:LinkButton><br />
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
                                                    <td class="pad1m" colspan="2">
                                                        <asp:Label ID="lbl136" runat="server" Text="Fecha Límite" CssClass="textoBold"></asp:Label><br />
                                                        <asp:Label ID="lbl156" runat="server" Text='<%# Bind("fecha_limite6") %>'></asp:Label>
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
                                            CssClass="link textoBold alert-primary ancho100 text-center" CommandArgument='<%# Eval("id_prov7")+";"+Eval("id_cotizacion_detalle")+";"+Eval("costo_unitario7") %>'
                                            OnClick="lnkProveedor_Click"></asp:LinkButton><br />
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
                                                    <td class="pad1m" colspan="2">
                                                        <asp:Label ID="lbl137" runat="server" Text="Fecha Límite" CssClass="textoBold"></asp:Label><br />
                                                        <asp:Label ID="lbl157" runat="server" Text='<%# Bind("fecha_limite7") %>'></asp:Label>
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
                                            CssClass="link textoBold alert-primary ancho100 text-center" CommandArgument='<%# Eval("id_prov8")+";"+Eval("id_cotizacion_detalle")+";"+Eval("costo_unitario8") %>'
                                            OnClick="lnkProveedor_Click"></asp:LinkButton><br />
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
                                                    <td class="pad1m" colspan="2">
                                                        <asp:Label ID="lbl138" runat="server" Text="Fecha Límite" CssClass="textoBold"></asp:Label><br />
                                                        <asp:Label ID="lbl158" runat="server" Text='<%# Bind("fecha_limite8") %>'></asp:Label>
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
                                            CssClass="link textoBold alert-primary ancho100 text-center" CommandArgument='<%# Eval("id_prov9")+";"+Eval("id_cotizacion_detalle")+";"+Eval("costo_unitario9") %>'
                                            OnClick="lnkProveedor_Click"></asp:LinkButton><br />
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
                                                    <td class="pad1m" colspan="2">
                                                        <asp:Label ID="lbl139" runat="server" Text="Fecha Límite" CssClass="textoBold"></asp:Label><br />
                                                        <asp:Label ID="lbl159" runat="server" Text='<%# Bind("fecha_limite9") %>'></asp:Label>
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
                                            CssClass="link textoBold alert-primary ancho100 text-center" CommandArgument='<%# Eval("id_prov10")+";"+Eval("id_cotizacion_detalle")+";"+Eval("costo_unitario10") %>'
                                            OnClick="lnkProveedor_Click"></asp:LinkButton><br />
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
                                                    <td class="pad1m" colspan="2">
                                                        <asp:Label ID="lbl1310" runat="server" Text="Fecha Límite" CssClass="textoBold"></asp:Label><br />
                                                        <asp:Label ID="lbl1510" runat="server" Text='<%# Bind("fecha_limite10") %>'></asp:Label>
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
                                            CssClass="link textoBold alert-primary ancho100 text-center" CommandArgument='<%# Eval("id_prov11")+";"+Eval("id_cotizacion_detalle")+";"+Eval("costo_unitario11") %>'
                                            OnClick="lnkProveedor_Click"></asp:LinkButton><br />
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
                                                    <td class="pad1m" colspan="2">
                                                        <asp:Label ID="lbl1311" runat="server" Text="Fecha Límite" CssClass="textoBold"></asp:Label><br />
                                                        <asp:Label ID="lbl1511" runat="server" Text='<%# Bind("fecha_limite11") %>'></asp:Label>
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
                                            CssClass="link textoBold alert-primary ancho100 text-center" CommandArgument='<%# Eval("id_prov12")+";"+Eval("id_cotizacion_detalle")+";"+Eval("costo_unitario12") %>'
                                            OnClick="lnkProveedor_Click"></asp:LinkButton><br />
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
                                                    <td class="pad1m" colspan="2">
                                                        <asp:Label ID="lbl1312" runat="server" Text="Fecha Límite" CssClass="textoBold"></asp:Label><br />
                                                        <asp:Label ID="lbl1512" runat="server" Text='<%# Bind("fecha_limite12") %>'></asp:Label>
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
                                            CssClass="link textoBold alert-primary ancho100 text-center" CommandArgument='<%# Eval("id_prov13")+";"+Eval("id_cotizacion_detalle")+";"+Eval("costo_unitario13") %>'
                                            OnClick="lnkProveedor_Click"></asp:LinkButton><br />
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
                                                    <td class="pad1m" colspan="2">
                                                        <asp:Label ID="lbl1313" runat="server" Text="Fecha Límite" CssClass="textoBold"></asp:Label><br />
                                                        <asp:Label ID="lbl1513" runat="server" Text='<%# Bind("fecha_limite13") %>'></asp:Label>
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
                                            CssClass="link textoBold alert-primary ancho100 text-center" CommandArgument='<%# Eval("id_prov14")+";"+Eval("id_cotizacion_detalle")+";"+Eval("costo_unitario14") %>'
                                            OnClick="lnkProveedor_Click"></asp:LinkButton><br />
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
                                                    <td class="pad1m" colspan="2">
                                                        <asp:Label ID="lbl1314" runat="server" Text="Fecha Límite" CssClass="textoBold"></asp:Label><br />
                                                        <asp:Label ID="lbl1514" runat="server" Text='<%# Bind("fecha_limite14") %>'></asp:Label>
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
                                            CssClass="link textoBold alert-primary ancho100 text-center" CommandArgument='<%# Eval("id_prov15")+";"+Eval("id_cotizacion_detalle")+";"+Eval("costo_unitario15") %>'
                                            OnClick="lnkProveedor_Click"></asp:LinkButton><br />
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
                                                    <td class="pad1m" colspan="2">
                                                        <asp:Label ID="lbl1315" runat="server" Text="Fecha Límite" CssClass="textoBold"></asp:Label><br />
                                                        <asp:Label ID="lbl1515" runat="server" Text='<%# Bind("fecha_limite15") %>'></asp:Label>
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
                                            CssClass="link textoBold alert-primary ancho100 text-center" CommandArgument='<%# Eval("id_prov16")+";"+Eval("id_cotizacion_detalle")+";"+Eval("costo_unitario16") %>'
                                            OnClick="lnkProveedor_Click"></asp:LinkButton><br />
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
                                                    <td class="pad1m" colspan="2">
                                                        <asp:Label ID="lbl1316" runat="server" Text="Fecha Límite" CssClass="textoBold"></asp:Label><br />
                                                        <asp:Label ID="lbl1516" runat="server" Text='<%# Bind("fecha_limite16") %>'></asp:Label>
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
                                            CssClass="link textoBold alert-primary ancho100 text-center" CommandArgument='<%# Eval("id_prov17")+";"+Eval("id_cotizacion_detalle")+";"+Eval("costo_unitario17") %>'
                                            OnClick="lnkProveedor_Click"></asp:LinkButton><br />
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
                                                    <td class="pad1m" colspan="2">
                                                        <asp:Label ID="lbl1317" runat="server" Text="Fecha Límite" CssClass="textoBold"></asp:Label><br />
                                                        <asp:Label ID="lbl1517" runat="server" Text='<%# Bind("fecha_limite17") %>'></asp:Label>
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
                                            CssClass="link textoBold alert-primary ancho100 text-center" CommandArgument='<%# Eval("id_prov18")+";"+Eval("id_cotizacion_detalle")+";"+Eval("costo_unitario18") %>'
                                            OnClick="lnkProveedor_Click"></asp:LinkButton><br />
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
                                                    <td class="pad1m" colspan="2">
                                                        <asp:Label ID="lbl1318" runat="server" Text="Fecha Límite" CssClass="textoBold"></asp:Label><br />
                                                        <asp:Label ID="lbl1518" runat="server" Text='<%# Bind("fecha_limite18") %>'></asp:Label>
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
                                            CssClass="link textoBold alert-primary ancho100 text-center" CommandArgument='<%# Eval("id_prov19")+";"+Eval("id_cotizacion_detalle")+";"+Eval("costo_unitario19") %>'
                                            OnClick="lnkProveedor_Click"></asp:LinkButton><br />
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
                                                    <td class="pad1m" colspan="2">
                                                        <asp:Label ID="lbl1319" runat="server" Text="Fecha Límite" CssClass="textoBold"></asp:Label><br />
                                                        <asp:Label ID="lbl1519" runat="server" Text='<%# Bind("fecha_limite19") %>'></asp:Label>
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
                                            CssClass="link textoBold alert-primary ancho100 text-center" CommandArgument='<%# Eval("id_prov20")+";"+Eval("id_cotizacion_detalle")+";"+Eval("costo_unitario20") %>'
                                            OnClick="lnkProveedor_Click"></asp:LinkButton><br />
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
                                                    <td class="pad1m" colspan="2">
                                                        <asp:Label ID="lbl1320" runat="server" Text="Fecha Límite" CssClass="textoBold"></asp:Label><br />
                                                        <asp:Label ID="lbl1520" runat="server" Text='<%# Bind("fecha_limite20") %>'></asp:Label>
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
                            <asp:Panel ID="pnlNuevoProv" Visible="false" runat="server" CssClass="ancho95 centrado">
                                <div class="col-lg-3 col-sm-3 text-left">
                                    <asp:Label ID="Label14" runat="server" Visible="false" Text="Refacción:" CssClass="textoBold"></asp:Label>
                                    <asp:DropDownList  RenderMode="Lightweight" runat="server" Visible="false" ID="ddlRefaccionesNueva" 
                                    CssClass="input-xlarge" DataSourceID="SqlDataSource3"  
                                    DataTextField="refaccion" DataValueField="reford_id"></asp:DropDownList >
                                    <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>"
                                        SelectCommand="select reford_id,'('+rtrim(ltrim(cast(refcantidad as char(10))))+') '+refdescripcion as refaccion from refacciones_orden where refEstatusSolicitud in(1,2,4) and ref_id_empresa=@empresa and reF_id_taller=@taller and ref_no_orden=@orden and refOrd_Id in(select distinct id_cotizacion_detalle from Cotizacion_Detalle where id_empresa=@empresa and id_taller=@taller and no_orden=@orden and id_cotizacion=@cotizacion)">
                                        <SelectParameters>
                                            <asp:QueryStringParameter Name="empresa" QueryStringField="e" DefaultValue="0" />
                                            <asp:QueryStringParameter Name="taller" QueryStringField="t" DefaultValue="0" />
                                            <asp:QueryStringParameter Name="orden" QueryStringField="o" DefaultValue="0" />
                                            <asp:QueryStringParameter Name="cotizacion" QueryStringField="c" DefaultValue="0" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>                                
                                <div class="col-lg-3 col-sm-3 text-left">
                                    <asp:Label ID="lblEtiquetaProveedor" Visible="false" runat="server" Text="Proveedor:" CssClass="textoBold"></asp:Label>
                                    <asp:TextBox ID="txtProveedorAdd" Visible="false" runat="server" CssClass="input-large"></asp:TextBox>
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
                                    <asp:Label ID="lblCU" runat="server" Visible="false" Text="Cost. Unit.:" CssClass="textoBold"></asp:Label>
                                    <telerik:RadNumericTextBox Visible="false" RenderMode="Lightweight" runat="server" ID="radCostoUnitario" CssClass="input-mini" Value="0" EmptyMessage="Cost. Unit." MinValue="0" MaxValue="1000000" ShowSpinButtons="true" NumberFormat-DecimalDigits="2" Skin="MetroTouch"></telerik:RadNumericTextBox>
                                    <%--<asp:TextBox ID="txtContoUnitario" runat="server" CssClass="input-small" MaxLength="16"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Debe indicar le costo unitario"
                                        Text="*" CssClass="errores" ControlToValidate="txtContoUnitario" ValidationGroup="manual"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtContoUnitario"
                                        runat="server" ValidationExpression="^\d{1,13}(\.\d{1,2})?$" ErrorMessage="El costo solo puede contener dígitos y un punto decimal"
                                        ValidationGroup="manual" Text="*" CssClass="errores"></asp:RegularExpressionValidator>--%>
                                </div>                                
                                <div class="col-lg-2 col-sm-2 text-left">
                                    <asp:Label ID="lblPorcDesc" Visible="false" runat="server" Text="% Dto.:" CssClass="textoBold"></asp:Label>
                                    <telerik:RadNumericTextBox RenderMode="Lightweight" Visible="false" runat="server" ID="radPorcDesc" CssClass="input-mini" Value="0" EmptyMessage="% Dto." MinValue="0" MaxValue="100" ShowSpinButtons="true" NumberFormat-DecimalDigits="2" Skin="MetroTouch"></telerik:RadNumericTextBox>
                                    <%--<asp:TextBox ID="txtPorcDesc" runat="server" CssClass="input-mini" MaxLength="6"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="txtPorcDesc"
                                        runat="server" ValidationExpression="^100$|^100.00$|^\d{0,2}(\.\d{1,2})? *%?$"
                                        ErrorMessage="El porcentaje solo puede contener dígitos y un punto decimal" ValidationGroup="manual"
                                        Text="*" CssClass="errores"></asp:RegularExpressionValidator>--%>
                                </div>
                                <div class="col-lg-1 col-sm-1 text-left">
                                    <asp:CheckBox ID="chkExistencia" Visible="false" runat="server" Text="Existencia" />
                                </div>                                
                                <div class="col-lg-3 col-sm-3 text-left">
                                    <asp:Label ID="lblDias" Visible="false" runat="server" Text="Días Entrega:" CssClass="textoBold"></asp:Label>
                                    <telerik:RadNumericTextBox RenderMode="Lightweight" Visible="false" runat="server" ID="radDias" CssClass="input-mini" Value="0" EmptyMessage="Días" MinValue="0" MaxValue="365" ShowSpinButtons="true" NumberFormat-DecimalDigits="0" Skin="MetroTouch"></telerik:RadNumericTextBox>
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
                                    <asp:LinkButton ID="lnkAgregarProveedor" Visible="false" runat="server" ToolTip="Agregar" CssClass="btn btn-success alingMiddle"
                                        ValidationGroup="manual" OnClick="lnkAgregarProveedor_Click"><i class="fa fa-save"></i><span>&nbsp;Agregar</span>
                                    </asp:LinkButton>
                                </div>
                                <div class="col-lg-4 col-sm-4 text-center">
                                    <asp:LinkButton ID="lnkImprimeComparativo" runat="server" ToolTip="Imprimir Comparativo"
                                        CssClass="btn btn-info alingMiddle" OnClick="lnkImprimeComparativo_Click" ><i class="fa fa-print"></i><span>&nbsp;Imprimir</span>
                                    </asp:LinkButton>
                                </div>
                                <div class="col-lg-4 col-sm-4 text-center">
                                    <asp:LinkButton ID="lnkCerrar" runat="server" ToolTip="Cerrar"
                                        CssClass="btn btn-danger alingMiddle" OnClientClick="cierraWinComp()"><i class="fa fa-remove"></i><span>&nbsp;Cerrar</span>
                                    </asp:LinkButton>
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
            </asp:UpdatePanel>
        </ContentTemplate>
    </telerik:RadWindow>



</asp:Content>
