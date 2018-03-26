<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CatEstatusRefacciones.aspx.cs" Inherits="CatEstatusRefacciones" MasterPageFile="~/Administracion.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Panel ID="pnlContenido" CssClass="col-lg-12 col-sm-12" runat="server">
        <div class="row">
            <div class="col-lg-12 col-sm-12 text-center alert-info">
                <h3>
                    <i class="fa fa-cog"></i>&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="Label1" runat="server" Text="Estatus Refacciones"></asp:Label>
                </h3>
            </div>
        </div>
        <div class="row pad1m">
            <div class="col-lg-12 col-sm-12 text-center">
                <asp:ValidationSummary ID="ValidationSummary2" runat="server" CssClass="errores alert-danger" DisplayMode="List" ValidationGroup="agrega" />
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="edita" CssClass="errores alert-danger" DisplayMode="List" />
                <asp:Label ID="lblError" runat="server" CssClass="errores alert-danger"></asp:Label>
            </div>
        </div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:Panel ID="pnlCatalogos" runat="server" CssClass="col-lg-12 col-sm-12" ScrollBars="Auto">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table class="centrado">
                        <tr>
                            <td>
                                <asp:TextBox ID="txtDescripcionAdd" runat="server" CssClass="ancho150px" MaxLength="50" />
                                <cc1:TextBoxWatermarkExtender ID="txtDescripcionAdd_TextBoxWatermarkExtender"
                                    runat="server" BehaviorID="txtDescripcionAdd_TextBoxWatermarkExtender"
                                    TargetControlID="txtDescripcionAdd" WatermarkText="Descripción" WatermarkCssClass="ancho150px water" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Necesita colocar una descripción" CssClass="errores alert-danger" Text="*" ControlToValidate="txtDescripcionAdd" ValidationGroup="agrega" />
                            </td>
                            <td>
                                <asp:LinkButton ID="btnAgregar" runat="server" CssClass="btn btn-info t14" OnClick="btnAgregar_Click" ToolTip="Agregar" ValidationGroup="agrega"><i class="fa fa-plus-circle"></i>&nbsp;<span>Agregar</span></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                    <asp:GridView ID="GridGrupoOp" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered"
                        AllowPaging="True" PageSize="10" AllowSorting="True" OnRowDataBound="GridGrupoOp_RowDataBound"
                        EmptyDataText="No existen grupos de operaciones" DataKeyNames="staRefID"
                        DataSourceID="SqlDataSource1" OnRowCommand="GridGrupoOp_RowCommand"
                        OnRowDeleting="GridGrupoOp_RowDeleting">
                        <Columns>
                            <asp:BoundField DataField="staRefID" HeaderText="staRefID" ReadOnly="True" SortExpression="staRefID" Visible="false" />
                            <asp:TemplateField HeaderText="Descripción" SortExpression="staDescripcion">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtDescripcionedita" runat="server" Text='<%# Bind("staDescripcion") %>' MaxLength="50" />
                                    <cc1:TextBoxWatermarkExtender ID="txtDescripcionedita_TextBoxWatermarkExtender"
                                        runat="server" BehaviorID="txtDescripcionedita_TextBoxWatermarkExtender"
                                        TargetControlID="txtDescripcionedita" WatermarkText="Descripción" WatermarkCssClass="ancho150px water" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="Necesita colocar una descripción" CssClass="errores alert-danger" Text="*" ControlToValidate="txtDescripcionedita" ValidationGroup="edita" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDescripcionGo" runat="server" Text='<%# Bind("staDescripcion") %>' />
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
                        SelectCommand="select staRefID,staDescripcion from Rafacciones_Estatus"
                        DeleteCommand="DELETE FROM Rafacciones_Estatus where staRefID=@id_grupo_op"
                        InsertCommand="insert into Rafacciones_Estatus(staDescripcion) values( @descripcion_go)"
                        UpdateCommand="update Rafacciones_Estatus set staDescripcion=@descripcion_go where staRefID=@id_grupo_op">
                        <InsertParameters>
                            <asp:ControlParameter ControlID="txtDescripcionAdd" Type="String" Name="descripcion_go" />
                        </InsertParameters>
                        <DeleteParameters>
                            <asp:Parameter Name="id_grupo_op" Type="Int32" />
                        </DeleteParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="id_grupo_op" Type="Int32" />
                            <asp:Parameter Name="descripcion_go" Type="String" />
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
    </asp:Panel>
</asp:Content>