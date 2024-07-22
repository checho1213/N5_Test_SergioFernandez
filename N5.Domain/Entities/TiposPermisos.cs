using System.ComponentModel.DataAnnotations;

namespace N5.Domain.Entities
{
    public class TiposPermisos
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(300)]
        public string Descripcion { get; set; }
    }
}
