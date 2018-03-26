<%@ Page Title="" Language="C#" MasterPageFile="~/Administracion.master" AutoEventWireup="true" CodeFile="Marcas.aspx.cs" Inherits="Marcas" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="row">
        <div class="col-lg-12 col-sm-12 text-center alert-info">
            <h3>
                <i class="fa fa-shield"></i>&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="Label1" runat="server" Text="Marcas"></asp:Label>
            </h3>
        </div>
    </div>
    <div class="row pad1m">
        <div class="col-lg-12 col-sm-12 text-center">
            <asp:ValidationSummary ID="ValidationSummary3" runat="server" ValidationGroup="agrega" CssClass="errores alert-danger" DisplayMode="List" />
            <asp:ValidationSummary ID="ValidationSummary4" runat="server" ValidationGroup="edita" CssClass="errores alert-danger" DisplayMode="List" />
            <asp:Label ID="Label2" runat="server" CssClass="errores alert-danger"></asp:Label>
        </div>
    </div>
    <asp:Panel ID="pnlCatalogos" runat="server" CssClass="col-lg-12 col-sm-12" ScrollBars="Auto">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table class="centrado">
                    <tr class="alineado">
                        <td>
                            <asp:TextBox ID="txtDescripcionNew" runat="server" MaxLength="50" Width="180px" />
                            <cc1:TextBoxWatermarkExtender ID="txtDescripcionNew_TextBoxWatermarkExtender"
                                runat="server" BehaviorID="txtDescripcionNew_TextBoxWatermarkExtender"
                                TargetControlID="txtDescripcionNew" WatermarkCssClass="ancho180px water" WatermarkText="Descripción" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Debe indicar la Descripción" CssClass="errores alineado" ValidationGroup="agrega" ControlToValidate="txtDescripcionNew" Text="*" />
                        </td>
                        <td>
                            <asp:LinkButton ID="btnAceptarNew" runat="server" CssClass="btn btn-info t14" OnClick="btnAceptarNew_Click" ToolTip="Agregar" ValidationGroup="agrega"><i class="fa fa-plus-circle"></i>&nbsp;<span>Agregar</span></asp:LinkButton>
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="grvMarcas" runat="server" AutoGenerateColumns="False"
                    CssClass="table table-bordered" AllowPaging="True" PageSize="10" AllowSorting="True"
                    EmptyDataText="No existe Información para mostrar" DataKeyNames="id_marca"
                    DataSourceID="SqlDataSource1" OnRowCommand="grvMarcas_RowCommand" OnRowDataBound="grvMarcas_RowDataBound"
                    OnRowDeleting="grvMarcas_RowDeleting">
                    <Columns>
                        <asp:BoundField DataField="id_marca" HeaderText="id_marca" ReadOnly="True" SortExpression="id_marca" Visible="false" />
                        <asp:TemplateField HeaderText="Descripción" SortExpression="descripcion">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtDecripcion" runat="server" Text='<%# Bind("descripcion") %>' Width="180px" MaxLength="50" CssClass="ancho70px alineado" />
                                <cc1:TextBoxWatermarkExtender ID="txtDecripcion_TextBoxWatermarkExtender"
                                    runat="server" BehaviorID="txtDecripcion_TextBoxWatermarkExtender"
                                    TargetControlID="txtDecripcion" WatermarkCssClass="ancho180px water" WatermarkText="Descripción" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Debe indicar la Descripción" CssClass="errores alineado" ValidationGroup="edita" ControlToValidate="txtDecripcion" Text="*" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("descripcion") %>' />
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
                    DeleteCommand="DELETE FROM [marcas] WHERE [id_marca] = @id_marca"
                    InsertCommand="insert into marcas values(isnull((select top 1 id_marca from marcas order by id_marca desc),0)+1, @descripcion)"
                    SelectCommand="SELECT [id_marca], [descripcion] FROM [marcas]"
                    UpdateCommand="UPDATE [marcas] SET [descripcion] = @descripcion WHERE [id_marca] = @id_marca">
                    <DeleteParameters>
                        <asp:Parameter Name="id_marca" Type="Int32" />
                    </DeleteParameters>
                    <InsertParameters>
                        <asp:ControlParameter ControlID="txtDescripcionNew" Name="descripcion" Type="String" />
                    </InsertParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="id_marca" Type="Int32" />
                        <asp:Parameter Name="descripcion" Type="String" />
                    </UpdateParameters>
                </asp:SqlDataSource>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="agrega" CssClass="errores alert-danger" DisplayMode="List" />
                <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="edita" CssClass="errores alert-danger" DisplayMode="List" />
                <asp:Label ID="lblError" runat="server" CssClass="errores alert-danger"></asp:Label>
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
