<%@ Page Title="" Language="C#" MasterPageFile="~/AdmonOrden.master" AutoEventWireup="true" CodeFile="CalificaOperarios.aspx.cs" Inherits="CalificaOperarios" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="row">
        <div class="col-lg-12 col-sm-12 text-center alert-info">
            <h3>
                <i class="fa fa-star"></i>
                <i class="fa fa-star"></i>
                <i class="fa fa-star"></i>
                <i class="fa fa-star"></i>
                <i class="fa fa-star"></i>&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="Label1" runat="server" Text="Calificación Operarios"></asp:Label>
            </h3>
        </div>
        <div class="row">
            <div class="col-lg-12 col-sm-12 text-center">
                <asp:Label ID="lblErroresOO" runat="server" CssClass="errores alert-danger"></asp:Label>
            </div>
        </div>
    </div>
    
        <asp:Panel ID="Panel4" runat="server" CssClass="paneles col-lg-12 col-sm-12" ScrollBars="Auto">
            <div class="col-lg-12 col-sm-12 text-center">
                <div class="table-responsive">
                    <asp:GridView ID="GridOperarios" runat="server" AutoGenerateColumns="False" 
                        CssClass="table table-bordered" DataSourceID="SqlDataSource1" AllowPaging="true" 
                        PageSize="10" AllowSorting="true">
                        <Columns>                            
                            <asp:BoundField DataField="nombre" HeaderText="Operador" ReadOnly="True" SortExpression="nombre"></asp:BoundField>                            
                            <asp:BoundField DataField="descripcion_go" HeaderText="Grupo" SortExpression="descripcion_go"></asp:BoundField>                            
                            <asp:BoundField DataField="descripcion" HeaderText="Calificación" SortExpression="descripcion"></asp:BoundField>                            
                        </Columns>
                    </asp:GridView>
                    <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:PVW %>'
                        SelectCommand="select * from (
select distinct ltrim(rtrim(isnull(e.Nombres,'')))+' '+ltrim(rtrim(isnull(e.Apellido_Paterno,'')))+' '+ltrim(rtrim(isnull(e.Apellido_Materno,''))) as nombre,mo.id_grupo_op,g.descripcion_go,oo.id_calificacion,c.descripcion--,o.descripcion_op,mo.id_refaccion--
from mano_obra mo
left join operativos_orden oo on oo.id_empresa=mo.id_empresa and oo.id_taller=mo.id_taller and oo.no_orden=mo.no_orden and mo.id_grupo_op=(
(select tabla.grupo from(
select distinct case when CHARINDEX('-', oor.idgops) = 0 then oor.idgops else substring(oor.idgops, 1, CHARINDEX('-', oor.idgops) - 1) end as grupo from operativos_orden oor 
where oor.no_orden = 163709 and oor.id_empresa = 1 and oor.id_taller = 1 and oor.idemp=oo.idemp) as tabla
 where tabla.grupo = mo.id_grupo_op)
) 
left join empleados e on e.idemp=oo.idemp
--inner join operaciones o on o.id_operacion=mo.id_operacion
left join Calificacion c on c.id_calificacion=oo.id_calificacion
left join Grupo_Operacion g on g.id_grupo_op=mo.id_grupo_op
where mo.id_empresa=@id_empresa and mo.id_taller=@id_taller and mo.no_orden=@no_orden
) as t where t.nombre<>'' ">
                        <SelectParameters>
                            <asp:QueryStringParameter QueryStringField="o" Name="no_orden"></asp:QueryStringParameter>
                            <asp:QueryStringParameter QueryStringField="e" Name="id_empresa"></asp:QueryStringParameter>
                            <asp:QueryStringParameter QueryStringField="t" Name="id_taller"></asp:QueryStringParameter>
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
            </div>
        </asp:Panel>
        <div class="pie pad1m">		                                		                                
		    <div class="clearfix">
			    <div class="row colorBlanco textoBold">
                    <div class="col-lg-4 col-sm-4 text-center">
                        <asp:Label ID="Label21" runat="server" Text="Tipo Orden:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
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
                        <asp:Label ID="lblPerfilPie" runat="server" ></asp:Label>
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
    
</asp:Content>


