<%@ Page Title="" Language="C#" MasterPageFile="~/Administracion.master" AutoEventWireup="true" CodeFile="Talleres.aspx.cs" Inherits="Talleres" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:Panel ID="pnlContenido" CssClass="panelContenido centrado" runat="server">
        <div class="row">
            <div class="col-lg-12 col-sm-12 text-center alert-info">
                <h3>
                    <i class="fa fa-industry"></i>&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="Label1" runat="server" Text="Talleres"></asp:Label>
                </h3>
            </div>
        </div>
        <div class="row pad1m">
            <div class="col-lg-12 col-sm-12 text-center">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="agrega" CssClass="errores alert-danger" DisplayMode="List" />
                <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="edita" CssClass="errores alert-danger" DisplayMode="List" />
                <asp:Label ID="lblError" runat="server" CssClass="errores alert-danger" />
            </div>
        </div>
        <asp:Panel ID="pnlCatalogos" runat="server" CssClass="paneles col-lg-12 col-sm-12" ScrollBars="Auto">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table class="centrado">
                        <tr>
                            <td>
                                <asp:TextBox ID="txtAltaTaller" runat="server" MaxLength="200" CssClass="input-medium"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="txtAltaTaller_TextBoxWatermarkExtender"
                                    runat="server" BehaviorID="txtAltaTaller_TextBoxWatermarkExtender"
                                    TargetControlID="txtAltaTaller" WatermarkCssClass="ancho150px water" WatermarkText="Taller" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Debe indicar Taller" CssClass="errores alineado" ValidationGroup="agrega" ControlToValidate="txtAltaTaller" Text="*"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAltaPrefijo" runat="server" MaxLength="3" CssClass="input-medium"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="txtAltaPrefijoWatermarkExtender1"
                                    runat="server" BehaviorID="txtAltaPrefijo_TextBoxWatermarkExtender"
                                    TargetControlID="txtAltaPrefijo" WatermarkCssClass="ancho70px water" WatermarkText="Prefijo" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Debe indicar el prefijo del taller" CssClass="errores alineado" ValidationGroup="agrega" ControlToValidate="txtAltaPrefijo" Text="*"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:LinkButton ID="btnAgregar" runat="server" CssClass="btn btn-info t14" OnClick="btnAgregar_Click" ToolTip="Agregar" ValidationGroup="agrega"><i class="fa fa-plus-circle"></i>&nbsp;<span>Agregar</span></asp:LinkButton>
                        </tr>
                    </table>
                    <asp:GridView ID="grvTalleres" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered" OnRowDeleting="grvTalleres_RowDeleting"
                        DataKeyNames="id_taller" DataSourceID="SqlDataSource1" AllowPaging="True" OnRowEditing="grvTalleres_RowEditing" PageSize="10"
                        AllowSorting="True" EmptyDataText="No existen Talleres Registrados" OnRowDataBound="grvTalleres_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="id_taller" HeaderText="id_taller" ReadOnly="True" SortExpression="id_taller" Visible="false" />
                            <asp:TemplateField HeaderText="Taller" SortExpression="nombre_taller">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtTaller" runat="server" Text='<%# Bind("nombre_taller") %>' MaxLength="200" CssClass="input-medium"></asp:TextBox>
                                    <cc1:TextBoxWatermarkExtender ID="txtTaller_TextBoxWatermarkExtender"
                                        runat="server" BehaviorID="txtTaller_TextBoxWatermarkExtender"
                                        TargetControlID="txtTaller" WatermarkCssClass="ancho100px water" WatermarkText="Taller" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Debe indicar el Taller" CssClass="errores alineado" ValidationGroup="edita" ControlToValidate="txtTaller" Text="*"></asp:RequiredFieldValidator>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblTaller" runat="server" Text='<%# Bind("nombre_taller") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Identificador Taller" SortExpression="identificador">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtPrefijo" runat="server" Text='<%# Bind("identificador") %>' MaxLength="3" CssClass="input-medium"></asp:TextBox>
                                    <cc1:TextBoxWatermarkExtender ID="txtPrefijo_TextBoxWatermarkExtender"
                                        runat="server" BehaviorID="txtPrefijo_TextBoxWatermarkExtender"
                                        TargetControlID="txtPrefijo" WatermarkCssClass="ancho70px water" WatermarkText="Prefijo" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Debe indicar el prefijo del Taller" CssClass="errores alineado" ValidationGroup="edita" ControlToValidate="txtPrefijo" Text="*"></asp:RequiredFieldValidator>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblI" runat="server" Text='<%# Bind("identificador") %>'></asp:Label>
                                </ItemTemplate>
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
                                    <asp:LinkButton ID="btnEliminar" runat="server" CausesValidation="False" CommandName="Delete" ToolTip="Eliminar" CssClass="btn btn-danger t14" OnClientClick="return confirm('¿Esta seguro de eliminar el Taller?');"><i class="fa fa-trash"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EditRowStyle CssClass="alert-warning" />
                        <EmptyDataRowStyle CssClass="errores alert-danger" />
                        <SelectedRowStyle CssClass="alert-success" />
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                        ConnectionString="<%$ ConnectionStrings:PVW %>"
                        SelectCommand="SELECT id_taller, nombre_taller, identificador FROM Talleres"
                        UpdateCommand="UPDATE Talleres SET nombre_taller=@nombre_taller, identificador=@identificador WHERE (id_taller = @id_taller)"
                        DeleteCommand="DELETE FROM Talleres where id_taller=@id_taller"
                        InsertCommand="insert into Talleres values(isnull((select top 1 id_taller from Talleres order by id_taller desc),0)+1, @nombre_taller, @identificador)">
                        <InsertParameters>
                            <asp:ControlParameter ControlID="txtAltaTaller" Type="String" Name="nombre_taller" PropertyName="Text" />
                            <asp:ControlParameter ControlID="txtAltaPrefijo" Type="String" Name="identificador" PropertyName="Text" />
                        </InsertParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="nombre_taller" />
                            <asp:Parameter Name="identificador" />
                        </UpdateParameters>
                        <DeleteParameters>
                            <asp:Parameter Name="id_taller" />
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

