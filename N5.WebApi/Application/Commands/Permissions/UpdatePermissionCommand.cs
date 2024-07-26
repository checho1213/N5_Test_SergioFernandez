using MediatR;
using N5.Domain.Entities;
using N5.WebApi.dto;

namespace N5.WebApi.Application.Commands.Permissions
{
    public class UpdatePermissionCommand : IRequest<ResponseMessageDto<Permission>>
    {
        public int IdPermission { get; set; }
        public DateTime PermissionDate { get; set; }
        public int PermissionType { get; set; }
    }
}
