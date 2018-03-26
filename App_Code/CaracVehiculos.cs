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

public class CaracVehiculos
{
    Ejecuciones ejecuta = new Ejecuciones();
    Fechas fechas = new Fechas();
    public CaracVehiculos()
    { 
        
    }

    public string GenerarRep(int empresa, int taller, int orden, string nombre_taller, string usuario, string contacto, string correo, string telefonos)
    {

        // Crear documento
        Document documento = new Document(iTextSharp.text.PageSize.LETTER);
        documento.AddTitle("CV_E" + empresa.ToString() + "T" + taller.ToString() + "_Orden" + orden.ToString());
        documento.AddCreator("MoncarWeb");

        string ruta = HttpContext.Current.Server.MapPath("~/files");
        string archivo = ruta + "\\" + "CV_E" + empresa.ToString() + "T" + taller.ToString() + "_Orden" + orden.ToString() + ".pdf";

        //si no existe la carpeta temporal la creamos 
        if (!(Directory.Exists(ruta)))
        {
            Directory.CreateDirectory(ruta);
        }

        if (archivo.Trim() != "")
        {
            FileStream file = new FileStream(archivo,
            FileMode.OpenOrCreate,
            FileAccess.ReadWrite,
            FileShare.ReadWrite);
            PdfWriter.GetInstance(documento, file);
            // Abrir documento.
            documento.Open();
            string personal = "Usuario: " + usuario;
            string envio = "Fecha: " + fechas.obtieneFechaLocal().ToString();

            Chunk chunk = new Chunk("Taller: " + nombre_taller, FontFactory.GetFont("ARIAL", 13, iTextSharp.text.Font.ITALIC));
            //Insertar logo o imagen            
            string rutaLogo = HttpContext.Current.Server.MapPath("~/img/logo.png");
            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(rutaLogo);
            logo.ScaleToFit(100, 100);
            logo.Alignment = iTextSharp.text.Image.ALIGN_LEFT;
            documento.Add(logo);
            Paragraph num_orden = new Paragraph("N° Orden: " + orden, FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLDITALIC, BaseColor.BLACK));
            num_orden.Alignment = Element.ALIGN_RIGHT;
            documento.Add(num_orden);
            documento.Add(new Paragraph(chunk));
            documento.Add(new Paragraph(personal, FontFactory.GetFont("ARIAL", 11, iTextSharp.text.Font.ITALIC)));
            documento.Add(new Paragraph(envio, FontFactory.GetFont("ARIAL", 11, iTextSharp.text.Font.ITALIC)));
            documento.Add(new Paragraph(" "));
            GenerarTablas(documento, empresa, taller, orden, nombre_taller, usuario);
            documento.AddCreationDate();
            documento.Add(new Paragraph(" "));
            Paragraph contac = new Paragraph("Contacto: " + contacto, FontFactory.GetFont("ARIAL", 11, iTextSharp.text.Font.NORMAL, BaseColor.BLACK));
            contac.Alignment = Element.ALIGN_LEFT;
            documento.Add(contac);
            Paragraph tel = new Paragraph("Teléfono(s): " + telefonos, FontFactory.GetFont("ARIAL", 11, iTextSharp.text.Font.NORMAL, BaseColor.BLACK));
            tel.Alignment = Element.ALIGN_LEFT;
            documento.Add(tel);
            Paragraph corr = new Paragraph("Correo: " + correo, FontFactory.GetFont("ARIAL", 11, iTextSharp.text.Font.NORMAL, BaseColor.BLACK));
            corr.Alignment = Element.ALIGN_LEFT;
            documento.Add(corr);
            documento.Close();

        }
        return archivo;
    }


    private void GenerarTablas(Document document, int empresa, int taller, int orden, string nombre_taller, string usuario)
    {

        // Tipo de Font que vamos utilizar
        iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLDITALIC, BaseColor.BLACK);
        iTextSharp.text.Font fuente2 = new iTextSharp.text.Font(FontFactory.GetFont(FontFactory.COURIER, 12, iTextSharp.text.Font.ITALIC));

        PdfPTable tblCaracteristicas = new PdfPTable(2);
        // Columna Datos      
        PdfPCell clDatos = new PdfPCell(new Phrase("CARACTERÍSTICAS", _standardFont));
        clDatos.BorderWidth = 1;        
        clDatos.BackgroundColor = BaseColor.LIGHT_GRAY;
        clDatos.HorizontalAlignment = 1;
        clDatos.VerticalAlignment = 2;
        clDatos.Padding = 3;
        // Columna Descripcion  
        PdfPCell clDescripcion = new PdfPCell(new Phrase("DESCRIPCIÓN", _standardFont));
        clDescripcion.BorderWidth = 1;
        clDescripcion.BackgroundColor = BaseColor.LIGHT_GRAY;
        clDescripcion.HorizontalAlignment = 1;
        clDescripcion.Padding = 3;
        // Añadimos las celdas a la tabla
        tblCaracteristicas.AddCell(clDatos);
        tblCaracteristicas.AddCell(clDescripcion);

        string[] valores = new string[26];
        Vehiculos datosVehi = new Vehiculos();
        object[] campusVehiculo = datosVehi.obtieneInfoVehiculoImpr(orden, empresa, taller);
        if (Convert.ToBoolean(campusVehiculo[0]))
        {
            DataSet datos = (DataSet)campusVehiculo[1];            
            foreach (DataRow fila in datos.Tables[0].Rows)
            {
                for (int i = 0; i < fila.ItemArray.Length; i++)
                {
                    valores[i] = fila[i].ToString();                    
                }
            }
        }

        string[] etiquetas = { "Marca", "Modelo", "Serie Vin", "Placas", "Versión","Motor",
        "Puertas", "Medidas Llantas", "Color Exterior", "Color Interior", "Rin", "Quemacocos",
            "Bolsas de Aire","Transmisión","Aire Acondicionado","Dirección Hidráulica",
            "Elevadores Puertas","Espejos Laterales","Color de Espejos","Molduras",
             "Cantoneras","Vestiduras","Faros de Niebla","Facia o Defensa","Cabina","Defensa Cromada"};

        for (int i = 0; i < etiquetas.Length; i++) {
            clDatos = new PdfPCell(new Phrase(etiquetas[i], _standardFont));
            clDatos.BorderWidth = 1;
            clDatos.HorizontalAlignment = 1;
            clDatos.VerticalAlignment = 1;
            clDatos.Padding = 3;
            tblCaracteristicas.AddCell(clDatos);

            clDescripcion = new PdfPCell(new Phrase(valores[i].ToString(), fuente2));
            clDescripcion.BorderWidth = 1;
            clDescripcion.HorizontalAlignment = 1;
            clDescripcion.VerticalAlignment = 1;
            clDescripcion.Padding = 3;
            tblCaracteristicas.AddCell(clDescripcion);
        }

        document.Add(tblCaracteristicas);       

    }

}