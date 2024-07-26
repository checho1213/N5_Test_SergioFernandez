using System.ComponentModel.DataAnnotations;

namespace N5.Domain.Entities
{
    public class Permission
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(300)]
        public string EmployeeName { get; set; }

        [Required]
        [StringLength(300)]
        public string LastName { get; set; }

        [Required]
        public DateOnly Date { get; set; }

        public PermissionType PemissionType { get; set; }

        public int PemissionTypeId { get; set; }
    }
}
