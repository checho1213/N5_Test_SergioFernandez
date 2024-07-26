using MediatR;
using Microsoft.Extensions.Options;
using N5.Domain.Entities;
using N5.Domain.Interfaces;
using N5.Infraestructure.Interfaces;
using N5.Infraestructure.Settings;
using N5.WebApi.Application.Queries;
using N5.WebApi.dto;
using Newtonsoft.Json;

namespace N5.WebApi.Application.Handlers;

public class GetPermissionsByEmployeeQueryHandler : IRequestHandler<GetPermissionsQuery, ResponseMessageDto<List<Permission>>>
{
    #region privateMembers 
    private readonly IUnitofWork _unitofWork;
    private readonly IKafkaRepository _kafkaRepository;
    private readonly string permissionEventTopic;
    private readonly IPermissionDomainServices _permissionDomainServices;
    #endregion

    public GetPermissionsByEmployeeQueryHandler(IUnitofWork unitOfWork, IKafkaRepository kafkaRepository, IOptions<InfraestructureSettings> infraSettings
        , IPermissionDomainServices permissionDomainServices)
    {
        _unitofWork = unitOfWork;
        _kafkaRepository = kafkaRepository;
        permissionEventTopic = infraSettings.Value.KafkaSettings.Topics.PermissionEvent;
        _permissionDomainServices = permissionDomainServices;
    }

    public async Task<ResponseMessageDto<List<Permission>>> Handle(GetPermissionsQuery request, CancellationToken cancellationToken)
    {
        ResponseMessageDto<Permission> response = new ResponseMessageDto<Permission>();
        int statusCode = StatusCodes.Status500InternalServerError;
        try
        {
            var data = _unitofWork.PermisosRepository.GetAll();
            await SendMessageToPermissionEventsTopic(data.ToList());
            return BuildMessage(StatusCodes.Status200OK, data.ToList(), "");
        }
        catch (Exception ex)
        {
            return BuildMessage(statusCode, null, ex.InnerException.Message);
        }
    }

    private async Task SendMessageToPermissionEventsTopic(List<Permission> permission)
    {
        PermissionEnventDto message = new PermissionEnventDto();
        message.Operation = "Get";
        message.Message = JsonConvert.SerializeObject(permission);
        message.Id = Guid.NewGuid();
        await _kafkaRepository.SendMessageToTopic(permissionEventTopic, JsonConvert.SerializeObject(message));
    }

    private ResponseMessageDto<List<Permission>> BuildMessage(int statusCode, List<Permission> permission, string message)
    {
        ResponseMessageDto<List<Permission>> response = new ResponseMessageDto<List<Permission>>();
        response.IsSuccess = statusCode != StatusCodes.Status200OK ? false : true;
        response.Data = permission;
        response.StatusCode = statusCode;
        response.ErrorMessage = message;
        return response;
    }
}
