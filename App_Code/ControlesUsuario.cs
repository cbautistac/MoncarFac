using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de ControlesUsuario
/// </summary>
public class ControlesUsuario
{
    string sql = "";
    Ejecuciones ejecuta = new Ejecuciones();
    object[] guardado = { false, false };
    object[] existe = { false, false };
    bool retorna = false;
    public ControlesUsuario()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public bool guardaInventarioCajuela(int idEmpresa, int idTaller, int noOrden, bool cables, bool refaccion, bool gato, bool herramientas, bool llaveRueda, bool señalCarretera, bool tapete, bool tapaCarton, bool extintor)
    {
        try
        {
            sql = "select count(*) from Inventario_Vehiculo where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString();
            existe = ejecuta.scalarToBool(sql);
            if (Convert.ToBoolean(existe[1]))
            {
                sql = "update Inventario_Vehiculo set cables_corriente_caj=" + pasarCeroUno(cables).ToString() + ",llantas_refaccion_caj=" + pasarCeroUno(refaccion).ToString() + ",gato_caj=" + pasarCeroUno(gato).ToString() + ",herramientas_caj=" + pasarCeroUno(herramientas).ToString() + ",llave_rueda_caj=" + pasarCeroUno(llaveRueda).ToString() + ",señales_carretera_caj=" + pasarCeroUno(señalCarretera).ToString() + ",tapetes_caj=" + pasarCeroUno(tapete).ToString() + ",tapa_carton_caj=" + pasarCeroUno(tapaCarton).ToString() + ",extinguidor_caj=" + pasarCeroUno(extintor).ToString() + ",caj=1 " +
                      "where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString();
            }
            else
            {
                sql = "insert into Inventario_Vehiculo (no_orden,id_empresa,id_taller,cables_corriente_caj,llantas_refaccion_caj,gato_caj,herramientas_caj,llave_rueda_caj,señales_carretera_caj,tapetes_caj,tapa_carton_caj,extinguidor_caj,caj)" +
                      "values(" + noOrden.ToString() + ", " + idEmpresa.ToString() + ", " + idTaller.ToString() + ", " + pasarCeroUno(cables).ToString() + "," + pasarCeroUno(refaccion).ToString() + "," + pasarCeroUno(gato).ToString() + "," + pasarCeroUno(herramientas).ToString() + "," + pasarCeroUno(llaveRueda).ToString() + "," + pasarCeroUno(señalCarretera).ToString() + "," + pasarCeroUno(tapete).ToString() + "," + pasarCeroUno(tapaCarton).ToString() + "," + pasarCeroUno(extintor).ToString() + ",1)";
            }
            guardado = ejecuta.insertUpdateDelete(sql);
            if (Convert.ToBoolean(guardado[0]))
            {
                if (Convert.ToBoolean(guardado[1])) { retorna = true; }
                else
                    retorna = false;
            }
            else
                retorna = false;
        }
        catch (Exception x)
        {
            retorna = false;
        }
        return retorna;
    }

    public bool guardaInventarioCajuelaDaños(int idEmpresa, int idTaller, int noOrden, string cablesD, string refaccionD, string gatoD, string herramientasD, string llaveRuedaD, string señalCarreteraD, string tapeteD, string tapaCartonD, string extintorD)
    {
        try
        {
            string[] campos = { cablesD, refaccionD, gatoD, herramientasD, llaveRuedaD, señalCarreteraD, tapeteD, tapaCartonD, extintorD };
            int caracteristica = 83;
            for (int contador = 0; contador < campos.Length; contador++)
            {
                sql = "select count(*) from Observaciones_Inventario_Vehiculo where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString() + " and caracteristica=" + (caracteristica + contador).ToString();
                existe = ejecuta.scalarToBool(sql);
                if (!Convert.ToBoolean(existe[1]))
                {

                    sql = "INSERT INTO Observaciones_Inventario_Vehiculo " +
                          "VALUES(" + noOrden.ToString() + ", " + idEmpresa.ToString() + ", " + idTaller.ToString() + ", " + (caracteristica + contador).ToString() + ", '" + campos[contador] + "')";
                    guardado = ejecuta.insertUpdateDelete(sql);
                    if (Convert.ToBoolean(guardado[0]))
                    {
                        if (Convert.ToBoolean(guardado[1]))
                            retorna = true;
                        else
                            break;
                    }

                }
                else
                {
                    sql = "UPDATE Observaciones_Inventario_Vehiculo " +
                          "SET observaciones = '" + campos[contador] +
                          "' where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString() + " and caracteristica = " + (caracteristica + contador).ToString();
                    guardado = ejecuta.insertUpdateDelete(sql);
                    if (Convert.ToBoolean(guardado[0]))
                    {
                        if (Convert.ToBoolean(guardado[1]))
                            retorna = true;
                        else
                            break;
                    }
                }
            }
        }
        catch (Exception x)
        {
            retorna = false;
        }
        return retorna;
    }

