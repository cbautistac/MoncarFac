<%@ Page Title="" Language="C#" MasterPageFile="~/AdmonOrden.master" AutoEventWireup="true" CodeFile="Inconformidades.aspx.cs" Inherits="Inconformidades" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-lg-12 col-sm-12 text-center alert-info">
                    <h3>
                        <i class="fa fa-frown-o"></i>&nbsp;<i class="fa fa-book"></i>&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="Label1" runat="server" Text="Inconformidades"></asp:Label>
                    </h3>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-12 col-sm-12 text-center">
                    <asp:Label ID="lblError" runat="server" CssClass="errores" />
                </div>
            </div>
            <br />
            <asp:Panel runat="server" ID="pnlInconf" CssClass="paneles col-lg-12 col-sm-12" ScrollBars="Auto">
                <div class="row">
                    <div class="col-lg-12 col-sm-12 text-center">
                        <asp:TextBox ID="txtComentario" runat="server" MaxLength="500" CssClass="alingMiddle textNota" Rows="10" TextMode="MultiLine"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtComentario_TextBoxWatermarkExtender" runat="server" BehaviorID="txtComentario_TextBoxWatermarkExtender" TargetControlID="txtComentario" WatermarkText="Inconformidad" WatermarkCssClass="water textNota" />
                    </div>
                    <div class="col-lg-12 col-sm-12 text-center">
                        <br />
                        <asp:LinkButton ID="lnkGuarda" runat="server" ToolTip="Guarda Inconformidad" CssClass="btn btn-success t14" OnClick="lnkGuarda_Click"><i class="fa fa-save"></i><span>&nbsp;Guarda Inconformidad</span></asp:LinkButton>
                    </div>
                </div>
                <br />
                <br />
                <div class="row">
                    <div class="col-lg-12 col-sm-12">
                        <asp:Panel ID="Panel1" runat="server" CssClass="panelCatalogos text-center" ScrollBars="Auto">
                            <div class="table-responsive">
                                <asp:GridView ID="GridView1" runat="server" EmptyDataText="No existen incorformidades registradas" EmptyDataRowStyle-CssClass="errores" AutoGenerateColumns="False" CssClass="table table-bordered" AllowPaging="true" AllowSorting="true" PageSize="7" DataKeyNames="no_orden,id_empresa,id_taller,id_inconformidad" DataSourceID="SqlDataSource1">
                                    <Columns>
                                        <asp:BoundField Visible="false" DataField="no_orden" HeaderText="no_orden" ReadOnly="True" SortExpression="no_orden"></asp:BoundField>
                                        <asp:BoundField Visible="false" DataField="id_empresa" HeaderText="id_empresa" ReadOnly="True" SortExpression="id_empresa"></asp:BoundField>
                                        <asp:BoundField Visible="false" DataField="id_taller" HeaderText="id_taller" ReadOnly="True" SortExpression="id_taller"></asp:BoundField>
                                        <asp:BoundField Visible="false" DataField="id_inconformidad" HeaderText="id_inconformidad" ReadOnly="True" SortExpression="id_inconformidad"></asp:BoundField>
                                        <asp:TemplateField HeaderText="Descripción" SortExpression="descripcion">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Bind("descripcion") %>' ID="Label50"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" Text="Eliminar" CommandName="Delete" CausesValidation="False" ID="lnkEliminar" CssClass="btn btn-danger" OnClientClick="retunr confirm('¿Esta seguro de eliminar el registro?');"><i class="fa fa-trash"></i></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="col-lg-2 col-sm-2" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:PVW %>'
                                    DeleteCommand="delete from inconformidades where no_orden=@no_orden and id_empresa=@id_empresa and id_taller=@id_taller and id_inconformidad=@id_inconformidad"
                                    InsertCommand="insert into inconformidades values(@no_orden,@id_empresa,@id_taller,(select isnull((select top 1 i.id_inconformidad from inconformidades i where i.no_orden=@no_orden and i.id_empresa=@id_empresa and i.id_taller=@id_taller order by i.id_inconformidad desc),0)+1),@descripcion)"
                                    SelectCommand="select no_orden,id_empresa,id_taller,id_inconformidad,descripcion from inconformidades where no_orden=@no_orden and id_empresa=@id_empresa and id_taller=@id_taller">
                                    <DeleteParameters>
                                        <asp:Parameter Name="no_orden"></asp:Parameter>
                                        <asp:Parameter Name="id_empresa"></asp:Parameter>
                                        <asp:Parameter Name="id_taller"></asp:Parameter>
                                        <asp:Parameter Name="id_inconformidad"></asp:Parameter>
                                    </DeleteParameters>
                                    <InsertParameters>
                                        <asp:QueryStringParameter QueryStringField="o" Name="no_orden" />
                                        <asp:QueryStringParameter QueryStringField="e" Name="id_empresa" />
                                        <asp:QueryStringParameter QueryStringField="t" Name="id_taller" />
                                        <asp:ControlParameter ControlID="txtComentario" Name="descripcion" />
                                    </InsertParameters>
                                    <SelectParameters>
                                        <asp:QueryStringParameter QueryStringField="o" Name="no_orden"></asp:QueryStringParameter>
                                        <asp:QueryStringParameter QueryStringField="e" Name="id_empresa"></asp:QueryStringParameter>
                                        <asp:QueryStringParameter QueryStringField="t" Name="id_taller"></asp:QueryStringParameter>
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </asp:Panel>
            <div class="pie pad1m">
                <div class="clearfix">
                    <div class="row colorBlanco textoBold">
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label21" runat="server" Text="Tipo Orden:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
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
                    <asp:Label ID="lblPerfilPie" runat="server"></asp:Label>
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
                            <asp:Label ID="Label111" Visible="false" runat="server" Text="Total Orden:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
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
</asp:Content>
