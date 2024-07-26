namespace N5.WebApi.dto
{
    public class CreateRequestPermissionDto
    {        
        public string EmployeeName { get; set; }
        public string LastName { get; set; }
        public DateTime Date { get; set; }
        public int TypePermissionId { get; set; }
    }
}
