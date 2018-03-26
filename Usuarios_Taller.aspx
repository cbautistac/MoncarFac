<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Usuarios_Taller.aspx.cs" Inherits="Usuarios_Taller" MasterPageFile="~/Administracion.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="row">
        <div class="col-lg-12 col-sm-12 text-center alert-info">
            <h3>
                <i class="fa fa-user"></i>&nbsp;
                    <i class="fa fa-industry"></i>&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="Label1" runat="server" Text="Usuarios Talleres"></asp:Label>
            </h3>
        </div>
    </div>
    <div class="row pad1m">
        <div class="col-lg-12 col-sm-12 text-center">
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="agrega" CssClass="errores alert-danger" DisplayMode="List" />
            <asp:ValidationSummary ID="ValidationSummary3" runat="server" ValidationGroup="edita" CssClass="errores alert-danger" DisplayMode="List" />
            <asp:Label ID="Label2" runat="server" CssClass="errores alert-danger" />
        </div>
    </div>
    <asp:Panel ID="pnlContenido" CssClass="panelContenido centrado" runat="server">
        <h1 class="centrado textoCentrado colorMoncarAzul">Usuarios Talleres</h1>
        <asp:Panel ID="pnlCatalogos" runat="server" CssClass="col-lg-12 col-sm-12" ScrollBars="Auto">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table class="centrado ancho100">
                        <tr>
                            <td>
                                <asp:DropDownList ID="ddlEmpresas" runat="server" DataSourceID="SqlDataSource4" DataTextField="razon_social" DataValueField="id_empresa" AutoPostBack="true"></asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select e.id_empresa,e.razon_social from Empresas e"></asp:SqlDataSource>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Debe indicar la Empresa" Text="*" CssClass="errores alineado" ControlToValidate="ddlEmpresas" ValidationGroup="agrega"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlTalleres" runat="server" DataSourceID="SqlDataSource2" DataTextField="nombre_taller" DataValueField="id_taller" AutoPostBack="true"></asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select et.id_taller,t.nombre_taller from Empresas_Taller et inner join Talleres t on t.id_taller=et.id_taller where et.id_empresa=@id_empresa">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="ddlEmpresas" DefaultValue="0" Name="id_empresa" PropertyName="SelectedValue" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Debe indicar el Taller" Text="*" CssClass="errores alineado" ControlToValidate="ddlTalleres" ValidationGroup="agrega"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlUsuario" runat="server" DataSourceID="SqlDataSource1" DataTextField="nombre_usuario" DataValueField="id_usuario"></asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select u.id_usuario,u.nombre_usuario from Usuarios u where u.id_usuario not in(select id_usuario from Usuarios_Taller where id_empresa=@id_empresa and id_taller=@id_taller) and u.id_usuario<>1">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="ddlEmpresas" DefaultValue="0" Name="id_empresa" PropertyName="SelectedValue" />
                                        <asp:ControlParameter ControlID="ddlTalleres" DefaultValue="0" Name="id_taller" PropertyName="SelectedValue" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Debe indicar el Usuario" Text="*" CssClass="errores alineado" ControlToValidate="ddlUsuario" ValidationGroup="agrega"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:LinkButton ID="btnAgregar" runat="server" CssClass="btn btn-info t14" OnClick="btnAgregar_Click" ToolTip="Agregar" ValidationGroup="agrega"><i class="fa fa-plus-circle"></i>&nbsp;<span>Agregar</span></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                    <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
                        OnRowCommand="GridView1_RowCommand" AutoGenerateColumns="False"
                        DataKeyNames="id_empresa,id_taller,id_usuario" DataSourceID="SqlDataSource3"
                        CssClass="table table-bordered"
                        EmptyDataText="No existen usuaros registrados al taller">
                        <Columns>
                            <asp:BoundField DataField="id_empresa" HeaderText="id_empresa" ReadOnly="True" SortExpression="id_empresa" Visible="false" />
                            <asp:BoundField DataField="id_taller" HeaderText="id_taller" ReadOnly="True" SortExpression="id_taller" Visible="false" />
                            <asp:BoundField DataField="id_usuario" HeaderText="id_usuario" ReadOnly="True" SortExpression="id_usuario" Visible="false" />
                            <asp:BoundField DataField="razon_social" HeaderText="Empresa" SortExpression="razon_social" />
                            <asp:BoundField DataField="nombre_taller" HeaderText="Taller" SortExpression="nombre_taller" />
                            <asp:BoundField DataField="nombre_usuario" HeaderText="Usuario" SortExpression="nombre_usuario" />
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEliminar" runat="server" CommandArgument='<%# Eval("id_empresa")+";"+Eval("id_taller")+";"+Eval("id_usuario") %>' CausesValidation="False" CommandName="Delete" ToolTip="Eliminar" CssClass="btn btn-danger t14" OnClientClick="return confirm('¿Esta seguro de eliminar la relación del Usuario con el taller?');"><i class="fa fa-trash"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EditRowStyle CssClass="alert-warning" />
                        <EmptyDataRowStyle CssClass="errores alert-danger" />
                        <SelectedRowStyle CssClass="alert-success" />
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>"
                        DeleteCommand="delete from Usuarios_Taller where id_usuario=@id_usuario and id_empresa=@id_empresa and id_taller=@id_taller"
                        InsertCommand="insert into Usuarios_Taller values(@id_usuario,@id_empresa,@id_taller)"
                        SelectCommand="select ut.id_empresa,ut.id_taller,ut.id_usuario,e.razon_social,t.nombre_taller,u.nombre_usuario from Usuarios_Taller ut 
                            inner join Empresas e on e.id_empresa=ut.id_empresa
                            inner join Talleres t on t.id_taller=ut.id_taller
                            inner join Usuarios u on u.id_usuario=ut.id_usuario">
                        <DeleteParameters>
                            <asp:Parameter Name="id_usuario" />
                            <asp:Parameter Name="id_empresa" />
                            <asp:Parameter Name="id_taller" />
                        </DeleteParameters>
                        <InsertParameters>
                            <asp:ControlParameter ControlID="ddlEmpresas" Name="id_empresa" PropertyName="SelectedValue" Type="Int32" />
                            <asp:ControlParameter ControlID="ddlTalleres" Name="id_taller" PropertyName="SelectedValue" Type="Int32" />
                            <asp:ControlParameter ControlID="ddlUsuario" Name="id_usuario" PropertyName="SelectedValue" Type="Int32" />
                        </InsertParameters>
                    </asp:SqlDataSource>
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="agrega" CssClass="errores alert-danger" DisplayMode="List" />
                    <asp:Label ID="lblError" runat="server" CssClass="errores alert-danger"></asp:Label>
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                        <ProgressTemplate>
                            <div class="mask zen3">
                                <div class="carga zen4">
                                    <asp:Image ID="Cargando" runat="server" ImageUrl="~/img/eapps2.gif" Width="100%" />
                                </div>
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
    </asp:Panel>
</asp:Content>
