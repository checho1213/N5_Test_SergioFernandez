using MediatR;
using N5.Domain.Entities;
using N5.WebApi.dto;

namespace N5.WebApi.Application.Commands.Permissions
{
    public class CreatePermissionCommand : IRequest<ResponseMessageDto<Permisos>>
    {
        public string EmployeeName { get; set; }
        public string EmployeeSurname { get; set; }
        public DateTime PermissionDate { get; set; }
        public int PermissionType { get; set; }
    }

}
