using System.ComponentModel.DataAnnotations;


namespace PARCIAL1B.Models
{
    public class Platos
    {
        [Key]
        public int PlatosID { get; set; }
        public int? EmpresaID { get; set; }
        public int? GrupoID { get; set; }
        public string NombrePlato { get; set; }
        public string DescripcionPlato { get; set; }
        public decimal Precio { get; set; }
    }
}
