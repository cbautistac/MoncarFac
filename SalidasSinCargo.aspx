<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SalidasSinCargo.aspx.cs" Inherits="SalidasSinCargo" MasterPageFile="~/AdmonOrden.master" UICulture="es-Mx" Culture="es-Mx"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" />
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server"></telerik:RadSkinManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-lg-12 col-sm-12 text-center alert-info">
                    <h3>
                        <i class="fa fa-sticky-note"></i>&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="Label1" runat="server" Text="Salidas Sin Cargo"></asp:Label>
                    </h3>
                </div>
            </div>
            <asp:Panel runat="server" ID="pnlDaños" CssClass="paneles col-lg-12 col-sm-12" ScrollBars="Auto">
                <div class="row">
                    <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid3" GridLines="None" runat="server" PageSize="50" CssClass="col-lg-12 col-sm-12" AllowAutomaticUpdates="false" AllowPaging="True" AutoGenerateColumns="False" PagerStyle-AlwaysVisible="true" 
                        AllowSorting="true" DataSourceID="SqlDataSource3" Skin="Metro" AllowAutomaticInserts="false" AllowAutomaticDeletes="false">
                        <MasterTableView CommandItemDisplay="None" DataKeyNames="id_remision_ss" HorizontalAlign="NotSet" AutoGenerateColumns="False" DataSourceID="SqlDataSource3">                                                                
                            <Columns>
                                <telerik:GridBoundColumn DataField="no_remision_ss" HeaderText="Salida Sin Cargo" HeaderStyle-Width="80px" SortExpression="no_remision_ss" UniqueName="no_remision_ss" ReadOnly="true"/>
                                <telerik:GridBoundColumn DataField="fecha" HeaderText="Fecha" HeaderStyle-Width="80px" SortExpression="fecha" UniqueName="fecha" ReadOnly="true"  DataFormatString="{0:yyyy-MM-dd}"/>
                                <telerik:GridBoundColumn DataField="hora" HeaderText="Hora" HeaderStyle-Width="80px" SortExpression="hora" UniqueName="hora" ReadOnly="true"/>
                                <telerik:GridBoundColumn DataField="nombre_usuario" HeaderText="Usuario" HeaderStyle-Width="200px" SortExpression="nombre_usuario" UniqueName="nombre_usuario" ReadOnly="true"/>
                                <telerik:GridBoundColumn DataField="total_mo" HeaderText="Mano de Obra" HeaderStyle-Width="80px" SortExpression="total_mo" UniqueName="total_mo" ReadOnly="true" DataFormatString="{0:C2}"/>
                                <telerik:GridBoundColumn DataField="total_refacciones" HeaderText="Refacciones" HeaderStyle-Width="80px" SortExpression="total_refacciones" UniqueName="total_refacciones" ReadOnly="true"  DataFormatString="{0:C2}"/>
                                <telerik:GridBoundColumn DataField="total" HeaderText="Total" HeaderStyle-Width="80px" SortExpression="total" UniqueName="total" ReadOnly="true"  DataFormatString="{0:C2}"/>
                                <telerik:GridBoundColumn DataField="comentarios" HeaderText="Comentarios" HeaderStyle-Width="100px" SortExpression="comentarios" UniqueName="comentarios" ReadOnly="true" />
                            </Columns>
                            <NoRecordsTemplate>
                                <asp:Label ID="lblnoReecMo" runat="server" Text="No existen salidas previas" CssClass="errores"></asp:Label>
                            </NoRecordsTemplate>
                        </MasterTableView>                                
                        <ClientSettings AllowKeyboardNavigation="true">
                            <Selecting AllowRowSelect="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </div>
                <div class="row marTop">
                    <div class="col-lg-6 col-sm-6">
                        <div class="row text-center alert-info"><asp:Label ID="lblTituloMo" runat="server" Text="Mano Obra" CssClass="textoBold"></asp:Label></div>
                        <div class="row text-center"><asp:Label ID="lblErrorMano" runat="server" CssClass="errores"></asp:Label></div>
                        <div class="row">                            
                            <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" GridLines="None" runat="server" PageSize="50" CssClass="col-lg-12 col-sm-12" AllowAutomaticUpdates="True" AllowPaging="True" AutoGenerateColumns="False" PagerStyle-AlwaysVisible="true" 
                                AllowSorting="true" DataSourceID="SqlDataSource1" Skin="Metro" AllowAutomaticInserts="false" AllowAutomaticDeletes="false" >
                                <MasterTableView CommandItemDisplay="Bottom" DataKeyNames="consecutivo" HorizontalAlign="NotSet" EditMode="Batch" AutoGenerateColumns="False" DataSourceID="SqlDataSource1">                                    
                                    <BatchEditingSettings EditType="Row" />
                                    <CommandItemStyle CssClass="text-right" />
                                    <CommandItemSettings SaveChangesText="Guardar Cambios" ShowAddNewRecordButton="false"  ShowRefreshButton="false" ShowSaveChangesButton="true" CancelChangesText="Cancelar Cambios"/>
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="consecutivo" HeaderText="Renglon" HeaderStyle-Width="80px" SortExpression="consecutivo" UniqueName="consecutivo" ReadOnly="true" Visible="false"/>
                                        <telerik:GridBoundColumn DataField="manoObra" HeaderText="Mano de Obra" SortExpression="manoObra" UniqueName="manoObra" ReadOnly="true"/>
                                        <telerik:GridBoundColumn DataField="monto_mo" HeaderText="Monto" HeaderStyle-Width="100px" SortExpression="monto_mo" UniqueName="monto_mo" ReadOnly="true" DataFormatString="{0:C2}"/>                                       
                                        <telerik:GridCheckBoxColumn DataField="aplica_ss" HeaderStyle-Width="80px" HeaderText="Aplicar" SortExpression="aplica_ss" UniqueName="aplica_ss"/>                                        
                                    </Columns>
                                    <NoRecordsTemplate>
                                        <asp:Label ID="lblnoReecMo" runat="server" Text="No existe mano de obra registrada" CssClass="errores"></asp:Label>
                                    </NoRecordsTemplate>
                                </MasterTableView>                                
                                <ClientSettings AllowKeyboardNavigation="true">
                                    <Selecting AllowRowSelect="true" />                                    
                                </ClientSettings>
                            </telerik:RadGrid>
                        </div>
                    </div>                    
                    <div class="col-lg-6 col-sm-6">
                        <div class="row text-center alert-info"><asp:Label ID="Label3" runat="server" Text="Refacciones" CssClass="textoBold"></asp:Label></div>
                        <div class="row text-center"><asp:Label ID="lblErrorRef" runat="server" CssClass="errores"></asp:Label></div>
                        <div class="row">                            
                            <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid2" GridLines="None" runat="server" PageSize="50" AllowAutomaticUpdates="True" CssClass="col-lg-12 col-sm-12" AllowPaging="True" AutoGenerateColumns="False" PagerStyle-AlwaysVisible="true" 
                                AllowSorting="true" DataSourceID="SqlDataSource2" Skin="Metro" AllowAutomaticInserts="false" AllowAutomaticDeletes="false" >
                                <MasterTableView CommandItemDisplay="Bottom" DataKeyNames="refOrd_Id" HorizontalAlign="NotSet" EditMode="Batch" AutoGenerateColumns="False" DataSourceID="SqlDataSource2">                                    
                                    <BatchEditingSettings EditType="Row" />
                                    <CommandItemStyle CssClass="text-right" />
                                    <CommandItemSettings SaveChangesText="Guardar Cambios" ShowAddNewRecordButton="false"  ShowRefreshButton="false" ShowSaveChangesButton="true" CancelChangesText="Cancelar Cambios"/>
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="refOrd_Id" HeaderText="Renglon" HeaderStyle-Width="80px" SortExpression="refOrd_Id" UniqueName="refOrd_Id" ReadOnly="true" Visible="false"/>
                                        <telerik:GridBoundColumn DataField="refCantidad" HeaderText="Cantidad" HeaderStyle-Width="80px" SortExpression="refCantidad" UniqueName="refCantidad" ReadOnly="true"/>
                                        <telerik:GridBoundColumn DataField="refDescripcion" HeaderText="Refacción" SortExpression="refDescripcion" UniqueName="refDescripcion" ReadOnly="true"/>
                                        <telerik:GridBoundColumn DataField="refCosto" HeaderText="Costo Unitario" HeaderStyle-Width="80px" SortExpression="refCosto" UniqueName="refCosto" ReadOnly="true" DataFormatString="{0:C2}"/>
                                        <telerik:GridBoundColumn DataField="refporcentsobrecosto" HeaderText="% Sobre Costo" HeaderStyle-Width="80px" SortExpression="refporcentsobrecosto" UniqueName="refporcentsobrecosto" ReadOnly="true" DataFormatString="{0:F2} %"/>
                                        <telerik:GridBoundColumn DataField="refPrecioVenta" HeaderText="Venta Unitaria" HeaderStyle-Width="80px" SortExpression="refPrecioVenta" UniqueName="refPrecioVenta" ReadOnly="true" DataFormatString="{0:C2}"/>
                                        <telerik:GridBoundColumn DataField="importe" HeaderText="Importe" HeaderStyle-Width="80px" SortExpression="importe" UniqueName="importe" ReadOnly="true" DataFormatString="{0:C2}"/>
                                        <telerik:GridCheckBoxColumn DataField="aplica_ss" HeaderStyle-Width="80px" HeaderText="Aplicar" SortExpression="aplica_ss" UniqueName="aplica_ss"/>                                        
                                    </Columns>
                                    <NoRecordsTemplate>
                                        <asp:Label ID="lblnoReecMo" runat="server" Text="No existen refacciones registradas" CssClass="errores"></asp:Label>
                                    </NoRecordsTemplate>
                                </MasterTableView>                                
                                <ClientSettings AllowKeyboardNavigation="true">
                                    <Selecting AllowRowSelect="true" />                                    
                                </ClientSettings>
                            </telerik:RadGrid>
                        </div>
                    </div>
                </div>

                <div class="row marTop">
                    <div class="col-lg-12 col-sm-12 text-center alert-info"><asp:Label ID="Label5" runat="server" Text="Comentarios" CssClass="textoBold"></asp:Label></div>
                    <div class="col-lg-12 col-sm-12 text-center">
                        <asp:TextBox ID="txtObs" runat="server" CssClass="alingMiddle txtObs col-lg-12 col-sm-12" MaxLength="500" TextMode="MultiLine" Rows="10"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtObsOrdenWatermarkExtender1" runat="server" BehaviorID="txtObs_TextBoxWatermarkExtender" TargetControlID="txtObs" WatermarkText="Comentarios" WatermarkCssClass="water txtObs col-lg-12 col-sm-12" />
                    </div>
                </div>
            </asp:Panel>

            <div class="row pad1m">
                <div class="col-lg-12 col-sm-12 text-center">
                    <asp:Label ID="lblError" runat="server" CssClass="errores"></asp:Label>
                </div>
                <div class="col-lg-6 col-sm-6 text-center">
                    <asp:LinkButton ID="lnkGeneraRemision" runat="server" CssClass="btn btn-primary" OnClick="lnkGeneraRemision_Click"><i class="fa fa-bookmark"></i><span>&nbsp; Generar Salida</span></asp:LinkButton>
                </div>
                <div class="col-lg-6 col-sm- text-center">
                    <asp:LinkButton ID="lnkImprime" runat="server" CssClass="btn btn-primary" OnClick="lnkImprime_Click"><i class="fa fa-print"></i><span>&nbsp; Imprimir Salida</span></asp:LinkButton>
                </div>                   
            </div>

            <div class="pie pad1m">
                <div class="clearfix">
                    <div class="row colorBlanco textoBold">
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label2" runat="server" Text="Tipo Orden:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
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
                            <asp:Label ID="Label111" runat="server" Text="Total Orden:" CssClass="colorEtiqueta" Visible="false"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="lblTotOrden" Visible="false" runat="server"></asp:Label>
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

    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>"                                
        SelectCommand="select mo.consecutivo, (gop.descripcion_go+' '+o.descripcion_op+' '+mo.id_refaccion) as manoObra,mo.monto_mo,mo.aplica_ss,mo.id_salidasincargo from mano_obra mo inner join Grupo_Operacion gop on gop.id_grupo_op=mo.id_grupo_op inner join Operaciones o on o.id_operacion=mo.id_operacion where mo.no_orden=@orden and mo.id_empresa=@empresa and mo.id_taller=@taller and mo.id_salidasincargo=0"
        UpdateCommand="UPDATE mano_obra SET aplica_ss = @aplica_ss WHERE no_orden=@orden and id_empresa=@empresa and id_taller=@taller and consecutivo=@consecutivo">
        <SelectParameters>
            <asp:QueryStringParameter Name="orden" QueryStringField="o" DbType="Int32" DefaultValue="0" />
            <asp:QueryStringParameter Name="empresa" QueryStringField="e" DbType="Int32" DefaultValue="0" />
            <asp:QueryStringParameter Name="taller" QueryStringField="t" DbType="Int32" DefaultValue="0" />
        </SelectParameters>                                
        <UpdateParameters>
            <asp:Parameter Name="aplica_ss" Type="Boolean"></asp:Parameter>
            <asp:Parameter Name="consecutivo" Type="Int32"></asp:Parameter>
            <asp:QueryStringParameter Name="orden" QueryStringField="o" DbType="Int32" DefaultValue="0" />
            <asp:QueryStringParameter Name="empresa" QueryStringField="e" DbType="Int32" DefaultValue="0" />
            <asp:QueryStringParameter Name="taller" QueryStringField="t" DbType="Int32" DefaultValue="0" />
        </UpdateParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>"                                
        SelectCommand="select refOrd_Id,refCantidad,refDescripcion,refCosto,refporcentsobrecosto,refPrecioVenta,(refcantidad*refprecioventa) as importe, aplica_ss from Refacciones_Orden where ref_no_orden=@orden and ref_id_empresa=@empresa and ref_id_taller=@taller and refestatus<>'CA' and id_salidasincargo=0 and refestatusSolicitud<>11"
        UpdateCommand="UPDATE Refacciones_Orden SET aplica_ss = @aplica_ss WHERE ref_no_orden=@orden and ref_id_empresa=@empresa and ref_id_taller=@taller and refOrd_Id=@refOrd_Id">
        <SelectParameters>
            <asp:QueryStringParameter Name="orden" QueryStringField="o" DbType="Int32" DefaultValue="0" />
            <asp:QueryStringParameter Name="empresa" QueryStringField="e" DbType="Int32" DefaultValue="0" />
            <asp:QueryStringParameter Name="taller" QueryStringField="t" DbType="Int32" DefaultValue="0" />
        </SelectParameters>                                
        <UpdateParameters>
            <asp:Parameter Name="aplica_ss" Type="Boolean"></asp:Parameter>
            <asp:Parameter Name="refOrd_Id" Type="Int32"></asp:Parameter>
            <asp:QueryStringParameter Name="orden" QueryStringField="o" DbType="Int32" DefaultValue="0" />
            <asp:QueryStringParameter Name="empresa" QueryStringField="e" DbType="Int32" DefaultValue="0" />
            <asp:QueryStringParameter Name="taller" QueryStringField="t" DbType="Int32" DefaultValue="0" />
        </UpdateParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>"                                
        SelectCommand="select r.id_remision_ss,r.no_remision_ss,r.fecha,cast(convert(time,r.hora,120) as char(8)) as hora,u.nombre_usuario,r.total_mo,r.total_refacciones,r.total,r.comentarios
from Remision_SalidasSinCargo R
INNER JOIN Usuarios u on u.id_usuario=r.id_usuario
where R.id_empresa=@empresa and R.id_taller=@taller and R.no_orden=@orden and R.tipo='S'
order by r.no_remision_ss desc">
        <SelectParameters>
            <asp:QueryStringParameter Name="orden" QueryStringField="o" DbType="Int32" DefaultValue="0" />
            <asp:QueryStringParameter Name="empresa" QueryStringField="e" DbType="Int32" DefaultValue="0" />
            <asp:QueryStringParameter Name="taller" QueryStringField="t" DbType="Int32" DefaultValue="0" />
        </SelectParameters>                                        
    </asp:SqlDataSource>
</asp:Content>