<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Empleados.aspx.cs" Inherits="Empleados" MasterPageFile="~/Administracion.master" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
     </asp:ScriptManager>
     <asp:Panel ID="pnlContenido" CssClass="panelContenido centrado" runat="server">
        <h1 class="centrado textoCentrado colorMoncarAzul">Empleados</h1>
         <asp:Panel ID="pnlCatalogos" runat="server" CssClass="panelCatalogos textoCentrado" ScrollBars="Auto">    
             <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>  
                    <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="idEmp" DataSourceID="SqlDataSource1" GridLines="None" EmptyDataText="No existen Empleados registrados" CssClass="ancho100 textoCentrado" PageSize="20">
                        <Columns>
                            <asp:BoundField DataField="idEmp" HeaderText="idEmp" ReadOnly="True" SortExpression="idEmp" Visible="false" />                            
                            <asp:BoundField DataField="Llave_nombre_empleado" HeaderText="Empleado" SortExpression="Llave_nombre_empleado" />
                            <asp:BoundField DataField="puesto" HeaderText="puesto" SortExpression="puesto" Visible="false"/>
                            <asp:BoundField DataField="descripcion" HeaderText="Puesto" SortExpression="puesto" />                            
                        </Columns>
                        <EditRowStyle CssClass="editarFila" />
                         <EmptyDataRowStyle CssClass="errores alert-danger" />
                         <HeaderStyle CssClass="encabezadoTabla" />                                                  
                         <AlternatingRowStyle CssClass="alterTable" />                         
                         <PagerStyle CssClass="paginador" HorizontalAlign="Center" VerticalAlign="Middle" />                         
                         <SelectedRowStyle CssClass="selectFila" />
                         <SortedAscendingHeaderStyle CssClass="encabezadoSortAsc" />
                         <SortedDescendingHeaderStyle CssClass="encabezadoSortDesc" />
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:RecursosHumanos %>" SelectCommand="select e.idEmp,e.Llave_nombre_empleado,e.puesto,p.descripcion from empleados e inner join catalogo_puestos p on p.clv_puesto=e.puesto"></asp:SqlDataSource>
                    <asp:Label ID="lblError" runat="server" CssClass="errores alert-danger"></asp:Label>         
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                        <ProgressTemplate>
                            <div class="mask zen3"> 
                                <div class="carga zen4">
                                    <asp:Image ID="Cargando" runat="server" ImageUrl="~/img/eapps2.gif"  Width="100%" />
                                </div>
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </ContentTemplate>
            </asp:UpdatePanel>
     </asp:Panel>
         </asp:Panel>
    
</asp:Content>