    public bool guardaInventarioDer(int noOrden, int idEmpresa, int idTaller, bool aletas, bool antena, bool costado, bool cristalesPuertDel, bool cristalesPuertTras, bool espejosExt, bool manijasExt, bool molduras, bool puertaDel, bool puertaTras, bool refleLatDel, bool refleLatTras, bool salpicadera, bool taponesRuedas)
    {
        try
        {
            sql = "select count(*) from Inventario_Vehiculo where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString();
            existe = ejecuta.scalarToBool(sql);
            if (Convert.ToBoolean(existe[1]))
            {
                sql = "UPDATE Inventario_Vehiculo SET aletas_der=" + pasarCeroUno(aletas).ToString() + ",antena_der = " + pasarCeroUno(antena).ToString() + ",costado_der=" + pasarCeroUno(costado).ToString() + ",cristales_puerta_delantera_der = " + pasarCeroUno(cristalesPuertDel).ToString() + ",cristales_puerta_trasera_der = " + pasarCeroUno(cristalesPuertTras).ToString() + ",espejos_exteriores_der = " + pasarCeroUno(espejosExt).ToString() + ",manijas_exteriores_der = " + pasarCeroUno(manijasExt).ToString() + ",molduras_der = " + pasarCeroUno(molduras).ToString() + ",puerta_delantera_der = " + pasarCeroUno(puertaDel).ToString() + ",puerta_trasera_der = " + pasarCeroUno(puertaTras).ToString() + ",reflejante_lateral_delantero_der = " + pasarCeroUno(refleLatDel).ToString() + ",reflejante_lateral_trasero_der = " + pasarCeroUno(refleLatTras).ToString() + ",salpicadera_der = " + pasarCeroUno(salpicadera).ToString() + ",tapones_rueda_der = " + pasarCeroUno(taponesRuedas).ToString() + ",der=1 " +
                      " where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString();
            }
            else
            {
                sql = "insert into Inventario_Vehiculo (no_orden,id_empresa,id_taller,aletas_der,antena_der,costado_der,cristales_puerta_delantera_der,cristales_puerta_trasera_der,espejos_exteriores_der,manijas_exteriores_der,molduras_der,puerta_delantera_der,puerta_trasera_der,reflejante_lateral_delantero_der,reflejante_lateral_trasero_der,salpicadera_der,tapones_rueda_der,der)" +
                      "values(" + noOrden.ToString() + ", " + idEmpresa.ToString() + ", " + idTaller.ToString() + "," + pasarCeroUno(aletas).ToString() + "," + pasarCeroUno(antena).ToString() + "," + pasarCeroUno(costado).ToString() + "," + pasarCeroUno(cristalesPuertDel).ToString() + "," + pasarCeroUno(cristalesPuertTras).ToString() + "," + pasarCeroUno(espejosExt).ToString() + "," + pasarCeroUno(manijasExt).ToString() + "," + pasarCeroUno(molduras).ToString() + "," + pasarCeroUno(puertaDel).ToString() + "," + pasarCeroUno(puertaTras).ToString() + "," + pasarCeroUno(refleLatDel).ToString() + "," + pasarCeroUno(refleLatTras).ToString() + "," + pasarCeroUno(salpicadera).ToString() + "," + pasarCeroUno(taponesRuedas).ToString() + ",1)";
            }
            guardado = ejecuta.insertUpdateDelete(sql);
            if (Convert.ToBoolean(guardado[0]))
            {
                if (Convert.ToBoolean(guardado[1])) { retorna = true; }
                else
                    retorna = false;
            }
            else
                retorna = false;
        }
        catch (Exception x)
        {
            retorna = false;
        }
        return retorna;
    }

    public object[] obtieneGeneralesExtras(int noOrden, int idEmpresa, int idTaller)
    {
        object[] valores = new object[4];
        try
        {
            sql = "select llantas from Inventario_Vehiculo" +
                  " where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString();
            valores[0] = ejecuta.scalarToStringSimple(sql);
            if (valores[0].ToString() == "" || valores[0].ToString() == null)
                valores[0] = "";
            sql = "select gasolina from Inventario_Vehiculo" +
                  " where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString();
            valores[1] = ejecuta.scalarToStringSimple(sql);
            if (valores[1].ToString() == "" || valores[1].ToString() == null)
                valores[1] = "";
            sql = "select observaciones from Inventario_Vehiculo" +
                  " where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString();
            valores[2] = ejecuta.scalarToStringSimple(sql);
            if (valores[2].ToString() == "" || valores[2].ToString() == null)
                valores[2] = "";
            sql = "select marca from Inventario_Vehiculo" +
                  " where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString();
            valores[3] = ejecuta.scalarToStringSimple(sql);
            if (valores[3].ToString() == "" || valores[3].ToString() == null)
                valores[3] = "";
        }
        catch (Exception x)
        {
            valores[0] = "";
            valores[1] = "1";
            valores[2] = "";
            valores[3] = "";
        }
        return valores;
    }

