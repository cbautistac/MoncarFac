<%@ Page Title="" Language="C#" MasterPageFile="~/AdmonOrden.master" AutoEventWireup="true" CodeFile="Asignacion.aspx.cs" Inherits="Asignacion" Culture="es-Mx" UICulture="es-Mx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-lg-12 col-sm-12 text-center alert-info">
                    <h3><i class="fa fa-list-alt"></i>&nbsp;&nbsp;&nbsp;<asp:Label ID="Label1" runat="server" Text="Asignación"></asp:Label></h3>
                </div>
            </div>            
            <div class="row">
                <div class="col-lg-12 col-sm-12 text-center">
                    <asp:Label ID="Label13" runat="server" Text="Fecha Promesa" />
                    <asp:TextBox ID="txtFechaPromesa" runat="server" Enabled="false" CssClass="input-small" />
                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" BehaviorID="txtFechaPromesa_CalendarExtender" TargetControlID="txtFechaPromesa" Format="yyyy-MM-dd" PopupButtonID="lnkFechaPromesa" />
                    <asp:LinkButton ID="lnkFechaPromesa" runat="server" CssClass="btn btn-info t14"><i class="fa fa-calendar"></i></asp:LinkButton>&nbsp;&nbsp;                    
                    <asp:TextBox ID="txtHoraPromesa" runat="server" CssClass="input-small" Visible="false" />
                    <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" BehaviorID="txtHoraPromesa_TextBoxWatermarkExtender" TargetControlID="txtHoraPromesa" WatermarkText="hh:mm:ss" WatermarkCssClass="water input-small" />
                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" BehaviorID="txtHoraPromesa_FilteredTextBoxExtender" TargetControlID="txtHoraPromesa" FilterType="Numbers, Custom" ValidChars=":" />
                </div>
            </div>
            <div class="row">
                <div class="col-lg-4 col-sm-4 text-left">
                    <asp:Label ID="Label5" runat="server" Text="Operario:"></asp:Label>
                    <asp:Label ID="lblIdOperativo" runat="server" Visible="false"></asp:Label>
                    <asp:TextBox ID="txtOperativo" runat="server" CssClass="input-large" Enabled="false"></asp:TextBox>
                    <cc1:TextBoxWatermarkExtender ID="txtOperativo_TextBoxWatermarkExtender" runat="server" BehaviorID="txtOperativo_TextBoxWatermarkExtender" TargetControlID="txtOperativo" WatermarkText="Operario" WatermarkCssClass="water input-large" />
                    <asp:LinkButton ID="lnkBuscar" runat="server" CssClass="btn btn-info t14" OnClick="lnkBuscar_Click"><i class="fa fa-search"></i></asp:LinkButton>&nbsp;&nbsp;
                </div>
                <div class="col-lg-4 col-sm-4 text-left">
                    <asp:Label ID="Label7" runat="server" Text="Inicio Programado:"></asp:Label>
                    <asp:TextBox ID="txtFechaIniProg" runat="server" CssClass="alingMiddle input-small" Enabled="false"></asp:TextBox>
                    <cc1:CalendarExtender ID="txtFechaIniProg_CalendarExtender" runat="server" BehaviorID="txtFechaIniProg_CalendarExtender" TargetControlID="txtFechaIniProg" Format="yyyy-MM-dd" PopupButtonID="lnkFechaIniProg" />
                    <asp:LinkButton ID="lnkFechaIniProg" runat="server" CssClass="btn btn-info t14"><i class="fa fa-calendar"></i></asp:LinkButton>&nbsp;&nbsp;                                        
                    <asp:TextBox ID="txtHoraIniProg" runat="server" CssClass="alingMiddle input-small" MaxLength="8" Visible="false" ></asp:TextBox>
                    <cc1:TextBoxWatermarkExtender ID="txtHoraIniProg_TextBoxWatermarkExtender" runat="server" BehaviorID="txtHoraIniProg_TextBoxWatermarkExtender" TargetControlID="txtHoraIniProg" WatermarkText="hh:mm:ss" WatermarkCssClass="water input-small" />
                    <cc1:FilteredTextBoxExtender ID="txtHoraIniProg_FilteredTextBoxExtender" runat="server" BehaviorID="txtHoraIniProg_FilteredTextBoxExtender" TargetControlID="txtHoraIniProg" FilterType="Numbers, Custom" ValidChars=":" />
                </div>
                <div class="col-lg-2 col-sm-2 text-center">
                    <asp:Label ID="Label11" runat="server" Text="Monto:"></asp:Label>
                    <asp:TextBox ID="txtMonto" runat="server" CssClass="alingMiddle input-small" MaxLength="14"></asp:TextBox>
                    <cc1:TextBoxWatermarkExtender ID="txtMontoWatermarkExtender1" runat="server" BehaviorID="txtMonto_TextBoxWatermarkExtender" TargetControlID="txtMonto" WatermarkText="Monto" WatermarkCssClass="water input-small" />
                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" FilterType="Numbers, Custom" ValidChars="." TargetControlID="txtMonto" />
                </div>
            </div>
            <div class="row">
                <div class="col-lg-12 col-sm-12 text-center alert-info">
                    <asp:Label ID="Label14" runat="server" Text="Grupos de Operación a Trabajar:" CssClass="textoBold"/>&nbsp;
                </div>
            </div>
            <div class="row">
                <asp:Panel ID="Panel4" runat="server" CssClass="col-lg-12 col-sm-12 text-center">
                    <asp:CheckBoxList ID="chkGopsOper" RepeatColumns="6" runat="server" DataSourceID="SqlDataSource3" CssClass="centrado"
                        DataTextField="descripcion_go" DataValueField="id_grupo_op" RepeatDirection="Horizontal" CellPadding="10" TextAlign="Right">
                    </asp:CheckBoxList>
                    <asp:SqlDataSource runat="server" ID="SqlDataSource3" ConnectionString='<%$ ConnectionStrings:PVW%>' 
                        SelectCommand="select mo.id_grupo_op,'&nbsp;&nbsp;'+gop.descripcion_go as descripcion_go
                            from mano_obra mo
                            inner join Grupo_Operacion gop on gop.id_grupo_op=mo.id_grupo_op
                            where mo.no_orden=@no_orden and mo.id_empresa=@id_empresa and mo.id_taller=@id_taller
                            group by mo.id_grupo_op,mo.no_orden,gop.descripcion_go
                            order by mo.id_grupo_op asc">
                        <SelectParameters>
                            <asp:QueryStringParameter QueryStringField="o" Name="no_orden"></asp:QueryStringParameter>
                            <asp:QueryStringParameter QueryStringField="e" Name="id_empresa"></asp:QueryStringParameter>
                            <asp:QueryStringParameter QueryStringField="t" Name="id_taller"></asp:QueryStringParameter>
                        </SelectParameters>
                    </asp:SqlDataSource>
                </asp:Panel>
            </div>
            <div class="row">
                <div class="col-lg-12 col-sm-12 text-center">
                    <asp:LinkButton ID="lnkAgregarOperario" runat="server" CssClass="btn btn-info t14" OnClick="lnkAgregarOperario_Click"><i class="fa fa-plus-circle"></i>&nbsp;<span>Agregar Operario</span></asp:LinkButton>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-12 col-sm-12 text-center">
                    <asp:Label ID="lblError" runat="server" CssClass="errores"></asp:Label>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-12 col-sm-12 text-center">
                    <asp:Label ID="Label2" runat="server" Text="* Operarios sin cajones disponibles en rojo." CssClass="errores" Visible="false" />
                    <asp:LinkButton ID="lnkCancelaVista" runat="server" CssClass="btn btn-danger t14" OnClick="lnkCancelaVista_Click" Visible="false"><i class="fa fa-close"></i>&nbsp;<span>Cancelar</span></asp:LinkButton>
                </div>
            </div>
            <asp:Panel runat="server" ID="pnlVehiculo" CssClass="paneles col-lg-12 col-sm-12" ScrollBars="Auto">
                <asp:Panel ID="PanelManoObra" runat="server" CssClass="row">

                    <asp:Panel ID="Panel2" runat="server" CssClass="col-lg-6 col-sm-6 table-responsive" Visible="false">
                        <div class="col-lg-12 col-sm-12 text-center t14 textoBold alert-info">Consulta Disponibilidad de Operarios</div>
                        <div class="col-lg-12 col-sm-12 text-center">
                            <asp:Label ID="Label9" runat="server" Text="Buscar:"></asp:Label>
                            <asp:TextBox ID="txtBuscaOperario" runat="server" CssClass="input-medium"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="txtBuscaOperario_TextBoxWatermarkExtender" runat="server" BehaviorID="txtBuscaOperario_TextBoxWatermarkExtender" TargetControlID="txtBuscaOperario" WatermarkCssClass="water input-medium" WatermarkText="Buscar . . ." />
                            <asp:LinkButton ID="lnkBuscarOpe" runat="server" CssClass="btn btn-info t14" OnClick="lnkBuscarOpe_Click"><i class="fa fa-search"></i></asp:LinkButton>
                            <asp:LinkButton ID="hpkLimpiar" runat="server" Font-Size="8pt" OnClick="hpkLimpiar_Click">Limpiar Búsqueda</asp:LinkButton>
                        </div>
                        <asp:GridView ID="GridOperarios" runat="server" AutoGenerateColumns="False" EmptyDataText="No existen operarios registrados" EmptyDataRowStyle-CssClass="errores"
                            DataSourceID="SqlDataSourceOperadoresRH" OnRowDataBound="GridOperarios_RowDataBound" DataKeyNames="IdEmp"
                            CssClass="table table-bordered" AllowPaging="True" AllowSorting="True" PageSize="7">
                            <Columns>
                                <asp:TemplateField HeaderText="Operarios Disponibles" SortExpression="nombre">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Bind("nombre") %>' ID="lblAsigOperMO" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="clv_pichonera" ReadOnly="True" SortExpression="clv_pichonera" HeaderText="Cajones" />
                                <asp:BoundField DataField="resta" ReadOnly="True" SortExpression="resta" HeaderText="Disponibles" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkAsigOperMO" runat="server" OnClick="lnkAsigOperMO_Click" ToolTip="Asignar" CommandArgument='<%# Eval("IdEmp")+";"+Eval("nombre")+";"+Eval("resta") %>' CssClass="btn btn-success t14"><i class="fa fa-user-plus"></i></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false" HeaderText="clv_pichonera" SortExpression="clv_pichonera">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Bind("clv_pichonera") %>' ID="lblPichonera"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDetalleOperario" runat="server" OnClick="lnkDetalleOperario_Click" ToolTip="Asignaciones" CommandArgument='<%# Eval("IdEmp")+";"+Eval("nombre") %>' CssClass="btn btn-info t14"><i class="fa fa-plus-circle"></i></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EditRowStyle CssClass="alert-warning" />
                            <EmptyDataRowStyle CssClass="errores" />
                        </asp:GridView>
                        <asp:SqlDataSource runat="server" ID="SqlDataSourceOperadoresRH" ConnectionString='<%$ ConnectionStrings:PVW%>'
                            SelectCommand="select (rtrim(ltrim(e.Nombres))+' '+rtrim(ltrim(e.Apellido_Paterno))+' '+isnull(rtrim(ltrim(e.Apellido_Materno)),'')) as nombre,e.clv_pichonera,e.IdEmp,(e.clv_pichonera - (select count(*) from Operativos_Orden oo where oo.IdEmp=e.IdEmp and oo.estatus<>'T'))as resta from Empleados e where e.status_empleado!='B' and e.tipo_empleado in ('EX','IN') order by resta desc"></asp:SqlDataSource>
                    </asp:Panel>

                    <asp:Panel ID="Panel1" runat="server" CssClass="col-lg-12 col-sm-12 table-responsive" Visible="true">
                        <div class="col-lg-12 col-sm-12 text-center t14 textoBold alert-info">Operarios Asignados</div>
                        <asp:GridView ID="grdOperativosOrden" runat="server" PageSize="7" DataKeyNames="id_asignacion"
                            EmptyDataText="No existen operarios asignados a la orden" EmptyDataRowStyle-CssClass="errores"
                            CssClass="table table-bordered" AllowPaging="True" AllowSorting="True"
                            AutoGenerateColumns="False" DataSourceID="SqlDataSource1"
                            OnRowCommand="grdOperativosOrden_RowCommand"
                            OnRowDataBound="grdOperativosOrden_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="id_asignacion" HeaderText="id_asignacion" SortExpression="id_asignacion" Visible="false" ReadOnly="true" />
                                <asp:BoundField DataField="idEmp" HeaderText="idEmp" SortExpression="idEmp" Visible="false" ReadOnly="true" />
                                <asp:BoundField DataField="LLave_nombre_empleado" HeaderText="Operario" SortExpression="LLave_nombre_empleado" ReadOnly="true" />
                                <asp:TemplateField HeaderText="Inicio Programado" SortExpression="fIniProg">
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("fIniProg") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtFechaIniProgM" runat="server" CssClass="alingMiddle input-small" Enabled="false" Text='<%# Eval("fechaIniProg") %>'></asp:TextBox>
                                        <cc1:CalendarExtender ID="txtFechaIniProgM_CalendarExtender" runat="server" BehaviorID="txtFechaIniProgM_CalendarExtender" TargetControlID="txtFechaIniProgM" Format="yyyy-MM-dd" PopupButtonID="lnkFechaIniProgM" />
                                        <asp:LinkButton ID="lnkFechaIniProgM" runat="server" CssClass="btn btn-info t14"><i class="fa fa-calendar"></i></asp:LinkButton>&nbsp;&nbsp;                                        
                                        <asp:TextBox ID="txtHoraIniProgM" runat="server" CssClass="alingMiddle input-small" MaxLength="8" Text='<%# Eval("hIniProg") %>' Visible="false"></asp:TextBox>
                                        <cc1:TextBoxWatermarkExtender ID="txtHoraIniProgM_TextBoxWatermarkExtender" runat="server" BehaviorID="txtHoraIniProgM_TextBoxWatermarkExtender" TargetControlID="txtHoraIniProgM" WatermarkText="hh:mm:ss" WatermarkCssClass="water input-small" />
                                        <cc1:FilteredTextBoxExtender ID="txtHoraIniProgM_FilteredTextBoxExtender" runat="server" BehaviorID="txtHoraIniProgM_FilteredTextBoxExtender" TargetControlID="txtHoraIniProgM" FilterType="Numbers, Custom" ValidChars=":" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Monto" SortExpression="monto">
                                    <ItemTemplate>
                                        <asp:Label ID="Label21" runat="server" Text='<%# Eval("monto","{0:C2}") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtMontoM" runat="server" CssClass="alingMiddle input-small" MaxLength="14" Text='<%# Eval("monto","{0:C2}") %>'></asp:TextBox>
                                        <cc1:TextBoxWatermarkExtender ID="txtMontoMWatermarkExtender1" runat="server" BehaviorID="txtMontoM_TextBoxWatermarkExtender" TargetControlID="txtMontoM" WatermarkText="Monto" WatermarkCssClass="water input-small" />
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" FilterType="Numbers, Custom" ValidChars="." TargetControlID="txtMontoM" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="estatus" HeaderText="estatus" SortExpression="estatus" Visible="False" />

                                <asp:TemplateField HeaderText="Grup. Operación" SortExpression="descripcion_go">
                                    <EditItemTemplate>
                                        <asp:Label ID="lblGopsIds" runat="server" Visible="false" Text='<%# Eval("idgops") %>' />
                                        <asp:CheckBoxList ID="chkGopsEdit" runat="server" RepeatColumns="3" RepeatDirection="Horizontal" CellPadding="10"
                                            DataSourceID="SqlDataSource4" DataTextField="descripcion_go" DataValueField="id_grupo_op">
                                        </asp:CheckBoxList>
                                        <asp:SqlDataSource runat="server" ID="SqlDataSource4" ConnectionString='<%$ ConnectionStrings:PVW%>' 
                                            SelectCommand="select mo.id_grupo_op,'&nbsp;&nbsp;'+gop.descripcion_go as descripcion_go
                                                from mano_obra mo
                                                inner join Grupo_Operacion gop on gop.id_grupo_op=mo.id_grupo_op
                                                where mo.no_orden=@no_orden and mo.id_empresa=@id_empresa and mo.id_taller=@id_taller
                                                group by mo.id_grupo_op,mo.no_orden,gop.descripcion_go
                                                order by mo.id_grupo_op asc">
                                            <SelectParameters>
                                                <asp:QueryStringParameter QueryStringField="o" Name="no_orden"></asp:QueryStringParameter>
                                                <asp:QueryStringParameter QueryStringField="e" Name="id_empresa"></asp:QueryStringParameter>
                                                <asp:QueryStringParameter QueryStringField="t" Name="id_taller"></asp:QueryStringParameter>
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblGopsIds" runat="server" Visible="false" Text='<%# Eval("idgops") %>' />
                                        <asp:Label ID="lblGopsText" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEdita" runat="server" CausesValidation="False" CommandName="Edit" CssClass="btn btn-warning t14"><i class="fa fa-edit"></i></asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton ID="lnkActualiza" runat="server" CausesValidation="True" CommandName="Update" CssClass="btn btn-success t14" CommandArgument='<%# Eval("id_asignacion")+";"+Eval("idEmp") %>'><i class="fa fa-save"></i></asp:LinkButton>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False">
                                    <EditItemTemplate>
                                        <asp:LinkButton ID="lnkCancela" runat="server" CausesValidation="False" CommandName="Cancel" CssClass="btn btn-danger t14"><i class="fa fa-close"></i></asp:LinkButton>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkElimina" runat="server" CausesValidation="False" CommandName="Delete" CssClass="btn btn-danger t14" CommandArgument='<%# Eval("id_asignacion")+";"+Eval("idEmp") %>' OnClientClick="return confirm('¿Está seguro de eliminar el operativo de la orden?');"><i class="fa fa-trash"></i></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EditRowStyle CssClass="alert-warning" />
                            <EmptyDataRowStyle CssClass="errores" />
                        </asp:GridView>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PVW%>" 
                            SelectCommand="select o.id_asignacion,o.idEmp,CASE WHEN e.LLave_nombre_empleado IS NULL THEN isnull(e.nombres,'')+' '+isnull(e.apellido_paterno,'')+' '+isnull(e.apellido_materno,'') else e.llave_nombre_empleado end as Llave_nombre_empleado,Convert(char(10),o.fecha_ini_prog,126) as fIniProg,Convert(char(8),o.hora_ini_prog,108) as hIniProg,Convert(char(10),o.fecha_ini_prog,126) as fechaIniProg,Convert(char(8),o.hora_ini_prog,108) as hIniProg,o.estatus,isnull(o.monto,0) as monto, isnull(idgops,'0')as idgops from operativos_orden o left join Empleados e on e.IdEmp=o.idEmp where o.id_empresa=@id_empresa and o.id_taller=@id_taller and o.no_orden=@no_orden">
                            <SelectParameters>
                                <asp:QueryStringParameter DefaultValue="0" Name="id_empresa" QueryStringField="e" />
                                <asp:QueryStringParameter DefaultValue="0" Name="id_taller" QueryStringField="t" />
                                <asp:QueryStringParameter DefaultValue="0" Name="no_orden" QueryStringField="o" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </asp:Panel>

                    <asp:Panel ID="Panel3" runat="server" CssClass="col-lg-6 col-sm-6 table-responsive" Visible="false">
                        <div class="col-lg-12 col-sm-12 text-center t14 textoBold alert-info">
                            <asp:Label ID="lblOrdAsig" runat="server"></asp:Label>
                        </div>
                        <asp:GridView ID="grdDetalle" runat="server" DataSourceID="SqlDataSource2" AutoGenerateColumns="false" EmptyDataText="No existen ordenes asignadas" EmptyDataRowStyle-CssClass="errores" CssClass="table table-bordered" AllowPaging="True" AllowSorting="True" PageSize="7">
                            <Columns>
                                <asp:BoundField DataField="nombre_taller" ReadOnly="True" SortExpression="nombre_taller" HeaderText="Taller" />
                                <asp:BoundField DataField="no_orden" ReadOnly="True" SortExpression="no_orden" HeaderText="Orden" />
                                <asp:BoundField DataField="fecha_ini_prog" ReadOnly="True" SortExpression="fecha_ini_prog" HeaderText="Inicio Programado" />
                                <asp:BoundField DataField="fecha_ini" ReadOnly="True" SortExpression="fecha_ini" HeaderText="Fecha Inicio" />
                                <asp:BoundField DataField="fecha_fin" ReadOnly="True" SortExpression="fecha_fin" HeaderText="Fecha Fin" />
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource runat="server" ID="SqlDataSource2" ConnectionString='<%$ ConnectionStrings:PVW%>'></asp:SqlDataSource>
                    </asp:Panel>
                </asp:Panel>
            </asp:Panel>


            <asp:UpdateProgress ID="UpdateProgress100" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
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

    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <div class="row pad1m">
                <div class="col-lg-6 col-sm-6 text-center pad1m">
                    <asp:LinkButton ID="btnImprime" runat="server" ToolTip="Orden de Trabajo" CssClass="btn btn-info t14" OnClick="btnImprime_Click"><i class="fa fa-print"></i><span>&nbsp;Imprime Orden de Trabajo</span></asp:LinkButton>
                </div>
                <div class="col-lg-6 col-sm-6 text-center pad1m">
                    <asp:LinkButton ID="lnkNotificar" runat="server" ToolTip="Notificar" CssClass="btn btn-warning t14" OnClick="lnkNotificar_Click"><i class="fa fa-bell"></i><span>&nbsp;Notificar</span></asp:LinkButton>
                </div>
            </div>

            <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel2">
                <ProgressTemplate>
                    <asp:Panel ID="pnlMaskLoad1" runat="server" CssClass="maskLoad">
                    </asp:Panel>
                    <asp:Panel ID="pnlCargando1" runat="server" CssClass="pnlPopUpLoad">
                        <asp:Image ID="imgLoad1" runat="server" ImageUrl="~/img/loading.gif" CssClass="ancho100" />
                    </asp:Panel>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </ContentTemplate>
    </asp:UpdatePanel>




    <div class="pie pad1m">
        <div class="clearfix">
            <div class="row colorBlanco textoBold">
                <div class="col-lg-4 col-sm-4 text-center">
                    <asp:Label ID="lblFaseIni" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="lblLocalizacionIni" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="lblAvanceIni" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="Label3" runat="server" Text="Tipo Orden:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
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
                    <asp:Label Visible="false" ID="Label111" runat="server" Text="Total Orden:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label Visible="false" ID="lblTotOrden" runat="server"></asp:Label>
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

</asp:Content>
