<%@ Page Title="" Language="C#" MasterPageFile="~/AdmOrdenes.master" AutoEventWireup="true" CodeFile="catImpuestos.aspx.cs" Inherits="catImpuestos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" />    
 
    <br /><br />

    <telerik:RadTabStrip RenderMode="Lightweight" ID="radTabOpciones" runat="server"  SelectedIndex="4" MultiPageID="multiOpciones" Skin="MetroTouch" Align="Left" Orientation="HorizontalTop" CssClass="col-lg-12 col-sm-12">
                    <Tabs>                        
                        <telerik:RadTab Text="Impuestos Retenidos"></telerik:RadTab>                        
                        <telerik:RadTab Text="Impuestos Traslado"></telerik:RadTab>                       
                    </Tabs>
    </telerik:RadTabStrip>
    
  
    
     <telerik:RadMultiPage runat="server" ID="multiOpciones" SelectedIndex="0" CssClass="col-lg-12 col-sm-12" SkinID="MetroTouch">
         <%-- Nueva unidad Emisor --%>



           <telerik:RadPageView runat="server" ID="detalleEmisor" CssClass="text-left">
                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                 <ContentTemplate>
                 <%--   WINDOW AGREGA RETENIDOS --%>
                        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
    <telerik:RadFormDecorator ID="RadFormDecorator1" RenderMode="Lightweight" runat="server" DecoratedControls="Buttons" />
                  <telerik:RadWindow RenderMode="Lightweight" ID="UnidadImpuestoR"  EnableShadow="true" Skin="Metro"
                Behaviors="Move,Close,Resize,Pin" VisibleOnPageLoad="false" ShowContentDuringLoad="false" DestroyOnClose="true" 
                Animation="Fade" runat="server" Modal="true" Width="900px" Height="300px" Style="z-index: 1000;">
        <ContentTemplate>
            <asp:UpdatePanel ID="UpdatePanelEmi" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pnlPrincipal" runat="server" CssClass="ancho100 text-center">
                        <div class="col-lg-12 col-sm-12 text-center">
                            <asp:Label ID="lblErrorRetencion" runat="server"></asp:Label>
                            <asp:Label ID="lblEditaAgrega" Visible="false" runat="server"></asp:Label>
                        </div>                 
                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label4" runat="server" Text="Clave:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtClave" runat="server" CssClass="input-xxlarge" Enabled="false" MaxLength="128" />
                            
                                                     
                        </div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label5" runat="server" Text="Nombre:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtNombre" runat="server" MaxLength="100" CssClass="input-xxlarge"></asp:TextBox>
                        </div>

                         <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label21" runat="server" Text="Descripcion:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtDescripcion" runat="server" MaxLength="100" CssClass="input-xxlarge"></asp:TextBox>
                        </div>

                         <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label22" runat="server" Text="Tasa:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtTasa" runat="server" MaxLength="100" CssClass="input-xxlarge"></asp:TextBox>
                        </div>

                         <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label23" runat="server" Text="Cuenta:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtCuenta" runat="server" MaxLength="100" CssClass="input-xxlarge"></asp:TextBox>
                        </div>


                         <div class="col-lg-6 col-sm-6 text-center pad1m">
                            <asp:LinkButton ID="lnkAgregarNuevo" runat="server" CssClass="btn btn-success t14" OnClick="lnkAgregarNuevo_Click" ValidationGroup="agrega" ><i class="fa fa-check-circle"></i>&nbsp; Agregar</asp:LinkButton>
                        </div>
                        <div class="col-lg-6 col-sm-6 text-center pad1m">
                            <asp:LinkButton ID="lnkCancelarNuevo" runat="server" CssClass="btn btn-danger t14" ><i class="fa fa-remove"></i>&nbsp; Cancelar</asp:LinkButton>
                        </div>
                    </asp:Panel>
                    <asp:UpdateProgress ID="updProgEmi" runat="server" AssociatedUpdatePanelID="UpdatePanelEmi">
                        <ProgressTemplate>
                            <asp:Panel ID="pnlMaskLoadEmi" runat="server" CssClass="maskLoad" />
                            <asp:Panel ID="pnlCargandoEmi" runat="server" CssClass="pnlPopUpLoad">
                                <asp:Image ID="imgLoadEmi" runat="server" ImageUrl="~/img/loading.gif" CssClass="ancho100" />
                            </asp:Panel>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
    </telerik:RadWindow>



                             <div class="row">
        <div class="col-lg-12 col-sm-12 text-center alert-info">
            <h3>
                <i class="fa fa-money"></i>&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label10" runat="server" Text="Retenidos"></asp:Label>
            </h3>
        </div>
    </div>

   <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>            
            <div class="col-lg-12 col-sm-12 text-right">
                   <div class="col-lg-3 col-sm-3 text-center">
                <asp:LinkButton ID="btnAgregar" runat="server" CssClass="btn btn-info t14" ToolTip="Agregar" OnClick="btnAgregar_Click"><i class="fa fa-plus-circle"></i>&nbsp;<span>Agregar</span></asp:LinkButton>
                     </div>
                <div class="col-lg-3 col-sm-3 text-center">
                <asp:LinkButton ID="btnEditar" runat="server" Visible="false" ToolTip="Editar" CssClass="btn btn-warning t14" onclick="btnEditar_Click"><i class="fa fa-edit"></i>&nbsp;<span>Editar</span></asp:LinkButton>
                </div>
                 <div class="col-lg-3 col-sm-3 text-center">
                <asp:LinkButton ID="btnEliminar" runat="server" Visible="false" ToolTip="Eliminar" CssClass="btn btn-danger" onclick="btnEliminar_Click"><i class="fa fa-times"></i>&nbsp;<span>Eliminar</span></asp:LinkButton>
                     </div>
                <div class="col-lg-3 col-sm-3 text-center">
                <asp:LinkButton ID="btnSelec" runat="server" Visible="false" ToolTip="Seleccionar" CssClass="btn btn-primary" ><i class="fa fa-check"></i>&nbsp;<span>Seleccionar</span></asp:LinkButton>
                    </div>
            </div>
            <div class="row pad1m">
                <div class="col-lg-12 col-sm-12 text-center">
                    <asp:Label ID="lblError" runat="server" CssClass="errores" />
                </div>
            </div>

            <asp:Panel ID="pnlContenido" CssClass="col-lg-12 col-sm-12" runat="server" ScrollBars="Auto">
                <asp:Panel ID="pnlCatalogos" runat="server" CssClass="col-lg-12 col-sm-12" ScrollBars="Auto">
                    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" EnableAJAX="true">
                    <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" AllowFilteringByColumn="true"  OnSelectedIndexChanged="RadGrid1_SelectedIndexChanged" runat="server" EnableHeaderContextMenu="true" Culture="es-Mx" Skin="Metro" 
                                EnableHeaderContextFilterMenu="true" AllowPaging="True" PagerStyle-AlwaysVisible="true" DataSourceID="SqlDataSource1" AllowSorting="true" GroupingEnabled="false" PageSize="50" >
                        <MasterTableView DataSourceID="SqlDataSource1" AutoGenerateColumns="false" DataKeyNames="ID_Ret">
                            <Columns>
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="ID_Ret" FilterControlAltText="Filtro Clave" HeaderText="Clave" SortExpression="ID_Ret" UniqueName="ID_Ret" Resizable="true"  />
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="RetNombre" FilterControlAltText="Filtro Nombre" HeaderText="Nombre" SortExpression="RetNombre" UniqueName="RetNombre" Resizable="true" />
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="RetDescrip" FilterControlAltText="Filtro Descripcion" HeaderText="Descripcion" SortExpression="RetDescrip" UniqueName="RetDescrip" Resizable="true" />
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="RetTasa" FilterControlAltText="Filtro Tasa" HeaderText="Tasa" SortExpression="RetTasa" UniqueName="RetTasa" Resizable="true" />
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="RetCuentaConta" FilterControlAltText="Filtro Cuenta" HeaderText="Cuenta" SortExpression="RetCuentaConta" UniqueName="RetCuentaConta" Resizable="true" />
                            </Columns>
                        </MasterTableView>
                        <ClientSettings AllowKeyboardNavigation="true" EnablePostBackOnRowClick="true">
                            <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="1"></Scrolling>
                            <Selecting AllowRowSelect="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </telerik:RadAjaxPanel>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" SelectCommand="select * from impRetenidos_f " ConnectionString="<%$ ConnectionStrings:PVW %>">
                </asp:SqlDataSource>
                </asp:Panel>
            </asp:Panel>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                <ProgressTemplate>
                    <asp:Panel ID="pnlMaskLoad" runat="server" CssClass="maskLoad">
                    </asp:Panel>
                    <asp:Panel ID="pnlCargando" runat="server" CssClass="pnlPopUpLoad">
                        <asp:Image ID="imgLoad" runat="server" ImageUrl="~/img/loading.gif" CssClass="ancho100" />
                    </asp:Panel>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </ContentTemplate>
    </asp:UpdatePanel>
                 </ContentTemplate>
                    </asp:UpdatePanel>
                  
                </telerik:RadPageView>

         <telerik:RadPageView runat="server" ID="VtaTaller"> 
                    <asp:UpdatePanel ID="updVtaTaller" runat="server">
              <ContentTemplate>
                   <telerik:RadWindow ID="UnidadSAT" Title="Unidad SAT" EnableShadow="true" Skin="Metro"
        Behaviors="Move,Close,Resize,Pin" VisibleOnPageLoad="false" ShowContentDuringLoad="false" DestroyOnClose="true" 
        Animation="Fade" runat="server" Modal="true" Width="900px" Height="300px" Style="z-index: 1000;">
                       <ContentTemplate>
                            <asp:Panel ID="UpdatePanelSat" runat="server" CssClass="ancho100 text-center">
                        <div class="col-lg-12 col-sm-12 text-center">
                            <asp:Label ID="Label1" runat="server" CssClass="errores"></asp:Label>
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="agrega" CssClass="errores" DisplayMode="List" />
                        </div>                 
                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label7" runat="server" Text="Clave:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtClaveTras" runat="server" CssClass="input-xxlarge" MaxLength="128" />
                            
                                                     
                        </div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label8" runat="server" Text="Nombre:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtNombreTras" runat="server" MaxLength="100" CssClass="input-xxlarge"></asp:TextBox>
                        </div>

                         <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label9" runat="server" Text="Descripcion:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtDescTras" runat="server" MaxLength="100" CssClass="input-xxlarge"></asp:TextBox>
                        </div>

                         <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label11" runat="server" Text="Tasa:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtTasaTras" runat="server" MaxLength="100" CssClass="input-xxlarge"></asp:TextBox>
                        </div>

                         <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label12" runat="server" Text="Cuenta:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtCuentaTras" runat="server" MaxLength="100" CssClass="input-xxlarge"></asp:TextBox>
                        </div>



                         <div class="col-lg-6 col-sm-6 text-center pad1m">
                            <asp:LinkButton ID="AgregarNuevoSat" runat="server" CssClass="btn btn-success t14" OnClick="lnkAgregarNuevoSAT_Click" ><i class="fa fa-check-circle"></i>&nbsp; Agregar</asp:LinkButton>
                        </div>
                        <div class="col-lg-6 col-sm-6 text-center pad1m">
                            <asp:LinkButton ID="LinkButton3" runat="server" CssClass="btn btn-danger t14" ><i class="fa fa-remove"></i>&nbsp; Cancelar</asp:LinkButton>
                        </div>
                    </asp:Panel>
                        
                       </ContentTemplate>
                   </telerik:RadWindow>
                   
                   <telerik:RadWindow ID="UnidadSATMOD" Title="Edita Unidad SAT" EnableShadow="true" Skin="Metro"
        Behaviors="Move,Resize,Pin,Close" VisibleOnPageLoad="false"  DestroyOnClose="true" 
        Animation="Fade" runat="server" Modal="true" Width="900px" Height="300px" Style="z-index: 1000;">
                        <ContentTemplate>
                             <asp:Panel ID="UpdatePanelSatEDt" runat="server" CssClass="ancho100 text-center">
                        <div class="col-lg-12 col-sm-12 text-center">
                            <asp:Label ID="Label13" runat="server" CssClass="errores"></asp:Label>
                            <asp:ValidationSummary ID="ValidationSummary3" runat="server" ValidationGroup="agrega" CssClass="errores" DisplayMode="List" />
                        </div>                 
                       <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label15" runat="server" Text="Clave:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtClaveTrasEDT" runat="server" CssClass="input-xxlarge" MaxLength="128" />
                            
                                                     
                        </div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label16" runat="server" Text="Nombre:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtNombreTrasEDT" runat="server" MaxLength="100" CssClass="input-xxlarge"></asp:TextBox>
                        </div>

                         <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label17" runat="server" Text="Descripcion:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtDescTrasEDT" runat="server" MaxLength="100" CssClass="input-xxlarge"></asp:TextBox>
                        </div>

                         <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label18" runat="server" Text="Tasa:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtTasaTrasEDT" runat="server" MaxLength="100" CssClass="input-xxlarge"></asp:TextBox>
                        </div>

                         <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label19" runat="server" Text="Cuenta:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtCuentaTrasEDT" runat="server" MaxLength="100" CssClass="input-xxlarge"></asp:TextBox>
                        </div>


                         <div class="col-lg-6 col-sm-6 text-center pad1m">
                            <asp:LinkButton ID="LinkButton4" runat="server" CssClass="btn btn-success t14"  OnClick="BtnActualizar_Click2" ValidationGroup="agrega" ><i class="fa fa-check-circle"></i>&nbsp; Agregar</asp:LinkButton>
                        </div>
                        <div class="col-lg-6 col-sm-6 text-center pad1m">
                            <asp:LinkButton ID="LinkButton5" runat="server" CssClass="btn btn-danger t14" ><i class="fa fa-remove"></i>&nbsp; Cancelar</asp:LinkButton>
                        </div>
                    </asp:Panel>
                         
                        </ContentTemplate>
                    </telerik:RadWindow>
                    <div class="row">
        <div class="col-lg-12 col-sm-12 text-center alert-info">
            <h3>
                <i class="fa fa-money"></i>&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label14" runat="server" Text="Traslado"></asp:Label>
            </h3>
        </div>
    </div>

   <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>            
            <div class="col-lg-12 col-sm-12 text-right">
                   <div class="col-lg-3 col-sm-3 text-center">
                <asp:LinkButton ID="AgregarSat" runat="server" CssClass="btn btn-info t14" ToolTip="Agregar" OnClick="AgregarSat_Click"><i class="fa fa-plus-circle"></i>&nbsp;<span>Agregar</span></asp:LinkButton>
                     </div>
                <div class="col-lg-3 col-sm-3 text-center">
                <asp:LinkButton ID="LinkButton7" runat="server" Visible="false" ToolTip="Editar" CssClass="btn btn-warning t14" onclick="btnEditar_Click2"><i class="fa fa-edit"></i>&nbsp;<span>Editar</span></asp:LinkButton>
                </div>
                 <div class="col-lg-3 col-sm-3 text-center">
                <asp:LinkButton ID="LinkButton8" runat="server" Visible="false" ToolTip="Eliminar" CssClass="btn btn-danger" onclick="btnEliminar_Click2"><i class="fa fa-times"></i>&nbsp;<span>Eliminar</span></asp:LinkButton>
                     </div>
                <div class="col-lg-3 col-sm-3 text-center">
                <asp:LinkButton ID="LinkButton9" runat="server" Visible="false" ToolTip="Seleccionar" CssClass="btn btn-primary" onclick="btnEliminar_Click"><i class="fa fa-check"></i>&nbsp;<span>Seleccionar</span></asp:LinkButton>
                    </div>
            </div>
            <div class="row pad1m">
                <div class="col-lg-12 col-sm-12 text-center">
                    <asp:Label ID="Label20" runat="server" CssClass="errores" />
                </div>
            </div>

            <asp:Panel ID="Panel3" CssClass="col-lg-12 col-sm-12" runat="server" ScrollBars="Auto">
                <asp:Panel ID="Panel4" runat="server" CssClass="col-lg-12 col-sm-12" ScrollBars="Auto">
                    <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" EnableAJAX="true">
                    <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid2" AllowFilteringByColumn="true"  OnSelectedIndexChanged="RadGrid2_SelectedIndexChanged" runat="server" EnableHeaderContextMenu="true" Culture="es-Mx" Skin="Metro" 
                                EnableHeaderContextFilterMenu="true" AllowPaging="True" PagerStyle-AlwaysVisible="true" DataSourceID="SqlDataSource2" AllowSorting="true" GroupingEnabled="false" PageSize="50" >
                        <MasterTableView DataSourceID="SqlDataSource2" AutoGenerateColumns="false" DataKeyNames="ID_Tras">
                            <Columns>
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="ID_Tras" FilterControlAltText="Filtro Clave" HeaderText="Clave" SortExpression="ID_Tras" UniqueName="ID_Tras" Resizable="true"  />
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="TrasNombre" FilterControlAltText="Filtro Nombre" HeaderText="Nombre" SortExpression="TrasNombre" UniqueName="TrasNombre" Resizable="true" />
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="TrasDescrip" FilterControlAltText="Filtro Descripcion" HeaderText="Descripcion" SortExpression="TrasDescrip" UniqueName="TrasDescrip" Resizable="true" />
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="TrasTasa" FilterControlAltText="Filtro Tasa" HeaderText="Tasa" SortExpression="TrasTasa" UniqueName="TrasTasa" Resizable="true" />
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="TrasCuentaConta" FilterControlAltText="Filtro Cuenta" HeaderText="Cuenta" SortExpression="TrasCuentaConta" UniqueName="TrasCuentaConta" Resizable="true" />
                            </Columns>
                        </MasterTableView>
                        <ClientSettings AllowKeyboardNavigation="true" EnablePostBackOnRowClick="true">
                            <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="1"></Scrolling>
                            <Selecting AllowRowSelect="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </telerik:RadAjaxPanel>
                <asp:SqlDataSource ID="SqlDataSource2" runat="server" SelectCommand="select * from ImpTrasladado_f " ConnectionString="<%$ ConnectionStrings:PVW %>">
                </asp:SqlDataSource>
                </asp:Panel>
            </asp:Panel>
          
        </ContentTemplate>
    </asp:UpdatePanel>

              </ContentTemplate>
                    </asp:UpdatePanel>
                </telerik:RadPageView>

     </telerik:RadMultiPage>
   

</asp:Content>

