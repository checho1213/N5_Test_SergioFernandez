using MediatR;

namespace N5.WebApi.Application.Commands.Permissions
{
    public class CreatePermissionCommand : IRequest<bool>
    {
        public string EmployeeName { get; set; }
        public string EmployeeSurname { get; set; }
        public DateTime PermissionDate { get; set; }
        public int PermissionType { get; set; }
    }

}
