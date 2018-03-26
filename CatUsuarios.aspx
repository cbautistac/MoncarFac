<%@ Page Title="" Language="C#" MasterPageFile="~/AdmOrdenes.master" AutoEventWireup="true" CodeFile="CatUsuarios.aspx.cs" Inherits="CatUsuarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
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


     <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
            <ContentTemplate>

            <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true">
                    </asp:ScriptManager>

                    <div class="row">
                        <div class="col-lg-12 col-sm-12 text-center alert-info">
                            <h3>
                                <i class="fa fa-hospital-o"></i>&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="Label1" runat="server" Text="Alta Usuarios"></asp:Label>
                            </h3>
                        </div>
                    </div>


            <div class="row marTop">
                <asp:Label ID="lblErrorAfuera" runat="server" Visible="false"></asp:Label>
                <asp:Label ID="lblIdUsuario" runat="server" Visible="false"></asp:Label>
            </div>
            <div class="row text-center marTop">
                <div class="row col-lg-12 col-sm-12 text-center">
                    <div class="col-lg-3 col-sm-3 text-center">
                        <asp:LinkButton ID="lnkAbreAgrega" runat="server" CssClass="btn btn-info t14" Visible="true" OnClick="lnkAbreAgrega_Click"  ToolTip="Agregar" ><i class="fa fa-plus-circle"></i>&nbsp;<span>Agrega</span></asp:LinkButton>
                    </div>
                    <div class="col-lg-3 col-sm-3 text-center">
                        <asp:LinkButton ID="lnkAbreEdicion" runat="server" CssClass="btn btn-warning t14" Visible="false" OnClick="lnkAbreEdicion_Click"  ToolTip="Editar" ><i class="fa fa-edit"></i>&nbsp;<span>Editar</span></asp:LinkButton>
                    </div>
                    <div class="col-lg-3 col-sm-3 text-center">
                        <asp:LinkButton ID="lnkEliminar" runat="server" CssClass="btn btn-danger t14" Visible="false" OnClick="lnkEliminar_Click"  ToolTip="Borrar" ><i class="fa fa-times"></i>&nbsp;<span>Borra</span></asp:LinkButton>
                    </div>
                </div>
            </div>
                
            <div class="row">
                <div class="col-lg-12 col-sm-12 text-center marTop">
                 <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" EnableAJAX="true">
                            <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1"  AllowFilteringByColumn="true" runat="server" OnSelectedIndexChanged="RadGrid_1OnSelectedIndexChangued"  EnableHeaderContextMenu="true" Culture="es-Mx" Skin="Metro"
                                EnableHeaderContextFilterMenu="true" AllowPaging="True" PagerStyle-AlwaysVisible="true" DataSourceID="SqlDataSource1" AllowSorting="true" GroupingEnabled="false" PageSize="50" >                        
                                <MasterTableView DataSourceID="SqlDataSource1" AutoGenerateColumns="false" DataKeyNames="id_usuario">
                                    <Columns>
                                        <telerik:GridBoundColumn HeaderStyle-Width="25%" FilterCheckListEnableLoadOnDemand="true" DataField="nick" FilterControlAltText="Filtro Usuario" HeaderText="Usuario" SortExpression="nick" UniqueName="nick"  />
                                        <telerik:GridBoundColumn HeaderStyle-Width="25%" FilterCheckListEnableLoadOnDemand="true" DataField="contrasena" FilterControlAltText="Filtro Contraseña" HeaderText="Contraseña" SortExpression="contrasena" UniqueName="contrasena"  />
                                        <telerik:GridBoundColumn HeaderStyle-Width="25%" FilterCheckListEnableLoadOnDemand="true" DataField="nombre_usuario" FilterControlAltText="Filtro Nombre" HeaderText="Nombre" SortExpression="nombre_usuario" UniqueName="nombre_usuario" />
                                    </Columns>
                                    <NoRecordsTemplate>
                                            <asp:Label ID="Label1" runat="server" Text="No existen usuarios registrados" CssClass="text-danger"></asp:Label>
                                    </NoRecordsTemplate>
                                </MasterTableView>
                                <ClientSettings AllowKeyboardNavigation="true" EnablePostBackOnRowClick="true">
                                    <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="1"></Scrolling>
                                    <Selecting AllowRowSelect="true" />
                                </ClientSettings>                                               
                            </telerik:RadGrid>
                    </telerik:RadAjaxPanel>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" SelectCommand="select a.id_usuario,a.nick,a.contrasena,a.nombre_usuario from usuarios a" ConnectionString="<%$ ConnectionStrings:PVW %>">
                    </asp:SqlDataSource>
                </div>
           </div>
        </ContentTemplate>
    </asp:UpdatePanel>


  <telerik:RadWindow RenderMode="Lightweight" ID="modalNuevo" Title="Nuevo Emisor" EnableShadow="true" Skin="Metro"
        Behaviors="Move,Close,Resize,Pin" VisibleOnPageLoad="false" ShowContentDuringLoad="false" DestroyOnClose="true" 
        Animation="Fade" runat="server" Modal="true" Width="900px" Height="650px" Style="z-index: 1000;">
        <ContentTemplate>
            <asp:UpdatePanel ID="UpdatePanelEmi" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pnlPrincipal" runat="server" CssClass="ancho100 text-center">
                        <div class="col-lg-12 col-sm-12 text-center">
                            <asp:Label ID="lblErrorNuevo" runat="server" CssClass="errores"></asp:Label>
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="agrega" CssClass="errores" DisplayMode="List" />
                        </div>

                        <%-- TEXTBOX y LABELS --%>

                            <div class="row pad1m textoBold">
                                <div class="col-lg-6 col-sm-6 text-center">
                                    <asp:Label ID="Label11" runat="server" Text="Usuario:"></asp:Label>
                                </div>
                                <div class="col-lg-6 col-sm-6 text-center">
                                    <asp:TextBox ID="txtUsuario" runat="server" PlaceHolder="Usuario"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row pad1m textoBold">
                                <div class="col-lg-6 col-sm-6 text-center">
                                    <asp:Label ID="Label2" runat="server" Text="Nombre:"></asp:Label>
                                </div>
                                <div class="col-lg-6 col-sm-6 text-center">
                                    <asp:TextBox ID="txtNombre" runat="server" PlaceHolder="Nombre"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row pad1m textoBold">
                                <div class="col-lg-6 col-sm-6 text-center">
                                    <asp:Label ID="Label3" runat="server" Text="Contraseña:"></asp:Label>
                                </div>
                                <div class="col-lg-6 col-sm-6 text-center">
                                    <asp:TextBox ID="txtContraseña" runat="server" PlaceHolder="Contraseña"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row marTop">
                                <asp:Label ID="lblErrorAdentro" runat="server" Visible="false"></asp:Label>
                            </div>
                            <div class="row text-center marTop">
                                <div class="row col-lg-6 col-sm-6 text-center">
                                    <asp:LinkButton ID="LinkButton1" runat="server" CssClass="btn btn-success t14" ValidationGroup="crea"  OnClick="lnkAgrega_Click"  ToolTip="Agregar" ><i class="fa fa-edit"></i>&nbsp;<span>Guardar</span></asp:LinkButton>
                                </div>
                                <div class="row col-lg-6 col-sm-6 text-center">
                                    <asp:LinkButton ID="LinkButton2" runat="server" CssClass="btn btn-warning t14" OnClick="lnkCerrar_Click"  ToolTip="Agregar" ><i class="fa fa-times"></i>&nbsp;<span>Cerrar</span></asp:LinkButton>
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

    <br />
    <br />
    <br />
    <br />



</asp:Content>

