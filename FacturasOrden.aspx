<%@ Page Title="" Language="C#" MasterPageFile="~/AdmonOrden.master" AutoEventWireup="true"
    CodeFile="FacturasOrden.aspx.cs" Inherits="FacturasOrden" Culture="es-Mx" UICulture="es-Mx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function abreWinEmi() {
            var oWnd = $find("<%=modalCorreo.ClientID%>");
            oWnd.setUrl('');
            oWnd.show();
        }

        function cierraWinEmi() {
            var oWnd = $find("<%=modalCorreo.ClientID%>");
            oWnd.close();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" />

    <telerik:RadWindow RenderMode="Lightweight" ID="modalCorreo" Title="Envio de Correo" EnableShadow="true"
        Behaviors="Default" VisibleOnPageLoad="false" ShowContentDuringLoad="false" DestroyOnClose="true"
        Animation="Fade" runat="server" Modal="true" Width="790px" Height="590px" Style="z-index: 1000;">
        <ContentTemplate>
            <asp:UpdatePanel ID="upd1" runat="server">
                <ContentTemplate>
            <div class="col-lg-12 col-sm-12">
                <div class="col-lg-10 col-sm-10 text-right">
                    <asp:Label ID="lblDocumnetoPopup" runat="server" Visible="false" />
                </div>
                <div class="col-lg-1 col-sm-1 text-right">
                    <asp:LinkButton ID="lnkEnviaCorreoPop" runat="server" CssClass="btn btn-success" ToolTip="Enviar" OnClick="lnkEnviaCorreoPop_Click"><i class="fa fa-send-o"></i></asp:LinkButton>
                </div>
                <div class="col-lg-1 col-sm-1 text-right">
                    <asp:LinkButton ID="lnkClose" OnClientClick="cierraWinEmi()" OnClick="lnkClose_Click" ToolTip="Cancelar" runat="server" CssClass="btn btn-danger"><i class="fa fa-close"></i></asp:LinkButton>
                </div>

                <div class="col-lg-2 col-sm-2 text-right padding-top-10">
                    <asp:Label ID="Label3" runat="server" Text="Para..."></asp:Label>
                </div>
                <div class="col-lg-10 col-sm-10 text-left padding-top-10">
                    <asp:TextBox ID="txtPara" runat="server" CssClass="input-xxlarge" />
                </div>

                <div class="col-lg-2 col-sm-2 text-right">
                    <asp:Label ID="Label5" runat="server" Text="CC..."></asp:Label>
                </div>
                <div class="col-lg-10 col-sm-10 text-left">
                    <asp:TextBox ID="txtCC" runat="server" CssClass="input-xxlarge" />
                </div>

                <div class="col-lg-2 col-sm-2 text-right">
                    <asp:Label ID="Label7" runat="server" Text="CCO..."></asp:Label>
                </div>
                <div class="col-lg-10 col-sm-10 text-left">
                    <asp:TextBox ID="txtCCO" runat="server" CssClass="input-xxlarge" />
                </div>

                <div class="col-lg-2 col-sm-2 text-right">
                    <asp:Label ID="Label9" runat="server" Text="Asunto:"></asp:Label>
                </div>
                <div class="col-lg-10 col-sm-10 text-left">
                    <asp:TextBox ID="txtAsunto" runat="server" CssClass="input-xxlarge" />
                </div>
                
                <div class="col-lg-12 col-sm-12 text-center padding-top-10">
                    <asp:TextBox ID="txtContenido" runat="server" TextMode="MultiLine" Width="600px" Height="300px" CssClass="input-xxlarge maxH maxW"></asp:TextBox>
                </div>
            </div>
                    </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
    </telerik:RadWindow>

    <div class="row">
        <div class="col-lg-12 col-sm-12 text-center alert-info">
            <h3>
                <i class="fa fa-qrcode"></i>&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label1" runat="server" Text="Facturas Orden"></asp:Label>
            </h3>
        </div>
    </div>
    <asp:UpdatePanel runat="server" ID="updPanelGeneralesFact">
        <ContentTemplate>
            <div class="col-lg-12 col-ms-12">
                <asp:LinkButton ID="lnkCerrarOrden" runat="server" 
                    CssClass="btn btn-warning t14" 
                    OnClientClick="return confirm('¿Está seguro de terminar la orden?, Una vez realizado este cierre no se podran agregar mas datos a la orden.')" 
                    onclick="lnkCerrarOrden_Click"><i class="fa fa-folder-open"></i><span>&nbsp; Terminar Orden</span></asp:LinkButton>
            </div>
            <div class="row">
                <div class="col-lg-12 col-sm-12 text-center">
                    <asp:Label ID="lblError" runat="server" CssClass="errores" />
                </div>
            </div>
            <asp:Panel runat="server" ID="Panel1" CssClass="col-lg-12 col-ms-12 text-center pad1m">                
                <div class="col-lg-12 col-sm-12 text-center">
                    <asp:LinkButton ID="lnkNuevo" runat="server" CssClass="btn btn-primary t14" 
                        onclick="lnkNuevo_Click"><i class="fa fa-plus-circle"></i><span>&nbsp;Nuevo Documento</span></asp:LinkButton>
                </div>                                                
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlDaños" CssClass="col-lg-12 col-sm-12 paneles" >                
                <div class="table-responsive">
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                        EmptyDataRowStyle-CssClass="errores" EmptyDataText="No existen facturas para esta órden"
                        CssClass="table table-bordered" AllowPaging="True" AllowSorting="True" 
                        PageSize="7" DataKeyNames="idCfd" DataSourceID="SqlDataSource1" 
                        onrowdatabound="GridView1_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="idCfd" HeaderText="idCfd" InsertVisible="False" ReadOnly="True" SortExpression="idCfd" Visible="false" />
                            <asp:BoundField DataField="EncFolioUUID" HeaderText="UUID" SortExpression="EncFolioUUID" />
                            <asp:BoundField DataField="EncReferencia" HeaderText="Referencia Externa" SortExpression="EncReferencia" />
                            <asp:BoundField DataField="EncFechaGenera" HeaderText="Fecha" SortExpression="EncFechaGenera" />
                            <asp:BoundField DataField="EncReRFC" HeaderText="Emitida al R.F.C." SortExpression="EncReRFC" />
                            <asp:BoundField DataField="EncReNombre" HeaderText="Nombre del Receptor del Documento" SortExpression="EncReNombre" />                            
                            <asp:BoundField DataField="tipo" HeaderText="Tipo Factura" SortExpression="tipo" />
                            <asp:BoundField DataField="est" HeaderText="Estatus" ReadOnly="True" SortExpression="est" />
                            <asp:TemplateField ShowHeader="true" HeaderText="Seleccionar">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkSeleccionaDocumento" runat="server" 
                                        CausesValidation="False" CssClass="btn btn-info t14" CommandName="Select" 
                                        ToolTip="Seleccionar" CommandArgument='<%# Eval("idCfd") %>' 
                                        onclick="lnkSeleccionaDocumento_Click"><i class="fa fa-check-square"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="true" HeaderText="Cancelar">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkCancelar" runat="server" 
                                        CausesValidation="False" CssClass="btn btn-danger t14" CommandName="Cancel" 
                                        ToolTip="Cancelar" onclick="lnkCancelar_Click" CommandArgument='<%# Eval("idCfd") %>' ><i class="fa fa-ban"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="true"  HeaderText="Imprimir">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkImprimir" runat="server" onclick="lnkImprimir_Click"
                                        CausesValidation="False" CssClass="btn btn-primary t14" CommandName="Print" 
                                        ToolTip="Imprimir" CommandArgument='<%# Eval("idCfd") %>' ><i class="fa fa-print"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="true"  HeaderText="Enviar Correo">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEnviar" runat="server" onclick="lnkEnviarCorreo_Click" 
                                        CausesValidation="False" CssClass="btn btn-success t14" CommandName="SendEmail" 
                                        ToolTip="Enviar" CommandArgument='<%# Eval("idCfd")+";"+Eval("recorreo") %>' ><i class="fa fa-envelope"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="true"  HeaderText="Addenda">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkAddenda" runat="server" onclick="lnkAddenda_Click"
                                        CausesValidation="False" CssClass="btn btn-info t14" CommandName="Addenda" 
                                        ToolTip="Addenda" CommandArgument='<%# Eval("idCfd") %>' ><i class="fa fa-qrcode"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField ShowHeader="true" HeaderText="Refacturar">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkRefacturarDocumento" runat="server" 
                                        CausesValidation="False" CssClass="btn btn-grey t14" CommandName="Select" 
                                        ToolTip="Refacturar" CommandArgument='<%# Eval("idCfd") %>' 
                                        onclick="lnkRefacturarDocumento_Click"><i class="fa fa-reply"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                        <EmptyDataRowStyle CssClass="errores" />
                        <SelectedRowStyle CssClass="alert-info" />
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:PVW %>" 
                        SelectCommand="select e.idCfd,e.EncFolioUUID,e.EncReferencia,convert(char(10),e.EncFechaGenera,120)+' '+convert(char(8),e.enchoragenera,120) as EncFechaGenera,e.EncReRFC,e.EncReNombre,e.EncEstatus, 
case e.encestatus when 'P' then 'En Captura' when 'E' then 'En Tránsito' when 'T' then 'Timbrado' when 'R' then 'Rechazado' when 'C' then 'Cancelado'	 else 'Otro' end as est,r.recorreo,
case e.tipo when 'GL' then 'GLOBAL' when 'MO' then 'MANO OBRA' when 'RE' then 'REFACCIONES' when 'SO' then 'SIN ORDEN' when 'PV' then 'PUNTO DE VENTA' when 'PA' then 'PARTICULAR' when 'NC' then 'NOTA DE CREDITO' else '' end as tipo
from EncCFD e 
inner join receptores r on r.idrecep=e.idrecep where e.encfolioimpresion=@orden and SUBSTRING(e.encserieimpresion,5,charindex('-',e.encserieimpresion)-2)=@taller order by e.idcfd desc">
                        <SelectParameters>
                            <asp:QueryStringParameter DefaultValue="0" Name="orden" QueryStringField="o" />
                            <asp:QueryStringParameter DefaultValue="0" Name="taller" QueryStringField="t" />
                        </SelectParameters>
                    </asp:SqlDataSource>
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


            <asp:UpdateProgress ID="UpdateProgressgral" runat="server" AssociatedUpdatePanelID="updPanelGeneralesFact">
                <ProgressTemplate>
                    <asp:Panel ID="pnlMaskLoadgral" runat="server" CssClass="maskLoad" />
                    <asp:Panel ID="pnlCargandogral" runat="server" CssClass="pnlPopUpLoad">
                        <asp:Image ID="imgLoadgral" runat="server" ImageUrl="~/img/loading.gif" CssClass="ancho100" />
                    </asp:Panel>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
