<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BienvenidaOrdenes.aspx.cs"
    Inherits="BienvenidaOrdenes" MasterPageFile="~/AdmonOrden.master" %>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <asp:Panel runat="server" ID="pnlRecepcion" CssClass="paneles ancho100" ScrollBars="Auto">

        <div class="row ">
            <div class="col-lg-6 col-sm-6 text-center">                
                <asp:Image ID="imgFase" runat="server" CssClass="img-responsive" Visible="false" />
                <telerik:RadRadialGauge runat="server" ID="RadRadialGauge1" CssClass="ancho100 alto100">
                    <Pointer Value="2.5">
                        <Cap Size="0.1" />
                    </Pointer>
                    <Scale Min="1" Max="9" MajorUnit="1" MinorUnit="1">
                        <Labels Format="Fase {0}" Position="Inside" Visible="false" />
                        <Ranges>
                            <telerik:GaugeRange Color="#ff0000" From="1" To="2" />
                            <telerik:GaugeRange Color="#ff4000" From="2" To="3" />
                            <telerik:GaugeRange Color="#ff8000" From="3" To="4" />
                            <telerik:GaugeRange Color="#ffbf00" From="4" To="5" />
                            <telerik:GaugeRange Color="#bfff00" From="5" To="6" />
                            <telerik:GaugeRange Color="#80ff00" From="6" To="7" />
                            <telerik:GaugeRange Color="#40ff00" From="7" To="8" />
                            <telerik:GaugeRange Color="#01df01" From="8" To="9" />
                        </Ranges>
                    </Scale>                    
                </telerik:RadRadialGauge>
            </div>
            <div class="col-lg-6 col-sm-6 text-center centrado">
                <div class="col-lg-6 col-sm-6">
                    <asp:Label ID="e1" runat="server" Text="Recepción" CssClass="pad1m  colorBlanco t10" style="background:#ff0000; display:block"></asp:Label>
                </div>
                <div class="col-lg-6 col-sm-6">
                    <asp:Label ID="e2" runat="server" Text="Presupuesto" CssClass="pad1m  colorBlanco t10" style="background:#ff4000; display:block"></asp:Label>
                </div>        
                <div class="col-lg-6 col-sm-6">
                    <asp:Label ID="e3" runat="server" Text="Asignación" CssClass="pad1m  colorBlanco t10" style="background:#ff8000; display:block"></asp:Label>
                </div>
                <div class="col-lg-6 col-sm-6">
                    <asp:Label ID="e4" runat="server" Text="Refacciones" CssClass="pad1m  t10" style="background:#ffbf00; display:block"></asp:Label>
                </div>
                <div class="col-lg-6 col-sm-6">
                    <asp:Label ID="e5" runat="server" Text="Valuación" CssClass="pad1m  t10" style="background:#bfff00; display:block"></asp:Label>
                </div>
                <div class="col-lg-6 col-sm-6">
                    <asp:Label ID="e6" runat="server" Text="Operación" CssClass="pad1m  t10" style="background:#80ff00; display:block"></asp:Label>
                </div>
                <div class="col-lg-6 col-sm-6">
                    <asp:Label ID="e7" runat="server" Text="Entrega" CssClass="pad1m  t10" style="background:#40ff00; display:block"></asp:Label>
                </div>
                <div class="col-lg-6 col-sm-6">
                    <asp:Label ID="e8" runat="server" Text="Facturación" CssClass="pad1m  t10" style="background:#01df01; display:block"></asp:Label>
                </div>
            </div>
        </div>
        <div class="row marTop">            
            <div class="col-lg-6 col-sm-6 text-center text-success pad1m t18 textoBold">
                <asp:Label ID="lblPerfil" runat="server" Text="Perfil"></asp:Label>
            </div>
            <div class="col-lg-6 col-sm-6 text-center text-success pad1m t18 textoBold">
                <asp:Label ID="lblAvanceOrden" runat="server" Text="100%"></asp:Label>
            </div>
        </div>
        <div class="row ">                   
            <div class="col-lg-2 col-sm-12 text-center">
                <div class="col-lg-12 col-sm-6 text-center centrado pad1m">
                    <asp:LinkButton ID="lnkMano" runat="server" CssClass="text-primary pad1m t18 textoBold"
                        OnClick="lnkMano_Click"><i class="fa fa-user"></i>&nbsp;<span class="t14">Mano Obra</span></asp:LinkButton></div>
                <div class="col-lg-12 col-sm-6 text-center centrado pad1m ">
                    <asp:LinkButton ID="lnkRef" runat="server" CssClass="text-primary pad1m t18 textoBold"
                        OnClick="lnkRef_Click"><i class="fa fa-cog"></i>&nbsp;<span class="t14">Refacciones</span></asp:LinkButton></div>
                <div class="col-lg-12 col-sm-6 text-center centrado pad1m">
                    <asp:LinkButton ID="lnkComen" runat="server" CssClass="text-primary pad1m t18 textoBold"
                        OnClick="lnkComen_Click"><i class="fa fa-weixin"></i>&nbsp;<span class="t14">Comentarios</span></asp:LinkButton></div>
                <div class="col-lg-12 col-sm-6 text-center centrado pad1m">
                    <asp:LinkButton ID="lnkLlamada" runat="server" CssClass="text-primary pad1m t18 textoBold"
                        OnClick="lnkLlamada_Click"><i class="fa fa-phone"></i>&nbsp;<span class="t14">Llamadas</span></asp:LinkButton></div>
                <div class="col-lg-12 col-sm-6 text-center centrado pad1m">
                    <asp:LinkButton ID="lnkOperacion" runat="server" CssClass="text-primary pad1m t18 textoBold"
                        OnClick="lnkOperacion_Click"><i class="fa fa-wrench"></i>&nbsp;<span class="t14">Operaci&oacute;n</span></asp:LinkButton></div>
            </div>
            <div class="col-lg-10 col-sm-12"> 
                <div class="col-lg-12 col-sm-12 text-center centrado alert-info t18 textoBold">
                    <asp:Label ID="lblTítulo" runat="server" Visible="false"></asp:Label>
                </div>                
                <div class="table-responsive col-lg-12 col-sm-12 text-center">
                    <asp:Panel ID="pnlMano" runat="server" CssClass="panelesInv col-lg-12 col-sm-12" ScrollBars="Auto" Visible="false">
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered"
                            ShowHeader="false" DataSourceID="SqlDataSource1" EmptyDataText="No existe Mano de Obra registrada"
                            EmptyDataRowStyle-CssClass="errores">
                            <Columns>
                                <asp:BoundField DataField="mano" HeaderText="mano" ReadOnly="True" SortExpression="mano" />
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>"
                            SelectCommand="select upper(g.descripcion_go+' '+o.descripcion_op+' '+mo.id_refaccion) as mano from mano_obra mo inner join grupo_operacion g on g.id_grupo_op=mo.id_grupo_op inner join operaciones o on o.id_operacion=mo.id_operacion where mo.id_empresa=@id_empresa and mo.id_taller=@id_taller and mo.no_orden=@orden order by mo.id_grupo_op asc">
                            <SelectParameters>
                                <asp:QueryStringParameter DefaultValue="0" Name="id_empresa" QueryStringField="e" />
                                <asp:QueryStringParameter DefaultValue="0" Name="id_taller" QueryStringField="t" />
                                <asp:QueryStringParameter DefaultValue="0" Name="orden" QueryStringField="o" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </asp:Panel>
                    <asp:Panel ID="pnlRefacciones" runat="server" CssClass="panelesInv col-lg-12 col-sm-12"
                        ScrollBars="Auto" Visible="false">
                        <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered"
                            EmptyDataText="No existen Refacciones registradas" EmptyDataRowStyle-CssClass="errores"
                            DataSourceID="SqlDataSource2">
                            <Columns>
                                <asp:BoundField DataField="refCantidad" HeaderText="Cantidad" SortExpression="refCantidad" />
                                <asp:BoundField DataField="refDescripcion" HeaderText="Refacción" SortExpression="refDescripcion" />
                                <asp:BoundField DataField="descripcion" HeaderText="Estatus Refacción" ReadOnly="True"
                                    SortExpression="descripcion" />
                                <asp:BoundField DataField="estatus" HeaderText="Estatus Presupuesto" ReadOnly="True"
                                    SortExpression="estatus" />
                                <asp:BoundField DataField="festima" HeaderText="Entrega Estimada" ReadOnly="True"
                                    SortExpression="festima" />
                                <asp:BoundField DataField="fereal" HeaderText="Entrega Real" ReadOnly="True" SortExpression="fereal" />
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>"
                            SelectCommand="select refCantidad,refDescripcion,isnull(re.staDescripcion ,'-------------') as descripcion,
                                case ro.refestatus when 'NA' then 'No Aplica' when 'CO' then 'Compra' when 'RP' then 'Reparación' when 'EV' then 'Evaluación' when 'AP' then 'Aplicado' when 'AU' then 'Autorizado' when 'FA' then 'Facturado' when 'CA' then 'Cancelada' else 'No Aplica' end as estatus,
                                isnull((convert(varchar, ro.refFechEntregaReal,126)),'')as fereal,isnull((convert(varchar, ro.refFechEntregaEst,126)),'')as festima from refacciones_orden ro left join rafacciones_estatus re on re.staRefId=ro.refEstatusSolicitud where ro.ref_id_empresa=@id_empresa and ro.ref_id_taller=@id_taller and ro.ref_no_orden=@orden">
                            <SelectParameters>
                                <asp:QueryStringParameter DefaultValue="0" Name="id_empresa" QueryStringField="e" />
                                <asp:QueryStringParameter DefaultValue="0" Name="id_taller" QueryStringField="t" />
                                <asp:QueryStringParameter DefaultValue="0" Name="orden" QueryStringField="o" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </asp:Panel>
                    <asp:Panel ID="pnlComentarios" runat="server" CssClass="panelesInv col-lg-12 col-sm-12"
                        ScrollBars="Auto" Visible="false">
                        <asp:GridView ID="GridView4" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered"
                            EmptyDataText="No existen comentarios registrados" EmptyDataRowStyle-CssClass="errores"
                            DataSourceID="SqlDataSource4">
                            <Columns>
                                <asp:BoundField DataField="observacion" HeaderText="Comentario" SortExpression="observacion" />
                                <asp:BoundField DataField="fecha" HeaderText="Fecha" SortExpression="fecha" />
                                <asp:BoundField DataField="hora" HeaderText="Hora" ReadOnly="True" SortExpression="hora" />
                                <asp:BoundField DataField="nick" HeaderText="Usuario" ReadOnly="True" SortExpression="nick" />
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>"
                            SelectCommand="select upper(b.observacion) as observacion, convert(char(10),b.fecha,126) as fecha, convert(char(8), b.hora,108) as hora,
                                            u.nick
                                            from bitacora_orden_comentarios b
                                            left join usuarios u on u.id_usuario=b.id_usuario
                                            where b.id_empresa=@id_empresa and b.id_taller=@id_taller and b.no_orden=@orden order by b.id_observacion desc">
                            <SelectParameters>
                                <asp:QueryStringParameter DefaultValue="0" Name="id_empresa" QueryStringField="e" />
                                <asp:QueryStringParameter DefaultValue="0" Name="id_taller" QueryStringField="t" />
                                <asp:QueryStringParameter DefaultValue="0" Name="orden" QueryStringField="o" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </asp:Panel>
                    <asp:Panel ID="pnlLlamadas" runat="server" CssClass="panelesInv col-lg-12 col-sm-12"
                        ScrollBars="Auto" Visible="false">
                        <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered"
                            DataSourceID="SqlDataSource3" EmptyDataText="No existen llamadas realizadas"
                            EmptyDataRowStyle-CssClass="errores">
                            <Columns>
                                <asp:BoundField DataField="fecha" HeaderText="Fecha" ReadOnly="True" SortExpression="fecha" />
                                <asp:BoundField DataField="hora" HeaderText="Hora" ReadOnly="True" SortExpression="hora" />
                                <asp:BoundField DataField="usuario_llamada" HeaderText="Cliente" ReadOnly="True"
                                    SortExpression="usuario_llamada" />
                                <asp:BoundField DataField="tipo" HeaderText="Tipo" ReadOnly="True" SortExpression="tipo" />
                                <asp:BoundField DataField="estatus" HeaderText="Estatus" ReadOnly="True" SortExpression="estatus" />
                                <asp:BoundField DataField="observaciones" HeaderText="Observaciones" ReadOnly="True"
                                    SortExpression="observaciones" />
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>"
                            SelectCommand="select isnull(Convert(char(10),lo.fecha_llamada,126),'') as fecha,isnull(Convert(char(8),lo.hora,108),'') as hora,lo.usuario_llamada,
                                            case lo.tipo_llamada when 'S' then 'Salida' else 'Entrada' end as tipo, case lo.estatus when 'P' then 'Pendiente' else 'Realizada' end as estatus,
                                            lo.observaciones
                                            from llamadas_orden lo where lo.id_empresa=@id_empresa and lo.id_taller=@id_taller and lo.no_orden=@orden 
                                            order by lo.consecutivo desc">
                            <SelectParameters>
                                <asp:QueryStringParameter DefaultValue="0" Name="id_empresa" QueryStringField="e" />
                                <asp:QueryStringParameter DefaultValue="0" Name="id_taller" QueryStringField="t" />
                                <asp:QueryStringParameter DefaultValue="0" Name="orden" QueryStringField="o" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </asp:Panel>
                    <asp:Panel ID="pnlOperacion" runat="server" CssClass="panelesInv col-lg-12 col-sm-12" ScrollBars="Auto" Visible="false">
                        <asp:GridView ID="GridView5" runat="server" AutoGenerateColumns="False" ShowHeader="false"
                            CssClass="table table-bordered" DataSourceID="SqlDataSource5" EmptyDataText="No existen Operaciones a realizar"
                            EmptyDataRowStyle-CssClass="errores">
                            <Columns>
                                <asp:BoundField DataField="Grupo" HeaderText="Grupo" ReadOnly="True" SortExpression="Grupo" />
                                <asp:BoundField DataField="avance" HeaderText="Avance" ReadOnly="True" SortExpression="avance" />
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>"
                            SelectCommand="select distinct mo.id_grupo_op, upper(g.descripcion_go) as Grupo,
                                            case cast((select isnull((select (case p25 when 1 then 1 else 0 end+case p50 when 1 then 1 else 0 end+case p75 when 1 then 1 else 0 end+case p100 when 1 then 1 else 0 end) from Seguimiento_Operacion where no_orden=mo.no_orden and id_empresa=mo.id_empresa and id_taller=mo.id_taller and id_grupo_op=mo.id_grupo_op),0)) as char(1))
                                            when '4' then '100%' when '3' then '75%'  when '2' then '50%' when '1' then '25%' else '' end as avance
                                            from mano_obra mo
                                            inner join grupo_operacion g on g.id_grupo_op=mo.id_grupo_op
                                            where mo.id_empresa=@id_empresa and mo.id_taller=@id_taller and mo.no_orden=@orden
                                            order by mo.id_grupo_op asc">
                            <SelectParameters>
                                <asp:QueryStringParameter DefaultValue="0" Name="id_empresa" QueryStringField="e" />
                                <asp:QueryStringParameter DefaultValue="0" Name="id_taller" QueryStringField="t" />
                                <asp:QueryStringParameter DefaultValue="0" Name="orden" QueryStringField="o" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </asp:Panel>
                </div> 
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
                    <asp:Label ID="Label3" runat="server" Text="Perfil:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                    <asp:Label ID="ddlPerfil" runat="server"></asp:Label>
                </div>
                <div class="col-lg-4 col-sm-4 text-center">
                    <asp:Label ID="Label7" runat="server" Text="Siniestro:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                    <asp:Label ID="lblSiniestro" runat="server"></asp:Label>
                </div>
                <div class="col-lg-4 col-sm-4 text-center">
                    <asp:Label ID="Label11" runat="server" Text="Deducible:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                    <asp:Label ID="lblDeducible" runat="server"></asp:Label>
                </div>
            </div>
            <div class="row colorBlanco textoBold">
                <div class="col-lg-4 col-sm-4 text-center">
                    <asp:Label ID="Label5" runat="server" Text="Total Orden:" CssClass="colorEtiqueta"
                        Visible="false"></asp:Label>&nbsp;&nbsp;
                    <asp:Label ID="lblTotOrden" runat="server" Visible="false"></asp:Label>
                </div>
                <div class="col-lg-4 col-sm-4 text-center">
                    <asp:Label ID="Label1" runat="server" Text="Promesa:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                    <asp:Label ID="lblEntregaEstimada" runat="server"></asp:Label>
                </div>
                <div class="col-lg-4 col-sm-4 text-center">
                    <asp:Label ID="lblPorcDeduEti" runat="server" Text="% Deducible:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                    <asp:Label ID="lblPorcDedu" runat="server"></asp:Label>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
