 using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPeliculas.Modelos.Dtos
{
    public class CrearPeliculaDto
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Duracion { get; set; }
        public string RutaImagen { get; set; }
        public enum CrearTipoClasificaticion { Siete, Trece, Dieciseis, Dieciocho }
        public CrearTipoClasificaticion Clasificacion { get; set; }

        //Relación por categoria
        public int categoriaId { get; set; }
    }
}
