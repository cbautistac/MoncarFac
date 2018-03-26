<%@ Page Title="" Language="C#" MasterPageFile="~/AdmonOrden.master" AutoEventWireup="true" CodeFile="RegistroPinturas.aspx.cs" Inherits="RegistroPinturas" Culture="es-Mx" UICulture="es-Mx"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-lg-12 col-sm-12 text-center alert-info">
                    <h3>
                        <i class="fa fa-paint-brush"></i></i>&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblTit" runat="server" Text="Registro de Pintura"></asp:Label>
                    </h3>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-12 col-sm-12 text-center">  
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="errores" DisplayMode="List" ValidationGroup="agrega" />                  
                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" CssClass="errores" DisplayMode="List" ValidationGroup="sigue" />
                    <asp:ValidationSummary ID="ValidationSummary3" runat="server" CssClass="errores" DisplayMode="List" ValidationGroup="agregaDet" />                  
                    <asp:Label ID="lblError" runat="server" CssClass="errores" />

                </div>
            </div>
            <%--Alx: Ya no se usa por integrar PV--%>
            <asp:Panel ID="pnlDatosIni" runat="server" CssClass="col-lg-12 col-sm-12" >
                <div class="row">
                    <div class="col-lg-1 col-sm-1 text-left"><asp:Label ID="Label1" runat="server" Text="Solicitud:" CssClass="textoBold"></asp:Label></div>
                    <div class="col-lg-3 col-sm-3 text-left">
                        <asp:TextBox ID="txtFsolicitud" runat="server" CssClass="alingMiddle input-small" Enabled="false" AutoPostBack="true" ontextchanged="txtFsolicitud_TextChanged"></asp:TextBox>
                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" BehaviorID="txtFsolicitud_CalendarExtender" TargetControlID="txtFsolicitud" Format="yyyy-MM-dd" PopupButtonID="lnkFsolicitud" />
                        <asp:LinkButton ID="lnkFsolicitud" runat="server" CssClass="btn btn-info t14"><i class="fa fa-calendar"></i></asp:LinkButton>
                        <telerik:RadTimePicker RenderMode="Lightweight" ID="txtHsolicitud" Culture="es-Mx" DateInput-DateFormat="HH:mm:ss" DateInput-EmptyMessage="Hora" Skin="MetroTouch" Width="120px" runat="server" OnSelectedDateChanged="txtHsolicitud_SelectedDateChanged" AutoPostBack="true"></telerik:RadTimePicker>                                                
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Debe indicar la fecha de solicitud" Text="*" CssClass="errores" ValidationGroup="agrega" ControlToValidate="txtFsolicitud"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Debe indicar la hora de solicitud" Text="*" CssClass="errores" ValidationGroup="agrega" ControlToValidate="txtHsolicitud"></asp:RequiredFieldValidator>                        
                    </div>
                    <div class="col-lg-1 col-sm-1 text-left"><asp:Label ID="Label3" runat="server" Text="Recepción de Muestra:" CssClass="textoBold"></asp:Label></div>
                    <div class="col-lg-3 col-sm-3 text-left">
                        <asp:TextBox ID="txtFrecepcion" runat="server" CssClass="alingMiddle input-small" Enabled="false"></asp:TextBox>
                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" BehaviorID="txtFrecepcion_CalendarExtender" TargetControlID="txtFrecepcion" Format="yyyy-MM-dd" PopupButtonID="lnkFrecepcion" />
                        <asp:LinkButton ID="lnkFrecepcion" runat="server" CssClass="btn btn-info t14"><i class="fa fa-calendar"></i></asp:LinkButton>
                        <telerik:RadTimePicker RenderMode="Lightweight" ID="txtHrecepcion" Culture="es-Mx" DateInput-DateFormat="HH:mm:ss" DateInput-EmptyMessage="Hora" Skin="MetroTouch" Width="120px" runat="server"></telerik:RadTimePicker>                        
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Debe indicar la fecha de recepción" Text="*" CssClass="errores" ValidationGroup="agrega" ControlToValidate="txtFrecepcion"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Debe indicar la hora de recepción" Text="*" CssClass="errores" ValidationGroup="agrega" ControlToValidate="txtHrecepcion"></asp:RequiredFieldValidator>                        
                    </div>
                    <div class="col-lg-1 col-sm-1 text-left"><asp:Label ID="Label5" runat="server" Text="Operario:" CssClass="textoBold"></asp:Label>
                        <asp:Label ID="lblTipoOrden" runat="server" Visible="false" ></asp:Label>
                    </div>
                    <div class="col-lg-3 col-sm-3 text-left">
                        <asp:DropDownList ID="ddlOperativo" runat="server" AutoPostBack="true" 
                            CssClass="input-medium" DataSourceID="SqlDataSource1" DataTextField="nombre" 
                            DataValueField="IdEmp" 
                            onselectedindexchanged="ddlOperativo_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:Label ID="lblMontoAutorizado" runat="server" ></asp:Label>
                        <asp:Label ID="lblMonto" runat="server" Visible="false"></asp:Label>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                            ConnectionString="<%$ ConnectionStrings:PVW %>" 
                            SelectCommand="select 0 as IdEmp,'Pendiente' as nombre union all select O.IdEmp,UPPER(LTRIM(RTRIM(E.Nombres))+' '+LTRIM(RTRIM(ISNULL(E.Apellido_Paterno,'')))+' '+LTRIM(RTRIM(ISNULL(E.Apellido_Materno,'')))) AS nombre
