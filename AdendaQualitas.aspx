<%@ Page Title="" Language="C#" MasterPageFile="~/AdmOrdenes.master" AutoEventWireup="true" CodeFile="AdendaQualitas.aspx.cs" Inherits="AdendaQualitas" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" />
    <div class="page-header">
        <!-- /BREADCRUMBS -->
        <div class="clearfix">
            <h3 class="content-title pull-left">
                Addenda Qualitas</h3>            
        </div>
    </div>    
        <asp:UpdatePanel runat="server" ID="updPanelGeneralesFact">
            <ContentTemplate>                
                <div class="row marTop">
                    <div class="col-lg-12 col-sm-12 text-center">
                        <asp:Label ID="lblError" runat="server" CssClass="errores" />
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="addenda" CssClass="errores" DisplayMode="List" />
                    </div>
                </div>
                <div class="row marTop">
                    <div class="col-sm-2 col-lg-2 text-left"><asp:Label ID="Label2" runat="server" Text="No. Interno:" CssClass="textoBold"></asp:Label></div>
                    <div class="col-sm-2 col-lg-2 text-left">
                        <asp:TextBox ID="txtNoInterno" runat="server" MaxLength="2" CssClass="input-mini"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Debe indicar el No. Interno" CssClass="errores" Text="*" ControlToValidate="txtNoInterno" ValidationGroup="addenda"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-sm-2 col-lg-2 text-left"><asp:Label ID="Label3" runat="server" Text="Id Area:" CssClass="textoBold"></asp:Label></div>
                    <div class="col-sm-2 col-lg-2 text-left">
                        <asp:TextBox ID="txtIdArea" runat="server" MaxLength="3" CssClass="input-mini"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Debe indicar el Id Area" CssClass="errores" Text="*" ControlToValidate="txtIdArea" ValidationGroup="addenda"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-sm-2 col-lg-2 text-left"><asp:Label ID="Label4" runat="server" Text="Id Revisión:" CssClass="textoBold"></asp:Label></div>
                    <div class="col-sm-2 col-lg-2 text-left">
                        <asp:TextBox ID="txtIdRevision" runat="server" MaxLength="3" CssClass="input-mini"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Debe indicar el Id Revisión" CssClass="errores" Text="*" ControlToValidate="txtIdRevision" ValidationGroup="addenda"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row marTop">
                    <div class="col-sm-2 col-lg-2 text-left"><asp:Label ID="Label5" runat="server" Text="Cdg. Int. Emisor:" CssClass="textoBold"></asp:Label></div>
                    <div class="col-sm-2 col-lg-2 text-left">
                        <asp:TextBox ID="txtCdgIntEmisor" runat="server" MaxLength="5" CssClass="input-mini"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Debe indicar el Cdg. Int. Emisor" CssClass="errores" Text="*" ControlToValidate="txtCdgIntEmisor" ValidationGroup="addenda"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-sm-2 col-lg-2 text-left"><asp:Label ID="Label6" runat="server" Text="Oficina Entrega:" CssClass="textoBold"></asp:Label></div>
                    <div class="col-sm-2 col-lg-2 text-left">
                        <asp:TextBox ID="txtOficina" runat="server" MaxLength="3" CssClass="input-mini"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Debe indicar la Oficina Entrega" CssClass="errores" Text="*" ControlToValidate="txtOficina" ValidationGroup="addenda"></asp:RequiredFieldValidator>
                    </div>                    

                    <div class="col-sm-2 col-lg-2 text-left"><asp:Label ID="Label7" runat="server" Text="Cdg. Int. Receptor:" CssClass="textoBold"></asp:Label></div>
                    <div class="col-sm-2 col-lg-2 text-left">
                        <asp:TextBox ID="txtCdgIntRec" runat="server" MaxLength="40" CssClass="input-medium"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Debe indicar el Cdg. Int. Receptor" CssClass="errores" Text="*" ControlToValidate="txtCdgIntRec" ValidationGroup="addenda"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row marTop">
                    <div class="col-lg-6 col-sm-6">
                        <div class="col-sm-12 col-lg-12 text-left pad1m alert-info">
                            <asp:Label ID="Label1" runat="server" Text="Contacto Emisor" CssClass="textoBold"></asp:Label>
                        </div>
                        <div class="col-sm-2 col-lg-2 text-left"><asp:Label ID="Label9" runat="server" Text="Tipo:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-sm-10 col-lg-10 text-left">
                            <asp:DropDownList ID="ddlTipoEmisor" runat="server" CssClass="input-medium">
                                <asp:ListItem Text="Seleccione..." Value=""></asp:ListItem>
                                <asp:ListItem Text="vendedor" Value="vendedor"></asp:ListItem>
                                <asp:ListItem Text="empleado" Value="empleado"></asp:ListItem>
                                <asp:ListItem Text="sucursal" Value="sucursal"></asp:ListItem>
                                <asp:ListItem Text="agencia" Value="agencia"></asp:ListItem>
                                <asp:ListItem Text="departamento" Value="departamento"></asp:ListItem>
                                <asp:ListItem Text="transportista" Value="transportista"></asp:ListItem>
                                <asp:ListItem Text="distribuidor" Value="distribuidor"></asp:ListItem>
                                <asp:ListItem Text="matriz" Value="matriz"></asp:ListItem>
                                <asp:ListItem Text="otro" Value="otro"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="Debe seleccionar un Tipo de Emisor" CssClass="errores" Text="*" ControlToValidate="ddlTipoEmisor" ValidationGroup="addenda"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-sm-2 col-lg-2 text-left"><asp:Label ID="Label8" runat="server" Text="Nombre:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-sm-10 col-lg-10 text-left">
                            <asp:TextBox ID="txtNombreEmisor" runat="server" MaxLength="60" CssClass="input-large"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Debe indicar el Nombre de Emisor" CssClass="errores" Text="*" ControlToValidate="txtNombreEmisor" ValidationGroup="addenda"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-sm-2 col-lg-2 text-left"><asp:Label ID="Label10" runat="server" Text="Email:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-sm-10 col-lg-10 text-left">
                            <asp:TextBox ID="txtEmailEmisor" runat="server" MaxLength="60" CssClass="input-large"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="Debe indicar el Email de Emisor" CssClass="errores" Text="*" ControlToValidate="txtEmailEmisor" ValidationGroup="addenda"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-sm-2 col-lg-2 text-left"><asp:Label ID="Label11" runat="server" Text="Teléfono:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-sm-10 col-lg-10 text-left">
                            <asp:TextBox ID="txtTelEmisor" runat="server" MaxLength="25" CssClass="input-large"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="Debe indicar el Teléfono de Emisor" CssClass="errores" Text="*" ControlToValidate="txtTelEmisor" ValidationGroup="addenda"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-lg-6 col-sm-6">
                        <div class="col-sm-12 col-lg-12 text-left pad1m alert-info">
                            <asp:Label ID="Label16" runat="server" Text="Contacto Receptor" CssClass="textoBold"></asp:Label>
                        </div>
                        <div class="col-sm-2 col-lg-2 text-left"><asp:Label ID="Label12" runat="server" Text="Tipo:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-sm-10 col-lg-10 text-left">
                            <asp:DropDownList ID="ddlTipoReceptor" runat="server" CssClass="input-medium">
                                <asp:ListItem Text="Seleccione..." Value=""></asp:ListItem>
                                <asp:ListItem Text="vendedor" Value="vendedor"></asp:ListItem>
                                <asp:ListItem Text="empleado" Value="empleado"></asp:ListItem>
                                <asp:ListItem Text="sucursal" Value="sucursal"></asp:ListItem>
                                <asp:ListItem Text="agencia" Value="agencia"></asp:ListItem>
                                <asp:ListItem Text="departamento" Value="departamento"></asp:ListItem>
                                <asp:ListItem Text="transportista" Value="transportista"></asp:ListItem>
                                <asp:ListItem Text="distribuidor" Value="distribuidor"></asp:ListItem>
                                <asp:ListItem Text="matriz" Value="matriz"></asp:ListItem>
                                <asp:ListItem Text="coordinador" Value="coordinador"></asp:ListItem>
                                <asp:ListItem Text="otro" Value="otro"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="Debe seleccionar un Tipo de Receptor" CssClass="errores" Text="*" ControlToValidate="ddlTipoReceptor" ValidationGroup="addenda"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-sm-2 col-lg-2 text-left"><asp:Label ID="Label13" runat="server" Text="Nombre:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-sm-10 col-lg-10 text-left">
                            <asp:TextBox ID="txtNombreReceptor" runat="server" MaxLength="60" CssClass="input-large"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="Debe indicar el Nombre de Receptor" CssClass="errores" Text="*" ControlToValidate="txtNombreReceptor" ValidationGroup="addenda"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-sm-2 col-lg-2 text-left"><asp:Label ID="Label14" runat="server" Text="Email:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-sm-10 col-lg-10 text-left">
                            <asp:TextBox ID="txtEmailReceptor" runat="server" MaxLength="60" CssClass="input-large"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="Debe indicar el Email de Receptor" CssClass="errores" Text="*" ControlToValidate="txtEmailReceptor" ValidationGroup="addenda"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-sm-2 col-lg-2 text-left"><asp:Label ID="Label15" runat="server" Text="Teléfono:" CssClass="textoBold"></asp:Label></div>
                        <div class="col-sm-10 col-lg-10 text-left">
                            <asp:TextBox ID="txtTelReceptor" runat="server" MaxLength="25" CssClass="input-large"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="Debe indicar el Teléfono de Emisor" CssClass="errores" Text="*" ControlToValidate="txtTelReceptor" ValidationGroup="addenda"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div class="row text-center marTop">
                    <div class="col-sm-12 col-lg-12 text-center pad1m alert-info">
                        <asp:Label ID="Label17" runat="server" Text="Generales" CssClass="textoBold"></asp:Label>
                    </div>                   
                </div>
                <div class="row text-center marTop">                    
                     <div class="col-sm-4 col-lg-4 text-center marTop center">
                        <asp:RadioButtonList ID="rbTipo" runat="server" RepeatColumns="3" CellPadding="10" CellSpacing="10">
                            <asp:ListItem Text="Mano Obra" Value="M"></asp:ListItem>
                            <asp:ListItem Text="Refacciones" Value="R"></asp:ListItem>
                            <asp:ListItem Text="Global" Value="G" Selected="True"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>                    
                    <div class="col-sm-4 col-lg-4 text-center marTop center">
                        <asp:Label ID="Label19" runat="server" Text="Periodo: " CssClass="textoBold"></asp:Label>
                        <asp:TextBox ID="txtPeriodo" runat="server" MaxLength="4" CssClass="input-small" placeholder="periodo"></asp:TextBox>
                    </div>
                    <div class="col-sm-4 col-lg-4 text-center marTop center">
                        <asp:Label ID="Label20" runat="server" Text="Tipo Cliente: " CssClass="textoBold"></asp:Label>
                        <asp:DropDownList ID="ddlTipoCliente" runat="server" CssClass="input-medium">                            
                            <asp:ListItem Text="Asegurado" Value="0" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Tercero" Value="1"></asp:ListItem>                            
                        </asp:DropDownList>                        
                    </div>
                </div>
                <div class="row text-center marTop">                    
                     <div class="col-sm-3 col-lg-3 text-center marTop center">
                        <asp:Label ID="Label25" runat="server" Text="Deducible: " CssClass="textoBold"></asp:Label>
                        <telerik:RadNumericTextBox RenderMode="Lightweight" runat="server" ID="txtDeducible" CssClass="input-mini" EmptyMessage="Deducible" MinValue="0" MaxValue="9999999999999.99" ShowSpinButtons="false" NumberFormat-DecimalDigits="2" Skin="MetroTouch"></telerik:RadNumericTextBox>
                    </div>                    
                    <div class="col-sm-3 col-lg-3 text-center marTop center">
                        <asp:Label ID="Label21" runat="server" Text="Banco: " CssClass="textoBold"></asp:Label>
                        <asp:TextBox ID="txtBancoDeduc" runat="server" MaxLength="200" CssClass="input-large" placeholder="Banco"></asp:TextBox>
                    </div>
                    <div class="col-sm-3 col-lg-3 text-center marTop center">
                        <asp:Label ID="Label22" runat="server" Text="Fecha: " CssClass="textoBold"></asp:Label>
                        <asp:TextBox ID="txtFechaDeduc" runat="server" CssClass="alingMiddle input-small" Enabled="false"></asp:TextBox>
                        <cc1:CalendarExtender ID="CalendarExtender16" runat="server" TargetControlID="txtFechaDeduc" Format="yyyy-MM-dd" PopupButtonID="lnkFProgTra" />
                        <asp:LinkButton ID="lnkFProgTra" runat="server" CssClass="btn btn-info t14"><i class="fa fa-calendar"></i></asp:LinkButton>
                    </div>
                    <div class="col-sm-3 col-lg-3 text-center marTop center">
                        <asp:Label ID="Label23" runat="server" Text="Folio: " CssClass="textoBold"></asp:Label>
                        <asp:TextBox ID="txtFolioDeduc" runat="server" MaxLength="200" CssClass="input-large" placeholder="Folio"></asp:TextBox>
                    </div>
                </div>
                <div class="row text-center marTop">                    
                     <div class="col-sm-3 col-lg-3 text-center marTop center">
                        <asp:Label ID="Label24" runat="server" Text="Demerito: " CssClass="textoBold"></asp:Label>
                        <telerik:RadNumericTextBox RenderMode="Lightweight" runat="server" ID="txtDemerito" CssClass="input-mini" EmptyMessage="Deducible" MinValue="0" MaxValue="9999999999999.99" ShowSpinButtons="false" NumberFormat-DecimalDigits="2" Skin="MetroTouch"></telerik:RadNumericTextBox>
                    </div>                    
                    <div class="col-sm-3 col-lg-3 text-center marTop center">
                        <asp:Label ID="Label26" runat="server" Text="Banco: " CssClass="textoBold"></asp:Label>
                        <asp:TextBox ID="txtBancoDeme" runat="server" MaxLength="200" CssClass="input-large" placeholder="Banco"></asp:TextBox>
                    </div>
                    <div class="col-sm-3 col-lg-3 text-center marTop center">
                        <asp:Label ID="Label27" runat="server" Text="Fecha: " CssClass="textoBold"></asp:Label>
                        <asp:TextBox ID="txtFechaDeme" runat="server" CssClass="alingMiddle input-small" Enabled="false"></asp:TextBox>
                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFechaDeme" Format="yyyy-MM-dd" PopupButtonID="lnkDeme" />
                        <asp:LinkButton ID="lnkDeme" runat="server" CssClass="btn btn-info t14"><i class="fa fa-calendar"></i></asp:LinkButton>
                    </div>
                    <div class="col-sm-3 col-lg-3 text-center marTop center">
                        <asp:Label ID="Label28" runat="server" Text="Folio: " CssClass="textoBold"></asp:Label>
                        <asp:TextBox ID="txtFolioDeme" runat="server" MaxLength="200" CssClass="input-large" placeholder="Folio"></asp:TextBox>
                    </div>
                </div>

                
                <div class="row text-center marTop">                    
                    <div class="col-lg-12 col-sm-12 text-center">
                        <asp:Label ID="Label18" runat="server" Text="Folio:"></asp:Label>
                        <asp:TextBox ID="txtFolio" runat="server" MaxLength="12" CssClass="input-medium" placeholder="Folio"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="Indique el Folio" CssClass="text-danger" ValidationGroup="folio" ControlToValidate="txtFolio"></asp:RequiredFieldValidator><br /><br />
                        <asp:LinkButton ID="lnkAgregarFolio" runat="server" ValidationGroup="folio" CssClass="btn btn-primary" OnClick="lnkAgregarFolio_Click"><i class="fa fa-plus-circle"></i><span>&nbsp;Agregar Folio</span></asp:LinkButton>
                    </div>                    
                </div>
                <div class="row text-center marTop">                    
                    <div class="col-lg-12 col-sm-12 text-center">
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered" AllowPaging="True" AllowSorting="True" PageSize="7" EmptyDataText="No existen Folios registrados" >
                            <Columns>                                
                                <asp:BoundField DataField="renglon" HeaderText="renglon" SortExpression="renglon" Visible="false" />
                                <asp:BoundField DataField="folio" HeaderText="Folio" SortExpression="folio" />
                                <asp:TemplateField ShowHeader="False" Visible="false">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkCancelar" runat="server" CausesValidation="False" CommandArgument='<%# Eval("renglon") %>'
                                            CommandName="Cancel" ToolTip="Cancelar" CssClass="btn btn-danger" OnClientClick="return confirm('¿Está seguro de eliminar el folio?')"
                                            onclick="lnkCancelar_Click"><i class="fa fa-trash"></i></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>                    
                </div>



                <div class="row text-center marTop">
                    <div class="col-sm-6 col-lg-6 text-center">
                        <asp:LinkButton ID="lnkGuardar" runat="server" CssClass="btn btn-info t14" 
                            ValidationGroup="addenda" onclick="lnkGuardar_Click"><i class="fa fa-save"></i><span>&nbsp;Guardar</span></asp:LinkButton>
                    </div>
                    <div class="col-sm-6 col-lg-6 text-center">
                        <asp:LinkButton ID="lnkRegresa" runat="server" CssClass="btn btn-danger t14" 
                                onclick="lnkRegresa_Click"><i class="fa fa-reply"></i><span>&nbsp;Regresar</span></asp:LinkButton>
                    </div>
                </div>
                <asp:UpdateProgress ID="UpdateProgressgral" runat="server" AssociatedUpdatePanelID="updPanelGeneralesFact">
                    <ProgressTemplate>
                        <asp:Panel ID="pnlMaskLoadgral" runat="server" CssClass="maskLoad" />
                        <asp:Panel ID="pnlCargandogral" runat="server" CssClass="pnlPopUpLoad">
                            <asp:Image ID="imgLoadgral" runat="server" ImageUrl="~/img/loading.gif" CssClass="ancho100" />
                        </asp:Panel>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="lnkGuardar" />
            </Triggers>
        </asp:UpdatePanel>
    
</asp:Content>

