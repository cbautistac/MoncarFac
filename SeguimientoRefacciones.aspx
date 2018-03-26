<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SeguimientoRefacciones.aspx.cs" Inherits="SeguimientoRefacciones" MasterPageFile="~/AdmonOrden.master" Culture="es-Mx" UICulture="es-Mx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-lg-12 col-sm-12 text-center alert-info">
                    <h3>
                        <i class="fa fa-cog"></i></i>&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblTit" runat="server" Text="Seguimiento de Refacciones"></asp:Label>
                    </h3>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-12 col-sm-12 text-center">                    
                    <asp:Label ID="lblError" runat="server" CssClass="errores" />
                </div>
            </div>

            <asp:Panel runat="server" ID="Panel1" CssClass="paneles col-lg-12 col-sm-12" ScrollBars="Auto">
                <asp:Panel ID="PnlCotizaciones" runat="server" CssClass="col-lg-12 col-sm-12">
                    <div class="table-responsive">
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                            AllowPaging="True" AllowSorting="True" PageSize="9"
                            DataKeyNames="refOrd_Id" 
                            DataSourceID="SqlDataSource1" CssClass="table table-bordered" 
                            onrowcommand="GridView1_RowCommand" onrowdatabound="GridView1_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="refOrd_Id" InsertVisible="False" 
                                    SortExpression="refOrd_Id" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("refOrd_Id") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="Label2" runat="server" Text='<%# Eval("refOrd_Id") %>'></asp:Label>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="refCantidad" HeaderText="Cantidad" SortExpression="refCantidad" ReadOnly="True"/>
                                <asp:BoundField DataField="refDescripcion" HeaderText="Refacción" SortExpression="refDescripcion" ReadOnly="True"/>
                                <asp:TemplateField HeaderText="Estatus" SortExpression="estatus">
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("estatus") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="Label11" runat="server" Text='<%# Bind("estatus") %>'></asp:Label>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="refFechSolicitud" HeaderText="Solicitud" SortExpression="refFechSolicitud" ReadOnly="True"/>
                                <asp:BoundField DataField="refFechEntregaEst" HeaderText="Promesa" SortExpression="refFechEntregaEst" ReadOnly="True"/>
                                <asp:TemplateField HeaderText="Entrega Real" 
                                    SortExpression="refFechEntregaReal">
                                    <ItemTemplate>
                                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("refFechEntregaReal") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtFecha" runat="server" CssClass="alingMiddle input-small" Enabled="false"></asp:TextBox>
                                        <cc1:CalendarExtender ID="txtFecha_CalendarExtender" runat="server" BehaviorID="txtFecha_CalendarExtender" TargetControlID="txtFecha" Format="yyyy-MM-dd" PopupButtonID="lnkFecha" />
                                        <asp:LinkButton ID="lnkFecha" runat="server" CssClass="btn btn-info t14"><i class="fa fa-calendar"></i></asp:LinkButton>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEdita" runat="server" CausesValidation="False" 
                                            CommandName="Edit" ToolTip="Surtido" CommandArgument='<%# Eval("refOrd_id") %>' CssClass="btn btn-primary"><i class="fa fa-check"></i><span>&nbsp;Surtido</span></asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton ID="lnkActualiza" runat="server" CausesValidation="True" 
                                            CommandName="Update" ToolTip="Actualizar" CommandArgument='<%# Eval("refOrd_id") %>' CssClass="btn btn-success"><i class="fa fa-save"></i></asp:LinkButton>
                                        &nbsp;<asp:LinkButton ID="lnkCancela" runat="server" CausesValidation="False" 
                                            CommandName="Cancel" ToolTip="Cancelar" CssClass="btn btn-danger"><i class="fa fa-remove"></i></asp:LinkButton>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                            ConnectionString="<%$ ConnectionStrings:PVW %>" 
                            SelectCommand="SELECT r.refOrd_Id,r.refCantidad,r.refDescripcion,e.staDescripcion as estatus,r.refEstatusSolicitud,convert(char(10),r.refFechSolicitud,126) as refFechSolicitud,
convert(char(10),r.refFechEntregaEst,126) as refFechEntregaEst,convert(char(10),r.refFechEntregaReal,126) as refFechEntregaReal,r.refEstatus
FROM Refacciones_Orden r
inner join Rafacciones_Estatus e on e.staRefID=r.refEstatusSolicitud WHERE r.ref_id_empresa=@empresa AND r.ref_id_taller=@taller AND r.ref_no_orden=@orden AND r.refEstatus in ('CO','AU')">
                            <SelectParameters>
                                <asp:QueryStringParameter Name="empresa" QueryStringField="e" />
                                <asp:QueryStringParameter Name="taller" QueryStringField="t" />
                                <asp:QueryStringParameter Name="orden" QueryStringField="o" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>   
                </asp:Panel>
            </asp:Panel>

            <div class="pie pad1m">		                                		                                
		        <div class="clearfix">
			        <div class="row colorBlanco textoBold">
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label2" runat="server" Text="Tipo Orden:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="ddlToOrden" runat="server" ></asp:Label>
                        </div>
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label4" runat="server" Text="Cliente:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="ddlClienteOrden" runat="server" ></asp:Label>
                        </div>
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label6" runat="server" Text="Tipo Servicio:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="ddlTsOrden" runat="server" ></asp:Label>
                        </div>
                    </div>                                              
                    <div class="row colorBlanco textoBold">
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label8" runat="server" Text="Tipo Valuación:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="ddlValOrden" runat="server" ></asp:Label>
                        </div>
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label10" runat="server" Text="Tipo Asegurado:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="ddlTaOrden" runat="server" ></asp:Label>
                        </div>
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label12" runat="server" Text="Localización:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="ddlLocOrden" runat="server" ></asp:Label>
                        </div>
                    </div>    
                    <div class="row colorBlanco textoBold">
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label108" runat="server" Text="Perfil:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="ddlPerfil" runat="server" ></asp:Label>
                        </div>
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label109" runat="server" Text="Siniestro:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="lblSiniestro" runat="server" ></asp:Label>
                        </div>
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label110" runat="server" Text="Deducible:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="lblDeducible" runat="server" ></asp:Label>
                        </div>
                    </div>  
                    <div class="row colorBlanco textoBold">
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label111" runat="server" Text="Total Orden:" CssClass="colorEtiqueta" Visible="false"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="lblTotOrden" runat="server" Visible="false" ></asp:Label>
                        </div>  
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="Label112" runat="server" Text="Promesa:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="lblEntregaEstimada" runat="server" ></asp:Label>
                        </div>
                        <div class="col-lg-4 col-sm-4 text-center">
                            <asp:Label ID="lblPorcDeduEti" runat="server" Text="% Deducible:" CssClass="colorEtiqueta"></asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="lblPorcDedu" runat="server" ></asp:Label>
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