    public bool guardaInventarioDerDaños(int noOrden, int idEmpresa, int idTaller, string aletasD, string antenaD, string costadoD, string cristalesPuertDelD, string cristalesPuertTrasD, string espejosExtD, string manijasExtD, string moldurasD, string puertaDelD, string puertaTrasD, string refleLatDelD, string refleLatTrasD, string salpicaderaD, string taponesRuedasD)
    {
        try
        {
            string[] campos = { aletasD, antenaD, costadoD, cristalesPuertDelD, cristalesPuertTrasD, espejosExtD, manijasExtD, moldurasD, puertaDelD, puertaTrasD, refleLatDelD, refleLatTrasD, salpicaderaD, taponesRuedasD };
            int caracteristica = 18;
            for (int contador = 0; contador < campos.Length; contador++)
            {
                sql = "select count(*) from Observaciones_Inventario_Vehiculo where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString() + " and caracteristica=" + (caracteristica + contador).ToString();
                existe = ejecuta.scalarToBool(sql);
                if (!Convert.ToBoolean(existe[1]))
                {
                    sql = "INSERT INTO Observaciones_Inventario_Vehiculo " +
                          "VALUES(" + noOrden.ToString() + ", " + idEmpresa.ToString() + ", " + idTaller.ToString() + ", " + (caracteristica + contador).ToString() + ", '" + campos[contador] + "')";
                    guardado = ejecuta.insertUpdateDelete(sql);
                    if (Convert.ToBoolean(guardado[0]))
                    {
                        if (Convert.ToBoolean(guardado[1]))
                            retorna = true;
                        else
                            break;
                    }

                }
                else
                {
                    sql = "UPDATE Observaciones_Inventario_Vehiculo " +
                          "SET observaciones = '" + campos[contador] +
                          "' where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString() + " and caracteristica = " + (caracteristica + contador).ToString();
                    guardado = ejecuta.insertUpdateDelete(sql);
                    if (Convert.ToBoolean(guardado[0]))
                    {
                        if (Convert.ToBoolean(guardado[1]))
                            retorna = true;
                        else
                            break;
                    }
                }
            }
        }
        catch (Exception x)
        {
            retorna = false;
        }
        return retorna;
    }
    public bool guardaInventarioIzq(int noOrden, int idEmpresa, int idTaller, bool aletas, bool antena, bool costado, bool cristalesPuertDel, bool cristalesPuertTras, bool espejosExt, bool manijasExt, bool molduras, bool puertaDel, bool puertaTras, bool refleLatDel, bool refleLatTras, bool salpicadera, bool taponesRuedas)
    {
        try
        {
            sql = "select count(*) from Inventario_Vehiculo where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString();
            existe = ejecuta.scalarToBool(sql);
            if (Convert.ToBoolean(existe[1]))
            {
                sql = "UPDATE Inventario_Vehiculo SET aletas_izq=" + pasarCeroUno(aletas).ToString() + ",antena_izq = " + pasarCeroUno(antena).ToString() + ",costado_izq=" + pasarCeroUno(costado).ToString() + ",cristales_puerta_delantera_izq = " + pasarCeroUno(cristalesPuertDel).ToString() + ",cristales_puerta_trasera_izq = " + pasarCeroUno(cristalesPuertTras).ToString() + ",espejos_exteriores_izq = " + pasarCeroUno(espejosExt).ToString() + ",manijas_exteriores_izq = " + pasarCeroUno(manijasExt).ToString() + ",molduras_izq = " + pasarCeroUno(molduras).ToString() + ",puerta_delantera_izq = " + pasarCeroUno(puertaDel).ToString() + ",puerta_trasera_izq = " + pasarCeroUno(puertaTras).ToString() + ",reflejante_lateral_delantero_izq = " + pasarCeroUno(refleLatDel).ToString() + ",reflejante_lateral_trasero_izq = " + pasarCeroUno(refleLatTras).ToString() + ",salpicadera_izq = " + pasarCeroUno(salpicadera).ToString() + ",tapones_rueda_izq = " + pasarCeroUno(taponesRuedas).ToString() + ",izq=1 " +
                      " where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString();
            }
            else
            {
                sql = "insert into Inventario_Vehiculo (no_orden,id_empresa,id_taller,aletas_izq,antena_izq,costado_izq,cristales_puerta_delantera_izq,cristales_puerta_trasera_izq,espejos_exteriores_izq,manijas_exteriores_izq,molduras_izq,puerta_delantera_izq,puerta_trasera_izq,reflejante_lateral_delantero_izq,reflejante_lateral_trasero_izq,salpicadera_izq,tapones_rueda_izq,izq)" +
                      "values(" + noOrden.ToString() + ", " + idEmpresa.ToString() + ", " + idTaller.ToString() + "," + pasarCeroUno(aletas).ToString() + "," + pasarCeroUno(antena).ToString() + "," + pasarCeroUno(costado).ToString() + "," + pasarCeroUno(cristalesPuertDel).ToString() + "," + pasarCeroUno(cristalesPuertTras).ToString() + "," + pasarCeroUno(espejosExt).ToString() + "," + pasarCeroUno(manijasExt).ToString() + "," + pasarCeroUno(molduras).ToString() + "," + pasarCeroUno(puertaDel).ToString() + "," + pasarCeroUno(puertaTras).ToString() + "," + pasarCeroUno(refleLatDel).ToString() + "," + pasarCeroUno(refleLatTras).ToString() + "," + pasarCeroUno(salpicadera).ToString() + "," + pasarCeroUno(taponesRuedas).ToString() + ",1)";
            }
            guardado = ejecuta.insertUpdateDelete(sql);
            if (Convert.ToBoolean(guardado[0]))
            {
                if (Convert.ToBoolean(guardado[1])) { retorna = true; }
                else
                    retorna = false;
            }
            else
                retorna = false;
        }
        catch (Exception x)
        {
            retorna = false;
        }
        return retorna;
    }

    public bool guardaInventarioIzqDaños(int noOrden, int idEmpresa, int idTaller, string aletasD, string antenaD, string costadoD, string cristalesPuertDelD, string cristalesPuertTrasD, string espejosExtD, string manijasExtD, string moldurasD, string puertaDelD, string puertaTrasD, string refleLatDelD, string refleLatTrasD, string salpicaderaD, string taponesRuedasD)
    {
        try
        {
            string[] campos = { aletasD, antenaD, costadoD, cristalesPuertDelD, cristalesPuertTrasD, espejosExtD, manijasExtD, moldurasD, puertaDelD, puertaTrasD, refleLatDelD, refleLatTrasD, salpicaderaD, taponesRuedasD };
            int caracteristica = 4;
            for (int contador = 0; contador < campos.Length; contador++)
            {
                sql = "select count(*) from Observaciones_Inventario_Vehiculo where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString() + " and caracteristica=" + (caracteristica + contador).ToString();
                existe = ejecuta.scalarToBool(sql);
                if (!Convert.ToBoolean(existe[1]))
                {
                    sql = "INSERT INTO Observaciones_Inventario_Vehiculo " +
                          "VALUES(" + noOrden.ToString() + ", " + idEmpresa.ToString() + ", " + idTaller.ToString() + ", " + (caracteristica + contador).ToString() + ", '" + campos[contador] + "')";
                    guardado = ejecuta.insertUpdateDelete(sql);
                    if (Convert.ToBoolean(guardado[0]))
                    {
                        if (Convert.ToBoolean(guardado[1]))
                            retorna = true;
                        else
                            break;
                    }

                }
                else
                {
                    sql = "UPDATE Observaciones_Inventario_Vehiculo " +
                          "SET observaciones = '" + campos[contador] +
                          "' where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString() + " and caracteristica = " + (caracteristica + contador).ToString();
                    guardado = ejecuta.insertUpdateDelete(sql);
                    if (Convert.ToBoolean(guardado[0]))
                    {
                        if (Convert.ToBoolean(guardado[1]))
                            retorna = true;
                        else
                            break;
                    }
                }
            }
        }
        catch (Exception x)
        {
            retorna = false;
        }
        return retorna;
    }

