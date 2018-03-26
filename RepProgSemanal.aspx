<%@ Page Title="" Language="C#" MasterPageFile="~/AdmOrdenes.master" AutoEventWireup="true"
    CodeFile="RepProgSemanal.aspx.cs" Inherits="RepProgSemanal" Culture="es-Mx" UICulture="es-Mx" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%-- revisar que jale las referencias --%>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="css/metro/metro.css" rel="stylesheet">
    <script src="js/metro/metro.js"></script>
    
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true">
    </asp:ScriptManager>
    <div class="page-header">
        <!-- /BREADCRUMBS -->
        <div class="clearfix">
            <h3 class="content-title pull-left">
                Programaci&oacute;n Semanal</h3>
        </div>
    </div>
    <asp:UpdatePanel runat="server" ID="updLocaliza">
        <ContentTemplate>
            <div class="col-lg-12 col-sm-12">
                <div class="col-lg-5 col-sm-5 text-left">
                    <span class="textoBold alingMiddle">Inicio:</span>
                    <asp:TextBox ID="txtFechaIni" runat="server" CssClass="alingMiddle input-small" Enabled="false"></asp:TextBox>
                    <cc1:CalendarExtender ID="txttxtFechaIni_CalendarExtender" runat="server" BehaviorID="txtFechaIni_CalendarExtender"
                        TargetControlID="txtFechaIni" Format="yyyy-MM-dd" PopupButtonID="lnktxtFechaIni" />
                    <asp:LinkButton ID="lnktxtFechaIni" runat="server" CssClass="btn btn-info t14"><i class="fa fa-calendar"></i></asp:LinkButton>
                </div>
                <div class="col-lg-5 col-sm-5 text-left">
                    <span class="textoBold alingMiddle">Fin:</span>
                    <asp:TextBox ID="txtFechaFin" runat="server" CssClass="alingMiddle input-small" Enabled="false"></asp:TextBox>
                    <cc1:CalendarExtender ID="txttxtFechaFin_CalendarExtender" runat="server" BehaviorID="txtFechaFin_CalendarExtender"
                        TargetControlID="txtFechaFin" Format="yyyy-MM-dd" PopupButtonID="lnktxtFechaFin" />
                    <asp:LinkButton ID="lnktxtFechaFin" runat="server" CssClass="btn btn-info t14"><i class="fa fa-calendar"></i></asp:LinkButton>
                </div>
                <div class="col-lg-1 col-sm-1 text-center">
                    <asp:LinkButton ID="lnkBuscar" runat="server" CssClass="btn btn-info" ToolTip="Generar"
                        OnClick="lnkBuscar_Click"><i class="fa fa-cog"></i><span>&nbsp;Generar</span></asp:LinkButton>
                </div>
                <div class="col-lg-1 col-sm-1 text-center">
                    <asp:LinkButton ID="lnkRegresarOrdenes" runat="server" OnClick="lnkRegresarOrdenes_Click"
                        CssClass="btn btn-info t14"><i class="fa fa-reply">&nbsp;&nbsp;</i><i class="fa fa-th-list"></i>&nbsp;<span>&Oacute;rdenes</span></asp:LinkButton>
                </div>
            </div>
            <div class="col-lg-12 col-sm-12 text-center">
                <asp:Label ID="lblError" runat="server" CssClass="errores" />
            </div>
            
                <div class="col-lg-6 col-sm-6 text-center">
                    <asp:Chart ID="Chart1" runat="server" Height="500px" Width="500px" BackColor="#f5f6f6">
                        <Titles>
                            <asp:Title ShadowOffset="3" Name="Aseguradoras" Visible="false" />
                        </Titles>
                        <Legends>
                            <asp:Legend Alignment="Center" BackColor="#f5f6f6" Docking="Bottom" IsTextAutoFit="False"
                                Name="Default" LegendStyle="Column">
                            </asp:Legend>
                        </Legends>
                        <Series>
                            <asp:Series Name="Default" Color="#f5f6f6" />
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="ChartArea1" BackColor="#f5f6f6" BorderWidth="0" Visible="true" />
                        </ChartAreas>
                    </asp:Chart>
                </div>
                <div class="col-lg-6 col-sm-6 text-center">
                    <div class="tile-container">
                        <asp:DataList ID="DataList1" runat="server" RepeatColumns="3" OnItemDataBound="DataList1_ItemDataBound">
                            <ItemTemplate>
                                <div class="tile-square" data-role="tile" data-effect="slideUpDown" runat="server"
                                    style="background-color: transparent;" id="cuadro">
                                    <div class="tile-content">
                                        <div class="live-slide">
                                            <center>
                                                <asp:LinkButton ID="lnkSeleccion" CssClass="link" runat="server" CommandArgument='<%# Eval("id_cliprov")+";1" %>' OnClick="lnkProceso">
                                                    <asp:Image ID="Image1" runat="server" CssClass="pad1m" ImageUrl='<%# "~/ImgCliente.ashx?id="+Eval("id_cliprov") %>' ToolTip='<%# Eval("cliente") %>' AlternateText='<%# Eval("cliente") %>' />
                                                </asp:LinkButton>
                                            </center>
                                        </div>
                                        <div class="live-slide">
                                            <div class="pad1m">
                                                <center>
                                                    <asp:LinkButton ID="LinkButton11" CssClass="link" runat="server" CommandArgument='<%# Eval("id_cliprov")+";0" %>'
                                                        OnClick="lnkProceso">
                                                        <asp:Label ID="lblEtiqueta" runat="server" Text='<%# Eval("cliente") %>' CssClass="textoBold colorBlanco pad1m"></asp:Label><br />
                                                        <asp:Label ID="lblIndicadores" runat="server" Text='<%# Eval("total") %>' CssClass="textoBold colorBlanco pad1m"
                                                            Font-Size="22pt"></asp:Label>
                                                    </asp:LinkButton>
                                                </center>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:DataList>
                        <div class="tile-square" data-role="tile" data-effect="slideLeft" style="background-color:#b94a48;" >
                            <div class="tile-content">
                                <div class="live-slide">
                                    <center>
                                        <asp:LinkButton ID="LinkButton2" CssClass="link" runat="server" CommandArgument="1;2"
                                            OnClick="lnkProceso">
                                                    <img alt="VENCIDOS" src="img/bitacoras/vencidos.png" class="pad1m">
                                        </asp:LinkButton>
                                    </center>
                                </div>
                                <div class="live-slide">
                                    <div class="pad1m">
                                        <center>
                                            <asp:LinkButton ID="LinkButton3" CssClass="link" runat="server" CommandArgument="1;2"
                                                OnClick="lnkProceso">
                                                <asp:Label ID="lblEtiqueta" runat="server" Text="VENCIDOS" CssClass="colorBlanco textoBold pad1m"></asp:Label><br />
                                                <asp:Label ID="lblVencidos" runat="server" Text="0" CssClass="colorBlanco textoBold" Font-Size="22pt" />
                                            </asp:LinkButton>
                                        </center>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="tile-square" data-role="tile" data-effect="slideRight" style="background-color:#c09853;">
                            <div class="tile-content">
                                <div class="live-slide">
                                    <center>
                                        <asp:LinkButton ID="LinkButton4" CssClass="link" runat="server" CommandArgument="2;2"
                                            OnClick="lnkProceso">
                                                    <img alt="POR VENCER" src="img/bitacoras/provencer.png" class="pad1m">
                                        </asp:LinkButton>
                                    </center>
                                </div>
                                <div class="live-slide">
                                    <div class="pad1m">
                                        <center>
                                            <asp:LinkButton ID="LinkButton5" CssClass="link" runat="server" CommandArgument="2;2"
                                                OnClick="lnkProceso">
                                                <asp:Label ID="Label1" runat="server" Text="PROXIMOS A VENCER" CssClass="colorBlanco textoBold pad1m"></asp:Label><br />
                                                <asp:Label ID="lblProxVencer" runat="server" Text="0" CssClass="colorBlanco textoBold" Font-Size="22pt" />
                                            </asp:LinkButton>
                                        </center>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="tile-square" data-role="tile" data-effect="slideLeftRight" style="background-color:#3FD43B;">
                            <div class="tile-content">
                                <div class="live-slide">
                                    <center>
                                        <asp:LinkButton ID="LinkButton6" CssClass="link" runat="server" CommandArgument="3;2"
                                            OnClick="lnkProceso">
                                                    <img alt="TERMINADOS" src="img/bitacoras/terminados.png" class="pad1m">
                                        </asp:LinkButton>
                                    </center>
                                </div>
                                <div class="live-slide">
                                    <div class="pad1m">
                                        <center>
                                            <asp:LinkButton ID="LinkButton7" CssClass="link" runat="server" CommandArgument="3;2"
                                                OnClick="lnkProceso">
                                                <asp:Label ID="Label2" runat="server" Text="TERMINADOS" CssClass="colorBlanco textoBold pad1m"></asp:Label><br />
                                                <asp:Label ID="lblTerminados" runat="server" Text="0" CssClass="colorBlanco textoBold" Font-Size="22pt" />
                                            </asp:LinkButton>
                                        </center>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="tile-square" data-role="tile" data-effect="slideUp" style="background-color:#4986E8;">
                            <div class="tile-content">
                                <div class="live-slide">
                                    <center>
                                        <asp:LinkButton ID="LinkButton8" CssClass="link" runat="server" CommandArgument="4;2"
                                            OnClick="lnkProceso">
                                                    <img alt="ENTREGADOS" src="img/bitacoras/entregados.png" class="pad1m">
                                        </asp:LinkButton>
                                    </center>
                                </div>
                                <div class="live-slide">
                                    <div class="pad1m">
                                        <center>
                                            <asp:LinkButton ID="LinkButton9" CssClass="link" runat="server" CommandArgument="4;2"
                                                OnClick="lnkProceso">
                                                <asp:Label ID="Label4" runat="server" Text="ENTREGADOS" CssClass="colorBlanco textoBold pad1m"></asp:Label><br />
                                                <asp:Label ID="lblEntregados" runat="server" Text="0" CssClass="colorBlanco textoBold" Font-Size="22pt" />
                                            </asp:LinkButton>
                                        </center>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="tile-square" data-role="tile" data-effect="slideDown" style="background-color:#EE9123;">
                            <div class="tile-content">
                                <div class="live-slide">
                                    <center>
                                        <asp:LinkButton ID="LinkButton10" CssClass="link" runat="server" CommandArgument="5;2"
                                            OnClick="lnkProceso">
                                                    <img alt="PROCESO" src="img/bitacoras/proceso.png" class="pad1m">
                                        </asp:LinkButton>
                                    </center>
                                </div>
                                <div class="live-slide">
                                    <div class="pad1m">
                                        <center>
                                            <asp:LinkButton ID="LinkButton11" CssClass="link" runat="server" CommandArgument="5;2"
                                                OnClick="lnkProceso">
                                                <asp:Label ID="Label5" runat="server" Text="PROCESO" CssClass="colorBlanco textoBold pad1m"></asp:Label><br />
                                                <asp:Label ID="lblProceso" runat="server" Text="0" CssClass="colorBlanco textoBold" Font-Size="22pt" />
                                            </asp:LinkButton>
                                        </center>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="tile-square" data-role="tile" data-effect="slideUpDown" style="background-color:#5E5E5E;">
                            <div class="tile-content">
                                <div class="live-slide">
                                    <center>
                                        <asp:LinkButton ID="LinkButton12" CssClass="link" runat="server" OnClick="lnkBuscar_Click">
                                                    <img alt="TOTAL" src="img/bitacoras/totales.png" class="pad1m">
                                        </asp:LinkButton>
                                    </center>
                                </div>
                                <div class="live-slide">
                                    <div class="pad1m">
                                        <center>
                                            <asp:LinkButton ID="LinkButton13" CssClass="link" runat="server" OnClick="lnkBuscar_Click">
                                                <asp:Label ID="Label6" runat="server" Text="TOTAL" CssClass="colorBlanco textoBold pad1m"></asp:Label><br />
                                                <asp:Label ID="lblTotales" runat="server" Text="0" CssClass="colorBlanco textoBold" Font-Size="22pt" />
                                            </asp:LinkButton>
                                        </center>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>                    
                </div>
                <%-- <div class="col-lg-4 col-sm-4 text-center">
                    <asp:Chart ID="Chart2" runat="server" Height="300px" Width="300px" BackColor="#f5f6f6"
                        Palette="Bright" BorderlineColor="WhiteSmoke">
                        <Titles>
                            <asp:Title ShadowOffset="3" Name="Title1" />
                        </Titles>
                        <Legends>
                            <asp:Legend Alignment="Center" BackColor="#f5f6f6" Docking="Bottom" IsTextAutoFit="False"
                                Name="Clientes" LegendStyle="Column">
                            </asp:Legend>
                        </Legends>
                        <Series>
                            <asp:Series Name="Default" Color="#f5f6f6" />
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="ChartArea1" BackColor="#f5f6f6" Visible="true" />
                        </ChartAreas>
                    </asp:Chart>
                </div>--%>
            <div class="col-lg-12 col-sm-12 text-center">
                    <asp:LinkButton ID="lnkImprimir" Visible="true" ToolTip="Imprimir Programación Semanal" OnClick="lnkImprimir_Click" runat="server" CssClass="btn btn-info"><i class="fa fa-print"></i><span>&nbsp;Imprimir</span></asp:LinkButton>
                </div>
            <asp:Panel ID="Panel4" runat="server" CssClass="panelCatalogos text-center" ScrollBars="Auto">
                <div class="table-responsive">
                    <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" runat="server" ShowStatusBar="true"
                        AutoGenerateColumns="False" AllowSorting="True" AllowMultiRowSelection="False"
                        OnDetailTableDataBind="RadGrid1_DetailTableDataBind" OnNeedDataSource="RadGrid1_NeedDataSource"
                        OnPreRender="RadGrid1_PreRender" Skin="MetroTouch">
                        <PagerStyle Mode="NumericPages"></PagerStyle>
                        <MasterTableView DataKeyNames="fecha" AllowMultiColumnSorting="True">
                            <DetailTables>
                                <telerik:GridTableView DataKeyNames="no_orden" Name="ingreso" Width="100%">
                                    <Columns>
                                        <telerik:GridTemplateColumn SortExpression="no_orden" HeaderText="Orden">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnOrden" runat="server" Text='<%# Eval("no_orden") %>' CommandArgument='<%# Eval("fase_orden") %>'
                                                    OnClick="btnOrden_Click" CssClass="btn btn-info textoBold"></asp:LinkButton>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn SortExpression="tipo_auto" HeaderText="Vehículo" HeaderButtonType="TextButton"
                                            DataField="tipo_auto">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="placas" HeaderText="Placas" SortExpression="placas"
                                            HeaderButtonType="TextButton" />
                                        <telerik:GridBoundColumn DataField="color" HeaderText="Color" SortExpression="color"
                                            HeaderButtonType="TextButton" />
                                        <telerik:GridBoundColumn DataField="fecha" HeaderText="Fecha" SortExpression="fecha"
                                            DataFormatString="{0:d}" />
                                        <telerik:GridBoundColumn DataField="cliente" HeaderText="Cliente" SortExpression="cliente"
                                            HeaderButtonType="TextButton" />
                                        <telerik:GridBoundColumn DataField="localizacion" HeaderText="Localización" SortExpression="localizacion"
                                            HeaderButtonType="TextButton" />
                                    </Columns>
                                    <NoRecordsTemplate>
                                        <asp:Label ID="grdVacio" runat="server" Text="No existe información" CssClass="errores"></asp:Label>
                                    </NoRecordsTemplate>
                                </telerik:GridTableView>
                            </DetailTables>
                            <Columns>
                                <telerik:GridTemplateColumn SortExpression="fecha" HeaderText="Fecha">
                                    <ItemTemplate>
                                        <asp:Label ID="Label3" runat="server" Text='<%# Eval("dw")+" "+Eval("fecha") %>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <ItemStyle CssClass="text-bold" />
                            <AlternatingItemStyle CssClass="text-bold" />
                            <NoRecordsTemplate>
                                <asp:Label ID="lnlemptygrid" runat="server" CssClass="errores" Text="No existe información"></asp:Label>
                            </NoRecordsTemplate>
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>
            </asp:Panel>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="updLocaliza">
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
