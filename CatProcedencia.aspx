<%@ Page Title="" Language="C#" MasterPageFile="~/Administracion.master" AutoEventWireup="true" CodeFile="CatProcedencia.aspx.cs" Inherits="CatProcedencia" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    
    <div class="row">
        <div class="col-lg-12 col-sm-12 text-center alert-info">
            <h3>
                <i class="fa fa-globe"></i>&nbsp;&nbsp;&nbsp;
                <asp:Label ID="lblTit" runat="server" Text="Procedencia de Refacciones"></asp:Label>
            </h3>
        </div>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row pad1m">
                <div class="col-lg-12 col-sm-12 text-center">
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="agrega" CssClass="errores alert-danger" DisplayMode="List" />
                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="edita" CssClass="errores alert-danger" DisplayMode="List" />
                    <asp:Label ID="lblError" runat="server" CssClass="errores alert-danger" />
                </div>
            </div>
            <table class="centrado">
                <tr class="alineado">
                    <td>
                        <asp:TextBox ID="txtProcDescrip" runat="server" MaxLength="50" CssClass="ancho150px" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Debe indicar la Descripción" CssClass="errores alineado" ValidationGroup="agrega" ControlToValidate="txtProcDescrip" Text="*" />
                        <cc1:TextBoxWatermarkExtender runat="server" ID="txtProcDescrip_TextBoxWatermarkExtender" BehaviorID="txtDescripcionNew_TextBoxWatermarkExtender"
                            TargetControlID="txtProcDescrip" WatermarkCssClass="ancho150px water" WatermarkText="Descripción Procedencia" />
                    </td>
                    <td>
                        <asp:LinkButton ID="btnAgregar" runat="server" CssClass="btn btn-info t14" ToolTip="Agregar" ValidationGroup="agrega" OnClick="btnAgregar_Click"><i class="fa fa-plus-circle"></i>&nbsp;<span>Agregar</span></asp:LinkButton>
                    </td>
                </tr>
            </table>
            <asp:GridView ID="grdProc" runat="server" DataSourceID="SqlDsCatProcedencia" AutoGenerateColumns="False" DataKeyNames="id_Proc" CssClass="table table-bordered"
                EmptyDataText="No hay procedencias capturadas." AllowPaging="True" PageSize="10" OnRowDeleting="grdProc_RowDeleting" OnRowCommand="grdProc_RowCommand">
                <Columns>
                    <asp:BoundField DataField="id_Proc" HeaderText="No." ReadOnly="True" InsertVisible="False" SortExpression="id_Proc"></asp:BoundField>
                    <asp:BoundField DataField="proc_Descrip" HeaderText="Procedencia" SortExpression="proc_Descrip"></asp:BoundField>
                    <asp:TemplateField ShowHeader="False">
                        <EditItemTemplate>
                            <asp:LinkButton ID="btnActualizar" runat="server" CausesValidation="True" CommandName="Update" ToolTip="Guardar" ValidationGroup="edita" CssClass="btn btn-success t14"><i class="fa fa-save"></i></asp:LinkButton>
                            &nbsp;<asp:LinkButton ID="btnCancelar" runat="server" CausesValidation="False" CommandName="Cancel" ToolTip="Cancelar" CssClass="btn btn-danger t14"><i class="fa fa-times-circle"></i></asp:LinkButton>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEditar" runat="server" CausesValidation="False" CommandName="Edit" ToolTip="Editar" CssClass="btn btn-warning t14"><i class="fa fa-edit"></i></asp:LinkButton>
                            &nbsp;<asp:LinkButton ID="btnEliminar" runat="server" CausesValidation="False" CommandName="Delete" ToolTip="Eliminar" CssClass="btn btn-danger t14" OnClientClick="return confirm('¿Esta seguro de eliminar la procedenica?');"><i class="fa fa-trash"></i></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <asp:SqlDataSource runat="server" ID="SqlDsCatProcedencia" ConnectionString='<%$ ConnectionStrings:PVW %>' 
                DeleteCommand="DELETE FROM [cat_Procedencia] WHERE [id_Proc] = @id_Proc" 
                InsertCommand="INSERT INTO [cat_Procedencia] ([proc_Descrip]) VALUES (@proc_Descrip)" 
                SelectCommand="SELECT [id_Proc], [proc_Descrip] FROM [cat_Procedencia]" 
                UpdateCommand="UPDATE [cat_Procedencia] SET [proc_Descrip] = @proc_Descrip WHERE [id_Proc] = @id_Proc">
                <DeleteParameters>
                    <asp:Parameter Name="id_Proc" Type="Int32"></asp:Parameter>
                </DeleteParameters>
                <InsertParameters>
                    <asp:Parameter Name="proc_Descrip" Type="String"></asp:Parameter>
                </InsertParameters>
                <UpdateParameters>
                    <asp:Parameter Name="proc_Descrip" Type="String"></asp:Parameter>
                    <asp:Parameter Name="id_Proc" Type="Int32"></asp:Parameter>
                </UpdateParameters>
            </asp:SqlDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

