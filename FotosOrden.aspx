<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FotosOrden.aspx.cs" Inherits="FotosOrden"
    MasterPageFile="~/AdmonOrden.master" Culture="es-Mx" UICulture="es-Mx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="False" >
        <ContentTemplate>
            <div class="row">
                <div class="col-lg-12 col-sm-12 text-center alert-info">
                    <h3>
                        <i class="fa fa-instagram"></i>&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="Label1" runat="server" Text="Fotografías"></asp:Label>
                    </h3>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-12 col-sm-12 text-center">
                    <asp:Label ID="lblErrorFotos" runat="server" CssClass="errores" />
                </div>
            </div>
            <asp:Panel runat="server" ID="pnlDaños" CssClass="paneles col-lg-12 col-sm-12" ScrollBars="Auto">
                <div class="col-lg-12 col-sm-12 text-center">
                    <div class="col-lg-4 col-sm-4 text-center">
                        <asp:Label runat="server" ID="lblx" Text="Proceso: " CssClass="alingMiddle textoBold" />&nbsp;&nbsp;
                        <asp:DropDownList ID="ddlProcesoFotoFiltro" runat="server" AutoPostBack="true" CssClass="alingMiddle input-large"
                            OnSelectedIndexChanged="ddlProcesoFotoFiltro_SelectedIndexChanged">
                            <asp:ListItem Text="Recepción" Value="1" Selected="True" />
                            <asp:ListItem Text="Presupuesto" Value="2" />
                            <asp:ListItem Text="Operación" Value="3" />
                            <asp:ListItem Text="Terminado" Value="4" />
                            <asp:ListItem Text="Refacciones" Value="5" />
                        </asp:DropDownList>
                    </div>
                    <div class="col-lg-6 col-sm-6 text-center">
                        <telerik:RadSkinManager ID="RadSkinManager1" runat="server">
                        </telerik:RadSkinManager>
                        <telerik:RadAsyncUpload RenderMode="Lightweight" runat="server" Culture="es-Mx" CssClass="async-attachment"
                            MaxFileInputsCount="10" MultipleFileSelection="Automatic" ID="AsyncUpload1" HideFileInput="true"
                            AllowedFileExtensions=".jpeg,.jpg,.png,.gif,.bmp,.tiff,.JPEG,.JPG,.PNG,.GIF,.BMP,.TIFF">
                        </telerik:RadAsyncUpload>
                    </div>
                    <div class="col-lg-2 col-sm-2 text-center">
                        <asp:LinkButton ID="btnAddFotoDanos" runat="server" ToolTip="Agregar Foto" OnClick="btnAddFotoDanos_Click"
                            CssClass="alingMiddle btn btn-info t14"><i class="fa fa-plus-circle"></i>&nbsp;<span>Agregar Fotograf&iacute;a</span></asp:LinkButton>
                    </div>
                </div>
                <asp:Panel ID="Panel4" runat="server" CssClass="panelCatalogos text-center" ScrollBars="Auto">
                    <div class="table-responsive">
                        <asp:DataList ID="DataListFotosDanos" runat="server" RepeatColumns="9" RepeatDirection="Horizontal"
                            DataKeyField="id_empresa" DataSourceID="SqlDataSourceFotosDanos" OnItemCommand="DataListFotosDanos_ItemCommand">
                            <ItemTemplate>
                                <asp:Label ID="id_empresaLabel" runat="server" Text='<%# Eval("id_empresa") %>' Visible="false" />
                                <asp:Label ID="id_tallerLabel" runat="server" Text='<%# Eval("id_taller") %>' Visible="false" />
                                <asp:Label ID="no_ordenLabel" runat="server" Text='<%# Eval("no_orden") %>' Visible="false" />
                                <asp:Label ID="consecutivoLabel" runat="server" Text='<%# Eval("consecutivo") %>'
                                    Visible="false" />
                                <asp:Label ID="procesoLabel" runat="server" Text='<%# Eval("proceso") %>' Visible="false" />
                                <asp:Label ID="nombre_imagenLabel" runat="server" Text='<%# Eval("nombre_imagen") %>'
                                    Visible="false" />
                                <asp:Label ID="rutaLabel" runat="server" Text='<%# Eval("ruta") %>' Visible="false" />
                                <br />
                                <asp:LinkButton ID="btnLogo" runat="server" ToolTip='<%# Eval("nombre_imagen") %>'
                                    CommandName="zoom" CommandArgument='<%# Eval("id_empresa")+";"+Eval("id_taller")+";"+Eval("no_orden")+";"+Eval("consecutivo")+";"+Eval("proceso") %>'>
                                    <asp:Image ID="Image1" runat="server" AlternateText='<%# Eval("nombre_imagen") %>'
                                        Width="120px" ImageUrl='<%# "~/ImgEmpresas.ashx?id="+Eval("id_empresa")+";"+Eval("id_taller")+";"+Eval("no_orden")+";"+Eval("consecutivo")+";"+Eval("proceso") %>' />
                                </asp:LinkButton>
                                <br />
                                <asp:LinkButton ID="btnEliminaFotoDanos" runat="server" CommandName="Delete" ToolTip="Eliminar"
                                    CommandArgument='<%# Eval("id_empresa")+";"+Eval("id_taller")+";"+Eval("no_orden")+";"+Eval("consecutivo")+";"+Eval("proceso") %>'
                                    OnClientClick="return confirm('¿Esta seguro de eliminar la fotografía?');" CssClass="btn btn-danger t14"><i class="fa fa-trash"></i></asp:LinkButton>
                               <asp:LinkButton ID="btnGuardaImg" runat="server" ToolTip="Guardar" OnClick="btnGuardaImg_Click"
                                    CommandArgument='<%# Eval("id_empresa")+";"+Eval("id_taller")+";"+Eval("no_orden")+";"+Eval("consecutivo")+";"+Eval("proceso") %>'
                                    CssClass="btn btn-primary t14"><i class="fa fa-download"></i></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle CssClass="ancho180px textoCentrado" />
                        </asp:DataList>
                        <asp:SqlDataSource runat="server" ID="SqlDataSourceFotosDanos" ConnectionString='<%$ ConnectionStrings:PVW %>'
                            DeleteCommand="delete from Fotografias_Orden where id_empresa=@id_empresa and id_taller=@id_taller and no_orden=@no_orden and consecutivo=@consecutivo and proceso=@proceso"
                            SelectCommand="select id_empresa,id_taller,no_orden,consecutivo,proceso,nombre_imagen,ruta from Fotografias_Orden where id_empresa=@id_empresa and id_taller=@id_taller and no_orden=@no_orden and proceso=@proceso">
                            <DeleteParameters>
                                <asp:Parameter Name="id_empresa"></asp:Parameter>
                                <asp:Parameter Name="id_taller"></asp:Parameter>
                                <asp:Parameter Name="no_orden"></asp:Parameter>
                                <asp:Parameter Name="consecutivo"></asp:Parameter>
                                <asp:Parameter Name="proceso"></asp:Parameter>
                            </DeleteParameters>
                            <SelectParameters>
                                <asp:QueryStringParameter Name="no_orden" QueryStringField="o" Type="Int32" />
                                <asp:QueryStringParameter Name="id_empresa" QueryStringField="e" Type="Int32" />
                                <asp:QueryStringParameter Name="id_taller" QueryStringField="t" Type="Int32" />
                                <asp:ControlParameter ControlID="ddlProcesoFotoFiltro" PropertyName="SelectedValue"
                                    Name="proceso" Type="Int32" DefaultValue="2" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                        <!-- Zoom de imagenes -->
                        <asp:Panel ID="PanelMascara" runat="server" CssClass="mask zen2">
                        </asp:Panel>
                        <asp:Panel ID="PanelImgZoom" runat="server" CssClass="popUp zen3 textoCentrado ancho80 centrado">
                            <table class="ancho100">
                                <tr class="ancho100 centrado">
                                    <td class="ancho95 text-center encabezadoTabla roundTopLeft ">
                                        <asp:Label ID="Label3" runat="server" Text="Fotografía" CssClass="t22 colorMorado textoBold" />
                                    </td>
                                    <td class="ancho5 text-right encabezadoTabla roundTopRight">
                                        <asp:LinkButton ID="btnCerrarImgZomm" runat="server" ToolTip="Cerrar" OnClick="btnCerrarImgZomm_Click"
                                            CssClass="btn btn-danger alingMiddle"><i class="fa fa-remove t18"></i></asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                            <div class="row">
                                <asp:Panel ID="Panel1" runat="server" CssClass="col-lg-12 col-sm-12 text-center ancho100 pnlImagen"
                                    ScrollBars="Auto">
                                    <asp:Image ID="imgZoom" runat="server" CssClass="ancho100" AlternateText="Imagen no disponible" />
                                </asp:Panel>
                            </div>
                        </asp:Panel>
                    </div>
                </asp:Panel>
            </asp:Panel>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                <ProgressTemplate>
                    <asp:Panel ID="pnlMaskLoad21" runat="server" CssClass="maskLoad">
                    </asp:Panel>
                    <asp:Panel ID="pnlCargando21" runat="server" CssClass="pnlPopUpLoad">
                        <asp:Image ID="imgLoad21" runat="server" ImageUrl="~/img/loading.gif" CssClass="ancho100" />
                    </asp:Panel>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnAddFotoDanos" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnCerrarImgZomm" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="ddlProcesoFotoFiltro" EventName="SelectedIndexChanged" />
            <asp:PostBackTrigger ControlID="DataListFotosDanos" />
        </Triggers>
    </asp:UpdatePanel>

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
                    <asp:Label ID="Label111" runat="server" Text="Total Orden:" CssClass="colorEtiqueta"
                        Visible="false"></asp:Label>&nbsp;&nbsp;
                    <asp:Label ID="lblTotOrden" runat="server" Visible="false"></asp:Label>
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
</asp:Content>
