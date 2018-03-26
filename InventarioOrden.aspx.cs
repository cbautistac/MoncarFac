using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;


public partial class InventarioOrden : System.Web.UI.Page
{
    Recepciones recepciones = new Recepciones();
    ControlesUsuario datos = new ControlesUsuario();
    DatosOrdenes datosOrdenes = new DatosOrdenes();
    protected void Page_Load(object sender, EventArgs e)
    {
        obtieneSesiones();
        cargaImagenesSecciones();
        if (!IsPostBack)
        {
            PanelImgZoom.Visible = PanelMascara.Visible = false;
            cargaDatosPie();
            lblError.Text = "";
            cargaDatosInventario();
            checaInventario();
            string estatus = obtieneEstatus();
            if (estatus != "")
            {
                if (estatus == "R" || estatus == "T" || estatus == "F" || estatus == "S")
                {
                    btnGuardarIzq.Visible = btnGuardarDer.Visible = btnGuardaPos.Visible = btnGuardarFron.Visible = btnGuardaInt.Visible = btnGuardaCajuela.Visible = btnGuardaGenerales.Visible = false;
                    AsyncUpload1.Visible = false;
                    btnAddFoto.Visible = false;
                }
            }
            else
                Response.Redirect("BienvenidaOrdenes.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + Request.QueryString["o"] + "&f=" + Request.QueryString["f"]);
        }
    }

    private string obtieneEstatus()
    {
        string estatus = "";
        try
        {
            int[] sesiones = obtieneSesiones();
            object[] infoEstatus = recepciones.obtieneEstatusOrden(sesiones[2].ToString(), sesiones[3].ToString(), sesiones[4].ToString());
            if (Convert.ToBoolean(infoEstatus[0]))
                estatus = Convert.ToString(infoEstatus[1]);
            else
                estatus = "";
        }
        catch (Exception) { estatus = ""; }
        return estatus;
    }

    private void checaInventario()
    {
        int[] sesiones = obtieneSesiones();
        object[] inventario = datosOrdenes.obtieneInfoInventario(sesiones[2], sesiones[3], sesiones[4]);
        if (Convert.ToBoolean(inventario[0]))
        {
            if (Convert.ToBoolean(inventario[1]))
            {
                actualizaProceso(2);
                actualizaLocalizacion();
            }
        }
    }

    private void actualizaProceso(int seccion)
    {
        int[] sesiones = obtieneSesiones();
        int compleato = 1;
        string error = "Aun hace falta completar información del inventario";
        object[] actualizaOrden = recepciones.actualizaProcesosOrden(sesiones[2], sesiones[3], sesiones[4], compleato, seccion);
        if (!Convert.ToBoolean(actualizaOrden[0]))
            lblError.Text = error;
    }

    private void cargaImagenesSecciones()
    {
        int idEmpresa = 0;
        int idTaller = 0;
        int orden = 0;
        try
        {
            idEmpresa = Convert.ToInt32(Request.QueryString["e"]);
            idTaller = Convert.ToInt32(Request.QueryString["t"]);
            orden = Convert.ToInt32(Request.QueryString["o"]);
        }
        catch (Exception)
        {
            idEmpresa = idTaller = orden = 0;
        }
        if (datos.obtieneSeccionInventario(idEmpresa, idTaller, orden, 1))
            imgIzq.ImageUrl = "~/img/Inventario/izquierdo_verde.png";
        else
            imgIzq.ImageUrl = "~/img/Inventario/izquierdo_rojo.png";
        if (datos.obtieneSeccionInventario(idEmpresa, idTaller, orden, 2))
            imgDer.ImageUrl = "~/img/Inventario/derecho_verde.png";
        else
            imgDer.ImageUrl = "~/img/Inventario/derecho_rojo.png";
        if (datos.obtieneSeccionInventario(idEmpresa, idTaller, orden, 3))
            imgFro.ImageUrl = "~/img/Inventario/frontal_verde.png";
        else
            imgFro.ImageUrl = "~/img/Inventario/frontal_rojo.png";
        if (datos.obtieneSeccionInventario(idEmpresa, idTaller, orden, 4))
            imgPos.ImageUrl = "~/img/Inventario/posterior_verde.png";
        else
            imgPos.ImageUrl = "~/img/Inventario/posterior_rojo.png";
        if (datos.obtieneSeccionInventario(idEmpresa, idTaller, orden, 5))
            imgInt.ImageUrl = "~/img/Inventario/interior_verde.png";
        else
            imgInt.ImageUrl = "~/img/Inventario/interior_rojo.png";
        if (datos.obtieneSeccionInventario(idEmpresa, idTaller, orden, 6))
            imgCaj.ImageUrl = "~/img/Inventario/cajuela_verde.png";
        else
            imgCaj.ImageUrl = "~/img/Inventario/cajuela_rojo.png";
        if (datos.obtieneSeccionInventario(idEmpresa, idTaller, orden, 7))
            imgGen.ImageUrl = "~/img/Inventario/generales_verde.png";
        else
            imgGen.ImageUrl = "~/img/Inventario/generales_rojo.png";
    }

    private void cargaDatosInventario()
    {
        cargaLadoIzq();
        cargaLadoDer();
        cargaFrontal();
        cargaPosterior();
        cargaInterior();
        cargaCajuela();
        cargaGenerales();
    }

    private void cargaGenerales()
    {
        CheckBox[] checks = { chkLlaves, chkCanastilla, chkEmblema, chkBateria, chkCompacDisc, chkEcualizador, chkRines };

        TextBox[] textBoxes = { txtLlaves, txtCanastilla, txtEmblema, txtBateria, txtCompacDisc, txtEcualizador, txtRines };

        for (int i = 0; i < 7; i++)
        {
            checks[i].Checked = true;
            textBoxes[i].Text = "";
        }
        txtLlantas.Text = txtObservaciones.Text = txtMarca.Text = "";
        ddlGasolina.SelectedIndex = -1;


        try
        {
            int idEmpresa = Convert.ToInt32(Request.QueryString["e"]);
            int idTaller = Convert.ToInt32(Request.QueryString["t"]);
            int noOrden = Convert.ToInt32(Request.QueryString["o"]);
            bool existe = datos.existeInventarioOrden(noOrden, idEmpresa, idTaller);
            if (existe)
            {
                string[] columnas = { "llaves_gen", "canastilla_gen", "emblemas_gen", "bateria_gen", "compac_disc_gen", "ecualizador_gen", "rines_gen" };
                int numeroInicial = 92;
                int totales = 6;
                llenaInfo(columnas, numeroInicial, totales, noOrden, idEmpresa, idTaller, checks, textBoxes);
                object[] generales = datos.obtieneGeneralesExtras(noOrden, idEmpresa, idTaller);
                txtLlantas.Text = generales[0].ToString();
                if (generales[1].ToString() != "")
                    ddlGasolina.SelectedValue = generales[1].ToString();
                else
                {
                    int idGasRec = datos.obtieneMedGasOrden(noOrden, idEmpresa, idTaller);
                    if (idGasRec != 0)
                        ddlGasolina.SelectedValue = idGasRec.ToString();
                }
                txtObservaciones.Text = generales[2].ToString();
                txtMarca.Text = generales[3].ToString();
            }
            else
            {
                int idGasRec = datos.obtieneMedGasOrden(noOrden, idEmpresa, idTaller);
                if (idGasRec != 0)
                    ddlGasolina.SelectedValue = idGasRec.ToString();
                else
                    ddlGasolina.SelectedIndex = -1;
            }
        }
        catch (Exception)
        {
            for (int i = 0; i < 7; i++)
            {
                checks[i].Checked = true;
                textBoxes[i].Text = "";
            }
            txtLlantas.Text = txtObservaciones.Text = txtMarca.Text = "";
            ddlGasolina.SelectedIndex = -1;
        }
    }

    private void cargaCajuela()
    {
        CheckBox[] checks = { chkCables_Corriente, chkLlanta_Refaccion, chkGato, chkHerramientas, chkLave_Rueda, chkSeñales_Carretera, chkTapetes, chkTapa_Carton, chkExtinguidor };

        TextBox[] textBoxes = { txtCables_Corriente, txtLlanta_Refaccion, txtGato, txtHerramientas, txtLave_Rueda, txtSeñales_Carretera, txtTapetes, txtTapa_Carton, txtExtinguidor };

        for (int i = 0; i < 9; i++)
        {
            checks[i].Checked = true;
            textBoxes[i].Text = "";
        }

        try
        {
            int idEmpresa = Convert.ToInt32(Request.QueryString["e"]);
            int idTaller = Convert.ToInt32(Request.QueryString["t"]);
            int noOrden = Convert.ToInt32(Request.QueryString["o"]);
            bool existe = datos.existeInventarioOrden(noOrden, idEmpresa, idTaller);
            if (existe)
            {
                string[] columnas = { "cables_corriente_caj", "llantas_refaccion_caj", "gato_caj", "herramientas_caj", "llave_rueda_caj", "señales_carretera_caj", "tapetes_caj", "tapa_carton_caj", "extinguidor_caj" };
                int numeroInicial = 83;
                int totales = 8;
                llenaInfo(columnas, numeroInicial, totales, noOrden, idEmpresa, idTaller, checks, textBoxes);
            }
        }
        catch (Exception)
        {
            for (int i = 0; i < 9; i++)
            {
                checks[i].Checked = true;
                textBoxes[i].Text = "";
            }
        }
    }

    private void cargaInterior()
    {
        CheckBox[] checks = {chkAlfombra, chkAsientosDelanteros, chkAsientosTraseros, chkRadioEstereoAgencia, chkBocinas, chkEsterero,
                                chkBotonesPuerta, chkBotonesRadioAutoestereo, chkCabeceras, chkCajuelaGuantes, chkCenicero, chkCinturonesSeguridad,
                                chkCoderas, chkConsola, chkControlElectricoElev, chkEncendedor, chkEspejoInt, chkLuzInterioir, chkManijasInteriores,
                                chkPalancaVelocidades, chkPerillaPalanca, chkReloj, chkTablero, chkViceras, chkTapetesInt, chkCieloToldo};

        TextBox[] textBoxes = {txtAlfombra, txtAsientosDelanteros, txtAsientosTraseros, txtRadioEstereoAgencia, txtBocinas, txtEsterero,
                                  txtBotonesPuerta, txtBotonesRadioAutoestereo, txtCabeceras, txtCajuelaGuantes, txtCenicero, txtCinturonesSeguridad,
                                  txtCoderas, txtConsola, txtControlElectricoElev, txtEncendedor, txtEspejoInt, txtLuzInterioir, txtManijasInteriores,
                                  txtPalancaVelocidades, txtPerillaPalanca, txtReloj, txtTablero, txtViceras, txtTapetesInt, txtCieloToldo};

        for (int i = 0; i < 26; i++)
        {
            checks[i].Checked = true;
            textBoxes[i].Text = "";
        }

        try
        {
            int idEmpresa = Convert.ToInt32(Request.QueryString["e"]);
            int idTaller = Convert.ToInt32(Request.QueryString["t"]);
            int noOrden = Convert.ToInt32(Request.QueryString["o"]);
            bool existe = datos.existeInventarioOrden(noOrden, idEmpresa, idTaller);
            if (existe)
            {
                string[] columnas = { "alfombra_int", "asientos_delanteros_int", "asientos_traseros_int", "radio_estereo_agencia_int", "bocinas_int", "estereo_int", "botones_puerta_int", "botones_radio_autoestero_int", "cabeceras_int", "cajuela_guantes_int", "cenicero_int", "cinturones_seguridad_int", "coderas_int", "consola_int", "control_electrico_elevacion_int", "encendedor_int", "espejo_interior_int", "luz_interior_int", "manijas_interiores_int", "palanca_velocidades_int", "prilla_palanca_int", "reloj_int", "tablero_int", "viceras_int", "tapetes_int", "cielo_toldo_int" };
                int numeroInicial = 57;
                int totales = 25;
                llenaInfo(columnas, numeroInicial, totales, noOrden, idEmpresa, idTaller, checks, textBoxes);
            }
        }
        catch (Exception)
        {
            for (int i = 0; i < 26; i++)
            {
                checks[i].Checked = true;
                textBoxes[i].Text = "";
            }
        }
    }

    private void cargaPosterior()
    {
        CheckBox[] checks = {chkCalaveras, chkCuartos, chkDefensaTrasera, chkFacia, chkPortaPlacaP, chkTopes, chkLimpiadores, chkMedallon,
                                chkMica, chkSistemaEscape, chkSpoiles, chkTaponGasolina, chkLuzPlaca};

        TextBox[] textBoxes = {txtCalaveras, txtCuartos, txtDefensaTrasera, txtFacia, txtPortaPlacaP, txtTopes, txtLimpiadores, txtMedallon,
                                  txtMica, txtSistemaEscape, txtSpoiles, txtTaponGasolina, txtLuzPlaca};

        for (int i = 0; i < 13; i++)
        {
            checks[i].Checked = true;
            textBoxes[i].Text = "";
        }

        try
        {
            int idEmpresa = Convert.ToInt32(Request.QueryString["e"]);
            int idTaller = Convert.ToInt32(Request.QueryString["t"]);
            int noOrden = Convert.ToInt32(Request.QueryString["o"]);
            bool existe = datos.existeInventarioOrden(noOrden, idEmpresa, idTaller);
            if (existe)
            {
                string[] columnas = { "calaveras_pos", "cuartos_pos", "defensa_trasera_pos", "facia_pos", "porta_placa_pos", "topes_pos", "limpiadores_pos", "medallon_pos", "mica_pos", "sistema_escape_pos", "spoiler_pos", "tapon_gasolina_pos", "luz_placa_pos" };
                int numeroInicial = 44;
                int totales = 12;
                llenaInfo(columnas, numeroInicial, totales, noOrden, idEmpresa, idTaller, checks, textBoxes);
            }
        }
        catch (Exception)
        {
            for (int i = 0; i < 13; i++)
            {
                checks[i].Checked = true;
                textBoxes[i].Text = "";
            }
        }
    }

    private void cargaFrontal()
    {
        CheckBox[] checks = {chkBiseles, chkBrazosLimpiadores, chkCofre, chkCuartosLuz, chkDefensaDelantera, chkFarosHalogeno, chkFarosNiebla,
                                chkParabrisas, chkParrilla, chkPlumasLimpiadoras, chkPortaPlaca, chkEspoiler};

        TextBox[] textBoxes = {txtBiseles, txtBrazosLimpiadores, txtCofre, txtCuartosLuz, txtDefensaDelantera, txtFarosHalogeno, txtFarosNiebla,
                                  txtParabrisas, txtParrilla, txtPlumasLimpiadoras, txtPortaPlaca, txtEspoiler};

        for (int i = 0; i < 12; i++)
        {
            checks[i].Checked = true;
            textBoxes[i].Text = "";
        }

        try
        {
            int idEmpresa = Convert.ToInt32(Request.QueryString["e"]);
            int idTaller = Convert.ToInt32(Request.QueryString["t"]);
            int noOrden = Convert.ToInt32(Request.QueryString["o"]);
            bool existe = datos.existeInventarioOrden(noOrden, idEmpresa, idTaller);
            if (existe)
            {
                string[] columnas = { "biseles_fro", "brazos_limpiadores_fro", "cofre_fro", "cuartos_luz_fro", "defensa_delantera_fro", "faros_con_halogeno_fro", "faros_niebla_fro", "parabrisas_fro", "parrilla_fro", "plumas_limpiadoras_fro", "porta_placa_fro", "spoiler_fro" };
                int numeroInicial = 32;
                int totales = 11;
                llenaInfo(columnas, numeroInicial, totales, noOrden, idEmpresa, idTaller, checks, textBoxes);
            }
        }
        catch (Exception)
        {
            for (int i = 0; i < 12; i++)
            {
                checks[i].Checked = true;
                textBoxes[i].Text = "";
            }
        }
    }

    private void cargaLadoDer()
    {
        CheckBox[] checks = {chkAletasD,chkAntenaD,chkCostadoD,chkCristales_Puerta_DelD,chkCristales_Puerta_TrasD,chkEspejos_ExterioresD,
                             chkManijas_ExterioresD,chkMoldurasD,chkPuerta_DelD,chkPuerta_TrasD,chkReflejante_Lateral_DelD,chkReflejante_Lateral_TrasD,
                             chkSalpicaderaD,chkTapones_RuedaD};

        TextBox[] textBoxes = {txtAletasD, txtAntenaD, txtCostadoD, txtCristales_Puerta_DelD, txtCristales_Puerta_TrasD, txtEspejos_ExterioresD,
                txtManijas_ExterioresD, txtMoldurasD, txtPuerta_DelD, txtPuerta_TrasD, txtReflejante_Lateral_DelD, txtReflejante_Lateral_TrasD,
                txtSalpicaderaD, txtTapones_RuedaD};

        for (int i = 0; i < 14; i++)
        {
            checks[i].Checked = true;
            textBoxes[i].Text = "";
        }

        try
        {
            int idEmpresa = Convert.ToInt32(Request.QueryString["e"]);
            int idTaller = Convert.ToInt32(Request.QueryString["t"]);
            int noOrden = Convert.ToInt32(Request.QueryString["o"]);
            bool existe = datos.existeInventarioOrden(noOrden, idEmpresa, idTaller);
            if (existe)
            {
                string[] columnas = { "aletas_der", "antena_der", "costado_der", "cristales_puerta_delantera_der", "cristales_puerta_trasera_der", "espejos_exteriores_der", "manijas_exteriores_der", "molduras_der", "puerta_delantera_der", "puerta_trasera_der", "reflejante_lateral_delantero_der", "reflejante_lateral_trasero_der", "salpicadera_der", "tapones_rueda_der" };
                int numeroInicial = 18;
                int totales = 13;
                llenaInfo(columnas, numeroInicial, totales, noOrden, idEmpresa, idTaller, checks, textBoxes);
            }
        }
        catch (Exception)
        {
            for (int i = 0; i < 14; i++)
            {
                checks[i].Checked = true;
                textBoxes[i].Text = "";
            }
        }
    }

    private void cargaLadoIzq()
    {
        CheckBox[] checks = {chkAletas,chkAntena,chkCostado,chkCristales_Puerta_Del,chkCristales_Puerta_Tras,chkEspejos_Exteriores,
                             chkManijas_Exteriores,chkMolduras,chkPuerta_Del,chkPuerta_Tras,chkReflejante_Lateral_Del,chkReflejante_Lateral_Tras,
                             chkSalpicadera,chkTapones_Rueda};

        TextBox[] textBoxes = {txtAletas, txtAntena, txtCostado, txtCristales_Puerta_Del, txtCristales_Puerta_Tras, txtEspejos_Exteriores,
                txtManijas_Exteriores, txtMolduras, txtPuerta_Del, txtPuerta_Tras, txtReflejante_Lateral_Del, txtReflejante_Lateral_Tras,
                txtSalpicadera, txtTapones_Rueda};

        for (int i = 0; i < 14; i++)
        {
            checks[i].Checked = true;
            textBoxes[i].Text = "";
        }

        try
        {
            int idEmpresa = Convert.ToInt32(Request.QueryString["e"]);
            int idTaller = Convert.ToInt32(Request.QueryString["t"]);
            int noOrden = Convert.ToInt32(Request.QueryString["o"]);
            bool existe = datos.existeInventarioOrden(noOrden, idEmpresa, idTaller);
            if (existe)
            {
                string[] columnas = { "aletas_izq", "antena_izq", "costado_izq", "cristales_puerta_delantera_izq", "cristales_puerta_trasera_izq", "espejos_exteriores_izq", "manijas_exteriores_izq", "molduras_izq", "puerta_delantera_izq", "puerta_trasera_izq", "reflejante_lateral_delantero_izq", "reflejante_lateral_trasero_izq", "salpicadera_izq", "tapones_rueda_izq" };
                int numeroInicial = 4;
                int totales = 13;
                llenaInfo(columnas, numeroInicial, totales, noOrden, idEmpresa, idTaller, checks, textBoxes);
            }
        }
        catch (Exception)
        {
            for (int i = 0; i < 14; i++)
            {
                checks[i].Checked = true;
                textBoxes[i].Text = "";
            }
        }
    }

    private void llenaInfo(string[] columnas, int numeroInicial, int totales, int noOrden, int idEmpresa, int idTaller, CheckBox[] checks, TextBox[] textBoxes)
    {
        try
        {
            bool[] checado = datos.obtieneBit(noOrden, idEmpresa, idTaller, columnas);
            for (int i = 0; i < checado.Length; i++)
            {
                checks[i].Checked = checado[i];
            }
            string[] danos = datos.obtieneDanos(noOrden, idEmpresa, idTaller, numeroInicial, totales);
            for (int i = 0; i < danos.Length; i++)
            {
                textBoxes[i].Text = danos[i];
            }
        }
        catch (Exception)
        {
            for (int i = 0; i < checks.Length; i++)
            {
                checks[i].Checked = true;
                textBoxes[i].Text = "";
            }
        }
    }

    private void cargaDatosPie()
    {
        int empresa = Convert.ToInt32(Request.QueryString["e"]);
        int taller = Convert.ToInt32(Request.QueryString["t"]);
        int orden = Convert.ToInt32(Request.QueryString["o"]);
        object[] datosOrden = recepciones.obtieneInfoOrdenPie(orden, empresa, taller);
        if (Convert.ToBoolean(datosOrden[0]))
        {
            DataSet ordenDatos = (DataSet)datosOrden[1];
            foreach (DataRow filaOrd in ordenDatos.Tables[0].Rows)
            {
                ddlToOrden.Text = filaOrd[0].ToString();
                ddlClienteOrden.Text = filaOrd[1].ToString();
                ddlTsOrden.Text = filaOrd[2].ToString();
                ddlValOrden.Text = filaOrd[3].ToString();
                ddlTaOrden.Text = filaOrd[4].ToString();
                ddlLocOrden.Text = filaOrd[5].ToString();
                ddlPerfil.Text = filaOrd[13].ToString();
                lblSiniestro.Text = filaOrd[9].ToString();
                lblDeducible.Text = Convert.ToDecimal(filaOrd[10].ToString()).ToString("C2");
                lblTotOrden.Text = Convert.ToDecimal(filaOrd[11].ToString()).ToString("C2");
                try
                {
                    DateTime fechaEntrega = Convert.ToDateTime(filaOrd[14].ToString());
                    if (fechaEntrega.ToString("yyyy-MM-dd") == "1900-01-01")
                        lblEntregaEstimada.Text = "No establecida";
                    else
                        lblEntregaEstimada.Text = filaOrd[14].ToString();
                }
                catch (Exception)
                {
                    lblEntregaEstimada.Text = "No establecida";
                }
                if (filaOrd[15].ToString() == "N/A")
                {
                    txtCorreo.Text = "";
                }
                else
                {
                    txtCorreo.Text = filaOrd[15].ToString();
                }
                
                lblPorcDedu.Text = filaOrd[16].ToString() + "%";
            }
        }
    }

    private int[] obtieneSesiones()
    {
        int[] sesiones = new int[6] { 0, 0, 0, 0, 0, 0 };
        try
        {
            sesiones[0] = Convert.ToInt32(Request.QueryString["u"]);
            sesiones[1] = Convert.ToInt32(Request.QueryString["p"]);
            sesiones[2] = Convert.ToInt32(Request.QueryString["e"]);
            sesiones[3] = Convert.ToInt32(Request.QueryString["t"]);
            sesiones[4] = Convert.ToInt32(Request.QueryString["o"]);
            sesiones[5] = Convert.ToInt32(Request.QueryString["f"]);
        }
        catch (Exception)
        {
            sesiones = new int[4] { 0, 0, 0, 0 };
            Session["paginaOrigen"] = "Ordenes.aspx";
            Session["errores"] = "Su sesión a expirado vuelva a iniciar Sesión";
            Response.Redirect("AppErrorLog.aspx");
        }
        return sesiones;
    }

    protected void btnGuardarIzq_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        try
        {
            int[] sesiones = obtieneSesiones();
            int idEmpresa = sesiones[2];
            int idTaller = sesiones[3];
            int noOrden = sesiones[4];
            bool aletas = chkAletas.Checked;
            bool antena = chkAntena.Checked;
            bool costado = chkCostado.Checked;
            bool cristalesPuertDel = chkCristales_Puerta_Del.Checked;
            bool cristalesPuertTras = chkCristales_Puerta_Tras.Checked;
            bool espejosExt = chkEspejos_Exteriores.Checked;
            bool manijasExt = chkManijas_Exteriores.Checked;
            bool molduras = chkMolduras.Checked;
            bool puertaDel = chkPuerta_Del.Checked;
            bool puertaTras = chkPuerta_Tras.Checked;
            bool refleLatDel = chkReflejante_Lateral_Del.Checked;
            bool refleLatTras = chkReflejante_Lateral_Tras.Checked;
            bool salpicadera = chkSalpicadera.Checked;
            bool taponesRuedas = chkTapones_Rueda.Checked;
            string aletasD = txtAletas.Text;
            string antenaD = txtAntena.Text;
            string costadoD = txtCostado.Text;
            string cristalesPuertDelD = txtCristales_Puerta_Del.Text;
            string cristalesPuertTrasD = txtCristales_Puerta_Tras.Text;
            string espejosExtD = txtEspejos_Exteriores.Text;
            string manijasExtD = txtManijas_Exteriores.Text;
            string moldurasD = txtMolduras.Text;
            string puertaDelD = txtPuerta_Del.Text;
            string puertaTrasD = txtPuerta_Tras.Text;
            string refleLatDelD = txtReflejante_Lateral_Del.Text;
            string refleLatTrasD = txtReflejante_Lateral_Tras.Text;
            string salpicaderaD = txtSalpicadera.Text;
            string taponesRuedasD = txtTapones_Rueda.Text;
            bool guardardado = datos.guardaInventarioIzq(noOrden, idEmpresa, idTaller, aletas, antena, costado, cristalesPuertDel, cristalesPuertTras, espejosExt, manijasExt, molduras, puertaDel, puertaTras, refleLatDel, refleLatTras, salpicadera, taponesRuedas);
            if (guardardado)
            {
                guardardado = datos.guardaInventarioIzqDaños(noOrden, idEmpresa, idTaller, aletasD, antenaD, costadoD, cristalesPuertDelD, cristalesPuertTrasD, espejosExtD, manijasExtD, moldurasD, puertaDelD, puertaTrasD, refleLatDelD, refleLatTrasD, salpicaderaD, taponesRuedasD);
                if (guardardado)
                {
                    lblError.Text = "Inventario del lado Izquierdo fue guardado Exitosamente";
                    imgIzq.ImageUrl = "~/img/Inventario/izquierdo_verde.png";
                    checaInventario();
                }
            }
            else
                lblError.Text = "No se pudo guardar el inventario, verifique su conexión e intentelo nuevamente";
        }
        catch (Exception x)
        {
            lblError.Text = "Error: " + x.Message;
        }
    }

