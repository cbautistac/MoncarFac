using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

/// <summary>
/// Descripción breve de ConvertirImagenBytes
/// </summary>
public class ConvertirImagenBytes
{
	public ConvertirImagenBytes()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}

    //FUNCION PARA CONVERTIR LA IMAGEN A BYTES

    public Byte[] Imagen_A_Bytes(String ruta)
    {

        FileStream foto = new FileStream(ruta, FileMode.OpenOrCreate, FileAccess.ReadWrite);

        Byte[] arreglo = new Byte[foto.Length];

        BinaryReader reader = new BinaryReader(foto);

        arreglo = reader.ReadBytes(Convert.ToInt32(foto.Length));

        return arreglo;

    }
}