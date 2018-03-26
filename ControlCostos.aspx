<%@ Page Title="" Language="C#" MasterPageFile="~/Cuentas.master" AutoEventWireup="true" CodeFile="ControlCostos.aspx.cs" Inherits="ControlCostos" Culture="es-Mx" UICulture="es-Mx"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <script type="text/javascript">
        function abreWinCtrl() {
            var oWnd = $find("<%=modalPopupControl.ClientID%>");
            oWnd.setUrl('');
            oWnd.show(); 
        }        
        function cierraWinCtrl() {
            var oWnd = $find("<%=modalPopupControl.ClientID%>");
            oWnd.close();
        }
        function abreWin() {
            var oWnd = $find("<%=ImprimeElige.ClientID%>");
            oWnd.setUrl('');
            oWnd.show();
            var oWnd = $find("<%=modalPopupControl.ClientID%>");
            oWnd.close();
        }        
        function cierraWin() {
            var oWnd = $find("<%=ImprimeElige.ClientID%>");
            oWnd.close();
        }
        function fnActivaAdd(sender, eventArgs) {
            var ddlProv = sender;
            //alert(ddlProv.get_selectedItem());
            var lnkAddProv = $get("<%= lnkAgregaProv.ClientID %>");
            if (ddlProv.get_selectedItem() == null)
            {
                //alert(ddlProv.get_selectedItem().get_value())
                lnkAddProv.style.display = "inline";
                //lnkAddProv.disabled = false;
            }
            else
                lnkAddProv.style.display = "none";
        }

        function fnActivaAddCc(sender, eventArgs) {
            var radcboProv = sender;
            //alert(ddlProv.get_selectedItem());
            var lnkAgregaProvCc = $get("<%= lnkAgregaProvCc.ClientID %>");
            if (radcboProv.get_selectedItem() == null)
            {
                //alert(ddlProv.get_selectedItem().get_value())
                lnkAgregaProvCc.style.display = "inline";
                //lnkAddProv.disabled = false;
            }
            else
                lnkAgregaProvCc.style.display = "none";
        }

    </script>

    <telerik:RadScriptManager ID="RadScriptManajer1" runat="server" EnableScriptGlobalization="true"></telerik:RadScriptManager>
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server"></telerik:RadSkinManager>
    <div class="row">
        <div class="col-lg-12 col-sm-12 text-center alert-info">
            <h3>
                <i class="fa fa-cubes"></i>&nbsp;&nbsp;&nbsp;<asp:Label ID="lblTit" runat="server" Text="Control de Costos por Unidad"></asp:Label>&nbsp;&nbsp;&nbsp;<i class="fa fa-car"></i>            
            </h3>
        </div>
    </div>
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
            <div class="page-header">
                <!-- /BREADCRUMBS -->
                <div class="clearfix">
                    <h3 class="content-title pull-left">Ordenes</h3>
                    <asp:Label ID="lblError" runat="server" CssClass="alert-danger"></asp:Label>
                </div>
            </div>
            <asp:Panel ID="Panel4" runat="server" CssClass="panelCatalogos text-center" ScrollBars="Auto">
                <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" EnableAJAX="true" >
                    <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" AllowFilteringByColumn="true" runat="server"  EnableHeaderContextMenu="true" Culture="es-Mx" Skin="Metro" OnItemDataBound="RadGrid1_ItemDataBound" 
                        EnableHeaderContextFilterMenu="true" AllowPaging="True" PagerStyle-AlwaysVisible="true" DataSourceID="SqlDataSource1" AllowSorting="true" PageSize="1000" ShowGroupPanel="True">
                        <MasterTableView DataSourceID="SqlDataSource1" AutoGenerateColumns="False" DataKeyNames="no_orden,id_taller,id_empresa" ShowGroupFooter="true">
                            <GroupByExpressions>
                                <telerik:GridGroupByExpression>
                                    <SelectFields>
                                        <telerik:GridGroupByField FieldAlias="Empresa" FieldName="empresa" ></telerik:GridGroupByField>
                                    </SelectFields>
                                    <GroupByFields>
                                        <telerik:GridGroupByField FieldName="empresa" SortOrder="Descending"></telerik:GridGroupByField>
                                    </GroupByFields>
                                </telerik:GridGroupByExpression>
                                <telerik:GridGroupByExpression>
                                    <SelectFields>
                                        <telerik:GridGroupByField FieldAlias="Taller" FieldName="taller"></telerik:GridGroupByField>
                                    </SelectFields>
                                    <GroupByFields>
                                        <telerik:GridGroupByField FieldName="taller" SortOrder="Descending"></telerik:GridGroupByField>
                                    </GroupByFields>
                                </telerik:GridGroupByExpression>                                                               
                            </GroupByExpressions>
                            <Columns>
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="empresa" FilterControlAltText="Filtro Empresa" HeaderText="Empresa" SortExpression="empresa" UniqueName="empresa" Visible="false"/>
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="taller" FilterControlAltText="Filtro Taller" HeaderText="Taller" SortExpression="taller" UniqueName="taller" Visible="false"/>
                                <telerik:GridTemplateColumn FooterText="Total Ordenes: " Aggregate="Count" FilterCheckListEnableLoadOnDemand="true" HeaderText="Orden" SortExpression="no_orden" UniqueName="no_orden" FilterControlAltText="Filtro Orden" DataField="no_orden">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnOrden" runat="server" Text='<%# Eval("no_orden") %>' OnClick="btnOrden_Click" CommandArgument='<%# Eval("no_orden")+";"+Eval("id_taller")+";"+Eval("id_empresa") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>                                
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="f_recepcion" FilterControlAltText="Filtro Ingreso" HeaderText="Ingreso" SortExpression="f_recepcion" UniqueName="f_recepcion" DataFormatString="{0:yyyy-MM-dd}" Resizable="true"/>
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="descripcion" FilterControlAltText="Filtro Vehículo" HeaderText="Vehículo" SortExpression="descripcion" UniqueName="descripcion" Resizable="true"/>
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="modelo" FilterControlAltText="Filtro Modelo" HeaderText="Modelo" SortExpression="modelo" UniqueName="modelo" Resizable="true"/>
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="color_ext" FilterControlAltText="Filtro Color" HeaderText="Color" SortExpression="color_ext" UniqueName="color_ext" Resizable="true"/>
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="placas" FilterControlAltText="Filtro Placas" HeaderText="Placas" SortExpression="placas" UniqueName="placas" Resizable="true"/>
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="perfil" FilterControlAltText="Filtro Perfil" HeaderText="Perfil" SortExpression="perfil" UniqueName="perfil" Resizable="true"/>
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="localizacion" FilterControlAltText="Filtro Localización" HeaderText="Localización" SortExpression="localizacion" UniqueName="localizacion" Resizable="true"/>
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="razon_social" FilterControlAltText="Filtro Cliente" HeaderText="Cliente" SortExpression="razon_social" UniqueName="razon_social" Resizable="true"/>
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="no_siniestro" FilterControlAltText="Filtro Siniestro" HeaderText="Siniestro" SortExpression="no_siniestro" UniqueName="no_siniestro" Resizable="true"/>
                            </Columns>
                        </MasterTableView>
                        <ClientSettings>
                            <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="1"></Scrolling>
                        </ClientSettings>                        
                        <PagerStyle PageSizeControlType="RadDropDownList" Mode="NextPrevAndNumeric"></PagerStyle>
                    </telerik:RadGrid>
                </telerik:RadAjaxPanel>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>"/>
                <div class="ancho100 text-center marTop">
                    <div class="col-lg-12 col-sm-12 alert-info">
                        <h4>Estatus</h4>
                    </div>
                </div>
                <div class="ancho100 text-center marTop">                                       
                    <div class="col-lg-2 col-sm-2 text-center btn btn-primary "><asp:Label ID="Label14" runat="server" Text="Abiertas" CssClass=""></asp:Label></div>
                    <div class="col-lg-2 col-sm-2 text-center btn btn-info"><asp:Label ID="Label16" runat="server" Text="Completadas" CssClass=""></asp:Label></div>
                    <div class="col-lg-2 col-sm-2 text-center btn btn-success"><asp:Label ID="Label17" runat="server" Text="Remisionadas" CssClass=""></asp:Label></div>
                    <div class="col-lg-2 col-sm-2 text-center btn btn-warning"><asp:Label ID="Label18" runat="server" Text="Facturadas" CssClass=""></asp:Label></div>
                    <div class="col-lg-2 col-sm-2 text-center btn btn-default"><asp:Label ID="Label19" runat="server" Text="Cerradas" CssClass=""></asp:Label></div>
                    <div class="col-lg-2 col-sm-2 text-center btn btn-danger"><asp:Label ID="Label45" runat="server" Text="Salida Sin Cargos" CssClass=""></asp:Label></div>
                </div>
            </asp:Panel>
            <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel3">
                <ProgressTemplate>
                    <asp:Panel ID="pnlMaskLoad1" runat="server" CssClass="maskLoad"></asp:Panel>
                    <asp:Panel ID="pnlCargando1" runat="server" CssClass="pnlPopUpLoad">
                        <asp:Image ID="imgLoad1" runat="server" ImageUrl="~/img/loading.gif" CssClass="ancho100" />
                    </asp:Panel>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Label ID="lblOrden" runat="server" Visible="false"></asp:Label>
    <asp:Label ID="lblTaller" runat="server" Visible="false"></asp:Label>
    <asp:Label ID="lblEmpresa" runat="server" Visible="false"></asp:Label>
    <%-- IMPRIME POR TITULO --%>
    <telerik:RadWindow RenderMode="Lightweight" ID="ImprimeElige" Title="Control de Costos" EnableShadow="true" Skin="Metro"
        Behaviors="Maximize,Move,Resize,Reload" VisibleOnPageLoad="false" ShowContentDuringLoad="false" DestroyOnClose="true"
        Animation="Fade" runat="server" Modal="true" Width="1000px" Height="700px" Style="z-index: 1000;">
        <ContentTemplate>
            <asp:UpdatePanel ID="UpdatePanelControl1" runat="server">
                <ContentTemplate>
                    <div class="row">
                            <div class="col-lg-12 col-sm-12 text-center alert-info"><h3><asp:Label ID="lblOrdenSele" runat="server" ></asp:Label></h3></div>
                        </div>
                        <div class="row">
                            <div class="col-lg-12 col-sm-12 text-center"><asp:Label ID="lblErrorCosto" runat="server" CssClass="errores" ></asp:Label></div>
                        </div>
                    <asp:Label ID="lblEmp" runat="server" Visible="false"> </asp:Label>
                    <asp:Label ID="lblTall" runat="server" Visible="false"> </asp:Label>
                    <asp:Label ID="lblOrd" runat="server" Visible="false"> </asp:Label>
                                       <div class="row pad1m"> 
                                                                                                                            
                                            <div class="col-lg-12 col-sm-12 text-center"><asp:LinkButton ID="lnkRefresh" runat="server" CssClass="btn btn-info" OnClick="lnkRefrescar_Click"> <i class="fa fa-refresh"></i>&nbsp;<span>Generar Informaci&oacute;n</span></asp:LinkButton></div>
                                        </div>
               <div class="row marTop">
                <div class="col-lg-6 col-sm-6">
                    <asp:LinkButton ID="lnkAgregarTodo" runat="server" CssClass="btn btn-primary" OnClick="lnkAgregarTodo_Click"><i class="fa fa-plus"></i><span>&nbsp;Agregar Todo</span></asp:LinkButton><br />
                    <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" EnableAJAX="true">                        
                        <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid4" AllowFilteringByColumn="true" runat="server"  EnableHeaderContextMenu="true" Culture="es-Mx" Skin="Metro"
                                EnableHeaderContextFilterMenu="true" AllowPaging="True" PagerStyle-AlwaysVisible="true" DataSourceID="SqlDataSource3" AllowSorting="true" GroupingEnabled="false" ShowFooter="true" PageSize="100">
                            <MasterTableView DataSourceID="SqlDataSource3" AutoGenerateColumns="False" DataKeyNames="no_orden">
                                <Columns>
                                    <telerik:GridTemplateColumn FilterCheckListEnableLoadOnDemand="true" HeaderText="Agregar">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkAgrega" ToolTip="Agregar a lista de Impresion" CommandArgument='<%# Eval("no_orden") + ";" + Eval("id_material") + ";" + Eval("fecha") + ";" + Eval("identificador") + ";" + Eval("cantidad") + ";" + Eval("nombre") + ";" + Eval("montoAutorizado") + ";" + Eval("razon_social") + ";" + Eval("monto1") + ";" + Eval("monto2")%>' OnClick="lnkAgregar_Click" CssClass="textoBold link"  runat="server" Text='<%# Bind("identificador")%>'/>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <%-- 
                                    <telerik:GridBoundColumn DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:d}" SortExpression="Fecha" UniqueName="Fecha"/>
                                    --%>
                                    
                                    <telerik:GridBoundColumn DataField="nombre" HeaderText="Concepto" SortExpression="nombre" UniqueName="nombre"/>
                                    <telerik:GridBoundColumn DataField="montoAutorizado" Visible="false" HeaderText="Monto" SortExpression="montoAutorizado" UniqueName="montoAutorizado"/>
                                    <telerik:GridBoundColumn DataField="razon_social" HeaderText="Tienda" SortExpression="razon_social" UniqueName="razon_social"/>
                                    <telerik:GridBoundColumn DataField="Monto2" HeaderText="Precio Autorizado" SortExpression="Monto2" UniqueName="Monto2" Aggregate="Sum" DataFormatString="{0:N2}" FooterAggregateFormatString="{0:N2}"/>
                                    <telerik:GridBoundColumn DataField="cantidad" HeaderText="Cantidad" SortExpression="cantidad" UniqueName="cantidad"/>
                                    <telerik:GridBoundColumn DataField="importe" HeaderText="Importe" SortExpression="importe" UniqueName="importe" Aggregate="Sum" DataFormatString="{0:N2}" FooterAggregateFormatString="{0:N2}"/>
                                    </Columns>    
                                <NoRecordsTemplate>
                                        <asp:Label runat="server" ID="lblVacio" Text="No se ha seleccionado un Concepto" CssClass="errores"></asp:Label>
                                </NoRecordsTemplate>
                            </MasterTableView>
                            <ClientSettings>
                                <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="1"></Scrolling>
                            </ClientSettings> 
                            <PagerStyle PageSizeControlType="RadDropDownList" Mode="NextPrevAndNumeric"></PagerStyle>
                        </telerik:RadGrid>
                    </telerik:RadAjaxPanel>
                    <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ConnectionStrings:PVW %>" 
                        SelectCommand="SELECT id_empresa,id_taller,no_orden,id_material,fecha,identificador,cantidad,upper(nombre) as nombre,montoAutorizado,razon_social,monto1,(monto2*cantidad) as monto2,(monto1*cantidad) as importe
