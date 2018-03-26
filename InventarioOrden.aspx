<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InventarioOrden.aspx.cs" Inherits="InventarioOrden" MasterPageFile="~/AdmonOrden.master" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">  
    <asp:ScriptManager ID="ScriptManager1" runat="server"/>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-lg-12 col-sm-12 text-center alert-info">
                    <h3>
                        <i class="fa fa-list"></i>&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="Label1" runat="server" Text="Inventario de Vehículo"></asp:Label>
                    </h3>                    
                </div>
            </div>
            <div class="row">
                <div class="col-lg-12 col-sm-12 text-center">                     
                    <asp:Label ID="lblError" runat="server" CssClass="errores" />      
                </div>
            </div>
            <asp:Panel runat="server" ID="pnlCronos" CssClass="paneles col-lg-12 col-sm-12" ScrollBars="Auto">
                <div class="row">
                   <div class="col-lg-6 col-sm-6 text-center">
                       <cc1:Accordion ID="inventarios" runat="server" CssClass="ancho95 centrado" 
                           FadeTransitions="true" HeaderCssClass="encabezadoAcordeonPanel" 
                           HeaderSelectedCssClass="encabezadoAcordeonPanelSelect" SelectedIndex="-1" RequireOpenedPane="false" >
                        <Panes>
                            <cc1:AccordionPane ID="acpIzquierdo" runat="server" CssClass="ancho95" style="cursor:pointer;" >
                                <Header>Lado Izquierdo &nbsp;&nbsp;<asp:Image ID="imgIzq" runat="server" /></Header>
                                <Content>              
                                    <asp:Panel ID="Panel1" runat="server"  CssClass="panelesInv col-lg-12 col-sm-12" ScrollBars="Auto">                                                                                 
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label3" runat="server" Text="Aletas" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkAletas" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtAletas" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtAletas_TextBoxWatermarkExtender" TargetControlID="txtAletas" ID="txtAletas_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label5" runat="server" Text="Antena" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkAntena" runat="server" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtAntena" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtAntena_TextBoxWatermarkExtender" TargetControlID="txtAntena" ID="txtAntena_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div> 
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label105" runat="server" Text="Costado" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkCostado" runat="server" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtCostado" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtCostado_TextBoxWatermarkExtender" TargetControlID="txtCostado" ID="txtCostado_TextBoxWatermarkExtender1" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>                    
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label7" runat="server" Text="Cristales Puerta Delantera" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkCristales_Puerta_Del" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtCristales_Puerta_Del" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtCristales_Puerta_Del_TextBoxWatermarkExtender" TargetControlID="txtCristales_Puerta_Del" ID="txtCristales_Puerta_Del_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label9" runat="server" Text="Cristales Puerta Trasera" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkCristales_Puerta_Tras" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtCristales_Puerta_Tras" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtCristales_Puerta_Tras_TextBoxWatermarkExtender" TargetControlID="txtCristales_Puerta_Tras" ID="txtCristales_Puerta_Tras_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label11" runat="server" Text="Espejos Exteriores" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkEspejos_Exteriores" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtEspejos_Exteriores" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtEspejos_Exteriores_TextBoxWatermarkExtender" TargetControlID="txtEspejos_Exteriores" ID="txtEspejos_Exteriores_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label13" runat="server" Text="Manijas Exteriores" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkManijas_Exteriores" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtManijas_Exteriores" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtManijas_Exteriores_TextBoxWatermarkExtender" TargetControlID="txtManijas_Exteriores" ID="txtManijas_Exteriores_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label14" runat="server" Text="Molduras" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkMolduras" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtMolduras" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtMolduras_TextBoxWatermarkExtender" TargetControlID="txtMolduras" ID="txtMolduras_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label15" runat="server" Text="Puerta Delantera" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkPuerta_Del" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtPuerta_Del" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtPuerta_Del_TextBoxWatermarkExtender" TargetControlID="txtPuerta_Del" ID="txtPuerta_Del_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label16" runat="server" Text="Puerta Trasera" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkPuerta_Tras" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtPuerta_Tras" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtPuerta_Tras_TextBoxWatermarkExtender" TargetControlID="txtPuerta_Tras" ID="txtPuerta_Tras_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label17" runat="server" Text="Reflejante Lateral Del." /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkReflejante_Lateral_Del" runat="server" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtReflejante_Lateral_Del" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtReflejante_Lateral_Del_TextBoxWatermarkExtender" TargetControlID="txtReflejante_Lateral_Del" ID="txtReflejante_Lateral_Del_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label18" runat="server" Text="Reflejante Lateral Tras." /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkReflejante_Lateral_Tras" runat="server" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtReflejante_Lateral_Tras" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtReflejante_Lateral_Tras_TextBoxWatermarkExtender" TargetControlID="txtReflejante_Lateral_Tras" ID="txtReflejante_Lateral_Tras_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label19" runat="server" Text="Salpicadera" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkSalpicadera" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtSalpicadera" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtSalpicadera_TextBoxWatermarkExtender" TargetControlID="txtSalpicadera" ID="txtSalpicadera_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label20" runat="server" Text="Tapones de Ruedas" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkTapones_Rueda" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtTapones_Rueda" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtTapones_Rueda_TextBoxWatermarkExtender" TargetControlID="txtTapones_Rueda" ID="txtTapones_Rueda_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    </asp:Panel>
                                    <div class="col-lg-12 col-sm-12 text-center pad1m">                                        
                                        <asp:LinkButton ID="btnGuardarIzq" runat="server" ToolTip="Guardar Lado Izquierdo" OnClick="btnGuardarIzq_Click"  CssClass="btn btn-success t14" ><i class="fa fa-save"></i>&nbsp;<span>Guardar</span></asp:LinkButton>
                                    </div>
                                </Content>
                            </cc1:AccordionPane>
                            <cc1:AccordionPane ID="acpDerecho" runat="server" CssClass="ancho95" style="cursor:pointer;">
                                <Header>Lado Derecho&nbsp;&nbsp;<asp:Image ID="imgDer" runat="server" /></Header>
                                <Content>
                                    <asp:Panel ID="Panel2" runat="server"  CssClass="panelesInv col-lg-12 col-sm-12" ScrollBars="Auto">                                                                                 
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label21" runat="server" Text="Aletas" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkAletasD" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtAletasD" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtAletasD_TextBoxWatermarkExtender" TargetControlID="txtAletasD" ID="txtAletasD_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label22" runat="server" Text="Antena" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkAntenaD" runat="server" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtAntenaD" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtAntenaD_TextBoxWatermarkExtender" TargetControlID="txtAntenaD" ID="txtAntenaD_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label106" runat="server" Text="Costado" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkCostadoD" runat="server" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtCostadoD" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtCostadoD_TextBoxWatermarkExtender" TargetControlID="txtCostadoD" ID="txtCostadoD_TextBoxWatermarkExtender1" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>                    
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label23" runat="server" Text="Cristales Puerta Delantera" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkCristales_Puerta_DelD" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtCristales_Puerta_DelD" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtCristales_Puerta_DelD_TextBoxWatermarkExtender" TargetControlID="txtCristales_Puerta_DelD" ID="txtCristales_Puerta_DelD_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label24" runat="server" Text="Cristales Puerta Trasera" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkCristales_Puerta_TrasD" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtCristales_Puerta_TrasD" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtCristales_Puerta_TrasD_TextBoxWatermarkExtender" TargetControlID="txtCristales_Puerta_TrasD" ID="txtCristales_Puerta_TrasD_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label25" runat="server" Text="Espejos Exteriores" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkEspejos_ExterioresD" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtEspejos_ExterioresD" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtEspejos_ExterioresD_TextBoxWatermarkExtender" TargetControlID="txtEspejos_ExterioresD" ID="txtEspejos_ExterioresD_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label26" runat="server" Text="Manijas Exteriores" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkManijas_ExterioresD" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtManijas_ExterioresD" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtManijas_ExterioresD_TextBoxWatermarkExtender" TargetControlID="txtManijas_ExterioresD" ID="txtManijas_ExterioresD_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label27" runat="server" Text="Molduras" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkMoldurasD" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtMoldurasD" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtMoldurasD_TextBoxWatermarkExtender" TargetControlID="txtMoldurasD" ID="txtMoldurasD_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label28" runat="server" Text="Puerta Delantera" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkPuerta_DelD" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtPuerta_DelD" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtPuerta_DelD_TextBoxWatermarkExtender" TargetControlID="txtPuerta_DelD" ID="txtPuerta_DelD_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label29" runat="server" Text="Puerta Trasera" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkPuerta_TrasD" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtPuerta_TrasD" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtPuerta_TrasD_TextBoxWatermarkExtender" TargetControlID="txtPuerta_TrasD" ID="txtPuerta_TrasD_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label30" runat="server" Text="Reflejante Lateral Del." /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkReflejante_Lateral_DelD" runat="server" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtReflejante_Lateral_DelD" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtReflejante_Lateral_DelD_TextBoxWatermarkExtender" TargetControlID="txtReflejante_Lateral_DelD" ID="txtReflejante_Lateral_DelD_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label31" runat="server" Text="Reflejante Lateral Tras." /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkReflejante_Lateral_TrasD" runat="server" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtReflejante_Lateral_TrasD" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtReflejante_Lateral_TrasD_TextBoxWatermarkExtender" TargetControlID="txtReflejante_Lateral_TrasD" ID="txtReflejante_Lateral_TrasD_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label32" runat="server" Text="Salpicadera" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkSalpicaderaD" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtSalpicaderaD" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtSalpicaderaD_TextBoxWatermarkExtender" TargetControlID="txtSalpicaderaD" ID="txtSalpicaderaD_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label33" runat="server" Text="Tapones de Ruedas" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkTapones_RuedaD" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtTapones_RuedaD" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtTapones_RuedaD_TextBoxWatermarkExtender" TargetControlID="txtTapones_RuedaD" ID="txtTapones_RuedaD_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>                                    
                                    </asp:Panel>
                                    <div class="col-lg-12 col-sm-12 text-center pad1m">                                        
                                        <asp:LinkButton ID="btnGuardarDer" runat="server" ToolTip="Guardar Lado Derecho" OnClick="btnGuardarDer_Click"  CssClass="btn btn-success t14" ><i class="fa fa-save"></i>&nbsp;<span>Guardar</span></asp:LinkButton>
                                    </div>
                                </Content>
                            </cc1:AccordionPane>
                            <cc1:AccordionPane ID="acpFrontal" runat="server" CssClass="ancho95" style="cursor:pointer;" >
                                <Header>Parte Frontal&nbsp;&nbsp;<asp:Image ID="imgFro" runat="server" /></Header>
                                <Content>
                                   <asp:Panel ID="Panel3" runat="server"  CssClass="panelesInv col-lg-12 col-sm-12" ScrollBars="Auto">                                                                                 
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label34" runat="server" Text="Biseles" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkBiseles" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtBiseles" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtBiseles_TextBoxWatermarkExtender" TargetControlID="txtBiseles" ID="txtBiseles_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label35" runat="server" Text="Brazos Limpiadores" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkBrazosLimpiadores" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtBrazosLimpiadores" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtBrazosLimpiadores_TextBoxWatermarkExtender" TargetControlID="txtBrazosLimpiadores" ID="txtBrazosLimpiadores_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label36" runat="server" Text="Cofre" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkCofre" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtCofre" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtCofre_TextBoxWatermarkExtender" TargetControlID="txtCofre" ID="txtCofre_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label37" runat="server" Text="Cuartos de Luz" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkCuartosLuz" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtCuartosLuz" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtCuartosLuz_TextBoxWatermarkExtender" TargetControlID="txtCuartosLuz" ID="txtCuartosLuz_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label38" runat="server" Text="Defensa Delantera" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkDefensaDelantera" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtDefensaDelantera" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtDefensaDelantera_TextBoxWatermarkExtender" TargetControlID="txtDefensaDelantera" ID="txtDefensaDelantera_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label39" runat="server" Text="Faros con Halógeno" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkFarosHalogeno" runat="server" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtFarosHalogeno" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtFarosHalogeno_TextBoxWatermarkExtender" TargetControlID="txtFarosHalogeno" ID="txtFarosHalogeno_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label40" runat="server" Text="Faros de Niebla" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkFarosNiebla" runat="server" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtFarosNiebla" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtFarosNiebla_TextBoxWatermarkExtender" TargetControlID="txtFarosNiebla" ID="txtFarosNiebla_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label41" runat="server" Text="Parabrisas" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkParabrisas" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtParabrisas" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtParabrisas_TextBoxWatermarkExtender" TargetControlID="txtParabrisas" ID="txtParabrisas_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label42" runat="server" Text="Parrilla" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkParrilla" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtParrilla" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtParrilla_TextBoxWatermarkExtender" TargetControlID="txtParrilla" ID="txtParrilla_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label43" runat="server" Text="Plumas Limpiadoras" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkPlumasLimpiadoras" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtPlumasLimpiadoras" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtPlumasLimpiadoras_TextBoxWatermarkExtender" TargetControlID="txtPlumasLimpiadoras" ID="txtPlumasLimpiadoras_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label44" runat="server" Text="Porta Placa" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkPortaPlaca" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtPortaPlaca" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtPortaPlaca_TextBoxWatermarkExtender" TargetControlID="txtPortaPlaca" ID="txtPortaPlaca_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label45" runat="server" Text="Spoiler" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkEspoiler" runat="server" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtEspoiler" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtEspoiler_TextBoxWatermarkExtender" TargetControlID="txtEspoiler" ID="txtEspoiler_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>             
                                    </asp:Panel>
                                    <div class="col-lg-12 col-sm-12 text-center pad1m">                                        
                                        <asp:LinkButton ID="btnGuardarFron" runat="server" ToolTip="Guardar Parte Frontal" OnClick="btnGuardarFron_Click"  CssClass="btn btn-success t14" ><i class="fa fa-save"></i>&nbsp;<span>Guardar</span></asp:LinkButton>
                                    </div>
                                </Content>
                            </cc1:AccordionPane>
                            <cc1:AccordionPane ID="acpPosterior" runat="server" CssClass="ancho95" style="cursor:pointer;">
                                <Header>Parte Posterior&nbsp;&nbsp;<asp:Image ID="imgPos" runat="server" /></Header>
                                <Content>
                                   <asp:Panel ID="Panel4" runat="server"  CssClass="panelesInv col-lg-12 col-sm-12" ScrollBars="Auto">                                                                                 
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label46" runat="server" Text="Calaveras" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkCalaveras" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtCalaveras" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtCalaveras_TextBoxWatermarkExtender" TargetControlID="txtCalaveras" ID="txtCalaveras_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label47" runat="server" Text="Cuartos" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkCuartos" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtCuartos" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtCuartos_TextBoxWatermarkExtender" TargetControlID="txtCuartos" ID="txtCuartos_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label48" runat="server" Text="Defensa Trasera" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkDefensaTrasera" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtDefensaTrasera" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtDefensaTrasera_TextBoxWatermarkExtender" TargetControlID="txtDefensaTrasera" ID="txtDefensaTrasera_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label49" runat="server" Text="Facia" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkFacia" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtFacia" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtFacia_TextBoxWatermarkExtender" TargetControlID="txtFacia" ID="txtFacia_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label50" runat="server" Text="Porta Placa" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkPortaPlacaP" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtPortaPlacaP" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtPortaPlacaP_TextBoxWatermarkExtender" TargetControlID="txtPortaPlacaP" ID="txtPortaPlacaP_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label51" runat="server" Text="Topes" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkTopes" runat="server" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtTopes" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtTopes_TextBoxWatermarkExtender" TargetControlID="txtTopes" ID="txtTopes_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label52" runat="server" Text="Limpiadores" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkLimpiadores" runat="server" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtLimpiadores" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtLimpiadores_TextBoxWatermarkExtender" TargetControlID="txtLimpiadores" ID="txtLimpiadores_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label53" runat="server" Text="Medallón" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkMedallon" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtMedallon" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtMedallon_TextBoxWatermarkExtender" TargetControlID="txtMedallon" ID="txtMedallon_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label54" runat="server" Text="Mica" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkMica" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtMica" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtMica_TextBoxWatermarkExtender" TargetControlID="txtMica" ID="txtMica_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label55" runat="server" Text="Sistema Escape" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkSistemaEscape" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtSistemaEscape" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtSistemaEscape_TextBoxWatermarkExtender" TargetControlID="txtSistemaEscape" ID="txtSistemaEscape_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label56" runat="server" Text="Spoiler" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkSpoiles" runat="server" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtSpoiles" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtSpoiles_TextBoxWatermarkExtender" TargetControlID="txtSpoiles" ID="txtSpoiles_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label57" runat="server" Text="Tapón de Gasolina" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkTaponGasolina" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtTaponGasolina" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtTaponGasolina_TextBoxWatermarkExtender" TargetControlID="txtTaponGasolina" ID="txtTaponGasolina_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label58" runat="server" Text="Luz Placa" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkLuzPlaca" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtLuzPlaca" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtLuzPlaca_TextBoxWatermarkExtender" TargetControlID="txtLuzPlaca" ID="txtLuzPlaca_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    </asp:Panel>
                                    <div class="col-lg-12 col-sm-12 text-center pad1m">                                        
                                        <asp:LinkButton ID="btnGuardaPos" runat="server" ToolTip="Guardar Parte Posterior" OnClick="btnGuardaPos_Click"  CssClass="btn btn-success t14" ><i class="fa fa-save"></i>&nbsp;<span>Guardar</span></asp:LinkButton>
                                    </div>
                                </Content>
                            </cc1:AccordionPane>
                            <cc1:AccordionPane ID="acpInterior" runat="server" CssClass="ancho95" style="cursor:pointer;">
                                <Header>Interior&nbsp;&nbsp;<asp:Image ID="imgInt" runat="server" /></Header>
                                <Content>
                                   <asp:Panel ID="Panel5" runat="server"  CssClass="panelesInv col-lg-12 col-sm-12" ScrollBars="Auto">                                                                                 
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label79" runat="server" Text="Alfombra" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkAlfombra" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtAlfombra" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtAlfombra_TextBoxWatermarkExtender" TargetControlID="txtAlfombra" ID="txtAlfombra_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label81" runat="server" Text="Asientos Delanteros" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkAsientosDelanteros" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtAsientosDelanteros" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtAsientosDelanteros_TextBoxWatermarkExtender" TargetControlID="txtAsientosDelanteros" ID="txtAsientosDelanteros_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label83" runat="server" Text="Asientos Traseros" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkAsientosTraseros" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtAsientosTraseros" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtAsientosTraseros_TextBoxWatermarkExtender" TargetControlID="txtAsientosTraseros" ID="txtAsientosTraseros_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label85" runat="server" Text="Radio Estéreo Agencia" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkRadioEstereoAgencia" runat="server" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtRadioEstereoAgencia" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtRadioEstereoAgencia_TextBoxWatermarkExtender" TargetControlID="txtRadioEstereoAgencia" ID="txtRadioEstereoAgencia_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label87" runat="server" Text="Bocinas" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkBocinas" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtBocinas" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtBocinas_TextBoxWatermarkExtender" TargetControlID="txtBocinas" ID="txtBocinas_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label89" runat="server" Text="Estéreo" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkEsterero" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtEsterero" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtEsterero_TextBoxWatermarkExtender" TargetControlID="txtEsterero" ID="txtEsterero_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label91" runat="server" Text="Botones de Puerta" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkBotonesPuerta" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtBotonesPuerta" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtBotonesPuerta_TextBoxWatermarkExtender" TargetControlID="txtBotonesPuerta" ID="txtBotonesPuerta_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label93" runat="server" Text="Botones Radio-Autoest." /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkBotonesRadioAutoestereo" runat="server" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtBotonesRadioAutoestereo" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtBotonesRadioAutoestereo_TextBoxWatermarkExtender" TargetControlID="txtBotonesRadioAutoestereo" ID="txtBotonesRadioAutoestereo_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label95" runat="server" Text="Cabeceras" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkCabeceras" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtCabeceras" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtCabeceras_TextBoxWatermarkExtender" TargetControlID="txtCabeceras" ID="txtCabeceras_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label97" runat="server" Text="Cajuela de Guantes" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkCajuelaGuantes" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtCajuelaGuantes" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtCajuelaGuantes_TextBoxWatermarkExtender" TargetControlID="txtCajuelaGuantes" ID="txtCajuelaGuantes_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label99" runat="server" Text="Cenicero" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkCenicero" runat="server" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtCenicero" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtCenicero_TextBoxWatermarkExtender" TargetControlID="txtCenicero" ID="txtCenicero_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label101" runat="server" Text="Cinturones de Seguridad" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkCinturonesSeguridad" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtCinturonesSeguridad" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtCinturonesSeguridad_TextBoxWatermarkExtender" TargetControlID="txtCinturonesSeguridad" ID="txtCinturonesSeguridad_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label103" runat="server" Text="Coderas" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkCoderas" runat="server" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtCoderas" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtCoderas_TextBoxWatermarkExtender" TargetControlID="txtCoderas" ID="txtCoderas_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label80" runat="server" Text="Consola" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkConsola" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtConsola" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtConsola_TextBoxWatermarkExtender" TargetControlID="txtConsola" ID="txtConsola_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>                                    
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label82" runat="server" Text="Control Eléctrico de Elv." /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkControlElectricoElev" runat="server" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtControlElectricoElev" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtControlElectricoElev_TextBoxWatermarkExtender" TargetControlID="txtControlElectricoElev" ID="txtControlElectricoElev_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>                                    
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label84" runat="server" Text="Encendedor" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkEncendedor" runat="server" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtEncendedor" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtEncendedor_TextBoxWatermarkExtender" TargetControlID="txtEncendedor" ID="txtEncendedor_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>                                    
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label86" runat="server" Text="Espejo Interior" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkEspejoInt" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtEspejoInt" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtEspejoInt_TextBoxWatermarkExtender" TargetControlID="txtEspejoInt" ID="txtEspejoInt_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>                                    
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label88" runat="server" Text="Luz Interior" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkLuzInterioir" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtLuzInterioir" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtLuzInterioir_TextBoxWatermarkExtender" TargetControlID="txtLuzInterioir" ID="txtLuzInterioir_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>                                    
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label90" runat="server" Text="Manijas Interiores" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkManijasInteriores" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtManijasInteriores" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtManijasInteriores_TextBoxWatermarkExtender" TargetControlID="txtManijasInteriores" ID="txtManijasInteriores_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>                                    
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label92" runat="server" Text="Palanca de Velocidades" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkPalancaVelocidades" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtPalancaVelocidades" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtPalancaVelocidades_TextBoxWatermarkExtender" TargetControlID="txtPalancaVelocidades" ID="txtPalancaVelocidades_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>                                    
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label94" runat="server" Text="Perilla Palanca" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkPerillaPalanca" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtPerillaPalanca" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtPerillaPalanca_TextBoxWatermarkExtender" TargetControlID="txtPerillaPalanca" ID="txtPerillaPalanca_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>                                    
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label96" runat="server" Text="Reloj" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkReloj" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtReloj" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtReloj_TextBoxWatermarkExtender" TargetControlID="txtReloj" ID="txtReloj_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>                                    
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label98" runat="server" Text="Tablero" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkTablero" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtTablero" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtTablero_TextBoxWatermarkExtender" TargetControlID="txtTablero" ID="txtTablero_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>                                    
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label100" runat="server" Text="Viceras" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkViceras" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtViceras" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtViceras_TextBoxWatermarkExtender" TargetControlID="txtViceras" ID="txtViceras_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>                                    
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label102" runat="server" Text="Tapetes" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkTapetesInt" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtTapetesInt" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtTapetesInt_TextBoxWatermarkExtender" TargetControlID="txtTapetesInt" ID="txtTapetesInt_TextBoxWatermarkExtender1" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>                                    
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label104" runat="server" Text="Cielo de Toldo" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkCieloToldo" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtCieloToldo" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtCieloToldo_TextBoxWatermarkExtender" TargetControlID="txtCieloToldo" ID="txtCieloToldo_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    </asp:Panel>
                                    <div class="col-lg-12 col-sm-12 text-center pad1m">                                        
                                        <asp:LinkButton ID="btnGuardaInt" runat="server" ToolTip="Guardar Interior" OnClick="btnGuardarInt_Click"  CssClass="btn btn-success t14" ><i class="fa fa-save"></i>&nbsp;<span>Guardar</span></asp:LinkButton>
                                    </div>
                                </Content>
                            </cc1:AccordionPane>
                            <cc1:AccordionPane ID="acpCajuela" runat="server" CssClass="ancho95" style="cursor:pointer;">
                                <Header>Cajuela&nbsp;&nbsp;<asp:Image ID="imgCaj" runat="server" /></Header>
                                <Content>
                                   <asp:Panel ID="Panel6" runat="server"  CssClass="panelesInv col-lg-12 col-sm-12" ScrollBars="Auto">                                                                                 
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label59" runat="server" Text="Cables para Corriente" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkCables_Corriente" runat="server" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtCables_Corriente" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtCables_Corriente_TextBoxWatermarkExtender" TargetControlID="txtCables_Corriente" ID="txtCables_Corriente_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label60" runat="server" Text="Llantas de Refacción" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkLlanta_Refaccion" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtLlanta_Refaccion" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtLlanta_Refaccion_TextBoxWatermarkExtender" TargetControlID="txtLlanta_Refaccion" ID="txtLlanta_Refaccion_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label61" runat="server" Text="Gato Hidráulico" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkGato" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtGato" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtGato_TextBoxWatermarkExtender" TargetControlID="txtGato" ID="txtGato_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label62" runat="server" Text="Herramientas" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkHerramientas" runat="server" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtHerramientas" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtHerramientas_TextBoxWatermarkExtender" TargetControlID="txtHerramientas" ID="txtHerramientas_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label63" runat="server" Text="Llave de Rueda" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkLave_Rueda" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtLave_Rueda" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtLave_Rueda_TextBoxWatermarkExtender" TargetControlID="txtLave_Rueda" ID="txtLave_Rueda_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label64" runat="server" Text="Señales de Carretera" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkSeñales_Carretera" runat="server" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtSeñales_Carretera" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtSeñales_Carretera_TextBoxWatermarkExtender" TargetControlID="txtSeñales_Carretera" ID="txtSeñales_Carretera_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label65" runat="server" Text="Tapetes" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkTapetes" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtTapetes" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtTapetes_TextBoxWatermarkExtender" TargetControlID="txtTapetes" ID="txtTapetes_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label66" runat="server" Text="Tapa Cartón" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkTapa_Carton" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtTapa_Carton" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtTapa_Carton_TextBoxWatermarkExtender" TargetControlID="txtTapa_Carton" ID="txtTapa_Carton_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label67" runat="server" Text="Extinguidor" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkExtinguidor" runat="server" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtExtinguidor" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtExtinguidor_TextBoxWatermarkExtender" TargetControlID="txtExtinguidor" ID="txtExtinguidor_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    </asp:Panel>
                                    <div class="col-lg-12 col-sm-12 text-center pad1m">                                        
                                        <asp:LinkButton ID="btnGuardaCajuela" runat="server" ToolTip="Guardar Cajuela" OnClick="btnGuardaCajuela_Click"  CssClass="btn btn-success t14" ><i class="fa fa-save"></i>&nbsp;<span>Guardar</span></asp:LinkButton>
                                    </div>
                                </Content>
                            </cc1:AccordionPane>
                            <cc1:AccordionPane ID="acpGenerales" runat="server" CssClass="ancho95" style="cursor:pointer;">
                                <Header>Generales&nbsp;&nbsp;<asp:Image ID="imgGen" runat="server" /></Header>
                                <Content>
                                   <asp:Panel ID="Panel7" runat="server"  CssClass="panelesInv col-lg-12 col-sm-12" ScrollBars="Auto">                                                                                 
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label68" runat="server" Text="Llaves" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkLlaves" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtLlaves" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtLlaves_TextBoxWatermarkExtender" TargetControlID="txtLlaves" ID="txtLlaves_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label69" runat="server" Text="Canastilla" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkCanastilla" runat="server" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtCanastilla" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtCanastilla_TextBoxWatermarkExtender" TargetControlID="txtCanastilla" ID="txtCanastilla_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label70" runat="server" Text="Emblemas" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkEmblema" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtEmblema" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtEmblema_TextBoxWatermarkExtender" TargetControlID="txtEmblema" ID="txtEmblema_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label71" runat="server" Text="Batería" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkBateria" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtBateria" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtBateria_TextBoxWatermarkExtender" TargetControlID="txtBateria" ID="txtBateria_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label72" runat="server" Text="Compac Disc" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkCompacDisc" runat="server" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtCompacDisc" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtCompacDisc_TextBoxWatermarkExtender" TargetControlID="txtCompacDisc" ID="txtCompacDisc_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label73" runat="server" Text="Ecualizador" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkEcualizador" runat="server" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtEcualizador" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtEcualizador_TextBoxWatermarkExtender" TargetControlID="txtEcualizador" ID="txtEcualizador_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label74" runat="server" Text="Rines" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"><asp:CheckBox ID="chkRines" runat="server" Checked="true" /></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtRines" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtRines_TextBoxWatermarkExtender" TargetControlID="txtRines" ID="txtRines_TextBoxWatermarkExtender" WatermarkText="Daños" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label75" runat="server" Text="Llantas" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtLlantas" runat="server" MaxLength="6" CssClass="input-small" /><asp:Label
                                                ID="Label114" runat="server" Text="% Vida"></asp:Label>
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtLlantas_TextBoxWatermarkExtender" TargetControlID="txtLlantas" ID="txtLlantas_TextBoxWatermarkExtender" WatermarkText="Llantas % Vida" WatermarkCssClass="input-small water" />
                                            <cc1:FilteredTextBoxExtender ID="txtLlantas_FilteredTextBoxExtender" runat="server" BehaviorID="txtLlantas_FilteredTextBoxExtender" TargetControlID="txtLlantas" FilterType="Numbers, Custom" ValidChars="." />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label76" runat="server" Text="Marca" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtMarca" runat="server" MaxLength="50" CssClass="input-large" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtMarca_TextBoxWatermarkExtender" TargetControlID="txtMarca" ID="txtMarca_TextBoxWatermarkExtender" WatermarkText="Marca" WatermarkCssClass="input-large water" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label77" runat="server" Text="Gasolina" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:DropDownList ID="ddlGasolina" runat="server" DataSourceID="SqlDataSourceGas" DataTextField="descripcion" DataValueField="id_med_gas" CssClass="input-large"></asp:DropDownList>
                                            <asp:SqlDataSource runat="server" ID="SqlDataSourceGas" ConnectionString='<%$ ConnectionStrings:PVW %>' SelectCommand="select '' as id_med_gas, '' as descripcion union all SELECT [id_med_gas], [descripcion] FROM [Medidas_Gasolina]"></asp:SqlDataSource>
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="col-lg-4 col-sm-12 text-left"><asp:Label ID="Label78" runat="server" Text="Observaciones" /></div>
                                        <div class="col-lg-2 col-sm-2 text-center"></div>
                                        <div class="col-lg-6 col-sm-6 text-left">
                                            <asp:TextBox ID="txtObservaciones" runat="server" MaxLength="200" Rows="10" TextMode="MultiLine" CssClass="textNota" />
                                            <cc1:TextBoxWatermarkExtender runat="server" BehaviorID="txtObservaciones_TextBoxWatermarkExtender" TargetControlID="txtObservaciones" ID="txtObservaciones_TextBoxWatermarkExtender" WatermarkText="Observaciones" WatermarkCssClass="textNota water" />
                                        </div>
                                    </div>
                                    </asp:Panel>
                                    <div class="col-lg-12 col-sm-12 text-center pad1m">                                        
                                        <asp:LinkButton ID="btnGuardaGenerales" runat="server" ToolTip="Guardar Generales" OnClick="btnGuardaGenerales_Click"  CssClass="btn btn-success t14" ><i class="fa fa-save"></i>&nbsp;<span>Guardar</span></asp:LinkButton>
                                    </div>
                                </Content>
                            </cc1:AccordionPane>                            
                        </Panes>
                    </cc1:Accordion>
                   </div>
                   <div class="col-lg-6 col-sm-6 text-center">
                        <div class="col-lg-12 col-sm-12 text-center ">
                            <asp:Label ID="lblImagenUp" runat="server" Text="Seleccione Imagen" CssClass="alingMiddle textoBold"></asp:Label>
                            <telerik:RadSkinManager ID="RadSkinManager1" runat="server"></telerik:RadSkinManager>                             
                            <telerik:RadAsyncUpload RenderMode="Lightweight" runat="server" Culture="es-Mx" CssClass="async-attachment" MaxFileInputsCount="10" MultipleFileSelection="Automatic"
                                ID="AsyncUpload1" HideFileInput="true" AllowedFileExtensions=".jpeg,.jpg,.png,.gif,.bmp,.tiff,.JPEG,.JPG,.PNG,.GIF,.BMP,.TIFF">
                            </telerik:RadAsyncUpload>                            
                            <asp:LinkButton ID="btnAddFoto" runat="server" ToolTip="Agregar Foto" OnClick="btnAddFoto_Click" CssClass="alingMiddle btn btn-info t14"><i class="fa fa-plus-circle"></i>&nbsp;<span>Agregar</span></asp:LinkButton>
                        </div>
                        <div class="col-lg-12 col-sm-12 text-center">
                        
                            <asp:DataList ID="DataListFotosDanos" runat="server" RepeatColumns="3" RepeatDirection="Horizontal" 
                                DataKeyField="id_empresa" DataSourceID="SqlDataSourceFotosDanos" OnItemCommand="DataListFotosDanos_ItemCommand" >
                                <ItemTemplate>
                                    <asp:Label ID="id_empresaLabel" runat="server" Text='<%# Eval("id_empresa") %>' Visible="false" />
                                    <asp:Label ID="id_tallerLabel" runat="server" Text='<%# Eval("id_taller") %>' Visible="false" />
                                    <asp:Label ID="no_ordenLabel" runat="server" Text='<%# Eval("no_orden") %>' Visible="false" />
                                    <asp:Label ID="consecutivoLabel" runat="server" Text='<%# Eval("consecutivo") %>' Visible="false" />
                                    <asp:Label ID="procesoLabel" runat="server" Text='<%# Eval("proceso") %>' Visible="false" />
                                    <asp:Label ID="nombre_imagenLabel" runat="server" Text='<%# Eval("nombre_imagen") %>' Visible="false" />
                                    <asp:Label ID="rutaLabel" runat="server" Text='<%# Eval("ruta") %>' Visible="false" />
                                    <br />
                                    <asp:LinkButton ID="btnLogo" runat="server" ToolTip='<%# Eval("nombre_imagen") %>' CommandName="zoom" CommandArgument='<%# Eval("id_empresa")+";"+Eval("id_taller")+";"+Eval("no_orden")+";"+Eval("consecutivo")+";"+Eval("proceso") %>'>
                                        <asp:Image ID="Image1" runat="server" AlternateText='<%# Eval("nombre_imagen") %>' Width="120px" ImageUrl='<%# "~/ImgEmpresas.ashx?id="+Eval("id_empresa")+";"+Eval("id_taller")+";"+Eval("no_orden")+";"+Eval("consecutivo")+";"+Eval("proceso") %>' />
                                    </asp:LinkButton>                                    
                                    <br />
                                    <asp:LinkButton ID="btnEliminaFotoDanos" runat="server" CommandName="elimina" ToolTip="Eliminar" CommandArgument='<%# Eval("id_empresa")+";"+Eval("id_taller")+";"+Eval("no_orden")+";"+Eval("consecutivo")+";"+Eval("proceso") %>' OnClientClick="return confirm('¿Esta seguro de eliminar la fotografía?');" CssClass="btn btn-danger t14"><i class="fa fa-trash"></i></asp:LinkButton>                                    
                                </ItemTemplate>
                                <ItemStyle CssClass="ancho180px textoCentrado" />                                                    
                            </asp:DataList>
                            <asp:SqlDataSource runat="server" ID="SqlDataSourceFotosDanos" ConnectionString='<%$ ConnectionStrings:PVW %>'                                 
                                DeleteCommand="delete from Fotografias_Orden where id_empresa=@id_empresa and id_taller=@id_taller and no_orden=@no_orden and consecutivo=@consecutivo and proceso=1" 
                                SelectCommand="select id_empresa,id_taller,no_orden,consecutivo,proceso,nombre_imagen,ruta from Fotografias_Orden where id_empresa=@id_empresa and id_taller=@id_taller and no_orden=@no_orden and proceso=1">
                                <DeleteParameters>
                                    <asp:Parameter Name="id_empresa"></asp:Parameter>
                                    <asp:Parameter Name="id_taller"></asp:Parameter>
                                    <asp:Parameter Name="no_orden"></asp:Parameter>
                                    <asp:Parameter Name="consecutivo"></asp:Parameter>                                    
                                </DeleteParameters>
                                <SelectParameters>
                                    <asp:QueryStringParameter Name="no_orden" QueryStringField="o" Type="Int32" />
                                    <asp:QueryStringParameter Name="id_empresa" QueryStringField="e" Type="Int32" />
                                    <asp:QueryStringParameter Name="id_taller" QueryStringField="t" Type="Int32" />                                                                        
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <!-- Zoom de imagenes -->
                            <asp:Panel ID="PanelMascara" runat="server" CssClass="mask zen2" ></asp:Panel>
                            <asp:Panel ID="PanelImgZoom" runat="server" CssClass="popUp zen3 textoCentrado ancho80 centrado" ScrollBars="Auto" >
                                <table class="ancho100">
                                    <tr class="ancho100 centrado">
                                        <td class="ancho95 text-center encabezadoTabla roundTopLeft ">
                                            <asp:Label ID="Label107" runat="server" Text="Fotografía" CssClass="t22 colorMorado textoBold" />                                            
                                        </td>
                                        <td class="ancho5 text-right encabezadoTabla roundTopRight">
                                            <asp:LinkButton ID="btnCerrarImgZomm" runat="server" ToolTip="Cerrar" OnClick="btnCerrarImgZomm_Click"  CssClass="btn btn-danger alingMiddle"><i class="fa fa-remove t18"></i></asp:LinkButton>                            
                                        </td>
                                    </tr>                        
                                </table>
                                <div class="row">
                                    <asp:Panel ID="Panel8" runat="server" CssClass="col-lg-12 col-sm-12 text-center ancho100 pnlImagen" ScrollBars="Auto">
                                        <asp:Image ID="imgZoom" runat="server" CssClass="ancho100" AlternateText="Imagen no disponible" />
                                    </asp:Panel>
                                </div>                                
                            </asp:Panel>                          
                        </div>
                   </div>
                </div>
            </asp:Panel>           

    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                <ProgressTemplate>
                    <asp:Panel ID="pnlMaskLoad21" runat="server" CssClass="maskLoad"></asp:Panel>
                    <asp:Panel ID="pnlCargando21" runat="server" CssClass="pnlPopUpLoad">
                        <asp:Image ID="imgLoad21" runat="server" ImageUrl="~/img/loading.gif" CssClass="ancho100" />                            
                    </asp:Panel>
                </ProgressTemplate>                            
            </asp:UpdateProgress>
        </ContentTemplate>
    </asp:UpdatePanel>


    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
             <div class="row pad1m">                       
                 <div class="col-lg-3 col-sm-3 text-center">
                    <asp:Label ID="Label113" runat="server" Text="Correo Electrónico:"></asp:Label>
                    <asp:TextBox ID="txtCorreo" runat="server" CssClass="input-large" ValidationGroup="envio"></asp:TextBox>
                    <asp:LinkButton ID="lnkEnviarCorreo" runat="server" Visible="false" ToolTip="Enviar por Correo" CssClass="btn btn-info t14" onclick="lnkEnviarCorreo_Click" ValidationGroup="envio" ><i class="fa fa-envelope"></i><span>&nbsp;Enviar Correo</span></asp:LinkButton>
                    <cc1:TextBoxWatermarkExtender ID="txtCorreo_TextBoxWatermarkExtender" runat="server" BehaviorID="txtCorreo_TextBoxWatermarkExtender" TargetControlID="txtCorreo" WatermarkCssClass="water input-large" WatermarkText="Correo Electrónico" /><br />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Debe indicar el Correo Electrónico al cual se enviará la información" CssClass="errores" ValidationGroup="imprime" ControlToValidate="txtCorreo"></asp:RequiredFieldValidator>
                 </div>                 
                <div class="col-lg-3 col-sm-3 text-center">
                    <asp:LinkButton ID="lnkImprimirInv" ValidationGroup="imprime" runat="server" ToolTip="Imprimir Inventario" CssClass="btn btn-info t14" onclick="lnkImprimirInv_Click"  ><i class="fa fa-print"></i><span>&nbsp;Imprimir Inventario</span></asp:LinkButton>
                </div>                                                     
                <div class="col-lg-9 col-sm-9 text-center">                    
                </div>                 
            </div>

            <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="UpdatePanel2">
                <ProgressTemplate>
                    <asp:Panel ID="pnlMaskLoad11" runat="server" CssClass="maskLoad"></asp:Panel>
                    <asp:Panel ID="pnlCargando11" runat="server" CssClass="pnlPopUpLoad">
                        <asp:Image ID="imgLoad11" runat="server" ImageUrl="~/img/loading.gif" CssClass="ancho100" />                            
                    </asp:Panel>
                </ProgressTemplate>                            
            </asp:UpdateProgress>
        </ContentTemplate>
    </asp:UpdatePanel>


            <div class="pie pad1m">		                                		                                
		        <div class="clearfix">
			        <div class="row colorBlanco textoBold">
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label2" runat="server" Text="Tipo Orden:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
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
                            <asp:Label ID="ddlPerfil" runat="server" ></asp:Label>
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
