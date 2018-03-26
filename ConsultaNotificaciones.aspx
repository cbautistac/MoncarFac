<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ConsultaNotificaciones.aspx.cs" Inherits="ConsultaNotificaciones" Culture="es-Mx" UICulture="es-Mx" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="UTF-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1"/> 
	<meta name="viewport" content="width=device-width, initial-scale=1.0"/> 
    <title>Descarga Archivos</title>
    <link rel="stylesheet" type="text/css" href="css/cloud-admin.css" />
	<link rel="stylesheet" type="text/css"  href="css/themes/default.css" />
	<link rel="stylesheet" type="text/css"  href="css/responsive.css" />	
	<link href="css/4.4.0/css/font-awesome.min.css" rel="stylesheet"/>
	<link rel="stylesheet" type="text/css"  href="css/generales.css" />
	<!-- FONTS -->
	<link href='http://fonts.googleapis.com/css?family=Open+Sans:300,400,600,700' rel='stylesheet' type='text/css'/>
	<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>    
</head>
<body>
    <form id="form1" runat="server">
        <div class="encabezado">        
            <table class="ancho100 colorBlanco alineado alto100">
                <tr>
                    <td class="marginLeft top0">
                        <asp:Label ID="Label2" runat="server" Text="MoncarWeb" CssClass="margin-left-50 titulo negritas colorMoncarAzul alineado margenLeft" Visible="false" />&nbsp;
                        <asp:Label ID="lblEmpresa" runat="server" CssClass="colorMoncarAzul alineado" Visible="false" ></asp:Label>                        
                        <asp:Image ID="imgEmpresa" runat="server" CssClass="img-responsive imagenLogo" ImageUrl="~/img/logo.png"/>                        
                    </td>                    
                    <td class="textoBold text-center">
                        <asp:Label ID="lblTallerSesion" runat="server" CssClass="colorMorado"></asp:Label><br />
                        <asp:Label ID="lblUser" runat="server" CssClass="colorBlanco"></asp:Label>
                    </td>                    
                </tr>
            </table>
        </div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True"/>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <div class="row">
                    <div class="col-lg-12 col-sm-12 text-center alert-info">
                        <h3>
                            <i class="fa fa-bell"></i>&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="Label3" runat="server" Text="Notificaciones"></asp:Label>                        
                        </h3>                    
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-12 col-sm-12 text-center">
                        <asp:Label ID="Label1" runat="server" Text="Fecha:"></asp:Label>
                        <asp:TextBox ID="txtFechaIni" runat="server" CssClass="input-medium" MaxLength="10" Enabled="false" ></asp:TextBox>
                        <cc1:CalendarExtender ID="txtFechaIni_CalendarExtender" runat="server" PopupButtonID="lnkFini" BehaviorID="txtFechaIni_CalendarExtender" TargetControlID="txtFechaIni" Format="yyyy-MM-dd"></cc1:CalendarExtender>
                        <asp:LinkButton ID="lnkFini" runat="server" CssClass="btn btn-info t14"><i class="fa fa-calendar"></i></asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="btnConsultar" runat="server" CssClass="btn btn-info t14" ToolTip="Consultar" onclick="btnConsultar_Click" ><i class="fa fa-search"></i>&nbsp;<span>Consultar</span></asp:LinkButton>
                    </div>                        
                </div>                    
                <div class="row">
                    <div class="col-lg-12 col-sm-12 center">
                        <asp:Label ID="lblError" runat="server" CssClass="errores textoBold"></asp:Label>
                    </div>
                </div>   
                <asp:Panel ID="Panel4" runat="server" CssClass="panelCatalogos text-center" ScrollBars="Auto">
                    <div class="table-responsive">
                        <asp:GridView ID="GridView1" runat="server" 
                            EmptyDataRowStyle-CssClass="errores" EmptyDataText="No existen notificaciones registradas"
                            CssClass="table table-bordered center" AutoGenerateColumns="False"   
                            AllowPaging="True" AllowSorting="True" DataKeyNames="id_notificacion"
                            DataSourceID="SqlDataSource1" onrowdatabound="GridView1_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="id_notificacion" HeaderText="id_notificacion" SortExpression="id_notificacion" Visible="false" />
                                <asp:BoundField DataField="hora" HeaderText="hora" SortExpression="hora" ReadOnly="True" />
                                <asp:BoundField DataField="notificacion" HeaderText="notificacion" SortExpression="notificacion" />                                
                                <asp:BoundField DataField="estatus" HeaderText="estatus" SortExpression="estatus" Visible="false" />
                                <asp:BoundField DataField="clase" HeaderText="clase" SortExpression="clase" ReadOnly="True" Visible="false" />
                                <asp:BoundField DataField="usuario" HeaderText="usuario" SortExpression="usuario" Visible="false" />
                                <asp:BoundField DataField="nombre_usuario" HeaderText="nombre_usuario" SortExpression="nombre_usuario" ReadOnly="True" />                                                                                                
                                <asp:TemplateField HeaderText="Marcar como Leído">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkLeido" runat="server" oncheckedchanged="chkLeido_CheckedChanged" AutoPostBack="true" ToolTip='<%# Eval("id_notificacion") %>'  />                                        
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                            </Columns>
                            <SelectedRowStyle CssClass="alert-success-org" />
                            <EmptyDataRowStyle CssClass="errores" />
                        </asp:GridView>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                            ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select n.id_notificacion,convert(char(10),n.hora,108) as hora,n.notificacion,n.clasificacion,n.estatus,
case n.clasificacion when 1 then 'fa fa-sign-in' when 2 then 'fa fa-list-alt' when 3 then 'fa fa-cogs' when 4 then 'fa fa-check-circle' when 5 then 'fa fa-check-square' when 6 then 'fa fa-info-circle'  when 7 then 'fa fa-sign-out'  else '' end clase,
n.usuario,u.nombre_usuario
from Notificaciones n 
left join usuarios u on u.id_usuario=n.usuario
where n.fecha=@fecha and n.id_empresa=@idEmpresa and n.id_taller=@idTaller order by n.hora desc ">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="txtFechaIni" Name="fecha" PropertyName="Text" />
                                <asp:QueryStringParameter Name="idEmpresa" QueryStringField="e" />
                                <asp:QueryStringParameter Name="idTaller" QueryStringField="t" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                </asp:Panel>
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel2">
                    <ProgressTemplate>
                        <asp:Panel ID="pnlMaskLoad" runat="server" CssClass="maskLoad"></asp:Panel>
                        <asp:Panel ID="pnlCargando" runat="server" CssClass="pnlPopUpLoad">
                            <asp:Image ID="imgLoad" runat="server" ImageUrl="~/img/loading.gif" CssClass="ancho100" />                            
                        </asp:Panel>
                    </ProgressTemplate>                            
                </asp:UpdateProgress>

            </ContentTemplate>
        </asp:UpdatePanel>




        <div class="row">
            <div class="col-lg-12 col-sm-12 text-center">
                <asp:LinkButton ID="lnkRegresar" runat="server" CssClass="btn btn-info t14" onclick="lnkRegresar_Click" 
                    ><i class="fa fa-reply"></i>&nbsp;<span>Regresar</span></asp:LinkButton>
            </div>
        </div>
    </form>
</body>
</html>