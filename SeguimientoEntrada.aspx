<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SeguimientoEntrada.aspx.cs" Inherits="SeguimientoEntrada" MasterPageFile="~/AdmonOrden.master" Culture="es-Mx" UICulture="es-Mx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-lg-12 col-sm-12 text-center alert-info">
                    <h3>
                        <i class="fa fa-clock-o"></i>&nbsp;<i class="fa fa-user-plus"></i>&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="Label1" runat="server" Text="Registro de Entrega"></asp:Label>
                    </h3>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-12 col-sm-12 text-center">
                    <asp:Label ID="lblError" runat="server" CssClass="errores" />
                </div>
            </div>
            <br />
            <asp:Panel runat="server" ID="pnlInconf" CssClass="paneles col-lg-12 col-sm-12" ScrollBars="Auto">
                <div class="row">

                    <div class="col-lg-12 col-sm-12 text-center">
                        <div class="row">
                            <div class="col-lg-2 col-sm-2 text-right">
                                <asp:Label ID="Label211" runat="server" Text="Localización:" CssClass="textoBold alingMiddle" />
                            </div>
                            <div class="col-lg-2 col-sm-2 text-left">
                                <asp:Label ID="lblLocalizacionIni" runat="server" Visible="false"></asp:Label>
                                <asp:Label ID="lblAvanceIni" runat="server" Visible="false"></asp:Label>
                                <asp:Label ID="lblFaseIni" runat="server" Visible="false"></asp:Label>
                                <asp:Label ID="lblPerfilIni" runat="server" Visible="false"></asp:Label>
                                <asp:DropDownList ID="ddlLocalizacion" AutoPostBack="true" OnSelectedIndexChanged="ddlLocalizacion_SelectedIndexChanged" runat="server" DataSourceID="SqlDataSource26" DataTextField="descripcion" DataValueField="id_localizacion" CssClass="alingMiddle input-medium"></asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource26" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select id_localizacion, descripcion from Localizaciones"></asp:SqlDataSource>
                            </div>
                            <div class="col-lg-2 col-sm-2 text-right">
                                <asp:Label ID="Label14" runat="server" Text="Perfil:" CssClass="textoBold alingMiddle" />
                            </div>
                            <div class="col-lg-2 col-sm-2 text-left">
                                <asp:DropDownList ID="DropDownList1" AutoPostBack="true" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" runat="server" DataSourceID="SqlDataSource2" DataTextField="descripcion" DataValueField="id_perfilOrden" CssClass="alingMiddle input-large"></asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select id_perfilOrden, descripcion from perfilesOrdenes"></asp:SqlDataSource>
                            </div>
                            <div class="col-lg-2 col-sm-2 text-right">
                                <asp:Label ID="Label22" runat="server" Text="% Avance:" CssClass="textoBold alingMiddle" />
                            </div>
                            <div class="col-lg-2 col-sm-2 text-left">
                                <asp:TextBox ID="txtAvance" runat="server" MaxLength="5" CssClass="alingMiddle input-mini" Enabled="false"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" BehaviorID="txtAvance_FilteredTextBoxExtender" TargetControlID="txtAvance" FilterType="Numbers, Custom" ValidChars="." />
                            </div>
                        </div>
                    </div>
                    <br />
                    <br />
                    <br />
                    <div class="col-lg-12 col-sm-12 text-center">
                        <div class="row">
                            <div class="col-lg-2 col-sm-2 text-right">
                                <asp:Label ID="Label3" runat="server" Text="Fecha Baja Portal:" CssClass="textoBold alingMiddle" />
                            </div>
                            <div class="col-lg-2 col-sm-2 text-left">
                                <asp:TextBox ID="txtFechaBaja" runat="server" CssClass="input-small" Enabled="false" />
                                <asp:LinkButton ID="LinkButton1" runat="server" CssClass="btn btn-info"><i class="fa fa-calendar"></i></asp:LinkButton>
                                <cc1:CalendarExtender ID="CalendarExtender16" runat="server" TargetControlID="txtFechaBaja" Format="yyyy-MM-dd" PopupButtonID="LinkButton1" />
                            </div>
                            <div class="col-lg-2 col-sm-2 text-right">
                                <asp:Label ID="Label13" runat="server" Text="Fecha Entrega:" CssClass="textoBold alingMiddle" />
                            </div>
                            <div class="col-lg-2 col-sm-2 text-left">
                                <asp:TextBox ID="txtFechaEntrega" runat="server" CssClass="input-small" Enabled="false" />
                                <asp:LinkButton ID="LinkButton2" runat="server" CssClass="btn btn-info"><i class="fa fa-calendar"></i></asp:LinkButton>
                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFechaEntrega" Format="yyyy-MM-dd" PopupButtonID="LinkButton2" />
                            </div>
                            <div class="col-lg-2 col-sm-2 text-right">
                                <asp:Label ID="Label19" runat="server" Text="Hora Entrega:" CssClass="textoBold alingMiddle" />
                            </div>
                            <div class="col-lg-2 col-sm-2 text-left">
                                <telerik:RadTimePicker RenderMode="Lightweight" ID="timpHoraEntrega" Culture="es-Mx" DateInput-DateFormat="HH:mm:ss" DateInput-EmptyMessage="Hora" Skin="MetroTouch" Width="120px" runat="server"></telerik:RadTimePicker>
                            </div>
                        </div>
                    </div>
                    <br />
                    <br />
                    <br />
                    <div class="col-lg-12 col-sm-12 text-center">
                        <div class="row">
                            <div class="col-lg-2 col-sm-2 text-right">
                                <asp:Label ID="Label17" runat="server" Text="Fecha Entrada Cliente:" CssClass="textoBold alingMiddle" />
                            </div>
                            <div class="col-lg-2 col-sm-2 text-left">
                                <asp:TextBox ID="txtFechaEntrada" runat="server" CssClass="input-small" Enabled="false" />
                                <asp:LinkButton ID="LinkButton4" runat="server" CssClass="btn btn-info"><i class="fa fa-calendar"></i></asp:LinkButton>
                                <cc1:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtFechaEntrada" Format="yyyy-MM-dd" PopupButtonID="LinkButton4" />
                            </div>
                            <div class="col-lg-2 col-sm-2 text-right">
                                <asp:Label ID="Label18" runat="server" Text="Hora Entrada Cliente:" CssClass="textoBold alingMiddle" />
                            </div>
                            <div class="col-lg-2 col-sm-2 text-left">
                                <telerik:RadTimePicker RenderMode="Lightweight" ID="timpHoraEntrada" Culture="es-Mx" DateInput-DateFormat="HH:mm:ss" DateInput-EmptyMessage="Hora" Skin="MetroTouch" Width="120px" runat="server"></telerik:RadTimePicker>                                
                            </div>
                        </div>
                    </div>
                    <br />
                    <br />
                    <br />
                    <div class="col-lg-12 col-sm-12 text-center">
                        <div class="row">
                            <div class="col-lg-2 col-sm-2 text-right">
                                <asp:Label ID="Label5" runat="server" Text="Fecha Salida Cliente:" CssClass="textoBold alingMiddle" />
                            </div>
                            <div class="col-lg-2 col-sm-2 text-left">
                                <asp:TextBox ID="txtFechaSalCliente" runat="server" CssClass="input-small" Enabled="false" />
                                <asp:LinkButton ID="LinkButton3" runat="server" CssClass="btn btn-info"><i class="fa fa-calendar"></i></asp:LinkButton>
                                <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtFechaSalCliente" Format="yyyy-MM-dd" PopupButtonID="LinkButton3" />
                            </div>
                            <div class="col-lg-2 col-sm-2 text-right">
                                <asp:Label ID="Label7" runat="server" Text="Hora Salida Cliente:" CssClass="textoBold alingMiddle" />
                            </div>
                            <div class="col-lg-2 col-sm-2 text-left">
                                <telerik:RadTimePicker RenderMode="Lightweight" ID="timpSalCliente" Culture="es-Mx" DateInput-DateFormat="HH:mm:ss" DateInput-EmptyMessage="Hora" Skin="MetroTouch" Width="120px" runat="server"></telerik:RadTimePicker>                                
                            </div>
                        </div>
                    </div>
                    <br />
                    <br />
                    <br />
                    <div class="col-lg-12 col-sm-12 text-center">
                        <div class="row">
                            <div class="col-lg-2 col-sm-2 text-right">
                                <asp:Label ID="Label9" runat="server" Text="Nombre Entregó:" CssClass="textoBold alingMiddle" />
                            </div>
                            <div class="col-lg-2 col-sm-2 text-left">
                                <asp:TextBox ID="txtEntrego" MaxLength="100" runat="server" CssClass="input-large" />
                            </div>
                            <div class="col-lg-2 col-sm-2 text-right">
                                <asp:Label ID="Label11" runat="server" Text="Nombre Recibió:" CssClass="textoBold alingMiddle" />
                            </div>
                            <div class="col-lg-4 col-sm-4 text-left">
                                <asp:TextBox ID="txtRecibio" MaxLength="100" runat="server" CssClass="input-large" />
                            </div>
                        </div>
                    </div>

                    <div class="col-lg-12 col-sm-12 text-center">
                        <br />
                        <asp:LinkButton ID="lnkGuarda" runat="server" ToolTip="Guarda Ingreso Cliente" CssClass="btn btn-success t14" OnClick="lnkGuarda_Click"><i class="fa fa-save"></i><span>&nbsp;Guarda Ingreso Cliente</span></asp:LinkButton>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkTerminarOrden" runat="server" ToolTip="Terminar Orden" CssClass="btn btn-info t14" OnClick="lnkTerminarOrden_Click"><i class="fa fa-lock"></i><span>&nbsp;Terminar Orden</span></asp:LinkButton>
                    </div>
                </div>

                <asp:Panel ID="PanelMask" runat="server" CssClass="mask" Visible="false"></asp:Panel>
                <asp:Panel ID="PanelPopUpPermiso" runat="server" CssClass="popUp zen2  textoCentrado ancho40 centrado" Visible="false">
                    <table class="ancho100">
                        <tr class="ancho100 centrado">
                            <td class="ancho100 text-center encabezadoTabla roundTopLeft ">
                                <asp:Label ID="lblPop" runat="server" Text="Autorización" CssClass="t22 colorMorado textoBold" />
                            </td>
                        </tr>
                    </table>
                    <asp:Panel ID="prov" runat="server" CssClass="ancho80 centrado">
                        <div class="row">
                            <div class="col-lg-4 col-sm-4 text-left">
                                <asp:Label ID="Label15" runat="server" Text="Usuario:" CssClass="textoBold" />
                            </div>
                            <div class="col-lg-8 col-sm-8 text-left">
                                <asp:TextBox ID="txtUsuarioLog" runat="server" CssClass="login input-large" MaxLength="20" placeholder="Usuario" TextMode="Password" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" Text="*" ForeColor="Red" ValidationGroup="log" ControlToValidate="txtUsuarioLog" ErrorMessage="Debe indicar el usuario." CssClass="pull-right" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" Text="*" ForeColor="Red" ValidationGroup="log" ErrorMessage="El usuario debe contener de entre 3 y 20 caracteres." CssClass="pull-right" ControlToValidate="txtUsuarioLog" ValidationExpression="[a-zA-Z0-9]{3,20}" />
                            </div>
                            <div class="col-lg-4 col-sm-4 text-left padding-top-10">
                                <asp:Label ID="Label16" runat="server" Text="Contraseña:" CssClass="textoBold" />&nbsp;
                            </div>
                            <div class="col-lg-8 col-sm-8 text-left padding-top-10">
                                <asp:TextBox ID="txtContraseñaLog" runat="server" CssClass="login input-large" TextMode="Password" MaxLength="20" placeholder="Contraseña" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" Text="*" ForeColor="Red" ValidationGroup="log" ControlToValidate="txtContraseñaLog" ErrorMessage="Debe indicar la contraseña." CssClass="pull-right" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" Text="*" ForeColor="Red" ValidationGroup="log" ErrorMessage="La contraseña debe contener de entre 5 y 20 caracteres." CssClass="pull-right" ControlToValidate="txtContraseñaLog" ValidationExpression="[a-zA-Z0-9]{5,20}" />
                            </div>
                            <div class="field col-lg-12 col-sm-12 textoCentrado textoBold padding-top-10">
                                <div class="col-lg-12 col-sm-12 text-center">
                                    <asp:Label ID="lblErrorLog" runat="server" CssClass="errores" />
                                </div>
                                <div class="col-lg-12 col-sm-12 text-center">
                                    <asp:ValidationSummary ID="ValidationSummary4" ValidationGroup="log" CssClass="errores" runat="server" DisplayMode="List" />
                                </div>
                            </div>

                            <div class="col-lg-12 col-sm-12 text-center pad1m">
                                <div class="col-lg-6 col-sm-6 text-center">
                                    <asp:LinkButton ID="btnAceptar" runat="server" OnClick="btnAceptar_Click" CssClass="btn btn-success" ValidationGroup="log"><i class="fa fa-check"></i><span>&nbsp;Autorizar</span></asp:LinkButton>
                                </div>
                                <div class="col-lg-6 col-sm-6 text-center">
                                    <asp:LinkButton ID="btnCancelar" OnClick="btnCancelar_Click" runat="server" CssClass="btn btn-danger"><i class="fa fa-ban"></i><span>&nbsp;Cancelar</span></asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </asp:Panel>

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
                            <asp:Label ID="Label111" runat="server" Text="Total Orden:" CssClass="colorEtiqueta" Visible="false"></asp:Label>&nbsp;&nbsp;
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
                    <asp:Panel ID="pnlMaskLoad" runat="server" CssClass="maskLoad"></asp:Panel>
                    <asp:Panel ID="pnlCargando" runat="server" CssClass="pnlPopUpLoad">
                        <asp:Image ID="imgLoad" runat="server" ImageUrl="~/img/loading.gif" CssClass="ancho100" />
                    </asp:Panel>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