    public bool guardaInventarioFront(int noOrden, int idEmpresa, int idTaller, bool biseles, bool brazosLim, bool cofre, bool cuartos, bool defensa, bool halogeno, bool niebla, bool parabrisas, bool parrilla, bool plumas, bool portaPlaca, bool spoiler)
    {
        try
        {
            sql = "select count(*) from Inventario_Vehiculo where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString();
            existe = ejecuta.scalarToBool(sql);
            if (Convert.ToBoolean(existe[1]))
            {
                sql = "UPDATE Inventario_Vehiculo SET biseles_fro=" + pasarCeroUno(biseles).ToString() + ",brazos_limpiadores_fro = " + pasarCeroUno(brazosLim).ToString() + ",cofre_fro = " + pasarCeroUno(cofre).ToString() + ",cuartos_luz_fro = " + pasarCeroUno(cuartos).ToString() + ",defensa_delantera_fro = " + pasarCeroUno(defensa).ToString() + ",faros_con_halogeno_fro = " + pasarCeroUno(halogeno).ToString() + ",faros_niebla_fro = " + pasarCeroUno(niebla).ToString() + ",parabrisas_fro = " + pasarCeroUno(parabrisas).ToString() + ",parrilla_fro = " + pasarCeroUno(parrilla).ToString() + ",plumas_limpiadoras_fro = " + pasarCeroUno(plumas).ToString() + ",porta_placa_fro = " + pasarCeroUno(portaPlaca).ToString() + ",spoiler_fro = " + pasarCeroUno(spoiler).ToString() + ",fron=1 " +
                      " where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString();
            }
            else
            {
                sql = "insert into Inventario_Vehiculo (no_orden,id_empresa,id_taller,biseles_fro,brazos_limpiadores_fro,cofre_fro,cuartos_luz_fro,defensa_delantera_fro,faros_con_halogeno_fro,faros_niebla_fro,parabrisas_fro,parrilla_fro,plumas_limpiadoras_fro,porta_placa_fro,spoiler_fro,fron)" +
                      "values(" + noOrden.ToString() + ", " + idEmpresa.ToString() + ", " + idTaller.ToString() + "," + pasarCeroUno(biseles).ToString() + "," + pasarCeroUno(brazosLim).ToString() + "," + pasarCeroUno(cofre).ToString() + "," + pasarCeroUno(cuartos).ToString() + "," + pasarCeroUno(defensa).ToString() + "," + pasarCeroUno(halogeno).ToString() + "," + pasarCeroUno(niebla).ToString() + "," + pasarCeroUno(parabrisas).ToString() + "," + pasarCeroUno(parrilla).ToString() + "," + pasarCeroUno(plumas).ToString() + "," + pasarCeroUno(portaPlaca).ToString() + "," + pasarCeroUno(spoiler).ToString() + ",1)";
            }
            guardado = ejecuta.insertUpdateDelete(sql);
            if (Convert.ToBoolean(guardado[0]))
            {
                if (Convert.ToBoolean(guardado[1])) { retorna = true; }
                else
                    retorna = false;
            }
            else
                retorna = false;
        }
        catch (Exception x)
        {
            retorna = false;
        }
        return retorna;
    }

    public bool guardaInventarioFrontDaños(int noOrden, int idEmpresa, int idTaller, string biselesD, string brazosLimD, string cofreD, string cuartosD, string defensaD, string halogenoD, string nieblaD, string parabrisasD, string parrillaD, string plumasD, string portaPlacaD, string spoilerD)
    {
        try
        {
            string[] campos = { biselesD, brazosLimD, cofreD, cuartosD, defensaD, halogenoD, nieblaD, parabrisasD, parrillaD, plumasD, portaPlacaD, spoilerD };
            int caracteristica = 32;
            for (int contador = 0; contador < campos.Length; contador++)
            {
                sql = "select count(*) from Observaciones_Inventario_Vehiculo where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString() + " and caracteristica=" + (caracteristica + contador).ToString();
                existe = ejecuta.scalarToBool(sql);
                if (!Convert.ToBoolean(existe[1]))
                {
                    sql = "INSERT INTO Observaciones_Inventario_Vehiculo " +
                          "VALUES(" + noOrden.ToString() + ", " + idEmpresa.ToString() + ", " + idTaller.ToString() + ", " + (caracteristica + contador).ToString() + ", '" + campos[contador] + "')";
                    guardado = ejecuta.insertUpdateDelete(sql);
                    if (Convert.ToBoolean(guardado[0]))
                    {
                        if (Convert.ToBoolean(guardado[1]))
                            retorna = true;
                        else
                            break;
                    }

                }
                else
                {
                    sql = "UPDATE Observaciones_Inventario_Vehiculo " +
                          "SET observaciones = '" + campos[contador] +
                          "' where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString() + " and caracteristica = " + (caracteristica + contador).ToString();
                    guardado = ejecuta.insertUpdateDelete(sql);
                    if (Convert.ToBoolean(guardado[0]))
                    {
                        if (Convert.ToBoolean(guardado[1]))
                            retorna = true;
                        else
                            break;
                    }
                }

            }
        }
        catch (Exception x)
        {
            retorna = false;
        }
        return retorna;
    }

