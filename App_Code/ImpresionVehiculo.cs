using System;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.io;
using System.Diagnostics;
using System.IO;
using E_Utilities;

/// <summary>
/// Descripción breve de ImpresionVehiculo
/// </summary>
public class ImpresionVehiculo
{
    ControlesUsuario datosCU = new ControlesUsuario();
    Ejecuciones ejecuta = new Ejecuciones();
    Fechas fechas = new Fechas();
    string sql;
    object[] retorno = new object[2];
    private Stream os;
    private PdfReader reader;

    public ImpresionVehiculo()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public string GenRepOrdTrabajo(int idEmpresa, int idTaller, int noOrden, string nombre_taller, string usuario)
    {
        // Crear documento
        Document documento = new Document(iTextSharp.text.PageSize.LETTER);
        documento.AddTitle("OrdenTrabajo_E" + idEmpresa.ToString() + "_T" + idTaller.ToString() + "_Orden" + noOrden.ToString());
        documento.AddCreator("MoncarWeb");

        string ruta = HttpContext.Current.Server.MapPath("~/files");
        string archivo = ruta + "\\" + "Inventario_Vehículo_E" + idEmpresa.ToString() + "_T" + idTaller.ToString() + "_Orden" + noOrden.ToString() + ".pdf";

        //si no existe la carpeta temporal la creamos 
        if (!(Directory.Exists(ruta)))
            Directory.CreateDirectory(ruta);
              

        if (archivo.Trim() != "")
        {
            FileInfo arhc = new FileInfo(archivo.Trim());
            if (arhc.Exists)
                arhc.Delete();
            
            FileStream file = new FileStream(archivo,
            FileMode.OpenOrCreate,
            FileAccess.ReadWrite,
            FileShare.ReadWrite);
            PdfWriter.GetInstance(documento, file);
            // Abrir documento.
            documento.Open();
            
            PdfPTable tablaHead = new PdfPTable(2);
            tablaHead.SetWidths(new float[] { 3f, 7f });
            tablaHead.DefaultCell.Border = 0;
            PdfPTable tablaIMG = new PdfPTable(1);
            PdfPTable tablaTitulo = new PdfPTable(1);
            tablaTitulo.DefaultCell.Border = 0;
            tablaTitulo.DefaultCell.Border = 0;
            PdfPTable tablaTitulo2 = new PdfPTable(2);
            tablaTitulo2.DefaultCell.Border = 0;
            tablaTitulo2.SetWidths(new float[] { 7f, 3f });
            PdfPTable tablaNom = new PdfPTable(2);
            tablaNom.DefaultCell.Border = 0;
            tablaNom.SetWidths(new float[] { 8f, 2f });
            PdfPTable tablaNomText = new PdfPTable(1);
            tablaNomText.DefaultCell.Border = 0;
            PdfPTable tablaTel = new PdfPTable(3);
            tablaTel.DefaultCell.Border = 0;
            tablaTel.SetWidths(new float[] { 5f, 2.5f, 2.5f });
            PdfPTable tablaPlacas = new PdfPTable(3);
            tablaPlacas.DefaultCell.Border = 0;
            PdfPTable tablaFecha = new PdfPTable(3);
            tablaPlacas.SetWidths(new float[] { 3f, 3.5f, 3.5f });

            string imagepath = HttpContext.Current.Server.MapPath("img/");
            //string imagepath = Server.MapPath("Images");
            iTextSharp.text.Image gif = iTextSharp.text.Image.GetInstance(imagepath + "logo_direccion(2).jpg");
            tablaHead.AddCell(gif);

            object[] datosEncabezado = llenaEncabezado(noOrden,idEmpresa,idTaller);

            iTextSharp.text.Font _fuenteTitulo = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.NORMAL, BaseColor.WHITE);
            iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 6, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            iTextSharp.text.Font _standardFontN = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 6, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            PdfPCell cellTitulo1 = new PdfPCell(new Paragraph("INVENTARIO DEL VEHICULO", _fuenteTitulo));
            cellTitulo1.BackgroundColor = BaseColor.BLACK;
            tablaTitulo2.AddCell(cellTitulo1);
            PdfPCell cellTitulo2 = new PdfPCell(new Paragraph("OT "+datosEncabezado[0].ToString().Trim()+" "+noOrden));
            tablaTitulo2.AddCell(cellTitulo2);
            tablaTitulo.AddCell(tablaTitulo2);
            PdfPCell cellNom = new PdfPCell(new Paragraph("NOMBRE DEL ASEGURADO", _standardFontN));
            tablaNomText.AddCell(cellNom);
            PdfPCell cellNomAseg = new PdfPCell(new Paragraph(datosEncabezado[1].ToString(), _standardFont));
            tablaNomText.AddCell(cellNomAseg);
            tablaNomText.WidthPercentage = 100f;
            tablaNom.AddCell(tablaNomText);
            PdfPCell cellFecha = new PdfPCell(new Paragraph("FECHA", _standardFontN));
            cellFecha.Colspan = 3;
            cellFecha.HorizontalAlignment = 1;
            tablaFecha.AddCell(cellFecha);
            string[] fechaInv = datosEncabezado[2].ToString().Split('-');
            PdfPCell cellAño = new PdfPCell(new Paragraph(fechaInv[0], _standardFont));
            PdfPCell cellMes = new PdfPCell(new Paragraph(fechaInv[1], _standardFont));
            PdfPCell cellDia = new PdfPCell(new Paragraph(fechaInv[2], _standardFont));
            cellAño.HorizontalAlignment = 1;
            cellMes.HorizontalAlignment = 1;
            cellDia.HorizontalAlignment = 1;
            tablaFecha.AddCell(cellAño);
            tablaFecha.AddCell(cellMes);
            tablaFecha.AddCell(cellDia);
            tablaNom.AddCell(tablaFecha);            
            tablaTitulo.AddCell(tablaNom);
            PdfPCell cellTel = new PdfPCell(new Paragraph("TELEFONO", _standardFontN));
            PdfPCell cellUnida = new PdfPCell(new Paragraph("UNIDAD", _standardFontN));
            PdfPCell cellMod = new PdfPCell(new Paragraph("MODELO", _standardFontN));
            tablaTel.AddCell(cellTel);
            tablaTel.AddCell(cellUnida);
            tablaTel.AddCell(cellMod);

            PdfPCell cellTelDat = new PdfPCell(new Paragraph(datosEncabezado[3].ToString(), _standardFont));
            PdfPCell cellUnidaDat = new PdfPCell(new Paragraph(datosEncabezado[4].ToString(), _standardFont));
            PdfPCell cellModDat = new PdfPCell(new Paragraph(datosEncabezado[5].ToString(), _standardFont));
            tablaTel.AddCell(cellTelDat);
            tablaTel.AddCell(cellUnidaDat);
            tablaTel.AddCell(cellModDat);

            tablaTitulo.AddCell(tablaTel);

            PdfPCell cellPlaca = new PdfPCell(new Paragraph("PLACA", _standardFontN));
            PdfPCell cellCompan = new PdfPCell(new Paragraph("COMPAÑIA", _standardFontN));
            PdfPCell cellSiniestro = new PdfPCell(new Paragraph("SINIESTRO", _standardFontN));
            tablaPlacas.AddCell(cellPlaca);
            tablaPlacas.AddCell(cellCompan);
            tablaPlacas.AddCell(cellSiniestro);

            PdfPCell cellPlacaDat = new PdfPCell(new Paragraph(datosEncabezado[6].ToString(), _standardFont));
            PdfPCell cellCompanDat = new PdfPCell(new Paragraph(datosEncabezado[7].ToString(), _standardFont));
            PdfPCell cellSiniestroDat = new PdfPCell(new Paragraph(datosEncabezado[8].ToString(), _standardFont));
            tablaPlacas.AddCell(cellPlacaDat);
            tablaPlacas.AddCell(cellCompanDat);
            tablaPlacas.AddCell(cellSiniestroDat);

            tablaTitulo.AddCell(tablaPlacas);

            //tablaTitulo.AddCell(tablaFecha);


            tablaTitulo.WidthPercentage = 100f;
            tablaHead.AddCell(tablaTitulo);
            tablaHead.WidthPercentage = 100f;
            tablaHead.DefaultCell.Border = 0;
            documento.Add(tablaHead);
            //Paragraph esp1 = new Paragraph(" ");
            //documento.Add(esp1);
            Paragraph titu = new Paragraph("CARACTERISTICAS DE LA UNIDAD");
            titu.Alignment = Element.ALIGN_CENTER;
            documento.Add(titu);

            fechas.fecha = fechas.obtieneFechaLocal();
            fechas.tipoFormato = 1;
            string fechaRetorno = fechas.obtieneFechaConFormato();

            string fecha = "Fecha: " + fechaRetorno;

//            documento.Add(new Paragraph(" "));
            obtieneInventario(idEmpresa, idTaller, noOrden, documento);
            /*documento.Add(new Paragraph(" "));
            documento.AddCreationDate();
            documento.Add(new Paragraph(""));*/
            documento.Close();

        }
        return archivo;
    }

    private object[] llenaEncabezado(int noOrden, int idEmpresa, int idTaller)
    {
        string datosQ = "";
        object[] datos = new object[9];
        try
        {
            sql = "select cast(ore.id_cliprov as varchar)+';'+tall.identificador+';'+isnull((ore.nombre_propietario+' '+ore.ap_paterno_propietario+' '+(isnull(ore.ap_materno_propietario, ''))),'Sin nombre registrado')+';'+" +
                  " isnull(ore.tel_part_propietario, (isnull(tel_ofi_propietario, (isnull(tel_cel_propietario, 'Sin telefonos registrados')))))" +
                  " + ';' + ltrim(rtrim(ore.placas)) + ';' + isnull(ore.no_siniestro, 'Sin No. de Siniestro')+';'+convert(varchar, sor.f_recepcion, 126)" +
                  " from Ordenes_Reparacion ore" +
                  " inner" +
                  " join talleres tall on tall.id_taller = ore.id_taller" +
                  " inner join seguimiento_orden sor on sor.no_orden=ore.no_orden and sor.id_empresa=ore.id_empresa and sor.id_taller=ore.id_taller" +
                  " where ore.no_orden = " + noOrden.ToString() + " and ore.id_empresa = " + idEmpresa.ToString() + " and ore.id_taller =" + idTaller.ToString();
            datosQ = ejecuta.scalarToStringSimple(sql);
            string[] datosAr = datosQ.Split(';');
            string idCliprov = datosAr[0];
            string identTaller = datosAr[1];
            string nombreAsegurado = datosAr[2];
            string numTelefono = datosAr[3];
            string placa = datosAr[4];
            string noSiniestro = datosAr[5];
            string fIngreso = datosAr[6];
            sql = "select c.razon_social from cliprov c where c.id_cliprov=" + idCliprov;
            string razonCliprov = ejecuta.scalarToStringSimple(sql);
            sql = "select m.descripcion+' '+tu.descripcion+';'+cast(v.modelo as varchar)" +
                  " from vehiculos v" +
                  " inner join marcas m on m.id_marca = v.id_marca" +
                  " inner join tipo_unidad tu on tu.id_tipo_unidad = v.id_tipo_unidad and tu.id_marca = v.id_marca and tu.id_tipo_vehiculo=v.id_tipo_vehiculo " +
                  " where v.placas = '" + placa + "'";
            datosQ = ejecuta.scalarToStringSimple(sql);
            datosAr = datosQ.Split(';');
            string unidad = datosAr[0];
            string modelo = datosAr[1];

            datos[0] = identTaller;
            datos[1] = nombreAsegurado;
            datos[2] = fIngreso;
            datos[3] = numTelefono;
            datos[4] = unidad;
            datos[5] = modelo;
            datos[6] = placa;
            datos[7] = razonCliprov;
            datos[8] = noSiniestro;
        }
        catch (Exception ex)
        {
            datos[0] = " ";
            datos[1] = " ";
            datos[2] = " ";
            datos[3] = " ";
            datos[4] = " ";
            datos[5] = " ";
            datos[6] = " ";
            datos[7] = " ";
            datos[8] = " ";
        }
        return datos;
    }

    private void obtieneInventario(int idEmpresa, int idTaller, int noOrden, Document documento)
    {
        try
        {
            iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 6, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

            string[] izqDerText = { "ALETAS", "ANTENA", "COSTADO", "CRISTALES PUERTA DELANTERA", "CRISTALES PUERTA TRASERA", "ESPEJOS EXTERIORES", "MANIJAS EXTERIORES", "MOLDURAS", "PUERTA DELANTERA", "PUERTA TRASERA", "REFLEJANTE LATERAL DEL.", "REFLEJANTE LATERAL TRAS.", "SALPICADERA", "TAPONES DE RUEDAS" };
            string[] frontText = { "BISELES", "BRAZOS LIMPIADORES", "COFRE", "CUARTOS DE LUZ", "DEFENSA DELANTERA", "FAROS CON HALOGENO", "FAROS DE NIEBLA", "PARABRISAS", "PARRILLA", "PLUMAS LIMPIADORAS", "PORTA PLACA", "SPOILER" };
            string[] postText = { "CALAVERAS", "CUARTOS", "DEFENSA TRASERA", "FACIA", "PORTA PLACA", "TOPES", "LIMPIADORES", "MEDALLON", "MICA", "SISTEMA ESCAPE", "SPOILER", "TAPON GASOLINA", "LUZ PLACA" };
            string[] inteText = { "ALFOMBRA", "ASIENTOS DELANTEROS", "ASIENTOS TRASEROS", "RADIO ESTEREO AGENCIA", "BOCINAS", "ESTEREO", "BOTONES DE PUERTA", "BOTONES RADIO-AUTOEST.", "CABECERAS", "CAJUELA DE GUANTES", "CENICERO", "CINTURONES DE SEGURIDAD", "CODERAS", "CONSOLA", "CONTROL ELECTRICO ELV.", "ENCENDEDOR", "ESPEJO INTERIOR", "LUZ INTERIOR", "MANIJAS INTERIORES", "PALANCA DE VELOCIDADES", "PRILLA PALANCA", "RELOJ", "TABLERO", "VICERAS", "TAPETES", "CIELO TOLDO" };
            string[] cajText = { "CABLES PARA CORRIENTE", "LLANTAS REFACCION", "GATO HIDRAULICO", "HERRAMIENTAS", "LLAVE DE RUEDA", "SEÑALES DE CARRETERA", "TAPETES", "TAPA CARTON", "EXTINGUIDOR" };
            string[] genText = { "LLAVES", "CANASTILLA", "EMBLEMAS", "BATERIA", "COMPAC DISC", "ECUALIZADOR", "RINES", "LLANTAS", "MARCA", "GASOLINA", "OBSEVACIONES" };

            object[] izq = new object[14];
            string[] columnasIzq = { "aletas_izq", "antena_izq", "costado_izq", "cristales_puerta_delantera_izq", "cristales_puerta_trasera_izq", "espejos_exteriores_izq", "manijas_exteriores_izq", "molduras_izq", "puerta_delantera_izq", "puerta_trasera_izq", "reflejante_lateral_delantero_izq", "reflejante_lateral_trasero_izq", "salpicadera_izq", "tapones_rueda_izq" };
            bool[] checadoIzq = datosCU.obtieneBit(noOrden, idEmpresa, idTaller, columnasIzq);
            int numeroInicialIzq = 4;
            int totalesIzq = 13;
            string[] danosIzq = datosCU.obtieneDanos(noOrden, idEmpresa, idTaller, numeroInicialIzq, totalesIzq);

            object[] der = new object[14];
            string[] columnasDer = { "aletas_der", "antena_der", "costado_der", "cristales_puerta_delantera_der", "cristales_puerta_trasera_der", "espejos_exteriores_der", "manijas_exteriores_der", "molduras_der", "puerta_delantera_der", "puerta_trasera_der", "reflejante_lateral_delantero_der", "reflejante_lateral_trasero_der", "salpicadera_der", "tapones_rueda_der" };
            bool[] checadoDer = datosCU.obtieneBit(noOrden, idEmpresa, idTaller, columnasDer);
            int numeroInicialDer = 18;
            int totalesDer = 13;
            string[] danosDer = datosCU.obtieneDanos(noOrden, idEmpresa, idTaller, numeroInicialDer, totalesDer);

            object[] front = new object[12];
            string[] columnasFront = { "biseles_fro", "brazos_limpiadores_fro", "cofre_fro", "cuartos_luz_fro", "defensa_delantera_fro", "faros_con_halogeno_fro", "faros_niebla_fro", "parabrisas_fro", "parrilla_fro", "plumas_limpiadoras_fro", "porta_placa_fro", "spoiler_fro" };
            bool[] checadoFront = datosCU.obtieneBit(noOrden, idEmpresa, idTaller, columnasFront);
            int numeroInicialFront = 32;
            int totalesFront = 11;
            string[] danosFront = datosCU.obtieneDanos(noOrden, idEmpresa, idTaller, numeroInicialFront, totalesFront);

            object[] post = new object[13];
            string[] columnasPost = { "calaveras_pos", "cuartos_pos", "defensa_trasera_pos", "facia_pos", "porta_placa_pos", "topes_pos", "limpiadores_pos", "medallon_pos", "mica_pos", "sistema_escape_pos", "spoiler_pos", "tapon_gasolina_pos", "luz_placa_pos" };
            bool[] checadoPost = datosCU.obtieneBit(noOrden, idEmpresa, idTaller, columnasPost);
            int numeroInicialPost = 44;
            int totalesPost = 12;
            string[] danosPost = datosCU.obtieneDanos(noOrden, idEmpresa, idTaller, numeroInicialPost, totalesPost);

            object[] inte = new object[23];
            string[] columnasInte = { "alfombra_int", "asientos_delanteros_int", "asientos_traseros_int", "radio_estereo_agencia_int", "bocinas_int", "estereo_int", "botones_puerta_int", "botones_radio_autoestero_int", "cabeceras_int", "cajuela_guantes_int", "cenicero_int", "cinturones_seguridad_int", "coderas_int", "consola_int", "control_electrico_elevacion_int", "encendedor_int", "espejo_interior_int", "luz_interior_int", "manijas_interiores_int", "palanca_velocidades_int", "prilla_palanca_int", "reloj_int", "tablero_int", "viceras_int", "tapetes_int", "cielo_toldo_int" };
            bool[] checadoInte = datosCU.obtieneBit(noOrden, idEmpresa, idTaller, columnasInte);
            int numeroInicialInte = 57;
            int totalesInte = 25;
            string[] danosInte = datosCU.obtieneDanos(noOrden, idEmpresa, idTaller, numeroInicialInte, totalesInte);

            object[] caj = new object[9];
            string[] columnasCaj = { "cables_corriente_caj", "llantas_refaccion_caj", "gato_caj", "herramientas_caj", "llave_rueda_caj", "señales_carretera_caj", "tapetes_caj", "tapa_carton_caj", "extinguidor_caj" };
            bool[] checadoCaj = datosCU.obtieneBit(noOrden, idEmpresa, idTaller, columnasCaj);
            int numeroInicialCaj = 83;
            int totalesCaj = 8;
            string[] danosCaj = datosCU.obtieneDanos(noOrden, idEmpresa, idTaller, numeroInicialCaj, totalesCaj);

            object[] gen = new object[11];
            string[] columnasGen = { "llaves_gen", "canastilla_gen", "emblemas_gen", "bateria_gen", "compac_disc_gen", "ecualizador_gen", "rines_gen" };
            bool[] checadoGen = datosCU.obtieneBit(noOrden, idEmpresa, idTaller, columnasGen);
            int numeroInicialGen = 92;
            int totalesGen = 8;
            string[] danosGen = datosCU.obtieneDanos(noOrden, idEmpresa, idTaller, numeroInicialGen, totalesGen);

            PdfPTable tablaPrincipal = new PdfPTable(1);
            PdfPTable tablaPie = new PdfPTable(1);
            PdfPTable tablaPie1 = new PdfPTable(1);
            PdfPTable tablaPie2 = new PdfPTable(1);
            PdfPTable tablaCelda1M = new PdfPTable(3);
            PdfPTable tablaCelda2M = new PdfPTable(3);
            PdfPTable tablaCelda3M = new PdfPTable(3);
            PdfPTable tablaCelda4F = new PdfPTable(3);
            PdfPTable tablaCelda1 = new PdfPTable(3);
            tablaCelda1.SetWidths(new float[] { 6f, 1f, 3f });
            PdfPTable tablaCelda2 = new PdfPTable(3);
            tablaCelda2.SetWidths(new float[] { 6f, 1f, 3f });
            PdfPTable tablaCelda3 = new PdfPTable(3);
            tablaCelda3.SetWidths(new float[] { 6f, 1f, 3f });
            PdfPTable tablaCelda4 = new PdfPTable(3);
            tablaCelda4.SetWidths(new float[] { 6f, 1f, 3f });
            PdfPTable tablaCelda5 = new PdfPTable(3);
            tablaCelda5.SetWidths(new float[] { 6f, 1f, 3f });
            PdfPTable tablaCelda6 = new PdfPTable(3);
            tablaCelda6.SetWidths(new float[] { 6f, 1f, 3f });
            PdfPTable tablaCelda7 = new PdfPTable(3);
            tablaCelda7.SetWidths(new float[] { 6f, 1f, 3f });
            PdfPTable tablaCelda8 = new PdfPTable(3);
            tablaCelda8.SetWidths(new float[] { 6f, 1f, 3f });
            PdfPTable tablaCelda9 = new PdfPTable(3);
            tablaCelda9.SetWidths(new float[] { 6f, 1f, 3f });

            tablaCelda1M.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tablaCelda2M.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tablaCelda3M.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tablaPrincipal.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tablaPie.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tablaPie1.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tablaPie2.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

            /*PdfPTable textoT = new PdfPTable(1);
            PdfPTable checksT = new PdfPTable(1);
            PdfPTable dañosT = new PdfPTable(1);*/

            PdfPCell temp11 = new PdfPCell(new Paragraph("LADO IZQUIERDO"));
            PdfPCell temp12 = new PdfPCell(new Paragraph("LADO DERECHO"));
            PdfPCell temp13 = new PdfPCell(new Paragraph("PARTE FRONTAL"));
            temp11.Border = temp12.Border = temp13.Border = 0;
            temp11.PaddingTop = temp12.PaddingTop = temp13.PaddingTop = 1;
            temp11.Padding = temp12.Padding = temp13.Padding = 5;
            temp11.BackgroundColor = temp12.BackgroundColor = temp13.BackgroundColor = BaseColor.LIGHT_GRAY;
            temp11.HorizontalAlignment = temp12.HorizontalAlignment = temp13.HorizontalAlignment = 1;
            tablaCelda1M.AddCell(temp11);
            tablaCelda1M.AddCell(temp12);
            tablaCelda1M.AddCell(temp13);
            try
            {
                for (int contador = 0; contador < izqDerText.Length; contador++)
                {
                    try
                    {
                        PdfPCell A1 = new PdfPCell(new Paragraph(izqDerText[contador] + " ", _standardFont));
                        PdfPCell A2 = new PdfPCell();
                        if (contador < 2)
                        {
                            A2 = new PdfPCell(new Paragraph(stringDeCheck(Convert.ToBoolean(checadoIzq[contador])) + " ", _standardFont));
                        }
                        else if (contador == 2)
                        {
                            sql = "select isnull(costado_izq,1) from Inventario_Vehiculo " +
                                  " where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString();
                            bool checado = (bool)ejecuta.scalarToBool(sql)[1];
                            A2 = new PdfPCell(new Paragraph(stringDeCheck(checado) + " ", _standardFont));
                        }
                        else if (contador > 2)
                        {
                            A2 = new PdfPCell(new Paragraph(stringDeCheck(Convert.ToBoolean(checadoIzq[contador])) + " ", _standardFont));
                        }
                        PdfPCell A3 = new PdfPCell(new Paragraph(danosIzq[contador], _standardFont));
                        A1.Border = 0; A2.Border = 0; A3.Border = 0;
                        tablaCelda1.AddCell(A1);
                        tablaCelda1.AddCell(A2);
                        tablaCelda1.AddCell(A3);
                    }
                    catch (Exception ex)
                    {
                        string x = "";
                    }
                    try
                    {
                        PdfPCell B1 = new PdfPCell(new Paragraph(izqDerText[contador] + " ", _standardFont));
                        PdfPCell B2 = new PdfPCell();
                        if (contador < 2)
                        {
                            B2 = new PdfPCell(new Paragraph(stringDeCheck(Convert.ToBoolean(checadoDer[contador])) + " ", _standardFont));
                        }
                        else if (contador == 2)
                        {
                            sql = "select isnull(costado_der,1) from Inventario_Vehiculo " +
                                  " where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString();
                            bool checado = (bool)ejecuta.scalarToBool(sql)[1];
                            B2 = new PdfPCell(new Paragraph(stringDeCheck(checado) + " ", _standardFont));
                        }
                        else if (contador > 2)
                        {
                            B2 = new PdfPCell(new Paragraph(stringDeCheck(Convert.ToBoolean(checadoDer[contador])) + " ", _standardFont));
                        }
                        PdfPCell B3 = new PdfPCell(new Paragraph(danosDer[contador], _standardFont));
                        B1.Border = 0; B2.Border = 0; B3.Border = 0;
                        tablaCelda2.AddCell(B1);
                        tablaCelda2.AddCell(B2);
                        tablaCelda2.AddCell(B3);
                    }
                    catch (Exception ex)
                    {
                        string x = "";
                    }
                    try
                    {
                        if (contador < frontText.Length)
                        {
                            PdfPCell C1 = new PdfPCell(new Paragraph(frontText[contador] + " ", _standardFont));
                            PdfPCell C2 = new PdfPCell(new Paragraph(stringDeCheck(Convert.ToBoolean(checadoFront[contador])) + " ", _standardFont));
                            PdfPCell C3 = new PdfPCell(new Paragraph(danosFront[contador], _standardFont));
                            C1.Border = 0; C2.Border = 0; C3.Border = 0;
                            tablaCelda3.AddCell(C1);
                            tablaCelda3.AddCell(C2);
                            tablaCelda3.AddCell(C3);
                        }
                        else
                        {
                            PdfPCell ext1 = new PdfPCell(new Paragraph(""));
                            PdfPCell ext2 = new PdfPCell(new Paragraph(""));
                            PdfPCell ext3 = new PdfPCell(new Paragraph(""));
                            ext1.Border = 0; ext2.Border = 0; ext3.Border = 0;
                            tablaCelda3.AddCell(ext1);
                            tablaCelda3.AddCell(ext2);
                            tablaCelda3.AddCell(ext3);
                        }
                    }
                    catch (Exception ex)
                    {
                        string x = "";
                    }
                }
            }
            catch (Exception ex)
            {
                string x = "";
            }
            tablaCelda1M.AddCell(tablaCelda1);
            tablaCelda1M.AddCell(tablaCelda2);
            tablaCelda1M.AddCell(tablaCelda3);

            PdfPCell temp14 = new PdfPCell(new Paragraph("PARTE POSTERIOR"));
            temp14.BackgroundColor = BaseColor.LIGHT_GRAY;
            temp14.Padding = 5;
            temp14.PaddingTop = 1;
            temp14.Border = 0;
            temp14.HorizontalAlignment = 1;
            tablaCelda2M.AddCell(temp14);

            PdfPCell temp1 = new PdfPCell(new Paragraph("I     N     T     E     R     I     O     R"));
            temp1.Colspan = 2;
            temp1.Padding = 5;
            temp1.PaddingTop = 1;
            temp1.BackgroundColor = BaseColor.LIGHT_GRAY;
            temp1.Border = 0;
            temp1.HorizontalAlignment = 1;
            tablaCelda2M.AddCell(temp1);

            try
            {
                for (int contador = 0; contador < postText.Length; contador++)
                {
                    PdfPCell D1 = new PdfPCell(new Paragraph(postText[contador] + " ", _standardFont));
                    PdfPCell D2 = new PdfPCell(new Paragraph(stringDeCheck(Convert.ToBoolean(checadoPost[contador])) + " ", _standardFont));
                    PdfPCell D3 = new PdfPCell(new Paragraph(danosPost[contador], _standardFont));
                    D1.Border = 0; D2.Border = 0; D3.Border = 0;
                    tablaCelda4.AddCell(D1);
                    tablaCelda4.AddCell(D2);
                    tablaCelda4.AddCell(D3);

                    PdfPCell E1 = new PdfPCell(new Paragraph(inteText[contador] + " ", _standardFont));
                    PdfPCell E2 = new PdfPCell(new Paragraph(stringDeCheck(Convert.ToBoolean(checadoInte[contador])) + " ", _standardFont));
                    PdfPCell E3 = new PdfPCell(new Paragraph(danosInte[contador], _standardFont));
                    E1.Border = 0; E2.Border = 0; E3.Border = 0;
                    tablaCelda5.AddCell(E1);
                    tablaCelda5.AddCell(E2);
                    tablaCelda5.AddCell(E3);

                    PdfPCell F1 = new PdfPCell(new Paragraph(inteText[contador + 13] + " ", _standardFont));
                    PdfPCell F2 = new PdfPCell(new Paragraph(stringDeCheck(Convert.ToBoolean(checadoInte[contador + 13])) + " ", _standardFont));
                    PdfPCell F3 = new PdfPCell(new Paragraph(danosInte[contador + 13], _standardFont));
                    F1.Border = 0; F2.Border = 0; F3.Border = 0;
                    tablaCelda6.AddCell(F1);
                    tablaCelda6.AddCell(F2);
                    tablaCelda6.AddCell(F3);
                }
            }
            catch (Exception ex)
            {
                string x = "";
            }

            tablaCelda2M.AddCell(tablaCelda4);
            tablaCelda2M.AddCell(tablaCelda5);
            tablaCelda2M.AddCell(tablaCelda6);

            PdfPCell temp15 = new PdfPCell(new Paragraph("CAJUELA"));
            temp15.Border = 0;
            temp15.Padding = 5;
            temp15.PaddingTop = 1;
            temp15.BackgroundColor = BaseColor.LIGHT_GRAY;
            temp15.HorizontalAlignment = 1;
            tablaCelda3M.AddCell(temp15);
            PdfPCell temp16 = new PdfPCell(new Paragraph("GENERALES"));
            temp16.Border = 0;
            temp16.Padding = 5;
            temp16.PaddingTop = 1;
            temp16.BackgroundColor = BaseColor.LIGHT_GRAY;
            temp16.HorizontalAlignment = 1;
            tablaCelda3M.AddCell(temp16);

            PdfPCell temp20 = new PdfPCell(new Paragraph("OBSERVACIONES"));
            temp20.Border = 0;
            temp20.Padding = 5;
            temp20.PaddingTop = 1;
            temp20.BackgroundColor = BaseColor.LIGHT_GRAY;
            temp20.HorizontalAlignment = 1;
            tablaCelda3M.AddCell(temp20);

            for (int contador = 0; contador < cajText.Length; contador++)
            {
                if (contador < 9)
                {
                    PdfPCell G1 = new PdfPCell(new Paragraph(cajText[contador] + " ", _standardFont));
                    PdfPCell G2 = new PdfPCell(new Paragraph(stringDeCheck(Convert.ToBoolean(checadoCaj[contador])) + " ", _standardFont));
                    PdfPCell G3 = new PdfPCell(new Paragraph(danosCaj[contador], _standardFont));
                    G1.Border = 0; G2.Border = 0; G3.Border = 0;
                    tablaCelda7.AddCell(G1);
                    tablaCelda7.AddCell(G2);
                    tablaCelda7.AddCell(G3);
                }

                if (contador < 7)
                {
                    PdfPCell H1 = new PdfPCell(new Paragraph(genText[contador] + " ", _standardFont));
                    PdfPCell H2 = new PdfPCell(new Paragraph(stringDeCheck(Convert.ToBoolean(checadoGen[contador])) + " ", _standardFont));
                    PdfPCell H3 = new PdfPCell(new Paragraph(danosGen[contador], _standardFont));
                    H1.Border = 0; H2.Border = 0; H3.Border = 0;
                    tablaCelda8.AddCell(H1);
                    tablaCelda8.AddCell(H2);
                    tablaCelda8.AddCell(H3);
                }
                else if (contador == 7)
                {
                    PdfPCell J1 = new PdfPCell(new Paragraph(genText[contador] + " ", _standardFont));
                    try
                    {
                        sql = "select isnull(llantas,'') from inventario_vehiculo where no_orden=" + noOrden.ToString() + " and id_taller=" + idTaller.ToString() + " and id_empresa=" + idEmpresa.ToString();
                        string llantasVid = ejecuta.scalarToStringSimple(sql);
                        J1.Border = 0;
                        tablaCelda8.AddCell(J1);
                        PdfPCell temp4 = new PdfPCell(new Paragraph(llantasVid + " % VIDA", _standardFont));
                        temp4.Colspan = 2;
                        temp4.Border = 0;
                        tablaCelda8.AddCell(temp4);
                    }
                    catch (Exception ex)
                    {
                        string x = "";
                    }
                }
                else if (contador == 8)
                {
                    PdfPCell J1 = new PdfPCell(new Paragraph(genText[contador] + " ", _standardFont));
                    try
                    {
                        sql = "select isnull(marca,'') from inventario_vehiculo where no_orden=" + noOrden.ToString() + " and id_taller=" + idTaller.ToString() + " and id_empresa=" + idEmpresa.ToString();
                        string marca = ejecuta.scalarToStringSimple(sql);
                        J1.Border = 0;
                        tablaCelda8.AddCell(J1);
                        PdfPCell temp4 = new PdfPCell(new Paragraph(marca, _standardFont));
                        temp4.Colspan = 2;
                        temp4.Border = 0;
                        tablaCelda8.AddCell(temp4);
                    }
                    catch (Exception ex)
                    {
                        string x = "";
                    }
                }
                if (contador == 0)
                {
                    sql = "select isnull(observaciones,'') from inventario_vehiculo where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString();
                    string observs = ejecuta.scalarToStringSimple(sql);
                    PdfPCell temp7 = new PdfPCell(new Paragraph(observs, _standardFont));
                    temp7.Colspan = 3;
                    temp7.Border = 0;
                    tablaCelda9.AddCell(temp7);
                }
                else
                {
                    PdfPCell temp6 = new PdfPCell(new Paragraph(" ", _standardFont));
                    temp6.Colspan = 3;
                    temp6.Border = 0;
                    tablaCelda9.AddCell(temp6);
                }
            }
            sql = "select isnull(gasolina,0) from inventario_vehiculo where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString();
            int idMedGas = Convert.ToInt32(ejecuta.scalarToInt(sql)[1]);// noOrden, idEmpresa, idTaller, 99, 1)[0]);
            sql = "select isnull(descripcion,'') from medidas_gasolina where id_med_gas=" + idMedGas;
            string medidaGas = ejecuta.scalarToStringSimple(sql);

            PdfPCell temp2 = new PdfPCell(new Paragraph("GASOLINA", _standardFont));
            temp2.Border = 0;
            tablaCelda8.AddCell(temp2);
            PdfPCell temp3 = new PdfPCell(new Paragraph(medidaGas, _standardFont));
            temp3.Colspan = 2;
            temp3.Border = 0;
            tablaCelda8.AddCell(temp3);

            sql = "SELECT ISNULL(km_actual,0) FROM Ordenes_Reparacion WHERE no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString();
            string kmtje = ejecuta.scalarToStringSimple(sql);

            PdfPCell celKmtxt = new PdfPCell(new Paragraph("KILOMETRAJE", _standardFont));
            celKmtxt.Border = 0;
            tablaCelda8.AddCell(celKmtxt);
            PdfPCell celKmVal = new PdfPCell(new Paragraph(kmtje, _standardFont));
            celKmVal.Colspan = 2;
            celKmVal.Border = 0;
            tablaCelda8.AddCell(celKmVal);

            tablaCelda3M.AddCell(tablaCelda7);
            tablaCelda3M.AddCell(tablaCelda8);
            tablaCelda3M.AddCell(tablaCelda9);

            PdfPCell esp2 = new PdfPCell(new Paragraph(" "));
            esp2.Colspan = 3;
            esp2.Border = 0;
            tablaCelda4F.AddCell(esp2);
            PdfPCell temp8 = new PdfPCell(new Paragraph("TALLER"));
            temp8.Border = 0;
            temp8.HorizontalAlignment = 1;
            tablaPie.AddCell(temp8);
            PdfPCell K1 = new PdfPCell(new Paragraph("NOMBRE Y FIRMA", _standardFont));
            K1.Border = 0;
            K1.HorizontalAlignment = 1;
            PdfPCell K2 = new PdfPCell(new Paragraph(" "));
            K2.Padding = 3;
            K2.Border = 0;
            tablaPie.AddCell(K2);
            tablaPie.AddCell(K1);
            tablaCelda4F.AddCell(tablaPie);

            PdfPCell temp9 = new PdfPCell(new Paragraph("GRUA"));
            temp9.Border = 0;
            temp9.HorizontalAlignment = 1;
            tablaPie1.AddCell(temp9);
            tablaPie1.AddCell(K2);
            tablaPie1.AddCell(K1);
            tablaCelda4F.AddCell(tablaPie1);

            PdfPCell temp10 = new PdfPCell(new Paragraph("ASEGURADO"));
            temp10.Border = 0;
            temp10.HorizontalAlignment = 1;
            tablaPie2.AddCell(temp10);
            tablaPie2.AddCell(K2);
            tablaPie2.AddCell(K1);
            tablaCelda4F.AddCell(tablaPie2);


            PdfPCell temp5 = new PdfPCell(new Paragraph(""));
            temp5.Border = 0;
            temp5.Border = 0;
            temp5.Colspan = 3;
            temp5.Padding = 15;
            tablaCelda4F.AddCell(temp5);


            tablaPrincipal.AddCell(tablaCelda1M);
            tablaPrincipal.AddCell(tablaCelda2M);
            tablaPrincipal.AddCell(tablaCelda3M);
            tablaPrincipal.AddCell(tablaCelda4F);
            tablaPrincipal.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tablaPrincipal.WidthPercentage = 100f;
            documento.Add(tablaPrincipal);
        }
        catch (Exception ex)
        {
            string hola = "";
        }
    }

    private string stringDeCheck(bool checado)
    {
        if (checado)
            return "Si";
        else
            return "No";
    }
}