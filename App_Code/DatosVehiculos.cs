using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de Vehiculos
/// </summary>
public class DatosVehiculos
{
    Ejecuciones ejecuta = new Ejecuciones();
	public DatosVehiculos()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}

    public object[] obtieneInfoVehiculo(int orden, int empresa, int taller)
    {
        string sql = "select * from Vehiculos where placas in (select placas from Ordenes_Reparacion where id_empresa=" + empresa.ToString() + " and id_taller=" + taller.ToString() + " and no_orden=" + orden.ToString() + ")";
        return ejecuta.dataSet(sql);
    }

    public object[] actualizaVehiculo(string placas, string[] datos, string id_vehiculo, bool cambio)
    {
        object[] retorno = new object[2] { false, "" };
        for (int i = 0; i < datos.Length; i++) {
            if (datos[i] == "-1")
            {
                if (i == 26 || i == 30)
                    datos[i] = "null";
                else
                    datos[i] = "null";
            }
            else {
                if (i == 26 || i == 30)
                    datos[i] = "'" + datos[i] + "'";
                else
                    datos[i] = datos[i];
            }
        }
        if (cambio)
        {
            string query = "select count(*) from vehiculos where id_marca=" + datos[0] + " and id_tipo_vehiculo=" + datos[1] + " and id_tipo_unidad=" + datos[2] + " and id_vehiculo=" + id_vehiculo;
            object[] existe = ejecuta.scalarToBool(query);
            if (Convert.ToBoolean(existe[0]))
            {
                bool previo = Convert.ToBoolean(existe[1]);
                if (previo)
                {
                    string q = "select isnull((select top 1 id_vehiculo from vehiculos where id_marca=" + datos[0] + " and id_tipo_vehiculo=" + datos[1] + " and id_tipo_unidad=" + datos[2] + " order by id_vehiculo desc),0)+1";
                    id_vehiculo = ejecuta.scalarToStringSimple(q);
                    try
                    {
                        if (Convert.ToInt32(id_vehiculo) > 0)
                        {
                            string sql = "update Vehiculos set id_marca=" + datos[0] + " ,id_tipo_vehiculo=" + datos[1] + " ,id_tipo_unidad=" + datos[2] + ",id_vehiculo=" + id_vehiculo + " ,modelo=" + datos[4] + " ,placas='" + datos[5].ToUpper() + "' ,serie_vin='" + datos[6] + "' ,motor='" + datos[7] + "' ,color_int='" + datos[8] + "' ,color_ext='" + datos[9] + "' ,id_tipo_transmision=" + datos[10] + " ,id_tipo_traccion=" + datos[11] + " ,cilindros=" + datos[12] + " ,no_economico='" + datos[15] + "' ,puertas=" + datos[14] + " ,version='" + datos[13] + "' ,id_tipo_rin=" + datos[16] + " ,medida_llanta='" + datos[17] + "' ,quemacocos=" + datos[18] + " ,bolsas_aire=" + datos[19] + " ,aire_acondicionado=" + datos[20] + " ,direccion_hidraulica=" + datos[21] + " ,elevadores_manuales=" + datos[22] + " ,espejos_lat_man=" + datos[23] + " ,color_espejos_negro=" + datos[24] + " ,molduras=" + datos[25] + " ,cantoneras_negras=" + datos[27] + " ,vertiduras=" + datos[26] + " ,faros_niebla=" + datos[28] + " ,facia_defensa_corrugada=" + datos[29] + " ,cabina=" + datos[30] + " ,defensa_cromada=" + datos[31] + " where placas='" + placas.ToUpper() + "' "
                                        + "update ordenes_reparacion set id_marca=" + datos[0] + " ,id_tipo_vehiculo=" + datos[1] + " ,id_tipo_unidad=" + datos[2] + ", id_vehiculo=" + id_vehiculo + " where placas ='" + placas.ToUpper()+"'";
                            retorno = ejecuta.insertUpdateDelete(sql);
                        }

                    }
                    catch (Exception ex)
                    {
                        retorno = new object[2] { false, "No se puedo actualizar los datos del vehiculo. " + ex.Message };
                    }
                }
                else
                {
                    string sql = "update Vehiculos set id_marca=" + datos[0] + " ,id_tipo_vehiculo=" + datos[1] + " ,id_tipo_unidad=" + datos[2] + ",id_vehiculo=" + id_vehiculo + " ,modelo=" + datos[4] + " ,placas='" + datos[5].ToUpper() + "' ,serie_vin='" + datos[6] + "' ,motor='" + datos[7] + "' ,color_int='" + datos[8] + "' ,color_ext='" + datos[9] + "' ,id_tipo_transmision=" + datos[10] + " ,id_tipo_traccion=" + datos[11] + " ,cilindros=" + datos[12] + " ,no_economico='" + datos[15] + "' ,puertas=" + datos[14] + " ,version='" + datos[13] + "' ,id_tipo_rin=" + datos[16] + " ,medida_llanta='" + datos[17] + "' ,quemacocos=" + datos[18] + " ,bolsas_aire=" + datos[19] + " ,aire_acondicionado=" + datos[20] + " ,direccion_hidraulica=" + datos[21] + " ,elevadores_manuales=" + datos[22] + " ,espejos_lat_man=" + datos[23] + " ,color_espejos_negro=" + datos[24] + " ,molduras=" + datos[25] + " ,cantoneras_negras=" + datos[27] + " ,vertiduras=" + datos[26] + " ,faros_niebla=" + datos[28] + " ,facia_defensa_corrugada=" + datos[29] + " ,cabina=" + datos[30] + " ,defensa_cromada=" + datos[31] + " where placas='" + placas.ToUpper() + "' "
                                       + "update ordenes_reparacion set id_marca=" + datos[0] + " ,id_tipo_vehiculo=" + datos[1] + " ,id_tipo_unidad=" + datos[2] + ", id_vehiculo=" + id_vehiculo + " where placas ='" + placas.ToUpper() + "'";
                    retorno = ejecuta.insertUpdateDelete(sql);
                }
                
            }
            else
                retorno = existe;
        }
        else {
            string sql = "update Vehiculos set id_marca=" + datos[0] + " ,id_tipo_vehiculo=" + datos[1] + " ,id_tipo_unidad=" + datos[2] + ",id_vehiculo=" + id_vehiculo + " ,modelo=" + datos[4] + " ,placas='" + datos[5].ToUpper() + "' ,serie_vin='" + datos[6] + "' ,motor='" + datos[7] + "' ,color_int='" + datos[8] + "' ,color_ext='" + datos[9] + "' ,id_tipo_transmision=" + datos[10] + " ,id_tipo_traccion=" + datos[11] + " ,cilindros=" + datos[12] + " ,no_economico='" + datos[15] + "' ,puertas=" + datos[14] + " ,version='" + datos[13] + "' ,id_tipo_rin=" + datos[16] + " ,medida_llanta='" + datos[17] + "' ,quemacocos=" + datos[18] + " ,bolsas_aire=" + datos[19] + " ,aire_acondicionado=" + datos[20] + " ,direccion_hidraulica=" + datos[21] + " ,elevadores_manuales=" + datos[22] + " ,espejos_lat_man=" + datos[23] + " ,color_espejos_negro=" + datos[24] + " ,molduras=" + datos[25] + " ,cantoneras_negras=" + datos[27] + " ,vertiduras=" + datos[26] + " ,faros_niebla=" + datos[28] + " ,facia_defensa_corrugada=" + datos[29] + " ,cabina=" + datos[30] + " ,defensa_cromada=" + datos[31] + " where placas='" + placas.ToUpper() + "' "
                        + "update ordenes_reparacion set id_marca=" + datos[0] + " ,id_tipo_vehiculo=" + datos[1] + " ,id_tipo_unidad=" + datos[2] + ", id_vehiculo=" + id_vehiculo + " where placas ='" + placas.ToUpper() + "'";
            retorno = ejecuta.insertUpdateDelete(sql);
        }
        return retorno;        
    }

    public object[] obtieneDatosBasicosVehiculo(int orden, int empresa, int taller)
    {
        string sql = "select orp.no_orden,tv.descripcion+' '+m.descripcion+' '+tu.descripcion+' '+cast(v.modelo as char(4))+' '+rtrim(ltrim(v.color_ext))+' '+rtrim(ltrim(orp.placas)) as descripcion"
                    + " from Ordenes_Reparacion orp"                    
                    + " left join Vehiculos v on v.id_marca=orp.id_marca and v.id_tipo_vehiculo=orp.id_tipo_vehiculo and v.id_tipo_unidad=orp.id_tipo_unidad and v.id_vehiculo=orp.id_vehiculo"
                    + " left join Marcas m on m.id_marca=orp.id_marca"
                    + " left join Tipo_Vehiculo tv on tv.id_tipo_vehiculo=orp.id_tipo_vehiculo"
                    + " left join Tipo_Unidad tu on tu.id_marca=orp.id_marca and tu.id_tipo_vehiculo=orp.id_tipo_vehiculo and tu.id_tipo_unidad=orp.id_tipo_unidad"
                    + " where orp.id_empresa=" + empresa.ToString() + " and orp.id_taller=" + taller.ToString() + " and orp.no_orden=" + orden.ToString();
        return ejecuta.dataSet(sql);
    }

    public object[] obtieneDatosVehiculoCC(int orden, int empresa, int taller)
    {
        string sql = "SELECT  Ordenes_Reparacion.no_orden, Ordenes_Reparacion.placas, Cliprov.razon_social," +
                    "(SELECT descripcion FROM Marcas WHERE(id_marca = Ordenes_Reparacion.id_marca)) + ' ' + (SELECT descripcion FROM Tipo_Unidad WHERE(id_marca = Ordenes_Reparacion.id_marca) AND(id_tipo_vehiculo = Ordenes_Reparacion.id_tipo_vehiculo) AND(id_tipo_unidad = Ordenes_Reparacion.id_tipo_unidad))  +' ' + CAST(Vehiculos.modelo AS char(4)) AS vehiculo, Tipo_Valuacion.descripcion " +
                    "FROM Ordenes_Reparacion INNER JOIN Cliprov ON Ordenes_Reparacion.id_cliprov = Cliprov.id_cliprov AND Cliprov.tipo = 'c' INNER JOIN Vehiculos ON Ordenes_Reparacion.id_marca = Vehiculos.id_marca AND Ordenes_Reparacion.id_tipo_vehiculo = Vehiculos.id_tipo_vehiculo AND Ordenes_Reparacion.id_tipo_unidad = Vehiculos.id_tipo_unidad AND Ordenes_Reparacion.id_vehiculo = Vehiculos.id_vehiculo " +
                    "INNER JOIN Tipo_Valuacion ON Ordenes_Reparacion.id_tipo_valuacion = Tipo_Valuacion.id_tipo_valuacion WHERE(Ordenes_Reparacion.id_empresa = " + empresa.ToString() + ") AND(Ordenes_Reparacion.id_taller = " + taller.ToString() + ") AND(Ordenes_Reparacion.no_orden = " + orden.ToString() + ")";
        return ejecuta.dataSet(sql);
    }

    public object[] obtieneDatosBasicosVehiculoNotaFactura(int orden, int empresa, int taller)
    {
        string sql = "select orp.no_orden,m.descripcion as marca,tu.descripcion as unidad,cast(v.modelo as char(4)) as modelo,orp.placas,ltrim(rtrim(isnull(orp.nombre_propietario,'')))+' '+ltrim(rtrim(isnull(orp.ap_paterno_propietario,'')))+' '+ltrim(rtrim(isnull(orp.ap_materno_propietario,''))) as propietario,isnull(v.serie_vin,'') as serie_vin "
                    + " from Ordenes_Reparacion orp"
                    + " left join Vehiculos v on v.id_marca=orp.id_marca and v.id_tipo_vehiculo=orp.id_tipo_vehiculo and v.id_tipo_unidad=orp.id_tipo_unidad and v.id_vehiculo=orp.id_vehiculo"
                    + " left join Marcas m on m.id_marca=orp.id_marca"
                    + " left join Tipo_Vehiculo tv on tv.id_tipo_vehiculo=orp.id_tipo_vehiculo"
                    + " left join Tipo_Unidad tu on tu.id_marca=orp.id_marca and tu.id_tipo_vehiculo=orp.id_tipo_vehiculo and tu.id_tipo_unidad=orp.id_tipo_unidad"
                    + " where orp.id_empresa=" + empresa.ToString() + " and orp.id_taller=" + taller.ToString() + " and orp.no_orden=" + orden.ToString();
        return ejecuta.dataSet(sql);
    }

    public object[] obtieneDatosBasicosVehiculoCot(int orden, int empresa, int taller, int cotizacion, int proveedor)
    {
        string sql = "select orp.no_orden,m.descripcion+' '+tu.descripcion+' '+cast(v.modelo as char(4))+' '+cast(v.version as char(10)) as descripcion,(select razon_social from Cliprov where id_cliprov=" + proveedor.ToString() + " and tipo='P') AS proveedor,c.folio,v.serie_vin"
                    + " from Ordenes_Reparacion orp"
                    + " left join Vehiculos v on v.id_marca=orp.id_marca and v.id_tipo_vehiculo=orp.id_tipo_vehiculo and v.id_tipo_unidad=orp.id_tipo_unidad and v.id_vehiculo=orp.id_vehiculo"
                    + " left join Marcas m on m.id_marca=orp.id_marca"
                    + " left join Tipo_Unidad tu on tu.id_marca=orp.id_marca and tu.id_tipo_vehiculo=orp.id_tipo_vehiculo and tu.id_tipo_unidad=orp.id_tipo_unidad"
                    + " left join Cotizacion_Encabezado c on c.id_empresa=orp.id_empresa and c.id_taller= orp.id_taller and c.no_orden=orp.no_orden and c.id_cotizacion=" + cotizacion.ToString()
                    + " where orp.id_empresa=" + empresa.ToString() + " and orp.id_taller=" + taller.ToString() + " and orp.no_orden=" + orden.ToString();
        return ejecuta.dataSet(sql);
    }

    public object[] obtieneDatosBasicosVehiculoCorreo(int orden, int empresa, int taller)
    {
        string sql = "select orp.no_orden,m.descripcion as marca,tu.descripcion as tipo,v.modelo,orp.placas,t.identificador"
                    + " from Ordenes_Reparacion orp"
                    + " left join Vehiculos v on v.id_marca=orp.id_marca and v.id_tipo_vehiculo=orp.id_tipo_vehiculo and v.id_tipo_unidad=orp.id_tipo_unidad and v.id_vehiculo=orp.id_vehiculo"
                    + " left join Marcas m on m.id_marca=orp.id_marca"
                    + " left join Tipo_Vehiculo tv on tv.id_tipo_vehiculo=orp.id_tipo_vehiculo"
                    + " left join Tipo_Unidad tu on tu.id_marca=orp.id_marca and tu.id_tipo_vehiculo=orp.id_tipo_vehiculo and tu.id_tipo_unidad=orp.id_tipo_unidad"
                    + " left join Talleres t on t.id_taller=orp.id_taller"
                    + " where orp.id_empresa=" + empresa.ToString() + " and orp.id_taller=" + taller.ToString() + " and orp.no_orden=" + orden.ToString();
        return ejecuta.dataSet(sql);
    }

    public object[] obtieneDatosBasicosVehiculoOrd(int orden, int empresa, int taller, int compra, int proveedor)
    {
        string sql = "select orp.no_orden,m.descripcion+' '+tu.descripcion+' '+cast(v.modelo as char(4))+' '+cast(v.version as char(10)) as descripcion,(select razon_social from Cliprov where id_cliprov=" + proveedor.ToString() + " and tipo='P') AS proveedor,c.folio_orden"
                    + " from Ordenes_Reparacion orp"
                    + " left join Vehiculos v on v.id_marca=orp.id_marca and v.id_tipo_vehiculo=orp.id_tipo_vehiculo and v.id_tipo_unidad=orp.id_tipo_unidad and v.id_vehiculo=orp.id_vehiculo"
                    + " left join Marcas m on m.id_marca=orp.id_marca"
                    + " left join Tipo_Unidad tu on tu.id_marca=orp.id_marca and tu.id_tipo_vehiculo=orp.id_tipo_vehiculo and tu.id_tipo_unidad=orp.id_tipo_unidad"
                    + " left join Orden_Compra_Encabezado c on c.id_empresa=orp.id_empresa and c.id_taller= orp.id_taller and c.no_orden=orp.no_orden and c.id_orden=" + compra.ToString()
                    + " where orp.id_empresa=" + empresa.ToString() + " and orp.id_taller=" + taller.ToString() + " and orp.no_orden=" + orden.ToString();
        return ejecuta.dataSet(sql);
    }
}