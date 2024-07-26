using MediatR;
using N5.Domain.Entities;
using N5.WebApi.dto;

namespace N5.WebApi.Application.Queries;
public class GetPermissionsQuery : IRequest<ResponseMessageDto<List<Permission>>>
{   
}
