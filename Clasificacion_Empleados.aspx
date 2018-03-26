<%@ Page Title="" Language="C#" MasterPageFile="~/Administracion.master" AutoEventWireup="true" CodeFile="Clasificacion_Empleados.aspx.cs" Inherits="Clasificacion_Empleados" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:ScriptManager ID="ScriptManager1" runat="server">
     </asp:ScriptManager>
     <asp:Panel ID="pnlContenido" CssClass="panelContenido centrado" runat="server">
        <h1 class="centrado textoCentrado colorMoncarAzul">Clasificación Empleados</h1>
         <asp:Panel ID="pnlCatalogos" runat="server" CssClass="panelCatalogos textoCentrado" ScrollBars="Auto">  
             <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table class="centrado">
                        <tr>                            
                            <td>
                                <asp:TextBox ID="txtAltaClasificacion" runat="server" MaxLength="200" CssClass="ancho150px alineado"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="txtAltaClasificacion_TextBoxWatermarkExtender" 
                                    runat="server" BehaviorID="txtAltaClasificacion_TextBoxWatermarkExtender" 
                                    TargetControlID="txtAltaClasificacion" WatermarkCssClass="ancho150px water" WatermarkText="Clasificación Empleado" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Debe indicar la Clasificación del Empleado" CssClass="errores alineado" ValidationGroup="agrega" ControlToValidate="txtAltaClasificacion" Text="*" ></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlIE" runat="server" ValidationGroup="agrega">
                                    <asp:ListItem Value="I">Interno</asp:ListItem>
                                    <asp:ListItem Value="E">Externo</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                         <td><asp:ImageButton ID="btnAgregar" runat="server" AlternateText="Agregar" CssClass="anchoOpcion"
                                    ImageUrl="~/img/agregar.png" ToolTip="Agregar" onclick="btnAgregar_Click" ValidationGroup="agrega" /></td>
                        </tr>
                    </table>   
                    <asp:GridView ID="grvClasificacion" runat="server" AutoGenerateColumns="False"  CssClass="centrado textoCentrado"
                         DataKeyNames="id_clasificacion_emp" DataSourceID="SqlDataSource1" AllowPaging="True" GridLines="None"
                         AllowSorting="True" EmptyDataText="No existen Clasificaciones de Empleados Registradas">
                         <Columns>
                             <asp:BoundField DataField="id_clasificacion_emp" HeaderText="id_clasificacion" ReadOnly="True" SortExpression="id_clasificacion_emp" Visible="false" />
                             <asp:TemplateField HeaderText="Clasificación Empleado" SortExpression="descripcion">
                                 <EditItemTemplate>
                                     <asp:TextBox ID="txtC" runat="server" Text='<%# Bind("descripcion") %>' MaxLength="200" CssClass="ancho130px alineado"></asp:TextBox>
                                     <cc1:TextBoxWatermarkExtender ID="txtC_TextBoxWatermarkExtender" 
                                    runat="server" BehaviorID="txtC_TextBoxWatermarkExtender" 
                                    TargetControlID="txtC" WatermarkCssClass="ancho130px water" WatermarkText="Clasificación Empleado"/>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Debe indicar la Clasificación del Empleado" CssClass="errores alineado" ValidationGroup="edita" ControlToValidate="txtC" Text="*" ></asp:RequiredFieldValidator>                                
                                 </EditItemTemplate>
                                 <ItemTemplate>
                                     <asp:Label ID="Label1" runat="server" Text='<%# Bind("descripcion") %>' ></asp:Label>
                                 </ItemTemplate>
                                 <ItemStyle CssClass="ancho150px" />
                             </asp:TemplateField>
                             <asp:TemplateField HeaderText="Interno/Externo" SortExpression="int_ext">
                                 <EditItemTemplate>
                                   <asp:DropDownList ID="ddlIE" runat="server" ValidationGroup="agrega" 
                                         SelectedValue='<%# Bind("int_ext") %>'>
                                    <asp:ListItem Value="I">Interno</asp:ListItem>
                                    <asp:ListItem Value="E">Externo</asp:ListItem>
                                </asp:DropDownList>  
                                    
                                 </EditItemTemplate>
                                 <ItemTemplate>
                                     <asp:Label ID="Label2" runat="server" Text='<%# Bind("descripcion_int_ext") %>' ></asp:Label>
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
                                         CommandName="Delete" ImageUrl="~/img/eliminar.png" AlternateText="Eliminar" ToolTip="Eliminar" OnClientClick="return confirm('¿Esta seguro de eliminar la Clasificación del Empleado?');" />
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
                         SelectCommand="SELECT id_clasificacion_emp, descripcion,int_ext, case int_ext when 'I' then 'Interno' else 'Externo' end as descripcion_int_ext  FROM Clasificacion_Empleados" 
                         UpdateCommand="UPDATE clasificacion_empleados  SET descripcion=@descripcion, int_ext=@int_ext WHERE (id_clasificacion_emp = @id_clasificacion_emp)"
                         DeleteCommand="DELETE FROM clasificacion_empleados where id_clasificacion_emp=@id_clasificacion_emp" 
                         InsertCommand="insert into clasificacion_empleados values(isnull((select top 1 id_clasificacion_emp from Clasificacion_empleados order by id_clasificacion_emp desc),0)+1, @descripcion, @int_ext)">
                         <InsertParameters>
                             <asp:ControlParameter ControlID="txtAltaClasificacion" Type="String" Name="descripcion" />                             
                             <asp:ControlParameter ControlID="ddlIE" Type="Char" Name="int_ext" />                             
                         </InsertParameters>
                         <UpdateParameters>
                             <asp:Parameter Name="descripcion" />
                             <asp:Parameter Name="int_ext" />                            
                         </UpdateParameters>
                         <DeleteParameters>
                            <asp:Parameter Name="id_clasificacion_emp" />
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