    public bool guardaInventarioGen(int noOrden, int idEmpresa, int idTaller, bool llaves, bool canastilla, bool emblemas, bool bateria, bool compacDisc, bool ecualizador, bool rines, string llantasVida, string marca, int gasolina, string observaciones)
    {
        try
        {
            sql = "select count(*) from Inventario_Vehiculo where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString();
            existe = ejecuta.scalarToBool(sql);
            if (Convert.ToBoolean(existe[1]))
            {
                sql = "UPDATE Inventario_Vehiculo SET llaves_gen=" + pasarCeroUno(llaves).ToString() + ",canastilla_gen = " + pasarCeroUno(canastilla).ToString() + ",emblemas_gen = " + pasarCeroUno(emblemas).ToString() + ",bateria_gen = " + pasarCeroUno(bateria).ToString() + ",compac_disc_gen = " + pasarCeroUno(compacDisc).ToString() + ",ecualizador_gen = " + pasarCeroUno(ecualizador).ToString() + ",rines_gen = " + pasarCeroUno(rines).ToString() + ",llantas = '" + llantasVida + "',marca ='" + marca + "',gasolina = " + gasolina.ToString() + ",observaciones =' " + observaciones + "',gen=1 " +
                      " where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString();
            }
            else
            {
                sql = "insert into Inventario_Vehiculo (no_orden,id_empresa,id_taller,llaves_gen,canastilla_gen,emblemas_gen,bateria_gen,compac_disc_gen,ecualizador_gen,rines_gen,llantas,marca,gasolina,observaciones,gen)" +
                      "values("+noOrden.ToString()+","+idEmpresa.ToString()+","+idTaller.ToString()+"," + pasarCeroUno(llaves).ToString() + "," + pasarCeroUno(canastilla).ToString() + "," + pasarCeroUno(emblemas).ToString() + "," + pasarCeroUno(bateria).ToString() + "," + pasarCeroUno(compacDisc).ToString() + "," + pasarCeroUno(ecualizador).ToString() + "," + pasarCeroUno(rines).ToString() + ",'" + llantasVida + "','" + marca + "'," + gasolina.ToString() + ",'" + observaciones + "',1)";
            }
            sql = sql + " update Ordenes_Reparacion set id_med_gas=" + gasolina.ToString() + " where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString();
            guardado = ejecuta.insertUpdateDelete(sql);
            if (Convert.ToBoolean(guardado[0]))
            {
                if (Convert.ToBoolean(guardado[1])) { retorna = true; }
                else
                    retorna = false;
            }
            else
                retorna = false;
        }
        catch (Exception x)
        {
            retorna = false;
        }
        return retorna;
    }

    public bool guardaInventarioGenDaños(int noOrden, int idEmpresa, int idTaller, string llavesD, string canastillaD, string emblemasD, string bateriaD, string compacDiscD, string ecualizadorD, string rinesD)
    {
        try
        {
            object[] campos = { llavesD, canastillaD, emblemasD, bateriaD, compacDiscD, ecualizadorD, rinesD };
            int caracteristica = 92;
            for (int contador = 0; contador < campos.Length; contador++)
            {
                sql = "select count(*) from Observaciones_Inventario_Vehiculo where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString() + " and caracteristica=" + (caracteristica + contador).ToString();
                existe = ejecuta.scalarToBool(sql);
                if (!Convert.ToBoolean(existe[1]))
                {
                    sql = "INSERT INTO Observaciones_Inventario_Vehiculo " +
                          "VALUES(" + noOrden.ToString() + ", " + idEmpresa.ToString() + ", " + idTaller.ToString() + ", " + (caracteristica + contador).ToString() + ", '" + campos[contador] + "')";
                    guardado = ejecuta.insertUpdateDelete(sql);
                    if (Convert.ToBoolean(guardado[0]))
                    {
                        if (Convert.ToBoolean(guardado[1]))
                            retorna = true;
                        else
                            break;
                    }

                }
                else
                {
                    sql = "UPDATE Observaciones_Inventario_Vehiculo " +
                          "SET observaciones = '" + campos[contador] +
                          "' where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString() + " and caracteristica = " + (caracteristica + contador).ToString();
                    guardado = ejecuta.insertUpdateDelete(sql);
                    if (Convert.ToBoolean(guardado[0]))
                    {
                        if (Convert.ToBoolean(guardado[1]))
                            retorna = true;
                        else
                            break;
                    }
                }
            }
        }
        catch (Exception x)
        {
            retorna = false;
        }
        return retorna;
    }

