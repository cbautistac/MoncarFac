<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RecepcionOrden.aspx.cs" Inherits="RecepcionOrden" MasterPageFile="~/AdmonOrden.master" Culture="es-Mx" UICulture="es-Mx" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">  
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true"/>
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server"></telerik:RadSkinManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-lg-12 col-sm-12 text-center alert-info">
                    <h3>
                        <i class="fa fa-sign-in"></i>&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="Label1" runat="server" Text="Orden"></asp:Label>                        
                    </h3>                    
                </div>
            </div>
            <div class="row">
                <div class="col-lg-12 col-sm-12 text-center"> 
                        <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="orden" DisplayMode="List" CssClass="errores"/>                   
                        <asp:Label ID="lblErrorRecepcion" runat="server" CssClass="errores alert-danger"></asp:Label>                        
                    </h3>                    
                </div>
            </div>
            <asp:Panel runat="server" ID="pnlRecepcion" CssClass="paneles col-lg-12 col-sm-12" ScrollBars="Auto">                
                <div class="row">
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label46" runat="server" Text="Tipo Orden:"  CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <telerik:RadComboBox RenderMode="Lightweight" ID="ddlToOrden" runat="server" DataSourceID="SqlDataSource21" DataTextField="descripcion" DataValueField="id_tipo_orden" AllowCustomText="true" Width="210px" MaxHeight="300px" Skin="MetroTouch" EmptyMessage="Seleccione Tipo Orden" Filter="Contains" ></telerik:RadComboBox>                        
                        <asp:SqlDataSource ID="SqlDataSource21" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select 0 as id_tipo_orden,'Seleccione Tipo Orden' as descripcion union all select id_tipo_orden,descripcion from Tipo_Orden "></asp:SqlDataSource>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ErrorMessage="Debe seleccionar ún tipo de Orden" Text="*" CssClass="errores alineado" ValidationGroup="orden" ControlToValidate="ddlToOrden"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label47" runat="server" Text="Cliente:"  CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="ddlClienteOrden" AllowCustomText="true" Width="210px" MaxHeight="300px" DataSourceID="SqlDataSource22" DataTextField="razon_social" DataValueField="id_cliprov" Skin="MetroTouch" EmptyMessage="Seleccione Cliente" Filter="Contains" ></telerik:RadComboBox>                                                                
                        <asp:SqlDataSource ID="SqlDataSource22" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select 0 as id_cliprov,'Seleccione un Cliente' as razon_social union all select id_cliprov,upper(razon_social) as razon_social from Cliprov where tipo='C' and estatus='A'"></asp:SqlDataSource>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ErrorMessage="Debe seleccionar ún cliente" Text="*" CssClass="errores alineado" ValidationGroup="orden" ControlToValidate="ddlClienteOrden"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label48" runat="server" Text="Tipo Servicio:"  CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="ddlTsOrden" AllowCustomText="true" Width="210px" MaxHeight="300px" DataSourceID="SqlDataSource23" DataTextField="descripcion" DataValueField="id_tipo_servicio" Skin="MetroTouch" EmptyMessage="Seleccione Tipo Servicio" Filter="Contains" ></telerik:RadComboBox>                        
                        <asp:SqlDataSource ID="SqlDataSource23" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select 0 as  id_tipo_servicio,'Seleccione Tipo Servicio' as descripcion union all select id_tipo_servicio, descripcion from Tipo_Servicios " ></asp:SqlDataSource>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ErrorMessage="Debe seleccionar ún tipo de servicio" Text="*" CssClass="errores alineado" ValidationGroup="orden" ControlToValidate="ddlTsOrden"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label49" runat="server" Text="Tipo Valuación:"  CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="ddlValOrden" AllowCustomText="true" Width="210px" MaxHeight="300px" DataSourceID="SqlDataSource24" DataTextField="descripcion" DataValueField="id_tipo_valuacion" Skin="MetroTouch" EmptyMessage="Seleccione Tipo Valuación" Filter="Contains" ></telerik:RadComboBox>                        
                        <asp:SqlDataSource ID="SqlDataSource24" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select 0 as  id_tipo_valuacion,'Seleccione Tipo Valuación' as descripcion union all select id_tipo_valuacion, descripcion from Tipo_Valuacion "></asp:SqlDataSource>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" ErrorMessage="Debe seleccionar ún tipo de valuación" Text="*" CssClass="errores alineado" ValidationGroup="orden" ControlToValidate="ddlValOrden"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label50" runat="server" Text="Tipo Asegurado:"  CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="ddlTaOrden" AllowCustomText="true" Width="210px" MaxHeight="300px" DataSourceID="SqlDataSource25" DataTextField="descripcion" DataValueField="id_tipo_asegurado" Skin="MetroTouch" EmptyMessage="Seleccione Tipo Asegurado" Filter="Contains" ></telerik:RadComboBox>                        
                        <asp:SqlDataSource ID="SqlDataSource25" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select 0 as  id_tipo_asegurado,'Seleccione Tipo Asegurado' as descripcion union all select id_tipo_asegurado,descripcion from Tipo_Asegurados  "></asp:SqlDataSource>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" ErrorMessage="Debe seleccionar ún tipo de asegurado" Text="*" CssClass="errores alineado" ValidationGroup="orden" ControlToValidate="ddlTaOrden"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-lg-2 col-sm-2 text-left">
                        <asp:Label ID="Label3" runat="server" Text="Perfil:"  CssClass="alingMiddle textoBold"></asp:Label>
                        <asp:Label ID="lblPerfilIni" runat="server" Visible="false"></asp:Label>
                    </div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="ddlPerfil" AllowCustomText="true" Width="210px" MaxHeight="300px" DataSourceID="SqlDataSource1" DataTextField="descripcion" DataValueField="id_perfilOrden" Skin="MetroTouch" EmptyMessage="Seleccione Perfil" Filter="Contains" ></telerik:RadComboBox>                        
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select 0 as  id_perfilOrden,'Seleccione Perfil' as descripcion union all select id_perfilOrden, descripcion from PerfilesOrdenes " ></asp:SqlDataSource>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Debe seleccionar un perfil" Text="*" CssClass="errores alineado" ValidationGroup="orden" ControlToValidate="ddlPerfil"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-2 col-sm-2 text-left">
                        <asp:Label ID="Label51" runat="server" Text="Localización:"  CssClass="alingMiddle textoBold"></asp:Label>
                        <asp:Label ID="lblLocIni" runat="server" Visible="false"></asp:Label>
                    </div>                
                    <div class="col-lg-4 col-sm-4 text-left">
                        <telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="ddlLocOrden" AllowCustomText="true" Width="210px" MaxHeight="300px" DataSourceID="SqlDataSource26" DataTextField="descripcion" DataValueField="id_localizacion" Skin="MetroTouch" EmptyMessage="Seleccione Localización" Filter="Contains" ></telerik:RadComboBox>                        
                        <asp:SqlDataSource ID="SqlDataSource26" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select 0 as  id_localizacion,'Seleccione Localización' as descripcion union all select id_localizacion, descripcion from Localizaciones " ></asp:SqlDataSource>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server" ErrorMessage="Debe seleccionar una localizacion" Text="*" CssClass="errores alineado" ValidationGroup="orden" ControlToValidate="ddlLocOrden"></asp:RequiredFieldValidator>
                    </div>                
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label52" runat="server" Text="No. Torre:" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:TextBox ID="txtTorreOrden" runat="server" CssClass="alingMiddle input-medium" MaxLength="8"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtTorreOrdenWatermarkExtender1" runat="server" BehaviorID="txtTorreOrden_TextBoxWatermarkExtender" TargetControlID="txtTorreOrden" WatermarkText="No. Torre" WatermarkCssClass="water input-medium" />                        
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label53" runat="server" Text="Tanque Gasolina:" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:DropDownList ID="ddlGasOrden" runat="server" DataSourceID="SqlDataSource18" CssClass="alingMiddle input-large" DataTextField="descripcion" DataValueField="id_med_gas"></asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSource18" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select 0 as id_med_gas, '' as descripcion union all select id_med_gas,descripcion from Medidas_Gasolina"></asp:SqlDataSource>
                    </div>                
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label54" runat="server" Text="Km. Actual:" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:TextBox ID="txtKmOrden" runat="server" CssClass="alingMiddle input-mini" MaxLength="6"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtKmOrdenWatermarkExtender1" runat="server" BehaviorID="txtKmOrden_TextBoxWatermarkExtender" TargetControlID="txtKmOrden" WatermarkText="Km. Actual" WatermarkCssClass="water input-mini" />
                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" FilterType="Numbers" TargetControlID="txtKmOrden"/>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label55" runat="server" Text="Cliente Problema:" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:CheckBox ID="cbxCproblemaOrden" runat="server" />
                    </div>                
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label56" runat="server" Text="Categoría Cliente:" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="ddlCategoriaOrden" AllowCustomText="true" Width="210px" MaxHeight="300px" DataSourceID="SqlDataSource19" DataTextField="descripcion" DataValueField="id_cat_cliente" Skin="MetroTouch" EmptyMessage="Seleccione Aseguradora" Filter="Contains" ></telerik:RadComboBox>                        
                        <asp:SqlDataSource ID="SqlDataSource19" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select 0 as id_cat_cliente, '' as descripcion union all select id_cat_cliente,descripcion+' ('+prefijo+')' as descripcion from Categoria_Cliente"></asp:SqlDataSource>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label76" runat="server" Text="Flotilla:" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:CheckBox ID="cbxFlotillaOrden" runat="server" />
                    </div>  
                </div>
                <div class="row">              
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label91" runat="server" Text="Empresa Grúa Entrada:" CssClass="alingMiddle textoBold"/></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:TextBox ID="txtEmpresaGruaEnOrden" runat="server" MaxLength="100" CssClass="alingMiddle input-large" />
                        <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtEmpresaGruaEnOrden_TextBoxWatermarkExtender" TargetControlID="txtEmpresaGruaEnOrden" ID="txtEmpresaGruaEnOrden_TextBoxWatermarkExtender" WatermarkText="Empresa Grúa Entrada" WatermarkCssClass="water input-large" />
                    </div>
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label96" runat="server" Text="Empresa Grúa Salida:" CssClass="alingMiddle textoBold"/></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:TextBox ID="txtEmpresaGruaSalOrden" runat="server" MaxLength="100" CssClass="alingMiddle input-large" />
                        <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtEmpresaGruaSalOrden_TextBoxWatermarkExtender" TargetControlID="txtEmpresaGruaSalOrden" ID="txtEmpresaGruaSalOrden_TextBoxWatermarkExtender" WatermarkText="Empresa Grúa Salida" WatermarkCssClass="water input-large" />
                    </div>                    
                </div>
                <div class="row">
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label83" runat="server" Text="No. Grúa Entrada:" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:TextBox ID="txtGruaEntOrden" runat="server" CssClass="alingMiddle input-large" MaxLength="20"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtGruaEntOrdenWatermarkExtender1" runat="server" BehaviorID="txtGruaEntOrden_TextBoxWatermarkExtender" TargetControlID="txtGruaEntOrden" WatermarkText="No. Grúa Entrada" WatermarkCssClass="water input-large" />                                    
                    </div>
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label84" runat="server" Text="No. Grúa Salida:" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:TextBox ID="txtGruaSalOrden" runat="server" CssClass="alingMiddle input-large" MaxLength="20"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtGruaSalOrdenWatermarkExtender2" runat="server" BehaviorID="txtGruaSalOrden_TextBoxWatermarkExtender" TargetControlID="txtGruaSalOrden" WatermarkText="No. Grúa Salida" WatermarkCssClass="water input-large" />                                    
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label95" runat="server" Text="Operador Grúa Entrada:" CssClass="alingMiddle textoBold"/></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:TextBox ID="txtOperadorGruaEnOrden" runat="server" MaxLength="100" CssClass="alingMiddle input-large" />
                        <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtOperadorGruaEnOrden_TextBoxWatermarkExtender" TargetControlID="txtOperadorGruaEnOrden" ID="txtOperadorGruaEnOrden_TextBoxWatermarkExtender" WatermarkText="Operador Grúa Entrada" WatermarkCssClass="water input-large" />
                    </div>
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label97" runat="server" Text="Operador Grúa Salida:" CssClass="alingMiddle textoBold"/></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:TextBox ID="txtOperadorGruaSalOrden" runat="server" MaxLength="100" CssClass="alingMiddle input-large" />
                        <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtOperadorGruaSalOrden_TextBoxWatermarkExtender" TargetControlID="txtOperadorGruaSalOrden" ID="txtOperadorGruaSalOrden_TextBoxWatermarkExtender" WatermarkText="Operador Grúa Salida" WatermarkCssClass="water input-large" />
                    </div>                    
                </div>
                <div class="row">
                    <div class="col-lg-12 col-sm-12 text-center pad1m alert-info textoBold t14"><asp:Label ID="Label58" runat="server" Text="Propietario"></asp:Label></div>                    
                </div>
                <script>
                    function contacto() {
                        var nombre = document.getElementById("ContentPlaceHolder1_txtNombreOrden").value;
                        nombre = nombre.replace("Nombre","");
                        var ap = document.getElementById("ContentPlaceHolder1_txtApPatOrden").value;
                        ap = ap.replace("Apellido Paterno", "");
                        var am = document.getElementById("ContentPlaceHolder1_txtApMatOrden").value;
                        am = am.replace("Apellido Materno", "");
                        document.getElementById("ContentPlaceHolder1_txtContactoOrden").value = nombre + ' ' + ap + ' ' + am;
                    }
                </script>
                <div class="row">
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label57" runat="server" Text="Nombre(s):" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:TextBox ID="txtNombreOrden" runat="server" 
                            CssClass="alingMiddle input-large" MaxLength="100" 
                            onchange="contacto()"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtNombreOrdenWatermarkExtender1" runat="server" BehaviorID="txtNombreOrden_TextBoxWatermarkExtender" TargetControlID="txtNombreOrden" WatermarkText="Nombre" WatermarkCssClass="water input-large" />
                    </div>
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label59" runat="server" Text="Apellidos:" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:TextBox ID="txtApPatOrden" runat="server" 
                            CssClass="alingMiddle input-medium" MaxLength="50"  
                            onchange="contacto()"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtApPatOrdenWatermarkExtender1" runat="server" BehaviorID="txtApPatOrden_TextBoxWatermarkExtender" TargetControlID="txtApPatOrden" WatermarkText="Apellido Paterno" WatermarkCssClass="water input-medium" />
                        <asp:TextBox ID="txtApMatOrden" runat="server" 
                            CssClass="alingMiddle input-medium" MaxLength="50"  
                            onchange="contacto()"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtApMatOrdenWatermarkExtender2" runat="server" BehaviorID="txtApMatOrden_TextBoxWatermarkExtender" TargetControlID="txtApMatOrden" WatermarkText="Apellido Materno" WatermarkCssClass="water input-medium" />
                    </div>
                </div>
               <!-- <div class="row">
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label61" runat="server" Text="Calle:" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:TextBox ID="txtCalleOrden" runat="server" CssClass="alingMiddle input-large" MaxLength="200"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtCalleOrdenWatermarkExtender1" runat="server" BehaviorID="txtCalleOrden_TextBoxWatermarkExtender" TargetControlID="txtCalleOrden" WatermarkText="Calle" WatermarkCssClass="water input-large" />                                    
                    </div>
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label62" runat="server" Text="No. Ext.:" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:TextBox ID="txtNoExtOrden" runat="server" CssClass="alingMiddle input-small" MaxLength="20"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtNoExtOrdenWatermarkExtender2" runat="server" BehaviorID="txtNoExtOrden_TextBoxWatermarkExtender" TargetControlID="txtNoExtOrden" WatermarkText="No. Ext." WatermarkCssClass="water input-small" />                                    
                        &nbsp;&nbsp;&nbsp;<asp:Label ID="Label63" runat="server" Text="No. Int.:" CssClass="alingMiddle textoBold"></asp:Label>
                        <asp:TextBox ID="txtNoIntOrden" runat="server" CssClass="alingMiddle input-small" MaxLength="20"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtNoIntOrdenWatermarkExtender3" runat="server" BehaviorID="txtNoIntOrden_TextBoxWatermarkExtender" TargetControlID="txtNoIntOrden" WatermarkText="No. Int." WatermarkCssClass="water input-small" />                                    
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label64" runat="server" Text="Colonia:" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:TextBox ID="txtColoniaOrden" runat="server" CssClass="alingMiddle input-large" MaxLength="200"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtColoniaOrdenWatermarkExtender4" runat="server" BehaviorID="txtColoniaOrden_TextBoxWatermarkExtender" TargetControlID="txtColoniaOrden" WatermarkText="Colonia" WatermarkCssClass="water input-large" />                                    
                    </div>
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label65" runat="server" Text="Municip./Deleg.:" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:TextBox ID="txtMunOrden" runat="server" CssClass="alingMiddle input-large" MaxLength="100"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtMunOrdenWatermarkExtender5" runat="server" BehaviorID="txtMunOrden_TextBoxWatermarkExtender" TargetControlID="txtMunOrden" WatermarkText="Municip./ Deleg." WatermarkCssClass="water input-large" />                                    
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label66" runat="server" Text="Estado:" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:TextBox ID="txtEdoOrden" runat="server" CssClass="alingMiddle input-large" MaxLength="100"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtEdoOrdenWatermarkExtender6" runat="server" BehaviorID="txtEdoOrden_TextBoxWatermarkExtender" TargetControlID="txtEdoOrden" WatermarkText="Estado" WatermarkCssClass="water input-large" />
                    </div>
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label67" runat="server" Text="C.P.:" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:TextBox ID="txtCpOrden" runat="server" CssClass="alingMiddle input-small" MaxLength="5"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtCpOrdenWatermarkExtender7" runat="server" BehaviorID="txtCpOrden_TextBoxWatermarkExtender" TargetControlID="txtCpOrden" WatermarkText="C.P." WatermarkCssClass="water input-small" />                                    
                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" FilterType="Numbers" TargetControlID="txtCpOrden"/>
                    </div>
                </div>-->
                <div class="row">
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label68" runat="server" Text="Tel. Particular:" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:TextBox ID="txtTel1Orden" runat="server" CssClass="alingMiddle input-large" MaxLength="20"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtTel1OrdenWatermarkExtender8" runat="server" BehaviorID="txtTel1Orden_TextBoxWatermarkExtender" TargetControlID="txtTel1Orden" WatermarkText="Teléfono Particular" WatermarkCssClass="water input-large" />                                    
                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" FilterType="Numbers,Custom" TargetControlID="txtTel1Orden" ValidChars="()-+N/A"/>
                    </div>
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label69" runat="server" Text="Tel. Celular:" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:TextBox ID="txtTel3Orden" runat="server" CssClass="alingMiddle input-large" MaxLength="20"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtTel3OrdenWatermarkExtender9" runat="server" BehaviorID="txtTel3Orden_TextBoxWatermarkExtender" TargetControlID="txtTel3Orden" WatermarkText="Teléfono Celular" WatermarkCssClass="water input-large" />                                    
                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" FilterType="Numbers,Custom" TargetControlID="txtTel3Orden" ValidChars="()-+N/A"/>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label70" runat="server" Text="Tel. Oficina:" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:TextBox ID="txtTel2Orden" runat="server" CssClass="alingMiddle input-large" MaxLength="20"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtTel2OrdenWatermarkExtender1" runat="server" BehaviorID="txtTel2Orden_TextBoxWatermarkExtender" TargetControlID="txtTel2Orden" WatermarkText="Teléfono Oficina" WatermarkCssClass="water input-large" />                                    
                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" FilterType="Numbers,Custom" TargetControlID="txtTel2Orden" ValidChars="()-+N/A"/>
                    </div>
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label71" runat="server" Text="Correo Electrónico:" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:TextBox ID="txtCorreoOrden" runat="server" CssClass="alingMiddle input-large" MaxLength="250"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtCorreoOrdenWatermarkExtender1" runat="server" BehaviorID="txtCorreoOrden_TextBoxWatermarkExtender" TargetControlID="txtCorreoOrden" WatermarkText="Correo Electrónico" WatermarkCssClass="water input-large" />                                                                        
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label72" runat="server" Text="Contacto Con:" CssClass="alingMiddle textoBold" ></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:TextBox ID="txtContactoOrden" runat="server" CssClass="alingMiddle input-large" MaxLength="200" placeholder="Contacto Con"></asp:TextBox>                        
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-12 col-sm-12 text-center pad1m alert-info textoBold t14"><asp:Label ID="Label2" runat="server" Text="Siniestro"></asp:Label></div>                    
                </div>
                <div class="row">
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label74" runat="server" Text="No. Siniestro:" CssClass="textoBold alingMiddle"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:TextBox ID="txtSiniestroOrden" runat="server" CssClass="alingMiddle input-large" MaxLength="20"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtSiniestroOrdenWatermarkExtender1" runat="server" BehaviorID="txtSiniestroOrden_TextBoxWatermarkExtender" TargetControlID="txtSiniestroOrden" WatermarkText="No. Siniestro" WatermarkCssClass="water input-large" />                                    
                    </div>
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label78" runat="server" Text="Fecha Siniestro:" CssClass="textoBold alingMiddle"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:TextBox ID="txtFechaSiniestro" runat="server" CssClass="alingMiddle input-small" MaxLength="10" Enabled="false"></asp:TextBox>
                        <cc1:CalendarExtender ID="txtFechaSiniestro_CalendarExtender" runat="server" BehaviorID="txtFechaSiniestro_CalendarExtender" TargetControlID="txtFechaSiniestro" Format="yyyy-MM-dd" PopupButtonID="lnkFSIN"/>                        
                        <cc1:TextBoxWatermarkExtender ID="txtFechaSiniestroWatermarkExtender1" runat="server" BehaviorID="txtFechaSiniestro_TextBoxWatermarkExtender" TargetControlID="txtFechaSiniestro" WatermarkText="aaaa-mm-dd" WatermarkCssClass="water input-small" />                                    
                        <asp:LinkButton ID="lnkFSIN" runat="server" CssClass="btn btn-info t14"><i class="fa fa-calendar"></i></asp:LinkButton>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label75" runat="server" Text="No. Póliza:" CssClass="textoBold alingMiddle"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:TextBox ID="txtPolizaOrden" runat="server" CssClass="alingMiddle input-large" MaxLength="20"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtPolizaOrdenWatermarkExtender1" runat="server" BehaviorID="txtPolizaOrden_TextBoxWatermarkExtender" TargetControlID="txtPolizaOrden" WatermarkText="No. Póliza" WatermarkCssClass="water input-large" />                                    
                    </div>
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label77" runat="server" Text="No. Reporte:" CssClass="textoBold alingMiddle"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:TextBox ID="txtReporteOrden" runat="server" CssClass="alingMiddle input-large" MaxLength="20"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtReporteOrdenWatermarkExtender1" runat="server" BehaviorID="txtReporteOrden_TextBoxWatermarkExtender" TargetControlID="txtReporteOrden" WatermarkText="No. Reporte" WatermarkCssClass="water input-large" />                                    
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label80" runat="server" Text="Folio Electrónico:" CssClass="textoBold alingMiddle"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:TextBox ID="txtFolioOrden" runat="server" CssClass="alingMiddle input-large" MaxLength="12"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtFolioOrdenWatermarkExtender1" runat="server" BehaviorID="txtFolioOrden_TextBoxWatermarkExtender" TargetControlID="txtFolioOrden" WatermarkText="Folio Electrónico" WatermarkCssClass="water input-large" />                                    
                    </div>
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label81" runat="server" Text="Ajustador Aseguradora:" CssClass="textoBold alingMiddle"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:TextBox ID="txtAjustadorOrden" runat="server" CssClass="alingMiddle input-large" MaxLength="200"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtAjustadorOrdenWatermarkExtender1" runat="server" BehaviorID="txtAjustadorOrden_TextBoxWatermarkExtender" TargetControlID="txtAjustadorOrden" WatermarkText="Ajustador Aseguradora" WatermarkCssClass="water input-large" />                                    
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label82" runat="server" Text="Deducible: $" CssClass="textoBold alingMiddle"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:TextBox ID="txtDeducibleOrden" runat="server" CssClass="alingMiddle input-small" MaxLength="14"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtDeducibleOrdenWatermarkExtender1" runat="server" BehaviorID="txtDeducibleOrden_TextBoxWatermarkExtender" TargetControlID="txtDeducibleOrden" WatermarkText="Deducible" WatermarkCssClass="water input-small" />                                    
                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" FilterType="Numbers, Custom" ValidChars="." TargetControlID="txtDeducibleOrden"/>
                    </div>
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label85" runat="server" Text="Aseguradora:" CssClass="textoBold alingMiddle"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="ddlAseguradoraOrden" AllowCustomText="true" Width="210px" MaxHeight="300px" DataSourceID="SqlDataSource20" DataTextField="razon_social" DataValueField="id_cliprov" Skin="MetroTouch" EmptyMessage="Seleccione Aseguradora" Filter="Contains" ></telerik:RadComboBox>
                        <asp:SqlDataSource ID="SqlDataSource20" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select 0 as id_cliprov, '' as razon_social union all select id_cliprov, razon_social from Cliprov where aseguradora=1 and tipo='C' and estatus='A' order by 1"></asp:SqlDataSource>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label4" runat="server" Text="% Deducible:" CssClass="textoBold alingMiddle"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:TextBox ID="txtPorcDeducible" runat="server" CssClass="alingMiddle input-small" MaxLength="6"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtPorcDeducibleWatermarkExtender1" runat="server" BehaviorID="txtPorcDeducible_TextBoxWatermarkExtender" TargetControlID="txtPorcDeducible" WatermarkText="% Deducible" WatermarkCssClass="water input-small" />                                    
                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers, Custom" ValidChars="." TargetControlID="txtPorcDeducible"/>
                    </div>
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label5" runat="server" Text="Demérito: $" CssClass="textoBold alingMiddle"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:TextBox ID="txtDemerito" runat="server" CssClass="alingMiddle input-small" MaxLength="9"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtDemeritoWatermarkExtender1" runat="server" BehaviorID="txtDemerito_TextBoxWatermarkExtender" TargetControlID="txtDemerito" WatermarkText="Demérito" WatermarkCssClass="water input-small" />                                    
                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Numbers, Custom" ValidChars="." TargetControlID="txtDemerito"/>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label86" runat="server" Text="Observacion Recepción:" CssClass="textoBold alingMiddle"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:TextBox ID="txtObsOrden" runat="server" CssClass="alingMiddle textNota " MaxLength="200" TextMode="MultiLine" Rows="10"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtObsOrdenWatermarkExtender1" runat="server" BehaviorID="txtObsOrden_TextBoxWatermarkExtender" TargetControlID="txtObsOrden" WatermarkText="Observaciones Recepción" WatermarkCssClass="water textNota" />                                    
                    </div>                    
                    
                </div>                
            </asp:Panel>
            <div class="row">                                    
                <div class="col-lg-12 col-sm-12 text-center pad1m">
                    <asp:LinkButton ID="lnkGuardarRecepcion" runat="server" ToolTip="Guarda Cambios" CssClass="btn btn-success t14" ValidationGroup="orden" onclick="lnkGuardarRecepcion_Click" ><i class="fa fa-save"></i>&nbsp;<span>Guardar Cambios</span></asp:LinkButton>
                </div>
            </div>
            
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
