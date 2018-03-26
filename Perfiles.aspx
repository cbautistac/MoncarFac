<%@ Page Title="" Language="C#" MasterPageFile="~/Administracion.master" AutoEventWireup="true" CodeFile="Perfiles.aspx.cs" Inherits="Perfiles" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:ScriptManager ID="ScriptManager1" runat="server">
     </asp:ScriptManager>
     <asp:Panel ID="pnlContenido" CssClass="panelContenido centrado" runat="server">
        <h1 class="centrado textoCentrado colorMoncarAzul">Perfiles</h1>
         <asp:Panel ID="pnlCatalogos" runat="server" CssClass="panelCatalogos textoCentrado" ScrollBars="Auto">  
             <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table class="centrado">
                        <tr>                            
                            <td>
                                <asp:TextBox ID="txtAltaPerfil" runat="server" MaxLength="100" CssClass="ancho150px alineado"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="txtAltaPerfil_TextBoxWatermarkExtender" 
                                    runat="server" BehaviorID="txtAltaPerfil_TextBoxWatermarkExtender" 
                                    TargetControlID="txtAltaPerfil" WatermarkCssClass="ancho150px water" WatermarkText="Perfil" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Debe indicar el Perfil" CssClass="errores alineado" ValidationGroup="agrega" ControlToValidate="txtAltaPerfil" Text="*" ></asp:RequiredFieldValidator>
                            </td>
                         <td><asp:ImageButton ID="btnAgregar" runat="server" AlternateText="Agregar" CssClass="anchoOpcion"
                                    ImageUrl="~/img/agregar.png" ToolTip="Agregar" onclick="btnAgregar_Click" ValidationGroup="agrega" /></td>
                        </tr>
                    </table>   
                    <asp:GridView ID="grvPerfiles" runat="server" AutoGenerateColumns="False"  CssClass="centrado textoCentrado"
                         DataKeyNames="id_perfil" DataSourceID="SqlDataSource1" AllowPaging="True" GridLines="None"
                         AllowSorting="True" EmptyDataText="No existen Perfiles Registrados">
                         <Columns>
                             <asp:BoundField DataField="id_perfil" HeaderText="id_Perfil" ReadOnly="True" SortExpression="id_perfil" Visible="false" />
                             <asp:TemplateField HeaderText="Perfil" SortExpression="nombre_perfil">
                                 <EditItemTemplate>
                                     <asp:TextBox ID="txtPerfil" runat="server" Text='<%# Bind("nombre_perfil") %>' MaxLength="100" CssClass="ancho130px alineado"></asp:TextBox>
                                     <cc1:TextBoxWatermarkExtender ID="txtPerfil_TextBoxWatermarkExtender" 
                                    runat="server" BehaviorID="txtPerfil_TextBoxWatermarkExtender" 
                                    TargetControlID="txtPerfil" WatermarkCssClass="ancho130px water" WatermarkText="Perfil"/>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Debe indicar el Perfil" CssClass="errores alineado" ValidationGroup="edita" ControlToValidate="txtPerfil" Text="*"></asp:RequiredFieldValidator>                                
                                 </EditItemTemplate>
                                 <ItemTemplate>
                                     <asp:Label ID="Label1" runat="server" Text='<%# Bind("nombre_perfil") %>' ></asp:Label>
                                 </ItemTemplate>
                                 <ItemStyle CssClass="ancho150px" />
                             </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                 <EditItemTemplate>
                                     <asp:ImageButton ID="btnActualizar" runat="server" CausesValidation="True" 
                                         CommandName="Update" ImageUrl="~/img/guardar.png" AlternateText="Actualizar" ValidationGroup="edita" ToolTip="Guardar"/>
                                     &nbsp;<asp:ImageButton ID="btnCancelar" runat="server" CausesValidation="False" 
                                         CommandName="Cancel" ImageUrl="~/img/cancelar.png" AlternateText="Cancelar" ToolTip="Cancelar"/>
                                 </EditItemTemplate>
                                 <ItemTemplate>
                                     <asp:ImageButton ID="btnEditar" runat="server" CausesValidation="False" 
                                         CommandName="Edit" ImageUrl="~/img/editar.png" AlternateText="Editar"  ToolTip="Editar"/>
                                 </ItemTemplate>
                             </asp:TemplateField>
                             <asp:TemplateField ShowHeader="False">
                                <EditItemTemplate>
                                    <asp:Label ID="Label3" runat="server" Text=" "></asp:Label>
                                </EditItemTemplate>
                                 <ItemTemplate>
                                     <asp:ImageButton ID="btnEliminar" runat="server" CausesValidation="False" 
                                         CommandName="Delete" ImageUrl="~/img/eliminar.png" AlternateText="Eliminar" ToolTip="Eliminar" OnClientClick="return confirm('¿Esta seguro de eliminar el Perfil?');" />
                                 </ItemTemplate>
                             </asp:TemplateField>                                  
                         </Columns>
                         <EditRowStyle CssClass="editarFila" />
                         <EmptyDataRowStyle CssClass="errores" />
                         <HeaderStyle CssClass="encabezadoTabla" />                                                  
                         <AlternatingRowStyle CssClass="alterTable" />                         
                         <PagerStyle CssClass="paginador" HorizontalAlign="Center" VerticalAlign="Middle" />                         
                         <SelectedRowStyle CssClass="selectFila" />
                         <SortedAscendingHeaderStyle CssClass="encabezadoSortAsc" />
                         <SortedDescendingHeaderStyle CssClass="encabezadoSortDesc" />                         
                     </asp:GridView>
                     <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="agrega" CssClass="errores" DisplayMode="List"/>
                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="edita" CssClass="errores" DisplayMode="List"/>
                    <asp:Label ID="lblError" runat="server" CssClass="errores"></asp:Label>
                     <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                         ConnectionString="<%$ ConnectionStrings:PVW %>" 
                         SelectCommand="SELECT id_perfil, nombre_perfil  FROM Perfiles" 
                         UpdateCommand="UPDATE Perfiles SET nombre_perfil=@nombre_perfil WHERE (id_perfil = @id_perfil)"
                         DeleteCommand="DELETE FROM Perfiles where id_perfil=@id_perfil" 
                         InsertCommand="insert into perfiles values(isnull((select top 1 id_perfil from perfiles order by id_perfil desc),0)+1, @nombre_perfil)">
                         <InsertParameters>
                             <asp:ControlParameter ControlID="txtAltaPerfil" Type="String" Name="nombre_perfil" />                             
                         </InsertParameters>
                         <UpdateParameters>
                             <asp:Parameter Name="nombre_perfil" />                            
                         </UpdateParameters>
                         <DeleteParameters>
                            <asp:Parameter Name="id_perfil" />
                         </DeleteParameters>
                     </asp:SqlDataSource>                    

                     <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1" >
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

