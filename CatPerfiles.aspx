<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CatPerfiles.aspx.cs" Inherits="CatPerfiles" EnableEventValidation="false" MasterPageFile="~/Administracion.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="row">
        <div class="col-lg-12 col-sm-12 text-center alert-info">
            <h3>
                <i class="fa fa-credit-card"></i>&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="Label1" runat="server" Text="Perfiles"></asp:Label>
            </h3>
        </div>
    </div>
    <div class="row pad1m">
        <div class="col-lg-12 col-sm-12 text-center">
            <asp:ValidationSummary ID="ValidationSummary3" runat="server" ValidationGroup="agrega" CssClass="errores alert-danger" DisplayMode="List" />
            <asp:ValidationSummary ID="ValidationSummary4" runat="server" ValidationGroup="edita" CssClass="errores alert-danger" DisplayMode="List" />
            <asp:Label ID="Label2" runat="server" CssClass="errores alert-danger" />
        </div>
    </div>
    <asp:Panel ID="pnlCatalogos" runat="server" CssClass="panelCatalogos textoCentrado" ScrollBars="Auto">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table class="centrado">
                    <tr class="alineado">
                        <td>
                            <asp:TextBox ID="txtDescripcionNew" runat="server" MaxLength="50" CssClass="ancho180px" />
                            <cc1:TextBoxWatermarkExtender ID="txtDescripcionNew_TextBoxWatermarkExtender"
                                runat="server" BehaviorID="txtDescripcionNew_TextBoxWatermarkExtender"
                                TargetControlID="txtDescripcionNew" WatermarkCssClass="ancho180px water" WatermarkText="Descripción" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Debe indicar la Descripción" CssClass="errores alineado" ValidationGroup="agrega" ControlToValidate="txtDescripcionNew" Text="*" />
                        </td>
                        <td>
                            <asp:LinkButton ID="btnAceptarNew" runat="server" CssClass="btn btn-info t14" OnClick="btnAceptarNew_Click" ToolTip="Agregar" ValidationGroup="agrega"><i class="fa fa-plus-circle"></i>&nbsp;<span>Agregar</span></asp:LinkButton>
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="grvLocalizaciones" runat="server" AutoGenerateColumns="False"
                    CssClass="table table-bordered" AllowPaging="True" GridLines="None" AllowSorting="True"
                    EmptyDataText="No existen Perfiles registrados." DataKeyNames="id_perfilOrden"
                    DataSourceID="SqlDataSource1" OnRowCommand="grvLocalizaciones_RowCommand"
                    OnRowDataBound="grvLocalizaciones_RowDataBound" OnRowDeleting="grvLocalizaciones_RowDeleting">
                    <Columns>
                        <asp:BoundField DataField="id_perfilOrden" HeaderText="id_perfilOrden"
                            ReadOnly="True" SortExpression="id_perfilOrden" Visible="false" />
                        <asp:TemplateField HeaderText="Descripción" SortExpression="descripcion">
                            <ItemTemplate>
                                <asp:Label ID="lblDescripcion" runat="server" Text='<%# Bind("descripcion") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("descripcion") %>' CssClass="ancho150px alineado" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Necesita colocar una descripción" Text="*" CssClass="errores alineado" ControlToValidate="TextBox1" ValidationGroup="edita" />
                                <cc1:TextBoxWatermarkExtender ID="TextBox1_TextBoxWatermarkExtender"
                                    runat="server" BehaviorID="TextBox1_TextBoxWatermarkExtender"
                                    TargetControlID="TextBox1" WatermarkCssClass="ancho150px water" WatermarkText="Descripción" />
                            </EditItemTemplate>
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
                                <asp:LinkButton ID="btnEliminar" runat="server" CausesValidation="False" CommandName="Delete" ToolTip="Eliminar" CssClass="btn btn-danger t14" OnClientClick="return confirm('¿Esta seguro de eliminar el perfil?');"><i class="fa fa-trash"></i></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EditRowStyle CssClass="alert-warning" />
                    <EmptyDataRowStyle CssClass="errores alert-danger" />
                    <SelectedRowStyle CssClass="alert-success" />
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                    ConnectionString="<%$ ConnectionStrings:PVW%>"
                    DeleteCommand="DELETE FROM [PerfilesOrdenes] WHERE [id_perfilOrden] = @id_localizacion"
                    InsertCommand="insert into PerfilesOrdenes values(isnull((select top 1 id_perfilOrden from PerfilesOrdenes order by id_perfilOrden desc),0)+1, @descripcion)"
                    SelectCommand="SELECT [id_perfilOrden], [descripcion] FROM [PerfilesOrdenes]"
                    UpdateCommand="update PerfilesOrdenes set descripcion=@descripcion WHERE id_perfilOrden=@id_localizacion">
                    <DeleteParameters>
                        <asp:Parameter Name="id_perfilOrden" Type="Int32" />
                    </DeleteParameters>
                    <InsertParameters>
                        <asp:ControlParameter ControlID="txtDescripcionNew" Name="descripcion" Type="String" />
                    </InsertParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="id_perfilOrden" Type="Int32" />
                        <asp:Parameter Name="descripcion" Type="String" />
                    </UpdateParameters>
                </asp:SqlDataSource>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="agrega" CssClass="errores alert-danger" DisplayMode="List" />
                <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="edita" CssClass="errores alert-danger" DisplayMode="List" />
                <asp:Label ID="lblError" runat="server" CssClass="errores alert-danger"></asp:Label>
                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
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

