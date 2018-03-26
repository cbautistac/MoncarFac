<%@ Page Title="" Language="C#" MasterPageFile="~/AdmOrdenes.master" AutoEventWireup="true" CodeFile="CatFormaPago.aspx.cs" Inherits="CatFormaPago" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">
        function abreNewEmi() {
            var oWnd = $find("<%=modalNuevo.ClientID%>");
            oWnd.setUrl('');

            oWnd.show();
        }

        function cierraNewEmi() {
            var oWnd = $find("<%=modalNuevo.ClientID%>");
            oWnd.close();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" /> 
    <div class="row">
        <div class="col-lg-12 col-sm-12 text-center alert-info">
            <h3>
                <i class="fa fa-credit-card"></i>&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label2" runat="server" Text="Forma de Pago"></asp:Label>
            </h3>
        </div>
    </div>

    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

    <telerik:RadWindow RenderMode="Lightweight" ID="modalNuevo" Title="" EnableShadow="true" Skin="Metro"
        Behaviors="Move,Close,Resize,Pin" VisibleOnPageLoad="false" ShowContentDuringLoad="true" DestroyOnClose="true" 
        Animation="Fade" runat="server" Modal="true" Width="950px" Height="800px" Style="z-index: 1000;">
        <ContentTemplate>
            <asp:UpdatePanel ID="UpdatePanelEmi" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pnlPrincipal" runat="server" CssClass="ancho100 text-center">
                        <div class="row">
        <div class="col-lg-12 col-sm-12 text-center alert-info">
            <h3>
                
                <asp:Label ID="lblTitulo" runat="server" Text=""></asp:Label>
            </h3>
        </div>
    </div>
                        <br />
                        <div class="col-lg-12 col-sm-12 text-center">
                            <asp:Label ID="lblErrorNuevo" runat="server" CssClass="errores"></asp:Label>
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="agrega" CssClass="errores" DisplayMode="List" />
                        </div>
                       <div class="col-lg-12 col-sm-12 text-center">

                        <!--para descripción -->
                          <div class="col-lg-3 col-sm-3 text-center"><asp:Label ID="lblDesc" runat="server" Text="Descripci&oacute;n:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtDesc" runat="server" MaxLength="80" Width="400px" Height="100px" CssClass="input-left" />
                            <cc1:TextBoxWatermarkExtender ID="txtDescWatermarkExtender1" runat="server" BehaviorID="txtDesc_TextBoxWatermarkExtender" TargetControlID="txtDesc" WatermarkCssClass="input-medium water" WatermarkText="Descripción" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Debe indicar la Descripción" CssClass="errores alineado" ValidationGroup="agrega" ControlToValidate="txtDesc" Text="*"></asp:RequiredFieldValidator>
                        </div> 
                           <br /> 
                           <!--para  bancarizado-->
                        <div class="col-lg-3 col-sm-3 text-center"><asp:Label ID="lblBancarizado" runat="server" Text="Bancarizado:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtBancarizado" runat="server" MaxLength="10" Width="100px" CssClass="input-left" />
                            <cc1:TextBoxWatermarkExtender ID="txtBancarizadoWatermarkExtender1" runat="server" BehaviorID="txtBancarizado_TextBoxWatermarkExtender" TargetControlID="txtBancarizado" WatermarkCssClass="input-medium water" WatermarkText="Bancarizado" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Debe indicar si es Bancarizado o no" CssClass="errores alineado" ValidationGroup="agrega" ControlToValidate="txtBancarizado" Text="*"></asp:RequiredFieldValidator>
                        </div> 
                        <!--para número de operación-->
                        <div class="col-lg-3 col-sm-3 text-center"><asp:Label ID="lblNumOp" runat="server" Text="Número de Operación:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtNumOp" runat="server" MaxLength="10" Width="500px" CssClass="input-left" />
                            <cc1:TextBoxWatermarkExtender ID="txtNumOpWatermarkExtender1" runat="server" BehaviorID="txtNumOp_TextBoxWatermarkExtender" TargetControlID="txtNumOp" WatermarkCssClass="input-medium water" WatermarkText="Número de Operación" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Debe indicar el número de operación" CssClass="errores alineado" ValidationGroup="agrega" ControlToValidate="txtNumOp" Text="*"></asp:RequiredFieldValidator>
                        </div> 
                           <br />  
                           <!-- RFC del emisor de la cuenta Ordenante-->
                           <div class="col-lg-3 col-sm-3 text-center"><asp:Label ID="Label1" runat="server" Text="RFC del Emisor de la Cta Orden:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtRFCCO" runat="server" MaxLength="10" Width="500px" CssClass="input-left" />
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" BehaviorID="txtRFCCO_TextBoxWatermarkExtender" TargetControlID="txtRFCCO" WatermarkCssClass="input-medium water" WatermarkText="RFC del Emisor de la Cta Orden" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Debe indicar RFC del Emisor de la Cta Orden" CssClass="errores alineado" ValidationGroup="agrega" ControlToValidate="txtRFCCO" Text="*"></asp:RequiredFieldValidator>
                        </div>        
                           <!-- Cuenta ordenante-->      

                        <div class="col-lg-3 col-sm-3 text-center"><asp:Label ID="Label3" runat="server" Text="Cuenta Ordenante:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtCtaOrd" runat="server" MaxLength="10" Width="500px" CssClass="input-left" />
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" BehaviorID="txtCtaOrd_TextBoxWatermarkExtender" TargetControlID="txtCtaOrd" WatermarkCssClass="input-medium water" WatermarkText="Cuenta Ordenante" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Debe indicar la Cuenta Ordenante" CssClass="errores alineado" ValidationGroup="agrega" ControlToValidate="txtCtaOrd" Text="*"></asp:RequiredFieldValidator>
                            </div>
                            <!-- Patron Para cuenta Ordenante-->
                             <div class="col-lg-3 col-sm-3 text-center"><asp:Label ID="Label4" runat="server" Text="Patrón para Cuenta Ordenante:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtPtnCtaO" runat="server" MaxLength="10" Width="500px" CssClass="input-left" />
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server" BehaviorID="txtPtnCtaO_TextBoxWatermarkExtender" TargetControlID="txtPtnCtaO" WatermarkCssClass="input-medium water" WatermarkText="Patrón para Cuenta Ordenante" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Debe indicar el Patron de la Cuenta Ordenante" CssClass="errores alineado" ValidationGroup="agrega" ControlToValidate="txtPtnCtaO" Text="*"></asp:RequiredFieldValidator>
                            </div>

                            <!-- RFC del Emisor Cuenta del Beneficiario-->

                            <div class="col-lg-3 col-sm-3 text-center"><asp:Label ID="Label5" runat="server" Text="RFC del Emisor Cuenta del Beneficiario:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtRFCCtaBen" runat="server" MaxLength="10" Width="500px" CssClass="input-left" />
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender4" runat="server" BehaviorID="txtRFCCtaBen_TextBoxWatermarkExtender" TargetControlID="txtRFCCtaBen" WatermarkCssClass="input-medium water" WatermarkText="RFC del Emisor Cuenta del Beneficiario" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Debe indicar el RFC del Emisor Cuenta de Beneficiario" CssClass="errores alineado" ValidationGroup="agrega" ControlToValidate="txtRFCCtaBen" Text="*"></asp:RequiredFieldValidator>
                            </div>
                            <!-- Cuenta del Beneficiario-->
                            <div class="col-lg-3 col-sm-3 text-center"><asp:Label ID="Label6" runat="server" Text="Cuenta de Beneficiario:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtCtaBen" runat="server" MaxLength="10" Width="500px" CssClass="input-left" />
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender5" runat="server" BehaviorID="txtCtaBen_TextBoxWatermarkExtender" TargetControlID="txtCtaBen" WatermarkCssClass="input-medium water" WatermarkText="Cuenta Beneficiario" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="Debe indicar la Cuenta de Beneficiario" CssClass="errores alineado" ValidationGroup="agrega" ControlToValidate="txtCtaBen" Text="*"></asp:RequiredFieldValidator>
                            </div>

                           <!-- patro cuenta ordenante beneficiario -->
                           <div class="col-lg-3 col-sm-3 text-center"><asp:Label ID="Label9" runat="server" Text="Patrón para Cuenta Beneficiario:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtPtnCtaBen" runat="server" MaxLength="10" Width="500px" CssClass="input-left" />
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender8" runat="server" BehaviorID="txtPtnCtaBen_TextBoxWatermarkExtender" TargetControlID="txtPtnCtaBen" WatermarkCssClass="input-medium water" WatermarkText="Patrón para Cuenta Beneficiario" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="Debe indicar el Patron de la Cuenta Beneficiario" CssClass="errores alineado" ValidationGroup="agrega" ControlToValidate="txtPtnCtaBen" Text="*"></asp:RequiredFieldValidator>
                            </div>

                            <!-- Tipo Cadena Pago-->

                            <div class="col-lg-3 col-sm-3 text-center"><asp:Label ID="Label7" runat="server" Text="Tipo Cadena Pago:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtTipoCadena" runat="server" MaxLength="10" Width="500px" CssClass="input-left" />
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender6" runat="server" BehaviorID="txtTipoCadena_TextBoxWatermarkExtender" TargetControlID="txtTipoCadena" WatermarkCssClass="input-medium water" WatermarkText="Tipo Cadena Pago" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="Debe indicar la Cuenta de Beneficiario" CssClass="errores alineado" ValidationGroup="agrega" ControlToValidate="txtTipoCadena" Text="*"></asp:RequiredFieldValidator>
                            </div>
                            <!-- Nombre del Banco emisor de la Cuenta Ordenenante Extranjero -->

                            <div class="col-lg-3 col-sm-3 text-center"><asp:Label ID="Label8" runat="server" Text="Nombre del Banco Emisor de la Cta. Ordenante del Extrajero:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-9 col-sm-9 text-left">
                            <asp:TextBox ID="txtNomCtaExtra" runat="server" MaxLength="10" Width="500px" CssClass="input-left" />
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender7" runat="server" BehaviorID="txtNomCtaExtra_TextBoxWatermarkExtender" TargetControlID="txtNomCtaExtra" WatermarkCssClass="input-medium water" WatermarkText="Nombre del Banco Emisor de la Cta. Ordenante del Extrajero" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="Debe indicar la Cuenta de Beneficiario" CssClass="errores alineado" ValidationGroup="agrega" ControlToValidate="txtNomCtaExtra" Text="*"></asp:RequiredFieldValidator>
                          </div>   



                        <div class="col-lg-6 col-sm-6 text-center ">
                            <asp:LinkButton ID="lnkAgregarNuevo" runat="server" CssClass="btn btn-success t14" ValidationGroup="agrega" OnClick="lnkAgregarNuevo_Click"><i class="fa fa-check-circle"></i>&nbsp; Agregar</asp:LinkButton>
                        </div>
                        <div class="col-lg-6 col-sm-6 text-center ">
                            <asp:LinkButton ID="lnkCancelarNuevo" runat="server" CssClass="btn btn-danger t14" OnClientClick="cierraNewEmi()"><i class="fa fa-remove"></i>&nbsp; Cancelar</asp:LinkButton>
                        </div>
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

      <asp:UpdatePanel ID="UpDate2" runat="server">
        <ContentTemplate>

             <div class="row marTop">        
                
                <div class="col-lg-3 col-sm-3 text-center">
                    <asp:LinkButton ID="lnkAgregar" OnClick="lnkAgregar_Click" runat="server" CssClass="btn btn-info"  ><i class="fa fa-plus-circle"></i>&nbsp;<span>Agregar</span></asp:LinkButton>
                </div>
                <div class=" col-lg-3 col-sm-3 text-center">
                    <asp:LinkButton ID="lnkEditar" Visible="false" OnClick="lnkEditar_Click" runat="server" CssClass="btn btn-warning" ><i class="fa fa-pencil"></i>&nbsp;<span>Editar</span></asp:LinkButton>
                </div>
                <div class=" col-lg-3 col-sm-3 text-center">
                    <asp:LinkButton ID="lnkBorrar" Visible="false" OnClick="lnkBorrar_Click" runat="server" CssClass="btn btn-danger" ><i class="fa fa-times"></i>&nbsp;<span>Borrar</span></asp:LinkButton>
                </div>
                <div class=" col-lg-3 col-sm-3 text-center">
                    <asp:LinkButton ID="lnkSeleccionar" Visible="false" OnClick="lnkSeleccionar_Click" runat="server" CssClass="btn btn-primary" ><i class="fa fa-check"></i>&nbsp;<span>Seleccionar</span></asp:LinkButton>
                </div>
                

           </div> 
            
                <asp:Panel ID="panelGrid" runat="server" CssClass="ancho100">
                    <div class="col-lg-12 col-sm-12 text-center marTop ">

                    
   <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" EnableAJAX="true">
                    <telerik:RadGrid OnSelectedIndexChanged="RadGrid1_SelectedIndexChanged" RenderMode="Lightweight" ID="RadGrid1" AllowFilteringByColumn="true"  runat="server" EnableHeaderContextMenu="true" Culture="es-Mx" Skin="Metro"
                                EnableHeaderContextFilterMenu="true" AllowPaging="True" PagerStyle-AlwaysVisible="true" DataSourceID="SqlDataSource1" AllowSorting="true" GroupingEnabled="false" PageSize="50" >
                        <MasterTableView DataSourceID="SqlDataSource1" AutoGenerateColumns="false" DataKeyNames="ClaveFormaPago">
                            <Columns>
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" Visible="false" DataField="ClaveFormaPago" FilterControlAltText="Filtro idBanco" HeaderText="Clave Forma de Pago" SortExpression="ClaveFormaPago" UniqueName="ClaveFormaPago" Resizable="true"  />

                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="Descripcion" FilterControlAltText="Filtro Descripcion" HeaderText="Descripci&oacute;n" SortExpression="Descripcion" UniqueName="Descripcion" Resizable="true"  />
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="Bancarizado" FilterControlAltText="Filtro Bancarizado" HeaderText="Bancarizado" SortExpression="Bancarizado" UniqueName="Bancarizado" Resizable="true" />
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="NumeroOperacion" FilterControlAltText="Filtro NumeroOperacion" HeaderText="NumeroOperacion" SortExpression="NumeroOperacion" UniqueName="NumeroOperacion" Resizable="true" />
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="RFCEmisorCtaOrden" FilterControlAltText="Filtro RFCEmisorCtaOrden" HeaderText="RFC Emisor Cta Orden" SortExpression="RFCEmisorCtaOrden" UniqueName="RFCEmisorCtaOrden" Resizable="true" />
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="CuentaOrden" FilterControlAltText="Filtro CuentaOrden" HeaderText="Cuenta Orden" SortExpression="CuentaOrden" UniqueName="CuentaOrden" Resizable="true" />
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="PatronCtaOrden" FilterControlAltText="Filtro PatronCtaOrden" HeaderText="Patron Cta Orden" SortExpression="PatronCtaOrden" UniqueName="PatronCtaOrden" Resizable="true" />
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="RFCEmisorCtaBen" FilterControlAltText="Filtro RFCEmisorCtaBen" HeaderText="RFC Emisor Cta Ben" SortExpression="RFCEmisorCtaBen" UniqueName="RFCEmisorCtaBen" Resizable="true" />
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="CuentaBen" FilterControlAltText="Filtro CuentaBen" HeaderText="Cuenta Beneficiario" SortExpression="CuentaBen" UniqueName="CuentaBen" Resizable="true" />
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="PatronCtaBen" FilterControlAltText="Filtro PatronCtaBen" HeaderText="Patron Cta Beneficiario" SortExpression="PatronCtaBen" UniqueName="PatronCtaBen" Resizable="true" />
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="TipoCadenaPago" FilterControlAltText="Filtro TipoCadenaPago" HeaderText="Tipo Cadena Pago" SortExpression="TipoCadenaPago" UniqueName="TipoCadenaPago" Resizable="true" />
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="NombreBancoEmisorCOE" FilterControlAltText="Filtro NombreBancoEmisorCOE" HeaderText="Nombre Banco Emisor COE" SortExpression="NombreBancoEmisorCOE" UniqueName="NombreBancoEmisorCOE" Resizable="true" />
                            </Columns>
                        </MasterTableView>
                        <ClientSettings AllowKeyboardNavigation="true" EnablePostBackOnRowClick="true">
                            <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="1"></Scrolling>
                            <Selecting AllowRowSelect="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </telerik:RadAjaxPanel>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" SelectCommand="select ClaveFormaPago, Descripcion, Bancarizado, NumeroOperacion, RFCEmisorCtaOrden,CuentaOrden, PatronCtaOrden, RFCEmisorCtaBen, CuentaBen, PatronCtaBen, TipoCadenaPago, NombreBancoEmisorCOE from c_formapago_f" ConnectionString="<%$ ConnectionStrings:PVW %>">
        </asp:SqlDataSource>
                        </div>
                    </asp:Panel>
                
            </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content>

