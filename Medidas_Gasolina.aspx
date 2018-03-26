<%@ Page Title="" Language="C#" MasterPageFile="~/Administracion.master" AutoEventWireup="true" CodeFile="Medidas_Gasolina.aspx.cs" Inherits="Medidas_Gasolina" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:Panel ID="pnlContenido" CssClass="col-lg-12 col-sm-12" runat="server">
        <div class="row">
            <div class="col-lg-12 col-sm-12 text-center alert-info">
                <h3>
                    <i class="fa fa-tachometer"></i>&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="Label1" runat="server" Text="Medidas Gasolina"></asp:Label>
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
                        <tr>
                            <td>
                                <asp:TextBox ID="txtAltaGasolina" runat="server" MaxLength="50" CssClass="ancho150px alineado"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="txtAltaGasolina_TextBoxWatermarkExtender"
                                    runat="server" BehaviorID="txtAltaGasolina_TextBoxWatermarkExtender"
                                    TargetControlID="txtAltaGasolina" WatermarkCssClass="ancho150px water" WatermarkText="Medida Gasolina" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Debe indicar la Medida de Gasolina" CssClass="errores alineado" ValidationGroup="agrega" ControlToValidate="txtAltaGasolina" Text="*"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:LinkButton ID="btnAgregar" runat="server" CssClass="btn btn-info t14" OnClick="btnAgregar_Click" ToolTip="Agregar" ValidationGroup="agrega"><i class="fa fa-plus-circle"></i>&nbsp;<span>Agregar</span></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                    <asp:GridView ID="grvGasolina" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered"
                        DataKeyNames="id_med_gas" DataSourceID="SqlDataSource1" AllowPaging="True" PageSize="10"
                        AllowSorting="True" EmptyDataText="No existen Medidas de Gasolina Registradas" OnRowDataBound="grvGasolina_RowDataBound"
                        OnRowDeleting="grvGasolina_RowDeleting">
                        <Columns>
                            <asp:BoundField DataField="id_med_gas" HeaderText="id_Gasolina" ReadOnly="True" SortExpression="id_med_gas" Visible="false" />
                            <asp:TemplateField HeaderText="Medida Gasolina" SortExpression="descripcion">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtGasolina" runat="server" Text='<%# Bind("descripcion") %>' MaxLength="50" CssClass="ancho100px alineado"></asp:TextBox>
                                    <cc1:TextBoxWatermarkExtender ID="txtGasolina_TextBoxWatermarkExtender"
                                        runat="server" BehaviorID="txtGasolina_TextBoxWatermarkExtender"
                                        TargetControlID="txtGasolina" WatermarkCssClass="ancho100px water" WatermarkText="Medida Gasolina" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Debe indicar la medida de Gasolina" CssClass="errores alineado" ValidationGroup="edita" ControlToValidate="txtGasolina" Text="*"></asp:RequiredFieldValidator>

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
                        SelectCommand="SELECT id_med_gas, descripcion FROM Medidas_Gasolina"
                        UpdateCommand="UPDATE Medidas_Gasolina SET descripcion=@descripcion WHERE (id_med_gas = @id_med_gas)"
                        DeleteCommand="DELETE FROM Medidas_Gasolina where id_med_gas=@id_med_gas"
                        InsertCommand="insert into Medidas_Gasolina values(isnull((select top 1 id_med_gas from Medidas_Gasolina order by id_med_gas desc),0)+1, @descripcion)">
                        <InsertParameters>
                            <asp:ControlParameter ControlID="txtAltaGasolina" Type="String" Name="descripcion" />
                        </InsertParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="descripcion" />
                        </UpdateParameters>
                        <DeleteParameters>
                            <asp:Parameter Name="id_med_gas" />
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




