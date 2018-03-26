<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VehiculoOrden.aspx.cs" Inherits="VehiculoOrden" MasterPageFile="~/AdmonOrden.master" Culture="es-Mx" UICulture="es-Mx" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">  
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true"/>    
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server"></telerik:RadSkinManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-lg-12 col-sm-12 text-center alert-info">
                    <h3>
                        <i class="fa fa-car"></i>&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="Label1" runat="server" Text="Características"></asp:Label>  
                        <asp:Label ID="lblIniciales" runat="server" Visible="false"></asp:Label>                        
                    </h3>                    
                </div>
            </div>
            <div class="row">
                <div class="col-lg-12 col-sm-12 text-center"> 
                        <asp:Label ID="lblErrorVehiculo" runat="server" CssClass="errores alert-danger"></asp:Label>                        
                    </h3>                    
                </div>
            </div>
            <asp:Panel runat="server" ID="pnlVehiculo" CssClass="paneles col-lg-12 col-sm-12" ScrollBars="Auto">
                <div class="row">
                    <div class="col-lg-2 col-sm-2 text-left">
                        <asp:Label ID="Label15" runat="server" Text="Marca:" CssClass="alingMiddle textoBold"></asp:Label>
                        <asp:Label ID="lblIdVehiculo" runat="server" Visible="false"></asp:Label>
                    </div>
                    <div class="col-lg-4 col-sm-4 text-left">                        
                        <telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="ddlMarcaOrd" AllowCustomText="true" CssClass="input-xlarge" Width="300px" MaxHeight="300px" DataSourceID="SqlDataSource13" Skin="MetroTouch" DataTextField="marca" DataValueField="id_marca" EmptyMessage="Seleccione Marca" Filter="Contains" AutoPostBack="true" OnSelectedIndexChanged="ddlMarcaOrd_SelectedIndexChanged"></telerik:RadComboBox>                                        
                        <asp:SqlDataSource ID="SqlDataSource13" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select id_marca, descripcion as marca from marcas">
                        </asp:SqlDataSource>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="Debe indicar la marca del Vehículo" Text="*" ControlToValidate="ddlMarcaOrd" CssClass="alingMiddle errores" ValidationGroup="orden"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label16" runat="server" Text="Vehículo:" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">                        
                        <telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="ddlTvOrden" AllowCustomText="true" CssClass="input-xlarge" Width="300px" MaxHeight="300px" DataSourceID="SqlDataSource14" DataTextField="descripcion" DataValueField="id_tipo_vehiculo" Skin="MetroTouch" EmptyMessage="Seleccione Vehículo" Filter="Contains" AutoPostBack="true" OnSelectedIndexChanged="ddlTvOrden_SelectedIndexChanged" ></telerik:RadComboBox>
                        <asp:SqlDataSource ID="SqlDataSource14" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select id_tipo_vehiculo, descripcion from Tipo_Vehiculo"></asp:SqlDataSource>                                            
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="Debe seleccionar ún tipo de vehículo" Text="*" CssClass="errores alingMiddle" ValidationGroup="orden" ControlToValidate="ddlTvOrden"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label17" runat="server" Text="Línea:" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="ddlUnidadOrden" AllowCustomText="true" CssClass="input-xlarge" Width="300px" MaxHeight="300px" DataSourceID="SqlDataSource12" DataTextField="descripcion" DataValueField="id_tipo_unidad" Skin="MetroTouch" EmptyMessage="Seleccione Línea" Filter="Contains" ></telerik:RadComboBox>                                                                                
                        <asp:SqlDataSource ID="SqlDataSource12" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select id_tipo_unidad, upper(descripcion) as descripcion from Tipo_Unidad where id_marca=@id_marca and id_tipo_vehiculo=@id_tipo_vehiculo">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="ddlMarcaOrd" DefaultValue="0" Type="Int32" Name="id_marca" PropertyName="SelectedValue" />
                                <asp:ControlParameter ControlID="ddlTvOrden" DefaultValue="0" Type="Int32" Name="id_tipo_vehiculo" PropertyName="SelectedValue" />
                            </SelectParameters>
                        </asp:SqlDataSource>                                                                 
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="Debe seleccionar una línea" Text="*" CssClass="errores alingMiddle" ValidationGroup="orden" ControlToValidate="ddlUnidadOrden"></asp:RequiredFieldValidator>
                    </div>                    
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label18" runat="server" Text="Modelo:" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:TextBox ID="txtModeloOrden" runat="server" CssClass="alingMiddle input-mini" MaxLength="4"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtModeloOrdenWatermarkExtender" runat="server" BehaviorID="txtModeloOrden_TextBoxWatermarkExtender" TargetControlID="txtModeloOrden" WatermarkText="Modelo" WatermarkCssClass="water input-mini" />
                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Numbers" TargetControlID="txtModeloOrden"/>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ErrorMessage="Debe indicar el modelo" Text="*" CssClass="errores alingMiddle" ValidationGroup="orden" ControlToValidate="txtModeloOrden"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label19" runat="server" Text="Placas:" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:TextBox ID="txtPlacasOrden" runat="server" MaxLength="12" CssClass="input-small alingMiddle" Enabled="false"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtPlacasOrdenWatermarkExtender" runat="server" BehaviorID="txtPlacasOrden_TextBoxWatermarkExtender" TargetControlID="txtPlacasOrden" WatermarkText="Placa" WatermarkCssClass="water input-small" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ErrorMessage="Debe indicar la placa del vehículo" Text="*" CssClass="errores alingMiddle" ValidationGroup="orden" ControlToValidate="txtPlacasOrden"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label20" runat="server" Text="Serie/VIN:" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:TextBox ID="txtSerieOrden" runat="server" MaxLength="20" CssClass="alingMiddle input-large"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtSerieOrdenWatermarkExtender1" runat="server" BehaviorID="txtSerieOrden_TextBoxWatermarkExtender" TargetControlID="txtSerieOrden" WatermarkText="Serie/Vin" WatermarkCssClass="water input-large" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label21" runat="server" Text="Motor:" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:TextBox ID="txtMotorOrden" runat="server" CssClass="alingMiddle input-large" MaxLength="20"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtMotorOrdenWatermarkExtender1" runat="server" BehaviorID="txtMotorOrden_TextBoxWatermarkExtender" TargetControlID="txtMotorOrden" WatermarkText="Motor" WatermarkCssClass="water input-large" />                                            
                    </div>
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label22" runat="server" Text="Color Exterior:" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:TextBox ID="txtColorExtOrden" runat="server" MaxLength="20" CssClass="alingMiddle input-large"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtColorExtOrdenWatermarkExtender" runat="server" BehaviorID="txtColorExtOrden_TextBoxWatermarkExtender" TargetControlID="txtColorExtOrden" WatermarkText="Color Ext." WatermarkCssClass="water input-large" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ErrorMessage="Debe indicar el color exterior del vehículo" Text="*" CssClass="errores alingMiddle" ValidationGroup="orden" ControlToValidate="txtColorExtOrden"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label23" runat="server" Text="Color Interior:" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:TextBox ID="txtColorIntOrden" runat="server" MaxLength="20" CssClass="alingMiddle input-large"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtColorIntOrdenWatermarkExtender" runat="server" BehaviorID="txtColorIntOrden_TextBoxWatermarkExtender" TargetControlID="txtColorIntOrden" WatermarkText="Color Int." WatermarkCssClass="water input-large" />
                    </div>
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label24" runat="server" Text="Transmisión:" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:DropDownList ID="ddlTransOrden" runat="server" DataSourceID="SqlDataSource15" CssClass="alingMiddle input-large" DataTextField="descripcion" DataValueField="id_tipo_transmision">                                                
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSource15" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select 0 as id_tipo_transmision, '' as descripcion union all select id_tipo_transmision,descripcion from Tipo_Transmision order by 1">                                    
                        </asp:SqlDataSource>                                            
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label25" runat="server" Text="Tracción:" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:DropDownList ID="ddlTracOrden" runat="server" DataSourceID="SqlDataSource16" CssClass="alingMiddle input-large" DataTextField="descripcion" DataValueField="id_tipo_traccion">                                                
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSource16" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select 0 as id_tipo_traccion, '' as descripcion union all select id_tipo_traccion,descripcion from Tipo_Tracccion order by 1">                                    
                        </asp:SqlDataSource>
                    </div>
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label26" runat="server" Text="Cilindros:" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:TextBox ID="txtCilindrosOrden" runat="server" CssClass="alingMiddle input-mini" MaxLength="2"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtCilindrosOrdenWatermarkExtender" runat="server" BehaviorID="txtCilindrosOrden_TextBoxWatermarkExtender" TargetControlID="txtCilindrosOrden" WatermarkText="Cilindros" WatermarkCssClass="water input-mini" />
                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="Numbers" TargetControlID="txtCilindrosOrden"/>                                            
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label27" runat="server" Text="Versión:" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:TextBox ID="txtVersionOrden" runat="server" CssClass="alingMiddle input-large" MaxLength="20"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtVersionOrdenWatermarkExtender1" runat="server" BehaviorID="txtVersionOrden_TextBoxWatermarkExtender" TargetControlID="txtVersionOrden" WatermarkText="Versión" WatermarkCssClass="water input-large" />
                    </div>
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label28" runat="server" Text="Puertas:" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:TextBox ID="txtPuertasOrden" runat="server" CssClass="alingMiddle input-mini" MaxLength="2"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtPuertasOrdenWatermarkExtender1" runat="server" BehaviorID="txtPuertasOrden_TextBoxWatermarkExtender" TargetControlID="txtPuertasOrden" WatermarkText="Puertas" WatermarkCssClass="water input-mini" />
                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" FilterType="Numbers" TargetControlID="txtPuertasOrden"/>                                            
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label29" runat="server" Text="No. Económico:" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:TextBox ID="txtNoEcoOrden" runat="server" CssClass="alingMiddle input-large" MaxLength="20"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtNoEcoOrdenWatermarkExtender1" runat="server" BehaviorID="txtNoEcoOrden_TextBoxWatermarkExtender" TargetControlID="txtNoEcoOrden" WatermarkText="No. Económico" WatermarkCssClass="water input-large" />
                    </div>
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label30" runat="server" Text="Rin:" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:DropDownList ID="ddlRinOrden" runat="server" DataSourceID="SqlDataSource17" CssClass="alingMiddle input-large" DataTextField="descripcion" DataValueField="id_tipo_rin">                                                
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSource17" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select 0 as id_tipo_rin, '' as descripcion union all select id_tipo_rin,descripcion from Tipo_Rin order by 1">                                    
                        </asp:SqlDataSource>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label31" runat="server" Text="LLanta:" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:TextBox ID="txtLlantaOrden" runat="server" CssClass="alingMiddle input-large" MaxLength="20"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtLlantaOrdenWatermarkExtender1" runat="server" BehaviorID="txtLlantaOrden_TextBoxWatermarkExtender" TargetControlID="txtLlantaOrden" WatermarkText="Llanta" WatermarkCssClass="water input-large" />
                    </div>
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label32" runat="server" Text="Quemacocos:" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:RadioButtonList ID="rblQuemaCocoOrden" runat="server" RepeatColumns="3" RepeatDirection="Horizontal" CssClass="ancho30">
                            <asp:ListItem Text="Si" Value="1"></asp:ListItem>
                            <asp:ListItem Text="No" Value="0"></asp:ListItem>
                            <asp:ListItem Text="N/A" Value="-1"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label33" runat="server" Text="Bolsas de Aire:" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:RadioButtonList ID="rdlBolsasOrden" runat="server" RepeatColumns="3" RepeatDirection="Horizontal"  CssClass="ancho30">
                            <asp:ListItem Text="Si" Value="1"></asp:ListItem>
                            <asp:ListItem Text="No" Value="0"></asp:ListItem>
                            <asp:ListItem Text="N/A" Value="-1"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label34" runat="server" Text="Aire Acondicionado:" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:RadioButtonList ID="rdlAireOrden" runat="server" RepeatColumns="3" RepeatDirection="Horizontal" CssClass="ancho30" >
                            <asp:ListItem Text="Si" Value="1"></asp:ListItem>
                            <asp:ListItem Text="No" Value="0"></asp:ListItem>
                            <asp:ListItem Text="N/A" Value="-1"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label35" runat="server" Text="Dir. Hidráulica:" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:RadioButtonList ID="rdlHidraulica" runat="server" RepeatColumns="3" RepeatDirection="Horizontal"  CssClass="ancho30">
                            <asp:ListItem Text="Si" Value="1"></asp:ListItem>
                            <asp:ListItem Text="No" Value="0"></asp:ListItem>
                            <asp:ListItem Text="N/A" Value="-1"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label36" runat="server" Text="Elevadores Puertas:" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:RadioButtonList ID="rdlElevadoresOrden" runat="server" RepeatColumns="3" RepeatDirection="Horizontal"  CssClass="ancho80">
                            <asp:ListItem Text="Manuales" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Eléctricos" Value="0"></asp:ListItem>
                            <asp:ListItem Text="N/A" Value="-1"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label37" runat="server" Text="Espejos Laterales:" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:RadioButtonList ID="rdlEspejosOrden" runat="server" RepeatColumns="3" RepeatDirection="Horizontal"  CssClass="ancho80">
                            <asp:ListItem Text="Manuales" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Eléctricos" Value="0"></asp:ListItem>
                            <asp:ListItem Text="N/A" Value="-1"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div> 
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label38" runat="server" Text="Color Espejos:" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:RadioButtonList ID="rdlColorEspOrden" runat="server" RepeatColumns="3" RepeatDirection="Horizontal"  CssClass="ancho80">
                            <asp:ListItem Text="Negros" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Color Auto" Value="0"></asp:ListItem>
                            <asp:ListItem Text="N/A" Value="-1"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label39" runat="server" Text="Molduras Laterales:" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:RadioButtonList ID="rdlMoldurasOrden" runat="server" RepeatColumns="3" RepeatDirection="Horizontal" CssClass="ancho30">
                            <asp:ListItem Text="Si" Value="1"></asp:ListItem>
                            <asp:ListItem Text="No" Value="0"></asp:ListItem>
                            <asp:ListItem Text="N/A" Value="-1"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label40" runat="server" Text="Cantoneras:" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:RadioButtonList ID="rdlCantoneras" runat="server" RepeatColumns="3" RepeatDirection="Horizontal" CssClass="ancho80">
                            <asp:ListItem Text="Negros" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Color Auto" Value="0"></asp:ListItem>
                            <asp:ListItem Text="N/A" Value="-1"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label41" runat="server" Text="Vestiduras:" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:RadioButtonList ID="rdlVestidurasOrden" runat="server" RepeatColumns="3" RepeatDirection="Horizontal" CssClass="ancho80">
                            <asp:ListItem Text="Tela" Value="T"></asp:ListItem>
                            <asp:ListItem Text="Piel" Value="P"></asp:ListItem>
                            <asp:ListItem Text="N/A" Value="-1"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label42" runat="server" Text="Faros de Niebla:" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:RadioButtonList ID="rdlFarosOrden" runat="server" RepeatColumns="3" RepeatDirection="Horizontal" CssClass="ancho30">
                            <asp:ListItem Text="Si" Value="1"></asp:ListItem>
                            <asp:ListItem Text="No" Value="0"></asp:ListItem>
                            <asp:ListItem Text="N/A" Value="-1"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label43" runat="server" Text="Facia o Defensa:" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:RadioButtonList ID="rdlFaciaOrden" runat="server" RepeatColumns="3" RepeatDirection="Horizontal" CssClass="ancho80">
                            <asp:ListItem Text="Corrugada" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Color Auto" Value="0"></asp:ListItem>
                            <asp:ListItem Text="N/A" Value="-1"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label44" runat="server" Text="Cabina:" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:RadioButtonList ID="rdlCabinaOrden" runat="server" RepeatColumns="3" RepeatDirection="Horizontal" CssClass="ancho80">
                            <asp:ListItem Text="Sencilla" Value="S"></asp:ListItem>
                            <asp:ListItem Text="Doble" Value="D"></asp:ListItem>
                            <asp:ListItem Text="N/A" Value="-1"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label45" runat="server" Text="Defensa Cromada:" CssClass="alingMiddle textoBold"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:RadioButtonList ID="rdlDefensaOrden" runat="server" RepeatColumns="3" RepeatDirection="Horizontal" CssClass="ancho30">
                            <asp:ListItem Text="Si" Value="1"></asp:ListItem>
                            <asp:ListItem Text="No" Value="0"></asp:ListItem>
                            <asp:ListItem Text="N/A" Value="-1"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>                                        
                </div>                
            </asp:Panel>
            <div class="row pad1m">                                        
                <div class="col-lg-6 col-sm-6 text-center">
                    <asp:LinkButton ID="btnImprime" runat="server" ToolTip="Datos del Vehículo"
                        CssClass="btn btn-info t14" onclick="btnImprime_Click" ><i class="fa fa-print"></i><span>&nbsp;Imprime Datos de Veh&iacute;culo</span></asp:LinkButton>
                </div>
                <div class="col-lg-6 col-sm-6 text-center">
                    <asp:LinkButton ID="lnkGuardarVehiculo" runat="server" ToolTip="Guarda Cambios" CssClass="btn btn-success t14 textoBold" ValidationGroup="orden" onclick="lnkGuardarVehiculo_Click" ><i class="fa fa-save"></i>&nbsp;<span>Guardar Cambios</span></asp:LinkButton>
                </div>
            </div>

            <div class="pie pad1m">		                                		                                
		        <div class="clearfix">
			        <div class="row colorBlanco textoBold">
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label2" runat="server" Text="Tipo Orden:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="ddlToOrden" runat="server" ></asp:Label>
                        </div>
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label4" runat="server" Text="Cliente:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="ddlClienteOrden" runat="server" ></asp:Label>
                        </div>
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label6" runat="server" Text="Tipo Servicio:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="ddlTsOrden" runat="server" ></asp:Label>
                        </div>
                    </div>                                              
                    <div class="row colorBlanco textoBold">
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label8" runat="server" Text="Tipo Valuación:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="ddlValOrden" runat="server" ></asp:Label>
                        </div>
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label10" runat="server" Text="Tipo Asegurado:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="ddlTaOrden" runat="server" ></asp:Label>
                        </div>
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label12" runat="server" Text="Localización:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="ddlLocOrden" runat="server" ></asp:Label>
                        </div>
                    </div>    
                    <div class="row colorBlanco textoBold">
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label108" runat="server" Text="Perfil:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="ddlPerfil" runat="server" ></asp:Label>
                        </div>
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label109" runat="server" Text="Siniestro:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="lblSiniestro" runat="server" ></asp:Label>
                        </div>
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label110" runat="server" Text="Deducible:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="lblDeducible" runat="server" ></asp:Label>
                        </div>
                    </div>  
                    <div class="row colorBlanco textoBold">
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label111" runat="server" Text="Total Orden:" CssClass="colorEtiqueta" Visible="false"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="lblTotOrden" runat="server" Visible="false" ></asp:Label>
                        </div>  
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label112" runat="server" Text="Promesa:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="lblEntregaEstimada" runat="server" ></asp:Label>
                        </div> 
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="lblPorcDeduEti" runat="server" Text="% Deducible:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="lblPorcDedu" runat="server" ></asp:Label>
                        </div>                     
                    </div>
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
