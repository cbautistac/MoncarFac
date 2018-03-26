<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DetalleOrdenCompra.aspx.cs" Inherits="DetalleOrdenCompra" MasterPageFile="~/AdmonOrden.master" Culture="es-Mx" UICulture="es-Mx"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">  
        <ContentTemplate>
            <div class="row">
                <div class="col-lg-12 col-sm-12 text-center alert-info">
                    <h3>
                        <i class="fa fa-list"></i></i>&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblTit" runat="server" Text="Orden de Compra"></asp:Label>&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkCancelar" runat="server" CssClass="btn btn-danger t14" 
                            onclick="lnkCancelar_Click"><i class="fa fa-ban"></i><span>&nbsp;Salir</span></asp:LinkButton>                    
                    </h3>
                </div>
            </div>
             <div class="row">
                <div class="col-lg-12 col-sm-12 text-center">                                       
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label1" runat="server" Text="Fecha y Hora de recepción:"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:TextBox ID="txtFechaRecepcion" runat="server" CssClass="input-small" MaxLength="10" Enabled="false"></asp:TextBox>
                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" BehaviorID="txtFechaRecepcion_CalendarExtender" TargetControlID="txtFechaRecepcion" Format="yyyy-MM-dd" PopupButtonID="lnkFechaRecepcion" />
                        <asp:LinkButton ID="lnkFechaRecepcion" runat="server" CssClass="btn btn-info t14"><i class="fa fa-calendar"></i></asp:LinkButton>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Debe indicar una fecha de entrega" ValidationGroup="acepta" CssClass="errores" Text="*" ControlToValidate="txtFechaRecepcion" ></asp:RequiredFieldValidator>
                        <asp:TextBox ID="txtHoraPact" runat="server" MaxLength="8" CssClass="alingMiddle input-small"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="TtxtHoraPactWatermarkExtender1" runat="server" BehaviorID="txtHoraPact_TextBoxWatermarkExtender" TargetControlID="txtHoraPact" WatermarkCssClass="water input-small" WatermarkText="hh:mm:ss" />
                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" BehaviorID="txtHoraPact_FilteredTextBoxExtender" TargetControlID="txtHoraPact" FilterType="Numbers,Custom" ValidChars=":" />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Debe indicar una hora válida" CssClass="errores" ControlToValidate="txtHoraPact" Text="*" ValidationExpression="(([0-1][0-9])|([2][0-3])):([0-5][0-9]):([0-5][0-9])" ValidationGroup="acepta"></asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Debe indicar una hora de entrega" ValidationGroup="acepta" CssClass="errores" Text="*" ControlToValidate="txtHoraPact" ></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label3" runat="server" Text="Nombre de quien Entrega:"></asp:Label></div>
                    <div class="col-lg-4 col-sm-4 text-left">
                        <asp:TextBox ID="txtNombreEntrega" runat="server" CssClass="input-large" MaxLength="300" ValidationGroup="acepta"></asp:TextBox>                        
                        <cc1:TextBoxWatermarkExtender ID="txtNombreEntrega_TextBoxWatermarkExtender" runat="server" BehaviorID="txtNombreEntrega_TextBoxWatermarkExtender" TargetControlID="txtNombreEntrega" WatermarkText="Nombre Entrega" WatermarkCssClass="water input-large" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Debe indicar el nombre de la persona que le esta entregando las refacciones" CssClass="errores" Text="*" ValidationGroup="acepta" ControlToValidate="txtNombreEntrega"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="row marTop">
                <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label13" runat="server" Text="Factura:"></asp:Label></div>
                <div class="col-lg-4 col-sm-4 text-left">
                    <asp:TextBox ID="txtFactura" runat="server" CssClass="input-large" MaxLength="50" ValidationGroup="acepta"></asp:TextBox>                        
                    <cc1:TextBoxWatermarkExtender ID="txtFacturaWatermarkExtender1" runat="server" BehaviorID="txtFactura_TextBoxWatermarkExtender" TargetControlID="txtFactura" WatermarkText="Factura" WatermarkCssClass="water input-large" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Debe indicar el folio de la factura" CssClass="errores" Text="*" ValidationGroup="acepta" ControlToValidate="txtFactura"></asp:RequiredFieldValidator>
                </div>
                <div class="col-lg-1 col-sm-1 text-left">
                         <asp:LinkButton ID="lnkAceptar" runat="server" CssClass="btn btn-success t14" 
                             onclick="lnkAceptar_Click" ValidationGroup="acepta" ><i class="fa fa-check"></i><span>&nbsp;Aceptar</span></asp:LinkButton>  
                    </div>
            </div>
            <div class="row">
                <div class="col-lg-12 col-sm-12 text-center">                                       
                    <asp:Label ID="lblError" runat="server" CssClass="errores" />
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="errores" DisplayMode="List" ValidationGroup="acepta" />
                </div>
            </div>

            <asp:Panel runat="server" ID="Panel1" CssClass="paneles col-lg-12 col-sm-12" ScrollBars="Auto">
                
                <asp:Panel ID="pnlRefacciones" runat="server" CssClass="panelCatalogos text-center" ScrollBars="Auto">
                    <div class="table-responsive">
                        <asp:GridView ID="grdRefacciones" runat="server" AutoGenerateColumns="False" 
                            DataSourceID="SqlDataSource1" CssClass="table table-bordered" 
                            onrowdatabound="grdRefacciones_RowDataBound" 
                            onrowcommand="grdRefacciones_RowCommand" DataKeyNames="estatusPre">
                            <Columns>
                                <asp:TemplateField HeaderText="N0."  SortExpression="id_detalle" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("id_detalle") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblRef" runat="server" Text='<%# Eval("id_detalle") %>'></asp:Label>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="cantidad" HeaderText="Cant." SortExpression="cantidad" ReadOnly="True"/>

                                <asp:TemplateField HeaderText="Refacción"  SortExpression="descripcion">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRefaccion" runat="server" Text='<%# Bind("descripcion") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblRefaccionEd" runat="server" Text='<%# Bind("descripcion") %>'></asp:Label>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="costo_unitario" HeaderText="C. U." SortExpression="costo_unitario" ReadOnly="True" DataFormatString="{0:C2}"/>
                                <asp:BoundField DataField="porc_desc" HeaderText="% Desc." ReadOnly="True" SortExpression="porc_desc" />
                                <asp:BoundField DataField="importe_desc" HeaderText="Imp. Desc." SortExpression="importe_desc" ReadOnly="True" DataFormatString="{0:C2}"/>
                                <asp:BoundField DataField="importe" HeaderText="Importe" SortExpression="importe" ReadOnly="True" DataFormatString="{0:C2}"/>
                                <asp:BoundField DataField="estatusPre" HeaderText="Estatus" ReadOnly="True" SortExpression="estatusPre" />
                                <asp:TemplateField HeaderText="Estatus Refacción" SortExpression="estatusRef">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEstatus" runat="server" Text='<%# Bind("descripEstatus") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblEstatus" runat="server" Text='<%# Bind("descripEstatus") %>'></asp:Label>
                                        <asp:Label ID="lblPPEstatus" Visible="false" runat="server" Text='<%# Bind("estatusRef") %>'></asp:Label>
                                        <asp:DropDownList ID="ddlEstatus" runat="server" 
                                            SelectedValue='<%# Bind("estatusRef") %>' DataSourceID="SqlDataSource2" 
                                            DataTextField="stadescripcion" DataValueField="starefid" AutoPostBack="True" 
                                            onselectedindexchanged="ddlEstatus_SelectedIndexChanged"></asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select starefid,stadescripcion from rafacciones_estatus"></asp:SqlDataSource>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Observación"  SortExpression="observacion">
                                    <ItemTemplate>
                                        <asp:Label ID="lblObservacion" runat="server" Text='<%# Bind("observacion") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtObservaciones" runat="server" MaxLength="500" CssClass="textNota" TextMode="MultiLine" Rows="5"  Text='<%# Bind("observacion") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                               
                          
                                <asp:BoundField DataField="fSolicitud" HeaderText="Solicitud" ReadOnly="True" SortExpression="fSolicitud" />
                                <asp:BoundField DataField="fEntregaEstimada" HeaderText="Promesa" ReadOnly="True" SortExpression="fEntregaEstimada" />
                                <asp:TemplateField HeaderText="Entrega"  SortExpression="fEntrega">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFEntrega" runat="server" Text='<%# Bind("fEntrega") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEntrega" runat="server" MaxLength="10" CssClass="input-small" Text='<%# Bind("fEntrega") %>' Enabled="false"></asp:TextBox>
                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" BehaviorID="txtEntrega_CalendarExtender" TargetControlID="txtEntrega" Format="yyyy-MM-dd" PopupButtonID="lnkEntrega" />
                                        <asp:LinkButton ID="lnkEntrega" runat="server" CssClass="btn btn-info t14"><i class="fa fa-calendar"></i></asp:LinkButton>                                        
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                 <asp:TemplateField >
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkimptk" runat="server" CausesValidation="False" CommandArgument='<%# Eval("id_refaccion") %>' CommandName="Select" ToolTip="Seleccionar" CssClass="btn btn-success" 
                                            onclick="lnkimptk_Click"><i class="fa fa-check-circle"></i></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                 <asp:TemplateField >
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnknoimptk" runat="server" CausesValidation="False"  CommandArgument='<%# Eval("id_refaccion") %>' CommandName="Select" ToolTip="Seleccionar" CssClass="btn alert-danger" 
                                            onclick="lnknoimptk_Click"><i class="fa fa-check-circle"></i></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="estatus_imp" HeaderText="Estatus Impresion" SortExpression="estatus_imp" ReadOnly="True"/>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEditar" runat="server" CausesValidation="False"  CommandName="Edit" ToolTip="Actualizar" CssClass="btn btn-warning"><i class="fa fa-edit"></i></asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton ID="lnkActualiza" runat="server" CausesValidation="True" ValidationGroup="acepta" OnClick="lnkActualiza_Click"
                                            CommandName="Update" ToolTip="Actualizar" CssClass="btn btn-success" CommandArgument='<%# Eval("id_refaccion")+";"+Eval("id_cliprov")+";"+Eval("total_orden") %>'><i class="fa fa-save"></i></asp:LinkButton>
                                        <asp:LinkButton ID="lnkCancelar" runat="server" CausesValidation="False" 
                                            CommandName="Cancel" ToolTip="Cancelar" CssClass="btn btn-danger"><i class="fa fa-remove"></i></asp:LinkButton>                                      
                                    </EditItemTemplate>
                                </asp:TemplateField>
                             
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRef" runat="server" Text='<%# Bind("id_refaccion") %>' Visible="false"></asp:Label>
                                        <asp:DataList ID="DataListFotosDanos" runat="server" RepeatColumns="3" RepeatDirection="Horizontal"
                                            DataKeyField="id_empresa" DataSourceID="SqlDataSourceFotosDanos">
                                            <ItemTemplate>                                                                
                                                    <asp:ImageButton OnClick="Image1_Click" ID="Image1" runat="server" AlternateText='<%# Eval("nombre_imagen") %>'
                                                        Width="40px" ImageUrl='<%# "~/ImgRefaccion.ashx?id="+Eval("id_empresa")+";"+Eval("id_taller")+";"+Eval("no_orden")+";"+Eval("id_refaccion")+";"+Eval("id_fotografia") %>' />                                       
                                            </ItemTemplate>
                                            <ItemStyle CssClass="ancho180px textoCentrado" />
                                        </asp:DataList>
                                        <asp:SqlDataSource runat="server" ID="SqlDataSourceFotosDanos" ConnectionString='<%$ ConnectionStrings:PVW %>'                            
                                            SelectCommand="select id_empresa,id_taller,no_orden,id_refaccion,id_fotografia, nimbre_imagen as nombre_imagen from fotos_refacciones where id_empresa=@id_empresa and id_taller=@id_taller and no_orden=@no_orden and id_refaccion=@id_refaccion">                            
                                            <SelectParameters>
                                                <asp:QueryStringParameter Name="no_orden" QueryStringField="o" Type="Int32" />
                                                <asp:QueryStringParameter Name="id_empresa" QueryStringField="e" Type="Int32" />
                                                <asp:QueryStringParameter Name="id_taller" QueryStringField="t" Type="Int32" />
                                                <asp:ControlParameter ControlID="lblRef" PropertyName="Text" Name="id_refaccion" Type="Int32"  />
                                            </SelectParameters>
                                        </asp:SqlDataSource>                        
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="Label11" runat="server" Text='<%# Eval("countFotos") %>'></asp:Label>
                                        <telerik:RadAsyncUpload RenderMode="Lightweight" runat="server" Culture="es-Mx" CssClass="async-attachment"
                                            MaxFileInputsCount="10" MultipleFileSelection="Automatic" ID="AsyncUpload1" HideFileInput="true"
                                            AllowedFileExtensions=".jpeg,.jpg,.png,.gif,.bmp,.tiff,.JPEG,.JPG,.PNG,.GIF,.BMP,.TIFF">
                                        </telerik:RadAsyncUpload>
                                        <asp:LinkButton ID="lnkFotos" runat="server" CausesValidation="False" ValidationGroup="acepta" OnClick="Fotos"
                                            CommandName="Foto" ToolTip="Guardar Fotos" CssClass="btn btn-info" CommandArgument='<%# Eval("id_refaccion") %>'><i class="fa fa-camera"></i></asp:LinkButton>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEnviCorreo" Visible="true" OnClick="lnkEnviCorreo_Click" CommandArgument='<%# Eval("id_refaccion") %>' runat="server" CssClass="btn btn-info"><i class="fa fa-send"></i><spa>&nbsp;Enviar Foto</spa></asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <SelectedRowStyle CssClass="alert-success" />
                            <EditRowStyle CssClass="alert-warning" />
                        </asp:GridView>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                            ConnectionString="<%$ ConnectionStrings:PVW %>" 
                            SelectCommand="select tabla.estatus_imp,tabla.id_detalle,tabla.id_refaccion,tabla.descripcion,tabla.cantidad,tabla.costo_unitario,tabla.porc_desc,tabla.importe_desc,tabla.importe,tabla.estatus,tabla.estatusPre,tabla.estatusRef,
