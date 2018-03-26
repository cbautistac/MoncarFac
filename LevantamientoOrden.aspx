<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LevantamientoOrden.aspx.cs" Inherits="LevantamientoOrden" MasterPageFile="~/AdmonOrden.master" UICulture="es-Mx" Culture="es-Mx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" />
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server"></telerik:RadSkinManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-lg-12 col-sm-12 text-center alert-info">
                    <h3>
                        <i class="fa fa-car"></i><i class="fa fa-chain-broken"></i>&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="Label1" runat="server" Text="Presupuesto"></asp:Label>
                    </h3>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-12 col-sm-12 text-center">
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="errores" DisplayMode="List" ValidationGroup="mo" />
                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" CssClass="errores" DisplayMode="List" ValidationGroup="edit" />
                    <asp:ValidationSummary runat="server" ID="vsuAddRefacc" CssClass="errores" ValidationGroup="valRefacc" DisplayMode="List" />
                    <asp:ValidationSummary runat="server" ID="ValidationSummary3" CssClass="errores" ValidationGroup="valRefaccMod" DisplayMode="List" />
                    <asp:Label ID="lblErrorNew" runat="server" CssClass="alert-danger" />
                </div>
            </div>
            <asp:Panel runat="server" ID="pnlDaños" CssClass="paneles col-lg-12 col-sm-12" ScrollBars="Auto">
                <div class="row">
                    <div class="col-lg-6 col-sm-6">
                        <div class="row text-center">
                            <div class="col-lg-12 col-sm-12 pad1m alert-info textoBold text-center">Mano de Obra</div>
                        </div>
                        <div class="row">
                            <div class="col-lg-2 col-sm-2 text-left">
                                <asp:Label ID="Label87" runat="server" Text="Grupo:" CssClass="textoBold alingMiddle" /></div>
                            <div class="col-lg-4 col-sm-4 text-left">
                                <telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="ddlGONew" AllowCustomText="true" Width="210px" MaxHeight="300px" DataSourceID="SqlDataSourceGO" DataTextField="descripcion_go" DataValueField="id_grupo_op" Skin="MetroTouch" EmptyMessage="Seleccione Grupo" Filter="Contains" ></telerik:RadComboBox>                                
                                <asp:SqlDataSource ID="SqlDataSourceGO" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="SELECT [id_grupo_op], [descripcion_go] FROM [Grupo_Operacion]"></asp:SqlDataSource>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-2 col-sm-2 text-left">
                                <asp:Label ID="Label88" runat="server" Text="Operación:" CssClass="textoBold alingMiddle" /></div>
                            <div class="col-lg-4 col-sm-4 text-left">
                                <telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="ddlOpNew" AllowCustomText="true" Width="210px" MaxHeight="300px" DataSourceID="SqlDataSourceOp"  DataTextField="descripcion_op" DataValueField="id_operacion" Skin="MetroTouch" EmptyMessage="Seleccione Operación" Filter="Contains" ></telerik:RadComboBox>                                                                
                                <asp:SqlDataSource ID="SqlDataSourceOp" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="SELECT [id_operacion], [descripcion_op] FROM [Operaciones]"></asp:SqlDataSource>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-2 col-sm-2 text-left">
                                <asp:Label ID="Label89" runat="server" Text="Descripción:" CssClass="textoBold alingMiddle" /></div>
                            <div class="col-lg-4 col-sm-4 text-left">
                                <asp:TextBox ID="txtRefaccion" runat="server" CssClass="alingMiddle input-large" MaxLength="150" />                                
                                <cc1:TextBoxWatermarkExtender ID="txtRefaccionWatermarkExtender1" runat="server" BehaviorID="txtRefaccion_TextBoxWatermarkExtender" TargetControlID="txtRefaccion" WatermarkText="Descripción" WatermarkCssClass="input-large water" />
                            </div>
                            <div class="col-lg-3 col-sm-3 text-center">
                                <asp:CheckBox ID="chkRefaccion" runat="server" AutoPostBack="True" Text="Refacción" OnCheckedChanged="chkRefaccion_CheckedChanged" /></div>
                        </div>
                        <div class="row">
                            <div class="col-lg-2 col-sm-2 text-left">
                                <asp:Label ID="Label90" runat="server" Text="Monto:" CssClass="textoBold alingMiddle" /></div>
                            <div class="col-lg-7 col-sm-7 text-left">
                                <telerik:RadNumericTextBox RenderMode="Lightweight" runat="server" ID="txtMontoNew" CssClass="input-mini" EmptyMessage="Monto" MinValue="0" MaxValue="9999999.99" ShowSpinButtons="true" NumberFormat-DecimalDigits="2" Skin="MetroTouch"></telerik:RadNumericTextBox>                                
                            </div>
                            <div class="col-lg-3 col-sm-3 text-center">
                                <asp:LinkButton ID="btnAceptarNew" runat="server" ToolTip="Agregar" OnClick="btnAceptarNew_Click" CssClass="btn btn-info t14"><i class="fa fa-plus-circle"></i>&nbsp;<span>Agregar M.O.</span></asp:LinkButton></div>
                        </div>                        
                    </div>
                    <div class="col-lg-6 col-sm-6">
                        <div class="row text-center">
                            <div class="col-lg-12 col-sm-12 pad1m alert-info textoBold text-center">Refacciones</div>
                        </div>
                        <div class="row">
                            <div class="col-lg-2 col-sm-2 text-left">
                                <asp:Label ID="Label3" runat="server" Text="Refacción:" CssClass="textoBold alingMiddle" /></div>
                            <div class="col-lg-4 col-sm-4 text-left">
                                <asp:TextBox runat="server" ID="txtRefDesc" CssClass="alingMiddle input-large" MaxLength="150" />
                                <cc1:TextBoxWatermarkExtender ID="txtRefDesc_TextBoxWatermarkExtender" runat="server" BehaviorID="txtRefDesc_TextBoxWatermarkExtender" TargetControlID="txtRefDesc" WatermarkCssClass="water input-large" WatermarkText="Refacción" />
                                <asp:RequiredFieldValidator runat="server" ID="rfvRefDescrip" ControlToValidate="txtRefDesc" CssClass="errores" ErrorMessage="Escribe la descripción de la Refacción." Text="*" ValidationGroup="valRefacc" />
                            </div>
                        </div>                        
                        <div class="row">
                            <div class="col-lg-2 col-sm-2 text-left">
                                <asp:Label ID="Label5" runat="server" Text="Cantidad:" CssClass="textoBold alingMiddle" /></div>
                            <div class="col-lg-3 col-sm-3 text-left">
                                <telerik:RadNumericTextBox RenderMode="Lightweight" runat="server" ID="txtRefCant" CssClass="input-mini" Value="0" EmptyMessage="Cantidad" MinValue="0" MaxValue="999" ShowSpinButtons="true" NumberFormat-DecimalDigits="0" Skin="MetroTouch"></telerik:RadNumericTextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-2 col-sm-2 text-left">
                                <asp:Label ID="Label11" runat="server" Text="Estatus Refacción:" CssClass="textoBold alingMiddle" /></div>
                            <div class="col-lg-4 col-sm-4 text-left">
                                <asp:DropDownList ID="ddlEstatusRefaccion" runat="server" Enabled="false">
                                    <asp:ListItem Text="Evaluación" Value="EV" />
                                    <asp:ListItem Text="Reparación" Value="RP" />
                                    <asp:ListItem Text="Compra" Selected="True" Value="CO" />
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-2 col-sm-2 text-left">
                                <asp:Label ID="Label7" runat="server" Text="Precio Unitario:" CssClass="textoBold alingMiddle" /></div>
                            <div class="col-lg-4 col-sm-4 text-left">                                
                                <telerik:RadNumericTextBox RenderMode="Lightweight" runat="server" ID="txtPrecio" CssClass="input-mini" Value="0" EmptyMessage="Precio" MinValue="0" MaxValue="9999999.99" ShowSpinButtons="true" NumberFormat-DecimalDigits="2" Skin="MetroTouch"></telerik:RadNumericTextBox>                                
                            </div>
                            <div class="col-lg-2 col-sm-2 text-right">
                                <asp:LinkButton ID="btnAddRefac" runat="server" ToolTip="Agregar Refacción" OnClick="btnAddRefac_Click" CssClass="btn btn-info t14" ValidationGroup="valRefacc"><i class="fa fa-plus-circle"></i>&nbsp;<span>Agregar Refacci&oacute;n</span></asp:LinkButton>
                            </div>
                        </div>                        
                    </div>
                </div>
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
                                        <asp:Panel ID="Panel4" runat="server" CssClass="panelCatalogos text-center" ScrollBars="Auto">
                                            <div class="table-responsive">
                                                <asp:GridView ID="GridView2" runat="server" DataSourceID="SqlDataSourceDanos" EmptyDataRowStyle-CssClass="errores"
                                                    OnRowCommand="GridView2_RowCommand" OnRowDataBound="GridView2_RowDataBound"
                                                    EmptyDataText="No se ha registrado mano de obra" CssClass="table table-bordered"
                                                    AutoGenerateColumns="false"
                                                    DataKeyNames="id_empresa,id_taller,no_orden,consecutivo" AllowSorting="true" OnRowDeleted="GridView2_RowDeleted">
                                                    <Columns>
                                                        <asp:BoundField DataField="id_empresa" HeaderText="id_empresa" ReadOnly="True" SortExpression="id_empresa" Visible="False" />
                                                        <asp:BoundField DataField="id_taller" HeaderText="id_taller" ReadOnly="True" SortExpression="id_taller" Visible="False" />
                                                        <asp:BoundField DataField="no_orden" HeaderText="no_orden" ReadOnly="True" SortExpression="no_orden" Visible="False" />
                                                        <asp:TemplateField Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblConsecutivoEdit" Text='<%# Bind("no_orden") %>' /></ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="consecutivo" HeaderText="consecutivo" ReadOnly="True" SortExpression="consecutivo" Visible="False" />
                                                        <asp:TemplateField HeaderText="Grupo Operación" SortExpression="descripcion_go">
                                                            <EditItemTemplate>
                                                                <asp:DropDownList runat="server" ID="ddlGoEditDanos"
                                                                    DataSourceID="SqlDataSourceGOEdit" DataTextField="descripcion_go"
                                                                    DataValueField="id_grupo_op" CssClass="alingMiddle input-large"
                                                                    SelectedValue='<%# Bind("id_grupo_op") %>'>
                                                                </asp:DropDownList>
                                                                <asp:SqlDataSource runat="server" ID="SqlDataSourceGOEdit" ConnectionString='<%$ ConnectionStrings:PVW %>'
                                                                    SelectCommand="SELECT [id_grupo_op], [descripcion_go] FROM [Grupo_Operacion]"></asp:SqlDataSource>
                                                            </EditItemTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblGO" Text='<%# Bind("descripcion_go") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="ancho100px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Operación" SortExpression="descripcion_op">
                                                            <EditItemTemplate>
                                                                <asp:DropDownList runat="server" ID="ddlOpEditDanos" DataSourceID="SqlDataSourceOPEdit" DataTextField="descripcion_op" DataValueField="id_operacion" CssClass="alingMiddle input-large" SelectedValue='<%# Bind("id_operacion") %>'>
                                                                </asp:DropDownList>
                                                                <asp:SqlDataSource runat="server" ID="SqlDataSourceOPEdit" ConnectionString='<%$ ConnectionStrings:PVW %>'
                                                                    SelectCommand="SELECT [id_operacion], [descripcion_op] FROM [Operaciones]"></asp:SqlDataSource>
                                                            </EditItemTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblOp" Text='<%# Bind("descripcion_op") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="ancho100px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Descripción" SortExpression="id_refaccion">
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="txtRefaccionMod" runat="server" CssClass="alingMiddle input-large" MaxLength="150" Text='<%# Bind("id_refaccion") %>' />
                                                                <cc1:TextBoxWatermarkExtender ID="txtRefaccionModWatermarkExtender1" runat="server" BehaviorID="txtRefaccionMod_TextBoxWatermarkExtender" TargetControlID="txtRefaccionMod" WatermarkText="Descripción" WatermarkCssClass="input-large water" />
                                                            </EditItemTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblRef" Text='<%# Bind("id_refaccion") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="ancho100px" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="monto_ini" HeaderText="Monto Inicial" ReadOnly="True" SortExpression="monto_ini" DataFormatString="{0:C2}" />
                                                        <asp:TemplateField HeaderText="Monto Autorizado" SortExpression="monto_mo">
                                                            <EditItemTemplate>
                                                                <telerik:RadNumericTextBox RenderMode="Lightweight" runat="server" ID="txtMontoEdit" CssClass="input-mini" DbValue='<%# Eval("monto_mo") %>' EmptyMessage="Monto Autorizado" MinValue="0" MaxValue="9999999.99" ShowSpinButtons="true" NumberFormat-DecimalDigits="2" Skin="MetroTouch"></telerik:RadNumericTextBox>                                                
                                                            </EditItemTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblMontoEdit" Text='<%# Bind("monto_mo","{0:C2}") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="ancho70px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ShowHeader="False">
                                                            <EditItemTemplate>
                                                                <asp:LinkButton ID="lnkActualiza" runat="server" CausesValidation="True" CommandName="Update" ToolTip="Guardar" CssClass="btn btn-success t14" CommandArgument='<%# Eval("consecutivo") %>'><i class="fa fa-save"></i></asp:LinkButton>
                                                            </EditItemTemplate>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkEditar" runat="server" CausesValidation="False" CommandName="Edit" ToolTip="Editar" CssClass="btn btn-warning t14"><i class="fa fa-edit"></i></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ShowHeader="False">
                                                            <EditItemTemplate>
                                                                <asp:LinkButton ID="lnkCancelar" runat="server" CausesValidation="false" CommandName="Cancel" ToolTip="Cancelar" CssClass="btn btn-danger t14"><i class="fa fa-times-circle"></i></asp:LinkButton>
                                                            </EditItemTemplate>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkEliminar" runat="server" CausesValidation="False" CommandName="Delete" ToolTip="Eliminar" CssClass="btn btn-danger t14" CommandArgument='<%# Eval("consecutivo") %>' OnClientClick="return confirm('¿Está seguro de eliminar la mano de obra?')"><i class="fa fa-trash"></i></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EditRowStyle CssClass="alert-warning" />
                                                    <EmptyDataRowStyle CssClass="errores" />
                                                </asp:GridView>
                                                <asp:SqlDataSource ID="SqlDataSourceDanos" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>"
                                                    InsertCommand="insert into mano_obra values (@id_empresa,@id_taller,@no_orden,(select isnull(max(mo.consecutivo),0) from mano_obra mo where id_empresa=@id_empresa and id_taller=@id_taller and no_orden=@no_orden)+1,@id_grupo_op,@id_operacion,lower(@id_refaccion),@monto_mo,@monto_ini,0,'P','P',0,0,0,0,0,0,null,null)"
                                                    SelectCommand="select mo.id_empresa,mo.id_taller,mo.no_orden,mo.consecutivo,mo.id_grupo_op,mo.id_operacion,
                                                        mo.id_refaccion,isnull(mo.monto_mo,0) as monto_mo,gop.descripcion_go,op.descripcion_op,
                                                        isnull(mo.monto_ini,0) as monto_ini,mo.estatus_presupuesto,mo.idCfd,mo.statusCfd,mo.aplica_rem,mo.aplica_ss from mano_obra mo
                                                        inner join Grupo_Operacion gop on gop.id_grupo_op=mo.id_grupo_op
                                                        inner join Operaciones op on op.id_operacion=mo.id_operacion                                        
                                                        where id_empresa=@id_empresa and id_taller=@id_taller and no_orden=@no_orden">
                                                    <SelectParameters>
                                                        <asp:QueryStringParameter Name="no_orden" QueryStringField="o" Type="Int32" />
                                                        <asp:QueryStringParameter Name="id_empresa" QueryStringField="e" Type="Int32" />
                                                        <asp:QueryStringParameter Name="id_taller" QueryStringField="t" Type="Int32" />
                                                    </SelectParameters>
                                                    <InsertParameters>
                                                        <asp:QueryStringParameter Name="no_orden" QueryStringField="o" Type="Int32" />
                                                        <asp:QueryStringParameter Name="id_empresa" QueryStringField="e" Type="Int32" />
                                                        <asp:QueryStringParameter Name="id_taller" QueryStringField="t" Type="Int32" />
                                                        <asp:ControlParameter ControlID="ddlGONew" Name="id_grupo_op" Type="Int32" PropertyName="SelectedValue" />
                                                        <asp:ControlParameter ControlID="ddlOpNew" Name="id_operacion" Type="Int32" PropertyName="SelectedValue" />
                                                        <asp:ControlParameter ControlID="txtRefaccion" Name="id_refaccion" Type="String" PropertyName="Text"  />
                                                        <asp:ControlParameter ControlID="txtMontoNew" Name="monto_mo" Type="Decimal" PropertyName="Text" />
                                                        <asp:ControlParameter ControlID="txtMontoNew" Name="monto_ini" Type="Decimal" PropertyName="Text" />
                                                    </InsertParameters>
                                                </asp:SqlDataSource>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                </div>
                            </telerik:RadPageView>
                            <telerik:RadPageView runat="server" ID="RadPageView_REF">
                                <div class="row">
                                    <div class="col-lg-12 col-sm-12">
                                        <asp:Panel ID="Panel1" runat="server" CssClass="panelCatalogos text-center" ScrollBars="Auto">
                                            <div class="table-responsive">
                                                <asp:GridView ID="grdRefacOrd" runat="server" 
                                                    EmptyDataText="No se han agregado refacciones a la orden." 
                                                    CssClass="table table-bordered" EmptyDataRowStyle-CssClass="errores" AutoGenerateColumns="False"
                                                    DataKeyNames="refOrd_Id" DataSourceID="sqlDSRefOrden" AllowSorting="True" OnRowCommand="grdRefacOrd_RowCommand"
                                                    OnRowDeleted="grdRefacOrd_RowDeleted" OnRowDataBound="grdRefacOrd_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Refacción" InsertVisible="False" SortExpression="refDescripcion">
                                                            <EditItemTemplate>
                                                                <asp:TextBox runat="server" Text='<%# Bind("refDescripcion") %>' ID="TextBox1" CssClass="alingMiddle input-large" MaxLength="150"></asp:TextBox>
                                                                <cc1:TextBoxWatermarkExtender ID="TextBox1_TextBoxWatermarkExtender" runat="server" BehaviorID="TextBox1_TextBoxWatermarkExtender" TargetControlID="TextBox1" WatermarkCssClass="water input-large" WatermarkText="Refacción" />
                                                                <asp:RequiredFieldValidator runat="server" ID="rfvRefDescripMod" ControlToValidate="TextBox1" CssClass="errores" ErrorMessage="Escribe la descripción de la Refacción." Text="*" ValidationGroup="valRefaccMod" />
                                                            </EditItemTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# Bind("refDescripcion") %>' ID="Label31"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>                                                                                                               
                                                        <asp:TemplateField HeaderText="Cantidad" InsertVisible="False" SortExpression="refCantidad">
                                                            <EditItemTemplate>
                                                                <telerik:RadNumericTextBox RenderMode="Lightweight" runat="server" ID="txtEdtRefCant" CssClass="input-mini" DbValue='<%# Eval("refCantidad") %>' EmptyMessage="Cantidad" MinValue="0" MaxValue="999" ShowSpinButtons="true" NumberFormat-DecimalDigits="0" Skin="MetroTouch"></telerik:RadNumericTextBox>                                               
                                                            </EditItemTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# Bind("refCantidad") %>' ID="Label2"></asp:Label>
                                                                <asp:Label runat="server" Text='<%# Bind("estatus") %>' Visible=false ID="Label13"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Precio Unitario" SortExpression="precio_venta_ini">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblPrecioVentaIni" Text='<%# Bind("precio_venta_ini") %>' />
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <telerik:RadNumericTextBox RenderMode="Lightweight" runat="server" ID="txtEdtRefCosto" CssClass="input-mini" DbValue='<%# Eval("precio_venta_ini") %>' EmptyMessage="Precio Unitario"  ShowSpinButtons="true"  Skin="MetroTouch"></telerik:RadNumericTextBox>                                               
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Importe" InsertVisible="False" SortExpression="importe">
                                                            <EditItemTemplate></EditItemTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# Bind("importe","{0:C2}") %>' ID="Label4"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                      
                                                        <asp:TemplateField ShowHeader="False">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkEditarR" runat="server" CausesValidation="False" CommandName="Edit" ToolTip="Editar" CssClass="btn btn-warning t14"><i class="fa fa-edit"></i></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:LinkButton ID="lnkActualizaR" runat="server" CausesValidation="True" CommandName="Update" ToolTip="Guardar" CssClass="btn btn-success t14" CommandArgument='<%# Eval("refOrd_Id") %>' ValidationGroup="valRefaccMod"><i class="fa fa-save"></i></asp:LinkButton>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ShowHeader="False">
                                                            <EditItemTemplate>
                                                                <asp:LinkButton ID="lnkCancelarR" runat="server" CausesValidation="false" CommandName="Cancel" ToolTip="Cancelar" CssClass="btn btn-danger t14"><i class="fa fa-times-circle"></i></asp:LinkButton>
                                                            </EditItemTemplate>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkEliminarR" runat="server" CausesValidation="False" CommandName="Delete" ToolTip="Eliminar" CssClass="btn btn-danger t14" CommandArgument='<%# Eval("refOrd_Id") %>' OnClientClick="return confirm('¿Está seguro de eliminar la refacción?')"><i class="fa fa-trash"></i></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EditRowStyle CssClass="alert-warning" />
                                                    <EmptyDataRowStyle CssClass="errores" />
                                                </asp:GridView>
                                                <asp:SqlDataSource runat="server" ID="sqlDSRefOrden" ConnectionString='<%$ ConnectionStrings:PVW %>'
                                                    SelectCommand="SELECT ro.refOrd_Id, ro.ref_no_orden, ro.refDescripcion,ro.refestatus,ro.refestatussolicitud,ro.refCantidad, ro.refProveedor, ro.refCosto,case ro.refestatus when 'NA' then 'No Aplica' when 'CO' then 'Compra' when 'RP' then 'Reparación' when 'EV' then 'Evaluación' when 'AP' then 'Aplicado' when 'AU' then 'Autorizado' when 'FA' then 'Facturado' when 'CA' then 'Cancelada' else 'No Aplica' end as estatus,
                                                        (SELECT c.razon_social FROM Cliprov c WHERE c.id_cliprov = ro.refProveedor AND c.tipo = 'P') AS provRazSoc, ro.refPorcentSobreCosto, ro.refPrecioVenta, ro.refObservaciones, (isnull(ro.refPrecioVenta,0)*ro.refCantidad) as importe, ro.estatus_presupuesto
                                                        ,ro.precio_venta_ini,ro.aplica_rem,ro.aplica_ss, ISNULL(ro.id_Procedencia,-1) AS id_Procedencia, p.proc_Descrip,isnull(ro.idCfd,0) as idCfd 
                                                        FROM Refacciones_Orden ro 
                                                        LEFT JOIN cat_Procedencia p on p.id_Proc=ro.id_procedencia
                                                        WHERE ([ref_no_orden] = @ref_no_orden) and [ref_id_empresa]=@ref_id_empresa and [ref_id_taller]=@ref_id_taller and ro.proceso is null and ro.refEstatusSolicitud <> 11 order by refOrd_id">
                                                    <SelectParameters>
                                                        <asp:QueryStringParameter Name="ref_no_orden" QueryStringField="o" Type="Int32" />
                                                        <asp:QueryStringParameter Name="ref_id_empresa" QueryStringField="e" Type="Int32" />
                                                        <asp:QueryStringParameter Name="ref_id_taller" QueryStringField="t" Type="Int32" />
                                                    </SelectParameters>
                                                </asp:SqlDataSource>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                </div>
                            </telerik:RadPageView>                                    
                        </telerik:RadMultiPage>
                    </div>
                </div>
            </asp:Panel>
            <div class="row pad1m">
                 <div class="col-lg-6 col-sm-6 text-center">
                     <asp:Label ID="lblTotalManoObra" runat="server" CssClass="font-14 textoBold text-primary"></asp:Label></div>
                <div class="col-lg-6 col-sm-6 text-center">
                    <asp:Label ID="lblTotalRefacciones" runat="server" CssClass="font-14 textoBold text-primary"></asp:Label></div>
            </div>
            <div class="row pad1m">
                <div class="col-lg-6 col-sm-6 text-center pad1m">
                    <asp:Label ID="Label9" runat="server" Text="Firmantes:"></asp:Label>
                    <asp:DropDownList ID="ddlFirmantes" runat="server" DataSourceID="SqlDataSource1" DataTextField="firmante" DataValueField="id_firma">
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                        ConnectionString="<%$ ConnectionStrings:PVW %>"
                        SelectCommand="SELECT [id_firma], [firmante] FROM [Firmantes]"></asp:SqlDataSource>
                    <asp:DropDownList ID="ddlDetalle" runat="server">
                        <asp:ListItem Text="No Detallado" Selected="True" Value="N" />
                        <asp:ListItem Text="Detallado" Value="S" />
                    </asp:DropDownList>
                    <asp:LinkButton ID="btnImprime" runat="server" ToolTip="Presupuesto" CssClass="btn btn-info t14" OnClick="btnImprime_Click"><i class="fa fa-print"></i><span>&nbsp;Imprime Presupuesto</span></asp:LinkButton>
                </div>
                <div class="col-lg-2 col-sm-2 text-center pad1m">
                    <asp:LinkButton ID="lnkTerminado" runat="server" ToolTip="Presupuesto Terminado"
                        CssClass="btn btn-success t14" OnClick="lnkTerminado_Click" OnClientClick="return confirm('¿Está seguro dar por teminado el presupuesto?, tome en cuenta que una vez terminado no puede ser modificado de nuevo')"><i class="fa fa-check-square-o"></i><span>&nbsp;Presupuesto Terminado</span></asp:LinkButton>
                </div>
                <%--<div class="col-lg-2 col-sm-2 text-center pad1m">
                    <asp:LinkButton ID="lnkNotificar" runat="server" ToolTip="Notificar"
                        CssClass="btn btn-warning t14" OnClick="lnkNotificar_Click"><i class="fa fa-bell"></i><span>&nbsp;Notificar</span></asp:LinkButton>
                </div>--%>
                <div class="col-lg-2 col-sm-2 text-center pad1m">
                    <asp:LinkButton ID="lnkCotizar" runat="server" ToolTip="Cotizar"
                        CssClass="btn btn-primary t14" OnClick="lnkCotizar_Click"><i class="fa fa-dollar"></i><span>&nbsp;Cotizar</span></asp:LinkButton>
                </div>
                <div class="col-lg-2 col-sm-2 text-center pad1m">
                    <asp:LinkButton ID="lnkEditar" runat="server" ToolTip="Activa Presupuesto" Visible="false"
                        CssClass="btn btn-warning t14" OnClick="lnkEditar_Click"><i class="fa fa-mail-reply"></i><span>&nbsp;Habilitar Edición</span></asp:LinkButton>
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


            <asp:Panel ID="PanelMask" runat="server" CssClass="mask" Visible="false"></asp:Panel>
            <asp:Panel ID="PanelPopUpPermiso" runat="server" CssClass="popUp zen2  textoCentrado ancho40 centrado" Visible="false">                
                <table class="ancho100">
                    <tr class="ancho100 centrado">
                        <td class="ancho100 text-center encabezadoTabla roundTopLeft ">
                            <asp:Label ID="lblPop" runat="server" Text="Autorización" CssClass="t22 colorMorado textoBold" />                              
                        </td>                        
                    </tr>                        
                </table>
                <asp:Panel ID="prov" runat="server" CssClass="ancho80 centrado">
                    <div class="row">
                        <div class="col-lg-4 col-sm-4 text-left">
                            <asp:Label ID="Label14" runat="server" Text="Usuario:" CssClass="textoBold" />
                        </div>
                        <div class="col-lg-8 col-sm-8 text-left">
                            <asp:TextBox ID="txtUsuarioLog" runat="server" CssClass="login input-large" MaxLength="20" placeholder="Usuario" TextMode="Password" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" Text="*" ForeColor="Red" ValidationGroup="log" ControlToValidate="txtUsuarioLog" ErrorMessage="Debe indicar el usuario." CssClass="pull-right" />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" Text="*" ForeColor="Red" ValidationGroup="log" ErrorMessage="El usuario debe contener de entre 3 y 20 caracteres." CssClass="pull-right" ControlToValidate="txtUsuarioLog" ValidationExpression="[a-zA-Z0-9]{3,20}" />
                        </div>                                                
                        <div class="col-lg-4 col-sm-4 text-left padding-top-10">
                            <asp:Label ID="Label15" runat="server" Text="Contraseña:" CssClass="textoBold"/>&nbsp;
                        </div>
                        <div class="col-lg-8 col-sm-8 text-left padding-top-10">
                            <asp:TextBox ID="txtContraseñaLog" runat="server" CssClass="login input-large" TextMode="Password" MaxLength="20" placeholder="Contraseña" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" Text="*" ForeColor="Red" ValidationGroup="log" ControlToValidate="txtContraseñaLog" ErrorMessage="Debe indicar la contraseña." CssClass="pull-right" />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" Text="*" ForeColor="Red" ValidationGroup="log" ErrorMessage="La contraseña debe contener de entre 5 y 20 caracteres." CssClass="pull-right" ControlToValidate="txtContraseñaLog" ValidationExpression="[a-zA-Z0-9]{5,20}" />
                        </div>                        
                        <div class="field col-lg-12 col-sm-12 textoCentrado textoBold padding-top-10">
                            <div class="col-lg-12 col-sm-12 text-center">
                                <asp:Label ID="lblErrorLog" runat="server" CssClass="errores" />
                            </div>
                            <div class="col-lg-12 col-sm-12 text-center">
                                <asp:ValidationSummary ID="ValidationSummary4" ValidationGroup="log" CssClass="errores" runat="server" DisplayMode="List" />
                            </div>
                        </div>
                            
                        <div class="col-lg-12 col-sm-12 text-center pad1m">
                            <div class="col-lg-6 col-sm-6 text-center">
                                <asp:LinkButton ID="btnAceptar" runat="server" OnClick="btnAceptar_Click" CssClass="btn btn-success" ValidationGroup="log"><i class="fa fa-check"></i><span>&nbsp;Autorizar</span></asp:LinkButton>
                            </div>
                            <div class="col-lg-6 col-sm-6 text-center">
                                <asp:LinkButton ID="btnCancelar" OnClick="btnCancelar_Click" runat="server" CssClass="btn btn-danger"><i class="fa fa-ban"></i><span>&nbsp;Cancelar</span></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                 </asp:Panel>
            </asp:Panel>



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
