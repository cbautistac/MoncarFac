<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Empresas_Talleres.aspx.cs" Inherits="Empresas_Talleres" MasterPageFile="~/Administracion.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="row">
        <div class="col-lg-12 col-sm-12 text-center alert-info">
            <h3>
                <i class="fa fa-hospital-o"></i>&nbsp;
                        <i class="fa fa-industry"></i>&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="Label1" runat="server" Text="Empresas Talleres"></asp:Label>
            </h3>
        </div>
    </div>
    <div class="row pad1m">
        <div class="col-lg-12 col-sm-12 text-center">
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="agrega" CssClass="errores alert-danger" DisplayMode="List" />
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="edit" CssClass="errores alert-danger" DisplayMode="List" />
            <asp:Label ID="lblError" runat="server" CssClass="errores alert-danger" />
        </div>
    </div>
    <asp:Panel ID="pnlContenido" CssClass="col-lg-12 col-sm-12" runat="server" ScrollBars="Auto">
        <asp:Panel ID="pnlCatalogos" runat="server" CssClass="col-lg-12 col-sm-12" ScrollBars="Auto">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table class="centrado ancho100">
                        <tr>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="Empresa:" CssClass="textoBold"></asp:Label>
                                <asp:DropDownList ID="ddlEmpresas" runat="server" DataSourceID="SqlDataSource1" DataTextField="razon_social" DataValueField="id_empresa" AutoPostBack="true"></asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" 
                                    SelectCommand="select e.id_empresa,e.razon_social from Empresas e where (select COUNT(*) from Empresas_Taller where id_empresa=e.id_empresa)&lt;&gt;(select COUNT(*) from Talleres)"></asp:SqlDataSource>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Debe indicar la Empresa" Text="*" CssClass="errores alineado" ControlToValidate="ddlEmpresas" ValidationGroup="agrega"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="Taller:" CssClass="textoBold"></asp:Label>
                                <asp:DropDownList ID="ddlTalleres" runat="server" DataSourceID="SqlDataSource2" DataTextField="nombre_taller" DataValueField="id_taller"></asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select id_taller,nombre_taller from Talleres where id_taller not in(select et.id_taller from Empresas_Taller et where et.id_empresa=@id_empresa)">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="ddlEmpresas" DefaultValue="0" Name="id_empresa" PropertyName="SelectedValue" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Debe indicar el Taller" Text="*" CssClass="errores alineado" ControlToValidate="ddlTalleres" ValidationGroup="agrega"></asp:RequiredFieldValidator>
                                <asp:Label ID="lblOrden" runat="server" Visible="false"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="Tope Económico:" CssClass="textoBold"></asp:Label>
                                <asp:TextBox ID="txtTopeEco" runat="server" CssClass="input-medium" MaxLength="16"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="txtTopeEco_TextBoxWatermarkExtender" 
                                    runat="server" BehaviorID="txtTopeEco_TextBoxWatermarkExtender" 
                                    TargetControlID="txtTopeEco" WatermarkText=">0.00" WatermarkCssClass="water input-medium" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtTopeEco" runat="server" ValidationExpression="^\d{1,13}(\.\d{1,2})?$" ErrorMessage="El tope económico solo puede contener dígitos y un punto decimal"  ValidationGroup="agrega" Text="*" CssClass="errores" ></asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Debe indicar un tope económico" Text="*" CssClass="errores alineado" ControlToValidate="txtTopeEco" ValidationGroup="agrega"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="Tope Máx. Refacciones:" CssClass="textoBold"></asp:Label>
                                <asp:TextBox ID="txTopetRef" runat="server" CssClass="input-small" MaxLength="5"></asp:TextBox>                                
                                <cc1:TextBoxWatermarkExtender ID="txTopetRef_TextBoxWatermarkExtender" 
                                    runat="server" BehaviorID="txTopetRef_TextBoxWatermarkExtender" 
                                    TargetControlID="txTopetRef" WatermarkText=">0" WatermarkCssClass="water input-small"/>                                
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Debe indicar un tope máximo de refacciones" Text="*" CssClass="errores alineado" ControlToValidate="txTopetRef" ValidationGroup="agrega"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:Label ID="lbltetxtoMaxDias" runat="server" Text="Horas Max. Cotización:" CssClass="textoBold" />
                                <asp:TextBox ID="txtDiasMaxCot" runat="server" CssClass="input-small" placeholder=">=1" />
                                <asp:RequiredFieldValidator ControlToValidate="txtDiasMaxCot" Text="*" CssClass="errores alineado" ID="RequiredFieldValidator5" runat="server" ErrorMessage="Debe indicar un máximo de dias para la cotización" ValidationGroup="agrega" />
                            </td>
                            <td>
                                <asp:LinkButton ID="btnAgregar" runat="server" CssClass="btn btn-info t14" OnClick="btnAgregar_Click" ToolTip="Agregar" ValidationGroup="agrega"><i class="fa fa-plus-circle"></i>&nbsp;<span>Agregar</span></asp:LinkButton>
                        </tr>
                    </table>
                    <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                        DataKeyNames="id_empresa,id_taller" DataSourceID="SqlDataSource3"
                        AllowSorting="True" CssClass="table table-bordered" OnRowDeleting="GridView1_RowDeleting"
                        EmptyDataText="No existen relaciones registradas entre Empresas y Talleres"
                        OnRowCommand="GridView1_RowCommand" 
                        OnRowDataBound="GridView1_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="id_empresa" HeaderText="id_empresa" ReadOnly="True" SortExpression="id_empresa" Visible="false" />
                            <asp:BoundField DataField="id_taller" HeaderText="id_taller" ReadOnly="True" SortExpression="id_taller" Visible="false" />
                            <asp:BoundField DataField="razon_social" HeaderText="Empresa" SortExpression="razon_social" ReadOnly="true"/>
                            <asp:BoundField DataField="nombre_taller" HeaderText="Taller" SortExpression="nombre_taller" ReadOnly="true"/>
                            <asp:BoundField DataField="orden" HeaderText="Ultima Orden" SortExpression="orden" ReadOnly="true" />
                            <asp:TemplateField HeaderText="Tope Económico" SortExpression="tope_economico">
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("tope_economico", "{0:C2}") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtTopeEcoM" runat="server" CssClass="input-medium" MaxLength="16" Text='<%# Eval("tope_economico") %>'></asp:TextBox>
                                    <cc1:TextBoxWatermarkExtender ID="txtTopeEcoM_TextBoxWatermarkExtender" 
                                        runat="server" BehaviorID="txtTopeEcoM_TextBoxWatermarkExtender" 
                                        TargetControlID="txtTopeEcoM" WatermarkText=">0.00" WatermarkCssClass="water input-medium" />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1M" ControlToValidate="txtTopeEcoM" runat="server" ValidationExpression="^\d{1,13}(\.\d{1,2})?$" ErrorMessage="El tope económico solo puede contener dígitos y un punto decimal"  ValidationGroup="edit" Text="*" CssClass="errores" ></asp:RegularExpressionValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3M" runat="server" ErrorMessage="Debe indicar un tope económico" Text="*" CssClass="errores alineado" ControlToValidate="txtTopeEcoM" ValidationGroup="edit"></asp:RequiredFieldValidator>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tope Refacciones" 
                                SortExpression="tope_refacciones">
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("tope_refacciones") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txTopetRefM" runat="server" CssClass="input-small" MaxLength="5" Text='<%# Eval("tope_refacciones") %>'></asp:TextBox>                                
                                    <cc1:TextBoxWatermarkExtender ID="txTopetRefM_TextBoxWatermarkExtender" 
                                        runat="server" BehaviorID="txTopetRefM_TextBoxWatermarkExtender" 
                                        TargetControlID="txTopetRefM" WatermarkText=">0" WatermarkCssClass="water input-small"/>                                
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4M" runat="server" ErrorMessage="Debe indicar un tope máximo de refacciones" Text="*" CssClass="errores alineado" ControlToValidate="txTopetRefM" ValidationGroup="edit"></asp:RequiredFieldValidator>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Horas Max. Cotización" SortExpression="tiempo_max_cot">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtTiempoMaxCot" runat="server" CssClass="input-small" Text='<%# Bind("tiempo_max_cot") %>' />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblTiempoMaxCot" runat="server" Text='<%# Bind("tiempo_max_cot") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEditar" runat="server" CausesValidation="False" CssClass="btn btn-warning t14"
                                        CommandName="Edit" ToolTip="Editar"><i class="fa fa-edit"></i></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton ID="btnActualizar" runat="server" CausesValidation="True" CssClass="btn btn-success t14" ValidationGroup="edit"
                                        CommandName="Update" ToolTip="Actualizar" CommandArgument='<%# Eval("id_empresa")+";"+Eval("id_taller") %>'><i class="fa fa-save"></i></asp:LinkButton>                                    
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEliminar" runat="server" CausesValidation="False" CommandArgument='<%# Eval("id_empresa")+";"+Eval("id_taller") %>' CommandName="Delete" ToolTip="Eliminar" CssClass="btn btn-danger t14" OnClientClick="return confirm('¿Esta seguro de eliminar la relación del taller con la empresa?');"><i class="fa fa-trash"></i></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton ID="btnCancelar" runat="server" CausesValidation="False" CssClass="btn btn-danger t14"
                                        CommandName="Cancel" ToolTip="Cancelar"><i class="fa fa-times"></i></asp:LinkButton>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            
                        </Columns>
                        <EditRowStyle CssClass="alert-warning" />
                        <EmptyDataRowStyle CssClass="errores alert-danger" />
                        <SelectedRowStyle CssClass="alert-success" />
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>"
                        DeleteCommand="delete from Empresas_Taller where id_empresa=@id_empresa and id_taller=@id_taller"
                        SelectCommand="select et.tiempo_max_cot,et.id_empresa,et.id_taller,e.razon_social,t.nombre_taller,et.tope_economico,et.tope_refacciones,et.orden from Empresas_Taller et inner join Empresas e on e.id_empresa=et.id_empresa inner join Talleres t on t.id_taller=et.id_taller"
                        InsertCommand="insert into Empresas_Taller (id_empresa,id_taller,orden, orden_oc, factura,revision,cod_publico,tope_economico,tope_refacciones,tiempo_max_cot,remision,salida_sin_cargo) values(@id_empresa,@id_taller,@orden,0,0,0,0,@economico,@refacciones,@tiempo_max_cot,0,0)">
                        <DeleteParameters>
                            <asp:Parameter Name="id_empresa" />
                            <asp:Parameter Name="id_taller" />
                        </DeleteParameters>
                        <InsertParameters>
                            <asp:ControlParameter ControlID="ddlEmpresas" Name="id_empresa" PropertyName="SelectedValue" Type="Int32" />
                            <asp:ControlParameter ControlID="ddlTalleres" Name="id_taller" PropertyName="SelectedValue" Type="Int32" />
                            <asp:ControlParameter ControlID="lblOrden" Name="orden" PropertyName="Text" Type="Int32" />
                            <asp:ControlParameter ControlID="txtTopeEco" Name="economico" PropertyName="Text" Type="Decimal" />
                            <asp:ControlParameter ControlID="txTopetRef" Name="refacciones" PropertyName="Text" Type="Int32" />
                            <asp:ControlParameter ControlID="txtDiasMaxCot" Name="tiempo_max_cot" PropertyName="Text" Type="Int32" />
                        </InsertParameters>
                    </asp:SqlDataSource>
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
        </asp:Panel>
    </asp:Panel>
</asp:Content>