    public bool guardaInventarioPost(int noOrden, int idEmpresa, int idTaller, bool calaveras, bool cuartos, bool defensa, bool facia, bool portaPlaca, bool topes, bool limpiadores, bool medallon, bool mica, bool sistemaEscape, bool spoiler, bool taponGas, bool luzPlaca)
    {
        try
        {
            sql = "select count(*) from Inventario_Vehiculo where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString();
            existe = ejecuta.scalarToBool(sql);
            if (Convert.ToBoolean(existe[1]))
            {
                sql = "UPDATE Inventario_Vehiculo SET calaveras_pos=" + pasarCeroUno(calaveras).ToString() + ",cuartos_pos = " + pasarCeroUno(cuartos).ToString() + ",defensa_trasera_pos = " + pasarCeroUno(defensa).ToString() + ",facia_pos = " + pasarCeroUno(facia).ToString() + ",porta_placa_pos = " + pasarCeroUno(portaPlaca).ToString() + ",topes_pos = " + pasarCeroUno(topes).ToString() + ",limpiadores_pos = " + pasarCeroUno(limpiadores).ToString() + ",medallon_pos = " + pasarCeroUno(medallon).ToString() + ",mica_pos = " + pasarCeroUno(mica).ToString() + ",sistema_escape_pos = " + pasarCeroUno(sistemaEscape).ToString() + ",spoiler_pos = " + pasarCeroUno(spoiler).ToString() + ",tapon_gasolina_pos = " + pasarCeroUno(taponGas).ToString() + ",luz_placa_pos = " + pasarCeroUno(luzPlaca).ToString() + ",post=1 " +
                      " where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString();
            }
            else
            {
                sql = "insert into Inventario_Vehiculo (no_orden,id_empresa,id_taller,calaveras_pos,cuartos_pos,defensa_trasera_pos,facia_pos,porta_placa_pos,topes_pos,limpiadores_pos,medallon_pos,mica_pos,sistema_escape_pos,spoiler_pos,tapon_gasolina_pos,luz_placa_pos,post)" +
                      "values(" + pasarCeroUno(calaveras).ToString() + "," + pasarCeroUno(cuartos).ToString() + "," + pasarCeroUno(defensa).ToString() + "," + pasarCeroUno(facia).ToString() + "," + pasarCeroUno(portaPlaca).ToString() + "," + pasarCeroUno(topes).ToString() + "," + pasarCeroUno(limpiadores).ToString() + "," + pasarCeroUno(medallon).ToString() + "," + pasarCeroUno(mica).ToString() + "," + pasarCeroUno(sistemaEscape).ToString() + "," + pasarCeroUno(spoiler).ToString() + "," + pasarCeroUno(taponGas).ToString() + "," + pasarCeroUno(luzPlaca).ToString() + ",1)";
            }
            guardado = ejecuta.insertUpdateDelete(sql);
            if (Convert.ToBoolean(guardado[0]))
            {
                if (Convert.ToBoolean(guardado[1])) { retorna = true; }
                else
                    retorna = false;
            }
            else
                retorna = false;
        }
        catch (Exception x)
        {
            retorna = false;
        }
        return retorna;
    }
    public bool guardaInventarioPostDaños(int noOrden, int idEmpresa, int idTaller, string calaberasD, string cuartosD, string defensaD, string faciaD, string portaPlacaD, string topesD, string limpiadoresD, string medallonD, string micaD, string sistemaEscapeD, string spoilerD, string taponGasD, string luzPlacaD)
    {
        try
        {
            object[] campos = { calaberasD, cuartosD, defensaD, faciaD, portaPlacaD, topesD, limpiadoresD, medallonD, micaD, sistemaEscapeD, spoilerD, taponGasD, luzPlacaD };
            int caracteristica = 44;
            for (int contador = 0; contador < campos.Length; contador++)
            {
                sql = "select count(*) from Observaciones_Inventario_Vehiculo where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString() + " and caracteristica=" + (caracteristica + contador).ToString();
                existe = ejecuta.scalarToBool(sql);
                if (!Convert.ToBoolean(existe[1]))
                {
                    sql = "INSERT INTO Observaciones_Inventario_Vehiculo " +
                          "VALUES(" + noOrden.ToString() + ", " + idEmpresa.ToString() + ", " + idTaller.ToString() + ", " + (caracteristica + contador).ToString() + ", '" + campos[contador] + "')";
                    guardado = ejecuta.insertUpdateDelete(sql);
                    if (Convert.ToBoolean(guardado[0]))
                    {
                        if (Convert.ToBoolean(guardado[1]))
                            retorna = true;
                        else
                            break;
                    }

                }
                else
                {
                    sql = "UPDATE Observaciones_Inventario_Vehiculo " +
                          "SET observaciones = '" + campos[contador] +
                          "' where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString() + " and caracteristica = " + (caracteristica + contador).ToString();
                    guardado = ejecuta.insertUpdateDelete(sql);
                    if (Convert.ToBoolean(guardado[0]))
                    {
                        if (Convert.ToBoolean(guardado[1]))
                            retorna = true;
                        else
                            break;
                    }
                }
            }
        }
        catch (Exception x)
        {
            retorna = false;
        }
        return retorna;
    }

