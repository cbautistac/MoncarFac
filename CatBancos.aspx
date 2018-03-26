<%@ Page Title="" Language="C#" MasterPageFile="~/AdmOrdenes.master" AutoEventWireup="true" CodeFile="CatBancos.aspx.cs" Inherits="CatBancos" %>
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
                <i class="fa fa-university"></i>&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label2" runat="server" Text="Bancos"></asp:Label>
            </h3>
        </div>
    </div>
   
       
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
    <!--rad para agregar banco nuevo-->

    <telerik:RadWindow RenderMode="Lightweight" ID="modalNuevo" Title="" EnableShadow="true" Skin="Metro"
        Behaviors="Move,Close,Resize,Pin" VisibleOnPageLoad="false" ShowContentDuringLoad="true" DestroyOnClose="true" 
        Animation="Fade" runat="server" Modal="true" Width="950px" Height="400px" Style="z-index: 1000;">
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

                        <!--para seleccionar el banco -->
                          <div class="col-lg-2 col-sm-2 text-center"><asp:Label ID="Label1" runat="server" Text="Banco:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-2 col-sm-2 text-left">
                            <asp:TextBox ID="txtBanco" runat="server" MaxLength="13" CssClass="input-left" />
                            <cc1:TextBoxWatermarkExtender ID="txtBancoWatermarkExtender1" runat="server" BehaviorID="txtBanco_TextBoxWatermarkExtender" TargetControlID="txtBanco" WatermarkCssClass="input-medium water" WatermarkText="Nombre del Banco" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Debe indicar el Banco" CssClass="errores alineado" ValidationGroup="agrega" ControlToValidate="txtBanco" Text="*"></asp:RequiredFieldValidator>
                        </div>  
                           <!--para seleccionar el RFC del banco-->
                        <div class="col-lg-2 col-sm-2 text-center"><asp:Label ID="lblRfcBanco" runat="server" Text="RFC del Banco:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-2 col-sm-2 text-left">
                            <asp:TextBox ID="txtRfcBanco" runat="server" MaxLength="13" CssClass="input-left" />
                            <cc1:TextBoxWatermarkExtender ID="txtRfcBancoWatermarkExtender1" runat="server" BehaviorID="txtRfcBanco_TextBoxWatermarkExtender" TargetControlID="txtRfcBanco" WatermarkCssClass="input-medium water" WatermarkText="R.F.C. del Banco" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Debe indicar el R.F.C. del Banco" CssClass="errores alineado" ValidationGroup="agrega" ControlToValidate="txtRfcBanco" Text="*"></asp:RequiredFieldValidator>
                        </div> 
                          
                        <!--para agregar la clave del banco-->
                        <div class="col-lg-2 col-sm-2 text-center"><asp:Label ID="lblIDBanco" runat="server" Text="Clave del Banco:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-lg-2 col-sm-2 text-left">
                            <asp:TextBox ID="txtIdBanco" runat="server" MaxLength="13" CssClass="input-left" />
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" BehaviorID="txtIdBanco_TextBoxWatermarkExtender" TargetControlID="txtIdBanco" WatermarkCssClass="input-medium water" WatermarkText="Clave del Banco" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Debe indicar la clave del Banco" CssClass="errores alineado" ValidationGroup="agrega" ControlToValidate="txtIdBanco" Text="*"></asp:RequiredFieldValidator>
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
                    <asp:LinkButton ID="lnkAgregar"  OnClick="lnkAgregar_Click" runat="server" CssClass="btn btn-info"  ><i class="fa fa-plus-circle"></i>&nbsp;<span>Agregar</span></asp:LinkButton>
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
                        <MasterTableView DataSourceID="SqlDataSource1" AutoGenerateColumns="false" DataKeyNames="id_Banco">
                            <Columns>
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="Clave_Banco" FilterControlAltText="Filtro idBanco" HeaderText="Clave del Banco" SortExpression="Clave_Banco" UniqueName="Clave_Banco" Resizable="true"  />
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="RFC_Banco" FilterControlAltText="Filtro RFC" HeaderText="RFC del Banco" SortExpression="RFC_Banco" UniqueName="RFC_Banco" Resizable="true" />
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="Nombre_Banco" FilterControlAltText="Filtro Nombre" HeaderText="Nombre Banco" SortExpression="Nombre_Banco" UniqueName="Nombre_Banco" Resizable="true" />
                                
                            </Columns>
                        </MasterTableView>
                        <ClientSettings AllowKeyboardNavigation="true" EnablePostBackOnRowClick="true">
                            <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="1"></Scrolling>
                            <Selecting AllowRowSelect="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </telerik:RadAjaxPanel>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" SelectCommand="select Clave_Banco, id_Banco, RFC_Banco, Nombre_Banco from cat_bancos_f" ConnectionString="<%$ ConnectionStrings:PVW %>">
        </asp:SqlDataSource>
                        </div>
                    </asp:Panel>
                
            </ContentTemplate>
        </asp:UpdatePanel>

</asp:Content>