    protected void btnGuardarDer_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        try
        {
            int[] sesiones = obtieneSesiones();
            int idEmpresa = sesiones[2];
            int idTaller = sesiones[3];
            int noOrden = sesiones[4];
            bool aletas = chkAletasD.Checked;
            bool antena = chkAntenaD.Checked;
            bool costado = chkCostadoD.Checked;
            bool cristalesPuertDel = chkCristales_Puerta_DelD.Checked;
            bool cristalesPuertTras = chkCristales_Puerta_TrasD.Checked;
            bool espejosExt = chkEspejos_ExterioresD.Checked;
            bool manijasExt = chkManijas_ExterioresD.Checked;
            bool molduras = chkMoldurasD.Checked;
            bool puertaDel = chkPuerta_DelD.Checked;
            bool puertaTras = chkPuerta_TrasD.Checked;
            bool refleLatDel = chkReflejante_Lateral_DelD.Checked;
            bool refleLatTras = chkReflejante_Lateral_TrasD.Checked;
            bool salpicadera = chkSalpicaderaD.Checked;
            bool taponesRuedas = chkTapones_RuedaD.Checked;
            string aletasD = txtAletasD.Text;
            string antenaD = txtAntenaD.Text;
            string costadoD = txtCostadoD.Text;
            string cristalesPuertDelD = txtCristales_Puerta_DelD.Text;
            string cristalesPuertTrasD = txtCristales_Puerta_TrasD.Text;
            string espejosExtD = txtEspejos_ExterioresD.Text;
            string manijasExtD = txtManijas_ExterioresD.Text;
            string moldurasD = txtMoldurasD.Text;
            string puertaDelD = txtPuerta_DelD.Text;
            string puertaTrasD = txtPuerta_TrasD.Text;
            string refleLatDelD = txtReflejante_Lateral_DelD.Text;
            string refleLatTrasD = txtReflejante_Lateral_TrasD.Text;
            string salpicaderaD = txtSalpicaderaD.Text;
            string taponesRuedasD = txtTapones_RuedaD.Text;
            bool guardardado = datos.guardaInventarioDer(noOrden, idEmpresa, idTaller, aletas, antena, costado, cristalesPuertDel, cristalesPuertTras, espejosExt, manijasExt, molduras, puertaDel, puertaTras, refleLatDel, refleLatTras, salpicadera, taponesRuedas);
            if (guardardado)
            {
                guardardado = datos.guardaInventarioDerDaños(noOrden, idEmpresa, idTaller, aletasD, antenaD, costadoD, cristalesPuertDelD, cristalesPuertTrasD, espejosExtD, manijasExtD, moldurasD, puertaDelD, puertaTrasD, refleLatDelD, refleLatTrasD, salpicaderaD, taponesRuedasD);
                if (guardardado)
                {
                    lblError.Text = "Inventario del lado Derecho fue guardado Exitosamente";
                    imgDer.ImageUrl = "~/img/Inventario/derecho_verde.png";
                    checaInventario();
                }
            }
            else
                lblError.Text = "No se pudo guardar el inventario, verifique su conexión e intentelo nuevamente";
        }
        catch (Exception x)
        {
            lblError.Text = "Error: " + x.Message;
        }
    }

    protected void btnGuardarFron_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        try
        {
            int[] sesiones = obtieneSesiones();
            int idEmpresa = sesiones[2];
            int idTaller = sesiones[3];
            int noOrden = sesiones[4];
            bool biseles = chkBiseles.Checked;
            bool brazosLim = chkBrazosLimpiadores.Checked;
            bool cofre = chkCofre.Checked;
            bool cuartos = chkCuartosLuz.Checked;
            bool defensa = chkDefensaDelantera.Checked;
            bool halogeno = chkFarosHalogeno.Checked;
            bool niebla = chkFarosNiebla.Checked;
            bool parabrisas = chkParabrisas.Checked;
            bool parrilla = chkParrilla.Checked;
            bool plumas = chkPlumasLimpiadoras.Checked;
            bool portaPlaca = chkPortaPlaca.Checked;
            bool spoiler = chkEspoiler.Checked;
            string biselesD = txtBiseles.Text;
            string brazosLimD = txtBrazosLimpiadores.Text;
            string cofreD = txtCofre.Text;
            string cuartosD = txtCuartosLuz.Text;
            string defensaD = txtDefensaDelantera.Text;
            string halogenoD = txtFarosHalogeno.Text;
            string nieblaD = txtFarosNiebla.Text;
            string parabrisasD = txtParabrisas.Text;
            string parrillaD = txtParrilla.Text;
            string plumasD = txtPlumasLimpiadoras.Text;
            string portaPlacaD = txtPortaPlaca.Text;
            string spoilerD = txtEspoiler.Text;
            bool guardardado = datos.guardaInventarioFront(noOrden, idEmpresa, idTaller, biseles, brazosLim, cofre, cuartos, defensa, halogeno, niebla, parabrisas, parrilla, plumas, portaPlaca, spoiler);
            if (guardardado)
            {
                guardardado = datos.guardaInventarioFrontDaños(noOrden, idEmpresa, idTaller, biselesD, brazosLimD, cofreD, cuartosD, defensaD, halogenoD, nieblaD, parabrisasD, parrillaD, plumasD, portaPlacaD, spoilerD);
                if (guardardado)
                {
                    lblError.Text = "Inventario Frontal fue guardado Exitosamente";
                    imgFro.ImageUrl = "~/img/Inventario/frontal_verde.png";
                    checaInventario();
                }
            }
            else
                lblError.Text = "No se pudo guardar el inventario, verifique su conexión e intentelo nuevamente";
        }
        catch (Exception x)
        {
            lblError.Text = "Error: " + x.Message;
        }
    }

    protected void btnGuardaPos_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        try
        {
            int[] sesiones = obtieneSesiones();
            int idEmpresa = sesiones[2];
            int idTaller = sesiones[3];
            int noOrden = sesiones[4];
            bool calaveras = chkCalaveras.Checked;
            bool cuartos = chkCuartos.Checked;
            bool defensa = chkDefensaTrasera.Checked;
            bool facia = chkFacia.Checked;
            bool portaPlaca = chkPortaPlacaP.Checked;
            bool topes = chkTopes.Checked;
            bool limpiadores = chkLimpiadores.Checked;
            bool medallon = chkMedallon.Checked;
            bool mica = chkMica.Checked;
            bool sistemaEscape = chkSistemaEscape.Checked;
            bool spoiler = chkSpoiles.Checked;
            bool taponGas = chkTaponGasolina.Checked;
            bool luzPlaca = chkLuzPlaca.Checked;
            string calaverasD = txtCalaveras.Text;
            string cuartosD = txtCuartos.Text;
            string defensaD = txtDefensaTrasera.Text;
            string faciaD = txtFacia.Text;
            string portaPlacaD = txtPortaPlacaP.Text;
            string topesD = txtTopes.Text;
            string limpiadoresD = txtLimpiadores.Text;
            string medallonD = txtMedallon.Text;
            string micaD = txtMica.Text;
            string sistemaEscapeD = txtSistemaEscape.Text;
            string spoilerD = txtSpoiles.Text;
            string taponGasD = txtTaponGasolina.Text;
            string luzPlacaD = txtLuzPlaca.Text;
            bool guardardado = datos.guardaInventarioPost(noOrden, idEmpresa, idTaller, calaveras, cuartos, defensa, facia, portaPlaca, topes, limpiadores, medallon, mica, sistemaEscape, spoiler, taponGas, luzPlaca);
            if (guardardado)
            {
                guardardado = datos.guardaInventarioPostDaños(noOrden, idEmpresa, idTaller, calaverasD, cuartosD, defensaD, faciaD, portaPlacaD, topesD, limpiadoresD, medallonD, micaD, sistemaEscapeD, spoilerD, taponGasD, luzPlacaD);
                if (guardardado)
                {
                    lblError.Text = "Inventario General fue guardado Exitosamente";
                    imgPos.ImageUrl = "~/img/Inventario/posterior_verde.png";
                    checaInventario();
                }
            }
            else
                lblError.Text = "No se pudo guardar el inventario, verifique su conexión e intentelo nuevamente";
        }
        catch (Exception x)
        {
            lblError.Text = "Error: " + x.Message;
        }
    }

    protected void btnGuardarInt_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        try
        {
            int[] sesiones = obtieneSesiones();
            int idEmpresa = sesiones[2];
            int idTaller = sesiones[3];
            int noOrden = sesiones[4];
            bool alfombra = chkAlfombra.Checked;
            bool asientoDel = chkAsientosDelanteros.Checked;
            bool asientoTras = chkAsientosTraseros.Checked;
            bool radioAgencia = chkRadioEstereoAgencia.Checked;
            bool bocinas = chkBocinas.Checked;
            bool estereo = chkEsterero.Checked;
            bool btnsPuerts = chkBotonesPuerta.Checked;
            bool btnsRadio = chkBotonesRadioAutoestereo.Checked;
            bool cabeceras = chkCabeceras.Checked;
            bool guantera = chkCajuelaGuantes.Checked;
            bool cenicero = chkCenicero.Checked;
            bool cinturonesSeg = chkCinturonesSeguridad.Checked;
            bool coderas = chkCoderas.Checked;
            bool consola = chkConsola.Checked;
            bool elevacionElect = chkControlElectricoElev.Checked;
            bool encendedor = chkEncendedor.Checked;
            bool espejo = chkEspejoInt.Checked;
            bool luzInt = chkLuzInterioir.Checked;
            bool manijasInt = chkManijasInteriores.Checked;
            bool palanca = chkPalancaVelocidades.Checked;
            bool perillaPalanca = chkPerillaPalanca.Checked;
            bool reloj = chkReloj.Checked;
            bool tablero = chkTablero.Checked;
            bool viceras = chkViceras.Checked;
            bool tapetes = chkTapetesInt.Checked;
            bool cieloToldo = chkCieloToldo.Checked;
            string alfombraD = txtAlfombra.Text;
            string asientoDelD = txtAsientosDelanteros.Text;
            string asientoTrasD = txtAsientosTraseros.Text;
            string radioAgenciaD = txtRadioEstereoAgencia.Text;
            string bocinasD = txtBocinas.Text;
            string estereoD = txtEsterero.Text;
            string btnsPuertsD = txtBotonesPuerta.Text;
            string btnsRadioD = txtBotonesRadioAutoestereo.Text;
            string cabecerasD = txtCabeceras.Text;
            string guanteraD = txtCajuelaGuantes.Text;
            string ceniceroD = txtCenicero.Text;
            string cinturonesSegD = txtCinturonesSeguridad.Text;
            string coderasD = txtCoderas.Text;
            string consolaD = txtConsola.Text;
            string elevacionElectD = txtControlElectricoElev.Text;
            string encendedorD = txtEncendedor.Text;
            string espejoD = txtEspejoInt.Text;
            string luzIntD = txtLuzInterioir.Text;
            string manijasIntD = txtManijasInteriores.Text;
            string palancaD = txtPalancaVelocidades.Text;
            string perillaPalancaD = txtPerillaPalanca.Text;
            string relojD = txtReloj.Text;
            string tableroD = txtTablero.Text;
            string vicerasD = txtViceras.Text;
            string tapetesD = txtTapetesInt.Text;
            string cieloToldoD = txtCieloToldo.Text;
            bool guardardado = datos.guardaInventarioInter(noOrden, idEmpresa, idTaller, alfombra, asientoDel, asientoTras, radioAgencia, bocinas, estereo, btnsPuerts, btnsRadio, cabeceras, guantera, cenicero, cinturonesSeg, coderas, consola, elevacionElect, encendedor, espejo, luzInt, manijasInt, palanca, perillaPalanca, reloj, tablero, viceras, tapetes, cieloToldo);
            if (guardardado)
            {
                guardardado = datos.guardaInventarioInterDaños(noOrden, idEmpresa, idTaller, alfombraD, asientoDelD, asientoTrasD, radioAgenciaD, bocinasD, estereoD, btnsPuertsD, btnsRadioD, cabecerasD, guanteraD, ceniceroD, cinturonesSegD, coderasD, consolaD, elevacionElectD, encendedorD, espejoD, luzIntD, manijasIntD, palancaD, perillaPalancaD, relojD, tableroD, vicerasD, tapetesD, cieloToldoD);
                if (guardardado)
                {
                    lblError.Text = "Inventario General fue guardado Exitosamente";
                    imgInt.ImageUrl = "~/img/Inventario/interior_verde.png";
                    checaInventario();
                }
            }
            else
                lblError.Text = "No se pudo guardar el inventario, verifique su conexión e intentelo nuevamente";
        }
        catch (Exception x)
        {
            lblError.Text = "Error: " + x.Message;
        }
    }

    protected void btnGuardaCajuela_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        try
        {
            int[] sesiones = obtieneSesiones();
            int idEmpresa = sesiones[2];
            int idTaller = sesiones[3];
            int noOrden = sesiones[4];
            bool cables = chkCables_Corriente.Checked;
            bool refaccion = chkLlanta_Refaccion.Checked;
            bool gato = chkGato.Checked;
            bool herramientas = chkHerramientas.Checked;
            bool llaveRueda = chkLave_Rueda.Checked;
            bool señalCarretera = chkSeñales_Carretera.Checked;
            bool tapete = chkTapetes.Checked;
            bool tapaCarton = chkTapa_Carton.Checked;
            bool extintor = chkExtinguidor.Checked;
            string cablesD = txtCables_Corriente.Text;
            string refaccionD = txtLlanta_Refaccion.Text;
            string gatoD = txtGato.Text;
            string herramientasD = txtHerramientas.Text;
            string llaveRuedaD = txtLave_Rueda.Text;
            string señalCarreteraD = txtSeñales_Carretera.Text;
            string tapeteD = txtTapetes.Text;
            string tapaCartonD = txtTapa_Carton.Text;
            string extintorD = txtExtinguidor.Text;
            bool guardardado = datos.guardaInventarioCajuela(idEmpresa, idTaller, noOrden, cables, refaccion, gato, herramientas, llaveRueda, señalCarretera, tapete, tapaCarton, extintor);
            if (guardardado)
            {
                guardardado = datos.guardaInventarioCajuelaDaños(idEmpresa, idTaller, noOrden, cablesD, refaccionD, gatoD, herramientasD, llaveRuedaD, señalCarreteraD, tapeteD, tapaCartonD, extintorD);
                if (guardardado)
                {
                    lblError.Text = "Inventario de Cajuela fue guardado Exitosamente";
                    imgCaj.ImageUrl = "~/img/Inventario/cajuela_verde.png";
                    checaInventario();
                }
            }
            else
                lblError.Text = "No se pudo guardar el inventario, verifique su conexión e intentelo nuevamente";

        }
        catch (Exception x)
        {
            lblError.Text = "Error: " + x.Message;
        }
    }

    protected void btnGuardaGenerales_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        if (txtMarca.Text != "")
            if (ddlGasolina.SelectedValue != "")
                if (txtLlantas.Text != "")//vida
                {
                    try
                    {
                        int[] sesiones = obtieneSesiones();
                        int idEmpresa = sesiones[2];
                        int idTaller = sesiones[3];
                        int noOrden = sesiones[4];
                        bool llaves = chkLlaves.Checked;
                        bool canastilla = chkCanastilla.Checked;
                        bool emblemas = chkEmblema.Checked;
                        bool bateria = chkBateria.Checked;
                        bool compacDisc = chkCompacDisc.Checked;
                        bool ecualizador = chkEmblema.Checked;
                        bool rines = chkRines.Checked;
                        string llavesD = txtLlaves.Text;
                        string canastillaD = txtCanastilla.Text;
                        string emblemasD = txtEmblema.Text;
                        string bateriaD = txtBateria.Text;
                        string compacDiscD = txtCompacDisc.Text;
                        string ecualizadorD = txtEmblema.Text;
                        string rinesD = txtRines.Text;
                        string llantasVida = txtLlantas.Text;
                        string marca = txtMarca.Text;
                        string observaciones = txtObservaciones.Text;
                        int gasolina = Convert.ToInt32(ddlGasolina.SelectedValue);

                        decimal porcVida = 0;
                        try
                        {
                            porcVida = Convert.ToDecimal(llantasVida);
                            bool guardardado = datos.guardaInventarioGen(noOrden, idEmpresa, idTaller, llaves, canastilla, emblemas, bateria, compacDisc, ecualizador, rines, porcVida.ToString(), marca, gasolina, observaciones);
                            if (guardardado)
                            {
                                guardardado = datos.guardaInventarioGenDaños(noOrden, idEmpresa, idTaller, llavesD, canastillaD, emblemasD, bateriaD, compacDiscD, ecualizadorD, rinesD);
                                if (guardardado)
                                {
                                    lblError.Text = "Inventario General fue guardado Exitosamente";
                                    imgGen.ImageUrl = "~/img/Inventario/generales_verde.png";
                                    checaInventario();
                                }
                            }
                            else
                                lblError.Text = "No se pudo guardar el inventario, verifique su conexión e intentelo nuevamente";
                        }
                        catch (Exception ex) { lblError.Text = "Error: " + ex.Message; }
                    }
                    catch (Exception x)
                    {
                        lblError.Text = "Error: " + x.Message;
                    }
                }
                else
                    lblError.Text = "Necesita colocar un porcentaje de vida de las llantas";
            else
                lblError.Text = "Necesita seleccionar una medida de gasolina";
        else
            lblError.Text = "Necesita indicar la marca de las llantas";
    }


    protected void btnAddFoto_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        string[] extensiones = { "jpg", "bmp", "png", "gif", "tiff", "jpeg" };
        bool archivoValido = false;
        byte[] imagen = null;
        string[] archivosAborrar = null;
        //string extension = "";
        try
        {
            string filename = "";
            string extension = "";
            string ruta = Server.MapPath("~/TMP");

            // Si el directorio no existe, crearlo
            if (!Directory.Exists(ruta))
                Directory.CreateDirectory(ruta);


            int documentos = AsyncUpload1.UploadedFiles.Count;
            int guardados = 0;
            archivosAborrar = new string[documentos];

            for (int i = 0; i < documentos; i++)
            {
                filename = AsyncUpload1.UploadedFiles[i].FileName;
                string[] segmenatado = filename.Split(new char[] { '.' });

                archivoValido = validaArchivo(segmenatado[1]);
                extension = segmenatado[1];
                string archivo = String.Format("{0}\\{1}", ruta, filename);

                FileInfo file = new FileInfo(archivo);
                if (archivoValido)
                {

                    // Verificar que el archivo no exista
                    if (File.Exists(archivo))
                        file.Delete();


                    Telerik.Web.UI.UploadedFile up = AsyncUpload1.UploadedFiles[i];
                    up.SaveAs(archivo);
                    archivosAborrar[i] = archivo;
                    imagen = File.ReadAllBytes(archivo);

                    int[] sesiones = obtieneSesiones();
                    int idEmpresa = sesiones[2];
                    int idTaller = sesiones[3];
                    int proceso = 1;
                    int noOrden = sesiones[4];

                    string nombre = filename;
                    bool agregado = datosOrdenes.agregaFotoDanos(idEmpresa, idTaller, noOrden, nombre, imagen, proceso);
                    if (agregado)
                        guardados++;
                }
                else
                    imagen = null;
            }

            if (guardados > 0)
                checaInventario();


            lblError.Text = "Se guardaron " + guardados.ToString() + " imagenes de " + documentos.ToString() + " seleccionadas.";
            for (int j = 0; j < archivosAborrar.Length; j++)
            {
                FileInfo archivoBorrar = new FileInfo(archivosAborrar[j]);
                archivoBorrar.Delete();
            }

            DataListFotosDanos.DataBind();
        }
        catch (Exception ex) { lblError.Text = "Error: " + ex.Message; }
    }

    private bool validaArchivo(string extencion)
    {
        string[] extenciones = { "jpeg", "jpg", "png", "gif", "bmp", "tiff" };
        bool valido = false;
        for (int i = 0; i < extenciones.Length; i++)
        {
            if (extencion.ToUpper() == extenciones[i].ToUpper())
            {
                valido = true;
                break;
            }
        }
        return valido;
    }

    protected void DataListFotosDanos_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName == "zoom")
        {
            PanelMascara.Visible = true;
            PanelImgZoom.Visible = true;
            string[] valores = e.CommandArgument.ToString().Split(';');
            int id_empresa = Convert.ToInt32(valores[0]);
            int id_taller = Convert.ToInt32(valores[1]);
            int no_orden = Convert.ToInt32(valores[2]);
            int consecutivo = Convert.ToInt32(valores[3]);
            int proceso = Convert.ToInt32(valores[4]);
            imgZoom.ImageUrl = "~/ImgEmpresas.ashx?id=" + id_empresa + ";" + id_taller + ";" + no_orden + ";" + consecutivo + ";" + proceso;
        }
        else if (e.CommandName == "elimina")
        {
            lblError.Text = "";
            string estatus = obtieneEstatus();
            if (estatus != "")
            {
                if (estatus == "R" || estatus == "T" || estatus == "F" || estatus == "S" || estatus == "C")
                    lblError.Text = "No es posible eliminar imagenes ya que la orden se encuentra en estatus Remisionada, Cerrada, Facturada o fue una Salida sin Cargo.";
                else
                {
                    try
                    {
                        string[] valores = e.CommandArgument.ToString().Split(';');
                        int id_empresa = Convert.ToInt32(valores[0]);
                        int id_taller = Convert.ToInt32(valores[1]);
                        int no_orden = Convert.ToInt32(valores[2]);
                        int consecutivo = Convert.ToInt32(valores[3]);
                        int proceso = Convert.ToInt32(valores[4]);
                        bool eliminado = datosOrdenes.eliminarFotoDanos(id_empresa, id_taller, no_orden, consecutivo, proceso);
                        if (eliminado)
                        {
                            DataListFotosDanos.DataBind();
                        }
                        else
                            lblError.Text = "La imagen no se logro eliminar, verifique su conexión e intentelo nuevamente mas tarde";
                    }
                    catch (Exception x)
                    {
                        lblError.Text = "La carga de las imagenes no se logró por un error inesperado: " + x.Message;
                    }
                }
            }
        }
    }

    protected void btnCerrarImgZomm_Click(object sender, EventArgs e)
    {
        PanelImgZoom.Visible = false;
        PanelMascara.Visible = false;
    }

    protected void lnkImprimirInv_Click(object sender, EventArgs e)
    {
        int[] sesiones = obtieneSesiones();
        object[] inventario = datosOrdenes.obtieneInfoInventario(sesiones[2], sesiones[3], sesiones[4]);
        if (Convert.ToBoolean(inventario[1]))
        {
            lnkEnviarCorr();
            ImpresionVehiculo imprimeInventario = new ImpresionVehiculo();
            string nomTaller = recepciones.obtieneNombreTaller(sesiones[3].ToString());
            string usuario = recepciones.obtieneNombreUsuario(sesiones[0].ToString());
            string archivo = imprimeInventario.GenRepOrdTrabajo(sesiones[2], sesiones[3], sesiones[4], nomTaller, usuario);
            try
            {
                if (archivo != "")
                {
                    FileInfo filename = new FileInfo(archivo);
                    if (filename.Exists)
                    {
                        string url = "Descargas.aspx?filename=" + filename.Name;
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "pdfs", "window.open('" + url + "', 'nuevo', 'directories=no, location=no, menubar=no, scrollbars=yes, statusbar=no, titlebar=no, fullscreen=yes,resizable=yes');", true);
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Error al descargar el archivo: " + ex.Message;
            }
        }
        else
            lblError.Text = "No es posible imprimir el inventario ya que aun le hacen falta opciones por capturar o imagenes por adjuntar, por favor verifique";
    }
    protected void lnkEnviarCorr()
    {
        int[] sesiones = obtieneSesiones();
        ImpresionVehiculo imprimeInventario = new ImpresionVehiculo();
        string nomTaller = recepciones.obtieneNombreTaller(sesiones[3].ToString());
        string usuario = recepciones.obtieneNombreUsuario(sesiones[0].ToString());
        string archivo = imprimeInventario.GenRepOrdTrabajo(sesiones[2], sesiones[3], sesiones[4], nomTaller, usuario);
        string ruta = HttpContext.Current.Server.MapPath("~/files/TMP/Emp" + sesiones[2].ToString() + "/Taller" + sesiones[3].ToString() + "/" + sesiones[4].ToString() + "/Inventario");

        //si no existe la carpeta temporal la creamos 
        if (!(Directory.Exists(ruta)))
            Directory.CreateDirectory(ruta);
        ListBox archivosEnviar = new ListBox();
        try
        {
            if (archivo != "")
            {
                FileInfo filename = new FileInfo(archivo);
                if (filename.Exists)
                {
                    ListItem adjuntos = new ListItem();
                    adjuntos.Value = adjuntos.Text = ruta + "\\" + filename.Name;
                    archivosEnviar.Items.Add(adjuntos);
                    FileInfo copia = new FileInfo(ruta + "\\" + filename.Name);
                    if (!copia.Exists)
                        filename.CopyTo(ruta + "\\" + filename.Name);
                    //ObtieneImagenes
                    object[] imagenes = datosOrdenes.obtieneFotosBienvenida(sesiones[2], sesiones[3], sesiones[4], 1);
                    if (Convert.ToBoolean(imagenes[0]))
                    {
                        DataSet fotos = (DataSet)imagenes[1];
                        foreach (DataRow fila in fotos.Tables[0].Rows)
                        {
                            string nombreFoto = fila[0].ToString();
                            byte[] imagenBuffer = (byte[])fila[1];
                            System.IO.MemoryStream st = new System.IO.MemoryStream(imagenBuffer);
                            System.Drawing.Image foto = System.Drawing.Image.FromStream(st);
                            ListItem adjuntosf = new ListItem();
                            adjuntosf.Value = adjuntosf.Text = ruta + "\\" + nombreFoto.Trim();
                            string[] infoImagen = nombreFoto.Split(new char[] { '.' });
                            string extencion = infoImagen[1].ToLower();
                            System.Drawing.Imaging.ImageFormat formato;
                            switch (extencion)
                            {
                                case "jpg":
                                    formato = System.Drawing.Imaging.ImageFormat.Jpeg;
                                    break;
                                case "png":
                                    formato = System.Drawing.Imaging.ImageFormat.Png;
                                    break;
                                case "jpeg":
                                    formato = System.Drawing.Imaging.ImageFormat.Jpeg;
                                    break;
                                case "gif":
                                    formato = System.Drawing.Imaging.ImageFormat.Gif;
                                    break;
                                case "bmp":
                                    formato = System.Drawing.Imaging.ImageFormat.Bmp;
                                    break;
                                case "tiff":
                                    formato = System.Drawing.Imaging.ImageFormat.Tiff;
                                    break;
                                default:
                                    formato = System.Drawing.Imaging.ImageFormat.Jpeg;
                                    break;
                            }
                            foto.Save(adjuntosf.Text, formato);
                            archivosEnviar.Items.Add(adjuntosf);
                        }
                    }
                    Envio_Mail enviar = new Envio_Mail();
                    DatosVehiculos vehiculos = new DatosVehiculos();
                    string marca, tipo, modelo, orden, placas, prefijo;
                    marca = tipo = modelo = orden = placas = prefijo = "";
                    object[] vehiculo = vehiculos.obtieneDatosBasicosVehiculoCorreo(sesiones[4], sesiones[2], sesiones[3]);
                    if (Convert.ToBoolean(vehiculo[0]))
                    {
                        DataSet valores = (DataSet)vehiculo[1];
                        foreach (DataRow fila in valores.Tables[0].Rows)
                        {
                            orden = fila[0].ToString();
                            marca = fila[1].ToString();
                            tipo = fila[2].ToString();
                            modelo = fila[3].ToString();
                            placas = fila[4].ToString();
                            prefijo = fila[5].ToString();
                        }
                    }
                    string mensaje = string.Format("<table width='553' border='0' align='center' cellpadding='0' cellspacing='0'>" +
                        "<tr><td><p>&nbsp;</p><p>&nbsp;</p></td></tr>" +
                        "<tr><td><img src='http://www.formulasistemas.com/empresas/logoMoncar.png' widht='200' height='100'></td></tr><tr><td><p>&nbsp;</p></td></tr><tr><td>&nbsp;</td></tr><tr><td>&nbsp;</td></tr>" +
                        "<tr><td><p align='justify'>Moncar Aztahuacan le agradece su preferencia y al mismo tiempo le informamos que su veh&iacute;culo {0} {1}, modelo {2}, placas {5} y OT {3}{4} ha ingresado a nuestras instalaciones.</p></td></tr>" +
                        "<tr><td>&nbsp;</td></tr>" +
                        "<tr><td><p align='justify'>Posteriormente uno de nuestros asesores lo contactar&aacute; para otorgarle informaci&oacute;n precisa del estatus de su veh&iacute;culo, o si lo desea tambi&eacute;n puede comunicarse al siguiente n&uacute;mero telef&oacute;nico:  5693 5996.</p></td></tr>" +
                        "<tr><td>&nbsp;</td></tr><tr><td>ATENTAMENTE</td></tr><tr><td><p>&nbsp;</p></td></tr><tr><td><b>&nbsp;</b></td></tr>" +
                        "<tr><td><p>Moncar Aztahuacan, S.A. de C.V.<br>&nbsp;</p></td></tr><tr><td>Gerencia de Servicio</td></tr><tr><td>&nbsp;<br>&nbsp;</td></tr></table>", marca, tipo, modelo, prefijo, orden, placas);
                    object[] enviado = enviar.obtieneDatosServidor("", txtCorreo.Text.ToLower().Trim(), mensaje, "", "Bienvenida", archivosEnviar, sesiones[2], "", "");
                    if (Convert.ToBoolean(enviado[0]))
                        lblError.Text = "Se ha enviado la información de la orden vía correo electrónico";
                    else
                        lblError.Text = "No pudo enviar el correo electrónico, intente de nuevo. Detalle: " + Convert.ToString(enviado[1]);
                }
                else
                    lblError.Text = "No es posible envia la información por correo electrónico ya que no se encuentra el archivo del inventario";
            }
        }
        catch (Exception ex)
        {
            lblError.Text = "Error al enviar el correo: " + ex.Message;
        }
        finally
        {
            try
            {
                recepciones.actualizaCorreoOrden(sesiones[2], sesiones[3], sesiones[4], txtCorreo.Text);
                Directory.Delete(ruta, true);
            }
            catch (Exception) { }
        }
    }
    protected void lnkEnviarCorreo_Click(object sender, EventArgs e)
    {
        int[] sesiones = obtieneSesiones();
        object[] inventario = datosOrdenes.obtieneInfoInventario(sesiones[2], sesiones[3], sesiones[4]);
        if (Convert.ToBoolean(inventario[0]))
        {
            if (Convert.ToBoolean(inventario[1]))
            {
                ImpresionVehiculo imprimeInventario = new ImpresionVehiculo();
                string nomTaller = recepciones.obtieneNombreTaller(sesiones[3].ToString());
                string usuario = recepciones.obtieneNombreUsuario(sesiones[0].ToString());
                string archivo = imprimeInventario.GenRepOrdTrabajo(sesiones[2], sesiones[3], sesiones[4], nomTaller, usuario);
                string ruta = HttpContext.Current.Server.MapPath("~/files/TMP/Emp" + sesiones[2].ToString() + "/Taller" + sesiones[3].ToString() + "/" + sesiones[4].ToString() + "/Inventario");

                //si no existe la carpeta temporal la creamos 
                if (!(Directory.Exists(ruta)))
                    Directory.CreateDirectory(ruta);
                ListBox archivosEnviar = new ListBox();
                try
                {
                    if (archivo != "")
                    {
                        FileInfo filename = new FileInfo(archivo);
                        if (filename.Exists)
                        {
                            ListItem adjuntos = new ListItem();
                            adjuntos.Value = adjuntos.Text = ruta + "\\" + filename.Name;
                            archivosEnviar.Items.Add(adjuntos);
                            FileInfo copia = new FileInfo(ruta + "\\" + filename.Name);
                            if (!copia.Exists)
                                filename.CopyTo(ruta + "\\" + filename.Name);
                            //ObtieneImagenes
                            object[] imagenes = datosOrdenes.obtieneFotosBienvenida(sesiones[2], sesiones[3], sesiones[4], 1);
                            if (Convert.ToBoolean(imagenes[0]))
                            {
                                DataSet fotos = (DataSet)imagenes[1];
                                foreach (DataRow fila in fotos.Tables[0].Rows)
                                {
                                    string nombreFoto = fila[0].ToString();
                                    byte[] imagenBuffer = (byte[])fila[1];
                                    System.IO.MemoryStream st = new System.IO.MemoryStream(imagenBuffer);
                                    System.Drawing.Image foto = System.Drawing.Image.FromStream(st);
                                    ListItem adjuntosf = new ListItem();
                                    adjuntosf.Value = adjuntosf.Text = ruta + "\\" + nombreFoto.Trim();
                                    string[] infoImagen = nombreFoto.Split(new char[] { '.' });
                                    string extencion = infoImagen[1].ToLower();
                                    System.Drawing.Imaging.ImageFormat formato;
                                    switch (extencion)
                                    {
                                        case "jpg":
                                            formato = System.Drawing.Imaging.ImageFormat.Jpeg;
                                            break;
                                        case "png":
                                            formato = System.Drawing.Imaging.ImageFormat.Png;
                                            break;
                                        case "jpeg":
                                            formato = System.Drawing.Imaging.ImageFormat.Jpeg;
                                            break;
                                        case "gif":
                                            formato = System.Drawing.Imaging.ImageFormat.Gif;
                                            break;
                                        case "bmp":
                                            formato = System.Drawing.Imaging.ImageFormat.Bmp;
                                            break;
                                        case "tiff":
                                            formato = System.Drawing.Imaging.ImageFormat.Tiff;
                                            break;
                                        default:
                                            formato = System.Drawing.Imaging.ImageFormat.Jpeg;
                                            break;
                                    }
                                    foto.Save(adjuntosf.Text, formato);
                                    archivosEnviar.Items.Add(adjuntosf);
                                }
                            }
                            Envio_Mail enviar = new Envio_Mail();
                            DatosVehiculos vehiculos = new DatosVehiculos();
                            string marca, tipo, modelo, orden, placas, prefijo;
                            marca = tipo = modelo = orden = placas = prefijo = "";
                            object[] vehiculo = vehiculos.obtieneDatosBasicosVehiculoCorreo(sesiones[4], sesiones[2], sesiones[3]);
                            if (Convert.ToBoolean(vehiculo[0]))
                            {
                                DataSet valores = (DataSet)vehiculo[1];
                                foreach (DataRow fila in valores.Tables[0].Rows)
                                {
                                    orden = fila[0].ToString();
                                    marca = fila[1].ToString();
                                    tipo = fila[2].ToString();
                                    modelo = fila[3].ToString();
                                    placas = fila[4].ToString();
                                    prefijo = fila[5].ToString();
                                }
                            }
                            string mensaje = string.Format("<table width='553' border='0' align='center' cellpadding='0' cellspacing='0'>" +
                                "<tr><td><p>&nbsp;</p><p>&nbsp;</p></td></tr>" +
                                "<tr><td><img src='http://www.formulasistemas.com/empresas/logoMoncar.png' widht='200' height='100'></td></tr><tr><td><p>&nbsp;</p></td></tr><tr><td>&nbsp;</td></tr><tr><td>&nbsp;</td></tr>" +
                                "<tr><td><p align='justify'>Moncar Aztahuacan le agradece su preferencia y al mismo tiempo le informamos que su veh&iacute;culo {0} {1}, modelo {2}, placas {5} y OT {3}{4} ha ingresado a nuestras instalaciones.</p></td></tr>" +
                                "<tr><td>&nbsp;</td></tr>" +
                                "<tr><td><p align='justify'>Posteriormente uno de nuestros asesores lo contactar&aacute; para otorgarle informaci&oacute;n precisa del estatus de su veh&iacute;culo, o si lo desea tambi&eacute;n puede comunicarse al siguiente n&uacute;mero telef&oacute;nico:  5693 5996.</p></td></tr>" +
                                "<tr><td>&nbsp;</td></tr><tr><td>ATENTAMENTE</td></tr><tr><td><p>&nbsp;</p></td></tr><tr><td><b>&nbsp;</b></td></tr>" +
                                "<tr><td><p>Moncar Aztahuacan, S.A. de C.V.<br>&nbsp;</p></td></tr><tr><td>Gerencia de Servicio</td></tr><tr><td>&nbsp;<br>&nbsp;</td></tr></table>", marca, tipo, modelo, prefijo, orden, placas);
                            object[] enviado = enviar.obtieneDatosServidor("", txtCorreo.Text.ToLower().Trim(), mensaje, "", "Bienvenida", archivosEnviar, sesiones[2], "", "");
                            if (Convert.ToBoolean(enviado[0]))
                                lblError.Text = "Se ha enviado la información de la orden vía correo electrónico";
                            else
                                lblError.Text = "No pudo enviar el correo electrónico, intente de nuevo. Detalle: "+Convert.ToString(enviado[1]);
                        }
                        else
                            lblError.Text = "No es posible envia la información por correo electrónico ya que no se encuentra el archivo del inventario";
                    }
                }
                catch (Exception ex)
                {
                    lblError.Text = "Error al enviar el correo: " + ex.Message;
                }
                finally
                {
                    try
                    {
                        recepciones.actualizaCorreoOrden(sesiones[2], sesiones[3], sesiones[4], txtCorreo.Text);
                        Directory.Delete(ruta, true);
                    }
                    catch (Exception) { }
                }
            }
            else
                lblError.Text = "No es posible enviar el inventario por correo ya que aun le hacen falta opciones por capturar";
        }
        else
            lblError.Text = "No es posible el inventario por correo ya que aun le hacen falta opciones por capturar, por favor verifique";
    }

    private void actualizaLocalizacion()
    {
        int idEmpresa = Convert.ToInt32(Request.QueryString["e"]);
        int idTaller = Convert.ToInt32(Request.QueryString["t"]);
        int noOrden = Convert.ToInt32(Request.QueryString["o"]);
        int idUsuario = Convert.ToInt32(Request.QueryString["u"]);
        object[] controlParametros = recepciones.cuentaRecepcionRealizada(noOrden, idEmpresa, idTaller);
        int recepRealizados = 0;
        int regBitLocInt = 0;
        if (Convert.ToBoolean(controlParametros[0]))
            recepRealizados = Convert.ToInt32(controlParametros[1]);
        if (recepRealizados == 3)
        {
            object[] regBitLoc = recepciones.obtieneRegBitacoraLoc(noOrden, idEmpresa, idTaller);
            if (Convert.ToBoolean(regBitLoc[0]))
            {
                regBitLocInt = Convert.ToInt32(regBitLoc[1]);
                if (regBitLocInt == 1)
                {
                    object[] actualizaLocalizacion = recepciones.actualizaLocRec(noOrden, idEmpresa, idTaller, idUsuario);
                    if (Convert.ToBoolean(actualizaLocalizacion[0]))
                        if (Convert.ToBoolean(actualizaLocalizacion[1])) { }
                        else
                            lblError.Text = "Hubo un error en la conexión, ";
                    else
                        lblError.Text = "Ocurrio un error inesperado: " + actualizaLocalizacion[1].ToString();
                }
            }
            else
                lblError.Text = "Ocurrio un error inesperado: " + regBitLoc[1].ToString();
        }
    }
}