    public bool guardaInventarioInter(int noOrden, int idEmpresa, int idTaller, bool alfombra, bool asientoDel, bool asientoTras, bool radioAgencia, bool bocinas, bool estereo, bool btnsPuerts, bool btnsRadio, bool cabeceras, bool guantera, bool cenicero, bool cinturonesSeg, bool coderas, bool consola, bool elevacionElect, bool encendedor, bool espejo, bool luzInt, bool manijasInt, bool palanca, bool perillaPalanca, bool reloj, bool tablero, bool viceras, bool tapetes, bool cieloToldo)
    {
        try
        {
            sql = "select count(*) from Inventario_Vehiculo where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString();
            existe = ejecuta.scalarToBool(sql);
            if (Convert.ToBoolean(existe[1]))
            {
                sql = "UPDATE Inventario_Vehiculo SET alfombra_int=" + pasarCeroUno(alfombra).ToString() + ",asientos_delanteros_int = " + pasarCeroUno(asientoDel).ToString() + ",asientos_traseros_int = " + pasarCeroUno(asientoTras).ToString() + ",radio_estereo_agencia_int = " + pasarCeroUno(radioAgencia).ToString() + ",bocinas_int = " + pasarCeroUno(bocinas).ToString() + ",estereo_int = " + pasarCeroUno(estereo).ToString() + ",botones_puerta_int = " + pasarCeroUno(btnsPuerts).ToString() + ",botones_radio_autoestero_int = " + pasarCeroUno(btnsRadio).ToString() + ",cabeceras_int = " + pasarCeroUno(cabeceras).ToString() + ",cajuela_guantes_int = " + pasarCeroUno(guantera).ToString() + ",cenicero_int = " + pasarCeroUno(cenicero).ToString() + ",cinturones_seguridad_int = " + pasarCeroUno(cinturonesSeg).ToString() + ",coderas_int = " + pasarCeroUno(coderas).ToString() + ",consola_int = " + pasarCeroUno(consola).ToString() + ",control_electrico_elevacion_int = " + pasarCeroUno(elevacionElect).ToString() + ",encendedor_int = " + pasarCeroUno(encendedor).ToString() + ",espejo_interior_int = " + pasarCeroUno(espejo).ToString() + ",luz_interior_int = " + pasarCeroUno(luzInt).ToString() + ",manijas_interiores_int = " + pasarCeroUno(manijasInt).ToString() + ",palanca_velocidades_int = " + pasarCeroUno(palanca).ToString() + ",prilla_palanca_int = " + pasarCeroUno(perillaPalanca).ToString() + ",reloj_int = " + pasarCeroUno(reloj).ToString() + ",tablero_int = " + pasarCeroUno(tablero).ToString() + ",viceras_int = " + pasarCeroUno(viceras).ToString() + ",tapetes_int = " + pasarCeroUno(tapetes).ToString() + ",cielo_toldo_int = " + pasarCeroUno(cieloToldo).ToString() + ",int=1 " +
                      " where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString();
            }
            else
            {
                sql = "insert into Inventario_Vehiculo (no_orden,id_empresa,id_taller,alfombra_int,asientos_delanteros_int,asientos_traseros_int,radio_estereo_agencia_int,bocinas_int,estereo_int,botones_puerta_int,botones_radio_autoestero_int,cabeceras_int,cajuela_guantes_int,cenicero_int,cinturones_seguridad_int,coderas_int,consola_int,control_electrico_elevacion_int,encendedor_int,espejo_interior_int,luz_interior_int,manijas_interiores_int,palanca_velocidades_int,prilla_palanca_int,reloj_int,tablero_int,viceras_int,tapetes_int,cielo_toldo_int,int)" +
                      "values(" + pasarCeroUno(alfombra).ToString() + "," + pasarCeroUno(asientoDel).ToString() + "," + pasarCeroUno(asientoTras).ToString() + "," + pasarCeroUno(radioAgencia).ToString() + "," + pasarCeroUno(bocinas).ToString() + "," + pasarCeroUno(estereo).ToString() + "," + pasarCeroUno(btnsPuerts).ToString() + "," + pasarCeroUno(btnsRadio).ToString() + "," + pasarCeroUno(cabeceras).ToString() + "," + pasarCeroUno(guantera).ToString() + "," + pasarCeroUno(cenicero).ToString() + "," + pasarCeroUno(cinturonesSeg).ToString() + "," + pasarCeroUno(coderas).ToString() + "," + pasarCeroUno(consola).ToString() + "," + pasarCeroUno(elevacionElect).ToString() + "," + pasarCeroUno(encendedor).ToString() + "," + pasarCeroUno(espejo).ToString() + "," + pasarCeroUno(luzInt).ToString() + "," + pasarCeroUno(manijasInt).ToString() + "," + pasarCeroUno(palanca).ToString() + "," + pasarCeroUno(perillaPalanca).ToString() + "," + pasarCeroUno(reloj).ToString() + "," + pasarCeroUno(tablero).ToString() + "," + pasarCeroUno(viceras).ToString() + "," + pasarCeroUno(tapetes).ToString() + "," + pasarCeroUno(cieloToldo).ToString() + ",1)";
            }
            guardado = ejecuta.insertUpdateDelete(sql);
            if (Convert.ToBoolean(guardado[0]))
            {
                if (Convert.ToBoolean(guardado[1])) { retorna = true; }
                else
                    retorna = false;
            }
            else
                retorna = false;
        }
        catch (Exception x)
        {
            retorna = false;
        }
        return retorna;
    }
    public bool guardaInventarioInterDaños(int noOrden, int idEmpresa, int idTaller, string alfombraD, string asientoDelD, string asientoTrasD, string radioAgenciaD, string bocinasD, string estereoD, string btnsPuertsD, string btnsRadioD, string cabecerasD, string guanteraD, string ceniceroD, string cinturonesSegD, string coderasD, string consolaD, string elevacionElectD, string encendedorD, string espejoD, string luzIntD, string manijasIntD, string palancaD, string perillaPalancaD, string relojD, string tableroD, string vicerasD, string tapetesD, string cieloToldoD)
    {
        try
        {
            object[] campos = { alfombraD, asientoDelD, asientoTrasD, radioAgenciaD, bocinasD, estereoD, btnsPuertsD, btnsRadioD, cabecerasD, guanteraD, ceniceroD, cinturonesSegD, coderasD, consolaD, elevacionElectD, encendedorD, espejoD, luzIntD, manijasIntD, palancaD, perillaPalancaD, relojD, tableroD, vicerasD, tapetesD, cieloToldoD };
            int caracteristica = 57;
            for (int contador = 0; contador < campos.Length; contador++)
            {
                sql = "select count(*) from Observaciones_Inventario_Vehiculo where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString() + " and caracteristica=" + (caracteristica + contador).ToString();
                existe = ejecuta.scalarToBool(sql);
                if (!Convert.ToBoolean(existe[1]))
                {
                    sql = "INSERT INTO Observaciones_Inventario_Vehiculo " +
                          "VALUES(" + noOrden.ToString() + ", " + idEmpresa.ToString() + ", " + idTaller.ToString() + ", " + (caracteristica + contador).ToString() + ", '" + campos[contador] + "')";
                    guardado = ejecuta.insertUpdateDelete(sql);
                    if (Convert.ToBoolean(guardado[0]))
                    {
                        if (Convert.ToBoolean(guardado[1]))
                            retorna = true;
                        else
                            break;
                    }

                }
                else
                {
                    sql = "UPDATE Observaciones_Inventario_Vehiculo " +
                          "SET observaciones = '" + campos[contador] +
                          "' where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString() + " and caracteristica = " + (caracteristica + contador).ToString();
                    guardado = ejecuta.insertUpdateDelete(sql);
                    if (Convert.ToBoolean(guardado[0]))
                    {
                        if (Convert.ToBoolean(guardado[1]))
                            retorna = true;
                        else
                            break;
                    }
                }
            }
        }
        catch (Exception x)
        {
            retorna = false;
        }
        return retorna;
    }

