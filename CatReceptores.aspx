<%@ Page Title="" Language="C#" MasterPageFile="~/AdmOrdenes.master" AutoEventWireup="true" CodeFile="CatReceptores.aspx.cs" Inherits="CatReceptores" %>

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
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" />

    <div class="row">
        <div class="col-lg-12 col-sm-12 text-center alert-info">
            <h3>
                <i class="fa fa-hospital-o"></i>&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label1" runat="server" Text="Catalogo Receptores"></asp:Label>
            </h3>
        </div>
    </div>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>            
            <asp:Panel ID="pnlContenido" CssClass="col-lg-12 col-sm-12" runat="server" ScrollBars="Auto">
            
                <div class="col-lg-12 col-sm-12 marTop text-center">
                    <div class="col-lg-3 col-sm-3 text-center">
                        <asp:LinkButton ID="btnAgregar" runat="server" CssClass="btn btn-info t14" ToolTip="Agregar" OnClick="lnkAbreAgrega_Click"><i class="fa fa-plus-circle"></i>&nbsp;<span>Agregar</span></asp:LinkButton>
                    </div>
                    <div class="col-lg-3 col-sm-3 text-center">
                        <asp:LinkButton ID="btnEditar" runat="server" CssClass="btn btn-warning t14" Visible="false"  ToolTip="Editar" ><i class="fa fa-pencil-square "></i>&nbsp;<span>Editar</span></asp:LinkButton>
                    </div>
                    <div class="col-lg-3 col-sm-3 text-center">
                        <asp:LinkButton ID="btnBorrar" runat="server" CssClass="btn btn-danger t14" Visible="false"  ToolTip="Borrar" OnClick="btnBorrar_Click" ><i class="fa fa-times"></i>&nbsp;<span>Borrar</span></asp:LinkButton>
                    </div>
                    <div class="col-lg-3 col-sm-3 text-center">
                        <asp:LinkButton ID="btnSeleccionar" runat="server" CssClass="btn btn-primary t14" Visible="false" ToolTip="Seleccionar" ><i class="fa fa-check"></i>&nbsp;<span>Seleccionar</span></asp:LinkButton>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-12 col-sm-12 text-center">
                        <asp:Label ID="lblError" runat="server" CssClass="errores" />
                    </div>
                </div>

                <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" EnableAJAX="true">
                    <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" AllowFilteringByColumn="true"  runat="server" EnableHeaderContextMenu="true" Culture="es-Mx" Skin="Metro" OnSelectedIndexChanged="RadGrid1_SelectedIndexChanged" 
                                EnableHeaderContextFilterMenu="true" AllowPaging="True" PagerStyle-AlwaysVisible="true" DataSourceID="SqlDataSource1" AllowSorting="true" GroupingEnabled="false" PageSize="50" >
                        <MasterTableView DataSourceID="SqlDataSource1" AutoGenerateColumns="false" DataKeyNames="IdRecep">
                            <Columns>
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="ReRFC" FilterControlAltText="Filtro Nombre" HeaderText="RFC" SortExpression="ReRFC" UniqueName="ReRFC" Resizable="true"  />
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="ReNombre" FilterControlAltText="Filtro RFC" HeaderText="Razon Social / Nombre" SortExpression="ReNombre" UniqueName="ReNombre" Resizable="true" />
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="correo" FilterControlAltText="Filtro RFC" HeaderText="Correo" SortExpression="correo" UniqueName="correo" Resizable="true" />
                                <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="correoCC" FilterControlAltText="Filtro RFC" HeaderText="CorreoCC" SortExpression="correoCC" UniqueName="correoCC" Resizable="true" />
                            </Columns>
                        </MasterTableView>
                        <ClientSettings AllowKeyboardNavigation="true" EnablePostBackOnRowClick="true">
                            <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="1"></Scrolling>
                            <Selecting AllowRowSelect="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </telerik:RadAjaxPanel>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" 
                SelectCommand="select IdRecep,ReRFC,ReNombre,case ReCorreo when '' then '...' else ReCorreo end as correo,case  when ReCorreoCC is null then '...' when ReCorreoCC=''  then '...' else ReCorreoCC end as correoCC  from receptores_f">
                </asp:SqlDataSource>
                
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



    <telerik:RadWindow RenderMode="Lightweight" ID="modalNuevo" EnableShadow="true" Skin="Metro"
        Behaviors="Move,Close,Resize,Pin" VisibleOnPageLoad="false" ShowContentDuringLoad="false" DestroyOnClose="true" 
        Animation="Fade" runat="server" Modal="true" Width="900px" Height="650px" Style="z-index: 1000;">
        <ContentTemplate>
            <asp:UpdatePanel ID="UpdatePanelRece" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pnlPrincipal" runat="server" CssClass="ancho100 text-center">
                        <div class="col-lg-12 col-sm-12 text-center">
                            <asp:Label ID="lblEditaAgrega" runat="server" Visible="false"></asp:Label>
                            
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="agrega" CssClass="errores" DisplayMode="List" />
                        </div>

                        <%-- TEXTBOX y LABELS --%>
                        <div class="col-lg-12 col-sm-12">
                            <div class="col-lg-3 col-sm-3 text-center marTop">
                                <asp:Label ID="Label2" runat="server" Text="RFC:" CssClass="textoBold" />
                            </div>
                            <div class="col-lg-3 col-sm-3 text-center marTop">
                                <asp:TextBox ID="txtRFC" runat="server" PlaceHolder="RFC" MaxLength="13"  CssClass="input-small" Width="200"></asp:TextBox>
                            </div>
                            <div class="col-lg-3 col-sm-3 text-center marTop">
                                <asp:Label ID="Label3" runat="server" Text="Nombre:" CssClass="textoBold" />
                            </div>
                            <div class="col-lg-3 col-sm-3 text-center marTop">
                                <asp:TextBox ID="txtNombre" runat="server" PlaceHolder="Nombre / Razon Social"  CssClass="input-small" Width="200"></asp:TextBox>
                            </div>
                            <div class="col-lg-3 col-sm-3 text-center marTop">
                                <asp:Label ID="Label4" runat="server" Text="Correo:" CssClass="textoBold" />
                            </div>
                            <div class="col-lg-3 col-sm-3 text-center marTop">
                                <asp:TextBox ID="txtCorreo" runat="server" PlaceHolder="Correo"  CssClass="input-small" Width="200"></asp:TextBox>
                            </div>
                            <div class="col-lg-3 col-sm-3 text-center marTop">
                                <asp:Label ID="Label5" runat="server" Text="CorreoCC:" CssClass="textoBold" />
                            </div>
                            <div class="col-lg-3 col-sm-3 text-center marTop">
                                <asp:TextBox ID="txtCoreoCC" runat="server" PlaceHolder="CorreoCC"  CssClass="input-small" Width="200"></asp:TextBox>
                            </div>
                            <div class="col-lg-3 col-sm-3 text-center marTop">
                                <asp:Label ID="Label6" runat="server" Text="CorreoCCO:" CssClass="textoBold" />
                            </div>
                            <div class="col-lg-3 col-sm-3 text-center marTop">
                                <asp:TextBox ID="txtCorreoCCO" runat="server" PlaceHolder="CorreoCCO"  CssClass="input-small" Width="200"></asp:TextBox>
                            </div>
                            <div class="col-lg-3 col-sm-3 text-center marTop">
                                <asp:Label ID="Label7" runat="server" Text="Calle:" CssClass="textoBold" />
                            </div>
                            <div class="col-lg-3 col-sm-3 text-center marTop">
                                <asp:TextBox ID="txtCalle" runat="server" PlaceHolder="Calle"  CssClass="input-small" Width="200"></asp:TextBox>
                            </div>
                            <div class="col-lg-3 col-sm-3 text-center marTop">
                                <asp:Label ID="Label8" runat="server" Text="No Ext:" CssClass="textoBold" />
                            </div>
                            <div class="col-lg-3 col-sm-3 text-center marTop">
                                <asp:TextBox ID="txtNoExt" runat="server" PlaceHolder="No Ext"  CssClass="input-small" Width="200"></asp:TextBox>
                            </div>
                            <div class="col-lg-3 col-sm-3 text-center marTop">
                                <asp:Label ID="Label9" runat="server" Text="No Int:" CssClass="textoBold" />
                            </div>
                            <div class="col-lg-3 col-sm-3 text-center marTop">
                                <asp:TextBox ID="txtNoInt" runat="server" PlaceHolder="No Int"  CssClass="input-small" Width="200"></asp:TextBox>
                            </div>
                            
                            <div class="col-lg-3 col-sm-3 text-center marTop">
                                <asp:Label ID="Label12" runat="server" Text="Pais:" CssClass="textoBold" />
                            </div>
                            <div class="col-lg-3 col-sm-3 text-center marTop">
                                <telerik:RadComboBox RenderMode="Lightweight" ID="ddlPais" runat="server" Width="200" Height="200px" DataValueField="cve_pais" AutoPostBack="true"  Filter="Contains" 
                                EmptyMessage="Seleccione País..." DataSourceID="SqlDataSource2" DataTextField="desc_pais" Skin="Silk" ></telerik:RadComboBox>
                                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select cve_pais,desc_pais from Paises_f"></asp:SqlDataSource>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="Debe indicar el País." Text="*" CssClass="errores" ControlToValidate="ddlPais" ValidationGroup="agrega"></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-lg-3 col-sm-3 text-center marTop">
                                <asp:Label ID="Label13" runat="server" Text="Estado:" CssClass="textoBold" />
                            </div>
                            <div class="col-lg-3 col-sm-3 text-center marTop">
                                <telerik:RadComboBox RenderMode="Lightweight" ID="ddlEstado" runat="server" Width="200" Height="200px" DataValueField="cve_edo" AutoPostBack="true" MarkFirstMatch="true" AllowCustomText="true" AutoCompleteSeparator="None" Filter="Contains" 
                                EmptyMessage="Seleccione País..." DataSourceID="SqlDataSource3" DataTextField="nom_edo" Skin="Silk" ></telerik:RadComboBox>
                                <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select cve_edo,nom_edo from estados_f where cve_pais=@pais">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="ddlPais" DbType="Int32" Name="pais" DefaultValue="0" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Debe indicar el estado." Text="*" CssClass="errores" ControlToValidate="ddlEstado" ValidationGroup="agrega"></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-lg-3 col-sm-3 text-center marTop">
                                <asp:Label ID="Label14" runat="server" Text="Municipio:" CssClass="textoBold" />
                            </div>
                            <div class="col-lg-3 col-sm-3 text-center marTop">
                                <telerik:RadComboBox RenderMode="Lightweight" ID="ddlMunicipio" runat="server" Width="200" Height="200px" DataValueField="ID_Del_Mun" AutoPostBack="true" MarkFirstMatch="true" AllowCustomText="true" AutoCompleteSeparator="None" Filter="Contains" 
                                EmptyMessage="Seleccione País..." DataSourceID="SqlDataSource4" DataTextField="Desc_Del_Mun" Skin="Silk" ></telerik:RadComboBox>
                                <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select ID_Del_Mun, Desc_Del_Mun from DelegacionMunicipio_f where cve_pais=@pais and ID_Estado=@estado">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="ddlPais" DbType="Int32" Name="pais" DefaultValue="0" />
                                        <asp:ControlParameter ControlID="ddlEstado" DbType="Int32" Name="estado" DefaultValue="0" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Debe indicar el municipio." Text="*" CssClass="errores" ControlToValidate="ddlMunicipio" ValidationGroup="agrega"></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-lg-3 col-sm-3 text-center marTop">
                                <asp:Label ID="Label16" runat="server" Text="C.P." CssClass="textoBold" />
                            </div>
                            <div class="col-lg-3 col-sm-3 text-center marTop">
                                <telerik:RadComboBox RenderMode="Lightweight" ID="ddlCP" runat="server" Width="200" Height="200px" DataValueField="id_cod_pos" AutoPostBack="true" MarkFirstMatch="true" AllowCustomText="true" AutoCompleteSeparator="None" Filter="Contains" 
                                EmptyMessage="Seleccione País..." DataSourceID="SqlDataSource6" DataTextField="id_cod_pos" Skin="Silk" ></telerik:RadComboBox>
                                <asp:SqlDataSource ID="SqlDataSource6" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select distinct id_cod_pos from Colonias_f where cve_pais=@pais and ID_Estado=@estado and ID_Del_Mun=@municipio">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="ddlPais" DbType="Int32" Name="pais" DefaultValue="0" />
                                        <asp:ControlParameter ControlID="ddlEstado" DbType="Int32" Name="estado" DefaultValue="0" />
                                        <asp:ControlParameter ControlID="ddlMunicipio" DbType="Int32" Name="municipio" DefaultValue="0" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Debe indicar el municipio." Text="*" CssClass="errores" ControlToValidate="ddlCP" ValidationGroup="agrega"></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-lg-3 col-sm-3 text-center marTop">
                                <asp:Label ID="Label15" runat="server" Text="Colonia:" CssClass="textoBold" />
                            </div>
                            <div class="col-lg-3 col-sm-3 text-center marTop">
                                <telerik:RadComboBox RenderMode="Lightweight" ID="ddlColonia" runat="server" Width="200" Height="200px" DataValueField="id_colonia" AutoPostBack="true" MarkFirstMatch="true" AllowCustomText="true" AutoCompleteSeparator="None" Filter="Contains" 
                                EmptyMessage="Seleccione País..." DataSourceID="SqlDataSource5" DataTextField="desc_colonia" Skin="Silk" ></telerik:RadComboBox>
                                <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select id_colonia, desc_colonia from colonias_f where cve_pais=@pais and  id_estado=@estado and ID_Del_Mun=@municipio and ID_Cod_Pos=@cp">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="ddlPais" DbType="Int32" Name="pais" DefaultValue="0" />
                                        <asp:ControlParameter ControlID="ddlEstado" DbType="Int32" Name="estado" DefaultValue="0" />
                                        <asp:ControlParameter ControlID="ddlMunicipio" DbType="Int32" Name="municipio" DefaultValue="0" />
                                        <asp:ControlParameter ControlID="ddlCP" DbType="Int32" Name="cp" DefaultValue="0" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Debe indicar el colonia." Text="*" CssClass="errores" ControlToValidate="ddlColonia" ValidationGroup="agrega"></asp:RequiredFieldValidator>
                            </div>
                            
                            <div class="col-lg-3 col-sm-3 text-center marTop">
                                <asp:Label ID="Label18" runat="server" Text="Cuenta:" CssClass="textoBold" />
                            </div>
                            <div class="col-lg-3 col-sm-3 text-center marTop">
                                <asp:TextBox ID="txtCuenta" runat="server" PlaceHolder="Cuenta"  CssClass="input-small" Width="200"></asp:TextBox>
                            </div>

                            <div class="col-lg-3 col-sm-3 text-center marTop">
                                <asp:Label ID="Label10" runat="server" Text="Localidad:" CssClass="textoBold" />
                            </div>
                            <div class="col-lg-3 col-sm-3 text-center marTop">
                                <asp:TextBox ID="txtLocalidad" runat="server" PlaceHolder="Localidad"  CssClass="input-small" Width="200"></asp:TextBox>
                            </div>
                            <div class="col-lg-3 col-sm-3 text-center marTop">
                                <asp:Label ID="Label11" runat="server" Text="Referencia:" CssClass="textoBold" />
                            </div>
                            <div class="col-lg-3 col-sm-3 text-center marTop">
                                <asp:TextBox ID="txtReferencia" runat="server" PlaceHolder="Referencia"  CssClass="input-small" Width="200"></asp:TextBox>
                            </div>
                        </div>
                        <br />
                            <div class="row marTop">
                                <asp:Label ID="lblErrorAdentro" runat="server" Visible="false"></asp:Label>
                            </div>
                            <div class="row text-center marTop">
                                <div class="row col-lg-6 col-sm-6 text-center">
                                    <asp:LinkButton ID="lnkGuardaEdita" runat="server" CssClass="btn btn-success t14" ValidationGroup="crea" OnClick="lnkGuardaEdita_Click"  ToolTip="Agregar" ><i class="fa fa-edit"></i>&nbsp;<span>Guardar</span></asp:LinkButton>
                                </div>
                                <div class="row col-lg-6 col-sm-6 text-center">
                                    <asp:LinkButton ID="LinkButton2" runat="server" CssClass="btn btn-danger t14" OnClick="lnkCerrar_Click"  ToolTip="Agregar" ><i class="fa fa-times"></i>&nbsp;<span>Cerrar</span></asp:LinkButton>
                                </div>
                            </div>
                       
                    </asp:Panel>
                    <asp:UpdateProgress ID="updProgEmi" runat="server" AssociatedUpdatePanelID="UpdatePanelRece">
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






</asp:Content>

