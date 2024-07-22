using System.ComponentModel.DataAnnotations;

namespace N5.Domain.Entities
{
    public class Permisos
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(300)]
        public string NombreEmpleado { get; set; }

        [Required]
        [StringLength(300)]
        public string ApellidoEmpleado { get; set; }

        [Required]
        public DateOnly FechaPermiso { get; set; }

        public TiposPermisos TiposPermiso { get; set; }
    }
}
