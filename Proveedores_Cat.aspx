<%@ Page Title="" Language="C#" MasterPageFile="~/Administracion.master" AutoEventWireup="true" CodeFile="Proveedores_Cat.aspx.cs" Inherits="Proveedores_Cat" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function abreWinCtrl() {
            var oWnd = $find("<%=modalPopupControl.ClientID%>");
            oWnd.setUrl('');
            oWnd.show();
        }
        function cierraWinCtrl() {
            var oWnd = $find("<%=modalPopupControl.ClientID%>");
            oWnd.close();
        }
    </script>
    <telerik:RadScriptManager ID="RadScriptManajer1" runat="server" EnableScriptGlobalization="true"></telerik:RadScriptManager>
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
    
    
    <telerik:RadWindow RenderMode="Lightweight" ID="modalPopupControl" Title="Alta y Modificación de Proveedores" EnableShadow="true" Skin="Silk"
        Behaviors="Close,Move" VisibleOnPageLoad="false" ShowContentDuringLoad="false" DestroyOnClose="true"
        Animation="Fade" runat="server" Modal="true" Width="1000px" Height="600px" Style="z-index: 1000;" >
        <ContentTemplate>
            <asp:UpdatePanel ID="UpdatePanelControl" runat="server">
                <ContentTemplate>
                    <div class="col-lg-12 col-sm-12">
                    <div class="row">
                        <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label29" runat="server" Text="Persona: "></asp:Label><asp:Label ID="lblProveedor" runat="server" Visible="false"></asp:Label></div>
                        <div class="col-lg-4 col-sm-4 text-left">
                            <asp:RadioButtonList ID="rdlPersonaMod" CellSpacing="10" runat="server" RepeatColumns="2" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rdlPersonaMod_SelectedIndexChanged">
                                <asp:ListItem Value="M" Text="Moral"></asp:ListItem>
                                <asp:ListItem Value="F" Text="Física"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                        <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label30" runat="server" Text="R.F.C.: "></asp:Label></div>
                        <div class="col-lg-4 col-sm-4 text-left">
                            <asp:TextBox ID="txtRFCMod" runat="server" CssClass="input-medium" MaxLength="13" placeholder="R.F.C."></asp:TextBox>                                                
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Debe indicar el RFC." Text="*" CssClass="alineado errores" ControlToValidate="txtRFCMod" ValidationGroup="edita" />
                            <%--<asp:RegularExpressionValidator ControlToValidate="txtRFCMod" ID="RegularExpressionValidator11" runat="server" ErrorMessage="El R.F.C. tiene un formato invalido" ValidationGroup="edita" ValidationExpression="^[A-Za-z]{3,4}[0-9]{6}[0-9A-Za-z]{3}$" Text="*" CssClass="errores alert-danger" />--%>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label31" runat="server" Text="Sexo: "></asp:Label></div>
                        <div class="col-lg-4 col-sm-4 text-left">
                            <asp:RadioButtonList ID="rdlSexoMod" runat="server" RepeatColumns="2" CellSpacing="10" RepeatDirection="Horizontal" >
                                <asp:ListItem Value="M" Text="Masculino"></asp:ListItem>
                                <asp:ListItem Value="F" Text="Femenino"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                        <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label32" runat="server" Text="Fecha Nacimiento: "></asp:Label></div>
                        <div class="col-lg-4 col-sm-4 text-left">
                            <asp:TextBox ID="txtfechaMod" runat="server" CssClass="input-small" MaxLength="10" Enabled="false"></asp:TextBox>
                            <cc1:CalendarExtender ID="txtfechaMod_CalendarExtender" runat="server" BehaviorID="txtfechaMod_CalendarExtender" TargetControlID="txtfechaMod" Format="yyyy-MM-dd" PopupButtonID="lnkFechaMod" />
                            <asp:LinkButton ID="lnkFechaMod" runat="server" CssClass="btn btn-info t14"><i class="fa fa-calendar"></i></asp:LinkButton>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Debe indicar la fecha de nacimiento." Text="*" CssClass="alineado errores" ControlToValidate="txtfechaMod" ValidationGroup="edita"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label33" runat="server" Text="Razón Social: "></asp:Label></div>
                        <div class="col-lg-10 col-sm-10 text-left">
                            <asp:TextBox ID="txtRazonMod" runat="server" MaxLength="200" CssClass="input-xlarge" placeholder="Razón Social"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Debe indicar la Razón Social." Text="*" CssClass="alineado errores" ControlToValidate="txtRazonMod" ValidationGroup="edita"></asp:RequiredFieldValidator>
                            <asp:TextBox ID="txtNombreMod" runat="server" MaxLength="100" CssClass="input-medium" placeholder="Nombre(s)"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="Debe indicar el Nombre." Text="*" CssClass="alineado errores" ControlToValidate="txtNombreMod" ValidationGroup="edita"></asp:RequiredFieldValidator>
                            <asp:TextBox ID="txtApMod" runat="server" MaxLength="50" CssClass="input-medium" placeholder="Apellido Paterno"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="Debe indicar el Apellido Paterno." Text="*" CssClass="alineado errores" ControlToValidate="txtApMod" ValidationGroup="edita"></asp:RequiredFieldValidator>
                            <asp:TextBox ID="txtAmMod" runat="server" MaxLength="50" CssClass="input-medium" placeholder="Apellido Materno"></asp:TextBox>                                                
                        </div>                                            
                    </div>
                    <div class="row">
                        <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label35" runat="server" Text="Calle: "></asp:Label></div>
                        <div class="col-lg-4 col-sm-4 text-left">
                            <asp:TextBox ID="txtCalleMod" runat="server" MaxLength="200" CssClass="input-large" placeholder="Calle"></asp:TextBox>
                        </div>
                        <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label36" runat="server" Text="No. Ext.: "></asp:Label></div>
                        <div class="col-lg-1 col-sm-1 text-left"><asp:TextBox ID="txtNoExtMod" runat="server" MaxLength="20" CssClass="input-small" placeholder="No. Ext."></asp:TextBox></div>
                        <div class="col-lg-1 col-sm-1 text-right"><asp:Label ID="Label34" runat="server" Text="No.Int.: "></asp:Label></div>
                        <div class="col-lg-1 col-sm-1 text-left"><asp:TextBox ID="txtNoIntMod" runat="server" MaxLength="20" CssClass="input-small" placeholder="No. Int."></asp:TextBox></div>
                    </div>
                    <div class="row">
                        <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label37" runat="server" Text="Colonia: "></asp:Label></div>
                        <div class="col-lg-4 col-sm-4 text-left"><asp:TextBox ID="txtColoniaMod" runat="server" MaxLength="200" CssClass="input-large" placeholder="Colonia"></asp:TextBox></div>
                        <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label38" runat="server" Text="Código Postal: "></asp:Label></div>
                        <div class="col-lg-4 col-sm-4 text-left"><telerik:RadNumericTextBox ID="txtCpMod" runat="server" MinValue="00001" MaxValue="99999" EmptyMessage="Código Postal" MaxLength="5" NumberFormat-GroupSeparator="," NumberFormat-GroupSizes="5" NumberFormat-DecimalDigits="0" DisplayText='<%# Bind("cp") %>' Skin="MetroTouch"></telerik:RadNumericTextBox></div>
                    </div>
                    <div class="row">
                        <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label39" runat="server" Text="Municipio o Delegación: "></asp:Label></div>
                        <div class="col-lg-4 col-sm-4 text-left"><asp:TextBox ID="txtMunicipMod" runat="server" MaxLength="200" CssClass="input-large" placeholder="Municipio o Delegación"></asp:TextBox></div>
                        <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label40" runat="server" Text="Estado: "></asp:Label></div>
                        <div class="col-lg-4 col-sm-4 text-left"><asp:TextBox ID="txtEstadoMod" runat="server" MaxLength="200" CssClass="input-large" placeholder="Estado"></asp:TextBox></div>
                    </div>
                    <div class="row">
                        <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label41" runat="server" Text="País: "></asp:Label></div>
                        <div class="col-lg-4 col-sm-4 text-left"><asp:TextBox ID="txtPaisMod" runat="server" MaxLength="200" CssClass="input-large" placeholdeR="País"></asp:TextBox></div>
                        <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label42" runat="server" Text="Teléfono Particular: "></asp:Label></div>
                        <div class="col-lg-4 col-sm-4 text-left"><telerik:RadNumericTextBox ID="txtTel1Mod" runat="server" MinValue="0" MaxLength="20" NumberFormat-GroupSeparator="" EmptyMessage="Teléfono Particular" NumberFormat-DecimalDigits="0" Skin="MetroTouch"></telerik:RadNumericTextBox></div>
                    </div>
                    <div class="row">
                        <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label43" runat="server" Text="Teléfono Oficina: "></asp:Label></div>
                        <div class="col-lg-4 col-sm-4 text-left"><telerik:RadNumericTextBox ID="txtTel2Mod" runat="server" MinValue="0" MaxLength="20" NumberFormat-GroupSeparator="" EmptyMessage="Teléfono Oficina" NumberFormat-DecimalDigits="0" Skin="MetroTouch"></telerik:RadNumericTextBox></div>
                        <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label44" runat="server" Text="Teléfono Celular: "></asp:Label></div>
                        <div class="col-lg-4 col-sm-4 text-left"><telerik:RadNumericTextBox ID="txtTel3Mod" runat="server" MinValue="0" MaxLength="20" NumberFormat-GroupSeparator="" EmptyMessage="Teléfono Celular" NumberFormat-DecimalDigits="0" Skin="MetroTouch"></telerik:RadNumericTextBox></div>
                    </div>
                    <div class="row">
                        <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label45" runat="server" Text="Días de Revisión: "></asp:Label></div>
                        <div class="col-lg-4 col-sm-4 text-left">
                            <asp:DropDownList CssClass="input-small" ID="ddlRevisionMod" runat="server">
                                <asp:ListItem Value="0" Text="0"></asp:ListItem>
                                <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                <asp:ListItem Value="5" Text="5"></asp:ListItem>
                                <asp:ListItem Value="6" Text="6"></asp:ListItem>
                                <asp:ListItem Value="7" Text="7"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label46" runat="server" Text="Días Cobranza: "></asp:Label></div>
                        <div class="col-lg-4 col-sm-4 text-left">
                            <asp:DropDownList CssClass="input-small" ID="ddlCobranzaMod" runat="server">
                                <asp:ListItem Value="0" Text="0"></asp:ListItem>
                                <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                <asp:ListItem Value="5" Text="5"></asp:ListItem>
                                <asp:ListItem Value="6" Text="6"></asp:ListItem>
                                <asp:ListItem Value="7" Text="7"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label47" runat="server" Text="Política de Pago: "></asp:Label></div>
                        <div class="col-lg-4 col-sm-4 text-left">
                            <asp:DropDownList CssClass="input-medium" ID="ddlPoliticaMod"  runat="server" DataSourceID="SqlDataSource3" DataTextField="descripcion" DataValueField="id_politica" ></asp:DropDownList>
                            <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select 0 as id_politica, 'Seleccione Política' as descripcion union all select id_politica,descripcion+' ('+clv_politica+' - '+Convert(char(5),dias_plazo)+')' as descripcion from politica_pago"></asp:SqlDataSource>
                        </div>
                        <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label48" runat="server" Text="Proveedor Autorizado: "></asp:Label></div>
                        <div class="col-lg-4 col-sm-4 text-left"><asp:CheckBox ID="chbAseguradoraMod" runat="server"  /></div>
                    </div>
                    <div class="row">
                        <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label49" runat="server" Text="Porcentaje Descuento: "></asp:Label></div>
                        <div class="col-lg-4 col-sm-4 text-left"><telerik:RadNumericTextBox ID="txtDescMod" runat="server" MinValue="0" MaxValue="100" MaxLength="6" NumberFormat-GroupSizes="3" EmptyMessage="Descuento" NumberFormat-DecimalDigits="2"  Skin="MetroTouch"></telerik:RadNumericTextBox></div>
                        <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label50" runat="server" Text="Correo: "></asp:Label></div>
                        <div class="col-lg-4 col-sm-4 text-left"><asp:TextBox ID="txtCorreoMod" runat="server" MaxLength="250" CssClass="input-xlarge" placeholder="Correo"></asp:TextBox></div>
                    </div>
                        <div class="row">
                        <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label1" runat="server" Text="Región: "></asp:Label></div>
                        <div class="col-lg-4 col-sm-4 text-left">
                            <asp:DropDownList CssClass="input-medium" ID="ddlZona"  runat="server" DataSourceID="SqlDataSource2" DataTextField="descripcion" DataValueField="id_zona" ></asp:DropDownList>
                            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select 0 as id_zona, 'Seleccione Región' as descripcion union all select id_zona,descripcion from cat_zona_cliprov order by 1"></asp:SqlDataSource>
                        </div>
                        <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label2" runat="server" Text="Número Proveedor: "></asp:Label></div>
                        <div class="col-lg-4 col-sm-4 text-left">
                            <div class="col-lg-4 col-sm-4 text-left"><asp:TextBox ID="txtNoProveedor" runat="server" MaxLength="50" CssClass="input-medium" placeholder="Número Proveedor"></asp:TextBox></div>
                        </div>                        
                    </div>
                    <div class="row pad1m">
                        <div class="col-lg-12 col-sm-12 text-center">                            
                            <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="edita" CssClass="errores " DisplayMode="List" />
                            <asp:Label ID="lblError" runat="server" CssClass="errores" />
                        </div>
                    </div>                      
                    <div class="row pad1m">
                        <div class="col-lg-4 col-sm-4 text-center"></div>
                        <div class="col-lg-2 col-sm-2 text-center"><asp:LinkButton ID="lnkAceptar" runat="server" CssClass="btn btn-success t14" ValidationGroup="edita" OnClick="lnkAceptar_Click"><i class="fa fa-save"></i><span>&nbsp; Guardar</span></asp:LinkButton></div>
                        <div class="col-lg-2 col-sm-2 text-center"><asp:LinkButton ID="lnkCancelar" runat="server" CssClass="btn btn-danger t14" OnClick="lnkCancelar_Click"><i class="fa fa-remove"></i><span>&nbsp; Cancelar</span></asp:LinkButton></div>
                        <div class="col-lg-4 col-sm-4 text-center"></div>
                    </div>
                    </div>
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanelControl">
                        <ProgressTemplate>
                            <asp:Panel ID="pnlMaskLoad" runat="server" CssClass="maskLoad"></asp:Panel>
                            <asp:Panel ID="pnlCargando" runat="server" CssClass="pnlPopUpLoad">
                                <asp:Image ID="imgLoad" runat="server" ImageUrl="~/img/loading.gif" CssClass="ancho100" />
                            </asp:Panel>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
    </telerik:RadWindow>

    <h1 class="centrado textoCentrado colorMoncarAzul">Proveedores</h1>
    <asp:Panel ID="pnlCatalogos" runat="server" CssClass="panelCatalogos textoCentrado enlinea" ScrollBars="Auto">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="row pad1m">
                    <div class="col-lg-12 col-sm-12 text-right">                
                        <asp:LinkButton ID="lnkAgregar" runat="server" CssClass="btn btn-primary t14" OnClick="lnkAgregar_Click"><i class="fa fa-plus-circle"></i><span>&nbsp;Agregar Proveedor</span></asp:LinkButton>
                    </div>
                </div>
                <div class="row pad1m">
                    <div class="col-lg-12 col-sm-12 text-center"><asp:Label ID="lblErrorGral" runat="server" CssClass="errores" /></div>
                </div>
            
        <asp:Panel ID="Panel1" runat="server" CssClass="row">
            <div class="col-lg-12 col-sm-12">
                <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" runat="server" CssClass="RadGrid" GridLines="None" AllowPaging="True" PageSize="100" AllowSorting="True" AutoGenerateColumns="False" Skin="Metro"
                    AllowAutomaticUpdates="True" DataSourceID="SqlDataSource1" AllowFilteringByColumn="true" EnableHeaderContextMenu="true" PagerStyle-AlwaysVisible="true" EnableHeaderContextFilterMenu="true" OnItemDataBound="RadGrid1_ItemDataBound">
                    <MasterTableView CommandItemDisplay="None" DataSourceID="SqlDataSource1" DataKeyNames="id_cliprov">
                        <Columns>                                    
                            <telerik:GridBoundColumn UniqueName="id_cliprov" HeaderText="Clave" Visible="false" DataField="id_cliprov" FilterCheckListEnableLoadOnDemand="true" FilterControlAltText="Filtro Clave"><HeaderStyle Width="150px" /></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="rfc" HeaderText="R.F.C." DataField="rfc" FilterCheckListEnableLoadOnDemand="true" FilterControlAltText="Filtro R.F.C."><HeaderStyle Width="150px" /></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="razon_social" HeaderText="Proveedor" DataField="razon_social" FilterCheckListEnableLoadOnDemand="true" FilterControlAltText="Filtro Proveedor"><HeaderStyle Width="300px" /></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="correo" HeaderText="Correo" DataField="correo" FilterCheckListEnableLoadOnDemand="true" FilterControlAltText="Filtro Correo"><HeaderStyle Width="300px" /></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="zona" HeaderText="Zona" DataField="zona" FilterCheckListEnableLoadOnDemand="true" FilterControlAltText="Filtro Zona"><HeaderStyle Width="300px" /></telerik:GridBoundColumn>
                            <telerik:GridCheckBoxColumn UniqueName="aseguradora" HeaderText="Autorizado" DataField="aseguradora" FilterCheckListEnableLoadOnDemand="true" FilterControlAltText="Filtro Autorizado"><HeaderStyle Width="100px" /></telerik:GridCheckBoxColumn>
                            <telerik:GridTemplateColumn HeaderText="">
                                <HeaderStyle  Width="50px"/>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEditar" runat="server" CommandArgument='<%# Eval("id_cliprov") %>' OnClick="lnkEditar_Click" CssClass="btn btn-warning"><i class="fa fa-edit"></i></asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="">
                                <HeaderStyle  Width="50px"/>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkActivar" runat="server" CommandArgument='<%# Eval("id_cliprov") %>' CssClass="btn btn-info" OnClick="lnkActivar_Click" OnClientClick="return confirm('¿Está seguro de reactivar el proveedor?')"><i class="fa fa-level-up"></i></asp:LinkButton>
                                    <asp:LinkButton ID="lnkInactiva" runat="server" CommandArgument='<%# Eval("id_cliprov") %>' CssClass="btn btn-info" OnClick="lnkInactiva_Click" OnClientClick="return confirm('¿Está seguro de inactivar el proveedor?')"><i class="fa fa-level-down"></i></asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="">
                                <HeaderStyle  Width="50px"/>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEliminar" runat="server" CommandArgument='<%# Eval("id_cliprov") %>' CssClass="btn btn-danger" OnClientClick="return confirm('¿Está seguro de eliminar el proveedor?')" OnClick="lnkEliminar_Click"><i class="fa fa-trash"></i></asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                        <ClientSettings>
                        <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true"></Scrolling>
                    </ClientSettings>                        
                    <PagerStyle PageSizeControlType="RadDropDownList" Mode="NextPrevAndNumeric"></PagerStyle>
                </telerik:RadGrid>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select c.id_cliprov,c.rfc,c.razon_social,c.correo,c.aseguradora, z.descripcion as zona from cliprov c left join cat_zona_cliprov z on z.id_zona=c.id_zona where c.tipo='P'"></asp:SqlDataSource>
            </div>
        </asp:Panel>
                <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                        <ProgressTemplate>
                            <asp:Panel ID="pnlMaskLoad1" runat="server" CssClass="maskLoad"></asp:Panel>
                            <asp:Panel ID="pnlCargando1" runat="server" CssClass="pnlPopUpLoad">
                                <asp:Image ID="imgLoad1" runat="server" ImageUrl="~/img/loading.gif" CssClass="ancho100" />
                            </asp:Panel>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>
