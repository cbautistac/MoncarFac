<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Empresas.aspx.cs" Inherits="Empresas"
    MasterPageFile="~/Administracion.master" UICulture="es-Mx" Culture="es-Mx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function abreNewEmi() {
            var oWnd = $find("<%=modalNuevo.ClientID%>");
            oWnd.setUrl('');
            oWnd.show();
        }

        function cierraNewEmi() {
            var oWnd = $find("<%=modalNuevo.ClientID%>");
            oWnd.close();
        }

        function abreModEmi() {
            var oWnd = $find("<%=modalModifica.ClientID%>");
            oWnd.setUrl('');
            oWnd.show();
        }

        function cierraModEmi() {
            var oWnd = $find("<%=modalModifica.ClientID%>");
            oWnd.close();
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" />    
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
    <telerik:RadFormDecorator ID="RadFormDecorator1" RenderMode="Lightweight" runat="server" DecoratedControls="Buttons" />

    <%-- Nueva Empresa --%>
    <telerik:RadWindow RenderMode="Lightweight" ID="modalNuevo" Title="Nueva Empresa" EnableShadow="true" Skin="Metro"
        Behaviors="Move,Close,Resize,Pin" VisibleOnPageLoad="false" ShowContentDuringLoad="false" DestroyOnClose="true" 
        Animation="Fade" runat="server" Modal="true" Width="800px" Height="590px" Style="z-index: 1000;">
        <ContentTemplate>
            <asp:UpdatePanel ID="UpdatePanelEmi" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pnlPrincipal" runat="server" CssClass="ancho100 text-center">
                        <div class="col-lg-12 col-sm-12 text-center">
                            <asp:Label ID="lblErrorNuevo" runat="server" CssClass="errores"></asp:Label>
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="agrega" CssClass="errores" DisplayMode="List" />
                        </div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label2" runat="server" Text="Persona:" CssClass="textoBold" /></div>
                        <div class="col-lg-3 col-sm-3 text-left">
                            <asp:RadioButtonList ID="rbtnPersona" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rbtnPersona_SelectedIndexChanged" CssClass="centrado">
                                <asp:ListItem Value="F" Selected="True" Text="Física" />
                                <asp:ListItem Value="M" Text="Moral" />
                            </asp:RadioButtonList>
                        </div>
                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label3" runat="server" Text="R.F.C.:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-3 col-sm-3 text-left">
                            <asp:TextBox ID="txtRfc" runat="server" MaxLength="13" CssClass="input-medium" />
                            <cc1:TextBoxWatermarkExtender ID="txtRfcWatermarkExtender1" runat="server" BehaviorID="txtRfc_TextBoxWatermarkExtender" TargetControlID="txtRfc" WatermarkCssClass="input-medium water" WatermarkText="R.F.C." />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Debe indicar el R.F.C." CssClass="errores alineado" ValidationGroup="agrega" ControlToValidate="txtRfc" Text="*"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ControlToValidate="txtRfc" ID="RegularExpressionValidator1" runat="server" ErrorMessage="El R.F.C. tiene un formato invalido" ValidationGroup="agrega" ValidationExpression="^[A-Za-z]{3,4}[0-9]{6}[0-9A-Za-z]{3}$" Text="*" CssClass="errores" />
                        </div>  
                                              
                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label4" runat="server" Text="Razón Social:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtRazon" runat="server" CssClass="input-xxlarge" MaxLength="128" />
                            <cc1:TextBoxWatermarkExtender ID="txtRazon_TextBoxWatermarkExtender" runat="server" BehaviorID="txtRazon_TextBoxWatermarkExtender" TargetControlID="txtRazon" WatermarkCssClass="input-xxlarge water" WatermarkText="Razón Social" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Debe indicar la Razón Social" Text="*" ValidationGroup="agrega" ControlToValidate="txtRazon" CssClass="alineado errores" ></asp:RequiredFieldValidator>                            
                        </div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label5" runat="server" Text="Calle:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtCalle" runat="server" MaxLength="100" CssClass="input-xxlarge"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="txtCalle_TextBoxWatermarkExtender" runat="server" BehaviorID="txtCalle_TextBoxWatermarkExtender" TargetControlID="txtCalle" WatermarkText="Calle" WatermarkCssClass="water input-xxlarge" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Debe indicar la Calle." Text="*" CssClass="alineado errores" ControlToValidate="txtCalle" ValidationGroup="agrega"></asp:RequiredFieldValidator>
                        </div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label6" runat="server" Text="No. Ext.:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-3 col-sm-3 text-left">
                            <asp:TextBox ID="txtNoExt" runat="server" MaxLength="20" CssClass="input-small"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="txtNoExt_TextBoxWatermarkExtender" runat="server" BehaviorID="txtNoExt_TextBoxWatermarkExtender" TargetControlID="txtNoExt" WatermarkText="Num. Ext." WatermarkCssClass="water input-small" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Debe indicar la No. Exterior." Text="*" CssClass="alineado errores" ControlToValidate="txtNoExt" ValidationGroup="agrega"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label7" runat="server" Text="No. Int.:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-3 col-sm-3 text-left">
                            <asp:TextBox ID="txtNoInt" runat="server" MaxLength="20" CssClass="input-small"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="txtNoInt_TextBoxWatermarkExtender" runat="server" BehaviorID="txtNoInt_TextBoxWatermarkExtender" TargetControlID="txtNoInt" WatermarkText="Num. Int." WatermarkCssClass="water input-small" />
                        </div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label8" runat="server" Text="País:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-3 col-sm-3 text-left">
                            <telerik:RadComboBox RenderMode="Lightweight" ID="ddlPais" runat="server" Width="200" Height="200px" DataValueField="cve_pais" AutoPostBack="true" MarkFirstMatch="true" AllowCustomText="true" AutoCompleteSeparator="None" Filter="Contains" 
                                EmptyMessage="Seleccione País..." DataSourceID="SqlDataSource10" DataTextField="desc_pais" Skin="Silk" OnSelectedIndexChanged="ddlPais_SelectedIndexChanged">
                            </telerik:RadComboBox>
                            <asp:SqlDataSource ID="SqlDataSource10" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select cve_pais,desc_pais from Paises_f"></asp:SqlDataSource>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Debe indicar el País." Text="*" CssClass="errores" ControlToValidate="ddlPais" ValidationGroup="agrega"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label9" runat="server" Text="Estado:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-3 col-sm-3 text-left">                            
                            <telerik:RadComboBox RenderMode="Lightweight" ID="ddlEstado" runat="server" Width="200" Height="200px" DataValueField="cve_edo" AutoPostBack="true" MarkFirstMatch="true" AllowCustomText="true" AutoCompleteSeparator="None" Filter="Contains"
                                EmptyMessage="Seleccione Estado..." DataSourceID="SqlDataSource11" DataTextField="nom_edo" Skin="Silk" OnSelectedIndexChanged="ddlEstado_SelectedIndexChanged">
                            </telerik:RadComboBox>
                            <asp:SqlDataSource ID="SqlDataSource11" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select cve_edo,nom_edo from Estados where cve_pais=@pais">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="ddlPais" Name="pais" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="Debe indicar el Estado." Text="*" CssClass="errores" ControlToValidate="ddlEstado" ValidationGroup="agrega"></asp:RequiredFieldValidator>
                        </div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label10" runat="server" Text="Deleg./Mun.:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-3 col-sm-3 text-left">
                            <telerik:RadComboBox RenderMode="Lightweight" ID="ddlMunicipio" runat="server" Width="200" Height="200px" DataValueField="ID_Del_Mun" AutoPostBack="true" MarkFirstMatch="true" AllowCustomText="true" AutoCompleteSeparator="None" Filter="Contains"
                                EmptyMessage="Seleccione Deleg./Mun. ..." DataSourceID="SqlDataSource12" DataTextField="Desc_Del_Mun" Skin="Silk" OnSelectedIndexChanged="ddlMunicipio_SelectedIndexChanged">
                            </telerik:RadComboBox>
                            <asp:SqlDataSource ID="SqlDataSource12" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select ID_Del_Mun,Desc_Del_Mun from DelegacionMunicipio where cve_pais=@pais and ID_Estado=@estado">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="ddlPais" Name="pais" />
                                    <asp:ControlParameter ControlID="ddlEstado" Name="estado" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="Debe indicar la Delegación o Municipio." Text="*" CssClass="alineado errores" ControlToValidate="ddlMunicipio" ValidationGroup="agrega"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label11" runat="server" Text="Colonia:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-3 col-sm-3 text-left">
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
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="Debe indicar la Colonia." Text="*" CssClass="alineado errores" ControlToValidate="ddlColonia" ValidationGroup="agrega"></asp:RequiredFieldValidator>
                        </div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label12" runat="server" Text="C.P.:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <telerik:RadComboBox RenderMode="Lightweight" ID="ddlCodigo" runat="server" Width="200" Height="200px" DataValueField="ID_Cod_Pos" AutoPostBack="true" MarkFirstMatch="true" AllowCustomText="true" AutoCompleteSeparator="None" Filter="Contains"
                                EmptyMessage="Seleccione C.P. ..." DataSourceID="SqlDataSource14" DataTextField="ID_Cod_Pos" Skin="Silk">
                            </telerik:RadComboBox>
                            <asp:SqlDataSource ID="SqlDataSource14" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select ID_Cod_Pos from Colonias where cve_pais=@pais and ID_Estado=@estado and ID_Del_Mun=@municipio and ID_Colonia=@colonia">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="ddlPais" Name="pais" />
                                    <asp:ControlParameter ControlID="ddlEstado" Name="estado" />
                                    <asp:ControlParameter ControlID="ddlMunicipio" Name="municipio" />
                                    <asp:ControlParameter ControlID="ddlColonia" Name="colonia" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="Debe indicar el Código Postal." Text="*" CssClass="alineado errores" ControlToValidate="ddlCodigo" ValidationGroup="agrega"></asp:RequiredFieldValidator>
                        </div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label13" runat="server" Text="Localidad:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtLocalidad" runat="server" MaxLength="50" CssClass="input-large"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="txtLocalidad_TextBoxWatermarkExtender" runat="server" BehaviorID="txtLocalidad_TextBoxWatermarkExtender" TargetControlID="txtLocalidad" WatermarkText="Localidad" WatermarkCssClass="water input-large" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="Debe indicar la Localidad." Text="*" CssClass="alineado errores" ControlToValidate="txtLocalidad" ValidationGroup="agrega"></asp:RequiredFieldValidator>
                        </div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label14" runat="server" Text="Referencia:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtReferencia" runat="server" MaxLength="100" CssClass="input-large"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="txtReferencia_TextBoxWatermarkExtender" runat="server" BehaviorID="txtReferencia_TextBoxWatermarkExtender" TargetControlID="txtReferencia" WatermarkText="Referencia" WatermarkCssClass="water input-large" />
                        </div>

                        <div class="col-lg-12 col-sm-12 text-center alert-info"><asp:Label ID="Label15" runat="server" Text="Lugar de Expedición" CssClass="textoBold"></asp:Label></div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label16" runat="server" Text="Calle:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtCalleEx" runat="server" MaxLength="100" CssClass="input-xxlarge"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" BehaviorID="txtCalleEx_TextBoxWatermarkExtender" TargetControlID="txtCalleEx" WatermarkText="Calle" WatermarkCssClass="water input-xxlarge" />                            
                        </div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label17" runat="server" Text="No. Ext.:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-3 col-sm-3 text-left">
                            <asp:TextBox ID="txtNoExtEx" runat="server" MaxLength="20" CssClass="input-small"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" BehaviorID="txtNoExtEx_TextBoxWatermarkExtender" TargetControlID="txtNoExtEx" WatermarkText="Num. Ext." WatermarkCssClass="water input-small" />                            
                        </div>
                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label18" runat="server" Text="No. Int.:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-3 col-sm-3 text-left">
                            <asp:TextBox ID="txtNoIntEx" runat="server" MaxLength="20" CssClass="input-small"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server" BehaviorID="txtNoIntEx_TextBoxWatermarkExtender" TargetControlID="txtNoIntEx" WatermarkText="Num. Int." WatermarkCssClass="water input-small" />
                        </div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label19" runat="server" Text="País:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-3 col-sm-3 text-left">
                            <telerik:RadComboBox RenderMode="Lightweight" ID="ddlPaisEx" runat="server" Width="200" Height="200px" DataValueField="cve_pais" AutoPostBack="true" MarkFirstMatch="true" AllowCustomText="true" AutoCompleteSeparator="None" Filter="Contains" 
                                EmptyMessage="Seleccione País..." DataSourceID="SqlDataSource2" DataTextField="desc_pais" Skin="Silk" OnSelectedIndexChanged="ddlPaisEx_SelectedIndexChanged">
                            </telerik:RadComboBox>
                            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select cve_pais,desc_pais from Paises_f"></asp:SqlDataSource>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="Debe indicar el País." Text="*" CssClass="errores" ControlToValidate="ddlPaisEx" ValidationGroup="agrega"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label20" runat="server" Text="Estado:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-3 col-sm-3 text-left">                            
                            <telerik:RadComboBox RenderMode="Lightweight" ID="ddlEstadoEx" runat="server" Width="200" Height="200px" DataValueField="cve_edo" AutoPostBack="true" MarkFirstMatch="true" AllowCustomText="true" AutoCompleteSeparator="None" Filter="Contains"
                                EmptyMessage="Seleccione Estado..." DataSourceID="SqlDataSource3" DataTextField="nom_edo" Skin="Silk" OnSelectedIndexChanged="ddlEstadoEx_SelectedIndexChanged">
                            </telerik:RadComboBox>
                            <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select cve_edo,nom_edo from Estados where cve_pais=@pais">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="ddlPaisEx" Name="pais" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ErrorMessage="Debe indicar el Estado." Text="*" CssClass="errores" ControlToValidate="ddlEstadoEx" ValidationGroup="agrega"></asp:RequiredFieldValidator>
                        </div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label21" runat="server" Text="Deleg./Mun.:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-3 col-sm-3 text-left">
                            <telerik:RadComboBox RenderMode="Lightweight" ID="ddlMunicipioEx" runat="server" Width="200" Height="200px" DataValueField="ID_Del_Mun" AutoPostBack="true" MarkFirstMatch="true" AllowCustomText="true" AutoCompleteSeparator="None" Filter="Contains"
                                EmptyMessage="Seleccione Deleg./Mun. ..." DataSourceID="SqlDataSource4" DataTextField="Desc_Del_Mun" Skin="Silk" OnSelectedIndexChanged="ddlMunicipioEx_SelectedIndexChanged">
                            </telerik:RadComboBox>
                            <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select ID_Del_Mun,Desc_Del_Mun from DelegacionMunicipio where cve_pais=@pais and ID_Estado=@estado">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="ddlPaisEx" Name="pais" />
                                    <asp:ControlParameter ControlID="ddlEstadoEx" Name="estado" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ErrorMessage="Debe indicar la Delegación o Municipio." Text="*" CssClass="alineado errores" ControlToValidate="ddlMunicipioEx" ValidationGroup="agrega"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label22" runat="server" Text="Colonia:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-3 col-sm-3 text-left">
                            <telerik:RadComboBox RenderMode="Lightweight" ID="ddlColoniaEx" runat="server" Width="200" Height="200px" DataValueField="ID_Colonia" AutoPostBack="true" MarkFirstMatch="true" AllowCustomText="true" AutoCompleteSeparator="None" Filter="Contains"
                                EmptyMessage="Seleccione Colonia ..." DataSourceID="SqlDataSource5" DataTextField="Desc_Colonia" Skin="Silk" OnSelectedIndexChanged="ddlColoniaEx_SelectedIndexChanged">
                            </telerik:RadComboBox>
                            <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select ID_Colonia,Desc_Colonia from Colonias where cve_pais=@pais and ID_Estado=@estado and ID_Del_Mun=@municipio">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="ddlPaisEx" Name="pais" />
                                    <asp:ControlParameter ControlID="ddlEstadoEx" Name="estado" />
                                    <asp:ControlParameter ControlID="ddlMunicipioEx" Name="municipio" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ErrorMessage="Debe indicar la Colonia." Text="*" CssClass="alineado errores" ControlToValidate="ddlColoniaEx" ValidationGroup="agrega"></asp:RequiredFieldValidator>
                        </div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label23" runat="server" Text="C.P.:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <telerik:RadComboBox RenderMode="Lightweight" ID="ddlCodigoEx" runat="server" Width="200" Height="200px" DataValueField="ID_Cod_Pos" AutoPostBack="true" MarkFirstMatch="true" AllowCustomText="true" AutoCompleteSeparator="None" Filter="Contains"
                                EmptyMessage="Seleccione C.P. ..." DataSourceID="SqlDataSource6" DataTextField="ID_Cod_Pos" Skin="Silk">
                            </telerik:RadComboBox>
                            <asp:SqlDataSource ID="SqlDataSource6" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select ID_Cod_Pos from Colonias where cve_pais=@pais and ID_Estado=@estado and ID_Del_Mun=@municipio and ID_Colonia=@colonia">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="ddlPaisEx" Name="pais" />
                                    <asp:ControlParameter ControlID="ddlEstadoEx" Name="estado" />
                                    <asp:ControlParameter ControlID="ddlMunicipioEx" Name="municipio" />
                                    <asp:ControlParameter ControlID="ddlColoniaEx" Name="colonia" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ErrorMessage="Debe indicar el Código Postal." Text="*" CssClass="alineado errores" ControlToValidate="ddlCodigoEx" ValidationGroup="agrega"></asp:RequiredFieldValidator>
                        </div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label24" runat="server" Text="Localidad:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtLocalidadEx" runat="server" MaxLength="50" CssClass="input-large"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender4" runat="server" BehaviorID="txtLocalidadEx_TextBoxWatermarkExtender" TargetControlID="txtLocalidadEx" WatermarkText="Localidad" WatermarkCssClass="water input-large" />                            
                        </div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label25" runat="server" Text="Referencia:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtReferenciaEx" runat="server" MaxLength="50" CssClass="input-large"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender5" runat="server" BehaviorID="txtReferenciaEx_TextBoxWatermarkExtender" TargetControlID="txtReferenciaEx" WatermarkText="Referencia" WatermarkCssClass="water input-large" />
                        </div>

                        <div class="col-lg-12 col-sm-12 text-center alert-info"><asp:Label ID="Label26" runat="server" Text="Parámetros de Correo" CssClass="textoBold"></asp:Label></div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label27" runat="server" Text="Tipo Servidor:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-3 col-sm-3 text-left">
                            <asp:DropDownList ID="ddlServidor" runat="server" CssClass="input-medium">
                                <asp:ListItem Text="SMTP" Value="1" Selected="True"/>
                                <asp:ListItem Text="POP3" Value="2" />
                                <asp:ListItem Text="IMAP" Value="3" />
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label28" runat="server" Text="Servidor:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-3 col-sm-3 text-left">
                            <asp:TextBox ID="txtServidor" runat="server" MaxLength="100" CssClass="input-large"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender6" runat="server" BehaviorID="txtServidor_TextBoxWatermarkExtender" TargetControlID="txtServidor" WatermarkText="Servidor" WatermarkCssClass="water input-large" />
                        </div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label29" runat="server" Text="Usuario:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-3 col-sm-3 text-left">
                            <asp:TextBox ID="txtUsuario" runat="server" MaxLength="50" CssClass="input-large"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender7" runat="server" BehaviorID="txtUsuario_TextBoxWatermarkExtender" TargetControlID="txtUsuario" WatermarkText="Usuario" WatermarkCssClass="water input-large" />
                        </div>
                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label30" runat="server" Text="Contraseña:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-3 col-sm-3 text-left">
                            <asp:TextBox ID="txtContrasena" runat="server" MaxLength="50" CssClass="input-large" TextMode="Password" placeholder="Contraseña"></asp:TextBox>                            
                        </div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label31" runat="server" Text="Correo:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-3 col-sm-3 text-left">
                            <asp:TextBox ID="txtCorreo" runat="server" MaxLength="50" CssClass="input-large"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender8" runat="server" BehaviorID="txtCorreo_TextBoxWatermarkExtender" TargetControlID="txtCorreo" WatermarkText="Correo" WatermarkCssClass="water input-large" />
                        </div>
                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label32" runat="server" Text="Correo CC:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-3 col-sm-3 text-left">
                            <asp:TextBox ID="txtCorreoCC" runat="server" MaxLength="100" CssClass="input-large"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender9" runat="server" BehaviorID="txtCorreoCC_TextBoxWatermarkExtender" TargetControlID="txtCorreoCC" WatermarkText="Correo CC" WatermarkCssClass="water input-large" />
                        </div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label33" runat="server" Text="Correo CCO:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-3 col-sm-3 text-left">
                            <asp:TextBox ID="txtCorreoCCO" runat="server" MaxLength="100" CssClass="input-large"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender10" runat="server" BehaviorID="txtCorreoCCO_TextBoxWatermarkExtender" TargetControlID="txtCorreoCCO" WatermarkText="Correo CCO" WatermarkCssClass="water input-large" />
                        </div>
                        <div class="col-lg-1 col-sm-1 text-left"><asp:Label ID="Label39" runat="server" Text="SSL:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-1 col-sm-1 text-left">
                            <asp:CheckBox ID="chkCifrado" runat="server" />
                        </div>
                        <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label40" runat="server" Text="Puerto:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-2 col-sm-2 text-left">
                            <asp:TextBox ID="txtPuerto" runat="server" MaxLength="4" CssClass="input-small"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" BehaviorID="txtPuerto_FilteredTextBoxExtender" TargetControlID="txtPuerto" FilterType="Numbers" />
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender13" runat="server" BehaviorID="txtPuerto_TextBoxWatermarkExtender" TargetControlID="txtPuerto" WatermarkText="Puerto" WatermarkCssClass="water input-small" />
                        </div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label41" runat="server" Text="Mensaje Correo:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtMsgCorreo" runat="server" CssClass="textNota" TextMode="MultiLine" Rows="5"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender14" runat="server" BehaviorID="txtMsgCorreo_TextBoxWatermarkExtender" TargetControlID="txtMsgCorreo" WatermarkText="Mensaje de Correo" WatermarkCssClass="water textNota" />
                        </div>
                        
                        <div class="col-lg-12 col-sm-12 text-center alert-info"><asp:Label ID="Label34" runat="server" Text="Generales" CssClass="textoBold"></asp:Label></div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label35" runat="server" Text="Nombre Corto:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-3 col-sm-3 text-left">
                            <asp:TextBox ID="txtNomCorto" runat="server" MaxLength="100" CssClass="input-large"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender11" runat="server" BehaviorID="txtNomCorto_TextBoxWatermarkExtender" TargetControlID="txtNomCorto" WatermarkText="Nombre Corto" WatermarkCssClass="water input-large" />
                        </div>
                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label37" runat="server" Text="Tel. Principal:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-3 col-sm-3 text-left">
                            <asp:TextBox ID="txtTel1" runat="server" MaxLength="20" CssClass="input-medium"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="txtTel1Mod_FilteredTextBoxExtender" runat="server" BehaviorID="txtTel1Mod_FilteredTextBoxExtender" TargetControlID="txtTel1" FilterType="Numbers" />
                            <cc1:TextBoxWatermarkExtender ID="txtTel1Mod_TextBoxWatermarkExtender" runat="server" BehaviorID="txtTel1Mod_TextBoxWatermarkExtender" TargetControlID="txtTel1" WatermarkText="Tel. Principal" WatermarkCssClass="water input-medium" />
                        </div>
                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label38" runat="server" Text="Tel. Oficina:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtTel2" runat="server" MaxLength="20" CssClass="input-medium"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" BehaviorID="txtTel2Mod_FilteredTextBoxExtender" TargetControlID="txtTel2" FilterType="Numbers" />
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender12" runat="server" BehaviorID="txtTel2Mod_TextBoxWatermarkExtender" TargetControlID="txtTel2" WatermarkText="Tel. Oficina" WatermarkCssClass="water input-medium" />
                        </div>
                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label36" runat="server" Text="Logo:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <telerik:RadAsyncUpload RenderMode="Lightweight" runat="server" Culture="es-Mx" CssClass="async-attachment col-lg-12 col-sm-12"
                                MaxFileInputsCount="1" MultipleFileSelection="Disabled" ID="AsyncUpload1" HideFileInput="true"
                                AllowedFileExtensions=".jpeg,.jpg,.png,.gif,.bmp,.tiff,.JPEG,.JPG,.PNG,.GIF,.BMP,.TIFF">
                            </telerik:RadAsyncUpload>
                        </div>


                        <div class="col-lg-12 col-sm-12 text-center alert-info"><asp:Label ID="Label42" runat="server" Text="Parametros de Facturación" CssClass="textoBold"></asp:Label></div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label43" runat="server" Text="Certificado:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <telerik:RadAsyncUpload RenderMode="Lightweight" runat="server" Culture="es-Mx" CssClass="async-attachment"
                                MaxFileInputsCount="1" MultipleFileSelection="Disabled" ID="RadAsyncUploadCer" HideFileInput="true"
                                AllowedFileExtensions=".cer,.CER">
                            </telerik:RadAsyncUpload>
                        </div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label44" runat="server" Text="Llave:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <telerik:RadAsyncUpload RenderMode="Lightweight" runat="server" Culture="es-Mx" CssClass="async-attachment"
                                MaxFileInputsCount="1" MultipleFileSelection="Disabled" ID="RadAsyncUploadKey" HideFileInput="true"
                                AllowedFileExtensions=".key,.KEY">
                            </telerik:RadAsyncUpload>
                        </div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label45" runat="server" Text="PFX:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <telerik:RadAsyncUpload RenderMode="Lightweight" runat="server" Culture="es-Mx" CssClass="async-attachment"
                                MaxFileInputsCount="1" MultipleFileSelection="Disabled" ID="RadAsyncUploadPfx" HideFileInput="true"
                                AllowedFileExtensions=".pfx,.PFX">
                            </telerik:RadAsyncUpload>
                        </div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label46" runat="server" Text="Contraseña Llave:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtPassLlave" runat="server" MaxLength="50" CssClass="input-large" TextMode="Password" placeholder="Contraseña Llave" ></asp:TextBox> 
                        </div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label47" runat="server" Text="Usuario WS:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtUserWs" runat="server" CssClass="input-large" MaxLength="50"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender15" runat="server" BehaviorID="txtUserWs_TextBoxWatermarkExtender" TargetControlID="txtUserWs" WatermarkText="Usuario WS" WatermarkCssClass="water input-large" />
                        </div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label48" runat="server" Text="Contraseña WS:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtPassWs" runat="server" MaxLength="50" CssClass="input-large" TextMode="Password" placeholder="Contraseña WS"></asp:TextBox>                            
                        </div>
                        <div class="col-lg-6 col-sm-6 text-center pad1m">
                            <asp:LinkButton ID="lnkAgregarNuevo" runat="server" CssClass="btn btn-success t14" ValidationGroup="agrega" OnClick="lnkAgregarNuevo_Click"><i class="fa fa-check-circle"></i>&nbsp; Agregar</asp:LinkButton>
                        </div>
                        <div class="col-lg-6 col-sm-6 text-center pad1m">
                            <asp:LinkButton ID="lnkCancelarNuevo" runat="server" CssClass="btn btn-danger t14" OnClientClick="cierraNewEmi()"><i class="fa fa-remove"></i>&nbsp; Cancelar</asp:LinkButton>
                        </div>
                    </asp:Panel>
                    <asp:UpdateProgress ID="updProgEmi" runat="server" AssociatedUpdatePanelID="UpdatePanelEmi">
                        <ProgressTemplate>
                            <asp:Panel ID="pnlMaskLoadEmi" runat="server" CssClass="maskLoad" />
                            <asp:Panel ID="pnlCargandoEmi" runat="server" CssClass="pnlPopUpLoad">
                                <asp:Image ID="imgLoadEmi" runat="server" ImageUrl="~/img/loading.gif" CssClass="ancho100" />
                            </asp:Panel>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
    </telerik:RadWindow>


    <%-- Modifica Empresa --%>
    <telerik:RadWindow RenderMode="Lightweight" ID="modalModifica" Title="Edita Empresa" EnableShadow="true" Skin="Metro"
        Behaviors="Move,Resize,Pin" VisibleOnPageLoad="false" ShowContentDuringLoad="false" DestroyOnClose="true" 
        Animation="Fade" runat="server" Modal="true" Width="800px" Height="590px" Style="z-index: 1000;">
        <ContentTemplate>
            <asp:UpdatePanel ID="UpdatePanelEmiMod" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pnlPrincipalMod" runat="server" CssClass="ancho100 text-center">
                        <div class="col-lg-12 col-sm-12 text-center">
                            <asp:Label ID="lblIdEmisorMod" runat="server" Visible="false"></asp:Label>
                            <asp:Label ID="lblErrorModifica" runat="server" CssClass="errores"></asp:Label>
                            <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="edita" CssClass="errores" DisplayMode="List" />
                        </div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="LabelMod2" runat="server" Text="Persona:" CssClass="textoBold" /></div>
                        <div class="col-lg-3 col-sm-3 text-left">
                            <asp:RadioButtonList ID="rbtnPersonaMod" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rbtnPersonaMod_SelectedIndexChanged" CssClass="centrado">
                                <asp:ListItem Value="F" Selected="True" Text="Física" />
                                <asp:ListItem Value="M" Text="Moral" />
                            </asp:RadioButtonList>
                        </div>
                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="LabelMod3" runat="server" Text="R.F.C.:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-3 col-sm-3 text-left">
                            <asp:TextBox ID="txtRfcMod" runat="server" MaxLength="13" CssClass="input-medium" />
                            <cc1:TextBoxWatermarkExtender ID="txtRfcModWatermarkExtender1" runat="server" BehaviorID="txtRfcMod_TextBoxWatermarkExtender" TargetControlID="txtRfcMod" WatermarkCssClass="input-medium water" WatermarkText="R.F.C." />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorMod1" runat="server" ErrorMessage="Debe indicar el R.F.C." CssClass="errores alineado" ValidationGroup="edita" ControlToValidate="txtRfcMod" Text="*"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ControlToValidate="txtRfcMod" ID="RegularExpressionValidatorMod1" runat="server" ErrorMessage="El R.F.C. tiene un formato invalido" ValidationGroup="edita" ValidationExpression="^[A-Za-z]{3,4}[0-9]{6}[0-9A-Za-z]{3}$" Text="*" CssClass="errores" />
                        </div>  
                                              
                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="LabelMod4" runat="server" Text="Razón Social:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtRazonMod" runat="server" CssClass="input-xxlarge" MaxLength="128" />
                            <cc1:TextBoxWatermarkExtender ID="txtRazonMod_TextBoxWatermarkExtender" runat="server" BehaviorID="txtRazonMod_TextBoxWatermarkExtender" TargetControlID="txtRazonMod" WatermarkCssClass="input-xxlarge water" WatermarkText="Razón Social" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorMod2" runat="server" ErrorMessage="Debe indicar la Razón Social" Text="*" ValidationGroup="edita" ControlToValidate="txtRazonMod" CssClass="alineado errores" ></asp:RequiredFieldValidator>                           
                        </div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="LabelMod5" runat="server" Text="Calle:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtCalleMod" runat="server" MaxLength="100" CssClass="input-xxlarge"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="txtCalleMod_TextBoxWatermarkExtender" runat="server" BehaviorID="txtCalleMod_TextBoxWatermarkExtender" TargetControlID="txtCalleMod" WatermarkText="Calle" WatermarkCssClass="water input-xxlarge" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorMod5" runat="server" ErrorMessage="Debe indicar la Calle." Text="*" CssClass="alineado errores" ControlToValidate="txtCalleMod" ValidationGroup="edita"></asp:RequiredFieldValidator>
                        </div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="LabelMod6" runat="server" Text="No. Ext.:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-3 col-sm-3 text-left">
                            <asp:TextBox ID="txtNoExtMod" runat="server" MaxLength="20" CssClass="input-small"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="txtNoExtMod_TextBoxWatermarkExtender" runat="server" BehaviorID="txtNoExtMod_TextBoxWatermarkExtender" TargetControlID="txtNoExtMod" WatermarkText="Num. Ext." WatermarkCssClass="water input-small" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorMod6" runat="server" ErrorMessage="Debe indicar la No. Exterior." Text="*" CssClass="alineado errores" ControlToValidate="txtNoExtMod" ValidationGroup="edita"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="LabelMod7" runat="server" Text="No. Int.:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-3 col-sm-3 text-left">
                            <asp:TextBox ID="txtNoIntMod" runat="server" MaxLength="20" CssClass="input-small"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="txtNoIntMod_TextBoxWatermarkExtender" runat="server" BehaviorID="txtNoIntMod_TextBoxWatermarkExtender" TargetControlID="txtNoIntMod" WatermarkText="Num. Int." WatermarkCssClass="water input-small" />
                        </div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="LabelMod8" runat="server" Text="País:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-3 col-sm-3 text-left">
                            <telerik:RadComboBox RenderMode="Lightweight" ID="ddlPaisMod" runat="server" Width="200" Height="200px" DataValueField="cve_pais" AutoPostBack="true" MarkFirstMatch="true" AllowCustomText="true" AutoCompleteSeparator="None" Filter="Contains" 
                                EmptyMessage="Seleccione País..." DataSourceID="SqlDataSourceMod10" DataTextField="desc_pais" Skin="Silk" OnSelectedIndexChanged="ddlPaisMod_SelectedIndexChanged">
                            </telerik:RadComboBox>
                            <asp:SqlDataSource ID="SqlDataSourceMod10" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select cve_pais,desc_pais from Paises_f"></asp:SqlDataSource>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorMod7" runat="server" ErrorMessage="Debe indicar el País." Text="*" CssClass="errores" ControlToValidate="ddlPaisMod" ValidationGroup="edita"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="LabelMod9" runat="server" Text="Estado:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-3 col-sm-3 text-left">                            
                            <telerik:RadComboBox RenderMode="Lightweight" ID="ddlEstadoMod" runat="server" Width="200" Height="200px" DataValueField="cve_edo" AutoPostBack="true" MarkFirstMatch="true" AllowCustomText="true" AutoCompleteSeparator="None" Filter="Contains"
                                EmptyMessage="Seleccione Estado..." DataSourceID="SqlDataSourceMod11" DataTextField="nom_edo" Skin="Silk" OnSelectedIndexChanged="ddlEstadoMod_SelectedIndexChanged">
                            </telerik:RadComboBox>
                            <asp:SqlDataSource ID="SqlDataSourceMod11" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select cve_edo,nom_edo from Estados where cve_pais=@pais">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="ddlPaisMod" Name="pais" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorMod8" runat="server" ErrorMessage="Debe indicar el Estado." Text="*" CssClass="errores" ControlToValidate="ddlEstadoMod" ValidationGroup="edita"></asp:RequiredFieldValidator>
                        </div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="LabelMod10" runat="server" Text="Deleg./Mun.:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-3 col-sm-3 text-left">
                            <telerik:RadComboBox RenderMode="Lightweight" ID="ddlMunicipioMod" runat="server" Width="200" Height="200px" DataValueField="ID_Del_Mun" AutoPostBack="true" MarkFirstMatch="true" AllowCustomText="true" AutoCompleteSeparator="None" Filter="Contains"
                                EmptyMessage="Seleccione Deleg./Mun. ..." DataSourceID="SqlDataSourceMod12" DataTextField="Desc_Del_Mun" Skin="Silk" OnSelectedIndexChanged="ddlMunicipioMod_SelectedIndexChanged">
                            </telerik:RadComboBox>
                            <asp:SqlDataSource ID="SqlDataSourceMod12" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select ID_Del_Mun,Desc_Del_Mun from DelegacionMunicipio where cve_pais=@pais and ID_Estado=@estado">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="ddlPaisMod" Name="pais" />
                                    <asp:ControlParameter ControlID="ddlEstadoMod" Name="estado" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorMod9" runat="server" ErrorMessage="Debe indicar la Delegación o Municipio." Text="*" CssClass="alineado errores" ControlToValidate="ddlMunicipioMod" ValidationGroup="edita"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="LabelMod11" runat="server" Text="Colonia:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-3 col-sm-3 text-left">
                            <telerik:RadComboBox RenderMode="Lightweight" ID="ddlColoniaMod" runat="server" Width="200" Height="200px" DataValueField="ID_Colonia" AutoPostBack="true" MarkFirstMatch="true" AllowCustomText="true" AutoCompleteSeparator="None" Filter="Contains"
                                EmptyMessage="Seleccione Colonia ..." DataSourceID="SqlDataSourceMod13" DataTextField="Desc_Colonia" Skin="Silk" OnSelectedIndexChanged="ddlColoniaMod_SelectedIndexChanged">
                            </telerik:RadComboBox>
                            <asp:SqlDataSource ID="SqlDataSourceMod13" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select ID_Colonia,Desc_Colonia from Colonias where cve_pais=@pais and ID_Estado=@estado and ID_Del_Mun=@municipio">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="ddlPaisMod" Name="pais" />
                                    <asp:ControlParameter ControlID="ddlEstadoMod" Name="estado" />
                                    <asp:ControlParameter ControlID="ddlMunicipioMod" Name="municipio" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorMod10" runat="server" ErrorMessage="Debe indicar la Colonia." Text="*" CssClass="alineado errores" ControlToValidate="ddlColoniaMod" ValidationGroup="edita"></asp:RequiredFieldValidator>
                        </div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="LabelMod12" runat="server" Text="C.P.:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <telerik:RadComboBox RenderMode="Lightweight" ID="ddlCodigoMod" runat="server" Width="200" Height="200px" DataValueField="ID_Cod_Pos" AutoPostBack="true" MarkFirstMatch="true" AllowCustomText="true" AutoCompleteSeparator="None" Filter="Contains"
                                EmptyMessage="Seleccione C.P. ..." DataSourceID="SqlDataSourceMod14" DataTextField="ID_Cod_Pos" Skin="Silk">
                            </telerik:RadComboBox>
                            <asp:SqlDataSource ID="SqlDataSourceMod14" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select ID_Cod_Pos from Colonias where cve_pais=@pais and ID_Estado=@estado and ID_Del_Mun=@municipio and ID_Colonia=@colonia">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="ddlPaisMod" Name="pais" />
                                    <asp:ControlParameter ControlID="ddlEstadoMod" Name="estado" />
                                    <asp:ControlParameter ControlID="ddlMunicipioMod" Name="municipio" />
                                    <asp:ControlParameter ControlID="ddlColoniaMod" Name="colonia" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorMod11" runat="server" ErrorMessage="Debe indicar el Código Postal." Text="*" CssClass="alineado errores" ControlToValidate="ddlCodigoMod" ValidationGroup="edita"></asp:RequiredFieldValidator>
                        </div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="LabelMod13" runat="server" Text="Localidad:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtLocalidadMod" runat="server" MaxLength="50" CssClass="input-large"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="txtLocalidadMod_TextBoxWatermarkExtender" runat="server" BehaviorID="txtLocalidadMod_TextBoxWatermarkExtender" TargetControlID="txtLocalidadMod" WatermarkText="Localidad" WatermarkCssClass="water input-large" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorMod12" runat="server" ErrorMessage="Debe indicar la Localidad." Text="*" CssClass="alineado errores" ControlToValidate="txtLocalidadMod" ValidationGroup="edita"></asp:RequiredFieldValidator>
                        </div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="LabelMod14" runat="server" Text="Referencia:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtReferenciaMod" runat="server" MaxLength="100" CssClass="input-large"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="txtReferenciaMod_TextBoxWatermarkExtender" runat="server" BehaviorID="txtReferenciaMod_TextBoxWatermarkExtender" TargetControlID="txtReferenciaMod" WatermarkText="Referencia" WatermarkCssClass="water input-large" />
                        </div>

                        <div class="col-lg-12 col-sm-12 text-center alert-info"><asp:Label ID="LabelMod15" runat="server" Text="Lugar de Expedición" CssClass="textoBold"></asp:Label></div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="LabelMod16" runat="server" Text="Calle:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtCalleModEx" runat="server" MaxLength="100" CssClass="input-xxlarge"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtenderMod1" runat="server" BehaviorID="txtCalleModEx_TextBoxWatermarkExtender" TargetControlID="txtCalleModEx" WatermarkText="Calle" WatermarkCssClass="water input-xxlarge" />                            
                        </div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="LabelMod17" runat="server" Text="No. Ext.:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-3 col-sm-3 text-left">
                            <asp:TextBox ID="txtNoExtModEx" runat="server" MaxLength="20" CssClass="input-small"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtenderMod2" runat="server" BehaviorID="txtNoExtModEx_TextBoxWatermarkExtender" TargetControlID="txtNoExtModEx" WatermarkText="Num. Ext." WatermarkCssClass="water input-small" />                            
                        </div>
                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="LabelMod18" runat="server" Text="No. Int.:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-3 col-sm-3 text-left">
                            <asp:TextBox ID="txtNoIntModEx" runat="server" MaxLength="20" CssClass="input-small"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtenderMod3" runat="server" BehaviorID="txtNoIntModEx_TextBoxWatermarkExtender" TargetControlID="txtNoIntModEx" WatermarkText="Num. Int." WatermarkCssClass="water input-small" />
                        </div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="LabelMod19" runat="server" Text="País:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-3 col-sm-3 text-left">
                            <telerik:RadComboBox RenderMode="Lightweight" ID="ddlPaisModEx" runat="server" Width="200" Height="200px" DataValueField="cve_pais" AutoPostBack="true" MarkFirstMatch="true" AllowCustomText="true" AutoCompleteSeparator="None" Filter="Contains" 
                                EmptyMessage="Seleccione País..." DataSourceID="SqlDataSourceMod2" DataTextField="desc_pais" Skin="Silk" OnSelectedIndexChanged="ddlPaisModEx_SelectedIndexChanged">
                            </telerik:RadComboBox>
                            <asp:SqlDataSource ID="SqlDataSourceMod2" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select cve_pais,desc_pais from Paises_f"></asp:SqlDataSource>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorMod15" runat="server" ErrorMessage="Debe indicar el País." Text="*" CssClass="errores" ControlToValidate="ddlPaisModEx" ValidationGroup="edita"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="LabelMod20" runat="server" Text="Estado:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-3 col-sm-3 text-left">                            
                            <telerik:RadComboBox RenderMode="Lightweight" ID="ddlEstadoModEx" runat="server" Width="200" Height="200px" DataValueField="cve_edo" AutoPostBack="true" MarkFirstMatch="true" AllowCustomText="true" AutoCompleteSeparator="None" Filter="Contains"
                                EmptyMessage="Seleccione Estado..." DataSourceID="SqlDataSourceMod3" DataTextField="nom_edo" Skin="Silk" OnSelectedIndexChanged="ddlEstadoModEx_SelectedIndexChanged">
                            </telerik:RadComboBox>
                            <asp:SqlDataSource ID="SqlDataSourceMod3" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select cve_edo,nom_edo from Estados where cve_pais=@pais">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="ddlPaisModEx" Name="pais" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorMod16" runat="server" ErrorMessage="Debe indicar el Estado." Text="*" CssClass="errores" ControlToValidate="ddlEstadoModEx" ValidationGroup="edita"></asp:RequiredFieldValidator>
                        </div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="LabelMod21" runat="server" Text="Deleg./Mun.:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-3 col-sm-3 text-left">
                            <telerik:RadComboBox RenderMode="Lightweight" ID="ddlMunicipioModEx" runat="server" Width="200" Height="200px" DataValueField="ID_Del_Mun" AutoPostBack="true" MarkFirstMatch="true" AllowCustomText="true" AutoCompleteSeparator="None" Filter="Contains"
                                EmptyMessage="Seleccione Deleg./Mun. ..." DataSourceID="SqlDataSourceMod4" DataTextField="Desc_Del_Mun" Skin="Silk" OnSelectedIndexChanged="ddlMunicipioModEx_SelectedIndexChanged">
                            </telerik:RadComboBox>
                            <asp:SqlDataSource ID="SqlDataSourceMod4" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select ID_Del_Mun,Desc_Del_Mun from DelegacionMunicipio where cve_pais=@pais and ID_Estado=@estado">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="ddlPaisModEx" Name="pais" />
                                    <asp:ControlParameter ControlID="ddlEstadoModEx" Name="estado" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorMod17" runat="server" ErrorMessage="Debe indicar la Delegación o Municipio." Text="*" CssClass="alineado errores" ControlToValidate="ddlMunicipioModEx" ValidationGroup="edita"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="LabelMod22" runat="server" Text="Colonia:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-3 col-sm-3 text-left">
                            <telerik:RadComboBox RenderMode="Lightweight" ID="ddlColoniaModEx" runat="server" Width="200" Height="200px" DataValueField="ID_Colonia" AutoPostBack="true" MarkFirstMatch="true" AllowCustomText="true" AutoCompleteSeparator="None" Filter="Contains"
                                EmptyMessage="Seleccione Colonia ..." DataSourceID="SqlDataSourceMod5" DataTextField="Desc_Colonia" Skin="Silk" OnSelectedIndexChanged="ddlColoniaModEx_SelectedIndexChanged">
                            </telerik:RadComboBox>
                            <asp:SqlDataSource ID="SqlDataSourceMod5" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select ID_Colonia,Desc_Colonia from Colonias where cve_pais=@pais and ID_Estado=@estado and ID_Del_Mun=@municipio">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="ddlPaisModEx" Name="pais" />
                                    <asp:ControlParameter ControlID="ddlEstadoModEx" Name="estado" />
                                    <asp:ControlParameter ControlID="ddlMunicipioModEx" Name="municipio" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorMod18" runat="server" ErrorMessage="Debe indicar la Colonia." Text="*" CssClass="alineado errores" ControlToValidate="ddlColoniaModEx" ValidationGroup="edita"></asp:RequiredFieldValidator>
                        </div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="LabelMod23" runat="server" Text="C.P.:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <telerik:RadComboBox RenderMode="Lightweight" ID="ddlCodigoModEx" runat="server" Width="200" Height="200px" DataValueField="ID_Cod_Pos" AutoPostBack="true" MarkFirstMatch="true" AllowCustomText="true" AutoCompleteSeparator="None" Filter="Contains"
                                EmptyMessage="Seleccione C.P. ..." DataSourceID="SqlDataSourceMod6" DataTextField="ID_Cod_Pos" Skin="Silk">
                            </telerik:RadComboBox>
                            <asp:SqlDataSource ID="SqlDataSourceMod6" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select ID_Cod_Pos from Colonias where cve_pais=@pais and ID_Estado=@estado and ID_Del_Mun=@municipio and ID_Colonia=@colonia">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="ddlPaisModEx" Name="pais" />
                                    <asp:ControlParameter ControlID="ddlEstadoModEx" Name="estado" />
                                    <asp:ControlParameter ControlID="ddlMunicipioModEx" Name="municipio" />
                                    <asp:ControlParameter ControlID="ddlColoniaModEx" Name="colonia" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorMod19" runat="server" ErrorMessage="Debe indicar el Código Postal." Text="*" CssClass="alineado errores" ControlToValidate="ddlCodigoModEx" ValidationGroup="edita"></asp:RequiredFieldValidator>
                        </div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="LabelMod24" runat="server" Text="Localidad:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtLocalidadModEx" runat="server" MaxLength="50" CssClass="input-large"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtenderMod4" runat="server" BehaviorID="txtLocalidadModEx_TextBoxWatermarkExtender" TargetControlID="txtLocalidadModEx" WatermarkText="Localidad" WatermarkCssClass="water input-large" />                            
                        </div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="LabelMod25" runat="server" Text="Referencia:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtReferenciaModEx" runat="server" MaxLength="50" CssClass="input-large"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtenderMod5" runat="server" BehaviorID="txtReferenciaModEx_TextBoxWatermarkExtender" TargetControlID="txtReferenciaModEx" WatermarkText="Referencia" WatermarkCssClass="water input-large" />
                        </div>

                        <div class="col-lg-12 col-sm-12 text-center alert-info"><asp:Label ID="LabelMod26" runat="server" Text="Parámetros de Correo" CssClass="textoBold"></asp:Label></div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="LabelMod27" runat="server" Text="Tipo Servidor:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-3 col-sm-3 text-left">
                            <asp:DropDownList ID="ddlServidorMod" runat="server" CssClass="input-medium">
                                <asp:ListItem Text="SMTP" Value="1" Selected="True"/>
                                <asp:ListItem Text="POP3" Value="2" />
                                <asp:ListItem Text="IMAP" Value="3" />
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="LabelMod28" runat="server" Text="Servidor:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-3 col-sm-3 text-left">
                            <asp:TextBox ID="txtServidorMod" runat="server" MaxLength="100" CssClass="input-large"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtenderMod6" runat="server" BehaviorID="txtServidorMod_TextBoxWatermarkExtender" TargetControlID="txtServidorMod" WatermarkText="Servidor" WatermarkCssClass="water input-large" />
                        </div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="LabelMod29" runat="server" Text="Usuario:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-3 col-sm-3 text-left">
                            <asp:TextBox ID="txtUsuarioMod" runat="server" MaxLength="50" CssClass="input-large"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtenderMod7" runat="server" BehaviorID="txtUsuarioMod_TextBoxWatermarkExtender" TargetControlID="txtUsuarioMod" WatermarkText="Usuario" WatermarkCssClass="water input-large" />
                        </div>
                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="LabelMod30" runat="server" Text="Contraseña:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-3 col-sm-3 text-left">
                            <asp:TextBox ID="txtContrasenaMod" runat="server" MaxLength="50" CssClass="input-large"  placeholder="Contraseña"></asp:TextBox>                            
                        </div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="LabelMod31" runat="server" Text="Correo:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-3 col-sm-3 text-left">
                            <asp:TextBox ID="txtCorreoMod" runat="server" MaxLength="50" CssClass="input-large"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtenderMod8" runat="server" BehaviorID="txtCorreoMod_TextBoxWatermarkExtender" TargetControlID="txtCorreoMod" WatermarkText="Correo" WatermarkCssClass="water input-large" />
                        </div>
                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="LabelMod32" runat="server" Text="Correo CC:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-3 col-sm-3 text-left">
                            <asp:TextBox ID="txtCorreoModCC" runat="server" MaxLength="100" CssClass="input-large"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtenderMod9" runat="server" BehaviorID="txtCorreoModCC_TextBoxWatermarkExtender" TargetControlID="txtCorreoModCC" WatermarkText="Correo CC" WatermarkCssClass="water input-large" />
                        </div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="LabelMod33" runat="server" Text="Correo CCO:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-3 col-sm-3 text-left">
                            <asp:TextBox ID="txtCorreoModCCO" runat="server" MaxLength="100" CssClass="input-large"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtenderMod10" runat="server" BehaviorID="txtCorreoModCCO_TextBoxWatermarkExtender" TargetControlID="txtCorreoModCCO" WatermarkText="Correo CCO" WatermarkCssClass="water input-large" />
                        </div>
                        <div class="col-lg-1 col-sm-1 text-left"><asp:Label ID="LabelMod39" runat="server" Text="SSL:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-1 col-sm-1 text-left">
                            <asp:CheckBox ID="chkCifradoMod" runat="server" />
                        </div>
                        <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="LabelMod40" runat="server" Text="Puerto:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-2 col-sm-2 text-left">
                            <asp:TextBox ID="txtPuertoMod" runat="server" MaxLength="4" CssClass="input-small"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderMod2" runat="server" BehaviorID="txtPuertoMod_FilteredTextBoxExtender" TargetControlID="txtPuertoMod" FilterType="Numbers" />
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtenderMod13" runat="server" BehaviorID="txtPuertoMod_TextBoxWatermarkExtender" TargetControlID="txtPuertoMod" WatermarkText="Puerto" WatermarkCssClass="water input-small" />
                        </div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="LabelMod41" runat="server" Text="Mensaje Correo:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtMsgCorreoMod" runat="server" CssClass="textNota" TextMode="MultiLine" Rows="5"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtenderMod14" runat="server" BehaviorID="txtMsgCorreoMod_TextBoxWatermarkExtender" TargetControlID="txtMsgCorreoMod" WatermarkText="Mensaje de Correo" WatermarkCssClass="water textNota" />
                        </div>
                        
                        <div class="col-lg-12 col-sm-12 text-center alert-info"><asp:Label ID="LabelMod34" runat="server" Text="Generales" CssClass="textoBold"></asp:Label></div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="LabelMod35" runat="server" Text="Nombre Corto:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-3 col-sm-3 text-left">
                            <asp:TextBox ID="txtNomCortoMod" runat="server" MaxLength="100" CssClass="input-large"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtenderMod11" runat="server" BehaviorID="txtNomCortoMod_TextBoxWatermarkExtender" TargetControlID="txtNomCortoMod" WatermarkText="Nombre Corto" WatermarkCssClass="water input-large" />
                        </div>
                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="LabelMod37" runat="server" Text="Tel. Principal:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-3 col-sm-3 text-left">
                            <asp:TextBox ID="txtTel1Mod" runat="server" MaxLength="20" CssClass="input-medium"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="txtTel1ModMod_FilteredTextBoxExtender" runat="server" BehaviorID="txtTel1ModMod_FilteredTextBoxExtender" TargetControlID="txtTel1Mod" FilterType="Numbers" />
                            <cc1:TextBoxWatermarkExtender ID="txtTel1ModMod_TextBoxWatermarkExtender" runat="server" BehaviorID="txtTel1ModMod_TextBoxWatermarkExtender" TargetControlID="txtTel1Mod" WatermarkText="Tel. Principal" WatermarkCssClass="water input-medium" />
                        </div>
                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="LabelMod38" runat="server" Text="Tel. Oficina:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtTel2Mod" runat="server" MaxLength="20" CssClass="input-medium"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderMod1" runat="server" BehaviorID="txtTel2ModMod_FilteredTextBoxExtender" TargetControlID="txtTel2Mod" FilterType="Numbers" />
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtenderMod12" runat="server" BehaviorID="txtTel2ModMod_TextBoxWatermarkExtender" TargetControlID="txtTel2Mod" WatermarkText="Tel. Oficina" WatermarkCssClass="water input-medium" />
                        </div>
                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="LabelMod36" runat="server" Text="Logo:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:Label ID="lblLogo" runat="server" Visible="false" ></asp:Label>
                            <asp:Image ID="imgLogo" runat="server" Width="100px" Height="50px" />
                            <telerik:RadAsyncUpload RenderMode="Lightweight" runat="server" Culture="es-Mx" CssClass="async-attachment col-lg-12 col-sm-12"
                                MaxFileInputsCount="1" MultipleFileSelection="Disabled" ID="AsyncUploadMod1" HideFileInput="true"
                                AllowedFileExtensions=".jpeg,.jpg,.png,.gif,.bmp,.tiff,.JPEG,.JPG,.PNG,.GIF,.BMP,.TIFF">
                            </telerik:RadAsyncUpload>                            
                        </div>


                        <div class="col-lg-12 col-sm-12 text-center alert-info"><asp:Label ID="LabelMod42" runat="server" Text="Parametros de Facturación" CssClass="textoBold"></asp:Label></div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="LabelMod43" runat="server" Text="Certificado:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:Label ID="lblRutaCer" runat="server" ></asp:Label>
                            <telerik:RadAsyncUpload RenderMode="Lightweight" runat="server" Culture="es-Mx" CssClass="async-attachment"
                                MaxFileInputsCount="1" MultipleFileSelection="Disabled" ID="RadAsyncUploadModCer" HideFileInput="true"
                                AllowedFileExtensions=".cer,.CER">
                            </telerik:RadAsyncUpload>
                        </div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="LabelMod44" runat="server" Text="Llave:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:Label ID="lblRutaKey" runat="server" ></asp:Label>
                            <telerik:RadAsyncUpload RenderMode="Lightweight" runat="server" Culture="es-Mx" CssClass="async-attachment"
                                MaxFileInputsCount="1" MultipleFileSelection="Disabled" ID="RadAsyncUploadModKey" HideFileInput="true"
                                AllowedFileExtensions=".key,.KEY">
                            </telerik:RadAsyncUpload>
                        </div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="LabelMod45" runat="server" Text="PFX:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:Label ID="lblRutaPfx" runat="server" ></asp:Label>
                            <telerik:RadAsyncUpload RenderMode="Lightweight" runat="server" Culture="es-Mx" CssClass="async-attachment"
                                MaxFileInputsCount="1" MultipleFileSelection="Disabled" ID="RadAsyncUploadModPfx" HideFileInput="true"
                                AllowedFileExtensions=".pfx,.PFX">
                            </telerik:RadAsyncUpload>
                        </div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="LabelMod46" runat="server" Text="Contraseña Llave:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtPassLlaveMod" runat="server" MaxLength="50" CssClass="input-large"  placeholder="Contraseña Llave"></asp:TextBox>                            
                        </div>
                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="LabelMod49" runat="server" Text="Vigencia:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left"><asp:Label ID="lblVigenciaCertificadoMod" runat="server" CssClass="input-large"></asp:Label></div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="LabelMod47" runat="server" Text="Usuario WS:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtUserWsMod" runat="server" CssClass="input-large" MaxLength="50"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtenderMod15" runat="server" BehaviorID="txtUserWsMod_TextBoxWatermarkExtender" TargetControlID="txtUserWsMod" WatermarkText="Usuario WS" WatermarkCssClass="water input-large" />
                        </div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="LabelMod48" runat="server" Text="Contraseña WS:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtPassWsMod" runat="server" MaxLength="50" CssClass="input-large"  placeholder="Contraseña WS"></asp:TextBox>                            
                        </div>
                        <div class="col-lg-6 col-sm-6 text-center pad1m">
                            <asp:LinkButton ID="lnkEdita" runat="server" CssClass="btn btn-success t14" ValidationGroup="edita" OnClick="lnkEdita_Click"><i class="fa fa-save"></i>&nbsp; Guardar</asp:LinkButton>
                        </div>
                        <div class="col-lg-6 col-sm-6 text-center pad1m">
                            <asp:LinkButton ID="lnkCancelarEdit" runat="server" CssClass="btn btn-danger t14" OnClick="lnkCancelaEdit_Click"><i class="fa fa-remove"></i>&nbsp; Cancelar</asp:LinkButton>
                        </div>
                    </asp:Panel>
                    <asp:UpdateProgress ID="updProgEmiMod" runat="server" AssociatedUpdatePanelID="UpdatePanelEmiMod">
                        <ProgressTemplate>
                            <asp:Panel ID="pnlMaskLoadEmiMod" runat="server" CssClass="maskLoad" />
                            <asp:Panel ID="pnlCargandoEmiMod" runat="server" CssClass="pnlPopUpLoad">
                                <asp:Image ID="imgLoadEmiMod" runat="server" ImageUrl="~/img/loading.gif" CssClass="ancho100" />
                            </asp:Panel>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
    </telerik:RadWindow>


    <div class="row">
        <div class="col-lg-12 col-sm-12 text-center alert-info">
            <h3>
                <i class="fa fa-hospital-o"></i>&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label1" runat="server" Text="Empresas"></asp:Label>
            </h3>
        </div>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>            
            <div class="col-lg-12 col-sm-12 text-right">
                <asp:LinkButton ID="btnAgregar" runat="server" CssClass="btn btn-info t14" ToolTip="Agregar" OnClientClick="abreNewEmi()"><i class="fa fa-plus-circle"></i>&nbsp;<span>Agregar</span></asp:LinkButton>
            </div>
            <div class="row pad1m">
                <div class="col-lg-12 col-sm-12 text-center">
                    <asp:Label ID="lblError" runat="server" CssClass="errores" />
                </div>
            </div>
            <asp:Panel ID="pnlContenido" CssClass="col-lg-12 col-sm-12" runat="server" ScrollBars="Auto">
                <asp:Panel ID="pnlCatalogos" runat="server" CssClass="col-lg-12 col-sm-12" ScrollBars="Auto">
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered"
                        DataKeyNames="id_empresa" DataSourceID="SqlDataSource1" AllowPaging="True" PageSize="10"
                        AllowSorting="True" EmptyDataText="No se han registrado empresas" OnRowDeleting="GridView1_RowDeleting"
                        OnRowDataBound="GridView1_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="id_empresa" HeaderText="id_empresa" ReadOnly="True" SortExpression="id_empresa"
                                Visible="false" />
                            <asp:BoundField DataField="razon_social" HeaderText="Empresa" SortExpression="razon_social" ReadOnly="True" />
                            <asp:BoundField DataField="rfc" HeaderText="R.F.C." SortExpression="rfc" ReadOnly="True" />
                            <asp:BoundField DataField="domicilio" HeaderText="Domicilio" SortExpression="domicilio" ReadOnly="true" />
                            <asp:BoundField DataField="tel_principal" HeaderText="Teléfono" SortExpression="tel_principal" ReadOnly="True"/>
                            <asp:TemplateField ShowHeader="False">
                                <EditItemTemplate>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEditar" runat="server" CausesValidation="False" CommandName="Edit" CommandArgument='<%# Eval("id_empresa") %>'
                                        ToolTip="Editar" CssClass="btn btn-warning t14" onclick="btnEditar_Click"><i class="fa fa-edit"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                <EditItemTemplate>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEliminar" runat="server" CausesValidation="False" CommandName="Delete"
                                        ToolTip="Eliminar" CssClass="btn btn-danger t14" OnClientClick="return confirm('¿Esta seguro de eliminar la Empresa?');"><i class="fa fa-trash"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EditRowStyle CssClass="alert-warning" />
                        <EmptyDataRowStyle CssClass="errores alert-danger" />
                        <SelectedRowStyle CssClass="alert-success" />
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>"
                        DeleteCommand="delete from Empresas where id_empresa=@id_empresa" 
                        SelectCommand="select id_empresa,razon_social,rfc, calle+' '+ num_ext+' '+num_int+', '+ colonia+', '+ municipio+', '+ estado+', '+ cp as domicilio, tel_principal from Empresas"
                        UpdateCommand="update Empresas set razon_social=@razon_social,rfc=@rfc, tel_principal=@tel_particular where id_empresa=@id_empresa">
                        <DeleteParameters>
                            <asp:Parameter Name="id_empresa" />
                        </DeleteParameters>                       
                        <UpdateParameters>
                            <asp:Parameter Name="razon_social" />
                            <asp:Parameter Name="rfc" />
                            <asp:Parameter Name="tel_particular" />
                            <asp:Parameter Name="id_empresa" />
                        </UpdateParameters>
                    </asp:SqlDataSource>
                </asp:Panel>
            </asp:Panel>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                <ProgressTemplate>
                    <asp:Panel ID="pnlMaskLoad" runat="server" CssClass="maskLoad">
                    </asp:Panel>
                    <asp:Panel ID="pnlCargando" runat="server" CssClass="pnlPopUpLoad">
                        <asp:Image ID="imgLoad" runat="server" ImageUrl="~/img/loading.gif" CssClass="ancho100" />
                    </asp:Panel>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </ContentTemplate>
    </asp:UpdatePanel>

    
</asp:Content>
