<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PagoDeducible.aspx.cs" Inherits="PagoDeducible" MasterPageFile="~/AdmonOrden.master" Culture="es-Mx" UICulture="es-Mx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-lg-12 col-sm-12 text-center alert-info">
                    <h3>
                        <i class="fa fa-file-text"></i>&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="Label1" runat="server" Text="Pago Deducible"></asp:Label>
                    </h3>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-12 col-sm-12 text-center">
                    <asp:Label ID="lblError" runat="server" CssClass="errores" />
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="save" CssClass="errores" DisplayMode="List" />
                </div>
            </div>
            <asp:Panel runat="server" ID="pnlDaños" CssClass="paneles col-lg-12 col-sm-12" ScrollBars="Auto">
                <div class="col-lg-12 col-sm-12 text-center pad1m">
                    <div class="col-lg-2 col-sm-2 text-left">
                        <asp:Label runat="server" ID="lblx" Text="Importe de Deducible: " CssClass="alingMiddle textoBold" />&nbsp;&nbsp;
                        <asp:TextBox ID="txtDeducibleOrden" runat="server" CssClass="alingMiddle input-small" MaxLength="14"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtDeducibleOrdenWatermarkExtender1" runat="server" BehaviorID="txtDeducibleOrden_TextBoxWatermarkExtender" TargetControlID="txtDeducibleOrden" WatermarkText="Deducible" WatermarkCssClass="water input-small" />                                    
                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" FilterType="Numbers, Custom" ValidChars="." TargetControlID="txtDeducibleOrden"/>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Debe indicar el monto del deducible" CssClass="errores" ValidationGroup="save" ControlToValidate="txtDeducibleOrden" Text="*"></asp:RequiredFieldValidator>
                    </div>

                    <div class="col-lg-2 col-sm-2 text-left">
                        <asp:Label runat="server" ID="Label7" Text="Importe de Demérito: " CssClass="alingMiddle textoBold" />&nbsp;&nbsp;
                        <asp:TextBox ID="txtDemerito" runat="server" CssClass="alingMiddle input-small" MaxLength="14"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtDemeritoWatermarkExtender1" runat="server" BehaviorID="txtDemerito_TextBoxWatermarkExtender" TargetControlID="txtDeducibleOrden" WatermarkText="Demérito" WatermarkCssClass="water input-small" />                                    
                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers, Custom" ValidChars="." TargetControlID="txtDemerito"/>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Debe indicar el monto de demérito" CssClass="errores" ValidationGroup="save" ControlToValidate="txtDemerito" Text="*"></asp:RequiredFieldValidator>
                    </div>

                    <div class="col-lg-6 col-sm-6 text-left" >
                        <asp:Label ID="Label3" runat="server" Text="Cliente:"></asp:Label>
                        <asp:TextBox ID="txtNombreOrden" runat="server" CssClass="alingMiddle input-large" MaxLength="100" ></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtNombreOrdenWatermarkExtender1" runat="server" BehaviorID="txtNombreOrden_TextBoxWatermarkExtender" TargetControlID="txtNombreOrden" WatermarkText="Nombre" WatermarkCssClass="water input-large" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Debe indicar el nombre del cliente" CssClass="errores" ValidationGroup="save" ControlToValidate="txtNombreOrden" Text="*"></asp:RequiredFieldValidator>
                        <asp:Label ID="Label5" runat="server" Text="Apellidos:"></asp:Label>
                        <asp:TextBox ID="txtApPatOrden" runat="server" CssClass="alingMiddle input-medium" MaxLength="50" ></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtApPatOrdenWatermarkExtender1" runat="server" BehaviorID="txtApPatOrden_TextBoxWatermarkExtender" TargetControlID="txtApPatOrden" WatermarkText="Apellido Paterno" WatermarkCssClass="water input-medium" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Debe indicar el apellido paterno del cliente" CssClass="errores" ValidationGroup="save" ControlToValidate="txtApPatOrden" Text="*"></asp:RequiredFieldValidator>
                        <asp:TextBox ID="txtApMatOrden" runat="server" CssClass="alingMiddle input-medium" MaxLength="50" ></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtApMatOrdenWatermarkExtender2" runat="server" BehaviorID="txtApMatOrden_TextBoxWatermarkExtender" TargetControlID="txtApMatOrden" WatermarkText="Apellido Materno" WatermarkCssClass="water input-medium" />
                    </div>
                    <div class="col-lg-2 col-sm-2 text-left">
                        <asp:Label ID="Label9" runat="server" Text="Firmantes:"></asp:Label>
                        <asp:DropDownList ID="ddlFirmantes" runat="server" DataSourceID="SqlDataSource1" DataTextField="firmante" DataValueField="id_firma">
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                            ConnectionString="<%$ ConnectionStrings:PVW %>"
                            SelectCommand="SELECT [id_firma], [firmante] FROM [Firmantes]"></asp:SqlDataSource>                        
                    </div>
                    <div class="col-lg-2 col-sm-2 text-center">
                        <asp:DropDownList ID="ddlForma" runat="server">
                            <asp:ListItem Selected="True" Value="D">Pago de Deducible</asp:ListItem>
                            <asp:ListItem Value="P">Pago de Reparación</asp:ListItem>
                            <asp:ListItem Value="M">Pago de Demérito</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-lg-12 col-sm-12 text-center">
                        <asp:LinkButton ID="btnAceptar" runat="server" ToolTip="Guardar" OnClick="btnAceptar_Click" ValidationGroup="save" CssClass="alingMiddle btn btn-info t14"><i class="fa fa-save"></i>&nbsp;<span>Guardar Cambios</span></asp:LinkButton>
                    </div>
                </div>
            </asp:Panel>
            <div class="pie pad1m">
                <div class="clearfix">
                    <div class="row colorBlanco textoBold">
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label2" runat="server" Text="Tipo Orden:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="ddlToOrden" runat="server"></asp:Label>
                        </div>
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label4" runat="server" Text="Cliente:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="ddlClienteOrden" runat="server"></asp:Label>
                        </div>
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label6" runat="server" Text="Tipo Servicio:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="ddlTsOrden" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="row colorBlanco textoBold">
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label8" runat="server" Text="Tipo Valuación:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="ddlValOrden" runat="server"></asp:Label>
                        </div>
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label10" runat="server" Text="Tipo Asegurado:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="ddlTaOrden" runat="server"></asp:Label>
                        </div>
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label12" runat="server" Text="Localización:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="ddlLocOrden" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="row colorBlanco textoBold">
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label108" runat="server" Text="Perfil:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="ddlPerfil" runat="server"></asp:Label>
                        </div>
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label109" runat="server" Text="Siniestro:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="lblSiniestro" runat="server"></asp:Label>
                        </div>
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label110" runat="server" Text="Deducible:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="lblDeducible" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="row colorBlanco textoBold">
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label111" runat="server" Text="Total Orden:" CssClass="colorEtiqueta"
                                Visible="false"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="lblTotOrden" runat="server" Visible="false"></asp:Label>
                        </div>
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label112" runat="server" Text="Promesa:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="lblEntregaEstimada" runat="server"></asp:Label>
                        </div>
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="lblPorcDeduEti" runat="server" Text="% Deducible:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="lblPorcDedu" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                <ProgressTemplate>
                    <asp:Panel ID="pnlMaskLoad21" runat="server" CssClass="maskLoad">
                    </asp:Panel>
                    <asp:Panel ID="pnlCargando21" runat="server" CssClass="pnlPopUpLoad">
                        <asp:Image ID="imgLoad21" runat="server" ImageUrl="~/img/loading.gif" CssClass="ancho100" />
                    </asp:Panel>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
