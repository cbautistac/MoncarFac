<%@ Page Title="" Language="C#" MasterPageFile="~/Administracion.master" AutoEventWireup="true" CodeFile="Clientes_Cat.aspx.cs" Inherits="Clientes_Cat" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <h1 class="centrado textoCentrado colorMoncarAzul">Clientes</h1>
    <asp:Panel ID="pnlCatalogos" runat="server" CssClass="panelCatalogos textoCentrado enlinea" ScrollBars="Auto">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Panel ID="PanelClientesBasicos" runat="server" CssClass="alineado">
                    <asp:Panel ID="PanelAdd" runat="server" CssClass="col-lg-12 col-sm-12 text-center">
                         <table class="centrado">
                        <tr class="alineado">
                            <td class="alineado textoCentrado" colspan="6">
                                <asp:Label ID="Label1" runat="server" Text="Persona: " /><br />
                                <asp:RadioButtonList ID="rbtnPersona" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rbtnPersona_SelectedIndexChanged" CssClass="centrado">
                                    <asp:ListItem Value="F" Selected="True" Text="Fisica" />
                                    <asp:ListItem Value="M" Text="Moral" /> 
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtRfc" runat="server" CssClass="input-medium" />
                                <cc1:TextBoxWatermarkExtender ID="txtRfcWatermarkExtender1" runat="server" BehaviorID="txtRfc_TextBoxWatermarkExtender" TargetControlID="txtRfc" WatermarkCssClass="input-medium water" WatermarkText="R.F.C." />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Debe indicar el R.F.C." Text="*" ValidationGroup="crea" ControlToValidate="txtRfc" CssClass="alineado errores"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRazonNew" runat="server" CssClass="input-medium" Visible="false" MaxLength="200" />
                                <cc1:TextBoxWatermarkExtender ID="txtRazonNew_TextBoxWatermarkExtender"
                                    runat="server" BehaviorID="txtRazonNew_TextBoxWatermarkExtender"
                                    TargetControlID="txtRazonNew" WatermarkCssClass="input-medium water" WatermarkText="Razón Social" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Debe indicar la Razón Social" Text="*" ValidationGroup="crea" ControlToValidate="txtRazonNew" CssClass="alineado errores" Enabled="false"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNombre" runat="server" CssClass="input-medium" MaxLength="100" />
                                <cc1:TextBoxWatermarkExtender ID="txtNombre_TextBoxWatermarkExtender" runat="server" BehaviorID="txtNombre_TextBoxWatermarkExtender" TargetControlID="txtNombre" WatermarkCssClass="input-medium water" WatermarkText="Nombre" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Debe indicar el Nombre" Text="*" ValidationGroup="crea" ControlToValidate="txtNombre" CssClass="alineado errores"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtApPat" runat="server" CssClass="input-medium" MaxLength="50" />
                                <cc1:TextBoxWatermarkExtender ID="txtApPat_TextBoxWatermarkExtender" runat="server" BehaviorID="txtApPat_TextBoxWatermarkExtender" TargetControlID="txtApPat" WatermarkCssClass="input-medium water" WatermarkText="Apellido Paterno" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Debe indicar el Apellido Paterno" Text="*" ValidationGroup="crea" ControlToValidate="txtApPat" CssClass="alineado errores"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtApMat" runat="server" CssClass="input-medium" MaxLength="50" />
                                <cc1:TextBoxWatermarkExtender ID="txtApMat_TextBoxWatermarkExtender" runat="server" BehaviorID="txtApMat_TextBoxWatermarkExtender" TargetControlID="txtApMat" WatermarkCssClass="input-medium water" WatermarkText="Apellido Materno" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmail" runat="server" placeholder="Correo" CssClass="input-medium" />
                            </td>
                            <td>
                                <asp:CheckBox ID="chkAseguradora" runat="server" Text="&nbsp;Aseguradora" />
                            </td>
                            <td>
                                <asp:LinkButton ID="lnkAgregar" runat="server" ToolTip="Agregar" CssClass="btn btn-info" ValidationGroup="crea" OnClick="lnkAgregar_Click"><i class="fa fa-plus"></i></asp:LinkButton>
                                <asp:Label runat="server" ID="lblFecha" Visible="false" />
                            </td>
                        </tr>
                    </table>
                    </asp:Panel>
                    <asp:Panel ID="Panel1" runat="server" CssClass="col-lg-12 col-sm-12">
                        <asp:Panel ID="PanelGrid" runat="server" CssClass="ancho65 textoCentrado alineatoTop enlinea">
                            <asp:GridView ID="GridClientes" runat="server" AutoGenerateColumns="False"
                                CssClass="table table-bordered" AllowPaging="True" AllowSorting="True"
                                EmptyDataText="No existe clientes para mostrar" DataKeyNames="id_cliprov"
                                DataSourceID="SqlDataSource1" OnRowCommand="GridCatClientes_RowCommand"
                                OnRowDataBound="GridClientes_RowDataBound" OnRowDeleting="GridClientes_RowDeleting">
                                <Columns>
                                    <asp:BoundField DataField="id_cliprov" HeaderText="id_cliprov" ReadOnly="True" SortExpression="id_cliprov" Visible="false" />
                                    <asp:BoundField DataField="rfc" HeaderText="R.F.C." SortExpression="rfc" />
                                    <asp:BoundField DataField="razon_social" HeaderText="Cliente" SortExpression="razon_social" />
                                    <asp:BoundField DataField="nombre" HeaderText="nombre" SortExpression="nombre" Visible="false" />
                                    <asp:BoundField DataField="ap_paterno" HeaderText="ap_paterno" SortExpression="ap_paterno" Visible="false" />
                                    <asp:BoundField DataField="ap_materno" HeaderText="ap_materno" SortExpression="ap_materno" Visible="false" />
                                    <asp:CheckBoxField DataField="aseguradora" HeaderText="Aseguradora" SortExpression="aseguradora" />

                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkSeleccionar" runat="server" CssClass="btn btn-success" CommandName="Select" ToolTip="Seleccionar" CommandArgument='<%# Eval("id_cliprov")%>'><i class="fa fa-check"></i></asp:LinkButton>
                                            <asp:LinkButton ID="lnkEliminar" runat="server" CssClass="btn btn-danger" CommandName="Delete" ToolTip="Eliminar" OnClientClick="return confirm('¿Esta seguro de eliminar al Cliente?');"><i class="fa fa-ban"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkAlta" runat="server" CommandName="alta" ToolTip="Alta" CssClass="btn btn-info" CommandArgument='<%# Bind("id_cliprov")%>'><i class="fa fa-chevron-up"></i></asp:LinkButton>
                                            <asp:LinkButton ID="lnkBaja" runat="server" CommandName="baja" ToolTip="Baja" CssClass="btn btn-info" CommandArgument='<%# Bind("id_cliprov")%>'><i class="fa fa-chevron-down"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                            </asp:GridView>
                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>"
                                DeleteCommand="delete from Cliprov where id_cliprov=@id_cliprov AND tipo='C'"
                                SelectCommand="select id_cliprov,rfc,razon_social,nombre,ap_paterno,ap_materno,aseguradora from Cliprov where tipo='C'">
                                <DeleteParameters>
                                    <asp:Parameter Name="id_cliprov" />
                                </DeleteParameters>
                            </asp:SqlDataSource>
                        </asp:Panel>
                        <asp:Panel ID="PanelDetalle" runat="server" CssClass="ancho50 textoCentrado alineatoTop enlinea alto400px" ScrollBars="Auto" Visible="false">
                            <asp:Panel ID="PanelDetalleCliente" runat="server" ScrollBars="Auto" CssClass="col-lg-12 col-sm-12 text-left">
                                <asp:Label ID="lblIdCliprovDetalle" runat="server" Visible="false" />
                                <asp:Label ID="lblTipoDetalle" runat="server" Visible="false" />
                                <div class="col-lg-12 col-sm-12 text-right">
                                    <br />
                                    <br />
                                    <asp:LinkButton ID="lnkCerrarDetalle" runat="server" CssClass="btn btn-danger" ToolTip="Cerrar" OnClick="lnkCerrarDetalle_Click"><i class="fa fa-times"></i></asp:LinkButton>
                                    <br />
                                    <br />
                                </div>
                                <asp:DetailsView ID="DetailsView1" runat="server" CssClass="table table-bordered"
                                    AutoGenerateRows="False" DataKeyNames="id_cliprov,tipo" OnDataBound="DetailsView1_DataBound"
                                    DataSourceID="SqlDataSource2" OnItemUpdating="DetailsView1_ItemUpdating" 
                                    onitemupdated="DetailsView1_ItemUpdated">
                                    <Fields>
                                        <asp:BoundField DataField="id_cliprov" HeaderText="id_cliprov" ReadOnly="True" SortExpression="id_cliprov" Visible="false" />
                                        <asp:BoundField DataField="tipo" HeaderText="tipo" ReadOnly="True" SortExpression="tipo" Visible="false" />
                                        <asp:TemplateField HeaderText="Persona:" SortExpression="persona">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("descripcion_persona") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:RadioButtonList ID="rdlPersonaMod" runat="server" RepeatColumns="2" RepeatDirection="Horizontal" SelectedValue='<%# Bind("persona") %>' AutoPostBack="true" OnSelectedIndexChanged="rdlPersonaMod_SelectedIndexChanged">
                                                    <asp:ListItem Value="M" Text="Moral"></asp:ListItem>
                                                    <asp:ListItem Value="F" Text="Física"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="R.F.C.:" SortExpression="rfc">
                                            <ItemTemplate>
                                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("rfc") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtRFCMod" runat="server" Text='<%# Bind("rfc") %>' CssClass="input-medium"></asp:TextBox>
                                                <cc1:TextBoxWatermarkExtender ID="txtRFCMod_TextBoxWatermarkExtender" runat="server" BehaviorID="txtRFCMod_TextBoxWatermarkExtender" TargetControlID="txtRFCMod" WatermarkText="R.F.C." WatermarkCssClass="water input-medium" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Debe indicar el RFC." Text="*" CssClass="alineado errores" ControlToValidate="txtRFCMod" ValidationGroup="edita" />
                                                <asp:RegularExpressionValidator ControlToValidate="txtRFCMod" ID="RegularExpressionValidator11" runat="server" ErrorMessage="El R.F.C. tiene un formato invalido" ValidationGroup="edita" ValidationExpression="^[A-Za-z]{3,4}[0-9]{6}[0-9A-Za-z]{3}$" Text="*" CssClass="errores alert-danger" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sexo:" SortExpression="sexo">
                                            <ItemTemplate>
                                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("descripcion_sexo") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:RadioButtonList ID="rdlSexoMod" runat="server" RepeatColumns="2"
                                                    RepeatDirection="Horizontal" SelectedValue='<%# Bind("sexo") %>'>
                                                    <asp:ListItem Value="M" Text="Masculino"></asp:ListItem>
                                                    <asp:ListItem Value="F" Text="Femenino"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="F. Nacimiento:" SortExpression="fecha_nacimiento">
                                            <ItemTemplate>
                                                <asp:Label ID="Label4" runat="server" Text='<%# Bind("fecha_nacimiento") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtfechaMod" runat="server" Text='<%# Bind("fecha_nacimiento") %>' CssClass="input-small" MaxLength="10"></asp:TextBox>
                                                <cc1:CalendarExtender ID="txtfechaMod_CalendarExtender" runat="server" BehaviorID="txtfechaMod_CalendarExtender" TargetControlID="txtfechaMod" Format="yyyy-MM-dd" />
                                                <cc1:TextBoxWatermarkExtender ID="txtfechaMod_TextBoxWatermarkExtender" runat="server" BehaviorID="txtfechaMod_TextBoxWatermarkExtender" TargetControlID="txtfechaMod" WatermarkText="AAAA-MM-DD" WatermarkCssClass="water input-small" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Debe indicar la fecha de nacimiento." Text="*" CssClass="alineado errores" ControlToValidate="txtfechaMod" ValidationGroup="edita"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="El formato de la fecha no es valido; AAAA-MM-DD" ValidationExpression="^[0-9]{4}-(((0[13578]|(10|12))-(0[1-9]|[1-2][0-9]|3[0-1]))|(02-(0[1-9]|[1-2][0-9]))|((0[469]|11)-(0[1-9]|[1-2][0-9]|30)))$" ValidationGroup="edita" ControlToValidate="txtfechaMod" Text="*" CssClass="errores alert-danger" />
                                                <asp:Label ID="Label24" runat="server" Text="AAAA-MM-DD" CssClass="water"></asp:Label>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="edad" HeaderText="edad" SortExpression="edad" Visible="False" />
                                        <asp:TemplateField HeaderText="Razón Social:" SortExpression="razon_social">
                                            <ItemTemplate>
                                                <asp:Label ID="Label5" runat="server" Text='<%# Bind("razon_social") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtRazonMod" runat="server" Text='<%# Bind("razon_social") %>' MaxLength="200" CssClass="input-medium"></asp:TextBox>
                                                <cc1:TextBoxWatermarkExtender ID="txtRazonMod_TextBoxWatermarkExtender" runat="server" BehaviorID="txtRazonMod_TextBoxWatermarkExtender" TargetControlID="txtRazonMod" WatermarkText="Razón Social" WatermarkCssClass="water input-medium" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Debe indicar la Razón Social." Text="*" CssClass="alineado errores" ControlToValidate="txtRazonMod" ValidationGroup="edita"></asp:RequiredFieldValidator>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Nombre:" SortExpression="nombre">
                                            <ItemTemplate>
                                                <asp:Label ID="Label6" runat="server" Text='<%# Bind("nombre") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtNombreMod" runat="server" Text='<%# Bind("nombre") %>' MaxLength="100" CssClass="input-medium"></asp:TextBox>
                                                <cc1:TextBoxWatermarkExtender ID="txtNombreMod_TextBoxWatermarkExtender" runat="server" BehaviorID="txtNombreMod_TextBoxWatermarkExtender" TargetControlID="txtNombreMod" WatermarkText="Nombre" WatermarkCssClass="water input-medium" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="Debe indicar el Nombre." Text="*" CssClass="alineado errores" ControlToValidate="txtNombreMod" ValidationGroup="edita"></asp:RequiredFieldValidator>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Apellido Paterno" SortExpression="ap_paterno">
                                            <ItemTemplate>
                                                <asp:Label ID="Label7" runat="server" Text='<%# Bind("ap_paterno") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtApMod" runat="server" Text='<%# Bind("ap_paterno") %>' MaxLength="50" CssClass="input-medium"></asp:TextBox>
                                                <cc1:TextBoxWatermarkExtender ID="txtApMod_TextBoxWatermarkExtender" runat="server" BehaviorID="txtApMod_TextBoxWatermarkExtender" TargetControlID="txtApMod" WatermarkText="Apellido Paterno" WatermarkCssClass="water input-medium" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="Debe indicar el Apellido Paterno." Text="*" CssClass="alineado errores" ControlToValidate="txtApMod" ValidationGroup="edita"></asp:RequiredFieldValidator>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Apellido Materno:" SortExpression="ap_materno">
                                            <ItemTemplate>
                                                <asp:Label ID="Label8" runat="server" Text='<%# Bind("ap_materno") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtAmMod" runat="server" Text='<%# Bind("ap_materno") %>' MaxLength="50" CssClass="input-medium"></asp:TextBox>
                                                <cc1:TextBoxWatermarkExtender ID="txtAmMod_TextBoxWatermarkExtender" runat="server" BehaviorID="txtAmMod_TextBoxWatermarkExtender" TargetControlID="txtAmMod" WatermarkText="Apellido Materno" WatermarkCssClass="water input-medium" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Calle:" SortExpression="calle">
                                            <ItemTemplate>
                                                <asp:Label ID="Label9" runat="server" Text='<%# Bind("calle") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtCalleMod" runat="server" Text='<%# Bind("calle") %>' MaxLength="200" CssClass="input-medium"></asp:TextBox>
                                                <cc1:TextBoxWatermarkExtender ID="txtCalleMod_TextBoxWatermarkExtender" runat="server" BehaviorID="txtCalleMod_TextBoxWatermarkExtender" TargetControlID="txtCalleMod" WatermarkText="Calle" WatermarkCssClass="water input-medium" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator71" runat="server" ErrorMessage="Debe indicar la Calle." Text="*" CssClass="alineado errores" ControlToValidate="txtCalleMod" ValidationGroup="edita"></asp:RequiredFieldValidator>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Num. Ext.:" SortExpression="num_ext">
                                            <ItemTemplate>
                                                <asp:Label ID="Label10" runat="server" Text='<%# Bind("num_ext") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtNoExtMod" runat="server" Text='<%# Bind("num_ext") %>' MaxLength="20" CssClass="input-small"></asp:TextBox>
                                                <cc1:TextBoxWatermarkExtender ID="txtNoExtMod_TextBoxWatermarkExtender" runat="server" BehaviorID="txtNoExtMod_TextBoxWatermarkExtender" TargetControlID="txtNoExtMod" WatermarkText="Num. Ext." WatermarkCssClass="water input-small" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator72" runat="server" ErrorMessage="Debe indicar la No. Exterior." Text="*" CssClass="alineado errores" ControlToValidate="txtNoExtMod" ValidationGroup="edita"></asp:RequiredFieldValidator>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Num.Int.:" SortExpression="num_int">
                                            <ItemTemplate>
                                                <asp:Label ID="Label11" runat="server" Text='<%# Bind("num_int") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtNoIntMod" runat="server" Text='<%# Bind("num_int") %>' MaxLength="20" CssClass="input-small"></asp:TextBox>
                                                <cc1:TextBoxWatermarkExtender ID="txtNoIntMod_TextBoxWatermarkExtender" runat="server" BehaviorID="txtNoIntMod_TextBoxWatermarkExtender" TargetControlID="txtNoIntMod" WatermarkText="Num. Int." WatermarkCssClass="water input-small" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="PaÍs:" SortExpression="pais">
                                            <ItemTemplate>
                                                <asp:Label ID="Label16" runat="server" Text='<%# Bind("pais") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtPaisMod" runat="server" Text='<%# Bind("pais") %>' MaxLength="200" CssClass="input-medium" Visible="false"></asp:TextBox>                                                
                                                <telerik:RadComboBox RenderMode="Lightweight" ID="ddlPais" runat="server" Width="200" Height="200px" DataValueField="cve_pais" AutoPostBack="true" MarkFirstMatch="true" AllowCustomText="true" AutoCompleteSeparator="None" Filter="Contains" 
                                                    EmptyMessage="Seleccione País..." DataSourceID="SqlDataSource10" DataTextField="desc_pais" Skin="Silk" OnSelectedIndexChanged="ddlPais_SelectedIndexChanged">
                                                </telerik:RadComboBox>
                                                <asp:SqlDataSource ID="SqlDataSource10" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select cve_pais,desc_pais from Paises_f"></asp:SqlDataSource>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator73" runat="server" ErrorMessage="Debe indicar el País." Text="*" CssClass="alineado errores" ControlToValidate="ddlPais" ValidationGroup="edita"></asp:RequiredFieldValidator>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Estado:" SortExpression="estado">
                                            <ItemTemplate>
                                                <asp:Label ID="Label15" runat="server" Text='<%# Bind("estado") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtEstadoMod" runat="server" Text='<%# Bind("estado") %>' MaxLength="200" CssClass="input-medium" Visible="false"></asp:TextBox>                                                
                                                <telerik:RadComboBox RenderMode="Lightweight" ID="ddlEstado" runat="server" Width="200" Height="200px" DataValueField="cve_edo" AutoPostBack="true" MarkFirstMatch="true" AllowCustomText="true" AutoCompleteSeparator="None" Filter="Contains"
                                                    EmptyMessage="Seleccione Estado..." DataSourceID="SqlDataSource11" DataTextField="nom_edo" Skin="Silk" OnSelectedIndexChanged="ddlEstado_SelectedIndexChanged">
                                                </telerik:RadComboBox>
                                                <asp:SqlDataSource ID="SqlDataSource11" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select cve_edo,nom_edo from Estados where cve_pais=@pais">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="ddlPais" Name="pais" />
                                                    </SelectParameters>
                                                </asp:SqlDataSource>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator74" runat="server" ErrorMessage="Debe indicar el Estado." Text="*" CssClass="alineado errores" ControlToValidate="ddlEstado" ValidationGroup="edita"></asp:RequiredFieldValidator>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Municip. / Deleg.:" SortExpression="municipio">
                                            <ItemTemplate>
                                                <asp:Label ID="Label14" runat="server" Text='<%# Bind("municipio") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtMunicipMod" runat="server" Text='<%# Bind("municipio") %>' MaxLength="200" CssClass="input-medium" Visible="false"></asp:TextBox>                                                
                                                <telerik:RadComboBox RenderMode="Lightweight" ID="ddlMunicipio" runat="server" Width="200" Height="200px" DataValueField="ID_Del_Mun" AutoPostBack="true" MarkFirstMatch="true" AllowCustomText="true" AutoCompleteSeparator="None" Filter="Contains"
                                                    EmptyMessage="Seleccione Deleg./Municip. ..." DataSourceID="SqlDataSource12" DataTextField="Desc_Del_Mun" Skin="Silk" OnSelectedIndexChanged="ddlMunicipio_SelectedIndexChanged">
                                                </telerik:RadComboBox>
                                                <asp:SqlDataSource ID="SqlDataSource12" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select ID_Del_Mun,Desc_Del_Mun from DelegacionMunicipio where cve_pais=@pais and ID_Estado=@estado">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="ddlPais" Name="pais" />
                                                        <asp:ControlParameter ControlID="ddlEstado" Name="estado" />
                                                    </SelectParameters>
                                                </asp:SqlDataSource>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator75" runat="server" ErrorMessage="Debe indicar el Municipio." Text="*" CssClass="alineado errores" ControlToValidate="ddlMunicipio" ValidationGroup="edita"></asp:RequiredFieldValidator>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Colonia:" SortExpression="colonia">
                                            <ItemTemplate>
                                                <asp:Label ID="Label12" runat="server" Text='<%# Bind("colonia") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtColoniaMod" runat="server" Text='<%# Bind("colonia") %>' MaxLength="200" CssClass="input-medium" Visible="false"></asp:TextBox>                                                
                                                <telerik:RadComboBox RenderMode="Lightweight" ID="ddlColonia" runat="server" Width="200" Height="200px" DataValueField="ID_Colonia" AutoPostBack="true" MarkFirstMatch="true" AllowCustomText="true" AutoCompleteSeparator="None" Filter="Contains"
                                                    EmptyMessage="Seleccione Colonia ..." DataSourceID="SqlDataSource13" DataTextField="Desc_Colonia" Skin="Silk" OnSelectedIndexChanged="ddlColonia_SelectedIndexChanged">
                                                </telerik:RadComboBox>
                                                <asp:SqlDataSource ID="SqlDataSource13" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select ID_Colonia,Desc_Colonia from Colonias where cve_pais=@pais and ID_Estado=@estado and ID_Del_Mun=@municipio">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="ddlPais" Name="pais" />
                                                        <asp:ControlParameter ControlID="ddlEstado" Name="estado" />
                                                        <asp:ControlParameter ControlID="ddlMunicipio" Name="municipio" />
                                                    </SelectParameters>
                                                </asp:SqlDataSource>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator76" runat="server" ErrorMessage="Debe indicar la Colonia." Text="*" CssClass="alineado errores" ControlToValidate="ddlColonia" ValidationGroup="edita"></asp:RequiredFieldValidator>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="C.P.:" SortExpression="cp">
                                            <ItemTemplate>
                                                <asp:Label ID="Label13" runat="server" Text='<%# Bind("cp") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtCpMod" runat="server" Text='<%# Bind("cp") %>' MaxLength="5" CssClass="input-small" Visible="false"></asp:TextBox>                                                
                                                <telerik:RadComboBox RenderMode="Lightweight" ID="ddlCodigo" runat="server" Width="100" Height="100px" DataValueField="ID_Cod_Pos" AutoPostBack="true" MarkFirstMatch="true" AllowCustomText="true" AutoCompleteSeparator="None" Filter="Contains"
                                                    EmptyMessage="Seleccione Código Postal ..." DataSourceID="SqlDataSource14" DataTextField="ID_Cod_Pos" Skin="Silk">
                                                </telerik:RadComboBox>
                                                <asp:SqlDataSource ID="SqlDataSource14" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select ID_Cod_Pos from Colonias where cve_pais=@pais and ID_Estado=@estado and ID_Del_Mun=@municipio and ID_Colonia=@colonia">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="ddlPais" Name="pais" />
                                                        <asp:ControlParameter ControlID="ddlEstado" Name="estado" />
                                                        <asp:ControlParameter ControlID="ddlMunicipio" Name="municipio" />
                                                        <asp:ControlParameter ControlID="ddlColonia" Name="colonia" />
                                                    </SelectParameters>
                                                </asp:SqlDataSource>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator77" runat="server" ErrorMessage="Debe indicar el Código Postal." Text="*" CssClass="alineado errores" ControlToValidate="ddlCodigo" ValidationGroup="edita"></asp:RequiredFieldValidator>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Localidad:" SortExpression="localidad">
                                            <ItemTemplate>
                                                <asp:Label ID="Label2403" runat="server" Text='<%# Bind("localidad") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtLocalidadMod" runat="server" Text='<%# Bind("localidad") %>' MaxLength="50" CssClass="input-large"></asp:TextBox>
                                                <cc1:TextBoxWatermarkExtender ID="txtLocalidadMod_TextBoxWatermarkExtender" runat="server" BehaviorID="txtLocalidadMod_TextBoxWatermarkExtender" TargetControlID="txtLocalidadMod" WatermarkText="Localidad" WatermarkCssClass="water input-large" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Referencia:" SortExpression="referencia">
                                            <ItemTemplate>
                                                <asp:Label ID="Label2402" runat="server" Text='<%# Bind("referencia") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtReferenciaMod" runat="server" Text='<%# Bind("referencia") %>' MaxLength="50" CssClass="input-large"></asp:TextBox>
                                                <cc1:TextBoxWatermarkExtender ID="txtReferenciaMod_TextBoxWatermarkExtender" runat="server" BehaviorID="txtReferenciaMod_TextBoxWatermarkExtender" TargetControlID="txtReferenciaMod" WatermarkText="Referencia" WatermarkCssClass="water input-large" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>                
                                        <asp:TemplateField HeaderText="Tel. Particular:" SortExpression="tel_particular">
                                            <ItemTemplate>
                                                <asp:Label ID="Label17" runat="server" Text='<%# Bind("tel_particular") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtTel1Mod" runat="server" Text='<%# Bind("tel_particular") %>' MaxLength="20" CssClass="input-medium"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="txtTel1Mod_FilteredTextBoxExtender" runat="server" BehaviorID="txtTel1Mod_FilteredTextBoxExtender" TargetControlID="txtTel1Mod" FilterType="Numbers" />
                                                <cc1:TextBoxWatermarkExtender ID="txtTel1Mod_TextBoxWatermarkExtender" runat="server" BehaviorID="txtTel1Mod_TextBoxWatermarkExtender" TargetControlID="txtTel1Mod" WatermarkText="Tel. Particular" WatermarkCssClass="water input-medium" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Tel. Oficina:" SortExpression="tel_oficina">
                                            <ItemTemplate>
                                                <asp:Label ID="Label18" runat="server" Text='<%# Bind("tel_oficina") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtTel2Mod" runat="server" Text='<%# Bind("tel_oficina") %>' MaxLength="20" CssClass="input-medium"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="txtTel2Mod_FilteredTextBoxExtender" runat="server" BehaviorID="txtTel2Mod_FilteredTextBoxExtender" TargetControlID="txtTel2Mod" FilterType="Numbers" />
                                                <cc1:TextBoxWatermarkExtender ID="txtTel2Mod_TextBoxWatermarkExtender" runat="server" BehaviorID="txtTel2Mod_TextBoxWatermarkExtender" TargetControlID="txtTel2Mod" WatermarkText="Tel. Oficina" WatermarkCssClass="water input-medium" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Tel. Celular:" SortExpression="tel_celular">
                                            <ItemTemplate>
                                                <asp:Label ID="Label19" runat="server" Text='<%# Bind("tel_celular") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtTel3Mod" runat="server" Text='<%# Bind("tel_celular") %>' MaxLength="20" CssClass="input-medium"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="txtTel3Mod_FilteredTextBoxExtender" runat="server" BehaviorID="txtTel3Mod_FilteredTextBoxExtender" TargetControlID="txtTel3Mod" FilterType="Numbers" />
                                                <cc1:TextBoxWatermarkExtender ID="txtTel3Mod_TextBoxWatermarkExtender" runat="server" BehaviorID="txtTel3Mod_TextBoxWatermarkExtender" TargetControlID="txtTel3Mod" WatermarkText="Tel. Celular" WatermarkCssClass="water input-medium" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Días Revisión:" SortExpression="dia_revision">
                                            <ItemTemplate>
                                                <asp:Label ID="Label20" runat="server" Text='<%# Bind("dia_revision") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList CssClass="input-small" ID="ddlRevisionMod" runat="server" SelectedValue='<%# Bind("dia_revision") %>'>
                                                    <asp:ListItem Value="0" Text="0"></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                                    <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                                    <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                                    <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                                    <asp:ListItem Value="5" Text="5"></asp:ListItem>
                                                    <asp:ListItem Value="6" Text="6"></asp:ListItem>
                                                    <asp:ListItem Value="7" Text="7"></asp:ListItem>
                                                </asp:DropDownList>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Días Cobranza:" SortExpression="dia_cobranza">
                                            <ItemTemplate>
                                                <asp:Label ID="Label21" runat="server" Text='<%# Bind("dia_cobranza") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList CssClass="input-small" ID="ddlCobranzaMod" runat="server" SelectedValue='<%# Bind("dia_cobranza") %>'>
                                                    <asp:ListItem Value="0" Text="0"></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                                    <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                                    <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                                    <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                                    <asp:ListItem Value="5" Text="5"></asp:ListItem>
                                                    <asp:ListItem Value="6" Text="6"></asp:ListItem>
                                                    <asp:ListItem Value="7" Text="7"></asp:ListItem>
                                                </asp:DropDownList>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Pólitica Pago:" SortExpression="id_politica">
                                            <ItemTemplate>
                                                <asp:Label ID="Label22" runat="server" Text='<%# Bind("descripcion_politica") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList CssClass="input-small" ID="ddlPoliticaMod"  runat="server" DataSourceID="SqlDataSource3" DataTextField="descripcion" DataValueField="id_politica" SelectedValue='<%# Bind("id_politica") %>'></asp:DropDownList>
                                                <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select 0 as id_politica, '' as descripcion union all select id_politica,descripcion+' ('+clv_politica+' - '+Convert(char(5),dias_plazo)+')' as descripcion from politica_pago"></asp:SqlDataSource>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Aseguradora:" SortExpression="aseguradora">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="CheckBox1" runat="server"
                                                    Checked='<%# Bind("aseguradora") %>' Enabled="false" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:CheckBox ID="chbAseguradoraMod" runat="server" Checked='<%# Bind("aseguradora") %>' AutoPostBack="true" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="% Descuento:" SortExpression="porc_descuento">
                                            <ItemTemplate>
                                                <asp:Label ID="Label23" runat="server" Text='<%# Bind("porc_descuento") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtDescMod" runat="server" Text='<%# Bind("porc_descuento") %>' MaxLength="5" CssClass="input-small"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="txtDescMod_FilteredTextBoxExtender" runat="server" BehaviorID="txtDescMod_FilteredTextBoxExtender" TargetControlID="txtDescMod" FilterType="Numbers, Custom" InvalidChars="." />
                                                <cc1:TextBoxWatermarkExtender ID="txtDescMod_TextBoxWatermarkExtender" runat="server" BehaviorID="txtDescMod_TextBoxWatermarkExtender" TargetControlID="txtDescMod" WatermarkText="% Descuento" WatermarkCssClass="water input-small" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Correo:" SortExpression="correo">
                                            <ItemTemplate>
                                                <asp:Label ID="Label240" runat="server" Text='<%# Bind("correo") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtCorreoMod" runat="server" Text='<%# Bind("correo") %>' MaxLength="250" CssClass="input-large"></asp:TextBox>
                                                <cc1:TextBoxWatermarkExtender ID="txtCorreoMod_TextBoxWatermarkExtender" runat="server" BehaviorID="txtCorreoMod_TextBoxWatermarkExtender" TargetControlID="txtCorreoMod" WatermarkText="Correo" WatermarkCssClass="water input-large" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator78" runat="server" ErrorMessage="Debe indicar el Correo." Text="*" CssClass="alineado errores" ControlToValidate="txtCorreoMod" ValidationGroup="edita"></asp:RequiredFieldValidator>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Color:" SortExpression="correo">
                                            <ItemTemplate>
                                                <asp:Label ID="Label2401" runat="server" Text='<%# Bind("rgb_r") %>'></asp:Label>&nbsp;&nbsp;
                                                <asp:Label ID="Label25" runat="server" Text='<%# Bind("rgb_g") %>'></asp:Label>&nbsp;&nbsp;
                                                <asp:Label ID="Label26" runat="server" Text='<%# Bind("rgb_b") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <telerik:RadColorPicker RenderMode="Lightweight" runat="server" ID="RadColorPicker1" PaletteModes="HSB" ShowIcon="true">
                                                </telerik:RadColorPicker>
                                                <telerik:RadNumericTextBox RenderMode="Lightweight" Visible="false" runat="server" ID="rgb_r_mod" EmptyMessage="Cod. Rojo" Width="80px" Value="0" MinValue="0" ShowSpinButtons="true" NumberFormat-DecimalDigits="0" MaxValue="255" ></telerik:RadNumericTextBox>&nbsp;
                                                <telerik:RadNumericTextBox RenderMode="Lightweight" Visible="false" runat="server" ID="rgb_g_mod" EmptyMessage="Cod. Verde" Width="80px" Value="0" MinValue="0" ShowSpinButtons="true" NumberFormat-DecimalDigits="0" MaxValue="255"></telerik:RadNumericTextBox>&nbsp;
                                                <telerik:RadNumericTextBox RenderMode="Lightweight" Visible="false" runat="server" ID="rgb_b_mod" EmptyMessage="Cod. Azul" Width="80px" Value="0" MinValue="0" ShowSpinButtons="true" NumberFormat-DecimalDigits="0" MaxValue="255"></telerik:RadNumericTextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Imagen Indicadores:" >
                                            <ItemTemplate>
                                                <asp:Image ID="Image1" runat="server" AlternateText="imagen no disponible" Width="113px" ImageUrl='<%# "~/ImgCliente.ashx?id="+Eval("id_cliprov") %>' />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Image ID="Image1" runat="server" AlternateText="imagen no disponible" Width="113px" ImageUrl='<%# "~/ImgCliente.ashx?id="+Eval("id_cliprov") %>' />
                                                <telerik:RadAsyncUpload RenderMode="Lightweight" runat="server" Culture="es-Mx" CssClass="async-attachment"
                                                    MaxFileInputsCount="1" MultipleFileSelection="Disabled" ID="AsyncUpload1" HideFileInput="true"
                                                    AllowedFileExtensions=".jpeg,.jpg,.png,.gif,.bmp,.tiff,.JPEG,.JPG,.PNG,.GIF,.BMP,.TIFF">
                                                </telerik:RadAsyncUpload>
                                            </EditItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ShowHeader="False">
                                            <EditItemTemplate>
                                                <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Update" ValidationGroup="edita" CssClass="btn btn-success"><i class="fa fa-save"></i></asp:LinkButton>
                                                <asp:LinkButton ID="LinkButton2" runat="server" CommandName="Cancel" CssClass="btn btn-danger"><i class="fa fa-ban"></i></asp:LinkButton>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="LinkButton3" runat="server" CommandName="Edit" CssClass="btn btn-warning"><i class="fa fa-edit"></i></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Fields>
                                </asp:DetailsView>

                                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>"
                                    SelectCommand="select c.id_cliprov,c.tipo,c.persona,case c.persona when 'M' then 'Moral' else 'Física' end as descripcion_persona,rtrim(c.rfc) as rfc,c.sexo,case c.sexo when 'M' then 'Masculino' else 'Femenino' end as descripcion_sexo,Convert(char(10),c.fecha_nacimiento,126) as fecha_nacimiento,c.edad, c.razon_social,c.nombre,c.ap_paterno,c.ap_materno,c.calle,rtrim(c.num_ext) as num_ext,rtrim(c.num_int) as num_int,c.colonia,c.cp,c.municipio,c.estado,c.pais,c.tel_particular,c.tel_oficina,c.tel_celular,c.dia_revision,c.dia_cobranza,c.id_politica,isnull(pp.descripcion+' ('+pp.clv_politica+' - '+convert(char(5),pp.dias_plazo)+')','') as descripcion_politica,c.aseguradora,c.porc_descuento,c.correo,cast(isnull(c.rgb_r,0) as int) as rgb_r,cast(isnull(c.rgb_g,0) as int) as rgb_g,cast(isnull(c.rgb_b,0) as int) as rgb_b,c.localidad,c.referencia from cliprov c left join politica_pago pp on pp.id_politica=c.id_politica where c.id_cliprov=@id_cliprov and c.tipo='C'"
                                    UpdateCommand="update cliprov set correo=lower(@correo), persona=@persona,rfc=upper(@rfc),sexo=@sexo,fecha_nacimiento=@fecha_nacimiento,razon_social=upper(@razon_social),nombre=upper(@nombre),ap_paterno=upper(@ap_paterno),ap_materno=upper(@ap_materno),calle=@calle,num_ext=@num_ext,num_int=@num_int,colonia=@colonia,cp=@cp,municipio=@municipio,estado=@estado,pais=@pais,tel_particular=@tel_particular,tel_oficina=@tel_oficina,tel_celular=@tel_celular,dia_revision=@dia_revision,dia_cobranza=@dia_cobranza,id_politica=@id_politica,aseguradora=@aseguradora,porc_descuento=@porc_descuento,rgb_r=@rgb_r,rgb_g=@rgb_g,rgb_b=@rgb_b,localidad=@localidad,referencia=@referencia where id_cliprov=@id_cliprov and tipo='C'">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="GridClientes" Name="id_cliprov" PropertyName="SelectedValue" />
                                    </SelectParameters>
                                    <UpdateParameters>
                                        <asp:Parameter Name="correo" Type="String" />
                                        <asp:Parameter Name="persona" Type="String" />
                                        <asp:Parameter Name="rfc" Type="String" />
                                        <asp:Parameter Name="sexo" Type="String" />
                                        <asp:Parameter Name="fecha_nacimiento" Type="String" />
                                        <asp:Parameter Name="razon_social" Type="String" />
                                        <asp:Parameter Name="nombre" Type="String" />
                                        <asp:Parameter Name="ap_paterno" Type="String" />
                                        <asp:Parameter Name="ap_materno" Type="String" />
                                        <asp:Parameter Name="calle" Type="String" />
                                        <asp:Parameter Name="num_ext" Type="String" />
                                        <asp:Parameter Name="num_int" Type="String" />
                                        <asp:Parameter Name="colonia" Type="String" />
                                        <asp:Parameter Name="cp" Type="String" />
                                        <asp:Parameter Name="municipio" Type="String" />
                                        <asp:Parameter Name="estado" Type="String" />
                                        <asp:Parameter Name="pais" Type="String" />
                                        <asp:Parameter Name="tel_particular" Type="String" />
                                        <asp:Parameter Name="tel_oficina" Type="String" />
                                        <asp:Parameter Name="tel_celular" Type="String" />
                                        <asp:Parameter Name="dia_revision" Type="Int32" />
                                        <asp:Parameter Name="dia_cobranza" Type="Int32" />
                                        <asp:Parameter Name="id_politica" Type="Int32" />
                                        <asp:Parameter Name="aseguradora" Type="Boolean" />
                                        <asp:Parameter Name="porc_descuento" Type="Decimal" />
                                        <asp:Parameter Name="id_cliprov" Type="Int32" />
                                        <asp:Parameter Name="rgb_r" Type="Int32" />
                                        <asp:Parameter Name="rgb_g" Type="Int32" />
                                        <asp:Parameter Name="rgb_b" Type="Int32" />
                                        <asp:Parameter Name="localidad" Type="String" />
                                        <asp:Parameter Name="referencia" Type="String" />
                                    </UpdateParameters>
                                </asp:SqlDataSource>

                            </asp:Panel>
                        </asp:Panel>
                    </asp:Panel>

                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="crea" CssClass="errores alert-danger" DisplayMode="List" />
                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="edita" CssClass="errores alert-danger" DisplayMode="List" />
                    <asp:Label ID="lblError" runat="server" CssClass="errores alert-danger" />
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
    </asp:Panel>
</asp:Content>