from Operativos_Orden O
INNER JOIN Empleados E ON E.IdEmp=O.IdEmp
where O.no_orden=@orden and O.id_empresa=@empresa and O.id_taller=@taller and Tipo_Empleado&lt;&gt;'AD' order by 1">
                            <SelectParameters>
                                <asp:QueryStringParameter DefaultValue="0" Name="orden" QueryStringField="o" />
                                <asp:QueryStringParameter DefaultValue="0" Name="empresa" QueryStringField="e" />
                                <asp:QueryStringParameter DefaultValue="0" Name="taller" QueryStringField="t" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                        <asp:SqlDataSource ID="SqlDataSourceTransito" runat="server" 
                            ConnectionString="<%$ ConnectionStrings:PVW %>" 
                            SelectCommand="select -1 as IdEmp,'Programado' as nombre">
                            <SelectParameters>
                                <asp:QueryStringParameter DefaultValue="0" Name="orden" QueryStringField="o" />
                                <asp:QueryStringParameter DefaultValue="0" Name="empresa" QueryStringField="e" />
                                <asp:QueryStringParameter DefaultValue="0" Name="taller" QueryStringField="t" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Debe indicar el operario" ControlToValidate="ddlOperativo" Text="*" CssClass="errores" ValidationGroup="agrega"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-1 col-sm-1 text-left"><asp:Label ID="Label11" runat="server" Text="Recibe:" CssClass="textoBold"></asp:Label></div>
                    <div class="col-lg-3 col-sm-3 text-left">
                        <asp:TextBox ID="txtRecibe" runat="server" MaxLength="250" CssClass="input-large"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtRecibe_TextBoxWatermarkExtender" runat="server" BehaviorID="txtRecibe_TextBoxWatermarkExtender" TargetControlID="txtRecibe" WatermarkText="Recibe" WatermarkCssClass="input-large water" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Debe indicar el nombre de quien recibe" Text="*" CssClass="errores" ControlToValidate="txtRecibe" ValidationGroup="agrega"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-lg-1 col-sm-1 text-left"><asp:Label ID="Label13" runat="server" Text="Entrega:" CssClass="textoBold"></asp:Label></div>
                    <div class="col-lg-3 col-sm-3 text-left">
                        <asp:TextBox ID="txtEntregaMuestra" runat="server" MaxLength="250" CssClass="input-large"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtEntregaMuestraWatermarkExtender3" runat="server" BehaviorID="txtEntregaMuestra_TextBoxWatermarkExtender" TargetControlID="txtEntregaMuestra" WatermarkText="Entrega" WatermarkCssClass="input-large water" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Debe indicar el nombre de quien entrega la muestra" Text="*" CssClass="errores" ControlToValidate="txtEntregaMuestra" ValidationGroup="agrega"></asp:RequiredFieldValidator>
                    </div>

                    <div class="col-lg-1 col-sm-1 text-left"><asp:Label ID="Label23" runat="server" Text="Observaciones:" CssClass="textoBold"></asp:Label></div>
                    <div class="col-lg-3 col-sm-3 text-left">
                        <asp:TextBox ID="txtObservaciones" runat="server" MaxLength="250" CssClass="input-large"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender9" runat="server" BehaviorID="txtObservaciones_TextBoxWatermarkExtender" TargetControlID="txtObservaciones" WatermarkText="Observaciones" WatermarkCssClass="input-large water" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ErrorMessage="Debe indicar una observacion o colocar una descripción del movimiento" Text="*" CssClass="errores" ControlToValidate="txtObservaciones" ValidationGroup="agrega"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-1 col-sm-1 text-left"><asp:Label ID="Label19" runat="server" Text="Detalle o Identificador de Solicitud:" CssClass="textoBold"></asp:Label></div>
                    <div class="col-lg-3 col-sm-3 text-left">
                        <asp:TextBox ID="txtDetalle" runat="server" MaxLength="250" CssClass="input-large" placeholder="Detalle"></asp:TextBox>                        
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="Debe indicar un detalle o identificador de solicitud para su posterior uso en punto de venta" Text="*" CssClass="errores" ControlToValidate="txtDetalle" ValidationGroup="agrega"></asp:RequiredFieldValidator>
                    </div>

                    <div class="col-lg-4 col-sm-4 text-center">
                        <asp:LinkButton ID="lnkAgregar" runat="server" CssClass="btn btn-info" ValidationGroup="agrega"
                            onclick="lnkAgregar_Click"><i class="fa fa-plus-circle"></i><span>&nbsp;Agregar Registro</span></asp:LinkButton>
                    </div>
                </div>                
            </asp:Panel>
            <asp:Panel ID="pnlDatosSeguimiento" runat="server" CssClass="col-lg-12 col-sm-12" Visible="false">
                <div class="row alert-info textoBold t18">
                    <div class="col-lg-11 col-sm-11 text-center">Orden:&nbsp;
                        <asp:Label ID="lblOrdenPintura" runat="server" CssClass="" ></asp:Label>
                        <asp:Label ID="lblEstatusOrden" runat="server" Visible="false" ></asp:Label>
                        <asp:Label ID="lblIdOrden" runat="server" Visible="false" ></asp:Label>
                        <asp:Label ID="lblanoOrdne" runat="server" Visible="false" ></asp:Label>
                        <asp:Label ID="lblFecharece" runat="server" Visible="false" ></asp:Label>
                        <asp:Label ID="lblAutorizado" runat="server" Visible="false" ></asp:Label>
                    </div>
                    <div class="col-lg-1 col-sm-1">
                        <asp:LinkButton ID="lnkSalir" runat="server" CssClass="btn btn-danger" 
                            onclick="lnkSalir_Click" ><i class="fa fa-remove"></i><span>&nbsp;Salir</span></asp:LinkButton>
                    </div>
                </div>
                
                <div class="row">
                    <div class="col-lg-1 col-sm-1 text-left"><asp:Label ID="Label14" runat="server" Text="Igualación:" CssClass="textoBold"></asp:Label></div>
                    <div class="col-lg-3 col-sm-3 text-left">
                        <asp:TextBox ID="txtFigualacion" runat="server" CssClass="alingMiddle input-small" Enabled="false"  ></asp:TextBox>
                        <cc1:CalendarExtender ID="CalendarExtender3" runat="server" BehaviorID="txtFigualacion_CalendarExtender" TargetControlID="txtFigualacion" Format="yyyy-MM-dd" PopupButtonID="lnkFigualacion" />
                        <asp:LinkButton ID="lnkFigualacion" runat="server" CssClass="btn btn-info t14"><i class="fa fa-calendar"></i></asp:LinkButton>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="Debe indicar la fecha de igualacion" Text="*" CssClass="errores" ValidationGroup="sigue" ControlToValidate="txtFigualacion"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-lg-1 col-sm-1 text-left"><asp:Label ID="Label15" runat="server" Text="Terminado:" CssClass="textoBold"></asp:Label></div>
                    <div class="col-lg-3 col-sm-3 text-left">
                        <asp:TextBox ID="txtFterminado" runat="server" CssClass="alingMiddle input-small" Enabled="false"></asp:TextBox>
                        <cc1:CalendarExtender ID="CalendarExtender4" runat="server" BehaviorID="txtFterminado_CalendarExtender" TargetControlID="txtFterminado" Format="yyyy-MM-dd" PopupButtonID="lnkFterminado" />
                        <asp:LinkButton ID="lnkFterminado" runat="server" CssClass="btn btn-info t14"><i class="fa fa-calendar"></i></asp:LinkButton>
                        <telerik:RadTimePicker RenderMode="Lightweight" ID="txtHterminado" Culture="es-Mx" DateInput-DateFormat="HH:mm:ss" DateInput-EmptyMessage="Hora" Skin="MetroTouch" Width="120px" runat="server"></telerik:RadTimePicker>                        
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="Debe indicar la fecha de terminado" Text="*" CssClass="errores" ValidationGroup="sigue" ControlToValidate="txtFterminado"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="Debe indicar la hora de terminado" Text="*" CssClass="errores" ValidationGroup="sigue" ControlToValidate="txtHterminado"></asp:RequiredFieldValidator>                        
                    </div>
                    <div class="col-lg-1 col-sm-1 text-left"><asp:Label ID="Label16" runat="server" Text="Entrega:" CssClass="textoBold"></asp:Label></div>
                    <div class="col-lg-3 col-sm-3 text-left">
                       <asp:TextBox ID="txtFentregado" runat="server" CssClass="alingMiddle input-small" Enabled="false"></asp:TextBox>
                        <cc1:CalendarExtender ID="CalendarExtender5" runat="server" BehaviorID="txtFentregado_CalendarExtender" TargetControlID="txtFentregado" Format="yyyy-MM-dd" PopupButtonID="lnkFentregado" />
                        <asp:LinkButton ID="lnkFentregado" runat="server" CssClass="btn btn-info t14"><i class="fa fa-calendar"></i></asp:LinkButton>
                        <telerik:RadTimePicker RenderMode="Lightweight" ID="txtHentregado" Culture="es-Mx" DateInput-DateFormat="HH:mm:ss" DateInput-EmptyMessage="Hora" Skin="MetroTouch" Width="120px" runat="server"></telerik:RadTimePicker>                       
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="Debe indicar la fecha de entrega" Text="*" CssClass="errores" ValidationGroup="sigue" ControlToValidate="txtFentregado"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="Debe indicar la hora de entrega" Text="*" CssClass="errores" ValidationGroup="sigue" ControlToValidate="txtHentregado"></asp:RequiredFieldValidator>                        
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-1 col-sm-1 text-left"><asp:Label ID="Label17" runat="server" Text="Recibió Pintura:" CssClass="textoBold"></asp:Label></div>
                    <div class="col-lg-3 col-sm-3 text-left">
                        <asp:TextBox ID="txtRecibePintura" runat="server" MaxLength="250" CssClass="input-large"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender5" runat="server" BehaviorID="txtRecibePintura_TextBoxWatermarkExtender" TargetControlID="txtRecibePintura" WatermarkText="Recibe Pintura" WatermarkCssClass="input-large water" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="Debe indicar el nombre de quien recibe la pintura" Text="*" CssClass="errores" ControlToValidate="txtRecibePintura" ValidationGroup="sigue"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-lg-1 col-sm-1 text-left"><asp:Label ID="Label18" runat="server" Text="Entrega:" CssClass="textoBold"></asp:Label></div>
                    <div class="col-lg-3 col-sm-3 text-left">
                        <asp:TextBox ID="txtEntregaReal" runat="server" MaxLength="250" CssClass="input-large"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender8" runat="server" BehaviorID="txtEntregaReal_TextBoxWatermarkExtender" TargetControlID="txtEntregaReal" WatermarkText="Entrega Pintura" WatermarkCssClass="input-large water" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ErrorMessage="Debe indicar el nombre de quien entrega la pintura" Text="*" CssClass="errores" ControlToValidate="txtEntregaReal" ValidationGroup="sigue"></asp:RequiredFieldValidator>
                    </div>
                     <div class="col-lg-1 col-sm-1 text-left"><asp:Label ID="Label20" runat="server" Text="Operario:" CssClass="textoBold"></asp:Label></div>
                    <div class="col-lg-3 col-sm-3 text-left">
                        <asp:DropDownList ID="ddlOperativoMod" runat="server" AutoPostBack="true" CssClass="input-medium" DataSourceID="SqlDataSource3" DataTextField="nombre" DataValueField="IdEmp" onselectedindexchanged="ddlOperativoMod_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:Label ID="Label21" runat="server" ></asp:Label>
                        <asp:Label ID="Label22" runat="server" Visible="false"></asp:Label>
                        <asp:SqlDataSource ID="SqlDataSource3" runat="server" 
                            ConnectionString="<%$ ConnectionStrings:PVW %>" 
                            SelectCommand="select 0 as IdEmp,'Pendiente' as nombre union all select O.IdEmp,UPPER(LTRIM(RTRIM(E.Nombres))+' '+LTRIM(RTRIM(ISNULL(E.Apellido_Paterno,'')))+' '+LTRIM(RTRIM(ISNULL(E.Apellido_Materno,'')))) AS nombre
                                            from Operativos_Orden O INNER JOIN Empleados E ON E.IdEmp=O.IdEmp where O.no_orden=@orden and O.id_empresa=@empresa and O.id_taller=@taller  and Tipo_Empleado&lt;&gt;'AD' order by 1">
                            <SelectParameters>
                                <asp:QueryStringParameter DefaultValue="0" Name="orden" QueryStringField="o" />
                                <asp:QueryStringParameter DefaultValue="0" Name="empresa" QueryStringField="e" />
                                <asp:QueryStringParameter DefaultValue="0" Name="taller" QueryStringField="t" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="Debe indicar el operario" ControlToValidate="ddlOperativoMod" Text="*" CssClass="errores" ValidationGroup="sigue"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-1 col-sm-1 text-left"><asp:Label ID="Label24" runat="server" Text="Ticket:" CssClass="textoBold"></asp:Label></div>
                    <div class="col-lg-3 col-sm-3 text-left">                        
                        <telerik:RadNumericTextBox ID="txtTicket" runat="server" MinValue="0" NumberFormat-DecimalDigits="0" EmptyMessage="Ticket" CssClass="input-medium"></telerik:RadNumericTextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ErrorMessage="Debe indicar el ticket" Text="*" CssClass="errores" ControlToValidate="txtTicket" ValidationGroup="sigue"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-12 col-sm-12 text-center">
                        <asp:LinkButton ID="lnkActualiza" runat="server" CssClass="btn btn-success" OnClientClick="return confirm('¿Está seguro de actualizar la información?, la orden pasará a estatus de terminado y no podra ser modificada')"
                            ValidationGroup="sigue" onclick="lnkActualiza_Click"><i class="fa fa-save"></i><span>&nbsp;Actualiza</span></asp:LinkButton>
                    </div>
                </div>                
            </asp:Panel>

            <asp:Panel runat="server" ID="Panel1" CssClass="paneles col-lg-12 col-sm-12 padding-top-10" ScrollBars="Auto">
                <asp:Panel ID="pnlRegistros" runat="server" CssClass="table-responsive col-lg-12 col-sm-12">
                    <asp:GridView ID="grdRegistro" runat="server" AllowPaging="True" 
                        CssClass="table table-bordered" PageSize="7"
                        AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="ano,id_solicitud" 
                        DataSourceID="SqlDataSource2" onrowdatabound="grdRegistro_RowDataBound">
                        <Columns>                            
                            <asp:BoundField DataField="ano" HeaderText="ano" ReadOnly="True" SortExpression="ano" Visible="false" />
                            <asp:BoundField DataField="id_solicitud" HeaderText="id_solicitud" ReadOnly="True" SortExpression="id_solicitud" Visible="false" />
                            <asp:BoundField DataField="folio_solicitud" HeaderText="Folio" SortExpression="folio_solicitud" />
                            <asp:TemplateField HeaderText="Solicitud" SortExpression="f_solicitud">
                                <EditItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("f_solicitud") %>'></asp:Label>&nbsp;
                                    <asp:Label ID="Label7" runat="server" Text='<%# Bind("h_solicitud") %>'></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("f_solicitud") %>'></asp:Label>&nbsp;
                                    <asp:Label ID="Label7" runat="server" Text='<%# Bind("h_solicitud") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>                            
                            <asp:TemplateField HeaderText="Recepción" SortExpression="f_recepcion">
                                <EditItemTemplate>
                                    <asp:Label ID="Label2" runat="server" Text='<%# Eval("f_recepcion") %>'></asp:Label>&nbsp;
                                    <asp:Label ID="Label9" runat="server" Text='<%# Bind("h_recepcion") %>'></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("f_recepcion") %>'></asp:Label>&nbsp;
                                    <asp:Label ID="Label9" runat="server" Text='<%# Bind("h_recepcion") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>                            
                            <asp:BoundField DataField="nombre" HeaderText="Operario" ReadOnly="True" SortExpression="nombre" />
                            <asp:BoundField DataField="recibe" HeaderText="Recibió" SortExpression="recibe" />
                            <asp:BoundField DataField="entrega_muestra" HeaderText="Entregó Muestra" SortExpression="entrega_muestra" />
                            <asp:BoundField DataField="dias_igualacion" HeaderText="Diás Igualación" SortExpression="dias_igualacion" />
                            <asp:BoundField DataField="total" HeaderText="Total" SortExpression="total" />
                            <asp:BoundField DataField="estatusDesc" HeaderText="Estatus" SortExpression="estatusDesc" />
                            <asp:BoundField DataField="desc_solicitud" HeaderText="Solicitud" SortExpression="desc_solicitud" />
                            <asp:BoundField DataField="detalle" HeaderText="Detalle de Solicitud" SortExpression="detalle" />
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkSeleccionar" runat="server" CausesValidation="False" CommandArgument='<%# Eval("ano") + ";" + Eval("id_solicitud") + ";" + Eval("folio_solicitud") + ";" + Eval("estatus") + ";" + Eval("f_recepcion") +";"+ Eval("ticket") %>'
                                        CommandName="Select" ToolTip="Seleccionar" CssClass="btn btn-success" 
                                        onclick="lnkSeleccionar_Click"><i class="fa fa-check"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkCancelar" runat="server" CausesValidation="False" CommandArgument='<%# Eval("ano")+";"+Eval("id_solicitud") %>'
                                        CommandName="Cancel" ToolTip="Cancelar" CssClass="btn btn-danger" OnClientClick="return confirm('¿Está seguro de cancelar la orden?')"
                                        onclick="lnkCancelar_Click"><i class="fa fa-remove"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <SelectedRowStyle CssClass="alert-success" />
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select r.ano,r.id_solicitud,r.folio_solicitud,ticket,Convert(char(10),r.fecha_solicitud,126) as f_solicitud,
convert(char(8),r.hora_solicitud,108) as h_solicitud,Convert(char(10),r.fecha_recepcion,126) as f_recepcion,
convert(char(8),r.hora_recepcion,108) as h_recepcion,r.recibe,r.entrega_muestra,r.total,e.idEmp,case when r.IdEmp=-1 then 'Programado' else UPPER(LTRIM(RTRIM(E.Nombres))+' '+LTRIM(RTRIM(ISNULL(E.Apellido_Paterno,'')))+' '+LTRIM(RTRIM(ISNULL(E.Apellido_Materno,'')))) end AS nombre,
case r.estatus when 'S' then 'Solicitado' when 'P' then 'En Proceso' when 'T' then 'Terminado' when 'C' then 'Cancelado' else '' end as estatusDesc, r.estatus, r.dias_igualacion,r.desc_solicitud,r.detalle
from Registro_Pinturas r
left join empleados e on e.idEmp=r.idEmp
where r.no_orden=@orden and r.id_empresa=@empresa and r.id_taller=@taller
order by r.ano desc, r.id_solicitud desc">
                        <SelectParameters>
                            <asp:QueryStringParameter DefaultValue="0" Name="orden" QueryStringField="o" />
                            <asp:QueryStringParameter DefaultValue="0" Name="empresa" 
                                QueryStringField="e" />
                            <asp:QueryStringParameter DefaultValue="0" Name="taller" QueryStringField="t" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </asp:Panel>
                <asp:Panel ID="pnlDetalle" runat="server" CssClass="table-responsive col-lg-12 col-sm-12" Visible="false">
                    <asp:GridView ID="grdDetalle" runat="server" AutoGenerateColumns="False" ShowFooter="true" EmptyDataText="No existe información adicional registrada" EmptyDataRowStyle-CssClass="errores"
                        CssClass="table table-bordered" AllowPaging="True" PageSize="7" AllowSorting="True"
                        DataSourceID="SqlDsEncDet" onrowdatabound="grdDetalle_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="ticket" HeaderText="ticket" SortExpression="ticket" Visible="false" />
                            <asp:BoundField DataField="cantidad" HeaderText="Cantidad" SortExpression="cantidad" />
                            <asp:BoundField DataField="descripcion" HeaderText="Concepto" SortExpression="descripcion" />
                            <asp:TemplateField HeaderText="Refacción" SortExpression="id_refaccion">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("id_refaccion") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("id_refaccion") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblAuto" runat="server"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Importe" SortExpression="importe">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("venta_unitaria") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("venta_unitaria", "{0:C2}") %>'></asp:Label>
                                </ItemTemplate>
                                 <FooterTemplate>
                                    <asp:Label ID="lblTotal" runat="server"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlDsEncDet" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:PVW %>" 
                        SelectCommand="select ticket,id_refaccion,cantidad,descripcion,venta_unitaria,importe 
                        FROM venta_det WHERE ticket=@ticket">
                        <SelectParameters>
                            <asp:Parameter DefaultValue="0" Name="ticket" Type="Int32"/>
                        </SelectParameters>
                    </asp:SqlDataSource>
                </asp:Panel>
            </asp:Panel>
            
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
