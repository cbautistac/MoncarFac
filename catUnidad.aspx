<%@ Page Title="" Language="C#" MasterPageFile="~/AdmOrdenes.master" AutoEventWireup="true" CodeFile="catUnidad.aspx.cs" Inherits="catUnidad" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
      <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" />    
 
    <br /><br />

    <telerik:RadTabStrip RenderMode="Lightweight" ID="radTabOpciones" runat="server"  SelectedIndex="4" MultiPageID="multiOpciones" Skin="MetroTouch" Align="Left" Orientation="HorizontalTop" CssClass="col-lg-12 col-sm-12">
                    <Tabs>                        
                        <telerik:RadTab Text="Catalogo de Emisor"></telerik:RadTab>                        
                        <telerik:RadTab Text="Catalogo del SAT"></telerik:RadTab>                       
                    </Tabs>
    </telerik:RadTabStrip>
    
  
    
     <telerik:RadMultiPage runat="server" ID="multiOpciones" SelectedIndex="0" CssClass="col-lg-12 col-sm-12" SkinID="MetroTouch">
         <%-- Nueva unidad Emisor --%>



           <telerik:RadPageView runat="server" ID="detalleEmisor" CssClass="text-left">
                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                 <ContentTemplate>

                        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
    <telerik:RadFormDecorator ID="RadFormDecorator1" RenderMode="Lightweight" runat="server" DecoratedControls="Buttons" />

                          <telerik:RadWindow RenderMode="Lightweight" ID="UnidadEmisor" Title="Nueva Unidad" EnableShadow="true" Skin="Metro"
        Behaviors="Move,Close,Resize,Pin" VisibleOnPageLoad="false" ShowContentDuringLoad="false" DestroyOnClose="true" 
        Animation="Fade" runat="server" Modal="true" Width="900px" Height="250px" Style="z-index: 1000;">
        <ContentTemplate>
            <asp:UpdatePanel ID="UpdatePanelEmi" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pnlPrincipal" runat="server" CssClass="ancho100 text-center">
                        <div class="col-lg-12 col-sm-12 text-center">
                            <asp:Label ID="lblErrorNuevo" runat="server"></asp:Label>
                           
                        </div>                 
                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label4" runat="server" Text="Clave Unidad:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtClaveMon" runat="server" CssClass="input-xxlarge" MaxLength="128" />
                            
                                                     
                        </div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label5" runat="server" Text="Descripción:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtDes" runat="server" MaxLength="100" CssClass="input-xxlarge"></asp:TextBox>
                            
                           
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
                            <%-- Modifica Empresa --%>

                            <telerik:RadWindow RenderMode="Lightweight" ID="modalModifica" Title="Edita Unidad" EnableShadow="true" Skin="Metro"
        Behaviors="Move,Resize,Pin,Close" VisibleOnPageLoad="false" ShowContentDuringLoad="false" DestroyOnClose="true" 
        Animation="Fade" runat="server" Modal="true" Width="900px" Height="250px" Style="z-index: 1000;">
        <ContentTemplate>
            <asp:UpdatePanel ID="UpdatePanelEmiMod" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="Panel1" runat="server" CssClass="ancho100 text-center">
                        <div class="col-lg-12 col-sm-12 text-center">
                            <asp:Label ID="Label2" runat="server" CssClass="errores"></asp:Label>
                            <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="agrega" CssClass="errores" DisplayMode="List" />
                        </div>                 
                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label3" runat="server" Text="Clave Unidad:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtClaveMonEdt" runat="server" CssClass="input-xxlarge" MaxLength="128" />
                           
                        </div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label6" runat="server" Text="Descripcion:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtDesEdt" runat="server" MaxLength="100" CssClass="input-xxlarge"></asp:TextBox>
                           
                        </div>

                         <div class="col-lg-6 col-sm-6 text-center pad1m">
                            <asp:LinkButton ID="BtnActualizar" runat="server" CssClass="btn btn-success t14"  ValidationGroup="agrega"  OnClick="BtnActualizar_Click" ><i class="fa fa-check-circle"></i>&nbsp; Agregar</asp:LinkButton>
                        </div>
                        <div class="col-lg-6 col-sm-6 text-center pad1m">
                            <asp:LinkButton ID="LinkButton2" runat="server" CssClass="btn btn-danger t14" ><i class="fa fa-remove"></i>&nbsp; Cancelar</asp:LinkButton>
                        </div>
                    </asp:Panel>
                  
                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
    </telerik:RadWindow>
                             <div class="row">
        <div class="col-lg-12 col-sm-12 text-center alert-info">
            <h3>
                <i class="fa fa-money"></i>&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label10" runat="server" Text="Unidad"></asp:Label>
            </h3>
        </div>
    </div>

       
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
                <asp:LinkButton ID="btnSelec" runat="server" Visible="false" ToolTip="Seleccionar" CssClass="btn btn-primary" onclick="btnEliminar_Click"><i class="fa fa-check"></i>&nbsp;<span>Seleccionar</span></asp:LinkButton>
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
                        <MasterTableView DataSourceID="SqlDataSource1" AutoGenerateColumns="false" DataKeyNames="idunid">
                            <Columns>
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="idunid" FilterControlAltText="Filtro Clave" HeaderText="Clave" SortExpression="idunid" UniqueName="idunid" Resizable="true"  />
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="uniddesc" FilterControlAltText="Filtro DEscripcion" HeaderText="Descripcion" SortExpression="uniddesc" UniqueName="uniddesc" Resizable="true" />
                             
                            </Columns>
                        </MasterTableView>
                        <ClientSettings AllowKeyboardNavigation="true" EnablePostBackOnRowClick="true">
                            <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="1"></Scrolling>
                            <Selecting AllowRowSelect="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </telerik:RadAjaxPanel>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" SelectCommand="select * from unidades_f " ConnectionString="<%$ ConnectionStrings:PVW %>">
                </asp:SqlDataSource>
                </asp:Panel>
            </asp:Panel>
                 </ContentTemplate>
                    </asp:UpdatePanel>
                  
                </telerik:RadPageView>

         <telerik:RadPageView runat="server" ID="VtaTaller"> 
                    <asp:UpdatePanel ID="updVtaTaller" runat="server">
              <ContentTemplate>
                   <telerik:RadWindow ID="UnidadSAT" Title="Unidad SAT" EnableShadow="true" Skin="Metro"
        Behaviors="Move,Close,Resize,Pin" VisibleOnPageLoad="false" ShowContentDuringLoad="false" DestroyOnClose="true" 
        Animation="Fade" runat="server" Modal="true" Width="1000px" Height="350px" Style="z-index: 1000;">
                       <ContentTemplate>
                            <asp:Panel ID="UpdatePanelSat" runat="server" CssClass="ancho100 text-center">
                        <div class="col-lg-12 col-sm-12 text-center">
                            <asp:Label ID="Label1" runat="server" CssClass="errores"></asp:Label>
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="agrega" CssClass="errores" DisplayMode="List" />
                        </div>                 
                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="lblclave" runat="server" Text="Clave Unidad:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtUnidadSat" runat="server" CssClass="input-xxlarge" MaxLength="128" />
                           
                        </div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label8" runat="server" Text="Nombre:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtNombre" runat="server" MaxLength="100" CssClass="input-xxlarge"></asp:TextBox>
                           
                        </div>

                             <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label7" runat="server" Text="Descripcion:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtDesSat" runat="server" CssClass="input-xxlarge" MaxLength="128" />
                           
                        </div>

                                 <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label9" runat="server" Text="Fecha Inicio:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                             <telerik:RadDatePicker ID="txtFecha_ini" runat="server"   >
                                                 <DateInput ID="DateInput3" runat="server" DateFormat="yyyy/MM/dd">
                                                 </DateInput>
                                             </telerik:RadDatePicker>
                        </div>

                                <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label11" runat="server" Text="Fecha Fin:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                             <telerik:RadDatePicker ID="txtFecha_fin" runat="server"   >
                                                 <DateInput ID="DateInput1" runat="server" DateFormat="yyyy/MM/dd">
                                                 </DateInput>
                                             </telerik:RadDatePicker>
                        </div>

                                 <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label12" runat="server" Text="Simbolo:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtSimbolo" runat="server" CssClass="input-xxlarge" MaxLength="128" />
                           
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
        Animation="Fade" runat="server" Modal="true" Width="900px" Height="250px" Style="z-index: 1000;">
                        <ContentTemplate>
                             <asp:Panel ID="UpdatePanelSatEDt" runat="server" CssClass="ancho100 text-center">
                        <div class="col-lg-12 col-sm-12 text-center">
                            <asp:Label ID="Label13" runat="server" CssClass="errores"></asp:Label>
                            <asp:ValidationSummary ID="ValidationSummary3" runat="server" ValidationGroup="agrega" CssClass="errores" DisplayMode="List" />
                        </div>                 
                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="txtUnidadEDT" runat="server" Text="Clave Unidad:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="TextBox1" runat="server" CssClass="input-xxlarge" MaxLength="128" />
                           
                        </div>

                        <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label15" runat="server" Text="Nombre:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtNombreEDT" runat="server" MaxLength="100" CssClass="input-xxlarge"></asp:TextBox>
                           
                        </div>

                             <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label16" runat="server" Text="Descripcion:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtDescEDT" runat="server" CssClass="input-xxlarge" MaxLength="128" />
                           
                        </div>

                                 <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label17" runat="server" Text="Fecha Inicio:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                             <telerik:RadDatePicker ID="txtFechainiEDT" runat="server"   >
                                                 <DateInput ID="DateInput2" runat="server" DateFormat="yyyy/MM/dd">
                                                 </DateInput>
                                             </telerik:RadDatePicker>
                        </div>

                                <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label18" runat="server" Text="Fecha Fin:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                             <telerik:RadDatePicker ID="txtFechafinEDT" runat="server"   >
                                                 <DateInput ID="DateInput4" runat="server" DateFormat="yyyy/MM/dd">
                                                 </DateInput>
                                             </telerik:RadDatePicker>
                        </div>

                                 <div class="col-lg-3 col-sm-3 text-left"><asp:Label ID="Label19" runat="server" Text="Simbolo:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtSimboloEDT" runat="server" CssClass="input-xxlarge" MaxLength="128" />
                           
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
                <asp:Label ID="Label14" runat="server" Text="Unidad Sat"></asp:Label>
            </h3>
        </div>
    </div>

  
                  
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
                        <MasterTableView DataSourceID="SqlDataSource2" AutoGenerateColumns="false" DataKeyNames="ClaveUnidad">
                            <Columns>
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="ClaveUnidad" FilterControlAltText="Filtro Clave" HeaderText="Clave" SortExpression="ClaveUnidad" UniqueName="ClaveUnidad" Resizable="true"  />
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="Nombre" FilterControlAltText="Filtro Nombre" HeaderText="Nombre" SortExpression="Nombre" UniqueName="Nombre" Resizable="true" />
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="Descripcion" FilterControlAltText="Filtro Descripcion" HeaderText="Descripcion" SortExpression="Descripcion" UniqueName="Descripcion" Resizable="true" />

                             
                            </Columns>
                        </MasterTableView>
                        <ClientSettings AllowKeyboardNavigation="true" EnablePostBackOnRowClick="true">
                            <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="1"></Scrolling>
                            <Selecting AllowRowSelect="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </telerik:RadAjaxPanel>
                <asp:SqlDataSource ID="SqlDataSource2" runat="server" SelectCommand="select * from c_unidad_f " ConnectionString="<%$ ConnectionStrings:PVW %>">
                </asp:SqlDataSource>
                </asp:Panel>
            </asp:Panel>
         
              </ContentTemplate>
                    </asp:UpdatePanel>
                </telerik:RadPageView>

     </telerik:RadMultiPage>
   

</asp:Content>

