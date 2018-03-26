<%@ Page Title="" Language="C#" MasterPageFile="~/Administracion.master" AutoEventWireup="true" CodeFile="CatEmpleados.aspx.cs" Inherits="CatEmpleados" %>

<%@ Register TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="row">
        <div class="col-lg-12 col-sm-12 text-center alert-info">
            <h3>
                <i class="fa fa-users"></i>&nbsp;
                <i class="fa fa-sitemap"></i>&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="Label3" runat="server" Text="Empleados"></asp:Label>
            </h3>
        </div>
    </div>
    <div class="row pad1m">
        <div class="col-lg-12 col-sm-12 text-center">
            <asp:ValidationSummary ID="ValidationSummary3" runat="server" ValidationGroup="agrega" CssClass="errores alert-danger" DisplayMode="List" />
            <asp:ValidationSummary ID="ValidationSummary4" runat="server" ValidationGroup="edita" CssClass="errores alert-danger" DisplayMode="List" />
            <asp:Label ID="Label4" runat="server" CssClass="errores alert-danger"></asp:Label>
        </div>
    </div>
    <asp:Panel ID="pnlCatalogos" runat="server" CssClass="panelCatalogos" ScrollBars="Auto">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <Triggers>
                <asp:PostBackTrigger ControlID="GridEmpleados" />
            </Triggers>
            <ContentTemplate>
                <div class="row">
                    <div class="col-lg-12 col-sm-12">
                        <div class="col-lg-1 col-sm-1 text-left"><asp:Label ID="Label1" runat="server" Text="Puesto:" /></div>
                        <div class="col-lg-3 col-sm-3 text-left">
                            <asp:DropDownList ID="ddlManoObra" runat="server" DataSourceID="SqlDataSource1" DataTextField="descripcion" DataValueField="id_puesto" CssClass="input-medium"></asp:DropDownList>
                            <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:PVW %>' SelectCommand="SELECT [id_puesto], [descripcion] FROM [Puestos]"></asp:SqlDataSource>
                        </div>
                        <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label7" runat="server" Text="Tipo Empleado:" /></div>
                        <div class="col-lg-2 col-sm-2 text-left">
                            <asp:DropDownList ID="ddlTipo" runat="server" CssClass="input-medium">
                                <asp:ListItem Selected="True" Value="IN" Text="Interno"></asp:ListItem>
                                <asp:ListItem Value="EX" Text="Externo"></asp:ListItem>
                                <asp:ListItem Value="AD" Text="Administrativo"></asp:ListItem>
                            </asp:DropDownList>                            
                        </div>
                        <div class="col-lg- col-sm-4 text-center">
                            <asp:LinkButton ID="btnAgregar" runat="server" CssClass="btn btn-info t14" OnClick="btnAgregar_Click" ToolTip="Agregar" ValidationGroup="agrega"><i class="fa fa-plus-circle"></i>&nbsp;<span>Agregar</span></asp:LinkButton>
                        </div>
                    </div>
                </div>
                <div class="row  marTop"> 
                    <div class="col-lg-12 col-sm-12">
                        <div class="col-lg-1 col-sm-1 text-right"><asp:Label ID="Label8" runat="server" Text="Nombre:" /></div>
                        <div class="col-lg-2 col-sm-2">
                            <asp:TextBox ID="txtNombre" MaxLength="30" runat="server" CssClass="input-medium" placeholder="Nombre(s)" />
                            <asp:RequiredFieldValidator ControlToValidate="txtNombre" ID="RequiredFieldValidator1" runat="server" ErrorMessage="Necesita colocar el/los nombre(s)" ValidationGroup="agrega" Text="*" CssClass="errores alert-danger" />
                        </div>
                        <div class="col-lg-2 col-sm-2">
                            <asp:TextBox ID="txtAPaterno" MaxLength="30" runat="server" CssClass="input-medium" placeholder="A. Paterno" />
                            <asp:RequiredFieldValidator ControlToValidate="txtAPaterno" ID="RequiredFieldValidator2" runat="server" ErrorMessage="Necesita colocar el apellido paterno" ValidationGroup="agrega" Text="*" CssClass="errores alert-danger" />
                        </div>
                        <div class="col-lg-2 col-sm-2">
                            <asp:TextBox ID="txtAMaterno" MaxLength="30" runat="server" CssClass="input-medium" placeholder="A. Materno" />
                        </div>
                        <div class="col-lg-1 col-sm-1 text-right"><asp:Label ID="Label9" runat="server" Text="Cajones:" /></div>
                        <div class="col-lg-2 col-sm-2">
                            <asp:TextBox ID="txtPichonera" MaxLength="2" runat="server" CssClass="input-small" placeholder="Cajones" />
                            <ajaxToolkit:FilteredTextBoxExtender runat="server" BehaviorID="txtPichonera_FilteredTextBoxExtender" TargetControlID="txtPichonera" ID="txtPichonera_FilteredTextBoxExtender" FilterType="Numbers"></ajaxToolkit:FilteredTextBoxExtender>
                            <asp:RequiredFieldValidator ControlToValidate="txtPichonera" ID="RequiredFieldValidator5" runat="server" ErrorMessage="Necesita colocar un numero de cajones" ValidationGroup="agrega" Text="*" CssClass="errores alert-danger" />
                        </div>                        
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-12 col-sm-12">
                        <asp:Label ID="lblError" runat="server" CssClass="alert-danger errores" />
                    </div>
                    <div class="col-lg-12 col-sm-12">
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="alert-danger errores" DisplayMode="List" ValidationGroup="agrega" />
                    </div>
                    <div class="col-lg-12 col-sm-12">
                        <asp:ValidationSummary ID="ValidationSummary2" runat="server" CssClass="alert-danger errores" DisplayMode="List" ValidationGroup="edita" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-12 col-sm-12">
                        <asp:GridView ID="GridEmpleados" runat="server" AutoGenerateColumns="False" 
                            DataKeyNames="idEmp" DataSourceID="SqlDataSource2" 
                            CssClass="table table-bordered" AllowSorting="true" 
                            OnRowEditing="GridEmpleados_RowEditing" OnRowCancelingEdit="GridEmpleados_RowCancelingEdit"
                            OnRowCommand="GridEmpleados_RowCommand" AllowPaging="true" PageSize="10" 
                            onpageindexchanging="GridEmpleados_PageIndexChanging" 
                            onrowdatabound="GridEmpleados_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="No." SortExpression="idEmp">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Bind("idEmp") %>' ID="Label2"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Puesto" SortExpression="Puesto" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Bind("Puesto") %>' ID="lblPuesto"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Nombre(s)" SortExpression="nombres">
                                    <EditItemTemplate>
                                        <asp:TextBox runat="server" MaxLength="30" Text='<%# Eval("nombres") %>' ID="txtNombreEdit" placeholder="Nombre(s)" />
                                        <asp:RequiredFieldValidator ControlToValidate="txtNombreEdit" ID="RequiredFieldValidator3" runat="server" ErrorMessage="Necesita colocar el nombre" ValidationGroup="edita" Text="*" CssClass="errores alert-danger" />
                                        <asp:TextBox runat="server" MaxLength="30" Text='<%# Eval("paterno") %>' ID="txtPaternoEdit" placeholder="A. Paterno" CssClass="input-medium" />
                                        <asp:RequiredFieldValidator ControlToValidate="txtPaternoEdit" ID="RequiredFieldValidator4" runat="server" ErrorMessage="Necesita colocar el apellido paterno" ValidationGroup="edita" Text="*" CssClass="errores alert-danger" />
                                        <asp:TextBox runat="server" MaxLength="30" Text='<%# Eval("materno") %>' ID="txtMaternoEdit" placeholder="A. Materno" CssClass="input-medium" />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Bind("nombreCompleto") %>' ID="Label1"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>                                
                                <asp:TemplateField HeaderText="Puesto" SortExpression="puesto">
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlPuestoEdit" runat="server" DataSourceID="SqlDataSource1" DataTextField="descripcion" DataValueField="id_puesto">
                                            <asp:ListItem Text="No asignado" Value=""></asp:ListItem>
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Bind("puesto") %>' ID="Label5"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tipo" SortExpression="tipo">
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlTipoEdit" runat="server" CssClass="input-medium">
                                            <asp:ListItem Value="" Text=""></asp:ListItem>
                                            <asp:ListItem Value="IN" Text="Interno"></asp:ListItem>
                                            <asp:ListItem Value="EX" Text="Externo"></asp:ListItem>
                                            <asp:ListItem Value="AD" Text="Administrativo"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ControlToValidate="ddlTipoEdit" ID="RequiredFieldValidator7" runat="server" ErrorMessage="Necesita indicar el tipo de empleado" ValidationGroup="edita" Text="*" CssClass="errores alert-danger" />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Bind("tipo") %>' ID="Label7"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cajones" SortExpression="clv_pichonera">
                                    <EditItemTemplate>
                                        <asp:TextBox runat="server" MaxLength="2" Text='<%# Bind("clv_pichonera") %>' ID="txtPichoneraEdit" placeholde="Cajones" CssClass="input-small" />
                                        <ajaxToolkit:FilteredTextBoxExtender runat="server" BehaviorID="txtPichoneraEdit_FilteredTextBoxExtender" TargetControlID="txtPichoneraEdit" ID="txtPichoneraEdit_FilteredTextBoxExtender" FilterType="Numbers"></ajaxToolkit:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ControlToValidate="txtPichoneraEdit" ID="RequiredFieldValidator6" runat="server" ErrorMessage="Necesita colocar un numero de cajones" ValidationGroup="edita" Text="*" CssClass="errores alert-danger" />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Bind("clv_pichonera") %>' ID="Label6"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False">
                                 <EditItemTemplate>
                                     <asp:LinkButton ID="btnActualizar" runat="server" CausesValidation="True" CommandArgument='<%# Bind("idEmp") %>' CommandName="Updates" ToolTip="Guardar" ValidationGroup="edita" CssClass="btn btn-success t14"><i class="fa fa-save"></i></asp:LinkButton>                                     
                                 </EditItemTemplate>
                                 <ItemTemplate>
                                     <asp:LinkButton ID="btnEditar" runat="server" CausesValidation="False" CommandName="Edit" ToolTip="Editar" CssClass="btn btn-warning t14"><i class="fa fa-edit"></i></asp:LinkButton>                                     
                                 </ItemTemplate>
                             </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False">
                                 <EditItemTemplate>
                                     <asp:LinkButton ID="btnCancelar" runat="server" CausesValidation="False" CommandName="Cancel" ToolTip="Cancelar" CssClass="btn btn-danger t14"><i class="fa fa-times-circle"></i></asp:LinkButton>
                                 </EditItemTemplate>
                                 <ItemTemplate>
                                     <asp:LinkButton ID="btnEliminar" runat="server" CausesValidation="False" CommandName="Delete" CommandArgument='<%# Bind("idEmp") %>' ToolTip="Eliminar" CssClass="btn btn-danger t14" OnClientClick="return confirm('¿Está seguro de eliminar el empleado?')"><i class="fa fa-trash"></i></asp:LinkButton>
                                 </ItemTemplate>
                             </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource runat="server" ID="SqlDataSource2" ConnectionString='<%$ ConnectionStrings:PVW %>' 
                            SelectCommand="select e.idEmp,ltrim(rtrim(e.nombres))+' '+ltrim(rtrim(e.apellido_paterno))+' '+ltrim(rtrim(e.apellido_materno)) as nombreCompleto,ltrim(rtrim(e.nombres))as nombres,ltrim(rtrim(e.apellido_paterno))as paterno,ltrim(rtrim(e.apellido_materno))as materno,isnull(p.descripcion,'No asignado')as puesto,e.clv_pichonera,e.Puesto,e.tipo_empleado, case e.tipo_empleado when 'IN' then 'Interno' when 'EX' then 'Externo' when 'AD' then 'Administrativo' else '' end as tipo from empleados e left join Puestos p on p.id_puesto=e.Puesto where e.status_empleado!='B'" 
                            DeleteCommand=" declare @relacion int set @relacion = (select count(*) from operativos_orden where idemp=@idemp)  if(@relacion=0) begin  delete from empleados where idemp=@idemp end">
                            <DeleteParameters>
                                <asp:Parameter Name="idemp" DbType="Int32" DefaultValue="0" />
                            </DeleteParameters>
                        </asp:SqlDataSource>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>
