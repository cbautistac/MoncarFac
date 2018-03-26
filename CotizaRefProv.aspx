<%@ Page Title="" Language="C#" MasterPageFile="~/AdmonOrden.master" AutoEventWireup="true" CodeFile="CotizaRefProv.aspx.cs" Inherits="CotizaRefProv" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" />
    <asp:UpdatePanel runat="server" ID="updPnlRefacc">
        <ContentTemplate>
            <div class="row">
                <div class="col-lg-12 col-sm-12 text-center alert-info">
                    <h3>
                        <i class="fa fa-file"></i>&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblTit" runat="server" Text="Cotización"></asp:Label>
                    </h3>
                </div>
            </div>
            <asp:Panel ID="PanelAsigProv" runat="server" CssClass="col-lg-12 col-sm-12 paneles">
                <div class="row">
                    <div class="col-lg-12 col-sm-12 text-center">
                        <asp:Label ID="Label3" runat="server" Text="Folio:" />&nbsp;
                        <asp:TextBox ID="txtFolio" MaxLength="50" runat="server" placeholder="Folio" />   
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Debe indicar el Folio de la Cotización" ControlToValidate="txtFolio" ValidationGroup="cotiza" CssClass="errores"></asp:RequiredFieldValidator>                     
                        <asp:Label ID="lblFolio" runat="server" Visible="false" />
                    </div>
                    <div class="col-lg-12 col-sm-12 text-center">
                        <asp:Label ID="lblError" runat="server" CssClass="alert-danger errores" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-6 col-sm-6">
                        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" EnableAJAX="true">
                            <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" AllowFilteringByColumn="true" runat="server"  EnableHeaderContextMenu="true" Culture="es-Mx" Skin="Metro"
                                EnableHeaderContextFilterMenu="true" AllowPaging="True" PagerStyle-AlwaysVisible="true" DataSourceID="SqlDataSource2" AllowSorting="true" GroupingEnabled="false" PageSize="100">
                                <MasterTableView DataSourceID="SqlDataSource2" AutoGenerateColumns="False" DataKeyNames="id_cliprov">
                                    <Columns>
                                    <telerik:GridTemplateColumn FilterCheckListEnableLoadOnDemand="true" HeaderText="Proveedor" SortExpression="razon_social" UniqueName="razon_social" FilterControlAltText="Filtro Proveedor" DataField="razon_social">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkAsignaProvCotiza" ToolTip="Agregar a lista de cotización" CommandArgument='<%# Bind("id_cliprov") %>' OnClick="lnkAsignaProvCotiza_Click" CssClass="textoBold link" runat="server" Text='<%# Bind("razon_social") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>                                
                                        <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="correo" FilterControlAltText="Filtro Correo" HeaderText="Correo" SortExpression="correo" UniqueName="correo" Resizable="true"/>
                                        <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="zona" FilterControlAltText="Filtro Zona" HeaderText="Zona" SortExpression="zona" UniqueName="zona" Resizable="true"/>
                                    </Columns>
                                    <NoRecordsTemplate>
                                        <asp:Label runat="server" ID="lblVacio" Text="No existen Proveedores autorizados" CssClass="errores"></asp:Label>
                                    </NoRecordsTemplate>
                                </MasterTableView>
                                <ClientSettings>
                                    <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" ></Scrolling>
                                </ClientSettings>                        
                                <PagerStyle PageSizeControlType="RadDropDownList" Mode="NextPrevAndNumeric"></PagerStyle>
                            </telerik:RadGrid>
                        </telerik:RadAjaxPanel>

                        <asp:SqlDataSource runat="server" ID="SqlDataSource2" ConnectionString='<%$ ConnectionStrings:PVW %>'
                            SelectCommand="select c.id_cliprov,c.razon_social,c.correo,z.descripcion as zona from cliprov c left join cat_zona_cliprov z on z.id_zona=c.id_zona where c.tipo = 'P' and c.estatus='A' and c.aseguradora=1 and c.id_cliprov not in (select pc.id_cliprov from Proveedor_Cotizacion_Tmp pc where pc.no_orden=@orden and pc.id_empresa=@empresa and pc.id_taller=@taller) and c.id_cliprov<>0">
                            <SelectParameters>
                                <asp:QueryStringParameter QueryStringField="o" Name="orden"></asp:QueryStringParameter>
                                <asp:QueryStringParameter QueryStringField="t" Name="taller"></asp:QueryStringParameter>
                                <asp:QueryStringParameter QueryStringField="e" Name="empresa"></asp:QueryStringParameter>
                            </SelectParameters>
                         </asp:SqlDataSource>
                    </div>
                    <div class="col-lg-6 col-sm-6">
                        <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" EnableAJAX="true">
                            <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid2" AllowFilteringByColumn="true" runat="server"  EnableHeaderContextMenu="true" Culture="es-Mx" Skin="Metro"
                                EnableHeaderContextFilterMenu="true" AllowPaging="True" PagerStyle-AlwaysVisible="true" DataSourceID="SqlDataSource3" AllowSorting="true" GroupingEnabled="false" PageSize="100">
                                <MasterTableView DataSourceID="SqlDataSource3" AutoGenerateColumns="False" DataKeyNames="id_cliprov">
                                    <Columns>
                                    <telerik:GridTemplateColumn FilterCheckListEnableLoadOnDemand="true" HeaderText="Proveedor A Cotizar" SortExpression="razon_social" UniqueName="razon_social" FilterControlAltText="Filtro Proveedor" DataField="razon_social">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDelProv" OnClick="lnkDelProv_Click" runat="server" Text='<%# Bind("razon_social") %>' CommandArgument='<%# Eval("id_cliprov")+";"+Eval("folio") %>' CssClass="textoBold link" ToolTip="Quitar proveedor de cotización" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>                                
                                        <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="correo" FilterControlAltText="Filtro Correo" HeaderText="Correo" SortExpression="correo" UniqueName="correo" Resizable="true"/>
                                        <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="zona" FilterControlAltText="Filtro Zona" HeaderText="Zona" SortExpression="zona" UniqueName="zona" Resizable="true"/>
                                    </Columns>
                                    <NoRecordsTemplate>
                                        <asp:Label runat="server" ID="lblVacio" Text="No se han seleccionado proveedores para cotizar" CssClass="errores"></asp:Label>
                                    </NoRecordsTemplate>
                                </MasterTableView>
                                <ClientSettings>
                                    <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" ></Scrolling>
                                </ClientSettings>                        
                                <PagerStyle PageSizeControlType="RadDropDownList" Mode="NextPrevAndNumeric"></PagerStyle>
                            </telerik:RadGrid>
                        </telerik:RadAjaxPanel>                        
                        <asp:SqlDataSource runat="server" ID="SqlDataSource3" ConnectionString='<%$ ConnectionStrings:PVW %>'
                            SelectCommand="select p.id_cliprov,c.razon_social,p.folio,c.correo,z.descripcion as zona from Proveedor_Cotizacion_Tmp p inner join Cliprov c on c.id_cliprov=p.id_cliprov and c.tipo='P' left join cat_zona_cliprov z on z.id_zona=c.id_zona where p.no_orden=@no_orden and p.id_taller =@id_taller and p.id_empresa=@id_empresa and c.correo<>'' and not c.correo is null and p.id_cliprov<>0">
                            <SelectParameters>
                                <asp:QueryStringParameter QueryStringField="o" Name="no_orden"></asp:QueryStringParameter>
                                <asp:QueryStringParameter QueryStringField="t" Name="id_taller"></asp:QueryStringParameter>
                                <asp:QueryStringParameter QueryStringField="e" Name="id_empresa"></asp:QueryStringParameter>                                
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                </div>
            </asp:Panel>
            <div class="row">
                <div class="col-lg-12 col-sm-12 text-center">
                    <asp:LinkButton ID="lnkEnviarCotizacion" runat="server" OnClick="lnkEnviarCotizacion_Click" CssClass="btn btn-info" ValidationGroup="cotiza" ><i class="fa fa-envelope"></i><span>&nbsp;Enviar</span></asp:LinkButton>
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-lg-12 col-sm-12 text-center pad1m alert-info">
                    <asp:Label runat="server" ID="lblTituloEnvios" Text="Envio de Cotización" ></asp:Label>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-12 col-sm-12 text-center">
                    <asp:Label ID="lblCotizacion" runat="server" Visible="false" />
                    
                    <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid3" runat="server"  Culture="es-Mx" Skin="Metro" CssClass="col-lg-12 col-sm-12" AllowAutomaticUpdates="True" AutoGenerateColumns="False" 
                                    AllowAutomaticInserts="false" AllowAutomaticDeletes="false" AllowPaging="True" PagerStyle-AlwaysVisible="true" DataSourceID="SqlDataSource4" AllowSorting="true" GroupingEnabled="false" PageSize="20">
                        <MasterTableView DataSourceID="SqlDataSource4" AutoGenerateColumns="False" DataKeyNames="no_orden,id_empresa,id_taller,id_cotizacion,id_cliprov"  CommandItemDisplay="Bottom" HorizontalAlign="NotSet" EditMode="Batch">
                            <BatchEditingSettings EditType="Row" />
                            <CommandItemStyle CssClass="text-right" />
                            <CommandItemSettings SaveChangesText="Guardar Cambios" ShowAddNewRecordButton="false"  ShowRefreshButton="false" ShowSaveChangesButton="true" CancelChangesText="Cancelar Cambios"/>
                            <Columns>
                                <telerik:GridBoundColumn UniqueName="id_cotizacion" HeaderText="id_cotizacion" SortExpression="id_cotizacion" Visible="false" DataField="id_cotizacion" ReadOnly="true"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn UniqueName="id_cliprov" HeaderText="id_cliprov" SortExpression="id_cliprov" Visible="false" DataField="id_cliprov" ReadOnly="true"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn UniqueName="razon_social" HeaderText="Proveedor" SortExpression="razon_social" DataField="razon_social" ReadOnly="true"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn UniqueName="fecha" HeaderText="Fecha" SortExpression="fecha" DataField="fecha" ReadOnly="true"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn UniqueName="hora" HeaderText="Hora" SortExpression="hora" DataField="hora" ReadOnly="true"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn UniqueName="correo" HeaderText="Correo" SortExpression="correo" DataField="correo"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn UniqueName="nombre_usuario" HeaderText="Usuario" SortExpression="nombre_usuario" DataField="nombre_usuario" ReadOnly="true"></telerik:GridBoundColumn>
                                <telerik:GridCheckBoxColumn UniqueName="enviado" HeaderText="Enviado" SortExpression="enviado" DataField="enviado" ReadOnly="true"></telerik:GridCheckBoxColumn>
                                <telerik:GridBoundColumn UniqueName="motivo_fallo" HeaderText="Motivo" SortExpression="motivo_fallo" DataField="motivo_fallo" ReadOnly="true"></telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkReenviar" runat="server" CssClass="btn btn-success colorBlanco" CommandArgument='<%# Eval("id_cotizacion")+";"+Eval("id_cliprov")+";"+Eval("correo") %>' OnClick="lnkReenviar_Click"><i class="fa fa-mail-reply"></i></asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>                       
                            </Columns>                            
                            <NoRecordsTemplate>
                                <asp:Label runat="server" ID="lblVacio" Text="No existen cotizaciones enviados" CssClass="errores"></asp:Label>
                            </NoRecordsTemplate>
                        </MasterTableView>                        
                        <ClientSettings AllowKeyboardNavigation="true">
                            <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" ></Scrolling>
                            <Selecting AllowRowSelect="true" /> 
                        </ClientSettings>                        
                        <PagerStyle PageSizeControlType="RadDropDownList" Mode="NextPrevAndNumeric"></PagerStyle>
                    </telerik:RadGrid>
                    <asp:SqlDataSource runat="server" ID="SqlDataSource4" ConnectionString='<%$ ConnectionStrings:PVW %>'
                            SelectCommand="select c.no_orden,c.id_empresa,c.id_taller, c.id_cotizacion, c.id_cliprov,p.razon_social,convert(char(10),c.fecha,120) as fecha,convert(char(8),c.hora,120) as hora,c.correo,u.nombre_usuario,c.motivo_fallo,c.enviado
