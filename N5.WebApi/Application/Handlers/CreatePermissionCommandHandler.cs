using MediatR;
using Microsoft.Extensions.Options;
using N5.Domain.Entities;
using N5.Infraestructure.Interfaces;
using N5.Infraestructure.Settings;
using N5.WebApi.Application.Commands.Permissions;
using N5.WebApi.dto;
using Newtonsoft.Json;

namespace N5.WebApi.Application.Handlers;

public class CreatePermissionCommandHandler : IRequestHandler<CreatePermissionCommand, ResponseMessageDto<Permisos>>
{
    #region privateMembers 
    private readonly IUnitofWork _unitofWork;
    private readonly IKafkaRepository _kafkaRepository;
    private readonly string permissionEventTopic;
    #endregion

    public CreatePermissionCommandHandler(IUnitofWork unitOfWork, IKafkaRepository kafkaRepository, IOptions<InfraestructureSettings> infraSettings)
    {
        _unitofWork = unitOfWork;
        _kafkaRepository = kafkaRepository;
        permissionEventTopic = infraSettings.Value.KafkaSettings.Topics.PermissionEvent;
    }

    public async Task<ResponseMessageDto<Permisos>> Handle(CreatePermissionCommand request, CancellationToken cancellationToken)
    {
        ResponseMessageDto<Permisos> response = new ResponseMessageDto<Permisos>();
        try
        {
            Domain.Entities.Permisos permiso = new Domain.Entities.Permisos();
            permiso.NombreEmpleado = request.EmployeeName;
            permiso.ApellidoEmpleado = request.EmployeeSurname;
            permiso.FechaPermiso = DateOnly.FromDateTime(request.PermissionDate);
            permiso.TiposPermisoId = request.PermissionType;
            _unitofWork.PermisosRepository.Add(permiso);

            response.IsSuccess = true;
            response.Data = permiso;
            response.StatusCode = StatusCodes.Status201Created;
            response.ErrorMessage = string.Empty;

            await SendMessageToPermissionEventsTopic(permiso);
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Data = null;
            response.StatusCode = StatusCodes.Status500InternalServerError;
            response.ErrorMessage = ex.Message;
        }
        return response;
    }

    private async Task SendMessageToPermissionEventsTopic(Permisos permission)
    {
        PermissionEnventDto message = new PermissionEnventDto();
        message.Operation = "Create";
        message.Message = JsonConvert.SerializeObject(permission);
        message.Id = Guid.NewGuid();
        await _kafkaRepository.SendMessageToTopic(permissionEventTopic, JsonConvert.SerializeObject(message));
    }
}