    private int pasarCeroUno(bool valor)
    {
        if (valor)
            return 1;
        else
            return 0;
    }
    private bool pasarCeroUnoABool(int valor)
    {
        if (valor == 1)
            return true;
        else
            return false;
    }

    public bool existeInventarioOrden(int noOrden, int idEmpresa, int idTaller)
    {
        sql = "select count(*) from Inventario_Vehiculo where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString();
        existe = ejecuta.scalarToBool(sql);
        if (Convert.ToBoolean(existe[0]))
            if (Convert.ToBoolean(existe[1]))
                return true;
            else
                return false;
        else
            return false;
    }

    public bool[] obtieneBit(int noOrden, int idEmpresa, int idTaller, string[] columnas)
    {
        bool[] resultado = new bool[columnas.Length];
        try
        {
            for (int contador = 0; contador < columnas.Length; contador++)
            {
                sql = "select isnull(" + columnas[contador] + ",1) from Inventario_Vehiculo " +
                      " where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString();
                object[] ejecutado = ejecuta.scalarToInt(sql);
                if (Convert.ToBoolean(ejecutado[0]))
                {
                    int valor = Convert.ToInt32(ejecutado[1]);
                    resultado[contador] = pasarCeroUnoABool(valor);
                }
                else
                    resultado[contador] = false;
            }
        }
        catch (Exception x)
        {
            for (int contador = 0; contador < columnas.Length; contador++)
            {
                resultado[contador] = false;
            }
        }
        return resultado;
    }

    public string[] obtieneDanos(int noOrden, int idEmpresa, int idTaller, int numeroInicial, int totales)
    {
        string[] valores = new string[totales + 1];
        int tope = totales + numeroInicial + 1;
        int fin = tope + valores.Length;
        int contadorInterno = 0;
        try
        {
            for (int contador = numeroInicial; contador < tope; contador++)
            {
                sql = "SELECT count(*) FROM Observaciones_Inventario_Vehiculo" +
                      " where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString() + " and caracteristica = " + (contador).ToString();
                existe = ejecuta.scalarToBool(sql);
                retorna = Convert.ToBoolean(existe[0]);
                if (retorna)
                    retorna = Convert.ToBoolean(existe[1]);
                if (retorna)
                {
                    sql = "SELECT observaciones FROM Observaciones_Inventario_Vehiculo" +
                          " where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString() + " and caracteristica = " + (contador).ToString();
                    valores[contadorInterno] = ejecuta.scalarToStringSimple(sql);
                    contadorInterno++;
                }
                else
                    valores[contadorInterno] = "";
            }
        }
        catch (Exception x)
        {
            for (int contador = numeroInicial; contador < tope; contador++)
            {
                valores[contadorInterno] = "";
                contadorInterno++;
            }
        }
        return valores;
    }

    public bool obtieneSeccionInventario(int idEmpresa, int idTaller, int orden, int seccion)
    {        
        string seccionBusqueda = "";
        switch (seccion) { 
            case 1:
                seccionBusqueda = "izq";
                break;
            case 2:
                seccionBusqueda = "der";
                break;
            case 3:
                seccionBusqueda = "fron";
                break;
            case 4:
                seccionBusqueda = "post";
                break;
            case 5:
                seccionBusqueda = "int";
                break;
            case 6:
                seccionBusqueda = "caj";
                break;
            case 7:
                seccionBusqueda = "gen";
                break;
            default:
                seccionBusqueda = "";
                break;
        }
        string sql = string.Format("select isnull(" + seccionBusqueda + ",0) as seccion from inventario_vehiculo where no_orden={0} and id_empresa={1} and id_taller={2}", orden, idEmpresa, idTaller);
        existe = ejecuta.scalarToBool(sql);
        retorna = Convert.ToBoolean(existe[0]);
        if (retorna)
            retorna = Convert.ToBoolean(existe[1]);

        return retorna;
    }

    public int obtieneMedGasOrden(int noOrden, int idEmpresa, int idTaller)
    {
        string sql = "select id_med_gas from Ordenes_Reparacion where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString();
        existe = ejecuta.scalarToInt(sql);
        retorna = Convert.ToBoolean(existe[0]);
        if (retorna)
            return Convert.ToInt32(existe[1]);
        else
            return 0;
    }



}