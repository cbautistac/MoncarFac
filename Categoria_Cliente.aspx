<%@ Page Title="" Language="C#" MasterPageFile="~/Administracion.master" AutoEventWireup="true" CodeFile="Categoria_Cliente.aspx.cs" Inherits="Categoria_Cliente" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="row">
        <div class="col-lg-12 col-sm-12 text-center alert-info">
            <h3>
                <i class="fa fa-users"></i>&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label1" runat="server" Text="Categoria Cliente"></asp:Label>
            </h3>
        </div>
    </div>
    <div class="row pad1m">
        <div class="col-lg-12 col-sm-12 text-center">
            <asp:Label ID="lblError" runat="server" CssClass="errores alert-danger" />
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" CssClass="errores alert-danger" ValidationGroup="agrega" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" CssClass="errores alert-danger" ValidationGroup="editar" />
        </div>
    </div>
         <asp:Panel ID="pnlCatalogos" runat="server" CssClass="col-lg-12 col-sm-12" ScrollBars="Auto">  
             <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table class="centrado">
                        <tr class="alineado">
                            <td >
                                <asp:TextBox ID="txtDesc" runat="server" MaxLength="50" CssClass="ancho150px" />
                                <cc1:TextBoxWatermarkExtender ID="txtDesc_TextBoxWatermarkExtender" 
                                    runat="server" BehaviorID="txtDesc_TextBoxWatermarkExtender" 
                                    TargetControlID="txtDesc" WatermarkCssClass="ancho150px water" WatermarkText="Descripción" />
                                <asp:RequiredFieldValidator ControlToValidate="txtDesc" ID="RequiredFieldValidator1" runat="server" ErrorMessage="Necesita colocar una descripción" ValidationGroup="agrega" Text="*" CssClass="errores alert-danger" />
                            </td>
                            <td >
                                <asp:TextBox ID="txtDesMo" runat="server" MaxLength="8" CssClass="ancho150px" />
                                <cc1:TextBoxWatermarkExtender ID="txtDesMo_TextBoxWatermarkExtender" 
                                    runat="server" BehaviorID="txtDesMo_TextBoxWatermarkExtender" 
                                    TargetControlID="txtDesMo" WatermarkCssClass="ancho150px water" WatermarkText="Desc Mo" />
                                <cc1:FilteredTextBoxExtender runat="server" BehaviorID="txtDesMo_FilteredTextBoxExtender" TargetControlID="txtDesMo" ID="txtDesMo_FilteredTextBoxExtender" FilterType="Numbers,Custom" ValidChars="." />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Necesita llenar el Desc Mo." Text="*" CssClass="errores alert-danger" ValidationGroup="agrega" ControlToValidate="txtDesMo" />
                                <asp:RegularExpressionValidator ControlToValidate="txtDesMo" ID="RegularExpressionValidator1" runat="server" ErrorMessage="El descuento de mano de obra no puede ser mayor de 100.00 y/o menor de 0.00" Text="*" CssClass="errores alert-danger" ValidationExpression="(?!^0*$)(?!^0*\.0*$)^\d{1,2}(\.\d{1,2})|(100|100\.0|100\.00)?$" ValidationGroup="agrega" />
                            </td>
                            <td >
                                <asp:TextBox ID="txtDesRef" runat="server" MaxLength="8" CssClass="ancho150px" />
                                <cc1:TextBoxWatermarkExtender ID="txtDesRef_TextBoxWatermarkExtender" 
                                    runat="server" BehaviorID="txtDesRef_TextBoxWatermarkExtender" 
                                    TargetControlID="txtDesRef" WatermarkCssClass="ancho150px water" WatermarkText="Desc Ref" />
                                <cc1:FilteredTextBoxExtender runat="server" BehaviorID="txtDesRef_FilteredTextBoxExtender" TargetControlID="txtDesRef" ID="FilteredTextBoxExtender1" FilterType="Numbers,Custom" ValidChars="." />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Necesita llenar el Desc Ref." Text="*" CssClass="errores alert-danger" ValidationGroup="agrega" ControlToValidate="txtDesRef" />
                                <asp:RegularExpressionValidator ControlToValidate="txtDesRef" ID="RegularExpressionValidator3" runat="server" ErrorMessage="El descuento de refacción no puede ser mayor de 100.00 y/o menor de 0.00" Text="*" CssClass="errores alert-danger" ValidationExpression="(?!^0*$)(?!^0*\.0*$)^\d{1,2}(\.\d{1,2})|(100|100\.0|100\.00)?$" ValidationGroup="agrega" />
                            </td>
                            <td >
                                <asp:TextBox ID="txtPrefijo" runat="server" MaxLength="2" CssClass="ancho150px" />
                                <cc1:TextBoxWatermarkExtender ID="txtPrefijo_TextBoxWatermarkExtender" 
                                    runat="server" BehaviorID="txtPrefijo_TextBoxWatermarkExtender" 
                                    TargetControlID="txtPrefijo" WatermarkCssClass="ancho150px water" WatermarkText="Prefijo" />
                            </td>
                            <td >
                                <asp:LinkButton ID="btnNuevo" runat="server" CssClass="btn btn-info t14" OnClick="btnNuevo_Click" ToolTip="Agregar" ValidationGroup="agrega"><i class="fa fa-plus-circle"></i>&nbsp;<span>Agregar</span></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                    <asp:GridView ID="GridCatClientes" runat="server" AutoGenerateColumns="False" OnRowDeleting="GridCatClientes_RowDeleting"
                        CssClass="table table-bordered" AllowPaging="True" PageSize="10" AllowSorting="True" 
                        EmptyDataText="No existe catgorias de cliente para mostrar" OnRowDataBound="GridCatClientes_RowDataBound"
                        DataKeyNames="id_cat_cliente" DataSourceID="SqlDataSource1" OnRowCommand="GridCatClientes_RowCommand" >
                        <Columns>                            
                            <asp:BoundField DataField="id_cat_cliente" HeaderText="id_cat_cliente" ReadOnly="True" SortExpression="id_cat_cliente" Visible="false" />
                            <asp:TemplateField HeaderText="Descripción" SortExpression="descripcion">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtDescripcionEdit" runat="server" Text='<%# Bind("descripcion") %>' CssClass="ancho150px" MaxLength="100" />
                                    <cc1:TextBoxWatermarkExtender ID="txtDescripcionEdit_TextBoxWatermarkExtender" 
                                        runat="server" BehaviorID="txtDescripcionEdit_TextBoxWatermarkExtender" 
                                        TargetControlID="txtDescripcionEdit" WatermarkCssClass="ancho150px water" WatermarkText="Descripcion" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lbldescripcion" runat="server" Text='<%# Bind("descripcion") %>' /></ItemTemplate>
                                <ItemStyle CssClass="ancho150px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Desc Mo" SortExpression="desc_mo">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtDescMoEdit" runat="server" Text='<%# Bind("desc_mo") %>' CssClass="ancho150px" MaxLength="6" />
                                    <cc1:TextBoxWatermarkExtender ID="txtDescMoEdit_TextBoxWatermarkExtender" 
                                        runat="server" BehaviorID="txtDescMoEdit_TextBoxWatermarkExtender" 
                                        TargetControlID="txtDescMoEdit" WatermarkCssClass="ancho150px water" WatermarkText="Desc Mo" />
                                    <cc1:FilteredTextBoxExtender runat="server" BehaviorID="txtDescMoEdit_FilteredTextBoxExtender" TargetControlID="txtDescMoEdit" ID="txtDesMo_FilteredTextBoxExtender" FilterType="Numbers,Custom" ValidChars="." />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Necesita llenar el Desc Mo." Text="*" CssClass="errores alert-danger" ValidationGroup="editar" ControlToValidate="txtDescMoEdit" />
                                    <asp:RegularExpressionValidator ControlToValidate="txtDescMoEdit" ID="RegularExpressionValidator101" runat="server" ErrorMessage="El descuento de mano de obra no puede ser mayor de 100.00 y/o menor de 0.00" Text="*" CssClass="errores alert-danger" ValidationExpression="(?!^0*$)(?!^0*\.0*$)^\d{1,2}(\.\d{1,2})|(100|100\.0|100\.00)?$" ValidationGroup="editar" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lbldesc_mo" runat="server" Text='<%# Bind("desc_mo") %>' /></ItemTemplate>
                                <ItemStyle CssClass="ancho180px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Desc Ref" SortExpression="desc_ref">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtDescRefEdit" runat="server" Text='<%# Bind("desc_ref") %>' CssClass="ancho150px" MaxLength="6" />
                                    <cc1:TextBoxWatermarkExtender ID="txtDescRefEdit_TextBoxWatermarkExtender" 
                                        runat="server" BehaviorID="txtDescRefEdit_TextBoxWatermarkExtender" 
                                        TargetControlID="txtDescRefEdit" WatermarkCssClass="ancho150px water" WatermarkText="Desc Ref" />
                                    <cc1:FilteredTextBoxExtender runat="server" BehaviorID="txtDescRefEdit_FilteredTextBoxExtender" TargetControlID="txtDescRefEdit" ID="FilteredTextBoxExtender1" FilterType="Numbers,Custom" ValidChars="." />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator202" runat="server" ErrorMessage="Necesita llenar el Desc Ref." Text="*" CssClass="errores alert-danger" ValidationGroup="editar" ControlToValidate="txtDescRefEdit" />
                                    <asp:RegularExpressionValidator ControlToValidate="txtDescRefEdit" ID="RegularExpressionValidator3" runat="server" ErrorMessage="El descuento de refacción no puede ser mayor de 100.00 y/o menor de 0.00" Text="*" CssClass="errores alert-danger" ValidationExpression="(?!^0*$)(?!^0*\.0*$)^\d{1,2}(\.\d{1,2})|(100|100\.0|100\.00)?$" ValidationGroup="editar" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lbldesc_ref" runat="server" Text='<%# Bind("desc_ref") %>' /></ItemTemplate>
                                <ItemStyle CssClass="ancho180px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Prefijo" SortExpression="prefijo">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtPrefijoEdit" runat="server" Text='<%# Bind("prefijo") %>' CssClass="ancho150px" MaxLength="2" />
                                    <cc1:TextBoxWatermarkExtender ID="txtPrefijoEdit_TextBoxWatermarkExtender" 
                                        runat="server" BehaviorID="txtPrefijoEdit_TextBoxWatermarkExtender" 
                                        TargetControlID="txtPrefijoEdit" WatermarkCssClass="ancho150px water" WatermarkText="Prefijo" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblprefijo" runat="server" Text='<%# Bind("prefijo") %>' /></ItemTemplate>
                                <ItemStyle CssClass="ancho150px" />
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                <EditItemTemplate>
                                    <asp:LinkButton ID="btnActualizar" runat="server" CausesValidation="True" CommandName="Update" ToolTip="Guardar" ValidationGroup="edita" CssClass="btn btn-success t14"><i class="fa fa-save"></i></asp:LinkButton>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEditar" runat="server" CausesValidation="False" CommandName="Edit" ToolTip="Editar" CssClass="btn btn-warning t14"><i class="fa fa-edit"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                <EditItemTemplate>
                                    <asp:LinkButton ID="btnCancelar" runat="server" CausesValidation="False" CommandName="Cancel" ToolTip="Cancelar" CssClass="btn btn-danger t14"><i class="fa fa-times-circle"></i></asp:LinkButton>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEliminar" runat="server" CausesValidation="False" CommandName="Delete" ToolTip="Eliminar" CssClass="btn btn-danger t14" OnClientClick="return confirm('¿Esta seguro de eliminar el registro?');"><i class="fa fa-trash"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EditRowStyle CssClass="alert-warning" />
                        <EmptyDataRowStyle CssClass="errores alert-danger" />
                        <SelectedRowStyle CssClass="alert-success" />
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" 
                        DeleteCommand="DELETE FROM [Categoria_Cliente] WHERE[id_cat_cliente]=@id_cat_cliente" 
                        InsertCommand="insert into Categoria_Cliente (id_cat_cliente,descripcion,desc_mo,desc_ref,prefijo)
                                        values(
                                        (select isnull(count(cc.id_cat_cliente)+1,0) from  Categoria_Cliente cc),
                                        @descripcion,@desc_mo,@desc_ref,@prefijo)" 
                        SelectCommand="SELECT [id_cat_cliente], [descripcion], [desc_mo], [desc_ref], [prefijo] FROM [Categoria_Cliente]" 
                        UpdateCommand="update Categoria_Cliente set descripcion=@descripcion,desc_mo=@desc_mo,desc_ref=@desc_ref,prefijo=@prefijo where id_cat_cliente=@id_cat_cliente">
                        <DeleteParameters>
                            <asp:Parameter Name="id_cat_cliente" />
                        </DeleteParameters>
                        <InsertParameters>
                            <asp:ControlParameter ControlID="txtDesc" Name="descripcion" Type="String" PropertyName="Text" />
                            <asp:ControlParameter ControlID="txtDesMo" Name="desc_mo" Type="Decimal" PropertyName="Text" />
                            <asp:ControlParameter ControlID="txtDesRef" Name="desc_ref" Type="Decimal" PropertyName="Text" />
                            <asp:ControlParameter ControlID="txtPrefijo" Name="prefijo" Type="String" PropertyName="Text" />
                        </InsertParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="descripcion" />
                            <asp:Parameter Name="desc_mo" />
                            <asp:Parameter Name="desc_ref" />
                            <asp:Parameter Name="id_cat_cliente" />
                            <asp:Parameter Name="prefijo" />
                        </UpdateParameters>
                    </asp:SqlDataSource>
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
        </asp:Panel>
</asp:Content>