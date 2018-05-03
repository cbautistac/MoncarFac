<%@ Page Title="" Language="C#" MasterPageFile="~/AdmOrdenes.master" AutoEventWireup="true" CodeFile="ComprobatePagos.aspx.cs" Inherits="ComprobatePagos" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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


    <div class="page-header">
        <!-- /BREADCRUMBS -->
        <div class="clearfix">
            <h3 class="content-title pull-left">
                Complemento de Pago</h3>            
        </div>
    </div>
    
        <asp:UpdatePanel runat="server" ID="updPanelGeneralesFact">
            <ContentTemplate>
                <div class="row">
                    <div class="col-lg-12 col-sm-12 text-center">
                        <asp:Label ID="lblError" runat="server" CssClass="errores" />
                    </div>
                </div>
               
                <asp:Panel runat="server" ID="Panel1" CssClass="row text-center pad1m">
                   
              
          
                    <br /><br />
                    <div class="col-lg-6 col-sm-6 text-center">
                        <asp:Label ID="Label2" runat="server" Text="Estatus:" CssClass="textoBold"></asp:Label>&nbsp;&nbsp;
                        <asp:DropDownList ID="ddlEstatus" runat="server" CssClass="input-medium" AutoPostBack="true">
                            <asp:ListItem Selected="True" Text="En Captura" Value="P"></asp:ListItem>
                            <asp:ListItem Text="Timbrado" Value="T"></asp:ListItem>
                            <asp:ListItem Text="Cancelada" Value="C"></asp:ListItem>
                        </asp:DropDownList>
                    
                    </div>
                    <div class="col-lg-6 col-sm-6 text-center">
                        <asp:LinkButton ID="lnkNuevo" runat="server" CssClass="btn btn-primary t14" OnClick="lnkNuevo_Click"><i class="fa fa-plus-circle"></i><span>&nbsp;Nuevo Documento</span></asp:LinkButton>
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlDaños" CssClass="row">
                    <div class="col-lg-12 col-sm-12">
                        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" EnableAJAX="true">
                            <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" AllowFilteringByColumn="true" runat="server"  EnableHeaderContextMenu="true" Culture="es-Mx" Skin="Metro" OnItemDataBound="RadGrid1_ItemDataBound"
                                EnableHeaderContextFilterMenu="true" AllowPaging="True" PagerStyle-AlwaysVisible="true" DataSourceID="SqlDataSource1" AllowSorting="true" GroupingEnabled="false" PageSize="100" >                        
                                <MasterTableView DataSourceID="SqlDataSource1" AutoGenerateColumns="False" DataKeyNames="idCfd">
                                    <Columns>                                                                
                                        <%--<telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="idCfd" HeaderStyle-Width="150px" FilterControlAltText="Filtro idCfd" HeaderText="idCfd" SortExpression="idCfd" UniqueName="idCfd" Visible="true"/>--%>
                                        <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="Folio" HeaderStyle-Width="70px" FilterControlAltText="Filtro Factura" HeaderText="Referencia Factura" SortExpression="EncReferencia" UniqueName="EncReferencia" Resizable="true"/>
                                        <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="EncFolioUUID" HeaderStyle-Width="250px" FilterControlAltText="Filtro UUID" HeaderText="UUID" SortExpression="EncFolioUUID" UniqueName="EncFolioUUID" Resizable="true"/>
                                        <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="EncFechaGenera" HeaderStyle-Width="130px" FilterControlAltText="Filtro Fecha" HeaderText="Fecha" SortExpression="idcfdAnt" UniqueName="idcfdAnt" Resizable="true"/>
                                        <%--<telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="EncReRFC" HeaderStyle-Width="150px" FilterControlAltText="Filtro RFC" HeaderText="Emitida al R.F.C." SortExpression="EncReRFC" UniqueName="EncReRFC" Resizable="true"/>--%>
                                        <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="EncReNombre" HeaderStyle-Width="300px" FilterControlAltText="Filtro Cliente" HeaderText="Nombre del Receptor del Documento" SortExpression="EncReNombre" UniqueName="EncReNombre" Resizable="true"/>
                                        <%--<telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="tipo" HeaderStyle-Width="120px" FilterControlAltText="Filtro Tipo" HeaderText="Tipo Factura" SortExpression="tipo" UniqueName="tipo" Resizable="true"/>--%>
                                        <telerik:GridTemplateColumn HeaderStyle-Width="50px">
                                            <ItemTemplate><asp:LinkButton ID="lnkSeleccionaDocumento" runat="server" CausesValidation="False"
                                                CssClass="btn btn-info t14" CommandName="Select" ToolTip="Seleccionar" CommandArgument='<%# Eval("idCfd") %>'
                                                OnClick="lnkSeleccionaDocumento_Click"><i class="fa fa-check-square"></i></asp:LinkButton></ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderStyle-Width="50px">
                                            <ItemTemplate><asp:LinkButton ID="lnkCancelar" runat="server" CausesValidation="False" CssClass="btn btn-danger t14"
                                                CommandName="Cancel" ToolTip="Cancelar" OnClick="btnCancelar_Click" OnClientClick="return confirm('¿Está seguro de cancelar la Factura?')" CommandArgument='<%# Eval("idCfd") %>'><i class="fa fa-ban"></i></asp:LinkButton></ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderStyle-Width="50px">
                                            <ItemTemplate><asp:LinkButton ID="lnkImprimir" runat="server" OnClick="lnkImprimir_Click" CausesValidation="False"
                                                CssClass="btn btn-primary t14" CommandName="Print" ToolTip="Imprimir" CommandArgument='<%# Eval("idCfd") %>'><i class="fa fa-print"></i></asp:LinkButton></ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <%--<telerik:GridTemplateColumn HeaderStyle-Width="50px">
                                            <ItemTemplate><asp:LinkButton ID="lnkEnviar" runat="server" CausesValidation="False" CssClass="btn btn-success t14"  onclick="lnkEnviarCorreo_Click"
                                                CommandName="SendEmail" ToolTip="Enviar" CommandArgument='<%# Eval("idCfd")+";"+Eval("recorreo") %>'><i class="fa fa-envelope"></i></asp:LinkButton></ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                         <telerik:GridTemplateColumn HeaderStyle-Width="50px">
                                            <ItemTemplate><asp:LinkButton ID="lnkADD" runat="server"  CssClass="btn btn-info t14"  onclick="lnkADD_Click"
                                                CommandName="SendEmail" ToolTip="Addenda" CommandArgument='<%# Eval("idCfd")+";"+Eval("recorreo") %>'><i class="fa fa-qrcode"></i></asp:LinkButton></ItemTemplate>
                                        </telerik:GridTemplateColumn> --%>                                       
                                    </Columns>
                                </MasterTableView>
                                <ClientSettings>
                                    <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true"></Scrolling>
                                </ClientSettings>                        
                                <PagerStyle PageSizeControlType="RadDropDownList" Mode="NextPrevAndNumeric"></PagerStyle>
                            </telerik:RadGrid>
                        </telerik:RadAjaxPanel>
                    </div>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>"
                            SelectCommand="
