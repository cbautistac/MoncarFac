<%@ Page Title="" Language="C#" MasterPageFile="~/Administracion.master" AutoEventWireup="true" CodeFile="Usuarios_Perfiles.aspx.cs" Inherits="Usuarios_Perfiles" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true"/>
     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
         <ContentTemplate>             
            <div class="row">
                <div class="col-lg-12 col-sm-12 text-center alert-info">
                    <h3>
                        <i class="fa fa-street-view"></i>&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="Label1" runat="server" Text="Usuarios Perfiles"></asp:Label>                        
                    </h3>                    
                </div>
            </div>
             <div class="row pad1m">
                <div class="col-lg-12 col-sm-12 text-center"> 
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="agrega" CssClass="errores" DisplayMode="List"/>                    
                    <asp:Label ID="lblError" runat="server" CssClass="errores"></asp:Label>
                </div>
            </div>
            <asp:Panel runat="server" ID="pnlContet" CssClass="paneles col-lg-12 col-sm-12" ScrollBars="Auto">  
                <div class="row">
                    <div class="col-lg-1 col-sm-1 text-left"><asp:Label ID="Label4" runat="server" Text="Usuario:" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-3 col-sm-3 text-left">
                        <asp:DropDownList ID="ddlUsuario" runat="server" DataTextField="nick" DataValueField="id_usuario" DataSourceID="SqlDataSource2" AutoPostBack="true" CssClass="alingMiddle input-large" ></asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSource2" runat="server"  ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select u.id_usuario,nick from Usuarios u where (select count(*) from Usuarios_Perfiles where id_usuario=u.id_usuario)<>(select count(*) from perfiles) and u.id_usuario<>1"></asp:SqlDataSource>        
                    </div>
                    <div class="col-lg-1 col-sm-1 text-left"><asp:Label ID="Label5" runat="server" Text="Perfil:" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-3 col-sm-3 text-left">
                        <asp:DropDownList ID="ddlPerfil" runat="server" DataTextField="nombre_perfil" DataValueField="id_perfil" DataSourceID="SqlDataSource3" AutoPostBack="true" CssClass="alingMiddle input-large"></asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select p.id_perfil,p.nombre_perfil from Perfiles p where (select count(*) from Usuarios_Perfiles where id_usuario=@id_usuario)<>(select count(*) from perfiles) and p.id_perfil not in(select id_perfil from Usuarios_Perfiles where id_usuario=@id_usuario)">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="ddlUsuario" Type="Int32" Name="id_usuario" PropertyName="SelectedValue" />
                            </SelectParameters>
                        </asp:SqlDataSource>    
                    </div>                    
                    <div class="col-lg-1 col-sm-1 text-left">                        
                        <asp:LinkButton ID="btnAgregar" runat="server" CssClass="btn btn-info t14" onclick="btnAgregar_Click" ToolTip="Agregar" ValidationGroup="agrega" ><i class="fa fa-plus-circle"></i>&nbsp;<span>Agregar</span></asp:LinkButton>
                    </div>
                </div>
                <br />
                <asp:Panel ID="Panel1" runat="server" CssClass="panelCatalogos text-center" ScrollBars="Auto">
                    <div class="table-responsive">
                        <asp:GridView ID="grvUPerfiles" runat="server" AutoGenerateColumns="False"  CssClass="table table-bordered" DataKeyNames="id_usuario, id_perfil" DataSourceID="SqlDataSource1" AllowPaging="True" PageSize="7"  AllowSorting="True" EmptyDataText="No existen Perfiles Asignados" OnRowCommand="grvUPerfiles_RowCommand">
                             <Columns>
                                 <asp:BoundField DataField="nick" HeaderText="Usuario" ReadOnly="True" SortExpression="id_usuario" />
                                 <asp:BoundField DataField="nombre_perfil" HeaderText="Perfil" ReadOnly="True" SortExpression="id_perfil" />                                                      
                                  <asp:TemplateField ShowHeader="False">
                                    <EditItemTemplate>
                                    </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEliminar" runat="server" CausesValidation="False" CommandArgument='<%# Eval("id_usuario")+";"+Eval("id_perfil") %>' CommandName="Delete" ToolTip="Eliminar" CssClass="btn btn-danger t14" OnClientClick="return confirm('¿Esta seguro de eliminar el Usuario?');"><i class="fa fa-trash"></i></asp:LinkButton>                                                                                 
                                        </ItemTemplate>
                                 </asp:TemplateField>                                  
                             </Columns>                             
                             <EmptyDataRowStyle CssClass="errores" />                                               
                         </asp:GridView>                     
                         <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" 
                             SelectCommand="select up.id_usuario, u.nick, p.nombre_perfil, up.id_perfil from usuarios_perfiles up inner join usuarios u on u.id_usuario=up.id_usuario inner join Perfiles p on p.id_perfil=up.id_perfil where u.id_usuario<>1"                          
                             DeleteCommand="DELETE FROM Usuarios_Perfiles where id_usuario=@id_usuario and id_perfil=@id_perfil" 
                             InsertCommand="insert into usuarios_perfiles values(@id_usuario, @id_perfil)">
                             <InsertParameters>
                                 <asp:ControlParameter ControlID="ddlUsuario" Type="Int32" Name="id_usuario" />                             
                                 <asp:ControlParameter ControlID="ddlPerfil" Type="Int32" Name="id_perfil" />
                             </InsertParameters>                         
                             <DeleteParameters>
                                <asp:Parameter Name="id_perfil" />
                                <asp:Parameter Name="id_usuario" />
                             </DeleteParameters>
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

