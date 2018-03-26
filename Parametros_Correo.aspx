<%@ Page Title="" Language="C#" MasterPageFile="~/Administracion.master" AutoEventWireup="true" CodeFile="Parametros_Correo.aspx.cs" Inherits="Parametros_Correo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-lg-12 col-sm-12 text-center alert-info">
                    <h3>
                        <i class="fa fa-file-text-o"></i>&nbsp;
                <i class="fa fa-at"></i>&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label1" runat="server" Text="Parametros Email"></asp:Label>
                    </h3>
                </div>
            </div>
            <div class="row pad1m">
                <div class="col-lg-12 col-sm-12 text-center">
                    <asp:Label ID="lblError" runat="server" CssClass="errores alert-danger" />
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="errores alert-danger" DisplayMode="List" ValidationGroup="edita" />
                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" CssClass="errores alert-danger" DisplayMode="List" ValidationGroup="agrega" />
                    <asp:Label ID="lblErrores" runat="server" CssClass="errores alert-danger" />
                </div>
            </div>
            <asp:Panel ID="pnlCatalogos" runat="server" CssClass="col-lg-12 col-sm-12" ScrollBars="Auto">
                <table class="centrado">
                    <tr class="alineado">
                        <td>
                            <asp:TextBox ID="txtUsuario" runat="server" placeholder="Usuario" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" ControlToValidate="txtUsuario" runat="server" ErrorMessage="Necesita colocar el Usuario" CssClass="errores alert-danger" Text="*" ValidationGroup="agrega" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtContraseña" runat="server" placeholder="Contraseña" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ControlToValidate="txtContraseña" runat="server" ErrorMessage="Necesita colocar la Contraseña" CssClass="errores alert-danger" Text="*" ValidationGroup="agrega" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtHost" runat="server" placeholder="Host" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" ControlToValidate="txtHost" runat="server" ErrorMessage="Necesita colocar el Host" CssClass="errores alert-danger" Text="*" ValidationGroup="agrega" />

                        </td>
                        <td rowspan="2">
                            <asp:LinkButton ID="btnNuevoParametro" runat="server" CssClass="btn btn-info t14" OnClick="btnNuevoParametro_Click" ToolTip="Agregar" ValidationGroup="agrega"><i class="fa fa-plus-circle"></i>&nbsp;<span>Agregar</span></asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtPuerto" runat="server" placeholder="Puerto" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" ControlToValidate="txtPuerto" runat="server" ErrorMessage="Necesita colocar el Puerto" CssClass="errores alert-danger" Text="*" ValidationGroup="agrega" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlTipoServidor" runat="server" CssClass="input-medium">
                                <asp:ListItem Text="SMTP" Value="1" Selected="True" />
                                <asp:ListItem Text="POP3" Value="2" />
                                <asp:ListItem Text="IMAP" Value="3" />
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkSSLHabilitado" runat="server" Checked="false" Text="Habilitar" AutoPostBack="true" OnCheckedChanged="chkSSLHabilitado_CheckedChanged" />
                            <asp:Label ID="lblHabilitado" runat="server" Visible="false" />
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="GridParametros" runat="server" AutoGenerateColumns="False"
                    CssClass="table table-bordered" AllowPaging="True" AllowSorting="True"
                    EmptyDataText="No existe parametros para mostrar" DataKeyNames="clave" DataSourceID="SqlDataSource1">
                    <Columns>
                        <asp:TemplateField HeaderText="clave" InsertVisible="False" SortExpression="clave" Visible="false">
                            <EditItemTemplate>
                                <asp:Label runat="server" Text='<%# Eval("clave") %>' ID="Label11"></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Bind("clave") %>' ID="Label2"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Usuario" SortExpression="usuario">
                            <EditItemTemplate>
                                <asp:TextBox runat="server" CssClass="ancho150px" Text='<%# Bind("usuario") %>' ID="TextBox2" placeholder="Usuario" MaxLength="200" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="TextBox2" runat="server" ErrorMessage="Necesita colocar el Usuario" CssClass="errores alert-danger" Text="*" ValidationGroup="edita" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Bind("usuario") %>' ID="Label3" />
                            </ItemTemplate>
                            <ItemStyle CssClass="ancho180px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Contraseña" SortExpression="contrasena">
                            <EditItemTemplate>
                                <asp:TextBox runat="server" CssClass="ancho150px" Text='<%# Bind("contrasena") %>' ID="TextBox3" placeholder="Contraseña" MaxLength="30" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="TextBox3" runat="server" ErrorMessage="Necesita colocar la Contraseña" CssClass="errores alert-danger" Text="*" ValidationGroup="edita" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Bind("contrasena") %>' ID="Label4" />
                            </ItemTemplate>
                            <ItemStyle CssClass="ancho180px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Host" SortExpression="host">
                            <EditItemTemplate>
                                <asp:TextBox runat="server" CssClass="ancho150px" Text='<%# Bind("host") %>' ID="TextBox4" placeholder="Host" MaxLength="50" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="TextBox4" runat="server" ErrorMessage="Necesita colocar el Host" CssClass="errores alert-danger" Text="*" ValidationGroup="edita" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Bind("host") %>' ID="Label5" />
                            </ItemTemplate>
                            <ItemStyle CssClass="ancho180px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Puerto" SortExpression="puerto">
                            <EditItemTemplate>
                                <asp:TextBox runat="server" CssClass="ancho90px" Text='<%# Bind("puerto") %>' ID="TextBox5" placeholder="Puerto" MaxLength="5" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="TextBox5" runat="server" ErrorMessage="Necesita colocar el Puerto" CssClass="errores alert-danger" Text="*" ValidationGroup="edita" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Bind("puerto") %>' ID="Label6" />
                            </ItemTemplate>
                            <ItemStyle CssClass="ancho90px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tipo Servidor" SortExpression="tipo_servidor">
                            <EditItemTemplate>
                                <asp:TextBox runat="server" CssClass="ancho130px" Text='<%# Bind("tipo_servidor") %>' ID="TextBox6" placeholder="Tipo Servidor" MaxLength="3" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ControlToValidate="TextBox6" runat="server" ErrorMessage="Necesita colocar el Tipo de Servidor" CssClass="errores alert-danger" Text="*" ValidationGroup="edita" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Bind("tipo_servidor") %>' ID="Label7" />
                            </ItemTemplate>
                            <ItemStyle CssClass="ancho130px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="SSL Habilitado" SortExpression="ssl_habilitado">
                            <EditItemTemplate>
                                <asp:TextBox runat="server" Text='<%# Bind("ssl_habilitado") %>' ID="TextBox1" placeholder="SSL" MaxLength="3" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ControlToValidate="TextBox1" runat="server" ErrorMessage="Necesita colocar el SSL" CssClass="errores alert-danger" Text="*" ValidationGroup="edita" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Bind("ssl_habilitado") %>' ID="Label1" />
                            </ItemTemplate>
                        </asp:TemplateField>
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
                                <asp:LinkButton ID="btnEliminar" runat="server" CausesValidation="False" CommandName="Delete" ToolTip="Eliminar" CssClass="btn btn-danger t14" OnClientClick="return confirm('¿Esta seguro de eliminar el Taller?');"><i class="fa fa-trash"></i></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EditRowStyle CssClass="alert-warning" />
                    <EmptyDataRowStyle CssClass="errores alert-danger" />
                    <SelectedRowStyle CssClass="alert-success" />
                </asp:GridView>
                <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:PVW %>'
                    SelectCommand="SELECT [clave], [usuario], [contrasena], [host], [puerto], [tipo_servidor], [ssl_habilitado] FROM [parametros_email]"
                    DeleteCommand="delete from parametros_email WHERE clave = @clave"
                    InsertCommand="INSERT INTO parametros_email VALUES((select isnull(count(*),0)+1 from parametros_email),@usuario,@contrasena,@host,@puerto,@tipo_servidor,@ssl_habilitado)"
                    UpdateCommand="UPDATE parametros_email SET usuario = @usuario, contrasena = @contrasena, host = @host, puerto = @puerto, tipo_servidor = @tipo_servidor, ssl_habilitado = @ssl_habilitado WHERE clave = @clave">
                    <DeleteParameters>
                        <asp:Parameter Name="clave" />
                    </DeleteParameters>
                    <InsertParameters>
                        <asp:ControlParameter ControlID="txtUsuario" Name="usuario" Type="String" />
                        <asp:ControlParameter ControlID="txtContraseña" Name="contrasena" Type="String" />
                        <asp:ControlParameter ControlID="txtHost" Name="host" Type="String" />
                        <asp:ControlParameter ControlID="txtPuerto" Name="puerto" Type="Int32" />
                        <asp:ControlParameter ControlID="ddlTipoServidor" Name="tipo_servidor" Type="Int32" PropertyName="SelectedValue" />
                        <asp:ControlParameter ControlID="lblHabilitado" Name="ssl_habilitado" Type="Int32" />
                    </InsertParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="usuario" />
                        <asp:Parameter Name="contrasena" />
                        <asp:Parameter Name="host" />
                        <asp:Parameter Name="puerto" />
                        <asp:Parameter Name="tipo_servidor" />
                        <asp:Parameter Name="ssl_habilitado" />
                        <asp:Parameter Name="clave" />
                    </UpdateParameters>
                </asp:SqlDataSource>
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                    <ProgressTemplate>
                        <asp:Panel ID="pnlMaskLoad" runat="server" CssClass="maskLoad"></asp:Panel>
                        <asp:Panel ID="pnlCargando" runat="server" CssClass="pnlPopUpLoad">
                            <asp:Image ID="imgLoad" runat="server" ImageUrl="~/img/loading.gif" CssClass="ancho100" />
                        </asp:Panel>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
