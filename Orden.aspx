<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Orden.aspx.cs" Inherits="Orden" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Orden de Compra</title>
    <meta charset="UTF-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1"/> 
	<meta name="viewport" content="width=device-width, initial-scale=1.0"/>     
    <link href="css/cloud-admin.css" rel="stylesheet" type="text/css" />
    <link href="css/menus.css" rel="stylesheet" type="text/css" />
	<link rel="stylesheet" type="text/css"  href="css/responsive.css" />	
	<link href="css/4.4.0/css/font-awesome.min.css" rel="stylesheet"/>
	<link rel="stylesheet" type="text/css"  href="css/generales.css" />
    <link href="css/dashboard.css" rel="stylesheet" type="text/css" />
	<!-- FONTS -->
	<link href='http://fonts.googleapis.com/css?family=Open+Sans:300,400,600,700' rel='stylesheet' type='text/css'/>
	<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
    <script src="js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="js/bootstrap.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    
        <div class="encabezado">        
            <table class="ancho100 colorBlanco alineado alto100">
                <tr>
                    <td class="marginLeft textoIzquierda">
                        <asp:Label ID="lbltitulo" runat="server" Text="MoncarWeb" CssClass="margin-left-50 titulo negritas colorMoncarAzul alineado margenLeft" Visible="false" />&nbsp;
                        <asp:Label ID="lblEmpresa" runat="server" CssClass="colorMoncarAzul alineado" Visible="false" ></asp:Label>
                        <asp:Image ID="imgEmpresa" runat="server" CssClass="img-responsive imagenLogo" ImageUrl="~/img/moncar.png"/>                       
                    </td>     
                </tr>
            </table>
        </div>
        <section id="page">
            
			    <div class="container">
				    <div class="row">
					    <div id="content" class="col-lg-12">
						    <!-- PAGE HEADER-->
						    <div class="row">
							    <div class="col-sm-12">
                                    <div class="page-header">		                                		                                
		                                <div class="clearfix">
			                                <h3 class="content-title pull-left">
                                                <asp:Label ID="lblOrdenSelect" runat="server" ></asp:Label><br />
                                                <asp:Label ID="lblPropveedor" runat="server" ></asp:Label>
                                            </h3>                                              
		                                </div>
                                    </div>                                   
	                                
                                    <div class="contenidos"> 
                                    
                                            <div class="col-lg-12 col-sm-12 text-center">
                                                <asp:Label ID="lblError" runat="server" CssClass="errores"></asp:Label>                                                
                                            </div>
                                        <asp:Panel ID="Panel2" runat="server" CssClass="col-lg-12 col-sm-12" Visible="false">
                                        <table class="table table-bordered" cellspacing="0" rules="all" border="1" style="border-collapse:collapse;">
                                            <tr class="alert-info">
                                                <th scope="col" class="col-lg-2 col-sm-2"><asp:Label ID="Label1" runat="server" Text="Cantidad"></asp:Label></th>
                                                <th scope="col" class="col-lg-4 col-sm-4"><asp:Label ID="Label2" runat="server" Text="Refacción"></asp:Label></th>
                                                <th scope="col" class="col-lg-2 col-sm-2"><asp:Label ID="Label3" runat="server" Text="Costo Unitario"></asp:Label></th>
                                                <th scope="col" class="col-lg-2 col-sm-2"><asp:Label ID="Label4" runat="server" Text="% Descuento"></asp:Label></th>
                                                <th scope="col" class="col-lg-2 col-sm-2"><asp:Label ID="Label5" runat="server" Text="Procedencia"></asp:Label></th>                                                
                                            </tr>                                       
                                        </table>
                                        </asp:Panel>
                                        <asp:Panel ID="Panel1" runat="server" CssClass="paneles col-lg-12 col-sm-12" ScrollBars="Auto" >
                                            <div class="table-responsive">
                                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered"
                                                    DataKeyNames="no_orden,id_empresa,id_taller,id_orden,id_detalle" AllowSorting="true" OnRowDataBound="GridView1_RowDataBound"
                                                    DataSourceID="SqlDataSource1">
                                                    <Columns>
                                                        <asp:BoundField DataField="no_orden" HeaderText="no_orden" ReadOnly="True" Visible="false" SortExpression="no_orden" />
                                                        <asp:BoundField DataField="id_empresa" HeaderText="id_empresa" ReadOnly="True" Visible="false" SortExpression="id_empresa" />
                                                        <asp:BoundField DataField="id_taller" HeaderText="id_taller" ReadOnly="True" Visible="false" SortExpression="id_taller" />
                                                        <asp:BoundField DataField="id_orden" HeaderText="id_orden" ReadOnly="True" Visible="false" SortExpression="id_orden" />
                                                        <asp:BoundField DataField="id_detalle" HeaderText="id_detalle" ReadOnly="True" Visible="false" SortExpression="id_detalle" />
                                                        <asp:BoundField DataField="id_refaccion" HeaderText="id_refaccion" Visible="false" SortExpression="id_refaccion" />
                                                        <asp:BoundField DataField="cantidad" HeaderText="Cantidad" SortExpression="cantidad" />
                                                        <asp:BoundField DataField="descripcion" HeaderText="Refaccion" SortExpression="descripcion" />                                                       
                                                        <asp:BoundField DataField="costo_unitario" HeaderText="Costo Unitario" SortExpression="costo_unitario" DataFormatString="{0:C2}" />
                                                        <asp:BoundField DataField="porc_desc" HeaderText="% Desc." SortExpression="porc_desc" />
                                                        <asp:BoundField DataField="importe_desc" HeaderText="Desc." SortExpression="importe_desc" DataFormatString="{0:C2}"/>
                                                        <asp:BoundField DataField="importe" HeaderText="Importe" SortExpression="importe" DataFormatString="{0:C2}"/>
                                                        <asp:BoundField DataField="staDescripcion" HeaderText="Estatus" SortExpression="staDescripcion" />
                                                        <asp:BoundField DataField="observacion" HeaderText="Observacion" SortExpression="observacion" />
                                                        <asp:BoundField DataField="procedencia" HeaderText="Procedencia" SortExpression="procedencia" />
                                                        <asp:TemplateField HeaderText="Imagen">
                                                            <ItemTemplate>
                                                                <asp:DataList ID="DataListFotosRef" runat="server" RepeatColumns="3" RepeatDirection="Horizontal"
                                                                    DataKeyField="id_empresa" DataSourceID="SqlDsFotosRef">
                                                                    <ItemTemplate>                                                                
                                                                            <asp:ImageButton OnClick="Image1_Click" ID="Image1" runat="server" AlternateText='<%# Eval("nombre_imagen") %>'
                                                                                Width="40px" ImageUrl='<%# "~/ImgRefaccion.ashx?id="+Eval("id_empresa")+";"+Eval("id_taller")+";"+Eval("no_orden")+";"+Eval("id_refaccion")+";"+Eval("id_fotografia") %>' />                                       
                                                                    </ItemTemplate>
                                                                    <ItemStyle CssClass="ancho180px textoCentrado" />
                                                                </asp:DataList>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                                <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                                                    ConnectionString="<%$ ConnectionStrings:PVW %>" 
                                                    SelectCommand="select o.no_orden,o.id_empresa,o.id_taller,o.id_orden,o.id_detalle,o.id_refaccion,o.descripcion,o.cantidad,
                                                    o.costo_unitario,o.porc_desc,o.importe_desc,o.importe,r.refEstatusSolicitud,e.staDescripcion,r.observacion,p.proc_Descrip as procedencia
                                                    from Orden_Compra_Detalle o
                                                    inner join Refacciones_Orden r on r.refOrd_Id=o.id_refaccion and r.ref_id_empresa=o.id_empresa and r.ref_id_taller=o.id_taller and r.ref_no_orden=o.no_orden
                                                    inner join Rafacciones_Estatus e on e.staRefID=r.refEstatusSolicitud
                                                    left join cat_Procedencia p on p.id_Proc=o.id_procedencia
                                                    where o.no_orden=@orden and o.id_empresa=@empresa and o.id_taller=@taller and o.id_orden=@compra">
                                                    <SelectParameters>
                                                        <asp:QueryStringParameter Name="orden" QueryStringField="o" />
                                                        <asp:QueryStringParameter Name="empresa" QueryStringField="e" />
                                                        <asp:QueryStringParameter Name="taller" QueryStringField="t" />
                                                        <asp:QueryStringParameter Name="compra" QueryStringField="c" />
                                                    </SelectParameters>
                                                </asp:SqlDataSource>
                                                <asp:SqlDataSource runat="server" ID="SqlDsFotosRef" ConnectionString='<%$ ConnectionStrings:PVW %>'                            
                                                    SelectCommand="select id_empresa,id_taller,no_orden,id_refaccion,id_fotografia, nimbre_imagen as nombre_imagen from fotos_refacciones where id_empresa=@id_empresa and id_taller=@id_taller and no_orden=@no_orden and id_refaccion=@id_refaccion">                            
                                                    <SelectParameters>
                                                        <asp:QueryStringParameter Name="no_orden" QueryStringField="o" Type="Int32" />
                                                        <asp:QueryStringParameter Name="id_empresa" QueryStringField="e" Type="Int32" />
                                                        <asp:QueryStringParameter Name="id_taller" QueryStringField="t" Type="Int32" />
                                                        <asp:Parameter Name="id_refaccion" Type="Int32" DefaultValue="0" />
                                                    </SelectParameters>
                                                </asp:SqlDataSource>   
                                            </div>
                                        </asp:Panel>

                                        <asp:Panel ID="PanelMascara" runat="server" CssClass="mask zen2"></asp:Panel>
                                        <asp:Panel ID="PanelImgZoom" runat="server" CssClass="popUp zen3 textoCentrado ancho80 centrado">
                                            <table class="ancho100">
                                                <tr class="ancho100 centrado">
                                                    <td class="ancho80 text-center encabezadoTabla roundTopLeft ">
                                                        <asp:Label ID="Label9" runat="server" Text="Fotografía" CssClass="t22 colorMorado textoBold" />
                                                    </td>
                                                    <td class="ancho20 text-right encabezadoTabla roundTopRight">
                                                        <asp:LinkButton ID="btnCerrarImgZomm" runat="server" ToolTip="Cerrar" OnClick="btnCerrarImgZomm_Click"
                                                            CssClass="btn btn-danger alingMiddle"><i class="fa fa-remove t18"></i></asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                            <div class="row">
                                                <asp:Panel ID="Panel3" runat="server" CssClass="col-lg-12 col-sm-12 text-center ancho100 pnlImagen"
                                                    ScrollBars="Auto">
                                                    <asp:Image ID="imgZoom" runat="server" CssClass="ancho100" AlternateText="Imagen no disponible" />
                                                </asp:Panel>
                                            </div>
                                        </asp:Panel>
                                        <div class="col-lg-12 col-sm-12 text-center pad1m">
                                            <asp:Label ID="NOTA" runat="server" CssClass="alert">
                                                <B>LOS MONTOS REFLEJADOS EN LA PRESENTE SUBASTA DEBER&Aacute;N SER RESPETADOS AL MOMENTO DEL SURTIMIENTO</B>
                                            </asp:Label> 

                                            </div>


                                        <div class="col-lg-12 col-sm-12 text-center pad1m">
                                            <asp:LinkButton ID="lnkImprimeOrden" runat="server" CssClass="btn btn-info t14" onclick="lnkImprimeOrden_Click"><i class="fa fa-print"></i><span>&nbsp;Imprimir Orden de Compra</span></asp:LinkButton>
                                        </div>
                                    </div>
							    </div>
                                </div>
						    </div>
						    <!-- /PAGE HEADER -->
					    </div>
				    </div>
        </section>        
    </form>
</body>
</html>
