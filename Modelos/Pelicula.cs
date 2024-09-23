using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPeliculas.Modelos
{
    public class Pelicula
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Duracion { get; set; }
        public string RutaImagen { get; set; }

        // Enum declarado dentro de la clase Pelicula (sin duplicaciones)
        public enum TipoClasificaticion { Siete, Trece, Dieciseis, Dieciocho }
        public TipoClasificaticion Clasificacion { get; set; }

        public DateTime FechaCreacion { get; set; }

        // Relación por categoría
        public int categoriaId { get; set; }

        [ForeignKey("categoriaId")]
        public Categoria Categoria { get; set; }
    }
}
