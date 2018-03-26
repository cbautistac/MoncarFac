<%@ Page Title="" Language="C#" MasterPageFile="~/AdmOrdenes.master" AutoEventWireup="true" CodeFile="FacturacionPv.aspx.cs" Inherits="FacturacionPv" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" />
    <div class="page-header">
        <!-- /BREADCRUMBS -->
        <div class="clearfix">
            <h3 class="content-title pull-left">
                Punto de Venta</h3>            
        </div>
    </div>
    <div class="row">                    
        <div class="col-lg-6 col-sm-6 center">
            <div class="table-responsive">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="grdFactPend" runat="server" AutoGenerateColumns="False"
                            CssClass="table table-bordered center" EmptyDataText="No existen facturas pendientes" EmptyDataRowStyle-ForeColor="Red"
                            DataKeyNames="ticket" AllowPaging="True" PageSize="10" OnPageIndexChanging="grdFactPend_PageIndexChanging"
                            AllowSorting="True" DataSourceID="SqlDataSource1">
                            <Columns>                                                                        
                                <asp:BoundField DataField="ticket" HeaderText="Ticket" ReadOnly="True" SortExpression="ticket"></asp:BoundField>
                                <asp:TemplateField HeaderText="Fecha Venta" SortExpression="fecha_venta">
                                    <EditItemTemplate>
                                        <asp:Label runat="server" Text='<%# Bind("fecha_venta") %>' ID="Label1"></asp:Label>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Bind("fecha_venta", "{0:yyyy-MM-dd}") %>' ID="Label1"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="nombre" HeaderText="Tienda" SortExpression="nombre"></asp:BoundField>
                                <asp:BoundField DataField="id_cliente" Visible="false" HeaderText="id_cliente" SortExpression="id_cliente"></asp:BoundField>                                
                                <asp:CommandField ButtonType="Button" ShowSelectButton="True" ControlStyle-CssClass="btn-success" SelectText="Seleccionar" >
                                <ControlStyle CssClass="btn-success" />
                                </asp:CommandField>                                    
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Button ID="btnFacturar" runat="server" Text="Facturar" CssClass="btn-warning" CommandArgument='<%# Eval("ticket")+";"+Eval("id_cliente") %>' onclick="btnFacturar_Click" OnClientClick="confirm('¿Esta seguro de facturar éste ticket?')"></asp:Button>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <SelectedRowStyle CssClass="alert-success-org" />
                        </asp:GridView>
                        <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:PVW %>' 
                            SelectCommand="select v.ticket,v.fecha_venta,c.nombre,v.id_cliente,v.estatus from venta_enc v left join catAlmacenes c on c.idAlmacen=v.id_punto where v.factura_Posterior=1 AND v.estatus='A' and CHARINDEX(';',v.tickets)=0">
                        </asp:SqlDataSource>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>        
        <div class="col-lg-6 col-sm-6 center">
            <div class="table-responsive">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="grdFactMasivas" runat="server" AutoGenerateColumns="False"
                            CssClass="table table-bordered center" EmptyDataText="No existen facturas pendientes" EmptyDataRowStyle-ForeColor="Red"
                            DataKeyNames="tickets" AllowPaging="True" PageSize="10" OnPageIndexChanging="grdFactMasivas_PageIndexChanging"
                            AllowSorting="True" DataSourceID="SqlDataSource2">
                            <Columns>                                                                        
                                <asp:BoundField DataField="tickets" HeaderText="Ticket" ReadOnly="True" SortExpression="tickets"></asp:BoundField>                                
                                <asp:BoundField DataField="nombre" HeaderText="Tienda" SortExpression="nombre"></asp:BoundField>
                                <asp:BoundField DataField="id_cliente" Visible="false" HeaderText="id_cliente" SortExpression="id_cliente"></asp:BoundField>                                
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Button ID="btnFacturar2" runat="server" Text="Facturar" CssClass="btn-warning" CommandArgument='<%# Eval("tickets")+"|"+Eval("id_cliente")+"|"+Eval("desglosado") %>' onclick="btnFacturar2_Click" OnClientClick="confirm('¿Esta seguro de facturar los tickets?')"></asp:Button>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <SelectedRowStyle CssClass="alert-success-org" />
                        </asp:GridView>
                        <asp:SqlDataSource runat="server" ID="SqlDataSource2" ConnectionString='<%$ ConnectionStrings:PVW %>' 
                            SelectCommand="select distinct isnull(cast(v.tickets as varchar(500)),'') as tickets,c.nombre,v.id_cliente,v.desglosado from venta_enc v left join catAlmacenes c on c.idAlmacen=v.id_punto where v.factura_Posterior=1 AND v.estatus='A' and CHARINDEX(';',v.tickets)>0">
                        </asp:SqlDataSource>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>        
    </div>
    <div class="row">
        <div class="col-lg-2 col-sm-2 center"></div>
        <div class="col-lg-8 col-sm-8 center">
            <div class="table-responsive">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:GridView runat="server" ID="GrdDetalle" ShowHeaderWhenEmpty="True"
                            AutoGenerateColumns="False" AllowSorting="True"
                            EmptyDataText="No existen movimientos registrados" 
                            EmptyDataRowStyle-ForeColor="Red" AllowPaging="True" PageSize="5"
                            CssClass="table table-bordered" 
                            DataSourceID="SqlDataSource3">
                            <Columns>
                                <asp:BoundField DataField="renglon" HeaderText="renglon" SortExpression="renglon" Visible="false" />
                                <asp:BoundField DataField="cantidad" HeaderText="Cantidad" SortExpression="cantidad" />
                                <asp:BoundField DataField="id_refaccion" HeaderText="Clave" SortExpression="id_refaccion" />
                                <asp:BoundField DataField="descripcion" HeaderText="Producto" SortExpression="descripcion" />
                                <asp:BoundField DataField="venta_unitaria" HeaderText="Precio Venta" SortExpression="venta_unitaria" DataFormatString="{0:C2}" />
                                <asp:BoundField DataField="importe" HeaderText="Importe" SortExpression="importe" DataFormatString="{0:C2}"/>
                                <asp:BoundField DataField="claveProductoSAT" HeaderText="Clave Prod/Serv" SortExpression="claveProductoSAT" />
                                <asp:BoundField DataField="claveUnidadSAT" HeaderText="Clave Unidad" SortExpression="claveUnidadSAT" />
                            </Columns>
                            <EmptyDataRowStyle ForeColor="Red" />
                            <SelectedRowStyle CssClass="alert-success-org" />
                        </asp:GridView>
                        <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" 
                            SelectCommand="SELECT V.renglon,V.cantidad,V.id_refaccion,V.descripcion,V.venta_unitaria,V.importe,c.claveProductoSAT,c.claveUnidadSAT FROM venta_det V left join catproductos c on v.id_refaccion=c.idProducto WHERE V.ticket=@ticket">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="grdFactPend" Name="ticket" PropertyName="SelectedValue" />
                            </SelectParameters>
                        </asp:SqlDataSource>                                                            
                    </ContentTemplate>
                </asp:UpdatePanel>                        
            </div>
        </div>
        <div class="col-lg-2 col-sm-2 center"></div>
    </div>

</asp:Content>

