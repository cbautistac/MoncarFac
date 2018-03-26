<%@ Page Title="" Language="C#" MasterPageFile="~/Administracion.master" AutoEventWireup="true" CodeFile="AsignacionPermisos.aspx.cs" Inherits="AsignacionPermisos" %>

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
                <asp:Label ID="Label1" runat="server" Text="Asignación de Permisos"></asp:Label>
            </h3>
        </div>
    </div>
    <asp:Panel ID="pnlCatalogos" runat="server" CssClass="col-lg-12 col-sm-12" ScrollBars="Auto">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="col-lg-12 col-sm-12 text-center">
                    <asp:Label ID="Label3" runat="server" Text="Usuario:" CssClass="textoBold"></asp:Label>
                    <asp:DropDownList ID="ddlUsuarios" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlUsuarios_SelectedIndexChanged" DataSourceID="SqlDataSource1" DataTextField="nick" DataValueField="id_usuario" CssClass="input-medium"></asp:DropDownList>
                    <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:PVW %>' SelectCommand="select id_usuario,nick from usuarios where estatus='A' and id_usuario<>1"></asp:SqlDataSource>
                </div>
                <br />
                <div class="col-lg-12 col-sm-12">
                    <div class="col-lg-6 col-sm-6 text-center">
                        <asp:GridView ID="GridPermisos" runat="server" AutoGenerateColumns="False" DataKeyNames="id_permiso" DataSourceID="SqlDataSource2" AllowPaging="true" PageSize="10"
                            CssClass="table table-bordered" AllowSorting="true" EmptyDataText="No hay permisos que asignar" EmptyDataRowStyle-CssClass="errores">
                            <Columns>
                                <asp:TemplateField HeaderText="id_permiso" SortExpression="id_permiso" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Bind("id_permiso") %>' ID="Label1"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Permisos Disponibles" SortExpression="permiso">
                                    <ItemTemplate>
                                        <asp:LinkButton OnClick="lnkAsignar_Click" ID="lnkAsignar" runat="server" CommandArgument='<%# Bind("id_permiso") %>' Text='<%# Bind("permiso") %>'/>
                                        <asp:Label runat="server" Visible="false" Text='<%# Bind("permiso") %>' ID="Label2"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource runat="server" ID="SqlDataSource2" ConnectionString='<%$ ConnectionStrings:PVW %>' 
                            SelectCommand="select id_permiso,permiso from permisos where id_permiso not in(select id_permiso from usuarios_permisos where id_usuario=@id_usuario)">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="ddlUsuarios" PropertyName="SelectedValue" DefaultValue="" Name="id_usuario"></asp:ControlParameter>
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                    <div class="col-lg-6 col-sm-6 text-center">
                        <asp:GridView ID="GridAsignados" runat="server" AutoGenerateColumns="False" DataKeyNames="id_permiso" DataSourceID="SqlDataSource3" AllowPaging="true" PageSize="10"
                            CssClass="table table-bordered" AllowSorting="true" EmptyDataText="No se han asignado permisos al usuario seleccionado" EmptyDataRowStyle-CssClass="errores">
                            <Columns>
                                <asp:TemplateField HeaderText="id_permiso" SortExpression="id_permiso" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Bind("id_permiso") %>' ID="Label1"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Permiso Asignados" SortExpression="permiso">
                                    <ItemTemplate>
                                        <asp:LinkButton OnClick="lnkDesAsignar_Click" ID="lnkDesAsignar" runat="server" CommandArgument='<%# Bind("id_permiso") %>' Text='<%# Bind("permiso") %>'/>
                                        <asp:Label runat="server" Visible="false" Text='<%# Bind("permiso") %>' ID="Label2"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource runat="server" ID="SqlDataSource3" ConnectionString='<%$ ConnectionStrings:PVW %>' 
                            SelectCommand="select u.id_permiso,p.permiso from usuarios_permisos u inner join permisos p on u.id_permiso=p.id_permiso where u.id_usuario=@id_usuario">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="ddlUsuarios" PropertyName="SelectedValue" DefaultValue="" Name="id_usuario"></asp:ControlParameter>
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                </div>
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
