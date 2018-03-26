<%@ Page Title="" Language="C#" MasterPageFile="~/Administracion.master" AutoEventWireup="true" CodeFile="Tipo_Unidad.aspx.cs" Inherits="Tipo_Unidad" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="er" TagName="SesionTerminada" Src="~/Errores.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="row">
        <div class="col-lg-12 col-sm-12 text-center alert-info">
            <h3>
                <i class="fa fa-shield"></i>&nbsp;
                <i class="fa fa-car"></i>&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="Label1" runat="server" Text="Línea de Vehículos"></asp:Label>
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
                            <asp:DropDownList ID="ddlMarca" runat="server" DataSourceID="SqlDataSource3"
                                DataTextField="descripcion" DataValueField="id_marca">
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SqlDataSource3" runat="server"
                                ConnectionString="<%$ ConnectionStrings:PVW %>"
                                SelectCommand="SELECT [id_marca], [descripcion] FROM [Marcas]"></asp:SqlDataSource>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlTipoVehiculo" runat="server"
                                DataSourceID="SqlDataSource4" DataTextField="descripcion"
                                DataValueField="id_tipo_vehiculo">
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SqlDataSource4" runat="server"
                                ConnectionString="<%$ ConnectionStrings:PVW %>"
                                SelectCommand="SELECT [id_tipo_vehiculo], [descripcion] FROM [Tipo_Vehiculo]"></asp:SqlDataSource>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDescripcionNew" runat="server" MaxLength="50" CssClass="ancho150px alineado" />
                            <cc1:TextBoxWatermarkExtender ID="txtDescripcionNew_TextBoxWatermarkExtender"
                                runat="server" BehaviorID="txtDescripcionNew_TextBoxWatermarkExtender"
                                TargetControlID="txtDescripcionNew" WatermarkCssClass="ancho150px water alineado" WatermarkText="Línea" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Necesita colocar la descripción." ValidationGroup="agrega" CssClass="errores alineado" Text="*" ControlToValidate="txtDescripcionNew" />
                        </td>
                        <td>
                            <asp:LinkButton ID="btnAceptarNew" runat="server" CssClass="btn btn-info t14" OnClick="btnAceptarNew_Click" ToolTip="Agregar" ValidationGroup="agrega"><i class="fa fa-plus-circle"></i>&nbsp;<span>Agregar</span></asp:LinkButton>
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="grvTipoUnidad" runat="server" AutoGenerateColumns="False" OnRowDeleting="grvTipoUnidad_RowDeleting"
                    CssClass="table table-bordered" AllowPaging="True" GridLines="None" AllowSorting="True"
                    EmptyDataText="No existe Información para mostrar" DataKeyNames="id_marca,id_tipo_vehiculo,id_tipo_unidad"
                    DataSourceID="SqlDataSource1" OnRowCommand="grvTipoUnidad_RowCommand" OnRowDataBound="grvTipoUnidad_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="marca" SortExpression="marca" HeaderText="Marca" ReadOnly="true" />
                        <asp:BoundField DataField="tipo_vehiculo" SortExpression="tipo_vehiculo" HeaderText="Tipo Vehiculo" ReadOnly="true" />
                        <asp:BoundField DataField="id_tipo_unidad" HeaderText="id_tipo_unidad" SortExpression="id_tipo_unidad" Visible="false" ReadOnly="true" />
                        <asp:TemplateField HeaderText="Línea" SortExpression="descripcion">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtDescripcion" runat="server" MaxLength="50" CssClass="ancho150px alineado" Text='<%# Bind("descripcion") %>' />
                                <cc1:TextBoxWatermarkExtender ID="txtDescripcion_TextBoxWatermarkExtender"
                                    runat="server" BehaviorID="txtDescripcion_TextBoxWatermarkExtender"
                                    TargetControlID="txtDescripcion" WatermarkCssClass="ancho150px water" WatermarkText="Línea" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Debe indicar la Línea." ValidationGroup="edita" ControlToValidate="txtDescripcion" CssClass="errores alert-danger" Text="*" />
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
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>"
                    DeleteCommand="delete from Tipo_Unidad where id_marca=@id_marca and id_tipo_vehiculo=@id_tipo_vehiculo and id_tipo_unidad=@id_tipo_unidad"
                    InsertCommand="insert into Tipo_Unidad values(@id_marca,@id_tipo_vehiculo,(select isnull(count(tu2.id_tipo_unidad),0)+1 from Tipo_Unidad tu2 where tu2.id_marca=@id_marca and tu2.id_tipo_vehiculo=@id_tipo_vehiculo),@descripcion)"
                    SelectCommand="select m.id_marca,tv.id_tipo_vehiculo,tu.id_tipo_unidad,m.descripcion as marca,tv.descripcion as tipo_vehiculo,tu.id_tipo_unidad,tu.descripcion from Tipo_Unidad tu inner join Marcas m on m.id_marca=tu.id_marca inner join Tipo_Vehiculo tv on tv.id_tipo_vehiculo=tu.id_tipo_vehiculo"
                    UpdateCommand="update Tipo_Unidad set descripcion=@descripcion where id_marca=@id_marca and id_tipo_vehiculo=@id_tipo_vehiculo and id_tipo_unidad=@id_tipo_unidad">
                    <DeleteParameters>
                        <asp:Parameter Name="id_marca" Type="Int32" />
                        <asp:Parameter Name="id_tipo_vehiculo" Type="Int32" />
                        <asp:Parameter Name="id_tipo_unidad" Type="Int32" />
                    </DeleteParameters>
                    <InsertParameters>
                        <asp:ControlParameter ControlID="ddlMarca" Name="id_marca" Type="Int32" PropertyName="SelectedValue" />
                        <asp:ControlParameter ControlID="ddlTipoVehiculo" Name="id_tipo_vehiculo" Type="Int32" PropertyName="SelectedValue" />
                        <asp:ControlParameter ControlID="txtDescripcionNew" Name="descripcion" Type="String" />
                    </InsertParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="id_marca" Type="Int32" />
                        <asp:Parameter Name="id_tipo_vehiculo" Type="Int32" />
                        <asp:Parameter Name="id_tipo_unidad" Type="Int32" />
                        <asp:Parameter Name="descripcion" Type="String" />
                    </UpdateParameters>
                </asp:SqlDataSource>
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
