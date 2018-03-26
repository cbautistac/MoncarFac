<%@ Page Title="" Language="C#" MasterPageFile="~/Administracion.master" AutoEventWireup="true" CodeFile="Cambio_Placa.aspx.cs" Inherits="Cambio_Placa" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="row">
        <div class="col-lg-12 col-sm-12 text-center alert-info">
            <h3>
                <i class="fa fa-edit"></i>&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="Label3" runat="server" Text="Cambio de Placa"></asp:Label>
            </h3>
        </div>
    </div>
    <div class="row pad1m">
        <div class="col-lg-12 col-sm-12 text-center">
            <asp:Label ID="lblError" runat="server" CssClass="errores alert-danger" />
        </div>
    </div>
    <asp:Panel ID="PanleCambPlaca" runat="server" CssClass="panelCatalogos textoCentrado ancho30 pad5px centrado" ScrollBars="Auto" >
        <table class="centrado">
            <tr>
                <td>
                    <asp:TextBox ID="txtNoOrden" runat="server" placeholder="No. Orden" MaxLength="7" CssClass="alingMiddle input-medium" />
                </td>
                <td>
                    <asp:LinkButton ID="btnBuscar" runat="server" ToolTip="Buscar" OnClick="btnBuscar_Click" CssClass="btn btn-info t14"><i class="fa fa-search"></i>&nbsp;<span>Buscar</span></asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="txtPlaca" runat="server" Visible="false" placeholder="Placa" CssClass="alingMiddle input-medium"/>
                    <asp:Label ID="lblPlacaVieja" runat="server" Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:LinkButton ID="btnGuardar" runat="server" ToolTip="Guardar" OnClick="btnGuardar_Click" CssClass="btn btn-success t14" OnClientClick="return confirm('¿Esta seguro de cambiar la placa?');" Visible="false"><i class="fa fa-save"></i>&nbsp;<span>Guardar</span></asp:LinkButton>
                </td>                    
            </tr>
        </table>
    </asp:Panel>
</asp:Content>

