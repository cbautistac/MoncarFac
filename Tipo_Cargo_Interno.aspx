<%@ Page Title="" Language="C#" MasterPageFile="~/Administracion.master" AutoEventWireup="true" CodeFile="Tipo_Cargo_Interno.aspx.cs" Inherits="Tipo_Cargo_Interno" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:ScriptManager ID="ScriptManager1" runat="server">
     </asp:ScriptManager>
     <asp:Panel ID="pnlContenido" CssClass="panelContenido centrado" runat="server">
        <h1 class="centrado textoCentrado colorMoncarAzul">Tipos Cargo Interno</h1>
         <asp:Panel ID="pnlCatalogos" runat="server" CssClass="panelCatalogos textoCentrado" ScrollBars="Auto">  
             <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table class="centrado">
                        <tr>                            
                            <td>
                                <asp:TextBox ID="txtAltaCargo" runat="server" MaxLength="50" CssClass="ancho150px alineado"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="txtAltacargo_TextBoxWatermarkExtender" 
                                    runat="server" BehaviorID="txtAltaCargo_TextBoxWatermarkExtender" 
                                    TargetControlID="txtAltaCargo" WatermarkCssClass="ancho150px water" WatermarkText="Tipo Cargo" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Debe indicar el Tipo de Cargo" CssClass="errores alineado" ValidationGroup="agrega" ControlToValidate="txtAltaCargo" Text="*" ></asp:RequiredFieldValidator>
                            </td>
                         <td><asp:ImageButton ID="btnAgregar" runat="server" AlternateText="Agregar" CssClass="anchoOpcion"
                                    ImageUrl="~/img/agregar.png" ToolTip="Agregar" onclick="btnAgregar_Click" ValidationGroup="agrega" /></td>
                        </tr>
                    </table>   
                    <asp:GridView ID="grvCargos" runat="server" AutoGenerateColumns="False"  CssClass="centrado textoCentrado"
                         DataKeyNames="id_tipo_ci" DataSourceID="SqlDataSource1" AllowPaging="True" GridLines="None"
                         AllowSorting="True" EmptyDataText="No existen Tipos de Cargos Internos Registrados">
                         <Columns>
                             <asp:BoundField DataField="id_tipo_ci" HeaderText="id_ci" ReadOnly="True" SortExpression="id_tipo_ci" Visible="false" />
                             <asp:TemplateField HeaderText="Tipo Cargo" SortExpression="descripcion">
                                 <EditItemTemplate>
                                     <asp:TextBox ID="txtCargo" runat="server" Text='<%# Bind("descripcion") %>' MaxLength="50" CssClass="ancho130px alineado"></asp:TextBox>
                                     <cc1:TextBoxWatermarkExtender ID="txtCargo_TextBoxWatermarkExtender" 
                                    runat="server" BehaviorID="txtCargo_TextBoxWatermarkExtender" 
                                    TargetControlID="txtCargo" WatermarkCssClass="ancho130px water" WatermarkText="Tipo Cargo"/>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Debe indicar el Tipo de Cargo" CssClass="errores alineado" ValidationGroup="edita" ControlToValidate="txtCargo" Text="*"></asp:RequiredFieldValidator>
                                
                                 </EditItemTemplate>
                                 <ItemTemplate>
                                     <asp:Label ID="Label1" runat="server" Text='<%# Bind("descripcion") %>'></asp:Label>
                                 </ItemTemplate>
                                 <ItemStyle CssClass="ancho130px" />
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
                                         CommandName="Delete" ImageUrl="~/img/eliminar.png" AlternateText="Eliminar" ToolTip="Eliminar" OnClientClick="return confirm('¿Esta seguro de eliminar el Tipo de Cargo?');" />
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
                         SelectCommand="SELECT id_tipo_ci, descripcion FROM Tipo_Cargo_Interno" 
                         UpdateCommand="UPDATE Tipo_Cargo_Interno SET descripcion=@descripcion WHERE (id_tipo_ci = @id_tipo_ci)"
                         DeleteCommand="DELETE FROM Tipo_Cargo_Interno where id_tipo_ci=@id_tipo_ci" 
                         InsertCommand="insert into Tipo_Cargo_Interno values(isnull((select top 1 id_tipo_ci from Tipo_Cargo_Interno order by id_tipo_ci desc),0)+1, @descripcion)">
                         <InsertParameters>
                             <asp:ControlParameter ControlID="txtAltaCargo" Type="String" Name="descripcion" />                             
                         </InsertParameters>
                         <UpdateParameters>
                             <asp:Parameter Name="descripcion" />                            
                         </UpdateParameters>
                         <DeleteParameters>
                            <asp:Parameter Name="id_tipo_ci" />
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