(select staDescripcion from Rafacciones_Estatus where staRefID=tabla.estatusRef) as descripEstatus,tabla.observacion,tabla.fSolicitud,tabla.fEntregaEstimada,tabla.fEntrega,tabla.countFotos,tabla.id_cliprov,tabla.total_orden
from(
select o.id_detalle,o.id_refaccion,o.descripcion,o.cantidad,o.costo_unitario,o.porc_desc,o.importe_desc,o.importe,
(select r.refEstatus from Refacciones_Orden r where r.ref_no_orden=o.no_orden and r.ref_id_empresa=o.id_empresa and r.ref_id_taller=o.id_taller and r.refOrd_Id=o.id_refaccion) as estatus,
case (select r.refEstatus from Refacciones_Orden r where r.ref_no_orden=o.no_orden and r.ref_id_empresa=o.id_empresa and r.ref_id_taller=o.id_taller and r.refOrd_Id=o.id_refaccion) when 'NA' then 'No Aplica' when 'EV' then 'Evaluación' when 'RP' THEN 'Reparación' when 'CO' then 'Compra' when 'CA' THEN 'Cancelada' when 'AP' then 'Aplicada' when 'AU' then 'Autorizada' else '' end as estatusPre,
(select r.refEstatusSolicitud from Refacciones_Orden r where r.ref_no_orden=o.no_orden and r.ref_id_empresa=o.id_empresa and r.ref_id_taller=o.id_taller and r.refOrd_Id=o.id_refaccion) as estatusRef,
(select r.observacion from Refacciones_Orden r where r.ref_no_orden=o.no_orden and r.ref_id_empresa=o.id_empresa and r.ref_id_taller=o.id_taller and r.refOrd_Id=o.id_refaccion) as observacion,
(select convert(char(10),r.refFechSolicitud,126) from Refacciones_Orden r where r.ref_no_orden=o.no_orden and r.ref_id_empresa=o.id_empresa and r.ref_id_taller=o.id_taller and r.refOrd_Id=o.id_refaccion) as fSolicitud,
(select convert(char(10),r.refFechEntregaEst,126) from Refacciones_Orden r where r.ref_no_orden=o.no_orden and r.ref_id_empresa=o.id_empresa and r.ref_id_taller=o.id_taller and r.refOrd_Id=o.id_refaccion) as fEntregaEstimada,
(select convert(char(10),r.refFechEntregaReal,126) from Refacciones_Orden r where r.ref_no_orden=o.no_orden and r.ref_id_empresa=o.id_empresa and r.ref_id_taller=o.id_taller and r.refOrd_Id=o.id_refaccion) as fEntrega,
(select 'Existe(n) '+cast(count(*) as varchar)+' fotografia(s) agregada(s)' from fotos_refacciones fo where fo.no_orden=o.no_orden and fo.id_empresa=o.id_empresa and fo.id_taller=o.id_taller and fo.id_refaccion=o.id_refaccion)as countFotos,
oe.id_cliprov,oe.total_orden,o.estatus_imp
from Orden_Compra_Detalle o
inner join Orden_Compra_Encabezado oe on oe.no_orden=o.no_orden and oe.id_taller=o.id_taller and oe.id_empresa=o.id_empresa and oe.id_orden=o.id_orden
where o.no_orden=@orden and o.id_empresa=@empresa and o.id_taller=@taller and o.id_orden=@cotizacion) as tabla">
                            <SelectParameters>
                                <asp:QueryStringParameter DefaultValue="0" Name="cotizacion" QueryStringField="c" />
                                <asp:QueryStringParameter DefaultValue="0" Name="orden" QueryStringField="o" />
                                <asp:QueryStringParameter DefaultValue="0" Name="empresa" QueryStringField="e" />
                                <asp:QueryStringParameter DefaultValue="0" Name="taller" QueryStringField="t" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                </asp:Panel>

                <asp:Panel ID="PanelMascara" runat="server" CssClass="mask zen2">
                        </asp:Panel>
                        <asp:Panel ID="PanelImgZoom" runat="server" CssClass="popUp zen3 textoCentrado ancho80 centrado">
                            <table class="ancho100">
                                <tr class="ancho100 centrado">
                                    <td class="ancho80 text-center encabezadoTabla roundTopLeft ">
                                        <asp:Label ID="Label9" runat="server" Text="Fotografía" CssClass="t22 colorMorado textoBold" />
                                    </td>
                                    <td class="ancho20 text-right encabezadoTabla roundTopRight">
                                        <asp:LinkButton ID="lnkEliminaFoto" runat="server" ToolTip="Eliminar" OnClick="lnkEliminaFoto_Click"
                                            CssClass="btn btn-danger alingMiddle" OnClientClick="return confirm('¿Esta seguro de eliminar la fotografía?');"><i class="fa fa-trash t18"></i></asp:LinkButton>
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:LinkButton ID="btnCerrarImgZomm" runat="server" ToolTip="Cerrar" OnClick="btnCerrarImgZomm_Click"
                                            CssClass="btn btn-danger alingMiddle"><i class="fa fa-remove t18"></i></asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                            <div class="row">
                                <asp:Panel ID="Panel2" runat="server" CssClass="col-lg-12 col-sm-12 text-center ancho100 pnlImagen"
                                    ScrollBars="Auto">
                                    <asp:Image ID="imgZoom" runat="server" CssClass="ancho100" AlternateText="Imagen no disponible" />
                                </asp:Panel>
                            </div>
                        </asp:Panel>
                                 
            </asp:Panel>

            <div class="col-lg-6 col-sm-6 text-center pad1m">
                <asp:LinkButton ID="lnkImprimeOrden" runat="server" CssClass="btn btn-info t14" onclick="lnkImprimeOrden_Click" ValidationGroup="acepta"><i class="fa fa-print"></i><span>&nbsp;Imprimir Ticket</span></asp:LinkButton>
            </div>
            <div class="col-lg-6 col-sm-6 text-center pad1m">
                <asp:LinkButton ID="lnkNotificaOrden" runat="server" CssClass="btn btn-info t14" onclick="lnkNotificaOrden_Click"><i class="fa fa-send"></i><span>&nbsp;Enviar por Correo</span></asp:LinkButton>
            </div>

            
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



            <asp:Panel ID="PanelMask" runat="server" CssClass="mask" Visible="false"></asp:Panel>
            <asp:Panel ID="PanelPopUpPermiso" runat="server" CssClass="popUp zen2  textoCentrado ancho40 centrado" Visible="false">                
                <table class="ancho100">
                    <tr class="ancho100 centrado">
                        <td class="ancho100 text-center encabezadoTabla roundTopLeft ">
                            <asp:Label ID="lblPop" runat="server" Text="Autorización" CssClass="t22 colorMorado textoBold" />                              
                        </td>                        
                    </tr>                        
                </table>
                <asp:Panel ID="prov" runat="server" CssClass="ancho80 centrado">
                    <div class="row">
                        <div class="col-lg-4 col-sm-4 text-left">
                            <asp:Label ID="Label7" runat="server" Text="Usuario:" CssClass="textoBold" />
                        </div>
                        <div class="col-lg-8 col-sm-8 text-left">
                            <asp:TextBox ID="txtUsuarioLog" runat="server" CssClass="login input-large" MaxLength="20" placeholder="Usuario" TextMode="Password" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" Text="*" ForeColor="Red" ValidationGroup="log" ControlToValidate="txtUsuarioLog" ErrorMessage="Debe indicar el usuario." CssClass="pull-right" />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" Text="*" ForeColor="Red" ValidationGroup="log" ErrorMessage="El usuario debe contener de entre 3 y 20 caracteres." CssClass="pull-right" ControlToValidate="txtUsuarioLog" ValidationExpression="[a-zA-Z0-9]{3,20}" />
                        </div>                                                
                        <div class="col-lg-4 col-sm-4 text-left padding-top-10">
                            <asp:Label ID="Label5" runat="server" Text="Contraseña:" CssClass="textoBold"/>&nbsp;
                        </div>
                        <div class="col-lg-8 col-sm-8 text-left padding-top-10">
                            <asp:TextBox ID="txtContraseñaLog" runat="server" CssClass="login input-large" TextMode="Password" MaxLength="20" placeholder="Contraseña" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" Text="*" ForeColor="Red" ValidationGroup="log" ControlToValidate="txtContraseñaLog" ErrorMessage="Debe indicar la contraseña." CssClass="pull-right" />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" Text="*" ForeColor="Red" ValidationGroup="log" ErrorMessage="La contraseña debe contener de entre 5 y 20 caracteres." CssClass="pull-right" ControlToValidate="txtContraseñaLog" ValidationExpression="[a-zA-Z0-9]{5,20}" />
                        </div>                        
                        <div class="field col-lg-12 col-sm-12 textoCentrado textoBold padding-top-10">
                            <div class="col-lg-12 col-sm-12 text-center">
                                <asp:Label ID="lblErrorLog" runat="server" CssClass="errores" />
                            </div>
                            <div class="col-lg-12 col-sm-12 text-center">
                                <asp:ValidationSummary ID="ValidationSummary2" ValidationGroup="log" CssClass="errores" runat="server" DisplayMode="List" />
                            </div>
                        </div>
                            
                        <div class="col-lg-12 col-sm-12 text-center pad1m">
                            <div class="col-lg-6 col-sm-6 text-center">
                                <asp:LinkButton ID="btnAceptar" runat="server" OnClick="btnAceptar_Click" CssClass="btn btn-success" ValidationGroup="log"><i class="fa fa-check"></i><span>&nbsp;Autorizar</span></asp:LinkButton>
                            </div>
                            <div class="col-lg-6 col-sm-6 text-center">
                                <asp:LinkButton ID="btnCancelar" OnClick="btnCancelar_Click" runat="server" CssClass="btn btn-danger"><i class="fa fa-ban"></i><span>&nbsp;Cancelar</span></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                 </asp:Panel>
            </asp:Panel>





            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                <ProgressTemplate>
                    <asp:Panel ID="pnlMaskLoad" runat="server" CssClass="maskLoad"></asp:Panel>
                    <asp:Panel ID="pnlCargando" runat="server" CssClass="pnlPopUpLoad">
                        <asp:Image ID="imgLoad" runat="server" ImageUrl="~/img/loading.gif" CssClass="ancho100" />
                    </asp:Panel>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>