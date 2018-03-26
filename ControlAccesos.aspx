<%@ Page Title="" Language="C#" MasterPageFile="~/Administracion.master" AutoEventWireup="true" CodeFile="ControlAccesos.aspx.cs" Inherits="ControlAccesos" %>

 <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %> 

 <asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
     <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true"/>
     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
         <ContentTemplate>             
            <div class="row">
                <div class="col-lg-12 col-sm-12 text-center alert-info">
                    <h3>
                        <i class="fa fa-users"></i>&nbsp;<i class="fa fa-lock"></i>&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="Label1" runat="server" Text="Control de Usuarios"></asp:Label>                        
                    </h3>                    
                </div>
            </div>
             <div class="row pad1m">
                <div class="col-lg-12 col-sm-12 text-center"> 
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="agrega" CssClass="errores" DisplayMode="List"/>
                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="edita" CssClass="errores" DisplayMode="List"/>
                    <asp:Label ID="lblError" runat="server" CssClass="errores"></asp:Label>
                </div>
            </div>
            <asp:Panel runat="server" ID="pnlDaños" CssClass="paneles col-lg-12 col-sm-12" ScrollBars="Auto">  
                <asp:Panel ID="Panel4" runat="server" CssClass="panelCatalogos text-center" ScrollBars="Auto">
                    <div class="table-responsive">
                        <asp:GridView ID="grvUsuarios" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered" EmptyDataRowStyle-CssClass="errores"
                            AllowPaging="True" AllowSorting="True" EmptyDataText="No existen usuarios dentro del sistema" PageSize="7" DataKeyNames="id_usuario" DataSourceID="SqlDataSource1">
                            <Columns>
                                <asp:BoundField DataField="id_usuario" Visible="false" HeaderText="id_usuario" ReadOnly="True" SortExpression="id_usuario"></asp:BoundField>
                                <asp:BoundField DataField="nick" HeaderText="Usuario Activo" SortExpression="nick"></asp:BoundField>
                                <asp:BoundField DataField="estatus_sistema" Visible="false" HeaderText="estatus_sistema" SortExpression="estatus_sistema"></asp:BoundField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkCerrarSesionUss" runat="server" CommandArgument='<%# Eval("id_usuario") %>' OnClick="lnkCerrarSesionUss_Click" CssClass="btn btn-info t14" ToolTip="Cerrar Sesión Usuario"><i class="fa fa-sign-out"></i><span>&nbsp;Cerrar Sesi&oacute;n</span></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:PVW %>' SelectCommand="SELECT [id_usuario], [nick], [estatus_sistema] FROM [Usuarios] WHERE id_usuario>1 and ([estatus_sistema] = @estatus_sistema)">
                            <SelectParameters>
                                <asp:Parameter DefaultValue="A" Name="estatus_sistema" Type="String"></asp:Parameter>
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                </asp:Panel>
            </asp:Panel>
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
 </asp:Content>