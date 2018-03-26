<%@ Page Title="Refacciones" Language="C#" MasterPageFile="~/AdmonOrden.master" AutoEventWireup="true" CodeFile="Refacciones_Orden.aspx.cs" Inherits="Refacciones_Orden" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script language="javascript" type="text/javascript">
        function calculaPrecVenta(txtPorc, costo) {
            var grdCot = $get('<%= grdRefacCotizado.ClientID %>');
            var cell = txtPorc.parentNode;
            var row = txtPorc.parentNode.parentNode;
            var cIndx = cell.cellIndex;
            var rIndx = row.rowIndex - 1;
            var txtPrecVta = row.cells[cIndx + 1].getElementsByTagName("input")[0];
            var porc = txtPorc.value;
            costo = Number(costo);
            txtPrecVta.value = ((costo * (porc / 100)) + costo).toFixed(2);
        }

        function calculaPorcent(txtPv, costo) {
            costo = Number(costo);
            var grdCot = $get('<%= grdRefacCotizado.ClientID %>');
                var cell = txtPv.parentNode;
                var row = txtPv.parentNode.parentNode;
                var cIndx = cell.cellIndex;
                var rIndx = row.rowIndex - 1;
                var txtPorc = row.cells[cIndx - 1].getElementsByTagName("input")[0];
                var pv = txtPv.value
                txtPorc.value = (((pv - costo) / costo) * 100).toFixed(2);
            }
    </script>
    <style type="text/css">
        .colBotones {
            display: inline-block;
            border: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" />
    <asp:UpdatePanel runat="server" ID="updPnlRefacc">
        <ContentTemplate>
            <div class="row">
                <div class="col-lg-12 col-sm-12 text-center alert-info">
                    <h3>
                        <i class="fa fa-cogs"></i></i>&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblTit" runat="server" Text="Cotización"></asp:Label>
                    </h3>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-12 col-sm-12 text-center">
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="errores" DisplayMode="List" ValidationGroup="mo" />
                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" CssClass="errores" DisplayMode="List" ValidationGroup="edit" />
                    <asp:ValidationSummary runat="server" ID="vsuAddRefacc" CssClass="errores" ValidationGroup="valRefacc" DisplayMode="List" />
                    <asp:ValidationSummary runat="server" ID="ValidationSummary3" CssClass="errores" ValidationGroup="valRefaccMod" DisplayMode="List" />
                    <asp:Label ID="lblErrorNew" runat="server" CssClass="errores" />
                </div>
            </div>
            <asp:Panel runat="server" ID="pnlRefacc" CssClass="paneles col-lg-12 col-sm-12" ScrollBars="Auto">
                <div class="row">
                    <div class="col-lg-2 col-sm-2 text-right">
                        <asp:LinkButton ID="btnCotiza" runat="server" ToolTip="Cotizar..." OnClick="btnCotiza_Click1" CssClass="btn btn-info t14">
                            <i class="fa fa-plus-circle"></i>&nbsp;<asp:Label runat="server" ID="lblCotiza" Text="Agregar Cotización" />
                        </asp:LinkButton>
                    </div>
                </div>
                <div class="table-responsive">
                    <asp:GridView ID="grdRefacCotizado" runat="server" EmptyDataText="No se han cotizado refacciones en la orden." CssClass="table table-bordered" EmptyDataRowStyle-CssClass="errores"
                        DataSourceID="sqlDSRefOrden" AllowPaging="True" AutoGenerateColumns="False" ViewStateMode="Disabled" DataKeyNames="refOrd_Id, ref_no_orden" OnRowCommand="grdRefacCotizado_RowCommand"
                        OnRowDataBound="grdRefacCotizado_RowDataBound" PageSize="7" AllowSorting="true">
                        <Columns>
                            <asp:BoundField DataField="refCantidad" HeaderText="Cant." ReadOnly="true" HeaderStyle-HorizontalAlign="Center"></asp:BoundField>
                            <asp:BoundField DataField="refDescripcion" HeaderText="Descripcion" ReadOnly="true" HeaderStyle-HorizontalAlign="Center"></asp:BoundField>
                            <asp:BoundField DataField="provRazSoc" HeaderText="Proveedor" ReadOnly="true" NullDisplayText="No hay cotizaci&#243;n" HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="refCosto" HeaderText="Costo" SortExpression="refCosto" ReadOnly="True" NullDisplayText="No hay cotizaci&#243;n" HeaderStyle-HorizontalAlign="Center" DataFormatString="{0:C2}"></asp:BoundField>
                            <asp:TemplateField HeaderText="Importe" HeaderStyle-HorizontalAlign="Center">
                                <EditItemTemplate>
                                    <asp:Label runat="server" ID="lblEdtRefImp" Text='<%# (Convert.ToDecimal(Eval("refCosto")) * Convert.ToInt16(Eval("refCantidad"))).ToString("C2") %>' />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblRefImp" Text='<%# (Convert.ToDecimal(Eval("refCosto")) * Convert.ToInt16(Eval("refCantidad"))).ToString("C2") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sobre costo(%)" HeaderStyle-HorizontalAlign="Center">
                                <EditItemTemplate>
                                    <asp:TextBox runat="server" ID="txtPorcent" Text='<%# Bind("refPorcentSobreCosto") %>' onblur='<%# String.Format("calculaPrecVenta(this, \"{0}\");", Eval("refCosto")) %>' CssClass="input-mini" MaxLength="5" />&nbsp;%
                                    <cc1:FilteredTextBoxExtender runat="server" ID="filTxtPorcent" TargetControlID="txtPorcent" FilterType="Numbers, Custom" ValidChars="." />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblPorcent" Text='<%# Eval("refPorcentSobreCosto") + " %" %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Precio Venta" HeaderStyle-HorizontalAlign="Center">
                                <EditItemTemplate>
                                    <asp:TextBox runat="server" ID="txtPrecVta" Text='<%# Bind("refPrecioVenta") %>' onblur='<%# String.Format("calculaPorcent(this, \"{0}\");", Eval("refCosto"))  %>' CssClass="input-mini" MaxLength="6" />
                                    <cc1:FilteredTextBoxExtender runat="server" ID="filTxtPrecVta" TargetControlID="txtPrecVta" FilterType="Numbers, Custom" ValidChars="." />
                                    <%-- onblur='<%# String.Format("calculaPorcent(this, \"{0}\");",((GridViewRow)grdRefacCotizado.Rows[grdRefacCotizado.EditIndex]).FindControl("txtPorcent").ClientID)--%>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblPrecVta" Text='<%# "$" + Eval("refPrecioVenta") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblRefTotal" Text='<%# (Convert.ToDecimal(Eval("refCosto")) * (Convert.ToDecimal(Eval("refPorcentSobreCosto"))/100 + 1) * Convert.ToInt16(Eval("refCantidad"))).ToString("C2") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Editar%" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEditarR" runat="server" CausesValidation="False" CommandName="Edit" ToolTip="Editar" CssClass="btn btn-warning t14"><i class="fa fa-edit"></i></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton ID="lnkActualizaR" runat="server" CausesValidation="True" CommandName="Update" ToolTip="Guardar" CssClass="btn btn-success t14" ValidationGroup="valRefaccMod"><i class="fa fa-save"></i></asp:LinkButton>
                                    <asp:LinkButton ID="lnkCancelarR" runat="server" CausesValidation="false" CommandName="Cancel" ToolTip="Cancelar" CssClass="btn btn-danger t14"><i class="fa fa-times-circle"></i></asp:LinkButton>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Aprobar / Autorizar" ItemStyle-CssClass="" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="lnkRefCotAp" CommandName="cotAprobar" Visible='<%# Eval("refEstatus").Equals("NA") && Convert.ToDecimal(Eval("refPorcentSobreCosto")) > 0 %>' CommandArgument='<%# Eval("refOrd_Id") +";"+ Eval("ref_no_orden") %>' CssClass="btn btn-success t14" ToolTip="Aprobar"><i class="fa fa-check"></i></asp:LinkButton>
                                    <asp:LinkButton runat="server" ID="lnkRefCotDes" CommandName="cotDesApr" Visible='<%# Eval("refEstatus").Equals("AP") %>' CommandArgument='<%# Eval("refOrd_Id") +";"+ Eval("ref_no_orden") %>' CssClass="btn btn-danger t14" ToolTip="Des-Aprobar"><i class="fa fa-trash"></i></asp:LinkButton>
                                    <asp:LinkButton runat="server" ID="lnkRefCotAut" CommandName="cotAutoriza" Visible='<%# Eval("refEstatus").Equals("AP") %>' CommandArgument='<%# Eval("refOrd_Id") +";"+ Eval("ref_no_orden") %>' CssClass="btn btn-success t14" ToolTip="Autorizar"><i class="fa fa-check-circle"></i></asp:LinkButton>
                                    <asp:Literal runat="server" ID="litAutorizado" Text="Autorizado" Visible='<%# Eval("refEstatus").Equals("AU") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Estatus">
                                <ItemTemplate>
                                    <asp:DropDownList runat="server" ID="ddlRefEstatus" AppendDataBoundItems="true" EnableViewState="false" CssClass="alingMiddle input-medium" AutoPostBack="true" OnSelectedIndexChanged="ddlRefEstatus_SelectedIndexChanged" Enabled='<%# !Eval("refEstatus").Equals("NA") %>' DataSourceID="SqlDsRefEstatus" DataTextField="staDescripcion" DataValueField="staRefID" />
                                    <asp:SqlDataSource runat="server" ID="SqlDsRefEstatus" ConnectionString='<%$ ConnectionStrings:PVW %>'
                                        SelectCommand="SELECT [staRefID], [staDescripcion] FROM [Rafacciones_Estatus]"></asp:SqlDataSource>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fecha Solicitud">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txtRefFechSoli" Text='<%# Bind("refFechSolicitud", "{0:d}") %>' Enabled='<%# Eval("refEstatus").Equals("NA") ? false : (Convert.ToInt16(Eval("refEstatusSolicitud")) != 2) %>' CssClass="input-small" />
                                    <cc1:CalendarExtender runat="server" ID="calRefSoli" TargetControlID="txtRefFechSoli" Animated="true" Format="yyyy-MM-dd" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fecha Ent. Est.">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txtRefFechEnt" Text='<%# Bind("refFechEntregaEst") %>' Enabled='<%# Eval("refEstatus").Equals("NA") ? false : Convert.ToInt16(Eval("refEstatusSolicitud")) != 2 %>' CssClass="input-small" />
                                    <cc1:CalendarExtender runat="server" ID="calRefEnt" TargetControlID="txtRefFechEnt" Animated="true" Format="yyyy-MM-dd" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fecha Ent. Real">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txtRefFechReal" Text='<%# Bind("refFechEntregaReal") %>' Enabled='<%# Eval("refEstatus").Equals("NA") ? false : Convert.ToInt16(Eval("refEstatusSolicitud")) == 2 %>' CssClass="input-small" />
                                    <cc1:CalendarExtender runat="server" ID="calRefEntReal" TargetControlID="txtRefFechReal" Animated="true" Format="yyyy-MM-dd" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Guarda Fechas">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="lnkGuardaFech" CommandName="GuardaEstSolic" CommandArgument='<%# Eval("refOrd_Id") +";"+ Eval("ref_no_orden") %>' CssClass="btn btn-success t14" ToolTip="Guardar Estatus, Fecha Solicitud y Fechas de Entrega" OnCommand="lnkGuardaFech_Command"><i class="fa fa-save"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EditRowStyle CssClass="alert-warning" />
                        <EmptyDataRowStyle CssClass="errores" />
                    </asp:GridView>
                </div>
                <div class="table-responsive">
                    <asp:GridView ID="grdRefCotiza" runat="server" DataSourceID="sqlDsRefCotiza" AutoGenerateColumns="False" Caption="Cotización de Refacciones por Proveedor" CssClass="table table-bordered"
                        Visible="false" DataKeyNames="refOrd_Id,ref_no_orden" OnRowDataBound="grdRefCotiza_RowDataBound" OnRowCommand="grdRefCotiza_RowCommand" EmptyDataRowStyle-CssClass="errores" AllowPaging="true" PageSize="7" AllowSorting="true">
                        <Columns>
                            <asp:TemplateField Visible="false" HeaderText="ID">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="lblRefOrdID" Text='<%# Eval("refOrd_Id") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="refDescripcion" HeaderText="Descripcion" ReadOnly="true"></asp:BoundField>
                            <asp:TemplateField HeaderText="Proveedor 1" ShowHeader="true">
                                <HeaderTemplate>
                                    <asp:DropDownList ID="ddlProvs1" runat="server" DataSourceID="SqlDSProvs" DataTextField="razon_social" DataValueField="id_cliprov"
                                        OnSelectedIndexChanged="ddlProvs1_SelectedIndexChanged" AppendDataBoundItems="true" CssClass="alingMiddle input-medium" AutoPostBack="True" Visible='<%# esModoEdicion %>'>
                                        <asp:ListItem Value="-1">Elije Proveedor</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Label runat="server" ID="lblHeadProv1" Text="Proveedor" Visible='<%# !esModoEdicion %>' />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblRefProv1" Visible='<%# esModoEdicion %>' />
                                    <asp:LinkButton runat="server" ID="lnkRefProv1" Text='<%# Bind("provRazSoc") %>' CommandName="Select" Visible='<%# !esModoEdicion %>' CommandArgument='<%# Eval("refOrd_Id")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Costo 1">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" Text='<%# Bind("refCosto") %>' ID="txtlRefCotCost1" CssClass="input-mini" MaxLength="9" Visible='<%# esModoEdicion %>'></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender runat="server" ID="filtxtlRefCotCost1" TargetControlID="txtlRefCotCost1" FilterType="Numbers, Custom" ValidChars="." />
                                    <asp:Label runat="server" Text="" ID="lblRefCotCost1" Visible='<%# !esModoEdicion %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Proveedor 2">
                                <HeaderTemplate>
                                    <asp:DropDownList ID="ddlProvs2" runat="server" AutoPostBack="True" DataSourceID="SqlDSProvs" DataTextField="razon_social" DataValueField="id_cliprov"
                                        OnSelectedIndexChanged="ddlProvs1_SelectedIndexChanged" AppendDataBoundItems="true" CssClass="alingMiddle input-medium" Visible='<%# esModoEdicion %>'>
                                        <asp:ListItem Value="-1">Elije Proveedor</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Label runat="server" ID="lblHeadProv2" Text="Proveedor" Visible='<%# !esModoEdicion %>' />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblRefProv" Visible='<%# esModoEdicion %>' />
                                    <asp:LinkButton runat="server" ID="lnkRefProv2" Text="" CommandName="Select" Visible='<%# !esModoEdicion %>' CommandArgument='<%# Eval("refOrd_Id")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Costo 2">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" Text='<%# Bind("refCosto") %>' ID="txtRefCotCost2" CssClass="input-mini" MaxLength="9" Visible='<%# esModoEdicion %>' />
                                    <cc1:FilteredTextBoxExtender runat="server" ID="filtxtRefCotCost2" TargetControlID="txtRefCotCost2" FilterType="Numbers, Custom" ValidChars="." />
                                    <asp:Label runat="server" Text="" ID="lblRefCotCost2" Visible='<%# !esModoEdicion %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Proveedor 3">
                                <HeaderTemplate>
                                    <asp:DropDownList ID="ddlProvs3" runat="server" DataSourceID="SqlDSProvs" DataTextField="razon_social" DataValueField="id_cliprov"
                                        OnSelectedIndexChanged="ddlProvs1_SelectedIndexChanged" AppendDataBoundItems="true" CssClass="alingMiddle input-medium" AutoPostBack="True" Visible='<%# esModoEdicion %>'>
                                        <asp:ListItem Value="-1">Elije Proveedor</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Label runat="server" ID="lblHeadProv3" Text="Proveedor" Visible='<%# !esModoEdicion %>' />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblRefProv3" Visible='<%# esModoEdicion %>' />
                                    <asp:LinkButton runat="server" ID="lnkRefProv3" Text="" CommandName="Select" Visible='<%# !esModoEdicion %>' CommandArgument='<%# Eval("refOrd_Id")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Costo 3">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" Text='<%# Bind("refCosto") %>' ID="txtRefCotCost3" Visible='<%# esModoEdicion %>' CssClass="input-mini" MaxLength="9" />
                                    <cc1:FilteredTextBoxExtender runat="server" ID="fitxtRefCotCost3l" TargetControlID="txtRefCotCost3" FilterType="Numbers, Custom" ValidChars="." />
                                    <asp:Label runat="server" Text="" ID="lblRefCotCost3" Visible='<%# !esModoEdicion %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <HeaderTemplate>
                                    <asp:DropDownList ID="ddlProvs4" runat="server" DataSourceID="SqlDSProvs" DataTextField="razon_social" DataValueField="id_cliprov"
                                        OnSelectedIndexChanged="ddlProvs1_SelectedIndexChanged" AppendDataBoundItems="true" CssClass="alingMiddle input-medium" AutoPostBack="True" Visible='<%# esModoEdicion %>'>
                                        <asp:ListItem Value="-1">Elije Proveedor</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Label runat="server" ID="lblHeadProv4" Text="Proveedor" Visible='<%# !esModoEdicion %>' />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblRefProv4" Visible='<%# esModoEdicion %>' />
                                    <asp:LinkButton runat="server" ID="lnkRefProv4" Text="" CommandName="Select" Visible='<%# !esModoEdicion %>' CommandArgument='<%# Eval("refOrd_Id")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Costo 4" Visible="false">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" Text='<%# Bind("refCosto") %>' ID="txtRefCotCost4" Visible='<%# esModoEdicion %>' CssClass="input-mini" MaxLength="9" />
                                    <cc1:FilteredTextBoxExtender runat="server" ID="filtxtRefCotCost4" TargetControlID="txtRefCotCost4" FilterType="Numbers, Custom" ValidChars="." />
                                    <asp:Label runat="server" Text="" ID="lblRefCotCost4" Visible='<%# !esModoEdicion %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Proveedor 5" Visible="false">
                                <HeaderTemplate>
                                    <asp:DropDownList ID="ddlProvs5" runat="server" DataSourceID="SqlDSProvs" DataTextField="razon_social" DataValueField="id_cliprov"
                                        OnSelectedIndexChanged="ddlProvs1_SelectedIndexChanged" AppendDataBoundItems="true" CssClass="alingMiddle input-medium" AutoPostBack="True" Visible='<%# esModoEdicion %>'>
                                        <asp:ListItem Value="-1">Elije Proveedor</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Label runat="server" ID="lblHeadProv5" Text="Proveedor" Visible='<%# !esModoEdicion %>' />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblRefProv5" Visible='<%# esModoEdicion %>' />
                                    <asp:LinkButton runat="server" ID="lnkRefProv5" Text="" CommandName="Select" Visible='<%# !esModoEdicion %>' CommandArgument='<%# Eval("refOrd_Id")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Costo 5" Visible="false">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" Text='<%# Bind("refCosto") %>' ID="txtRefCotCost5" Visible='<%# esModoEdicion %>' CssClass="input-mini" MaxLength="9" />
                                    <cc1:FilteredTextBoxExtender runat="server" ID="filtxtRefCotCost5" TargetControlID="txtRefCotCost5" FilterType="Numbers, Custom" ValidChars="." />
                                    <asp:Label runat="server" Text="" ID="lblRefCotCost5" Visible='<%# !esModoEdicion %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EditRowStyle CssClass="alert-warning" />
                        <EmptyDataRowStyle CssClass="errores" />
                    </asp:GridView>
                    <div style="margin: 0 auto" runat="server" id="divRefCotBtns" visible="false">
                        <asp:Literal runat="server" ID="litPorcentGlobal" Text="Porcentaje sobre costo global:&nbsp;" Mode="Encode" />
                        <asp:TextBox runat="server" ID="txtPorcentGlobal" Text="0.00" MaxLength="6" CssClass="input-mini" ToolTip="Si lo desea puede especicar un porcentaje para todas las cotizaciones." />
                        <cc1:FilteredTextBoxExtender runat="server" ID="filtxtPorcentGlobal" TargetControlID="txtPorcentGlobal" FilterType="Numbers, Custom" ValidChars="." />
                        <asp:LinkButton runat="server" ID="lnkGuardaCot" CommandName="Guardar" OnClick="lnkGuardaCot_Click" CssClass="btn btn-info t14"><i class="fa fa-plus-circle"></i>&nbsp;Guardar Cotización</asp:LinkButton>
                        <asp:LinkButton runat="server" ID="lnkAddProv" OnClick="lnkAddProv_Click" Enabled="true" CssClass="btn btn-info t14"><i class="fa fa-plus-circle"></i>&nbsp;Agregar Proveedor</asp:LinkButton>
                    </div>
                    <asp:SqlDataSource runat="server" ID="sqlDSRefOrden" ConnectionString='<%$ ConnectionStrings:PVW %>'
                        SelectCommand="SELECT [refOrd_Id], [ref_no_orden], [refDescripcion], [refCantidad], refProveedor, refCosto, 
                    (SELECT razon_social FROM Cliprov WHERE id_cliprov = ro.refProveedor  AND tipo = 'P') AS provRazSoc, refPorcentSobreCosto, refPrecioVenta, refEstatus, refEstatusSolicitud, refFechSolicitud, refFechEntregaEst, refFechEntregaReal
                    FROM [Refacciones_Orden] AS ro 
                    WHERE ([ref_no_orden] = @ref_no_orden)"
                        DeleteCommand="DELETE FROM [Refacciones_Orden] WHERE [refOrd_Id] = @refOrd_Id AND [ref_no_orden] = @ref_no_orden"
                        InsertCommand="INSERT INTO [Refacciones_Orden] ([refDescripcion], [refCantidad], [refObservaciones], refProveedor, refCosto, [ref_no_orden], ref_id_empresa, ref_id_taller, refPorcentSobreCosto, refEstatusSolicitud, refFechSolicitud, refFechEntregaEst, refFechEntregaReal) VALUES (@refDescripcion, @refCantidad, @refObservaciones, @refProveedor, @refCosto, @ref_no_orden, @ref_id_empresa, @ref_id_taller, @refPorcentSobreCosto, @refEstatusSolicitud, @refFechSolicitud, @refFechEntregaEst, @refFechEntregaReal)"
                        UpdateCommand="UPDATE [Refacciones_Orden] SET [refDescripcion] = @refDescripcion, [refCantidad] = @refCantidad, refPorcentSobreCosto = @refPorcentSobreCosto, refPrecioVenta = @refPrecioVenta, [refObservaciones] = @refObservaciones WHERE [refOrd_Id] = @refOrd_Id AND [ref_no_orden] = @ref_no_orden">
                        <DeleteParameters>
                            <asp:Parameter Name="refOrd_Id" Type="Int32"></asp:Parameter>
                            <asp:QueryStringParameter QueryStringField="o" Name="ref_no_orden" Type="Int32"></asp:QueryStringParameter>
                        </DeleteParameters>
                        <InsertParameters>
                            <asp:ControlParameter ControlID="txtRefDesc" Name="refDescripcion" Type="String"></asp:ControlParameter>
                            <asp:ControlParameter ControlID="txtRefCant" Name="refCantidad" Type="Int16"></asp:ControlParameter>
                            <asp:Parameter Name="refObservaciones" Type="String" DefaultValue="s/obs."></asp:Parameter>
                            <asp:QueryStringParameter QueryStringField="o" Name="ref_no_orden" Type="Int32"></asp:QueryStringParameter>
                            <asp:Parameter Name="refProveedor" Type="Int32" ConvertEmptyStringToNull="true" />
                            <asp:Parameter Name="refCosto" Type="Decimal" DefaultValue="0" />
                            <asp:Parameter Name="refPorcentSobreCosto" Type="Decimal" DefaultValue="0" />
                            <asp:Parameter Name="refPrecioVenta" Type="Decimal" ConvertEmptyStringToNull="true" />
                            <asp:Parameter Name="refEstatusSolicitud" Type="Int16" DefaultValue="4" />
                            <asp:Parameter Name="refFechSolicitud" Type="DateTime" ConvertEmptyStringToNull="true" />
                            <asp:Parameter Name="refFechEntregaEst" Type="DateTime" ConvertEmptyStringToNull="true" />
                            <asp:Parameter Name="refFechEntregaReal" Type="DateTime" ConvertEmptyStringToNull="true" />
                            <asp:QueryStringParameter Name="ref_id_empresa" QueryStringField="u" Type="Int32" />
                            <asp:QueryStringParameter Name="ref_id_taller" QueryStringField="t" Type="Int32" />
                        </InsertParameters>
                        <SelectParameters>
                            <asp:QueryStringParameter QueryStringField="o" Name="ref_no_orden" Type="Int32"></asp:QueryStringParameter>
                        </SelectParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="refDescripcion" Type="String"></asp:Parameter>
                            <asp:Parameter Name="refCantidad" Type="Int16"></asp:Parameter>
                            <asp:Parameter Name="refObservaciones" Type="String" DefaultValue="s/obs."></asp:Parameter>
                            <asp:Parameter Name="refOrd_Id" Type="Int32"></asp:Parameter>
                            <asp:QueryStringParameter QueryStringField="o" Name="ref_no_orden" Type="Int32"></asp:QueryStringParameter>
                            <asp:Parameter Name="refPorcentSobreCosto" Type="Decimal" DefaultValue="0" />
                            <asp:Parameter Name="refPrecioVenta" Type="Decimal" DefaultValue="0" />
                            <asp:Parameter Name="refEstatusSolicitud" Type="Int16" DefaultValue="4" />
                        </UpdateParameters>
                    </asp:SqlDataSource>
                    <asp:SqlDataSource runat="server" ID="SqlDSProvs" ConnectionString='<%$ ConnectionStrings:PVW %>'
                        SelectCommand="SELECT [razon_social], [id_cliprov] FROM [Cliprov] WHERE ([tipo] = 'P')"></asp:SqlDataSource>
                    <asp:SqlDataSource runat="server" ID="sqlDsRefCotiza" ConnectionString='<%$ ConnectionStrings:PVW %>'
                        SelectCommand="SELECT refOrd_Id, ref_no_orden, refDescripcion, refProveedor, refCosto,
                    (SELECT razon_social FROM Cliprov WHERE id_cliprov = ro.refProveedor  AND tipo = 'P') AS provRazSoc FROM Refacciones_Orden AS ro WHERE (ref_no_orden = @ref_no_orden) ORDER BY refOrd_Id">
                        <%--SelectCommand="SELECT refOrd_Id, ref_no_orden,refDescripcion, (SELECT cotProv_ID FROM Refacciones_Cotiza WHERE (cot_refOrd_ID = ro.refOrd_Id)) AS provID, (SELECT cotCosto FROM Refacciones_Cotiza WHERE (cot_refOrd_ID = ro.refOrd_Id)) AS Costo FROM Refacciones_Orden AS ro WHERE (ref_no_orden = @ref_no_orden)--%>
                        <SelectParameters>
                            <asp:QueryStringParameter QueryStringField="o" Name="ref_no_orden" Type="Int32"></asp:QueryStringParameter>
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

