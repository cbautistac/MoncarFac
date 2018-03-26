<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Descargas.aspx.cs" Inherits="Descargas" %>

<%@ Register Assembly="E-Utilities" Namespace="E_Utilities" TagPrefix="cc1" %>

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
    <form id="form1" runat="server" style="width:100%; height:100%; top:0; left:0; margin:0; padding:0;">
      <embed runat="server" id="pdf" src="archivo.pdf#toolbar=0&navpanes=0&scrollbar=0" style="width:100%; height:100%; top:0; left:0; margin:0; padding:0;"></embed>  
    <%--<cc1:ShowPdf ID="ShowPdf1" runat="server" CssClass="col-lg-12 col-sm-12"  style="width:100%; height:100%; top:0; left:0; margin:0; padding:0;" BorderStyle="Inset" BorderWidth="2px"/>  --%>
    </form>
</body>
</html>