from cotizaciones_enviadas c 
inner join Cliprov p on p.id_cliprov = c.id_cliprov and p.tipo='P'
inner join usuarios u on u.id_usuario=c.usuario
where c.no_orden=@no_orden and c.id_empresa=@id_empresa and c.id_taller=@id_taller and id_cotizacion=@id_cotizacion"
                            UpdateCommand ="update cotizaciones_enviadas set correo=@correo where no_orden=@no_orden and id_empresa=@id_empresa and id_taller=@id_taller and id_cotizacion=@id_cotizacion and id_cliprov=@id_cliprov update cliprov set correo=@correo where id_cliprov=@id_cliprov and tipo='P' ">
                            <SelectParameters>
                                <asp:QueryStringParameter QueryStringField="o" Name="no_orden"></asp:QueryStringParameter>
                                <asp:QueryStringParameter QueryStringField="t" Name="id_taller"></asp:QueryStringParameter>
                                <asp:QueryStringParameter QueryStringField="e" Name="id_empresa"></asp:QueryStringParameter>                                
                                <asp:ControlParameter ControlID="lblCotizacion" Name="id_cotizacion" Type="Int32" DefaultValue="0" PropertyName="Text" />
                            </SelectParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="correo" Type="String" />
                            <asp:Parameter Name="no_orden" Type="Int32" />
                            <asp:Parameter Name="id_empresa" Type="Int32" />
                            <asp:Parameter Name="id_taller" Type="Int32" />
                            <asp:Parameter Name="id_cotizacion" Type="Int32" />
                            <asp:Parameter Name="id_cliprov" Type="Int32" />                            
                        </UpdateParameters>
                        </asp:SqlDataSource>
                </div>
            </div>
            <br /><br /><br />

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
                            <asp:Label ID="Label111" runat="server" Text="Total Orden:" CssClass="colorEtiqueta" Visible="false"></asp:Label>&nbsp;&nbsp;
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


            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="updPnlRefacc">
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