select distinct e.Folio,e.idCfd,e.EncFolioUUID,e.idcfdAnt,convert(char(10),e.EncFechaGenera,120)+' '+convert(char(8),e.enchoragenera,120) as EncFechaGenera,e.EncReRFC,case when e.EncReNombre is null then (select renombre from receptores_f where idrecep=e.idrecep) when e.encrenombre='' then (select renombre from receptores_f where idrecep=e.idrecep) else e.encrenombre end as EncReNombre,e.EncEstatus, 
case e.encestatus when 'P' then 'En Captura' when 'E' then 'En Tránsito' when 'T' then 'Timbrado' when 'R' then 'Rechazado' when 'C' then 'Cancelado'	 else 'Otro' end as est,r.recorreo,case e.tipo when 'GL' then 'GLOBAL' when 'MO' then 'MANO OBRA' when 'RE' then 'REFACCIONES' when 'SO' then 'SIN ORDEN' when 'PV' then 'PUNTO DE VENTA' when 'PA' then 'PARTICULAR' when 'NC' then 'NOTA DE CREDITO' else '' end as tipo
from recepcion_pagos_f e left join receptores_f r on r.idrecep=e.idrecep where e.encestatus=@estatus order by e.idcfd desc">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="ddlEstatus" PropertyName="SelectedValue" Name="estatus" />
                            </SelectParameters>
                        </asp:SqlDataSource>

                </asp:Panel>
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
    <br /><br />
     <div class="col-lg-6 col-sm-6 text-center">
                                            <asp:Label ID="lblFechaIni" runat="server" Text="Fecha Inicial:" ></asp:Label>
                    <asp:TextBox ID="txtFechaIni" runat="server" CssClass="alingMiddle input-small"  ></asp:TextBox>
                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" BehaviorID="txtFechaIni_CalendarExtender" TargetControlID="txtFechaIni" Format="yyyy-MM-dd" PopupButtonID="lnkFechaIni"  />
                    <asp:LinkButton ID="lnkFechaIni" runat="server" CssClass="btn btn-info t14" ><i class="fa fa-calendar" ></i></asp:LinkButton>
                            </div>
     <div class="col-lg-6 col-sm-6 text-center">
                               <asp:Label ID="lbl" runat="server" Text="Fecha Final:"></asp:Label>
                    <asp:TextBox ID="txtFechaFin" runat="server" CssClass="alingMiddle input-small" ></asp:TextBox>
                    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" BehaviorID="txtFechaFin_CalendarExtender" TargetControlID="txtFechaFin" Format="yyyy-MM-dd" PopupButtonID="lnkFechaFin" />
                    <asp:LinkButton ID="lnkFechaFin" runat="server" CssClass="btn btn-info t14"><i class="fa fa-calendar"></i></asp:LinkButton> 
                            </div>
                    
                    

    <div class="col-lg-12 col-sm-12 text-center">
         <asp:Label ID="lblIni" runat="server" Visible="false" />
         <asp:Label ID="lblFin" runat="server" Visible="false" />
                    <asp:Button ID="Button1" runat="server" Text="Descargar Facturas" Visible="true" CssClass="btn-info" OnClick="impCatProd_Click" />
            </div> 

</asp:Content>