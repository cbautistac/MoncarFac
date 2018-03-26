<%@ Page Title="" Language="C#" MasterPageFile="~/Administracion.master" AutoEventWireup="true" CodeFile="Tipos_Servicios.aspx.cs" Inherits="Tipos_Servicios" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="row">
        <div class="col-lg-12 col-sm-12 text-center alert-info">
            <h3>
                <i class="fa fa-archive"></i>&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="Label1" runat="server" Text="Tipo Servicios"></asp:Label>
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
                            <asp:TextBox ID="txtDescripcionNew" runat="server" MaxLength="50" CssClass="ancho180px alineado" />
                            <cc1:TextBoxWatermarkExtender ID="txtDescripcionNew_TextBoxWatermarkExtender"
                                runat="server" BehaviorID="txtDescripcionNew_TextBoxWatermarkExtender"
                                TargetControlID="txtDescripcionNew" WatermarkCssClass="ancho180px water" WatermarkText="Servicio" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Debe indicar el Servicio" CssClass="errores alineado" ValidationGroup="agrega" ControlToValidate="txtDescripcionNew" Text="*" />
                        </td>
                        <td>&nbsp;&nbsp;<asp:TextBox ID="txtPrefijoNew" runat="server" MaxLength="3" CssClass="ancho70px" />
                            <cc1:TextBoxWatermarkExtender ID="txtPrefijoNew_TextBoxWatermarkExtender"
                                runat="server" BehaviorID="txtPrefijoNew_TextBoxWatermarkExtender"
                                TargetControlID="txtPrefijoNew" WatermarkCssClass="ancho70px water" WatermarkText="Prefijo" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Debe indicar el Prefijo" CssClass="errores alineado" ValidationGroup="agrega" ControlToValidate="txtPrefijoNew" Text="*" />
                        </td>
                        <td>
                            <asp:LinkButton ID="btnAceptarNew" runat="server" CssClass="btn btn-info t14" OnClick="btnAceptarNew_Click1" ToolTip="Agregar" ValidationGroup="agrega"><i class="fa fa-plus-circle"></i>&nbsp;<span>Agregar</span></asp:LinkButton>
                            </td>
                    </tr>
                </table>
                <asp:GridView ID="grvTipoServicios" runat="server" AutoGenerateColumns="False"
                    CssClass="table table-bordered" AllowPaging="True" PageSize="10" AllowSorting="True"
                    EmptyDataText="No existen Servicios registrados." DataKeyNames="id_tipo_servicio" OnRowDeleting="grvTipoServicios_RowDeleting"
                    DataSourceID="SqlDataSource1" OnRowCommand="grvTipoServicios_RowCommand" OnRowDataBound="grvTipoServicios_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="id_tipo_servicio" HeaderText="id_tipo_servicio" ReadOnly="True" SortExpression="id_tipo_servicio" Visible="false" />
                        <asp:TemplateField HeaderText="Servicio" SortExpression="descripcion">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtDecripcion" runat="server" Text='<%# Bind("descripcion") %>' MaxLength="50" CssClass="ancho150px alineado" />
                                <cc1:TextBoxWatermarkExtender ID="txtDecripcion_TextBoxWatermarkExtender"
                                    runat="server" BehaviorID="txtDecripcion_TextBoxWatermarkExtender"
                                    TargetControlID="txtDecripcion" WatermarkCssClass="ancho150px water" WatermarkText="Servicio" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Debe indicar el Servicio" CssClass="errores alineado" ValidationGroup="edita" ControlToValidate="txtDecripcion" Text="*" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("descripcion") %>' />
                            </ItemTemplate>
                            <ItemStyle CssClass="ancho180px alineado" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Prefijo" SortExpression="prefijo">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtPrefijo" runat="server" Text='<%# Bind("prefijo") %>' MaxLength="3" CssClass="ancho70px alineado" />
                                <cc1:TextBoxWatermarkExtender ID="txtPrefijo_TextBoxWatermarkExtender"
                                    runat="server" BehaviorID="txtPrefijo_TextBoxWatermarkExtender"
                                    TargetControlID="txtPrefijo" WatermarkCssClass="ancho70px water" WatermarkText="Prefijo" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Debe indicar el Prefijo" CssClass="errores alineado" ValidationGroup="edita" ControlToValidate="txtPrefijo" Text="*" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("prefijo") %>' />
                            </ItemTemplate>
                            <ItemStyle CssClass="ancho70px" />
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
                    DeleteCommand="DELETE FROM [tipo_servicios] WHERE [id_tipo_servicio] = @id_tipo_servicio"
                    InsertCommand="insert into tipo_servicios values(isnull((select top 1 id_tipo_servicio from tipo_servicios order by id_tipo_servicio desc),0)+1, @descripcion,@prefijo)"
                    SelectCommand="SELECT [id_tipo_servicio], [descripcion], [prefijo] FROM [tipo_servicios]"
                    UpdateCommand="UPDATE [tipo_servicios] SET [descripcion] = @descripcion, [prefijo] = @prefijo WHERE [id_tipo_servicio] = @id_tipo_servicio">
                    <DeleteParameters>
                        <asp:Parameter Name="id_tipo_servicio" Type="Int32" />
                    </DeleteParameters>
                    <InsertParameters>
                        <asp:ControlParameter ControlID="txtDescripcionNew" Name="descripcion" Type="String" />
                        <asp:ControlParameter ControlID="txtPrefijoNew" Name="prefijo" Type="String" />
                    </InsertParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="id_tipo_servicio" Type="Int32" />
                        <asp:Parameter Name="descripcion" Type="String" />
                        <asp:Parameter Name="prefijo" Type="String" />
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
