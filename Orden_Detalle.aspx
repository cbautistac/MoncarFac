<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Orden_Detalle.aspx.cs" Inherits="Orden_Detalle" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1"/> 
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>Ordenes</title>
    <link rel="Stylesheet" href="css/estilos.css" />
    <!--<link rel="Stylesheet" rev="Stylesheet" href="css/acordion.css" /> -->   
    <link rel="Stylesheet" href="css/style4.css" />
    <link rel="Stylesheet" href="css/style.css" />
    <link rel="Stylesheet" href="css/bootstrap.min.css" />
    <script src="js/domtab.js" type="text/javascript"></script>
    <link rel="icon" type="image/ico" href="img/ico_moncar.png" />
    <link rel="stylesheet" type="text/css" href="css/cloud-admin.css" />
    <link href="css/bootstrap-responsive.min.css" rel="stylesheet" />
    <link href="http://fonts.googleapis.com/css?family=Open+Sans:400italic,600italic,400,600"
        rel="stylesheet" />
    <link href="css/font-awesome.css" rel="stylesheet" />
    <link href="css/dashboard.css" rel="stylesheet" />
    <link rel="stylesheet" href="css/4.4.0/css/font-awesome.min.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="encabezado">
            <table class="ancho100">
                <tr>
                    <td class="ancho30 textoIzquierda negritas">
                        <asp:Label ID="lbltitulo" runat="server" Text="MoncarWeb" CssClass="titulo negritas colorMoncarAzul alineado" />&nbsp;
                        <asp:Label ID="lblEmpresa" runat="server" CssClass="colorMoncarAzul alineado" ></asp:Label>
                    </td>
                    <td class="ancho30 textoCentrado negritas">                    
                        <asp:Label ID="lblTallerSesion" runat="server" CssClass="colorMoncarRojo"></asp:Label><br />
                        <asp:Label ID="lblUser" runat="server" CssClass="colorGris"></asp:Label>
                    </td>
                    <td class="ancho30 textoDer alineado">
                        <a href="Default.aspx" class="font22 alineado"><i class="fa fa-sign-out"></i>Cerrar Sesi&oacute;n</a>&nbsp;&nbsp;&nbsp;
                        <asp:Image ID="Image1" AlternateText="e-Lockers" ImageUrl="~/IMG/logo.png" runat="server" Height="40px" CssClass="alineado" />
                    </td>
                </tr>
            </table>
        </div>
        <div class="subnavbar">
            <div class="subnavbar-inner">
                <div class="container">
                    <ul class="mainnav">
        
                        <li class="dropdown"><a href="javascript:;" class="dropdown-toggle" data-toggle="dropdown"><i class="fa fa-chevron-right"></i><i class="fa fa-car"></i><span>Recepci&oacute;n</span> <b class="caret"></b></a>
                        </li>
                        <li class="dropdown"><a href="javascript:;" class="dropdown-toggle" data-toggle="dropdown"> <i class="fa fa-car"></i><span>Datos del vehiculo</span> <b class="caret"></b></a>
                        </li>
                        <li><a href="index2.html"><i class="fa fa-exclamation-triangle"></i><span>Levantamiento de daños</span> </a> </li>
                        <li><a href="#"><i class="fa fa-check-square-o"></i><i class="fa fa-square-o"></i><span>Asignaci&oacute;n</span> </a></li>
                        <li><a href="#"><i class="fa fa-list"></i><i class="fa fa-check-circle-o"></i><span>Valuaci&oacute;n</span> </a> </li>
                        <li><a href="#"><i class="fa fa-cogs"></i><span>Refacciones</span> </a> </li>
                        <li class="dropdown"><a href="javascript:;" class="dropdown-toggle" data-toggle="dropdown"> <i class="fa fa-cubes"></i><span>Operaci&oacute;n</span> <b class="caret"></b></a>
                            <ul class="dropdown-menu">
                                <li><a href="#">Desarmado</a></li>
                                <li><a href="#">Mecanico</a></li>
                                <li><a href="#">Hojalateria</a></li>
                                <li><a href="#">Pintura</a></li>
                                <li><a href="#">Armado</a></li>
                                <li><a href="#">Valuaci&oacute;n</a></li>
                                <li><a href="producto.html">Terminado</a></li>
                            </ul> 
                        </li>
                        <li><a href="#"><i class="fa fa-car"></i><i class="fa fa-chevron-right"></i><span>Entrega</span> </a> </li>
                        <li><a href="#"><i class="fa fa-file"></i><i class="fa fa-qrcode"></i><span>Facturaci&oacute;n</span> </a> </li>
                    </ul>
                <!-- /subnavbar -->
                </div>
            <!-- /container --> 
            </div>
        <!-- /subnavbar-inner --> 
        </div>
        <!-- /footer --> 
    </form>
<script src="jquery.js"></script>
<script src="bootstrap-modal.js"></script>

<script src="js/jquery-1.7.2.min.js"></script> 
<script src="js/excanvas.min.js"></script> 
<script src="js/chart.min.js" type="text/javascript"></script> 
<script src="js/bootstrap.js"></script>
<script language="javascript" type="text/javascript" src="js/full-calendar/fullcalendar.min.js"></script>

<script type="text/javascript" src="https://cdn.datatables.net/r/bs-3.3.5/jqc-1.11.3,dt-1.10.8/datatables.min.js"></script>
    <script type="text/javascript" charset="utf-8">
      $(document).ready(function() {
        $('#example').DataTable();
      } );
    </script>
    <script type="text/javascript">
  // For demo to fit into DataTables site builder...
  $('#example')
    .removeClass( 'display' )
    .addClass('table table-striped table-bordered');
</script>
</body>
</html>
