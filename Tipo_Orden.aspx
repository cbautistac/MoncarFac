<%@ Page Title="" Language="C#" MasterPageFile="~/Administracion.master" AutoEventWireup="true" CodeFile="Tipo_Orden.aspx.cs" Inherits="Tipo_Orden" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="row">
        <div class="col-lg-12 col-sm-12 text-center alert-info">
            <h3>
                <i class="fa fa-list-ul"></i>&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="Label1" runat="server" Text="Tipo Orden"></asp:Label>
            </h3>
        </div>
    </div>
    <div class="row pad1m">
        <div class="col-lg-12 col-sm-12 text-center">
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" CssClass="errores alert-danger" DisplayMode="List" ValidationGroup="edita" />
            <asp:ValidationSummary ID="ValidationSummary5" runat="server" CssClass="errores alert-danger" DisplayMode="List" ValidationGroup="agrega" />
            <asp:Label ID="lblError" runat="server" CssClass="errores alert-danger" />
            <asp:Label ID="lblErrorNew" runat="server" CssClass="errores alert-danger" />
        </div>
    </div>
    <asp:Panel ID="pnlCatalogos" runat="server" CssClass="col-lg-12 col-sm-12" ScrollBars="Auto">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table class="centrado">
                    <tr class="alineado">
                        <td>
                            <asp:TextBox ID="txtDescripcionNew" runat="server" CssClass="ancho150px alineado" MaxLength="50" />
                            <cc1:TextBoxWatermarkExtender ID="txtDescripcionNew_TextBoxWatermarkExtender"
                                runat="server" BehaviorID="txtDescripcionNew_TextBoxWatermarkExtender"
                                TargetControlID="txtDescripcionNew" WatermarkCssClass="ancho150px water" WatermarkText="Descripción" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Necesita llenar la Descripcion." Text="*" CssClass="errores alert-danger" ValidationGroup="agrega" ControlToValidate="txtDescripcionNew" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtImporteHrsNew" runat="server" CssClass="ancho150px alineado" MaxLength="10" />
                            <cc1:FilteredTextBoxExtender runat="server" BehaviorID="txtImporteHrsNew_FilteredTextBoxExtender" TargetControlID="txtImporteHrsNew" ID="txtImporteHrsNew_FilteredTextBoxExtender" FilterType="Numbers,Custom" ValidChars="." />
                            <cc1:TextBoxWatermarkExtender ID="txtImporteHrsNew_TextBoxWatermarkExtender"
                                runat="server" BehaviorID="txtImporteHrsNew_TextBoxWatermarkExtender"
                                TargetControlID="txtImporteHrsNew" WatermarkCssClass="ancho150px water" WatermarkText="Importe Hrs" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Necesita llenar el Importe de Hrs." Text="*" CssClass="errores alert-danger" ValidationGroup="agrega" ControlToValidate="txtImporteHrsNew" />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtImporteHrsNew" Text="*" CssClass="errores alert-danger" runat="server" ValidationGroup="agrega" ErrorMessage="Solo puede introducir una cantidad con dos decimales en el importe por hrs 0000000.00" ValidationExpression="\d*[0-9]{1,7}\.?\d[0-9]{0,2}|\d*[0-9]{1,7}" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtImporteHojaNew" runat="server" CssClass="ancho150px alineado" MaxLength="10" />
                            <cc1:FilteredTextBoxExtender runat="server" BehaviorID="txtImporteHojaNew_FilteredTextBoxExtender" TargetControlID="txtImporteHojaNew" ID="txtImporteHojaNew_FilteredTextBoxExtender" FilterType="Numbers,Custom" ValidChars="." />
                            <cc1:TextBoxWatermarkExtender ID="txtImporteHojaNew_TextBoxWatermarkExtender"
                                runat="server" BehaviorID="txtImporteHojaNew_TextBoxWatermarkExtender"
                                TargetControlID="txtImporteHojaNew" WatermarkCssClass="ancho150px water" WatermarkText="Importe Hojalateria" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Necesita llenar el Importe de Hojalateria." Text="*" CssClass="errores alert-danger" ValidationGroup="agrega" ControlToValidate="txtImporteHojaNew" />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="txtImporteHojaNew" Text="*" CssClass="errores alert-danger" runat="server" ValidationGroup="agrega" ErrorMessage="Solo puede introducir una cantidad con dos decimales en el importe hojalateria 0000000.00" ValidationExpression="\d*[0-9]{1,7}\.?\d[0-9]{0,2}|\d*[0-9]{1,7}" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtImportePintNew" runat="server" CssClass="ancho150px alineado" MaxLength="10" />
                            <cc1:FilteredTextBoxExtender runat="server" BehaviorID="txtImportePintNew_FilteredTextBoxExtender" TargetControlID="txtImportePintNew" ID="txtImportePintNew_FilteredTextBoxExtender" FilterType="Numbers,Custom" ValidChars="." />
                            <cc1:TextBoxWatermarkExtender ID="txtImportePintNew_TextBoxWatermarkExtender"
                                runat="server" BehaviorID="txtImportePintNew_TextBoxWatermarkExtender"
                                TargetControlID="txtImportePintNew" WatermarkCssClass="ancho150px water" WatermarkText="Importe Pintura" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Necesita llenar el Importe de Pintura." Text="*" CssClass="errores alert-danger" ValidationGroup="agrega" ControlToValidate="txtImportePintNew" />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" ControlToValidate="txtImportePintNew" Text="*" CssClass="errores alert-danger" runat="server" ValidationGroup="agrega" ErrorMessage="Solo puede introducir una cantidad con dos decimales en el importe pintura 0000000.00" ValidationExpression="\d*[0-9]{1,7}\.?\d[0-9]{0,2}|\d*[0-9]{1,7}" />
                        </td>
                        <td>
                            <asp:LinkButton ID="btnAgregar" runat="server" CssClass="btn btn-info t14" OnClick="btnAgregar_Click" ToolTip="Agregar" ValidationGroup="agrega"><i class="fa fa-plus-circle"></i>&nbsp;<span>Agregar</span></asp:LinkButton>
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" AllowSorting="True"
                    CssClass="table table-bordered" AllowPaging="True" PageSize="10"
                    EmptyDataText="No existe Información para mostrar" DataKeyNames="id_tipo_orden"
                    DataSourceID="SqlDataSource1" OnRowCommand="GridView1_RowCommand" OnRowDataBound="GridView1_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="id_tipo_orden" HeaderText="id_tipo_orden" ReadOnly="True" SortExpression="id_tipo_orden" Visible="false" />
                        <asp:TemplateField HeaderText="Descripción" SortExpression="descripcion">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtDescripcionMod" runat="server" Text='<%# Bind("descripcion") %>' CssClass="ancho150px alineado" MaxLength="50" />
                                <cc1:TextBoxWatermarkExtender ID="txtDescripcionMod_TextBoxWatermarkExtender"
                                    runat="server" BehaviorID="txtDescripcionMod_TextBoxWatermarkExtender"
                                    TargetControlID="txtDescripcionMod" WatermarkCssClass="ancho150px water" WatermarkText="Descripción" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="Necesita llenar la Descripcion." Text="*" CssClass="errores alert-danger" ValidationGroup="edita" ControlToValidate="txtDescripcionMod" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("descripcion") %>' />
                            </ItemTemplate>
                            <ItemStyle CssClass="alineado ancho180px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Importe hrs" SortExpression="importe_hrs">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtImporteHrs" runat="server" Text='<%# Bind("importe_hrs","{0:N}") %>' CssClass="ancho150px alineado" MaxLength="10" />
                                <cc1:FilteredTextBoxExtender runat="server" BehaviorID="txtImporteHrs_FilteredTextBoxExtender" TargetControlID="txtImporteHrs" ID="txtImporteHrs_FilteredTextBoxExtender" FilterType="Numbers,Custom" ValidChars="." />
                                <cc1:TextBoxWatermarkExtender ID="txtImporteHrs_TextBoxWatermarkExtender"
                                    runat="server" BehaviorID="txtImporteHrs_TextBoxWatermarkExtender"
                                    TargetControlID="txtImporteHrs" WatermarkCssClass="ancho150px water" WatermarkText="Importe Hrs" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="Necesita llenar el Importe de Hrs." Text="*" CssClass="errores alert-danger" ValidationGroup="edita" ControlToValidate="txtImporteHrs" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" ControlToValidate="txtImporteHrs" Text="*" CssClass="errores alert-danger" runat="server" ValidationGroup="edita" ErrorMessage="Solo puede introducir una cantidad con dos decimales en el importe por hrs 0000000.00" ValidationExpression="\d*[0-9]{1,7}\.?\d[0-9]{0,2}|\d*[0-9]{1,7}" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("importe_hrs","{0:N}") %>' />
                            </ItemTemplate>
                            <ItemStyle CssClass="ancho180px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Importe Hojalateria" SortExpression="importe_ho">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtImporteHoja" runat="server" Text='<%# Bind("importe_ho","{0:N}") %>' CssClass="alineado ancho150px" MaxLength="10" />
                                <cc1:FilteredTextBoxExtender runat="server" BehaviorID="txtImporteHoja_FilteredTextBoxExtender" TargetControlID="txtImporteHoja" ID="txtImporteHoja_FilteredTextBoxExtender" FilterType="Numbers,Custom" ValidChars="." />
                                <cc1:TextBoxWatermarkExtender ID="txtImporteHoja_TextBoxWatermarkExtender"
                                    runat="server" BehaviorID="txtImporteHoja_TextBoxWatermarkExtender"
                                    TargetControlID="txtImporteHoja" WatermarkCssClass="ancho150px water" WatermarkText="Importe Hojalateria" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="Necesita llenar el Importe de Hojalateria." Text="*" CssClass="errores alert-danger" ValidationGroup="edita" ControlToValidate="txtImporteHoja" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" ControlToValidate="txtImporteHoja" Text="*" CssClass="errores alert-danger" runat="server" ValidationGroup="edita" ErrorMessage="Solo puede introducir una cantidad con dos decimales en el importe hojalateria 0000000.00" ValidationExpression="\d*[0-9]{1,7}\.?\d[0-9]{0,2}|\d*[0-9]{1,7}" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("importe_ho","{0:N}") %>' />
                            </ItemTemplate>
                            <ItemStyle CssClass="ancho180px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Importe Pintura" SortExpression="importe_pi">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtImportePint" runat="server" Text='<%# Bind("importe_pi","{0:N}") %>' CssClass="alineado ancho150px" MaxLength="10" />
                                <cc1:FilteredTextBoxExtender runat="server" BehaviorID="txtImportePint_FilteredTextBoxExtender" TargetControlID="txtImportePint" ID="txtImportePint_FilteredTextBoxExtender" FilterType="Numbers,Custom" ValidChars="." />
                                <cc1:TextBoxWatermarkExtender ID="txtImportePint_TextBoxWatermarkExtender"
                                    runat="server" BehaviorID="txtImportePint_TextBoxWatermarkExtender"
                                    TargetControlID="txtImportePint" WatermarkCssClass="ancho150px water" WatermarkText="Importe Pintura" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="Necesita llenar el Importe de Pintura." Text="*" CssClass="errores alert-danger" ValidationGroup="edita" ControlToValidate="txtImportePint" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" ControlToValidate="txtImportePint" Text="*" CssClass="errores alert-danger" runat="server" ValidationGroup="edita" ErrorMessage="Solo puede introducir una cantidad con dos decimales en el importe pintura 0000000.00" ValidationExpression="\d*[0-9]{1,7}\.?\d[0-9]{0,2}|\d*[0-9]{1,7}" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label4" runat="server" Text='<%# Bind("importe_pi","{0:N}") %>' />
                            </ItemTemplate>
                            <ItemStyle CssClass="ancho180px" />
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
                <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                    ConnectionString="<%$ ConnectionStrings:PVW %>"
                    DeleteCommand="delete from Tipo_Orden where id_tipo_orden=@id_tipo_orden"
                    InsertCommand="insert into Tipo_Orden values((select isnull(count(id_tipo_orden),0) from Tipo_Orden)+1,@descripcion, @importe_hrs, @importe_ho, @importe_pi)"
                    SelectCommand="select  id_tipo_orden, descripcion, importe_hrs, importe_ho, importe_pi from Tipo_Orden"
                    UpdateCommand="update Tipo_Orden set descripcion=@descripcion, importe_hrs=@importe_hrs, importe_ho=@importe_ho, importe_pi=@importe_pi where id_tipo_orden=@id_tipo_orden">
                    <DeleteParameters>
                        <asp:Parameter Name="id_tipo_orden" />
                    </DeleteParameters>
                    <InsertParameters>
                        <asp:Parameter Name="id_tipo_orden" />
                        <asp:ControlParameter ControlID="txtDescripcionNew" DbType="String" Name="descripcion" />
                        <asp:ControlParameter ControlID="txtImporteHrsNew" DbType="Decimal" Name="importe_hrs" />
                        <asp:ControlParameter ControlID="txtImporteHojaNew" DbType="Decimal" Name="importe_ho" />
                        <asp:ControlParameter ControlID="txtImportePintNew" DbType="Decimal" Name="importe_pi" />
                    </InsertParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="descripcion" />
                        <asp:Parameter Name="importe_hrs" />
                        <asp:Parameter Name="importe_ho" />
                        <asp:Parameter Name="importe_pi" />
                        <asp:Parameter Name="id_tipo_orden" />
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
