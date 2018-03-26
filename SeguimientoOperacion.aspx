<%@ Page Title="" Language="C#" MasterPageFile="~/AdmonOrden.master" AutoEventWireup="true" CodeFile="SeguimientoOperacion.aspx.cs" Inherits="SeguimientoOperacion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

        
    <div class="row">
        <div class="col-lg-12 col-sm-12 text-center alert-info">
            <h3>
                <i class="fa fa-check-square-o"></i>&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="Label1" runat="server" Text="Seguimiento Operación"></asp:Label>
            </h3>
        </div>
        <div class="row">
            <div class="col-lg-12 col-sm-12 text-center">
                <asp:Label ID="lblErroresOO" runat="server" CssClass="errores alert-danger"></asp:Label>
            </div>
        </div>
    </div>
    <asp:Panel ID="Panel4" runat="server" CssClass="paneles col-lg-12 col-sm-12" ScrollBars="Auto">
        <div class="row pad1m">
            <div class="col-lg-4 col-sm-4 text-center"><asp:Label ID="lblFestimada" runat="server" CssClass="textoBold alingMiddle" /></div>
            <div class="col-lg-4 col-sm-4 text-center"><asp:Label ID="lblFpactada" runat="server" CssClass="textoBold alingMiddle" /></div>
            <div class="col-lg-4 col-sm-4 text-center"><asp:Label ID="lblFConfirmacion" runat="server" CssClass="textoBold alingMiddle" /></div>
        </div>
        <div class="col-lg-12 col-sm-12 text-center">
            <div class="row">
                <div class="col-lg-2 col-sm-2 text-right">
                    <asp:Label ID="Label211" runat="server" Text="Localización:" CssClass="textoBold alingMiddle" />
                </div>
                <div class="col-lg-2 col-sm-2 text-left">
                    <asp:Label ID="lblLocalizacionIni" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="lblAvanceIni" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="lblFaseIni" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="lblPerfilIni" runat="server" Visible="false"></asp:Label>
                    <asp:DropDownList ID="ddlLocalizacion" runat="server" DataSourceID="SqlDataSource26" DataTextField="descripcion" DataValueField="id_localizacion" CssClass="alingMiddle input-medium"></asp:DropDownList>
                    <asp:SqlDataSource ID="SqlDataSource26" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select id_localizacion, descripcion from Localizaciones"></asp:SqlDataSource>
                </div>
                <div class="col-lg-1 col-sm-1 text-right">
                    <asp:Label ID="Label14" runat="server" Text="Perfil:" CssClass="textoBold alingMiddle" />
                </div>
                <div class="col-lg-3 col-sm-3 text-left">
                    <asp:DropDownList ID="ddlPerfil" runat="server" DataSourceID="SqlDataSource2" DataTextField="descripcion" DataValueField="id_perfilOrden" CssClass="alingMiddle input-large"></asp:DropDownList>
                    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select id_perfilOrden, descripcion from perfilesOrdenes"></asp:SqlDataSource>
                </div>
                <div class="col-lg-2 col-sm-2 text-right">
                    <asp:Label ID="Label22" runat="server" Text="% Avance:" CssClass="textoBold alingMiddle" />
                </div>
                <div class="col-lg-2 col-sm-2 text-left">
                    <asp:TextBox ID="txtAvance" runat="server" MaxLength="5" CssClass="alingMiddle input-mini" Enabled="false"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" BehaviorID="txtAvance_FilteredTextBoxExtender" TargetControlID="txtAvance" FilterType="Numbers, Custom" ValidChars="." />
                </div>
            </div>
        </div>

        <div class="col-lg-12 col-sm-12 text-center">
            <div class="col-lg-2 col-sm-2 text-right">
                <asp:Label ID="Label13" runat="server" Text="Visualizacion en Patio:" />&nbsp;
            </div>
            <div class="col-lg-7 col-sm-7 text-center">
                <asp:RadioButtonList ID="rbtGOP" runat="server" CellPadding="10" RepeatDirection="Horizontal" DataSourceID="SqlDataSource1" DataTextField="descripcion_go" DataValueField="id_grupo_op"></asp:RadioButtonList>&nbsp;
                <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:PVW %>'
                    SelectCommand="select mo.id_grupo_op,gop.descripcion_go from mano_obra mo inner join grupo_operacion gop on gop.id_grupo_op=mo.id_grupo_op  where mo.no_orden=@no_orden and mo.id_empresa=@id_empresa and mo.id_taller=@id_taller group by mo.id_grupo_op,gop.descripcion_go">
                    <SelectParameters>
                        <asp:QueryStringParameter QueryStringField="o" Name="no_orden"></asp:QueryStringParameter>
                        <asp:QueryStringParameter QueryStringField="e" Name="id_empresa"></asp:QueryStringParameter>
                        <asp:QueryStringParameter QueryStringField="t" Name="id_taller"></asp:QueryStringParameter>
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
            <div class="col-lg-2 col-sm-2 text-left">
                <asp:LinkButton ID="lnkPantallas" runat="server" OnClick="lnkPantallas_Click" CssClass="btn btn-info"
                    ToolTip="Visualizar en Patio"><i class="fa fa-binoculars"></i><span>&nbsp;Visualizar en Pantalla Patio</span></asp:LinkButton>
            </div>
        </div>

        <div class="col-lg-12 col-sm-12 text-center">
            <div class="col-lg-12 col-sm-12 table-bordered pad1m textoBold text-center">
                <div class="col-lg-4 col-sm-4 text-left">
                    <asp:Label ID="Label2" runat="server" Text="Operación"></asp:Label>
                </div>
                <div class="col-lg-1 col-sm-1 text-center">
                    <asp:Label ID="Label3" runat="server" Text="25%"></asp:Label>
                </div>
                <div class="col-lg-1 col-sm-1 text-center">
                    <asp:Label ID="Label5" runat="server" Text="50%"></asp:Label>
                </div>
                <div class="col-lg-1 col-sm-1 text-center">
                    <asp:Label ID="Label7" runat="server" Text="75%"></asp:Label>
                </div>
                <div class="col-lg-1 col-sm-1 text-center">
                    <asp:Label ID="Label17" runat="server" Text="100%"></asp:Label>
                </div>
                <div class="col-lg-1 col-sm-1 text-center">
                    <asp:Label ID="Label18" runat="server" Text="VoBo"></asp:Label>
                </div>
                <div class="col-lg-2 col-sm-2 text-left">
                    <asp:Label ID="Label15" runat="server" Text="Calificación"></asp:Label>
                </div>                
            </div>
            <asp:ListView ID="ListGrupos" runat="server" OnItemDataBound="ListGrupos_ItemDataBound" OnItemCommand="ListGrupos_ItemCommand">
                <ItemTemplate>
                    <div class="col-lg-12 col-sm-12">
                        <div class="col-lg-4 col-sm-4 text-left">
                            <asp:Label ID="lblIdGrupo" runat="server" Text='<%# Eval("id") %>' Visible="false"></asp:Label>
                            <asp:Label ID="lblGrupo" runat="server" Text='<%# Eval("grupo") %>'></asp:Label>
                        </div>
                        <div class="col-lg-1 col-sm-1 text-center">
                            <asp:CheckBox ID="chkP25" runat="server" Checked='<%# Eval("chk25") %>' AutoPostBack="true" OnCheckedChanged="CheckedChanged" />
                        </div>
                        <div class="col-lg-1 col-sm-1 text-center">
                            <asp:CheckBox ID="chkP50" runat="server" Checked='<%# Eval("chk50") %>' AutoPostBack="true" OnCheckedChanged="CheckedChanged" />
                        </div>
                        <div class="col-lg-1 col-sm-1 text-center">
                            <asp:CheckBox ID="chkP75" runat="server" Checked='<%# Eval("chk75") %>' AutoPostBack="true" OnCheckedChanged="CheckedChanged" />
                        </div>
                        <div class="col-lg-1 col-sm-1 text-center">
                            <asp:CheckBox ID="chkP100" runat="server" Checked='<%# Eval("chk100") %>' AutoPostBack="true" OnCheckedChanged="CheckedChanged" />
                        </div>
                        <div class="col-lg-1 col-sm-1 text-center">
                            <asp:CheckBox ID="chkVoBo" runat="server" Checked='<%# Eval("chkVoBo") %>' AutoPostBack="true" OnCheckedChanged="CheckedChanged" />
                        </div>
                        <div class="col-lg-2 col-sm-2 text-left">
                            <asp:DropDownList ID="ddlCalificacion" runat="server" DataSourceID="SqlDataSource3" DataTextField="descripcion" DataValueField="id_calificacion"></asp:DropDownList>
                            <asp:SqlDataSource runat="server" ID="SqlDataSource3" ConnectionString='<%$ ConnectionStrings:PVW %>' SelectCommand="SELECT [id_calificacion], [descripcion] FROM [Calificacion]"></asp:SqlDataSource>
                        </div>                        
                            <asp:LinkButton ID="lnkCalifica" CommandArgument='<%# Eval("id") %>' CommandName="califica" Visible="false" runat="server" CssClass="btn btn-success" ToolTip="Califica"><i class="fa fa-check-circle-o"></i></asp:LinkButton>
                        
                    </div>
                </ItemTemplate>
            </asp:ListView>
        </div>
        <div class="col-lg-12 col-sm-12 text-center pad1m">
            <asp:LinkButton ID="lnkGuardar" runat="server" CssClass="btn btn-success" OnClick="lnkGuardar_Click" OnClientClick="return confirm('¿Está seguro que los datos de avance y de calificación estan correctos?')"><span><i class="fa fa-save"></i>&nbsp;Guardar</span></asp:LinkButton>
        </div>
    </asp:Panel>
    <div class="pie pad1m">
        <div class="clearfix">
            <div class="row colorBlanco textoBold">
                <div class="col-lg-4 col-sm-4 text-center">
                    <asp:Label ID="Label21" runat="server" Text="Tipo Orden:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                    <asp:Label ID="ddlToOrden" runat="server"></asp:Label></div>
                <div class="col-lg-4 col-sm-4 text-center">
                    <asp:Label ID="Label4" runat="server" Text="Cliente:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                    <asp:Label ID="ddlClienteOrden" runat="server"></asp:Label></div>
                <div class="col-lg-4 col-sm-4 text-center">
                    <asp:Label ID="Label6" runat="server" Text="Tipo Servicio:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                    <asp:Label ID="ddlTsOrden" runat="server"></asp:Label></div>
            </div>
            <div class="row colorBlanco textoBold">
                <div class="col-lg-4 col-sm-4 text-center">
                    <asp:Label ID="Label8" runat="server" Text="Tipo Valuación:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                    <asp:Label ID="ddlValOrden" runat="server"></asp:Label></div>
                <div class="col-lg-4 col-sm-4 text-center">
                    <asp:Label ID="Label10" runat="server" Text="Tipo Asegurado:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                    <asp:Label ID="ddlTaOrden" runat="server"></asp:Label></div>
                <div class="col-lg-4 col-sm-4 text-center">
                    <asp:Label ID="Label12" runat="server" Text="Localización:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                    <asp:Label ID="ddlLocOrden" runat="server"></asp:Label></div>
            </div>
            <div class="row colorBlanco textoBold">
                <div class="col-lg-4 col-sm-4 text-center">
                    <asp:Label ID="Label108" runat="server" Text="Perfil:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                    <asp:Label ID="lblPerfilPie" runat="server"></asp:Label></div>
                <div class="col-lg-4 col-sm-4 text-center">
                    <asp:Label ID="Label109" runat="server" Text="Siniestro:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                    <asp:Label ID="lblSiniestro" runat="server"></asp:Label></div>
                <div class="col-lg-4 col-sm-4 text-center">
                    <asp:Label ID="Label110" runat="server" Text="Deducible:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                    <asp:Label ID="lblDeducible" runat="server"></asp:Label></div>
            </div>
            <div class="row colorBlanco textoBold">
                <div class="col-lg-4 col-sm-4 text-center">
                    <asp:Label ID="Label111" Visible="false" runat="server" Text="Total Orden:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                    <asp:Label ID="lblTotOrden" Visible="false" runat="server"></asp:Label></div>
                <div class="col-lg-4 col-sm-4 text-center">
                    <asp:Label ID="Label112" runat="server" Text="Promesa:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                    <asp:Label ID="lblEntregaEstimada" runat="server"></asp:Label></div>
                <div class="col-lg-4 col-sm-4 text-center">
                    <asp:Label ID="lblPorcDeduEti" runat="server" Text="% Deducible:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                    <asp:Label ID="lblPorcDedu" runat="server"></asp:Label></div>
            </div>
        </div>
    </div>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                <ProgressTemplate>
                    <asp:Panel ID="pnlMaskLoad1" runat="server" CssClass="maskLoad"></asp:Panel>
                    <asp:Panel ID="pnlCargando1" runat="server" CssClass="pnlPopUpLoad">
                        <asp:Image ID="imgLoad1" runat="server" ImageUrl="~/img/loading.gif" CssClass="ancho100" />
                    </asp:Panel>
                </ProgressTemplate>
            </asp:UpdateProgress>
            </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
