<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Vehiculos.aspx.cs" Inherits="Vehiculos" MasterPageFile="~/Administracion.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:Panel ID="pnlContenido" CssClass="col-lg-12 col-sm-12" runat="server">
        <h1 class="centrado textoCentrado colorMoncarAzul">Veh&iacute;culos</h1>
        <div class="row">
            <div class="col-lg-12 col-sm-12 text-center alert-info">
                <h3>
                    <i class="fa fa-list"></i>&nbsp;
                <i class="fa fa-car"></i>&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="Label1" runat="server" Text="Tipo Asegurados"></asp:Label>
                </h3>
            </div>
        </div>
        <div class="row pad1m">
            <div class="col-lg-12 col-sm-12 text-center">
                <asp:Label ID="lblError" runat="server" CssClass="errores alert-danger"></asp:Label>
            </div>
        </div>
        <asp:Panel ID="pnlCatalogos" runat="server" CssClass="col-lg-12 col-sm-12" ScrollBars="Auto">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="GridView1" runat="server" AllowPaging="True" PageSize="10" AllowSorting="True" AutoGenerateColumns="False"
                        DataKeyNames="id_marca,id_tipo_vehiculo,id_tipo_unidad,id_vehiculo" DataSourceID="SqlDataSource1" EmptyDataText="No existen Vehículos registrados" CssClass="table table-bordered">
                        <Columns>
                            <asp:BoundField DataField="id_marca" HeaderText="id_marca" ReadOnly="True" SortExpression="id_marca" Visible="false" />
                            <asp:BoundField DataField="id_tipo_vehiculo" HeaderText="id_tipo_vehiculo" ReadOnly="True" SortExpression="id_tipo_vehiculo" Visible="false" />
                            <asp:BoundField DataField="id_tipo_unidad" HeaderText="id_tipo_unidad" ReadOnly="True" SortExpression="id_tipo_unidad" Visible="false" />
                            <asp:BoundField DataField="id_vehiculo" HeaderText="id_vehiculo" ReadOnly="True" SortExpression="id_vehiculo" Visible="false" />
                            <asp:BoundField DataField="marca" HeaderText="Marca" SortExpression="marca" />
                            <asp:BoundField DataField="tipo_vehiculo" HeaderText="Tipo Vehículo" SortExpression="tipo_vehiculo" />
                            <asp:BoundField DataField="tipo_unidad" HeaderText="Línea" SortExpression="tipo_unidad" />
                            <asp:BoundField DataField="placas" HeaderText="Placas" SortExpression="placas" />
                            <asp:BoundField DataField="modelo" HeaderText="Modelo" SortExpression="modelo" />
                            <asp:BoundField DataField="color_ext" HeaderText="Color Exterior" SortExpression="color_ext" />
                            <asp:BoundField DataField="color_int" HeaderText="Color Interior" SortExpression="color_int" />
                        </Columns>
                        <EditRowStyle CssClass="alert-warning" />
                        <EmptyDataRowStyle CssClass="errores alert-danger" />
                        <SelectedRowStyle CssClass="alert-success" />
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>"
                        SelectCommand="select v.id_marca,v.id_tipo_vehiculo,v.id_tipo_unidad,v.id_vehiculo,m.descripcion as marca,tv.descripcion as tipo_vehiculo,(select descripcion from tipo_unidad where id_marca=v.id_marca and id_tipo_vehiculo=v.id_tipo_vehiculo and id_tipo_unidad=v.id_tipo_unidad) as tipo_unidad,v.placas,v.modelo,v.color_ext,v.color_int
                            from Vehiculos v
                            inner join Marcas m on m.id_marca=v.id_marca
                            inner join Tipo_Vehiculo tv on tv.id_tipo_vehiculo=v.id_tipo_vehiculo"></asp:SqlDataSource>
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

