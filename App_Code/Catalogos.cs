using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using E_Utilities;
     

/// <summary>
/// Descripción breve de Catalogos
/// </summary>
public class Catalogos
{
    Ejecuciones ejecuta = new Ejecuciones();
    string sql = "";
	public Catalogos()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}

    public object[] tieneRelacionTalleres(int taller)
    {
        string sql = "select sum(tabla.registros) from(select count(*) as registros from empresas_taller where id_taller="+taller+" union all select count(*) as registros from usuarios_taller where id_taller="+taller+" union all select count(*) as registros from ordenes_reparacion where id_taller="+taller+" union all select count(*) as registros from bitacora_orden_avance where id_taller="+taller+" union all select count(*) as registros from bitacora_orden_comentarios where id_taller="+taller+" union all select count(*) as registros from bitacora_orden_localizacion where id_taller="+taller+" union all select count(*) as registros from fotografias_orden where id_taller="+taller+") as tabla";
        return ejecuta.scalarToBool(sql);
    }

    public object[] tieneRelacionValuacion(int valor)
    {
        string sql = "select count(*) as registros from ordenes_reparacion where id_tipo_valuacion=" + valor ;
        return ejecuta.scalarToBool(sql);
    }

    public object[] tieneRelacionLocalizaciones(int valor)
    {
        string sql = "select sum(tabla.registros) from(select count(*) as registros from ordenes_reparacion where id_tipo_valuacion=" + valor + " union all select count(*) from bitacora_orden_localizacion where id_localizacion=" + valor + ") as tabla";
        return ejecuta.scalarToBool(sql);
    }

    public object[] tieneRelacionTipoAsegurado(int valor)
    {
        string sql = "select count(*) as registros from ordenes_reparacion where id_tipo_asegurado=" + valor;
        return ejecuta.scalarToBool(sql);
    }

    public object[] tieneRelacionServicios(int valor)
    {
        string sql = "select count(*) as registros from ordenes_reparacion where id_tipo_servicio=" + valor;
        return ejecuta.scalarToBool(sql);
    }

    public object[] tieneRelacionTipoOrden(int valor)
    {
        string sql = "select count(*) as registros from ordenes_reparacion where id_tipo_orden=" + valor;
        return ejecuta.scalarToBool(sql);
    }

    public object[] tieneRelacionPoliticaPago(int valor)
    {
        string sql = "select count(*) as registros from cliprov where id_politica=" + valor;
        return ejecuta.scalarToBool(sql);
    }

    public object[] tieneRelacionGrupoOp(int valor)
    {
        string sql = "select count(*) as registros from mano_obra where id_grupo_op=" + valor;
        return ejecuta.scalarToBool(sql);
    }

    public object[] tieneRelacionOp(int valor)
    {
        string sql = "select count(*) as registros from mano_obra where id_operacion=" + valor;
        return ejecuta.scalarToBool(sql);
    }

    public object[] tieneRelacionRefacciones(int valor)
    {
        string sql = "select count(*) as registros from mano_obra where id_refaccion=" + valor;
        return ejecuta.scalarToBool(sql);
    }

    public object[] tieneRelacionMarcas(int valor)
    {
        string sql = "select sum(tabla.registros) from(select count(*) as registros from ordenes_reparacion where id_marca=" + valor + " union all select count(*) from tipo_unidad where id_marca=" + valor + ") as tabla";
        return ejecuta.scalarToBool(sql);
    }

    public object[] tieneRelacionTipoVehiculo(int valor)
    {
        string sql = "select sum(tabla.registros) from(select count(*) as registros from ordenes_reparacion where id_tipo_vehiculo=" + valor + " union all select count(*) from tipo_unidad where id_tipo_vehiculo=" + valor + ") as tabla";
        return ejecuta.scalarToBool(sql);
    }

    public object[] tieneRelacionTipoUnidad(int marca, int tVehiculo, int unidad)
    {
        string sql = "select count(*) as registros from ordenes_reparacion where id_marca=" + marca + " and id_tipo_vehiculo=" + tVehiculo + " and id_tipo_unidad=" + unidad;
        return ejecuta.scalarToBool(sql);
    }

    public object[] tieneRelacionTransmision(int valor)
    {
        string sql = "select count(*) as registros from vehiculos where id_tipo_transmision=" + valor;
        return ejecuta.scalarToBool(sql);
    }

    public object[] tieneRelacionTranccion(int valor)
    {
        string sql = "select count(*) as registros from vehiculos where id_tipo_traccion=" + valor;
        return ejecuta.scalarToBool(sql);
    }

    public object[] tieneRelacionGasolina(int valor)
    {
        string sql = "select count(*) as registros from ordenes_reparacion where id_med_gas=" + valor;
        return ejecuta.scalarToBool(sql);
    }

    public object[] tieneRelacionRin(int valor)
    {
        string sql = "select count(*) as registros from vehiculos where id_tipo_rin=" + valor;
        return ejecuta.scalarToBool(sql);
    }

    public object[] tieneRelacionCategoCliente(int valor)
    {
        string sql = "select count(*) as registros from ordenes_reparacion where id_cat_cliente=" + valor;
        return ejecuta.scalarToBool(sql);
    }

    public object[] tieneRelacionPerfilesOrden(int valor)
    {
        string sql = "select sum(tabla.registros) from(select count(*) as registros from ordenes_reparacion where id_perfilOrden=" + valor + " union all select count(*) from bitacora_orden_perfiles where id_perfilOrden=" + valor + ") as tabla";
        return ejecuta.scalarToBool(sql);
    }

    public object[] tieneRelacionEstatus(int id)
    {
        string sql = "select count(*) as registros from refacciones_orden where refEstatusSolicitud=" + id.ToString();
        return ejecuta.scalarToBool(sql);
    }

    public object[] tieneRelacionCalificacion(int id)
    {
        string sql = "select count(*) as registros from operativos_orden where id_calificacion=" + id.ToString();
        return ejecuta.scalarToBool(sql);
    }

    public string obtieneNombreGrupoOperacion(string idGop)
    {
        string sql = "select descripcion_go from Grupo_Operacion where id_grupo_op=" + idGop;
        return ejecuta.scalarToStringSimple(sql);
    }

    public object[] obtieneTopes(int empresa, int taller)
    {
        string sql = "select isnull(tope_economico,0), isnull(tope_refacciones,0) from Empresas_Taller where id_empresa=" + empresa.ToString() + " and id_taller=" + taller.ToString();
        return ejecuta.dataSet(sql);
    }

    public object[] obtieneEmpresa(int empresa) {
        string sql = "select * from empresas where id_empresa=" + empresa.ToString();
        return ejecuta.dataSet(sql);
    }

    public object[] agregaMarca(string nombreMarca)
    {
        string sql = "declare @id int set @id = (select isnull((select top 1 id_marca from marcas order by id_marca desc),0))+1 insert into marcas values (@id,'" + nombreMarca.ToUpper() + "') select @id";
        return ejecuta.scalarToInt(sql);
    }

    public object[] existeMarca(string nombreMarca)
    {
        string sql = "select COUNT(*) from Marcas where ltrim(rtrim(upper(descripcion)))='" + nombreMarca.ToUpper() + "'";
        return ejecuta.scalarToBool(sql);
    }

    public object[] existeTipoVehiculo(string tipoVehiculo)
    {
        string sql = "select COUNT(*) from tipo_vehiculo where ltrim(rtrim(upper(descripcion)))='" + tipoVehiculo.ToUpper() + "'";
        return ejecuta.scalarToBool(sql);
    }

    public object[] agregaTipoVehiculo(string tipoVehiculo)
    {
        string sql = "declare @id int set @id = (select isnull((select top 1 id_tipo_vehiculo from tipo_vehiculo order by id_tipo_vehiculo desc),0))+1 insert into tipo_vehiculo values (@id,'" + tipoVehiculo.ToUpper() + "') select @id";
        return ejecuta.scalarToInt(sql);
    }

    public object[] existeTipoUnidad(string linea)
    {
        string sql = "declare @cont int declare @retorno varchar(200) set @cont = (select count(*) from tipo_unidad where ltrim(rtrim(upper(descripcion)))='" + linea + "') " +
            "if(@cont>0) begin 	set @retorno= (select top 1 'Marca: '+upper(M.descripcion)+' Vehículo: '+upper(TV.descripcion) AS CALSIFICAICON from tipo_unidad tu INNER JOIN Tipo_Vehiculo TV ON TV.id_tipo_vehiculo=TU.id_tipo_vehiculo INNER JOIN Marcas M ON M.id_marca=TU.id_marca where ltrim(rtrim(upper(tu.descripcion)))='" + linea + "') end " +
            "if(@cont=0) begin 	set @retorno=(select Cast(@cont as char(10))) end select @retorno";
        return ejecuta.scalarToString(sql);
    }

    public object[] agregaTipoUnidad(string marca, string tv, string linea)
    {
        string sql = "declare @id int set @id = (select isnull((select top 1 id_tipo_unidad from Tipo_Unidad where id_marca=" + marca + " and id_tipo_vehiculo=" + tv + " order by id_tipo_unidad desc),0))+1 insert into Tipo_Unidad(id_marca,id_tipo_vehiculo,id_tipo_unidad,descripcion) values (" + marca + "," + tv + ",@id,'" + linea.ToUpper() + "') select @id";
        return ejecuta.scalarToInt(sql);
    }

    public object[] existeCliente(string CLIENTE)
    {
        string sql = "select COUNT(*) from cliprov where tipo='C' AND LTRIM(RTRIM(UPPER(razon_social))) ='" + CLIENTE + "'";
        return ejecuta.scalarToBool(sql);
    }

    public object[] agregaCliente(string cliente)
    {
        string sql = "declare @id int set @id = (select isnull((select top 1 id_cliprov from Cliprov where tipo='C' order by id_cliprov desc),0))+1 insert into Cliprov(id_cliprov,tipo,persona,rfc,fecha_nacimiento,razon_social,id_politica) values(@id,'C','M','XXX010101XXX','1900-01-01','" + cliente + "',0) SELECT @id";
        return ejecuta.scalarToInt(sql);
    }
}