<%@ Page Title="" Language="C#" MasterPageFile="~/AdmonOrden.master" AutoEventWireup="true" CodeFile="Operacion.aspx.cs" Inherits="Operacion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="row">
        <div class="col-lg-12 col-sm-12 text-center alert-info">
            <h3>
                <i class="fa fa-hourglass-2"></i>&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="Label1" runat="server" Text="Operación"></asp:Label>
            </h3>
        </div>
        <div class="row">
            <div class="col-lg-12 col-sm-12 text-center">
                <asp:Label ID="lblErroresOO" runat="server" CssClass="errores alert-danger"></asp:Label>
            </div>
        </div>
    </div>
    <div class="col-lg-12 col-sm-12 text-center">
        <asp:Panel ID="Panel4" runat="server" CssClass="panelCatalogos text-center" ScrollBars="Auto">
            <div class="col-lg-12 col-sm-12 text-center">
                <div class="table-responsive">
                    <asp:GridView ID="GridAsignaciones" runat="server" AutoGenerateColumns="False" DataKeyNames="no_orden,id_empresa,id_taller,IdEmp,id_asignacion,id_consecutivo_mo"
                        DataSourceID="SqlDataSourceDesasignaciones" OnRowCommand="GridAsignaciones_RowCommand"
                        Visible="false" CssClass="table table-bordered" HeaderStyle-CssClass="encabezadoTabla"
                        AllowPaging="True" AllowSorting="True" OnRowDataBound="GridAsignaciones_RowDataBound"
                        EmptyDataText="No existen asignaciones para mostrar en la orden seleccionada." PageSize="7">
                        <Columns>
                            <asp:TemplateField HeaderText="No. Orden" SortExpression="no_orden" Visible="false">
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# Bind("no_orden") %>' ID="lblNoOrdenDes" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="id_empresa" SortExpression="id_empresa" Visible="false">
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# Bind("id_empresa") %>' ID="lblIdEmpresaDes" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="id_taller" SortExpression="id_taller" Visible="false">
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# Bind("id_taller") %>' ID="lblIdTallerDes" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="IdEmp" SortExpression="IdEmp" Visible="false">
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# Bind("IdEmp") %>' ID="lblIdEmpDes" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="id_asignacion" SortExpression="id_asignacion" Visible="false">
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# Bind("id_asignacion") %>' ID="lblIdAsignacionDes" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="id_consecutivo_mo" SortExpression="id_consecutivo_mo" Visible="false">
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# Bind("id_consecutivo_mo") %>' ID="lblIdMODes" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Operación" SortExpression="texto" HeaderStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblTetxoOP" Text='<%# Bind("texto") %>' CssClass="alingMiddle textoBold" />
                                </ItemTemplate>
                                <HeaderStyle CssClass="text-center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Operario" SortExpression="idEmp" HeaderStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblOperNomDes" CssClass="alingMiddle textoBold" />
                                </ItemTemplate>
                                <HeaderStyle CssClass="text-center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Inicio" SortExpression="fecha_ini" HeaderStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:Label runat="server" CssClass="alingMiddle textoBold" Text='<%# Bind("fecha_ini", "{0:yyyy-MM-dd}") %>'
                                        ID="lblFechaIniDes" />
                                    <asp:TextBox ID="txtFechaIniDes" runat="server" Text='<%# Bind("fecha_ini", "{0:yyyy-MM-dd}") %>' Enabled="false" CssClass="alingMiddle input-small" MaxLength="10" />
                                    <cc1:calendarextender id="txtFechaIniDes_CalendarExtender" runat="server" behaviorid="txtFechaIniDes_CalendarExtender" targetcontrolid="txtFechaIniDes" format="yyyy-MM-dd" popupbuttonid="lnkFini" />
                                    <asp:LinkButton ID="lnkFini" runat="server" CssClass="btn btn-info t14"><i class="fa fa-calendar"></i></asp:LinkButton>&nbsp;
                                    <asp:Label runat="server" CssClass="alingMiddle textoBold" Text='<%# Bind("hora_ini") %>' ID="lblHoraIniDes" />
                                    <asp:TextBox ID="txtHoraIniDes" CssClass="alingMiddle input-small" runat="server" Text='<%# Bind("hora_ini") %>' placeholder="HH:MM:SS" MaxLength="8" />
                                    <asp:LinkButton ID="btnIniCancelarOper" CommandName="cancelIniOper" runat="server" CommandArgument='<%# Eval("no_orden")+";"+Eval("id_empresa")+";"+Eval("id_taller")+";"+Eval("IdEmp")+";"+Eval("id_asignacion")+";"+Eval("id_consecutivo_mo") %>' CssClass="btn btn-danger t14"><i class="fa fa-times"></i></asp:LinkButton>
                                    <asp:LinkButton ID="btnIniTiempo" CommandName="IniTiempo" runat="server" CommandArgument='<%# Eval("no_orden")+";"+Eval("id_empresa")+";"+Eval("id_taller")+";"+Eval("IdEmp")+";"+Eval("id_asignacion")+";"+Eval("id_consecutivo_mo") %>' CssClass="btn btn-success t14"><i class="fa fa-check"></i></asp:LinkButton>
                                </ItemTemplate>
                                <HeaderStyle CssClass="text-center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fin" SortExpression="fecha_fin" HeaderStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:Label runat="server" CssClass="alingMiddle textoBold" Text='<%# Bind("fecha_fin", "{0:yyyy-MM-dd}") %>' ID="lblFechaFinDes" />
                                    <asp:TextBox ID="txtFechaFinDes" runat="server" Text='<%# Bind("fecha_fin", "{0:yyyy-MM-dd}") %>' Enabled="false" CssClass="alingMiddle input-small" MaxLength="10" />
                                    <cc1:calendarextender id="txtFechaFinDes_CalendarExtender" runat="server" behaviorid="txtFechaFinDes_CalendarExtender" targetcontrolid="txtFechaFinDes" format="yyyy-MM-dd" popupbuttonid="lnkFin" />
                                    <asp:LinkButton ID="lnkFin" runat="server" CssClass="btn btn-info t14"><i class="fa fa-calendar"></i></asp:LinkButton>&nbsp;
                                    <asp:Label runat="server" CssClass="alingMiddle textoBold" Text='<%# Bind("hora_fin") %>' ID="lblHoraFinDes" />
                                    <asp:TextBox ID="txtHoraFinDes" CssClass="alingMiddle input-small" runat="server" Text='<%# Bind("hora_fin") %>' placeholder="HH:MM:SS" MaxLength="8" />
                                    <asp:LinkButton ID="btnFinCancelarOper" CommandName="cancelFinOper" runat="server"
                                        CommandArgument='<%# Eval("no_orden")+";"+Eval("id_empresa")+";"+Eval("id_taller")+";"+Eval("IdEmp")+";"+Eval("id_asignacion")+";"+Eval("id_consecutivo_mo") %>'
                                        CssClass="btn btn-danger t14"><i class="fa fa-times"></i></asp:LinkButton>
                                    <asp:LinkButton ID="btnFinTiempo" ValidationGroup="timesFin" CommandName="FinTiempo"
                                        runat="server" CommandArgument='<%# Eval("no_orden")+";"+Eval("id_empresa")+";"+Eval("id_taller")+";"+Eval("IdEmp")+";"+Eval("id_asignacion")+";"+Eval("id_consecutivo_mo") %>'
                                        CssClass="btn btn-success t14"><i class="fa fa-check"></i></asp:LinkButton>

                                </ItemTemplate>
                                <HeaderStyle CssClass="text-center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="estatus" SortExpression="estatus" Visible="false">
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# Bind("estatus") %>' ID="lblEstatusDes" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="monto" SortExpression="monto" Visible="false">
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# Bind("monto") %>' ID="lblMontoDes" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="false">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnReasignaOper" runat="server" CommandName="reasignar" CommandArgument='<%# Eval("no_orden")+";"+Eval("id_empresa")+";"+Eval("id_taller")+";"+Eval("IdEmp")+";"+Eval("id_asignacion")+";"+Eval("id_consecutivo_mo") %>'
                                        CssClass="btn btn-info t14"><i class="fa fa-users"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EditRowStyle CssClass="editarFila" />
                        <EmptyDataRowStyle CssClass="errores" />
                        <HeaderStyle CssClass="encabezadoTablaConsulta colorBlanco textoSinEstilo text-center" />
                        <AlternatingRowStyle CssClass="alterTable" />
                        <PagerStyle CssClass="paginadorConsulta" HorizontalAlign="Center" VerticalAlign="Middle" />
                        <SelectedRowStyle CssClass="selectFila" />
                        <SortedAscendingHeaderStyle CssClass="encabezadoSortAsc colorBlanco textoSinEstilo" />
                        <SortedDescendingHeaderStyle CssClass="encabezadoSortDesc colorBlanco textoSinEstilo" />
                    </asp:GridView>
                    <asp:SqlDataSource runat="server" ID="SqlDataSourceDesasignaciones" ConnectionString='<%$ ConnectionStrings:PVW %>'
                        SelectCommand="select gop.descripcion_go+' '+op.descripcion_op+' '+mo.id_refaccion as texto,oo.no_orden,oo.id_empresa,oo.id_taller,oo.IdEmp,oo.id_asignacion,
                            oo.id_consecutivo_mo,oo.fecha_ini,oo.hora_ini,oo.fecha_fin,oo.hora_fin,oo.estatus,oo.monto 
                            from Operativos_Orden_MO oo
                            inner join Mano_Obra mo on mo.no_orden=oo.no_orden and mo.id_empresa=oo.id_empresa 
                            and mo.id_taller=oo.id_taller and mo.consecutivo=oo.id_consecutivo_mo
                            inner join grupo_operacion gop on gop.id_grupo_op=mo.id_grupo_op
                            inner join Operaciones op on op.id_operacion=mo.id_operacion
                            where oo.no_orden=@no_orden and oo.id_empresa=@id_empresa and oo.id_taller=@id_taller">
                        <SelectParameters>
                            <asp:QueryStringParameter QueryStringField="o" Name="no_orden" />
                            <asp:QueryStringParameter QueryStringField="e" Name="id_empresa" />
                            <asp:QueryStringParameter QueryStringField="t" Name="id_taller" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
            </div>
            <div class="col-lg-12 col-sm-12 text-center">
                <asp:Label ID="lblEmpresaMO" Visible="false" runat="server" />
                <asp:Label ID="lblTallerMO" Visible="false" runat="server" />
                <asp:Label ID="lblConsecutivoMO" Visible="false" runat="server" />
                <asp:Label ID="lblIdAsignacion" Visible="false" runat="server" />
                <div class="table-responsive">
                    <asp:GridView ID="GridOperarios" runat="server" AutoGenerateColumns="False" Visible="false"
                        DataSourceID="SqlDataSourceOperadoresRH" OnRowDataBound="GridOperarios_RowDataBound"
                        CssClass="table table-bordered" AllowPaging="True" AllowSorting="True" PageSize="7">
                        <Columns>
                            <asp:TemplateField HeaderText="Operarios Disponibles" SortExpression="nombre">
                                <ItemTemplate>
                                    <asp:Label runat="server" CssClass="alingMiddle textoBold" Text='<%# Bind("nombre") %>' ID="lblAsigOperMO" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="clv_pichonera" ReadOnly="True" SortExpression="clv_pichonera" HeaderText="Cajones" />
                            <asp:BoundField DataField="resta" ReadOnly="True" SortExpression="resta" HeaderText="Disponibles" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkAsigOperMO" runat="server" OnClick="lnkAsigOperMO_Click" CommandArgument='<%# Bind("IdEmp") %>'
                                        CssClass="btn btn-success t14"><i class="fa fa-user-plus"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false" HeaderText="clv_pichonera" SortExpression="clv_pichonera">
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# Bind("clv_pichonera") %>' ID="lblPichonera"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EditRowStyle CssClass="alert-warning" />
                        <EmptyDataRowStyle CssClass="errores" />
                    </asp:GridView>
                    <asp:SqlDataSource runat="server" ID="SqlDataSourceOperadoresRH" ConnectionString='<%$ ConnectionStrings:PVW %>'
                        SelectCommand="select (rtrim(ltrim(e.Nombres))+' '+rtrim(ltrim(e.Apellido_Paterno))+' '+isnull(rtrim(ltrim(e.Apellido_Materno)),'')) as nombre,e.clv_pichonera,e.IdEmp,(e.clv_pichonera - (select count(*) from Operativos_Orden_MO oo where oo.IdEmp=e.IdEmp and oo.estatus<>'T'))as resta from Empleados e where e.status_empleado!='B' order by resta desc"></asp:SqlDataSource>
                </div>
            </div>
        </asp:Panel>
        <div class="pie pad1m">		                                		                                
		        <div class="clearfix">
			        <div class="row colorBlanco textoBold">
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label2" runat="server" Text="Tipo Orden:"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="ddlToOrden" runat="server" ></asp:Label>
                        </div>
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label4" runat="server" Text="Cliente:"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="ddlClienteOrden" runat="server" ></asp:Label>
                        </div>
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label6" runat="server" Text="Tipo Servicio:"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="ddlTsOrden" runat="server" ></asp:Label>
                        </div>
                    </div>                                              
                    <div class="row colorBlanco textoBold">
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label8" runat="server" Text="Tipo Valuación:"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="ddlValOrden" runat="server" ></asp:Label>
                        </div>
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label10" runat="server" Text="Tipo Asegurado:"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="ddlTaOrden" runat="server" ></asp:Label>
                        </div>
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label12" runat="server" Text="Localización:"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="ddlLocOrden" runat="server" ></asp:Label>
                        </div>
                    </div>    
		        </div>
            </div>
    </div>
</asp:Content>