FROM tmp_costo_unidad_todos 
where no_orden =@orden and id_empresa=@empresa and id_taller=@taller and id_material not in (select id_material from tmp_costo_unidad) "><SelectParameters>
                           <asp:ControlParameter ControlID="lblOrd" Name="orden" PropertyName="Text" />
                           <asp:ControlParameter ControlID="lblEmp" Name="empresa" PropertyName="Text" />
                           <asp:ControlParameter ControlID="lblTall" Name="taller" PropertyName="Text" />
                           
                        </SelectParameters>
                    </asp:SqlDataSource>
                    </div>

                    <div class="col-lg-6 col-sm-6">
                        <asp:LinkButton ID="lnkQuitarTodo" runat="server" CssClass="btn btn-primary" OnClick="lnkQuitarTodo_Click"><i class="fa fa-remove"></i><span>&nbsp;Quitar Todo</span></asp:LinkButton><br />
                    <telerik:RadAjaxPanel ID="RadAjaxPanel3" runat="server" EnableAJAX="true">                        
                        <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid3" AllowFilteringByColumn="true" runat="server"  EnableHeaderContextMenu="true" Culture="es-Mx" Skin="Metro"
                                EnableHeaderContextFilterMenu="true" AllowPaging="True" PagerStyle-AlwaysVisible="true" DataSourceID="SqlDataSource4" AllowSorting="true" ShowFooter="true" GroupingEnabled="false" PageSize="100" >
                            <MasterTableView DataSourceID="SqlDataSource4" AutoGenerateColumns="False" DataKeyNames="no_orden,id_material" >
                                <Columns>
                                    <telerik:GridTemplateColumn FilterCheckListEnableLoadOnDemand="true" HeaderText="Agregar">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkQuita" ToolTip="Quita de la lista de Impresion" CommandArgument='<%# Eval("identificador") + ";" + Eval("no_orden") + ";" + Eval("id_material")%>' OnClick="lnkQuitar_Click" CssClass="textoBold link"  runat="server" Text='<%# Bind("identificador")%>'/>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <%--                                      
                                    <telerik:GridBoundColumn DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:d}" SortExpression="Fecha" UniqueName="Fecha"/>
                                    --%>
                                    
                                    <telerik:GridBoundColumn DataField="nombre" HeaderText="Concepto"  SortExpression="nombre" UniqueName="nombre"/>
                                    <telerik:GridBoundColumn DataField="montoAutorizado" Visible="false" HeaderText="Monto" SortExpression="montoAutorizado" UniqueName="montoAutorizado"/>
                                    <telerik:GridBoundColumn DataField="razon_social" HeaderText="Tienda" SortExpression="razon_social" UniqueName="razon_social"/>
                                    <telerik:GridBoundColumn DataField="Monto2" HeaderText="Precio Autorizado" SortExpression="Monto2" UniqueName="Monto2" Aggregate="Sum" DataFormatString="{0:N2}" FooterAggregateFormatString="{0:N2}"/> 
                                    <telerik:GridBoundColumn DataField="cantidad" HeaderText="Cantidad" SortExpression="cantidad" UniqueName="cantidad"/>
                                    <telerik:GridBoundColumn DataField="importe" HeaderText="Importe" SortExpression="importe" UniqueName="importe" Aggregate="Sum" DataFormatString="{0:N2}" FooterAggregateFormatString="{0:N2}"/>               
                                </Columns>
                                <NoRecordsTemplate>
                                        <asp:Label runat="server" ID="lblVacio" Text="No se ha seleccionado un Concepto" CssClass="errores"></asp:Label>
                                </NoRecordsTemplate>
                            </MasterTableView>
                            <ClientSettings>
                                <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="1"></Scrolling>
                            </ClientSettings> 
                            <PagerStyle PageSizeControlType="RadDropDownList" Mode="NextPrevAndNumeric"></PagerStyle>
                        </telerik:RadGrid>
                    </telerik:RadAjaxPanel>
                    <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ConnectionStrings:PVW %>" SelectCommand="select id_empresa, id_taller, no_orden, id_material, fecha, identificador, cantidad, upper(nombre) as nombre, montoAutorizado, razon_social, monto1, (monto2* cantidad) as monto2,estatus, (monto1*cantidad) as importe from tmp_costo_unidad where  id_empresa=@empresa and id_taller=@taller and no_orden=@orden">
                        <SelectParameters>
                           <asp:ControlParameter ControlID="lblOrd" Name="orden" PropertyName="Text" />
                           <asp:ControlParameter ControlID="lblEmp" Name="empresa" PropertyName="Text" />
                           <asp:ControlParameter ControlID="lblTall" Name="taller" PropertyName="Text" />
                        </SelectParameters>
                     </asp:SqlDataSource>
                    </div>
                       <div class="row pad1m"> 
                            <div class="col-lg-6 col-sm-6 text-center marTop"><asp:LinkButton ID="lnkImprimirDivididos" runat="server" CssClass="btn btn-primary" OnClick="lnkImprimirs_Click"><i class="fa fa-print"></i>&nbsp;<span>Imprimir</span></asp:LinkButton></div>                                        
                            <div class="col-lg-6 col-sm-6 text-center marTop"><asp:LinkButton ID="lnkTerminar" runat="server" CssClass="btn btn-danger" OnClick="lnkTerminar_Click">&nbsp;<span>Cerrar</span></asp:LinkButton></div>
                       </div>
                   </div>
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanelControl1">
                <ProgressTemplate>
                    <asp:Panel ID="pnlMaskLoad" runat="server" CssClass="maskLoad"></asp:Panel>
                    <asp:Panel ID="pnlCargando" runat="server" CssClass="pnlPopUpLoad">
                        <asp:Image ID="imgLoad" runat="server" ImageUrl="~/img/loading.gif" CssClass="ancho100" />
                    </asp:Panel>
                </ProgressTemplate>
            </asp:UpdateProgress>
               </ContentTemplate>
            </asp:UpdatePanel>
         </ContentTemplate>
    </telerik:RadWindow>
    <%-- VISUALIZAR TODO --%>
    <telerik:RadWindow RenderMode="Lightweight" ID="modalPopupControl" Title="Control de Costos" EnableShadow="true" Skin="Metro"
        Behaviors="Close,Maximize,Move,Resize,Reload" VisibleOnPageLoad="false" ShowContentDuringLoad="false" DestroyOnClose="true"
        Animation="Fade" runat="server" Modal="true" Width="1000px" Height="700px" Style="z-index: 1000;" >
        <ContentTemplate>
            <asp:UpdatePanel ID="UpdatePanelControl" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="Panel1" runat="server" CssClass="ancho95 centrado">
                        <div class="row">
                            <div class="col-lg-12 col-sm-12 text-center alert-info"><h3><asp:Label ID="lblOrdenSelect" runat="server" ></asp:Label></h3></div>
                        </div>
                        <div class="row">
                            <div class="col-lg-12 col-sm-12 text-center"><asp:Label ID="lblErrorCostos" runat="server" CssClass="errores" ></asp:Label></div>
                        </div>
                        <div class="row">
                            <div class="col-lg-12 col-sm-12">
                                <telerik:RadTabStrip RenderMode="Lightweight" runat="server" ID="RadTabStrip1"  Orientation="HorizontalTop"
                                    SelectedIndex="0" MultiPageID="RadMultiPage1" Skin="MetroTouch">
                                    <Tabs>
                                        <telerik:RadTab Text="General"/>                                        
                                        <telerik:RadTab Text="Mano de Obra"/>
                                        <telerik:RadTab Text="Refacciones"/>                                        
                                        <telerik:RadTab Text="Pintura"/>
                                        <telerik:RadTab Text="Caja Chica"/>
                                        <telerik:RadTab Text="Almacen"/>
                                    </Tabs>
                                </telerik:RadTabStrip>                                
                            </div>                        
                        </div>
                        <div class="row">
                            <div class="col-lg-12 col-sm-12">
                                <telerik:RadMultiPage runat="server" ID="RadMultiPage1"  SelectedIndex="0" >
                                    <telerik:RadPageView runat="server" ID="RadPageGeneral">
                                        <div class="row pad1m">
                                            <div class="col-lg-3 col-sm-3"></div>
                                            <div class="col-lg-3 col-sm-3 text-left border-bottom"><asp:Label ID="Label9" runat="server" CssClass="textoBold" Text="Costo Indirecto:" ></asp:Label></div>
                                            <div class="col-lg-3 col-sm-3 text-right border-bottom"><asp:Label ID="lblTotCostoIndirecto" runat="server" CssClass="textoBold" ></asp:Label></div>
                                            <div class="col-lg-3 col-sm-3"></div>
                                        </div>
                                        <div class="row pad1m">
                                            <div class="col-lg-3 col-sm-3"></div>
                                            <div class="col-lg-3 col-sm-3 text-left border-bottom"><asp:Label ID="Label13" runat="server" CssClass="textoBold" Text="Total Mano de Obra:" ></asp:Label></div>
                                            <div class="col-lg-3 col-sm-3 text-right border-bottom"><asp:Label ID="lblTotMo" runat="server" CssClass="textoBold" ></asp:Label></div>
                                            <div class="col-lg-3 col-sm-3"></div>
                                        </div>
                                        <div class="row pad1m">
                                            <div class="col-lg-3 col-sm-3"></div>
                                            <div class="col-lg-3 col-sm-3 text-left border-bottom"><asp:Label ID="Label43" runat="server" CssClass="textoBold" Text="Total Refacciones:" ></asp:Label></div>
                                            <div class="col-lg-3 col-sm-3 text-right border-bottom"><asp:Label ID="lblTotRef" runat="server" CssClass="textoBold" ></asp:Label></div>
                                            <div class="col-lg-3 col-sm-3"></div>
                                        </div>
                                        <div class="row pad1m">
                                            <div class="col-lg-3 col-sm-3"></div>
                                            <div class="col-lg-3 col-sm-3 text-left border-bottom"><asp:Label ID="Label20" runat="server" CssClass="textoBold" Text="Total Pintura:" ></asp:Label></div>
                                            <div class="col-lg-3 col-sm-3 text-right border-bottom"><asp:Label ID="lblTotPint" runat="server" CssClass="textoBold" ></asp:Label></div>
                                            <div class="col-lg-3 col-sm-3"></div>
                                        </div>
                                        <div class="row pad1m">
                                            <div class="col-lg-3 col-sm-3"></div>
                                            <div class="col-lg-3 col-sm-3 text-left border-bottom"><asp:Label ID="Label22" runat="server" CssClass="textoBold" Text="Total Caja Chica:" ></asp:Label></div>
                                            <div class="col-lg-3 col-sm-3 text-right border-bottom"><asp:Label ID="lblTotCc" runat="server" CssClass="textoBold" ></asp:Label></div>
                                            <div class="col-lg-3 col-sm-3"></div>
                                        </div>
                                        <div class="row pad1m">
                                            <div class="col-lg-3 col-sm-3"></div>
                                            <div class="col-lg-3 col-sm-3 text-left border-bottom"><asp:Label ID="Label46" runat="server" CssClass="textoBold" Text="Total Almacen:" ></asp:Label></div>
                                            <div class="col-lg-3 col-sm-3 text-right border-bottom"><asp:Label ID="lblTotExt" runat="server" CssClass="textoBold" ></asp:Label></div>
                                            <div class="col-lg-3 col-sm-3"></div>
                                        </div>
                                        <div class="row pad1m">
                                            <div class="col-lg-3 col-sm-3"></div>
                                            <div class="col-lg-3 col-sm-3 text-left border-bottom"><h4><asp:Label ID="Label24" runat="server" CssClass="textoBold" Text="Total:" ></asp:Label></h4></div>
                                            <div class="col-lg-3 col-sm-3 text-right border-bottom"><h4><asp:Label ID="lblTotTotal" runat="server" CssClass="textoBold" ></asp:Label></h4></div>
                                            <div class="col-lg-3 col-sm-3"></div>
                                        </div>
                                         <div class="row pad1m">
                                            <div class="col-lg-12 col-sm-12 text-center">
                                                <asp:Label ID="Label33" runat="server" CssClass="textoBold" Text="Costo Fijo:" ></asp:Label>&nbsp;
                                                <telerik:RadNumericTextBox RenderMode="Lightweight" runat="server" ID="radCostoFijo" CssClass="input-mini" Value="0" EmptyMessage="Costo Fijo" MinValue="0" MaxValue="9999999.99" ShowSpinButtons="true" NumberFormat-DecimalDigits="2" Skin="MetroTouch"></telerik:RadNumericTextBox>&nbsp;
                                                
                                            </div>
                                         </div>
                                        
                                        <div class="row pad1m">
                                            <div class="col-lg-4 col-sm-4 text-center"><asp:LinkButton ID="lnkAceptarCF" runat="server" CssClass="btn btn-success" OnClick="lnkAceptarCF_Click"><i class="fa fa-check"></i>&nbsp;<span>Aceptar</span></asp:LinkButton></div>
                                            <div class="col-lg-4 col-sm-4 text-center"><asp:LinkButton ID="lnkImprimirtodo" runat="server" CssClass="btn btn-primary"  OnClick="lnkImprimirtodo_Click"><i class="fa fa-print"></i>&nbsp;<span>Imprimir Control de Costos</span></asp:LinkButton></div>
                                            <div class="col-lg-4 col-sm-4 text-center"><asp:LinkButton ID="lnkimprimir" runat="server" CssClass="btn btn-primary" OnClick="lnkimprimir_Click"><i class="fa fa-print"></i>&nbsp;<span>Imprimir Comparativo</span></asp:LinkButton></div>
                                        </div>
                                    </telerik:RadPageView>
                                    <telerik:RadPageView runat="server" ID="RadPageManoObra">
                                        <div class="row">
                                            <div class="col-lg-12 col-sm-12">
                                                <asp:Label ID="lblAno" runat="server" Visible="false"></asp:Label>
                                                <telerik:RadGrid RenderMode="Lightweight" runat="server" ID="RadGrid2" DataSourceID="SqlDataSource2" AllowSorting="True" Skin="Metro" OnItemDataBound="RadGrid2_ItemDataBound"
                                                    AllowPaging="true" PageSize="100">
                                                    <MasterTableView AutoGenerateColumns="False" DataKeyNames="IdEmp,id_asignacion" DataSourceID="SqlDataSource2"
                                                        EnableHeaderContextMenu="true">
                                                        <Columns>
                                                            <telerik:GridBoundColumn DataField="nombre" HeaderText="Operario" SortExpression="nombre" UniqueName="nombre"/>                                                            
                                                            <telerik:GridBoundColumn DataField="puesto" HeaderText="Puesto" SortExpression="puesto" UniqueName="puesto"/>                                                            
                                                            <telerik:GridBoundColumn DataField="fecha_asignacion" HeaderText="Asignado" SortExpression="fecha_asignacion" UniqueName="fecha_asignacion" DataFormatString="{0:yyyy-MM-dd}"/>                                                            
                                                            <telerik:GridBoundColumn DataField="fecha_ini_prog" HeaderText="Inicio Prog." SortExpression="fecha_ini_prog" UniqueName="fecha_ini_prog" DataFormatString="{0:yyyy-MM-dd}"/>                                                            
                                                            <telerik:GridBoundColumn DataField="fecha_ini" HeaderText="Inicio" SortExpression="fecha_ini" UniqueName="fecha_ini" DataFormatString="{0:yyyy-MM-dd}"/>
                                                            <telerik:GridBoundColumn DataField="fecha_fin" HeaderText="Termino" SortExpression="fecha_fin" UniqueName="fecha_fin" DataFormatString="{0:yyyy-MM-dd}"/>
                                                            <telerik:GridBoundColumn DataField="montoAutorizado" HeaderText="Monto Autorizado" SortExpression="montoAutorizado" UniqueName="montoAutorizado"/>
                                                            <telerik:GridBoundColumn DataField="montoUsuado" HeaderText="Monto Ocupado" SortExpression="montoUsuado" UniqueName="montoUsuado"/>
                                                            <telerik:GridBoundColumn DataField="pagar" HeaderText="Monto Pagar" SortExpression="pagar" UniqueName="pagar"/>
                                                            <telerik:GridBoundColumn DataField="pagado" HeaderText="Pagado" SortExpression="pagado" UniqueName="pagado"/>
                                                            <telerik:GridBoundColumn DataField="fechaPago" HeaderText="Fecha Pago" SortExpression="fechaPago" UniqueName="fechaPago" DataFormatString="{0:yyyy-MM-dd}"/>
                                                        </Columns>
                                                        <NoRecordsTemplate>
                                                            <asp:Label ID="noRec1" Text="No existe información de mano de obra registrada" CssClass="errores" runat="server"></asp:Label>
                                                        </NoRecordsTemplate>                                                        
                                                    </MasterTableView>                                                    
                                                    <ClientSettings>
                                                        <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true"></Scrolling>                                                        
                                                    </ClientSettings>                        
                                                    <PagerStyle PageSizeControlType="RadDropDownList" Mode="NextPrevAndNumeric"></PagerStyle>
                                                </telerik:RadGrid>
                                                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select e.IdEmp,oo.id_asignacion,ltrim(rtrim(ltrim(rtrim(e.Nombres))+' '+ltrim(rtrim(isnull(e.Apellido_Paterno,'')))+' '+ltrim(rtrim(isnull(e.Apellido_Materno,''))))) as nombre,p.descripcion as puesto,oo.estatus,oo.fecha_asignacion,oo.fecha_ini_prog,oo.fecha_ini,oo.fecha_fin,
                                                 oo.monto as montoAutorizado,
                                                (select isnull(sum(total),0) from Registro_Pinturas r where r.id_empresa=oo.id_empresa and r.id_taller=oo.id_taller and r.no_orden=oo.no_orden and r.idEmp=oo.IdEmp and r.ano=@ano) as montoUsuado,
                                                oo.monto-((select isnull(sum(total),0) from Registro_Pinturas r where r.id_empresa=oo.id_empresa and r.id_taller=oo.id_taller and r.no_orden=oo.no_orden and r.idEmp=oo.IdEmp and r.ano=@ano)) as pagar,oo.pagado,oo.fechaPago
                                                from Operativos_Orden oo 
                                                inner join empleados e on e.IdEmp=oo.IdEmp
                                                left join Puestos p on p.id_puesto=e.Puesto
                                                where oo.id_empresa=@empresa and oo.id_taller=@taller and oo.no_orden=@orden">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="lblOrden" Name="orden" PropertyName="Text" />
                                                        <asp:ControlParameter ControlID="lblAno" Name="ano" PropertyName="Text" />
                                                        <asp:ControlParameter ControlID="lblEmpresa" Name="empresa" PropertyName="Text" />
                                                        <asp:ControlParameter ControlID="lblTaller" Name="taller" PropertyName="Text" />
                                                    </SelectParameters>
                                                </asp:SqlDataSource>
                                            </div>
                                        </div>
                                        <div class="row pad1m">
                                            <h4>
                                                <div class="col-lg-3 col-sm-3"></div>
                                                <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label15" runat="server" CssClass="textoBold" Text="Total Mano de Obra:" ></asp:Label></div>
                                                <div class="col-lg-3 col-sm-3 text-right"><asp:Label ID="lblMoTotal" runat="server" CssClass="textoBold" ></asp:Label></div>
                                                <div class="col-lg-3 col-sm-3"></div>
                                            </h4>
                                        </div>
                                    </telerik:RadPageView>
                                    <telerik:RadPageView runat="server" ID="RadPageRefacciones">
                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                            <ContentTemplate>
                                                <div class="row">
                                                    <div class="col-lg-12 col-sm-12">  
                                                        <telerik:RadGrid RenderMode="Lightweight" runat="server" ID="RadGrid5" DataSourceID="SqlDataSource7" AllowSorting="True" Skin="Metro" OnItemDataBound="RadGrid5_ItemDataBound" ShowFooter="false"
                                                            AllowPaging="false" PageSize="100">
                                                            <MasterTableView AutoGenerateColumns="False" DataKeyNames="refOrd_id" DataSourceID="SqlDataSource7"
                                                                EnableHeaderContextMenu="true">
                                                                <Columns>
                                                                    <telerik:GridBoundColumn DataField="refOrd_id" HeaderText="Clave" SortExpression="refOrd_id" UniqueName="refOrd_id" Visible="false"/>                                                            
                                                                    <telerik:GridBoundColumn DataField="refDescripcion" HeaderText="Refacción" SortExpression="refDescripcion" UniqueName="refDescripcion"/>                                                            
                                                                    <telerik:GridBoundColumn DataField="refCantidad" HeaderText="Cantidad" SortExpression="refCantidad" UniqueName="refCantidad" />                                                                                                                                
                                                                    <telerik:GridBoundColumn DataField="importe" HeaderText="Precio Autorizado" SortExpression="importe" UniqueName="importe" DataFormatString="{0:C2}" Aggregate="Sum"/>
                                                                    <telerik:GridBoundColumn DataField="RefCosto" HeaderText="Precio Compra" SortExpression="Compra" UniqueName="Compra" DataFormatString="{0:C2}" Aggregate="Sum"/>
                                                                    <telerik:GridBoundColumn DataField="razon_social" HeaderText="Proveedor" SortExpression="razon_social" UniqueName="razon_social"/>
                                                                    <telerik:GridBoundColumn DataField="staDescripcion" HeaderText="Estatus Refacción" SortExpression="staDescripcion" UniqueName="staDescripcion"/>
                                                                    <telerik:GridBoundColumn DataField="reffechSolicitud" HeaderText="Fecha Solicitud" SortExpression="reffechSolicitud" UniqueName="reffechSolicitud" DataFormatString="{0:yyyy-MM-dd}"/>
                                                                    <telerik:GridBoundColumn DataField="reffechEntregaEst" HeaderText="Fecha Entrega Estimada" SortExpression="reffechEntregaEst" UniqueName="reffechEntregaEst" DataFormatString="{0:yyyy-MM-dd}"/>
                                                                    <telerik:GridBoundColumn DataField="refFechEntregaReal" HeaderText="Fecha Entrega" SortExpression="refFechEntregaReal" UniqueName="refFechEntregaReal" DataFormatString="{0:yyyy-MM-dd}"/>                                                                    
                                                                </Columns>
                                                                <NoRecordsTemplate>
                                                                    <asp:Label ID="noRec2" Text="No existe información de refacciones registradas" CssClass="errores" runat="server"></asp:Label>
                                                                </NoRecordsTemplate>                                                        
                                                            </MasterTableView>                                                    
                                                            <ClientSettings>
                                                                <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true"></Scrolling>                                                        
                                                            </ClientSettings>                        
                                                            <PagerStyle PageSizeControlType="RadDropDownList" Mode="NextPrevAndNumeric"></PagerStyle>
                                                        </telerik:RadGrid>
                                                        <asp:SqlDataSource ID="SqlDataSource7" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" 
                                                            SelectCommand="select tabla.refOrd_id,tabla.refDescripcion,tabla.refCantidad,tabla.refProveedor,c.razon_social,tabla.refCosto,tabla.refprecioVenta,tabla.importe,tabla.refEstatus,tabla.refestatussolicitud,tabla.staDescripcion, tabla.reffechSolicitud,tabla.reffechEntregaEst,tabla.refFechEntregaReal,tabla.Compra from ( select ro.refOrd_id,ro.refDescripcion,ro.refCantidad,case ro.refProveedor when 0 then ro.id_cliprov_cotizacion else ro.refproveedor end as refProveedor,ro.refCosto,ro.refprecioVenta,(ro.refcantidad*ro.refprecioVenta) as importe,  ro.refEstatus,ro.refestatussolicitud,re.staDescripcion,ro.reffechSolicitud,ro.reffechEntregaEst,ro.refFechEntregaReal, isnull((select sum(o.importe) from orden_compra_detalle o inner join refacciones_orden r on r.ref_no_orden=o.no_orden and r.ref_id_empresa=o.id_empresa and r.ref_id_taller=o.id_taller and r.reford_id=o.id_refaccion where o.no_orden=ro.ref_no_orden and o.id_empresa=ro.ref_id_empresa and o.id_taller=ro.ref_id_taller and r.reford_id=ro.reford_id),0) as Compra from refacciones_orden ro left join Rafacciones_Estatus re on re.staRefID=ro.refEstatusSolicitud where ro.ref_no_orden=@orden and ro.ref_id_empresa=@empresa and ro.ref_id_taller=@taller and ro.refEstatusSolicitud =3 ) as tabla left join cliprov c on c.id_cliprov =tabla.refproveedor and c.tipo='P'">
                                                            <SelectParameters>
                                                                <asp:ControlParameter ControlID="lblOrden" Name="orden" PropertyName="Text" />                                                                
                                                                <asp:ControlParameter ControlID="lblEmpresa" Name="empresa" PropertyName="Text" />
                                                                <asp:ControlParameter ControlID="lblTaller" Name="taller" PropertyName="Text" />
                                                            </SelectParameters>
                                                        </asp:SqlDataSource>
                                                    </div>
                                                </div>
                                                <div class="row pad1m">
                                                    <h4>
                                                        <div class="col-lg-3 col-sm-3"></div>
                                                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label44" runat="server" CssClass="textoBold" Text="Total Refacciones:" ></asp:Label></div>
                                                        <div class="col-lg-3 col-sm-3 text-right"><asp:Label ID="lblTotRefacciones" runat="server" CssClass="textoBold" ></asp:Label></div>
                                                        <div class="col-lg-3 col-sm-3"></div>
                                                    </h4>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </telerik:RadPageView>
                                    <telerik:RadPageView runat="server" ID="RadPagePintura">
                                        <asp:UpdatePanel ID="UpdPintura" runat="server" ChildrenAsTriggers="true">
                                            <ContentTemplate>
                                                <div class="row">
                                                    <div class="col-lg-12 col-sm-12">  
                                                        <asp:Label ID="lblAcceso" runat="server" Visible="false"></asp:Label>
                                                        <asp:Label ID="lblRenglon" runat="server" Visible="false"></asp:Label>
                                                        <telerik:RadGrid RenderMode="Lightweight" runat="server" ID="grdPint" AllowSorting="True" Skin="Metro" OnItemDataBound="grdPint_ItemDataBound"
                                                            AllowPaging="true" PageSize="100" OnDeleteCommand="grdPint_DeleteCommand" >
                                                            <MasterTableView AutoGenerateColumns="False" DataKeyNames="renglon,Provedor" EnableHeaderContextMenu="true">
                                                                <Columns>
                                                                    <telerik:GridBoundColumn DataField="renglon" HeaderText="Clave" SortExpression="renglon" UniqueName="renglon" Visible="false"/>
                                                                    <telerik:GridBoundColumn DataField="Factura" HeaderText="Factura" SortExpression="Factura" UniqueName="Factura"  />                                                            
                                                                    <telerik:GridBoundColumn DataField="fecha" HeaderText="Fecha" SortExpression="fecha" UniqueName="fecha" DataFormatString="{0:yyyy-MM-dd}" ItemStyle-Width="70px"/>                                                            
                                                                    <telerik:GridBoundColumn DataField="razon_social" HeaderText="Proveedor" SortExpression="razon_social" UniqueName="razon_social"/>                                                            
                                                                    <telerik:GridBoundColumn DataField="cantidad" HeaderText="Cantidad" SortExpression="cantidad" UniqueName="cantidad" />                                                            
                                                                    <telerik:GridBoundColumn DataField="descripcion" HeaderText="Descripción" SortExpression="descripcion" UniqueName="descripcion"/>
                                                                    <telerik:GridBoundColumn DataField="Costo_Unitario" HeaderText="C.U." SortExpression="Costo_Unitario" UniqueName="Costo_Unitario"/>
                                                                    <telerik:GridBoundColumn DataField="Descuento" HeaderText="Dcto." SortExpression="Descuento" UniqueName="Descuento" />
                                                                    <telerik:GridBoundColumn DataField="Importe" HeaderText="Importe" SortExpression="Importe" UniqueName="Importe"/>
                                                                    <telerik:GridTemplateColumn  HeaderText="Seleccionar" ItemStyle-Width="96px">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="btnRegistroPintura" runat="server" OnClick="btnRegistroPintura_Click" CommandArgument='<%# Eval("renglon") %>' CssClass="btn btn-success"><i class="fa fa-check"></i></asp:LinkButton>
                                                                            <asp:LinkButton ID="btnBorraRegPint" runat="server" CommandName="Delete"  CssClass="btn btn-danger t14"><i class="fa fa-trash"></i></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>                                                                                                                        
                                                                </Columns>
                                                                <NoRecordsTemplate>
                                                                    <asp:Label ID="noRec3" Text="No existe información de pintura registrada" CssClass="errores" runat="server"></asp:Label>
                                                                </NoRecordsTemplate>                                                        
                                                            </MasterTableView>                                                    
                                                            <ClientSettings>
                                                                <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true"></Scrolling>                                                                
                                                            </ClientSettings>                        
                                                            <PagerStyle PageSizeControlType="RadDropDownList" Mode="NextPrevAndNumeric"></PagerStyle>
                                                        </telerik:RadGrid>
                                                        <asp:SqlDataSource ID="SqlDsPintura" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>"
                                                            SelectCommand="Select OC.renglon,OC.fecha,OC.descripcion,OC.proveedor,ltrim(rtrim(C.razon_social)) as razon_social,oc.cantidad,oc.Costo_Unitario,oc.Importe,oc.Factura,oc.Descuento,oc.id_nota_credito
                                                            FROM otros_costos OC 
                                                            LEFT JOIN CLIPROV C ON c.id_cliprov = Cast(oc.proveedor AS INT) anD c.tipo='P'
                                                            where OC.id_empresa=@empresa and OC.id_taller=@taller and OC.no_orden=@orden and OC.area_de_aplicacion='PI'"
                                                            DeleteCommand="DELETE FROM otros_costos WHERE (no_orden = @orden) AND (id_taller = @taller) AND (id_empresa = @emp) AND (Renglon = @reng) AND Area_de_Aplicacion='PI'">
                                                            <DeleteParameters>
                                                                <asp:ControlParameter ControlID="lblEmpresa" Name="emp" PropertyName="Text" Type="Int32" />
                                                                <asp:ControlParameter ControlID="lblTaller" Name="taller" PropertyName="Text" Type="Int32" />
                                                                <asp:ControlParameter ControlID="lblOrden" Name="orden" PropertyName="Text" Type="Int32" />
                                                                <asp:Parameter Name="reng" Type="Int32"></asp:Parameter>
                                                            </DeleteParameters>
                                                            <SelectParameters>
                                                                <asp:ControlParameter ControlID="lblEmpresa" Name="empresa" PropertyName="Text" />
                                                                <asp:ControlParameter ControlID="lblTaller" Name="taller" PropertyName="Text" />
                                                                <asp:ControlParameter ControlID="lblOrden" Name="orden" PropertyName="Text" />
                                                            </SelectParameters>
                                                        </asp:SqlDataSource>
                                                    </div>
                                                </div>
                                                <div class="row pad1m">
                                                    <h4>
                                                        <div class="col-lg-3 col-sm-3"></div>
                                                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label23" runat="server" CssClass="textoBold" Text="Total Pintura:" ></asp:Label></div>
                                                        <div class="col-lg-3 col-sm-3 text-right"><asp:Label ID="lblPinturaTot" runat="server" CssClass="textoBold" ></asp:Label></div>
                                                        <div class="col-lg-3 col-sm-3"></div>
                                                    </h4>
                                                </div>                                        
                                                <div class="row pad1m">
                                                    <div class="col-lg-5 col-sm-5">
                                                        <asp:Label ID="Label25" runat="server" Text="Proveedor:" CssClass="textoBold"></asp:Label>
                                                        <telerik:RadComboBox runat="server" ID="ddlProv" RenderMode="Lightweight" AllowCustomText="true" Width="234px" MaxHeight="300px" DataSourceID="SqlDsProv" Skin="Metro" DataTextField="razon_social" DataValueField="id_cliprov" EmptyMessage="Seleccione Proveedor"  Filter="Contains" AutoPostBack="false" CloseDropDownOnBlur="true" OnClientDropDownClosed="fnActivaAdd" ></telerik:RadComboBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Debe seleccionar una marca" Text="*" CssClass="errores alingMiddle " ValidationGroup="altav" ControlToValidate="ddlProv"></asp:RequiredFieldValidator>
                                                        <asp:LinkButton ID="lnkAgregaProv" runat="server" CssClass="btn btn-info t14 colorBlanco" ToolTip="Agregar Proveedor" onclick="lnkAgregaProv_Click" Style="display:none;"><i class="fa fa-plus t18"></i></asp:LinkButton>
                                                        <asp:SqlDataSource ID="SqlDsProv" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" 
                                                            SelectCommand="select id_cliprov,razon_social from Cliprov where tipo='P'"  
                                                            InsertCommand="INSERT INTO Cliprov (id_cliprov, tipo, persona, rfc, fecha_nacimiento, razon_social) VALUES((SELECT ISNULL((SELECT TOP 1 id_cliprov FROM Cliprov ORDER BY id_cliprov DESC),0) + 1), 'P', 'M', '.', '1900-01-01', @razSoc)" >
                                                            <InsertParameters>
                                                                <asp:ControlParameter Name="razSoc" ControlID="ddlProv" PropertyName="Text" Type="String" />
                                                            </InsertParameters>
                                                        </asp:SqlDataSource>
                                                    </div>
                                                    <div class="col-lg-3 col-sm-3">
                                                         <asp:Label ID="Label26" runat="server" Text="Factura:" CssClass="textoBold"></asp:Label>
                                                        <asp:TextBox ID="txtFactura" runat="server" MaxLength="50" CssClass="input-large" placeholder = "Factura"></asp:TextBox>
                                                    </div>
                                                    <div class="col-lg-3 col-sm-3">
                                                        <asp:Label ID="Label27" runat="server" Text="Fecha:" CssClass="textoBold"></asp:Label>                                                
                                                        <asp:TextBox ID="txtFechaPi" runat="server" Enabled="false" CssClass="input-small" />
                                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" BehaviorID="txtFechaPromesa_CalendarExtender" TargetControlID="txtFechaPi" Format="yyyy-MM-dd" PopupButtonID="lnkFechaPi" />
                                                        <asp:LinkButton ID="lnkFechaPi" runat="server" CssClass="btn btn-info t14"><i class="fa fa-calendar"></i></asp:LinkButton>
                                                    </div>
                                                </div>
                                                <div class="row pad1m">
                                                     <div class="col-lg-3 col-sm-3">
                                                         <asp:Label ID="Label28" runat="server" Text="Descripción:" CssClass="textoBold"></asp:Label>
                                                        <asp:TextBox ID="txtDescripcionPi" runat="server" MaxLength="100" CssClass="input-large" placeholder = "Descripcion"></asp:TextBox>
                                                    </div>
                                                     <div class="col-lg-2 col-sm-2">
                                                        <asp:Label ID="Label29" runat="server" Text="Cantidad:" CssClass="textoBold"></asp:Label>
                                                        <telerik:RadNumericTextBox RenderMode="Lightweight" runat="server" ID="radCatidadPi" CssClass="input-mini" AutoPostBack="true" OnTextChanged="radCatidadPi_TextChanged" Value="0" EmptyMessage="Cantidad" MinValue="0" MaxValue="9999999.99" ShowSpinButtons="true" NumberFormat-DecimalDigits="2" Skin="MetroTouch"></telerik:RadNumericTextBox>
                                                     </div>
                                                     <div class="col-lg-2 col-sm-2">
                                                        <asp:Label ID="Label30" runat="server" Text="Cost. Unit.:" CssClass="textoBold"></asp:Label>
                                                        <telerik:RadNumericTextBox RenderMode="Lightweight" runat="server" ID="radCuPi" CssClass="input-mini" AutoPostBack="true" OnTextChanged="radCuPi_TextChanged" Value="0" EmptyMessage="Cost. Unit." MinValue="0" MaxValue="9999999999.99" ShowSpinButtons="true" NumberFormat-DecimalDigits="2" Skin="MetroTouch"></telerik:RadNumericTextBox>
                                                     </div>
                                                     <div class="col-lg-2 col-sm-2">
                                                        <asp:Label ID="Label31" runat="server" Text="Descuento:" CssClass="textoBold"></asp:Label>
                                                        <telerik:RadNumericTextBox RenderMode="Lightweight" runat="server" ID="radDectoPi" CssClass="input-mini" AutoPostBack="true" OnTextChanged="radDesctoPi_TextChanged" Value="0" EmptyMessage="Descuento" MinValue="0" MaxValue="100" ShowSpinButtons="true" NumberFormat-DecimalDigits="2" Skin="MetroTouch"></telerik:RadNumericTextBox>
                                                    </div>
                                                     <div class="col-lg-2 col-sm-2">
                                                        <asp:Label ID="Label32" runat="server" Text="Importe:" CssClass="textoBold"></asp:Label>
                                                        <asp:Label ID="lblImportePi" runat="server"  CssClass="textoBold"></asp:Label>
                                                    </div>
                                                    <div class="col-lg-1 col-sm-1">
                                                        <asp:LinkButton ID="lnkAceptarPi" runat="server" CssClass="btn btn-success" OnClick="lnkAceptarPi_Click"><i class="fa fa-check"></i>&nbsp;<span>Aceptar</span></asp:LinkButton>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-lg-12 col-sm-12"><asp:Label ID="lblErrorPi" runat="server"  CssClass="errores"></asp:Label></div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </telerik:RadPageView>

                                    <telerik:RadPageView runat="server" ID="RadPageCajaChica">
                                         <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <div class="row">
                                                    <div class="col-lg-12 col-sm-12">                                                
                                                        <telerik:RadGrid RenderMode="Lightweight" runat="server" ID="grdCajaChica" AllowSorting="True" Skin="Metro" OnItemDataBound="grdCajaChica_ItemDataBound"
                                                            AllowPaging="true" PageSize="1000" OnDeleteCommand="grdCajaChica_DeleteCommand">
                                                            <MasterTableView AutoGenerateColumns="False" DataKeyNames="renglon,Provedor" EnableHeaderContextMenu="true">
                                                                <Columns>
                                                                    <telerik:GridBoundColumn DataField="renglon" HeaderText="Clave" SortExpression="renglon" UniqueName="renglon"  Visible="false"/>
                                                                    <telerik:GridBoundColumn DataField="Factura" HeaderText="Factura" SortExpression="Factura" UniqueName="Factura"/>                                                            
                                                                    <telerik:GridBoundColumn DataField="fecha" HeaderText="Fecha" SortExpression="fecha" UniqueName="fecha" DataFormatString="{0:yyyy-MM-dd}"/>                                                            
                                                                    <telerik:GridBoundColumn DataField="razon_social" HeaderText="Proveedor" SortExpression="razon_social" UniqueName="razon_social"/>                                                            
                                                                    <telerik:GridBoundColumn DataField="cantidad" HeaderText="Cantidad" SortExpression="cantidad" UniqueName="cantidad"/>
                                                                    <telerik:GridBoundColumn DataField="descripcion" HeaderText="Descripción" SortExpression="descripcion" UniqueName="descripcion"/>
                                                                    <telerik:GridBoundColumn DataField="Costo_Unitario" HeaderText="C.U." SortExpression="Costo_Unitario" UniqueName="Costo_Unitario"/>
                                                                    <telerik:GridBoundColumn DataField="Descuento" HeaderText="Dcto." SortExpression="Descuento" UniqueName="Descuento" />
                                                                    <telerik:GridBoundColumn DataField="Importe" HeaderText="Importe" SortExpression="Importe" UniqueName="Importe"/>                                                                                                                       
                                                                    <telerik:GridBoundColumn DataField="pago" HeaderText="Pago" SortExpression="pago" UniqueName="pago"/>
                                                                    <telerik:GridTemplateColumn  HeaderText="Seleccionar">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="btnRegistroCC" runat="server" OnClick="btnRegistroCC_Click" CommandArgument='<%# Eval("renglon") %>' CssClass="btn btn-success"><i class="fa fa-check"></i></asp:LinkButton>
                                                                        <asp:LinkButton ID="btnBorraCajChi" runat="server" CommandName="Delete"  CssClass="btn btn-danger t14"><i class="fa fa-trash"></i></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                </Columns>
                                                                <NoRecordsTemplate>
                                                                    <asp:Label ID="noRec4" Text="No existe información de caja chica regsitrada" CssClass="errores" runat="server"></asp:Label>
                                                                </NoRecordsTemplate>                                                        
                                                            </MasterTableView>                                                    
                                                            <ClientSettings>
                                                                <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true"></Scrolling>                                                        
                                                            </ClientSettings>                        
                                                            <PagerStyle PageSizeControlType="RadDropDownList" Mode="NextPrevAndNumeric"></PagerStyle>
                                                        </telerik:RadGrid>
                                                        <asp:SqlDataSource ID="SqlDsCajChica" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" 
                                                            SelectCommand="Select OC.renglon,OC.fecha,OC.descripcion,OC.proveedor,ltrim(rtrim(C.razon_social)) as razon_social,oc.cantidad,oc.Costo_Unitario,oc.Importe,oc.Factura,oc.Descuento,oc.id_nota_credito,case oc.pago when -1 then 'No Especificado' when 0 then 'Contado' when 1 then 'Crédito' else '' end as pago
                                                            from otros_costos OC 
                                                            LEFT JOIN CLIPROV C ON c.id_cliprov = Cast(oc.proveedor AS INT) anD c.tipo='P'
                                                            where OC. id_empresa=@empresa and OC.id_taller=@taller and OC.no_orden=@orden and OC.area_de_aplicacion='CA'"
                                                            DeleteCommand="DELETE FROM otros_costos WHERE (no_orden = @orden) AND (id_taller = @taller) AND (id_empresa = @emp) AND (Renglon = @reng) AND Area_de_Aplicacion='CA'">
                                                            <DeleteParameters>
                                                                <asp:ControlParameter ControlID="lblEmpresa" Name="emp" PropertyName="Text" Type="Int32" />
                                                                <asp:ControlParameter ControlID="lblTaller" Name="taller" PropertyName="Text" Type="Int32" />
                                                                <asp:ControlParameter ControlID="lblOrden" Name="orden" PropertyName="Text" Type="Int32" />
                                                                <asp:Parameter Name="reng" Type="Int32"></asp:Parameter>
                                                            </DeleteParameters>
                                                            <SelectParameters>
                                                                <asp:ControlParameter ControlID="lblOrden" Name="orden" PropertyName="Text" />                                                        
                                                                <asp:ControlParameter ControlID="lblEmpresa" Name="empresa" PropertyName="Text" />
                                                                <asp:ControlParameter ControlID="lblTaller" Name="taller" PropertyName="Text" />
                                                            </SelectParameters>
                                                        </asp:SqlDataSource>
                                                    </div>
                                                </div>
                                                <div class="row pad1m">
                                                    <h4>
                                                        <div class="col-lg-3 col-sm-3"></div>
                                                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label21" runat="server" CssClass="textoBold" Text="Total Caja Chica:" ></asp:Label></div>
                                                        <div class="col-lg-3 col-sm-3 text-right"><asp:Label ID="lblRefaccionesTot" runat="server" CssClass="textoBold" ></asp:Label></div>
                                                        <div class="col-lg-3 col-sm-3"></div>
                                                    </h4>
                                                </div>                                        
                                                <div class="row pad1m">
                                                    <div class="col-lg-5 col-sm-5">
                                                        <asp:Label ID="Label34" runat="server" Text="Proveedor:" CssClass="textoBold"></asp:Label>
                                                        <telerik:RadComboBox runat="server" ID="radcboProv" RenderMode="Lightweight" AllowCustomText="true" Width="234px" MaxHeight="300px" DataSourceID="SqlDsProvCc" Skin="Metro" DataTextField="razon_social" DataValueField="id_cliprov" EmptyMessage="Seleccione Proveedor"  Filter="Contains" AutoPostBack="false" CloseDropDownOnBlur="true" OnClientDropDownClosed="fnActivaAddCc" ></telerik:RadComboBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Debe seleccionar una marca" Text="*" CssClass="errores alingMiddle " ValidationGroup="altav" ControlToValidate="ddlProv"></asp:RequiredFieldValidator>
                                                        <asp:LinkButton ID="lnkAgregaProvCc" runat="server" CssClass="btn btn-info t14 colorBlanco" ToolTip="Agregar Proveedor" onclick="lnkAgregaProvCc_Click" Style="display:none;"><i class="fa fa-plus t18"></i></asp:LinkButton>
                                                        <asp:SqlDataSource ID="SqlDsProvCc" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" 
                                                            SelectCommand="select id_cliprov,razon_social from Cliprov where tipo='P'" >
                                                        </asp:SqlDataSource>
                                                    </div>
                                                    <div class="col-lg-4 col-sm-4">
                                                         <asp:Label ID="Label35" runat="server" Text="Factura:" CssClass="textoBold"></asp:Label>
                                                        <asp:TextBox ID="txtFacturaCc" runat="server" MaxLength="50" CssClass="input-large" placeholder = "Factura"></asp:TextBox>
                                                    </div>
                                                    <div class="col-lg-3 col-sm-3">
                                                        <asp:Label ID="Label36" runat="server" Text="Fecha:" CssClass="textoBold"></asp:Label>                                                
                                                        <asp:TextBox ID="txtFechaCc" runat="server" Enabled="false" CssClass="input-small" />
                                                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" BehaviorID="txtFechaCc_CalendarExtender" TargetControlID="txtFechaCc" Format="yyyy-MM-dd" PopupButtonID="lnkFechaCc" />
                                                        <asp:LinkButton ID="lnkFechaCc" runat="server" CssClass="btn btn-info t14"><i class="fa fa-calendar"></i></asp:LinkButton>
                                                    </div>
                                                </div>
                                                <div class="row pad1m">
                                                     <div class="col-lg-2 col-sm-2">
                                                         <asp:Label ID="Label37" runat="server" Text="Descripción:" CssClass="textoBold"></asp:Label>
                                                        <asp:TextBox ID="txtDescripcionCc" runat="server" MaxLength="100" CssClass="input-large" placeholder = "Descripcion"></asp:TextBox>
                                                    </div>
                                                     <div class="col-lg-2 col-sm-2">
                                                        <asp:Label ID="Label38" runat="server" Text="Cantidad:" CssClass="textoBold"></asp:Label>
                                                        <telerik:RadNumericTextBox RenderMode="Lightweight" runat="server" ID="radCatidadCc" CssClass="input-mini" AutoPostBack="true" OnTextChanged="radCatidadCc_TextChanged" Value="0" EmptyMessage="Cantidad" MinValue="0" MaxValue="9999999.99" ShowSpinButtons="true" NumberFormat-DecimalDigits="2" Skin="MetroTouch"></telerik:RadNumericTextBox>
                                                     </div>
                                                     <div class="col-lg-2 col-sm-2">
                                                        <asp:Label ID="Label39" runat="server" Text="Cost. Unit.:" CssClass="textoBold"></asp:Label>
                                                        <telerik:RadNumericTextBox RenderMode="Lightweight" runat="server" ID="radCuCc" CssClass="input-mini" AutoPostBack="true" OnTextChanged="radCuCc_TextChanged" Value="0" EmptyMessage="Cost. Unit." MinValue="0" MaxValue="9999999999.99" ShowSpinButtons="true" NumberFormat-DecimalDigits="2" Skin="MetroTouch"></telerik:RadNumericTextBox>
                                                     </div>
                                                     <div class="col-lg-2 col-sm-2">
                                                        <asp:Label ID="Label40" runat="server" Text="Descuento:" CssClass="textoBold"></asp:Label>
                                                        <telerik:RadNumericTextBox RenderMode="Lightweight" runat="server" ID="radDectoCc" CssClass="input-mini" AutoPostBack="true" OnTextChanged="radDesctoCc_TextChanged" Value="0" EmptyMessage="Descuento" MinValue="0" MaxValue="100" ShowSpinButtons="true" NumberFormat-DecimalDigits="2" Skin="MetroTouch"></telerik:RadNumericTextBox>
                                                    </div>
                                                     <div class="col-lg-2 col-sm-2">
                                                        <asp:Label ID="Label41" runat="server" Text="Importe:" CssClass="textoBold"></asp:Label>
                                                        <asp:Label ID="lblImporteCc" runat="server"  CssClass="textoBold"></asp:Label>
                                                    </div>
                                                    <div class="col-lg-2 col-sm-2">
                                                        <asp:Label ID="Label47" runat="server" Text="Pago:" CssClass="textoBold"></asp:Label>
                                                        <asp:RadioButtonList ID="rblPago" runat="server" RepeatDirection="Horizontal" RepeatColumns="3">
                                                            <asp:ListItem Text="Ninguno" Value="-1" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="Contado" Value="0" ></asp:ListItem>
                                                            <asp:ListItem Text="Crédito" Value="1"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </div>
                                                </div>
                                                <div class="row pad1m">
                                                    <div class="col-lg-12 col-sm-12 text-center">
                                                        <asp:LinkButton ID="lnkAceptarCc" runat="server" CssClass="btn btn-success" OnClick="lnkAceptarCc_Click"><i class="fa fa-check"></i>&nbsp;<span>Aceptar</span></asp:LinkButton>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-lg-12 col-sm-12"><asp:Label ID="lblErrorCc" runat="server"  CssClass="errores"></asp:Label></div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </telerik:RadPageView>
                                    <telerik:RadPageView runat="server" ID="RadPageView6">
                                         <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                            <ContentTemplate>
                                                <div class="row">
                                                    <div class="col-lg-12 col-sm-12">

                                                        <telerik:RadGrid RenderMode="Lightweight" runat="server" ID="RadGrid6" AllowSorting="True" Skin="Metro" OnItemDataBound="RadGrid6_ItemDataBound"
                                                            AllowPaging="true" PageSize="100">
                                                            <MasterTableView AutoGenerateColumns="False" DataKeyNames="renglon" EnableHeaderContextMenu="true">
                                                                <Columns>
                                                                    <telerik:GridBoundColumn DataField="renglon" HeaderText="Clave" SortExpression="renglon" UniqueName="renglon" Visible="false"/>
                                                                    <telerik:GridBoundColumn DataField="Factura" HeaderText="Factura" SortExpression="Factura" UniqueName="Factura"/>                                                            
                                                                    <telerik:GridBoundColumn DataField="fecha" HeaderText="Fecha" SortExpression="fecha" UniqueName="fecha" DataFormatString="{0:yyyy-MM-dd}"/>                                                            
                                                                    <telerik:GridBoundColumn DataField="razon_social" HeaderText="Proveedor" SortExpression="razon_social" UniqueName="razon_social"/>                                                            
                                                                    <telerik:GridBoundColumn DataField="cantidad" HeaderText="Cantidad" SortExpression="cantidad" UniqueName="cantidad"/>                                                            
                                                                    <telerik:GridBoundColumn DataField="descripcion" HeaderText="Descripción" SortExpression="descripcion" UniqueName="descripcion"/>
                                                                    <telerik:GridBoundColumn DataField="Costo_Unitario" HeaderText="C.U." SortExpression="Costo_Unitario" UniqueName="Costo_Unitario"/>
                                                                    <telerik:GridBoundColumn DataField="Descuento" HeaderText="Dcto." SortExpression="Descuento" UniqueName="Descuento" />
                                                                    <telerik:GridBoundColumn DataField="Importe" HeaderText="Importe" SortExpression="Importe" UniqueName="Importe"/>
                                                                    <telerik:GridTemplateColumn  HeaderText="Seleccionar">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="btnRegistroPintura" runat="server" OnClick="btnRegistroPintura_Click" CommandArgument='<%# Eval("renglon") %>' CssClass="btn btn-success"><i class="fa fa-check"></i></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>                                                                                                                        
                                                                </Columns>
                                                                <NoRecordsTemplate>
                                                                    <asp:Label ID="noRec3" Text="No existe información de almacén" CssClass="errores" runat="server"></asp:Label>
                                                                </NoRecordsTemplate>                                                        
                                                            </MasterTableView>                                                    
                                                            <ClientSettings>
                                                                <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true"></Scrolling>                                                                
                                                            </ClientSettings>                        
                                                            <PagerStyle PageSizeControlType="RadDropDownList" Mode="NextPrevAndNumeric"></PagerStyle>
                                                        </telerik:RadGrid>
                                                        <%-- %><telerik:RadGrid RenderMode="Lightweight" runat="server" ID="RadGrid6" DataSourceID="SqlDataSource8" AllowSorting="True" Skin="Metro" OnItemDataBound="RadGrid6_ItemDataBound"
                                                            AllowPaging="true" PageSize="20">
                                                            <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_solicitud,id_detalle" DataSourceID="SqlDataSource8"
                                                                EnableHeaderContextMenu="true">
                                                                <Columns>                                                                                                                 
                                                                    <telerik:GridBoundColumn DataField="id_solicitud" HeaderText="Solicitud" SortExpression="id_solicitud" UniqueName="id_solicitud" Visible="false" />               
                                                                    <telerik:GridBoundColumn DataField="id_detalle" HeaderText="Detalle" SortExpression="id_detalle" UniqueName="id_detalle" Visible="false" />
                                                                    <telerik:GridBoundColumn DataField="cantidad" HeaderText="Cantidad" SortExpression="cantidad" UniqueName="cantidad" />
                                                                    <telerik:GridBoundColumn DataField="descripcion" HeaderText="Descripción" SortExpression="descripcion" UniqueName="descripcion"/>
                                                                    <telerik:GridBoundColumn DataField="importe" HeaderText="Monto" SortExpression="importe" UniqueName="importe" DataFormatString="{0:C2}"/>                                                                                                                                     
                                                                </Columns> 
                                                                <NoRecordsTemplate>
                                                                    <asp:Label ID="noRec5" Text="No existe información de artículos extras registrados" CssClass="errores" runat="server"></asp:Label>
                                                                </NoRecordsTemplate>                                                       
                                                            </MasterTableView>                                                    
                                                            <ClientSettings>
                                                                <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true"></Scrolling>                                                        
                                                            </ClientSettings>                                                                                    
                                                            <PagerStyle PageSizeControlType="RadDropDownList" Mode="NextPrevAndNumeric"></PagerStyle>
                                                        </telerik:RadGrid>
                                                        <asp:SqlDataSource ID="SqlDataSource8" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select rp.id_empresa,rp.id_taller,rp.no_orden,dp.ano,dp.id_solicitud, dp.id_detalle,dp.cantidad,dp.descripcion,dp.importe
                                                                from Registro_Pinturas rp, Detalle_Registro_Pintura dp
                                                                where rp.id_empresa=@empresa and rp.id_taller=@taller and rp.no_orden=@orden and dp.id_solicitud=rp.id_solicitud and dp.ano=rp.ano and rp.ano=@ano">
                                                            <SelectParameters>
                                                                <asp:ControlParameter ControlID="lblOrden" Name="orden" PropertyName="Text" />                                                                
                                                                <asp:ControlParameter ControlID="lblEmpresa" Name="empresa" PropertyName="Text" />
                                                                <asp:ControlParameter ControlID="lblTaller" Name="taller" PropertyName="Text" />
                                                                <asp:ControlParameter ControlID="lblAno" Name="ano" PropertyName="Text" />
                                                            </SelectParameters>
                                                        </asp:SqlDataSource>--%>
                                                    </div>
                                                </div>
                                                <div class="row pad1m">
                                                    <h4>
                                                        <div class="col-lg-3 col-sm-3"></div>
                                                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label42" runat="server" CssClass="textoBold" Text="Total Almacen:" ></asp:Label></div>
                                                        <div class="col-lg-3 col-sm-3 text-right"><asp:Label ID="lblTotExtras" runat="server" CssClass="textoBold" ></asp:Label></div>
                                                        <div class="col-lg-3 col-sm-3"></div>
                                                    </h4>
                                                </div>
                                            </ContentTemplate>
                                         </asp:UpdatePanel>
                                    </telerik:RadPageView>
                                </telerik:RadMultiPage>
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
                                        <asp:Label ID="Label3" runat="server" Text="Perfil:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                                        <asp:Label ID="ddlPerfil" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-lg-4 col-sm-4 text-center">
                                        <asp:Label ID="Label7" runat="server" Text="Siniestro:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                                        <asp:Label ID="lblSiniestro" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-lg-4 col-sm-4 text-center">
                                        <asp:Label ID="Label11" runat="server" Text="Deducible:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                                        <asp:Label ID="lblDeducible" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="row colorBlanco textoBold">
                                    <div class="col-lg-4 col-sm-4 text-center">
                                        <asp:Label ID="Label5" runat="server" Text="Total Orden:" CssClass="colorEtiqueta"
                                            Visible="false"></asp:Label>&nbsp;&nbsp;
                                        <asp:Label ID="lblTotOrden" runat="server" Visible="false"></asp:Label>
                                    </div>
                                    <div class="col-lg-4 col-sm-4 text-center">
                                        <asp:Label ID="Label1" runat="server" Text="Promesa:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                                        <asp:Label ID="lblEntregaEstimada" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-lg-4 col-sm-4 text-center">
                                        <asp:Label ID="lblPorcDeduEti" runat="server" Text="% Deducible:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                                        <asp:Label ID="lblPorcDedu" runat="server"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>                    
                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
    </telerik:RadWindow>                
</asp:Content>

