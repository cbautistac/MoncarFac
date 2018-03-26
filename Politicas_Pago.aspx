<%@ Page Title="" Language="C#" MasterPageFile="~/Administracion.master" AutoEventWireup="true" CodeFile="Politicas_Pago.aspx.cs" Inherits="Politicas_Pago" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="row">
        <div class="col-lg-12 col-sm-12 text-center alert-info">
            <h3>
                <i class="fa fa-legal"></i>&nbsp;
                <i class="fa fa-dollar"></i>&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="Label1" runat="server" Text="Politicas Pago"></asp:Label>
            </h3>
        </div>
    </div>
    <div class="row pad1m">
        <div class="col-lg-12 col-sm-12 text-center">
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="agrega" CssClass="errores alert-danger" DisplayMode="List" />
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="edita" CssClass="errores alert-danger" DisplayMode="List" />
            <asp:Label ID="lblError" runat="server" CssClass="errores alert-danger"></asp:Label>
        </div>
    </div>
    <asp:Panel ID="pnlCatalogos" runat="server" CssClass="col-lg-12 col-sm-12" ScrollBars="Auto">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table class="centrado">
                    <tr class="alineado">
                        <td>
                            <asp:TextBox ID="txtClvPolitica" runat="server" MaxLength="20" CssClass="input-medium" />
                            <cc1:TextBoxWatermarkExtender ID="txtClvPolitica_TextBoxWatermarkExtender"
                                runat="server" BehaviorID="txtClvPolitica_TextBoxWatermarkExtender"
                                TargetControlID="txtClvPolitica" WatermarkCssClass="ancho150px water" WatermarkText="Clv Politica" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtDescripcion" runat="server" MaxLength="50" CssClass="input-medium" />
                            <cc1:TextBoxWatermarkExtender ID="txtDescripcion_TextBoxWatermarkExtender"
                                runat="server" BehaviorID="txtDescripcion_TextBoxWatermarkExtender"
                                TargetControlID="txtDescripcion" WatermarkCssClass="ancho150px water" WatermarkText="Descripción" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtDiasPlazo" runat="server" MaxLength="5" CssClass="input-medium" />
                            <cc1:TextBoxWatermarkExtender ID="txtDiasPlazo_TextBoxWatermarkExtender"
                                runat="server" BehaviorID="txtDiasPlazo_TextBoxWatermarkExtender"
                                TargetControlID="txtDiasPlazo" WatermarkCssClass="ancho150px water" WatermarkText="Días Plazo" />
                            <cc1:FilteredTextBoxExtender runat="server" BehaviorID="txtDiasPlazo_FilteredTextBoxExtender" TargetControlID="txtDiasPlazo" ID="txtDiasPlazo_FilteredTextBoxExtender" FilterType="Numbers,Custom" />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="txtDiasPlazo" Text="*" CssClass="errores alert-danger" runat="server" ValidationGroup="agrega" ErrorMessage="Solo puede introducir numeros enteros en el plazo 000" ValidationExpression="\d*[0-9]{1,5}" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtDescripcionDescuento" runat="server" MaxLength="100" CssClass="input-medium" />
                            <cc1:TextBoxWatermarkExtender ID="txtDescripcionDescuento_TextBoxWatermarkExtender"
                                runat="server" BehaviorID="txtDescripcionDescuento_TextBoxWatermarkExtender"
                                TargetControlID="txtDescripcionDescuento" WatermarkCssClass="ancho150px water" WatermarkText="Descripción Descuento" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtDescuento" runat="server" MaxLength="8" CssClass="input-medium" />
                            <cc1:TextBoxWatermarkExtender ID="txtDescuento_TextBoxWatermarkExtender"
                                runat="server" BehaviorID="txtDescuento_TextBoxWatermarkExtender"
                                TargetControlID="txtDescuento" WatermarkCssClass="ancho150px water" WatermarkText="% Descuento" />
                            <asp:RegularExpressionValidator ControlToValidate="txtDescuento" ID="RegularExpressionValidator1" runat="server" ErrorMessage="El descuento no puede ser mayor de 100.00 y/o menor de 0.00" Text="*" CssClass="errores alert-danger" ValidationExpression="(?!^0*$)(?!^0*\.0*$)^\d{1,2}(\.\d{1,2})|(100|100\.0|100\.00)?$" ValidationGroup="agrega" />
                            <cc1:FilteredTextBoxExtender runat="server" BehaviorID="txtDescuento_FilteredTextBoxExtender" TargetControlID="txtDescuento" ID="txtDescuento_FilteredTextBoxExtender" FilterType="Numbers,Custom" ValidChars="." />
                        </td>
                        <td>
                            <asp:LinkButton ID="btnNuevo" runat="server" CssClass="btn btn-info t14" OnClick="btnNuevo_Click" ToolTip="Agregar" ValidationGroup="agrega"><i class="fa fa-plus-circle"></i>&nbsp;<span>Agregar</span></asp:LinkButton>
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="GridPoliticasPag" runat="server" AutoGenerateColumns="False" OnRowDataBound="GridPoliticasPag_RowDataBound"
                    CssClass="table table-bordered" AllowPaging="True" PageSize="10" AllowSorting="True"
                    EmptyDataText="No existe politicas para mostrar" DataKeyNames="id_politica" DataSourceID="SqlDataSource1"
                    OnRowDeleting="GridPoliticasPag_RowDeleting">
                    <Columns>
                        <asp:BoundField DataField="id_politica" HeaderText="id_politica" ReadOnly="True" SortExpression="id_politica" Visible="false" />
                        <asp:TemplateField HeaderText="Clv Politica">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtClvPoliticaEdit" runat="server" Text='<%# Bind("clv_politica") %>' MaxLength="20" CssClass="input-medium" />
                                <cc1:TextBoxWatermarkExtender ID="txtClvPoliticaEdit_TextBoxWatermarkExtender"
                                    runat="server" BehaviorID="txtClvPoliticaEdit_TextBoxWatermarkExtender"
                                    TargetControlID="txtClvPoliticaEdit" WatermarkCssClass="ancho150px water" WatermarkText="Clv Politica" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblclv_politica" runat="server" Text='<%# Bind("clv_politica") %>' />
                            </ItemTemplate>
                            <ItemStyle />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Descripción">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtDescripcionEdit" runat="server" Text='<%# Bind("descripcion") %>' MaxLength="50" CssClass="input-medium" />
                                <cc1:TextBoxWatermarkExtender ID="txtDescripcionEdit_TextBoxWatermarkExtender"
                                    runat="server" BehaviorID="txtDescripcionEdit_TextBoxWatermarkExtender"
                                    TargetControlID="txtDescripcionEdit" WatermarkCssClass="ancho150px water" WatermarkText="Descripción" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lbldescripcion" runat="server" Text='<%# Bind("descripcion") %>' />
                            </ItemTemplate>
                            <ItemStyle />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Dias Plazo">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtDiasPlazoEdit" runat="server" Text='<%# Bind("dias_plazo") %>' MaxLength="5" CssClass="input-medium" />
                                <cc1:TextBoxWatermarkExtender ID="txtDiasPlazoEdit_TextBoxWatermarkExtender"
                                    runat="server" BehaviorID="txtDiasPlazoEdit_TextBoxWatermarkExtender"
                                    TargetControlID="txtDiasPlazoEdit" WatermarkCssClass="ancho150px water" WatermarkText="Días Plazo" />
                                <cc1:FilteredTextBoxExtender runat="server" BehaviorID="txtDiasPlazoEdit_FilteredTextBoxExtender" TargetControlID="txtDiasPlazoEdit" ID="txtDiasPlazoEdit_FilteredTextBoxExtender" FilterType="Numbers,Custom" ValidChars="." />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="txtDiasPlazoEdit" Text="*" CssClass="errores alert-danger" runat="server" ValidationGroup="edita" ErrorMessage="Solo puede numeros enterons en el plazo 000" ValidationExpression="\d*[0-9]{1,5}" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lbldias_plazo" runat="server" Text='<%# Bind("dias_plazo") %>' />
                            </ItemTemplate>
                            <ItemStyle />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Descripción Descuento">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtDescripcionDescuento" runat="server" Text='<%# Bind("descripcion_descuento") %>' MaxLength="100" CssClass="input-medium" />
                                <cc1:TextBoxWatermarkExtender ID="txtDescripcionDescuento_TextBoxWatermarkExtender"
                                    runat="server" BehaviorID="txtDescripcionDescuento_TextBoxWatermarkExtender"
                                    TargetControlID="txtDescripcionDescuento" WatermarkCssClass="ancho150px water" WatermarkText="Descripción Descuento" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lbldescripcion_descuento" runat="server" Text='<%# Bind("descripcion_descuento") %>' />
                            </ItemTemplate>
                            <ItemStyle />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="% Descuento">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtDescuentoEdit" runat="server" Text='<%# Bind("descuento") %>' MaxLength="8" CssClass="input-medium" />
                                <cc1:TextBoxWatermarkExtender ID="txtDescuentoEdit_TextBoxWatermarkExtender"
                                    runat="server" BehaviorID="txtDescuentoEdit_TextBoxWatermarkExtender"
                                    TargetControlID="txtDescuentoEdit" WatermarkCssClass="ancho150px water" WatermarkText="% Descuento" />
                                <asp:RegularExpressionValidator ControlToValidate="txtDescuentoEdit" ID="RegularExpressionValidator111" runat="server" ErrorMessage="El descuento no puede ser mayor de 100.00 y/o menor de 0.00" Text="*" CssClass="errores alert-danger" ValidationExpression="(?!^0*$)(?!^0*\.0*$)^\d{1,2}(\.\d{1,2})|(100|100\.0|100\.00)?$" ValidationGroup="edita" />
                                <cc1:FilteredTextBoxExtender runat="server" BehaviorID="txtDescuentoEdit_FilteredTextBoxExtender" TargetControlID="txtDescuentoEdit" ID="txtDescuentoEdit_FilteredTextBoxExtender" FilterType="Numbers,Custom" ValidChars="." />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lbldescuento" runat="server" Text='<%# Bind("descuento") %>' />
                            </ItemTemplate>
                            <ItemStyle />
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
                    DeleteCommand="delete from Politica_Pago where id_politica=@id_politica"
                    InsertCommand="insert into Politica_Pago values((select isnull((select top 1 pp.id_politica from Politica_Pago pp order by pp.id_politica desc),0))+1,@clv_politica,@descripcion,@dias_plazo,@descripcion_descuento,@descuento)"
                    SelectCommand="SELECT [id_politica], [clv_politica], [descripcion], [dias_plazo], [descripcion_descuento], [descuento] FROM [Politica_Pago]"
                    UpdateCommand="update Politica_Pago set clv_politica=@clv_politica, descripcion=@descripcion, dias_plazo=@dias_plazo, descripcion_descuento=@descripcion_descuento, descuento=@descuento where id_politica=@id_politica">
                    <DeleteParameters>
                        <asp:Parameter Name="id_politica" />
                    </DeleteParameters>
                    <InsertParameters>
                        <asp:ControlParameter ControlID="txtClvPolitica" Name="clv_politica" Type="String" />
                        <asp:ControlParameter ControlID="txtDescripcion" Name="descripcion" Type="String" />
                        <asp:ControlParameter ControlID="txtDiasPlazo" Name="dias_plazo" Type="Int32" />
                        <asp:ControlParameter ControlID="txtDescripcionDescuento" Name="descripcion_descuento" Type="String" />
                        <asp:ControlParameter ControlID="txtDescuento" Name="descuento" Type="Decimal" />
                    </InsertParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="clv_politica" />
                        <asp:Parameter Name="descripcion" />
                        <asp:Parameter Name="dias_plazo" />
                        <asp:Parameter Name="descripcion_descuento" />
                        <asp:Parameter Name="descuento" />
                        <asp:Parameter Name="id_politica" />
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
