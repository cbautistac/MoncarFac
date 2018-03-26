<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Usuarios.aspx.cs" Inherits="Usuarios" MasterPageFile="~/Administracion.master"%>

 <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %> 

 <asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
     <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true"/>
     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
         <ContentTemplate>             
            <div class="row">
                <div class="col-lg-12 col-sm-12 text-center alert-info">
                    <h3>
                        <i class="fa fa-users"></i>&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="Label1" runat="server" Text="Usuarios"></asp:Label>                        
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
                <div class="row">
                    <div class="col-lg-1 col-sm-1 text-left"><asp:Label ID="Label4" runat="server" Text="Usuario:" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-2 col-sm-2 text-left">
                        <asp:TextBox ID="txtNick" runat="server" MaxLength="15" CssClass="alingMiddle input-medium"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtNick_TextBoxWatermarkExtender" runat="server" BehaviorID="txtNick_TextBoxWatermarkExtender" TargetControlID="txtNick" WatermarkCssClass="input-medium water" WatermarkText="Usuario" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Debe indicar el usuario" CssClass="errores" ValidationGroup="agrega" ControlToValidate="txtNick" Text="*" ></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" Text="*" ErrorMessage="El usuario debe contener de entre 3 y 20 caracteres." CssClass="errores" ControlToValidate="txtNick" ValidationExpression="[a-zA-Z0-9]{3,20}" ValidationGroup="agrega" />
                    </div>
                    <div class="col-lg-1 col-sm-1 text-left"><asp:Label ID="Label5" runat="server" Text="Contraseña:" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-2 col-sm-2 text-left">
                        <asp:TextBox ID="txtPass" runat="server" MaxLength="15" CssClass="input-medium alingMiddle"></asp:TextBox>                                
                        <cc1:TextBoxWatermarkExtender ID="txtPass_TextBoxWatermarkExtender" runat="server" BehaviorID="txtPass_TextBoxWatermarkExtender" TargetControlID="txtPass" WatermarkCssClass="input-medium water" WatermarkText="Contraseña"/>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Debe indicar la Contraseña" CssClass="errores " ValidationGroup="agrega" ControlToValidate="txtPass" Text="*"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" Text="*" ValidationGroup="agrega" ErrorMessage="La contraseña debe contener de entre 5 y 20 caracteres." CssClass="errores" ControlToValidate="txtPass" ValidationExpression="[a-zA-Z0-9]{5,20}" />
                    </div>
                    <div class="col-lg-1 col-sm-1 text-left"><asp:Label ID="Label6" runat="server" Text="Nombre Usuario:" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-3 col-sm-4 text-left">
                        <asp:TextBox ID="txtNombreAlta" runat="server" MaxLength="200" CssClass="input-large alingMiddle"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtNombreAlta_TextBoxWatermarkExtender" runat="server" BehaviorID="txtNombreAlta_TextBoxWatermarkExtender" TargetControlID="txtNombreAlta" WatermarkCssClass="input-large water" WatermarkText="Nombre Usuario"/>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Debe indicar el nombre del Usuario" CssClass="errores" ValidationGroup="agrega" ControlToValidate="txtNombreAlta" Text="*"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-lg-1 col-sm-1 text-left">
                        <asp:LinkButton ID="btnAgregar" runat="server" CssClass="btn btn-info t14" onclick="btnAgregar_Click" ToolTip="Agregar" ValidationGroup="agrega" ><i class="fa fa-plus-circle"></i>&nbsp;<span>Agregar</span></asp:LinkButton>
                    </div>
                </div>
                <br />
                <asp:Panel ID="Panel4" runat="server" CssClass="panelCatalogos text-center" ScrollBars="Auto">
                    <div class="table-responsive">
                     <asp:GridView ID="grvUsuarios" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered" DataKeyNames="id_usuario" DataSourceID="SqlDataSource1" 
                        AllowPaging="True" AllowSorting="True" EmptyDataText="No existen Usuarios Registrados" onrowcancelingedit="grvUsuarios_RowCancelingEdit" PageSize="7"
                        onrowcommand="grvUsuarios_RowCommand" OnRowUpdating="grvUsuarios_RowUpdating" onrowediting="grvUsuarios_RowEditing" OnPageIndexChanging="grvUsuarios_PageIndexChanging" 
                        onrowdatabound="grvUsuarios_RowDataBound" onsorting="grvUsuarios_Sorting">
                         <Columns>
                             <asp:BoundField DataField="id_usuario" HeaderText="id_usuario" ReadOnly="True" SortExpression="id_usuario" Visible="false" />
                             <asp:BoundField DataField="nick" HeaderText="Usuario" SortExpression="nick" ReadOnly="True" ControlStyle-CssClass="ancho100px" />
                             <asp:TemplateField HeaderText="Contraseña" SortExpression="contrasena">
                                 <EditItemTemplate>
                                     <asp:TextBox ID="txtContrasena" runat="server" Text='<%# Bind("contrasena") %>' MaxLength="15" CssClass="input-medium alingMiddle" ></asp:TextBox>
                                     <cc1:TextBoxWatermarkExtender ID="txtContrasena_TextBoxWatermarkExtender" runat="server" BehaviorID="txtContrasena_TextBoxWatermarkExtender" TargetControlID="txtContrasena" WatermarkCssClass="input-medium water" WatermarkText="Contraseña"/>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Debe indicar la Contraseña" CssClass="errores" ValidationGroup="edita" ControlToValidate="txtContrasena" Text="*"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Text="*" ValidationGroup="edita" ErrorMessage="La contraseña debe contener de entre 5 y 20 caracteres." CssClass="errores" ControlToValidate="txtContrasena" ValidationExpression="[a-zA-Z0-9]{5,20}" />
                                 </EditItemTemplate>
                                 <ItemTemplate>
                                     <asp:Label ID="Label1" runat="server" Text='<%# Bind("contrasena") %>'></asp:Label>
                                 </ItemTemplate>                                 
                             </asp:TemplateField>
                             <asp:TemplateField HeaderText="Nombre de Usuario" SortExpression="nombre_usuario">
                                 <EditItemTemplate>
                                     <asp:TextBox ID="txtNombre" runat="server" Text='<%# Bind("nombre_usuario") %>' MaxLength="200" CssClass="input-large alingMiddle"></asp:TextBox>
                                     <cc1:TextBoxWatermarkExtender ID="txtNombre_TextBoxWatermarkExtender" runat="server" BehaviorID="txtNombre_TextBoxWatermarkExtender" TargetControlID="txtNombre" WatermarkCssClass="input-large water" WatermarkText="Nombre Usuario"/>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Debe indicar el nombre del Usuario" CssClass="errores" ValidationGroup="edita" ControlToValidate="txtNombre" Text="*"></asp:RequiredFieldValidator>
                                 </EditItemTemplate>
                                 <ItemTemplate>
                                     <asp:Label ID="Label2" runat="server" Text='<%# Bind("nombre_usuario") %>'></asp:Label>
                                 </ItemTemplate>                                 
                             </asp:TemplateField>
                             <asp:BoundField DataField="estatus_usuario" HeaderText="estatus_usuario" SortExpression="estatus_usuario" Visible="False" />
                             <asp:TemplateField ShowHeader="False">
                                 <EditItemTemplate>
                                     <asp:LinkButton ID="btnActualizar" runat="server" CausesValidation="True" CommandName="Update" ToolTip="Guardar" ValidationGroup="edita" CssClass="btn btn-success t14"><i class="fa fa-save"></i></asp:LinkButton>
                                 </EditItemTemplate>
                                 <ItemTemplate>
                                     <asp:LinkButton ID="btnEditar" runat="server" CausesValidation="False" CommandName="Edit" ToolTip="Editar" CssClass="btn btn-warning t14"><i class="fa fa-edit"></i></asp:LinkButton>                                     
                                 </ItemTemplate>
                             </asp:TemplateField>
                             <asp:TemplateField ShowHeader="False">
                                <EditItemTemplate>
                                    <asp:LinkButton ID="btnCancelar" runat="server" CausesValidation="False" CommandName="Cancel" ToolTip="Cancelar" CssClass="btn btn-danger t14"><i class="fa fa-times-circle"></i></asp:LinkButton>
                                </EditItemTemplate>
                                 <ItemTemplate>
                                     <asp:LinkButton ID="btnEliminar" runat="server" CausesValidation="False" CommandName="Delete" ToolTip="Eliminar" CssClass="btn btn-danger t14" OnClientClick="return confirm('¿Esta seguro de eliminar el Usuario?');"><i class="fa fa-trash"></i></asp:LinkButton>                                     
                                 </ItemTemplate>
                             </asp:TemplateField>
                             <asp:TemplateField>
                                 <EditItemTemplate></EditItemTemplate>
                                 <ItemTemplate>
                                     <asp:LinkButton ID="btnAlta" runat="server" CausesValidation="False" CommandName="alta" ToolTip="Alta" CssClass="btn btn-success t14" CommandArgument='<%# Bind("id_usuario")%>'><i class="fa fa-toggle-up"></i>&nbsp;<span>Activa</span></asp:LinkButton>                                     
                                     <asp:LinkButton ID="btnBaja" runat="server" CausesValidation="False" CommandName="baja" ToolTip="Baja" CssClass="btn btn-danger t14" CommandArgument='<%# Bind("id_usuario")%>'><i class="fa fa-toggle-down"></i>&nbsp;<span>Inactiva</span></asp:LinkButton>                                                                          
                                 </ItemTemplate>
                             </asp:TemplateField>
                         </Columns>
                         <EditRowStyle CssClass="alert-warning" />
                         <EmptyDataRowStyle CssClass="errores" />
                         <SelectedRowStyle CssClass="alert-success" />                         
                     </asp:GridView>
                     <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                         ConnectionString="<%$ ConnectionStrings:PVW %>"                          
                         SelectCommand="SELECT id_usuario, nick, contrasena, nombre_usuario FROM Usuarios where nick<>'SUPERVISOR'" 
                         UpdateCommand="UPDATE Usuarios SET contrasena = @contrasena, nombre_usuario = @nombre_usuario WHERE (id_usuario = @id_usuario)"
                         DeleteCommand="DELETE FROM Usuarios where id_usuario=@id_usuario" 
                         InsertCommand="insert into usuarios values(isnull((select top 1 id_usuario from usuarios order by id_usuario desc),0)+1, UPPER(@nick),@contrasena,@nombre_usuario,'A','I')">
                         <InsertParameters>
                             <asp:ControlParameter ControlID="txtNick" Type="String" Name="nick" />
                             <asp:ControlParameter ControlID="txtPass" Type="String" Name="contrasena" />
                             <asp:ControlParameter ControlID="txtNombreAlta" Type="String" Name="nombre_usuario" />
                         </InsertParameters>
                         <UpdateParameters>
                             <asp:Parameter Name="contrasena" />
                             <asp:Parameter Name="nombre_usuario" />
                             <asp:Parameter Name="id_usuario" />
                         </UpdateParameters>
                         <DeleteParameters>
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