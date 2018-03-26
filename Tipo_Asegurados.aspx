<%@ Page Title="" Language="C#" MasterPageFile="~/Administracion.master" AutoEventWireup="true" CodeFile="Tipo_Asegurados.aspx.cs" Inherits="Tipo_Asegurados" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="row">
        <div class="col-lg-12 col-sm-12 text-center alert-info">
            <h3>
                <i class="fa fa-archive"></i>&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="Label1" runat="server" Text="Tipo Asegurados"></asp:Label>
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
    <asp:Panel ID="pnlContenido" CssClass="col-lg-12 col-sm-12" runat="server" ScrollBars="Auto">
        <asp:Panel ID="pnlCatalogos" runat="server" CssClass="col-lg-12 col-sm-12" ScrollBars="Auto">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table class="centrado">
                        <tr>
                            <td>
                                <asp:TextBox ID="txtAltaAsegurado" runat="server" MaxLength="50" CssClass="ancho150px alineado"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="txtAltaAsegurado_TextBoxWatermarkExtender"
                                    runat="server" BehaviorID="txtAltaAsegurado_TextBoxWatermarkExtender"
                                    TargetControlID="txtAltaAsegurado" WatermarkCssClass="ancho150px water" WatermarkText="Tipo Asegurado" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Debe indicar el Tipo Asegurado" CssClass="errores alineado" ValidationGroup="agrega" ControlToValidate="txtAltaAsegurado" Text="*"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:LinkButton ID="btnAgregar" runat="server" CssClass="btn btn-info t14" OnClick="btnAgregar_Click" ToolTip="Agregar" ValidationGroup="agrega"><i class="fa fa-plus-circle"></i>&nbsp;<span>Agregar</span></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                    <asp:GridView ID="grvAsegurados" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered" PageSize="10"
                        DataKeyNames="id_tipo_asegurado" DataSourceID="SqlDataSource1" AllowPaging="True" OnRowDeleting="grvAsegurados_RowDeleting"
                        AllowSorting="True" EmptyDataText="No existen Tipos de Asegurados Registrados" OnRowDataBound="grvAsegurados_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="id_tipo_asegurado" HeaderText="id_Asegurado" ReadOnly="True" SortExpression="id_tipo_asegurado" Visible="false" />
                            <asp:TemplateField HeaderText="Tipo Asegurado" SortExpression="descripcion">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtAsegurado" runat="server" Text='<%# Bind("descripcion") %>' MaxLength="50" CssClass="ancho130px alineado"></asp:TextBox>
                                    <cc1:TextBoxWatermarkExtender ID="txtAsegurado_TextBoxWatermarkExtender"
                                        runat="server" BehaviorID="txtAsegurado_TextBoxWatermarkExtender"
                                        TargetControlID="txtAsegurado" WatermarkCssClass="ancho130px water" WatermarkText="Tipo Asegurado" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Debe indicar el Tipo de Asegurado" CssClass="errores alineado" ValidationGroup="edita" ControlToValidate="txtAsegurado" Text="*"></asp:RequiredFieldValidator>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("descripcion") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="ancho130px" />
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
                        SelectCommand="SELECT id_tipo_asegurado, descripcion FROM Tipo_Asegurados"
                        UpdateCommand="UPDATE Tipo_Asegurados SET descripcion=@descripcion WHERE (id_tipo_asegurado = @id_tipo_asegurado)"
                        DeleteCommand="DELETE FROM Tipo_Asegurados where id_tipo_asegurado=@id_tipo_asegurado"
                        InsertCommand="insert into Tipo_Asegurados values(isnull((select top 1 id_tipo_asegurado from Tipo_Asegurados order by id_tipo_asegurado desc),0)+1, @descripcion)">
                        <InsertParameters>
                            <asp:ControlParameter ControlID="txtAltaAsegurado" Type="String" Name="descripcion" />
                        </InsertParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="descripcion" />
                        </UpdateParameters>
                        <DeleteParameters>
                            <asp:Parameter Name="id_tipo_asegurado" />
                        </DeleteParameters>
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
    </asp:Panel>
</asp:Content>


